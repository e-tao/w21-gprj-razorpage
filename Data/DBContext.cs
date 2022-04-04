#nullable disable

public class DBContext : IdentityDbContext
{
    public DBContext(DbContextOptions<DBContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Product { get; set; }

    public DbSet<Employee> Employees { get; set; }
}
