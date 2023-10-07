using System;
using System.Collections.Generic;

namespace BankAPI.Data.BankModels;

public partial class Account
{
    public int Id { get; set; }

    public int AccountType { get; set; }

    public int? ClienId { get; set; }

    public decimal Balance { get; set; }

    public DateTime RegData { get; set; }

    public virtual AccountType AccountTypeNavigation { get; set; }

    public virtual ICollection<BankTransaction> BankTransactions { get; set; } = new List<BankTransaction>();

    public virtual Client Clien { get; set; }
}
