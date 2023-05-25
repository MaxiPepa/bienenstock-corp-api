namespace BienenstockCorpAPI.Models.UserModels
{
    public class ChangePasswordRequest
    {
        public string Password { get; set; } = null!;
        public string NewPassword { get; set; } = null!;
        public string ConfirmPassword { get; set; } = null!;
    }

    public class ChangePasswordResponse
    {
        public string? Message { get; set; }
        public bool Success { get; set; }
    }
}
