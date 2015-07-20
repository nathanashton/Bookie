using Bookie.Data;

namespace Bookie.Core.Domains
{
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