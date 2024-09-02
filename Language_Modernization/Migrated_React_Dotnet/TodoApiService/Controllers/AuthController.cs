using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApiService.Contexts;
using TodoApiService.Models;

namespace TodoApiService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(TodoContext context) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] User user)
    {
        if (await context.Users.AnyAsync(u => u.Name == user.Name))
        {
            return BadRequest("User already exists.");
        }

        context.Users.Add(user);
        await context.SaveChangesAsync();

        return Ok("User registered successfully.");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] User loginUser)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Name == loginUser.Name);

        if (user == null || user.Password != loginUser.Password)
        {
            return Unauthorized("Invalid username or password.");
        }

        return Ok(user);
    }
}
