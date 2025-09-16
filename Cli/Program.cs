using Ass2.UI;
using InMemoryRepositories;
using RepositoryContracts;
using Server.InMemoryRepositories;

Console.WriteLine("App is starting...");

IUserRepository repository = new UserInMemoryRepository();

ICommentRepository commentRepository = new CommentInMemoryRepository();

IPostRepository postRepository = new PostInMemoryRepository();

CliApp cliApp  = new CliApp(repository, commentRepository, postRepository);

await cliApp.StartAsync();