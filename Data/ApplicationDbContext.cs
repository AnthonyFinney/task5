using Microsoft.EntityFrameworkCore;

namespace Task5.Data;

public class ApplicationDbContext : DbContext {
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
}