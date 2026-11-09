using Microsoft.EntityFrameworkCore;
using WebApplication1.Models; 

namespace WebApplication1.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
    public DbSet<DataUserModel> User { get; set; }  // создание таблицы пользователей на основе модели DataUserModel
    public DbSet<DataAdminModel> Admin { get; set; } // создание таблицы Admin на основе модели DataAdminModel
}