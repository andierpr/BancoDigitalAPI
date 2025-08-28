namespace BancoDigitalAPI.TestUnit;


using BancoDigitalAPI.Data;
using BancoDigitalAPI.Models;
using BancoDigitalAPI.Services;
using HotChocolate;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Xunit;

public class ContaServiceTesteComMySql
{
    // Criar serviço usando banco MySQL real
    private ContaService CriarServiceComDB()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseMySql(
                "Server=localhost;Database=FuncionalDB;Uid=root;Pwd=admin;", // string de conexão 
                new MySqlServerVersion(new Version(8, 0, 36))
            )
            .Options;

        var context = new AppDbContext(options);

        // Garantir que a conta de teste existe
        var existente = context.Contas.FirstOrDefaultAsync(c => c.Conta == 54321).Result;
        if (existente == null)
        {
            context.Contas.Add(new ContaCorrente { Conta = 54321, Saldo = 200 });
            context.SaveChanges();
        }

        return new ContaService(context);
    }

    [Fact]
    public async Task ObterSaldo_DeveRetornarSaldoCorreto()
    {
        var service = CriarServiceComDB();
        //var saldo = await service.ObterSaldoAsync(54321);
        //Assert.Equal(200, saldo);

        // Obter saldo atual da conta
        var saldo = await service.ObterSaldoAsync(54321);
        // Validar que o saldo não é negativo
        Assert.True(saldo >= 0, $"Saldo inválido: {saldo}");
        // Opcional: validar que o saldo é suficiente para uma operação de teste
        var validaVlr = 200m;
        Assert.True(saldo >= validaVlr, $"Saldo ({saldo}) insuficiente para teste de {validaVlr}");
    }

    [Fact]
    public async Task Sacar_DeveDiminuirSaldo()
    {
        var service = CriarServiceComDB();
        var conta = await service.SacarAsync(54321, 100);
        Assert.Equal(100, conta.Saldo);
    }

    [Fact]
    public async Task Sacar_SemSaldo()
    {
        var service = CriarServiceComDB();
        var exception = await Assert.ThrowsAsync<GraphQLException>(
            () => service.SacarAsync(54321, 500)
        );
        Assert.Equal("Saldo insuficiente.", exception.Message);


        #region Teste 2
        //var service = CriarServiceComDB();
        //var nConta = 54321;
        //var valorSaque = 500m;
        //// Obter saldo atual da conta
        //var saldoAtual = await service.ObterSaldoAsync(nConta);
        //// Se o saldo já é insuficiente, o teste ainda deve passar
        //if (saldoAtual < valorSaque)
        //{
        //    var exception = await Assert.ThrowsAsync<GraphQLException>(
        //        () => service.SacarAsync(nConta, valorSaque)
        //    );

        //    // Verifica se a mensagem da exceção está correta
        //    Assert.Equal("Saldo insuficiente.", exception.Message);
        //}
        //else
        //{
        //    // Se o saldo fosse suficiente, reduzir para garantir teste determinístico
        //    var conta = await service.SacarAsync(nConta, saldoAtual);
        //    // Agora o saldo é zero, podemos testar saque insuficiente
        //    var exception = await Assert.ThrowsAsync<GraphQLException>(
        //        () => service.SacarAsync(nConta, 1m)
        //    );
        //    Assert.Equal("Saldo insuficiente.", exception.Message);
        //}
        #endregion
    }
}
