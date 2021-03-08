using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using notejamsinglepage.DAL;
using notejamsinglepage.Models;

namespace notejamsinglepage.Controllers
{
    public class NoteViewModelsController : Controller
    {
        private NotesContext db = new NotesContext();

        // GET: NoteViewModels
        public ActionResult Index()
        {
            return View(db.Notes.ToList());
        }

        // GET: NoteViewModels/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NoteViewModel noteViewModel = db.Notes.Find(id);
            if (noteViewModel == null)
            {
                return HttpNotFound();
            }
            return View(noteViewModel);
        }

        // GET: NoteViewModels/Create
        public ActionResult Create()
        {
            //return View();

            NoteViewModel model = new NoteViewModel();
            model.Pads = db.Pad.ToList();
            

            return View(model);
        }

        // POST: NoteViewModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Description,Created,PadId")] NoteViewModel noteViewModel)
        {
            if (ModelState.IsValid)
            {
                List<PadViewModel> nav = db.Pad.ToList();                
                noteViewModel.Created = DateTime.Now.ToString("dd-MM-yyyy");                
                db.Notes.Add(noteViewModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(noteViewModel);
        }

        // GET: NoteViewModels/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NoteViewModel noteViewModel = db.Notes.Find(id);
            if (noteViewModel == null)
            {
                return HttpNotFound();
            }
            return View(noteViewModel);
        }

        // POST: NoteViewModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Description,Created")] NoteViewModel noteViewModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(noteViewModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(noteViewModel);
        }

        // GET: NoteViewModels/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NoteViewModel noteViewModel = db.Notes.Find(id);
            if (noteViewModel == null)
            {
                return HttpNotFound();
            }
            return View(noteViewModel);
        }

        // POST: NoteViewModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NoteViewModel noteViewModel = db.Notes.Find(id);
            db.Notes.Remove(noteViewModel);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
