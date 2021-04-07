using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ConsoleBot.Commands
{
    class HelpCommand : Command
    {
        internal override string Name => "/help";

        internal override async void Execute(Message message, TelegramBotClient client)
        {
            string msg = "" +
                "<code>/help</code> - Get a list of available commands 📄\n" +
                "<code>/info</code> - Program information ℹ️\n" +
                "<code>/keyboard</code> - Demonstration of keyboard in a message ⌨️\n" +
                "<code>/photo</code> - Demonstration of sending a photo 🖼\n" +
                "<code>/audio</code> - Demonstration of sending an audio 🎵\n" +
                "<code>/voice</code> - Demonstration of sending a voice 🎤\n" +
                "<code>/video</code> - Demonstration of sending a video 🎞\n" +
                "<code>/poll</code> - Demonstration of sending a poll 📊\n" +
                "<code>/dice</code> - Roll a dice! 🎲\n" +
                "<code>/darts</code> - Play darts 🎯";

            await client.SendTextMessageAsync(
                chatId: message.Chat,
                text: msg,
                parseMode: ParseMode.Html,
                replyToMessageId: message.MessageId
            );
        }
    }
}
