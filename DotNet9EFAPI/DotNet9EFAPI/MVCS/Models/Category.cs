using DotNet9EFAPI.MVCS.Models.DB;

namespace DotNet9EFAPI.MVCS.Models;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public List<Item>? Items { get; set; }
}
