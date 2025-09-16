using Entities;
using RepositoryContracts;

namespace Ass2.UI.ManagePosts;

public class SinglePostView
{
    private readonly IPostRepository postRepository;
    private readonly ICommentRepository commentRepository;
    private readonly IUserRepository userRepository;
    private readonly int postId;

    public SinglePostView(IPostRepository postRepository, ICommentRepository commentRepository, IUserRepository userRepository, int postId)
    {
        this.postRepository = postRepository;
        this.postId = postId;
        this.userRepository = userRepository;
        this.commentRepository = commentRepository;
    }

    public async Task ShowAsync()
    {
        Post post = await postRepository.GetSingleAsync(postId);
        Console.WriteLine($"-----------------------------------------------------");
        Console.WriteLine($"Title: {post.Title}");
        Console.WriteLine($"-----------------------------------------------------");
        Console.WriteLine($"Content: {post.Body}");
        Console.WriteLine($"-----------------------------------------------------");
        
        // use comment repository to load all comments for this post. The Where() method is used to filter the comments by the post id.
        List<Comment> comments = commentRepository.GetManyAsync().Where(c => c.PostId == postId).ToList();

        foreach (Comment comment in comments)
        {
            User user = await userRepository.GetSingleAsync(comment.UserId); // For each comment, load the associated user to get the username.
            Console.WriteLine($"{user.Username}: {comment.Content}");
        }

        Console.WriteLine();
        const string options = """
                               1) Add comment;
                               <) Back
                               """;
        Console.WriteLine(options);

        while (true)
        {
            string? input = Console.ReadLine();
            if (string.IsNullOrEmpty(input))
            {
                Console.WriteLine("Please select a valid option.");
                continue;
            }

            switch(input)
            {
                case "<":
                    return;
                case "1":
                    CreateComments createComment =
                        new CreateComments(postRepository, commentRepository);
                    await createComment.ShowAsync(postId);
                    await ShowAsync();
                    break;
            }
            
        }
    }
}