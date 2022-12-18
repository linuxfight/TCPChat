using System;
using System.Net;
using System.Net.Sockets;
using System.Text; //для кодирования

namespace TCPChat_Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Client";
            Console.WriteLine("[CLIENT]");

            Socket socket_sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream,
                ProtocolType.Tcp);

            IPAddress address = IPAddress.Parse("127.0.0.1");
            IPEndPoint endPoint = new IPEndPoint(address, 7632);

            Console.WriteLine("Нажмите Enter для подключения");

            Console.ReadLine();

            socket_sender.Connect(endPoint); //connecting to remote endpoint

            Action<Socket> taskSendMessage = SendMessageForTask;
            Action<Socket> taskRecieveMessage = RecieveMessageForTask;

            IAsyncResult resSend = taskSendMessage.BeginInvoke(socket_sender, null, null);
            IAsyncResult resRecieve = taskRecieveMessage.BeginInvoke(socket_sender, null, null);

            taskSendMessage.EndInvoke(resSend);
            taskRecieveMessage.EndInvoke(resRecieve);
        }

        public static void SendMessageForTask(Socket socket_sender)
        {
            while (true)
            {
                string message = Console.ReadLine();
                SendMessage(socket_sender, $"[CLIENT]: {message}");
            }
        }

        public static void RecieveMessageForTask(Socket socket)
        {
            while (true)
            {
                Console.WriteLine(RecieveMessage(socket));
            }
        }

        private static void SendMessage(Socket client, string message)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(message);
            client.Send(bytes); //sending data to server
        }

        private static string RecieveMessage(Socket client)
        {
            byte[] byte_answer = new byte[1024];
            int num_bytes = client.Receive(byte_answer); //получение сообщения от сервера
            return Encoding.Unicode.GetString(byte_answer, 0, num_bytes);
        }
    }
}
