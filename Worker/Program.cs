using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.Database;
using Worker.Consumers;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDbContext<CineQueueDbContext>(options =>
{
    var connectionString = builder.Configuration["ConnectionStrings:DefaultConnection"] 
        ?? "Host=postgres;Port=5432;Database=CineQueueDb;Username=admin;Password=admin";
        
    options.UseNpgsql(connectionString);
});

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<ReserveSeatCommandConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        var host = builder.Configuration["RabbitMQ:HostName"] ?? "rabbitmq";

        cfg.Host(host, "/", h =>
        {   
            h.Username(builder.Configuration["RabbitMQ:UserName"] ?? "admin");
            h.Password(builder.Configuration["RabbitMQ:Password"] ?? "admin");
        });

        cfg.ConfigureEndpoints(context);
    });
});

var host = builder.Build();
host.Run();
