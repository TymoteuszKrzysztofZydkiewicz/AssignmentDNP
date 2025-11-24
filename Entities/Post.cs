namespace Entities;

public class Post
{
    public int Id { get; set; }
    public string Body { get; set; }
    public string Title { get; set; }
    public int UserId { get; set; }

    public Post (){}
    
    public User User { get; set; }

    public ICollection<Comment> Comments { get; set; }

}