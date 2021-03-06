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
    public class PadViewModelsController : Controller
    {
        private NotesContext db = new NotesContext();

        // GET: PadViewModels
        public ActionResult Index()
        {
            return View(db.Pad.ToList());
        }

        // GET: PadViewModels/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PadViewModel padViewModel = db.Pad.Find(id);
            if (padViewModel == null)
            {
                return HttpNotFound();
            }
            return View(padViewModel);
        }

        // GET: PadViewModels/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PadViewModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Pad")] PadViewModel padViewModel)
        {
            if (ModelState.IsValid)
            {
                db.Pad.Add(padViewModel);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            return View(padViewModel);
        }

        // GET: PadViewModels/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PadViewModel padViewModel = db.Pad.Find(id);
            if (padViewModel == null)
            {
                return HttpNotFound();
            }
            return View(padViewModel);
        }

        // POST: PadViewModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Pad")] PadViewModel padViewModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(padViewModel).State = EntityState.Modified;
                db.SaveChanges();
                //return RedirectToAction("Index");
            }
            
            return RedirectToAction("Index", "Home");
        }

        // GET: PadViewModels/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PadViewModel padViewModel = db.Pad.Find(id);
            if (padViewModel == null)
            {
                return HttpNotFound();
            }
            return View(padViewModel);
        }

        // POST: PadViewModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PadViewModel padViewModel = db.Pad.Find(id);
            db.Pad.Remove(padViewModel);
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
