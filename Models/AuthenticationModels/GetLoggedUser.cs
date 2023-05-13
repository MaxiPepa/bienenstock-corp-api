using System.Security.Claims;

namespace BienenstockCorpAPI.Models.AutenticationModels
{
    public class GetLoggedUserRequest
    {
        public ClaimsIdentity? Identity { get; set; }
    }

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
