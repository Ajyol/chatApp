using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer
{
    internal class Client
    {
        public string Username { get; set; }
        public Guid UID { get; set; }
        public TcpClient ClientScoket { get; set; }
        public Client(TcpClient client) {
            this.ClientScoket = client;
            UID = Guid.NewGuid();

            Console.WriteLine($"[{DateTime.Now}]: Client has connected with the username: {Username}");
        
        }
    }
}
