using Newtonsoft.Json;
using SocialMonster.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using WEBAPIJWT.Auth;

namespace SocialMonster.Controllers
{
    public class PermissionController : System.Web.Http.ApiController
    {
        [HttpPost]
        [AllowAnonymous]
        [JwtAuthentication]
        public IHttpActionResult GetPermissionPerson(string UserID)
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();
            var persons = (from person in monitoringDB.System_Person
                         join permPerson in monitoringDB.AspNetUserToSystemPersons on person.ID equals permPerson.PersonID
                         where permPerson.UserID == UserID
                         select person).ToList<System_Person>();

            var json = JsonConvert.SerializeObject(persons);
            return Ok
                (
                   json
                );
        }

        [HttpPost]
        [AllowAnonymous]
        [JwtAuthentication]
        public IHttpActionResult GetPermissionView(string UserID)
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();
            var views = (from view in monitoringDB.System_View
                         join perm in monitoringDB.AspNetUserToSystemViews on view.ID equals perm.ViewID
                         where perm.UserID == UserID
                         select view).ToList<System_View>();

            var json = JsonConvert.SerializeObject(views);
            return Ok
                (
                   json
                );
        }
    }
}