using System.Net.Sockets;
using System.Net;
using System.Diagnostics;
using System;
using System.IO;
using System.Text;

namespace HttpServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Any, 8080);
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            socket.Bind(ipPoint);   //связывает объект Socket с локальной конечной точкой
            socket.Listen(10);  // начинает прослушивание входящих запросов
            while (true)
            {
                Console.WriteLine(Directory.GetCurrentDirectory());
                Socket client = socket.Accept(); //создает новый объект Socket для обработки входящего подключения
                byte[] buffer = new byte[4096];
                client.Receive(buffer);     //получает данные
                HttpRequest request = new HttpRequest(Encoding.Default.GetString(Encoding.Convert(Encoding.ASCII, Encoding.Default, buffer)));

                if (request.url.Length > 1 && File.Exists(Directory.GetCurrentDirectory()+"//"+request.url))
                {
                    request.url = request.url.Substring(1, request.url.Length - 1);
                    StreamReader sr = new StreamReader(request.url);
                    string content = sr.ReadToEnd();
                    sr.Close();
                    string responce = $"HTTP/1.1 200 OK\nContent-Type: text/html\nContent-Length:{content.Length}\n\n{content}";
                    client.Send(Encoding.ASCII.GetBytes(responce.ToCharArray()));

                } else if (request.url.Length <= 1) {
                    StreamReader sr = new StreamReader("index.html");
                    string content = sr.ReadToEnd();
                    sr.Close();
                    string responce = $"HTTP/1.1 200 OK\nContent-Type: text/html\nContent-Length:{content.Length}\n\n{content}";
                    client.Send(Encoding.ASCII.GetBytes(responce.ToCharArray()));
                } else {
                    StreamReader sr = new StreamReader("404.html");
                    string content = sr.ReadToEnd();
                    sr.Close();
                    string responce = $"HTTP/1.1 200 OK\nContent-Type: text/html\nContent-Length:{content.Length}\n\n{content}";
                    client.Send(Encoding.ASCII.GetBytes(responce.ToCharArray()));
                }
                client.Close();
            }          
            Console.ReadKey();

        }
       
    }
}