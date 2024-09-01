using Microsoft.AspNetCore.Mvc;
using TodoApiService.Models;
using TodoApiService.Services;

namespace TodoApiService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TodoController : ControllerBase
{
    private readonly ITodoService _todoService;

    public TodoController(ITodoService todoService)
    {
        _todoService = todoService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodos()
    {
        var todos = await _todoService.GetTodosAsync();
        return Ok(todos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TodoItem>> GetTodoItem(int id)
    {
        var todo = await _todoService.GetTodosAsync();
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
        await _todoService.AddTodoAsync(todoItem);
        return CreatedAtAction(nameof(GetTodoItem), new { id = todoItem.Id }, todoItem);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutTodoItem(int id, TodoItem todoItem)
    {
        if (id != todoItem.Id)
        {
            return BadRequest();
        }

        var todoExists = (await _todoService.GetTodosAsync()).Any(t => t.Id == id);
        if (!todoExists)
        {
            return NotFound();
        }

        todoItem.Id = id; // Ensure the ID remains the same
        await _todoService.MarkAsCompletedAsync(id); // Assuming updating only marks as completed
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodoItem(int id)
    {
        var todoExists = (await _todoService.GetTodosAsync()).Any(t => t.Id == id);
        if (!todoExists)
        {
            return NotFound();
        }

        await _todoService.DeleteTodoAsync(id);
        return NoContent();
    }
}