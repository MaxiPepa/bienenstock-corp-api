using BienenstockCorpAPI.Data.Entities;
using BienenstockCorpAPI.Data;

namespace BienenstockCorpAPI.Services
{
    public class UserService
    {
        private readonly BienenstockCorpContext _context;

        public UserService(BienenstockCorpContext context)
        {
            _context = context;
        }

        public IEnumerable<User> GetUsers()
        {
            return _context.User.ToList();
        }
    }
}
