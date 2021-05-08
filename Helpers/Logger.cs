using System;
using System.IO;

namespace ConsoleBot.Helpers
{
    internal class Logger
    {
        private static string logFilePath = Directory.GetCurrentDirectory() + @"\log.txt";

        private static void LogInit()
        {
            if (!System.IO.File.Exists(logFilePath))
                System.IO.File.Create(logFilePath);
        }

        internal static async void AddData(string data)
        {
            using (StreamWriter sw = new StreamWriter(logFilePath, true, System.Text.Encoding.Default))
            {
                await sw.WriteLineAsync(data);
            }
        }

        internal static async void ReadLog()
        {
            LogInit();

            using (StreamReader sr = new StreamReader(logFilePath, System.Text.Encoding.Default))
            {
                string logLine;

                if ((logLine = await sr.ReadLineAsync()) == null)
                    Console.WriteLine("(i) Log file is empty.");

                while ((logLine = await sr.ReadLineAsync()) != null)
                {
                    Console.WriteLine(logLine);
                }
            }
        }

        internal static async void ClearLog()
        {
            using (StreamWriter sw = new StreamWriter(logFilePath, false, System.Text.Encoding.Default))
            {
                await sw.WriteLineAsync("");
            }
        }
    }
}
