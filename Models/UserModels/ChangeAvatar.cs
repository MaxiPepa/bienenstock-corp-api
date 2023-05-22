namespace BienenstockCorpAPI.Models.UserModels
{
    public class SaveChangeAvatarRequest
    {
        public string? Avatar { get; set; }
    }

    public class SaveChangeAvatarResponse
    {   
        public string Message { get; set; }
        public bool Success { get; set; }
        public string Avatar { get; set; }

    }
}
    