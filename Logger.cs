using System;
using System.IO;

namespace ToDoListApp
{
    // SINGLETON PATTERN 
    // Log mesajlari hem ekrana yazilir hem de log.txt dosyasina kaydedilir.
    public class Logger
    {
        // Tek nesneyi tutan static degisken
        private static Logger instance = null;

        private string logFile = "log.txt";

        // Private constructor - disaridan new ile olusturulamaz
        private Logger()
        {
        }

        // Singleton erisim noktasi
        public static Logger GetInstance()
        {
            if (instance == null)
            {
                instance = new Logger();
            }
            return instance;
        }

        // Mesaji ekrana yazar ve log.txt dosyasina ekler (StreamWriter)
        public void Log(string message)
        {
            string time = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
            string entry = "[" + time + "] " + message;

            Console.WriteLine(entry);

            // Try/catch ile hata yonetimi (Exceptions)
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
