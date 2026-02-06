namespace SocialChatTool.Models;

/// <summary>
/// Represents a company from management database
/// </summary>
public class Company
{
    public string CompanyCode { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public string DataSource { get; set; } = string.Empty;
    public string InitialCatalog { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public string DisplayText => $"{CompanyCode} - {CompanyName}";

    /// <summary>
    /// Build MySQL connection string from company info
    /// </summary>
    public string BuildConnectionString()
    {
        return $"Server={DataSource};Port=3306;Database={InitialCatalog};User={UserId};Password={Password};";
    }
}
