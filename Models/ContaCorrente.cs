using System.ComponentModel.DataAnnotations;

namespace BancoDigitalAPI.Models;

//public class ContaCorrente
//{
//    public int Conta { get; set; }
//    public decimal Saldo { get; set; }
//}

public class ContaCorrente
{
    [Key] // <-- chave primária
    public int Id { get; set; }
    public int Conta { get; set; }
    public decimal Saldo { get; set; }
}