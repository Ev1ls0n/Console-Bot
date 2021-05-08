using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

using ConsoleBot.Helpers;

namespace ConsoleBot.Commands
{
    class DiceCommand : Command
    {
        internal override string Name => "/dice";

        internal override async void Execute(Message message, TelegramBotClient client)
        {
            Message msg = await client.SendDiceAsync(
                chatId: message.Chat,
                emoji: Emoji.Dice,
                replyToMessageId: message.MessageId,
                replyMarkup: new InlineKeyboardMarkup(new[] { new[] { InlineKeyboardButton.WithCallbackData("Delete message", "delete_msg") } })
            );

            Logger.AddData($"(i) Dice result: {msg.Dice.Value}");
        }
    }
}
