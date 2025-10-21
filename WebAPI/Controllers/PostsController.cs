using ApiContracts;
using ApiContracts.Post;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers; // make sure this matches your Web API project namespace

[ApiController]
[Route("[controller]")]
public class PostsController : ControllerBase
{
    private readonly IPostRepository _posts;
    private readonly IUserRepository _users;

    public PostsController(IPostRepository posts, IUserRepository users)
    {
        _posts = posts;
        _users = users;
    }
    
    [HttpPost]
    public async Task<ActionResult<PostDto>> CreateAsync([FromBody] CreatePostDto request)
    {
        
        var author = await _users.GetSingleAsync(request.AuthorId);
        if (author is null) return BadRequest($"User {request.AuthorId} does not exist.");

        var created = await _posts.AddAsync(new Post
        {
            Title = request.Title,
            Body  = request.Body,
            UserId = request.AuthorId
        });

        var dto = new PostDto
        {
            Id = created.Id,
            Title = created.Title,
            Body = created.Body,
            AuthorId = created.UserId
        };

        return Created($"/posts/{dto.Id}", dto);
    }
    
    [HttpGet("{id:int}")]
    public async Task<ActionResult<PostDto>> GetByIdAsync(int id)
    {
        var post = await _posts.GetSingleAsync(id);
        if (post is null) return NotFound();

        var dto = new PostDto
        {
            Id = post.Id,
            Title = post.Title,
            Body = post.Body,
            AuthorId = post.UserId
        };
        return Ok(dto);
    }
    
    [HttpGet]
    public ActionResult<IEnumerable<PostDto>> GetMany(
        [FromQuery] string? titleContains,
        [FromQuery] int? authorId,
        [FromQuery] string? authorName)
    {
        var q = _posts.GetManyAsync(); // IQueryable<Post>

        if (!string.IsNullOrWhiteSpace(titleContains))
            q = q.Where(p => p.Title != null &&
                             p.Title.Contains(titleContains, StringComparison.OrdinalIgnoreCase));

        if (authorId.HasValue)
            q = q.Where(p => p.UserId == authorId.Value);

        if (!string.IsNullOrWhiteSpace(authorName))
        {
            var matchingUserIds = _users.GetManyAsync()
                .Where(u => u.Username != null &&
                            u.Username.Contains(authorName, StringComparison.OrdinalIgnoreCase))
                .Select(u => u.Id)
                .ToList();

            q = q.Where(p => matchingUserIds.Contains(p.UserId));
        }

        var result = q.Select(p => new PostDto
        {
            Id = p.Id,
            Title = p.Title,
            Body = p.Body,
            AuthorId = p.UserId
        }).ToList();

        return Ok(result);
    }
    
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] PostDto request)
    {
        if (id != request.Id) return BadRequest("Route id and body id must match.");

        var existing = await _posts.GetSingleAsync(id);
        if (existing is null) return NotFound();

    
        var author = await _users.GetSingleAsync(request.AuthorId);
        if (author is null) return BadRequest($"User {request.AuthorId} does not exist.");

        existing.Title = request.Title;
        existing.Body  = request.Body;
        existing.UserId = request.AuthorId;

        await _posts.UpdateAsync(existing);
        return NoContent();
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var existing = await _posts.GetSingleAsync(id);
        if (existing is null) return NotFound();

        await _posts.DeleteAsync(id);
        return NoContent();
    }
}
