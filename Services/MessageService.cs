using BienenstockCorpAPI.Data.Entities;
using BienenstockCorpAPI.Data;
using Microsoft.EntityFrameworkCore;
using BienenstockCorpAPI.Models.MessageModels;
using System.Security.Claims;
using BienenstockCorpAPI.Helpers;

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
                    FullName = m.User.FullName,
                }).ToList(),
            };
        }

        public async Task<SaveMessageResponse> SaveMessage(SaveMessageRequest rq, ClaimsIdentity? identity)
        {
            var token = identity.TokenVerifier();

            var validation = ValidateSaveMessage(rq, token);

            if (!string.IsNullOrEmpty(validation))
            {
                return new SaveMessageResponse
                {
                    Success = false,
                    Message = validation,
                };
            }

            var message = new Message
            {
                Desciption = rq.Description,
                UserId = token.UserId,
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

        #region Validations
        public static string ValidateSaveMessage(SaveMessageRequest rq, TokenVerifyResponse token)
        {
            var error = String.Empty;

            // Token
            if (!token.Success)
            {
                error = token.Message;
            }

            // Request
            if (rq == null || string.IsNullOrEmpty(rq.Description))
            {
                error = "There was a problem with your request";
            }

            return error;
        }
        #endregion
    }
}
