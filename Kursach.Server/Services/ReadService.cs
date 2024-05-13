using Kursach.Models;
using Kursach.Service.IService;
using Newtonsoft.Json;

namespace Kursach.Service;

public class ReadService: IReadService
{
  

    public async Task<IEnumerable<MbModel>> GetMbAsync()
    {
        string apiUrl = $"https://api.monobank.ua/bank/currency";

        try
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(responseBody);
                    MbModel[] currencyData = JsonConvert.DeserializeObject<MbModel[]>(responseBody);
                    return currencyData;
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                    return null;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return null;
        }
    }
}