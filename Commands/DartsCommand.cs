using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

using ConsoleBot.Helpers;

namespace ConsoleBot.Commands
{
    class DartsCommand : Command
    {
        internal override string Name => "/darts";

        internal override async void Execute(Message message, TelegramBotClient client)
        {
            Message msg = await client.SendDiceAsync(
                chatId: message.Chat,
                emoji: Emoji.Darts,
                replyToMessageId: message.MessageId,
                replyMarkup: new InlineKeyboardMarkup(new[] { new[] { InlineKeyboardButton.WithCallbackData("Delete message", "delete_msg") } })
            );

            Logger.AddData($"(i) Darts result: {msg.Dice.Value}");
        }
    }
}
