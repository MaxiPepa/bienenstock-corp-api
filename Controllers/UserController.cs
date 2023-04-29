using BienenstockCorpAPI.Data;
using BienenstockCorpAPI.Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BienenstockCorpAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly BienenstockCorpContext _context;

        public UserController(BienenstockCorpContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<User> GetUsers()
        {
            return _context.User.ToList();
        }
    }
}
