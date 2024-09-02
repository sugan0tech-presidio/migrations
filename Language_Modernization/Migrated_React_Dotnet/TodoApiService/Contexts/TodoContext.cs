using Microsoft.EntityFrameworkCore;
using TodoApiService.Models;

namespace TodoApiService.Contexts;

public class TodoContext(DbContextOptions<TodoContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<TodoItem> TodoItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasMany(u => u.TodoItems)
            .WithOne(t => t.User)
            .HasForeignKey(t => t.UserId);

        base.OnModelCreating(modelBuilder);
    }}