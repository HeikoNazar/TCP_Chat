using System;
using System.Net.Sockets;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace Chat
{
    class Client : WorkWithFiles,IСhat
    {
        public string Ip;
        public Client(string ip)
        {
            Ip = ip;
        }

        public Client(string ip, int port) 
        {
            Ip = ip;
            Port = port;
        }
        public void StartChat()
        {
            int port = Port == 0 ? defaultPort : Port;
            try
            {
                _client = new TcpClient(Ip, port);
            }
            catch (SocketException )
            {
                Console.WriteLine("Probelms with connection");
                return;
            }
            Console.WriteLine("Connected to server");
            _stream = _client.GetStream();

            StreamReader reader = new StreamReader(_stream,System.Text.Encoding.UTF8);
            StreamWriter writer = new StreamWriter(_stream,System.Text.Encoding.UTF8);
            var date = DateTime.Now.ToString("dd-MM-yyyy");
            if (!Directory.Exists(chatsFolder))
            {
                Directory.CreateDirectory(chatsFolder);
            }
            var filePath = Path.Combine(chatsFolder, date + ".txt");
            writer.AutoFlush = true;

            using (StreamWriter fileWriter = new StreamWriter(filePath, true))
            {
                fileWriter.AutoFlush = true;
                while (true)
                {
                    var key = Console.ReadKey().Key;
                    string messageFromServer;
                    try
                    {
                        messageFromServer = reader.ReadLine();
                    }
                    catch (IOException)
                    {
                        Console.WriteLine("Server disconected");
                        messageFromServer = String.Empty;
                    }
                    Console.WriteLine("Server: " + messageFromServer);
                    Message messageFromServerToFile = new Message(messageFromServer, "Server");
                    fileWriter.WriteLine(messageFromServer);

                    if (key == ConsoleKey.Escape)
                    {
                        EndChat();
                        return;
                    }
                    else if (key == ConsoleKey.Enter)
                    {
                        Console.Write("Client: ");
                        string messageToSend = Console.ReadLine();
                        writer.WriteLine(messageToSend);
                        Message messageFromClientToFile = new Message(messageToSend, "Client");
                        fileWriter.WriteLine(messageFromClientToFile);
                    }
                }
            }
        }

     
        public void EndChat()
        {
            if (isConnected)
            {
                _stream.Close();
                _client.Dispose();
            }
            else
            {
                Console.WriteLine("No stream is running");
            }
        }
    }
}
