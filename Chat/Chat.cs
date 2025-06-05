using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat
{
    class Chat
    {
        public static int Id;
        public List<string> Messages = new List<string>();

        //Ім'я файлу
        public string Name;

        public Chat(string name)
        {
            Name = name;
            Id++;
        }
        public void AddMessage(string content)
        {
            Messages.Add(content);
        }
    }
}
