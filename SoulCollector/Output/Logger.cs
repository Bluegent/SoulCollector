
namespace SoulCollector.Output
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    public interface ILogger
    {
        void Log(string message);
        void Flush();
    }

    public class ConsoleLogger : ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine(message);
        }

        public void Flush()
        {
        }
    }

    public class DelayedLogger : ILogger
    {
        private List<string> _messages;

        public void Clear()
        {
            _messages.Clear();;
        }
        DelayedLogger()
        {
            _messages = new List<string>();
        }

        public void Log(string message)
        {
            _messages.Add(message);
        }

        public void Flush()
        {

            foreach (string str in _messages)
            {
                Console.Write(str);
            }
            Clear();
            
        }
    }


}
