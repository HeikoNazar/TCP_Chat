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

namespace Chat
{
    class Server : IСhat
    {
        public readonly IPAddress Ip;
        public TcpListener? _listener;
        public bool isConnected;
        public TcpClient? _client;
        NetworkStream? _stream;
        public int Port;
        private readonly int defaultPort = 8888;

        private readonly string chatsFolder = Path.GetFullPath("ChatHistory");

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
        public void DeleteChat(string chatName)
        {
            string filePath = Path.Combine(chatsFolder, chatName + ".txt");
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            else
            {
                Console.WriteLine($"No chat with {chatName} name");
            }
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
        public Chat GetChat(string chatName)
        {
            var path = Path.Combine(chatsFolder, chatName + ".txt");
            if (File.Exists(path))
            {
                Chat chat = new Chat(chatName);
                using (StreamReader reader = new StreamReader(path))
                {
                    string content;
                    do
                    {
                        content = reader.ReadLine();
                        chat.AddMessage(content);  
                    }
                    while (content != null);
                }
                return chat;
            }
            else
            {
                Console.WriteLine($"No chat with {chatName} name");
                return null;
            }
        }
        public string[] GetAllChats()
        {
            var files = Directory.GetFiles(chatsFolder);
            return files;
        }
    }
}
