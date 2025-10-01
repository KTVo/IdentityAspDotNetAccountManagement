using DotNet9EFAPI.MVCS.Models._base;

namespace DotNet9EFAPI.MVCS.Models.DummyData;

public class ToDoResponse : BaseResponse
{
    public List<ToDo>? ToDos { get; set; }
}
