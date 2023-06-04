namespace BienenstockCorpAPI.Models.ReportModels
{
    public class GetCompanyStatsResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public int TotalProducts { get; set; }
        public int TotalUsers { get; set; }
        public int TotalPurchases { get; set; }
        public int TotalSales { get; set; }
        public int TotalPendingProducts { get; set; }
        public int TotalReports { get; set; }
    }
}
