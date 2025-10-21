namespace ApiContracts.Comment;

public class CreateCommentDto
{
    public int PostId { get; set; }
    public string Body { get; set; }
    public int AuthorId { get; set; }
}