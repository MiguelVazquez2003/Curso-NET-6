using BankAPI.Data;
using BankAPI.Data.BankModels;

namespace BankAPI.Services;

public class AccountTypeService 
{
private readonly BankContext _context;

public AccountTypeService(BankContext context){
    _context=context;
}

public AccountType? GetById (int id )
{
    return _context.AccountTypes.Find(id);
}




}