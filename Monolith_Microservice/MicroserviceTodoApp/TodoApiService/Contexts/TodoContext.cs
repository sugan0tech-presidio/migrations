using Microsoft.EntityFrameworkCore;
using TodoApiService.Models;

namespace TodoApiService.Contexts;

public class TodoContext(DbContextOptions<TodoContext> options) : DbContext(options)
{
    public DbSet<TodoItem> TodoItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TodoItem>().HasKey(t => t.Id);
        base.OnModelCreating(modelBuilder);
    }
}