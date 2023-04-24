using BankAPI.Data;
using BankAPI.Data.BankModels;
using BankAPI.Data.DTOs;
using Microsoft.EntityFrameworkCore;
namespace BankAPI.Services;

public class LoginClientService
{
    private readonly BankContext _context;
    public LoginClientService(BankContext context)
    {
        _context = context;
    }
    public async Task <Client?> GetClient(ClientDto client){
        //nos traemos tipo administrador cumpla las condiciones o un null
        return await _context.Clients.SingleOrDefaultAsync(
            x=>x.Email==client.Email && x.Pwd==client.Pwd);
    }
}