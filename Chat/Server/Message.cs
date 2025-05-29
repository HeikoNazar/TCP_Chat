using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Server.Server
{
    class Message
    {
        public int Id { get; set; }

        public readonly DateTime dateTimeNow;

        public string Content = string.Empty;

        private readonly string[] BannedWords = {"hitler", "niger", "дурень", "страховиско","666","stupid","fuck","чорт"};
        private const string hideBannedWord = "*****"; 

        public Message(int id, string content)
        {
            dateTimeNow = DateTime.UtcNow;
            Id = id;

            Content = HideBannedWords(content);
        }
        public string HideBannedWords(string content)
        {
            var split = content.Split();
            
            
            for(int i = 0; i < split.Length; i++)
            {
                string lowerContent = split[i].ToLower();
                if (BannedWords.Contains(lowerContent))
                {
                    split[i] = hideBannedWord;
                }
            }
            var result = string.Join(" ", split);
            return result;
        }

        public override string ToString()
        {
            return $"{Id} {dateTimeNow} {Content}";
        }

    }
}
