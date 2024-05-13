using Kursach.Shared.DTOs;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

public class Program
{
   
    private static ITelegramBotClient _botClient;

    private static ReceiverOptions _receiverOptions;
    private static readonly HttpClient _httpClient = new HttpClient();
    
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
                    var message = update.Message;
                    var chatId = message.Chat.Id;

                    if (message.Text == "/start")
                    {
                        await RegisterUser(botClient, update, cancellationToken);
                        await botClient.SendTextMessageAsync(
                            chatId,
                            "Choose a currency:",
                            replyMarkup: GetCurrencyKeyboardMarkup());

                    }
                    else if(message.Text == "/stop")
                    {
                        await DeleteUser(botClient, update, cancellationToken);
                        await botClient.SendTextMessageAsync(
                            chatId,
                            "You have been successfully deleted!");

                    }
                    else if (message.Text == "USD")
                    {
                        List<CurrencyDTO> currencies =    await SendRequestToServer("https://localhost:7200/api/Request/840");

                        if (currencies != null)
                        {
                            foreach (var currency in currencies)
                            {
                                await botClient.SendTextMessageAsync(
                                             chatId,
                                             $"Rate Sell:{currency.RateSell}.Rate Buy:{currency.RateBuy}");
                            }
                        }
         
                    }
                    else if (message.Text == "EUR")
                    {
                        List<CurrencyDTO> currencies = await SendRequestToServer("https://localhost:7200/api/Request/840");

                        if (currencies != null)
                        {
                            foreach (var currency in currencies)
                            {
                                await botClient.SendTextMessageAsync(
                                             chatId,
                                             $"Rate Sell:{currency.RateSell}.Rate Buy:{currency.RateBuy}");
                            }
                        }
                    }
                    break;
                default:
                    Console.WriteLine($"Unsupported update type: {update.Type}");
                    break;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }

    private static ReplyKeyboardMarkup GetCurrencyKeyboardMarkup()
    {
        var buttons = new[]
        {
        new KeyboardButton("USD"),
        new KeyboardButton("EUR"),
    };

        return new ReplyKeyboardMarkup(buttons);
    }
    private static async Task<List<CurrencyDTO>> SendRequestToServer(string url)
    {
        List<CurrencyDTO> currencyList = null;
        HttpResponseMessage response = await _httpClient.GetAsync($"{url}");

        // Handle the response
        if (response.IsSuccessStatusCode)
        {
            string json = await response.Content.ReadAsStringAsync();
            currencyList = JsonConvert.DeserializeObject<List<CurrencyDTO>>(json);
            Console.WriteLine($"Request to the server  was successful.");
        }
        else
        {
            Console.WriteLine("Error occurred while sending request to the server");

        }
        return currencyList;

    }
    private static async Task RegisterUser(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        var message = update.Message;
        var chatId = message.Chat.Id;

        var content = new StringContent(JsonConvert.SerializeObject(new UserDTO { UserId = chatId.ToString() }), Encoding.UTF8, "application/json");
        HttpResponseMessage response = await _httpClient.PostAsync($"https://localhost:7200/api/Request/{chatId.ToString()}", content);

        if (response.IsSuccessStatusCode)
        {
            await botClient.SendTextMessageAsync(
                chatId,
                "You have been successfully registered!");
        }
        else
        {
            await botClient.SendTextMessageAsync(
                chatId,
                "Error occurred while registering user");
        }
    }
    private static async Task DeleteUser(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        var message = update.Message;
        var chatId = message.Chat.Id;

        HttpResponseMessage response = await _httpClient.DeleteAsync($"https://localhost:7200/api/Request/{chatId.ToString()}");

        if (response.IsSuccessStatusCode)
        {
            await botClient.SendTextMessageAsync(
                chatId,
                "You have been successfully deleted!");
        }
        else
        {
            await botClient.SendTextMessageAsync(
                chatId,
                "Error occurred while deleting user");
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
