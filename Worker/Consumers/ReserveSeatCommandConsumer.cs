using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Shared.Database;
using Shared.Messages;
using Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace Worker.Consumers
{
    public class ReserveSeatCommandConsumer : IConsumer<ReserveSeatCommand>
    {
        private readonly ILogger<ReserveSeatCommandConsumer> _logger;
        private readonly CineQueueDbContext _dbContext;
        public ReserveSeatCommandConsumer(ILogger<ReserveSeatCommandConsumer> logger, CineQueueDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }
        public async Task Consume(ConsumeContext<ReserveSeatCommand> context)
        {
            var command = context.Message;
            _logger.LogInformation("[INFO][{timeStamp}] Started ReserveSeatCommand: ReservationId={ReservationId}.",
                DateTime.UtcNow, command.ReservationId);

            var seat = await _dbContext.Seats.FirstOrDefaultAsync(s => s.Id == command.SeatId);

            if (seat == null)
            {
                _logger.LogWarning("[WARN][{timeStamp}] Seat not found: SeatId={SeatId}.",
                    DateTime.UtcNow, command.SeatId);
                return;
            }

            if (seat.Status != SeatStatus.Free)
            {
                _logger.LogWarning("[WARN][{timeStamp}] Seat not available: SeatId={SeatId}.",
                    DateTime.UtcNow, command.SeatId);
                return;
            }

            seat.Status = SeatStatus.Reserved;

            var reservation = new Reservation
            {
                Id = command.ReservationId,
                SeatId = command.SeatId,
                CustomerEmail = command.CustomerEmail,
                ReservationTime = DateTime.UtcNow
            };

            _dbContext.Reservations.Add(reservation);

            await _dbContext.SaveChangesAsync();
            
            _logger.LogInformation("[INFO][{timeStamp}] Reservation created: ReservationId={ReservationId}.",
                DateTime.UtcNow, command.ReservationId);

        }
    }
}