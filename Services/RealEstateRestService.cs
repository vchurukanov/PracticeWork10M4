using PracticeWork10M4.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace PracticeWork10M4.Services;

public class RealEstateRestService
{
    private static readonly HttpClient httpClient = new(new HttpClientHandlerInsecure())
    {
        BaseAddress = new Uri("https://localhost:7204/")
    };

    private const string Endpoint = "real-estate";

    public async Task<List<RealEstateObject>> GetObjectsAsync()
    {
        return await httpClient.GetFromJsonAsync<List<RealEstateObject>>(Endpoint)
               ?? new List<RealEstateObject>();
    }

    public async Task<RealEstateObject?> GetObjectAsync(int id)
    {
        return await httpClient.GetFromJsonAsync<RealEstateObject>($"{Endpoint}/{id}");
    }

    public async Task<RealEstateObject?> CreateObjectAsync(RealEstateObject realEstateObject)
    {
        using HttpResponseMessage response =
            await httpClient.PostAsJsonAsync(Endpoint, realEstateObject);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<RealEstateObject>();
    }

    public async Task UpdateObjectAsync(RealEstateObject realEstateObject)
    {
        using HttpResponseMessage response =
            await httpClient.PutAsJsonAsync(
                $"{Endpoint}/{realEstateObject.Id}",
                realEstateObject);

        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteObjectAsync(int id)
    {
        using HttpResponseMessage response =
            await httpClient.DeleteAsync($"{Endpoint}/{id}");

        response.EnsureSuccessStatusCode();
    }
}