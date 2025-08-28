using BancoDigitalAPI.Data;
using BancoDigitalAPI.Models;
using HotChocolate;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace BancoDigitalAPI.Services;

public class ContaService
{
    private readonly AppDbContext _context;

    public ContaService(AppDbContext context) => _context = context;

    public async Task<ContaCorrente> SacarAsync(int conta, decimal valor)
    {
        // var c = await _context.Contas.FindAsync(conta);
        var c = await _context.Contas.FirstOrDefaultAsync(x => x.Conta == conta);
        if (c == null) throw new Exception("Conta não encontrada.");
        if (c.Saldo < valor) throw new GraphQLException("Saldo insuficiente.");
        c.Saldo -= valor;
        await _context.SaveChangesAsync();
        return c;
    }

    public async Task<ContaCorrente> DepositarAsync(int conta, decimal valor)
    {
        //var c = await _context.Contas.FindAsync(conta);
        var c = await _context.Contas.FirstOrDefaultAsync(x => x.Conta == conta);
        if (c == null) throw new Exception("Conta não encontrada.");
        c.Saldo += valor;
        await _context.SaveChangesAsync();
        return c;
    }

    public async Task<decimal> ObterSaldoAsync(int conta)
    {
        //var c = await _context.Contas.FindAsync(conta);
        //if (c == null) throw new Exception("Conta não encontrada.");
        var c = await _context.Contas.FirstOrDefaultAsync(x => x.Conta == conta);
        if (c == null) throw new Exception("Conta não encontrada.");

        
        return c.Saldo;
    }

    public async Task<ContaCorrente> CriarContaAsync(int conta, decimal saldoInicial)
    {
        var existente = await _context.Contas.FirstOrDefaultAsync(c => c.Conta == conta);
        if (existente != null)
            throw new Exception("Conta já existe.");

        var novaConta = new ContaCorrente
        {
            Conta = conta,
            Saldo = saldoInicial
        };

        _context.Contas.Add(novaConta);
        await _context.SaveChangesAsync();
        return novaConta;
    }

}