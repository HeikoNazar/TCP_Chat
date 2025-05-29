using System;
using System.Net.Sockets;
using System.IO;

class Client
{
    static void Main()
    {
        
        Console.Write("Введіть IP сервера: ");
        string serverIp = Console.ReadLine();

        TcpClient client = new TcpClient(serverIp, 8888);
        Console.WriteLine("Підключено до сервера.");

        NetworkStream stream = client.GetStream();
        StreamReader reader = new StreamReader(stream);
        StreamWriter writer = new StreamWriter(stream);
        writer.AutoFlush = true;

        while (true)
        {
            string messageFromServer = reader.ReadLine();
            Console.WriteLine("Сервер: " + messageFromServer);

            Console.Write("Ви: ");
            string messageToSend = Console.ReadLine();
            writer.WriteLine(messageToSend);
        }
    }
}
