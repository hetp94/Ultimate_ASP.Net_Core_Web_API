using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace CompanyEmployees.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly EmailService _emailService;
        private readonly ILogger<EmailController> _logger;
        private readonly HttpClient _httpClient;

        public EmailController(EmailService emailService, ILogger<EmailController> logger, HttpClient httpClient)
        {
            _emailService = emailService;
            _logger = logger;
            _httpClient = httpClient;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendEmailAsync([FromBody] EmailRequest emailRequest)
        {
            try
            {
                await _emailService.SendEmailAsync("ClientApp1", "your-api-key", emailRequest);
                return Ok("Email sent successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError("Error while sending email: {Error}", ex.Message);
                return StatusCode(500, "Failed to send email.");
            }
        }
    }

}
