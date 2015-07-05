using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookie.Core.Domains
{
    using Bookie.Data;

    public class Db
    {
        public void ReCreateDB()
        {
            using (var ctx = new Context())
            {
                ctx.Database.CreateIfNotExists();
            }
        }
    }
}
