using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            int Port;
            //Получение имени пк.
            string host = System.Net.Dns.GetHostName();
            //Получения ip-адреса.
            System.Net.IPAddress ip = System.Net.Dns.GetHostEntry(host).AddressList[0];
            Console.WriteLine("Ip adress " + ip.ToString());
            Console.WriteLine("Введите номер порта: ");
            Port = Convert.ToInt32(Console.ReadLine());
            Server server = new Server(Port, Console.Out);
            Console.WriteLine("Starting...");
            server.Work();
        }
    }
}
