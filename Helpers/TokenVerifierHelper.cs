using System.Security.Claims;

namespace BienenstockCorpAPI.Helpers
{
    public class TokenVerifyResponse
    {
        public int UserId { get; set; }
        public string Name { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string UserType { get; set; } = null!;
        public bool Success { get; set; }
        public string Message { get; set; } = null!;
    }

    public static class TokenVerifierHelper
    {

        public static TokenVerifyResponse TokenVerifier(this ClaimsIdentity? identity) 
        {
            try
            {
                if (identity == null || !identity.Claims.Any())
                {
                    return new TokenVerifyResponse
                    {
                        Success = false,
                        Message = "Invalid credentials",
                    };
                }

                return new TokenVerifyResponse
                {
                    UserId = Int32.Parse(identity.Claims.First(x => x.Type == "UserId").Value),
                    Name = identity.Claims.First(x => x.Type == "Name").Value,
                    LastName = identity.Claims.First(x => x.Type == "LastName").Value,
                    Email = identity.Claims.First(x => x.Type == "Email").Value,
                    UserType = identity.Claims.First(x => x.Type == "UserType").Value,
                    Success = true,
                    Message = "Token verified correctly"
                };
            }   
            catch (Exception ex)
            {
                return new TokenVerifyResponse
                {
                    Success = false,
                    Message = ex.Message,
                };
            };
        }
    }
}
