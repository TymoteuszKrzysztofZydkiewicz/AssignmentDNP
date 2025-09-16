namespace Entities;
public class Comment
{
    public int Id { get; set; }
    public string Content { get; set; }
    public int UserId { get; set; }
    public int PostId { get; set; }
    
    public Comment(string content, int postId, int userId)
    {
        Content = content;
        PostId = postId;
        UserId = userId;
    }
}