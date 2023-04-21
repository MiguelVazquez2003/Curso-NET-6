using Microsoft.AspNetCore.Mvc;
using BankAPI.Services;
using BankAPI.Data.BankModels;
using BankAPI.Data.DTOs;

namespace BankAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly AccountService accountService;
    private readonly AccountTypeService accountTypeService;

    private readonly ClientService clientService;
    public AccountController(AccountService accountService,
    AccountTypeService accountTypeService,
    ClientService clientService)
    {
        this.accountService = accountService;
        this.accountTypeService = accountTypeService;
        this.clientService = clientService;
    }
    [HttpGet]
    public IEnumerable<Account> Get()
    {
        return accountService.GetAll();
    }
    [HttpGet("{id}")]
    public ActionResult<Account> GetById(int id)
    {
        var account = accountService.GetById(id);

        if (account is null)
        {
            return NotFound();
        }
        return account;
    }

    [HttpPost]
    public IActionResult Create(AccountDTO account)
    {
        string validation = ValidateAccount(account);


        if (!validation.Equals("Valid"))
        {
            return BadRequest();
        }
        var newAccount = accountService.Create(account);
        return CreatedAtAction(nameof(GetById), new { id = account.Id }, newAccount); // retorna status 201

    }

    [HttpPut("{id}")]

    public IActionResult Update(int id, AccountDTO account)
    {
        if (id != account.Id)
            return BadRequest();


        var accountToUpdate = accountService.GetById(id);
        if (accountToUpdate is not null)
        {
            string validation = ValidateAccount(account);


            if (!validation.Equals("Valid"))
            {
                return BadRequest();
            }
            accountService.Update(id, account);
            return NoContent();
        }
        else
        {
            return NotFound();
        }
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {

        var accountToDelete = accountService.GetById(id);
        if (accountToDelete is not null)
        {
            accountService.Delete(id);
            return Ok();
        }
        else
        {
            return NotFound();
        }


    }
    public string ValidateAccount(AccountDTO account)
    {
        string result = "Valid";

        var accountType = accountTypeService.GetById(account.AccountType);

        if (accountType is null)
        {
            result = $"El tipo de cuenta {account.AccountType} no existe.";
        }
        var clientID = account.ClientId.GetValueOrDefault();

        var client = clientService.GetById(clientID);

        if (client is null)
        {
            result = $"El cliente (clienteID) no existe";

        }
        return result;
    }



}