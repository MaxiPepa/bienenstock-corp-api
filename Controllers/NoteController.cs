﻿using BienenstockCorpAPI.Data;
using BienenstockCorpAPI.Data.Entities;
using BienenstockCorpAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BienenstockCorpAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class NoteController : ControllerBase 
    {
        #region Constructor
        private readonly NoteService _noteService;

        public NoteController(NoteService noteService)
        {
            _noteService = noteService;
        }
        #endregion

        #region Endpoints
        [HttpGet]
        public IActionResult GetNotes()
        {
            return Ok(_noteService.GetNotes());
        }
        #endregion
    }
}

