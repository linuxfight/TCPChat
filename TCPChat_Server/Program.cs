using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

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

            IPAddress address = IPAddress.Parse("127.0.0.1");

            IPEndPoint endPoint = new IPEndPoint(address, 7632); //creating endpoint

            socket.Bind(endPoint); //binding socket to endpoint

            socket.Listen(1); //слушаем на наличие клиентов

            Console.WriteLine("Ожидаем звонка от клиента");

            Socket client = socket.Accept(); //принять подключение

            Console.WriteLine("Клиент на связи!");

            while (true)
            {
                byte[] bytes = new byte[1024];
                int num_bytes = client.Receive(bytes); //получение сообщения от клиента
                string textFromClient = Encoding.Unicode.GetString(bytes, 0, num_bytes);
                Console.WriteLine(textFromClient);

                //ответное сообщение от сервера к клиенту
                string answer = "Server: OK";
                byte[] bytes_answer = Encoding.Unicode.GetBytes(answer);
                client.Send(bytes_answer);
            }

            Console.ReadLine();
        }
    }
}
