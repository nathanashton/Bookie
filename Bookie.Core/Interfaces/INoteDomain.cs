namespace Bookie.Core.Interfaces
{
    using Bookie.Common.Model;
    using System.Collections.Generic;

    public interface INoteDomain
    {
        IList<Note> GetAllNotes();

        Note GetNotesForBook(Book book);

        void AddNote(params Note[] note);

        void UpdateNote(params Note[] note);

        void RemoveNote(params Note[] note);
    }
}