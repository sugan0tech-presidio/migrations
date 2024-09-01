using Microsoft.EntityFrameworkCore;
using TodoApiService.Contexts;
using TodoApiService.Services;

namespace TodoApiService;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();
        builder.Services.AddDbContext<TodoContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
        builder.Services.AddScoped<ITodoService, TodoService>();
        
        #region Cors

        builder.Services.AddCors(opts =>
        {
            opts.AddPolicy("AllowAll",
                corsPolicyBuilder =>
                {
                    corsPolicyBuilder.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:5173",
                            "https://orange-hill-0f44ca01e.5.azurestaticapps.net")
                        .AllowCredentials();
                });
        });

        #endregion

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseCors("AllowAll");

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}