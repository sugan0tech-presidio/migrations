using Microsoft.EntityFrameworkCore;

namespace BlazorTodoApp;

public class TodoService : ITodoService
{
    private readonly TodoContext _context;

    public TodoService(TodoContext context)
    {
        _context = context;
    }

    public async Task<List<TodoItem>> GetTodosAsync()
    {
        return await _context.TodoItems.ToListAsync();
    }

    public async Task AddTodoAsync(TodoItem todo)
    {
        _context.TodoItems.Add(todo);
        await _context.SaveChangesAsync();
    }

    public async Task MarkAsCompletedAsync(int id)
    {
        var todo = await _context.TodoItems.FindAsync(id);
        if (todo != null)
        {
            todo.IsCompleted = true;
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteTodoAsync(int id)
    {
        var todo = await _context.TodoItems.FindAsync(id);
        if (todo != null)
        {
            _context.TodoItems.Remove(todo);
            await _context.SaveChangesAsync();
        }
    }
}