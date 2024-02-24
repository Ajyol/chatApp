﻿using ChatClient.Net.IO;
using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace ChatServer
{
    class Program
    {
        static List<Client> _users;
        static TcpListener _listener;
        static void Main(string[] args)
        {
            _users = new List<Client>();
            _listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 7891);
            _listener.Start();

            while (true)
            {
                var client = new Client(_listener.AcceptTcpClient());
                _users.Add(client);

                /* Broadcast the connection to everyone on the server */
                BroadcastConnection();
            }
        }

        static void BroadcastConnection()
        {
            foreach(var user in _users) 
            {
                foreach (var usr in _users)
                {
                    var msgPacket = new PacketBuilder();
                    msgPacket.WriteOpCode(1);
                    msgPacket.WriteMessage(usr.Username);
                    msgPacket.WriteMessage(usr.UID.ToString());
                    usr.ClientScoket.Client.Send(msgPacket.GetPacketBytes());
                }
            }
        }

        public static void BroadcastMessage(string message)
        {
            foreach( var user in _users)
            {
                var broadcastPacket = new PacketBuilder();
                broadcastPacket.WriteOpCode(5);
                broadcastPacket.WriteMessage(message);
                user.ClientScoket.Client.Send(broadcastPacket.GetPacketBytes());
            }
        }

        public static void BroadcastDisconnect(string uid)
        {
            var disconnectedUser = _users.Where(x => x.UID.ToString() == uid).FirstOrDefault();
            foreach (var user in _users)
            {
                var broadcastPacket = new PacketBuilder();
                broadcastPacket.WriteOpCode(10);
                broadcastPacket.WriteMessage(uid);
                user.ClientScoket.Client.Send(broadcastPacket.GetPacketBytes());
            }

            BroadcastMessage($"[{disconnectedUser.Username}] disconnected!");
        }

    }
}