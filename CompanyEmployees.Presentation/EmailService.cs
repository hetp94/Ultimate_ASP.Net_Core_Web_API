using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CompanyEmployees.Presentation
{
    public class EmailService
    {
        private readonly HttpClient _httpClient;
        private readonly string _functionUrl = "http://localhost:7225/api/SendEmail";  // Function URL
        private readonly string _authUrl = "http://localhost:7225/api/gettoken"; // Token URL
        private string _jwtToken;
        private ILogger<EmailService> _logger;
        private DateTime _tokenExpiryTime;
        private string _appName;
        private string _apiKey;

        public EmailService(HttpClient httpClient, ILogger<EmailService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        private async Task EnsureTokenAsync()
        {
            if (string.IsNullOrEmpty(_jwtToken) || DateTime.UtcNow >= _tokenExpiryTime)
            {
                _logger.LogInformation("Token expired or missing. Re-authenticating...");
                await InitializeAsync(_appName, _apiKey);
            }
        }

        private async Task InitializeAsync(string appName, string apiKey)
        {
            _appName = appName;
            _apiKey = apiKey;

            var authRequest = new { AppName = appName, ApiKey = apiKey };
            var response = await _httpClient.PostAsJsonAsync(_authUrl, authRequest);
            response.EnsureSuccessStatusCode();

            var authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();
            _jwtToken = authResponse.Token;

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(_jwtToken);

            var expClaim = jwt.Claims.FirstOrDefault(c => c.Type == "exp")?.Value;
            if (expClaim != null && long.TryParse(expClaim, out var expUnix))
            {
                _tokenExpiryTime = DateTimeOffset.FromUnixTimeSeconds(expUnix).UtcDateTime;
                _logger.LogInformation("Token parsed with expiry: {ExpiryTime}", _tokenExpiryTime);
            }
            else
            {
                _logger.LogWarning("Could not find exp claim. Using default 30 mins expiration.");
                _tokenExpiryTime = DateTime.UtcNow.AddMinutes(30);
            }
        }

        public async Task SendEmailAsync(string appName, string apiKey, EmailRequest emailRequest)
        {
            await EnsureTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _jwtToken);
            var response = await _httpClient.PostAsJsonAsync(_functionUrl, emailRequest);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                _logger.LogError("Failed to send email. Status: {StatusCode}, Error: {Error}", response.StatusCode, error);
                throw new Exception($"Failed to send email: {response.StatusCode}");
            }
        }
    }

    public class AuthResponse
    {
        public string Token { get; set; }
    }

    public class EmailRequest
    {
        public string AppName { get; set; } // App sending the email
        public string MailTo { get; set; }
        public string MailFrom { get; set; }
        public string MailSubject { get; set; }
        public string Body { get; set; }
        public string Priority { get; set; }
    }

}
