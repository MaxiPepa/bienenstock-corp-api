namespace BienenstockCorpAPI.Models.AutenticationModels
{
    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginResponse
    {
        public string Token { get; set; }
        public string? Avatar { get; set; }
        public string Email { get; set; }
        public string Fullname { get; set; }
        public DateTime Expiration { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
