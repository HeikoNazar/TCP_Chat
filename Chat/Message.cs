using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat
{
    class Message
    {

        public readonly DateTime dateTimeNow;

        public string Content = string.Empty;

        public string Sender;

        private readonly string[] BannedWords = {"hitler", "дурень", "666","stupid","fuck","чорт"};
        private const string hideBannedWord = "*****";

        public Message(string? content)
        {
            Content = content;
        }

        public Message(string? content, string sender)
        {
            dateTimeNow = DateTime.Now;

            Content = HideBannedWords(content);
            Sender = sender;
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
            return $"{dateTimeNow} {Sender}: {Content}";
        }

    }
}
