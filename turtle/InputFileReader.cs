using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace turtle
{
    static class InputFileReader
    {
        private const string textFile = @".\..\..\..\Input\input.txt";

        public static void ReadFile()
        {
            if (File.Exists(textFile))
            {
                using (StreamReader file = new StreamReader(textFile))
                {
                    string ln;

                    while ((ln = file.ReadLine()) != null)
                    {
                        Console.WriteLine(ln);
                    }
                }
            }
        }
    }
}
