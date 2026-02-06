using MySqlConnector;
using SocialChatTool.Models;

namespace SocialChatTool.Services;

/// <summary>
/// Service for company management operations
/// </summary>
public class CompanyService
{
    private readonly string _managementConnectionString;

    public CompanyService(string managementConnectionString)
    {
        _managementConnectionString = managementConnectionString;
    }

    /// <summary>
    /// Get all companies from management database
    /// </summary>
    public async Task<List<Company>> GetCompaniesAsync()
    {
        var companies = new List<Company>();

        await using var connection = new MySqlConnection(_managementConnectionString);
        await connection.OpenAsync();

        var query = "SELECT CompanyCode, CompanyName, DataSource, InitialCatalog, UserId, Password FROM company";
        await using var command = new MySqlCommand(query, connection);
        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            companies.Add(new Company
            {
                CompanyCode = reader.GetString("CompanyCode"),
                CompanyName = reader.GetString("CompanyName"),
                DataSource = reader.GetString("DataSource"),
                InitialCatalog = reader.GetString("InitialCatalog"),
                UserId = reader.GetString("UserId"),
                Password = reader.GetString("Password")
            });
        }

        return companies;
    }
}
