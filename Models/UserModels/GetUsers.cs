namespace BienenstockCorpAPI.Models.UserModels
{
    public class GetUsersResponse
    {
        public List<Item> Users { get; set; }

        public class Item
        {
            public int UserId { get; set; }
            public string FullName { get; set; }
            public string Email { get; set; }
            public string UserType { get; set; }
        }
    }
}
