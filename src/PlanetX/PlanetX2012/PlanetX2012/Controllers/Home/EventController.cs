using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAO.DAO;
using DAO.Models;
using PlanetX2012.Models.DAO;

namespace PlanetX2012.Controllers.Home
{
    public class EventController : Controller
    {
        private PlanetXContext db = new PlanetXContext();

        //
        // GET: /Event/

        public ViewResult Index()
        {
            //var events = db.Events.Include(e => e.WebUser).Include(e => e.EventLocation);
            //return View(events.ToList());
            return View(new Event());
        }

        //
        // GET: /Event/Details/5

        public ViewResult Details(int id)
        {
            Event userEvent = db.Events.Find(id);
            return View(userEvent);
        }

        //
        // GET: /Event/Create
        public ActionResult Create()
        {
            //ViewBag.user_id = new SelectList(db.WebUsers, "user_id", "username");
            //ViewBag.event_id = new SelectList(db.EventLocations, "event_id", "event_id");

            return PartialView("../Event/ViewUserControl1");
        }


        //
        // POST: /Event/Create

        [HttpPost]
        public ActionResult Create(Event userEvent)
        {
            if (ModelState.IsValid)
            {
                db.Events.Add(userEvent);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.WebUsers, "UserId", "Password", userEvent.UserId);
            ViewBag.EventId = new SelectList(db.EventLocations, "EventId", "EventId", userEvent.EventId);
            return View(userEvent);
        }

        //
        // GET: /Event/Edit/5

        public ActionResult Edit(int id)
        {
            Event userEvent = db.Events.Find(id);
            ViewBag.UserId = new SelectList(db.WebUsers, "UserId", "Password", userEvent.UserId);
            ViewBag.EventId = new SelectList(db.EventLocations, "EventId", "EventId", userEvent.EventId);
            return View(userEvent);
        }

        //
        // POST: /Event/Edit/5

        [HttpPost]
        public ActionResult Edit(Event userEvent)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userEvent).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.WebUsers, "UserId", "Password", userEvent.UserId);
            ViewBag.EventId = new SelectList(db.EventLocations, "EventId", "EventId", userEvent.EventId);
            return View(userEvent);
        }

        //
        // GET: /Event/Delete/5

        public ActionResult Delete(int id)
        {
            Event userEvent = db.Events.Find(id);
            return View(userEvent);
        }

        //
        // POST: /Event/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Event userEvent = db.Events.Find(id);
            db.Events.Remove(userEvent);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}