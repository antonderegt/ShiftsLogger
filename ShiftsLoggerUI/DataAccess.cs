using System.Net.Http.Headers;
using System.Text.Json;
using ShiftsLoggerLibrary.Models;
using Microsoft.Extensions.Configuration;

namespace ShiftsLoggerUI;

public class DataAccess
{
    public string BasePath { get; set; }
    private readonly HttpClient _client;

    public DataAccess()
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        BasePath = GetConfigurationValue(configuration, "ShiftLoggerAPI", "BasePath");

        _client = new();
        _client.DefaultRequestHeaders.Accept.Clear();
        _client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
    }

    private static string GetConfigurationValue(IConfiguration configuration, string section, string key)
    {
        string? value = configuration.GetSection(section)[key];
        if (value == null)
        {
            return $"{key} not found";
        }
        return value;
    }

    public async Task<IEnumerable<Shift>?> GetShiftsAsync(int id)
    {
        try
        {
            var json = await _client.GetStringAsync($"{BasePath}/employee/{id}");
            return JsonSerializer.Deserialize<IEnumerable<Shift>>(json);
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"An error occurred while deserializing the JSON: {ex.Message}");
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"An error occurred while making the HTTP request: {ex.Message}");
        }

        return [];
    }
}