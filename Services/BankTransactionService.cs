using BankAPI.Data;
using BankAPI.Data.BankModels;
using Microsoft.EntityFrameworkCore;

namespace BankAPI.Services;

public class BankTransactionService{


private readonly BankContext _context;

public BankTransactionService(BankContext context){
    _context=context;
}
public async Task<IEnumerable<Account>> GetAccountsForClients(int clientId)
{
    var client = await _context.Clients
        .Include(c => c.Accounts)
        .FirstOrDefaultAsync(c => c.Id == clientId);

    if (client == null)
    {
        throw new ArgumentException($"Client with ID {clientId} not found.");
    }

    return client.Accounts.ToList();
}

public async Task Withdraw(int accountId, decimal amount, int? externalAccount = null)
{
    var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Id == accountId);

    if (account == null)
    {
        throw new ArgumentException($"Account with ID {accountId} not found.");
    }

    if (account.Balance < amount)
    {
        throw new ArgumentException("Insufficient funds.");
    }

    var transactionType = externalAccount.HasValue ? 4 : 2;


    if (transactionType == 4 && externalAccount == null)
    {
        throw new ArgumentException("External account is required for transfer transactions.");
    }

    var transaction = new BankTransaction
    {
        AccountId = accountId,
        TransactionType = transactionType,
        Amount = amount,
        ExternalAccount = externalAccount,
        RegDate = DateTime.UtcNow
    };

    _context.BankTransactions.Add(transaction);

    account.Balance -= amount;

    await _context.SaveChangesAsync();
}






public async Task Deposit(int accountId, decimal amount)
{
    var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Id == accountId);

    if (account == null)
    {
        throw new ArgumentException($"Account with ID {accountId} not found.");
    }

    var transaction = new BankTransaction
    {
        AccountId = accountId,
        TransactionType = 1,
        Amount = amount,
        RegDate = DateTime.UtcNow
    };

    _context.BankTransactions.Add(transaction);

    account.Balance += amount;

    await _context.SaveChangesAsync();
}

public async Task DeleteAccount(int accountId)
{
    var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Id == accountId);

    if (account == null)
    {
        throw new ArgumentException($"Account with ID {accountId} not found.");
    }

    if (account.Balance > 0)
    {
        throw new ArgumentException("Account must have a balance of 0 to be deleted.");
    }

    var transactions = await _context.BankTransactions.Where(t => t.AccountId == accountId).ToListAsync();
    _context.BankTransactions.RemoveRange(transactions);
    _context.Accounts.Remove(account);

    await _context.SaveChangesAsync();
}








}
