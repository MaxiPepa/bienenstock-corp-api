using BienenstockCorpAPI.Data.Entities;
using BienenstockCorpAPI.Data;
using BienenstockCorpAPI.Models.LogModels;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using BienenstockCorpAPI.Helpers;

namespace BienenstockCorpAPI.Services
{
    public class LogService
    {
        #region Constructor
        private readonly BienenstockCorpContext _context;

        public LogService(BienenstockCorpContext context)
        {
            _context = context;
        }
        #endregion

        #region Log
        public async Task<GetLogsResponse> GetLogs()
        {
            var logs = await _context.Log
                .Include(x => x.User)
                .ToListAsync();

            return new GetLogsResponse
            {
                Logs = logs.Select(l => new GetLogsResponse.LogItem
                {
                    LogId = l.LogId,
                    UserFullName = l.User.Name  + " " + l.User.LastName,
                    Description = l.Description,
                    Date = l.Date,
                }).OrderBy(x => x.Date).ToList(),
            };
        }

        public async Task<SaveLogResponse> CreateLog(SaveLogRequest rq, ClaimsIdentity? identity)
        {
            var token = identity.TokenVerifier();

            if (!token.Success) 
            {
                return new SaveLogResponse
                {
                    Message = "Error creating Log",
                    Success = false
                };
            }

            var logItem = new Log
            {
                Description = rq.Description,
                Date = DateTime.Now,
                UserId = rq.UserId,
            };

            try
            {
                _context.Log.Add(logItem);
                await _context.SaveChangesAsync();

                return new SaveLogResponse
                {
                    Message = "Log successfully created",
                    Success = true,
                };
            }
            catch (Exception ex)
            {
                return new SaveLogResponse
                {
                    Message = ex.Message,
                    Success = false,
                };
            }
        }
        #endregion
    }
}