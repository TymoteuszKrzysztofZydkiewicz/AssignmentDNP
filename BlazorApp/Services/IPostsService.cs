using ApiContracts.Post;

namespace BlazorApp.Services;

public interface IPostsService
{
    public Task<PostDto> AddPostAsync(CreatePostDto request);
    public Task<List<PostDto>> GetPostsAsync();
    public Task<PostDto> GetPostAsync(int id);
}