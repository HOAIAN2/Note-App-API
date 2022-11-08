// using System.Text;
// using System.Text.Json;
// using System.Security.Claims;
// using System.IdentityModel.Tokens.Jwt;
// using Microsoft.IdentityModel.Tokens;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.Cors;
// using Microsoft.AspNetCore.Authorization;
// namespace NodeWebAPI.Controllers;

// [ApiController]
// [Route("/[controller]")]

// public class NoteController : ControllerBase
// {
//     [Authorize]
//     [HttpPost]
//     public IActionResult CreateNote(NoteModel.CreateNote newNode)
//     {
//         var stream = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiaG9haWFuX2FkbWluIiwiZXhwIjoxNjY3ODg1ODA1fQ.QToFb9UbrGGR2WewyVEcIvPuKevqKWesVoCyZRIm_G4";
//         var handler = new JwtSecurityTokenHandler();
//         var jsonToken = handler.ReadToken(stream);
//         var tokenS = jsonToken as JwtSecurityToken;
//         Console.WriteLine(tokenS);
//         return Ok();
//     }
// }