namespace BienenstockCorpAPI.Models.LogModels
{
    public class CreateLogRequest
    {
        public string Description { get; set; } = null!;
        public int UserId { get; set; }
    }

    public class CreateLogResponse
    {
        public string? Message { get; set; }
        public bool Success { get; set; }
    }
}
