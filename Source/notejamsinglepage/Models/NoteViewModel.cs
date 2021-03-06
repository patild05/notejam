using System;
using System.Collections.Generic;

namespace notejamsinglepage.Models
{
    public class NoteViewModel
    {
        public int Id { get; set; }

        public int PadId { get; set; }
        public string Title { get; set; }

        public string Description { get; set; }

        public string Created { get; set; }

        //public virtual PadViewModel Pad { get; set; }

        public IEnumerable<PadViewModel> Pads { get; set; }

        public NoteViewModel ()
        {          

        }

        public NoteViewModel (int id, string title, string description, int padId)
        {
            Id = id;
            Title = title;
            Description = description;
            PadId = padId;
        }
    }
}