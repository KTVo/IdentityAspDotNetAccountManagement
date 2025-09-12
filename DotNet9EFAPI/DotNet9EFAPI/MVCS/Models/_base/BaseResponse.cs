namespace DotNet9EFAPI.MVCS.Models._base;

public class BaseResponse
{
    public string? Message { get; set; }
    public bool? IsSuccessful { get; set; }
    public int? StatusCode { get; set; } = null;
    public List<string>? Errors { get; set; }
}
