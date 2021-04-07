using System.Net;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ConsoleBot.Commands
{
    class VoiceCommand : Command
    {
        internal override string Name => "/voice";

        internal override async void Execute(Message message, TelegramBotClient client)
        {
            // Для чтения файла с ресурсов проекта
            //using (var stream = System.IO.File.OpenRead(@"C:\Users\User\Desktop\Telegram Bots\ConsoleBot\ConsoleBot\Files\Death002B.ogg"))
            //{
            //    var msg = await client.SendVoiceAsync(
            //    chatId: message.Chat,
            //    voice: stream,
            //    duration: 11,
            //    caption: "Voice in <code>.ogg</code> file format on my PC",
            //    replyToMessageId: message.MessageId,
            //    parseMode: ParseMode.Html
            //    );
            //}

            var webClient = new WebClient(); // Предоставляет общие методы отправки и получения данных из ресурса, идентифицированного URI

            // URI (Uniform Resource Identifier — унифицированный идентификатор ресурса)

            // Чтобы в сообщении отправлялось именно голосовое сообщение, а не файл формата .ogg, необходимо прочитать данные файла в потоке
            using (var stream = webClient.OpenRead("https://drive.google.com/u/0/uc?id=1GDh6yr1o5unnd8KDN7cRgvwv9J4y_WoM&export=download"))
            {
                var msg = await client.SendVoiceAsync(
                chatId: message.Chat,
                voice: stream,
                duration: 11,
                caption: "🎶 A voice (.ogg file) with a mystery soundtrack",
                replyToMessageId: message.MessageId,
                parseMode: ParseMode.Html
                );
            }
        }
    }
}
