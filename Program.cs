using BankAPI.Data;
using BankAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//Db Context
builder.Services.AddSqlServer<BankContext>(builder.Configuration.GetConnectionString("BankConnection"));
//Service Layer 
builder.Services.AddScoped<ClientService>();
builder.Services.AddScoped<AccountService>();
builder.Services.AddScoped<AccountTypeService>();
builder.Services.AddScoped<LoginService>();
builder.Services.AddScoped<LoginClientService>();
builder.Services.AddScoped<BankTransactionService>();


// Configuración para token de administrador
builder.Services.AddAuthentication("AdminToken")
    .AddJwtBearer("AdminToken", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

// Configuración para token de cliente
builder.Services.AddAuthentication("ClientToken")
    .AddJwtBearer("ClientToken", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:ClientKey"])),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

// Política de autorización para superadministradores
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("SuperAdmin", policy =>
    {
        policy.AuthenticationSchemes.Add("AdminToken");
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("AdminType", "Super");
    });
});

// Política de autorización para clientes autenticados
// Política de autorización para clientes autenticados
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ClientPolicy", policy =>
    {
        policy.AuthenticationSchemes.Add("ClientToken");
        policy.RequireAuthenticatedUser();
        policy.RequireRole("Client");
    });
});










var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
