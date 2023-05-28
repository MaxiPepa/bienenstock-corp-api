namespace BienenstockCorpAPI.Models.SaleModels
{
    public class DispatchSaleRequest
    {
        public int SaleId { get; set; }
        public DateTime DispatchDate { get; set; }
    }

    public class DispatchSaleResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
    }
}
