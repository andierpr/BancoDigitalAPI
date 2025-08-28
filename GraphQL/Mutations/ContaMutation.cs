using BancoDigitalAPI.Models;
using BancoDigitalAPI.Services;
using HotChocolate;
using System.Threading.Tasks;

namespace BancoDigitalAPI.GraphQL.Mutations;

public class ContaMutation
{
    public async Task<ContaCorrente> Sacar([Service] ContaService service, int conta, decimal valor) =>
        await service.SacarAsync(conta, valor);

    public async Task<ContaCorrente> Depositar([Service] ContaService service, int conta, decimal valor) =>
        await service.DepositarAsync(conta, valor);
}