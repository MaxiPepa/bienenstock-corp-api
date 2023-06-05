namespace BienenstockCorpAPI.Models.ReportModels
{
    public class GetCompanyStatsResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public int TotalProducts { get; set; }
        public int TotalUsers { get; set; }
        public int CompletedPurchases { get; set; }
        public int CompletedSales { get; set; }
        public int PendingTransactions { get; set; }
        public int TotalReports { get; set; }
    }
}
