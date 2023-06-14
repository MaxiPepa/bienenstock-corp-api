using System.Security.Claims;

namespace BienenstockCorpAPI.Models.AutenticationModels
{
    public class LogoutResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
    }
}
