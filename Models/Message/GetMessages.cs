namespace BienenstockCorpAPI.Models.Message
{
    public class GetMessagesResponse 
    {
        public List<Item> Messages { get; set; }

        public class Item
        {
            public int MessageId { get; set; }
            public string Description { get; set; }
            public DateTime Date { get; set; }
            public string Avatar { get; set; }
            public string FullName { get; set; }
        }
    }
}
