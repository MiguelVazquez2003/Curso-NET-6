using Microsoft.AspNetCore.Mvc;
using BankAPI.Services;
using BankAPI.Data.BankModels;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using BankAPI.Data.DTOs;
namespace BankAPI.Controllers;


[ApiController]
[Route("api/loginclient")]
public class LoginClientController : ControllerBase
{

    private readonly LoginClientService loginService;
    private IConfiguration config; //me permite trabajar con el archivo app settings .json
    public LoginClientController(LoginClientService service, IConfiguration config)
    {
        this.loginService = service;
        this.config = config;
    }

    [HttpPost("authenticate")]
    public async Task<ActionResult> Login(ClientDto clientDto)
    {
        var client = await loginService.GetClient(clientDto);
        if (client is null)
        {
            return BadRequest(new { message = $"Credenciales invalidas" });
        }
        string jwtToken = GenerateToken(client);

        return Ok(new { token = jwtToken });
    }

    private string GenerateToken(Client client)
    {
         var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, client.Name),
        new Claim(ClaimTypes.Email, client.Email),
        new Claim(ClaimTypes.NameIdentifier, client.Id.ToString()),
        new Claim(ClaimTypes.Role, "Client")
    };

    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:ClientKey"]));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var token = new JwtSecurityToken(
        issuer: null,
        audience: null,
        claims: claims,
        expires: DateTime.UtcNow.AddMinutes(60),
        signingCredentials: creds
    );

    return new JwtSecurityTokenHandler().WriteToken(token);
    }
}