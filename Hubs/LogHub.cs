using Microsoft.AspNetCore.SignalR;

namespace BienenstockCorpAPI.Hubs
{
    public class LogHub : Hub
    {
        #region Constructor
        public LogHub() { }
        #endregion

        #region Methods
        public override Task OnConnectedAsync()
        {
            var query = Context.GetHttpContext()?.Request.Query;

            Groups.AddToGroupAsync(Context.ConnectionId, query["HubCode"].ToString());

            return base.OnConnectedAsync();
        }
        #endregion
    }
}
