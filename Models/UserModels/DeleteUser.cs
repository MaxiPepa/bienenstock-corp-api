namespace BienenstockCorpAPI.Models.UserModels
{
    public class DeleteUserRequest
    {
        public int UserId { get; set; }
    }

    public class DeleteUserResponse
    {
        public string Message { get; set; }
        public bool Success { get; set; }
    }
}
