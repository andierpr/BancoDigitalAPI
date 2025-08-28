//using Microsoft.EntityFrameworkCore;

//internal class BancoDigitalContext
//{
//    private DbContextOptions<BancoDigitalContext> options;

//    public BancoDigitalContext(DbContextOptions<BancoDigitalContext> options)
//    {
//        this.options = options;
//    }
//}
using Microsoft.EntityFrameworkCore;
using BancoDigitalAPI.Models;

namespace BancoDigitalAPI.Data
{
    public class BancoDigitalContext : DbContext
    {
        // Construtor que recebe DbContextOptions
        public BancoDigitalContext(DbContextOptions<BancoDigitalContext> options)
            : base(options)
        {
        }

        // DbSet para suas entidades
        public DbSet<ContaCorrente> Contas { get; set; }
    }
}
