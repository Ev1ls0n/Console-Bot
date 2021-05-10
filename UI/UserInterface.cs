using System;
using ConsoleBot.Helpers;

namespace ConsoleBot.UI
{
    class UserInterface
    {
        // Метод главного меню программы
        internal static void MainMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("(i) Input the menu item number.\n1. Send message via console\n2. Bot info\n3. Log\n4. Exit");
                Console.Write("Input: ");

                int menuItem;

                while (true)
                {
                    bool isCorrect = Int32.TryParse(Console.ReadLine(), out menuItem);

                    if (isCorrect)
                        break;

                    ConsoleInformer.ErrorMessage("Incorrect input. Please try again.");
                    Console.Write("Input: ");
                }

                Console.Clear();

                switch (menuItem)
                {
                    case 1:
                        Program.SendConsoleMessage();
                        Console.ReadKey();
                        break;
                    case 2:
                        ConsoleInformer.PrintBotInfo(Program.Bot);
                        Console.ReadKey();
                        break;
                    case 3:
                        LogMenu();
                        break;
                    case 4:
                        return; // Выход из программы
                    default:
                        ConsoleInformer.ErrorMessage("Non-existent menu item.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        // Метод меню для взаимодействия с журналом
        internal static void LogMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("(i) Input the menu item number.\n1. Get log\n2. Clear log\n3. Back");
                Console.Write("Input: ");

                int menuItem;

                while (true)
                {
                    bool isCorrect = Int32.TryParse(Console.ReadLine(), out menuItem);

                    if (isCorrect)
                        break;

                    ConsoleInformer.ErrorMessage("Incorrect input. Please try again.");
                    Console.Write("Input: ");
                }

                Console.Clear();

                switch (menuItem)
                {
                    case 1:
                        Logger.ReadLog();
                        Console.WriteLine("(i) Press 'Enter' to go back...\n");
                        Console.ReadKey();
                        break;
                    case 2:
                        Logger.ClearLog();
                        Console.WriteLine("(i) Log cleared.");
                        Console.WriteLine("(i) Press 'Enter' to go back...\n");
                        Console.ReadKey();
                        break;
                    case 3:
                        return;
                    default:
                        ConsoleInformer.ErrorMessage("Non-existent menu item.");
                        Console.ReadKey();
                        break;
                }
            }
        }
    }
}
