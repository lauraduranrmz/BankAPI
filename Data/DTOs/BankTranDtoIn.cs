namespace BankAPI.Data.BankModels;

public partial class BankTransactionDtoIn
{
    public int Id { get; set; }
    public string AccountName { get; set; } = null!;

    public int AccountId { get; set; }

    public int TransactionType { get; set; }

    public decimal Amount { get; set; }

    public int? ExternalAccount { get; set; }

    public decimal AccountBalance {get; set;}

}