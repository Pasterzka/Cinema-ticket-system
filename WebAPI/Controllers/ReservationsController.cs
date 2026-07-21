using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Shared.Messages;

namespace WebAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ReservationsController : ControllerBase
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public ReservationsController(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        [HttpPost]
        public async Task<IActionResult> CreateReservation([FromBody] ReserveSeatCommand reserveSeatCommand)
        {
            await _publishEndpoint.Publish(reserveSeatCommand);

            return Accepted(new
            {
                message = "Reservation request received.",
                command = reserveSeatCommand
            });
            
        }
    }
}