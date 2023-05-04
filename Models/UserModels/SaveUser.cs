namespace BienenstockCorpAPI.Models.UserModels
{
    public class SaveUserRequest
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserType { get; set; }
    }

    public class SaveUserResponse
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }
    }
}
