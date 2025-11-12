using ApiContracts.Comment;

namespace BlazorApp.Services;

public interface ICommentsService
{
    public Task<List<CommentDto>> GetCommentsAsync(int postId);
    public Task<CommentDto> AddCommentAsync(CreateCommentDto request);
}