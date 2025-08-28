
using BancoDigitalAPI.Data;
using BancoDigitalAPI.Models;
using BancoDigitalAPI.Services;
using HotChocolate; // Para GraphQLException
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using System;
using System.Runtime.Intrinsics.X86;
using System.Threading.Tasks;
using Xunit;
using static System.Net.Mime.MediaTypeNames;

#region 1º TESTE SIMPLES E AVALIAÇÃO
/// <summary>
///class ContaServiceTests
///Classe de teste pública.
///Contém métodos marcados com [Fact] do xUnit.
///Cada método representa um cenário de teste independente.
///
/// Método Auxiliar: CriarServiceComDB()
///Função: 
///cria uma instância do serviço ContaService com banco InMemory, populando uma conta inicial para testes.
///UseInMemoryDatabase("TestDb") → Banco de dados temporário em memória.
///context.Contas.Add(...) → Insere uma conta inicial com número 54321 e saldo 200.
///SaveChanges() → Persiste os dados no InMemory DB.
///Retorna uma instância de ContaService pronta para testes. 
/// Observações: 
/// Cada teste é isolado, usando um banco InMemory diferente.
/// Métodos async Task permitem testes assíncronos.
/// [Fact] indica que o método é um teste independente (xUnit).
/// Assert.Equal e Assert.ThrowsAsync são usados para validar o comportamento esperado.
/// </summary>

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

    /// <summary>
    /// TESTE - ObterSaldoAsync retorna o saldo correto da conta.
    /// Cria o serviço com o banco InMemory.
    /// Consulta o saldo da conta 54321.
    /// Verifica se o valor retornado é igual a 200.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task ObterSaldo_DeveRetornarSaldoCorreto()
    {
        var service = CriarServiceComDB();
        var saldo = await service.ObterSaldoAsync(54321);
        Assert.Equal(200, saldo);
    }

    /// <summary>
    /// TESTE - Sacar_DeveDiminuirSaldo
    /// Método SacarAsync reduz corretamente o saldo da conta.
    /// Sacar 100 de uma conta que possui 200.
    /// Verifica se o saldo final é 100.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task Sacar_DeveDiminuirSaldo()
    {
        var service = CriarServiceComDB();
        var conta = await service.SacarAsync(54321, 100);
        Assert.Equal(100, conta.Saldo);
    }
    /// <summary>
    /// TESTE - Sacar_SemSaldo_DeveLancarException
    /// Método SacarAsync lança exceção quando o valor do saque excede o saldo disponível.
    /// Tenta sacar 500 de uma conta com saldo 200.
    ///Verifica se é lançada uma Exception com a mensagem esperada("Saldo insuficiente").
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task Sacar_SemSaldo_DeveLancarException()
    {
        /// retono simples
        //var service = CriarServiceComDB();
        //await Assert.ThrowsAsync<Exception>(() => service.SacarAsync(54321, 500));

        // Arrange
        var service = CriarServiceComDB();
        // Act & Assert
        var exception = await Assert.ThrowsAsync<GraphQLException>(
            () => service.SacarAsync(54321, 500)
        );
        // Verifica se a mensagem da exceção está correta
        Assert.Equal("Saldo insuficiente.", exception.Message);
    }


}

#endregion


/// <summary>
/// 2º TESTE E AVALIAÇÃO
/// </summary>

#region
//UseInMemoryDatabase(Guid.NewGuid().ToString()): 
//garante que cada teste tenha um banco separado,
//evitando interferência entre testes.
//Testes cobrem:
//ObterSaldoAsync(sucesso e falha)
//SacarAsync(sucesso, sem saldo, conta inexistente)
//DepositarAsync(sucesso e conta inexistente)
//CriarContaAsync(sucesso e conta existente)
//Exceções:
//GraphQLException é usada quando o saldo é insuficiente.
//Exception genérica para conta não encontrada ou já existente.
#endregion

//public class ContaServiceTests
//{
//    private ContaService CriarServiceComDB()
//    {
//        var options = new DbContextOptionsBuilder<AppDbContext>()
//            .UseInMemoryDatabase(Guid.NewGuid().ToString()) // Banco InMemory único por teste
//            .Options;

//        var context = new AppDbContext(options);

//        // Adiciona uma conta inicial para testes
//        context.Contas.Add(new ContaCorrente { Conta = 54321, Saldo = 200 });
//        context.SaveChanges();

//        return new ContaService(context);
//    }

//    [Fact]
//    public async Task ObterSaldo_DeveRetornarSaldoCorreto()
//    {
//        var service = CriarServiceComDB();
//        var saldo = await service.ObterSaldoAsync(54321);
//        Assert.Equal(200, saldo);
//    }

//    [Fact]
//    public async Task ObterSaldo_ContaInexistente_DeveLancarException()
//    {
//        var service = CriarServiceComDB();
//        await Assert.ThrowsAsync<Exception>(() => service.ObterSaldoAsync(99999));
//    }

//    [Fact]
//    public async Task Sacar_DeveDiminuirSaldo()
//    {
//        var service = CriarServiceComDB();
//        var conta = await service.SacarAsync(54321, 100);
//        Assert.Equal(100, conta.Saldo);
//    }

//    [Fact]
//    public async Task Sacar_SemSaldo_DeveLancarGraphQLException()
//    {
//        var service = CriarServiceComDB();
//        await Assert.ThrowsAsync<GraphQLException>(() => service.SacarAsync(54321, 500));
//    }

//    [Fact]
//    public async Task Sacar_ContaInexistente_DeveLancarException()
//    {
//        var service = CriarServiceComDB();
//        await Assert.ThrowsAsync<Exception>(() => service.SacarAsync(99999, 100));
//    }

//    [Fact]
//    public async Task Depositar_DeveAumentarSaldo()
//    {
//        var service = CriarServiceComDB();
//        var conta = await service.DepositarAsync(54321, 150);
//        Assert.Equal(350, conta.Saldo);
//    }

//    [Fact]
//    public async Task Depositar_ContaInexistente_DeveLancarException()
//    {
//        var service = CriarServiceComDB();
//        await Assert.ThrowsAsync<Exception>(() => service.DepositarAsync(99999, 100));
//    }

//    [Fact]
//    public async Task CriarConta_DeveCriarNovaConta()
//    {
//        var service = CriarServiceComDB();
//        var novaConta = await service.CriarContaAsync(12345, 500);
//        Assert.Equal(12345, novaConta.Conta);
//        Assert.Equal(500, novaConta.Saldo);
//    }

//    [Fact]
//    public async Task CriarConta_ContaExistente_DeveLancarException()
//    {
//        var service = CriarServiceComDB();
//        await Assert.ThrowsAsync<Exception>(() => service.CriarContaAsync(54321, 200));
//    }
//}