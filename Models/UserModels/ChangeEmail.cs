namespace BienenstockCorpAPI.Models.UserModels
{
    public class ChangeEmailRequest
    {
        public string Email { get; set; }
    }

    public class ChangeEmailResponse
    {
        public string Email { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }
    }
}
