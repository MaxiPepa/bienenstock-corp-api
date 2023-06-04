namespace BienenstockCorpAPI.Models.MessageModels
{
    public class SaveMessageRequest
    {
        public string Description { get; set; } = null!;
    }

    public class SaveMessageResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; } 
    }
}
