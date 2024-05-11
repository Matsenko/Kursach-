using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

public class Program
{
   
    private static ITelegramBotClient _botClient;

    private static ReceiverOptions _receiverOptions;
    
    static async Task Main()
    {
        
        _botClient = new TelegramBotClient("6531195607:AAE-DakooiBbKbjYj3bvKgG-NeHxGcMqoPo"); 
        _receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = new[] 
            {
                UpdateType.Message, 
            },

            ThrowPendingUpdates = true, 
        };
        
        using var cts = new CancellationTokenSource();
        
     
        _botClient.StartReceiving(UpdateHandler, ErrorHandler, _receiverOptions, cts.Token); 
        
        var me = await _botClient.GetMeAsync(); 
        Console.WriteLine($"{me.FirstName} запущен!");
        
        await Task.Delay(-1); 
    }
    private static async Task UpdateHandler(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {

        try
        {

            switch (update.Type)
            {
                case UpdateType.Message:
                {
                    Console.WriteLine("Пришло сообщение!");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }

    private static Task ErrorHandler(ITelegramBotClient botClient, Exception error, CancellationToken cancellationToken)
    {

        var ErrorMessage = error switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => error.ToString()
        };

        Console.WriteLine(ErrorMessage);
        return Task.CompletedTask;
    }
}
