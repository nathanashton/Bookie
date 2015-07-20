using System;
using System.Collections.Generic;
using Bookie.Common.Model;
using Bookie.Core.Interfaces;
using Bookie.Data.Interfaces;
using Bookie.Data.Repositories;

namespace Bookie.Core.Domains
{
    public class NoteDomain : INoteDomain
    {
        private readonly INoteRepository _noteRepository;

        public NoteDomain()
        {
            _noteRepository = new NoteRepository();
        }

        public IList<Note> GetAllNotes()
        {
            throw new NotImplementedException();
        }

        public Note GetNotesForBook(Book book)
        {
            throw new NotImplementedException();
        }

        public void AddNote(params Note[] note)
        {
            throw new NotImplementedException();
        }

        public void UpdateNote(params Note[] note)
        {
            throw new NotImplementedException();
        }

        public void RemoveNote(params Note[] note)
        {
            throw new NotImplementedException();
        }
    }
}