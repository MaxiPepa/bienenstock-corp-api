using BienenstockCorpAPI.Data;
using BienenstockCorpAPI.Data.Entities;
using BienenstockCorpAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BienenstockCorpAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NoteController : ControllerBase {
    
      
        private readonly NoteService _noteService;

        public NoteController(NoteService noteService)
        {
            _noteService = noteService;
        }

        [HttpGet]
        public IActionResult GetNotes()
        {
            return Ok(_noteService.GetNotes());
        }
    
    }
}

