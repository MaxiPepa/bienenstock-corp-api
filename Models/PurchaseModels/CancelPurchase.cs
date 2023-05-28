namespace BienenstockCorpAPI.Models.PurchaseModels
{
    public class CancelPurchaseRequest
    {
        public int PurchaseId { get; set; }
    }

    public class CancelPurchaseResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
    }
}
