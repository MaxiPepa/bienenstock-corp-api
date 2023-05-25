namespace BienenstockCorpAPI.Models.UserModels
{
    public class SaveUserRequest
    {
        public string Name { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string UserType { get; set; } = null!;
    }

    public class SaveUserResponse
    {
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Message { get; set; }
        public bool Success { get; set; }
    }
}
