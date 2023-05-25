namespace BienenstockCorpAPI.Models.AutenticationModels
{
    public class Jwt
    {
        public string Key { get; set; } = null!;
        public string Issuer { get; set; } = null!;
        public string Audience { get; set; } = null!;
        public string Subject { get; set; }  = null!;
    }
}
