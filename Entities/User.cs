namespace Entities;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    
    public User(string userName, string password)
    {
        Username = userName;
        Password = password;
    }
    
    private User() {}
    
    public ICollection<Post> Posts { get; set; }
    public ICollection<Comment> Comments { get; set; }

}