using System;
using System.Collections.Generic;

using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

using ConsoleBot.Commands;
using ConsoleBot.Helpers;
using ConsoleBot.UI;

namespace ConsoleBot
{
    class Program
    {
        internal static TelegramBotClient Bot { get; private set; } // Объект клиента бота

        private static List<Command> CommandsList; // Список подключенных команд бота
        private static IReadOnlyList<Command> Commands { get => CommandsList.AsReadOnly(); } // Делает список команд доступным только для чтения

        static void Main(string[] args)
        {
            Bot = new TelegramBotClient(BotConfiguration.BotToken); // Получение токена бота

            Console.Title = ConsoleInformer.GetBotInfo(Bot).Username;

            Bot.OnMessage += BotOnMessageReceived;
            Bot.OnCallbackQuery += BotOnCallbackQueryReceived;

            Bot.StartReceiving(Array.Empty<UpdateType>()); // Чтение обновлений с сервера для бота

            UserInterface.MainMenu();

            Bot.StopReceiving(); // Прекращение чтение обновлений с сервера для бота (остановка бота)
        }

        // Метод отправки сообщения ботом в чат через консоль
        internal static async void SendConsoleMessage()
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
