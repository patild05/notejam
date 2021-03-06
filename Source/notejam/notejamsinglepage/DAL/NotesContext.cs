using notejamsinglepage.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace notejamsinglepage.DAL
{
    public class NotesContext : DbContext
    {
        //public NotesContext() : base("NoteContext")
        public NotesContext() : base("name=MyDbConnection")
        {
            
        }

        public DbSet<PadViewModel> Pad { get; set; }

        public DbSet<NoteViewModel> Notes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            base.OnModelCreating(modelBuilder);
        }
    }
}