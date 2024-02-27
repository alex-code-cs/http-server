using System.Net.Sockets;
using System.Net;
using System.Diagnostics;
using System;
using System.IO;

namespace Сокет2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Any, 8080);
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(ipPoint);   //связывает объект Socket с локальной конечной точкой
            socket.Listen(10);  // начинает прослушивание входящих запросов
            while (true)
            {
                Console.WriteLine(Directory.GetCurrentDirectory());
                Socket client = socket.Accept(); //создает новый объект Socket для обработки входящего подключения
                byte[] buffer = new byte[4096];
                client.Receive(buffer);     //получает данные
                string request = System.Text.Encoding.ASCII.GetString(buffer);
                Console.WriteLine(request);
                string fileName = request.Split('\n')[0].Split(' ')[1];
                
                if (fileName.Length > 1 && File.Exists(Directory.GetCurrentDirectory()+"//"+fileName))
                {
                    fileName = fileName.Substring(1, fileName.Length - 1);
                    StreamReader sr = new StreamReader(fileName);
                    string content = sr.ReadToEnd();
                    sr.Close();
                    string responce = $"HTTP/1.1 200 OK\nContent-Type: text/html\nContent-Length:{content.Length}\n\n{content}";
                    client.Send(System.Text.Encoding.ASCII.GetBytes(responce.ToCharArray()));

                } else if (fileName.Length <= 1) {
                    StreamReader sr = new StreamReader("index.html");
                    string content = sr.ReadToEnd();
                    sr.Close();
                    string responce = $"HTTP/1.1 200 OK\nContent-Type: text/html\nContent-Length:{content.Length}\n\n{content}";
                    client.Send(System.Text.Encoding.ASCII.GetBytes(responce.ToCharArray()));
                } else {
                    StreamReader sr = new StreamReader("404.html");
                    string content = sr.ReadToEnd();
                    sr.Close();
                    string responce = $"HTTP/1.1 200 OK\nContent-Type: text/html\nContent-Length:{content.Length}\n\n{content}";
                    client.Send(System.Text.Encoding.ASCII.GetBytes(responce.ToCharArray()));
                }
                client.Close();
            }          
            Console.ReadKey();

        }
       
    }
}