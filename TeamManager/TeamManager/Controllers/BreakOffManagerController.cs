using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TeamManager.DataProvider;

namespace TeamManager.Controllers
{
    public class BreakOffManagerController : Controller
    {
        private TeamManage_Entities db = new TeamManage_Entities();

        //
        // GET: /BreakOffManager/

        public ActionResult Index()
        {
            DateTime a = DateTime.Now.AddDays(-7);
            var TBF = from traineebreakoff in db.BreakOffs
                      orderby traineebreakoff.BreakOffFrom descending
                      where traineebreakoff.BreakOffFrom >= a
                      select traineebreakoff;
            return View(TBF.ToList());

        }

        //
        // GET: /Default1/Details/5
        public ActionResult Search(string sortOrder,string SearchDatefrom, string SearchDateto, string searchString)
        {
            //前台的搜索数值的传送
            var from1 = Convert.ToDateTime(SearchDatefrom);
            var to1 = Convert.ToDateTime(SearchDateto);
            var TBF1 = db.BreakOffs.OrderByDescending(s => s.BreakOffFrom).Select(s => s);
            if (!String.IsNullOrEmpty(searchString) && !String.IsNullOrEmpty(SearchDatefrom) && !String.IsNullOrEmpty(SearchDateto))
            {
                TBF1 = TBF1.Where(breakoff =>(breakoff.Status.Contains(searchString) && breakoff.BreakOffFrom >= from1 && breakoff.BreakOffTo <= to1));
            }
            return View(TBF1.ToList());

        }

        public ActionResult Edit(Guid id)
        {
            BreakOff breakoff = db.BreakOffs.Find(id);
            if (breakoff == null)
            {
                return HttpNotFound();
            }
            else
            {
                breakoff.Status = "Approve";
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Cancle(Guid id)
        {
            BreakOff breakoff = db.BreakOffs.Find(id);
            if (breakoff == null)
            {
                return HttpNotFound();
            }
            else
            {
                breakoff.Status = "Cancle";
            }
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