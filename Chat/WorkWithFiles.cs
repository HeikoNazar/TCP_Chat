using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Chat
{
    class WorkWithFiles
    {
        protected readonly int defaultPort = 8888;
        public int Port;
        protected readonly string chatsFolder = Path.GetFullPath("ChatHistory");
        public bool isConnected;
        protected TcpClient? _client;
        protected NetworkStream? _stream;

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
