using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System.Linq.Expressions;
using Chat;
using System.Threading.Channels;

namespace Chat;
class Program
{
    static void Main() 
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.WriteLine("Choose role");
        string[] roles = { "1. Server", "2. Client" };
        foreach (var item in roles)
        {
            Console.WriteLine(item);
        }
        var role = Convert.ToInt32(Console.ReadLine());
        if (role == 1)
        {
            Server server;
            Console.WriteLine("Input port");
            var port = Convert.ToInt32(Console.ReadLine());
            if (port == 0)
            {
                server = new Server();
            }
            else
            {
                server = new Server(port);
            }
            RunningRole(server);
        }
        else if (role == 2)
        {
            Client client;
            Console.WriteLine("Input ip");
            string ip = Console.ReadLine();
            Console.WriteLine("Input port");
            var port = Convert.ToInt32(Console.ReadLine());
            if (port == 0)
            {
                client = new Client(ip);
            }
            else
            {
                client =  new Client(ip, port);
            }
            RunningRole(client);
        }
        else
        {
            throw new FormatException("Wrong format");
        }
    }
    static void RunningRole(IСhat role)
    {
        string[] options = { "1.Start chat", "2. Show chat", "3. Show all chats", "4. Delete chat", "5. Stop program","6. Clear console"};
        bool isWorking = true;
        while (isWorking)
        {
            Console.WriteLine("Choose option");
            foreach (var item in options)
            {
                Console.WriteLine(item);
            }
            int choise = Convert.ToInt32(Console.ReadLine());
            switch (choise)
            {
                case 1:
                    role.StartChat();
                    break;
                case 2:
                    Console.WriteLine("Input chat name \t Format: dd-MM-yyyy");
                    string chatName = Console.ReadLine();
                    var chat = role.GetChat(chatName);
                    if(chat == null)
                    {
                        break;
                    }
                    foreach (var item in chat.Messages)
                    {
                        Console.WriteLine(item);
                    }
                    break;
                case 3:
                    var chats = role.GetAllChats();
                    if (chats.Length == 0)
                    {
                        Console.WriteLine("No chats in history");
                    }
                    foreach (var item in chats)
                    {
                        Console.WriteLine(item);
                    }
                    break;
                case 4:
                    Console.WriteLine("Input chat name \t Format: dd-MM-yyyy");
                    var chatToDelete = Console.ReadLine();
                    role.DeleteChat(chatToDelete);
                    break;
                case 5:
                    isWorking = false;
                    role.EndChat();
                    break;
                case 6:
                    Console.Clear();
                    break;
            }
        }
    }
}

