using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookie.Data.Repositories
{
    using Bookie.Common.Model;
    using Bookie.Data.Interfaces;

    public class NoteRepository  : GenericDataRepository<Note>, INoteRepository
    {
    }
}
