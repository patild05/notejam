using notejamsinglepage.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace notejamsinglepage.DAL
{
    public class NoteInitializer : DropCreateDatabaseIfModelChanges<NotesContext> //CreateDatabaseIfNotExists<NotesContext>
    {
        protected override void Seed(NotesContext context)
        {
            var pad = new PadViewModel
            {
                Pad = "nordcloud"
            };

            var notes = new List<NoteViewModel> {
                new NoteViewModel {
                PadId=1,
                Description="Azure infrastructure is created",
                Title="Friday",
                Created=DateTime.Now.ToString("dd-MM-yyyy")
            },
                new NoteViewModel {
                PadId=1,
                Description="MVC application is created and it is up and running",
                Title="Monday",
                Created=DateTime.Now.ToString("dd-MM-yyyy")
            }
            };

            notes.ForEach(n => context.Notes.Add(n));
            context.Pad.Add(pad);
            context.SaveChanges();
        }
    }
}