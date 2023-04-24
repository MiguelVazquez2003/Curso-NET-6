using BankAPI.Data;
using BankAPI.Data.BankModels;
using BankAPI.Data.DTOs;
using Microsoft.EntityFrameworkCore;

namespace BankAPI.Services;

public class AccountService
{
    private readonly BankContext _context;
    public AccountService(BankContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<AccountDtoOut>> GetAll()
    {   
        //nos traemos las relaciones con los typenavegation
        return await _context.Accounts.Select(a=>new AccountDtoOut{
            Id=a.Id,
            AccountName=a.AccountTypeNavigation.Name,
            //si la relacion de client es diferente de nulo, asignar a client
            //el valor si no asigna una cadena vacia
            ClientName=a.Client!=null ? a.Client.Name:"",
            Balance=a.Balance,
            RegDate=a.RegDate
        }).ToListAsync();
    }
    public async Task<Account?> GetById(int id)
    {
        return  await _context.Accounts.FindAsync(id);
    }

    public async Task<Account> Create(AccountDTOIn newAccountDTO)
    {
        var newAccount = new Account();

        newAccount.AccountType = newAccountDTO.AccountType;
        newAccount.ClientId = newAccountDTO.ClientId;
        newAccount.Balance = newAccountDTO.Balance;

        _context.Accounts.Add(newAccount);
        await _context.SaveChangesAsync();

        return newAccount;
    }



    public async Task Update(int id, AccountDTOIn account)
    {


        var existingAccount = await GetById(id);


        if (existingAccount is not null)
        {
            existingAccount.AccountType = account.AccountType;
            existingAccount.ClientId = account.ClientId;
            existingAccount.Balance = account.Balance;



            await _context.SaveChangesAsync();
        }


    }

    public async Task Delete(int id)
    {
        var accountToDelete = await GetById(id);

        if (accountToDelete is not null)
        {
            _context.Accounts.Remove(accountToDelete);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<AccountDtoOut?> GetDtoById(int id)
    {   
        //usamos el where para que solamente sea en donde sea igual el id
        return await _context.Accounts.Where(a=>a.Id==id).
        Select(a=>new AccountDtoOut{
            Id=a.Id,
            //nos traemos las relaciones con los typenavegation
            AccountName=a.AccountTypeNavigation.Name,
            //si la relacion de client es diferente de nulo, asignar a client
            //el valor si no asigna una cadena vacia
            ClientName=a.Client!=null ? a.Client.Name:"",
            Balance=a.Balance,
            RegDate=a.RegDate
        }).SingleOrDefaultAsync();
    }
}
