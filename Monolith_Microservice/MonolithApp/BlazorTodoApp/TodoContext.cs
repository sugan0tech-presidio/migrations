namespace BlazorTodoApp;

using Microsoft.EntityFrameworkCore;

public class TodoContext : DbContext
{
    public DbSet<TodoItem> TodoItems { get; set; }

    public TodoContext(DbContextOptions<TodoContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TodoItem>().HasKey(t => t.Id);
        base.OnModelCreating(modelBuilder);
    }
}
