using System;
using System.Net.Sockets;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace Chat
{
    class Client : IСhat
    {
        public string Ip;
        public readonly int defaultPort = 8888;
        private readonly string chatsFolder = Path.GetFullPath("ChatHistory");
        TcpClient _client;
        NetworkStream _stream;

        public Client(string ip)
        {
            Ip = ip;
            _client = new TcpClient(Ip, defaultPort);
            Console.WriteLine("Connected to server");
            _stream = _client.GetStream();
        }

        public Client(string ip, int port) 
        {
            Ip = ip;
            _client = new TcpClient(Ip, port);
            Console.WriteLine("Connected to server");
            _stream = _client.GetStream();
        }
        public void StartChat()
        {
            StreamReader reader = new StreamReader(_stream);
            StreamWriter writer = new StreamWriter(_stream);
            var date = DateTime.Now.ToString("dd-MM-yyyy");
            if (!Directory.Exists(chatsFolder))
            {
                Directory.CreateDirectory(chatsFolder);
            }
            var filePath = Path.Combine(chatsFolder, date + ".txt");
            StreamWriter fileWriter = new StreamWriter(filePath, true);
            fileWriter.AutoFlush = true;

            while (true)
            {
                string messageFromServer = reader.ReadLine();
                if (messageFromServer == null)
                {
                    Console.WriteLine("Server disconected");
                    return;
                }
                Console.WriteLine("Server: " + messageFromServer);
                Message messageFromServerToFile = new Message(messageFromServer, "Server");
                fileWriter.WriteLine(messageFromServer);

                if (Console.ReadKey().Key == ConsoleKey.Escape)
                {
                    EndChat();
                    return;
                }
                else if (Console.ReadKey().Key == ConsoleKey.Enter)
                {
                    Console.Write("Client: ");
                    string messageToSend = Console.ReadLine();
                    writer.WriteLine(messageToSend);
                    Message messageFromClientToFile = new Message(messageToSend, "Client");
                    fileWriter.WriteLine(messageFromClientToFile);
                }
            }
        }

        public void DeleteChat(string chatName)
        {
            string filePath = Path.Combine(chatsFolder, chatName + ".txt");
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }


        public void EndChat()
        {
            if (_stream != null)
            {
                _stream.Close();
            }
            else
            {
                Console.WriteLine("No stream is running");
            }
        }

        public string[] GetAllChats()
        {
            var files = Directory.GetFiles(chatsFolder);
            return files;
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
                        Message message = new Message(content);
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
    }
}
