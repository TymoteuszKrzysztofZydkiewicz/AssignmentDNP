using System.Threading.Tasks;
using ApiContracts;

public interface IUserService
{
    Task<ReturnUserDto> CreateAsync(CreateUserDto dto);
}