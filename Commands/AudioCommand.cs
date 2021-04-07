using Telegram.Bot;
using Telegram.Bot.Types;

namespace ConsoleBot.Commands
{
    class AudioCommand : Command
    {
        internal override string Name => "/audio";

        internal override async void Execute(Message message, TelegramBotClient client)
        {
            await client.SendAudioAsync(
                chatId: message.Chat,
                audio: "https://drive.google.com/u/0/uc?id=1GOYWnEQwkqG_Byh3PPNDJn5j4m7JhDaf&export=download",
                performer: "Ludwig van Beethoven",
                title: "Piano Sonata in C Sharp Minor, Op. 27, \"Moonlight\""
            );
        }
    }
}
