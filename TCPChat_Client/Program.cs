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

            while (true)
            {
                Console.WriteLine("Введите сообщение для отправки на сервер:");
                string message = Console.ReadLine();
                byte[] bytes = Encoding.Unicode.GetBytes(message);

                socket_sender.Send(bytes); //sending data to server

                Console.WriteLine($"Посылка \"{message}\" отправлена на сервер");

                byte[] bytes_answer = new byte[1024];
                int num_bytes = socket_sender.Receive(bytes_answer); //получение сообщения от клиента
                string answer = Encoding.Unicode.GetString(bytes_answer, 0, num_bytes);
                Console.WriteLine(answer);
            }

            Console.ReadLine();
        }
    }
}
