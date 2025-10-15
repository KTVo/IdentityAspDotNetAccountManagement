namespace DotNet9EFAPI.MVCS.Models.Email;

public class EmailSettings
{
    public string? HostSmtpServer { get; set; }
    public required int HostSmtpServerPort { get; set; } = 587;
    public string? HostEmail { get; set; }
    public string? HostEmailPassword { get; set; }
    public string? TestToEmail { get; set; }
    
}