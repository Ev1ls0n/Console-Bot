using System;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ConsoleBot.Helpers
{
    internal class ConsoleInformer
    {
        internal static string MessageInfo(Message message)
        {
            string sender = message.From.ToString();
            string date = message.Date.ToString();
            string chat = message.Chat.Title;
            string msg = message.Text;

            if (chat == null)
                chat = "private chat";

            //Console.WriteLine($"\n>> Sender {sender} ({date}) in chat - {chat} ({message.Chat.Id}):\n{msg}\n");
            return $"> Sender {sender} ({date}) in chat - {chat} ({message.Chat.Id}):\n{msg}\n";
        }

        internal static string GetUser(Message message, bool fullInfo = true)
        {
            if (fullInfo)
                return message.From.ToString();
            else
                return message.From.Username;
        }

        internal static void PrintBotInfo(TelegramBotClient bot)
        {
            User me = GetBotInfo(bot);

            Console.WriteLine("(i) Information about bot.");
            Console.WriteLine($"- Username: {me.Username}\n- ID: {me.Id}\n- Can join to groups: {me.CanJoinGroups}\n- Can read all group messages: {me.CanReadAllGroupMessages}\n");
        }

        internal static User GetBotInfo(TelegramBotClient bot)
        {
            return bot.GetMeAsync().Result;
        }

        internal static void ErrorMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"(!) {message}");
            Console.ResetColor();
        }
    }
}
