using Microsoft.AspNetCore.Mvc;
using simpleAPI.Dtos;
using simpleAPI.Models;
using simpleAPI.Repositories;

namespace simpleAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController: ControllerBase
    {
        private readonly IUserRepository repo;

        public UserController(IUserRepository repo)
        {
            this.repo = repo;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAll()
            => Ok(await repo.GetAllAsync());

        [HttpGet("{id:int}")]
        public async Task<ActionResult<User>> GetById(int id)
        {
            var user = await repo.GetByIdAsync(id);
            return user is null ? NotFound() : Ok(user);
        }


        [HttpPost]
        public async Task<ActionResult<User>> Create([FromBody] CreateUserDto createUserDto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            if (await repo.EmailExistsAsync(createUserDto.Email))
                 return Conflict(new { message = "Email already exists" });

            var user = new User
            {
                Name = createUserDto.Name,
                Email = createUserDto.Email
            };
            await repo.CreateAsync(user);
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateUserDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            if (await repo.EmailExistsAsync(dto.Email, excludeId: id))
                return Conflict(new { message = "Email already exists" });

            var updated = await repo.UpdateAsync(new User
            {
                Id = id,
                Name = dto.Name,
                Email = dto.Email
            });

            return updated ? NoContent() : NotFound();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await repo.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}
