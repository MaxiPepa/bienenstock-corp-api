using BienenstockCorpAPI.Data.Entities;
using BienenstockCorpAPI.Data;

namespace BienenstockCorpAPI.Services
{
    public class NoteService
    {
        #region Constructor
        private readonly BienenstockCorpContext _context;

        public NoteService(BienenstockCorpContext context)
        {
            _context = context;
        }
        #endregion

        #region Notes
        public IEnumerable<Note> GetNotes()
        {
            return _context.Note.ToList();
        }
        #endregion
    }
}
