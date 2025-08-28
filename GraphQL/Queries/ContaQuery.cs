using BancoDigitalAPI.Services;
using HotChocolate;
using System.Threading.Tasks;

namespace BancoDigitalAPI.GraphQL.Queries;

public class ContaQuery
{
    public async Task<decimal> Saldo([Service] ContaService service, int conta) =>
        await service.ObterSaldoAsync(conta);
}