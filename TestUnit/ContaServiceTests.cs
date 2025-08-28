

using BancoDigitalAPI.Data;
using BancoDigitalAPI.Models;
using BancoDigitalAPI.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Xunit;

public class ContaServiceTests
{
    // Criar serviço com banco InMemory
    private ContaService CriarServiceComDB()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("TestDb")
            .Options;

        var context = new AppDbContext(options);
        context.Contas.Add(new ContaCorrente { Conta = 54321, Saldo = 200 });
        context.SaveChanges();

        return new ContaService(context);
    }

    [Fact]
    public async Task ObterSaldo_DeveRetornarSaldoCorreto()
    {
        var service = CriarServiceComDB();
        var saldo = await service.ObterSaldoAsync(54321);
        Assert.Equal(200, saldo);
    }

    [Fact]
    public async Task Sacar_DeveDiminuirSaldo()
    {
        var service = CriarServiceComDB();
        var conta = await service.SacarAsync(54321, 100);
        Assert.Equal(100, conta.Saldo);
    }

    [Fact]
    public async Task Sacar_SemSaldo_DeveLancarException()
    {
        var service = CriarServiceComDB();
        await Assert.ThrowsAsync<Exception>(() => service.SacarAsync(54321, 500));
    }
}


