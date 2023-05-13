namespace BienenstockCorpAPI.Models.LogModels
{
    public class GetLogsResponse
    {
        public List<LogItem> Logs { get; set; }

        public class LogItem
        {
            public int LogId { get; set; }
            public string Description { get; set; }
            public DateTime Date { get; set; }
            public string UserFullName { get; set; }
        }

    }
}