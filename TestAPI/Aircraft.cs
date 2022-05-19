#nullable disable

using Xunit;
using System.Net.Http;
using System.Text.Json;
using System.Collections.Generic;
using System;
using System.Text;
using MySqlConnector;
using System.Threading.Tasks;
using System.Linq;
using System.Net.Http.Json;

namespace TestFlightSimulatorAPI;

public class Aircraft
{
    static readonly string _apiUrl = "https://localhost:7191/api/Aircraft";
    static readonly HttpClient _client = new HttpClient();
    static readonly Random _random = new Random();

    private static readonly HttpClient httpClient = new HttpClient
    {
        BaseAddress = new Uri(_apiUrl)
    };

    private static readonly JsonSerializerOptions _jsonSerializeOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    private void ExecuteRawSQL(string querry)
    {
        var connection = new MySqlConnection("Server=localhost;Database=FlightSimulator;Uid=root;Pwd=root");
        connection.Open();
        var command = new MySqlCommand(querry, connection);
        command.ExecuteNonQuery();
        connection.Close();
    }

    private static async Task<List<FlightSimulatorAPI.Models.Aircraft>> GetAll()
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, _apiUrl);

        using var response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var AircraftsList = await response.Content.ReadFromJsonAsync<List<FlightSimulatorAPI.Models.Aircraft>>(_jsonSerializeOptions);
        return AircraftsList;
    }

    [Fact]
    public async void Connect()
    {
        ExecuteRawSQL("TRUNCATE Aircrafts;");
        try
        {
            HttpResponseMessage response = await _client.GetAsync(_apiUrl);
            Assert.True(response.IsSuccessStatusCode);
        }
        catch
        {
            Assert.True(false);
        }
    }

    [Fact]
    public async void Get()
    {
        ExecuteRawSQL("TRUNCATE Aircrafts;");
        Assert.True((await GetAll()).Count == 0);
    }

    [Fact]
    public async Task<bool> Post()
    {
        FlightSimulatorAPI.Models.Aircraft MyAircraft = new FlightSimulatorAPI.Models.Aircraft();
        MyAircraft.Model = "Boeing 737";

        using var httpContent = new StringContent(JsonSerializer.Serialize(MyAircraft, _jsonSerializeOptions), Encoding.UTF8, "application/json");
        using var response = await httpClient.PostAsync(_apiUrl, httpContent);

        Assert.True(response.IsSuccessStatusCode);
        return response.IsSuccessStatusCode;
    }

    [Fact]
    public async void GetByID()
    {
        ExecuteRawSQL("TRUNCATE Aircrafts;");
        Random random = new Random();
        int number = random.Next(1, 5);

        for (int i = 0; i < number; i++)
        {
            await Post();
        }
        Assert.Equal((await GetAll()).Count, number);
    }

    [Fact]
    public async void DeleteByID()
    {
        ExecuteRawSQL("TRUNCATE Aircrafts;");
        int number = _random.Next(1, 5);

        for (int i = 0; i < number; i++)
        {
            await Post();
        }
        Assert.Equal((await GetAll()).Count, number);

        var AircraftResult = (await GetAll())[0];
        using var httpContent = new StringContent(JsonSerializer.Serialize(AircraftResult, _jsonSerializeOptions), Encoding.UTF8, "application/json");
        using var response = await httpClient.DeleteAsync(_apiUrl + "/" + AircraftResult.Id);

        Assert.True(response.IsSuccessStatusCode);
        Assert.True((await GetAll()).Where(aircraft => aircraft.Id == AircraftResult.Id).Count() == 0);
    }
}
