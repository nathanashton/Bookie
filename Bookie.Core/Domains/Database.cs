namespace Bookie.Core.Domains
{
    using Data;

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