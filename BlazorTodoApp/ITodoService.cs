namespace BlazorTodoApp;

public interface ITodoService
{
    Task<List<TodoItem>> GetTodosAsync();
    Task AddTodoAsync(TodoItem todo);
    Task MarkAsCompletedAsync(int id);
    Task DeleteTodoAsync(int id);
}