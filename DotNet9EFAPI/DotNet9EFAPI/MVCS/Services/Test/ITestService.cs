
using DotNet9EFAPI.MVCS.Models.DB;

namespace DotNet9EFAPI.MVCS.Services.Test;

public interface ITestService
{
    Task<List<Item>?> GetItemsListAsync();
}
