using Microsoft.AspNetCore.Mvc;
using BankAPI.Services;
using BankAPI.Data.DTos;

namespace BankAPI.Controllers;

[ApiController]
[Route("api/[controller]")]

public class LoginController : ControllerBase
{
    private readonly LoginService loginService;

    public LoginController(LoginService loginService)
    {
        this.loginService = loginService;
    }

    [HttpPost("authenticate")]

    public async Task<ActionResult> Login (AdminDto adminDto)
    {
        var admin = await loginService.GetAdmin(adminDto);

        if(admin is null)
            return BadRequest(new {message = "Credenciales inválidas"});
        //generar un token

        return Ok( new {token = "some value"});
    }

}