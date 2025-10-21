using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _users;

        public UsersController(IUserRepository users) => _users = users;
        
        [HttpPost]
        public async Task<ActionResult<ReturnUserDto>> CreateAsync([FromBody] CreateUserDto request)
        {
            var existing = await _users.GetByUsernameAsync(request.Username);
            if (existing is not null)
                return Conflict($"Username '{request.Username}' is already taken.");

            var created = await _users.AddAsync(new User(request.Username, request.Password));

            var dto = new ReturnUserDto
            {
                Id = created.Id,
                Username = created.Username
            };
            
            return Created($"/users/{dto.Id}", dto);
        }
        
        [HttpGet]
        public ActionResult<GetUsersDto> GetMany([FromQuery] string? usernameContains)
        {
            var query = _users.GetManyAsync(); 

            if (!string.IsNullOrWhiteSpace(usernameContains))
            {
                query = query.Where(u =>
                    u.Username != null &&
                    u.Username.Contains(usernameContains, StringComparison.OrdinalIgnoreCase));
            }

            var result = new GetUsersDto
            {
                Users = query
                    .Select(u => new ReturnUserDto { Id = u.Id, Username = u.Username })
                    .ToList()
            };

            return Ok(result);
        }

        
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ReturnUserDto>> GetByIdAsync(int id)
        {
            var user = await _users.GetSingleAsync(id);
            if (user is null) return NotFound();

            return Ok(new ReturnUserDto { Id = user.Id, Username = user.Username });
        }

       
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] UserDto request)
        {
            if (id != request.Id)
                return BadRequest("Route id and body id must match.");

            var existing = await _users.GetSingleAsync(id);
            if (existing is null) return NotFound();

            existing.Username = request.Username;
            existing.Password = request.Password;

            await _users.UpdateAsync(existing);
            return NoContent();
        }

      
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var existing = await _users.GetSingleAsync(id);
            if (existing is null) return NotFound();

            await _users.DeleteAsync(id);
            return NoContent();
        }
    }
}
