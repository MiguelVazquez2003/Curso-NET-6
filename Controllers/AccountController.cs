using Microsoft.AspNetCore.Mvc;
using BankAPI.Services;
using BankAPI.Data.BankModels;
using BankAPI.Data.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace BankAPI.Controllers;

[Authorize(Policy = "SuperAdmin")]
[ApiController]
[Route("api/[controller]")]
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
   
    [HttpGet("getall")]
    public async Task<IEnumerable<AccountDtoOut>> Get()
    {
        return await accountService.GetAll();
    }
   
    [HttpGet("{id}")]
    public async Task<ActionResult<AccountDtoOut?>> GetById(int id)
    {
        var account = await accountService.GetDtoById(id);

        if (account is null)
        {
            return AccountNotFound(id);
        }
        return account;
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create(AccountDTOIn account)
    {
        string validation = await ValidateAccount(account);


        if (!validation.Equals("Valid"))
        {
            return BadRequest(new { message = validation });
        }
        var newAccount = await accountService.Create(account);
        return CreatedAtAction(nameof(GetById), new { id = account.Id }, newAccount); // retorna status 201

    }
    

    [HttpPut("update/{id}")]

    public async Task<IActionResult> Update(int id, AccountDTOIn account)
    {
        if (id != account.Id)
            return BadRequest(new { message = $"El ID{id} del parametro no coincide con el ID del cuerpo {account.Id}" });


        var accountToUpdate = await accountService.GetById(id);
        if (accountToUpdate is not null)
        {
            string validation = await ValidateAccount(account);


            if (!validation.Equals("Valid"))
            {
                return BadRequest(new { message = validation });
            }
            await accountService.Update(id, account);
            return NoContent();
        }
        else
        {
            return AccountNotFound(id);
        }
    }



    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {

        var accountToDelete = await accountService.GetById(id);
        if (accountToDelete is not null)
        {
            await accountService.Delete(id);
            return Ok();
        }
        else
        {
            return AccountNotFound(id);
        }


    }
    public async Task<string> ValidateAccount(AccountDTOIn account)
    {
        string result = "Valid";

        var accountType = await accountTypeService.GetById(account.AccountType);

        if (accountType is null)
        {
            result = $"El tipo de cuenta {account.AccountType} no existe.";
        }
        var clientID = account.ClientId.GetValueOrDefault();

        var client = await clientService.GetById(clientID);

        if (client is null)
        {
            result = $"El cliente (clienteID) no existe";

        }
        return result;
    }


    public NotFoundObjectResult AccountNotFound(int id)
    {
        return NotFound(new { message = $"La cuenta con ID={id} no existe" });
    }
}

