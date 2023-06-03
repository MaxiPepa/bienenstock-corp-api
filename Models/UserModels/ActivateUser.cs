namespace BienenstockCorpAPI.Models.UserModels
{
    public class ActivateUserRequest
    {
        public int UserId { get; set; }
    }

    public class ActivateUserResponse
    {
        public string? Message { get; set; }
        public bool Success { get; set; }
    }
}
