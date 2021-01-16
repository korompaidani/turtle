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

        public static string WriteErrorMessagesToConsole()
        {
            if(messages.Count > 0)
            {
                var sb = new StringBuilder();
                foreach(var line in messages)
                {
                    sb.Append(line);
                    Console.WriteLine(line);
                }

                messages.Clear();
                return sb.ToString();
            }

            return String.Empty;
        }
    }
}
