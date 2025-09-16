using Entities;
using RepositoryContracts;

namespace Ass2.UI.ManageUsers;

public class ListUsersView
{
    private readonly IUserRepository userRepository;

    public ListUsersView(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public void Show()
    {
        Console.WriteLine();
        ViewUsersAsync();
    }
    
    private void ViewUsersAsync()
    {
        IEnumerable<User> manyAsync = userRepository.GetManyAsync();
        List<User> users = manyAsync.OrderBy(u => u.Id).ToList();
        
        Console.WriteLine("Users:");
        Console.WriteLine("[");
        foreach (User user in users)
        {
            Console.WriteLine($"\tID: {user.Id}, Name: {user.Username}");
        }

        Console.WriteLine("]");
        Console.WriteLine();
    }
}