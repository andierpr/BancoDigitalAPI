using BancoDigitalAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net;

namespace BancoDigitalAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<ContaCorrente> Contas => Set<ContaCorrente>();


   // Método OnModelCreating no DbContext para configurar o tamanho e tipo dos campos da tabela no banco de dados.
   // Usando Fluent API do Entity Framework Core.
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ContaCorrente>()
            .Property(c => c.Saldo)
            .HasPrecision(14, 2); // 14 dígitos no total e 2 decimais
    }
}

 