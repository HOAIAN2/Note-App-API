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
    public IActionResult Get()
    {
        string jwt = Request.Headers.Authorization;
        int userID = AppController.ReadTokenUserID(jwt);
        UserModel.User? user = null;
        user = AppController.FindUser(userID);
        if (user == null) return BadRequest(JsonSerializer.Serialize("Error"));
        return Ok(user.GetInfos());
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
                    string? accessToken = CreateAccessToken(user);
                    string refreshToken = "";
                    if (accessToken == null) return NotFound(JsonSerializer.Serialize("Fail to Login"));
                    var token = new TokenModel.Token(accessToken, refreshToken);
                    return Ok(JsonSerializer.Serialize(token));
                }
            }
        }
        // Call Database if NotFound user in cache
        return NotFound(JsonSerializer.Serialize("Fail to Login"));
    }
    private string? CreateAccessToken(UserModel.User user)
    {
        List<Claim> claims = new List<Claim>
        {
            new Claim("id", user.id.ToString()),
            new Claim("username", user.username)
        };
        if (AppController.secretKey == null) return null;
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