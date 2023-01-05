using SocialMonster.DAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace SocialMonster.Controllers
{
    public class SearchQController : Controller
    {
        // GET: SearchQ
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult s(string username)
        {

            //q = Server.UrlDecode(q);
            string url = Request.Url.ToString();

            int index1 = url.IndexOf("?q=") + 3;
            int index2 = url.IndexOf("&user");
            if (index2 < 1)
            {
                ViewBag.Type = 1;
                return View();
            }
            int cusLength = index2 - index1;
            string q = url.Substring(index1, cusLength);
            var howManyBytes = q.Length * sizeof(Char);
            if (howManyBytes != 48)
            {
                ViewBag.Type = 1;
                return View();
            }
            byte[] encryptedText = Convert.FromBase64String(q.ToString());
            //byte[] encryptedText = Encoding.UTF8.GetBytes("leXtSTmpD0Jc0WwCg2GsJw==");
            //byte[] Key = Convert.FromBase64String("8080808080808080");
            //byte[] IV = Convert.FromBase64String("1234567898765432");
            string register = null;
            using (AesManaged aes = new AesManaged())
            {
                aes.Mode = CipherMode.ECB;
                aes.BlockSize = 128;
                aes.Padding = PaddingMode.None;
                aes.Key = Encoding.UTF8.GetBytes("H@McQfTjWnZr4u7x");
                aes.IV = Encoding.UTF8.GetBytes("1234567898765432");
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                using (MemoryStream ms = new MemoryStream(encryptedText))
                {
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader reader = new StreamReader(cs))
                            register = reader.ReadToEnd();
                    }
                }
            }
            ViewBag.Register = register;
            register = Regex.Replace(register, @"[\u0004]", "");
            MonitoringEntities monitoringDB = new MonitoringEntities();
            var person = monitoringDB.System_Voters.Where(a => a.RegisterNumber == register).FirstOrDefault();
            if (person == null)
            {
                ViewBag.Type = 1;
            }
            else
            {
                if (person.FacebookID != null && person.FacebookID.Length > 1)
                {
                    return Redirect("http://fb.com/" + person.FacebookID);
                }
                else
                {
                    ViewBag.FName = person.SureName;
                    ViewBag.LName = person.Name;
                    ViewBag.Type = 2;
                }
            }
            return View();
        }
        public JsonResult SetFB(string account, string personID)
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();
            System_Voters voter = monitoringDB.System_Voters.Where(a => a.ID.ToString() == personID).FirstOrDefault();
            voter.FacebookID = account;
            monitoringDB.SaveChanges();
            return Json(JsonRequestBehavior.AllowGet);
        }
    }
}