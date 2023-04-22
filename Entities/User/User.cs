using System.ComponentModel.DataAnnotations;

namespace BienenstockCorpAPI.Entities
{
    public class User
    {
        [Key]
        private int UserId { get; set; }
        private string Name { get; set; }
        private string LastName { get; set; }
        private string Email { get; set; }
        private string Password { get; set; }
        private string UserType { get; set; }
    }
}