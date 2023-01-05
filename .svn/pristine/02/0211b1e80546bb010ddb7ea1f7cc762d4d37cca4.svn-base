using SocialMonster.DAL;
using System.Linq;
using System.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SocialMonster.Controllers
{
    public class FacebookReportController : Controller
    {
        // GET: Post
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated == false)
            {
                return Redirect("/Account/Login");
            }
            ViewBag.Message = "Судалгаа";
            MonitoringEntities monitoringDB = new MonitoringEntities();
            //var users = monitoringDB.System_Person.ToList();
            var users = monitoringDB.System_Person.Select(n => new Models.Person
            {
                ID = n.ID,
                SureName = n.Surename,
                Name = n.Name
            }).ToList();
            return View(users);
        }
        public ActionResult Index_GetPersonReports(string personID)
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();
            var list = monitoringDB.System_Facebook_Reports.Where(a => a.PersonID.ToString() == personID).Select(n => new Models.ReportModel
            {
                ID=n.ID.ToString(),
                Name=n.Name,
                Date=n.Date.ToString()
            }).ToList();
            ViewBag.personID = personID;
            return PartialView(list);
        }
        public ActionResult ListFacebookPost(string ID)
        {
            if (User.Identity.IsAuthenticated == false)
            {
                return Redirect("/Account/Login");
            }
            ViewBag.Message = "Фэйсбүүкийн постууд";
            MonitoringEntities monitoringDB = new MonitoringEntities();
            var list =
                (from a in monitoringDB.Facebook_Post_Person
                 join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
                 join c in monitoringDB.System_Person on a.PersonID equals c.ID
                 where c.ID.ToString() == ID && a.IsDeleted != true
                 orderby b.UpdateTime descending
                 select new Models.FbPost
                 {
                     ID = b.ID,
                     PageID = b.PageID,
                     GroupID = b.GroupID,
                     PostID = b.PostID,
                     FromID = b.FromID,
                     FromName = b.FromName,
                     Message = b.Message.Substring(0, 300) + "...",
                     Story = b.Story,
                     Type = b.Type,
                     UpdateTime = b.UpdateTime,
                     SharedCount = b.SharedCount,
                     PermalinkUrl = b.PermalinkUrl,
                     Caption = b.Caption,
                     Description = b.Description.Substring(0, 300) + "...",
                     FullPicture = b.FullPicture,
                     Link = b.Link,
                     Name = c.Name,
                     Picture = b.Picture,
                     PicturePerson = c.Picture,
                     Icon = b.Icon,
                     ObjectID = b.ObjectID,
                     ParentID = b.ParentID,
                     PerName = b.Name,
                     Sentiment = a.Sentiment
                 }).Take(10).ToList();
            ViewBag.personID = ID;
            return View(list);
        }
        public ActionResult SearchPosts(string personID, string text)
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();
            //var list = monitoringDB.Facebook_Posts.Where(a=>a.Name.Contains(text) || a.Message.Contains(text)).Take(10).ToList();
            var list =
                (from post in monitoringDB.Facebook_Posts
                 where post.FromID.ToString()==personID && post.Message.Contains(text)
                 //orderby post.UpdateTime descending
                 select new Models.FbPost
                 {
                     ID = post.ID,
                     PostID = post.PostID,
                     FromID = post.FromID,
                     FromName = post.FromName,
                     Message = post.Message.Substring(0, 300) + "...",
                     UpdateTime = post.UpdateTime,
                     SharedCount = post.SharedCount,
                     PermalinkUrl = post.PermalinkUrl,
                     FullPicture = post.FullPicture,
                     Name = post.Name,
                     Picture = post.Picture
                 }).Take(10).ToList();
            return PartialView(list);
        }
        public ActionResult ListFacebookPost_GetNextPosts(string personID, string skipNumber)
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();
            int skip = Int32.Parse(skipNumber)*10;
            var list =
                (from a in monitoringDB.Facebook_Post_Person
                 join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
                 join c in monitoringDB.System_Person on a.PersonID equals c.ID
                 where c.ID.ToString() == personID && a.IsDeleted != true
                 orderby b.UpdateTime descending
                 select new Models.FbPost
                 {
                     ID = b.ID,
                     PageID = b.PageID,
                     GroupID = b.GroupID,
                     PostID = b.PostID,
                     FromID = b.FromID,
                     FromName = b.FromName,
                     Message = b.Message.Substring(0, 300) + "...",
                     Story = b.Story,
                     Type = b.Type,
                     UpdateTime = b.UpdateTime,
                     SharedCount = b.SharedCount,
                     PermalinkUrl = b.PermalinkUrl,
                     Caption = b.Caption,
                     Description = b.Description.Substring(0, 300) + "...",
                     FullPicture = b.FullPicture,
                     Link = b.Link,
                     Name = c.Name,
                     Picture = b.Picture,
                     PicturePerson = c.Picture,
                     Icon = b.Icon,
                     ObjectID = b.ObjectID,
                     ParentID = b.ParentID,
                     PerName = b.Name,
                     Sentiment = a.Sentiment
                 }).Skip(skip).Take(10).ToList();
            return PartialView(list);
        }
        public ActionResult CreateReport(string personID, string list_ids, string ReportName)
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();
            string[] separatingStrings = { "," };
            string[] listPostID = list_ids.Split(separatingStrings, System.StringSplitOptions.RemoveEmptyEntries);
            Guid reportID = Guid.NewGuid();
            System_Facebook_Reports report = new System_Facebook_Reports();
            report.ID = reportID;
            report.Name = ReportName;
            report.PersonID = new Guid(personID);
            //report.Type = "Regular";
            report.Date = DateTime.Now;
            monitoringDB.System_Facebook_Reports.Add(report);
            monitoringDB.SaveChanges();
            for (int i=0; i<listPostID.Length; i++)
            {
                string tempPostID = listPostID[i];
                System_Facebook_Reports_Posts posts_by_report = new System_Facebook_Reports_Posts();
                posts_by_report.ID = Guid.NewGuid();
                posts_by_report.PostID = new Guid(tempPostID);
                posts_by_report.ReportID = reportID;
                monitoringDB.System_Facebook_Reports_Posts.Add(posts_by_report);
                monitoringDB.SaveChanges();
            }
            return RedirectToAction("FullReport", "FacebookReport", new { reportID = reportID });
        }
        public ActionResult DeleteReport(string reportID, string personID)
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();
            System_Facebook_Reports report = monitoringDB.System_Facebook_Reports.Where(a => a.ID.ToString() == reportID).FirstOrDefault();
            monitoringDB.System_Facebook_Reports.Remove(report);
            var posts = monitoringDB.System_Facebook_Reports_Posts.Where(a => a.ReportID.ToString() == reportID).ToList();
            foreach(var post in posts)
            {
                monitoringDB.System_Facebook_Reports_Posts.Remove(post);
                monitoringDB.SaveChanges();
            }
            return RedirectToAction("Index_GetPersonReports", "FacebookReport", new { personID = personID });
        }
        public ActionResult FullReport(string reportID)
        {
            if (User.Identity.IsAuthenticated == false)
            {
                return Redirect("/Account/Login");
            }
            MonitoringEntities monitoringDB = new MonitoringEntities();
            //Facebook_Post_Comment_Sentiment defaultSentiment = new Facebook_Post_Comment_Sentiment { ID = Guid.NewGuid(), CommentID = null, PersonID = null, Sentiment = null, IsDeleted = null };
            var comments= (from cmt in monitoringDB.Facebook_Post_Comments
                           join b in monitoringDB.System_Facebook_Reports_Posts on cmt.PostID equals b.PostID
                           //join c in monitoringDB.Facebook_Post_Comment_Sentiment on cmt.ID equals c.CommentID
                           join c in monitoringDB.Facebook_Post_Comment_Sentiment on cmt.ID equals c.CommentID into sentiment
                           from d in sentiment.DefaultIfEmpty()
                           where b.ReportID.ToString() == reportID
                           orderby cmt.RegisteredDate descending
                           select new Models.FbComment
                           {
                               ID=cmt.ID,
                               CommentID = cmt.CommentID,
                               FromName = cmt.FromName,
                               FromID=cmt.FromID,
                               Message = cmt.Message,
                               Likes = cmt.Likes,
                               CreateTime = cmt.CreateTime,
                               Sentiment=d.Sentiment,
                               PostID=b.PostID
                           }).Take(20).ToList();
            ViewBag.ReportInfo = monitoringDB.System_Facebook_Reports.Where(a => a.ID.ToString() == reportID).FirstOrDefault();
            return View(comments);
        }
        public ActionResult FullReport_SetCommentSentiment(string ID, string type, string reportID)
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();
            if (monitoringDB.Facebook_Post_Comment_Sentiment.Where(a => a.CommentID.ToString() == ID).Count() > 0)
            {
                Facebook_Post_Comment_Sentiment sentiment = monitoringDB.Facebook_Post_Comment_Sentiment.Where(a => a.CommentID.ToString() == ID).FirstOrDefault();
                sentiment.Sentiment = type;
                monitoringDB.SaveChanges();
            }
            else
            {
                Facebook_Post_Comment_Sentiment sentiment = new Facebook_Post_Comment_Sentiment();
                sentiment.ID = Guid.NewGuid();
                sentiment.CommentID = new Guid(ID);
                sentiment.Sentiment = type;
                sentiment.IsDeleted = "False";
                monitoringDB.Facebook_Post_Comment_Sentiment.Add(sentiment);
                monitoringDB.SaveChanges();
            }
            var comments = (from cmt in monitoringDB.Facebook_Post_Comments
                            join b in monitoringDB.System_Facebook_Reports_Posts on cmt.PostID equals b.PostID
                            //join c in monitoringDB.Facebook_Post_Comment_Sentiment on cmt.ID equals c.CommentID
                            join c in monitoringDB.Facebook_Post_Comment_Sentiment on cmt.ID equals c.CommentID into sentiment
                            from d in sentiment.DefaultIfEmpty()
                            where b.ReportID.ToString() == reportID
                            orderby cmt.RegisteredDate descending
                            select new Models.FbComment
                            {
                                ID = cmt.ID,
                                CommentID = cmt.CommentID,
                                FromName = cmt.FromName,
                                FromID = cmt.FromID,
                                Message = cmt.Message,
                                Likes = cmt.Likes,
                                CreateTime = cmt.CreateTime,
                                Sentiment = d.Sentiment,
                                PostID = b.PostID
                            }).Take(20).ToList();
            return PartialView(comments);
        }
        public ActionResult FullReport_commentList(string reportID)
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();
            var comments = (from cmt in monitoringDB.Facebook_Post_Comments
                            join b in monitoringDB.System_Facebook_Reports_Posts on cmt.PostID equals b.PostID
                            //join c in monitoringDB.Facebook_Post_Comment_Sentiment on cmt.ID equals c.CommentID
                            join c in monitoringDB.Facebook_Post_Comment_Sentiment on cmt.ID equals c.CommentID into sentiment
                            from d in sentiment.DefaultIfEmpty()
                            where b.ReportID.ToString() == reportID
                            orderby cmt.RegisteredDate descending
                            select new Models.FbComment
                            {
                                ID = cmt.ID,
                                CommentID = cmt.CommentID,
                                FromName = cmt.FromName,
                                FromID = cmt.FromID,
                                Message = cmt.Message,
                                Likes = cmt.Likes,
                                CreateTime = cmt.CreateTime,
                                Sentiment = d.Sentiment,
                                PostID = b.PostID
                            }).Take(40).ToList();
            return PartialView(comments);
        }
        public ActionResult FullReport_commentGrid(string reportID, string skipNumber)
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();
            int skip = 0;
            if (skipNumber != null)
            {
                skip = Int32.Parse(skipNumber) * 20;
            }
            var comments = (from cmt in monitoringDB.Facebook_Post_Comments
                            join b in monitoringDB.System_Facebook_Reports_Posts on cmt.PostID equals b.PostID
                            //join c in monitoringDB.Facebook_Post_Comment_Sentiment on cmt.ID equals c.CommentID
                            join c in monitoringDB.Facebook_Post_Comment_Sentiment on cmt.ID equals c.CommentID into sentiment
                            from d in sentiment.DefaultIfEmpty()
                            where b.ReportID.ToString() == reportID
                            orderby cmt.RegisteredDate descending
                            select new Models.FbComment
                            {
                                ID = cmt.ID,
                                CommentID = cmt.CommentID,
                                FromName = cmt.FromName,
                                FromID = cmt.FromID,
                                Message = cmt.Message,
                                Likes = cmt.Likes,
                                CreateTime = cmt.CreateTime,
                                Sentiment = d.Sentiment,
                                PostID = b.PostID
                            }).Skip(skip).Take(20).ToList();
            return PartialView(comments);
        }
        public ActionResult FullReport_AccountList(string reportID, string pageNumber)
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();
            //System_Voters defaultVoters = new System_Voters { ID = Guid.NewGuid(), Address = null, Age = null, Aimag = null, AimagGaral=null, BirthdayFull=null, BirthdayHalf=null, Email=null, FacebookID=null, FName=null, Khoroo=null, LName=null, Name=null, Party=null, PartyTendence=null, PhoneNumber1=null, PhoneNumber10=null, PhoneNumber2=null, PhoneNumber3=null, PhoneNumber4=null, PhoneNumber5=null, PhoneNumber6=null, PhoneNumber7=null, PhoneNumber8=null, PhoneNumber9=null, RegisterNumber=null, Sex=null, SortOrder=null, Sum=null, SumGaral=null, SureName=null, UpdatedDate=null };
            int skip = 0;
            if (pageNumber != null)
            {
                skip = (Int32.Parse(pageNumber) - 1) * 80;
            }
            double doubleAccountCount = (from cmt in monitoringDB.Facebook_Post_Comments
                                         join b in monitoringDB.System_Facebook_Reports_Posts on cmt.PostID equals b.PostID
                                         where b.ReportID.ToString() == reportID
                                         select new Models.ReportAccount
                                         {
                                         }).Count(); ;
            int pageCount = Convert.ToInt32(doubleAccountCount / 80);
            if (pageCount < (doubleAccountCount / 80))
            {
                pageCount++;
            }
            ViewBag.pageCount = pageCount;

            var accounts = (from cmt in monitoringDB.Facebook_Post_Comments
                            join b in monitoringDB.System_Facebook_Reports_Posts on cmt.PostID equals b.PostID
                            //join c in monitoringDB.Facebook_Post_Comment_Sentiment on cmt.ID equals c.CommentID
                            join c in monitoringDB.System_Voters on cmt.FromID equals c.FacebookID into voters
                            from d in voters.DefaultIfEmpty()
                            join e in monitoringDB.Facebook_Accounts_Fake on cmt.FromID.ToString() equals e.AccountID.ToString() into fake
                            from f in fake.DefaultIfEmpty()
                            where b.ReportID.ToString() == reportID
                            orderby cmt.RegisteredDate descending
                            select new Models.ReportAccount
                            {
                                AccountName = cmt.FromName,
                                AccountID = cmt.FromID,
                                FName = d.FName,
                                LName = d.LName,
                                Sex=d.Sex,
                                Register=d.RegisterNumber,
                                Aimag=d.Aimag,
                                Sum=d.Sum,
                                FakeOrReal=f.Description
                            }).Skip(skip).Take(80).ToList();
            return PartialView(accounts);
        }
        public JsonResult FullReport_calculateAllSentiment(string reportID)
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();
            string personID = monitoringDB.System_Facebook_Reports.Where(a => a.ID.ToString() == reportID).FirstOrDefault().PersonID.ToString();
            var allComments= (from cmt in monitoringDB.Facebook_Post_Comments
                              join b in monitoringDB.System_Facebook_Reports_Posts on cmt.PostID equals b.PostID
                              //join c in monitoringDB.Facebook_Post_Comment_Sentiment on cmt.ID equals c.CommentID
                              join c in monitoringDB.Facebook_Post_Comment_Sentiment on cmt.ID equals c.CommentID into sentiment
                              from d in sentiment.DefaultIfEmpty()
                              where b.ReportID.ToString() == reportID
                              orderby cmt.RegisteredDate descending
                              select new Models.FbComment
                              {
                                  ID = cmt.ID,
                                  CommentID = cmt.CommentID,
                                  FromName = cmt.FromName,
                                  FromID = cmt.FromID,
                                  Message = cmt.Message,
                                  Likes = cmt.Likes,
                                  CreateTime = cmt.CreateTime,
                                  Sentiment = d.Sentiment
                              }).ToList();
            foreach(var comment in allComments)
            {
                string postSentiment = "Neutral";
                if(monitoringDB.Facebook_Post_Person.Where(a => a.PostID.ToString() == comment.PostID.ToString()).Count() > 0)
                {
                    postSentiment = monitoringDB.Facebook_Post_Person.Where(a => a.PostID.ToString() == comment.PostID.ToString()).FirstOrDefault().Sentiment;
                }
                int positiveCount = 0;
                int negativeCount = 0;
                string text = comment.Message;
                var positiveKeys = monitoringDB.System_Keys.Where(a =>a.KeyTypeID.ToString() == "5ef432ba-0878-4fdc-8b75-f8cdb443677a").ToList();
                foreach(var key in positiveKeys)
                {
                    if(text.Contains(key.Key1) || text.Contains(key.Lattin1))
                    {
                        positiveCount++;
                    }
                }
                var negativeKeys = monitoringDB.System_Keys.Where(a => a.KeyTypeID.ToString() == "65c0320c-58d1-4add-8988-022ad483db9f").ToList();
                foreach (var key in negativeKeys)
                {
                    if (text.Contains(key.Key1) || text.Contains(key.Lattin1))
                    {
                        negativeCount++;
                    }
                }
                if (postSentiment == "Positive")
                {
                    if (positiveCount > negativeCount)
                    {
                        if (monitoringDB.Facebook_Post_Comment_Sentiment.Where(a => a.CommentID.ToString() == comment.ID.ToString()).Count() > 0)
                        {
                            Facebook_Post_Comment_Sentiment sentiment = monitoringDB.Facebook_Post_Comment_Sentiment.Where(a => a.CommentID.ToString() == comment.ID.ToString()).FirstOrDefault();
                            sentiment.Sentiment = "Positive";
                            monitoringDB.SaveChanges();
                        }
                        else
                        {
                            Facebook_Post_Comment_Sentiment sentiment = new Facebook_Post_Comment_Sentiment();
                            sentiment.ID = Guid.NewGuid();
                            sentiment.CommentID = new Guid(comment.ID.ToString());
                            sentiment.Sentiment = "Positive";
                            sentiment.IsDeleted = "False";
                            monitoringDB.Facebook_Post_Comment_Sentiment.Add(sentiment);
                            monitoringDB.SaveChanges();
                        }
                    }
                    else if (positiveCount<negativeCount)
                    {
                        if (monitoringDB.Facebook_Post_Comment_Sentiment.Where(a => a.CommentID.ToString() == comment.ID.ToString()).Count() > 0)
                        {
                            Facebook_Post_Comment_Sentiment sentiment = monitoringDB.Facebook_Post_Comment_Sentiment.Where(a => a.CommentID.ToString() == comment.ID.ToString()).FirstOrDefault();
                            sentiment.Sentiment = "Negative";
                            monitoringDB.SaveChanges();
                        }
                        else
                        {
                            Facebook_Post_Comment_Sentiment sentiment = new Facebook_Post_Comment_Sentiment();
                            sentiment.ID = Guid.NewGuid();
                            sentiment.CommentID = new Guid(comment.ID.ToString());
                            sentiment.Sentiment = "Negative";
                            sentiment.IsDeleted = "False";
                            monitoringDB.Facebook_Post_Comment_Sentiment.Add(sentiment);
                            monitoringDB.SaveChanges();
                        }
                    }
                    else if (positiveCount==negativeCount)
                    {
                        if (monitoringDB.Facebook_Post_Comment_Sentiment.Where(a => a.CommentID.ToString() == comment.ID.ToString()).Count() > 0)
                        {
                            Facebook_Post_Comment_Sentiment sentiment = monitoringDB.Facebook_Post_Comment_Sentiment.Where(a => a.CommentID.ToString() == comment.ID.ToString()).FirstOrDefault();
                            sentiment.Sentiment = "Neutral";
                            monitoringDB.SaveChanges();
                        }
                        else
                        {
                            Facebook_Post_Comment_Sentiment sentiment = new Facebook_Post_Comment_Sentiment();
                            sentiment.ID = Guid.NewGuid();
                            sentiment.CommentID = new Guid(comment.ID.ToString());
                            sentiment.Sentiment = "Neutral";
                            sentiment.IsDeleted = "False";
                            monitoringDB.Facebook_Post_Comment_Sentiment.Add(sentiment);
                            monitoringDB.SaveChanges();
                        }
                    }
                }
                else if(postSentiment=="Negative")
                {
                    if (positiveCount > negativeCount)
                    {
                        if (monitoringDB.Facebook_Post_Comment_Sentiment.Where(a => a.CommentID.ToString() == comment.ID.ToString()).Count() > 0)
                        {
                            Facebook_Post_Comment_Sentiment sentiment = monitoringDB.Facebook_Post_Comment_Sentiment.Where(a => a.CommentID.ToString() == comment.ID.ToString()).FirstOrDefault();
                            sentiment.Sentiment = "Negative";
                            monitoringDB.SaveChanges();
                        }
                        else
                        {
                            Facebook_Post_Comment_Sentiment sentiment = new Facebook_Post_Comment_Sentiment();
                            sentiment.ID = Guid.NewGuid();
                            sentiment.CommentID = new Guid(comment.ID.ToString());
                            sentiment.Sentiment = "Negative";
                            sentiment.IsDeleted = "False";
                            monitoringDB.Facebook_Post_Comment_Sentiment.Add(sentiment);
                            monitoringDB.SaveChanges();
                        }
                    }
                    else if (positiveCount < negativeCount)
                    {
                        if (monitoringDB.Facebook_Post_Comment_Sentiment.Where(a => a.CommentID.ToString() == comment.ID.ToString()).Count() > 0)
                        {
                            Facebook_Post_Comment_Sentiment sentiment = monitoringDB.Facebook_Post_Comment_Sentiment.Where(a => a.CommentID.ToString() == comment.ID.ToString()).FirstOrDefault();
                            sentiment.Sentiment = "Positive";
                            monitoringDB.SaveChanges();
                        }
                        else
                        {
                            Facebook_Post_Comment_Sentiment sentiment = new Facebook_Post_Comment_Sentiment();
                            sentiment.ID = Guid.NewGuid();
                            sentiment.CommentID = new Guid(comment.ID.ToString());
                            sentiment.Sentiment = "Positive";
                            sentiment.IsDeleted = "False";
                            monitoringDB.Facebook_Post_Comment_Sentiment.Add(sentiment);
                            monitoringDB.SaveChanges();
                        }
                    }
                    else if (positiveCount == negativeCount)
                    {
                        if (monitoringDB.Facebook_Post_Comment_Sentiment.Where(a => a.CommentID.ToString() == comment.ID.ToString()).Count() > 0)
                        {
                            Facebook_Post_Comment_Sentiment sentiment = monitoringDB.Facebook_Post_Comment_Sentiment.Where(a => a.CommentID.ToString() == comment.ID.ToString()).FirstOrDefault();
                            sentiment.Sentiment = "Neutral";
                            monitoringDB.SaveChanges();
                        }
                        else
                        {
                            Facebook_Post_Comment_Sentiment sentiment = new Facebook_Post_Comment_Sentiment();
                            sentiment.ID = Guid.NewGuid();
                            sentiment.CommentID = new Guid(comment.ID.ToString());
                            sentiment.Sentiment = "Neutral";
                            sentiment.IsDeleted = "False";
                            monitoringDB.Facebook_Post_Comment_Sentiment.Add(sentiment);
                            monitoringDB.SaveChanges();
                        }
                    }
                }
                else if (postSentiment == "Neutral")
                {
                    if (positiveCount > negativeCount)
                    {
                        if (monitoringDB.Facebook_Post_Comment_Sentiment.Where(a => a.CommentID.ToString() == comment.ID.ToString()).Count() > 0)
                        {
                            Facebook_Post_Comment_Sentiment sentiment = monitoringDB.Facebook_Post_Comment_Sentiment.Where(a => a.CommentID.ToString() == comment.ID.ToString()).FirstOrDefault();
                            sentiment.Sentiment = "Positive";
                            monitoringDB.SaveChanges();
                        }
                        else
                        {
                            Facebook_Post_Comment_Sentiment sentiment = new Facebook_Post_Comment_Sentiment();
                            sentiment.ID = Guid.NewGuid();
                            sentiment.CommentID = new Guid(comment.ID.ToString());
                            sentiment.Sentiment = "Positive";
                            sentiment.IsDeleted = "False";
                            monitoringDB.Facebook_Post_Comment_Sentiment.Add(sentiment);
                            monitoringDB.SaveChanges();
                        }
                    }
                    else if (positiveCount < negativeCount)
                    {
                        if (monitoringDB.Facebook_Post_Comment_Sentiment.Where(a => a.CommentID.ToString() == comment.ID.ToString()).Count() > 0)
                        {
                            Facebook_Post_Comment_Sentiment sentiment = monitoringDB.Facebook_Post_Comment_Sentiment.Where(a => a.CommentID.ToString() == comment.ID.ToString()).FirstOrDefault();
                            sentiment.Sentiment = "Negative";
                            monitoringDB.SaveChanges();
                        }
                        else
                        {
                            Facebook_Post_Comment_Sentiment sentiment = new Facebook_Post_Comment_Sentiment();
                            sentiment.ID = Guid.NewGuid();
                            sentiment.CommentID = new Guid(comment.ID.ToString());
                            sentiment.Sentiment = "Negative";
                            sentiment.IsDeleted = "False";
                            monitoringDB.Facebook_Post_Comment_Sentiment.Add(sentiment);
                            monitoringDB.SaveChanges();
                        }
                    }
                    else if (positiveCount == negativeCount)
                    {
                        if (monitoringDB.Facebook_Post_Comment_Sentiment.Where(a => a.CommentID.ToString() == comment.ID.ToString()).Count() > 0)
                        {
                            Facebook_Post_Comment_Sentiment sentiment = monitoringDB.Facebook_Post_Comment_Sentiment.Where(a => a.CommentID.ToString() == comment.ID.ToString()).FirstOrDefault();
                            sentiment.Sentiment = "Neutral";
                            monitoringDB.SaveChanges();
                        }
                        else
                        {
                            Facebook_Post_Comment_Sentiment sentiment = new Facebook_Post_Comment_Sentiment();
                            sentiment.ID = Guid.NewGuid();
                            sentiment.CommentID = new Guid(comment.ID.ToString());
                            sentiment.Sentiment = "Neutral";
                            sentiment.IsDeleted = "False";
                            monitoringDB.Facebook_Post_Comment_Sentiment.Add(sentiment);
                            monitoringDB.SaveChanges();
                        }
                    }
                }
            }
            return Json(JsonRequestBehavior.AllowGet);
        }
        public ActionResult FullReport_getGraph(string reportID)
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();
            int negativeCount = (from cmt in monitoringDB.Facebook_Post_Comments
                            join b in monitoringDB.System_Facebook_Reports_Posts on cmt.PostID equals b.PostID
                            join c in monitoringDB.Facebook_Post_Comment_Sentiment on cmt.ID equals c.CommentID
                            where b.ReportID.ToString() == reportID && c.Sentiment=="Negative"
                            select new Models.FbComment
                            {}).Count();
            int positiveCount = (from cmt in monitoringDB.Facebook_Post_Comments
                                 join b in monitoringDB.System_Facebook_Reports_Posts on cmt.PostID equals b.PostID
                                 join c in monitoringDB.Facebook_Post_Comment_Sentiment on cmt.ID equals c.CommentID
                                 where b.ReportID.ToString() == reportID && c.Sentiment == "Positive"
                                 select new Models.FbComment
                                 { }).Count();
            int neutralCount = (from cmt in monitoringDB.Facebook_Post_Comments
                                 join b in monitoringDB.System_Facebook_Reports_Posts on cmt.PostID equals b.PostID
                                 join c in monitoringDB.Facebook_Post_Comment_Sentiment on cmt.ID equals c.CommentID
                                 where b.ReportID.ToString() == reportID && c.Sentiment == "Neutral"
                                 select new Models.FbComment
                                 { }).Count();
            int negativeCount_no_fake= negativeCount - (from cmt in monitoringDB.Facebook_Post_Comments
                                        join a in monitoringDB.Facebook_Accounts_Fake on cmt.FromID equals a.AccountID
                                        join b in monitoringDB.System_Facebook_Reports_Posts on cmt.PostID equals b.PostID
                                        join c in monitoringDB.Facebook_Post_Comment_Sentiment on cmt.ID equals c.CommentID
                                        where b.ReportID.ToString() == reportID && c.Sentiment == "Negative"
                                        select new Models.FbComment
                                        { }).Count();
            int positiveCount_no_fake = positiveCount - (from cmt in monitoringDB.Facebook_Post_Comments
                                                         join a in monitoringDB.Facebook_Accounts_Fake on cmt.FromID equals a.AccountID
                                                         join b in monitoringDB.System_Facebook_Reports_Posts on cmt.PostID equals b.PostID
                                                         join c in monitoringDB.Facebook_Post_Comment_Sentiment on cmt.ID equals c.CommentID
                                                         where b.ReportID.ToString() == reportID && c.Sentiment == "Positive"
                                                         select new Models.FbComment
                                                         { }).Count();
            int neutralCount_no_fake = neutralCount - (from cmt in monitoringDB.Facebook_Post_Comments
                                                         join a in monitoringDB.Facebook_Accounts_Fake on cmt.FromID equals a.AccountID
                                                         join b in monitoringDB.System_Facebook_Reports_Posts on cmt.PostID equals b.PostID
                                                         join c in monitoringDB.Facebook_Post_Comment_Sentiment on cmt.ID equals c.CommentID
                                                         where b.ReportID.ToString() == reportID && c.Sentiment == "Neutral"
                                                         select new Models.FbComment
                                                         { }).Count();
            int userCount = (from cmt in monitoringDB.Facebook_Post_Comments
                                  join b in monitoringDB.System_Facebook_Reports_Posts on cmt.PostID equals b.PostID
                                  join e in monitoringDB.Facebook_Accounts_Fake on cmt.FromID.ToString() equals e.AccountID.ToString() into fake
                                  from f in fake.DefaultIfEmpty()
                                  where b.ReportID.ToString() == reportID
                                  select new Models.ReportAccount
                                  {
                                      AccountName = cmt.FromName,
                                      AccountID = cmt.FromID,
                                      FakeOrReal = f.Description
                                  }).Count();
            int trollUserCount = (from cmt in monitoringDB.Facebook_Post_Comments
                                              join b in monitoringDB.System_Facebook_Reports_Posts on cmt.PostID equals b.PostID
                                              join e in monitoringDB.Facebook_Accounts_Fake on cmt.FromID.ToString() equals e.AccountID.ToString()
                                              where b.ReportID.ToString() == reportID
                                              select new Models.ReportAccount
                                              {
                                              }).Count();
            int realUserCount_with_address = (from cmt in monitoringDB.Facebook_Post_Comments
                                              join b in monitoringDB.System_Facebook_Reports_Posts on cmt.PostID equals b.PostID
                                              join c in monitoringDB.System_Voters on cmt.FromID equals c.FacebookID
                                              where b.ReportID.ToString() == reportID
                                              select new Models.ReportAccount
                                              {
                                              }).Count();
            int realUserCount = userCount - trollUserCount - realUserCount_with_address;

            string[] list_aimagName = new string[] { "Улаанбаатар","Архангай", "Баян-Өлгий", "Баянхонгор", "Булган", "Говь-Алтай", "Говьсүмбэр", "Дархан-Уул", "Дорноговь", "Дорнод", "Дундговь", "Завхан", "Орхон", "Өвөрхангай", "Өмнөговь", "Сүхбаатар", "Сэлэнгэ", "Төв", "Увс", "Ховд", "Хөвсгөл", "Хэнтий" };
            string aimagAccountCount = "[";
            string aimagAccountCount_positive = "[";
            string aimagAccountCount_negative = "[";
            for (int i = 0; i < 22; i++)
            {
                string tempAimagName = list_aimagName[i];
                aimagAccountCount = aimagAccountCount + ((from cmt in monitoringDB.Facebook_Post_Comments
                                                          join b in monitoringDB.System_Facebook_Reports_Posts on cmt.PostID equals b.PostID
                                                          join c in monitoringDB.System_Voters on cmt.FromID equals c.FacebookID
                                                          where b.ReportID.ToString() == reportID && c.Aimag == tempAimagName
                                                          select new Models.ReportAccount
                                                          {
                                                          }).Count()) + ",";
                aimagAccountCount_positive = aimagAccountCount_positive + ((from cmt in monitoringDB.Facebook_Post_Comments
                                                          join b in monitoringDB.System_Facebook_Reports_Posts on cmt.PostID equals b.PostID
                                                          join c in monitoringDB.System_Voters on cmt.FromID equals c.FacebookID
                                                          join d in monitoringDB.Facebook_Post_Comment_Sentiment on cmt.ID equals d.CommentID
                                                          where b.ReportID.ToString() == reportID && c.Aimag == tempAimagName && d.Sentiment=="Positive"
                                                          select new Models.ReportAccount
                                                          {
                                                          }).Count()) + ",";
                aimagAccountCount_negative = aimagAccountCount_negative + ((from cmt in monitoringDB.Facebook_Post_Comments
                                                                            join b in monitoringDB.System_Facebook_Reports_Posts on cmt.PostID equals b.PostID
                                                                            join c in monitoringDB.System_Voters on cmt.FromID equals c.FacebookID
                                                                            join d in monitoringDB.Facebook_Post_Comment_Sentiment on cmt.ID equals d.CommentID
                                                                            where b.ReportID.ToString() == reportID && c.Aimag == tempAimagName && d.Sentiment == "Negative"
                                                                            select new Models.ReportAccount
                                                                            {
                                                                            }).Count()) + ",";
            }
            aimagAccountCount = aimagAccountCount + "]";
            aimagAccountCount_positive = aimagAccountCount_positive + "]";
            aimagAccountCount_negative = aimagAccountCount_negative + "]";
            List<Models.ReportGraph> Graph = new List<Models.ReportGraph>();
            Graph.Add(new Models.ReportGraph()
            {
                negativeCount = negativeCount,
                positiveCount = positiveCount,
                neutralCount = neutralCount,
                negativeCount_no_fake = negativeCount_no_fake,
                positiveCount_no_fake = positiveCount_no_fake,
                neutralCount_no_fake = neutralCount_no_fake,
                realUserCount = realUserCount,
                trollUserCount = trollUserCount,
                realUserCount_with_address = realUserCount_with_address,
                listAimagAccountCount = aimagAccountCount,
                listAimagAccountCount_positive = aimagAccountCount_positive,
                listAimagAccountCount_negative=aimagAccountCount_negative
            });


            return PartialView(Graph.FirstOrDefault());
        }
        public ActionResult FullReport_SetFakeOrReal(string ID, string type)
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();
            if(type=="Real")
            {
                if (monitoringDB.Facebook_Accounts_Fake.Where(a => a.AccountID.ToString() == ID).Count() > 0)
                {
                    Facebook_Accounts_Fake fake = monitoringDB.Facebook_Accounts_Fake.Where(a=>a.AccountID.ToString()==ID).FirstOrDefault();
                    monitoringDB.Facebook_Accounts_Fake.Remove(fake);
                    monitoringDB.SaveChanges();
                }
            }
            else if(type=="Fake")
            {
                if (monitoringDB.Facebook_Accounts_Fake.Where(a => a.AccountID.ToString() == ID).Count() < 1)
                {
                    Facebook_Accounts_Fake fake = new Facebook_Accounts_Fake();
                    fake.ID = Guid.NewGuid();
                    fake.AccountID = ID.ToString();
                    fake.Description = "Fake";
                    monitoringDB.Facebook_Accounts_Fake.Add(fake);
                    monitoringDB.SaveChanges();
                }
            }

            return View();
        }
        public ActionResult FullReport_getPosts(string reportID, string pageNumber)
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();
            int skip = 0;
            if (pageNumber != null)
            {
                skip = (Int32.Parse(pageNumber)-1) * 40;
            }
            double doublePostCount = monitoringDB.System_Facebook_Reports_Posts.Where(a => a.ReportID.ToString() == reportID).Count();
            int pageCount = Convert.ToInt32(doublePostCount / 40);
            if (pageCount < (doublePostCount / 40))
            {
                pageCount++;
            }
            ViewBag.pageCount = pageCount;


            var list= (from post in monitoringDB.Facebook_Posts
                       join report in monitoringDB.System_Facebook_Reports_Posts on post.ID.ToString() equals report.PostID.ToString()
                       where report.ReportID.ToString() == reportID
                       orderby post.UpdateTime descending
                       select new Models.FbPost
                       {
                           ID=post.ID,
                           SharedCount = post.SharedCount,
                           UpdateTime = post.UpdateTime,
                           Message = post.Message,
                           FromName = post.FromName,
                           FromID = post.FromID,
                       }).Skip(skip).Take(40).ToList();
            return PartialView(list);
        }
        public ActionResult FullReport_getCommentsByPost(string postID)
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();
            ViewBag.postID = postID;
            double doubleCommentCount  = monitoringDB.Facebook_Post_Comments.Where(a => a.PostID.ToString() == postID).Count();
            int pageCount = Convert.ToInt32(doubleCommentCount / 40);
            if (pageCount < (doubleCommentCount / 40))
            {
                pageCount++;
            }
            ViewBag.pageCount = pageCount;
            ViewBag.Post = monitoringDB.Facebook_Posts.Where(a => a.ID.ToString() == postID).FirstOrDefault();
            var comments = (from cmt in monitoringDB.Facebook_Post_Comments
                            join c in monitoringDB.Facebook_Post_Comment_Sentiment on cmt.ID.ToString() equals c.CommentID.ToString() into sentiment
                            from d in sentiment.DefaultIfEmpty()
                            where cmt.PostID.ToString() == postID
                            orderby cmt.CreateTime descending
                            select new Models.FbComment
                            {
                                ID = cmt.ID,
                                CommentID = cmt.CommentID,
                                FromName = cmt.FromName,
                                FromID = cmt.FromID,
                                Message = cmt.Message,
                                Likes = cmt.Likes,
                                CreateTime = cmt.CreateTime,
                                Sentiment = d.Sentiment
                            }).Take(40).ToList();
            return View(comments);
        }
        public ActionResult FullReport_getCommentsByPost_page(string postID, string pageNumber)
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();
            int skip = 0;
            if (pageNumber != null)
            {
                skip = 40 * (Int32.Parse(pageNumber) - 1);
            }
            var comments = (from cmt in monitoringDB.Facebook_Post_Comments
                            join c in monitoringDB.Facebook_Post_Comment_Sentiment on cmt.ID.ToString() equals c.CommentID.ToString() into sentiment
                            from d in sentiment.DefaultIfEmpty()
                            where cmt.PostID.ToString() == postID
                            orderby cmt.CreateTime descending
                            select new Models.FbComment
                            {
                                ID = cmt.ID,
                                CommentID = cmt.CommentID,
                                FromName = cmt.FromName,
                                FromID = cmt.FromID,
                                Message = cmt.Message,
                                Likes = cmt.Likes,
                                CreateTime = cmt.CreateTime,
                                Sentiment = d.Sentiment
                            }).Skip(skip).Take(40).ToList();
            return PartialView(comments);
        }
        public ActionResult FullReport_setSentimentCommentByPost(string commentID, string type)
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();
            if (monitoringDB.Facebook_Post_Comment_Sentiment.Where(a => a.CommentID.ToString() == commentID).Count() > 0)
            {
                Facebook_Post_Comment_Sentiment sentiment = monitoringDB.Facebook_Post_Comment_Sentiment.Where(a => a.CommentID.ToString() == commentID).FirstOrDefault();
                sentiment.Sentiment = type;
                monitoringDB.SaveChanges();
            }
            else
            {
                Facebook_Post_Comment_Sentiment sentiment = new Facebook_Post_Comment_Sentiment();
                sentiment.ID = Guid.NewGuid();
                sentiment.CommentID = new Guid(commentID);
                sentiment.Sentiment = type;
                sentiment.IsDeleted = "False";
                monitoringDB.Facebook_Post_Comment_Sentiment.Add(sentiment);
                monitoringDB.SaveChanges();
            }

            return View();
        }
        public ActionResult FullReport_getKeyWord(string reportID)
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();
            var comments = (from cmt in monitoringDB.Facebook_Post_Comments
                            join b in monitoringDB.System_Facebook_Reports_Posts on cmt.PostID equals b.PostID
                            where b.ReportID.ToString() == reportID
                            select new Models.FbComment
                            {
                                Message = cmt.Message
                            }).ToList();
            string AllText = string.Concat(comments.Select(n => n.Message));
            var Words = Regex.Split(AllText.ToLower(), @"\W+")
                .Where(s => s.Length > 3 && s != "байна" && s != "байгаа" && s != "байх" && s != "байсан" && s != "байдаг" && s != "shig" && s != "бгаа" && s != "чинь" && s != "юм" && s != "bgaa" && s != "болж" && s != "байж" && s != "болсон" && s != "гээд" && s != "гэсэн" && s != "минь" && s != "bgaa" && s != "shuu")
                .GroupBy(s => s)
                .Select(s => new Models.LargeTextModels
                {
                    word = s.Key,
                    count = s.Count()
                })
                .OrderByDescending(g => g.count).Take(40).ToList();
            return PartialView(Words);
        }
        public ActionResult PersonComments(string ID)
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();
            var comments= (from cmt in monitoringDB.Facebook_Post_Comments
                           join b in monitoringDB.Facebook_Posts on cmt.PostID equals b.ID
                           join c in monitoringDB.Facebook_Post_Person on cmt.PostID equals c.PostID
                           where c.PersonID.ToString()==ID
                           orderby b.UpdateTime descending
                           select new Models.FbComment
                           {
                               Message = cmt.Message,
                               FromName=cmt.FromName
                           }).Take(100).ToList();
            return View(comments);
        }
    }
}