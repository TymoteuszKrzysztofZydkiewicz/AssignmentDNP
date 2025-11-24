namespace Entities;
public class Comment
{
    public int Id { get; set; }
    public string Content { get; set; }
    public int UserId { get; set; }
    public int PostId { get; set; }

    public Comment (){}
    
    public Post Post { get; set; }
    
    public User User { get; set; }

  
}