namespace BienenstockCorpAPI.Models.UserModels
{
    public class ChangePasswordRequest
    {
        public string Password { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }

    }

    public class ChangePasswordResponse
    {
        public string Message { get; set; }
        public bool Success { get; set; }

    }
}
