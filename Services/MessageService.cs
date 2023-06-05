using BienenstockCorpAPI.Data.Entities;
using BienenstockCorpAPI.Data;
using Microsoft.EntityFrameworkCore;
using BienenstockCorpAPI.Models.MessageModels;
using System.Security.Claims;
using BienenstockCorpAPI.Helpers;
using Microsoft.AspNetCore.SignalR;
using BienenstockCorpAPI.Hubs;
using BienenstockCorpAPI.Helpers.Consts;

namespace BienenstockCorpAPI.Services
{
    public class MessageService
    {
        #region Constructor
        private readonly BienenstockCorpContext _context;
        private readonly IHubContext<ChatHub> _chatHub;

        public MessageService(BienenstockCorpContext context, IHubContext<ChatHub> chatHub)
        {
            _context = context;
            _chatHub = chatHub;
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

                ChatUpdate(HubCode.CHAT);

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

        #region Privates
        private void ChatUpdate(string hubCode)
        {
            if (string.IsNullOrEmpty(hubCode))
                return;

            var group = _chatHub.Clients.Group(hubCode);

            // Trigger client side update
            if (group != null)
                group.SendAsync("ChatUpdate");
        }
        #endregion
    }
}
