using System;
using System.Collections.Generic;
using System.Text;

namespace turtle
{
    public static class ErrorLog
    {
        private static IList<string> messages;

        static ErrorLog()
        {
            messages = new List<string>();
        }

        public static void AddErrorMessage(string message)
        {
            messages.Add(message);
        }

        public static void WriteErrorMessagesToConsole()
        {
            if(messages.Count > 0)
            {
                foreach(var line in messages)
                {
                    Console.WriteLine(line);
                }

                messages.Clear();
            }
        }
    }
}
