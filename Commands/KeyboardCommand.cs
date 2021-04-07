using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types.Enums;

namespace ConsoleBot.Commands
{
    class KeyboardCommand : Command
    {
        internal override string Name => "/keyboard";

        internal override async void Execute(Message message, TelegramBotClient client)
        {
            var inlineKeyboard = new InlineKeyboardMarkup(new[]
            { 
                // first row
                new []
                {
                    InlineKeyboardButton.WithCallbackData("Btn1", "btn_1"),
                    InlineKeyboardButton.WithCallbackData("Btn2", "btn_2")
                },
                // second row
                new []
                {
                    InlineKeyboardButton.WithCallbackData("Btn3", "btn_3"),
                    InlineKeyboardButton.WithCallbackData("Btn4", "btn_4")
                }
            });

            // Для изменения стиля текстового сообщения используется язык разметки Markdown
            await client.SendTextMessageAsync(
                chatId: message.Chat,
                text: "🙃 *Click any button*",
                replyMarkup: inlineKeyboard,
                parseMode: ParseMode.MarkdownV2
            );
        }
    }
}
