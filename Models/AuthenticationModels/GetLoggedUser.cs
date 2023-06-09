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
        public int UserId { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string UserType { get; set; } = null!;
        public bool Success { get; set; }
        public string? Message { get; set; }
    }
}
