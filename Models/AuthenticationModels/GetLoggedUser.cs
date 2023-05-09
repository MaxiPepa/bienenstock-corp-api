namespace BienenstockCorpAPI.Models.AutenticationModels
{
    public class GetLoggedUserResponse
    {
        public string? Avatar { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string UserType { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
