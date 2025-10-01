using DotNet9EFAPI.MVCS.Models.DummyData;

namespace DotNet9EFAPI.MVCS.Services.REST;

public interface IRestService
{
    Task<ToDoResponse?> GetDataAsync(string url);
}
