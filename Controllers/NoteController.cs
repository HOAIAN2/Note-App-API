using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
namespace NodeWebAPI.Controllers;

[ApiController]
[Route("/[controller]")]

public class NoteController : ControllerBase
{
    [Authorize]
    [HttpPost]
    public IActionResult CreateNote(NoteModel.CreateNote newNote)
    {
        NoteModel.Note? note = null;
        string jwt = Request.Headers.Authorization;
        int userID = AppController.ReadTokenUserID(jwt);
        note = AppController.SaveNote(userID, newNote.title, newNote.content);
        if (note == null) return NotFound(JsonSerializer.Serialize("Error"));
        return Ok(note.GetInfos());
    }
    [Authorize]
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(JsonSerializer.Serialize(AppController.noteList));
    }
}