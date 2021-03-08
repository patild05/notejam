using System.Collections.Generic;

namespace notejamsinglepage.Models
{
    public class PadViewModel
    {
        
        public int Id { get; set; }

        public string Pad { get; set; }

        public virtual IEnumerable<NoteViewModel> Notes { get; set; }

        public PadViewModel()
        {
            this.Notes = new HashSet<NoteViewModel>();
        }
        public PadViewModel(int id,string pad)
        {           
            Pad = pad;
        }
    }
}