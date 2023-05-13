namespace BienenstockCorpAPI.Models.LogModels
{
    public class SaveLogRequest
    {
        public string Description { get; set; }
        public int UserId { get; set; }
    }

    public class SaveLogResponse
    {
        public string Message { get; set; }
        public bool Success { get; set; }
    }
}
