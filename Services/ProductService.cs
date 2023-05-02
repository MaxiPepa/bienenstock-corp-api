using BienenstockCorpAPI.Data.Entities;
using BienenstockCorpAPI.Data;

namespace BienenstockCorpAPI.Services
{
    public class ProductService
    {
        #region Constructor
        private readonly BienenstockCorpContext _context;

        public ProductService(BienenstockCorpContext context)
        {
            _context = context;
        }
        #endregion

        #region Products
        public IEnumerable<Product> GetProducts()
        {
            return _context.Product.ToList();
        }
        #endregion
    }
}
