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

public class NoteController : ControllerBase
{
    [Authorize]
    [HttpPost]
    public IActionResult CreateNote(NoteModel.CreateNote newNode)
    {
        NoteModel.Note? note = null;
        string jwt = Request.Headers.Authorization;
        string username = AppController.ReadToken(jwt);
        var user = AppController.FindUser(username);
        var time = DateTimeOffset.Now.ToUnixTimeSeconds();
        note = new NoteModel.Note(1, user.id, newNode.title, newNode.content, time, time);
        AppController.noteList.AddLast(note);
        return Ok(note.GetInfos());
    }
    [Authorize]
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(JsonSerializer.Serialize(AppController.noteList));
    }
}