using SocialMonster.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace SocialMonster.Controllers
{
    public class UsersController : Controller
    {
        // GET: Users
        public ActionResult Index(int? Date)
        {
            HttpContext.Server.ScriptTimeout = 300;

            if (User.Identity.IsAuthenticated == false)
            {
                return Redirect("/Account/Login");
            }
            MonitoringEntities monitoringDB = new MonitoringEntities();
            monitoringDB.Database.CommandTimeout = 600;
            if (Date == null)
            {
                Date = 1;
            }
            ViewBag.Date = Date;
            DateTime dt = DateTime.Now;
            DateTime dt1 = DateTime.Now;
            DateTime dt2 = DateTime.Now;
            //switch (Date)
            //{
            //    case 1:
            //        dt1 = dt.AddDays(-1);
            //        break;

            //    case 5:
            //        dt1 = dt.AddDays(-5);
            //        break;

            //    case 7:
            //        dt1 = dt.AddDays(-7);
            //        break;
            //    case 10:
            //        dt1 = dt.AddDays(-10);
            //        break;
            //    case 14:
            //        dt1 = dt.AddDays(-14);
            //        break;
            //    case 30:
            //        dt1 = dt.AddMonths(-1);
            //        break;
            //    case 365:
            //        dt1 = dt.AddYears(-1);
            //        break;
            //    case 2000:
            //        dt1 = dt.AddYears(-100);
            //        break;
            //    default:
            //        dt1 = dt.AddYears(-100);
            //        break;
            //}
            ViewBag.FacebookUsersCount = 108490;
            ViewBag.TwitterUsersCount = monitoringDB.Twitter_Users.Count();
            ViewBag.WebUsersCount = monitoringDB.WebSites.Count();

            List<Models.TopUser> allMostPostedUsers = new List<Models.TopUser>();
            List<Models.TopUser> allMostSharedUsers = new List<Models.TopUser>();
            List<Models.TopUser> allMostTweetedUsers = new List<Models.TopUser>();
            List<Models.TopUser> allTopWebsites = new List<Models.TopUser>();
            List<Models.TopUser> allTopCommentUsers = new List<Models.TopUser>();
            //var mostPostedWebsite =
            //from post in monitoringDB.WebSite_Posts.ToList()
            //join postList in monitoringDB.WebSite_PostList on post.PostListID equals postList.ID
            //join webSite in monitoringDB.WebSites on postList.WebSiteID equals webSite.ID
            //select new
            //{
            //    PostID = post.ID,
            //    PostLink = post.Link,
            //    ID = webSite.SiteName,
            //    Date = post.Date
            //};

            for (int i = 0; i < Date; i++)
            {
                double tmpDate = i;
                dt1 = dt.AddDays(-tmpDate);
                dt2 = dt1.AddDays(-1);
                //Хамгийн их посттой хэрэглэгч
                List<Models.TopUser> tmpFbList = new List<Models.TopUser>();
                tmpFbList=monitoringDB.Facebook_Posts.Where(a => a.UpdateTime >= dt2 && a.UpdateTime <= dt1)
                         .Select(a => new Models.TopUser
                         {
                             fb_ID = a.FromID,
                             fb_Name = a.FromName,
                             fb_shareCount=a.SharedCount ?? default(int)
                         })
                         .ToList();
                var mostPostedUsers = tmpFbList
                         .GroupBy(a => a.fb_ID)
                         .Select(a => new Models.TopUser
                         {
                             fb_ID = a.FirstOrDefault().fb_ID,
                             fb_Name = a.FirstOrDefault().fb_Name,
                             fb_Count = a.Count()
                         })
                         .Take(200)
                         .OrderByDescending(a => a.fb_Count)
                         .ToList();
                var mostSharedUsers = tmpFbList
                         .GroupBy(a => a.fb_ID)
                         .Select(a => new Models.TopUser
                         {
                             fb_ID = a.FirstOrDefault().fb_ID,
                             fb_Name = a.FirstOrDefault().fb_Name,
                             fb_shareCount = a.Sum(s=>s.fb_shareCount)
                         })
                         .Take(200)
                         .OrderByDescending(a => a.fb_shareCount)
                         .ToList();
                foreach (var user in mostPostedUsers)
                {
                    if (user.fb_Name != null)
                    {
                        if (user.fb_Name.Length > 15)
                        {

                            user.fb_Name = user.fb_Name.Substring(0, 15) + "...";
                        }
                    }
                    Models.TopUser key = new Models.TopUser();
                    key.fb_ID = user.fb_ID;
                    key.fb_Name = user.fb_Name;
                    key.fb_Count = user.fb_Count;
                    allMostPostedUsers.Add(key);
                }
                foreach (var user in mostSharedUsers)
                {
                    if (user.fb_Name != null)
                    {
                        if (user.fb_Name.Length > 15)
                        {

                            user.fb_Name = user.fb_Name.Substring(0, 15) + "...";
                        }
                    }
                    Models.TopUser key = new Models.TopUser();
                    key.fb_ID = user.fb_ID;
                    key.fb_Name = user.fb_Name;
                    key.fb_shareCount = user.fb_shareCount;
                    allMostSharedUsers.Add(key);
                }
                //Хамгийн их твиттэй хэрэглэгч
                var timeFilteredTwitterUsers = monitoringDB.Twitter_Tweets.Where(a => a.CreatedAt >= dt2 && a.CreatedAt <= dt1);
                List<Models.TopUser> tmpTwList = new List<Models.TopUser>();
                tmpTwList= monitoringDB.Twitter_Tweets.Where(a => a.CreatedAt >= dt2 && a.CreatedAt <= dt1)
                    .Select(a=> new Models.TopUser
                    {
                        UserID = a.TwitterUserID.ToString(),
                        UserName = a.ScreenName,
                    }
                    ).ToList();
                if (Date != 365 && Date != 2000)
                {
                    var mostTweetedUsers = tmpTwList
                        .GroupBy(a => a.UserID)
                        .Select(a => new Models.TopUser
                        {
                            UserID = a.FirstOrDefault().UserID,
                            UserName = a.FirstOrDefault().UserName,
                            TweetNumber = a.Count(),
                        }
                         )
                         .Take(200)
                         .OrderByDescending(a => a.TweetNumber)
                         .ToList();
                    foreach (var user in mostTweetedUsers)
                    {
                        if (user.UserName != null)
                        {
                            Models.TopUser key = new Models.TopUser();
                            key.UserID = user.UserID;
                            key.UserName = user.UserName;
                            key.TweetNumber = user.TweetNumber;
                            allMostTweetedUsers.Add(key);
                        }
                    }
                }
                else
                {
                    ViewBag.mostTweetedUsers = monitoringDB.Twitter_Users.Take(10).OrderByDescending(a => a.TweetNumber);
                }

                //Хамгийн их нийтлэлтэй сайтууд
                //var timeFilteredWebsites = mostPostedWebsite.Where(a => a.Date >= dt1 && a.Date <= dt);
                //List<Models.TopUser> tmpWbList = new List<Models.TopUser>();
                //tmpWbList = monitoringDB.WebSite_Posts.Where(a => a.Date >= dt2 && a.Date <= dt1)
                //    .Select(n => new Models.TopUser
                //    {
                //        web_name=n.
                //    });
                //foreach (var row in timeFilteredWebsites)
                //{
                //    Models.TopUser key = new Models.TopUser();
                //    key.web_name = row.ID.ToString();
                //    tmpWbList.Add(key);
                //}
                //var TopWebSites = tmpWbList
                //            .GroupBy(a => a.web_name).Select(n => new Models.TopUser
                //            {
                //                web_name = n.FirstOrDefault().web_name,
                //                web_count = n.Count()
                //            }).OrderByDescending(a => a.web_count).ToList();
                //foreach (var index in TopWebSites)
                //{
                //    if (index.web_name != null)
                //    {
                //        if (index.web_name.Length > 10)
                //        {

                //            index.web_name = index.web_name.Substring(0, 10) + "...";
                //        }
                //    }
                //    Models.TopUser key = new Models.TopUser();
                //    key.web_name = index.web_name;
                //    key.web_count = index.web_count;
                //    allTopWebsites.Add(key);
                //}

                //Хамгийн коммент бичсэн хэрэглэгч
                List<Models.TopUser> tmpCmList = new List<Models.TopUser>();
                tmpCmList= monitoringDB.Facebook_Post_Comments.Where(a => a.CreateTime >= dt2 && a.CreateTime <= dt1)
                    .Select(a=> new Models.TopUser
                    {
                        cmt_id=a.FromID,
                        cmt_name=a.FromName
                    }).ToList();
                var topCommentUsers = tmpCmList
                    .GroupBy(a => a.cmt_id).Select(n => new Models.TopUser
                    {
                        cmt_name = n.FirstOrDefault().cmt_name,
                        cmt_id = n.FirstOrDefault().cmt_id,
                        cmt_count = n.Count(),
                    }).Take(200).OrderByDescending(a => a.cmt_count).ToList();
                foreach (var row in topCommentUsers)
                {
                    if (row.cmt_name != null)
                    {
                        if (row.cmt_name.Length > 15)
                        {

                            row.cmt_name = row.cmt_name.Substring(0, 15) + "...";
                        }
                    }
                    Models.TopUser key = new Models.TopUser();
                    key.cmt_id = row.cmt_id;
                    key.cmt_name = row.cmt_name;
                    key.cmt_count = row.cmt_count;
                    allTopCommentUsers.Add(key);
                }
            }
            ViewBag.mostSharedUsers = allMostSharedUsers.GroupBy(a => a.fb_ID).Select(n => new Models.TopUser
            {
                fb_ID = n.FirstOrDefault().fb_ID,
                fb_Name = n.FirstOrDefault().fb_Name,
                fb_shareCount = n.Sum(s => s.fb_shareCount)
            }
            ).OrderByDescending(a => a.fb_shareCount).Take(10).ToList();
            ViewBag.mostPostedUsers = allMostPostedUsers.GroupBy(a => a.fb_ID).Select(n => new Models.TopUser
                        {
                            fb_ID = n.FirstOrDefault().fb_ID,
                            fb_Name = n.FirstOrDefault().fb_Name,
                            fb_Count = n.Sum(s => s.fb_Count)
                        }
                ).OrderByDescending(a => a.fb_Count).Take(10).ToList();
            ViewBag.mostTweetedUsers = allMostTweetedUsers.GroupBy(a => a.UserID).Select(n => new Models.TopUser
            {
                UserID = n.FirstOrDefault().UserID,
                UserName = n.FirstOrDefault().UserName,
                TweetNumber = n.Sum(s => s.TweetNumber)
            }
            ).OrderByDescending(a => a.TweetNumber).Take(10).ToList();
            //ViewBag.topWebSites = allTopWebsites.GroupBy(a => a.web_name).Select(n => new Models.TopUser
            //{
            //    web_name = n.FirstOrDefault().web_name,
            //    web_count = n.Sum(s => s.web_count)
            //}).OrderByDescending(a => a.web_count).Take(10).ToList();
            ViewBag.topCommentUsers = allTopCommentUsers.GroupBy(a => a.cmt_id).Select(n => new Models.TopUser
            {
                cmt_id = n.FirstOrDefault().cmt_id,
                cmt_name = n.FirstOrDefault().cmt_name,
                cmt_count = n.FirstOrDefault().cmt_count
            }).OrderByDescending(a => a.cmt_count).Take(10).ToList();

            allMostPostedUsers = null;
            allMostTweetedUsers = null;
            allTopCommentUsers = null;
            return View();
        }
        public ActionResult ListPeople()
        {
            if (User.Identity.IsAuthenticated == false)
            {
                return Redirect("/Account/Login");
            }
            MonitoringEntities monitoringDB = new MonitoringEntities();
            double doublePageCount= monitoringDB.System_Voters.Count();
            int pageCount = Convert.ToInt32(doublePageCount / 30);
            if (pageCount < (doublePageCount / 30))
            {
                pageCount++;
            }
            ViewBag.pageCount = pageCount;
            ViewBag.Aimag = monitoringDB.System_Aimag.ToList();
            var list = monitoringDB.System_Voters.Select(n => new Models.VotersModel
            {
                ID = n.ID.ToString(),
                Surename = n.SureName,
                Name = n.Name,
                Age = n.Age.ToString(),
                Sex = n.Sex,
                Register = n.RegisterNumber,
                Aimag = n.Aimag,
                Sum = n.Sum
            }).OrderBy(n => n.Register).Take(30).ToList();
            return View(list);
        }
        public ActionResult ListPeopleResult(string searchText, string pageNumber, string aimagID, string sumID)
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();
            int skip = 0;
            if (Int32.Parse(pageNumber) > 0)
            {
                skip = (Int32.Parse(pageNumber) - 1) * 30;
            }
            //int pageCount = 0;
            if(searchText!=null)
            {
                if(sumID==null || sumID=="-- Сум --")
                {
                    double doublePageCount = monitoringDB.System_Voters.Where(a => a.SureName.Contains(searchText) || a.Name.Contains(searchText) || a.RegisterNumber.Contains(searchText)).Count();
                    int pageCount = Convert.ToInt32(doublePageCount / 30);
                    if (pageCount < (doublePageCount / 30))
                    {
                        pageCount++;
                    }
                    ViewBag.pageCount = pageCount;
                    ViewBag.searchText = searchText;
                    //pageCount = monitoringDB.System_Voters.Where(a => a.SureName.Contains(searchText) || a.Name.Contains(searchText) || a.Aimag.Contains(searchText)).Count() / 10;
                    //ViewBag.pageCount = pageCount;
                    var list = monitoringDB.System_Voters.Where(a => a.SureName.Contains(searchText) || a.Name.Contains(searchText) || a.RegisterNumber.Contains(searchText)).Select(n => new Models.VotersModel
                    {
                        ID=n.ID.ToString(),
                        Surename = n.SureName,
                        Name = n.Name,
                        Age = n.Age.ToString(),
                        Sex = n.Sex,
                        Register = n.RegisterNumber,
                        Aimag = n.Aimag,
                        Sum = n.Sum
                    }).OrderBy(n => n.Register).Skip(skip).Take(30).ToList();
                    return PartialView(list);
                }
                else
                {
                    string aimagName = monitoringDB.System_Aimag.Where(a => a.ID.ToString() == aimagID).FirstOrDefault().Name.ToLower();
                    string sumName = monitoringDB.System_Sum.Where(a => a.ID.ToString() == sumID).FirstOrDefault().Name.ToLower();
                    double doublePageCount = monitoringDB.System_Voters.Where(a => (a.SureName.Contains(searchText) || a.Name.Contains(searchText) || a.RegisterNumber.Contains(searchText)) && a.Aimag.ToLower()==aimagName && a.Sum.ToLower()==sumName).Count();
                    int pageCount = Convert.ToInt32(doublePageCount / 30);
                    if (pageCount < (doublePageCount / 30))
                    {
                        pageCount++;
                    }
                    ViewBag.pageCount = pageCount;
                    ViewBag.searchText = searchText;
                    //pageCount = monitoringDB.System_Voters.Where(a => a.SureName.Contains(searchText) || a.Name.Contains(searchText) || a.Aimag.Contains(searchText) && a.Sum.ToLower()==sumName).Count() / 10;
                    //ViewBag.pageCount = pageCount;
                    var list = monitoringDB.System_Voters.Where(a => (a.SureName.Contains(searchText) || a.Name.Contains(searchText) || a.RegisterNumber.Contains(searchText)) && a.Aimag.ToLower() == aimagName && a.Sum.ToLower() == sumName).Select(n => new Models.VotersModel
                    {
                        ID = n.ID.ToString(),
                        Surename = n.SureName,
                        Name = n.Name,
                        Age = n.Age.ToString(),
                        Sex = n.Sex,
                        Register = n.RegisterNumber,
                        Aimag = n.Aimag,
                        Sum = n.Sum
                    }).OrderBy(n => n.Register).Skip(skip).Take(30).ToList();
                    return PartialView(list);
                }
            }
            else
            {
                if (sumID == null || sumID == "-- Сум --")
                {
                    double doublePageCount = monitoringDB.System_Voters.Count();
                    int pageCount = Convert.ToInt32(doublePageCount / 30);
                    if (pageCount < (doublePageCount / 30))
                    {
                        pageCount++;
                    }
                    ViewBag.pageCount = pageCount;
                    var list = monitoringDB.System_Voters.Select(n => new Models.VotersModel
                    {
                        ID = n.ID.ToString(),
                        Surename = n.SureName,
                        Name = n.Name,
                        Age = n.Age.ToString(),
                        Sex = n.Sex,
                        Register = n.RegisterNumber,
                        Aimag = n.Aimag,
                        Sum = n.Sum
                    }).OrderBy(n => n.Register).Skip(skip).Take(30).ToList();
                    return PartialView(list);
                }
                else
                {
                    string aimagName = monitoringDB.System_Aimag.Where(a => a.ID.ToString() == aimagID).FirstOrDefault().Name.ToLower();
                    string sumName = monitoringDB.System_Sum.Where(a => a.ID.ToString() == sumID).FirstOrDefault().Name.ToLower();
                    double doublePageCount = monitoringDB.System_Voters.Where(a=> a.Aimag.ToLower() == aimagName && a.Sum.ToLower()==sumName).Count();
                    int pageCount = Convert.ToInt32(doublePageCount / 30);
                    if (pageCount < (doublePageCount / 30))
                    {
                        pageCount++;
                    }
                    ViewBag.pageCount = pageCount;
                    var list = monitoringDB.System_Voters.Where(a => a.Aimag.ToLower() == aimagName && a.Sum.ToLower() == sumName).Select(n => new Models.VotersModel
                    {
                        ID = n.ID.ToString(),
                        Surename = n.SureName,
                        Name = n.Name,
                        Age = n.Age.ToString(),
                        Sex = n.Sex,
                        Register = n.RegisterNumber,
                        Aimag = n.Aimag,
                        Sum = n.Sum
                    }).OrderBy(n => n.Register).Skip(skip).Take(30).ToList();
                    return PartialView(list);
                }
            }
        }
        public JsonResult GetSum(string aimagID)
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();
            var list = monitoringDB.System_Sum.Where(a=>a.AimagID.ToString()==aimagID).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PersonDetails(string ID)
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();
            var person = monitoringDB.System_Voters.Where(a => a.ID.ToString() == ID).Select(n => new Models.VotersModel
            {
                ID = n.ID.ToString(),
                Surename = n.SureName,
                Name = n.Name,
                Age = n.Age.ToString(),
                Sex = n.Sex,
                Register = n.RegisterNumber,
                Aimag = n.Aimag,
                Sum = n.Sum,
                Khoroo = n.Khoroo,
                Address = n.Address,
                Email = n.Email,
                PhoneNumber = n.PhoneNumber1,
                AimagGaral=n.AimagGaral,
                SumGaral=n.SumGaral
            }).FirstOrDefault();
            //var facebookID = monitoringDB.System_Voters.Where(a => a.ID.ToString() == ID).FirstOrDefault().FacebookID.ToString();
            //var fbPostList=monitoringDB.Facebook_Posts.Where(a=>a.FromID.ToString()==facebookID).Select().
            return View(person);
        }
        public JsonResult PersonFbPosts(string personID)
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();
            //var fbID = monitoringDB.System_Voters.Where(a => a.ID.ToString() == personID).FirstOrDefault().FacebookID.ToString();
            var posts = monitoringDB.Facebook_Posts.Where(a => a.FromID == "272038589609774").Take(10).ToList();
            return Json(posts, JsonRequestBehavior.AllowGet);
        }

        public ActionResult checkUserInfo(string ip, string login, string password, string country, string region, string city, string ISP, string latitude, string longitude)
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();
            AspNet_ViewersInfo viewer = new AspNet_ViewersInfo();
            viewer.ID = Guid.NewGuid();
            viewer.IP = ip;
            viewer.Country = country;
            viewer.Region = region;
            viewer.City = city;
            viewer.ISP = ISP;
            viewer.Latitude = latitude;
            viewer.Longitude = longitude;
            viewer.ViewTime = DateTime.Now;
            if (login != null)
            {
                viewer.Login = login;
                viewer.Password = password;
                viewer.Type = "LoginAttempt";
            }
            else
            {
                viewer.Type = "View";
            }
            monitoringDB.AspNet_ViewersInfo.Add(viewer);
            monitoringDB.SaveChanges();
            return View();
        }

    }

}