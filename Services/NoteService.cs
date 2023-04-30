using BienenstockCorpAPI.Data.Entities;
using BienenstockCorpAPI.Data;

namespace BienenstockCorpAPI.Services
{
    public class NoteService
    {
        private readonly BienenstockCorpContext _context;

        public NoteService(BienenstockCorpContext context)
        {
            _context = context;
        }

        public IEnumerable<Note> GetNotes()
        {
            return _context.Note.ToList();
        }
    }
}
