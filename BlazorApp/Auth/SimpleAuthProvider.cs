using System.Security.Claims;
using System.Text.Json;
using ApiContracts;
using ApiContracts.Auth;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorApp1.Auth;

public class SimpleAuthProvider : AuthenticationStateProvider
{
    private readonly HttpClient httpClient;
    private ClaimsPrincipal currentClaimsPrincipal;

    public SimpleAuthProvider(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task Login(string userName, string password)
    {
        HttpResponseMessage response =
            await httpClient.PostAsJsonAsync("auth/", new LoginRequest{Username = userName, Password = password});
        string content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }

        UserDto userDto = JsonSerializer.Deserialize<UserDto>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;

        List<Claim> claims = new List<Claim>()
            { new(ClaimTypes.Name, userDto.Username), new("Id", userDto.Id.ToString()) };

        ClaimsIdentity identity = new ClaimsIdentity(claims, "apiauth");

        currentClaimsPrincipal = new ClaimsPrincipal(identity);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(currentClaimsPrincipal)));
    }
    
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        return new AuthenticationState(currentClaimsPrincipal ?? new());
    }

    public void Logout()
    {
        currentClaimsPrincipal = new(); 
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(currentClaimsPrincipal)));
    }
}