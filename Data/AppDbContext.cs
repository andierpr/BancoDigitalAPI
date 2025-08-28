using BancoDigitalAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BancoDigitalAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<ContaCorrente> Contas => Set<ContaCorrente>();
}

