using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace TCPChat_Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Server";
            Console.WriteLine("[SERVER]");

            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream,
                ProtocolType.Tcp); //TCP socket

            IPAddress address = IPAddress.Parse("127.0.0.1"); //server ip

            IPEndPoint endRemotePoint = new IPEndPoint(address, 7632); //creating endpoint //server port

            socket.Bind(endRemotePoint); //binding socket to endpoint

            socket.Listen(1); //слушаем на наличие клиентов

            Console.WriteLine("Ожидаем звонка от клиента");

            Socket client = socket.Accept(); //принять подключение

            Console.WriteLine("Клиент на связи!");

            //менеджер для приёма сообщений
            Thread threadRecieve = new Thread(RecieveMessageForManager); //создаём менеджера (что)
            threadRecieve.Start(client);
            //менеджер для отправки сообщений
            Thread threadSend = new Thread(SendMessageForManager);
            threadSend.Start(client);
        }

        private static string RecieveMessage(Socket client)
        {
            byte[] bytes = new byte[1024];
            int num_bytes = client.Receive(bytes); //получение сообщения от клиента
            return Encoding.Unicode.GetString(bytes, 0, num_bytes);
        }

        private static void RecieveMessageForManager(Object objSocket)
        {
            Socket client = objSocket as Socket;

            while (true)
            {
                Console.WriteLine($"[MANAGER]: {RecieveMessage(client)}");
            }
        }

        private static void SendMessageForManager(Object objSocket)
        {
            Socket client = objSocket as Socket;

            while (true)
            {
                string sendMessage = Console.ReadLine();
                SendMessage(client, sendMessage);
            }
        }

        private static void SendMessage(Socket client, string message)
        {
            byte[] bytes_answer = Encoding.Unicode.GetBytes(message);
            client.Send(bytes_answer);
        }
    }
}
