using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System.Linq.Expressions;
namespace Chat;
class Server
{
    static string defaultDir = @"C:\Users\nazar\IT\programming\studyc#\unik\Chat\Chat\Chats\";
    static void Main() 
    {
        
        Console.OutputEncoding = Encoding.UTF8;

        var test = new Message(1, "я не hitler і не 666 , я людина");
        TcpListener listener = new TcpListener(IPAddress.Any, 8888);
        listener.Start();
        Console.WriteLine("Очікуємо на клієнта");
        TcpClient client = listener.AcceptTcpClient();
        Console.WriteLine("Клієнт підєднався");
        NetworkStream stream = client.GetStream();
        StreamReader reader = new StreamReader(stream);
        StreamWriter writer = new StreamWriter(stream);
        writer.AutoFlush = true;

        bool isWorking = true;
        string[] choices = { "1.Start chat", "2. End chat", "3. Show chat", "4. Delete chat","5. Stop working" };
        while (isWorking)
        {
            for (int i = 0;  i < choices.Length; i++)
            {
                Console.WriteLine(choices[i]);
            }
            int choice;
            try
            {
                choice = int.Parse(Console.ReadLine());
            }
            catch
            {
                Console.WriteLine("Wrong format");
                continue;
            }
            switch (choice)
            {
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 5:
                    isWorking = false;
                    break;

            }

        }
    }
}

