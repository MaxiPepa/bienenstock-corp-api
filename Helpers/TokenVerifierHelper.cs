using System.Security.Claims;

namespace BienenstockCorpAPI.Helpers
{
    public class VerifyResponse
    {
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
