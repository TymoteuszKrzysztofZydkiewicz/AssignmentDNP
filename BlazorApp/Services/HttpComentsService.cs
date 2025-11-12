using System.Text.Json;
using System.Net.Http.Json; 
using ApiContracts; 
using System.Threading.Tasks;
using ApiContracts.Comment;
using ApiContracts.Post;
namespace BlazorApp.Services;

public class HttpComentsService:ICommentsService
{
    private readonly HttpClient client;

    public HttpComentsService(HttpClient client)
    {
        this.client = client;
    }
    
    public async Task<List<CommentDto>> GetCommentsAsync(int postId)
    { 
        HttpResponseMessage httpResponse = await client.GetAsync($"Comment/forPost/{postId}"); 
        string content = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }
        var dto = JsonSerializer.Deserialize<GetCommentsDto>(
            content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
        );
        return dto?.Comments ?? new List<CommentDto>();    
    
    }

    public async Task<CommentDto> AddCommentAsync(CreateCommentDto request)
    { 
        HttpResponseMessage httpResponse = await client.PostAsJsonAsync("Comment", request); 
       string content = await httpResponse.Content.ReadAsStringAsync();
       if (!httpResponse.IsSuccessStatusCode)
       {
           throw new Exception(content);
       }
       return JsonSerializer.Deserialize<CommentDto>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
    }
}