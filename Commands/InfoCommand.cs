using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace ConsoleBot.Commands
{
    class InfoCommand : Command
    {
        internal override string Name => "/info";

        internal override async void Execute(Message message, TelegramBotClient client)
        {
            string msg = $"" +
                $"• <b>Bot username:</b> {BotConfiguration.BotName}\n" +
                $"• <b>Bot developer:</b> {BotConfiguration.BotDeveloper}\n" +
                $"• <b>Bot code:</b> C#\n" +
                $"\nℹ️ <b>About me:</b> I was created for testing purposes. My commands demonstrate the basic functions that the Telegram API provides to the developer.";

            // Для изменения стиля текстового сообщения используется язык разметки HTML
            await client.SendTextMessageAsync(
                chatId: message.Chat,
                text: msg,
                parseMode: ParseMode.Html,
                replyToMessageId: message.MessageId,
                replyMarkup: new InlineKeyboardMarkup(InlineKeyboardButton.WithUrl(
                    "Bot code on GitHub",
                    "https://github.com/Ev1ls0n/Console-Bot"
                    ))
            );
        }
    }
}
