using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat
{
    
   interface IСhat
    {

        
        void StartChat();

        Chat GetChat(string chatName);

        void DeleteChat(string chatName);

        void EndChat();

        string[] GetAllChats();
    }
}
