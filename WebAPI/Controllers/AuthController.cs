using ApiContracts;
using ApiContracts.Auth;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]

public class AuthController : ControllerBase
{
    private readonly IUserRepository userRepository;

    public AuthController(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }
    
    [HttpPost] 
    public async Task<ActionResult<ReturnUserDto>> AuthenticateLogin([FromBody] LoginRequest request) {
        var user = await userRepository.GetByUsernameAsync(request.Username);
        if (user is null) return Unauthorized();
        if (user.Password != request.Password) return Unauthorized();
        var dto = new ReturnUserDto { Id = user.Id, Username = user.Username };
        return Ok(dto); 
    }
}