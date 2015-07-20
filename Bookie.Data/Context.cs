using System.Data.Entity;
using Bookie.Common;
using Bookie.Common.Model;

namespace Bookie.Data
{
    public class Context : DbContext
    {
        public Context()
            : base(Globals.DbConnectionString)
        {
#if EmptyDatabase
            Database.SetInitializer(new CreateDatabaseIfNotExists<Context>());
            Database.SetInitializer(new DropCreateDatabaseAlways<Context>());
#endif

            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }

        public DbSet<LogEntity> Logs { get; set; }

        public DbSet<Book> Books { get; set; }

        public DbSet<Author> Authors { get; set; }

        public DbSet<Publisher> Publishers { get; set; }

        public DbSet<SourceDirectory> SourceDirectories { get; set; }

        public DbSet<BookFile> BookFiles { get; set; }

        public DbSet<CoverImage> CoverImages { get; set; }

        public DbSet<BookHistory> BookHistories { get; set; }

        public DbSet<BookMark> BookMarks { get; set; }

        public DbSet<Note> Notes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //BookFile
            modelBuilder.Entity<BookFile>()
                .HasKey(e => e.Id);

            modelBuilder.Entity<Book>()
                        .HasOptional(s => s.BookFile)
                        .WithRequired(ad => ad.Book).WillCascadeOnDelete(true);

            //CoverImage
            modelBuilder.Entity<CoverImage>()
                .HasKey(e => e.Id);

            modelBuilder.Entity<Book>()
                        .HasOptional(s => s.CoverImage)
                        .WithRequired(ad => ad.Book).WillCascadeOnDelete(true);

            //BookHistory
            modelBuilder.Entity<BookHistory>()
                .HasKey(e => e.Id);

            modelBuilder.Entity<Book>()
                        .HasOptional(s => s.BookHistory)
                        .WithRequired(ad => ad.Book).WillCascadeOnDelete(true);

            //Publishers
            modelBuilder.Entity<Book>().HasMany<Publisher>(s => s.Publishers).WithOptional(c => c.Book).WillCascadeOnDelete(true);
            modelBuilder.Entity<Book>().HasMany<Author>(s => s.Authors).WithOptional(c => c.Book).WillCascadeOnDelete(true);

            modelBuilder.Entity<Book>().HasMany<BookMark>(s => s.BookMarks).WithRequired(c => c.Book).WillCascadeOnDelete(true);
            modelBuilder.Entity<Book>().HasMany<Note>(s => s.Notes).WithRequired(c => c.Book).WillCascadeOnDelete(true);

            //SourceDirectory
            modelBuilder.Entity<SourceDirectory>().HasMany<Book>(s => s.Books).WithRequired(s => s.SourceDirectory).HasForeignKey(o => o.SourceDirectoryId).WillCascadeOnDelete(true);
            modelBuilder.Entity<Book>().HasRequired<SourceDirectory>(p => p.SourceDirectory);
        }
    }
}