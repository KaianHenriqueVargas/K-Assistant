using Lib.Infraestrutura.Data;
using Microsoft.EntityFrameworkCore;

namespace Infraestrutura.Data;

public class AppDbContext : AppBaseDbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
        ApplicationAssembly = typeof(AppDbContext).Assembly;
    }
}