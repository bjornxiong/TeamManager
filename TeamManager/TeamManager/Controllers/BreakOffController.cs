using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TeamManager.DataProvider;

namespace TeamManager.Controllers
{
    public class BreakOffController : Controller
    {
        private TeamManage_Entities db = new TeamManage_Entities();

        //
        // GET: /BreakOff/
       
        public ActionResult Index(Models.Users user)
        {
            //显示7天以内的请假
            DateTime a = DateTime.Now.AddDays(-7);
            var TBF = (from traineebreakoff in db.BreakOffs
                       orderby traineebreakoff.BreakOffFrom descending
                       where traineebreakoff.UserID == User.Identity.Name && traineebreakoff.BreakOffFrom >= a
                       select traineebreakoff).ToList();

            return View(TBF);
        }
        //
        // GET: /BreakOffManager/Details/5

        public ActionResult Details(Guid id)
        {
            BreakOff breakoff = db.BreakOffs.Find(id);
            if (breakoff == null)
            {
                return HttpNotFound();
            }
            return View(breakoff);
        }

        //
        // GET: /BreakOffManager/Create

        public ActionResult Create()
        {
            List<SelectListItem> cutfrom = new List<SelectListItem>();
            cutfrom.Add(new SelectListItem { Text = "Annual Leave", Value = "Annual Leave" });
            cutfrom.Add(new SelectListItem { Text = "Change Off", Value = "Change Off" });
            ViewData["CutFrom"] = cutfrom;
            return View();
        }

        //
        // POST: /BreakOffManager/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BreakOff breakoff)
        {

            if (ModelState.IsValid)
            {
                breakoff.UserID = User.Identity.Name;
                breakoff.BreakOffGuid = Guid.NewGuid();
                ViewData["breakoffguid"] = breakoff.BreakOffGuid;
                breakoff.Status = "Ask";
                db.BreakOffs.Add(breakoff);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(breakoff);
        }

        //
        // GET: /BreakOffManager/Edit/5

        public ActionResult Edit(Guid id)
        {
            List<SelectListItem> cutfrom1 = new List<SelectListItem>();
            cutfrom1.Add(new SelectListItem { Text = "Annual Leave", Value = "Annual Leave" });
            cutfrom1.Add(new SelectListItem { Text = "Change Off", Value = "Change Off" });
            ViewData["CutFrom1"] = cutfrom1;
            BreakOff breakoff = db.BreakOffs.Find(id);
            if (breakoff == null)
            {
                return HttpNotFound();
            }
            return View(breakoff);
        }

        //
        // POST: /Default1/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BreakOff breakoff)
        {
            if (ModelState.IsValid)
            {
                List<SelectListItem> cutfrom1 = new List<SelectListItem>();
                cutfrom1.Add(new SelectListItem { Text = "Annual Leave", Value = "Annual Leave" });
                cutfrom1.Add(new SelectListItem { Text = "Change Off", Value = "Change Off" });
                ViewData["CutFrom1"] = cutfrom1;
                db.Entry(breakoff).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(breakoff);
        }

        //
        // GET: /BreakOffManager/Delete/5

        public ActionResult Delete(Guid id)
        {
            BreakOff breakoff = db.BreakOffs.Find(id);
            if (breakoff == null)
            {
                return HttpNotFound();
            }
            return View(breakoff);
        }

        //
        // POST: /BreakOffManager/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            BreakOff breakoff = db.BreakOffs.Find(id);
            db.BreakOffs.Remove(breakoff);
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