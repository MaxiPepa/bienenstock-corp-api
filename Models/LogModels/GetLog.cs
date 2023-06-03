namespace BienenstockCorpAPI.Models.LogModels
{
    public class GetLogsResponse
    {
        public List<LogItem> Logs { get; set; } = null!;

        public class LogItem
        {
            public int LogId { get; set; }
            public string Description { get; set; } = null!;
            public DateTime Date { get; set; }
            public string UserFullName { get; set; } = null!;
            public string? UserAvatar { get; set; }
        }

    }
}