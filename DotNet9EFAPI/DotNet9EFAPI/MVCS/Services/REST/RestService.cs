using System.Text.Json;
using DotNet9EFAPI.MVCS.Models.DummyData;
using DotNet9EFAPI.Statics.Messages.App;

namespace DotNet9EFAPI.MVCS.Services.REST;

public class RestService : IRestService
{
    
    private readonly HttpClient _httpClient;

    public RestService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    /// <summary>
    /// GETS DATA FROM ENDPOINT
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    public async Task<ToDoResponse?> GetDataAsync(string url)
    {
        try
        {
            HttpResponseMessage response = await _httpClient.GetAsync(url);

            response.EnsureSuccessStatusCode();

            Stream stream = await response.Content.ReadAsStreamAsync();

            JsonSerializerOptions options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            List<ToDo>? toDos = await JsonSerializer.DeserializeAsync<List<ToDo>?>(stream, options);

            if (toDos == null) { return null; }

            return new ToDoResponse { IsSuccessful = true, ToDos = toDos, Message = AppMessages.QueryAllItemsSuccess};
        }
        catch (Exception ex)
        {
            return new ToDoResponse { IsSuccessful = false, ToDos = null, Message = ex.Message };
        }
        

    }


}


