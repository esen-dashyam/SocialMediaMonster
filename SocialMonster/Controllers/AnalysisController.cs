using SocialMonster.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SocialMonster.Controllers
{
    public class AnalysisController : Controller
    {
        // GET: Analysis
        public ActionResult Index(string id)
        {
            if (User.Identity.IsAuthenticated == false)
            {
                return Redirect("/Account/Login");
            }

            MonitoringEntities monitoring = new MonitoringEntities();
            var emailConfirmed = monitoring.AspNetUsers.Where(p => p.Email.Equals(User.Identity.Name)).FirstOrDefault().EmailConfirmed;
            if (!emailConfirmed)
            {
                var AuthenticationManager = HttpContext.GetOwinContext().Authentication;
                AuthenticationManager.SignOut();
                return Redirect("/Account/Login");
            }

            MonitoringEntities monitoringDB = new MonitoringEntities();
            int countPeople = monitoringDB.System_Person.Count();
            var view = monitoringDB.System_View.Where(x => x.Name.Equals(id)).FirstOrDefault();

            string[] peopleID = new string[3];
            List<DAL.System_Person> peopleData = new List<System_Person>();
            string[] peopleName = new string[3];

            string[] personName = new string[3];
            string[] personID = new string[3];

            //var allPeople = new List<string>();
            //var FbData =

            //foreach(var row in view)
            //{
            peopleID[0] = view.PersonID1.ToString();
            peopleID[1] = view.PersonID2.ToString();
            peopleID[2] = view.PersonID3.ToString();
            //}

            for (int i = 0; i < 3; i++)
            {
                string tmpPID = peopleID[i];
                var people = monitoringDB.System_Person.Where(s => s.ID.ToString().Equals(tmpPID)).FirstOrDefault();
                //personID[i] = people.ID.ToString();
                personName[i] = people.Surename.Substring(0, 1) + '.' + people.Name;
                peopleData.Add(people);
                //allPeople.Add(peopleData[i]);
            }


            // ViewData["person1"]   = peopleData[0];
            // ViewData["personID1"] = peopleID[0];

            //string[] personName = new string[countPeople];
            //string[] personID   = new string[countPeople];

            //int k = 0;
            //foreach (var row in monitoringDB.System_Person)
            //{
            //    personName[k] = row.Name.ToString();
            //    personID[k] = row.ID.ToString();
            //    k++;
            //}

            //var allPeople = new List<string>();
            //foreach (string pName in personName)
            //{
            //allPeople.Add(pName);
            //}

            ViewData["countPeople"] = countPeople;
            ViewData["personName1"] = personName[0];
            ViewData["personName2"] = personName[1];
            ViewData["personName3"] = personName[2];

            ViewData["personID1"] = peopleID[0];
            ViewData["personID2"] = peopleID[1];
            ViewData["personID3"] = peopleID[2];

            ViewBag.peopleData = peopleData;
            ViewBag.Message = "Анализ";

            DateTime last = new DateTime();
            last = DateTime.Now.AddDays(-30);
            DateTime real = DateTime.Now;
            List<DateTime> dates = new List<DateTime>();
            do
            {
                dates.Add(last);
                last = last.AddDays(+1);
            } while (real != last);

            // Twitter Chart ///
            string queryTw1 = string.Format("select count(*) as count,Convert(date,b.CreatedAt) as date from dbo.[Twitter.Tweet.Person] a inner join dbo.[Twitter.Tweets] b on a.TweetID = b.ID  and (a.IsDeleted is null or a.IsDeleted <> 1) " +
                "inner join dbo.[System.Person] c on a.PersonID = c.ID  where CONVERT(date, getdate()) >= Convert(date,b.CreatedAt) and Convert(date,b.CreatedAt) > DATEADD(DD, -30, CONVERT(date, getdate())) and c.ID = '{0}' group by Convert(date, b.CreatedAt) order by Convert(date, b.CreatedAt) desc;", peopleID[0]);
            string queryTw2 = string.Format("select count(*) as count,Convert(date,b.CreatedAt) as date from dbo.[Twitter.Tweet.Person] a inner join dbo.[Twitter.Tweets] b on a.TweetID = b.ID  and (a.IsDeleted is null or a.IsDeleted <> 1) " +
                "inner join dbo.[System.Person] c on a.PersonID = c.ID  where CONVERT(date, getdate()) >= Convert(date,b.CreatedAt) and Convert(date,b.CreatedAt) > DATEADD(DD, -30, CONVERT(date, getdate())) and c.ID = '{0}' group by Convert(date, b.CreatedAt) order by Convert(date, b.CreatedAt) desc;", peopleID[1]);
            string queryTw3 = string.Format("select count(*) as count,Convert(date,b.CreatedAt) as date from dbo.[Twitter.Tweet.Person] a inner join dbo.[Twitter.Tweets] b on a.TweetID = b.ID  and (a.IsDeleted is null or a.IsDeleted <> 1) " +
                "inner join dbo.[System.Person] c on a.PersonID = c.ID  where CONVERT(date, getdate()) >= Convert(date,b.CreatedAt) and Convert(date,b.CreatedAt) > DATEADD(DD, -30, CONVERT(date, getdate())) and c.ID = '{0}' group by Convert(date, b.CreatedAt) order by Convert(date, b.CreatedAt) desc;", peopleID[2]);
            
            var resultsTw1 = monitoringDB.Database.SqlQuery<graphResult>(queryTw1).ToList<graphResult>();
            var resultsTw2 = monitoringDB.Database.SqlQuery<graphResult>(queryTw2).ToList<graphResult>();
            var resultsTw3 = monitoringDB.Database.SqlQuery<graphResult>(queryTw3).ToList<graphResult>();


            string countertw1 = "[";
            string countertw2 = "[";
            string countertw3 = "[";

            #region tw1loop
            for (int i = 0; i < dates.Count; i++)
            {
                DateTime date = dates.ElementAt(i).Date;
                //
                if (resultsTw1.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (countertw1 == "[")
                    {
                        countertw1 = countertw1 + resultsTw1.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    countertw1 = countertw1 + "," + resultsTw1.Where(p => p.date.Equals(date)).FirstOrDefault().count;

                }
                else
                {
                    if (countertw1 == "[")
                    {
                        countertw1 = countertw1 + 0;

                    }
                    countertw1 = countertw1 + "," + 0;

                }
            }
            #endregion
            #region tw2loop
            for (int i = 0; i < dates.Count; i++)
            {
                DateTime date = dates.ElementAt(i).Date;
                //
                if (resultsTw2.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (countertw2 == "[")
                    {
                        countertw2 = countertw2 + resultsTw2.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    countertw2 = countertw2 + "," + resultsTw2.Where(p => p.date.Equals(date)).FirstOrDefault().count;

                }
                else
                {
                    if (countertw2 == "[")
                    {
                        countertw2 = countertw2 + 0;

                    }
                    countertw2 = countertw2 + "," + 0;

                }
            }
            #endregion
            #region tw3loop
            for (int i = 0; i < dates.Count; i++)
            {
                DateTime date = dates.ElementAt(i).Date;
                //
                if (resultsTw3.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (countertw3 == "[")
                    {
                        countertw3 = countertw3 + resultsTw3.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }

                    countertw3 = countertw3 + "," + resultsTw3.Where(p => p.date.Equals(date)).FirstOrDefault().count;

                }
                else
                {
                    if (countertw3 == "[")
                    {
                        countertw3 = countertw3 + 0;

                    }
                    countertw3 = countertw3 + "," + 0;

                }
            }
            #endregion

            countertw1 = countertw1 + "]";
            countertw2 = countertw2 + "]";
            countertw3 = countertw3 + "]";
            ViewBag.Tw1ChartCount = countertw1;
            ViewBag.Tw2ChartCount = countertw2;
            ViewBag.Tw3ChartCount = countertw3;

            // End Twitter Chart //


            // Facebook Chart ///
            string queryFb1 = string.Format("select count(*) as count,Convert(date,b.UpdateTime) as date from dbo.[Facebook.Post.Person] a inner join dbo.[Facebook.Posts] b on a.PostID = b.ID  and (a.IsDeleted is null or a.IsDeleted <> 1) " +
                "inner join dbo.[System.Person] c on a.PersonID = c.ID  where CONVERT(date, getdate()) >= Convert(date,b.UpdateTime) and Convert(date,b.UpdateTime) > DATEADD(DD, -30, CONVERT(date, getdate())) and c.ID = '{0}' group by Convert(date, b.UpdateTime) order by Convert(date, b.UpdateTime) desc;", peopleID[0]);
            string queryFb2 = string.Format("select count(*) as count,Convert(date,b.UpdateTime) as date from dbo.[Facebook.Post.Person] a inner join dbo.[Facebook.Posts] b on a.PostID = b.ID  and (a.IsDeleted is null or a.IsDeleted <> 1) " +
                "inner join dbo.[System.Person] c on a.PersonID = c.ID  where CONVERT(date, getdate()) >= Convert(date,b.UpdateTime) and Convert(date,b.UpdateTime) > DATEADD(DD, -30, CONVERT(date, getdate())) and c.ID = '{0}' group by Convert(date, b.UpdateTime) order by Convert(date, b.UpdateTime) desc;", peopleID[1]);
            string queryFb3 = string.Format("select count(*) as count,Convert(date,b.UpdateTime) as date from dbo.[Facebook.Post.Person] a inner join dbo.[Facebook.Posts] b on a.PostID = b.ID  and (a.IsDeleted is null or a.IsDeleted <> 1)" +
                "inner join dbo.[System.Person] c on a.PersonID = c.ID  where CONVERT(date, getdate()) >= Convert(date,b.UpdateTime) and Convert(date,b.UpdateTime) > DATEADD(DD, -30, CONVERT(date, getdate())) and c.ID = '{0}' group by Convert(date, b.UpdateTime) order by Convert(date, b.UpdateTime) desc;", peopleID[2]);

            var resultsFb1 = monitoringDB.Database.SqlQuery<graphResult>(queryFb1).ToList<graphResult>();
            var resultsFb2 = monitoringDB.Database.SqlQuery<graphResult>(queryFb2).ToList<graphResult>();
            var resultsFb3 = monitoringDB.Database.SqlQuery<graphResult>(queryFb3).ToList<graphResult>();

            string counterfb1 = "[";
            string counterfb2 = "[";
            string counterfb3 = "[";

            #region Fb1loop
            for (int i = 0; i < dates.Count; i++)
            {
                DateTime date = dates.ElementAt(i).Date;
                //
                if (resultsFb1.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterfb1 == "[")
                    {
                        counterfb1 = counterfb1 + resultsFb1.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    counterfb1 = counterfb1 + "," + resultsFb1.Where(p => p.date.Equals(date)).FirstOrDefault().count;

                }
                else
                {
                    if (counterfb1 == "[")
                    {
                        counterfb1 = counterfb1 + 0;

                    }
                    counterfb1 = counterfb1 + "," + 0;

                }
            }
            #endregion
            #region Fb2loop
            for (int i = 0; i < dates.Count; i++)
            {
                DateTime date = dates.ElementAt(i).Date;
                //
                if (resultsFb2.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterfb2 == "[")
                    {
                        counterfb2 = counterfb2 + resultsFb2.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    counterfb2 = counterfb2 + "," + resultsFb2.Where(p => p.date.Equals(date)).FirstOrDefault().count;

                }
                else
                {
                    if (counterfb2 == "[")
                    {
                        counterfb2 = counterfb2 + 0;

                    }
                    counterfb2 = counterfb2 + "," + 0;

                }
            }
            #endregion
            #region Fb3loop
            for (int i = 0; i < dates.Count; i++)
            {
                DateTime date = dates.ElementAt(i).Date;
                //
                if (resultsFb3.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterfb3 == "[")
                    {
                        counterfb3 = counterfb3 + resultsFb3.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }

                    counterfb3 = counterfb3 + "," + resultsFb3.Where(p => p.date.Equals(date)).FirstOrDefault().count;

                }
                else
                {
                    if (counterfb3 == "[")
                    {
                        counterfb3 = counterfb3 + 0;

                    }
                    counterfb3 = counterfb3 + "," + 0;

                }
            }
            #endregion

            counterfb1 = counterfb1 + "]";
            counterfb2 = counterfb2 + "]";
            counterfb3 = counterfb3 + "]";
            ViewBag.Fb1ChartCount = counterfb1;
            ViewBag.Fb2ChartCount = counterfb2;
            ViewBag.Fb3ChartCount = counterfb3;

            // End Facebook Chart //

            // Web Chart ///
            string queryWeb1 = string.Format("select count(*) as count,Convert(date,b.Date) as date from dbo.[Website.Post.Person] a inner join dbo.[Website.Posts] b on a.PostID = b.ID  and (a.IsDeleted is null or a.IsDeleted <> 1) " +
                "inner join dbo.[System.Person] c on a.PersonID = c.ID  where CONVERT(date, getdate()) >= Convert(date,b.Date) and Convert(date,b.Date) > DATEADD(DD, -30, CONVERT(date, getdate())) and c.ID = '{0}' group by Convert(date, b.Date) order by Convert(date, b.Date) desc;", peopleID[0]);
            string queryWeb2 = string.Format("select count(*) as count,Convert(date,b.Date) as date from dbo.[Website.Post.Person] a inner join dbo.[Website.Posts] b on a.PostID = b.ID  and (a.IsDeleted is null or a.IsDeleted <> 1) " +
                "inner join dbo.[System.Person] c on a.PersonID = c.ID  where CONVERT(date, getdate()) >= Convert(date,b.Date) and Convert(date,b.Date) > DATEADD(DD, -30, CONVERT(date, getdate())) and c.ID = '{0}' group by Convert(date, b.Date) order by Convert(date, b.Date) desc;", peopleID[1]);
            string queryWeb3 = string.Format("select count(*) as count,Convert(date,b.Date) as date from dbo.[Website.Post.Person] a inner join dbo.[Website.Posts] b on a.PostID = b.ID  and (a.IsDeleted is null or a.IsDeleted <> 1) " +
                "inner join dbo.[System.Person] c on a.PersonID = c.ID  where CONVERT(date, getdate()) >= Convert(date,b.Date) and Convert(date,b.Date) > DATEADD(DD, -30, CONVERT(date, getdate())) and c.ID = '{0}' group by Convert(date, b.Date) order by Convert(date, b.Date) desc;", peopleID[2]);

            var resultsWeb1 = monitoringDB.Database.SqlQuery<graphResult>(queryWeb1).ToList<graphResult>();
            var resultsWeb2 = monitoringDB.Database.SqlQuery<graphResult>(queryWeb2).ToList<graphResult>();
            var resultsWeb3 = monitoringDB.Database.SqlQuery<graphResult>(queryWeb3).ToList<graphResult>();


            string counterweb1 = "[";
            string counterweb2 = "[";
            string counterweb3 = "[";

            #region Web1loop
            for (int i = 0; i < dates.Count; i++)
            {
                DateTime date = dates.ElementAt(i).Date;
                //
                if (resultsWeb1.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterweb1 == "[")
                    {
                        counterweb1 = counterweb1 + resultsWeb1.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    counterweb1 = counterweb1 + "," + resultsWeb1.Where(p => p.date.Equals(date)).FirstOrDefault().count;

                }
                else
                {
                    if (counterweb1 == "[")
                    {
                        counterweb1 = counterweb1 + 0;

                    }
                    counterweb1 = counterweb1 + "," + 0;

                }
            }
            #endregion
            #region Web2loop
            for (int i = 0; i < dates.Count; i++)
            {
                DateTime date = dates.ElementAt(i).Date;
                //
                if (resultsWeb2.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterweb2 == "[")
                    {
                        counterweb2 = counterweb2 + resultsWeb2.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    counterweb2 = counterweb2 + "," + resultsWeb2.Where(p => p.date.Equals(date)).FirstOrDefault().count;

                }
                else
                {
                    if (counterweb2 == "[")
                    {
                        counterweb2 = counterweb2 + 0;

                    }
                    counterweb2 = counterweb2 + "," + 0;

                }
            }
            #endregion
            #region Web3loop
            for (int i = 0; i < dates.Count; i++)
            {
                DateTime date = dates.ElementAt(i).Date;
                //
                if (resultsWeb3.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterweb3 == "[")
                    {
                        counterweb3 = counterweb3 + resultsWeb3.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }

                    counterweb3 = counterweb3 + "," + resultsWeb3.Where(p => p.date.Equals(date)).FirstOrDefault().count;

                }
                else
                {
                    if (counterweb3 == "[")
                    {
                        counterweb3 = counterweb3 + 0;

                    }
                    counterweb3 = counterweb3 + "," + 0;

                }
            }
            #endregion

            counterweb1 = counterweb1 + "]";
            counterweb2 = counterweb2 + "]";
            counterweb3 = counterweb3 + "]";
            ViewBag.Web1ChartCount = counterweb1;
            ViewBag.Web2ChartCount = counterweb2;
            ViewBag.Web3ChartCount = counterweb3;

            // End Twitter Chart //
            var per1 = peopleID[0];
            var per2 = peopleID[1];
            var per3 = peopleID[2];

            var color1 = (from a in monitoringDB.System_Groups
                          join b in monitoringDB.System_Person on a.ID equals b.GroupID
                          where b.ID.ToString().Equals(per1)
                          select new { code = a.Color }).ToList();

            var color2 = (from a in monitoringDB.System_Groups
                          join b in monitoringDB.System_Person on a.ID equals b.GroupID
                          where b.ID.ToString().Equals(per2)
                          select new { code = a.Color }).ToList();

            var color3 = (from a in monitoringDB.System_Groups
                          join b in monitoringDB.System_Person on a.ID equals b.GroupID
                          where b.ID.ToString().Equals(per3)
                          select new { code = a.Color }).ToList();

            ViewBag.PerColor1 = color1.FirstOrDefault().code;
            ViewBag.PerColor2 = color2.FirstOrDefault().code;
            ViewBag.PerColor3 = color3.FirstOrDefault().code;

            return View();
        }

        [HttpPost]
        public ActionResult FbInfo(string PeopleID)
        {
            ViewBag.Message = "FbInfo";
            ViewData["peopleID"] = PeopleID;

            MonitoringEntities monitoringDB = new MonitoringEntities();

            var FbData =
                (from a in monitoringDB.Facebook_Post_Person
                 join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
                 join c in monitoringDB.System_Person on a.PersonID equals c.ID
                 where a.PersonID.ToString() == PeopleID && a.IsDeleted!=true
                 select new Models.FbPost
                 {
                     ID = b.ID,
                     PageID = b.PageID,
                     GroupID = b.GroupID,
                     PostID = b.PostID,
                     FromID = b.FromID,
                     FromName = b.FromName,
                     Message = b.Message,
                     Story = b.Story,
                     Type = b.Type,
                     UpdateTime = b.UpdateTime,
                     SharedCount = b.SharedCount,
                     PermalinkUrl = b.PermalinkUrl,
                     Caption = b.Caption,
                     Description = b.Description,
                     FullPicture = b.FullPicture,
                     Link = b.Link,
                     Name = b.Name,
                     Picture = b.Picture,
                     Icon = b.Icon,
                     ObjectID = b.ObjectID,
                     ParentID = b.ParentID,
                     Sentiment = a.Sentiment,
                     PerName = c.Name
                 }).Take(5).ToList();

            //ViewBag.data = FbData;
            return PartialView(FbData);
        }


        [HttpPost]
        public ActionResult WebSiteInfo(string PeopleID)
        {
            ViewBag.Message = "WebSiteInfo";
            ViewData["peopleID"] = PeopleID;

            MonitoringEntities monitoringDB = new MonitoringEntities();

            var WebSiteData =
                (from a in monitoringDB.Website_Post_Person
                 join b in monitoringDB.WebSite_Posts on a.PostID equals b.ID
                 join c in monitoringDB.System_Person on a.PersonID equals c.ID
                 where a.PersonID.ToString() == PeopleID
                 select new Models.WebSitePost
                 {
                     ID = b.ID,
                     Link = b.Link,
                     Title = b.Title,
                     Text = b.Text,
                     Url = b.Url,
                     Body = b.Body,
                     Reporter = b.Reporter,
                     CoverUrl = b.CoverUrl
                 }).Take(5).ToList();

            //ViewBag.data = FbData;
            return PartialView(WebSiteData);
        }

        [HttpPost]
        public ActionResult TwitterInfo(string PeopleID)
        {
            ViewBag.Message = "TwitterInfo";
            ViewData["peopleID"] = PeopleID;

            MonitoringEntities monitoringDB = new MonitoringEntities();

            var TwitterData =
                (from a in monitoringDB.Twitter_Tweet_Person
                 join tweet in monitoringDB.Twitter_Tweets on a.TweetID equals tweet.ID
                 join c in monitoringDB.System_Person on a.PersonID equals c.ID
                 join user in monitoringDB.Twitter_User_Details on tweet.TwitterUserID equals user.TwitterUserID
                 where a.PersonID.ToString() == PeopleID
                 select new Models.Twitter
                 {
                     ID = tweet.ID,
                     TwitterUserID = tweet.TwitterUserID,
                     TweetID = tweet.TweetID,
                     CreatedAt = tweet.CreatedAt,
                     ScreenName = user.ScreenName,
                     Source = tweet.Source,
                     StatusID = tweet.StatusID,
                     RetweetCount = tweet.RetweetCount,
                     Retweeted = tweet.Retweeted,
                     FullText = tweet.FullText,
                     Language = tweet.Language,
                     Text = tweet.Text,
                     TrimUser = tweet.TrimUser,
                     Truncated = tweet.Truncated,
                     UserID = tweet.UserID,
                     HashEntity1 = tweet.HashEntity1,
                     HashEntity2 = tweet.HashEntity2,
                     HashEntity3 = tweet.HashEntity3,
                     SymbolEntity1 = tweet.SymbolEntity1,
                     SymbolEntity2 = tweet.SymbolEntity2,
                     SymbolEntity3 = tweet.SymbolEntity3,
                     UrlEntity1 = tweet.UrlEntity1,
                     UrlEntityDisplayUrl1 = tweet.UrlEntityDisplayUrl1,
                     UrlEntityExpandedUrl1 = tweet.UrlEntityExpandedUrl1,
                     UrlEntity2 = tweet.UrlEntity2,
                     UrlEntityDisplayUrl2 = tweet.UrlEntityDisplayUrl2,
                     UrlEntityExpandedUrl2 = tweet.UrlEntityExpandedUrl2,
                     UrlEntity3 = tweet.UrlEntity3,
                     UrlEntityDisplayUrl3 = tweet.UrlEntityDisplayUrl3,
                     UrlEntityExpandedUrl3 = tweet.UrlEntityExpandedUrl3,
                     MediaEntitiy1 = tweet.MediaEntitiy1,
                     MediaEntitiyDisplayUrl1 = tweet.MediaEntitiyDisplayUrl1,
                     MediaEntitiyMediaUrl1 = tweet.MediaEntitiyMediaUrl1,
                     MediaEntitiyVideoInfo1 = tweet.MediaEntitiyVideoInfo1,
                     MediaEntity2 = tweet.MediaEntity2,
                     MediaEntitiyDisplayUrl2 = tweet.MediaEntitiyDisplayUrl2,
                     MediaEntitiyMediaUrl2 = tweet.MediaEntitiyMediaUrl2,
                     MediaEntitiyVideoInfo2 = tweet.MediaEntitiyVideoInfo2,
                     MediaEntity3 = tweet.MediaEntity3,
                     MediaEntitiyDisplayUrl3 = tweet.MediaEntitiyDisplayUrl3,
                     MediaEntitiyMediaUrl3 = tweet.MediaEntitiyMediaUrl3,
                     MediaEntitiyVideoInfo3 = tweet.MediaEntitiyVideoInfo3,
                     UserMentionEntity1 = tweet.UserMentionEntity1,
                     UserMentionEntityScreenName1 = tweet.UserMentionEntityScreenName1,
                     UserMentionEntity2 = tweet.UserMentionEntity2,
                     UserMentionEntityScreenName2 = tweet.UserMentionEntityScreenName2,
                     UserMentionEntity3 = tweet.UserMentionEntity3,
                     UserMentionEntityScreenName3 = tweet.UserMentionEntityScreenName3,

                     //user
                     ContributorsEnabled = user.ContributorsEnabled,
                     Count = user.Count,
                     Cursor = user.Cursor,
                     DefaultProfile = user.DefaultProfile,
                     DefaultProfileImage = user.DefaultProfileImage,
                     Description = user.Description,
                     Email = user.Email,
                     FavoritesCount = user.FavoritesCount,
                     FollowRequestSent = user.FollowRequestSent,
                     FollowersCount = user.FollowersCount,
                     Following = user.Following,
                     FriendsCount = user.FriendsCount,
                     GeoEnabled = user.GeoEnabled,
                     ImageSize = user.ImageSize,
                     IncludeEntities = user.IncludeEntities,
                     IsTranslator = user.IsTranslator,
                     Lang = user.Lang,
                     LangResponse = user.LangResponse,
                     ListedCount = user.ListedCount,
                     Location = user.Location,
                     Name = user.Name,
                     Notifications = user.Notifications,
                     Page = user.Page,
                     ProfileBackgroundColor = user.ProfileBackgroundColor,
                     ProfileBackgroundImageUrl = user.ProfileBackgroundImageUrl,
                     ProfileBackgroundImageUrlHttps = user.ProfileBackgroundImageUrlHttps,
                     ProfileBackgroundTile = user.ProfileBackgroundTile,
                     ProfileBannerUrl = user.ProfileBannerUrl,
                     ProfileImage = user.ProfileImage,
                     ProfileImageUrl = user.ProfileImageUrl,
                     ProfileImageUrlHttps = user.ProfileImageUrlHttps,
                     ProfileLinkColor = user.ProfileLinkColor,
                     ProfileSidebarBorderColor = user.ProfileSidebarBorderColor,
                     ProfileSidebarFillColor = user.ProfileSidebarFillColor,
                     ProfileTextColor = user.ProfileTextColor,
                     ProfileUseBackgroundImage = user.ProfileUseBackgroundImage,
                     Protected = user.Protected,
                     Query = user.Query,
                     ScreenNameList = user.ScreenNameList,
                     ScreenNameResponse = user.ScreenNameResponse,
                     ShowAllInlineMedia = user.ShowAllInlineMedia,
                     SkipStatus = user.SkipStatus,
                     Slug = user.Slug,
                     StatusesCount = user.StatusesCount,
                     TimeZone = user.TimeZone,
                     Type = user.Type,
                     Url = user.Url,
                     UserIDResponse = user.UserIDResponse,
                     UserIdList = user.UserIdList,
                     UtcOffset = user.UtcOffset,
                     Verified = user.Verified

                 }).Take(5).ToList();

            //ViewBag.data = FbData;
            return PartialView(TwitterData);
        }

        [HttpPost]
        public ActionResult FbPosts(string PeopleID)
        {
            ViewBag.Message = "FbPosts";
            ViewData["peopleID"] = PeopleID;
            int CountID = 0;
            MonitoringEntities monitoringDB = new MonitoringEntities();

            var FbData =
                  (from a in monitoringDB.Facebook_Post_Person
                   join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
                   join c in monitoringDB.System_Person on a.PersonID equals c.ID
                   where a.PersonID.ToString() == PeopleID && a.IsDeleted != true
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
                       Sentiment = a.Sentiment,
                       PerName = c.Name
                   }).Take(5).ToList();

            foreach (var post in FbData)
            {
                CountID++;
                if (CountID == 1)
                {
                    ViewBag.PicturePerson = post.PicturePerson;
                    ViewBag.CusName = post.Name;
                }
            }
            return PartialView(FbData);
        }

        [HttpPost]
        public ActionResult TwitterPosts(string PeopleID)
        {
            ViewBag.Message = "TwitterPosts";
            ViewData["peopleID"] = PeopleID;
            int CountID = 0;
            MonitoringEntities monitoringDB = new MonitoringEntities();

            var TwitterData =
                (from a in monitoringDB.Twitter_Tweet_Person
                 join tweet in monitoringDB.Twitter_Tweets on a.TweetID equals tweet.ID
                 join c in monitoringDB.System_Person on a.PersonID equals c.ID
                 join user in monitoringDB.Twitter_User_Details on tweet.TwitterUserID equals user.TwitterUserID
                 where a.PersonID.ToString() == PeopleID && a.IsDeleted != true
                 select new Models.Twitter
                 {
                     ID = tweet.ID,
                     TwitterUserID = tweet.TwitterUserID,
                     TweetID = tweet.TweetID,
                     CreatedAt = tweet.CreatedAt,
                     ScreenName = user.ScreenName,
                     Source = tweet.Source,
                     StatusID = tweet.StatusID,
                     RetweetCount = tweet.RetweetCount,
                     Retweeted = tweet.Retweeted,
                     FullText = tweet.FullText,
                     Language = tweet.Language,
                     Text = tweet.Text,
                     TrimUser = tweet.TrimUser,
                     Truncated = tweet.Truncated,
                     UserID = tweet.UserID,
                     HashEntity1 = tweet.HashEntity1,
                     HashEntity2 = tweet.HashEntity2,
                     HashEntity3 = tweet.HashEntity3,
                     SymbolEntity1 = tweet.SymbolEntity1,
                     SymbolEntity2 = tweet.SymbolEntity2,
                     SymbolEntity3 = tweet.SymbolEntity3,
                     UrlEntity1 = tweet.UrlEntity1,
                     UrlEntityDisplayUrl1 = tweet.UrlEntityDisplayUrl1,
                     UrlEntityExpandedUrl1 = tweet.UrlEntityExpandedUrl1,
                     UrlEntity2 = tweet.UrlEntity2,
                     UrlEntityDisplayUrl2 = tweet.UrlEntityDisplayUrl2,
                     UrlEntityExpandedUrl2 = tweet.UrlEntityExpandedUrl2,
                     UrlEntity3 = tweet.UrlEntity3,
                     UrlEntityDisplayUrl3 = tweet.UrlEntityDisplayUrl3,
                     UrlEntityExpandedUrl3 = tweet.UrlEntityExpandedUrl3,
                     MediaEntitiy1 = tweet.MediaEntitiy1,
                     MediaEntitiyDisplayUrl1 = tweet.MediaEntitiyDisplayUrl1,
                     MediaEntitiyMediaUrl1 = tweet.MediaEntitiyMediaUrl1,
                     MediaEntitiyVideoInfo1 = tweet.MediaEntitiyVideoInfo1,
                     MediaEntity2 = tweet.MediaEntity2,
                     MediaEntitiyDisplayUrl2 = tweet.MediaEntitiyDisplayUrl2,
                     MediaEntitiyMediaUrl2 = tweet.MediaEntitiyMediaUrl2,
                     MediaEntitiyVideoInfo2 = tweet.MediaEntitiyVideoInfo2,
                     MediaEntity3 = tweet.MediaEntity3,
                     MediaEntitiyDisplayUrl3 = tweet.MediaEntitiyDisplayUrl3,
                     MediaEntitiyMediaUrl3 = tweet.MediaEntitiyMediaUrl3,
                     MediaEntitiyVideoInfo3 = tweet.MediaEntitiyVideoInfo3,
                     UserMentionEntity1 = tweet.UserMentionEntity1,
                     UserMentionEntityScreenName1 = tweet.UserMentionEntityScreenName1,
                     UserMentionEntity2 = tweet.UserMentionEntity2,
                     UserMentionEntityScreenName2 = tweet.UserMentionEntityScreenName2,
                     UserMentionEntity3 = tweet.UserMentionEntity3,
                     UserMentionEntityScreenName3 = tweet.UserMentionEntityScreenName3,

                     //user
                     ContributorsEnabled = user.ContributorsEnabled,
                     Count = user.Count,
                     Cursor = user.Cursor,
                     DefaultProfile = user.DefaultProfile,
                     DefaultProfileImage = user.DefaultProfileImage,
                     Description = user.Description,
                     Email = user.Email,
                     FavoritesCount = user.FavoritesCount,
                     FollowRequestSent = user.FollowRequestSent,
                     FollowersCount = user.FollowersCount,
                     Following = user.Following,
                     FriendsCount = user.FriendsCount,
                     GeoEnabled = user.GeoEnabled,
                     ImageSize = user.ImageSize,
                     IncludeEntities = user.IncludeEntities,
                     IsTranslator = user.IsTranslator,
                     Lang = user.Lang,
                     LangResponse = user.LangResponse,
                     ListedCount = user.ListedCount,
                     Location = user.Location,
                     Name = user.Name,
                     Notifications = user.Notifications,
                     Page = user.Page,
                     ProfileBackgroundColor = user.ProfileBackgroundColor,
                     ProfileBackgroundImageUrl = user.ProfileBackgroundImageUrl,
                     ProfileBackgroundImageUrlHttps = user.ProfileBackgroundImageUrlHttps,
                     ProfileBackgroundTile = user.ProfileBackgroundTile,
                     ProfileBannerUrl = user.ProfileBannerUrl,
                     ProfileImage = user.ProfileImage,
                     ProfileImageUrl = user.ProfileImageUrl,
                     ProfileImageUrlHttps = user.ProfileImageUrlHttps,
                     ProfileLinkColor = user.ProfileLinkColor,
                     ProfileSidebarBorderColor = user.ProfileSidebarBorderColor,
                     ProfileSidebarFillColor = user.ProfileSidebarFillColor,
                     ProfileTextColor = user.ProfileTextColor,
                     ProfileUseBackgroundImage = user.ProfileUseBackgroundImage,
                     Protected = user.Protected,
                     Query = user.Query,
                     ScreenNameList = user.ScreenNameList,
                     ScreenNameResponse = user.ScreenNameResponse,
                     ShowAllInlineMedia = user.ShowAllInlineMedia,
                     SkipStatus = user.SkipStatus,
                     Slug = user.Slug,
                     StatusesCount = user.StatusesCount,
                     TimeZone = user.TimeZone,
                     Type = user.Type,
                     Url = user.Url,
                     UserIDResponse = user.UserIDResponse,
                     UserIdList = user.UserIdList,
                     UtcOffset = user.UtcOffset,
                     Verified = user.Verified
                     //ID = b.ID,
                     //Tittle = b.Tittle,
                     //Text = b.Text,
                     //Date = b.CreatedAt,
                     //PicturePerson = c.Picture,
                     //ReTweetCount = b.RetweetCount,
                     //,
                     //SortOrder = b.SortOrder,
                     //Name = c.Name
                 }).Take(5).ToList();


            foreach (var post in TwitterData)
            {
                CountID++;
                if (CountID == 1)
                {
                    // ViewBag.PicturePerson = post.PicturePerson;
                    ViewBag.CusName = post.Name;
                }
            }
            //ViewBag.data = FbData;
            return PartialView(TwitterData);
        }

        [HttpPost]
        public ActionResult WebSitePosts(string PeopleID)
        {
            ViewBag.Message = "WebSitePost";
            ViewData["peopleID"] = PeopleID;
            int CountID = 0;
            MonitoringEntities monitoringDB = new MonitoringEntities();

            var WebSiteData =
                (from a in monitoringDB.Website_Post_Person
                 join b in monitoringDB.WebSite_Posts on a.PostID equals b.ID
                 join c in monitoringDB.System_Person on a.PersonID equals c.ID
                 where a.PersonID.ToString() == PeopleID && a.IsDeleted != true
                 select new Models.WebSitePost
                 {
                     ID = b.ID,
                     Link = b.Link,
                     Title = b.Title,
                     PicturePerson = c.Picture,
                     Text = b.Text,
                     Name = c.Name,
                     Url = b.Url,
                     Body = b.Body.Substring(0, 300) + "...",
                     Reporter = b.Reporter,
                     CoverUrl = b.CoverUrl
                 }).Take(5).ToList();

            foreach (var post in WebSiteData)
            {
                CountID++;
                if (CountID == 1)
                {
                    ViewBag.PicturePerson = post.PicturePerson;
                    ViewBag.CusName = post.Name;
                }
            }
            //ViewBag.data = FbData;
            return PartialView(WebSiteData);
        }

        public ActionResult SmartNewsCustomers(String personID1, String personID2, String personID3, String Sentiment)
        {
            ViewBag.Message = "Ухаалаг мэдээ";
            MonitoringEntities monitoringDB = new MonitoringEntities();
            if (Sentiment == "All")
            {
                ViewBag.FbData =
                                (from a in monitoringDB.Facebook_Post_Person
                                 join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
                                 join c in monitoringDB.System_Person on a.PersonID equals c.ID
                                 where (c.ID.ToString() == personID1 || c.ID.ToString() == personID2 || c.ID.ToString() == personID3) && a.IsDeleted != true
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
                                     Name = b.Name,
                                     Picture = b.Picture,
                                     Icon = b.Icon,
                                     ObjectID = b.ObjectID,
                                     ParentID = b.ParentID,
                                     PerName = c.Surename.Substring(0,1)+"."+ c.Name,
                                     Sentiment = a.Sentiment

                                 }).Take(200).ToList();

                ViewBag.TwitterData =
                   (from a in monitoringDB.Twitter_Tweet_Person
                    join tweet in monitoringDB.Twitter_Tweets on a.TweetID equals tweet.ID
                    join c in monitoringDB.System_Person on a.PersonID equals c.ID
                    join user in monitoringDB.Twitter_User_Details on tweet.TwitterUserID equals user.TwitterUserID
                    where (c.ID.ToString() == personID1 || c.ID.ToString() == personID2 || c.ID.ToString() == personID3) && a.IsDeleted != true
                    orderby tweet.CreatedAt descending
                    select new Models.Twitter
                    {
                        ID = tweet.ID,
                        TwitterUserID = tweet.TwitterUserID,
                        TweetID = tweet.TweetID,
                        CreatedAt = tweet.CreatedAt,
                        ScreenName = user.ScreenName,
                        Source = tweet.Source,
                        StatusID = tweet.StatusID,
                        RetweetCount = tweet.RetweetCount,
                        Retweeted = tweet.Retweeted,
                        FullText = tweet.FullText,
                        Language = tweet.Language,
                        Text = tweet.Text,
                        TrimUser = tweet.TrimUser,
                        Truncated = tweet.Truncated,
                        UserID = tweet.UserID,
                        HashEntity1 = tweet.HashEntity1,
                        HashEntity2 = tweet.HashEntity2,
                        HashEntity3 = tweet.HashEntity3,
                        SymbolEntity1 = tweet.SymbolEntity1,
                        SymbolEntity2 = tweet.SymbolEntity2,
                        SymbolEntity3 = tweet.SymbolEntity3,
                        UrlEntity1 = tweet.UrlEntity1,
                        UrlEntityDisplayUrl1 = tweet.UrlEntityDisplayUrl1,
                        UrlEntityExpandedUrl1 = tweet.UrlEntityExpandedUrl1,
                        UrlEntity2 = tweet.UrlEntity2,
                        UrlEntityDisplayUrl2 = tweet.UrlEntityDisplayUrl2,
                        UrlEntityExpandedUrl2 = tweet.UrlEntityExpandedUrl2,
                        UrlEntity3 = tweet.UrlEntity3,
                        UrlEntityDisplayUrl3 = tweet.UrlEntityDisplayUrl3,
                        UrlEntityExpandedUrl3 = tweet.UrlEntityExpandedUrl3,
                        MediaEntitiy1 = tweet.MediaEntitiy1,
                        MediaEntitiyDisplayUrl1 = tweet.MediaEntitiyDisplayUrl1,
                        MediaEntitiyMediaUrl1 = tweet.MediaEntitiyMediaUrl1,
                        MediaEntitiyVideoInfo1 = tweet.MediaEntitiyVideoInfo1,
                        MediaEntity2 = tweet.MediaEntity2,
                        MediaEntitiyDisplayUrl2 = tweet.MediaEntitiyDisplayUrl2,
                        MediaEntitiyMediaUrl2 = tweet.MediaEntitiyMediaUrl2,
                        MediaEntitiyVideoInfo2 = tweet.MediaEntitiyVideoInfo2,
                        MediaEntity3 = tweet.MediaEntity3,
                        MediaEntitiyDisplayUrl3 = tweet.MediaEntitiyDisplayUrl3,
                        MediaEntitiyMediaUrl3 = tweet.MediaEntitiyMediaUrl3,
                        MediaEntitiyVideoInfo3 = tweet.MediaEntitiyVideoInfo3,
                        UserMentionEntity1 = tweet.UserMentionEntity1,
                        UserMentionEntityScreenName1 = tweet.UserMentionEntityScreenName1,
                        UserMentionEntity2 = tweet.UserMentionEntity2,
                        UserMentionEntityScreenName2 = tweet.UserMentionEntityScreenName2,
                        UserMentionEntity3 = tweet.UserMentionEntity3,
                        UserMentionEntityScreenName3 = tweet.UserMentionEntityScreenName3,

                        //user
                        ContributorsEnabled = user.ContributorsEnabled,
                        Count = user.Count,
                        Cursor = user.Cursor,
                        DefaultProfile = user.DefaultProfile,
                        DefaultProfileImage = user.DefaultProfileImage,
                        Description = user.Description,
                        Email = user.Email,
                        FavoritesCount = user.FavoritesCount,
                        FollowRequestSent = user.FollowRequestSent,
                        FollowersCount = user.FollowersCount,
                        Following = user.Following,
                        FriendsCount = user.FriendsCount,
                        GeoEnabled = user.GeoEnabled,
                        ImageSize = user.ImageSize,
                        IncludeEntities = user.IncludeEntities,
                        IsTranslator = user.IsTranslator,
                        Lang = user.Lang,
                        LangResponse = user.LangResponse,
                        ListedCount = user.ListedCount,
                        Location = user.Location,
                        Name = user.Name,
                        Notifications = user.Notifications,
                        Page = user.Page,
                        ProfileBackgroundColor = user.ProfileBackgroundColor,
                        ProfileBackgroundImageUrl = user.ProfileBackgroundImageUrl,
                        ProfileBackgroundImageUrlHttps = user.ProfileBackgroundImageUrlHttps,
                        ProfileBackgroundTile = user.ProfileBackgroundTile,
                        ProfileBannerUrl = user.ProfileBannerUrl,
                        ProfileImage = user.ProfileImage,
                        ProfileImageUrl = user.ProfileImageUrl,
                        ProfileImageUrlHttps = user.ProfileImageUrlHttps,
                        ProfileLinkColor = user.ProfileLinkColor,
                        ProfileSidebarBorderColor = user.ProfileSidebarBorderColor,
                        ProfileSidebarFillColor = user.ProfileSidebarFillColor,
                        ProfileTextColor = user.ProfileTextColor,
                        ProfileUseBackgroundImage = user.ProfileUseBackgroundImage,
                        Protected = user.Protected,
                        Query = user.Query,
                        ScreenNameList = user.ScreenNameList,
                        ScreenNameResponse = user.ScreenNameResponse,
                        ShowAllInlineMedia = user.ShowAllInlineMedia,
                        SkipStatus = user.SkipStatus,
                        Slug = user.Slug,
                        StatusesCount = user.StatusesCount,
                        TimeZone = user.TimeZone,
                        Type = user.Type,
                        Url = user.Url,
                        UserIDResponse = user.UserIDResponse,
                        UserIdList = user.UserIdList,
                        UtcOffset = user.UtcOffset,
                        Verified = user.Verified,
                        Sentiment = a.Sentiment,
                        PerName = c.Surename.Substring(0, 1) + "." + c.Name,
                        //,
                        //SortOrder = b.SortOrder
                    }).Take(200).ToList();

                ViewBag.WebSiteData =
                  (from a in monitoringDB.Website_Post_Person
                   join b in monitoringDB.WebSite_Posts on a.PostID equals b.ID
                   join c in monitoringDB.System_Person on a.PersonID equals c.ID
                   where (c.ID.ToString() == personID1 || c.ID.ToString() == personID2 || c.ID.ToString() == personID3) && a.IsDeleted != true
                   orderby b.Date descending
                   select new Models.WebSitePost
                   {
                       ID = b.ID,
                       Link = b.Link,
                       Title = b.Title,
                       Text = b.Text,
                       Sentiment = a.Sentiment,
                       Url = b.Url,
                       Body = b.Body.Substring(0, 300) + "...",
                       Reporter = b.Reporter,
                       CoverUrl = b.CoverUrl,
                       DateTime = b.DateTime,
                       PerName = c.Surename.Substring(0, 1) + "." + c.Name,
                   }).Take(200).ToList();
            }
            else
            {
                ViewBag.FbData =
                                (from a in monitoringDB.Facebook_Post_Person
                                 join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
                                 join c in monitoringDB.System_Person on a.PersonID equals c.ID
                                 where (c.ID.ToString() == personID1 || c.ID.ToString() == personID2 || c.ID.ToString() == personID3) && a.Sentiment == Sentiment && a.IsDeleted != true
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
                                     Name = b.Name,
                                     Picture = b.Picture,
                                     Icon = b.Icon,
                                     ObjectID = b.ObjectID,
                                     ParentID = b.ParentID,
                                     PerName = b.Name,
                                     Sentiment = a.Sentiment
                                 }).Take(200).ToList();

                ViewBag.TwitterData =
                   (from a in monitoringDB.Twitter_Tweet_Person
                    join tweet in monitoringDB.Twitter_Tweets on a.TweetID equals tweet.ID
                    join c in monitoringDB.System_Person on a.PersonID equals c.ID
                    join user in monitoringDB.Twitter_User_Details on tweet.TwitterUserID equals user.TwitterUserID
                    where (c.ID.ToString() == personID1 || c.ID.ToString() == personID2 || c.ID.ToString() == personID3) && a.Sentiment == Sentiment && a.IsDeleted != true
                    orderby tweet.CreatedAt descending
                    select new Models.Twitter
                    {
                        ID = tweet.ID,
                        TwitterUserID = tweet.TwitterUserID,
                        TweetID = tweet.TweetID,
                        CreatedAt = tweet.CreatedAt,
                        ScreenName = user.ScreenName,
                        Source = tweet.Source,
                        StatusID = tweet.StatusID,
                        RetweetCount = tweet.RetweetCount,
                        Retweeted = tweet.Retweeted,
                        FullText = tweet.FullText,
                        Language = tweet.Language,
                        Text = tweet.Text,
                        TrimUser = tweet.TrimUser,
                        Truncated = tweet.Truncated,
                        UserID = tweet.UserID,
                        HashEntity1 = tweet.HashEntity1,
                        HashEntity2 = tweet.HashEntity2,
                        HashEntity3 = tweet.HashEntity3,
                        SymbolEntity1 = tweet.SymbolEntity1,
                        SymbolEntity2 = tweet.SymbolEntity2,
                        SymbolEntity3 = tweet.SymbolEntity3,
                        UrlEntity1 = tweet.UrlEntity1,
                        UrlEntityDisplayUrl1 = tweet.UrlEntityDisplayUrl1,
                        UrlEntityExpandedUrl1 = tweet.UrlEntityExpandedUrl1,
                        UrlEntity2 = tweet.UrlEntity2,
                        UrlEntityDisplayUrl2 = tweet.UrlEntityDisplayUrl2,
                        UrlEntityExpandedUrl2 = tweet.UrlEntityExpandedUrl2,
                        UrlEntity3 = tweet.UrlEntity3,
                        UrlEntityDisplayUrl3 = tweet.UrlEntityDisplayUrl3,
                        UrlEntityExpandedUrl3 = tweet.UrlEntityExpandedUrl3,
                        MediaEntitiy1 = tweet.MediaEntitiy1,
                        MediaEntitiyDisplayUrl1 = tweet.MediaEntitiyDisplayUrl1,
                        MediaEntitiyMediaUrl1 = tweet.MediaEntitiyMediaUrl1,
                        MediaEntitiyVideoInfo1 = tweet.MediaEntitiyVideoInfo1,
                        MediaEntity2 = tweet.MediaEntity2,
                        MediaEntitiyDisplayUrl2 = tweet.MediaEntitiyDisplayUrl2,
                        MediaEntitiyMediaUrl2 = tweet.MediaEntitiyMediaUrl2,
                        MediaEntitiyVideoInfo2 = tweet.MediaEntitiyVideoInfo2,
                        MediaEntity3 = tweet.MediaEntity3,
                        MediaEntitiyDisplayUrl3 = tweet.MediaEntitiyDisplayUrl3,
                        MediaEntitiyMediaUrl3 = tweet.MediaEntitiyMediaUrl3,
                        MediaEntitiyVideoInfo3 = tweet.MediaEntitiyVideoInfo3,
                        UserMentionEntity1 = tweet.UserMentionEntity1,
                        UserMentionEntityScreenName1 = tweet.UserMentionEntityScreenName1,
                        UserMentionEntity2 = tweet.UserMentionEntity2,
                        UserMentionEntityScreenName2 = tweet.UserMentionEntityScreenName2,
                        UserMentionEntity3 = tweet.UserMentionEntity3,
                        UserMentionEntityScreenName3 = tweet.UserMentionEntityScreenName3,
                        Sentiment = a.Sentiment,
                        //user
                        ContributorsEnabled = user.ContributorsEnabled,
                        Count = user.Count,
                        Cursor = user.Cursor,
                        DefaultProfile = user.DefaultProfile,
                        DefaultProfileImage = user.DefaultProfileImage,
                        Description = user.Description,
                        Email = user.Email,
                        FavoritesCount = user.FavoritesCount,
                        FollowRequestSent = user.FollowRequestSent,
                        FollowersCount = user.FollowersCount,
                        Following = user.Following,
                        FriendsCount = user.FriendsCount,
                        GeoEnabled = user.GeoEnabled,
                        ImageSize = user.ImageSize,
                        IncludeEntities = user.IncludeEntities,
                        IsTranslator = user.IsTranslator,
                        Lang = user.Lang,
                        LangResponse = user.LangResponse,
                        ListedCount = user.ListedCount,
                        Location = user.Location,
                        Name = user.Name,
                        Notifications = user.Notifications,
                        Page = user.Page,
                        ProfileBackgroundColor = user.ProfileBackgroundColor,
                        ProfileBackgroundImageUrl = user.ProfileBackgroundImageUrl,
                        ProfileBackgroundImageUrlHttps = user.ProfileBackgroundImageUrlHttps,
                        ProfileBackgroundTile = user.ProfileBackgroundTile,
                        ProfileBannerUrl = user.ProfileBannerUrl,
                        ProfileImage = user.ProfileImage,
                        ProfileImageUrl = user.ProfileImageUrl,
                        ProfileImageUrlHttps = user.ProfileImageUrlHttps,
                        ProfileLinkColor = user.ProfileLinkColor,
                        ProfileSidebarBorderColor = user.ProfileSidebarBorderColor,
                        ProfileSidebarFillColor = user.ProfileSidebarFillColor,
                        ProfileTextColor = user.ProfileTextColor,
                        ProfileUseBackgroundImage = user.ProfileUseBackgroundImage,
                        Protected = user.Protected,
                        Query = user.Query,
                        ScreenNameList = user.ScreenNameList,
                        ScreenNameResponse = user.ScreenNameResponse,
                        ShowAllInlineMedia = user.ShowAllInlineMedia,
                        SkipStatus = user.SkipStatus,
                        Slug = user.Slug,
                        StatusesCount = user.StatusesCount,
                        TimeZone = user.TimeZone,
                        Type = user.Type,
                        Url = user.Url,
                        UserIDResponse = user.UserIDResponse,
                        UserIdList = user.UserIdList,
                        UtcOffset = user.UtcOffset,
                        Verified = user.Verified
                    }).Take(200).ToList();

                ViewBag.WebSiteData =
                  (from a in monitoringDB.Website_Post_Person
                   join b in monitoringDB.WebSite_Posts on a.PostID equals b.ID
                   join c in monitoringDB.System_Person on a.PersonID equals c.ID
                   where (c.ID.ToString() == personID1 || c.ID.ToString() == personID2 || c.ID.ToString() == personID3) && a.Sentiment == Sentiment && a.IsDeleted != true
                   orderby b.Date descending
                   select new Models.WebSitePost
                   {
                       ID = b.ID,
                       Link = b.Link,
                       Title = b.Title,
                       Text = b.Text,
                       Sentiment = a.Sentiment,
                       Url = b.Url,
                       Body = b.Body.Substring(0, 300) + "...",
                       Reporter = b.Reporter,
                       CoverUrl = b.CoverUrl,
                       DateTime = b.DateTime
                   }).Take(200).ToList();
            }
            return PartialView();
        }

        public ActionResult SmartNewsCustomersFilter(String personID1, String personID2, String personID3, String Sentiment, String Filter)
        {
            ViewBag.Message = "Ухаалаг мэдээ";
            MonitoringEntities monitoringDB = new MonitoringEntities();
            if (Filter == "Date")
            {
                if (Sentiment == "All" || Sentiment == "Default")
                {
                    ViewBag.FbData =
                                    (from a in monitoringDB.Facebook_Post_Person
                                     join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
                                     join c in monitoringDB.System_Person on a.PersonID equals c.ID
                                     where (c.ID.ToString() == personID1 || c.ID.ToString() == personID2 || c.ID.ToString() == personID3) && a.IsDeleted != true
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
                                         Name = b.Name,
                                         Picture = b.Picture,
                                         Icon = b.Icon,
                                         ObjectID = b.ObjectID,
                                         ParentID = b.ParentID,
                                         PerName = b.Name,
                                         Sentiment = a.Sentiment

                                     }).Take(200).ToList();

                    ViewBag.TwitterData =
                       (from a in monitoringDB.Twitter_Tweet_Person
                        join tweet in monitoringDB.Twitter_Tweets on a.TweetID equals tweet.ID
                        join c in monitoringDB.System_Person on a.PersonID equals c.ID
                        join user in monitoringDB.Twitter_User_Details on tweet.TwitterUserID equals user.TwitterUserID
                        where (c.ID.ToString() == personID1 || c.ID.ToString() == personID2 || c.ID.ToString() == personID3) && a.IsDeleted != true
                        orderby tweet.CreatedAt descending
                        select new Models.Twitter
                        {
                            ID = tweet.ID,
                            TwitterUserID = tweet.TwitterUserID,
                            TweetID = tweet.TweetID,
                            CreatedAt = tweet.CreatedAt,
                            ScreenName = user.ScreenName,
                            Source = tweet.Source,
                            StatusID = tweet.StatusID,
                            RetweetCount = tweet.RetweetCount,
                            Retweeted = tweet.Retweeted,
                            FullText = tweet.FullText,
                            Language = tweet.Language,
                            Text = tweet.Text,
                            TrimUser = tweet.TrimUser,
                            Truncated = tweet.Truncated,
                            UserID = tweet.UserID,
                            HashEntity1 = tweet.HashEntity1,
                            HashEntity2 = tweet.HashEntity2,
                            HashEntity3 = tweet.HashEntity3,
                            SymbolEntity1 = tweet.SymbolEntity1,
                            SymbolEntity2 = tweet.SymbolEntity2,
                            SymbolEntity3 = tweet.SymbolEntity3,
                            UrlEntity1 = tweet.UrlEntity1,
                            UrlEntityDisplayUrl1 = tweet.UrlEntityDisplayUrl1,
                            UrlEntityExpandedUrl1 = tweet.UrlEntityExpandedUrl1,
                            UrlEntity2 = tweet.UrlEntity2,
                            UrlEntityDisplayUrl2 = tweet.UrlEntityDisplayUrl2,
                            UrlEntityExpandedUrl2 = tweet.UrlEntityExpandedUrl2,
                            UrlEntity3 = tweet.UrlEntity3,
                            UrlEntityDisplayUrl3 = tweet.UrlEntityDisplayUrl3,
                            UrlEntityExpandedUrl3 = tweet.UrlEntityExpandedUrl3,
                            MediaEntitiy1 = tweet.MediaEntitiy1,
                            MediaEntitiyDisplayUrl1 = tweet.MediaEntitiyDisplayUrl1,
                            MediaEntitiyMediaUrl1 = tweet.MediaEntitiyMediaUrl1,
                            MediaEntitiyVideoInfo1 = tweet.MediaEntitiyVideoInfo1,
                            MediaEntity2 = tweet.MediaEntity2,
                            MediaEntitiyDisplayUrl2 = tweet.MediaEntitiyDisplayUrl2,
                            MediaEntitiyMediaUrl2 = tweet.MediaEntitiyMediaUrl2,
                            MediaEntitiyVideoInfo2 = tweet.MediaEntitiyVideoInfo2,
                            MediaEntity3 = tweet.MediaEntity3,
                            MediaEntitiyDisplayUrl3 = tweet.MediaEntitiyDisplayUrl3,
                            MediaEntitiyMediaUrl3 = tweet.MediaEntitiyMediaUrl3,
                            MediaEntitiyVideoInfo3 = tweet.MediaEntitiyVideoInfo3,
                            UserMentionEntity1 = tweet.UserMentionEntity1,
                            UserMentionEntityScreenName1 = tweet.UserMentionEntityScreenName1,
                            UserMentionEntity2 = tweet.UserMentionEntity2,
                            UserMentionEntityScreenName2 = tweet.UserMentionEntityScreenName2,
                            UserMentionEntity3 = tweet.UserMentionEntity3,
                            UserMentionEntityScreenName3 = tweet.UserMentionEntityScreenName3,
                            Sentiment = a.Sentiment,
                            //user
                            ContributorsEnabled = user.ContributorsEnabled,
                            Count = user.Count,
                            Cursor = user.Cursor,
                            DefaultProfile = user.DefaultProfile,
                            DefaultProfileImage = user.DefaultProfileImage,
                            Description = user.Description,
                            Email = user.Email,
                            FavoritesCount = user.FavoritesCount,
                            FollowRequestSent = user.FollowRequestSent,
                            FollowersCount = user.FollowersCount,
                            Following = user.Following,
                            FriendsCount = user.FriendsCount,
                            GeoEnabled = user.GeoEnabled,
                            ImageSize = user.ImageSize,
                            IncludeEntities = user.IncludeEntities,
                            IsTranslator = user.IsTranslator,
                            Lang = user.Lang,
                            LangResponse = user.LangResponse,
                            ListedCount = user.ListedCount,
                            Location = user.Location,
                            Name = user.Name,
                            Notifications = user.Notifications,
                            Page = user.Page,
                            ProfileBackgroundColor = user.ProfileBackgroundColor,
                            ProfileBackgroundImageUrl = user.ProfileBackgroundImageUrl,
                            ProfileBackgroundImageUrlHttps = user.ProfileBackgroundImageUrlHttps,
                            ProfileBackgroundTile = user.ProfileBackgroundTile,
                            ProfileBannerUrl = user.ProfileBannerUrl,
                            ProfileImage = user.ProfileImage,
                            ProfileImageUrl = user.ProfileImageUrl,
                            ProfileImageUrlHttps = user.ProfileImageUrlHttps,
                            ProfileLinkColor = user.ProfileLinkColor,
                            ProfileSidebarBorderColor = user.ProfileSidebarBorderColor,
                            ProfileSidebarFillColor = user.ProfileSidebarFillColor,
                            ProfileTextColor = user.ProfileTextColor,
                            ProfileUseBackgroundImage = user.ProfileUseBackgroundImage,
                            Protected = user.Protected,
                            Query = user.Query,
                            ScreenNameList = user.ScreenNameList,
                            ScreenNameResponse = user.ScreenNameResponse,
                            ShowAllInlineMedia = user.ShowAllInlineMedia,
                            SkipStatus = user.SkipStatus,
                            Slug = user.Slug,
                            StatusesCount = user.StatusesCount,
                            TimeZone = user.TimeZone,
                            Type = user.Type,
                            Url = user.Url,
                            UserIDResponse = user.UserIDResponse,
                            UserIdList = user.UserIdList,
                            UtcOffset = user.UtcOffset,
                            Verified = user.Verified

                            //,
                            //SortOrder = b.SortOrder
                        }).Take(200).ToList();

                    ViewBag.WebSiteData =
                      (from a in monitoringDB.Website_Post_Person
                       join b in monitoringDB.WebSite_Posts on a.PostID equals b.ID
                       join c in monitoringDB.System_Person on a.PersonID equals c.ID
                       where (c.ID.ToString() == personID1 || c.ID.ToString() == personID2 || c.ID.ToString() == personID3) && a.IsDeleted != true
                       orderby b.Date descending
                       select new Models.WebSitePost
                       {
                           ID = b.ID,
                           Link = b.Link,
                           Title = b.Title,
                           Text = b.Text,
                           Sentiment = a.Sentiment,
                           Url = b.Url,
                           Body = b.Body.Substring(0, 300) + "...",
                           Reporter = b.Reporter,
                           CoverUrl = b.CoverUrl,
                           DateTime = b.DateTime
                       }).Take(200).ToList();
                }
                else
                {
                    ViewBag.FbData =
                                    (from a in monitoringDB.Facebook_Post_Person
                                     join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
                                     join c in monitoringDB.System_Person on a.PersonID equals c.ID
                                     where (c.ID.ToString() == personID1 || c.ID.ToString() == personID2 || c.ID.ToString() == personID3) && a.Sentiment == Sentiment && a.IsDeleted != true
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
                                         Name = b.Name,
                                         Picture = b.Picture,
                                         Icon = b.Icon,
                                         ObjectID = b.ObjectID,
                                         ParentID = b.ParentID,
                                         PerName = b.Name,
                                         Sentiment = a.Sentiment
                                     }).Take(200).ToList();

                    ViewBag.TwitterData =
                       (from a in monitoringDB.Twitter_Tweet_Person
                        join tweet in monitoringDB.Twitter_Tweets on a.TweetID equals tweet.ID
                        join c in monitoringDB.System_Person on a.PersonID equals c.ID
                        join user in monitoringDB.Twitter_User_Details on tweet.TwitterUserID equals user.TwitterUserID
                        where (c.ID.ToString() == personID1 || c.ID.ToString() == personID2 || c.ID.ToString() == personID3) & a.Sentiment == Sentiment && a.IsDeleted != true
                        orderby tweet.CreatedAt descending
                        select new Models.Twitter
                        {
                            ID = tweet.ID,
                            TwitterUserID = tweet.TwitterUserID,
                            TweetID = tweet.TweetID,
                            CreatedAt = tweet.CreatedAt,
                            ScreenName = user.ScreenName,
                            Source = tweet.Source,
                            StatusID = tweet.StatusID,
                            RetweetCount = tweet.RetweetCount,
                            Retweeted = tweet.Retweeted,
                            FullText = tweet.FullText,
                            Language = tweet.Language,
                            Text = tweet.Text,
                            TrimUser = tweet.TrimUser,
                            Truncated = tweet.Truncated,
                            UserID = tweet.UserID,
                            HashEntity1 = tweet.HashEntity1,
                            HashEntity2 = tweet.HashEntity2,
                            HashEntity3 = tweet.HashEntity3,
                            SymbolEntity1 = tweet.SymbolEntity1,
                            SymbolEntity2 = tweet.SymbolEntity2,
                            SymbolEntity3 = tweet.SymbolEntity3,
                            UrlEntity1 = tweet.UrlEntity1,
                            UrlEntityDisplayUrl1 = tweet.UrlEntityDisplayUrl1,
                            UrlEntityExpandedUrl1 = tweet.UrlEntityExpandedUrl1,
                            UrlEntity2 = tweet.UrlEntity2,
                            UrlEntityDisplayUrl2 = tweet.UrlEntityDisplayUrl2,
                            UrlEntityExpandedUrl2 = tweet.UrlEntityExpandedUrl2,
                            UrlEntity3 = tweet.UrlEntity3,
                            UrlEntityDisplayUrl3 = tweet.UrlEntityDisplayUrl3,
                            UrlEntityExpandedUrl3 = tweet.UrlEntityExpandedUrl3,
                            MediaEntitiy1 = tweet.MediaEntitiy1,
                            MediaEntitiyDisplayUrl1 = tweet.MediaEntitiyDisplayUrl1,
                            MediaEntitiyMediaUrl1 = tweet.MediaEntitiyMediaUrl1,
                            MediaEntitiyVideoInfo1 = tweet.MediaEntitiyVideoInfo1,
                            MediaEntity2 = tweet.MediaEntity2,
                            MediaEntitiyDisplayUrl2 = tweet.MediaEntitiyDisplayUrl2,
                            MediaEntitiyMediaUrl2 = tweet.MediaEntitiyMediaUrl2,
                            MediaEntitiyVideoInfo2 = tweet.MediaEntitiyVideoInfo2,
                            MediaEntity3 = tweet.MediaEntity3,
                            MediaEntitiyDisplayUrl3 = tweet.MediaEntitiyDisplayUrl3,
                            MediaEntitiyMediaUrl3 = tweet.MediaEntitiyMediaUrl3,
                            MediaEntitiyVideoInfo3 = tweet.MediaEntitiyVideoInfo3,
                            UserMentionEntity1 = tweet.UserMentionEntity1,
                            UserMentionEntityScreenName1 = tweet.UserMentionEntityScreenName1,
                            UserMentionEntity2 = tweet.UserMentionEntity2,
                            UserMentionEntityScreenName2 = tweet.UserMentionEntityScreenName2,
                            UserMentionEntity3 = tweet.UserMentionEntity3,
                            UserMentionEntityScreenName3 = tweet.UserMentionEntityScreenName3,
                            Sentiment = a.Sentiment,
                            //user
                            ContributorsEnabled = user.ContributorsEnabled,
                            Count = user.Count,
                            Cursor = user.Cursor,
                            DefaultProfile = user.DefaultProfile,
                            DefaultProfileImage = user.DefaultProfileImage,
                            Description = user.Description,
                            Email = user.Email,
                            FavoritesCount = user.FavoritesCount,
                            FollowRequestSent = user.FollowRequestSent,
                            FollowersCount = user.FollowersCount,
                            Following = user.Following,
                            FriendsCount = user.FriendsCount,
                            GeoEnabled = user.GeoEnabled,
                            ImageSize = user.ImageSize,
                            IncludeEntities = user.IncludeEntities,
                            IsTranslator = user.IsTranslator,
                            Lang = user.Lang,
                            LangResponse = user.LangResponse,
                            ListedCount = user.ListedCount,
                            Location = user.Location,
                            Name = user.Name,
                            Notifications = user.Notifications,
                            Page = user.Page,
                            ProfileBackgroundColor = user.ProfileBackgroundColor,
                            ProfileBackgroundImageUrl = user.ProfileBackgroundImageUrl,
                            ProfileBackgroundImageUrlHttps = user.ProfileBackgroundImageUrlHttps,
                            ProfileBackgroundTile = user.ProfileBackgroundTile,
                            ProfileBannerUrl = user.ProfileBannerUrl,
                            ProfileImage = user.ProfileImage,
                            ProfileImageUrl = user.ProfileImageUrl,
                            ProfileImageUrlHttps = user.ProfileImageUrlHttps,
                            ProfileLinkColor = user.ProfileLinkColor,
                            ProfileSidebarBorderColor = user.ProfileSidebarBorderColor,
                            ProfileSidebarFillColor = user.ProfileSidebarFillColor,
                            ProfileTextColor = user.ProfileTextColor,
                            ProfileUseBackgroundImage = user.ProfileUseBackgroundImage,
                            Protected = user.Protected,
                            Query = user.Query,
                            ScreenNameList = user.ScreenNameList,
                            ScreenNameResponse = user.ScreenNameResponse,
                            ShowAllInlineMedia = user.ShowAllInlineMedia,
                            SkipStatus = user.SkipStatus,
                            Slug = user.Slug,
                            StatusesCount = user.StatusesCount,
                            TimeZone = user.TimeZone,
                            Type = user.Type,
                            Url = user.Url,
                            UserIDResponse = user.UserIDResponse,
                            UserIdList = user.UserIdList,
                            UtcOffset = user.UtcOffset,
                            Verified = user.Verified
                        }).Take(200).ToList();

                    ViewBag.WebSiteData =
                      (from a in monitoringDB.Website_Post_Person
                       join b in monitoringDB.WebSite_Posts on a.PostID equals b.ID
                       join c in monitoringDB.System_Person on a.PersonID equals c.ID
                       where (c.ID.ToString() == personID1 || c.ID.ToString() == personID2 || c.ID.ToString() == personID3) & a.Sentiment == Sentiment && a.IsDeleted != true
                       orderby b.Date descending
                       select new Models.WebSitePost
                       {
                           ID = b.ID,
                           Link = b.Link,
                           Title = b.Title,
                           Text = b.Text,
                           Sentiment = a.Sentiment,
                           Url = b.Url,
                           Body = b.Body.Substring(0, 300) + "...",
                           Reporter = b.Reporter,
                           CoverUrl = b.CoverUrl,
                           DateTime = b.DateTime
                       }).Take(200).ToList();
                }
            }
            else if (Filter == "Count")
            {
                if (Sentiment == "All" || Sentiment == "Default")
                {
                    ViewBag.FbData =
                                    (from a in monitoringDB.Facebook_Post_Person
                                     join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
                                     join c in monitoringDB.System_Person on a.PersonID equals c.ID
                                     where (c.ID.ToString() == personID1 || c.ID.ToString() == personID2 || c.ID.ToString() == personID3) && a.IsDeleted != true
                                     orderby b.SharedCount descending
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
                                         Name = b.Name,
                                         Picture = b.Picture,
                                         Icon = b.Icon,
                                         ObjectID = b.ObjectID,
                                         ParentID = b.ParentID,
                                         PerName = b.Name,
                                         Sentiment = a.Sentiment

                                     }).Take(200).ToList();

                    ViewBag.TwitterData =
                       (from a in monitoringDB.Twitter_Tweet_Person
                        join tweet in monitoringDB.Twitter_Tweets on a.TweetID equals tweet.ID
                        join c in monitoringDB.System_Person on a.PersonID equals c.ID
                        join user in monitoringDB.Twitter_User_Details on tweet.TwitterUserID equals user.TwitterUserID
                        where (c.ID.ToString() == personID1 || c.ID.ToString() == personID2 || c.ID.ToString() == personID3) && a.IsDeleted != true
                        orderby tweet.RetweetCount descending
                        select new Models.Twitter
                        {
                            ID = tweet.ID,
                            TwitterUserID = tweet.TwitterUserID,
                            TweetID = tweet.TweetID,
                            CreatedAt = tweet.CreatedAt,
                            ScreenName = user.ScreenName,
                            Source = tweet.Source,
                            StatusID = tweet.StatusID,
                            RetweetCount = tweet.RetweetCount,
                            Retweeted = tweet.Retweeted,
                            FullText = tweet.FullText,
                            Language = tweet.Language,
                            Text = tweet.Text,
                            TrimUser = tweet.TrimUser,
                            Truncated = tweet.Truncated,
                            UserID = tweet.UserID,
                            HashEntity1 = tweet.HashEntity1,
                            HashEntity2 = tweet.HashEntity2,
                            HashEntity3 = tweet.HashEntity3,
                            SymbolEntity1 = tweet.SymbolEntity1,
                            SymbolEntity2 = tweet.SymbolEntity2,
                            SymbolEntity3 = tweet.SymbolEntity3,
                            UrlEntity1 = tweet.UrlEntity1,
                            UrlEntityDisplayUrl1 = tweet.UrlEntityDisplayUrl1,
                            UrlEntityExpandedUrl1 = tweet.UrlEntityExpandedUrl1,
                            UrlEntity2 = tweet.UrlEntity2,
                            UrlEntityDisplayUrl2 = tweet.UrlEntityDisplayUrl2,
                            UrlEntityExpandedUrl2 = tweet.UrlEntityExpandedUrl2,
                            UrlEntity3 = tweet.UrlEntity3,
                            UrlEntityDisplayUrl3 = tweet.UrlEntityDisplayUrl3,
                            UrlEntityExpandedUrl3 = tweet.UrlEntityExpandedUrl3,
                            MediaEntitiy1 = tweet.MediaEntitiy1,
                            MediaEntitiyDisplayUrl1 = tweet.MediaEntitiyDisplayUrl1,
                            MediaEntitiyMediaUrl1 = tweet.MediaEntitiyMediaUrl1,
                            MediaEntitiyVideoInfo1 = tweet.MediaEntitiyVideoInfo1,
                            MediaEntity2 = tweet.MediaEntity2,
                            MediaEntitiyDisplayUrl2 = tweet.MediaEntitiyDisplayUrl2,
                            MediaEntitiyMediaUrl2 = tweet.MediaEntitiyMediaUrl2,
                            MediaEntitiyVideoInfo2 = tweet.MediaEntitiyVideoInfo2,
                            MediaEntity3 = tweet.MediaEntity3,
                            MediaEntitiyDisplayUrl3 = tweet.MediaEntitiyDisplayUrl3,
                            MediaEntitiyMediaUrl3 = tweet.MediaEntitiyMediaUrl3,
                            MediaEntitiyVideoInfo3 = tweet.MediaEntitiyVideoInfo3,
                            UserMentionEntity1 = tweet.UserMentionEntity1,
                            UserMentionEntityScreenName1 = tweet.UserMentionEntityScreenName1,
                            UserMentionEntity2 = tweet.UserMentionEntity2,
                            UserMentionEntityScreenName2 = tweet.UserMentionEntityScreenName2,
                            UserMentionEntity3 = tweet.UserMentionEntity3,
                            UserMentionEntityScreenName3 = tweet.UserMentionEntityScreenName3,
                            Sentiment = a.Sentiment,
                            //user
                            ContributorsEnabled = user.ContributorsEnabled,
                            Count = user.Count,
                            Cursor = user.Cursor,
                            DefaultProfile = user.DefaultProfile,
                            DefaultProfileImage = user.DefaultProfileImage,
                            Description = user.Description,
                            Email = user.Email,
                            FavoritesCount = user.FavoritesCount,
                            FollowRequestSent = user.FollowRequestSent,
                            FollowersCount = user.FollowersCount,
                            Following = user.Following,
                            FriendsCount = user.FriendsCount,
                            GeoEnabled = user.GeoEnabled,
                            ImageSize = user.ImageSize,
                            IncludeEntities = user.IncludeEntities,
                            IsTranslator = user.IsTranslator,
                            Lang = user.Lang,
                            LangResponse = user.LangResponse,
                            ListedCount = user.ListedCount,
                            Location = user.Location,
                            Name = user.Name,
                            Notifications = user.Notifications,
                            Page = user.Page,
                            ProfileBackgroundColor = user.ProfileBackgroundColor,
                            ProfileBackgroundImageUrl = user.ProfileBackgroundImageUrl,
                            ProfileBackgroundImageUrlHttps = user.ProfileBackgroundImageUrlHttps,
                            ProfileBackgroundTile = user.ProfileBackgroundTile,
                            ProfileBannerUrl = user.ProfileBannerUrl,
                            ProfileImage = user.ProfileImage,
                            ProfileImageUrl = user.ProfileImageUrl,
                            ProfileImageUrlHttps = user.ProfileImageUrlHttps,
                            ProfileLinkColor = user.ProfileLinkColor,
                            ProfileSidebarBorderColor = user.ProfileSidebarBorderColor,
                            ProfileSidebarFillColor = user.ProfileSidebarFillColor,
                            ProfileTextColor = user.ProfileTextColor,
                            ProfileUseBackgroundImage = user.ProfileUseBackgroundImage,
                            Protected = user.Protected,
                            Query = user.Query,
                            ScreenNameList = user.ScreenNameList,
                            ScreenNameResponse = user.ScreenNameResponse,
                            ShowAllInlineMedia = user.ShowAllInlineMedia,
                            SkipStatus = user.SkipStatus,
                            Slug = user.Slug,
                            StatusesCount = user.StatusesCount,
                            TimeZone = user.TimeZone,
                            Type = user.Type,
                            Url = user.Url,
                            UserIDResponse = user.UserIDResponse,
                            UserIdList = user.UserIdList,
                            UtcOffset = user.UtcOffset,
                            Verified = user.Verified

                            //,
                            //SortOrder = b.SortOrder
                        }).Take(200).ToList();

                    ViewBag.WebSiteData =
                      (from a in monitoringDB.Website_Post_Person
                       join b in monitoringDB.WebSite_Posts on a.PostID equals b.ID
                       join c in monitoringDB.System_Person on a.PersonID equals c.ID
                       where (c.ID.ToString() == personID1 || c.ID.ToString() == personID2 || c.ID.ToString() == personID3) && a.IsDeleted != true
                       orderby b.CommentCount descending
                       select new Models.WebSitePost
                       {
                           ID = b.ID,
                           Link = b.Link,
                           Title = b.Title,
                           Text = b.Text,
                           Sentiment = a.Sentiment,
                           Url = b.Url,
                           Body = b.Body.Substring(0, 300) + "...",
                           Reporter = b.Reporter,
                           CoverUrl = b.CoverUrl,
                           DateTime = b.DateTime
                       }).Take(200).ToList();
                }
                else
                {
                    ViewBag.FbData =
                                    (from a in monitoringDB.Facebook_Post_Person
                                     join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
                                     join c in monitoringDB.System_Person on a.PersonID equals c.ID
                                     where (c.ID.ToString() == personID1 || c.ID.ToString() == personID2 || c.ID.ToString() == personID3) & a.Sentiment == Sentiment && a.IsDeleted != true
                                     orderby b.SharedCount descending
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
                                         Name = b.Name,
                                         Picture = b.Picture,
                                         Icon = b.Icon,
                                         ObjectID = b.ObjectID,
                                         ParentID = b.ParentID,
                                         PerName = b.Name,
                                         Sentiment = a.Sentiment
                                     }).Take(200).ToList();

                    ViewBag.TwitterData =
                       (from a in monitoringDB.Twitter_Tweet_Person
                        join tweet in monitoringDB.Twitter_Tweets on a.TweetID equals tweet.ID
                        join c in monitoringDB.System_Person on a.PersonID equals c.ID
                        join user in monitoringDB.Twitter_User_Details on tweet.TwitterUserID equals user.TwitterUserID
                        where (c.ID.ToString() == personID1 || c.ID.ToString() == personID2 || c.ID.ToString() == personID3) & a.Sentiment == Sentiment && a.IsDeleted != true
                        orderby tweet.RetweetCount descending
                        select new Models.Twitter
                        {
                            ID = tweet.ID,
                            TwitterUserID = tweet.TwitterUserID,
                            TweetID = tweet.TweetID,
                            CreatedAt = tweet.CreatedAt,
                            ScreenName = user.ScreenName,
                            Source = tweet.Source,
                            StatusID = tweet.StatusID,
                            RetweetCount = tweet.RetweetCount,
                            Retweeted = tweet.Retweeted,
                            FullText = tweet.FullText,
                            Language = tweet.Language,
                            Text = tweet.Text,
                            TrimUser = tweet.TrimUser,
                            Truncated = tweet.Truncated,
                            UserID = tweet.UserID,
                            HashEntity1 = tweet.HashEntity1,
                            HashEntity2 = tweet.HashEntity2,
                            HashEntity3 = tweet.HashEntity3,
                            SymbolEntity1 = tweet.SymbolEntity1,
                            SymbolEntity2 = tweet.SymbolEntity2,
                            SymbolEntity3 = tweet.SymbolEntity3,
                            UrlEntity1 = tweet.UrlEntity1,
                            UrlEntityDisplayUrl1 = tweet.UrlEntityDisplayUrl1,
                            UrlEntityExpandedUrl1 = tweet.UrlEntityExpandedUrl1,
                            UrlEntity2 = tweet.UrlEntity2,
                            UrlEntityDisplayUrl2 = tweet.UrlEntityDisplayUrl2,
                            UrlEntityExpandedUrl2 = tweet.UrlEntityExpandedUrl2,
                            UrlEntity3 = tweet.UrlEntity3,
                            UrlEntityDisplayUrl3 = tweet.UrlEntityDisplayUrl3,
                            UrlEntityExpandedUrl3 = tweet.UrlEntityExpandedUrl3,
                            MediaEntitiy1 = tweet.MediaEntitiy1,
                            MediaEntitiyDisplayUrl1 = tweet.MediaEntitiyDisplayUrl1,
                            MediaEntitiyMediaUrl1 = tweet.MediaEntitiyMediaUrl1,
                            MediaEntitiyVideoInfo1 = tweet.MediaEntitiyVideoInfo1,
                            MediaEntity2 = tweet.MediaEntity2,
                            MediaEntitiyDisplayUrl2 = tweet.MediaEntitiyDisplayUrl2,
                            MediaEntitiyMediaUrl2 = tweet.MediaEntitiyMediaUrl2,
                            MediaEntitiyVideoInfo2 = tweet.MediaEntitiyVideoInfo2,
                            MediaEntity3 = tweet.MediaEntity3,
                            MediaEntitiyDisplayUrl3 = tweet.MediaEntitiyDisplayUrl3,
                            MediaEntitiyMediaUrl3 = tweet.MediaEntitiyMediaUrl3,
                            MediaEntitiyVideoInfo3 = tweet.MediaEntitiyVideoInfo3,
                            UserMentionEntity1 = tweet.UserMentionEntity1,
                            UserMentionEntityScreenName1 = tweet.UserMentionEntityScreenName1,
                            UserMentionEntity2 = tweet.UserMentionEntity2,
                            UserMentionEntityScreenName2 = tweet.UserMentionEntityScreenName2,
                            UserMentionEntity3 = tweet.UserMentionEntity3,
                            UserMentionEntityScreenName3 = tweet.UserMentionEntityScreenName3,
                            Sentiment = a.Sentiment,
                            //user
                            ContributorsEnabled = user.ContributorsEnabled,
                            Count = user.Count,
                            Cursor = user.Cursor,
                            DefaultProfile = user.DefaultProfile,
                            DefaultProfileImage = user.DefaultProfileImage,
                            Description = user.Description,
                            Email = user.Email,
                            FavoritesCount = user.FavoritesCount,
                            FollowRequestSent = user.FollowRequestSent,
                            FollowersCount = user.FollowersCount,
                            Following = user.Following,
                            FriendsCount = user.FriendsCount,
                            GeoEnabled = user.GeoEnabled,
                            ImageSize = user.ImageSize,
                            IncludeEntities = user.IncludeEntities,
                            IsTranslator = user.IsTranslator,
                            Lang = user.Lang,
                            LangResponse = user.LangResponse,
                            ListedCount = user.ListedCount,
                            Location = user.Location,
                            Name = user.Name,
                            Notifications = user.Notifications,
                            Page = user.Page,
                            ProfileBackgroundColor = user.ProfileBackgroundColor,
                            ProfileBackgroundImageUrl = user.ProfileBackgroundImageUrl,
                            ProfileBackgroundImageUrlHttps = user.ProfileBackgroundImageUrlHttps,
                            ProfileBackgroundTile = user.ProfileBackgroundTile,
                            ProfileBannerUrl = user.ProfileBannerUrl,
                            ProfileImage = user.ProfileImage,
                            ProfileImageUrl = user.ProfileImageUrl,
                            ProfileImageUrlHttps = user.ProfileImageUrlHttps,
                            ProfileLinkColor = user.ProfileLinkColor,
                            ProfileSidebarBorderColor = user.ProfileSidebarBorderColor,
                            ProfileSidebarFillColor = user.ProfileSidebarFillColor,
                            ProfileTextColor = user.ProfileTextColor,
                            ProfileUseBackgroundImage = user.ProfileUseBackgroundImage,
                            Protected = user.Protected,
                            Query = user.Query,
                            ScreenNameList = user.ScreenNameList,
                            ScreenNameResponse = user.ScreenNameResponse,
                            ShowAllInlineMedia = user.ShowAllInlineMedia,
                            SkipStatus = user.SkipStatus,
                            Slug = user.Slug,
                            StatusesCount = user.StatusesCount,
                            TimeZone = user.TimeZone,
                            Type = user.Type,
                            Url = user.Url,
                            UserIDResponse = user.UserIDResponse,
                            UserIdList = user.UserIdList,
                            UtcOffset = user.UtcOffset,
                            Verified = user.Verified
                        }).Take(200).ToList();

                    ViewBag.WebSiteData =
                      (from a in monitoringDB.Website_Post_Person
                       join b in monitoringDB.WebSite_Posts on a.PostID equals b.ID
                       join c in monitoringDB.System_Person on a.PersonID equals c.ID
                       where (c.ID.ToString() == personID1 || c.ID.ToString() == personID2 || c.ID.ToString() == personID3) & a.Sentiment == Sentiment && a.IsDeleted != true
                       orderby b.CommentCount descending
                       select new Models.WebSitePost
                       {
                           ID = b.ID,
                           Link = b.Link,
                           Title = b.Title,
                           Text = b.Text,
                           Sentiment = a.Sentiment,
                           Url = b.Url,
                           Body = b.Body.Substring(0, 300) + "...",
                           Reporter = b.Reporter,
                           CoverUrl = b.CoverUrl,
                           DateTime = b.DateTime
                       }).Take(200).ToList();
                }
            }
            return PartialView();
        }


        public ActionResult Person1Info(string PeopleID, string Date)
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();
            DateTime dt = DateTime.Now;

            DateTime dt1 = DateTime.Now;
            switch (Date)
            {
                case "1":
                    dt1 = dt.AddDays(-1);
                    break;

                case "5":
                    dt1 = dt.AddDays(-5);
                    break;

                case "7":
                    dt1 = dt.AddDays(-7);
                    break;
                case "10":
                    dt1 = dt.AddDays(-10);
                    break;
                case "14":
                    dt1 = dt.AddDays(-14);
                    break;
                case "m":
                    dt1 = dt.AddMonths(-1);
                    break;
                case "y":
                    dt1 = dt.AddYears(-1);
                    break;
                case "0":
                    dt1 = dt.AddYears(-100);
                    break;
                default:
                    dt1 = dt.AddYears(-100);
                    break;
            }


            //Positive
            var fbDataPositive =
                 (from a in monitoringDB.Facebook_Post_Person
                  join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
                  join c in monitoringDB.System_Person on a.PersonID equals c.ID
                  where (c.ID.ToString() == PeopleID & a.Sentiment == "Positive" & b.UpdateTime >= dt1 && b.UpdateTime <= dt) && a.IsDeleted != true
                  select new Models.fbModel
                  {
                      ID = b.ID,
                      Text = b.FromName,
                      Date = b.UpdateTime
                  }).ToList();

            var twDataPositive =
                    (from d in monitoringDB.Twitter_Tweet_Person
                     join e in monitoringDB.Twitter_Tweets on d.TweetID equals e.ID
                     join f in monitoringDB.System_Person on d.PersonID equals f.ID
                     where (f.ID.ToString() == PeopleID & d.Sentiment == "Positive" & e.CreatedAt >= dt1 && e.CreatedAt <= dt) && d.IsDeleted != true
                     select new Models.fbModel
                     {
                         ID = e.ID,
                         Text = e.Text,
                         Date = e.CreatedAt,
                     }).ToList();

            var webDataPositive =
                    (from g in monitoringDB.Website_Post_Person
                     join w in monitoringDB.WebSite_Posts on g.PostID equals w.ID
                     join r in monitoringDB.System_Person on g.PersonID equals r.ID
                     where (r.ID.ToString() == PeopleID & g.Sentiment == "Positive" & w.Date >= dt1 && w.Date <= dt) && g.IsDeleted != true
                     select new Models.fbModel
                     {
                         ID = g.ID,
                         Text = w.Text,
                         Date = w.Date
                     }).ToList();

            //Negative
            var fbDataNegative =
                 (from a in monitoringDB.Facebook_Post_Person
                  join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
                  join c in monitoringDB.System_Person on a.PersonID equals c.ID
                  where (c.ID.ToString() == PeopleID & a.Sentiment == "Negative" & b.UpdateTime >= dt1 && b.UpdateTime <= dt) && a.IsDeleted != true
                  select new Models.fbModel
                  {
                      ID = b.ID,
                      Text = b.FromName,
                      Date = b.UpdateTime
                  }).ToList();

            var twDataNegative =
                    (from d in monitoringDB.Twitter_Tweet_Person
                     join e in monitoringDB.Twitter_Tweets on d.TweetID equals e.ID
                     join f in monitoringDB.System_Person on d.PersonID equals f.ID
                     where (f.ID.ToString() == PeopleID & d.Sentiment == "Negative" & e.CreatedAt >= dt1 && e.CreatedAt <= dt) && d.IsDeleted != true
                     select new Models.fbModel
                     {
                         ID = e.ID,
                         Text = e.Text,
                         Date = e.CreatedAt,
                     }).ToList();

            var webDataNegative =
                    (from g in monitoringDB.Website_Post_Person
                     join w in monitoringDB.WebSite_Posts on g.PostID equals w.ID
                     join r in monitoringDB.System_Person on g.PersonID equals r.ID
                     where (r.ID.ToString() == PeopleID & g.Sentiment == "Negative" & w.Date >= dt1 && w.Date <= dt) && g.IsDeleted != true
                     select new Models.fbModel
                     {
                         ID = g.ID,
                         Text = w.Text,
                         Date = w.Date
                     }).ToList();

            //Neutral
            var fbDataNeutral =
                 (from a in monitoringDB.Facebook_Post_Person
                  join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
                  join c in monitoringDB.System_Person on a.PersonID equals c.ID
                  where (c.ID.ToString() == PeopleID & a.Sentiment == "Neutral" & b.UpdateTime >= dt1 && b.UpdateTime <= dt) && a.IsDeleted != true
                  select new Models.fbModel
                  {
                      ID = b.ID,
                      Text = b.FromName,
                      Date = b.UpdateTime
                  }).ToList();

            var twDataNeutral =
                    (from d in monitoringDB.Twitter_Tweet_Person
                     join e in monitoringDB.Twitter_Tweets on d.TweetID equals e.ID
                     join f in monitoringDB.System_Person on d.PersonID equals f.ID
                     where (f.ID.ToString() == PeopleID & d.Sentiment == "Neutral" & e.CreatedAt >= dt1 && e.CreatedAt <= dt) && d.IsDeleted != true
                     select new Models.fbModel
                     {
                         ID = e.ID,
                         Text = e.Text,
                         Date = e.CreatedAt,
                     }).ToList();

            var webDataNeutral =
                    (from g in monitoringDB.Website_Post_Person
                     join w in monitoringDB.WebSite_Posts on g.PostID equals w.ID
                     join r in monitoringDB.System_Person on g.PersonID equals r.ID
                     where (r.ID.ToString() == PeopleID & g.Sentiment == "Neutral" & w.Date >= dt1 && w.Date <= dt) && g.IsDeleted != true
                     select new Models.fbModel
                     {
                         ID = g.ID,
                         Text = w.Text,
                         Date = w.Date
                     }).ToList();

            ViewBag.fbkPos = fbDataPositive.Count();
            ViewBag.twtPos = twDataPositive.Count();
            ViewBag.webPos = webDataPositive.Count();

            ViewBag.fbkNeg = fbDataNegative.Count();
            ViewBag.twtNeg = twDataNegative.Count();
            ViewBag.webNeg = webDataNegative.Count();

            ViewBag.fbkNeu = fbDataNeutral.Count();
            ViewBag.twtNeu = twDataNeutral.Count();
            ViewBag.webNeu = webDataNeutral.Count();


            return PartialView();
        }


        public ActionResult Person2Info(string PeopleID, string Date)
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();
            DateTime dt = DateTime.Now;

            DateTime dt1 = DateTime.Now;
            switch (Date)
            {
                case "1":
                    dt1 = dt.AddDays(-1);
                    break;

                case "5":
                    dt1 = dt.AddDays(-5);
                    break;

                case "7":
                    dt1 = dt.AddDays(-7);
                    break;
                case "10":
                    dt1 = dt.AddDays(-10);
                    break;
                case "14":
                    dt1 = dt.AddDays(-14);
                    break;
                case "m":
                    dt1 = dt.AddMonths(-1);
                    break;
                case "y":
                    dt1 = dt.AddYears(-1);
                    break;
                case "0":
                    dt1 = dt.AddYears(-100);
                    break;
                default:
                    dt1 = dt.AddYears(-100);
                    break;
            }

            //Positive
            var fbDataPositive =
                 (from a in monitoringDB.Facebook_Post_Person
                  join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
                  join c in monitoringDB.System_Person on a.PersonID equals c.ID
                  where (a.PersonID.ToString() == PeopleID & a.Sentiment == "Positive" & b.UpdateTime >= dt1 && b.UpdateTime <= dt) && a.IsDeleted != true
                  select new Models.fbModel
                  {
                      ID = b.ID,
                      Text = b.FromName,
                      Date = b.UpdateTime
                  }).ToList();

            var twDataPositive =
                    (from d in monitoringDB.Twitter_Tweet_Person
                     join e in monitoringDB.Twitter_Tweets on d.TweetID equals e.ID
                     join f in monitoringDB.System_Person on d.PersonID equals f.ID
                     where (d.PersonID.ToString() == PeopleID & d.Sentiment == "Positive" & e.CreatedAt >= dt1 && e.CreatedAt <= dt) && d.IsDeleted != true
                     select new Models.fbModel
                     {
                         ID = e.ID,
                         Text = e.Text,
                         Date = e.CreatedAt,
                     }).ToList();

            var webDataPositive =
                    (from g in monitoringDB.Website_Post_Person
                     join w in monitoringDB.WebSite_Posts on g.PostID equals w.ID
                     join r in monitoringDB.System_Person on g.PersonID equals r.ID
                     where (g.PersonID.ToString() == PeopleID & g.Sentiment == "Positive" & w.Date >= dt1 && w.Date <= dt) && g.IsDeleted != true
                     select new Models.fbModel
                     {
                         ID = g.ID,
                         Text = w.Text,
                         Date = w.Date
                     }).ToList();

            //Negative
            var fbDataNegative =
                 (from a in monitoringDB.Facebook_Post_Person
                  join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
                  join c in monitoringDB.System_Person on a.PersonID equals c.ID
                  where (a.PersonID.ToString() == PeopleID & a.Sentiment == "Negative" & b.UpdateTime >= dt1 && b.UpdateTime <= dt) && a.IsDeleted != true
                  select new Models.fbModel
                  {
                      ID = b.ID,
                      Text = b.FromName,
                      Date = b.UpdateTime
                  }).ToList();

            var twDataNegative =
                    (from d in monitoringDB.Twitter_Tweet_Person
                     join e in monitoringDB.Twitter_Tweets on d.TweetID equals e.ID
                     join f in monitoringDB.System_Person on d.PersonID equals f.ID
                     where (d.PersonID.ToString() == PeopleID & d.Sentiment == "Negative" & e.CreatedAt >= dt1 && e.CreatedAt <= dt) && d.IsDeleted != true
                     select new Models.fbModel
                     {
                         ID = e.ID,
                         Text = e.Text,
                         Date = e.CreatedAt,
                     }).ToList();

            var webDataNegative =
                    (from g in monitoringDB.Website_Post_Person
                     join w in monitoringDB.WebSite_Posts on g.PostID equals w.ID
                     join r in monitoringDB.System_Person on g.PersonID equals r.ID
                     where (g.PersonID.ToString() == PeopleID & g.Sentiment == "Negative" & w.Date >= dt1 && w.Date <= dt) && g.IsDeleted != true
                     select new Models.fbModel
                     {
                         ID = g.ID,
                         Text = w.Text,
                         Date = w.Date
                     }).ToList();

            //Neutral
            var fbDataNeutral =
                 (from a in monitoringDB.Facebook_Post_Person
                  join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
                  join c in monitoringDB.System_Person on a.PersonID equals c.ID
                  where (a.PersonID.ToString() == PeopleID & a.Sentiment == "Neutral" & b.UpdateTime >= dt1 && b.UpdateTime <= dt) && a.IsDeleted != true
                  select new Models.fbModel
                  {
                      ID = b.ID,
                      Text = b.FromName,
                      Date = b.UpdateTime
                  }).ToList();

            var twDataNeutral =
                    (from d in monitoringDB.Twitter_Tweet_Person
                     join e in monitoringDB.Twitter_Tweets on d.TweetID equals e.ID
                     join f in monitoringDB.System_Person on d.PersonID equals f.ID
                     where (d.PersonID.ToString() == PeopleID & d.Sentiment == "Neutral" & e.CreatedAt >= dt1 && e.CreatedAt <= dt) && d.IsDeleted != true
                     select new Models.fbModel
                     {
                         ID = e.ID,
                         Text = e.Text,
                         Date = e.CreatedAt,
                     }).ToList();

            var webDataNeutral =
                    (from g in monitoringDB.Website_Post_Person
                     join w in monitoringDB.WebSite_Posts on g.PostID equals w.ID
                     join r in monitoringDB.System_Person on g.PersonID equals r.ID
                     where (g.PersonID.ToString() == PeopleID & g.Sentiment == "Neutral" & w.Date >= dt1 && w.Date <= dt) && g.IsDeleted != true
                     select new Models.fbModel
                     {
                         ID = g.ID,
                         Text = w.Text,
                         Date = w.Date
                     }).ToList();

            ViewBag.fbkPos = fbDataPositive.Count();
            ViewBag.twtPos = twDataPositive.Count();
            ViewBag.webPos = webDataPositive.Count();

            ViewBag.fbkNeg = fbDataNegative.Count();
            ViewBag.twtNeg = twDataNegative.Count();
            ViewBag.webNeg = webDataNegative.Count();

            ViewBag.fbkNeu = fbDataNeutral.Count();
            ViewBag.twtNeu = twDataNeutral.Count();
            ViewBag.webNeu = webDataNeutral.Count();

            return PartialView();
        }


        public ActionResult Person3Info(string PeopleID, string Date)
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();
            DateTime dt = DateTime.Now;

            DateTime dt1 = DateTime.Now;
            switch (Date)
            {
                case "1":
                    dt1 = dt.AddDays(-1);
                    break;

                case "5":
                    dt1 = dt.AddDays(-5);
                    break;

                case "7":
                    dt1 = dt.AddDays(-7);
                    break;
                case "10":
                    dt1 = dt.AddDays(-10);
                    break;
                case "14":
                    dt1 = dt.AddDays(-14);
                    break;
                case "m":
                    dt1 = dt.AddMonths(-1);
                    break;
                case "y":
                    dt1 = dt.AddYears(-1);
                    break;
                case "0":
                    dt1 = dt.AddYears(-100);
                    break;
                default:
                    dt1 = dt.AddYears(-100);
                    break;
            }
            //Positive
            var fbDataPositive =
         (from a in monitoringDB.Facebook_Post_Person
          join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
          join c in monitoringDB.System_Person on a.PersonID equals c.ID
          where (a.PersonID.ToString() == PeopleID & a.Sentiment == "Positive" & b.UpdateTime >= dt1 && b.UpdateTime <= dt) && a.IsDeleted != true
          select new Models.fbModel
          {
              ID = b.ID,
              Text = b.FromName,
              Date = b.UpdateTime
          }).ToList();

            var twDataPositive =
                    (from d in monitoringDB.Twitter_Tweet_Person
                     join e in monitoringDB.Twitter_Tweets on d.TweetID equals e.ID
                     join f in monitoringDB.System_Person on d.PersonID equals f.ID
                     where (d.PersonID.ToString() == PeopleID & d.Sentiment == "Positive" & e.CreatedAt >= dt1 && e.CreatedAt <= dt) && d.IsDeleted != true
                     select new Models.fbModel
                     {
                         ID = e.ID,
                         Text = e.Text,
                         Date = e.CreatedAt,
                     }).ToList();

            var webDataPositive =
                    (from g in monitoringDB.Website_Post_Person
                     join w in monitoringDB.WebSite_Posts on g.PostID equals w.ID
                     join r in monitoringDB.System_Person on g.PersonID equals r.ID
                     where (g.PersonID.ToString() == PeopleID & g.Sentiment == "Positive" & w.Date >= dt1 && w.Date <= dt) && g.IsDeleted != true
                     select new Models.fbModel
                     {
                         ID = g.ID,
                         Text = w.Text,
                         Date = w.Date
                     }).ToList();

            //Negative
            var fbDataNegative =
                 (from a in monitoringDB.Facebook_Post_Person
                  join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
                  join c in monitoringDB.System_Person on a.PersonID equals c.ID
                  where (a.PersonID.ToString() == PeopleID & a.Sentiment == "Negative" & b.UpdateTime >= dt1 && b.UpdateTime <= dt) && a.IsDeleted != true
                  select new Models.fbModel
                  {
                      ID = b.ID,
                      Text = b.FromName,
                      Date = b.UpdateTime
                  }).ToList();

            var twDataNegative =
                    (from d in monitoringDB.Twitter_Tweet_Person
                     join e in monitoringDB.Twitter_Tweets on d.TweetID equals e.ID
                     join f in monitoringDB.System_Person on d.PersonID equals f.ID
                     where (d.PersonID.ToString() == PeopleID & d.Sentiment == "Negative" & e.CreatedAt >= dt1 && e.CreatedAt <= dt) && d.IsDeleted != true
                     select new Models.fbModel
                     {
                         ID = e.ID,
                         Text = e.Text,
                         Date = e.CreatedAt,
                     }).ToList();

            var webDataNegative =
                    (from g in monitoringDB.Website_Post_Person
                     join w in monitoringDB.WebSite_Posts on g.PostID equals w.ID
                     join r in monitoringDB.System_Person on g.PersonID equals r.ID
                     where (g.PersonID.ToString() == PeopleID & g.Sentiment == "Negative" & w.Date >= dt1 && w.Date <= dt) && g.IsDeleted != true
                     select new Models.fbModel
                     {
                         ID = g.ID,
                         Text = w.Text,
                         Date = w.Date
                     }).ToList();

            //Neutral
            var fbDataNeutral =
                 (from a in monitoringDB.Facebook_Post_Person
                  join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
                  join c in monitoringDB.System_Person on a.PersonID equals c.ID
                  where (a.PersonID.ToString() == PeopleID & a.Sentiment == "Neutral" & b.UpdateTime >= dt1 && b.UpdateTime <= dt) && a.IsDeleted != true
                  select new Models.fbModel
                  {
                      ID = b.ID,
                      Text = b.FromName,
                      Date = b.UpdateTime
                  }).ToList();

            var twDataNeutral =
                    (from d in monitoringDB.Twitter_Tweet_Person
                     join e in monitoringDB.Twitter_Tweets on d.TweetID equals e.ID
                     join f in monitoringDB.System_Person on d.PersonID equals f.ID
                     where (d.PersonID.ToString() == PeopleID & d.Sentiment == "Neutral" & e.CreatedAt >= dt1 && e.CreatedAt <= dt) && d.IsDeleted != true
                     select new Models.fbModel
                     {
                         ID = e.ID,
                         Text = e.Text,
                         Date = e.CreatedAt,
                     }).ToList();

            var webDataNeutral =
                    (from g in monitoringDB.Website_Post_Person
                     join w in monitoringDB.WebSite_Posts on g.PostID equals w.ID
                     join r in monitoringDB.System_Person on g.PersonID equals r.ID
                     where (g.PersonID.ToString() == PeopleID & g.Sentiment == "Neutral" & w.Date >= dt1 && w.Date <= dt) && g.IsDeleted != true
                     select new Models.fbModel
                     {
                         ID = g.ID,
                         Text = w.Text,
                         Date = w.Date
                     }).ToList();

            ViewBag.fbkPos = fbDataPositive.Count();
            ViewBag.twtPos = twDataPositive.Count();
            ViewBag.webPos = webDataPositive.Count();

            ViewBag.fbkNeg = fbDataNegative.Count();
            ViewBag.twtNeg = twDataNegative.Count();
            ViewBag.webNeg = webDataNegative.Count();

            ViewBag.fbkNeu = fbDataNeutral.Count();
            ViewBag.twtNeu = twDataNeutral.Count();
            ViewBag.webNeu = webDataNeutral.Count();

            return PartialView();
        }

        public ActionResult pieChartPerson(string per1, string per2, string per3, string Date)
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();
            DateTime dt = DateTime.Now;

            DateTime dt1 = DateTime.Now;
            switch (Date)
            {
                case "1":
                    dt1 = dt.AddDays(-1);
                    break;

                case "5":
                    dt1 = dt.AddDays(-5);
                    break;

                case "7":
                    dt1 = dt.AddDays(-7);
                    break;
                case "10":
                    dt1 = dt.AddDays(-10);
                    break;
                case "14":
                    dt1 = dt.AddDays(-14);
                    break;
                case "m":
                    dt1 = dt.AddMonths(-1);
                    break;
                case "y":
                    dt1 = dt.AddYears(-1);
                    break;
                case "0":
                    dt1 = dt.AddYears(-100);
                    break;
                default:
                    dt1 = dt.AddYears(-100);
                    break;
            }
            //person 1
            var fbData1 =
                 (from a in monitoringDB.Facebook_Post_Person
                  join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
                  join c in monitoringDB.System_Person on a.PersonID equals c.ID
                  where (a.PersonID.ToString() == per1 & b.UpdateTime >= dt1 && b.UpdateTime <= dt) && a.IsDeleted != true
                  select new Models.fbModel
                  {
                      ID = b.ID,
                      Text = b.FromName,
                      Date = b.UpdateTime,
                      PerName = c.Name
                  }).ToList();

            var twData1 =
                    (from d in monitoringDB.Twitter_Tweet_Person
                     join e in monitoringDB.Twitter_Tweets on d.TweetID equals e.ID
                     join f in monitoringDB.System_Person on d.PersonID equals f.ID
                     where (d.PersonID.ToString() == per1 & e.CreatedAt >= dt1 && e.CreatedAt <= dt) && d.IsDeleted != true
                     select new Models.fbModel
                     {
                         ID = e.ID,
                         Text = e.Text,
                         Date = e.CreatedAt,
                         PerName = f.Name
                     }).ToList();

            var webData1 =
                    (from g in monitoringDB.Website_Post_Person
                     join w in monitoringDB.WebSite_Posts on g.PostID equals w.ID
                     join r in monitoringDB.System_Person on g.PersonID equals r.ID
                     where (g.PersonID.ToString() == per1 & w.Date >= dt1 && w.Date <= dt) && g.IsDeleted != true
                     select new Models.fbModel
                     {
                         ID = g.ID,
                         Text = w.Text,
                         Date = w.Date,
                         PerName = r.Name
                     }).ToList();

            var person11 = fbData1.Union(twData1).ToList();
            var person1 = person11.Union(webData1).ToList();
            ViewBag.person1Cnt = person1.Count(); //all data count

            if (fbData1.Count > 0)
            {
                ViewBag.PerName1 = fbData1[0].PerName;
            }
            else if (twData1.Count > 0)
            {
                ViewBag.PerName3 = twData1[0].PerName;
            }
            else if (webData1.Count > 0)
            {
                ViewBag.PerName3 = webData1[0].PerName;
            }
            else
            {
                ViewBag.PerName3 = "";
            }

            ViewBag.fbk1 = fbData1.Count();
            ViewBag.twt1 = twData1.Count();
            ViewBag.web1 = webData1.Count();
            //person 1

            //person 2
            var fbData2 =
                 (from a in monitoringDB.Facebook_Post_Person
                  join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
                  join c in monitoringDB.System_Person on a.PersonID equals c.ID
                  where (c.ID.ToString() == per2 & b.UpdateTime >= dt1 && b.UpdateTime <= dt) && a.IsDeleted != true
                  select new Models.fbModel
                  {
                      ID = b.ID,
                      Text = b.FromName,
                      Date = b.UpdateTime,
                      PerName = c.Name
                  }).ToList();

            var twData2 =
                    (from d in monitoringDB.Twitter_Tweet_Person
                     join e in monitoringDB.Twitter_Tweets on d.TweetID equals e.ID
                     join f in monitoringDB.System_Person on d.PersonID equals f.ID
                     where (f.ID.ToString() == per2 & e.CreatedAt >= dt1 && e.CreatedAt <= dt) && d.IsDeleted != true
                     select new Models.fbModel
                     {
                         ID = e.ID,
                         Text = e.Text,
                         Date = e.CreatedAt,
                         PerName = f.Name
                     }).ToList();

            var webData2 =
                    (from g in monitoringDB.Website_Post_Person
                     join w in monitoringDB.WebSite_Posts on g.PostID equals w.ID
                     join r in monitoringDB.System_Person on g.PersonID equals r.ID
                     where (r.ID.ToString() == per2 & w.Date >= dt1 && w.Date <= dt) && g.IsDeleted != true
                     select new Models.fbModel
                     {
                         ID = g.ID,
                         Text = w.Text,
                         Date = w.Date,
                         PerName = r.Name
                     }).ToList();

            var person22 = fbData2.Union(twData2).ToList();
            var person2 = person22.Union(webData2).ToList();

            ViewBag.person2Cnt = person2.Count(); //all data count

            if (fbData2.Count > 0)
            {
                ViewBag.PerName2 = fbData2[0].PerName;
            }
            else if (twData2.Count > 0)
            {
                ViewBag.PerName2 = twData2[0].PerName;
            }
            else if (webData2.Count > 0)
            {
                ViewBag.PerName3 = webData2[0].PerName;
            }
            else
            {
                ViewBag.PerName3 = "";
            }

            ViewBag.fbk2 = fbData2.Count();
            ViewBag.twt2 = twData2.Count();
            ViewBag.web2 = webData2.Count();
            //person 2

            //person 3
            var fbData3 =
                 (from a in monitoringDB.Facebook_Post_Person
                  join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
                  join c in monitoringDB.System_Person on a.PersonID equals c.ID
                  where (c.ID.ToString() == per3 & b.UpdateTime >= dt1 && b.UpdateTime <= dt) && a.IsDeleted != true
                  select new Models.fbModel
                  {
                      ID = b.ID,
                      Text = b.FromName,
                      Date = b.UpdateTime,
                      PerName = c.Name
                  }).ToList();

            var twData3 =
                    (from d in monitoringDB.Twitter_Tweet_Person
                     join e in monitoringDB.Twitter_Tweets on d.TweetID equals e.ID
                     join f in monitoringDB.System_Person on d.PersonID equals f.ID
                     where (f.ID.ToString() == per3 & e.CreatedAt >= dt1 && e.CreatedAt <= dt) && d.IsDeleted != true
                     select new Models.fbModel
                     {
                         ID = e.ID,
                         Text = e.Text,
                         Date = e.CreatedAt,
                         PerName = f.Name
                     }).ToList();

            var webData3 =
                    (from g in monitoringDB.Website_Post_Person
                     join w in monitoringDB.WebSite_Posts on g.PostID equals w.ID
                     join r in monitoringDB.System_Person on g.PersonID equals r.ID
                     where (r.ID.ToString() == per3 & w.Date >= dt1 && w.Date <= dt) && g.IsDeleted != true
                     select new Models.fbModel
                     {
                         ID = g.ID,
                         Text = w.Text,
                         Date = w.Date,
                         PerName = r.Name
                     }).ToList();

            var person33 = fbData3.Union(twData3).ToList();
            var person3 = person33.Union(webData3).ToList();
            ViewBag.person3Cnt = person3.Count(); //all data count

            if (fbData3.Count > 0)
            {
                ViewBag.PerName3 = fbData3[0].PerName;
            }
            else if (twData3.Count > 0)
            {
                ViewBag.PerName3 = twData3[0].PerName;
            }
            else if (webData3.Count > 0)
            {
                ViewBag.PerName3 = webData3[0].PerName;
            }
            else
            {
                ViewBag.PerName3 = "";
            }

            ViewBag.fbk3 = fbData3.Count();
            ViewBag.twt3 = twData3.Count();
            ViewBag.web3 = webData3.Count();


            //person 3


            return PartialView();
        }




        public class graphResult
        {
            public int count { get; set; }
            public DateTime date { get; set; }
        }

    }
}