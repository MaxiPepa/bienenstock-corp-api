using BienenstockCorpAPI.Data.Entities;
using BienenstockCorpAPI.Data;
using Microsoft.EntityFrameworkCore;
using BienenstockCorpAPI.Models.Message;

namespace BienenstockCorpAPI.Services
{
    public class MessageService
    {
        #region Constructor
        private readonly BienenstockCorpContext _context;

        public MessageService(BienenstockCorpContext context)
        {
            _context = context;
        }
        #endregion

        #region Message
        public async Task<GetMessagesResponse> GetMessages()
        {
            var messages = await _context.Message
                .Include(x => x.User)
                .ToListAsync();

            return new GetMessagesResponse
            {
                Messages = messages.Select(m => new GetMessagesResponse.Item
                {
                    MessageId = m.MessageId,
                    Description = m.Desciption,
                    Date = m.Date,
                    Avatar = m.User.Avatar,
                    FullName = m.User.Name + ' ' + m.User.LastName,
                }).ToList(),
            };
        }

        public async Task<SaveMessageResponse> SaveMessage(SaveMessageRequest rq)
        {
            if (rq == null || string.IsNullOrEmpty(rq.Description))
            {
                return new SaveMessageResponse
                {
                    Success = false,
                    Message = "There was a problem with your request"
                };
            }

            var message = new Message
            {
                Date = DateTime.Now,
                Desciption = rq.Description,
                UserId = 1,
            };

            try
            {
                _context.Message.Add(message);
                await _context.SaveChangesAsync();

                return new SaveMessageResponse
                {
                    Success = true,
                    Message = "Message succesfully created"
                };
            }
            catch (Exception ex)
            {
                return new SaveMessageResponse
                {
                    Success = false,
                    Message = ex.Message,
                };
            }
        }
        #endregion
    }
}
