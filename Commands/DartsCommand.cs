using System;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

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

            Console.WriteLine($"Darts result: {msg.Dice.Value}");
        }
    }
}
