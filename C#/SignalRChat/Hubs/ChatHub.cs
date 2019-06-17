using System;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using SignalRChat.Models;
namespace SignalRChat
{
    public class ChatHub : Hub
    {
        static List<Users> SignalRUsers = new List<Users>();
        static HashSet<string> CurrentConnections = new HashSet<string>();

        public void Send(string name, string message)
        {
            // Call the addNewMessageToPage method to update clients.
            Clients.All.addNewMessageToPage(name, message);
        }

        

        public override Task OnConnected()
        {
            var id = Context.ConnectionId;
            CurrentConnections.Add(id);

            return base.OnConnected();
        }

        
        //Saves the name of the connected user
        public void Connect(string userName)
        {
            var id = Context.ConnectionId;

            if (SignalRUsers.Count(x => x.ConnectionId == id) == 0)
            {
                SignalRUsers.Add(new Users { ConnectionId = id, UserName = userName });
            }
        }

        public override System.Threading.Tasks.Task OnDisconnected()
        {
            var item = SignalRUsers.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            if (item != null)
            {
                SignalRUsers.Remove(item);
            }

            return base.OnDisconnected();
        }


        //return list of all users that are connected
        public List<string> GetAllActiveConnections()
        {
            HashSet<string> ConnectedUsers = new HashSet<string>();
            foreach (Users user in SignalRUsers)
            {
                ConnectedUsers.Add(user.UserName);
            }
            return ConnectedUsers.ToList();
        }
    }
}