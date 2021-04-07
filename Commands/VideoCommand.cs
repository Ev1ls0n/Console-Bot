using Telegram.Bot;
using Telegram.Bot.Types;

namespace ConsoleBot.Commands
{
    class VideoCommand : Command
    {
        internal override string Name => "/video";

        internal override async void Execute(Message message, TelegramBotClient client)
        {
            await client.SendVideoAsync(
                chatId: message.Chat,
                video: "https://drive.google.com/u/0/uc?id=17W1OjeWwI3jMdz1QFp5fLu63-iM08qyE&export=download",
                replyToMessageId: message.MessageId
            );
        }
    }
}
