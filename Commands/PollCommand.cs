using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace ConsoleBot.Commands
{
    class PollCommand : Command
    {
        internal override string Name => "/poll";

        internal override async void Execute(Message message, TelegramBotClient client)
        {
            if (message.Chat.Title == null)
            {
                await client.SendTextMessageAsync(
                    chatId: message.Chat,
                    text: "This chat isn't group or channel.",
                    replyToMessageId: message.MessageId
                );
            }
            else
            {
                await client.SendPollAsync(
                    chatId: message.Chat,
                    question: "Yes or No?",
                    options: new []
                    {
                        "Yes",
                        "No"
                    },
                    replyMarkup: new InlineKeyboardMarkup(new[] { new[] { InlineKeyboardButton.WithCallbackData("Close poll", "close_poll") } })
                );
            }
        }
    }
}
