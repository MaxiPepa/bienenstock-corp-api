namespace BienenstockCorpAPI.Models.SaleModels
{
    public class CancelSaleRequest
    {
        public int SaleId { get; set; }
    }

    public class CancelSaleResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
    }
}
