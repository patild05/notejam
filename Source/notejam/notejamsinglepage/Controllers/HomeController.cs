using notejamsinglepage.DAL;
using notejamsinglepage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace notejamsinglepage.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private NotesContext db = new NotesContext();
        // GET: /Pads/
        
        public ActionResult Index(int? id)
        {
            List<PadViewModel> nav = db.Pad.ToList();
            List<NoteViewModel> notes = db.Notes.ToList();

            List<CompositeModel> composite = new List<CompositeModel>();
            foreach (NoteViewModel note in notes)
            {
                composite.Add(new CompositeModel
                {
                    NoteId = note.Id,
                    Created = note.Created,
                    Title = note.Title,
                    PadName = nav.Where(x =>x.Id == note.PadId).First().Pad//"nordcloud"
                });
            }


            var tuple = new Tuple<List<PadViewModel>, List<CompositeModel>>(nav, composite);
            return View(tuple);
        }
        
        public ActionResult Details(int? id)
        {
            List<PadViewModel> nav = db.Pad.ToList();
            List<NoteViewModel> notes = db.Notes.ToList();
            if (id != null)
            {
                notes = db.Notes.Where(x => x.PadId == id).ToList();
            }

            List<CompositeModel> composite = new List<CompositeModel>();
            foreach (NoteViewModel note in notes)
            {
                composite.Add(new CompositeModel
                {
                    NoteId = note.Id,
                    Created = note.Created,
                    Title = note.Title,
                    PadName = nav.Where(x => x.Id == note.PadId).First().Pad//"nordcloud"
                });
            }

            var tuple = new Tuple<List<PadViewModel>, List<CompositeModel>>(nav, composite);
            return View("Details", tuple);
        }
    }
}
