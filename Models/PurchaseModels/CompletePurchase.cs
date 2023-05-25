using System.Security.Claims;

namespace BienenstockCorpAPI.Models.PurchaseModels
{
    public class CompletePurchaseRequest
    {
        public int PurchaseId { get; set; }

        public List<Expiration> ExpirationDates { get; set; } = null!;

        public class Expiration
        {
            public int ProductId { get; set; }
            public DateTime ExpirationDate { get; set; }
        }
    }

    public class CompletePurchaseResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
    }
}
