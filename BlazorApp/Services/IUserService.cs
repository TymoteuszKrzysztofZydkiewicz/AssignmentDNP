using System.Threading.Tasks;
using ApiContracts;

public interface IUserService
{
    public Task<ReturnUserDto> CreateAsync(CreateUserDto request);
    public Task<IResult> GetUsers();
    public Task<IResult> UpdateUserAsync();
    public Task<IResult> DeleteUserAsync();
    public Task<ReturnUserDto> GetUserAsync(int userId);
}