using BienenstockCorpAPI.Data.Entities;
using BienenstockCorpAPI.Data;
using BienenstockCorpAPI.Models.LogModels;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using BienenstockCorpAPI.Helpers;
using BienenstockCorpAPI.Helpers.Consts;
using Microsoft.AspNetCore.SignalR;
using BienenstockCorpAPI.Hubs;

namespace BienenstockCorpAPI.Services
{
    public class LogService
    {
        #region Constructor
        private readonly BienenstockCorpContext _context;
        private readonly IHubContext<PageHub> _pageHub;

        public LogService(BienenstockCorpContext context, IHubContext<PageHub> pageHub)
        {
            _context = context;
            _pageHub = pageHub;
        }
        #endregion

        #region Log
        public async Task<GetLogsResponse> GetLogs(ClaimsIdentity? identity)
        {
            var token = identity.TokenVerifier();

            var userType = token.UserType;

            var query = _context.Log
                .Include(x => x.User)
                .AsQueryable();

            if (userType == UserType.BUYER || userType == UserType.SELLER || userType == UserType.DEPOSITOR)
                query.Where(x => x.User.UserType == UserType.BUYER || x.User.UserType == UserType.SELLER || x.User.UserType == UserType.DEPOSITOR);

            if (userType == UserType.ANALYST)
                query.Where(x => x.User.UserType == UserType.ANALYST);

            var logs = await query.ToListAsync(); 

            return new GetLogsResponse
            {
                Logs = logs.Select(l => new GetLogsResponse.LogItem
                {
                    LogId = l.LogId,
                    UserFullName = l.User.FullName,
                    UserAvatar = l.User.Avatar,
                    Description = l.Description,
                    Date = l.Date,
                }).OrderByDescending(x => x.Date).ToList(),
            };
        }

        public async Task<CreateLogResponse> CreateLog(CreateLogRequest rq)
        { 
            var logItem = new Log
            {
                Description = rq.Description,
                UserId = rq.UserId,
            };

            try
            {
                _context.Log.Add(logItem);
                await _context.SaveChangesAsync();

                LogUpdate(HubCode.LOG);

                return new CreateLogResponse
                {
                    Message = "Log successfully created",
                    Success = true,
                };
            }
            catch (Exception ex)
            {
                return new CreateLogResponse
                {
                    Message = ex.Message,
                    Success = false,
                };
            }
        }
        #endregion

        #region Privates
        private void LogUpdate(string hubCode)
        {
            if (string.IsNullOrEmpty(hubCode))
                return;

            var group = _pageHub.Clients.Group(hubCode);

            // Trigger client side update
            if (group != null)
                group.SendAsync("LogUpdate");
        }
        #endregion
    }
}