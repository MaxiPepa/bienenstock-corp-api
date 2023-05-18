namespace BienenstockCorpAPI.Models.UserModels
{
    public class ModifyUserRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserType { get; set; }
    }

    public class ModifyUserResponse
    {
        public string Message { get; set; }
        public bool Success { get; set; }
    }
    
}
