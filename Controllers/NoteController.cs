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
        AppController.HandleIncreaseNoteCount(userID);
        if (note == null) return NotFound(JsonSerializer.Serialize("Error"));
        return Ok(note.GetInfos());
    }
    [Authorize]
    [HttpGet]
    public IActionResult Get()
    {
        LinkedList<NoteModel.Note> notes = new LinkedList<NoteModel.Note>();
        string jwt = Request.Headers.Authorization;
        int userID = AppController.ReadTokenUserID(jwt);
        foreach (var note in AppController.noteList)
        {
            if (note.ownerID == userID) notes.AddLast(note);
        }
        return Ok(JsonSerializer.Serialize(notes));
    }
}