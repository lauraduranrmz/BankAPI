
namespace TestBankAPI.Data.DTOs;

public class AccountDtoOut
{
    public int Id {get; set;}

    public string AccountName {get; set ;} = null!;

    public int AccountType;

    public string ClientName {get; set;} = null!;
    public int ClientId;
    public decimal Balance {get; set;}

    public DateTime RegData {get; set;}

}