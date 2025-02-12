using System.Text;
using System.Text.Json;
using FC.Codeflix.Catalog.EndToEndTests.Extensions.String;
using Microsoft.AspNetCore.WebUtilities;

namespace FC.Codeflix.Catalog.EndToEndTests.Base;

class SnakeCaseNamingPolicy : JsonNamingPolicy
{
    public override string ConvertName(string name) => name.ToSnakeCase();
}

public class ApiClient
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _defaultSerializerOptions;

    public ApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _defaultSerializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = new SnakeCaseNamingPolicy(),
            PropertyNameCaseInsensitive = true,
        };
    }

    public async Task<(HttpResponseMessage?, TOutput?)> Post<TOutput>(string route, object payload)
    where TOutput : class
    {
        var response = await _httpClient.PostAsync(
            route, 
            new StringContent(
                JsonSerializer.Serialize(payload, _defaultSerializerOptions), 
                Encoding.UTF8, 
                "application/json"
                )
            );
        
        var output = await GetOutput<TOutput>(response);
        return (response, output);
    }
    
    public async Task<(HttpResponseMessage?, TOutput?)> Put<TOutput>(string route, object payload)
        where TOutput : class
    {
        var response = await _httpClient.PutAsync(
            route, 
            new StringContent(
                JsonSerializer.Serialize(payload, _defaultSerializerOptions), 
                Encoding.UTF8, 
                "application/json"
            )
        );
 
        var output = await GetOutput<TOutput>(response);
        return (response, output);
    }

    public async Task<(HttpResponseMessage?, TOutput?)> Get<TOutput>(string route, object? queryStringParameters = null)
        where TOutput : class
    {
        var url = PrepareGetRoute(route, queryStringParameters);
        var response = await _httpClient.GetAsync(url);
        var output = await GetOutput<TOutput>(response);
        return (response, output);
    }

    private string PrepareGetRoute(string route, object? queryStringParameters)
    {
        if (queryStringParameters == null) 
            return route;

        var parametersJson = JsonSerializer.Serialize(queryStringParameters, _defaultSerializerOptions);
        var parametersDictionary = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(parametersJson);
        
        return QueryHelpers.AddQueryString(route, parametersDictionary!);
    }

    public async Task<(HttpResponseMessage?, TOutput?)> Delete<TOutput>(string route)
        where TOutput : class
    {
        var response = await _httpClient.DeleteAsync(route);
        var output = await GetOutput<TOutput>(response);
        return (response, output);
    }

    private async Task<TOutput?> GetOutput<TOutput>(HttpResponseMessage response) where TOutput : class
    {
        var outputString = await response.Content.ReadAsStringAsync();

        TOutput? output = null;
        if (!string.IsNullOrWhiteSpace(outputString))
            output = JsonSerializer.Deserialize<TOutput>(outputString, _defaultSerializerOptions);
        return output;
    }
}