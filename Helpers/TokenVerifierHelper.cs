using System.Security.Claims;

namespace BienenstockCorpAPI.Helpers
{
    public class VerifyResponse
    {
        public int? UserId { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    public class TokenVerifierHelper
    {

        public static VerifyResponse TokenVerifier(ClaimsIdentity identity) 
        {
            try
            {
                if (identity == null || identity.Claims.Count() == 0)
                {
                    return new VerifyResponse
                    {
                        Success = false,
                        Message = "Invalid credentials",
                    };
                }

                return new VerifyResponse
                {
                    UserId = Int32.Parse(identity.Claims.FirstOrDefault(x => x.Type == "UserId").Value),
                    Success = true,
                    Message = "Token verified correctly"
                };
            }   
            catch (Exception ex)
            {
                return new VerifyResponse
                {
                    Success = false,
                    Message = ex.Message,
                };
            };
        }
    }
}
