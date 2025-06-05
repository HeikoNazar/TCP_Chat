using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using System.IO;
using System.Diagnostics;

namespace Chat
{
    class Server : WorkWithFiles,IСhat
    {
        public readonly IPAddress Ip;
        public TcpListener? _listener;
        
        public Server()
        {
            var host = Dns.GetHostName();
            Ip = Dns.GetHostEntry(host).AddressList[3]; 
        }
        public Server(int port)
        {
            Port = port;
            var host = Dns.GetHostName();
            Ip = Dns.GetHostEntry(host).AddressList[3];
        }
        

        public void StartChat()
        {
            int port = Port == 0 ? defaultPort : Port;    
            _listener = new TcpListener(Ip,port);
            _listener.Start();
            Console.WriteLine($"Server is running on {Ip.ToString()}::{port}. Waiting for client...");
            _client = _listener.AcceptTcpClient();
            Console.WriteLine("Client connected");
            _stream = _client.GetStream();
            StreamReader reader = new StreamReader(_stream, System.Text.Encoding.UTF8);
            StreamWriter writer = new StreamWriter(_stream, System.Text.Encoding.UTF8);
            isConnected = true;
            writer.AutoFlush = true;
            var date = DateTime.Now.ToString("dd-MM-yyyy");
            if (!Directory.Exists(chatsFolder))
            {
                Directory.CreateDirectory(chatsFolder);
            }
            var filePath = Path.Combine(chatsFolder, date + ".txt");
            using (StreamWriter fileWriter = new StreamWriter(filePath, true))
            {

                fileWriter.AutoFlush = true;
                while (true)
                {
                    var key = Console.ReadKey();
                    if (key.Key == ConsoleKey.Escape)
                    {
                        EndChat();
                        return;
                    }
                    else if (key.Key == ConsoleKey.Enter)
                    {
                        Console.Write("Server: ");

                        string? messageToSend = Console.ReadLine();

                        writer.WriteLine(messageToSend);
                        Message messageWriteToFile = new Message(messageToSend, "Server");
                        fileWriter.WriteLine(messageWriteToFile.ToString());


                        string messagefromClient = reader.ReadLine();
                        if (messagefromClient == null)
                        {
                            Console.WriteLine("Client is disconnected");
                            return;
                        }
                        Console.WriteLine("Client:" + messagefromClient);
                        Message messageFromClientToFile = new Message(messagefromClient, "Client");
                        fileWriter.WriteLine(messageFromClientToFile.ToString());
                    }
                }
            }
        }
        public void EndChat()
        {
            if (isConnected)
            {
                _stream.Dispose();
                _listener.Stop();
                _client.Dispose();
            }
            else
            {
                Console.WriteLine("No stream is running");
            }
        }
        
    }
}
