using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApiService.Contexts;
using TodoApiService.Models;
using TodoApiService.Services;

namespace TodoApiService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TodoController(ITodoService todoService, TodoContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodos()
    {
        var todos = await todoService.GetTodosAsync();
        return Ok(todos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TodoItem>> GetTodoItem(int id)
    {
        var todo = await todoService.GetTodosAsync();
        var todoItem = todo.FirstOrDefault(t => t.Id == id);

        if (todoItem == null)
        {
            return NotFound();
        }

        return Ok(todoItem);
    }

    [HttpPost]
    public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem todoItem)
    {
        await todoService.AddTodoAsync(todoItem);
        return CreatedAtAction(nameof(GetTodoItem), new { id = todoItem.Id }, todoItem);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutTodoItem(int id, TodoItem todoItem)
    {
        if (id != todoItem.Id)
        {
            return BadRequest();
        }

        var todoExists = (await todoService.GetTodosAsync()).Any(t => t.Id == id);
        if (!todoExists)
        {
            return NotFound();
        }

        todoItem.Id = id; 
        await todoService.MarkAsCompletedAsync(id);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodoItem(int id)
    {
        var todoExists = (await todoService.GetTodosAsync()).Any(t => t.Id == id);
        if (!todoExists)
        {
            return NotFound();
        }

        await todoService.DeleteTodoAsync(id);
        return NoContent();
    }
    
    [HttpPost("todos")]
    public async Task<IActionResult> CreateTodoItem([FromBody] TodoItem todoItem)
    {
        var user = await context.Users.FindAsync(todoItem.UserId);

        if (user == null)
        {
            return NotFound("User not found.");
        }

        user.TodoItems.Add(todoItem);
        await context.SaveChangesAsync();

        return Ok(todoItem);
    }

    [HttpGet("todos/{userId}")]
    public async Task<IActionResult> GetUserTodos(int userId)
    {
        var todos = await context.TodoItems.Where(t => t.UserId == userId).ToListAsync();
        return Ok(todos);
    }

}