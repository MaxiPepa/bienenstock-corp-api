namespace BienenstockCorpAPI.Models.MessageModels
{
    public class GetMessagesResponse 
    {
        public List<Item> Messages { get; set; } = null!;

        public class Item
        {
            public int MessageId { get; set; }
            public string Description { get; set; } = null!;
            public DateTime Date { get; set; }
            public string? Avatar { get; set; }
            public string FullName { get; set; } = null!;
        }
    }
}
