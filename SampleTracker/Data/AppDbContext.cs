using Microsoft.EntityFrameworkCore;
using SampleTracker.Model;

namespace SampleTracker.Data;

public class AppDbContext : DbContext {
    public DbSet<Sample> Samples { get; set; }
    
    //Default constructor - DI Pattern
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
}   