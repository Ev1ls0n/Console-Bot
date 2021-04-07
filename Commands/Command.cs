using Telegram.Bot;
using Telegram.Bot.Types;

namespace ConsoleBot.Commands
{
    internal abstract class Command
    {
        internal abstract string Name { get; } // Имя команды.
        internal abstract void Execute(Message message, TelegramBotClient client); // Метод с логикой команды.

        // Метод для проверки корректности команды и имени бота, который её должен выполнять (на случай, если в чате много ботов).
        internal bool Contains(string command)
        {
            if (!command.Contains(BotConfiguration.BotName))
                return command.Contains(this.Name);
            else
                return command.Contains(this.Name) && command.Contains(BotConfiguration.BotName);
        }
    }
}
