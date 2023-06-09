﻿namespace BienenstockCorpAPI.Models.AutenticationModels
{
    public class LoginRequest
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

    public class TokenResponse
    {
        public string Token { get; set; } = null!;
        public int UserId { get; set; }
        public string? Avatar { get; set; }
        public string Email { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string UserType { get; set; } = null!;
        public DateTime Expiration { get; set; }
        public bool Success { get; set; }
        public string? Message { get; set; }
    }

    public class LoginResponse
    {
        public int UserId { get; set; }
        public string? Avatar { get; set; }
        public string Email { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string UserType { get; set; } = null!;
        public DateTime Expiration { get; set; }
        public bool Success { get; set; }
        public string? Message { get; set; }
    }
}
