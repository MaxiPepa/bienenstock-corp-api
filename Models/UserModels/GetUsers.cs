namespace BienenstockCorpAPI.Models.UserModels
{
    public class GetUsersResponse
    {
        public List<Item> Users { get; set; } = null!;

        public class Item
        {
            public int UserId { get; set; }
            public string FullName { get; set; } = null!;
            public string Email { get; set; } = null!;
            public string UserType { get; set; } = null!;
        }
    }
}
