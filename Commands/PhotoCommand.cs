using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ConsoleBot.Commands
{
    class PhotoCommand : Command
    {
        internal override string Name => "/photo";

        internal override async void Execute(Message message, TelegramBotClient client)
        {
            await client.SendPhotoAsync(
                chatId: message.Chat,
                photo: "https://upload.wikimedia.org/wikipedia/en/0/0d/Microsoft_.NET_Framework_v4.5_logo.png",
                caption: "<b>Microsoft .NET Framework v4.5 logo</b>.\n<i>Source</i>: <a href=\"https://en.wikipedia.org/wiki/.NET_Framework\" > Wikipedia</a>",
                parseMode: ParseMode.Html,
                replyToMessageId: message.MessageId
            );
        }
    }
}
