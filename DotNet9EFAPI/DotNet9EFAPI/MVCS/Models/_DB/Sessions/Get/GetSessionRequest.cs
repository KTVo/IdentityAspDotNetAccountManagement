using System.ComponentModel.DataAnnotations;

namespace DotNet9EFAPI.MVCS.Models._DB.Sessions.Get;

public class Session
{
    [Key] // Marks this as the primary key
    public int SessionId { get; set; }
    public string? UId { get; set; }
    public string? Email { get; set; }
}