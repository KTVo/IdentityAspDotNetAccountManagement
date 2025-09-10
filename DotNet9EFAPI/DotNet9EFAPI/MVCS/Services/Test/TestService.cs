using System.Diagnostics;
using DotNet9EFAPI.MVCS.Models.DB;
using DotNet9EFAPI.MVCS.Services._DB;
using DotNet9EFAPI.Statics.Messages.App;
using Microsoft.EntityFrameworkCore;

namespace DotNet9EFAPI.MVCS.Services.Test;

public class TestService : ITestService
{
    private readonly TestDBContext _contextTest;

    public TestService(TestDBContext context)
    {
        _contextTest = context ?? throw new Exception(nameof(context));
    }

    public async Task<List<Item>?> GetItemsListAsync()
    {
        try
        {
            if (_contextTest.Items == null) { return null; }
            //INCLUDES SEARCHES FOR SerialNumber and Category WHEN SEARCHING FOR ITEMS
            List<Item>? items = await _contextTest.Items.ToListAsync();
            return items;
        }
        catch (Exception e)
        {
            Debug.WriteLine(AppMessages.QueryAllItemsFailed + " -> " + e.Message);
            return null;
        }
    }
    
}
