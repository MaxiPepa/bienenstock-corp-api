using BienenstockCorpAPI.Data.Entities;
using BienenstockCorpAPI.Data;

namespace BienenstockCorpAPI.Services
{
    public class ProductService
    {
        private readonly BienenstockCorpContext _context;

        public ProductService(BienenstockCorpContext context)
        {
            _context = context;
        }

        public IEnumerable<Product> GetProducts()
        {
            return _context.Product.ToList();
        }
    }
}
