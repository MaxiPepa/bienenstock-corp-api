namespace BienenstockCorpAPI.Models.UserModels
{
    public class ModifyUserRequest
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string UserType { get; set; } = null!;
    }

    public class ModifyUserResponse
    {
        public string? Message { get; set; }
        public bool Success { get; set; }
    }
    
}
