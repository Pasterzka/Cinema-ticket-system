using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.Messages
{
    public record ReserveSeatCommand(
    Guid ReservationId,
    Guid ShowtimeId,
    Guid SeatId,
    string CustomerEmail
);
}