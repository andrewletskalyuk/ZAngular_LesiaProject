using System;

namespace TelegramBotHelper
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                TelegramBotHelper hlp = new TelegramBotHelper(token: "1460941080:AAG1sGSFNbKt5j8b2GWoNJkwyx3RE71C66w");
                hlp.GetUpdates();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
