using GetTicket.Models;
using Microsoft.AspNetCore.Mvc;

namespace GetTicket.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketController(GetTicket.Services.TicketService ticketService) : ControllerBase
    {
        private readonly GetTicket.Services.TicketService _ticketService = ticketService;

        [HttpPost("GenerateTicket")]
        public async Task<IActionResult> GenerateTicket([FromBody] TicketRequest request)
        {
            try
            {
                var ticket = await _ticketService.GenerateTicketAsync(request);
                return Ok(ticket);
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Сервис временно недоступен. Попробуйте позже." });
            }
        }
    }
}
