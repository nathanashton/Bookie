namespace Bookie.Core.Interfaces
{
    using System.Collections.Generic;
    using Common.Model;

    public interface INoteDomain
    {
        IList<Note> GetAllNotes();
        Note GetNotesForBook(Book book);
        void AddNote(params Note[] note);
        void UpdateNote(params Note[] note);
        void RemoveNote(params Note[] note);
    }
}