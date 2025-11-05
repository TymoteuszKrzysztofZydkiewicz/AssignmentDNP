using System.Text.Json;
using System.Net.Http.Json; 
using ApiContracts; 
using System.Threading.Tasks;

public class HttpUserService : IUserService
{
    private readonly HttpClient client;
    
    public HttpUserService(HttpClient client)
    {
        this.client = client;
    }
    public async Task<ReturnUserDto> CreateAsync(CreateUserDto request)
    {
        HttpResponseMessage httpResponse = await client.PostAsJsonAsync("users", request);

        
        string response = await httpResponse.Content.ReadAsStringAsync();
        
        if (!httpResponse.IsSuccessStatusCode)
        {
            //
            throw new Exception(response);
        }

        
        var user = JsonSerializer.Deserialize<ReturnUserDto>(
            response,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        
        return user!;
    }
}