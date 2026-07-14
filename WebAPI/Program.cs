using MassTransit;
using Shared.Messages;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        var host = builder.Configuration["RabbitMQ:HostName"] ?? "rabbitmq";

        cfg.Host(host, "/", h =>
        {   
            // Set the username and password for RabbitMQ
            h.Username(builder.Configuration["RabbitMQ:Username"] ?? "admin");
            h.Password(builder.Configuration["RabbitMQ:Password"] ?? "admin");
        });

        cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.MapControllers();


app.MapPost("/api/reservations", async (ReserveSeatCommand command, IPublishEndpoint publishEndpoint) =>
{
    await publishEndpoint.Publish(command);

    return Results.Accepted(value: new
    {
        message = "Reservation request received.",
        command = command
    });
})
.WithName("CreateReservation");

app.Run();