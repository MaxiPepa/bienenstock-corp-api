namespace BienenstockCorpAPI.Models.LogModels
{
    public class SaveLogRequest
    {
        public string Description { get; set; } = null!;
        public int UserId { get; set; }
    }

    public class SaveLogResponse
    {
        public string? Message { get; set; }
        public bool Success { get; set; }
    }
}
