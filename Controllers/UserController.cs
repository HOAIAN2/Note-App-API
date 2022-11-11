using System.Text;
using System.Text.Json;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;
namespace NodeWebAPI.Controllers;

[ApiController]
[Route("/[controller]")]

public class UserController : ControllerBase
{
    [Authorize]
    [HttpGet]
    public IActionResult Get(string username)
    {
        var user = AppController.userList.First;
        if (user == null) return BadRequest();
        return Ok(user.Value.GetInfos());
    }
    [EnableCors]
    [HttpPost("Login")]
    public IActionResult Login(UserModel.Login loginData)
    {
        foreach (var user in AppController.userList)
        {
            if (user.username == loginData.username)
            {
                if (user.hashedPassword == UserModel.User.CaculateHash(loginData.password))
                {
                    var token = new TokenModel.Token(CreateAccessToken(user), "");
                    return Ok(JsonSerializer.Serialize(token));
                }
            }
        }
        // Call Database if NotFound user in cache
        return NotFound(JsonSerializer.Serialize("Fail to Login"));
    }
    private string CreateAccessToken(UserModel.User user)
    {
        List<Claim> claims = new List<Claim>
        {
            new Claim("id", user.id.ToString()),
            new Claim("username", user.username)
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppController.secretKey));
        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: cred
        );
        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        return jwt;
    }
}