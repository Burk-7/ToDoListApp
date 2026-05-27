using System;
using System.IO;

namespace ToDoListApp
{
    public class Logger
    {
        private static Logger instance = null;

        private string logFile = "log.txt";

        private Logger()
        {
        }

        public static Logger GetInstance()
        {
            if (instance == null)
            {
                instance = new Logger();
            }
            return instance;
        }

        public void Log(string message)
        {
            string time = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
            string entry = "[" + time + "] " + message;

            Console.WriteLine(entry);

            try
            {
                StreamWriter writer = new StreamWriter(logFile, true);
                writer.WriteLine(entry);
                writer.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Log yazilamadi: " + ex.Message);
            }
        }
    }
}
