using System;
using System.Windows.Forms;

namespace ToDoListApp
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Singleton Logger - uygulama baslangicini log.txt dosyasina yazar
            Logger logger = Logger.GetInstance();
            logger.Log("Application started.");

            Application.Run(new MainForm());
        }
    }
}
