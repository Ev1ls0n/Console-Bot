using System;
using System.Collections.Generic;

using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;


using ConsoleBot.Commands;
using ConsoleBot.Helpers;

namespace ConsoleBot
{
    class Program
    {
        private static TelegramBotClient Bot; // Объект клиента бота

        private static List<Command> CommandsList; // Список подключенных команд бота
        private static IReadOnlyList<Command> Commands { get => CommandsList.AsReadOnly(); } // Делает список команд доступным только для чтения

        static void Main(string[] args)
        {
            Bot = new TelegramBotClient(BotConfiguration.BotToken); // Получение токена бота

            Console.Title = ConsoleInformer.GetBotInfo(Bot).Username;

            Bot.OnMessage += BotOnMessageReceived;
            Bot.OnCallbackQuery += BotOnCallbackQueryReceived;

            Bot.StartReceiving(Array.Empty<UpdateType>()); // Чтение обновлений с сервера для бота

            Menu();

            Bot.StopReceiving(); // Прекращение чтение обновлений с сервера для бота (остановка бота)
        }

        // Метод главного меню программы
        private static void Menu()
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
                }

                Console.Clear();

                switch (menuItem)
                {
                    case 1:
                        SendConsoleMessage();
                        Console.ReadKey();
                        break;
                    case 2:
                        ConsoleInformer.PrintBotInfo(Bot);
                        Console.ReadKey();
                        break;
                    case 3:
                        Logger.ReadLog();
                        Console.ReadKey();
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

        // Метод отправки сообщения ботом в чат через консоль
        private static async void SendConsoleMessage()
        {
            long currentId; // Может встретиться ID, значение которого не поместится в тип данных int
            string msg;

            ChatId id;

            Console.WriteLine("(i) Send message via console.");
            Console.Write("Enter chat id: ");

            while (true)
            {
                bool isCorrect = long.TryParse(Console.ReadLine(), out currentId);

                if (isCorrect)
                    break;

                ConsoleInformer.ErrorMessage("Incorrect input. Please try again.");
            }

            id = new ChatId(currentId);

            Console.Write("Enter the message: ");
            msg = Console.ReadLine();

            try
            {
                await Bot.SendTextMessageAsync(
                    chatId: id,
                    text: msg
                );
            }
            catch (Telegram.Bot.Exceptions.ChatNotInitiatedException)
            {
                ConsoleInformer.ErrorMessage("This user has not yet initiated a chat with the bot.");
            }
            catch (Telegram.Bot.Exceptions.ApiRequestException)
            {
                ConsoleInformer.ErrorMessage("Bot was blocked by the user.");
            }
        }

        // Метод для обработки входящих от пользователя команд (чтение сообщений от пользователя)
        private static async void BotOnMessageReceived(object sender, MessageEventArgs e)
        {
            bool notFound = true;

            // Инициализация списка команд и самих команд
            CommandsList = new List<Command>();

            CommandsList.Add(new HelpCommand());
            CommandsList.Add(new InfoCommand());
            CommandsList.Add(new KeyboardCommand());
            CommandsList.Add(new PhotoCommand());
            CommandsList.Add(new AudioCommand());
            CommandsList.Add(new VoiceCommand());
            CommandsList.Add(new VideoCommand());
            CommandsList.Add(new PollCommand());
            CommandsList.Add(new DiceCommand());
            CommandsList.Add(new DartsCommand());

            // ---

            Logger.AddData(ConsoleInformer.MessageInfo(e.Message)); // Дабавление информации о сообщении пользователя в специальный файл (log.txt)

            // Поиск и вызов введенной команды
            foreach (var command in Commands)
            {
                if (command.Contains(e.Message.Text))
                {
                    command.Execute(e.Message, Bot);
                    notFound = false;
                }
            }

            // Если введённая команда не найдена выводится соответствующие сообщение
            if (notFound)
            {
                await Bot.SendTextMessageAsync(
                    chatId: e.Message.Chat,
                    text: "Use <code>/help</code> command to get commands list.",
                    parseMode: ParseMode.Html,
                    replyToMessageId: e.Message.MessageId
                );
            }
        }

        // Метод для обработки данных обратного вызова. Например событий кнопок (keyboard)
        private static async void BotOnCallbackQueryReceived(object sender, CallbackQueryEventArgs callbackQueryEventArgs)
        {
            var callbackQuery = callbackQueryEventArgs.CallbackQuery;

            /*
            // Вывод специального всплывающего окна с сообщением, которое будет содержать данные, отправляемые обратным вызовом
            await Bot.AnswerCallbackQueryAsync(
                callbackQueryId: callbackQuery.Id,
                text: $"ℹ️ Received: {callbackQuery.Data}"
            );
            
            // Вывод в чат сообщения с данными, которые отправляет обратный вызов
            await Bot.SendTextMessageAsync(
                chatId: callbackQuery.Message.Chat.Id,
                text: $"ℹ️ Received: {callbackQuery.Data}"
            );

            // Удаление из чата сообщения
            await Bot.DeleteMessageAsync(
                chatId: callbackQuery.Message.Chat.Id,
                messageId: callbackQuery.Message.MessageId
            );
            */

            // Закрытие опроса, который отправил бот
            if (callbackQuery.Data == "close_poll")
            {
                await Bot.StopPollAsync(
                    chatId: callbackQuery.Message.Chat,
                    messageId: callbackQuery.Message.MessageId
                );
            }

            // Удаление сообщения, которое отправил бот
            if (callbackQuery.Data == "delete_msg")
            {
                // Если сообщение находится в личной переписке, то бот не может его удалить
                // У личной переписки свойство Title равно null
                if (callbackQuery.Message.Chat.Title == null)
                {
                    await Bot.AnswerCallbackQueryAsync(
                        callbackQueryId: callbackQuery.Id,
                        text: "⚠️ Bot can't delete messages in private chat."
                    );
                }
                else
                {
                    await Bot.DeleteMessageAsync(
                        chatId: callbackQuery.Message.Chat,
                        messageId: callbackQuery.Message.MessageId
                    );
                }
            }

            Logger.AddData($"(i) End callback query received. Callback data: {callbackQuery.Data}");

        }
    }
}
