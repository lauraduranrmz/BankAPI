using Microsoft.AspNetCore.Mvc;
using BankAPI.Services;
using BankAPI.Data.BankModels;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using BankAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace BankAPI.Controllers;

//Ruteo
[ApiController]
[Route("api/[controller]")]

public class BankTransactionController : ControllerBase
{
    //para modificar la base de datos
    private readonly BankTransactionService bankTransactionService;
    private readonly BankContext _context;
    private readonly AccountService accountService;
    public BankTransactionController(BankTransactionService bankTransactionService,BankContext context,AccountService accountService)
    {
        _context = context;
        this.bankTransactionService = bankTransactionService;
        this.accountService = accountService;

    }

    
    [HttpGet("MisCuentas")]
    //solo el cliente puede hace movimientos bancarios
    [Authorize(Roles = "Client")]
    public async Task<IActionResult> GetAccounts()
    {
        //obteniendo la info de los claims
        var nameClaim = HttpContext.User.FindFirst(ClaimTypes.Name);

        //extraer el nombre
        string name = nameClaim.Value;
        var accounts = await bankTransactionService.GetClientAccountsByClientName(name);
        return Ok(accounts);
    }

    [HttpGet("MisBankTransaction")]
    [Authorize(Roles = "Client")]
    public async Task<IActionResult> GetBankTransaction(int AccountId)
    {
        //obteniendo la info de los claims
        var nameClaim = HttpContext.User.FindFirst(ClaimTypes.Name);

        //extraer el nombre
        string name = nameClaim.Value;

        var transactions = await bankTransactionService.GetAllById(AccountId, name);
        return Ok(transactions);
    }

    [HttpGet("Movimientos/{Id}")]
    public async Task<IActionResult> GetTransaction(int id)
    {
        var movimiento = await _context.BankTransactions.FindAsync(id);

        if(movimiento is null)
            return NotFound();
        
        return Ok(movimiento);
    }

    [HttpPost("Retiros/{AccountId}")]
    //Solo los clientes pueden retirar dinero de SUS cuentas
    [Authorize(Roles = "Client")]
    public async Task<IActionResult> CreateRetiro(BankTransactionDtoIn bankTranDto, int AccountId)
    {
       //obteniendo la info de los claims
        var nameClaim = HttpContext.User.FindFirst(ClaimTypes.Name);

        //extraer el nombre
        string name = nameClaim.Value;

        //validar la cuenta 
        var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Id == AccountId && a.Clien.Name == name);

        if (account == null)
            return BadRequest("ERROR esta no es tu cuenta");

        var newretiro = await bankTransactionService.CreateRetiro(bankTranDto, name, account);
        return CreatedAtAction(nameof(GetTransaction),new {id = newretiro.Id}, newretiro);
    }

    [HttpPost("Depositos/{AccountId}")]
    [Authorize(Roles = "Client")]
    [AllowAnonymous]
    public async Task<IActionResult> CreateDeposito(BankTransactionDtoIn bankTranDto, int AccountId)
    {
        //validar la cuenta 
        var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Id == AccountId);

        var newDeposito = await bankTransactionService.CreateDeposito(bankTranDto, account);
        return CreatedAtAction(nameof(GetTransaction),new {id = newDeposito.Id}, newDeposito);
    }

    [HttpDelete("{AccountId}")]
    [Authorize(Roles = "Client")]
    public async Task<IActionResult> DeleteCuentas(int AccountId)
    {
        //obteniendo la info de los claims
        var nameClaim = HttpContext.User.FindFirst(ClaimTypes.Name);

        //extraer el nombre
        string name = nameClaim.Value;

        //validar la cuenta 
        var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Id == AccountId && a.Clien.Name == name);

        if (account == null)
            return BadRequest("ERROR no puedes efectuar este movimiento si no te pertenece esta cuenta");

        var balance = account.Balance;
        
        if(balance == 0) 
        {
            //validar si aun existe esa cuenta
            var accountToDelete =  accountService.GetById(AccountId);
            if(accountToDelete is not null)
            {
                await bankTransactionService.DeleteCuentas(AccountId);
                return NoContent();
            }else
                return NotFound();
        }else 
            return BadRequest("ERROR no puedes borrar tu cuenta, debe de estar en $0.00");
    
    }
}