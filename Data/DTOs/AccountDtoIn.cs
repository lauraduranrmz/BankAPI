using System.Text.Json.Serialization;

namespace TestBankAPI.Data.DTOs;

public class AccountDtoIn
{
    public int Id { get; set; }

    public int AccountType { get; set; }

    public int? ClienId { get; set; }

    public decimal Balance { get; set; }
}