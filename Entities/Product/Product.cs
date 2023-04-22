using System;


namespace BienenstockCorpAPI.Entities.Product
{
    public class Product
    {

        private int ProductId { get; set; }
        private string ProductName { get; set; }
        private DateTime EnterDate { get; set; }
        private DateTime? ExpirationDate { get; set; }
        private decimal Price { get; set; }

        public Product() { }
    }
}
