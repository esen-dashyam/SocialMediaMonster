using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SocialMonster.DAL;

namespace SocialMonster.Controllers
{
    public class TreeviewController : Controller
    {
        // GET: Treeview
        
        public ActionResult Simple()
        {
            /*
            List<SiteMenu> all = new List<SiteMenu>();
            using (MonitoringEntities monitoringDB = new MonitoringEntities())
            {
                all = monitoringDB.System_Group.OrderBy(a => a.GroupID).ToList();
            }
           
            return View(all);
            */


            return View();
           
        }

        public ActionResult TreeView()
        {
            var db = new MonitoringEntities();
            return View(db.System_Groups.Where(x => !x.ParentID.HasValue).ToList());

        }


    }
}