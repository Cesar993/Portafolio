#pragma warning disable CS8618
using Microsoft.EntityFrameworkCore;
namespace Portafolio.Models;
public class MyContext : DbContext 
{   
    
    public DbSet<User> Users { get; set; } 
    
    public DbSet<Post> Posts { get; set; }
    public MyContext(DbContextOptions options) : base(options) { }    

}
