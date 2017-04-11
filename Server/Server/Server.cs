using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;

namespace Server
{
    class Server
    {
        public TcpListener Listner; //Обьект принимающий TCP-клиантов.
        public List<ClientInfo> clients = new List<ClientInfo>();
        public List<ClientInfo> newClients = new List<ClientInfo>();
        public static Server server;
        static System.IO.TextWriter Out;

        public Server(int Port, System.IO.TextWriter _Out)
        {
            Out = _Out;
            Server.server = this;

            //Создаем слушателя для указанного порта
            Listner = new TcpListener(IPAddress.Any, Port);
            Listner.Start();
        }

        public void Work()
        {
            Thread clientListener = new Thread(ListnerClients);
            clientListener.Start();
            while (true)
            {
                foreach (ClientInfo client in clients)
                {
                    if (client.IsConnect)
                    {
                        NetworkStream stream = client.Client.GetStream();
                        while (stream.DataAvailable)
                        {
                            int ReadByte = stream.ReadByte();
                            if (ReadByte != -1)
                            {
                                client.buffer.Add((byte)ReadByte);
                            }
                        }

                        if (client.buffer.Count > 0)
                        {
                            Out.WriteLine("Resend");
                            foreach (ClientInfo otherClient in clients)
                            {
                                byte[] msg = client.buffer.ToArray();
                                client.buffer.Clear();
                                foreach (ClientInfo _otherClient in clients)
                                {
                                    _otherClient.Client.GetStream().Write(msg, 0, msg.Length);
                                }
                            }
                        }
                    }
                }
                if (newClients.Count > 0)
                {
                    clients.AddRange(newClients);
                    newClients.Clear();
                }
            }
        }

        //Остановка сервера
        ~Server()
        {
            if (Listner != null)
            {
                Listner.Stop();
            }
        }

        static void ListnerClients()
        {
            while (true)
            {
                TcpClient client1 = Server.server.Listner.AcceptTcpClient();
                Server.server.newClients.Add(new ClientInfo(client1));
                Out.WriteLine("New client " + client1.Client.RemoteEndPoint);
            }
        }
    }

    class ClientInfo
    {
        public TcpClient Client;
        public List<byte> buffer = new List<byte>();
        public bool IsConnect;
        public ClientInfo (TcpClient Client)
        {
            this.Client = Client;
            IsConnect = true;
        }
    }
}
