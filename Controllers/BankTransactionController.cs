using Microsoft.AspNetCore.Mvc;
using BankAPI.Services;
using BankAPI.Data.BankModels;
using BankAPI.Data.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using BankAPI.Data;
using Microsoft.EntityFrameworkCore;


namespace BankAPI.Controllers;
[Authorize(Policy = "ClientPolicy")]
[ApiController]
[Route("api/bank")]

    
public class BankTransactionController:ControllerBase{

 private readonly BankTransactionService _service;
     private readonly BankContext _context;
    public BankTransactionController(BankTransactionService service,BankContext context)
    {
        _service =  service;
        _context=context;
    }
    

[HttpGet("accounts/{customerId}")]
public async Task<ActionResult<IEnumerable<Account>>> GetAccountsForCustomer(int customerId)
{
    // Recuperar el id del usuario autenticado
    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

    // Verificar si el usuario autenticado es el cliente correspondiente
    if (userId != customerId.ToString())
    {
        return Unauthorized(new{message=$"Las cuentas que desea consultar no son las correspondientes.Su ID={userId} y el que desea consultar es ID={customerId.ToString()}"});
    }

    // Recuperar las cuentas del cliente correspondiente
    var accounts = await _service.GetAccountsForClients(customerId);

    return Ok(accounts);
}

[HttpPost("accounts/retiro")]
public async Task<ActionResult> Withdraw([FromBody] WithdrawDto withdrawDto)
{
    try
    {
        await _service.Withdraw(withdrawDto.AccountId, withdrawDto.Amount, withdrawDto.ExternalAccount);
        return Ok(new { message = "El retiro se realizó con éxito." });
    }
    catch (ArgumentException ex)
    {
        return BadRequest(new { message = ex.Message });
    }
}


[HttpPost("accounts/deposit")]
public async Task<ActionResult> Deposit([FromBody] DepositDto depositDto)
{
    // Recuperar el id del usuario autenticado
    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

    // Verificar si el usuario autenticado es el titular de la cuenta correspondiente
    var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Id == depositDto.AccountId && a.ClientId.ToString() == userId);

    if (account == null)
    {
        return Unauthorized(new { message = "La cuenta especificada no existe o no pertenece al usuario autenticado." });
    }

    if (!depositDto.IsValid())
    {
        return BadRequest(new { message = "El objeto depositDto no es válido.Solo se puede ingresar depositos en efectivo, es decir transactionType=1." });
    }

    try
    {
        await _service.Deposit(depositDto.AccountId, depositDto.Amount);
    }
    catch (ArgumentException ex)
    {
        return BadRequest(new { message = ex.Message });
    }

    return Ok(new { message = "El depósito se realizó con éxito." });
}
[HttpDelete("accounts/delete/{accountId}")]
public async Task<ActionResult> DeleteAccount(int accountId)
{
    // Recuperar el id del usuario autenticado
    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

    // Verificar si el usuario autenticado es el titular de la cuenta correspondiente
    var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Id == accountId && a.ClientId.ToString() == userId);

    if (account == null)
    {
        return NotFound(new { message = "La cuenta especificada no existe o no pertenece al usuario autenticado." });
    }

    try
    {
        await _service.DeleteAccount(accountId);
    }
    catch (ArgumentException ex)
    {
        return BadRequest(new { message = ex.Message });
    }
    catch (InvalidOperationException ex)
    {
        return BadRequest(new { message = ex.Message });
    }
    catch (DbUpdateException ex)
    {
        return BadRequest(new { message = ex.InnerException.Message });
    }

    return Ok(new { message = "La cuenta ha sido eliminada exitosamente." });
}



    
    
}