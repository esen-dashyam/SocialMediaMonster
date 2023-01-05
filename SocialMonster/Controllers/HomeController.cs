﻿using SocialMonster.DAL;
using SocialMonster.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions;
using System.Data.Entity.Infrastructure;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System.Dynamic;
using System.IO;
using System.Drawing;
using System.Security.Claims;

namespace SocialMonster.Controllers
{
    public class HomeController : Controller
    {
        private const string V = ", ";

        public ActionResult Index_old()
        {
            Session["latestComment"] = null;

            Session["latestTweet"] = null;
            if (User.Identity.IsAuthenticated == false)
            {
                return Redirect("/Account/Login");
            }
            //MonitoringEntities monitoring = new MonitoringEntities();
            //monitoring.Database.CommandTimeout=300;
            ViewBag.Message = "Ухаалаг мэдээ";
            MonitoringEntities monitoringDB = new MonitoringEntities();
            AspNetUserActivityLog userLog = new AspNetUserActivityLog();
            userLog.ID = Guid.NewGuid();
            userLog.UserName = User.Identity.GetUserName();
            userLog.Activity = "In Dashboard";
            userLog.Time = DateTime.Now;
            //monitoringDB.AspNetUserActivityLogs.Add(userLog);
            // monitoringDB.SaveChanges();
            var emailConfirmed = monitoringDB.AspNetUsers.Where(p => p.Email.Equals(User.Identity.Name)).FirstOrDefault().EmailConfirmed;
            if (!emailConfirmed)
            {
                var AuthenticationManager = HttpContext.GetOwinContext().Authentication;
                AuthenticationManager.SignOut();
                return Redirect("/Account/Login");
            }
            monitoringDB.Database.CommandTimeout = 300;
            ((IObjectContextAdapter)monitoringDB).ObjectContext.CommandTimeout = 300;

            //ViewBag.FacebookPostCount = monitoringDB.Facebook_Posts.Count();
            //ViewBag.TwitterTweetCount = monitoringDB.Twitter_Tweets.Count();
            ViewBag.WebPostCount = monitoringDB.WebSite_Posts.Count();
            //ViewBag.CommentCount = monitoringDB.Facebook_Post_Comments.Count();
            ViewBag.TwitterUserCount = monitoringDB.Twitter_User_Details.Count();
            ViewBag.WebsiteCount = monitoringDB.WebSites.Count();
            ViewBag.FbGroupCount = monitoringDB.Facebook_Groups.Count() + monitoringDB.Facebook_Pages.Count();

            var lastWeek = DateTime.Now.AddDays(-3);

            //int facebookPostPositive = monitoringDB.Facebook_Post_Person.Where(p => p.Sentiment.Equals("Positive")).Where(p => p.IsDeleted!= true).Count();
            //int facebookPostNegative = monitoringDB.Facebook_Post_Person.Where(p => p.Sentiment.Equals("Negative")).Where(p => p.IsDeleted != true).Count();

            //int twitterTweetPositive = monitoringDB.Twitter_Tweet_Person.Where(p => p.Sentiment.Equals("Positive")).Where(p => p.IsDeleted != true).Count();
            //int twitterTweetNegative = monitoringDB.Twitter_Tweet_Person.Where(p => p.Sentiment.Equals("Negative")).Where(p => p.IsDeleted != true).Count();

            //int webPostPositive = monitoringDB.Website_Post_Person.Where(p => p.Sentiment.Equals("Positive")).Where(p => p.IsDeleted != true).Count();
            //int webPostNegative = monitoringDB.Website_Post_Person.Where(p => p.Sentiment.Equals("Negative")).Where(p => p.IsDeleted != true).Count();
            int facebookPostPositive =
               (
               from person in monitoringDB.Facebook_Post_Person
               join post in monitoringDB.Facebook_Posts on person.PostID equals post.ID
               where post.UpdateTime > lastWeek && person.Sentiment == "Positive" && person.IsDeleted != true
               select post.ID
               ).Count();

            int facebookPostNegative =
                (
                from person in monitoringDB.Facebook_Post_Person
                join post in monitoringDB.Facebook_Posts on person.PostID equals post.ID
                where post.UpdateTime > lastWeek && person.Sentiment == "Negative" && person.IsDeleted != true
                select post.ID
                ).Count();

            int twitterTweetPositive =
               (
               from person in monitoringDB.Twitter_Tweet_Person
               join tweet in monitoringDB.Twitter_Tweets on person.TweetID equals tweet.ID
               where tweet.RegisteredDate > lastWeek && person.Sentiment == "Positive" && person.IsDeleted != true
               select tweet.ID
               ).Count();

            int twitterTweetNegative =
               (
               from person in monitoringDB.Twitter_Tweet_Person
               join tweet in monitoringDB.Twitter_Tweets on person.TweetID equals tweet.ID
               where tweet.RegisteredDate > lastWeek && person.Sentiment == "Negative" && person.IsDeleted != true
               select tweet.ID
               ).Count();

            int webPostPositive =
               (
               from person in monitoringDB.Website_Post_Person
               join post in monitoringDB.WebSite_Posts on person.PostID equals post.ID
               where post.DateTime > lastWeek && person.Sentiment == "Positive" && person.IsDeleted != true
               select post.ID
               ).Count();

            int webPostNegative =
               (
               from person in monitoringDB.Website_Post_Person
               join post in monitoringDB.WebSite_Posts on person.PostID equals post.ID
               where post.DateTime > lastWeek && person.Sentiment == "Negative" && person.IsDeleted != true
               select post.ID
               ).Count();

            int totatPostCount = facebookPostPositive + facebookPostNegative + twitterTweetPositive + twitterTweetNegative + webPostPositive + webPostNegative;
            int totalPostiveCount = facebookPostPositive + twitterTweetPositive + webPostPositive;
            ViewBag.TotalPostivePercent = (int)(((float)totalPostiveCount / (float)totatPostCount) * 100);

            //ViewBag.TopUserTweet = (from user in monitoringDB.Twitter_Users
            //                        join detail in monitoringDB.Twitter_User_Details on user.ID equals detail.TwitterUserID
            //                        orderby user.TweetNumber descending
            //                        select new Twitter
            //                        {
            //                            ScreenName = user.UserName,
            //                            ScreenNameResponse = detail.ScreenNameResponse,
            //                            Count = user.TweetNumber,
            //                            FollowersCount = user.Followers
            //                        }).Take(10);



            return View();
        }
        public ActionResult Index()
        {
            Session["latestComment"] = null;

            Session["latestTweet"] = null;
            if (User.Identity.IsAuthenticated == false)
            {
                return Redirect("/Account/Login");
            }
            //MonitoringEntities monitoring = new MonitoringEntities();
            //monitoring.Database.CommandTimeout=300;
            ViewBag.Message = "Ухаалаг мэдээ";
            MonitoringEntities monitoringDB = new MonitoringEntities();
            AspNetUserActivityLog userLog = new AspNetUserActivityLog();
            userLog.ID = Guid.NewGuid();
            userLog.UserName = User.Identity.GetUserName();
            userLog.Activity = "In Dashboard";
            userLog.Time = DateTime.Now;
            //monitoringDB.AspNetUserActivityLogs.Add(userLog);
            // monitoringDB.SaveChanges();
            var emailConfirmed = monitoringDB.AspNetUsers.Where(p => p.Email.Equals(User.Identity.Name)).FirstOrDefault().EmailConfirmed;
            if (!emailConfirmed)
            {
                var AuthenticationManager = HttpContext.GetOwinContext().Authentication;
                AuthenticationManager.SignOut();
                return Redirect("/Account/Login");
            }
            monitoringDB.Database.CommandTimeout = 300;
            ((IObjectContextAdapter)monitoringDB).ObjectContext.CommandTimeout = 300;
            /*            ViewBag.WebPostCount = monitoringDB.WebSite_Posts.Count();
                        ViewBag.TwitterUserCount = monitoringDB.Twitter_User_Details.Count();
                        ViewBag.WebsiteCount = monitoringDB.WebSites.Count();*/
            /*            ViewBag.FbGroupCount = monitoringDB.Facebook_Groups.Count() + monitoringDB.Facebook_Pages.Count();*/

            //bulgaa 2022-04-29 begin 
            //ViewBag.FacebookPostCount = monitoringDB.Facebook_Posts.Count();
            string FacebookPostCountStr = "";
            FacebookPostCountStr = monitoringDB.Statistics.Where(n => n.Name.Equals("FacebookPostCount")).Select(n => n.Value).FirstOrDefault();
            ViewBag.FacebookPostCount = Convert.ToInt32(FacebookPostCountStr);
            //ViewBag.TwitterTweetCount = monitoringDB.Twitter_Tweets.Count();
            string TwitterTweetCountStr = "";
            TwitterTweetCountStr = monitoringDB.Statistics.Where(n => n.Name.Equals("TwitterTweetCount")).Select(n => n.Value).FirstOrDefault();
            ViewBag.TwitterTweetCount = Convert.ToInt32(TwitterTweetCountStr);
            /*ViewBag.WebPostCount = monitoringDB.WebSite_Posts.Count();*/
            string WebPostCountStr = "";
            WebPostCountStr = monitoringDB.Statistics.Where(n => n.Name.Equals("WebPostCount")).Select(n => n.Value).FirstOrDefault();
            ViewBag.WebPostCount = Convert.ToInt32(WebPostCountStr);
            //ViewBag.CommentCount = monitoringDB.Facebook_Post_Comments.Count();
            string CommentCountStr = "";
            CommentCountStr = monitoringDB.Statistics.Where(n => n.Name.Equals("CommentCount")).Select(n => n.Value).FirstOrDefault();
            ViewBag.CommentCount = Convert.ToInt32(CommentCountStr);
            /*            ViewBag.TwitterUserCount = monitoringDB.Twitter_User_Details.Count();*/
            string TwitterUserCountStr = "";
            TwitterUserCountStr = monitoringDB.Statistics.Where(n => n.Name.Equals("TwitterUserCount")).Select(n => n.Value).FirstOrDefault();
            ViewBag.TwitterUserCount = Convert.ToInt32(TwitterUserCountStr);
            /* ViewBag.WebsiteCount = monitoringDB.WebSites.Count();*/
            string WebsiteCountStr = "";
            WebsiteCountStr = monitoringDB.Statistics.Where(n => n.Name.Equals("WebsiteCount")).Select(n => n.Value).FirstOrDefault();
            ViewBag.WebsiteCount = Convert.ToInt32(WebsiteCountStr);

            string FbGroupCountStr = "";
            FbGroupCountStr = monitoringDB.Statistics.Where(n => n.Name.Equals("FBGroupCount")).Select(n => n.Value).FirstOrDefault();
            ViewBag.FbGroupCount = Convert.ToInt32(FbGroupCountStr);
            /*            ViewBag.FbGroupCount = monitoringDB.Statistics.Count();*/
            //bulgaa 2022-04-29 end 
            var lastWeek = DateTime.Now.AddDays(-3);
            int facebookPostPositive =
               (
               from person in monitoringDB.Facebook_Post_Person
               join post in monitoringDB.Facebook_Posts on person.PostID equals post.ID
               where post.UpdateTime > lastWeek && person.Sentiment == "Positive" && person.IsDeleted != true
               select post.ID
               ).Count();

            int facebookPostNegative =
                (
                from person in monitoringDB.Facebook_Post_Person
                join post in monitoringDB.Facebook_Posts on person.PostID equals post.ID
                where post.UpdateTime > lastWeek && person.Sentiment == "Negative" && person.IsDeleted != true
                select post.ID
                ).Count();

            int twitterTweetPositive =
               (
               from person in monitoringDB.Twitter_Tweet_Person
               join tweet in monitoringDB.Twitter_Tweets on person.TweetID equals tweet.ID
               where tweet.RegisteredDate > lastWeek && person.Sentiment == "Positive" && person.IsDeleted != true
               select tweet.ID
               ).Count();

            int twitterTweetNegative =
               (
               from person in monitoringDB.Twitter_Tweet_Person
               join tweet in monitoringDB.Twitter_Tweets on person.TweetID equals tweet.ID
               where tweet.RegisteredDate > lastWeek && person.Sentiment == "Negative" && person.IsDeleted != true
               select tweet.ID
               ).Count();

            int webPostPositive =
               (
               from person in monitoringDB.Website_Post_Person
               join post in monitoringDB.WebSite_Posts on person.PostID equals post.ID
               where post.DateTime > lastWeek && person.Sentiment == "Positive" && person.IsDeleted != true
               select post.ID
               ).Count();

            int webPostNegative =
               (
               from person in monitoringDB.Website_Post_Person
               join post in monitoringDB.WebSite_Posts on person.PostID equals post.ID
               where post.DateTime > lastWeek && person.Sentiment == "Negative" && person.IsDeleted != true
               select post.ID
               ).Count();

            int totatPostCount = facebookPostPositive + facebookPostNegative + twitterTweetPositive + twitterTweetNegative + webPostPositive + webPostNegative;
            int totalPostiveCount = facebookPostPositive + twitterTweetPositive + webPostPositive;
            ViewBag.TotalPostivePercent = (int)(((float)totalPostiveCount / (float)totatPostCount) * 100);

            DateTime last = new DateTime();
            last = DateTime.Now.AddDays(-30);
            DateTime real = DateTime.Now;
            List<DateTime> dates = new List<DateTime>();
            do
            {
                dates.Add(last);
                last = last.AddDays(+1);
            } while (real != last);

            string queryFb = string.Format("select count(*) as count,Convert(date,post.UpdateTime) as date from dbo.[Facebook.Posts] post where CONVERT(date, getdate()) >= Convert(date,post.UpdateTime) and Convert(date,post.UpdateTime) > DATEADD(DD, -30, CONVERT(date, getdate())) group by Convert(date, post.UpdateTime) order by Convert(date, post.UpdateTime) desc;");
            string queryTw = string.Format("select count(*) as count,Convert(date,post.CreatedAt) as date from dbo.[Twitter.Tweets] post where CONVERT(date, getdate()) >= convert(date,post.CreatedAt) and convert(date,post.CreatedAt) > DATEADD(DD, -30, CONVERT(date, getdate())) group by Convert(date, post.CreatedAt) order by Convert(date, post.CreatedAt) desc;");
            string queryWb = string.Format("select count(*) as count,Convert(date,post.Date) as date from dbo.[WebSite.Posts] post where CONVERT(date, getdate()) >= convert(date,post.Date) and convert(date,post.Date) > DATEADD(DD, -30, CONVERT(date, getdate())) group by Convert(date, post.Date) order by Convert(date, post.Date) desc;");

            var resultsFb = monitoringDB.Database.SqlQuery<graphResult>(queryFb).ToList<graphResult>();
            var resultsTw = monitoringDB.Database.SqlQuery<graphResult>(queryTw).ToList<graphResult>();
            var resultsWb = monitoringDB.Database.SqlQuery<graphResult>(queryWb).ToList<graphResult>();

            string counterfb = "[";
            string countertw = "[";
            string counterweb = "[";

            #region facebookloop
            for (int i = 0; i < dates.Count; i++)
            {
                DateTime date = dates.ElementAt(i).Date;
                //
                if (resultsFb.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterfb == "[")
                    {
                        counterfb = counterfb + resultsFb.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    counterfb = counterfb + "," + resultsFb.Where(p => p.date.Equals(date)).FirstOrDefault().count;

                }
                else
                {
                    if (counterfb == "[")
                    {
                        counterfb = counterfb + 0;

                    }
                    counterfb = counterfb + "," + 0;

                }
            }
            #endregion

            #region twitterloop
            for (int i = 0; i < dates.Count; i++)
            {
                DateTime date = dates.ElementAt(i).Date;
                //
                if (resultsTw.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (countertw == "[")
                    {
                        countertw = countertw + resultsTw.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    countertw = countertw + "," + resultsTw.Where(p => p.date.Equals(date)).FirstOrDefault().count;

                }
                else
                {
                    if (countertw == "[")
                    {
                        countertw = countertw + 0;

                    }
                    countertw = countertw + "," + 0;

                }
            }
            #endregion

            #region webloop
            for (int i = 0; i < dates.Count; i++)
            {
                DateTime date = dates.ElementAt(i).Date;
                //
                if (resultsWb.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterweb == "[")
                    {
                        counterweb = counterweb + resultsWb.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }

                    counterweb = counterweb + "," + resultsWb.Where(p => p.date.Equals(date)).FirstOrDefault().count;

                }
                else
                {
                    if (counterweb == "[")
                    {
                        counterweb = counterweb + 0;

                    }
                    counterweb = counterweb + "," + 0;

                }
            }
            #endregion

            counterfb = counterfb + "]";
            countertw = countertw + "]";
            counterweb = counterweb + "]";
            ViewBag.facebookChartCount1 = counterfb;
            ViewBag.twitterChartCount1 = countertw;
            ViewBag.websiteChartCount1 = counterweb;

            return View();
        }
        public ActionResult LoadDashboardMainGraph()
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();
            DateTime last = new DateTime();
            last = DateTime.Now.AddDays(-30);
            DateTime real = DateTime.Now;
            List<DateTime> dates = new List<DateTime>();
            do
            {
                dates.Add(last);
                last = last.AddDays(+1);
            } while (real != last);

            string queryFb = string.Format("select count(*) as count,Convert(date,post.UpdateTime) as date from dbo.[Facebook.Posts] post where CONVERT(date, getdate()) >= Convert(date,post.UpdateTime) and Convert(date,post.UpdateTime) > DATEADD(DD, -30, CONVERT(date, getdate())) group by Convert(date, post.UpdateTime) order by Convert(date, post.UpdateTime) desc;");
            string queryTw = string.Format("select count(*) as count,Convert(date,post.CreatedAt) as date from dbo.[Twitter.Tweets] post where CONVERT(date, getdate()) >= convert(date,post.CreatedAt) and convert(date,post.CreatedAt) > DATEADD(DD, -30, CONVERT(date, getdate())) group by Convert(date, post.CreatedAt) order by Convert(date, post.CreatedAt) desc;");
            string queryWb = string.Format("select count(*) as count,Convert(date,post.Date) as date from dbo.[WebSite.Posts] post where CONVERT(date, getdate()) >= convert(date,post.Date) and convert(date,post.Date) > DATEADD(DD, -30, CONVERT(date, getdate())) group by Convert(date, post.Date) order by Convert(date, post.Date) desc;");

            var resultsFb = monitoringDB.Database.SqlQuery<graphResult>(queryFb).ToList<graphResult>();
            var resultsTw = monitoringDB.Database.SqlQuery<graphResult>(queryTw).ToList<graphResult>();
            var resultsWb = monitoringDB.Database.SqlQuery<graphResult>(queryWb).ToList<graphResult>();

            string counterfb = "[";
            string countertw = "[";
            string counterweb = "[";

            #region facebookloop
            for (int i = 0; i < dates.Count; i++)
            {
                DateTime date = dates.ElementAt(i).Date;
                //
                if (resultsFb.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterfb == "[")
                    {
                        counterfb = counterfb + resultsFb.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    counterfb = counterfb + "," + resultsFb.Where(p => p.date.Equals(date)).FirstOrDefault().count;

                }
                else
                {
                    if (counterfb == "[")
                    {
                        counterfb = counterfb + 0;

                    }
                    counterfb = counterfb + "," + 0;

                }
            }
            #endregion

            #region twitterloop
            for (int i = 0; i < dates.Count; i++)
            {
                DateTime date = dates.ElementAt(i).Date;
                //
                if (resultsTw.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (countertw == "[")
                    {
                        countertw = countertw + resultsTw.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    countertw = countertw + "," + resultsTw.Where(p => p.date.Equals(date)).FirstOrDefault().count;

                }
                else
                {
                    if (countertw == "[")
                    {
                        countertw = countertw + 0;

                    }
                    countertw = countertw + "," + 0;

                }
            }
            #endregion

            #region webloop
            for (int i = 0; i < dates.Count; i++)
            {
                DateTime date = dates.ElementAt(i).Date;
                //
                if (resultsWb.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterweb == "[")
                    {
                        counterweb = counterweb + resultsWb.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }

                    counterweb = counterweb + "," + resultsWb.Where(p => p.date.Equals(date)).FirstOrDefault().count;

                }
                else
                {
                    if (counterweb == "[")
                    {
                        counterweb = counterweb + 0;

                    }
                    counterweb = counterweb + "," + 0;

                }
            }
            #endregion

            counterfb = counterfb + "]";
            countertw = countertw + "]";
            counterweb = counterweb + "]";
            ViewBag.facebookChartCount1 = counterfb;
            ViewBag.twitterChartCount1 = countertw;
            ViewBag.websiteChartCount1 = counterweb;
            return PartialView();
        }

        public ActionResult LoadDashboardMainGraphV2()
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();
            DateTime last = new DateTime();
            last = DateTime.Now.AddDays(-30);
            DateTime real = DateTime.Now;
            List<DateTime> dates = new List<DateTime>();
            do
            {
                dates.Add(last);
                last = last.AddDays(+1);
            } while (real != last);

            string queryFb = string.Format("select count(*) as count,Convert(date,post.UpdateTime) as date from dbo.[Facebook.Posts] post where CONVERT(date, getdate()) >= Convert(date,post.UpdateTime) and Convert(date,post.UpdateTime) > DATEADD(DD, -30, CONVERT(date, getdate())) group by Convert(date, post.UpdateTime) order by Convert(date, post.UpdateTime) desc;");
            string queryTw = string.Format("select count(*) as count,Convert(date,post.CreatedAt) as date from dbo.[Twitter.Tweets] post where CONVERT(date, getdate()) >= convert(date,post.CreatedAt) and convert(date,post.CreatedAt) > DATEADD(DD, -30, CONVERT(date, getdate())) group by Convert(date, post.CreatedAt) order by Convert(date, post.CreatedAt) desc;");
            string queryWb = string.Format("select count(*) as count,Convert(date,post.Date) as date from dbo.[WebSite.Posts] post where CONVERT(date, getdate()) >= convert(date,post.Date) and convert(date,post.Date) > DATEADD(DD, -30, CONVERT(date, getdate())) group by Convert(date, post.Date) order by Convert(date, post.Date) desc;");

            var resultsFb = monitoringDB.Database.SqlQuery<graphResult>(queryFb).ToList<graphResult>();
            var resultsTw = monitoringDB.Database.SqlQuery<graphResult>(queryTw).ToList<graphResult>();
            var resultsWb = monitoringDB.Database.SqlQuery<graphResult>(queryWb).ToList<graphResult>();

            string counterfb = "[";
            string countertw = "[";
            string counterweb = "[";

            #region facebookloop
            for (int i = 0; i < dates.Count; i++)
            {
                DateTime date = dates.ElementAt(i).Date;
                //
                if (resultsFb.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterfb == "[")
                    {
                        counterfb = counterfb + resultsFb.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    counterfb = counterfb + "," + resultsFb.Where(p => p.date.Equals(date)).FirstOrDefault().count;

                }
                else
                {
                    if (counterfb == "[")
                    {
                        counterfb = counterfb + 0;

                    }
                    counterfb = counterfb + "," + 0;

                }
            }
            #endregion

            #region twitterloop
            for (int i = 0; i < dates.Count; i++)
            {
                DateTime date = dates.ElementAt(i).Date;
                //
                if (resultsTw.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (countertw == "[")
                    {
                        countertw = countertw + resultsTw.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    countertw = countertw + "," + resultsTw.Where(p => p.date.Equals(date)).FirstOrDefault().count;

                }
                else
                {
                    if (countertw == "[")
                    {
                        countertw = countertw + 0;

                    }
                    countertw = countertw + "," + 0;

                }
            }
            #endregion

            #region webloop
            for (int i = 0; i < dates.Count; i++)
            {
                DateTime date = dates.ElementAt(i).Date;
                //
                if (resultsWb.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterweb == "[")
                    {
                        counterweb = counterweb + resultsWb.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }

                    counterweb = counterweb + "," + resultsWb.Where(p => p.date.Equals(date)).FirstOrDefault().count;

                }
                else
                {
                    if (counterweb == "[")
                    {
                        counterweb = counterweb + 0;

                    }
                    counterweb = counterweb + "," + 0;

                }
            }
            #endregion

            counterfb = counterfb + "]";
            countertw = countertw + "]";
            counterweb = counterweb + "]";
            ViewBag.facebookChartCount1 = counterfb;
            ViewBag.twitterChartCount1 = countertw;
            ViewBag.websiteChartCount1 = counterweb;
            return PartialView();
        }
        public ActionResult DashboardSectionSecond()
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();
            var comments = (from comment in monitoringDB.Facebook_Post_Comments
                            orderby comment.RegisteredDate descending
                            select new Models.FbComment
                            {
                                CommentID = comment.CommentID,
                                FromName = comment.FromName,
                                Message = comment.Message,
                                CreateTime = comment.CreateTime
                            }).Take(50).ToList();
            ViewBag.Comments = comments;
            ViewBag.Tweets = (from tweet in monitoringDB.Twitter_Tweets
                              join user in monitoringDB.Twitter_User_Details on tweet.TwitterUserID equals user.TwitterUserID
                              orderby tweet.RegisteredDate descending
                              select new Models.Twitter
                              {
                                  ScreenName = user.ScreenName,
                                  StatusID = tweet.StatusID,
                                  Text = tweet.Text,
                                  Name = user.Name
                                  //SortOrder = b.SortOrder,
                                  //PicturePerson = c.Picture,
                              }).Take(200).ToList();
            return PartialView();
        }
        public ActionResult DashboardSectionThird()
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();
            var resultFb = (from post in monitoringDB.Facebook_Posts
                            join link in monitoringDB.Facebook_Post_Person on post.ID equals link.PostID
                            where link.IsDeleted != true
                            orderby post.UpdateTime
                            select new Models.FbPost
                            {
                                Link = post.Link,
                                Sentiment = link.Sentiment,
                                FullPicture = post.FullPicture,
                                SharedCount = post.SharedCount,
                                UpdateTime = post.UpdateTime,
                                Message = post.Message,
                                FromName = post.FromName,
                                FromID = post.FromID
                            }).Take(5).ToList();

            foreach (var i in resultFb)
            {
                if (i.FromName.Length > 10)
                {
                    i.FromName = i.FromName.Substring(0, 14) + "...";
                }
            }

            foreach (var i in resultFb)
            {
                if (i.Message != null && i.Message != "null")
                {
                    if (i.Message.Length > 14)
                    {
                        i.Message = i.Message.Substring(0, 14) + "...";
                    }
                }
            }

            var resultTw = (from tweet in monitoringDB.Twitter_Tweets
                            join user in monitoringDB.Twitter_User_Details on tweet.TwitterUserID equals user.TwitterUserID
                            orderby tweet.RegisteredDate descending
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
            foreach (var i in resultTw)
            {
                if (i.Name != null)
                {
                    if (i.Name.Length > 10)
                    {
                        i.Name = i.Name.Substring(0, 10) + "...";
                    }
                }
            }
            foreach (var i in resultTw)
            {
                if (i.Text != null)
                {
                    if (i.Text.Length > 10)
                    {
                        i.Text = i.Text.Substring(0, 10) + "...";
                    }
                }
            }

            var resultWeb = (from post in monitoringDB.WebSite_Posts
                             join link in monitoringDB.Website_Post_Person on post.ID equals link.PostID
                             orderby post.DateTime
                             select new Models.WebSitePost
                             {
                                 Url = post.Url,
                                 Sentiment = link.Sentiment,
                                 Date = post.DateTime,
                                 CoverUrl = post.CoverUrl,
                                 Title = post.Title,
                                 Reporter = post.Reporter
                             }).Take(5).ToList();
            foreach (var i in resultWeb)
            {
                if (i.Title != null)
                {
                    if (i.Title.Length > 10)
                    {
                        i.Title = i.Title.Substring(0, 14) + "...";
                    }
                }
                if (i.CoverUrl != null && i.CoverUrl != "")
                {
                    if (i.CoverUrl.Substring(0, 1).Equals("<"))
                    {
                        i.CoverUrl = Between(i.CoverUrl, "src=");
                    }
                }
            }
            ViewBag.FbDash = resultFb;
            ViewBag.TwitDash = resultTw;
            ViewBag.WebDash = resultWeb;
            return PartialView();
        }

        public ActionResult TopNews()
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();
            ViewBag.TopTweets = (from a in monitoringDB.TopTweets
                                 select a.Text).ToList();
            ViewBag.FbPostCount = monitoringDB.Facebook_Posts.Count();
            ViewBag.TweetCount = monitoringDB.Twitter_Tweets.Count();
            ViewBag.WebsitePostCount = monitoringDB.WebSite_Posts.Count();
            ViewBag.CommentCount = monitoringDB.Facebook_Post_Comments.Count();
            ViewBag.TwitterUserCout = monitoringDB.Twitter_User_Details.Count();
            ViewBag.TopTopics = (from a in monitoringDB.TopTopics
                                 join b in monitoringDB.TopSpecifiedNewsForTopics on a.TopicID equals b.TopicID
                                 orderby a.ViralCount descending
                                 select new Topic
                                 {
                                     TopicID = a.TopicID,
                                     Picture = b.Image,
                                     Type = b.Type,
                                     TopicPicture = a.Picture,
                                     TopicName = a.Name,
                                     Title = b.Title,
                                     Source = b.Source,
                                     SourceLink = b.SourceLink,
                                     Date = b.Date,
                                     Link = b.Link,
                                     Text = b.Text
                                 }).ToList().GroupBy(x => x.TopicID).Select(x => x.First()).ToList();
            ViewBag.TopicCount = ViewBag.TopTopics.Count;
            //ViewBag.TopNewsForTopics = (from a in monitoringDB.TopNewsForTopics
            //                            select a).ToList();
            return View();
        }

        public ActionResult TopUserTwitter()
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();
            var TopUserTwitter = (from user in monitoringDB.Twitter_Users
                                  join detail in monitoringDB.Twitter_User_Details on user.ID equals detail.TwitterUserID
                                  orderby user.TweetNumber descending
                                  select new Models.TopUser
                                  {
                                      UserName = user.UserName,
                                      tw_ScreenResponse = detail.ScreenNameResponse,
                                      tw_FollowersNumber = detail.FollowersCount ?? default(int),
                                      TweetNumber = user.TweetNumber ?? default(int)
                                  }).Take(10);

            return PartialView(TopUserTwitter);
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public string Between(string STR, string FirstString)
        {
            string FinalString;
            int Pos1 = STR.IndexOf(FirstString) + FirstString.Length + 1;
            FinalString = STR.Substring(Pos1);
            return FinalString;
        }

        public ActionResult TopNewsForTopic(Guid id)
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();
            ViewBag.ID = id;
            return View();
        }

        public Models.TopNewsForTopic PagedResult(List<DAL.TopNewsForTopic> list, int PageNumber, int PageSize)
        {
            var result = new Models.TopNewsForTopic();
            result.news = list.Skip(PageSize * (PageNumber - 1)).Take(PageSize).ToList();
            result.TotalPages = Convert.ToInt32(Math.Ceiling((double)list.Count() / PageSize));
            result.CurrentPage = PageNumber;
            return result;
        }
        public ActionResult GetPaggedData(Guid id, string filter, int pageNumber = 1, int pageSize = 20)
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();
            List<DAL.TopNewsForTopic> listData = new List<DAL.TopNewsForTopic>();
            if (filter == "All")
            {
                listData = (from a in monitoringDB.TopNewsForTopics
                            where a.TopicID == id
                            select a).ToList();
            }
            else if (filter == "Facebook")
            {
                listData = (from a in monitoringDB.TopNewsForTopics
                            where a.TopicID == id && a.Type == "Facebook"
                            select a).ToList();
            }
            else if (filter == "Twitter")
            {
                listData = (from a in monitoringDB.TopNewsForTopics
                            where a.TopicID == id && a.Type == "Twitter"
                            select a).ToList();
            }
            else
            {
                listData = (from a in monitoringDB.TopNewsForTopics
                            where a.TopicID == id && a.Type == "Website"
                            select a).ToList();
            }
            var pagedData = PagedResult(listData, pageNumber, pageSize);
            return Json(pagedData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CommentCount()
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();
            ViewBag.CommentCount = monitoringDB.Facebook_Post_Comments.Count();
            return PartialView();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Member()
        {
            ViewBag.Message = "Анализ";
            return View();
        }

        public ActionResult PositivePercent()
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();

            var lastWeek = DateTime.Now.AddDays(-1);

            int facebookPostPositive =
                (
                from person in monitoringDB.Facebook_Post_Person
                join post in monitoringDB.Facebook_Posts on person.PostID equals post.ID
                where post.UpdateTime > lastWeek && person.Sentiment == "Positive" && person.IsDeleted != true
                select post.ID
                ).Count();

            int facebookPostNegative =
                (
                from person in monitoringDB.Facebook_Post_Person
                join post in monitoringDB.Facebook_Posts on person.PostID equals post.ID
                where post.UpdateTime > lastWeek && person.Sentiment == "Negative" && person.IsDeleted != true
                select post.ID
                ).Count();

            int twitterTweetPositive =
               (
               from person in monitoringDB.Twitter_Tweet_Person
               join tweet in monitoringDB.Twitter_Tweets on person.TweetID equals tweet.ID
               where tweet.RegisteredDate > lastWeek && person.Sentiment == "Positive" && person.IsDeleted != true
               select tweet.ID
               ).Count();

            int twitterTweetNegative =
               (
               from person in monitoringDB.Twitter_Tweet_Person
               join tweet in monitoringDB.Twitter_Tweets on person.TweetID equals tweet.ID
               where tweet.RegisteredDate > lastWeek && person.Sentiment == "Negative" && person.IsDeleted != true
               select tweet.ID
               ).Count();

            int webPostPositive =
               (
               from person in monitoringDB.Website_Post_Person
               join post in monitoringDB.WebSite_Posts on person.PostID equals post.ID
               where post.DateTime > lastWeek && person.Sentiment == "Positive" && person.IsDeleted != true
               select post.ID
               ).Count();

            int webPostNegative =
               (
               from person in monitoringDB.Website_Post_Person
               join post in monitoringDB.WebSite_Posts on person.PostID equals post.ID
               where post.DateTime > lastWeek && person.Sentiment == "Negative" && person.IsDeleted != true
               select post.ID
               ).Count();

            int totatPostCount = facebookPostPositive + facebookPostNegative + twitterTweetPositive + twitterTweetNegative + webPostPositive + webPostNegative;
            int totalPostiveCount = facebookPostPositive + twitterTweetPositive + webPostPositive;
            ViewBag.TotalPostivePercent = (int)(((float)totalPostiveCount / (float)totatPostCount) * 100);
            return PartialView();
        }

        public ActionResult SmartNewsCustomerFilter(String id, String Filter, String Sentiment)
        {
            ViewBag.Message = "Ухаалаг мэдээ";
            MonitoringEntities monitoringDB = new MonitoringEntities();
            if (Filter == "Date")
            {
                if (Sentiment == "All" || Sentiment == "default" || Sentiment == "")
                {
                    ViewBag.FbData =
                                    (from a in monitoringDB.Facebook_Post_Person
                                     join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
                                     join c in monitoringDB.System_Person on a.PersonID equals c.ID
                                     where (c.ID.ToString() == id) && a.IsDeleted != true
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
                        where (c.ID.ToString() == id) && a.IsDeleted != true
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
                       where (c.ID.ToString() == id) && a.IsDeleted != true
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
                                     where c.ID.ToString() == id && a.Sentiment == Sentiment && a.IsDeleted != true
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
                        where (c.ID.ToString() == id) && a.Sentiment == Sentiment && a.IsDeleted != true
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
                       where (c.ID.ToString() == id) & a.Sentiment == Sentiment
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
                if (Sentiment == "All" || Sentiment == "default")
                {
                    ViewBag.FbData =
                                    (from a in monitoringDB.Facebook_Post_Person
                                     join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
                                     join c in monitoringDB.System_Person on a.PersonID equals c.ID
                                     where (c.ID.ToString() == id) && a.IsDeleted != true
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
                        where (c.ID.ToString() == id) && a.IsDeleted != true
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
                       where (c.ID.ToString() == id)
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
                                     where c.ID.ToString() == id && a.Sentiment == Sentiment && a.IsDeleted != true
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
                        where (c.ID.ToString() == id) && a.Sentiment == Sentiment && a.IsDeleted != true
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
                       where (c.ID.ToString() == id) & a.Sentiment == Sentiment
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

            else if (Filter == "Distinct")
            {
                if (Sentiment == "All" || Sentiment == "default")
                {
                    int thisMonth = DateTime.Now.Month;
                    int thisYear = DateTime.Now.Year;

                    /*
                    ViewBag.FbData =
                                    (from a in monitoringDB.Facebook_Post_Person
                                     join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
                                     join c in monitoringDB.System_Person on a.PersonID equals c.ID
                                     where (c.ID.ToString() == id) && a.IsDeleted != true && b.UpdateTime.Value.Month == thisMonth
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

                                     })
                                     .GroupBy(x => new { x.FromName, x.Message,x.Description})
                                     .Select( x => new Models.FbPost 
                                     {
                                         ID = x.FirstOrDefault().ID,
                                         PageID = x.FirstOrDefault().PageID,
                                         GroupID = x.FirstOrDefault().GroupID,
                                         PostID = x.FirstOrDefault().PostID,
                                         FromID = x.Max(y=> y.FromID),
                                         FromName =x.Key.FromName,
                                         Message = x.Key.Message,
                                         Story = x.Max(y => y.Story),
                                         Type = x.Max(y => y.Type),
                                         UpdateTime = x.Min(y => y.UpdateTime),
                                         SharedCount = x.Max(y => y.SharedCount),
                                         PermalinkUrl = x.Max(y => y.PermalinkUrl),
                                         Caption = x.Max(y => y.Caption),
                                         Description = x.Key.Description,
                                         FullPicture = x.Max(y => y.FullPicture),
                                         Link = x.Max(y => y.Link),
                                         Name = x.Max(y => y.Name),
                                         Picture = x.Max(y => y.Picture),
                                         Icon = x.Max(y => y.Icon),
                                         ObjectID = x.Max(y => y.ObjectID),
                                         ParentID = x.FirstOrDefault().ParentID,
                                         PerName = x.Max(y => y.Name),
                                         Sentiment = x.Max(y => y.Sentiment)
                                     })
                                     .OrderByDescending(x=>x.UpdateTime)
                                     .Take(5)
                                     .ToList();*/
                    ViewBag.FbData =
                                    (from a in monitoringDB.Facebook_Post_Person
                                     join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
                                     join c in monitoringDB.System_Person on a.PersonID equals c.ID
                                     where (c.ID.ToString() == id) && a.IsDeleted != true && ((b.UpdateTime.Value.Month == thisMonth || b.UpdateTime.Value.Month == thisMonth - 1) && b.UpdateTime.Value.Year == thisYear)
                                     group b by new { b.FromName, b.Message, b.Description, b.UpdateTime } into x
                                     orderby x.Key.UpdateTime descending
                                     select new Models.FbPost
                                     {
                                         ID = x.FirstOrDefault().ID,
                                         PageID = x.FirstOrDefault().PageID,
                                         GroupID = x.FirstOrDefault().GroupID,
                                         PostID = x.FirstOrDefault().PostID,
                                         FromID = x.Max(y => y.FromID),
                                         FromName = x.Key.FromName,
                                         Message = x.Key.Message,
                                         Story = x.Max(y => y.Story),
                                         Type = x.Max(y => y.Type),
                                         UpdateTime = x.Max(y => y.UpdateTime),
                                         SharedCount = x.Max(y => y.SharedCount),
                                         PermalinkUrl = x.Max(y => y.PermalinkUrl),
                                         Caption = x.Max(y => y.Caption),
                                         Description = x.Key.Description,
                                         FullPicture = x.Max(y => y.FullPicture),
                                         Link = x.Max(y => y.Link),
                                         Name = x.Max(y => y.Name),
                                         Picture = x.Max(y => y.Picture),
                                         Icon = x.Max(y => y.Icon),
                                         ObjectID = x.Max(y => y.ObjectID),
                                         ParentID = x.FirstOrDefault().ParentID,
                                         PerName = x.Max(y => y.Name),
                                         Sentiment = null,
                                     })
                                     .OrderByDescending(x => x.UpdateTime)
                                     .Take(5)
                                     .ToList();

                    ViewBag.TwitterData =
                       (from a in monitoringDB.Twitter_Tweet_Person
                        join tweet in monitoringDB.Twitter_Tweets on a.TweetID equals tweet.ID
                        join c in monitoringDB.System_Person on a.PersonID equals c.ID
                        join user in monitoringDB.Twitter_User_Details on tweet.TwitterUserID equals user.TwitterUserID
                        where (c.ID.ToString() == id) && a.IsDeleted != true && tweet.CreatedAt.Value.Month == thisMonth && tweet.CreatedAt.Value.Year == thisYear
                        group tweet by new { tweet.Text, tweet.FullText, user.ScreenName } into x
                        select new Models.Twitter
                        {
                            ID = x.FirstOrDefault().ID,
                            TwitterUserID = x.FirstOrDefault().TwitterUserID,
                            TweetID = x.Max(y => y.TweetID),
                            CreatedAt = x.Max(y => y.CreatedAt),
                            ScreenName = x.Max(y => y.ScreenName),
                            Source = null,//x.Max(y => y.Source),
                            StatusID = x.Max(y => y.StatusID),
                            RetweetCount = x.Max(y => y.RetweetCount),
                            Retweeted = null,//x.FirstOrDefault().Retweeted,
                            FullText = x.Key.FullText,
                            Language = x.FirstOrDefault().Language,
                            Text = x.Key.Text,
                            TrimUser = null,//x.FirstOrDefault().TrimUser,
                            Truncated = null,// x.FirstOrDefault().Truncated,
                            UserID = x.Max(y => y.UserID),
                            HashEntity1 = null,// x.FirstOrDefault().HashEntity1,
                            HashEntity2 = null,// x.FirstOrDefault().HashEntity2,
                            HashEntity3 = null,// x.FirstOrDefault().HashEntity3,
                            SymbolEntity1 = null,// x.FirstOrDefault().SymbolEntity1,
                            SymbolEntity2 = null,// x.FirstOrDefault().SymbolEntity2,
                            SymbolEntity3 = null,// x.FirstOrDefault().SymbolEntity3,
                            UrlEntity1 = null,// x.FirstOrDefault().UrlEntity1,
                            UrlEntityDisplayUrl1 = null,// x.FirstOrDefault().UrlEntityDisplayUrl1,
                            UrlEntityExpandedUrl1 = null,// x.FirstOrDefault().UrlEntityExpandedUrl1,
                            UrlEntity2 = null,// x.FirstOrDefault().UrlEntity2,
                            UrlEntityDisplayUrl2 = null,// x.FirstOrDefault().UrlEntityDisplayUrl2,
                            UrlEntityExpandedUrl2 = null,// x.FirstOrDefault().UrlEntityExpandedUrl2,
                            UrlEntity3 = null,// x.FirstOrDefault().UrlEntity3,
                            UrlEntityDisplayUrl3 = null,// x.FirstOrDefault().UrlEntityDisplayUrl3,
                            UrlEntityExpandedUrl3 = null,// x.FirstOrDefault().UrlEntityExpandedUrl3,
                            MediaEntitiy1 = null,// x.FirstOrDefault().MediaEntitiy1,
                            MediaEntitiyDisplayUrl1 = null,// x.FirstOrDefault().MediaEntitiyDisplayUrl1,
                            MediaEntitiyMediaUrl1 = null,// x.FirstOrDefault().MediaEntitiyMediaUrl1,
                            MediaEntitiyVideoInfo1 = null,// x.FirstOrDefault().MediaEntitiyVideoInfo1,
                            MediaEntity2 = null,// x.FirstOrDefault().MediaEntity2,
                            MediaEntitiyDisplayUrl2 = null,// x.FirstOrDefault().MediaEntitiyDisplayUrl2,
                            MediaEntitiyMediaUrl2 = null,// x.FirstOrDefault().MediaEntitiyMediaUrl2,
                            MediaEntitiyVideoInfo2 = null,// x.FirstOrDefault().MediaEntitiyVideoInfo2,
                            MediaEntity3 = null,// x.FirstOrDefault().MediaEntity3,
                            MediaEntitiyDisplayUrl3 = null,// x.FirstOrDefault().MediaEntitiyDisplayUrl3,
                            MediaEntitiyMediaUrl3 = null,// x.FirstOrDefault().MediaEntitiyMediaUrl3,
                            MediaEntitiyVideoInfo3 = null,// x.FirstOrDefault().MediaEntitiyVideoInfo3,
                            UserMentionEntity1 = null,// x.FirstOrDefault().UserMentionEntity1,
                            UserMentionEntityScreenName1 = null,// x.FirstOrDefault().UserMentionEntityScreenName1,
                            UserMentionEntity2 = null,// x.FirstOrDefault().UserMentionEntity2,
                            UserMentionEntityScreenName2 = null,// x.FirstOrDefault().UserMentionEntityScreenName2,
                            UserMentionEntity3 = null,// x.FirstOrDefault().UserMentionEntity3,
                            UserMentionEntityScreenName3 = null,// x.FirstOrDefault().UserMentionEntityScreenName3,
                            Sentiment = null,//x.FirstOrDefault().Sentiment,
                            //user
                            ContributorsEnabled = null,// x.FirstOrDefault().ContributorsEnabled,
                            Count = null,// x.Max(y = null,//> y.Count),
                            Cursor = null,// x.FirstOrDefault().Cursor,
                            DefaultProfile = null,// x.FirstOrDefault().DefaultProfile,
                            DefaultProfileImage = null,// x.FirstOrDefault().DefaultProfileImage,
                            Description = null,// x.FirstOrDefault().Description,
                            Email = null,// x.FirstOrDefault().Email,
                            FavoritesCount = null,// x.FirstOrDefault().FavoritesCount,
                            FollowRequestSent = null,// x.FirstOrDefault().FollowRequestSent,
                            FollowersCount = null,// x.Max(y = null,//> y.FollowersCount),
                            Following = null,// x.FirstOrDefault().Following,
                            FriendsCount = null,// x.FirstOrDefault().FriendsCount,
                            GeoEnabled = null,// x.FirstOrDefault().GeoEnabled,
                            ImageSize = null,// x.Max(y = null,//> y.ImageSize),
                            IncludeEntities = null,// x.FirstOrDefault().IncludeEntities,
                            IsTranslator = null,// x.FirstOrDefault().IsTranslator,
                            Lang = null,//x.Max(y => y.Lang),
                            LangResponse = null,// x.FirstOrDefault().LangResponse,
                            ListedCount = null,// x.Max(y = null,//> y.ListedCount),
                            Location = null,// x.Max(y = null,//> y.Location),
                            Name = null,//x.Max(y => y.Name),
                            Notifications = null,// x.Max(y = null,//> y.Notifications),
                            Page = null,// x.Max(y = null,//> y.Page),
                            ProfileBackgroundColor = null,// x.FirstOrDefault().ProfileBackgroundColor,
                            ProfileBackgroundImageUrl = null,// x.FirstOrDefault().ProfileBackgroundImageUrl,
                            ProfileBackgroundImageUrlHttps = null,// x.FirstOrDefault().ProfileBackgroundImageUrlHttps,
                            ProfileBackgroundTile = null,// x.FirstOrDefault().ProfileBackgroundTile,
                            ProfileBannerUrl = null,// x.FirstOrDefault().ProfileBannerUrl,
                            ProfileImage = null,// x.FirstOrDefault().ProfileImage,
                            ProfileImageUrl = null,// x.FirstOrDefault().ProfileImageUrl,
                            ProfileImageUrlHttps = null,// x.FirstOrDefault().ProfileImageUrlHttps,
                            ProfileLinkColor = null,// x.FirstOrDefault().ProfileLinkColor,
                            ProfileSidebarBorderColor = null,// x.FirstOrDefault().ProfileSidebarBorderColor,
                            ProfileSidebarFillColor = null,// x.FirstOrDefault().ProfileSidebarFillColor,
                            ProfileTextColor = null,// x.FirstOrDefault().ProfileTextColor,
                            ProfileUseBackgroundImage = null,// x.FirstOrDefault().ProfileUseBackgroundImage,
                            Protected = null,// x.FirstOrDefault().Protected,
                            Query = null,// x.Max(y = null,//> y.Query),
                            ScreenNameList = null,// x.Max(y = null,//> y.ScreenNameList),
                            ScreenNameResponse = x.Key.ScreenName,
                            ShowAllInlineMedia = null,// x.Max(y = null,//> y.ShowAllInlineMedia),
                            SkipStatus = null,// x.FirstOrDefault().SkipStatus,
                            Slug = null,// x.FirstOrDefault().Slug,
                            StatusesCount = null,// x.Max(y = null,//> y.StatusesCount),
                            TimeZone = null,// x.FirstOrDefault().TimeZone,
                            Type = null,// x.Max(y => y.Type),
                            Url = null,// x.Max(y => y.Url),
                            UserIDResponse = null,//x.FirstOrDefault().UserIDResponse,
                            UserIdList = null,//x.FirstOrDefault().UserIdList,
                            UtcOffset = null,//x.FirstOrDefault().UtcOffset,
                            Verified = null,//x.FirstOrDefault().Verified
                            //,
                            //SortOrder = b.SortOrder
                        })
                        .OrderByDescending(x => x.CreatedAt)
                        .Take(5)
                        .ToList();

                    ViewBag.WebSiteData =
                      (from a in monitoringDB.Website_Post_Person
                       join b in monitoringDB.WebSite_Posts on a.PostID equals b.ID
                       join c in monitoringDB.System_Person on a.PersonID equals c.ID
                       where (c.ID.ToString() == id) && a.IsDeleted != true && ((b.DateTime.Value.Month == thisMonth || b.DateTime.Value.Month == thisMonth - 1) && b.DateTime.Value.Year == thisYear)
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
                       })
                       .Take(5)
                       .ToList();
                }
                else
                {
                    ViewBag.FbData =
                                    (from a in monitoringDB.Facebook_Post_Person
                                     join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
                                     join c in monitoringDB.System_Person on a.PersonID equals c.ID
                                     where c.ID.ToString() == id && a.Sentiment == Sentiment && a.IsDeleted != true
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
                                     }).Distinct().Take(200).ToList();

                    ViewBag.TwitterData =
                       (from a in monitoringDB.Twitter_Tweet_Person
                        join tweet in monitoringDB.Twitter_Tweets on a.TweetID equals tweet.ID
                        join c in monitoringDB.System_Person on a.PersonID equals c.ID
                        join user in monitoringDB.Twitter_User_Details on tweet.TwitterUserID equals user.TwitterUserID
                        where (c.ID.ToString() == id) && a.Sentiment == Sentiment && a.IsDeleted != true
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
                        }).Distinct().Take(200).ToList();

                    ViewBag.WebSiteData =
                      (from a in monitoringDB.Website_Post_Person
                       join b in monitoringDB.WebSite_Posts on a.PostID equals b.ID
                       join c in monitoringDB.System_Person on a.PersonID equals c.ID
                       where (c.ID.ToString() == id) && a.Sentiment == Sentiment && a.IsDeleted != true
                       orderby b.DateTime descending
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
                       }).Distinct().Take(200).ToList();
                }
            }


            return PartialView();
        }

        public ActionResult SmartNews()
        {
            if (User.Identity.IsAuthenticated == false)
            {
                return Redirect("/Account/Login");
            }

            MonitoringEntities monitoringDB = new MonitoringEntities();
            var model = monitoringDB.System_Person.Select(n => new Models.SystemUserModels
            {
                ID = n.ID.ToString(),
                Name = n.Name,
                Surename = n.Surename,
            }).ToList();

            ViewBag.model = model;
            //MonitoringEntities monitoring = new MonitoringEntities();
            //var emailConfirmed = monitoring.AspNetUsers.Where(p => p.Email.Equals(User.Identity.Name)).FirstOrDefault().EmailConfirmed;
            //if (!emailConfirmed)
            //{
            //    var AuthenticationManager = HttpContext.GetOwinContext().Authentication;
            //    AuthenticationManager.SignOut();
            //    return Redirect("/Account/Login");
            //}

            //ViewBag.Message = "Ухаалаг мэдээ";
            //MonitoringEntities monitoringDB = new MonitoringEntities();
            //ViewBag.FbLatestID = monitoringDB.Facebook_Posts.OrderByDescending(a => a.UpdateTime).FirstOrDefault().ID.ToString();
            return View();
        }
        public JsonResult LatestIDCheckFb(string LatestID)
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();
            string currentLatestID = monitoringDB.Facebook_Posts.OrderByDescending(a => a.UpdateTime).FirstOrDefault().ID.ToString();
            if (LatestID != currentLatestID)
            {
                return Json(currentLatestID, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(LatestID, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult LatestIDCheckTw(string LatestID)
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();
            string currentLatestID = monitoringDB.Twitter_Tweets.OrderByDescending(a => a.CreatedAt).FirstOrDefault().ID.ToString();
            if (LatestID != currentLatestID)
            {
                return Json(currentLatestID, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(LatestID, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult LatestIDCheckWeb(string LatestID)
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();
            string currentLatestID = monitoringDB.WebSite_Posts.OrderByDescending(a => a.Date).FirstOrDefault().ID.ToString();
            if (LatestID != currentLatestID)
            {
                return Json(currentLatestID, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(LatestID, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult SmartNewsALoadFb(string count)
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();
            int skip = Int32.Parse(count) * 10;
            var FbData =
                (from b in monitoringDB.Facebook_Posts
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
                     PerName = b.Name
                 }).Skip(skip).Take(10).ToList();
            return PartialView(FbData);
        }
        public ActionResult SmartNewsALoadTw(string count)
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();
            int skip = Int32.Parse(count) * 10;
            var TwData = (
                from tweet in monitoringDB.Twitter_Tweets
                join user in monitoringDB.Twitter_User_Details on tweet.TwitterUserID equals user.TwitterUserID
                orderby tweet.CreatedAt descending
                select new Models.Twitter
                {
                    ID = tweet.ID,
                    TwitterUserID = tweet.TwitterUserID,
                    TweetID = tweet.TweetID,
                    ScreenName = user.ScreenName,
                    StatusID = tweet.StatusID,
                    UserID = tweet.UserID,
                    ScreenNameResponse = user.ScreenNameResponse,
                }).Skip(skip).Take(10).ToList();
            return PartialView(TwData);
        }
        public ActionResult SmartNewsALoadWeb(string count)
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();
            int skip = Int32.Parse(count) * 10;
            var WebData =
              (from b in monitoringDB.WebSite_Posts
               orderby b.Date descending
               select new Models.WebSitePost
               {
                   ID = b.ID,
                   Link = b.Link,
                   Title = b.Title,
                   Text = b.Text,
                   Url = b.Url,
                   Body = b.Body.Substring(0, 300) + "...",
                   Reporter = b.Reporter,
                   CoverUrl = b.CoverUrl,
                   DateTime = b.DateTime
               }).Skip(skip).Take(10).ToList();
            return PartialView(WebData);
        }
        public ActionResult TwitterLoad()
        {
            ViewBag.Message = "Ухаалаг мэдээ";
            MonitoringEntities monitoringDB = new MonitoringEntities();

            int TwitterLastID = 0;
            ViewBag.TwitterData = (from tweet in monitoringDB.Twitter_Tweets
                                   join user in monitoringDB.Twitter_User_Details on tweet.TwitterUserID equals user.TwitterUserID
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
                                       Verified = user.Verified
                                       //Tittle = b.Tittle,
                                       //Text = b.Text,
                                       //Date = b.CreatedAt,
                                       //ReTweetCount = b.RetweetCount
                                       //,
                                       // SortOrder = b.SortOrder
                                   }).Take(200).ToList();

            foreach (var post in ViewBag.TwitterData)
            {
                TwitterLastID++;
                if (TwitterLastID == 1)
                {
                    ViewBag.TwitterLastID = post.ID;
                }
            }
            return PartialView();
        }

        public ActionResult FbLoad()
        {
            ViewBag.Message = "Ухаалаг мэдээ";
            MonitoringEntities monitoringDB = new MonitoringEntities();
            int FbLastID = 0;
            ViewBag.FbData =
                (from b in monitoringDB.Facebook_Posts
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
                     PerName = b.Name
                 }).Take(200).ToList();

            foreach (var post in ViewBag.FbData)
            {
                FbLastID++;
                if (FbLastID == 1)
                {
                    ViewBag.FbLastID = post.ID;
                }
            }
            return PartialView();
        }

        public ActionResult WebSiteLoad()
        {
            ViewBag.Message = "Ухаалаг мэдээ";
            MonitoringEntities monitoringDB = new MonitoringEntities();

            int WebLastID = 0;
            ViewBag.WebSiteData =
            (from b in monitoringDB.WebSite_Posts
             orderby b.Date descending
             select new Models.WebSitePost
             {
                 ID = b.ID,
                 Link = b.Link,
                 Title = b.Title,
                 Text = b.Text,
                 Body = b.Body.Substring(0, 300) + "...",
                 Reporter = b.Reporter,
                 CoverUrl = b.CoverUrl,
                 DateTime = b.DateTime
             }).Take(200).ToList();

            foreach (var post in ViewBag.WebSiteData)
            {
                WebLastID++;
                if (WebLastID == 1)
                {
                    ViewBag.WebLastID = post.ID;
                }
            }
            return PartialView();
        }

        public ActionResult FbLastID(string PostID)
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();
            ViewBag.FbLastID = PostID;
            ViewBag.FbLastData =
                (from b in monitoringDB.Facebook_Posts
                 orderby b.UpdateTime descending
                 select new Models.FbPost
                 {
                     ID = b.ID,

                 }).Take(200).ToList();

            if (ViewBag.FbLastID == ViewBag.FbLastData[0].ID.ToString())
            {
                return Content("ok");
            }
            return Content("no");
        }

        public ActionResult TwitterLastID(string PostID)
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();
            ViewBag.TwitterLastID = PostID;
            ViewBag.TwitterLastData =
                (from b in monitoringDB.Twitter_Tweets
                 orderby b.CreatedAt descending
                 select new Models.Twitter
                 {
                     ID = b.ID,

                 }).Take(200).ToList();

            if (ViewBag.TwitterLastID == ViewBag.TwitterLastData[0].ID.ToString())
            {
                return Content("ok");
            }
            return Content("no");
        }
        public ActionResult WebPostCount()
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();
            ViewBag.Count = monitoringDB.WebSite_Posts.Count();
            return PartialView();
        }

        public ActionResult FbCount()
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();
            ViewBag.Count = monitoringDB.Facebook_Posts.Count();
            return PartialView();
        }

        public ActionResult TweetCount()
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();
            ViewBag.Count = monitoringDB.Twitter_Tweets.Count();
            return PartialView();
        }
        //start Baterdene's Custom Counts
        //Now using Dashboard
        public JsonResult FbCountCustom()
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();
            var Count = monitoringDB.Facebook_Posts.Count();
            return Json(Count, JsonRequestBehavior.AllowGet);
        }
        public JsonResult WebPostCountCustom()
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();
            var Count = monitoringDB.WebSite_Posts.Count();
            return Json(Count, JsonRequestBehavior.AllowGet);
        }
        public JsonResult TweetCountCustom()
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();
            var Count = monitoringDB.Twitter_Tweets.Count();
            return Json(Count, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CommentCountCustom()
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();
            var Count = monitoringDB.Facebook_Post_Comments.Count();
            return Json(Count, JsonRequestBehavior.AllowGet);
        }
        //end Baterdene's Custom Counts

        public ActionResult WebLastID(string PostID)
        {

            MonitoringEntities monitoringDB = new MonitoringEntities();
            ViewBag.WebLastID = PostID;
            ViewBag.WebLastData =
                (from b in monitoringDB.WebSite_Posts
                 orderby b.Date descending
                 select new Models.WebSitePost
                 {
                     ID = b.ID,

                 }).Take(200).ToList();

            if (ViewBag.WebLastID == ViewBag.WebLastData[0].ID.ToString())
            {
                return Content("ok");
            }
            return Content("no");
        }



        public ActionResult Posts()
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

            ViewBag.Message = "Posts";

            ViewBag.FbData =
         (from b in monitoringDB.Facebook_Posts

          orderby b.UpdateTime descending

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
              PerName = b.Name
          }).Take(200).ToList();

            ViewBag.TwitterData =
               (from tweet in monitoringDB.Twitter_Tweets
                join user in monitoringDB.Twitter_User_Details on tweet.TwitterUserID equals user.TwitterUserID
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
                    Verified = user.Verified
                }).Take(200).ToList();

            ViewBag.WebSiteData =
              (from b in monitoringDB.WebSite_Posts

               orderby b.Date descending
               select new Models.WebSitePost
               {
                   ID = b.ID,
                   Link = b.Link,
                   Title = b.Title,
                   Text = b.Text,
                   Body = b.Body.Substring(0, 300) + "...",
                   Reporter = b.Reporter,
                   CoverUrl = b.CoverUrl,
                   DateTime = b.DateTime,
                   Url = b.Url
               }).Take(200).ToList();
            return View();
        }


        public ActionResult PostSearch(String searchTxt)
        {

            MonitoringEntities monitoringDB = new MonitoringEntities();

            ViewBag.FbData =
                 (from b in monitoringDB.Facebook_Posts
                  where b.Message.Contains(searchTxt)
                  orderby b.UpdateTime descending

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
                      PerName = b.Name
                  }).Take(200).ToList();

            ViewBag.TwitterData =
               (from b in monitoringDB.Twitter_Tweets
                where b.Text.Contains(searchTxt)
                orderby b.CreatedAt descending
                select new Models.Twitter
                {
                    ID = b.ID,
                    //Tittle = b.Tittle,
                    Text = b.Text,
                    CreatedAt = b.CreatedAt,
                    RetweetCount = b.RetweetCount
                    //,
                    // SortOrder = b.SortOrder
                }).Take(200).ToList();

            ViewBag.WebSiteData =
              (from b in monitoringDB.WebSite_Posts
               where b.Text.Contains(searchTxt) || b.Title.Contains(searchTxt)
               orderby b.Date descending
               select new Models.WebSitePost
               {
                   ID = b.ID,
                   Link = b.Link,
                   Title = b.Title,
                   Text = b.Text,
                   Body = b.Body.Substring(0, 300) + "...",
                   Reporter = b.Reporter,
                   CoverUrl = b.CoverUrl,
                   DateTime = b.DateTime
               }).Take(200).ToList();


            return PartialView();
        }


        public ActionResult SmartNewsCus(string id)
        {
            if (User.Identity.IsAuthenticated == false)
            {
                return Redirect("/Account/Login");
            }
            ViewBag.Message = "Ухаалаг мэдээ";
            MonitoringEntities monitoringDB = new MonitoringEntities();
            int FbLastID = 0;
            ViewBag.CusID = id;
            ViewBag.UserID = new Guid(User.Identity.GetUserId());
            ViewBag.FbData =
               (from a in monitoringDB.Facebook_Post_Person
                join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
                join c in monitoringDB.System_Person on a.PersonID equals c.ID
                where c.ID.ToString() == id && a.IsDeleted != true
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
                }).Take(30).ToList();

            ViewBag.TwitterData =
             (from tweetPerson in monitoringDB.Twitter_Tweet_Person
              join tweet in monitoringDB.Twitter_Tweets on tweetPerson.TweetID equals tweet.ID
              join sysPerson in monitoringDB.System_Person on tweetPerson.PersonID equals sysPerson.ID
              join tweetUser in monitoringDB.Twitter_User_Details on tweet.TwitterUserID equals tweetUser.TwitterUserID
              where sysPerson.ID.ToString() == id && tweetPerson.IsDeleted != true /*&& tweet.IsManual.Equals("True")*/
              orderby tweet.CreatedAt descending
              select new Models.Twitter
              {
                  ID = tweet.ID,
                  TwitterUserID = tweet.TwitterUserID,
                  TweetID = tweet.TweetID,
                  CreatedAt = tweet.CreatedAt,
                  ScreenName = tweetUser.ScreenName,
                  TwFullPicture = tweet.TwFullPicture,
                  Source = tweet.Source,
                  StatusID = tweet.StatusID,
                  FullText = tweet.FullText,
                  Text = tweet.Text,
                  UserID = tweet.UserID,
                  UrlEntity1 = tweet.UrlEntity1,
                  Sentiment = tweetPerson.Sentiment,
              }).Take(30).ToList();

            ViewBag.WebSiteData =
              (from a in monitoringDB.Website_Post_Person
               join b in monitoringDB.WebSite_Posts on a.PostID equals b.ID
               join c in monitoringDB.System_Person on a.PersonID equals c.ID
               where c.ID.ToString() == id && a.IsDeleted != true
               orderby b.DateTime descending
               select new Models.WebSitePost
               {
                   ID = b.ID,
                   Link = b.Link,
                   Title = b.Title,
                   Name = c.Name,
                   Text = b.Text,
                   PicturePerson = c.Picture,
                   WbFullPicture = b.WbFullPicture,
                   Sentiment = a.Sentiment,
                   Url = b.Url,
                   Body = b.Body.Substring(0, 300) + "...",
                   Reporter = b.Reporter,
                   CoverUrl = b.CoverUrl,
                   DateTime = b.DateTime
               }).Take(30).ToList();

            var PositiveFb = monitoringDB.Facebook_Post_Person.Where(a => a.PersonID.ToString().Equals(id) && a.Sentiment.ToString() == "Positive" && a.IsDeleted != true).Select(n => new Models.SystemUserModels
            {
                ID = n.ID.ToString()
            }).Count();
            var NegativeFb = monitoringDB.Facebook_Post_Person.Where(a => a.PersonID.ToString().Equals(id) && a.Sentiment.ToString() == "Negative" && a.IsDeleted != true).Select(n => new Models.SystemUserModels
            {
                ID = n.ID.ToString()
            }).Count();
            var NeutralFB = monitoringDB.Facebook_Post_Person.Where(a => a.PersonID.ToString().Equals(id) && a.Sentiment.ToString() == "Neutral" && a.IsDeleted != true).Select(n => new Models.SystemUserModels
            {
                ID = n.ID.ToString()
            }).Count();
            var PositiveWb = monitoringDB.Website_Post_Person.Where(a => a.PersonID.ToString().Equals(id) && a.Sentiment.ToString() == "Positive" && a.IsDeleted != true).Select(n => new Models.SystemUserModels
            {
                ID = n.ID.ToString()
            }).Count();
            var NegativeWb = monitoringDB.Website_Post_Person.Where(a => a.PersonID.ToString().Equals(id) && a.Sentiment.ToString() == "Negative" && a.IsDeleted != true).Select(n => new Models.SystemUserModels
            {
                ID = n.ID.ToString()
            }).Count();
            var NeutralWB = monitoringDB.Website_Post_Person.Where(a => a.PersonID.ToString().Equals(id) && a.Sentiment.ToString() == "Neutral" && a.IsDeleted != true).Select(n => new Models.SystemUserModels
            {
                ID = n.ID.ToString()
            }).Count();
            var PositiveTW = monitoringDB.Twitter_Tweet_Person.Where(a => a.PersonID.ToString().Equals(id) && a.Sentiment.ToString() == "Positive" && a.IsDeleted != true).Select(n => new Models.SystemUserModels
            {
                ID = n.ID.ToString()
            }).Count();
            var NegativeTW = monitoringDB.Twitter_Tweet_Person.Where(a => a.PersonID.ToString().Equals(id) && a.Sentiment.ToString() == "Negative" && a.IsDeleted != true).Select(n => new Models.SystemUserModels
            {
                ID = n.ID.ToString()
            }).Count();
            var NeutralTW = monitoringDB.Twitter_Tweet_Person.Where(a => a.PersonID.ToString().Equals(id) && a.Sentiment.ToString() == "Neutral" && a.IsDeleted != true).Select(n => new Models.SystemUserModels
            {
                ID = n.ID.ToString()
            }).Count();
            ViewBag.Positive = PositiveFb + PositiveWb + PositiveTW;
            ViewBag.Negative = NegativeFb + NegativeWb + NegativeTW;
            ViewBag.Neutral = NeutralFB + NeutralWB + NeutralTW;
            ViewBag.FBCount = PositiveFb + NegativeFb + NeutralFB;
            ViewBag.TWCount = PositiveTW + NegativeTW + NeutralTW;
            ViewBag.WBCount = PositiveWb + NegativeWb + NeutralWB;
            foreach (var post in ViewBag.FbData)
            {
                FbLastID++;
                if (FbLastID == 1)
                {
                    ViewBag.FbLastID = post.ID;
                    ViewBag.FbCusName = post.Name;
                    ViewBag.FbPicture = post.PicturePerson;
                }
            }


            //var person = monitoringDB.System_Person.Where(s => s.ID.ToString().Equals(id)).FirstOrDefault();
            //ViewBag.ProfileImage = person.Picture;
            //ViewBag.PersonName = person.Name;
            //ViewBag.Person = person;
            //Bulgaa 04-06 begin
            DateTime last = new DateTime();
            last = DateTime.Now.AddDays(-30);
            DateTime real = DateTime.Now;
            List<DateTime> dates = new List<DateTime>();
            do
            {
                dates.Add(last);
                last = last.AddDays(+1);
            } while (real.Date != last.Date);


            string queryFb = string.Format("select count(*) as count,Convert(date,post.UpdateTime) as date from dbo.[Facebook.Posts] post inner join dbo.[Facebook.Post.Person] person on post.ID = person.PostID and  (person.IsDeleted is null or person.IsDeleted<>1) where CONVERT(date, getdate()) >= Convert(date,post.UpdateTime) and Convert(date,post.UpdateTime) > DATEADD(DD, -30, CONVERT(date, getdate())) and person.PersonID = '{0}' group by Convert(date, post.UpdateTime) order by Convert(date, post.UpdateTime) desc;", id);
            string queryTw = string.Format("select count(*) as count,Convert(date,post.CreatedAt) as date from dbo.[Twitter.Tweets] post inner join dbo.[Twitter.Tweet.Person] person on post.ID = person.TweetID and  (person.IsDeleted is null or person.IsDeleted<>1)  where CONVERT(date, getdate()) >= Convert(date,post.CreatedAt) and Convert(date,post.CreatedAt) > DATEADD(DD, -30, CONVERT(date, getdate())) and person.PersonID = '{0}' group by Convert(date, post.CreatedAt) order by Convert(date, post.CreatedAt) desc;", id);
            string queryWb = string.Format("select count(*) as count,Convert(date,post.Date) as date from dbo.[WebSite.Posts] post inner join dbo.[Website.Post.Person] person on post.ID = person.PostID  and  (person.IsDeleted is null or person.IsDeleted<>1) where CONVERT(date, getdate()) >= Convert(date,post.Date) and Convert(date,post.Date) > DATEADD(DD, -30, CONVERT(date, getdate())) and person.PersonID = '{0}' group by Convert(date, post.Date) order by Convert(date, post.Date) desc;", id);

            var resultsFb = monitoringDB.Database.SqlQuery<graphResult>(queryFb).ToList<graphResult>();
            var resultsTw = monitoringDB.Database.SqlQuery<graphResult>(queryTw).ToList<graphResult>();
            var resultsWb = monitoringDB.Database.SqlQuery<graphResult>(queryWb).ToList<graphResult>();

            string counterfb = "[";
            string countertw = "[";
            string counterweb = "[";
            var fbCount = new List<int>();
            var twCount = new List<int>();
            var webCount = new List<int>();
            #region facebookloop
            for (int i = 0; i < dates.Count; i++)
            {
                DateTime date = dates.ElementAt(i).Date;
                //
                if (resultsFb.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterfb == "[")
                    {
                        counterfb = counterfb + resultsFb.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    counterfb = counterfb + "," + resultsFb.Where(p => p.date.Equals(date)).FirstOrDefault().count;

                }
                else
                {
                    if (counterfb == "[")
                    {
                        counterfb = counterfb + 0;

                    }
                    counterfb = counterfb + "," + 0;


                }
            }
            #endregion
            #region twitterloop
            for (int i = 0; i < dates.Count; i++)
            {
                DateTime date = dates.ElementAt(i).Date;
                //
                if (resultsTw.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (countertw == "[")
                    {
                        countertw = countertw + resultsTw.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    countertw = countertw + "," + resultsTw.Where(p => p.date.Equals(date)).FirstOrDefault().count;

                }
                else
                {
                    if (countertw == "[")
                    {
                        countertw = countertw + 0;

                    }
                    countertw = countertw + "," + 0;

                }
            }
            #endregion
            #region webloop
            for (int i = 0; i < dates.Count; i++)
            {
                DateTime date = dates.ElementAt(i).Date;
                //
                if (resultsWb.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterweb == "[")
                    {
                        counterweb = counterweb + resultsWb.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    counterweb = counterweb + "," + resultsWb.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                }
                else
                {
                    if (counterweb == "[")
                    {
                        counterweb = counterweb + 0;

                    }
                    counterweb = counterweb + "," + 0;

                }
            }
            #endregion

            counterfb = counterfb + "]";
            countertw = countertw + "]";
            counterweb = counterweb + "]";

            ViewBag.facebookChartCount1 = counterfb;
            ViewBag.twitterChartCount1 = countertw;
            ViewBag.websiteChartCount1 = counterweb;
            //bulgaa 2022-04-06 end

            var currentUser = monitoringDB.System_Person.Where(a => a.ID.ToString().Equals(id)).Select(n => new Models.SystemUserModels
            {
                ID = n.ID.ToString(),
                Name = n.Name,
                Surename = n.Surename,
                Tittlename = n.Tittlename,
                Picture = n.Picture,
                Description = n.Description,
                FacebookAccount = n.FacebookAccount.ToString(),
                TwitterAccount = n.TwitterAccount.ToString(),
                Email = n.Email.ToString(),
                PhoneNumber = n.PhoneNumber.ToString(),
                Website = n.Website.ToString()

            }).FirstOrDefault();
            string counterfb14day = "[";
            string countertw14day = "[";
            string counterweb14day = "[";
            string days14 = "[";
            var todayTime = DateTime.Now.AddDays(-2).ToString("dd");
            int j = 1;
            #region facebookloop
            for (int i = 0; i < 29; i++)
            {
                DateTime facebookDate = dates.ElementAt(j + i).Date;
                //
                if (resultsFb.Where(p => p.date.Equals(facebookDate)).Count() > 0)
                {
                    if (counterfb14day == "[")
                    {
                        counterfb14day = counterfb14day + resultsFb.Where(p => p.date.Equals(facebookDate)).FirstOrDefault().count;
                    }
                    counterfb14day = counterfb14day + "," + resultsFb.Where(p => p.date.Equals(facebookDate)).FirstOrDefault().count;

                }
                else
                {
                    if (counterfb14day == "[")
                    {
                        counterfb14day = counterfb14day + 0;

                    }
                    counterfb14day = counterfb14day + "," + 0;


                }
            }
            #endregion
            #region twitterloop
            for (int i = 0; i < 29; i++)
            {
                DateTime twitterDate = dates.ElementAt(j + i).Date;
                //
                if (resultsTw.Where(p => p.date.Equals(twitterDate)).Count() > 0)
                {
                    if (countertw14day == "[")
                    {
                        countertw14day = countertw14day + resultsTw.Where(p => p.date.Equals(twitterDate)).FirstOrDefault().count;
                    }
                    countertw14day = countertw14day + "," + resultsTw.Where(p => p.date.Equals(twitterDate)).FirstOrDefault().count;

                }
                else
                {
                    if (countertw14day == "[")
                    {
                        countertw14day = countertw14day + 0;

                    }
                    countertw14day = countertw14day + "," + 0;

                }
            }
            #endregion
            #region webloop
            for (int i = 0; i < 29; i++)
            {
                DateTime webDate = dates.ElementAt(j + i).Date;
                //
                if (resultsWb.Where(p => p.date.Equals(webDate)).Count() > 0)
                {
                    if (counterweb14day == "[")
                    {
                        counterweb14day = counterweb14day + resultsWb.Where(p => p.date.Equals(webDate)).FirstOrDefault().count;
                    }
                    counterweb14day = counterweb14day + "," + resultsWb.Where(p => p.date.Equals(webDate)).FirstOrDefault().count;
                }
                else
                {
                    if (counterweb14day == "[")
                    {
                        counterweb14day = counterweb14day + 0;

                    }
                    counterweb14day = counterweb14day + "," + 0;

                }
            }
            #endregion

            #region 14daysloop
            for (int i = 0; i < 30; i++)
            {
                if (i == 29)
                {
                    days14 = days14 + DateTime.Now.AddDays(-29 + i).ToString("dd");
                }
                else
                {
                    days14 = days14 + DateTime.Now.AddDays(-29 + i).ToString("dd") + ",";
                }

            }
            #endregion
            var teste123 = DateTime.Now.AddDays(-2).ToString("dd");

            DateTime date324 = dates.ElementAt(28).Date;
            counterfb14day = counterfb14day + "]";
            countertw14day = countertw14day + "]";
            counterweb14day = counterweb14day + "]";
            days14 = days14 + "]";

            ViewBag.counterfb14day = counterfb14day;
            ViewBag.countertw14day = countertw14day;
            ViewBag.counterweb14day = counterweb14day;
            ViewBag.days14 = days14;

            return View(currentUser);
        }
        public ActionResult SmartNewsCusAllList(string id)
        {
            ViewBag.Message = "Ухаалаг мэдээ";
            MonitoringEntities monitoringDB = new MonitoringEntities();
            int FbLastID = 0;
            ViewBag.CusAlllistID = id;
            ViewBag.FbData =
               (from a in monitoringDB.Facebook_Post_Person
                join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
                join c in monitoringDB.System_Person on a.PersonID equals c.ID
                where c.ID.ToString() == id && a.IsDeleted != true
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
                }).Take(5).ToList();

            ViewBag.TwitterData =
               (from tweetPerson in monitoringDB.Twitter_Tweet_Person
                join tweet in monitoringDB.Twitter_Tweets on tweetPerson.TweetID equals tweet.ID
                join sysPerson in monitoringDB.System_Person on tweetPerson.PersonID equals sysPerson.ID
                join tweetUser in monitoringDB.Twitter_User_Details on tweet.TwitterUserID equals tweetUser.ID
                where sysPerson.ID.ToString() == id && tweetPerson.IsDeleted != true/* && tweet.IsManual.Equals("True")*/
                orderby tweet.CreatedAt descending
                select new Models.Twitter
                {
                    ID = tweet.ID,
                    TwitterUserID = tweet.TwitterUserID,
                    TweetID = tweet.TweetID,
                    CreatedAt = tweet.CreatedAt,
                    ScreenName = tweetUser.ScreenName,
                    TwFullPicture = tweet.TwFullPicture,
                    Source = tweet.Source,
                    StatusID = tweet.StatusID,
                    UserID = tweet.UserID,
                    Sentiment = tweetPerson.Sentiment,

                }).Take(5).ToList();

            ViewBag.WebSiteData =
              (from a in monitoringDB.Website_Post_Person
               join b in monitoringDB.WebSite_Posts on a.PostID equals b.ID
               join c in monitoringDB.System_Person on a.PersonID equals c.ID
               where c.ID.ToString() == id && a.IsDeleted != true
               orderby b.Date descending
               select new Models.WebSitePost
               {
                   ID = b.ID,
                   Link = b.Link,
                   Title = b.Title,
                   Name = c.Name,
                   Text = b.Text,
                   PicturePerson = c.Picture,
                   WbFullPicture = b.WbFullPicture,
                   Sentiment = a.Sentiment,
                   Url = b.Url,
                   Body = b.Body.Substring(0, 300) + "...",
                   Reporter = b.Reporter,
                   CoverUrl = b.CoverUrl,
                   DateTime = b.DateTime
               }).Take(5).ToList();



            DateTime last = new DateTime();
            last = DateTime.Now.AddDays(-30);
            DateTime real = DateTime.Now;
            List<DateTime> dates = new List<DateTime>();
            do
            {
                dates.Add(last);
                last = last.AddDays(+1);
            } while (real.Date != last.Date);


            string queryFb = string.Format("select count(*) as count,Convert(date,post.UpdateTime) as date from dbo.[Facebook.Posts] post inner join dbo.[Facebook.Post.Person] person on post.ID = person.PostID and  (person.IsDeleted is null or person.IsDeleted<>1) where CONVERT(date, getdate()) >= Convert(date,post.UpdateTime) and Convert(date,post.UpdateTime) > DATEADD(DD, -30, CONVERT(date, getdate())) and person.PersonID = '{0}' group by Convert(date, post.UpdateTime) order by Convert(date, post.UpdateTime) desc;", id);
            string queryTw = string.Format("select count(*) as count,Convert(date,post.CreatedAt) as date from dbo.[Twitter.Tweets] post inner join dbo.[Twitter.Tweet.Person] person on post.ID = person.TweetID and  (person.IsDeleted is null or person.IsDeleted<>1)  where CONVERT(date, getdate()) >= Convert(date,post.CreatedAt) and Convert(date,post.CreatedAt) > DATEADD(DD, -30, CONVERT(date, getdate())) and person.PersonID = '{0}' group by Convert(date, post.CreatedAt) order by Convert(date, post.CreatedAt) desc;", id);
            string queryWb = string.Format("select count(*) as count,Convert(date,post.Date) as date from dbo.[WebSite.Posts] post inner join dbo.[Website.Post.Person] person on post.ID = person.PostID  and  (person.IsDeleted is null or person.IsDeleted<>1) where CONVERT(date, getdate()) >= Convert(date,post.Date) and Convert(date,post.Date) > DATEADD(DD, -30, CONVERT(date, getdate())) and person.PersonID = '{0}' group by Convert(date, post.Date) order by Convert(date, post.Date) desc;", id);

            var resultsFb = monitoringDB.Database.SqlQuery<graphResult>(queryFb).ToList<graphResult>();
            var resultsTw = monitoringDB.Database.SqlQuery<graphResult>(queryTw).ToList<graphResult>();
            var resultsWb = monitoringDB.Database.SqlQuery<graphResult>(queryWb).ToList<graphResult>();

            string counterfb = "[";
            string countertw = "[";
            string counterweb = "[";
            var fbCount = new List<int>();
            var twCount = new List<int>();
            var webCount = new List<int>();
            #region facebookloop
            for (int i = 0; i < dates.Count; i++)
            {
                DateTime date = dates.ElementAt(i).Date;
                //
                if (resultsFb.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterfb == "[")
                    {
                        counterfb = counterfb + resultsFb.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    counterfb = counterfb + "," + resultsFb.Where(p => p.date.Equals(date)).FirstOrDefault().count;

                }
                else
                {
                    if (counterfb == "[")
                    {
                        counterfb = counterfb + 0;

                    }
                    counterfb = counterfb + "," + 0;


                }
            }
            #endregion
            #region twitterloop
            for (int i = 0; i < dates.Count; i++)
            {
                DateTime date = dates.ElementAt(i).Date;
                //
                if (resultsTw.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (countertw == "[")
                    {
                        countertw = countertw + resultsTw.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    countertw = countertw + "," + resultsTw.Where(p => p.date.Equals(date)).FirstOrDefault().count;

                }
                else
                {
                    if (countertw == "[")
                    {
                        countertw = countertw + 0;

                    }
                    countertw = countertw + "," + 0;

                }
            }
            #endregion
            #region webloop
            for (int i = 0; i < dates.Count; i++)
            {
                DateTime date = dates.ElementAt(i).Date;
                //
                if (resultsWb.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterweb == "[")
                    {
                        counterweb = counterweb + resultsWb.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    counterweb = counterweb + "," + resultsWb.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                }
                else
                {
                    if (counterweb == "[")
                    {
                        counterweb = counterweb + 0;

                    }
                    counterweb = counterweb + "," + 0;

                }
            }
            #endregion

            counterfb = counterfb + "]";
            countertw = countertw + "]";
            counterweb = counterweb + "]";

            ViewBag.facebookChartCount1 = counterfb;
            ViewBag.twitterChartCount1 = countertw;
            ViewBag.websiteChartCount1 = counterweb;

            var currentUser = monitoringDB.System_Person.Where(a => a.ID.ToString().Equals(id)).Select(n => new Models.SystemUserModels
            {
                ID = n.ID.ToString(),
                Name = n.Name,
                Surename = n.Surename,
                Tittlename = n.Tittlename,
                Picture = n.Picture,
                Description = n.Description,
                FacebookAccount = n.FacebookAccount.ToString(),
                TwitterAccount = n.TwitterAccount.ToString(),
                Email = n.Email.ToString(),
                PhoneNumber = n.PhoneNumber.ToString(),
                Website = n.Website.ToString()

            }).FirstOrDefault();
            return View(currentUser);
        }

        public ActionResult SmartNewsCusMainGraph(string id)
        {

            MonitoringEntities monitoringDB = new MonitoringEntities();
            // making date list
            DateTime last = new DateTime();
            last = DateTime.Now.AddDays(-30);
            DateTime real = DateTime.Now;
            List<DateTime> dates = new List<DateTime>();
            do
            {
                dates.Add(last);
                last = last.AddDays(+1);
            } while (real.Date != last.Date);


            string queryFb = string.Format("select count(*) as count,Convert(date,post.UpdateTime) as date from dbo.[Facebook.Posts] post inner join dbo.[Facebook.Post.Person] person on post.ID = person.PostID and  (person.IsDeleted is null or person.IsDeleted<>1) where CONVERT(date, getdate()) >= Convert(date,post.UpdateTime) and Convert(date,post.UpdateTime) > DATEADD(DD, -30, CONVERT(date, getdate())) and person.PersonID = '{0}' group by Convert(date, post.UpdateTime) order by Convert(date, post.UpdateTime) desc;", id);
            string queryTw = string.Format("select count(*) as count,Convert(date,post.CreatedAt) as date from dbo.[Twitter.Tweets] post inner join dbo.[Twitter.Tweet.Person] person on post.ID = person.TweetID and  (person.IsDeleted is null or person.IsDeleted<>1)  where CONVERT(date, getdate()) >= Convert(date,post.CreatedAt) and Convert(date,post.CreatedAt) > DATEADD(DD, -30, CONVERT(date, getdate())) and person.PersonID = '{0}' group by Convert(date, post.CreatedAt) order by Convert(date, post.CreatedAt) desc;", id);
            string queryWb = string.Format("select count(*) as count,Convert(date,post.Date) as date from dbo.[WebSite.Posts] post inner join dbo.[Website.Post.Person] person on post.ID = person.PostID  and  (person.IsDeleted is null or person.IsDeleted<>1) where CONVERT(date, getdate()) >= Convert(date,post.Date) and Convert(date,post.Date) > DATEADD(DD, -30, CONVERT(date, getdate())) and person.PersonID = '{0}' group by Convert(date, post.Date) order by Convert(date, post.Date) desc;", id);

            var resultsFb = monitoringDB.Database.SqlQuery<graphResult>(queryFb).ToList<graphResult>();
            var resultsTw = monitoringDB.Database.SqlQuery<graphResult>(queryTw).ToList<graphResult>();
            var resultsWb = monitoringDB.Database.SqlQuery<graphResult>(queryWb).ToList<graphResult>();

            string counterfb = "[";
            string countertw = "[";
            string counterweb = "[";
            var fbCount = new List<int>();
            var twCount = new List<int>();
            var webCount = new List<int>();
            #region facebookloop
            for (int i = 0; i < dates.Count; i++)
            {
                DateTime date = dates.ElementAt(i).Date;
                //
                if (resultsFb.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterfb == "[")
                    {
                        counterfb = counterfb + resultsFb.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    counterfb = counterfb + "," + resultsFb.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    fbCount.Add(resultsFb.Where(p => p.date.Equals(date)).FirstOrDefault().count);
                }
                else
                {
                    if (counterfb == "[")
                    {
                        counterfb = counterfb + 0;

                    }
                    counterfb = counterfb + "," + 0;
                    fbCount.Add(0);

                }
            }
            #endregion
            #region twitterloop
            for (int i = 0; i < dates.Count; i++)
            {
                DateTime date = dates.ElementAt(i).Date;
                //
                if (resultsTw.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (countertw == "[")
                    {
                        countertw = countertw + resultsTw.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    countertw = countertw + "," + resultsTw.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    twCount.Add(resultsTw.Where(p => p.date.Equals(date)).FirstOrDefault().count);
                }
                else
                {
                    if (countertw == "[")
                    {
                        countertw = countertw + 0;

                    }
                    countertw = countertw + "," + 0;
                    twCount.Add(0);
                }
            }
            #endregion
            #region webloop
            for (int i = 0; i < dates.Count; i++)
            {
                DateTime date = dates.ElementAt(i).Date;
                //
                if (resultsWb.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterweb == "[")
                    {
                        counterweb = counterweb + resultsWb.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    webCount.Add(resultsWb.Where(p => p.date.Equals(date)).FirstOrDefault().count);
                }
                else
                {
                    if (counterweb == "[")
                    {
                        counterweb = counterweb + 0;

                    }
                    counterweb = counterweb + "," + 0;
                    webCount.Add(0);
                }
            }
            #endregion

            counterfb = counterfb + "]";
            countertw = countertw + "]";
            counterweb = counterweb + "]";

            ViewBag.facebookChartCount = counterfb;
            ViewBag.twitterChartCount = countertw;
            ViewBag.websiteChartCount = counterweb;
            ViewBag.fbCount = fbCount;
            ViewBag.twCount = twCount;
            ViewBag.webCount = webCount;
            // Facebook Chart ///
            string queryFbPos = string.Format("select count(*) as count,Convert(date,post.UpdateTime) as date from dbo.[Facebook.Posts] post inner join dbo.[Facebook.Post.Person] person on post.ID = person.PostID and (person.IsDeleted is null or person.IsDeleted<>1) where CONVERT(date, getdate()) >= Convert(date,post.UpdateTime) and Convert(date,post.UpdateTime) > DATEADD(DD, -30, CONVERT(date, getdate())) and person.PersonID = '{0}' and person.Sentiment = 'Positive' group by Convert(date, post.UpdateTime) order by Convert(date, post.UpdateTime) desc;", id);
            string queryFbNeg = string.Format("select count(*) as count,Convert(date,post.UpdateTime) as date from dbo.[Facebook.Posts] post inner join dbo.[Facebook.Post.Person] person on post.ID = person.PostID and (person.IsDeleted is null or person.IsDeleted<>1)  where CONVERT(date, getdate()) >= Convert(date,post.UpdateTime) and Convert(date,post.UpdateTime) > DATEADD(DD, -30, CONVERT(date, getdate())) and person.PersonID = '{0}' and person.Sentiment = 'Negative' group by Convert(date, post.UpdateTime) order by Convert(date, post.UpdateTime) desc;", id);
            string queryFbNeu = string.Format("select count(*) as count,Convert(date,post.UpdateTime) as date from dbo.[Facebook.Posts] post inner join dbo.[Facebook.Post.Person] person on post.ID = person.PostID and (person.IsDeleted is null or person.IsDeleted<>1)  where CONVERT(date, getdate()) >= Convert(date,post.UpdateTime) and Convert(date,post.UpdateTime) > DATEADD(DD, -30, CONVERT(date, getdate())) and person.PersonID = '{0}' and person.Sentiment = 'Neutral' group by Convert(date, post.UpdateTime) order by Convert(date, post.UpdateTime) desc;", id);

            var resultsFbPos = monitoringDB.Database.SqlQuery<graphResult>(queryFbPos).ToList<graphResult>();
            var resultsFbNeg = monitoringDB.Database.SqlQuery<graphResult>(queryFbNeg).ToList<graphResult>();
            var resultsFbNeu = monitoringDB.Database.SqlQuery<graphResult>(queryFbNeu).ToList<graphResult>();

            string counterfbPos = "[";
            string counterfbNeg = "[";
            string counterfbNeu = "[";

            #region FbPosloop
            for (int i = 0; i < dates.Count; i++)
            {
                DateTime date = dates.ElementAt(i).Date;
                //
                if (resultsFbPos.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterfbPos == "[")
                    {
                        counterfbPos = counterfbPos + resultsFbPos.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    counterfbPos = counterfbPos + "," + resultsFbPos.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                }
                else
                {
                    if (counterfbPos == "[")
                    {
                        counterfbPos = counterfbPos + 0;

                    }
                    counterfbPos = counterfbPos + "," + 0;
                }
            }
            #endregion           
            #region FbNegloop
            for (int i = 0; i < dates.Count; i++)
            {
                DateTime date = dates.ElementAt(i).Date;
                //
                if (resultsFbNeg.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterfbNeg == "[")
                    {
                        counterfbNeg = counterfbNeg + resultsFbNeg.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    counterfbNeg = counterfbNeg + "," + resultsFbNeg.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                }
                else
                {
                    if (counterfbNeg == "[")
                    {
                        counterfbNeg = counterfbNeg + 0;
                    }
                    counterfbNeg = counterfbNeg + "," + 0;
                }
            }
            #endregion   
            #region FbNeuloop
            for (int i = 0; i < dates.Count; i++)
            {
                DateTime date = dates.ElementAt(i).Date;
                //
                if (resultsFbNeu.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterfbNeu == "[")
                    {
                        counterfbNeu = counterfbNeu + resultsFbNeu.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    counterfbNeu = counterfbNeu + "," + resultsFbNeu.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                }
                else
                {
                    if (counterfbNeu == "[")
                    {
                        counterfbNeu = counterfbNeu + 0;
                    }
                    counterfbNeu = counterfbNeu + "," + 0;
                }
            }
            #endregion 

            counterfbPos = counterfbPos + "]";
            counterfbNeg = counterfbNeg + "]";
            counterfbNeu = counterfbNeu + "]";

            ViewBag.FbPosChartCount = counterfbPos;
            ViewBag.FbNegChartCount = counterfbNeg;
            ViewBag.FbNeuChartCount = counterfbNeu;
            // End Facebook Chart //

            // Facebook Chart ///
            string queryTwPos = string.Format("select count(*) as count,Convert(date,tweet.CreatedAt) as date from dbo.[Twitter.Tweets] tweet inner join dbo.[Twitter.Tweet.Person] person on tweet.ID = person.TweetID  and (person.IsDeleted is null or person.IsDeleted<>1)  where CONVERT(date, getdate()) >= Convert(date,tweet.CreatedAt) and Convert(date,tweet.CreatedAt) > DATEADD(DD, -30, CONVERT(date, getdate())) and person.PersonID = '{0}' and person.Sentiment = 'Positive' group by Convert(date, tweet.CreatedAt) order by Convert(date, tweet.CreatedAt) desc;", id);
            string queryTwNeg = string.Format("select count(*) as count,Convert(date,tweet.CreatedAt) as date from dbo.[Twitter.Tweets] tweet inner join dbo.[Twitter.Tweet.Person] person on tweet.ID = person.TweetID  and (person.IsDeleted is null or person.IsDeleted<>1)  where CONVERT(date, getdate()) >= Convert(date,tweet.CreatedAt) and Convert(date,tweet.CreatedAt) > DATEADD(DD, -30, CONVERT(date, getdate())) and person.PersonID = '{0}' and person.Sentiment = 'Negative' group by Convert(date, tweet.CreatedAt) order by Convert(date, tweet.CreatedAt) desc;", id);
            string queryTwNeu = string.Format("select count(*) as count,Convert(date,tweet.CreatedAt) as date from dbo.[Twitter.Tweets] tweet inner join dbo.[Twitter.Tweet.Person] person on tweet.ID = person.TweetID  and (person.IsDeleted is null or person.IsDeleted<>1)  where CONVERT(date, getdate()) >= Convert(date,tweet.CreatedAt) and Convert(date,tweet.CreatedAt) > DATEADD(DD, -30, CONVERT(date, getdate())) and person.PersonID = '{0}' and person.Sentiment = 'Neutral' group by Convert(date, tweet.CreatedAt) order by Convert(date, tweet.CreatedAt) desc;", id);

            var resultsTwPos = monitoringDB.Database.SqlQuery<graphResult>(queryTwPos).ToList<graphResult>();
            var resultsTwNeg = monitoringDB.Database.SqlQuery<graphResult>(queryTwNeg).ToList<graphResult>();
            var resultsTwNeu = monitoringDB.Database.SqlQuery<graphResult>(queryTwNeu).ToList<graphResult>();

            string counterTwPos = "[";
            string counterTwNeg = "[";
            string counterTwNeu = "[";

            #region TwPosloop
            for (int i = 0; i < dates.Count; i++)
            {
                DateTime date = dates.ElementAt(i).Date;
                //
                if (resultsTwPos.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterTwPos == "[")
                    {
                        counterTwPos = counterTwPos + resultsTwPos.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    counterTwPos = counterTwPos + "," + resultsTwPos.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                }
                else
                {
                    if (counterTwPos == "[")
                    {
                        counterTwPos = counterTwPos + 0;

                    }
                    counterTwPos = counterTwPos + "," + 0;
                }
            }
            #endregion           
            #region TwNegloop
            for (int i = 0; i < dates.Count; i++)
            {
                DateTime date = dates.ElementAt(i).Date;
                //
                if (resultsTwNeg.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterTwNeg == "[")
                    {
                        counterTwNeg = counterTwNeg + resultsTwNeg.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    counterTwNeg = counterTwNeg + "," + resultsTwNeg.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                }
                else
                {
                    if (counterTwNeg == "[")
                    {
                        counterTwNeg = counterTwNeg + 0;
                    }
                    counterTwNeg = counterTwNeg + "," + 0;
                }
            }
            #endregion   
            #region TwNeuloop
            for (int i = 0; i < dates.Count; i++)
            {
                DateTime date = dates.ElementAt(i).Date;
                //
                if (resultsTwNeu.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterTwNeu == "[")
                    {
                        counterTwNeu = counterTwNeu + resultsTwNeu.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    counterTwNeu = counterTwNeu + "," + resultsTwNeu.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                }
                else
                {
                    if (counterTwNeu == "[")
                    {
                        counterTwNeu = counterTwNeu + 0;
                    }
                    counterTwNeu = counterTwNeu + "," + 0;
                }
            }
            #endregion 

            counterTwPos = counterTwPos + "]";
            counterTwNeg = counterTwNeg + "]";
            counterTwNeu = counterTwNeu + "]";

            ViewBag.TwPosChartCount = counterTwPos;
            ViewBag.TwNegChartCount = counterTwNeg;
            ViewBag.TwNeuChartCount = counterTwNeu;
            // End Twitter Chart //

            // Facebook Chart ///
            string queryWebPos = string.Format("select count(*) as count,Convert(date,post.Date) as date from dbo.[WebSite.Posts] post inner join dbo.[Website.Post.Person] person on post.ID = person.PostID  and (person.IsDeleted is null or person.IsDeleted<>1)  where CONVERT(date, getdate()) >= Convert(date,post.Date) and Convert(date,post.Date) > DATEADD(DD, -30, CONVERT(date, getdate())) and person.PersonID = '{0}' and person.Sentiment = 'Positive' group by Convert(date, post.Date) order by Convert(date, post.Date) desc;", id);
            string queryWebNeg = string.Format("select count(*) as count,Convert(date,post.Date) as date from dbo.[WebSite.Posts] post inner join dbo.[Website.Post.Person] person on post.ID = person.PostID  and (person.IsDeleted is null or person.IsDeleted<>1)  where CONVERT(date, getdate()) >= Convert(date,post.Date) and Convert(date,post.Date) > DATEADD(DD, -30, CONVERT(date, getdate())) and person.PersonID = '{0}' and person.Sentiment = 'Negative' group by Convert(date, post.Date) order by Convert(date, post.Date) desc;", id);
            string queryWebNeu = string.Format("select count(*) as count,Convert(date,post.Date) as date from dbo.[WebSite.Posts] post inner join dbo.[Website.Post.Person] person on post.ID = person.PostID  and (person.IsDeleted is null or person.IsDeleted<>1) where CONVERT(date, getdate()) >= Convert(date,post.Date) and Convert(date,post.Date) > DATEADD(DD, -30, CONVERT(date, getdate())) and person.PersonID = '{0}' and person.Sentiment = 'Neutral' group by Convert(date, post.Date) order by Convert(date, post.Date) desc;", id);

            var resultsWebPos = monitoringDB.Database.SqlQuery<graphResult>(queryWebPos).ToList<graphResult>();
            var resultsWebNeg = monitoringDB.Database.SqlQuery<graphResult>(queryWebNeg).ToList<graphResult>();
            var resultsWebNeu = monitoringDB.Database.SqlQuery<graphResult>(queryWebNeu).ToList<graphResult>();

            string counterWebPos = "[";
            string counterWebNeg = "[";
            string counterWebNeu = "[";

            #region WebPosloop
            for (int i = 0; i < dates.Count; i++)
            {
                DateTime date = dates.ElementAt(i).Date;
                //
                if (resultsWebPos.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterWebPos == "[")
                    {
                        counterWebPos = counterWebPos + resultsWebPos.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    counterWebPos = counterWebPos + "," + resultsWebPos.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                }
                else
                {
                    if (counterWebPos == "[")
                    {
                        counterWebPos = counterWebPos + 0;

                    }
                    counterWebPos = counterWebPos + "," + 0;
                }
            }
            #endregion           
            #region WebNegloop
            for (int i = 0; i < dates.Count; i++)
            {
                DateTime date = dates.ElementAt(i).Date;
                //
                if (resultsWebNeu.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterWebNeg == "[")
                    {
                        counterWebNeg = counterWebNeg + resultsWebNeu.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    counterWebNeg = counterWebNeg + "," + resultsWebNeu.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                }
                else
                {
                    if (counterWebNeg == "[")
                    {
                        counterWebNeg = counterWebNeg + 0;
                    }
                    counterWebNeg = counterWebNeg + "," + 0;
                }
            }
            #endregion   
            #region WebNeuloop
            for (int i = 0; i < dates.Count; i++)
            {
                DateTime date = dates.ElementAt(i).Date;
                //
                if (resultsWebNeu.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterWebNeu == "[")
                    {
                        counterWebNeu = counterWebNeu + resultsWebNeu.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    counterWebNeu = counterWebNeu + "," + resultsWebNeu.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                }
                else
                {
                    if (counterWebNeu == "[")
                    {
                        counterWebNeu = counterWebNeu + 0;
                    }
                    counterWebNeu = counterWebNeu + "," + 0;
                }
            }
            #endregion 

            counterWebPos = counterWebPos + "]";
            counterWebNeg = counterWebNeg + "]";
            counterWebNeu = counterWebNeu + "]";

            ViewBag.WebPosChartCount = counterWebPos;
            ViewBag.WebNegChartCount = counterWebNeg;
            ViewBag.WebNeuChartCount = counterWebNeu;
            return PartialView();
        }
        public ActionResult SmartNewsCusComments(string UserID)
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();
            var comments = (from comment in monitoringDB.Facebook_Post_Comments
                            join post in monitoringDB.Facebook_Posts on comment.PostID equals post.ID
                            join per in monitoringDB.Facebook_Post_Person on post.ID equals per.PostID
                            orderby comment.RegisteredDate descending
                            where per.PersonID.ToString() == UserID
                            select new Models.FbComment
                            {
                                CommentID = comment.CommentID,
                                FromName = comment.FromName,
                                Message = comment.Message,
                                CreateTime = comment.CreateTime
                            }).Take(40).ToList();
            return PartialView(comments);
        }
        public ActionResult SmartNewsCusNextFbData(string UserID, string counter)
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();
            int skip = Int32.Parse(counter) * 5;
            var FbData = (from a in monitoringDB.Facebook_Post_Person
                          join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
                          join c in monitoringDB.System_Person on a.PersonID equals c.ID
                          where c.ID.ToString() == UserID && a.IsDeleted != true
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
                          }).Skip(skip).Take(5).ToList();
            return PartialView(FbData);
        }
        public ActionResult SmartNewsCusNextTwData(string UserID, string counter)
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();
            int skip = Int32.Parse(counter) * 5;
            ViewBag.UserID = new Guid(User.Identity.GetUserId());
            var TwData = (from tweetPerson in monitoringDB.Twitter_Tweet_Person
                          join tweet in monitoringDB.Twitter_Tweets on tweetPerson.TweetID equals tweet.ID
                          join sysPerson in monitoringDB.System_Person on tweetPerson.PersonID equals sysPerson.ID
                          join tweetUser in monitoringDB.Twitter_User_Details on tweet.TwitterUserID equals tweetUser.TwitterUserID
                          where sysPerson.ID.ToString() == UserID && tweetPerson.IsDeleted != true /*&& tweet.IsManual.Equals("True")*/
                          orderby tweet.CreatedAt descending
                          select new Models.Twitter
                          {
                              ID = tweet.ID,
                              TwitterUserID = tweet.TwitterUserID,
                              TweetID = tweet.TweetID,
                              CreatedAt = tweet.CreatedAt,
                              ScreenName = tweetUser.ScreenName,
                              TwFullPicture = tweet.TwFullPicture,
                              Source = tweet.Source,
                              StatusID = tweet.StatusID,
                              FullText = tweet.FullText,
                              Text = tweet.Text,
                              UserID = tweet.UserID,
                              UrlEntity1 = tweet.UrlEntity1,
                              Sentiment = tweetPerson.Sentiment,
                          }).Skip(skip).Take(5).ToList();
            /*var TwData= (from a in monitoringDB.Twitter_Tweet_Person
                         join tweet in monitoringDB.Twitter_Tweets on a.TweetID equals tweet.ID
                         join c in monitoringDB.System_Person on a.PersonID equals c.ID
                         join user in monitoringDB.Twitter_User_Details on tweet.TwitterUserID equals user.TwitterUserID
                         where c.ID.ToString() == UserID && a.IsDeleted != true
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
                             UserID = tweet.UserID,
                             Sentiment = a.Sentiment,
                             TwFullPicture = tweet.TwFullPicture,
                         }).Skip(skip).Take(5).ToList();*/
            return PartialView(TwData);
        }
        public ActionResult SmartNewsCusNextWebData(string UserID, string counter)
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();
            int skip = Int32.Parse(counter) * 10;
            var WebData = (from a in monitoringDB.Website_Post_Person
                           join b in monitoringDB.WebSite_Posts on a.PostID equals b.ID
                           join c in monitoringDB.System_Person on a.PersonID equals c.ID
                           where c.ID.ToString() == UserID && a.IsDeleted != true
                           orderby b.DateTime descending
                           select new Models.WebSitePost
                           {
                               ID = b.ID,
                               Link = b.Link,
                               Title = b.Title,
                               Name = c.Name,
                               Text = b.Text,
                               PicturePerson = c.Picture,
                               Sentiment = a.Sentiment,
                               Url = b.Url,
                               Body = b.Body.Substring(0, 300) + "...",
                               Reporter = b.Reporter,
                               CoverUrl = b.CoverUrl,
                               DateTime = b.DateTime,
                               WbFullPicture = b.WbFullPicture
                           }).Skip(skip).Take(10).ToList();
            return PartialView(WebData);
        }
        /*bulgaa Only Website Data 2022-04-14 Begin*/
        public ActionResult SmartNewsCusOnlyWebData(string UserID, string counter)
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();
            int skip = Int32.Parse(counter) * 6;
            var WebData = (from a in monitoringDB.Website_Post_Person
                           join b in monitoringDB.WebSite_Posts on a.PostID equals b.ID
                           join c in monitoringDB.System_Person on a.PersonID equals c.ID
                           where c.ID.ToString() == UserID && a.IsDeleted != true
                           orderby b.Date descending
                           select new Models.WebSitePost
                           {
                               ID = b.ID,
                               Link = b.Link,
                               Title = b.Title,
                               Name = c.Name,
                               Text = b.Text,
                               PicturePerson = c.Picture,
                               Sentiment = a.Sentiment,
                               Url = b.Url,
                               //bulgaa 2022-05-05
                               StringUrl = b.Url.Replace(@"//", "").Substring(0, 22) + "...",
                               Body = b.Body.Substring(0, 300) + "...",
                               Reporter = b.Reporter,
                               CoverUrl = b.CoverUrl,
                               DateTime = b.DateTime
                           }).Skip(skip).Take(6).ToList();
            return PartialView(WebData);
        }
        /*bulgaa Only Website Data 2022-04-14 End*/

        /*bulgaa Only Facebook Data 2022-04-14 Begin*/
        public ActionResult SmartNewsCusOnlyFacebookData(string UserID, string counter)
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();
            int skip = Int32.Parse(counter) * 6;
            var FbData = (from a in monitoringDB.Facebook_Post_Person
                          join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
                          join c in monitoringDB.System_Person on a.PersonID equals c.ID
                          where c.ID.ToString() == UserID && a.IsDeleted != true
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
                          }).Skip(skip).Take(6).ToList();
            return PartialView(FbData);
        }
        /*bulgaa Only Facebook Data 2022-04-14 End*/

        /*bulgaa Only Twitter Data 2022-04-14 Begin*/
        public ActionResult SmartNewsCusOnlyTwitterData(string UserID, string counter)
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();
            int skip = Int32.Parse(counter) * 6;
            var TwData = (from a in monitoringDB.Twitter_Tweet_Person
                          join tweet in monitoringDB.Twitter_Tweets on a.TweetID equals tweet.ID
                          join c in monitoringDB.System_Person on a.PersonID equals c.ID
                          join user in monitoringDB.Twitter_User_Details on tweet.TwitterUserID equals user.TwitterUserID
                          where c.ID.ToString() == UserID && a.IsDeleted != true
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
                              UserID = tweet.UserID,
                              Sentiment = a.Sentiment,
                          }).Skip(skip).Take(6).ToList();
            return PartialView(TwData);
        }
        /*bulgaa Only Twitter Data 2022-04-14 End*/

        /*javhaa analyze, fb, twitter, web post save start*/
        public ActionResult Analyze(string id2)
        {
            if (User.Identity.IsAuthenticated == false)
            {
                return Redirect("/Account/Login");
            }
            MonitoringEntities monitoringDB = new MonitoringEntities();
            var id = id2;
            ViewBag.id = id;
            var userIdentity = (ClaimsIdentity)User.Identity;
            var claims = userIdentity.Claims;
            var roles = claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).FirstOrDefault();
            ViewBag.userList =
                 (from a in monitoringDB.Person_User
                  where a.userID == roles
                  select new Models.Person
                  {
                      AspNetUserID = a.personID,
                      Name = a.name
                  }).ToList();

            ViewBag.FbData =
              (from a in monitoringDB.Facebook_Post_Person
               join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
               join c in monitoringDB.System_Person on a.PersonID equals c.ID
               where c.ID.ToString() == id && a.IsDeleted != true
               orderby b.UpdateTime descending
               select new Models.FbPost
               {
                   ID = b.ID,
               }).Count();

            ViewBag.TwitterData =
               (from tweetPerson in monitoringDB.Twitter_Tweet_Person
                join tweet in monitoringDB.Twitter_Tweets on tweetPerson.TweetID equals tweet.ID
                join sysPerson in monitoringDB.System_Person on tweetPerson.PersonID equals sysPerson.ID
                join tweetUser in monitoringDB.Twitter_User_Details on tweet.TwitterUserID equals tweetUser.TwitterUserID
                where sysPerson.ID.ToString() == id && tweetPerson.IsDeleted != true
                orderby tweet.CreatedAt descending
                select new Models.Twitter
                {
                    ID = tweet.ID,
                }).Count();

            ViewBag.WebSiteData =
              (from a in monitoringDB.Website_Post_Person
               join b in monitoringDB.WebSite_Posts on a.PostID equals b.ID
               join c in monitoringDB.System_Person on a.PersonID equals c.ID
               where c.ID.ToString() == id && a.IsDeleted != true
               orderby b.Date descending
               select new Models.WebSitePost
               {
                   ID = b.ID,
               }).Count();
            var currentUser = monitoringDB.System_Person.Where(a => a.ID.ToString().Equals(id)).Select(n => new Models.SystemUserModels
            {
                ID = n.ID.ToString(),
                Name = n.Name,
                Surename = n.Surename,
                Tittlename = n.Tittlename,
                Picture = n.Picture,
                Description = n.Description,
                FacebookAccount = n.FacebookAccount.ToString(),
                TwitterAccount = n.TwitterAccount.ToString()
            }).FirstOrDefault();
            var PositiveFb = monitoringDB.Facebook_Post_Person.Where(a => a.PersonID.ToString().Equals(id) && a.Sentiment.ToString() == "Positive").Select(n => new Models.SystemUserModels
            {
                ID = n.ID.ToString()
            }).Count();
            var NegativeFb = monitoringDB.Facebook_Post_Person.Where(a => a.PersonID.ToString().Equals(id) && a.Sentiment.ToString() == "Negative").Select(n => new Models.SystemUserModels
            {
                ID = n.ID.ToString()
            }).Count();
            var NeutralFB = monitoringDB.Facebook_Post_Person.Where(a => a.PersonID.ToString().Equals(id) && a.Sentiment.ToString() == "Neutral").Select(n => new Models.SystemUserModels
            {
                ID = n.ID.ToString()
            }).Count();
            var PositiveWb = monitoringDB.Website_Post_Person.Where(a => a.PersonID.ToString().Equals(id) && a.Sentiment.ToString() == "Positive").Select(n => new Models.SystemUserModels
            {
                ID = n.ID.ToString()
            }).Count();
            var NegativeWb = monitoringDB.Website_Post_Person.Where(a => a.PersonID.ToString().Equals(id) && a.Sentiment.ToString() == "Negative").Select(n => new Models.SystemUserModels
            {
                ID = n.ID.ToString()
            }).Count();
            var NeutralWB = monitoringDB.Website_Post_Person.Where(a => a.PersonID.ToString().Equals(id) && a.Sentiment.ToString() == "Neutral").Select(n => new Models.SystemUserModels
            {
                ID = n.ID.ToString()
            }).Count();
            var PositiveTW = monitoringDB.Twitter_Tweet_Person.Where(a => a.PersonID.ToString().Equals(id) && a.Sentiment.ToString() == "Positive").Select(n => new Models.SystemUserModels
            {
                ID = n.ID.ToString()
            }).Count();
            var NegativeTW = monitoringDB.Twitter_Tweet_Person.Where(a => a.PersonID.ToString().Equals(id) && a.Sentiment.ToString() == "Negative").Select(n => new Models.SystemUserModels
            {
                ID = n.ID.ToString()
            }).Count();
            var NeutralTW = monitoringDB.Twitter_Tweet_Person.Where(a => a.PersonID.ToString().Equals(id) && a.Sentiment.ToString() == "Neutral").Select(n => new Models.SystemUserModels
            {
                ID = n.ID.ToString()
            }).Count();
            var ss = monitoringDB.Twitter_Tweet_Person.Where(a => a.PersonID.ToString().Equals(id) && a.Sentiment.ToString() == "Neutral").Select(n => new Models.SystemUserModels
            {
                ID = n.ID.ToString()
            }).Count();


            int[] arr = new int[8];
            var i = 0;
            while (i < 8)
            {
                DateTime testDate = DateTime.Now.AddMonths(-i);
                DateTime testDate2 = DateTime.Now.AddMonths(-(i + 1));
                arr[i] =
                     (from postPerson in monitoringDB.Facebook_Post_Person
                      join a in monitoringDB.Facebook_Posts on postPerson.PostID equals a.ID
                      where postPerson.PersonID.ToString().Equals(id)
                      where a.UpdateTime < testDate
                      where a.UpdateTime > testDate2
                      select new Models.FbPost
                      {
                          ID = a.ID,

                      }).Count();
                i++;
            }
            var fbPosts =
                (from a in monitoringDB.Facebook_Post_Person
                 join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
                 join c in monitoringDB.System_Person on a.PersonID equals c.ID
                 where (c.ID.ToString() == id) && a.IsDeleted != true
                 select new Models.FbPost
                 {
                     Message = b.Message,

                 }).ToList();
            string AllTextFb = string.Concat(fbPosts.Select(n => n.Message));
            var FbWords = Regex.Split(AllTextFb.ToLower(), @"\W+")
                .Where(s => s.Length > 3 && s != "байна" && s != "байгаа" && s != "байх" && s != "байсан" && s != "байдаг" && s != "shig" && s != "бгаа" && s != "чинь" && s != "юм" && s != "bgaa" && s != "болж" && s != "байж" && s != "болсон" && s != "гээд" && s != "гэсэн" && s != "минь" && s != "bgaa" && s != "shuu" && s != "https")
                .GroupBy(s => s)
                .Select(s => new Models.LargeTextModels
                {
                    word = s.Key,
                    count = s.Count()
                })
                .OrderByDescending(g => g.count).Take(10).ToList();

            var twPosts =
                (from tweetPerson in monitoringDB.Twitter_Tweet_Person
                 join tweet in monitoringDB.Twitter_Tweets on tweetPerson.TweetID equals tweet.ID
                 join sysPerson in monitoringDB.System_Person on tweetPerson.PersonID equals sysPerson.ID
                 join tweetUser in monitoringDB.Twitter_User_Details on tweet.TwitterUserID equals tweetUser.TwitterUserID
                 where sysPerson.ID.ToString() == id && tweetPerson.IsDeleted != true
                 orderby tweet.CreatedAt descending
                 select new Models.FbPost
                 {
                     Message = tweet.FullText,
                 }).ToList();
            string TwAllText = string.Concat(twPosts.Select(n => n.Message));
            var TwWords = Regex.Split(TwAllText.ToLower(), @"\W+")
                .Where(s => s.Length > 3 && s != "байна" && s != "байгаа" && s != "байх" && s != "байсан" && s != "байдаг" && s != "shig" && s != "бгаа" && s != "чинь" && s != "юм" && s != "bgaa" && s != "болж" && s != "байж" && s != "болсон" && s != "гээд" && s != "гэсэн" && s != "минь" && s != "bgaa" && s != "shuu" && s != "https")
                .GroupBy(s => s)
                .Select(s => new Models.LargeTextModels
                {
                    word = s.Key,
                    count = s.Count()
                })
                .OrderByDescending(g => g.count).Take(10).ToList();

            var wbPosts =
                (from a in monitoringDB.Website_Post_Person
                 join b in monitoringDB.WebSite_Posts on a.PostID equals b.ID
                 join c in monitoringDB.System_Person on a.PersonID equals c.ID
                 where c.ID.ToString() == id && a.IsDeleted != true
                 orderby b.Date descending
                 select new Models.FbPost
                 {
                     Message = b.Text,
                 }).ToList();
            string WbAllText = string.Concat(wbPosts.Select(n => n.Message));
            var WbWords = Regex.Split(WbAllText.ToLower(), @"\W+")
                .Where(s => s.Length > 3 && s != "байна" && s != "байгаа" && s != "байх" && s != "байсан" && s != "байдаг" && s != "shig" && s != "бгаа" && s != "чинь" && s != "юм" && s != "bgaa" && s != "болж" && s != "байж" && s != "болсон" && s != "гээд" && s != "гэсэн" && s != "минь" && s != "bgaa" && s != "shuu" && s != "https")
                .GroupBy(s => s)
                .Select(s => new Models.LargeTextModels
                {
                    word = s.Key,
                    count = s.Count()
                })
                .OrderByDescending(g => g.count).Take(10).ToList();

            ViewBag.Mounth = arr;
            ViewBag.FbWords = FbWords;
            ViewBag.TwWords = TwWords;
            ViewBag.WbWords = WbWords;
            ViewBag.NeutralTW = NeutralTW;
            ViewBag.NegativeTW = NegativeTW;
            ViewBag.PositiveTW = PositiveTW;
            ViewBag.NeutralWB = NeutralWB;
            ViewBag.NegativeWb = NegativeWb;
            ViewBag.PositiveWb = PositiveWb;
            ViewBag.NeutralFB = NeutralFB;
            ViewBag.NegativeFb = NegativeFb;
            ViewBag.PositiveFb = PositiveFb;
            ViewBag.socialAll = ViewBag.TwitterData + ViewBag.FbData;
            ViewBag.tvAll = ViewBag.WebSiteData;
            if (ViewBag.socialAll == 0)
            {
                ViewBag.PercentageFb = 0;
                ViewBag.PercentageTwitter = 0;
            }
            else
            {
                ViewBag.PercentageFb = ((ViewBag.FbData * 100) / ViewBag.socialAll);
                ViewBag.PercentageTwitter = ((ViewBag.TwitterData * 100) / ViewBag.socialAll);
            }

            //2022-05-11 begin
            // making date list
            DateTime last = new DateTime();
            last = DateTime.Now.AddDays(-30);
            DateTime real = DateTime.Now;
            List<DateTime> dates = new List<DateTime>();
            do
            {
                dates.Add(last);
                last = last.AddDays(+1);
            } while (real.Date != last.Date);



            // Facebook Chart ///
            string queryFbPos = string.Format("select count(*) as count,Convert(date,post.UpdateTime) as date from dbo.[Facebook.Posts] post inner join dbo.[Facebook.Post.Person] person on post.ID = person.PostID and (person.IsDeleted is null or person.IsDeleted<>1) where CONVERT(date, getdate()) >= Convert(date,post.UpdateTime) and Convert(date,post.UpdateTime) > DATEADD(DD, -30, CONVERT(date, getdate())) and person.PersonID = '{0}' and person.Sentiment = 'Positive' group by Convert(date, post.UpdateTime) order by Convert(date, post.UpdateTime) desc;", id);
            string queryFbNeg = string.Format("select count(*) as count,Convert(date,post.UpdateTime) as date from dbo.[Facebook.Posts] post inner join dbo.[Facebook.Post.Person] person on post.ID = person.PostID and (person.IsDeleted is null or person.IsDeleted<>1)  where CONVERT(date, getdate()) >= Convert(date,post.UpdateTime) and Convert(date,post.UpdateTime) > DATEADD(DD, -30, CONVERT(date, getdate())) and person.PersonID = '{0}' and person.Sentiment = 'Negative' group by Convert(date, post.UpdateTime) order by Convert(date, post.UpdateTime) desc;", id);
            string queryFbNeu = string.Format("select count(*) as count,Convert(date,post.UpdateTime) as date from dbo.[Facebook.Posts] post inner join dbo.[Facebook.Post.Person] person on post.ID = person.PostID and (person.IsDeleted is null or person.IsDeleted<>1)  where CONVERT(date, getdate()) >= Convert(date,post.UpdateTime) and Convert(date,post.UpdateTime) > DATEADD(DD, -30, CONVERT(date, getdate())) and person.PersonID = '{0}' and person.Sentiment = 'Neutral' group by Convert(date, post.UpdateTime) order by Convert(date, post.UpdateTime) desc;", id);

            var resultsFbPos = monitoringDB.Database.SqlQuery<graphResult>(queryFbPos).ToList<graphResult>();
            var resultsFbNeg = monitoringDB.Database.SqlQuery<graphResult>(queryFbNeg).ToList<graphResult>();
            var resultsFbNeu = monitoringDB.Database.SqlQuery<graphResult>(queryFbNeu).ToList<graphResult>();

            string counterfbPos = "[";
            string counterfbNeg = "[";
            string counterfbNeu = "[";

            #region FbPosloop
            for (int s = 0; s < dates.Count; s++)
            {
                DateTime date = dates.ElementAt(s).Date;
                //
                if (resultsFbPos.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterfbPos == "[")
                    {
                        counterfbPos = counterfbPos + resultsFbPos.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    counterfbPos = counterfbPos + "," + resultsFbPos.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                }
                else
                {
                    if (counterfbPos == "[")
                    {
                        counterfbPos = counterfbPos + 0;

                    }
                    counterfbPos = counterfbPos + "," + 0;
                }
            }
            #endregion           
            #region FbNegloop
            for (int s = 0; s < dates.Count; s++)
            {
                DateTime date = dates.ElementAt(s).Date;
                //
                if (resultsFbNeg.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterfbNeg == "[")
                    {
                        counterfbNeg = counterfbNeg + resultsFbNeg.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    counterfbNeg = counterfbNeg + "," + resultsFbNeg.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                }
                else
                {
                    if (counterfbNeg == "[")
                    {
                        counterfbNeg = counterfbNeg + 0;
                    }
                    counterfbNeg = counterfbNeg + "," + 0;
                }
            }
            #endregion   
            #region FbNeuloop
            for (int s = 0; s < dates.Count; s++)
            {
                DateTime date = dates.ElementAt(s).Date;
                //
                if (resultsFbNeu.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterfbNeu == "[")
                    {
                        counterfbNeu = counterfbNeu + resultsFbNeu.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    counterfbNeu = counterfbNeu + "," + resultsFbNeu.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                }
                else
                {
                    if (counterfbNeu == "[")
                    {
                        counterfbNeu = counterfbNeu + 0;
                    }
                    counterfbNeu = counterfbNeu + "," + 0;
                }
            }
            #endregion 

            counterfbPos = counterfbPos + "]";
            counterfbNeg = counterfbNeg + "]";
            counterfbNeu = counterfbNeu + "]";

            ViewBag.FbPosChartCount = counterfbPos;
            ViewBag.FbNegChartCount = counterfbNeg;
            ViewBag.FbNeuChartCount = counterfbNeu;
            // End Facebook Chart //

            // Facebook Chart ///
            string queryTwPos = string.Format("select count(*) as count,Convert(date,tweet.CreatedAt) as date from dbo.[Twitter.Tweets] tweet inner join dbo.[Twitter.Tweet.Person] person on tweet.ID = person.TweetID  and (person.IsDeleted is null or person.IsDeleted<>1)  where CONVERT(date, getdate()) >= Convert(date,tweet.CreatedAt) and Convert(date,tweet.CreatedAt) > DATEADD(DD, -30, CONVERT(date, getdate())) and person.PersonID = '{0}' and person.Sentiment = 'Positive' group by Convert(date, tweet.CreatedAt) order by Convert(date, tweet.CreatedAt) desc;", id);
            string queryTwNeg = string.Format("select count(*) as count,Convert(date,tweet.CreatedAt) as date from dbo.[Twitter.Tweets] tweet inner join dbo.[Twitter.Tweet.Person] person on tweet.ID = person.TweetID  and (person.IsDeleted is null or person.IsDeleted<>1)  where CONVERT(date, getdate()) >= Convert(date,tweet.CreatedAt) and Convert(date,tweet.CreatedAt) > DATEADD(DD, -30, CONVERT(date, getdate())) and person.PersonID = '{0}' and person.Sentiment = 'Negative' group by Convert(date, tweet.CreatedAt) order by Convert(date, tweet.CreatedAt) desc;", id);
            string queryTwNeu = string.Format("select count(*) as count,Convert(date,tweet.CreatedAt) as date from dbo.[Twitter.Tweets] tweet inner join dbo.[Twitter.Tweet.Person] person on tweet.ID = person.TweetID  and (person.IsDeleted is null or person.IsDeleted<>1)  where CONVERT(date, getdate()) >= Convert(date,tweet.CreatedAt) and Convert(date,tweet.CreatedAt) > DATEADD(DD, -30, CONVERT(date, getdate())) and person.PersonID = '{0}' and person.Sentiment = 'Neutral' group by Convert(date, tweet.CreatedAt) order by Convert(date, tweet.CreatedAt) desc;", id);

            var resultsTwPos = monitoringDB.Database.SqlQuery<graphResult>(queryTwPos).ToList<graphResult>();
            var resultsTwNeg = monitoringDB.Database.SqlQuery<graphResult>(queryTwNeg).ToList<graphResult>();
            var resultsTwNeu = monitoringDB.Database.SqlQuery<graphResult>(queryTwNeu).ToList<graphResult>();

            string counterTwPos = "[";
            string counterTwNeg = "[";
            string counterTwNeu = "[";

            #region TwPosloop
            for (int s = 0; s < dates.Count; s++)
            {
                DateTime date = dates.ElementAt(s).Date;
                //
                if (resultsTwPos.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterTwPos == "[")
                    {
                        counterTwPos = counterTwPos + resultsTwPos.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    counterTwPos = counterTwPos + "," + resultsTwPos.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                }
                else
                {
                    if (counterTwPos == "[")
                    {
                        counterTwPos = counterTwPos + 0;

                    }
                    counterTwPos = counterTwPos + "," + 0;
                }
            }
            #endregion           
            #region TwNegloop
            for (int s = 0; s < dates.Count; s++)
            {
                DateTime date = dates.ElementAt(s).Date;
                //
                if (resultsTwNeg.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterTwNeg == "[")
                    {
                        counterTwNeg = counterTwNeg + resultsTwNeg.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    counterTwNeg = counterTwNeg + "," + resultsTwNeg.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                }
                else
                {
                    if (counterTwNeg == "[")
                    {
                        counterTwNeg = counterTwNeg + 0;
                    }
                    counterTwNeg = counterTwNeg + "," + 0;
                }
            }
            #endregion   
            #region TwNeuloop
            for (int s = 0; s < dates.Count; s++)
            {
                DateTime date = dates.ElementAt(s).Date;
                //
                if (resultsTwNeu.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterTwNeu == "[")
                    {
                        counterTwNeu = counterTwNeu + resultsTwNeu.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    counterTwNeu = counterTwNeu + "," + resultsTwNeu.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                }
                else
                {
                    if (counterTwNeu == "[")
                    {
                        counterTwNeu = counterTwNeu + 0;
                    }
                    counterTwNeu = counterTwNeu + "," + 0;
                }
            }
            #endregion 

            counterTwPos = counterTwPos + "]";
            counterTwNeg = counterTwNeg + "]";
            counterTwNeu = counterTwNeu + "]";

            ViewBag.TwPosChartCount = counterTwPos;
            ViewBag.TwNegChartCount = counterTwNeg;
            ViewBag.TwNeuChartCount = counterTwNeu;
            // End Twitter Chart //

            // Facebook Chart ///
            string queryWebPos = string.Format("select count(*) as count,Convert(date,post.Date) as date from dbo.[WebSite.Posts] post inner join dbo.[Website.Post.Person] person on post.ID = person.PostID  and (person.IsDeleted is null or person.IsDeleted<>1)  where CONVERT(date, getdate()) >= Convert(date,post.Date) and Convert(date,post.Date) > DATEADD(DD, -30, CONVERT(date, getdate())) and person.PersonID = '{0}' and person.Sentiment = 'Positive' group by Convert(date, post.Date) order by Convert(date, post.Date) desc;", id);
            string queryWebNeg = string.Format("select count(*) as count,Convert(date,post.Date) as date from dbo.[WebSite.Posts] post inner join dbo.[Website.Post.Person] person on post.ID = person.PostID  and (person.IsDeleted is null or person.IsDeleted<>1)  where CONVERT(date, getdate()) >= Convert(date,post.Date) and Convert(date,post.Date) > DATEADD(DD, -30, CONVERT(date, getdate())) and person.PersonID = '{0}' and person.Sentiment = 'Negative' group by Convert(date, post.Date) order by Convert(date, post.Date) desc;", id);
            string queryWebNeu = string.Format("select count(*) as count,Convert(date,post.Date) as date from dbo.[WebSite.Posts] post inner join dbo.[Website.Post.Person] person on post.ID = person.PostID  and (person.IsDeleted is null or person.IsDeleted<>1) where CONVERT(date, getdate()) >= Convert(date,post.Date) and Convert(date,post.Date) > DATEADD(DD, -30, CONVERT(date, getdate())) and person.PersonID = '{0}' and person.Sentiment = 'Neutral' group by Convert(date, post.Date) order by Convert(date, post.Date) desc;", id);

            var resultsWebPos = monitoringDB.Database.SqlQuery<graphResult>(queryWebPos).ToList<graphResult>();
            var resultsWebNeg = monitoringDB.Database.SqlQuery<graphResult>(queryWebNeg).ToList<graphResult>();
            var resultsWebNeu = monitoringDB.Database.SqlQuery<graphResult>(queryWebNeu).ToList<graphResult>();

            string counterWebPos = "[";
            string counterWebNeg = "[";
            string counterWebNeu = "[";

            #region WebPosloop
            for (int s = 0; s < dates.Count; s++)
            {
                DateTime date = dates.ElementAt(s).Date;
                //
                if (resultsWebPos.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterWebPos == "[")
                    {
                        counterWebPos = counterWebPos + resultsWebPos.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    counterWebPos = counterWebPos + "," + resultsWebPos.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                }
                else
                {
                    if (counterWebPos == "[")
                    {
                        counterWebPos = counterWebPos + 0;

                    }
                    counterWebPos = counterWebPos + "," + 0;
                }
            }
            #endregion           
            #region WebNegloop
            for (int s = 0; s < dates.Count; s++)
            {
                DateTime date = dates.ElementAt(s).Date;
                //
                if (resultsWebNeu.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterWebNeg == "[")
                    {
                        counterWebNeg = counterWebNeg + resultsWebNeu.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    counterWebNeg = counterWebNeg + "," + resultsWebNeu.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                }
                else
                {
                    if (counterWebNeg == "[")
                    {
                        counterWebNeg = counterWebNeg + 0;
                    }
                    counterWebNeg = counterWebNeg + "," + 0;
                }
            }
            #endregion   
            #region WebNeuloop
            for (int s = 0; s < dates.Count; s++)
            {
                DateTime date = dates.ElementAt(s).Date;
                //
                if (resultsWebNeu.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterWebNeu == "[")
                    {
                        counterWebNeu = counterWebNeu + resultsWebNeu.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    counterWebNeu = counterWebNeu + "," + resultsWebNeu.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                }
                else
                {
                    if (counterWebNeu == "[")
                    {
                        counterWebNeu = counterWebNeu + 0;
                    }
                    counterWebNeu = counterWebNeu + "," + 0;
                }
            }
            #endregion 

            counterWebPos = counterWebPos + "]";
            counterWebNeg = counterWebNeg + "]";
            counterWebNeu = counterWebNeu + "]";

            ViewBag.WebPosChartCount = counterWebPos;
            ViewBag.WebNegChartCount = counterWebNeg;
            ViewBag.WebNeuChartCount = counterWebNeu;
            //2022-05-11 bulgaa end


            return View(currentUser);
        }



        public ActionResult AnalyzeChoose()
        {
            if (User.Identity.IsAuthenticated == false)
            {
                return Redirect("/Account/Login");
            }
            MonitoringEntities monitoringDB = new MonitoringEntities();

            var userIdentity = (ClaimsIdentity)User.Identity;
            var claims = userIdentity.Claims;
            var roles = claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).FirstOrDefault();
            var userList =
                 (from a in monitoringDB.Person_User
                  where a.userID == roles
                  select new Models.Person
                  {
                      AspNetUserID = a.personID,
                      Name = a.name
                  }).ToList();
            ViewBag.userList = userList;
            return View();
        }

        public ActionResult AnalysisToday(string ObjectId)
        {
            if (User.Identity.IsAuthenticated == false)
            {
                return Redirect("/Account/Login");
            }
            MonitoringEntities monitoringDB = new MonitoringEntities();
            /* var id = "e7aca6b9-5973-49dd-9c9d-1e93222b759a";*/
            var id = ObjectId;
            DateTime LastOneDay = DateTime.Now.AddDays(-1);
            ViewBag.FbData =
              (from a in monitoringDB.Facebook_Post_Person
               join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
               join c in monitoringDB.System_Person on a.PersonID equals c.ID
               where c.ID.ToString() == id && a.IsDeleted != true && b.UpdateTime >= LastOneDay
               orderby b.UpdateTime descending
               select new Models.FbPost
               {
                   ID = b.ID,
               }).Count();

            ViewBag.TwitterData =
               (from tweetPerson in monitoringDB.Twitter_Tweet_Person
                join tweet in monitoringDB.Twitter_Tweets on tweetPerson.TweetID equals tweet.ID
                join sysPerson in monitoringDB.System_Person on tweetPerson.PersonID equals sysPerson.ID
                join tweetUser in monitoringDB.Twitter_User_Details on tweet.TwitterUserID equals tweetUser.TwitterUserID
                where sysPerson.ID.ToString() == id && tweetPerson.IsDeleted != true && tweet.CreatedAt >= LastOneDay
                orderby tweet.CreatedAt descending
                select new Models.Twitter
                {
                    ID = tweet.ID,
                }).Count();

            ViewBag.WebSiteData =
              (from a in monitoringDB.Website_Post_Person
               join b in monitoringDB.WebSite_Posts on a.PostID equals b.ID
               join c in monitoringDB.System_Person on a.PersonID equals c.ID
               where c.ID.ToString() == id && a.IsDeleted != true && b.DateTime >= LastOneDay
               orderby b.Date descending
               select new Models.WebSitePost
               {
                   ID = b.ID,
               }).Count();
            var currentUser = monitoringDB.System_Person.Where(a => a.ID.ToString().Equals(id)).Select(n => new Models.SystemUserModels
            {
                ID = n.ID.ToString(),
                Name = n.Name,
                Surename = n.Surename,
                Tittlename = n.Tittlename,
                Picture = n.Picture,
                Description = n.Description,
                FacebookAccount = n.FacebookAccount.ToString(),
                TwitterAccount = n.TwitterAccount.ToString()
            }).FirstOrDefault();


            //Sentiment Web
            var PositiveWb =
             (from a in monitoringDB.Website_Post_Person
              join b in monitoringDB.WebSite_Posts on a.PostID equals b.ID
              join c in monitoringDB.System_Person on a.PersonID equals c.ID
              where c.ID.ToString() == id && a.IsDeleted != true && b.DateTime >= LastOneDay && a.Sentiment.ToString() == "Positive"

              select new Models.SystemUserModels
              {
                  ID = b.ID.ToString(),
              }).Count();

            var NegativeWb =
                (from a in monitoringDB.Website_Post_Person
                 join b in monitoringDB.WebSite_Posts on a.PostID equals b.ID
                 join c in monitoringDB.System_Person on a.PersonID equals c.ID
                 where c.ID.ToString() == id && a.IsDeleted != true && b.DateTime >= LastOneDay && a.Sentiment.ToString() == "Negative"

                 select new Models.SystemUserModels
                 {
                     ID = b.ID.ToString(),
                 }).Count();
            var NeutralWB =
               (from a in monitoringDB.Website_Post_Person
                join b in monitoringDB.WebSite_Posts on a.PostID equals b.ID
                join c in monitoringDB.System_Person on a.PersonID equals c.ID
                where c.ID.ToString() == id && a.IsDeleted != true && b.DateTime >= LastOneDay && a.Sentiment.ToString() == "Neutral"

                select new Models.SystemUserModels
                {
                    ID = b.ID.ToString(),
                }).Count();
            //Facebook posts
            var PositiveFb =
              (from a in monitoringDB.Facebook_Post_Person
               join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
               join c in monitoringDB.System_Person on a.PersonID equals c.ID
               where c.ID.ToString() == id && a.IsDeleted != true && b.UpdateTime >= LastOneDay && a.Sentiment.ToString() == "Positive"

               select new Models.SystemUserModels
               {
                   ID = a.ID.ToString(),
               }).Count();

            var NegativeFb =
               (from a in monitoringDB.Facebook_Post_Person
                join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
                join c in monitoringDB.System_Person on a.PersonID equals c.ID
                where c.ID.ToString() == id && a.IsDeleted != true && b.UpdateTime >= LastOneDay && a.Sentiment.ToString() == "Negative"

                select new Models.SystemUserModels
                {
                    ID = a.ID.ToString(),
                }).Count();
            var NeutralFB =
              (from a in monitoringDB.Facebook_Post_Person
               join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
               join c in monitoringDB.System_Person on a.PersonID equals c.ID
               where c.ID.ToString() == id && a.IsDeleted != true && b.UpdateTime >= LastOneDay && a.Sentiment.ToString() == "Neutral"

               select new Models.SystemUserModels
               {
                   ID = a.ID.ToString(),
               }).Count();
            //Twitter posts
            var PositiveTW =
              (from tweetPerson in monitoringDB.Twitter_Tweet_Person
               join tweet in monitoringDB.Twitter_Tweets on tweetPerson.TweetID equals tweet.ID
               join sysPerson in monitoringDB.System_Person on tweetPerson.PersonID equals sysPerson.ID
               join tweetUser in monitoringDB.Twitter_User_Details on tweet.TwitterUserID equals tweetUser.TwitterUserID
               where sysPerson.ID.ToString() == id && tweetPerson.IsDeleted != true && tweet.CreatedAt >= LastOneDay && tweetPerson.Sentiment.ToString() == "Positive"
               select new Models.SystemUserModels
               {
                   ID = tweetPerson.ID.ToString(),
               }).Count();
            var NegativeTW =
               (from tweetPerson in monitoringDB.Twitter_Tweet_Person
                join tweet in monitoringDB.Twitter_Tweets on tweetPerson.TweetID equals tweet.ID
                join sysPerson in monitoringDB.System_Person on tweetPerson.PersonID equals sysPerson.ID
                join tweetUser in monitoringDB.Twitter_User_Details on tweet.TwitterUserID equals tweetUser.TwitterUserID
                where sysPerson.ID.ToString() == id && tweetPerson.IsDeleted != true && tweet.CreatedAt >= LastOneDay && tweetPerson.Sentiment.ToString() == "Negative"
                select new Models.SystemUserModels
                {
                    ID = tweetPerson.ID.ToString(),
                }).Count();
            var NeutralTW =
             (from tweetPerson in monitoringDB.Twitter_Tweet_Person
              join tweet in monitoringDB.Twitter_Tweets on tweetPerson.TweetID equals tweet.ID
              join sysPerson in monitoringDB.System_Person on tweetPerson.PersonID equals sysPerson.ID
              join tweetUser in monitoringDB.Twitter_User_Details on tweet.TwitterUserID equals tweetUser.TwitterUserID
              where sysPerson.ID.ToString() == id && tweetPerson.IsDeleted != true && tweet.CreatedAt >= LastOneDay && tweetPerson.Sentiment.ToString() == "Neutral"
              select new Models.SystemUserModels
              {
                  ID = tweetPerson.ID.ToString(),
              }).Count();
            var ss = monitoringDB.Twitter_Tweet_Person.Where(a => a.PersonID.ToString().Equals(id) && a.Sentiment.ToString() == "Neutral").Select(n => new Models.SystemUserModels
            {
                ID = n.ID.ToString()
            }).Count();


            int[] arr = new int[8];
            var i = 0;
            while (i < 8)
            {
                DateTime testDate = DateTime.Now.AddMonths(-i);
                DateTime testDate2 = DateTime.Now.AddMonths(-(i + 1));
                arr[i] =
                     (from postPerson in monitoringDB.Facebook_Post_Person
                      join a in monitoringDB.Facebook_Posts on postPerson.PostID equals a.ID
                      where postPerson.PersonID.ToString().Equals(id)
                      where a.UpdateTime < testDate
                      where a.UpdateTime > testDate2
                      select new Models.FbPost
                      {
                          ID = a.ID,

                      }).Count();
                i++;
            }
            var fbPosts =
                (from a in monitoringDB.Facebook_Post_Person
                 join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
                 join c in monitoringDB.System_Person on a.PersonID equals c.ID
                 where (c.ID.ToString() == id) && a.IsDeleted != true && b.UpdateTime >= LastOneDay
                 select new Models.FbPost
                 {
                     Message = b.Message,

                 }).ToList();
            string AllTextFb = string.Concat(fbPosts.Select(n => n.Message));
            var FbWords = Regex.Split(AllTextFb.ToLower(), @"\W+")
                .Where(s => s.Length > 3 && s != "байна" && s != "байгаа" && s != "байх" && s != "байсан" && s != "байдаг" && s != "shig" && s != "бгаа" && s != "чинь" && s != "юм" && s != "bgaa" && s != "болж" && s != "байж" && s != "болсон" && s != "гээд" && s != "гэсэн" && s != "минь" && s != "bgaa" && s != "shuu")
                .GroupBy(s => s)
                .Select(s => new Models.LargeTextModels
                {
                    word = s.Key,
                    count = s.Count()
                })
                .OrderByDescending(g => g.count).Take(10).ToList();

            var twPosts =
                (from a in monitoringDB.Facebook_Post_Person
                 join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
                 join c in monitoringDB.System_Person on a.PersonID equals c.ID
                 where (c.ID.ToString() == id) && a.IsDeleted != true && b.UpdateTime >= LastOneDay
                 select new Models.FbPost
                 {
                     Message = b.Message,

                 }).ToList();
            string TwAllText = string.Concat(twPosts.Select(n => n.Message));
            var TwWords = Regex.Split(TwAllText.ToLower(), @"\W+")
                .Where(s => s.Length > 3 && s != "байна" && s != "байгаа" && s != "байх" && s != "байсан" && s != "байдаг" && s != "shig" && s != "бгаа" && s != "чинь" && s != "юм" && s != "bgaa" && s != "болж" && s != "байж" && s != "болсон" && s != "гээд" && s != "гэсэн" && s != "минь" && s != "bgaa" && s != "shuu" && s != "https")
                .GroupBy(s => s)
                .Select(s => new Models.LargeTextModels
                {
                    word = s.Key,
                    count = s.Count()
                })
                .OrderByDescending(g => g.count).Take(10).ToList();
            var wbPosts =
                (from a in monitoringDB.Facebook_Post_Person
                 join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
                 join c in monitoringDB.System_Person on a.PersonID equals c.ID
                 where (c.ID.ToString() == id) && a.IsDeleted != true && b.UpdateTime >= LastOneDay
                 select new Models.FbPost
                 {
                     Message = b.Message,

                 }).ToList();
            string WbAllText = string.Concat(wbPosts.Select(n => n.Message));
            var WbWords = Regex.Split(WbAllText.ToLower(), @"\W+")
                .Where(s => s.Length > 3 && s != "байна" && s != "байгаа" && s != "байх" && s != "байсан" && s != "байдаг" && s != "shig" && s != "бгаа" && s != "чинь" && s != "юм" && s != "bgaa" && s != "болж" && s != "байж" && s != "болсон" && s != "гээд" && s != "гэсэн" && s != "минь" && s != "bgaa" && s != "shuu" && s != "https")
                .GroupBy(s => s)
                .Select(s => new Models.LargeTextModels
                {
                    word = s.Key,
                    count = s.Count()
                })
                .OrderByDescending(g => g.count).Take(10).ToList();
            ViewBag.Mounth = arr;
            ViewBag.FbWords = FbWords;
            ViewBag.TwWords = TwWords;
            ViewBag.WbWords = WbWords;
            ViewBag.NeutralTW = NeutralTW;
            ViewBag.NegativeTW = NegativeTW;
            ViewBag.PositiveTW = PositiveTW;
            ViewBag.NeutralWB = NeutralWB;
            ViewBag.NegativeWb = NegativeWb;
            ViewBag.PositiveWb = PositiveWb;
            ViewBag.NeutralFB = NeutralFB;
            ViewBag.NegativeFb = NegativeFb;
            ViewBag.PositiveFb = PositiveFb;
            ViewBag.socialAll = ViewBag.TwitterData + ViewBag.FbData;
            ViewBag.tvAll = ViewBag.WebSiteData;
            if (ViewBag.socialAll == 0)
            {
                ViewBag.PercentageFb = 0;
                ViewBag.PercentageTwitter = 0;
            }
            else
            {
                ViewBag.PercentageFb = ((ViewBag.FbData * 100) / ViewBag.socialAll);
                ViewBag.PercentageTwitter = ((ViewBag.TwitterData * 100) / ViewBag.socialAll);
            }
            /*          ViewBag.PercentageFb = ((ViewBag.FbData * 100) / ViewBag.socialAll);
                      ViewBag.PercentageTwitter = ((ViewBag.TwitterData * 100) / ViewBag.socialAll);*/

            //2022-05-11 begin
            // making date list
            DateTime last = new DateTime();
            last = DateTime.Now.AddDays(-30);
            DateTime real = DateTime.Now;
            List<DateTime> dates = new List<DateTime>();
            do
            {
                dates.Add(last);
                last = last.AddDays(+1);
            } while (real.Date != last.Date);



            // Facebook Chart ///
            string queryFbPos = string.Format("select count(*) as count,Convert(date,post.UpdateTime) as date from dbo.[Facebook.Posts] post inner join dbo.[Facebook.Post.Person] person on post.ID = person.PostID and (person.IsDeleted is null or person.IsDeleted<>1) where CONVERT(date, getdate()) >= Convert(date,post.UpdateTime) and Convert(date,post.UpdateTime) > DATEADD(DD, -30, CONVERT(date, getdate())) and person.PersonID = '{0}' and person.Sentiment = 'Positive' group by Convert(date, post.UpdateTime) order by Convert(date, post.UpdateTime) desc;", id);
            string queryFbNeg = string.Format("select count(*) as count,Convert(date,post.UpdateTime) as date from dbo.[Facebook.Posts] post inner join dbo.[Facebook.Post.Person] person on post.ID = person.PostID and (person.IsDeleted is null or person.IsDeleted<>1)  where CONVERT(date, getdate()) >= Convert(date,post.UpdateTime) and Convert(date,post.UpdateTime) > DATEADD(DD, -30, CONVERT(date, getdate())) and person.PersonID = '{0}' and person.Sentiment = 'Negative' group by Convert(date, post.UpdateTime) order by Convert(date, post.UpdateTime) desc;", id);
            string queryFbNeu = string.Format("select count(*) as count,Convert(date,post.UpdateTime) as date from dbo.[Facebook.Posts] post inner join dbo.[Facebook.Post.Person] person on post.ID = person.PostID and (person.IsDeleted is null or person.IsDeleted<>1)  where CONVERT(date, getdate()) >= Convert(date,post.UpdateTime) and Convert(date,post.UpdateTime) > DATEADD(DD, -30, CONVERT(date, getdate())) and person.PersonID = '{0}' and person.Sentiment = 'Neutral' group by Convert(date, post.UpdateTime) order by Convert(date, post.UpdateTime) desc;", id);

            var resultsFbPos = monitoringDB.Database.SqlQuery<graphResult>(queryFbPos).ToList<graphResult>();
            var resultsFbNeg = monitoringDB.Database.SqlQuery<graphResult>(queryFbNeg).ToList<graphResult>();
            var resultsFbNeu = monitoringDB.Database.SqlQuery<graphResult>(queryFbNeu).ToList<graphResult>();

            string counterfbPos = "[";
            string counterfbNeg = "[";
            string counterfbNeu = "[";

            #region FbPosloop
            for (int s = 0; s < dates.Count; s++)
            {
                DateTime date = dates.ElementAt(s).Date;
                //
                if (resultsFbPos.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterfbPos == "[")
                    {
                        counterfbPos = counterfbPos + resultsFbPos.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    counterfbPos = counterfbPos + "," + resultsFbPos.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                }
                else
                {
                    if (counterfbPos == "[")
                    {
                        counterfbPos = counterfbPos + 0;

                    }
                    counterfbPos = counterfbPos + "," + 0;
                }
            }
            #endregion           
            #region FbNegloop
            for (int s = 0; s < dates.Count; s++)
            {
                DateTime date = dates.ElementAt(s).Date;
                //
                if (resultsFbNeg.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterfbNeg == "[")
                    {
                        counterfbNeg = counterfbNeg + resultsFbNeg.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    counterfbNeg = counterfbNeg + "," + resultsFbNeg.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                }
                else
                {
                    if (counterfbNeg == "[")
                    {
                        counterfbNeg = counterfbNeg + 0;
                    }
                    counterfbNeg = counterfbNeg + "," + 0;
                }
            }
            #endregion   
            #region FbNeuloop
            for (int s = 0; s < dates.Count; s++)
            {
                DateTime date = dates.ElementAt(s).Date;
                //
                if (resultsFbNeu.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterfbNeu == "[")
                    {
                        counterfbNeu = counterfbNeu + resultsFbNeu.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    counterfbNeu = counterfbNeu + "," + resultsFbNeu.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                }
                else
                {
                    if (counterfbNeu == "[")
                    {
                        counterfbNeu = counterfbNeu + 0;
                    }
                    counterfbNeu = counterfbNeu + "," + 0;
                }
            }
            #endregion 

            counterfbPos = counterfbPos + "]";
            counterfbNeg = counterfbNeg + "]";
            counterfbNeu = counterfbNeu + "]";

            ViewBag.FbPosChartCount = counterfbPos;
            ViewBag.FbNegChartCount = counterfbNeg;
            ViewBag.FbNeuChartCount = counterfbNeu;
            // End Facebook Chart //

            // Facebook Chart ///
            string queryTwPos = string.Format("select count(*) as count,Convert(date,tweet.CreatedAt) as date from dbo.[Twitter.Tweets] tweet inner join dbo.[Twitter.Tweet.Person] person on tweet.ID = person.TweetID  and (person.IsDeleted is null or person.IsDeleted<>1)  where CONVERT(date, getdate()) >= Convert(date,tweet.CreatedAt) and Convert(date,tweet.CreatedAt) > DATEADD(DD, -30, CONVERT(date, getdate())) and person.PersonID = '{0}' and person.Sentiment = 'Positive' group by Convert(date, tweet.CreatedAt) order by Convert(date, tweet.CreatedAt) desc;", id);
            string queryTwNeg = string.Format("select count(*) as count,Convert(date,tweet.CreatedAt) as date from dbo.[Twitter.Tweets] tweet inner join dbo.[Twitter.Tweet.Person] person on tweet.ID = person.TweetID  and (person.IsDeleted is null or person.IsDeleted<>1)  where CONVERT(date, getdate()) >= Convert(date,tweet.CreatedAt) and Convert(date,tweet.CreatedAt) > DATEADD(DD, -30, CONVERT(date, getdate())) and person.PersonID = '{0}' and person.Sentiment = 'Negative' group by Convert(date, tweet.CreatedAt) order by Convert(date, tweet.CreatedAt) desc;", id);
            string queryTwNeu = string.Format("select count(*) as count,Convert(date,tweet.CreatedAt) as date from dbo.[Twitter.Tweets] tweet inner join dbo.[Twitter.Tweet.Person] person on tweet.ID = person.TweetID  and (person.IsDeleted is null or person.IsDeleted<>1)  where CONVERT(date, getdate()) >= Convert(date,tweet.CreatedAt) and Convert(date,tweet.CreatedAt) > DATEADD(DD, -30, CONVERT(date, getdate())) and person.PersonID = '{0}' and person.Sentiment = 'Neutral' group by Convert(date, tweet.CreatedAt) order by Convert(date, tweet.CreatedAt) desc;", id);

            var resultsTwPos = monitoringDB.Database.SqlQuery<graphResult>(queryTwPos).ToList<graphResult>();
            var resultsTwNeg = monitoringDB.Database.SqlQuery<graphResult>(queryTwNeg).ToList<graphResult>();
            var resultsTwNeu = monitoringDB.Database.SqlQuery<graphResult>(queryTwNeu).ToList<graphResult>();

            string counterTwPos = "[";
            string counterTwNeg = "[";
            string counterTwNeu = "[";

            #region TwPosloop
            for (int s = 0; s < dates.Count; s++)
            {
                DateTime date = dates.ElementAt(s).Date;
                //
                if (resultsTwPos.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterTwPos == "[")
                    {
                        counterTwPos = counterTwPos + resultsTwPos.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    counterTwPos = counterTwPos + "," + resultsTwPos.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                }
                else
                {
                    if (counterTwPos == "[")
                    {
                        counterTwPos = counterTwPos + 0;

                    }
                    counterTwPos = counterTwPos + "," + 0;
                }
            }
            #endregion           
            #region TwNegloop
            for (int s = 0; s < dates.Count; s++)
            {
                DateTime date = dates.ElementAt(s).Date;
                //
                if (resultsTwNeg.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterTwNeg == "[")
                    {
                        counterTwNeg = counterTwNeg + resultsTwNeg.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    counterTwNeg = counterTwNeg + "," + resultsTwNeg.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                }
                else
                {
                    if (counterTwNeg == "[")
                    {
                        counterTwNeg = counterTwNeg + 0;
                    }
                    counterTwNeg = counterTwNeg + "," + 0;
                }
            }
            #endregion   
            #region TwNeuloop
            for (int s = 0; s < dates.Count; s++)
            {
                DateTime date = dates.ElementAt(s).Date;
                //
                if (resultsTwNeu.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterTwNeu == "[")
                    {
                        counterTwNeu = counterTwNeu + resultsTwNeu.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    counterTwNeu = counterTwNeu + "," + resultsTwNeu.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                }
                else
                {
                    if (counterTwNeu == "[")
                    {
                        counterTwNeu = counterTwNeu + 0;
                    }
                    counterTwNeu = counterTwNeu + "," + 0;
                }
            }
            #endregion 

            counterTwPos = counterTwPos + "]";
            counterTwNeg = counterTwNeg + "]";
            counterTwNeu = counterTwNeu + "]";

            ViewBag.TwPosChartCount = counterTwPos;
            ViewBag.TwNegChartCount = counterTwNeg;
            ViewBag.TwNeuChartCount = counterTwNeu;
            // End Twitter Chart //

            // Facebook Chart ///
            string queryWebPos = string.Format("select count(*) as count,Convert(date,post.Date) as date from dbo.[WebSite.Posts] post inner join dbo.[Website.Post.Person] person on post.ID = person.PostID  and (person.IsDeleted is null or person.IsDeleted<>1)  where CONVERT(date, getdate()) >= Convert(date,post.Date) and Convert(date,post.Date) > DATEADD(DD, -30, CONVERT(date, getdate())) and person.PersonID = '{0}' and person.Sentiment = 'Positive' group by Convert(date, post.Date) order by Convert(date, post.Date) desc;", id);
            string queryWebNeg = string.Format("select count(*) as count,Convert(date,post.Date) as date from dbo.[WebSite.Posts] post inner join dbo.[Website.Post.Person] person on post.ID = person.PostID  and (person.IsDeleted is null or person.IsDeleted<>1)  where CONVERT(date, getdate()) >= Convert(date,post.Date) and Convert(date,post.Date) > DATEADD(DD, -30, CONVERT(date, getdate())) and person.PersonID = '{0}' and person.Sentiment = 'Negative' group by Convert(date, post.Date) order by Convert(date, post.Date) desc;", id);
            string queryWebNeu = string.Format("select count(*) as count,Convert(date,post.Date) as date from dbo.[WebSite.Posts] post inner join dbo.[Website.Post.Person] person on post.ID = person.PostID  and (person.IsDeleted is null or person.IsDeleted<>1) where CONVERT(date, getdate()) >= Convert(date,post.Date) and Convert(date,post.Date) > DATEADD(DD, -30, CONVERT(date, getdate())) and person.PersonID = '{0}' and person.Sentiment = 'Neutral' group by Convert(date, post.Date) order by Convert(date, post.Date) desc;", id);

            var resultsWebPos = monitoringDB.Database.SqlQuery<graphResult>(queryWebPos).ToList<graphResult>();
            var resultsWebNeg = monitoringDB.Database.SqlQuery<graphResult>(queryWebNeg).ToList<graphResult>();
            var resultsWebNeu = monitoringDB.Database.SqlQuery<graphResult>(queryWebNeu).ToList<graphResult>();

            string counterWebPos = "[";
            string counterWebNeg = "[";
            string counterWebNeu = "[";

            #region WebPosloop
            for (int s = 0; s < dates.Count; s++)
            {
                DateTime date = dates.ElementAt(s).Date;
                //
                if (resultsWebPos.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterWebPos == "[")
                    {
                        counterWebPos = counterWebPos + resultsWebPos.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    counterWebPos = counterWebPos + "," + resultsWebPos.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                }
                else
                {
                    if (counterWebPos == "[")
                    {
                        counterWebPos = counterWebPos + 0;

                    }
                    counterWebPos = counterWebPos + "," + 0;
                }
            }
            #endregion           
            #region WebNegloop
            for (int s = 0; s < dates.Count; s++)
            {
                DateTime date = dates.ElementAt(s).Date;
                //
                if (resultsWebNeu.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterWebNeg == "[")
                    {
                        counterWebNeg = counterWebNeg + resultsWebNeu.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    counterWebNeg = counterWebNeg + "," + resultsWebNeu.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                }
                else
                {
                    if (counterWebNeg == "[")
                    {
                        counterWebNeg = counterWebNeg + 0;
                    }
                    counterWebNeg = counterWebNeg + "," + 0;
                }
            }
            #endregion   
            #region WebNeuloop
            for (int s = 0; s < dates.Count; s++)
            {
                DateTime date = dates.ElementAt(s).Date;
                //
                if (resultsWebNeu.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterWebNeu == "[")
                    {
                        counterWebNeu = counterWebNeu + resultsWebNeu.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    counterWebNeu = counterWebNeu + "," + resultsWebNeu.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                }
                else
                {
                    if (counterWebNeu == "[")
                    {
                        counterWebNeu = counterWebNeu + 0;
                    }
                    counterWebNeu = counterWebNeu + "," + 0;
                }
            }
            #endregion 

            counterWebPos = counterWebPos + "]";
            counterWebNeg = counterWebNeg + "]";
            counterWebNeu = counterWebNeu + "]";

            ViewBag.WebPosChartCount = counterWebPos;
            ViewBag.WebNegChartCount = counterWebNeg;
            ViewBag.WebNeuChartCount = counterWebNeu;
            //2022-05-11 bulgaa end


            return View(currentUser);
        }
        public ActionResult AnalysisOfLastThreeDays(string ObjectId)
        {
            if (User.Identity.IsAuthenticated == false)
            {
                return Redirect("/Account/Login");
            }
            MonitoringEntities monitoringDB = new MonitoringEntities();
            /* var id = "e7aca6b9-5973-49dd-9c9d-1e93222b759a";*/
            var id = "8902d568-4f77-4874-97b8-f7dd7bc6d655";
            DateTime LastOneDay = DateTime.Now.AddDays(-3);
            ViewBag.FbData =
              (from a in monitoringDB.Facebook_Post_Person
               join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
               join c in monitoringDB.System_Person on a.PersonID equals c.ID
               where c.ID.ToString() == id && a.IsDeleted != true && b.UpdateTime >= LastOneDay
               orderby b.UpdateTime descending
               select new Models.FbPost
               {
                   ID = b.ID,
               }).Count();

            ViewBag.TwitterData =
               (from tweetPerson in monitoringDB.Twitter_Tweet_Person
                join tweet in monitoringDB.Twitter_Tweets on tweetPerson.TweetID equals tweet.ID
                join sysPerson in monitoringDB.System_Person on tweetPerson.PersonID equals sysPerson.ID
                join tweetUser in monitoringDB.Twitter_User_Details on tweet.TwitterUserID equals tweetUser.TwitterUserID
                where sysPerson.ID.ToString() == id && tweetPerson.IsDeleted != true && tweet.CreatedAt >= LastOneDay
                orderby tweet.CreatedAt descending
                select new Models.Twitter
                {
                    ID = tweet.ID,
                }).Count();

            ViewBag.WebSiteData =
              (from a in monitoringDB.Website_Post_Person
               join b in monitoringDB.WebSite_Posts on a.PostID equals b.ID
               join c in monitoringDB.System_Person on a.PersonID equals c.ID
               where c.ID.ToString() == id && a.IsDeleted != true && b.DateTime >= LastOneDay
               orderby b.Date descending
               select new Models.WebSitePost
               {
                   ID = b.ID,
               }).Count();
            var currentUser = monitoringDB.System_Person.Where(a => a.ID.ToString().Equals(id)).Select(n => new Models.SystemUserModels
            {
                ID = n.ID.ToString(),
                Name = n.Name,
                Surename = n.Surename,
                Tittlename = n.Tittlename,
                Picture = n.Picture,
                Description = n.Description,
                FacebookAccount = n.FacebookAccount.ToString(),
                TwitterAccount = n.TwitterAccount.ToString()
            }).FirstOrDefault();


            //Sentiment Web
            var PositiveWb =
             (from a in monitoringDB.Website_Post_Person
              join b in monitoringDB.WebSite_Posts on a.PostID equals b.ID
              join c in monitoringDB.System_Person on a.PersonID equals c.ID
              where c.ID.ToString() == id && a.IsDeleted != true && b.DateTime >= LastOneDay && a.Sentiment.ToString() == "Positive"

              select new Models.SystemUserModels
              {
                  ID = b.ID.ToString(),
              }).Count();

            var NegativeWb =
                (from a in monitoringDB.Website_Post_Person
                 join b in monitoringDB.WebSite_Posts on a.PostID equals b.ID
                 join c in monitoringDB.System_Person on a.PersonID equals c.ID
                 where c.ID.ToString() == id && a.IsDeleted != true && b.DateTime >= LastOneDay && a.Sentiment.ToString() == "Negative"

                 select new Models.SystemUserModels
                 {
                     ID = b.ID.ToString(),
                 }).Count();
            var NeutralWB =
               (from a in monitoringDB.Website_Post_Person
                join b in monitoringDB.WebSite_Posts on a.PostID equals b.ID
                join c in monitoringDB.System_Person on a.PersonID equals c.ID
                where c.ID.ToString() == id && a.IsDeleted != true && b.DateTime >= LastOneDay && a.Sentiment.ToString() == "Neutral"

                select new Models.SystemUserModels
                {
                    ID = b.ID.ToString(),
                }).Count();
            //Facebook posts
            var PositiveFb =
              (from a in monitoringDB.Facebook_Post_Person
               join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
               join c in monitoringDB.System_Person on a.PersonID equals c.ID
               where c.ID.ToString() == id && a.IsDeleted != true && b.UpdateTime >= LastOneDay && a.Sentiment.ToString() == "Positive"

               select new Models.SystemUserModels
               {
                   ID = a.ID.ToString(),
               }).Count();

            var NegativeFb =
               (from a in monitoringDB.Facebook_Post_Person
                join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
                join c in monitoringDB.System_Person on a.PersonID equals c.ID
                where c.ID.ToString() == id && a.IsDeleted != true && b.UpdateTime >= LastOneDay && a.Sentiment.ToString() == "Negative"

                select new Models.SystemUserModels
                {
                    ID = a.ID.ToString(),
                }).Count();
            var NeutralFB =
              (from a in monitoringDB.Facebook_Post_Person
               join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
               join c in monitoringDB.System_Person on a.PersonID equals c.ID
               where c.ID.ToString() == id && a.IsDeleted != true && b.UpdateTime >= LastOneDay && a.Sentiment.ToString() == "Neutral"

               select new Models.SystemUserModels
               {
                   ID = a.ID.ToString(),
               }).Count();
            //Twitter posts
            var PositiveTW =
              (from tweetPerson in monitoringDB.Twitter_Tweet_Person
               join tweet in monitoringDB.Twitter_Tweets on tweetPerson.TweetID equals tweet.ID
               join sysPerson in monitoringDB.System_Person on tweetPerson.PersonID equals sysPerson.ID
               join tweetUser in monitoringDB.Twitter_User_Details on tweet.TwitterUserID equals tweetUser.TwitterUserID
               where sysPerson.ID.ToString() == id && tweetPerson.IsDeleted != true && tweet.CreatedAt >= LastOneDay && tweetPerson.Sentiment.ToString() == "Positive"
               select new Models.SystemUserModels
               {
                   ID = tweetPerson.ID.ToString(),
               }).Count();
            var NegativeTW =
               (from tweetPerson in monitoringDB.Twitter_Tweet_Person
                join tweet in monitoringDB.Twitter_Tweets on tweetPerson.TweetID equals tweet.ID
                join sysPerson in monitoringDB.System_Person on tweetPerson.PersonID equals sysPerson.ID
                join tweetUser in monitoringDB.Twitter_User_Details on tweet.TwitterUserID equals tweetUser.TwitterUserID
                where sysPerson.ID.ToString() == id && tweetPerson.IsDeleted != true && tweet.CreatedAt >= LastOneDay && tweetPerson.Sentiment.ToString() == "Negative"
                select new Models.SystemUserModels
                {
                    ID = tweetPerson.ID.ToString(),
                }).Count();
            var NeutralTW =
             (from tweetPerson in monitoringDB.Twitter_Tweet_Person
              join tweet in monitoringDB.Twitter_Tweets on tweetPerson.TweetID equals tweet.ID
              join sysPerson in monitoringDB.System_Person on tweetPerson.PersonID equals sysPerson.ID
              join tweetUser in monitoringDB.Twitter_User_Details on tweet.TwitterUserID equals tweetUser.TwitterUserID
              where sysPerson.ID.ToString() == id && tweetPerson.IsDeleted != true && tweet.CreatedAt >= LastOneDay && tweetPerson.Sentiment.ToString() == "Neutral"
              select new Models.SystemUserModels
              {
                  ID = tweetPerson.ID.ToString(),
              }).Count();
            var ss = monitoringDB.Twitter_Tweet_Person.Where(a => a.PersonID.ToString().Equals(id) && a.Sentiment.ToString() == "Neutral").Select(n => new Models.SystemUserModels
            {
                ID = n.ID.ToString()
            }).Count();


            int[] arr = new int[8];
            var i = 0;
            while (i < 8)
            {
                DateTime testDate = DateTime.Now.AddMonths(-i);
                DateTime testDate2 = DateTime.Now.AddMonths(-(i + 1));
                arr[i] =
                     (from postPerson in monitoringDB.Facebook_Post_Person
                      join a in monitoringDB.Facebook_Posts on postPerson.PostID equals a.ID
                      where postPerson.PersonID.ToString().Equals(id)
                      where a.UpdateTime < testDate
                      where a.UpdateTime > testDate2
                      select new Models.FbPost
                      {
                          ID = a.ID,

                      }).Count();
                i++;
            }
            var fbPosts =
                (from a in monitoringDB.Facebook_Post_Person
                 join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
                 join c in monitoringDB.System_Person on a.PersonID equals c.ID
                 where (c.ID.ToString() == id) && a.IsDeleted != true && b.UpdateTime >= LastOneDay
                 select new Models.FbPost
                 {
                     Message = b.Message,

                 }).ToList();
            string AllTextFb = string.Concat(fbPosts.Select(n => n.Message));
            var FbWords = Regex.Split(AllTextFb.ToLower(), @"\W+")
                .Where(s => s.Length > 3 && s != "байна" && s != "байгаа" && s != "байх" && s != "байсан" && s != "байдаг" && s != "shig" && s != "бгаа" && s != "чинь" && s != "юм" && s != "bgaa" && s != "болж" && s != "байж" && s != "болсон" && s != "гээд" && s != "гэсэн" && s != "минь" && s != "bgaa" && s != "shuu" && s != "https")
                .GroupBy(s => s)
                .Select(s => new Models.LargeTextModels
                {
                    word = s.Key,
                    count = s.Count()
                })
                .OrderByDescending(g => g.count).Take(10).ToList();

            var twPosts =
                (from a in monitoringDB.Facebook_Post_Person
                 join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
                 join c in monitoringDB.System_Person on a.PersonID equals c.ID
                 where (c.ID.ToString() == id) && a.IsDeleted != true && b.UpdateTime >= LastOneDay
                 select new Models.FbPost
                 {
                     Message = b.Message,

                 }).ToList();
            string TwAllText = string.Concat(twPosts.Select(n => n.Message));
            var TwWords = Regex.Split(TwAllText.ToLower(), @"\W+")
                .Where(s => s.Length > 3 && s != "байна" && s != "байгаа" && s != "байх" && s != "байсан" && s != "байдаг" && s != "shig" && s != "бгаа" && s != "чинь" && s != "юм" && s != "bgaa" && s != "болж" && s != "байж" && s != "болсон" && s != "гээд" && s != "гэсэн" && s != "минь" && s != "bgaa" && s != "shuu" && s != "https")
                .GroupBy(s => s)
                .Select(s => new Models.LargeTextModels
                {
                    word = s.Key,
                    count = s.Count()
                })
                .OrderByDescending(g => g.count).Take(10).ToList();
            var wbPosts =
                (from a in monitoringDB.Facebook_Post_Person
                 join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
                 join c in monitoringDB.System_Person on a.PersonID equals c.ID
                 where (c.ID.ToString() == id) && a.IsDeleted != true && b.UpdateTime >= LastOneDay
                 select new Models.FbPost
                 {
                     Message = b.Message,

                 }).ToList();
            string WbAllText = string.Concat(wbPosts.Select(n => n.Message));
            var WbWords = Regex.Split(WbAllText.ToLower(), @"\W+")
                .Where(s => s.Length > 3 && s != "байна" && s != "байгаа" && s != "байх" && s != "байсан" && s != "байдаг" && s != "shig" && s != "бгаа" && s != "чинь" && s != "юм" && s != "bgaa" && s != "болж" && s != "байж" && s != "болсон" && s != "гээд" && s != "гэсэн" && s != "минь" && s != "bgaa" && s != "shuu" && s != "https")
                .GroupBy(s => s)
                .Select(s => new Models.LargeTextModels
                {
                    word = s.Key,
                    count = s.Count()
                })
                .OrderByDescending(g => g.count).Take(10).ToList();
            ViewBag.Mounth = arr;
            ViewBag.FbWords = FbWords;
            ViewBag.TwWords = TwWords;
            ViewBag.WbWords = WbWords;
            ViewBag.NeutralTW = NeutralTW;
            ViewBag.NegativeTW = NegativeTW;
            ViewBag.PositiveTW = PositiveTW;
            ViewBag.NeutralWB = NeutralWB;
            ViewBag.NegativeWb = NegativeWb;
            ViewBag.PositiveWb = PositiveWb;
            ViewBag.NeutralFB = NeutralFB;
            ViewBag.NegativeFb = NegativeFb;
            ViewBag.PositiveFb = PositiveFb;
            ViewBag.socialAll = ViewBag.TwitterData + ViewBag.FbData;
            ViewBag.tvAll = ViewBag.WebSiteData;
            ViewBag.PercentageFb = ((ViewBag.FbData * 100) / ViewBag.socialAll);
            ViewBag.PercentageTwitter = ((ViewBag.TwitterData * 100) / ViewBag.socialAll);

            //2022-05-11 begin
            // making date list
            DateTime last = new DateTime();
            last = DateTime.Now.AddDays(-30);
            DateTime real = DateTime.Now;
            List<DateTime> dates = new List<DateTime>();
            do
            {
                dates.Add(last);
                last = last.AddDays(+1);
            } while (real.Date != last.Date);



            // Facebook Chart ///
            string queryFbPos = string.Format("select count(*) as count,Convert(date,post.UpdateTime) as date from dbo.[Facebook.Posts] post inner join dbo.[Facebook.Post.Person] person on post.ID = person.PostID and (person.IsDeleted is null or person.IsDeleted<>1) where CONVERT(date, getdate()) >= Convert(date,post.UpdateTime) and Convert(date,post.UpdateTime) > DATEADD(DD, -30, CONVERT(date, getdate())) and person.PersonID = '{0}' and person.Sentiment = 'Positive' group by Convert(date, post.UpdateTime) order by Convert(date, post.UpdateTime) desc;", id);
            string queryFbNeg = string.Format("select count(*) as count,Convert(date,post.UpdateTime) as date from dbo.[Facebook.Posts] post inner join dbo.[Facebook.Post.Person] person on post.ID = person.PostID and (person.IsDeleted is null or person.IsDeleted<>1)  where CONVERT(date, getdate()) >= Convert(date,post.UpdateTime) and Convert(date,post.UpdateTime) > DATEADD(DD, -30, CONVERT(date, getdate())) and person.PersonID = '{0}' and person.Sentiment = 'Negative' group by Convert(date, post.UpdateTime) order by Convert(date, post.UpdateTime) desc;", id);
            string queryFbNeu = string.Format("select count(*) as count,Convert(date,post.UpdateTime) as date from dbo.[Facebook.Posts] post inner join dbo.[Facebook.Post.Person] person on post.ID = person.PostID and (person.IsDeleted is null or person.IsDeleted<>1)  where CONVERT(date, getdate()) >= Convert(date,post.UpdateTime) and Convert(date,post.UpdateTime) > DATEADD(DD, -30, CONVERT(date, getdate())) and person.PersonID = '{0}' and person.Sentiment = 'Neutral' group by Convert(date, post.UpdateTime) order by Convert(date, post.UpdateTime) desc;", id);

            var resultsFbPos = monitoringDB.Database.SqlQuery<graphResult>(queryFbPos).ToList<graphResult>();
            var resultsFbNeg = monitoringDB.Database.SqlQuery<graphResult>(queryFbNeg).ToList<graphResult>();
            var resultsFbNeu = monitoringDB.Database.SqlQuery<graphResult>(queryFbNeu).ToList<graphResult>();

            string counterfbPos = "[";
            string counterfbNeg = "[";
            string counterfbNeu = "[";

            #region FbPosloop
            for (int s = 0; s < dates.Count; s++)
            {
                DateTime date = dates.ElementAt(s).Date;
                //
                if (resultsFbPos.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterfbPos == "[")
                    {
                        counterfbPos = counterfbPos + resultsFbPos.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    counterfbPos = counterfbPos + "," + resultsFbPos.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                }
                else
                {
                    if (counterfbPos == "[")
                    {
                        counterfbPos = counterfbPos + 0;

                    }
                    counterfbPos = counterfbPos + "," + 0;
                }
            }
            #endregion           
            #region FbNegloop
            for (int s = 0; s < dates.Count; s++)
            {
                DateTime date = dates.ElementAt(s).Date;
                //
                if (resultsFbNeg.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterfbNeg == "[")
                    {
                        counterfbNeg = counterfbNeg + resultsFbNeg.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    counterfbNeg = counterfbNeg + "," + resultsFbNeg.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                }
                else
                {
                    if (counterfbNeg == "[")
                    {
                        counterfbNeg = counterfbNeg + 0;
                    }
                    counterfbNeg = counterfbNeg + "," + 0;
                }
            }
            #endregion   
            #region FbNeuloop
            for (int s = 0; s < dates.Count; s++)
            {
                DateTime date = dates.ElementAt(s).Date;
                //
                if (resultsFbNeu.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterfbNeu == "[")
                    {
                        counterfbNeu = counterfbNeu + resultsFbNeu.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    counterfbNeu = counterfbNeu + "," + resultsFbNeu.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                }
                else
                {
                    if (counterfbNeu == "[")
                    {
                        counterfbNeu = counterfbNeu + 0;
                    }
                    counterfbNeu = counterfbNeu + "," + 0;
                }
            }
            #endregion 

            counterfbPos = counterfbPos + "]";
            counterfbNeg = counterfbNeg + "]";
            counterfbNeu = counterfbNeu + "]";

            ViewBag.FbPosChartCount = counterfbPos;
            ViewBag.FbNegChartCount = counterfbNeg;
            ViewBag.FbNeuChartCount = counterfbNeu;
            // End Facebook Chart //

            // Facebook Chart ///
            string queryTwPos = string.Format("select count(*) as count,Convert(date,tweet.CreatedAt) as date from dbo.[Twitter.Tweets] tweet inner join dbo.[Twitter.Tweet.Person] person on tweet.ID = person.TweetID  and (person.IsDeleted is null or person.IsDeleted<>1)  where CONVERT(date, getdate()) >= Convert(date,tweet.CreatedAt) and Convert(date,tweet.CreatedAt) > DATEADD(DD, -30, CONVERT(date, getdate())) and person.PersonID = '{0}' and person.Sentiment = 'Positive' group by Convert(date, tweet.CreatedAt) order by Convert(date, tweet.CreatedAt) desc;", id);
            string queryTwNeg = string.Format("select count(*) as count,Convert(date,tweet.CreatedAt) as date from dbo.[Twitter.Tweets] tweet inner join dbo.[Twitter.Tweet.Person] person on tweet.ID = person.TweetID  and (person.IsDeleted is null or person.IsDeleted<>1)  where CONVERT(date, getdate()) >= Convert(date,tweet.CreatedAt) and Convert(date,tweet.CreatedAt) > DATEADD(DD, -30, CONVERT(date, getdate())) and person.PersonID = '{0}' and person.Sentiment = 'Negative' group by Convert(date, tweet.CreatedAt) order by Convert(date, tweet.CreatedAt) desc;", id);
            string queryTwNeu = string.Format("select count(*) as count,Convert(date,tweet.CreatedAt) as date from dbo.[Twitter.Tweets] tweet inner join dbo.[Twitter.Tweet.Person] person on tweet.ID = person.TweetID  and (person.IsDeleted is null or person.IsDeleted<>1)  where CONVERT(date, getdate()) >= Convert(date,tweet.CreatedAt) and Convert(date,tweet.CreatedAt) > DATEADD(DD, -30, CONVERT(date, getdate())) and person.PersonID = '{0}' and person.Sentiment = 'Neutral' group by Convert(date, tweet.CreatedAt) order by Convert(date, tweet.CreatedAt) desc;", id);

            var resultsTwPos = monitoringDB.Database.SqlQuery<graphResult>(queryTwPos).ToList<graphResult>();
            var resultsTwNeg = monitoringDB.Database.SqlQuery<graphResult>(queryTwNeg).ToList<graphResult>();
            var resultsTwNeu = monitoringDB.Database.SqlQuery<graphResult>(queryTwNeu).ToList<graphResult>();

            string counterTwPos = "[";
            string counterTwNeg = "[";
            string counterTwNeu = "[";

            #region TwPosloop
            for (int s = 0; s < dates.Count; s++)
            {
                DateTime date = dates.ElementAt(s).Date;
                //
                if (resultsTwPos.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterTwPos == "[")
                    {
                        counterTwPos = counterTwPos + resultsTwPos.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    counterTwPos = counterTwPos + "," + resultsTwPos.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                }
                else
                {
                    if (counterTwPos == "[")
                    {
                        counterTwPos = counterTwPos + 0;

                    }
                    counterTwPos = counterTwPos + "," + 0;
                }
            }
            #endregion           
            #region TwNegloop
            for (int s = 0; s < dates.Count; s++)
            {
                DateTime date = dates.ElementAt(s).Date;
                //
                if (resultsTwNeg.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterTwNeg == "[")
                    {
                        counterTwNeg = counterTwNeg + resultsTwNeg.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    counterTwNeg = counterTwNeg + "," + resultsTwNeg.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                }
                else
                {
                    if (counterTwNeg == "[")
                    {
                        counterTwNeg = counterTwNeg + 0;
                    }
                    counterTwNeg = counterTwNeg + "," + 0;
                }
            }
            #endregion   
            #region TwNeuloop
            for (int s = 0; s < dates.Count; s++)
            {
                DateTime date = dates.ElementAt(s).Date;
                //
                if (resultsTwNeu.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterTwNeu == "[")
                    {
                        counterTwNeu = counterTwNeu + resultsTwNeu.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    counterTwNeu = counterTwNeu + "," + resultsTwNeu.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                }
                else
                {
                    if (counterTwNeu == "[")
                    {
                        counterTwNeu = counterTwNeu + 0;
                    }
                    counterTwNeu = counterTwNeu + "," + 0;
                }
            }
            #endregion 

            counterTwPos = counterTwPos + "]";
            counterTwNeg = counterTwNeg + "]";
            counterTwNeu = counterTwNeu + "]";

            ViewBag.TwPosChartCount = counterTwPos;
            ViewBag.TwNegChartCount = counterTwNeg;
            ViewBag.TwNeuChartCount = counterTwNeu;
            // End Twitter Chart //

            // Facebook Chart ///
            string queryWebPos = string.Format("select count(*) as count,Convert(date,post.Date) as date from dbo.[WebSite.Posts] post inner join dbo.[Website.Post.Person] person on post.ID = person.PostID  and (person.IsDeleted is null or person.IsDeleted<>1)  where CONVERT(date, getdate()) >= Convert(date,post.Date) and Convert(date,post.Date) > DATEADD(DD, -30, CONVERT(date, getdate())) and person.PersonID = '{0}' and person.Sentiment = 'Positive' group by Convert(date, post.Date) order by Convert(date, post.Date) desc;", id);
            string queryWebNeg = string.Format("select count(*) as count,Convert(date,post.Date) as date from dbo.[WebSite.Posts] post inner join dbo.[Website.Post.Person] person on post.ID = person.PostID  and (person.IsDeleted is null or person.IsDeleted<>1)  where CONVERT(date, getdate()) >= Convert(date,post.Date) and Convert(date,post.Date) > DATEADD(DD, -30, CONVERT(date, getdate())) and person.PersonID = '{0}' and person.Sentiment = 'Negative' group by Convert(date, post.Date) order by Convert(date, post.Date) desc;", id);
            string queryWebNeu = string.Format("select count(*) as count,Convert(date,post.Date) as date from dbo.[WebSite.Posts] post inner join dbo.[Website.Post.Person] person on post.ID = person.PostID  and (person.IsDeleted is null or person.IsDeleted<>1) where CONVERT(date, getdate()) >= Convert(date,post.Date) and Convert(date,post.Date) > DATEADD(DD, -30, CONVERT(date, getdate())) and person.PersonID = '{0}' and person.Sentiment = 'Neutral' group by Convert(date, post.Date) order by Convert(date, post.Date) desc;", id);

            var resultsWebPos = monitoringDB.Database.SqlQuery<graphResult>(queryWebPos).ToList<graphResult>();
            var resultsWebNeg = monitoringDB.Database.SqlQuery<graphResult>(queryWebNeg).ToList<graphResult>();
            var resultsWebNeu = monitoringDB.Database.SqlQuery<graphResult>(queryWebNeu).ToList<graphResult>();

            string counterWebPos = "[";
            string counterWebNeg = "[";
            string counterWebNeu = "[";

            #region WebPosloop
            for (int s = 0; s < dates.Count; s++)
            {
                DateTime date = dates.ElementAt(s).Date;
                //
                if (resultsWebPos.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterWebPos == "[")
                    {
                        counterWebPos = counterWebPos + resultsWebPos.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    counterWebPos = counterWebPos + "," + resultsWebPos.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                }
                else
                {
                    if (counterWebPos == "[")
                    {
                        counterWebPos = counterWebPos + 0;

                    }
                    counterWebPos = counterWebPos + "," + 0;
                }
            }
            #endregion           
            #region WebNegloop
            for (int s = 0; s < dates.Count; s++)
            {
                DateTime date = dates.ElementAt(s).Date;
                //
                if (resultsWebNeu.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterWebNeg == "[")
                    {
                        counterWebNeg = counterWebNeg + resultsWebNeu.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    counterWebNeg = counterWebNeg + "," + resultsWebNeu.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                }
                else
                {
                    if (counterWebNeg == "[")
                    {
                        counterWebNeg = counterWebNeg + 0;
                    }
                    counterWebNeg = counterWebNeg + "," + 0;
                }
            }
            #endregion   
            #region WebNeuloop
            for (int s = 0; s < dates.Count; s++)
            {
                DateTime date = dates.ElementAt(s).Date;
                //
                if (resultsWebNeu.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterWebNeu == "[")
                    {
                        counterWebNeu = counterWebNeu + resultsWebNeu.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    counterWebNeu = counterWebNeu + "," + resultsWebNeu.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                }
                else
                {
                    if (counterWebNeu == "[")
                    {
                        counterWebNeu = counterWebNeu + 0;
                    }
                    counterWebNeu = counterWebNeu + "," + 0;
                }
            }
            #endregion 

            counterWebPos = counterWebPos + "]";
            counterWebNeg = counterWebNeg + "]";
            counterWebNeu = counterWebNeu + "]";

            ViewBag.WebPosChartCount = counterWebPos;
            ViewBag.WebNegChartCount = counterWebNeg;
            ViewBag.WebNeuChartCount = counterWebNeu;
            //2022-05-11 bulgaa end


            return View(currentUser);
        }
        public ActionResult AnalysisOfWeek(string ObjectId)
        {
            if (User.Identity.IsAuthenticated == false)
            {
                return Redirect("/Account/Login");
            }
            MonitoringEntities monitoringDB = new MonitoringEntities();
            /* var id = "e7aca6b9-5973-49dd-9c9d-1e93222b759a";*/
            var id = "8902d568-4f77-4874-97b8-f7dd7bc6d655";
            DateTime LastWeek = DateTime.Now.AddDays(-7);
            ViewBag.FbData =
              (from a in monitoringDB.Facebook_Post_Person
               join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
               join c in monitoringDB.System_Person on a.PersonID equals c.ID
               where c.ID.ToString() == id && a.IsDeleted != true && b.UpdateTime >= LastWeek
               orderby b.UpdateTime descending
               select new Models.FbPost
               {
                   ID = b.ID,
               }).Count();

            ViewBag.TwitterData =
               (from tweetPerson in monitoringDB.Twitter_Tweet_Person
                join tweet in monitoringDB.Twitter_Tweets on tweetPerson.TweetID equals tweet.ID
                join sysPerson in monitoringDB.System_Person on tweetPerson.PersonID equals sysPerson.ID
                join tweetUser in monitoringDB.Twitter_User_Details on tweet.TwitterUserID equals tweetUser.TwitterUserID
                where sysPerson.ID.ToString() == id && tweetPerson.IsDeleted != true && tweet.CreatedAt >= LastWeek
                orderby tweet.CreatedAt descending
                select new Models.Twitter
                {
                    ID = tweet.ID,
                }).Count();

            ViewBag.WebSiteData =
              (from a in monitoringDB.Website_Post_Person
               join b in monitoringDB.WebSite_Posts on a.PostID equals b.ID
               join c in monitoringDB.System_Person on a.PersonID equals c.ID
               where c.ID.ToString() == id && a.IsDeleted != true && b.DateTime >= LastWeek
               orderby b.Date descending
               select new Models.WebSitePost
               {
                   ID = b.ID,
               }).Count();
            var currentUser = monitoringDB.System_Person.Where(a => a.ID.ToString().Equals(id)).Select(n => new Models.SystemUserModels
            {
                ID = n.ID.ToString(),
                Name = n.Name,
                Surename = n.Surename,
                Tittlename = n.Tittlename,
                Picture = n.Picture,
                Description = n.Description,
                FacebookAccount = n.FacebookAccount.ToString(),
                TwitterAccount = n.TwitterAccount.ToString()
            }).FirstOrDefault();


            //Sentiment Web
            var PositiveWb =
             (from a in monitoringDB.Website_Post_Person
              join b in monitoringDB.WebSite_Posts on a.PostID equals b.ID
              join c in monitoringDB.System_Person on a.PersonID equals c.ID
              where c.ID.ToString() == id && a.IsDeleted != true && b.DateTime >= LastWeek && a.Sentiment.ToString() == "Positive"

              select new Models.SystemUserModels
              {
                  ID = b.ID.ToString(),
              }).Count();

            var NegativeWb =
                (from a in monitoringDB.Website_Post_Person
                 join b in monitoringDB.WebSite_Posts on a.PostID equals b.ID
                 join c in monitoringDB.System_Person on a.PersonID equals c.ID
                 where c.ID.ToString() == id && a.IsDeleted != true && b.DateTime >= LastWeek && a.Sentiment.ToString() == "Negative"

                 select new Models.SystemUserModels
                 {
                     ID = b.ID.ToString(),
                 }).Count();
            var NeutralWB =
               (from a in monitoringDB.Website_Post_Person
                join b in monitoringDB.WebSite_Posts on a.PostID equals b.ID
                join c in monitoringDB.System_Person on a.PersonID equals c.ID
                where c.ID.ToString() == id && a.IsDeleted != true && b.DateTime >= LastWeek && a.Sentiment.ToString() == "Neutral"

                select new Models.SystemUserModels
                {
                    ID = b.ID.ToString(),
                }).Count();
            //Facebook posts
            var PositiveFb =
              (from a in monitoringDB.Facebook_Post_Person
               join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
               join c in monitoringDB.System_Person on a.PersonID equals c.ID
               where c.ID.ToString() == id && a.IsDeleted != true && b.UpdateTime >= LastWeek && a.Sentiment.ToString() == "Positive"

               select new Models.SystemUserModels
               {
                   ID = a.ID.ToString(),
               }).Count();

            var NegativeFb =
               (from a in monitoringDB.Facebook_Post_Person
                join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
                join c in monitoringDB.System_Person on a.PersonID equals c.ID
                where c.ID.ToString() == id && a.IsDeleted != true && b.UpdateTime >= LastWeek && a.Sentiment.ToString() == "Negative"

                select new Models.SystemUserModels
                {
                    ID = a.ID.ToString(),
                }).Count();
            var NeutralFB =
              (from a in monitoringDB.Facebook_Post_Person
               join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
               join c in monitoringDB.System_Person on a.PersonID equals c.ID
               where c.ID.ToString() == id && a.IsDeleted != true && b.UpdateTime >= LastWeek && a.Sentiment.ToString() == "Neutral"

               select new Models.SystemUserModels
               {
                   ID = a.ID.ToString(),
               }).Count();
            //Twitter posts
            var PositiveTW =
              (from tweetPerson in monitoringDB.Twitter_Tweet_Person
               join tweet in monitoringDB.Twitter_Tweets on tweetPerson.TweetID equals tweet.ID
               join sysPerson in monitoringDB.System_Person on tweetPerson.PersonID equals sysPerson.ID
               join tweetUser in monitoringDB.Twitter_User_Details on tweet.TwitterUserID equals tweetUser.TwitterUserID
               where sysPerson.ID.ToString() == id && tweetPerson.IsDeleted != true && tweet.CreatedAt >= LastWeek && tweetPerson.Sentiment.ToString() == "Positive"
               select new Models.SystemUserModels
               {
                   ID = tweetPerson.ID.ToString(),
               }).Count();
            var NegativeTW =
               (from tweetPerson in monitoringDB.Twitter_Tweet_Person
                join tweet in monitoringDB.Twitter_Tweets on tweetPerson.TweetID equals tweet.ID
                join sysPerson in monitoringDB.System_Person on tweetPerson.PersonID equals sysPerson.ID
                join tweetUser in monitoringDB.Twitter_User_Details on tweet.TwitterUserID equals tweetUser.TwitterUserID
                where sysPerson.ID.ToString() == id && tweetPerson.IsDeleted != true && tweet.CreatedAt >= LastWeek && tweetPerson.Sentiment.ToString() == "Negative"
                select new Models.SystemUserModels
                {
                    ID = tweetPerson.ID.ToString(),
                }).Count();
            var NeutralTW =
             (from tweetPerson in monitoringDB.Twitter_Tweet_Person
              join tweet in monitoringDB.Twitter_Tweets on tweetPerson.TweetID equals tweet.ID
              join sysPerson in monitoringDB.System_Person on tweetPerson.PersonID equals sysPerson.ID
              join tweetUser in monitoringDB.Twitter_User_Details on tweet.TwitterUserID equals tweetUser.TwitterUserID
              where sysPerson.ID.ToString() == id && tweetPerson.IsDeleted != true && tweet.CreatedAt >= LastWeek && tweetPerson.Sentiment.ToString() == "Neutral"
              select new Models.SystemUserModels
              {
                  ID = tweetPerson.ID.ToString(),
              }).Count();
            var ss = monitoringDB.Twitter_Tweet_Person.Where(a => a.PersonID.ToString().Equals(id) && a.Sentiment.ToString() == "Neutral").Select(n => new Models.SystemUserModels
            {
                ID = n.ID.ToString()
            }).Count();


            int[] arr = new int[8];
            var i = 0;
            while (i < 8)
            {
                DateTime testDate = DateTime.Now.AddMonths(-i);
                DateTime testDate2 = DateTime.Now.AddMonths(-(i + 1));
                arr[i] =
                     (from postPerson in monitoringDB.Facebook_Post_Person
                      join a in monitoringDB.Facebook_Posts on postPerson.PostID equals a.ID
                      where postPerson.PersonID.ToString().Equals(id)
                      where a.UpdateTime < testDate
                      where a.UpdateTime > testDate2
                      select new Models.FbPost
                      {
                          ID = a.ID,

                      }).Count();
                i++;
            }
            var fbPosts =
                (from a in monitoringDB.Facebook_Post_Person
                 join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
                 join c in monitoringDB.System_Person on a.PersonID equals c.ID
                 where (c.ID.ToString() == id) && a.IsDeleted != true && b.UpdateTime >= LastWeek
                 select new Models.FbPost
                 {
                     Message = b.Message,

                 }).ToList();
            string AllTextFb = string.Concat(fbPosts.Select(n => n.Message));
            var FbWords = Regex.Split(AllTextFb.ToLower(), @"\W+")
                .Where(s => s.Length > 3 && s != "байна" && s != "байгаа" && s != "байх" && s != "байсан" && s != "байдаг" && s != "shig" && s != "бгаа" && s != "чинь" && s != "юм" && s != "bgaa" && s != "болж" && s != "байж" && s != "болсон" && s != "гээд" && s != "гэсэн" && s != "минь" && s != "bgaa" && s != "shuu" && s != "https")
                .GroupBy(s => s)
                .Select(s => new Models.LargeTextModels
                {
                    word = s.Key,
                    count = s.Count()
                })
                .OrderByDescending(g => g.count).Take(10).ToList();

            var twPosts =
                (from a in monitoringDB.Facebook_Post_Person
                 join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
                 join c in monitoringDB.System_Person on a.PersonID equals c.ID
                 where (c.ID.ToString() == id) && a.IsDeleted != true && b.UpdateTime >= LastWeek
                 select new Models.FbPost
                 {
                     Message = b.Message,

                 }).ToList();
            string TwAllText = string.Concat(twPosts.Select(n => n.Message));
            var TwWords = Regex.Split(TwAllText.ToLower(), @"\W+")
                .Where(s => s.Length > 3 && s != "байна" && s != "байгаа" && s != "байх" && s != "байсан" && s != "байдаг" && s != "shig" && s != "бгаа" && s != "чинь" && s != "юм" && s != "bgaa" && s != "болж" && s != "байж" && s != "болсон" && s != "гээд" && s != "гэсэн" && s != "минь" && s != "bgaa" && s != "shuu" && s != "https")
                .GroupBy(s => s)
                .Select(s => new Models.LargeTextModels
                {
                    word = s.Key,
                    count = s.Count()
                })
                .OrderByDescending(g => g.count).Take(10).ToList();
            var wbPosts =
                (from a in monitoringDB.Facebook_Post_Person
                 join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
                 join c in monitoringDB.System_Person on a.PersonID equals c.ID
                 where (c.ID.ToString() == id) && a.IsDeleted != true && b.UpdateTime >= LastWeek
                 select new Models.FbPost
                 {
                     Message = b.Message,

                 }).ToList();
            string WbAllText = string.Concat(wbPosts.Select(n => n.Message));
            var WbWords = Regex.Split(WbAllText.ToLower(), @"\W+")
                .Where(s => s.Length > 3 && s != "байна" && s != "байгаа" && s != "байх" && s != "байсан" && s != "байдаг" && s != "shig" && s != "бгаа" && s != "чинь" && s != "юм" && s != "bgaa" && s != "болж" && s != "байж" && s != "болсон" && s != "гээд" && s != "гэсэн" && s != "минь" && s != "bgaa" && s != "shuu" && s != "https")
                .GroupBy(s => s)
                .Select(s => new Models.LargeTextModels
                {
                    word = s.Key,
                    count = s.Count()
                })
                .OrderByDescending(g => g.count).Take(10).ToList();
            ViewBag.Mounth = arr;
            ViewBag.FbWords = FbWords;
            ViewBag.TwWords = TwWords;
            ViewBag.WbWords = WbWords;
            ViewBag.NeutralTW = NeutralTW;
            ViewBag.NegativeTW = NegativeTW;
            ViewBag.PositiveTW = PositiveTW;
            ViewBag.NeutralWB = NeutralWB;
            ViewBag.NegativeWb = NegativeWb;
            ViewBag.PositiveWb = PositiveWb;
            ViewBag.NeutralFB = NeutralFB;
            ViewBag.NegativeFb = NegativeFb;
            ViewBag.PositiveFb = PositiveFb;
            ViewBag.socialAll = ViewBag.TwitterData + ViewBag.FbData;
            ViewBag.tvAll = ViewBag.WebSiteData;
            ViewBag.PercentageFb = ((ViewBag.FbData * 100) / ViewBag.socialAll);
            ViewBag.PercentageTwitter = ((ViewBag.TwitterData * 100) / ViewBag.socialAll);

            //2022-05-11 begin
            // making date list
            DateTime last = new DateTime();
            last = DateTime.Now.AddDays(-7);
            DateTime real = DateTime.Now;
            List<DateTime> dates = new List<DateTime>();
            do
            {
                dates.Add(last);
                last = last.AddDays(+1);
            } while (real.Date != last.Date);



            // Facebook Chart ///
            string queryFbPos = string.Format("select count(*) as count,Convert(date,post.UpdateTime) as date from dbo.[Facebook.Posts] post inner join dbo.[Facebook.Post.Person] person on post.ID = person.PostID and (person.IsDeleted is null or person.IsDeleted<>1) where CONVERT(date, getdate()) >= Convert(date,post.UpdateTime) and Convert(date,post.UpdateTime) > DATEADD(DD, -7, CONVERT(date, getdate())) and person.PersonID = '{0}' and person.Sentiment = 'Positive' group by Convert(date, post.UpdateTime) order by Convert(date, post.UpdateTime) desc;", id);
            string queryFbNeg = string.Format("select count(*) as count,Convert(date,post.UpdateTime) as date from dbo.[Facebook.Posts] post inner join dbo.[Facebook.Post.Person] person on post.ID = person.PostID and (person.IsDeleted is null or person.IsDeleted<>1)  where CONVERT(date, getdate()) >= Convert(date,post.UpdateTime) and Convert(date,post.UpdateTime) > DATEADD(DD, -7, CONVERT(date, getdate())) and person.PersonID = '{0}' and person.Sentiment = 'Negative' group by Convert(date, post.UpdateTime) order by Convert(date, post.UpdateTime) desc;", id);
            string queryFbNeu = string.Format("select count(*) as count,Convert(date,post.UpdateTime) as date from dbo.[Facebook.Posts] post inner join dbo.[Facebook.Post.Person] person on post.ID = person.PostID and (person.IsDeleted is null or person.IsDeleted<>1)  where CONVERT(date, getdate()) >= Convert(date,post.UpdateTime) and Convert(date,post.UpdateTime) > DATEADD(DD, -7, CONVERT(date, getdate())) and person.PersonID = '{0}' and person.Sentiment = 'Neutral' group by Convert(date, post.UpdateTime) order by Convert(date, post.UpdateTime) desc;", id);

            var resultsFbPos = monitoringDB.Database.SqlQuery<graphResult>(queryFbPos).ToList<graphResult>();
            var resultsFbNeg = monitoringDB.Database.SqlQuery<graphResult>(queryFbNeg).ToList<graphResult>();
            var resultsFbNeu = monitoringDB.Database.SqlQuery<graphResult>(queryFbNeu).ToList<graphResult>();

            string counterfbPos = "[";
            string counterfbNeg = "[";
            string counterfbNeu = "[";

            #region FbPosloop
            for (int s = 0; s < dates.Count; s++)
            {
                DateTime date = dates.ElementAt(s).Date;
                //
                if (resultsFbPos.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterfbPos == "[")
                    {
                        counterfbPos = counterfbPos + resultsFbPos.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    counterfbPos = counterfbPos + "," + resultsFbPos.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                }
                else
                {
                    if (counterfbPos == "[")
                    {
                        counterfbPos = counterfbPos + 0;

                    }
                    counterfbPos = counterfbPos + "," + 0;
                }
            }
            #endregion           
            #region FbNegloop
            for (int s = 0; s < dates.Count; s++)
            {
                DateTime date = dates.ElementAt(s).Date;
                //
                if (resultsFbNeg.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterfbNeg == "[")
                    {
                        counterfbNeg = counterfbNeg + resultsFbNeg.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    counterfbNeg = counterfbNeg + "," + resultsFbNeg.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                }
                else
                {
                    if (counterfbNeg == "[")
                    {
                        counterfbNeg = counterfbNeg + 0;
                    }
                    counterfbNeg = counterfbNeg + "," + 0;
                }
            }
            #endregion   
            #region FbNeuloop
            for (int s = 0; s < dates.Count; s++)
            {
                DateTime date = dates.ElementAt(s).Date;
                //
                if (resultsFbNeu.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterfbNeu == "[")
                    {
                        counterfbNeu = counterfbNeu + resultsFbNeu.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    counterfbNeu = counterfbNeu + "," + resultsFbNeu.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                }
                else
                {
                    if (counterfbNeu == "[")
                    {
                        counterfbNeu = counterfbNeu + 0;
                    }
                    counterfbNeu = counterfbNeu + "," + 0;
                }
            }
            #endregion 

            counterfbPos = counterfbPos + "]";
            counterfbNeg = counterfbNeg + "]";
            counterfbNeu = counterfbNeu + "]";

            ViewBag.FbPosChartCount = counterfbPos;
            ViewBag.FbNegChartCount = counterfbNeg;
            ViewBag.FbNeuChartCount = counterfbNeu;
            // End Facebook Chart //

            // Facebook Chart ///
            string queryTwPos = string.Format("select count(*) as count,Convert(date,tweet.CreatedAt) as date from dbo.[Twitter.Tweets] tweet inner join dbo.[Twitter.Tweet.Person] person on tweet.ID = person.TweetID  and (person.IsDeleted is null or person.IsDeleted<>1)  where CONVERT(date, getdate()) >= Convert(date,tweet.CreatedAt) and Convert(date,tweet.CreatedAt) > DATEADD(DD, -7, CONVERT(date, getdate())) and person.PersonID = '{0}' and person.Sentiment = 'Positive' group by Convert(date, tweet.CreatedAt) order by Convert(date, tweet.CreatedAt) desc;", id);
            string queryTwNeg = string.Format("select count(*) as count,Convert(date,tweet.CreatedAt) as date from dbo.[Twitter.Tweets] tweet inner join dbo.[Twitter.Tweet.Person] person on tweet.ID = person.TweetID  and (person.IsDeleted is null or person.IsDeleted<>1)  where CONVERT(date, getdate()) >= Convert(date,tweet.CreatedAt) and Convert(date,tweet.CreatedAt) > DATEADD(DD, -7, CONVERT(date, getdate())) and person.PersonID = '{0}' and person.Sentiment = 'Negative' group by Convert(date, tweet.CreatedAt) order by Convert(date, tweet.CreatedAt) desc;", id);
            string queryTwNeu = string.Format("select count(*) as count,Convert(date,tweet.CreatedAt) as date from dbo.[Twitter.Tweets] tweet inner join dbo.[Twitter.Tweet.Person] person on tweet.ID = person.TweetID  and (person.IsDeleted is null or person.IsDeleted<>1)  where CONVERT(date, getdate()) >= Convert(date,tweet.CreatedAt) and Convert(date,tweet.CreatedAt) > DATEADD(DD, -7, CONVERT(date, getdate())) and person.PersonID = '{0}' and person.Sentiment = 'Neutral' group by Convert(date, tweet.CreatedAt) order by Convert(date, tweet.CreatedAt) desc;", id);

            var resultsTwPos = monitoringDB.Database.SqlQuery<graphResult>(queryTwPos).ToList<graphResult>();
            var resultsTwNeg = monitoringDB.Database.SqlQuery<graphResult>(queryTwNeg).ToList<graphResult>();
            var resultsTwNeu = monitoringDB.Database.SqlQuery<graphResult>(queryTwNeu).ToList<graphResult>();

            string counterTwPos = "[";
            string counterTwNeg = "[";
            string counterTwNeu = "[";

            #region TwPosloop
            for (int s = 0; s < dates.Count; s++)
            {
                DateTime date = dates.ElementAt(s).Date;
                //
                if (resultsTwPos.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterTwPos == "[")
                    {
                        counterTwPos = counterTwPos + resultsTwPos.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    counterTwPos = counterTwPos + "," + resultsTwPos.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                }
                else
                {
                    if (counterTwPos == "[")
                    {
                        counterTwPos = counterTwPos + 0;

                    }
                    counterTwPos = counterTwPos + "," + 0;
                }
            }
            #endregion           
            #region TwNegloop
            for (int s = 0; s < dates.Count; s++)
            {
                DateTime date = dates.ElementAt(s).Date;
                //
                if (resultsTwNeg.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterTwNeg == "[")
                    {
                        counterTwNeg = counterTwNeg + resultsTwNeg.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    counterTwNeg = counterTwNeg + "," + resultsTwNeg.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                }
                else
                {
                    if (counterTwNeg == "[")
                    {
                        counterTwNeg = counterTwNeg + 0;
                    }
                    counterTwNeg = counterTwNeg + "," + 0;
                }
            }
            #endregion   
            #region TwNeuloop
            for (int s = 0; s < dates.Count; s++)
            {
                DateTime date = dates.ElementAt(s).Date;
                //
                if (resultsTwNeu.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterTwNeu == "[")
                    {
                        counterTwNeu = counterTwNeu + resultsTwNeu.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    counterTwNeu = counterTwNeu + "," + resultsTwNeu.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                }
                else
                {
                    if (counterTwNeu == "[")
                    {
                        counterTwNeu = counterTwNeu + 0;
                    }
                    counterTwNeu = counterTwNeu + "," + 0;
                }
            }
            #endregion 

            counterTwPos = counterTwPos + "]";
            counterTwNeg = counterTwNeg + "]";
            counterTwNeu = counterTwNeu + "]";

            ViewBag.TwPosChartCount = counterTwPos;
            ViewBag.TwNegChartCount = counterTwNeg;
            ViewBag.TwNeuChartCount = counterTwNeu;
            // End Twitter Chart //

            // Facebook Chart ///
            string queryWebPos = string.Format("select count(*) as count,Convert(date,post.Date) as date from dbo.[WebSite.Posts] post inner join dbo.[Website.Post.Person] person on post.ID = person.PostID  and (person.IsDeleted is null or person.IsDeleted<>1)  where CONVERT(date, getdate()) >= Convert(date,post.Date) and Convert(date,post.Date) > DATEADD(DD, -7, CONVERT(date, getdate())) and person.PersonID = '{0}' and person.Sentiment = 'Positive' group by Convert(date, post.Date) order by Convert(date, post.Date) desc;", id);
            string queryWebNeg = string.Format("select count(*) as count,Convert(date,post.Date) as date from dbo.[WebSite.Posts] post inner join dbo.[Website.Post.Person] person on post.ID = person.PostID  and (person.IsDeleted is null or person.IsDeleted<>1)  where CONVERT(date, getdate()) >= Convert(date,post.Date) and Convert(date,post.Date) > DATEADD(DD, -7, CONVERT(date, getdate())) and person.PersonID = '{0}' and person.Sentiment = 'Negative' group by Convert(date, post.Date) order by Convert(date, post.Date) desc;", id);
            string queryWebNeu = string.Format("select count(*) as count,Convert(date,post.Date) as date from dbo.[WebSite.Posts] post inner join dbo.[Website.Post.Person] person on post.ID = person.PostID  and (person.IsDeleted is null or person.IsDeleted<>1) where CONVERT(date, getdate()) >= Convert(date,post.Date) and Convert(date,post.Date) > DATEADD(DD, -7, CONVERT(date, getdate())) and person.PersonID = '{0}' and person.Sentiment = 'Neutral' group by Convert(date, post.Date) order by Convert(date, post.Date) desc;", id);

            var resultsWebPos = monitoringDB.Database.SqlQuery<graphResult>(queryWebPos).ToList<graphResult>();
            var resultsWebNeg = monitoringDB.Database.SqlQuery<graphResult>(queryWebNeg).ToList<graphResult>();
            var resultsWebNeu = monitoringDB.Database.SqlQuery<graphResult>(queryWebNeu).ToList<graphResult>();

            string counterWebPos = "[";
            string counterWebNeg = "[";
            string counterWebNeu = "[";

            #region WebPosloop
            for (int s = 0; s < dates.Count; s++)
            {
                DateTime date = dates.ElementAt(s).Date;
                //
                if (resultsWebPos.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterWebPos == "[")
                    {
                        counterWebPos = counterWebPos + resultsWebPos.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    counterWebPos = counterWebPos + "," + resultsWebPos.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                }
                else
                {
                    if (counterWebPos == "[")
                    {
                        counterWebPos = counterWebPos + 0;

                    }
                    counterWebPos = counterWebPos + "," + 0;
                }
            }
            #endregion           
            #region WebNegloop
            for (int s = 0; s < dates.Count; s++)
            {
                DateTime date = dates.ElementAt(s).Date;
                //
                if (resultsWebNeu.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterWebNeg == "[")
                    {
                        counterWebNeg = counterWebNeg + resultsWebNeu.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    counterWebNeg = counterWebNeg + "," + resultsWebNeu.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                }
                else
                {
                    if (counterWebNeg == "[")
                    {
                        counterWebNeg = counterWebNeg + 0;
                    }
                    counterWebNeg = counterWebNeg + "," + 0;
                }
            }
            #endregion   
            #region WebNeuloop
            for (int s = 0; s < dates.Count; s++)
            {
                DateTime date = dates.ElementAt(s).Date;
                //
                if (resultsWebNeu.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterWebNeu == "[")
                    {
                        counterWebNeu = counterWebNeu + resultsWebNeu.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    counterWebNeu = counterWebNeu + "," + resultsWebNeu.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                }
                else
                {
                    if (counterWebNeu == "[")
                    {
                        counterWebNeu = counterWebNeu + 0;
                    }
                    counterWebNeu = counterWebNeu + "," + 0;
                }
            }
            #endregion 

            counterWebPos = counterWebPos + "]";
            counterWebNeg = counterWebNeg + "]";
            counterWebNeu = counterWebNeu + "]";

            ViewBag.WebPosChartCount = counterWebPos;
            ViewBag.WebNegChartCount = counterWebNeg;
            ViewBag.WebNeuChartCount = counterWebNeu;
            //2022-05-11 bulgaa end


            return View(currentUser);
        }
        public ActionResult AnalysisOfTwoWeeks(string ObjectId)
        {
            if (User.Identity.IsAuthenticated == false)
            {
                return Redirect("/Account/Login");
            }
            MonitoringEntities monitoringDB = new MonitoringEntities();
            /* var id = "e7aca6b9-5973-49dd-9c9d-1e93222b759a";*/
            var id = ObjectId;
            ViewBag.id = id;
            DateTime LastWeek = DateTime.Now.AddDays(-14);
            ViewBag.FbData =
              (from a in monitoringDB.Facebook_Post_Person
               join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
               join c in monitoringDB.System_Person on a.PersonID equals c.ID
               where c.ID.ToString() == id && a.IsDeleted != true && b.UpdateTime >= LastWeek
               orderby b.UpdateTime descending
               select new Models.FbPost
               {
                   ID = b.ID,
               }).Count();

            ViewBag.TwitterData =
               (from tweetPerson in monitoringDB.Twitter_Tweet_Person
                join tweet in monitoringDB.Twitter_Tweets on tweetPerson.TweetID equals tweet.ID
                join sysPerson in monitoringDB.System_Person on tweetPerson.PersonID equals sysPerson.ID
                join tweetUser in monitoringDB.Twitter_User_Details on tweet.TwitterUserID equals tweetUser.TwitterUserID
                where sysPerson.ID.ToString() == id && tweetPerson.IsDeleted != true && tweet.CreatedAt >= LastWeek
                orderby tweet.CreatedAt descending
                select new Models.Twitter
                {
                    ID = tweet.ID,
                }).Count();

            ViewBag.WebSiteData =
              (from a in monitoringDB.Website_Post_Person
               join b in monitoringDB.WebSite_Posts on a.PostID equals b.ID
               join c in monitoringDB.System_Person on a.PersonID equals c.ID
               where c.ID.ToString() == id && a.IsDeleted != true && b.DateTime >= LastWeek
               orderby b.Date descending
               select new Models.WebSitePost
               {
                   ID = b.ID,
               }).Count();
            var currentUser = monitoringDB.System_Person.Where(a => a.ID.ToString().Equals(id)).Select(n => new Models.SystemUserModels
            {
                ID = n.ID.ToString(),
                Name = n.Name,
                Surename = n.Surename,
                Tittlename = n.Tittlename,
                Picture = n.Picture,
                Description = n.Description,
                FacebookAccount = n.FacebookAccount.ToString(),
                TwitterAccount = n.TwitterAccount.ToString()
            }).FirstOrDefault();


            //Sentiment Web
            var PositiveWb =
             (from a in monitoringDB.Website_Post_Person
              join b in monitoringDB.WebSite_Posts on a.PostID equals b.ID
              join c in monitoringDB.System_Person on a.PersonID equals c.ID
              where c.ID.ToString() == id && a.IsDeleted != true && b.DateTime >= LastWeek && a.Sentiment.ToString() == "Positive"

              select new Models.SystemUserModels
              {
                  ID = b.ID.ToString(),
              }).Count();

            var NegativeWb =
                (from a in monitoringDB.Website_Post_Person
                 join b in monitoringDB.WebSite_Posts on a.PostID equals b.ID
                 join c in monitoringDB.System_Person on a.PersonID equals c.ID
                 where c.ID.ToString() == id && a.IsDeleted != true && b.DateTime >= LastWeek && a.Sentiment.ToString() == "Negative"

                 select new Models.SystemUserModels
                 {
                     ID = b.ID.ToString(),
                 }).Count();
            var NeutralWB =
               (from a in monitoringDB.Website_Post_Person
                join b in monitoringDB.WebSite_Posts on a.PostID equals b.ID
                join c in monitoringDB.System_Person on a.PersonID equals c.ID
                where c.ID.ToString() == id && a.IsDeleted != true && b.DateTime >= LastWeek && a.Sentiment.ToString() == "Neutral"

                select new Models.SystemUserModels
                {
                    ID = b.ID.ToString(),
                }).Count();
            //Facebook posts
            var PositiveFb =
              (from a in monitoringDB.Facebook_Post_Person
               join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
               join c in monitoringDB.System_Person on a.PersonID equals c.ID
               where c.ID.ToString() == id && a.IsDeleted != true && b.UpdateTime >= LastWeek && a.Sentiment.ToString() == "Positive"

               select new Models.SystemUserModels
               {
                   ID = a.ID.ToString(),
               }).Count();

            var NegativeFb =
               (from a in monitoringDB.Facebook_Post_Person
                join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
                join c in monitoringDB.System_Person on a.PersonID equals c.ID
                where c.ID.ToString() == id && a.IsDeleted != true && b.UpdateTime >= LastWeek && a.Sentiment.ToString() == "Negative"

                select new Models.SystemUserModels
                {
                    ID = a.ID.ToString(),
                }).Count();
            var NeutralFB =
              (from a in monitoringDB.Facebook_Post_Person
               join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
               join c in monitoringDB.System_Person on a.PersonID equals c.ID
               where c.ID.ToString() == id && a.IsDeleted != true && b.UpdateTime >= LastWeek && a.Sentiment.ToString() == "Neutral"

               select new Models.SystemUserModels
               {
                   ID = a.ID.ToString(),
               }).Count();
            //Twitter posts
            var PositiveTW =
              (from tweetPerson in monitoringDB.Twitter_Tweet_Person
               join tweet in monitoringDB.Twitter_Tweets on tweetPerson.TweetID equals tweet.ID
               join sysPerson in monitoringDB.System_Person on tweetPerson.PersonID equals sysPerson.ID
               join tweetUser in monitoringDB.Twitter_User_Details on tweet.TwitterUserID equals tweetUser.TwitterUserID
               where sysPerson.ID.ToString() == id && tweetPerson.IsDeleted != true && tweet.CreatedAt >= LastWeek && tweetPerson.Sentiment.ToString() == "Positive"
               select new Models.SystemUserModels
               {
                   ID = tweetPerson.ID.ToString(),
               }).Count();
            var NegativeTW =
               (from tweetPerson in monitoringDB.Twitter_Tweet_Person
                join tweet in monitoringDB.Twitter_Tweets on tweetPerson.TweetID equals tweet.ID
                join sysPerson in monitoringDB.System_Person on tweetPerson.PersonID equals sysPerson.ID
                join tweetUser in monitoringDB.Twitter_User_Details on tweet.TwitterUserID equals tweetUser.TwitterUserID
                where sysPerson.ID.ToString() == id && tweetPerson.IsDeleted != true && tweet.CreatedAt >= LastWeek && tweetPerson.Sentiment.ToString() == "Negative"
                select new Models.SystemUserModels
                {
                    ID = tweetPerson.ID.ToString(),
                }).Count();
            var NeutralTW =
             (from tweetPerson in monitoringDB.Twitter_Tweet_Person
              join tweet in monitoringDB.Twitter_Tweets on tweetPerson.TweetID equals tweet.ID
              join sysPerson in monitoringDB.System_Person on tweetPerson.PersonID equals sysPerson.ID
              join tweetUser in monitoringDB.Twitter_User_Details on tweet.TwitterUserID equals tweetUser.TwitterUserID
              where sysPerson.ID.ToString() == id && tweetPerson.IsDeleted != true && tweet.CreatedAt >= LastWeek && tweetPerson.Sentiment.ToString() == "Neutral"
              select new Models.SystemUserModels
              {
                  ID = tweetPerson.ID.ToString(),
              }).Count();
            var ss = monitoringDB.Twitter_Tweet_Person.Where(a => a.PersonID.ToString().Equals(id) && a.Sentiment.ToString() == "Neutral").Select(n => new Models.SystemUserModels
            {
                ID = n.ID.ToString()
            }).Count();


            int[] arr = new int[8];
            var i = 0;
            while (i < 8)
            {
                DateTime testDate = DateTime.Now.AddMonths(-i);
                DateTime testDate2 = DateTime.Now.AddMonths(-(i + 1));
                arr[i] =
                     (from postPerson in monitoringDB.Facebook_Post_Person
                      join a in monitoringDB.Facebook_Posts on postPerson.PostID equals a.ID
                      where postPerson.PersonID.ToString().Equals(id)
                      where a.UpdateTime < testDate
                      where a.UpdateTime > testDate2
                      select new Models.FbPost
                      {
                          ID = a.ID,

                      }).Count();
                i++;
            }
            var fbPosts =
                (from a in monitoringDB.Facebook_Post_Person
                 join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
                 join c in monitoringDB.System_Person on a.PersonID equals c.ID
                 where (c.ID.ToString() == id) && a.IsDeleted != true && b.UpdateTime >= LastWeek
                 select new Models.FbPost
                 {
                     Message = b.Message,

                 }).ToList();
            string AllTextFb = string.Concat(fbPosts.Select(n => n.Message));
            var FbWords = Regex.Split(AllTextFb.ToLower(), @"\W+")
                .Where(s => s.Length > 3 && s != "байна" && s != "байгаа" && s != "байх" && s != "байсан" && s != "байдаг" && s != "shig" && s != "бгаа" && s != "чинь" && s != "юм" && s != "bgaa" && s != "болж" && s != "байж" && s != "болсон" && s != "гээд" && s != "гэсэн" && s != "минь" && s != "bgaa" && s != "shuu" && s != "https")
                .GroupBy(s => s)
                .Select(s => new Models.LargeTextModels
                {
                    word = s.Key,
                    count = s.Count()
                })
                .OrderByDescending(g => g.count).Take(10).ToList();

            var twPosts =
                (from a in monitoringDB.Facebook_Post_Person
                 join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
                 join c in monitoringDB.System_Person on a.PersonID equals c.ID
                 where (c.ID.ToString() == id) && a.IsDeleted != true && b.UpdateTime >= LastWeek
                 select new Models.FbPost
                 {
                     Message = b.Message,

                 }).ToList();
            string TwAllText = string.Concat(twPosts.Select(n => n.Message));
            var TwWords = Regex.Split(TwAllText.ToLower(), @"\W+")
                .Where(s => s.Length > 3 && s != "байна" && s != "байгаа" && s != "байх" && s != "байсан" && s != "байдаг" && s != "shig" && s != "бгаа" && s != "чинь" && s != "юм" && s != "bgaa" && s != "болж" && s != "байж" && s != "болсон" && s != "гээд" && s != "гэсэн" && s != "минь" && s != "bgaa" && s != "shuu" && s != "https")
                .GroupBy(s => s)
                .Select(s => new Models.LargeTextModels
                {
                    word = s.Key,
                    count = s.Count()
                })
                .OrderByDescending(g => g.count).Take(10).ToList();
            var wbPosts =
                (from a in monitoringDB.Facebook_Post_Person
                 join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
                 join c in monitoringDB.System_Person on a.PersonID equals c.ID
                 where (c.ID.ToString() == id) && a.IsDeleted != true && b.UpdateTime >= LastWeek
                 select new Models.FbPost
                 {
                     Message = b.Message,

                 }).ToList();
            string WbAllText = string.Concat(wbPosts.Select(n => n.Message));
            var WbWords = Regex.Split(WbAllText.ToLower(), @"\W+")
                .Where(s => s.Length > 3 && s != "байна" && s != "байгаа" && s != "байх" && s != "байсан" && s != "байдаг" && s != "shig" && s != "бгаа" && s != "чинь" && s != "юм" && s != "bgaa" && s != "болж" && s != "байж" && s != "болсон" && s != "гээд" && s != "гэсэн" && s != "минь" && s != "bgaa" && s != "shuu" && s != "https")
                .GroupBy(s => s)
                .Select(s => new Models.LargeTextModels
                {
                    word = s.Key,
                    count = s.Count()
                })
                .OrderByDescending(g => g.count).Take(10).ToList();
            ViewBag.Mounth = arr;
            ViewBag.FbWords = FbWords;
            ViewBag.TwWords = TwWords;
            ViewBag.WbWords = WbWords;
            ViewBag.NeutralTW = NeutralTW;
            ViewBag.NegativeTW = NegativeTW;
            ViewBag.PositiveTW = PositiveTW;
            ViewBag.NeutralWB = NeutralWB;
            ViewBag.NegativeWb = NegativeWb;
            ViewBag.PositiveWb = PositiveWb;
            ViewBag.NeutralFB = NeutralFB;
            ViewBag.NegativeFb = NegativeFb;
            ViewBag.PositiveFb = PositiveFb;
            ViewBag.socialAll = ViewBag.TwitterData + ViewBag.FbData;
            ViewBag.tvAll = ViewBag.WebSiteData;
            ViewBag.PercentageFb = ((ViewBag.FbData * 100) / ViewBag.socialAll);
            ViewBag.PercentageTwitter = ((ViewBag.TwitterData * 100) / ViewBag.socialAll);

            //2022-05-11 begin
            // making date list
            DateTime last = new DateTime();
            last = DateTime.Now.AddDays(-14);
            DateTime real = DateTime.Now;
            List<DateTime> dates = new List<DateTime>();
            do
            {
                dates.Add(last);
                last = last.AddDays(+1);
            } while (real.Date != last.Date);



            // Facebook Chart ///
            string queryFbPos = string.Format("select count(*) as count,Convert(date,post.UpdateTime) as date from dbo.[Facebook.Posts] post inner join dbo.[Facebook.Post.Person] person on post.ID = person.PostID and (person.IsDeleted is null or person.IsDeleted<>1) where CONVERT(date, getdate()) >= Convert(date,post.UpdateTime) and Convert(date,post.UpdateTime) > DATEADD(DD, -14, CONVERT(date, getdate())) and person.PersonID = '{0}' and person.Sentiment = 'Positive' group by Convert(date, post.UpdateTime) order by Convert(date, post.UpdateTime) desc;", id);
            string queryFbNeg = string.Format("select count(*) as count,Convert(date,post.UpdateTime) as date from dbo.[Facebook.Posts] post inner join dbo.[Facebook.Post.Person] person on post.ID = person.PostID and (person.IsDeleted is null or person.IsDeleted<>1)  where CONVERT(date, getdate()) >= Convert(date,post.UpdateTime) and Convert(date,post.UpdateTime) > DATEADD(DD, -14, CONVERT(date, getdate())) and person.PersonID = '{0}' and person.Sentiment = 'Negative' group by Convert(date, post.UpdateTime) order by Convert(date, post.UpdateTime) desc;", id);
            string queryFbNeu = string.Format("select count(*) as count,Convert(date,post.UpdateTime) as date from dbo.[Facebook.Posts] post inner join dbo.[Facebook.Post.Person] person on post.ID = person.PostID and (person.IsDeleted is null or person.IsDeleted<>1)  where CONVERT(date, getdate()) >= Convert(date,post.UpdateTime) and Convert(date,post.UpdateTime) > DATEADD(DD, -14, CONVERT(date, getdate())) and person.PersonID = '{0}' and person.Sentiment = 'Neutral' group by Convert(date, post.UpdateTime) order by Convert(date, post.UpdateTime) desc;", id);

            var resultsFbPos = monitoringDB.Database.SqlQuery<graphResult>(queryFbPos).ToList<graphResult>();
            var resultsFbNeg = monitoringDB.Database.SqlQuery<graphResult>(queryFbNeg).ToList<graphResult>();
            var resultsFbNeu = monitoringDB.Database.SqlQuery<graphResult>(queryFbNeu).ToList<graphResult>();

            string counterfbPos = "[";
            string counterfbNeg = "[";
            string counterfbNeu = "[";

            #region FbPosloop
            for (int s = 0; s < dates.Count; s++)
            {
                DateTime date = dates.ElementAt(s).Date;
                //
                if (resultsFbPos.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterfbPos == "[")
                    {
                        counterfbPos = counterfbPos + resultsFbPos.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    counterfbPos = counterfbPos + "," + resultsFbPos.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                }
                else
                {
                    if (counterfbPos == "[")
                    {
                        counterfbPos = counterfbPos + 0;

                    }
                    counterfbPos = counterfbPos + "," + 0;
                }
            }
            #endregion           
            #region FbNegloop
            for (int s = 0; s < dates.Count; s++)
            {
                DateTime date = dates.ElementAt(s).Date;
                //
                if (resultsFbNeg.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterfbNeg == "[")
                    {
                        counterfbNeg = counterfbNeg + resultsFbNeg.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    counterfbNeg = counterfbNeg + "," + resultsFbNeg.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                }
                else
                {
                    if (counterfbNeg == "[")
                    {
                        counterfbNeg = counterfbNeg + 0;
                    }
                    counterfbNeg = counterfbNeg + "," + 0;
                }
            }
            #endregion   
            #region FbNeuloop
            for (int s = 0; s < dates.Count; s++)
            {
                DateTime date = dates.ElementAt(s).Date;
                //
                if (resultsFbNeu.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterfbNeu == "[")
                    {
                        counterfbNeu = counterfbNeu + resultsFbNeu.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    counterfbNeu = counterfbNeu + "," + resultsFbNeu.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                }
                else
                {
                    if (counterfbNeu == "[")
                    {
                        counterfbNeu = counterfbNeu + 0;
                    }
                    counterfbNeu = counterfbNeu + "," + 0;
                }
            }
            #endregion 

            counterfbPos = counterfbPos + "]";
            counterfbNeg = counterfbNeg + "]";
            counterfbNeu = counterfbNeu + "]";

            ViewBag.FbPosChartCount = counterfbPos;
            ViewBag.FbNegChartCount = counterfbNeg;
            ViewBag.FbNeuChartCount = counterfbNeu;
            // End Facebook Chart //

            // Facebook Chart ///
            string queryTwPos = string.Format("select count(*) as count,Convert(date,tweet.CreatedAt) as date from dbo.[Twitter.Tweets] tweet inner join dbo.[Twitter.Tweet.Person] person on tweet.ID = person.TweetID  and (person.IsDeleted is null or person.IsDeleted<>1)  where CONVERT(date, getdate()) >= Convert(date,tweet.CreatedAt) and Convert(date,tweet.CreatedAt) > DATEADD(DD, -14, CONVERT(date, getdate())) and person.PersonID = '{0}' and person.Sentiment = 'Positive' group by Convert(date, tweet.CreatedAt) order by Convert(date, tweet.CreatedAt) desc;", id);
            string queryTwNeg = string.Format("select count(*) as count,Convert(date,tweet.CreatedAt) as date from dbo.[Twitter.Tweets] tweet inner join dbo.[Twitter.Tweet.Person] person on tweet.ID = person.TweetID  and (person.IsDeleted is null or person.IsDeleted<>1)  where CONVERT(date, getdate()) >= Convert(date,tweet.CreatedAt) and Convert(date,tweet.CreatedAt) > DATEADD(DD, -14, CONVERT(date, getdate())) and person.PersonID = '{0}' and person.Sentiment = 'Negative' group by Convert(date, tweet.CreatedAt) order by Convert(date, tweet.CreatedAt) desc;", id);
            string queryTwNeu = string.Format("select count(*) as count,Convert(date,tweet.CreatedAt) as date from dbo.[Twitter.Tweets] tweet inner join dbo.[Twitter.Tweet.Person] person on tweet.ID = person.TweetID  and (person.IsDeleted is null or person.IsDeleted<>1)  where CONVERT(date, getdate()) >= Convert(date,tweet.CreatedAt) and Convert(date,tweet.CreatedAt) > DATEADD(DD, -14, CONVERT(date, getdate())) and person.PersonID = '{0}' and person.Sentiment = 'Neutral' group by Convert(date, tweet.CreatedAt) order by Convert(date, tweet.CreatedAt) desc;", id);

            var resultsTwPos = monitoringDB.Database.SqlQuery<graphResult>(queryTwPos).ToList<graphResult>();
            var resultsTwNeg = monitoringDB.Database.SqlQuery<graphResult>(queryTwNeg).ToList<graphResult>();
            var resultsTwNeu = monitoringDB.Database.SqlQuery<graphResult>(queryTwNeu).ToList<graphResult>();

            string counterTwPos = "[";
            string counterTwNeg = "[";
            string counterTwNeu = "[";

            #region TwPosloop
            for (int s = 0; s < dates.Count; s++)
            {
                DateTime date = dates.ElementAt(s).Date;
                //
                if (resultsTwPos.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterTwPos == "[")
                    {
                        counterTwPos = counterTwPos + resultsTwPos.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    counterTwPos = counterTwPos + "," + resultsTwPos.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                }
                else
                {
                    if (counterTwPos == "[")
                    {
                        counterTwPos = counterTwPos + 0;

                    }
                    counterTwPos = counterTwPos + "," + 0;
                }
            }
            #endregion           
            #region TwNegloop
            for (int s = 0; s < dates.Count; s++)
            {
                DateTime date = dates.ElementAt(s).Date;
                //
                if (resultsTwNeg.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterTwNeg == "[")
                    {
                        counterTwNeg = counterTwNeg + resultsTwNeg.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    counterTwNeg = counterTwNeg + "," + resultsTwNeg.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                }
                else
                {
                    if (counterTwNeg == "[")
                    {
                        counterTwNeg = counterTwNeg + 0;
                    }
                    counterTwNeg = counterTwNeg + "," + 0;
                }
            }
            #endregion   
            #region TwNeuloop
            for (int s = 0; s < dates.Count; s++)
            {
                DateTime date = dates.ElementAt(s).Date;
                //
                if (resultsTwNeu.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterTwNeu == "[")
                    {
                        counterTwNeu = counterTwNeu + resultsTwNeu.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    counterTwNeu = counterTwNeu + "," + resultsTwNeu.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                }
                else
                {
                    if (counterTwNeu == "[")
                    {
                        counterTwNeu = counterTwNeu + 0;
                    }
                    counterTwNeu = counterTwNeu + "," + 0;
                }
            }
            #endregion 

            counterTwPos = counterTwPos + "]";
            counterTwNeg = counterTwNeg + "]";
            counterTwNeu = counterTwNeu + "]";

            ViewBag.TwPosChartCount = counterTwPos;
            ViewBag.TwNegChartCount = counterTwNeg;
            ViewBag.TwNeuChartCount = counterTwNeu;
            // End Twitter Chart //

            // Facebook Chart ///
            string queryWebPos = string.Format("select count(*) as count,Convert(date,post.Date) as date from dbo.[WebSite.Posts] post inner join dbo.[Website.Post.Person] person on post.ID = person.PostID  and (person.IsDeleted is null or person.IsDeleted<>1)  where CONVERT(date, getdate()) >= Convert(date,post.Date) and Convert(date,post.Date) > DATEADD(DD, -14, CONVERT(date, getdate())) and person.PersonID = '{0}' and person.Sentiment = 'Positive' group by Convert(date, post.Date) order by Convert(date, post.Date) desc;", id);
            string queryWebNeg = string.Format("select count(*) as count,Convert(date,post.Date) as date from dbo.[WebSite.Posts] post inner join dbo.[Website.Post.Person] person on post.ID = person.PostID  and (person.IsDeleted is null or person.IsDeleted<>1)  where CONVERT(date, getdate()) >= Convert(date,post.Date) and Convert(date,post.Date) > DATEADD(DD, -14, CONVERT(date, getdate())) and person.PersonID = '{0}' and person.Sentiment = 'Negative' group by Convert(date, post.Date) order by Convert(date, post.Date) desc;", id);
            string queryWebNeu = string.Format("select count(*) as count,Convert(date,post.Date) as date from dbo.[WebSite.Posts] post inner join dbo.[Website.Post.Person] person on post.ID = person.PostID  and (person.IsDeleted is null or person.IsDeleted<>1) where CONVERT(date, getdate()) >= Convert(date,post.Date) and Convert(date,post.Date) > DATEADD(DD, -14, CONVERT(date, getdate())) and person.PersonID = '{0}' and person.Sentiment = 'Neutral' group by Convert(date, post.Date) order by Convert(date, post.Date) desc;", id);

            var resultsWebPos = monitoringDB.Database.SqlQuery<graphResult>(queryWebPos).ToList<graphResult>();
            var resultsWebNeg = monitoringDB.Database.SqlQuery<graphResult>(queryWebNeg).ToList<graphResult>();
            var resultsWebNeu = monitoringDB.Database.SqlQuery<graphResult>(queryWebNeu).ToList<graphResult>();

            string counterWebPos = "[";
            string counterWebNeg = "[";
            string counterWebNeu = "[";

            #region WebPosloop
            for (int s = 0; s < dates.Count; s++)
            {
                DateTime date = dates.ElementAt(s).Date;
                //
                if (resultsWebPos.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterWebPos == "[")
                    {
                        counterWebPos = counterWebPos + resultsWebPos.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    counterWebPos = counterWebPos + "," + resultsWebPos.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                }
                else
                {
                    if (counterWebPos == "[")
                    {
                        counterWebPos = counterWebPos + 0;

                    }
                    counterWebPos = counterWebPos + "," + 0;
                }
            }
            #endregion           
            #region WebNegloop
            for (int s = 0; s < dates.Count; s++)
            {
                DateTime date = dates.ElementAt(s).Date;
                //
                if (resultsWebNeu.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterWebNeg == "[")
                    {
                        counterWebNeg = counterWebNeg + resultsWebNeu.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    counterWebNeg = counterWebNeg + "," + resultsWebNeu.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                }
                else
                {
                    if (counterWebNeg == "[")
                    {
                        counterWebNeg = counterWebNeg + 0;
                    }
                    counterWebNeg = counterWebNeg + "," + 0;
                }
            }
            #endregion   
            #region WebNeuloop
            for (int s = 0; s < dates.Count; s++)
            {
                DateTime date = dates.ElementAt(s).Date;
                //
                if (resultsWebNeu.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterWebNeu == "[")
                    {
                        counterWebNeu = counterWebNeu + resultsWebNeu.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    counterWebNeu = counterWebNeu + "," + resultsWebNeu.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                }
                else
                {
                    if (counterWebNeu == "[")
                    {
                        counterWebNeu = counterWebNeu + 0;
                    }
                    counterWebNeu = counterWebNeu + "," + 0;
                }
            }
            #endregion 

            counterWebPos = counterWebPos + "]";
            counterWebNeg = counterWebNeg + "]";
            counterWebNeu = counterWebNeu + "]";

            ViewBag.WebPosChartCount = counterWebPos;
            ViewBag.WebNegChartCount = counterWebNeg;
            ViewBag.WebNeuChartCount = counterWebNeu;
            //2022-05-11 bulgaa end


            return View(currentUser);
        }
        public ActionResult AnalysisOfMonthlyData(string ObjectId)
        {
            if (User.Identity.IsAuthenticated == false)
            {
                return Redirect("/Account/Login");
            }
            MonitoringEntities monitoringDB = new MonitoringEntities();
            /* var id = "e7aca6b9-5973-49dd-9c9d-1e93222b759a";*/
            var id = "8902d568-4f77-4874-97b8-f7dd7bc6d655";
            DateTime LastWeek = DateTime.Now.AddDays(-30);
            ViewBag.FbData =
              (from a in monitoringDB.Facebook_Post_Person
               join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
               join c in monitoringDB.System_Person on a.PersonID equals c.ID
               where c.ID.ToString() == id && a.IsDeleted != true && b.UpdateTime >= LastWeek
               orderby b.UpdateTime descending
               select new Models.FbPost
               {
                   ID = b.ID,
               }).Count();

            ViewBag.TwitterData =
               (from tweetPerson in monitoringDB.Twitter_Tweet_Person
                join tweet in monitoringDB.Twitter_Tweets on tweetPerson.TweetID equals tweet.ID
                join sysPerson in monitoringDB.System_Person on tweetPerson.PersonID equals sysPerson.ID
                join tweetUser in monitoringDB.Twitter_User_Details on tweet.TwitterUserID equals tweetUser.TwitterUserID
                where sysPerson.ID.ToString() == id && tweetPerson.IsDeleted != true && tweet.CreatedAt >= LastWeek
                orderby tweet.CreatedAt descending
                select new Models.Twitter
                {
                    ID = tweet.ID,
                }).Count();

            ViewBag.WebSiteData =
              (from a in monitoringDB.Website_Post_Person
               join b in monitoringDB.WebSite_Posts on a.PostID equals b.ID
               join c in monitoringDB.System_Person on a.PersonID equals c.ID
               where c.ID.ToString() == id && a.IsDeleted != true && b.DateTime >= LastWeek
               orderby b.Date descending
               select new Models.WebSitePost
               {
                   ID = b.ID,
               }).Count();
            var currentUser = monitoringDB.System_Person.Where(a => a.ID.ToString().Equals(id)).Select(n => new Models.SystemUserModels
            {
                ID = n.ID.ToString(),
                Name = n.Name,
                Surename = n.Surename,
                Tittlename = n.Tittlename,
                Picture = n.Picture,
                Description = n.Description,
                FacebookAccount = n.FacebookAccount.ToString(),
                TwitterAccount = n.TwitterAccount.ToString()
            }).FirstOrDefault();


            //Sentiment Web
            var PositiveWb =
             (from a in monitoringDB.Website_Post_Person
              join b in monitoringDB.WebSite_Posts on a.PostID equals b.ID
              join c in monitoringDB.System_Person on a.PersonID equals c.ID
              where c.ID.ToString() == id && a.IsDeleted != true && b.DateTime >= LastWeek && a.Sentiment.ToString() == "Positive"

              select new Models.SystemUserModels
              {
                  ID = b.ID.ToString(),
              }).Count();

            var NegativeWb =
                (from a in monitoringDB.Website_Post_Person
                 join b in monitoringDB.WebSite_Posts on a.PostID equals b.ID
                 join c in monitoringDB.System_Person on a.PersonID equals c.ID
                 where c.ID.ToString() == id && a.IsDeleted != true && b.DateTime >= LastWeek && a.Sentiment.ToString() == "Negative"

                 select new Models.SystemUserModels
                 {
                     ID = b.ID.ToString(),
                 }).Count();
            var NeutralWB =
               (from a in monitoringDB.Website_Post_Person
                join b in monitoringDB.WebSite_Posts on a.PostID equals b.ID
                join c in monitoringDB.System_Person on a.PersonID equals c.ID
                where c.ID.ToString() == id && a.IsDeleted != true && b.DateTime >= LastWeek && a.Sentiment.ToString() == "Neutral"

                select new Models.SystemUserModels
                {
                    ID = b.ID.ToString(),
                }).Count();
            //Facebook posts
            var PositiveFb =
              (from a in monitoringDB.Facebook_Post_Person
               join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
               join c in monitoringDB.System_Person on a.PersonID equals c.ID
               where c.ID.ToString() == id && a.IsDeleted != true && b.UpdateTime >= LastWeek && a.Sentiment.ToString() == "Positive"

               select new Models.SystemUserModels
               {
                   ID = a.ID.ToString(),
               }).Count();

            var NegativeFb =
               (from a in monitoringDB.Facebook_Post_Person
                join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
                join c in monitoringDB.System_Person on a.PersonID equals c.ID
                where c.ID.ToString() == id && a.IsDeleted != true && b.UpdateTime >= LastWeek && a.Sentiment.ToString() == "Negative"

                select new Models.SystemUserModels
                {
                    ID = a.ID.ToString(),
                }).Count();
            var NeutralFB =
              (from a in monitoringDB.Facebook_Post_Person
               join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
               join c in monitoringDB.System_Person on a.PersonID equals c.ID
               where c.ID.ToString() == id && a.IsDeleted != true && b.UpdateTime >= LastWeek && a.Sentiment.ToString() == "Neutral"

               select new Models.SystemUserModels
               {
                   ID = a.ID.ToString(),
               }).Count();
            //Twitter posts
            var PositiveTW =
              (from tweetPerson in monitoringDB.Twitter_Tweet_Person
               join tweet in monitoringDB.Twitter_Tweets on tweetPerson.TweetID equals tweet.ID
               join sysPerson in monitoringDB.System_Person on tweetPerson.PersonID equals sysPerson.ID
               join tweetUser in monitoringDB.Twitter_User_Details on tweet.TwitterUserID equals tweetUser.TwitterUserID
               where sysPerson.ID.ToString() == id && tweetPerson.IsDeleted != true && tweet.CreatedAt >= LastWeek && tweetPerson.Sentiment.ToString() == "Positive"
               select new Models.SystemUserModels
               {
                   ID = tweetPerson.ID.ToString(),
               }).Count();
            var NegativeTW =
               (from tweetPerson in monitoringDB.Twitter_Tweet_Person
                join tweet in monitoringDB.Twitter_Tweets on tweetPerson.TweetID equals tweet.ID
                join sysPerson in monitoringDB.System_Person on tweetPerson.PersonID equals sysPerson.ID
                join tweetUser in monitoringDB.Twitter_User_Details on tweet.TwitterUserID equals tweetUser.TwitterUserID
                where sysPerson.ID.ToString() == id && tweetPerson.IsDeleted != true && tweet.CreatedAt >= LastWeek && tweetPerson.Sentiment.ToString() == "Negative"
                select new Models.SystemUserModels
                {
                    ID = tweetPerson.ID.ToString(),
                }).Count();
            var NeutralTW =
             (from tweetPerson in monitoringDB.Twitter_Tweet_Person
              join tweet in monitoringDB.Twitter_Tweets on tweetPerson.TweetID equals tweet.ID
              join sysPerson in monitoringDB.System_Person on tweetPerson.PersonID equals sysPerson.ID
              join tweetUser in monitoringDB.Twitter_User_Details on tweet.TwitterUserID equals tweetUser.TwitterUserID
              where sysPerson.ID.ToString() == id && tweetPerson.IsDeleted != true && tweet.CreatedAt >= LastWeek && tweetPerson.Sentiment.ToString() == "Neutral"
              select new Models.SystemUserModels
              {
                  ID = tweetPerson.ID.ToString(),
              }).Count();
            var ss = monitoringDB.Twitter_Tweet_Person.Where(a => a.PersonID.ToString().Equals(id) && a.Sentiment.ToString() == "Neutral").Select(n => new Models.SystemUserModels
            {
                ID = n.ID.ToString()
            }).Count();


            int[] arr = new int[8];
            var i = 0;
            while (i < 8)
            {
                DateTime testDate = DateTime.Now.AddMonths(-i);
                DateTime testDate2 = DateTime.Now.AddMonths(-(i + 1));
                arr[i] =
                     (from postPerson in monitoringDB.Facebook_Post_Person
                      join a in monitoringDB.Facebook_Posts on postPerson.PostID equals a.ID
                      where postPerson.PersonID.ToString().Equals(id)
                      where a.UpdateTime < testDate
                      where a.UpdateTime > testDate2
                      select new Models.FbPost
                      {
                          ID = a.ID,

                      }).Count();
                i++;
            }
            var fbPosts =
                (from a in monitoringDB.Facebook_Post_Person
                 join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
                 join c in monitoringDB.System_Person on a.PersonID equals c.ID
                 where (c.ID.ToString() == id) && a.IsDeleted != true && b.UpdateTime >= LastWeek
                 select new Models.FbPost
                 {
                     Message = b.Message,

                 }).ToList();
            string AllTextFb = string.Concat(fbPosts.Select(n => n.Message));
            var FbWords = Regex.Split(AllTextFb.ToLower(), @"\W+")
                .Where(s => s.Length > 3 && s != "байна" && s != "байгаа" && s != "байх" && s != "байсан" && s != "байдаг" && s != "shig" && s != "бгаа" && s != "чинь" && s != "юм" && s != "bgaa" && s != "болж" && s != "байж" && s != "болсон" && s != "гээд" && s != "гэсэн" && s != "минь" && s != "bgaa" && s != "shuu" && s != "https")
                .GroupBy(s => s)
                .Select(s => new Models.LargeTextModels
                {
                    word = s.Key,
                    count = s.Count()
                })
                .OrderByDescending(g => g.count).Take(10).ToList();

            var twPosts =
                (from a in monitoringDB.Facebook_Post_Person
                 join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
                 join c in monitoringDB.System_Person on a.PersonID equals c.ID
                 where (c.ID.ToString() == id) && a.IsDeleted != true && b.UpdateTime >= LastWeek
                 select new Models.FbPost
                 {
                     Message = b.Message,

                 }).ToList();
            string TwAllText = string.Concat(twPosts.Select(n => n.Message));
            var TwWords = Regex.Split(TwAllText.ToLower(), @"\W+")
                .Where(s => s.Length > 3 && s != "байна" && s != "байгаа" && s != "байх" && s != "байсан" && s != "байдаг" && s != "shig" && s != "бгаа" && s != "чинь" && s != "юм" && s != "bgaa" && s != "болж" && s != "байж" && s != "болсон" && s != "гээд" && s != "гэсэн" && s != "минь" && s != "bgaa" && s != "shuu" && s != "https")
                .GroupBy(s => s)
                .Select(s => new Models.LargeTextModels
                {
                    word = s.Key,
                    count = s.Count()
                })
                .OrderByDescending(g => g.count).Take(10).ToList();
            var wbPosts =
                (from a in monitoringDB.Facebook_Post_Person
                 join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
                 join c in monitoringDB.System_Person on a.PersonID equals c.ID
                 where (c.ID.ToString() == id) && a.IsDeleted != true && b.UpdateTime >= LastWeek
                 select new Models.FbPost
                 {
                     Message = b.Message,

                 }).ToList();
            string WbAllText = string.Concat(wbPosts.Select(n => n.Message));
            var WbWords = Regex.Split(WbAllText.ToLower(), @"\W+")
                .Where(s => s.Length > 3 && s != "байна" && s != "байгаа" && s != "байх" && s != "байсан" && s != "байдаг" && s != "shig" && s != "бгаа" && s != "чинь" && s != "юм" && s != "bgaa" && s != "болж" && s != "байж" && s != "болсон" && s != "гээд" && s != "гэсэн" && s != "минь" && s != "bgaa" && s != "shuu" && s != "https")
                .GroupBy(s => s)
                .Select(s => new Models.LargeTextModels
                {
                    word = s.Key,
                    count = s.Count()
                })
                .OrderByDescending(g => g.count).Take(10).ToList();
            ViewBag.Mounth = arr;
            ViewBag.FbWords = FbWords;
            ViewBag.TwWords = TwWords;
            ViewBag.WbWords = WbWords;
            ViewBag.NeutralTW = NeutralTW;
            ViewBag.NegativeTW = NegativeTW;
            ViewBag.PositiveTW = PositiveTW;
            ViewBag.NeutralWB = NeutralWB;
            ViewBag.NegativeWb = NegativeWb;
            ViewBag.PositiveWb = PositiveWb;
            ViewBag.NeutralFB = NeutralFB;
            ViewBag.NegativeFb = NegativeFb;
            ViewBag.PositiveFb = PositiveFb;
            ViewBag.socialAll = ViewBag.TwitterData + ViewBag.FbData;
            ViewBag.tvAll = ViewBag.WebSiteData;
            ViewBag.PercentageFb = ((ViewBag.FbData * 100) / ViewBag.socialAll);
            ViewBag.PercentageTwitter = ((ViewBag.TwitterData * 100) / ViewBag.socialAll);

            //2022-05-11 begin
            // making date list
            DateTime last = new DateTime();
            last = DateTime.Now.AddDays(-30);
            DateTime real = DateTime.Now;
            List<DateTime> dates = new List<DateTime>();
            do
            {
                dates.Add(last);
                last = last.AddDays(+1);
            } while (real.Date != last.Date);



            // Facebook Chart ///
            string queryFbPos = string.Format("select count(*) as count,Convert(date,post.UpdateTime) as date from dbo.[Facebook.Posts] post inner join dbo.[Facebook.Post.Person] person on post.ID = person.PostID and (person.IsDeleted is null or person.IsDeleted<>1) where CONVERT(date, getdate()) >= Convert(date,post.UpdateTime) and Convert(date,post.UpdateTime) > DATEADD(DD, -30, CONVERT(date, getdate())) and person.PersonID = '{0}' and person.Sentiment = 'Positive' group by Convert(date, post.UpdateTime) order by Convert(date, post.UpdateTime) desc;", id);
            string queryFbNeg = string.Format("select count(*) as count,Convert(date,post.UpdateTime) as date from dbo.[Facebook.Posts] post inner join dbo.[Facebook.Post.Person] person on post.ID = person.PostID and (person.IsDeleted is null or person.IsDeleted<>1)  where CONVERT(date, getdate()) >= Convert(date,post.UpdateTime) and Convert(date,post.UpdateTime) > DATEADD(DD, -30, CONVERT(date, getdate())) and person.PersonID = '{0}' and person.Sentiment = 'Negative' group by Convert(date, post.UpdateTime) order by Convert(date, post.UpdateTime) desc;", id);
            string queryFbNeu = string.Format("select count(*) as count,Convert(date,post.UpdateTime) as date from dbo.[Facebook.Posts] post inner join dbo.[Facebook.Post.Person] person on post.ID = person.PostID and (person.IsDeleted is null or person.IsDeleted<>1)  where CONVERT(date, getdate()) >= Convert(date,post.UpdateTime) and Convert(date,post.UpdateTime) > DATEADD(DD, -30, CONVERT(date, getdate())) and person.PersonID = '{0}' and person.Sentiment = 'Neutral' group by Convert(date, post.UpdateTime) order by Convert(date, post.UpdateTime) desc;", id);

            var resultsFbPos = monitoringDB.Database.SqlQuery<graphResult>(queryFbPos).ToList<graphResult>();
            var resultsFbNeg = monitoringDB.Database.SqlQuery<graphResult>(queryFbNeg).ToList<graphResult>();
            var resultsFbNeu = monitoringDB.Database.SqlQuery<graphResult>(queryFbNeu).ToList<graphResult>();

            string counterfbPos = "[";
            string counterfbNeg = "[";
            string counterfbNeu = "[";

            #region FbPosloop
            for (int s = 0; s < dates.Count; s++)
            {
                DateTime date = dates.ElementAt(s).Date;
                //
                if (resultsFbPos.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterfbPos == "[")
                    {
                        counterfbPos = counterfbPos + resultsFbPos.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    counterfbPos = counterfbPos + "," + resultsFbPos.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                }
                else
                {
                    if (counterfbPos == "[")
                    {
                        counterfbPos = counterfbPos + 0;

                    }
                    counterfbPos = counterfbPos + "," + 0;
                }
            }
            #endregion           
            #region FbNegloop
            for (int s = 0; s < dates.Count; s++)
            {
                DateTime date = dates.ElementAt(s).Date;
                //
                if (resultsFbNeg.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterfbNeg == "[")
                    {
                        counterfbNeg = counterfbNeg + resultsFbNeg.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    counterfbNeg = counterfbNeg + "," + resultsFbNeg.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                }
                else
                {
                    if (counterfbNeg == "[")
                    {
                        counterfbNeg = counterfbNeg + 0;
                    }
                    counterfbNeg = counterfbNeg + "," + 0;
                }
            }
            #endregion   
            #region FbNeuloop
            for (int s = 0; s < dates.Count; s++)
            {
                DateTime date = dates.ElementAt(s).Date;
                //
                if (resultsFbNeu.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterfbNeu == "[")
                    {
                        counterfbNeu = counterfbNeu + resultsFbNeu.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    counterfbNeu = counterfbNeu + "," + resultsFbNeu.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                }
                else
                {
                    if (counterfbNeu == "[")
                    {
                        counterfbNeu = counterfbNeu + 0;
                    }
                    counterfbNeu = counterfbNeu + "," + 0;
                }
            }
            #endregion 

            counterfbPos = counterfbPos + "]";
            counterfbNeg = counterfbNeg + "]";
            counterfbNeu = counterfbNeu + "]";

            ViewBag.FbPosChartCount = counterfbPos;
            ViewBag.FbNegChartCount = counterfbNeg;
            ViewBag.FbNeuChartCount = counterfbNeu;
            // End Facebook Chart //

            // Facebook Chart ///
            string queryTwPos = string.Format("select count(*) as count,Convert(date,tweet.CreatedAt) as date from dbo.[Twitter.Tweets] tweet inner join dbo.[Twitter.Tweet.Person] person on tweet.ID = person.TweetID  and (person.IsDeleted is null or person.IsDeleted<>1)  where CONVERT(date, getdate()) >= Convert(date,tweet.CreatedAt) and Convert(date,tweet.CreatedAt) > DATEADD(DD, -30, CONVERT(date, getdate())) and person.PersonID = '{0}' and person.Sentiment = 'Positive' group by Convert(date, tweet.CreatedAt) order by Convert(date, tweet.CreatedAt) desc;", id);
            string queryTwNeg = string.Format("select count(*) as count,Convert(date,tweet.CreatedAt) as date from dbo.[Twitter.Tweets] tweet inner join dbo.[Twitter.Tweet.Person] person on tweet.ID = person.TweetID  and (person.IsDeleted is null or person.IsDeleted<>1)  where CONVERT(date, getdate()) >= Convert(date,tweet.CreatedAt) and Convert(date,tweet.CreatedAt) > DATEADD(DD, -30, CONVERT(date, getdate())) and person.PersonID = '{0}' and person.Sentiment = 'Negative' group by Convert(date, tweet.CreatedAt) order by Convert(date, tweet.CreatedAt) desc;", id);
            string queryTwNeu = string.Format("select count(*) as count,Convert(date,tweet.CreatedAt) as date from dbo.[Twitter.Tweets] tweet inner join dbo.[Twitter.Tweet.Person] person on tweet.ID = person.TweetID  and (person.IsDeleted is null or person.IsDeleted<>1)  where CONVERT(date, getdate()) >= Convert(date,tweet.CreatedAt) and Convert(date,tweet.CreatedAt) > DATEADD(DD, -30, CONVERT(date, getdate())) and person.PersonID = '{0}' and person.Sentiment = 'Neutral' group by Convert(date, tweet.CreatedAt) order by Convert(date, tweet.CreatedAt) desc;", id);

            var resultsTwPos = monitoringDB.Database.SqlQuery<graphResult>(queryTwPos).ToList<graphResult>();
            var resultsTwNeg = monitoringDB.Database.SqlQuery<graphResult>(queryTwNeg).ToList<graphResult>();
            var resultsTwNeu = monitoringDB.Database.SqlQuery<graphResult>(queryTwNeu).ToList<graphResult>();

            string counterTwPos = "[";
            string counterTwNeg = "[";
            string counterTwNeu = "[";

            #region TwPosloop
            for (int s = 0; s < dates.Count; s++)
            {
                DateTime date = dates.ElementAt(s).Date;
                //
                if (resultsTwPos.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterTwPos == "[")
                    {
                        counterTwPos = counterTwPos + resultsTwPos.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    counterTwPos = counterTwPos + "," + resultsTwPos.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                }
                else
                {
                    if (counterTwPos == "[")
                    {
                        counterTwPos = counterTwPos + 0;

                    }
                    counterTwPos = counterTwPos + "," + 0;
                }
            }
            #endregion           
            #region TwNegloop
            for (int s = 0; s < dates.Count; s++)
            {
                DateTime date = dates.ElementAt(s).Date;
                //
                if (resultsTwNeg.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterTwNeg == "[")
                    {
                        counterTwNeg = counterTwNeg + resultsTwNeg.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    counterTwNeg = counterTwNeg + "," + resultsTwNeg.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                }
                else
                {
                    if (counterTwNeg == "[")
                    {
                        counterTwNeg = counterTwNeg + 0;
                    }
                    counterTwNeg = counterTwNeg + "," + 0;
                }
            }
            #endregion   
            #region TwNeuloop
            for (int s = 0; s < dates.Count; s++)
            {
                DateTime date = dates.ElementAt(s).Date;
                //
                if (resultsTwNeu.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterTwNeu == "[")
                    {
                        counterTwNeu = counterTwNeu + resultsTwNeu.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    counterTwNeu = counterTwNeu + "," + resultsTwNeu.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                }
                else
                {
                    if (counterTwNeu == "[")
                    {
                        counterTwNeu = counterTwNeu + 0;
                    }
                    counterTwNeu = counterTwNeu + "," + 0;
                }
            }
            #endregion 

            counterTwPos = counterTwPos + "]";
            counterTwNeg = counterTwNeg + "]";
            counterTwNeu = counterTwNeu + "]";

            ViewBag.TwPosChartCount = counterTwPos;
            ViewBag.TwNegChartCount = counterTwNeg;
            ViewBag.TwNeuChartCount = counterTwNeu;
            // End Twitter Chart //

            // Facebook Chart ///
            string queryWebPos = string.Format("select count(*) as count,Convert(date,post.Date) as date from dbo.[WebSite.Posts] post inner join dbo.[Website.Post.Person] person on post.ID = person.PostID  and (person.IsDeleted is null or person.IsDeleted<>1)  where CONVERT(date, getdate()) >= Convert(date,post.Date) and Convert(date,post.Date) > DATEADD(DD, -30, CONVERT(date, getdate())) and person.PersonID = '{0}' and person.Sentiment = 'Positive' group by Convert(date, post.Date) order by Convert(date, post.Date) desc;", id);
            string queryWebNeg = string.Format("select count(*) as count,Convert(date,post.Date) as date from dbo.[WebSite.Posts] post inner join dbo.[Website.Post.Person] person on post.ID = person.PostID  and (person.IsDeleted is null or person.IsDeleted<>1)  where CONVERT(date, getdate()) >= Convert(date,post.Date) and Convert(date,post.Date) > DATEADD(DD, -30, CONVERT(date, getdate())) and person.PersonID = '{0}' and person.Sentiment = 'Negative' group by Convert(date, post.Date) order by Convert(date, post.Date) desc;", id);
            string queryWebNeu = string.Format("select count(*) as count,Convert(date,post.Date) as date from dbo.[WebSite.Posts] post inner join dbo.[Website.Post.Person] person on post.ID = person.PostID  and (person.IsDeleted is null or person.IsDeleted<>1) where CONVERT(date, getdate()) >= Convert(date,post.Date) and Convert(date,post.Date) > DATEADD(DD, -30, CONVERT(date, getdate())) and person.PersonID = '{0}' and person.Sentiment = 'Neutral' group by Convert(date, post.Date) order by Convert(date, post.Date) desc;", id);

            var resultsWebPos = monitoringDB.Database.SqlQuery<graphResult>(queryWebPos).ToList<graphResult>();
            var resultsWebNeg = monitoringDB.Database.SqlQuery<graphResult>(queryWebNeg).ToList<graphResult>();
            var resultsWebNeu = monitoringDB.Database.SqlQuery<graphResult>(queryWebNeu).ToList<graphResult>();

            string counterWebPos = "[";
            string counterWebNeg = "[";
            string counterWebNeu = "[";

            #region WebPosloop
            for (int s = 0; s < dates.Count; s++)
            {
                DateTime date = dates.ElementAt(s).Date;
                //
                if (resultsWebPos.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterWebPos == "[")
                    {
                        counterWebPos = counterWebPos + resultsWebPos.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    counterWebPos = counterWebPos + "," + resultsWebPos.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                }
                else
                {
                    if (counterWebPos == "[")
                    {
                        counterWebPos = counterWebPos + 0;

                    }
                    counterWebPos = counterWebPos + "," + 0;
                }
            }
            #endregion           
            #region WebNegloop
            for (int s = 0; s < dates.Count; s++)
            {
                DateTime date = dates.ElementAt(s).Date;
                //
                if (resultsWebNeu.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterWebNeg == "[")
                    {
                        counterWebNeg = counterWebNeg + resultsWebNeu.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    counterWebNeg = counterWebNeg + "," + resultsWebNeu.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                }
                else
                {
                    if (counterWebNeg == "[")
                    {
                        counterWebNeg = counterWebNeg + 0;
                    }
                    counterWebNeg = counterWebNeg + "," + 0;
                }
            }
            #endregion   
            #region WebNeuloop
            for (int s = 0; s < dates.Count; s++)
            {
                DateTime date = dates.ElementAt(s).Date;
                //
                if (resultsWebNeu.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterWebNeu == "[")
                    {
                        counterWebNeu = counterWebNeu + resultsWebNeu.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    counterWebNeu = counterWebNeu + "," + resultsWebNeu.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                }
                else
                {
                    if (counterWebNeu == "[")
                    {
                        counterWebNeu = counterWebNeu + 0;
                    }
                    counterWebNeu = counterWebNeu + "," + 0;
                }
            }
            #endregion 

            counterWebPos = counterWebPos + "]";
            counterWebNeg = counterWebNeg + "]";
            counterWebNeu = counterWebNeu + "]";

            ViewBag.WebPosChartCount = counterWebPos;
            ViewBag.WebNegChartCount = counterWebNeg;
            ViewBag.WebNeuChartCount = counterWebNeu;
            //2022-05-11 bulgaa end


            return View(currentUser);
        }

        //2022-05-11 bulgaa begin Analyze main graph
        public ActionResult AnalyzeMainGraph(string id)
        {

            MonitoringEntities monitoringDB = new MonitoringEntities();
            // making date list
            DateTime last = new DateTime();
            last = DateTime.Now.AddDays(-30);
            DateTime real = DateTime.Now;
            List<DateTime> dates = new List<DateTime>();
            do
            {
                dates.Add(last);
                last = last.AddDays(+1);
            } while (real.Date != last.Date);


            string queryFb = string.Format("select count(*) as count,Convert(date,post.UpdateTime) as date from dbo.[Facebook.Posts] post inner join dbo.[Facebook.Post.Person] person on post.ID = person.PostID and  (person.IsDeleted is null or person.IsDeleted<>1) where CONVERT(date, getdate()) >= Convert(date,post.UpdateTime) and Convert(date,post.UpdateTime) > DATEADD(DD, -30, CONVERT(date, getdate())) and person.PersonID = '{0}' group by Convert(date, post.UpdateTime) order by Convert(date, post.UpdateTime) desc;", id);
            string queryTw = string.Format("select count(*) as count,Convert(date,post.CreatedAt) as date from dbo.[Twitter.Tweets] post inner join dbo.[Twitter.Tweet.Person] person on post.ID = person.TweetID and  (person.IsDeleted is null or person.IsDeleted<>1)  where CONVERT(date, getdate()) >= Convert(date,post.CreatedAt) and Convert(date,post.CreatedAt) > DATEADD(DD, -30, CONVERT(date, getdate())) and person.PersonID = '{0}' group by Convert(date, post.CreatedAt) order by Convert(date, post.CreatedAt) desc;", id);
            string queryWb = string.Format("select count(*) as count,Convert(date,post.Date) as date from dbo.[WebSite.Posts] post inner join dbo.[Website.Post.Person] person on post.ID = person.PostID  and  (person.IsDeleted is null or person.IsDeleted<>1) where CONVERT(date, getdate()) >= Convert(date,post.Date) and Convert(date,post.Date) > DATEADD(DD, -30, CONVERT(date, getdate())) and person.PersonID = '{0}' group by Convert(date, post.Date) order by Convert(date, post.Date) desc;", id);

            var resultsFb = monitoringDB.Database.SqlQuery<graphResult>(queryFb).ToList<graphResult>();
            var resultsTw = monitoringDB.Database.SqlQuery<graphResult>(queryTw).ToList<graphResult>();
            var resultsWb = monitoringDB.Database.SqlQuery<graphResult>(queryWb).ToList<graphResult>();

            string counterfb = "[";
            string countertw = "[";
            string counterweb = "[";
            var fbCount = new List<int>();
            var twCount = new List<int>();
            var webCount = new List<int>();
            #region facebookloop
            for (int i = 0; i < dates.Count; i++)
            {
                DateTime date = dates.ElementAt(i).Date;
                //
                if (resultsFb.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterfb == "[")
                    {
                        counterfb = counterfb + resultsFb.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    counterfb = counterfb + "," + resultsFb.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    fbCount.Add(resultsFb.Where(p => p.date.Equals(date)).FirstOrDefault().count);
                }
                else
                {
                    if (counterfb == "[")
                    {
                        counterfb = counterfb + 0;

                    }
                    counterfb = counterfb + "," + 0;
                    fbCount.Add(0);

                }
            }
            #endregion
            #region twitterloop
            for (int i = 0; i < dates.Count; i++)
            {
                DateTime date = dates.ElementAt(i).Date;
                //
                if (resultsTw.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (countertw == "[")
                    {
                        countertw = countertw + resultsTw.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    countertw = countertw + "," + resultsTw.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    twCount.Add(resultsTw.Where(p => p.date.Equals(date)).FirstOrDefault().count);
                }
                else
                {
                    if (countertw == "[")
                    {
                        countertw = countertw + 0;

                    }
                    countertw = countertw + "," + 0;
                    twCount.Add(0);
                }
            }
            #endregion
            #region webloop
            for (int i = 0; i < dates.Count; i++)
            {
                DateTime date = dates.ElementAt(i).Date;
                //
                if (resultsWb.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterweb == "[")
                    {
                        counterweb = counterweb + resultsWb.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    webCount.Add(resultsWb.Where(p => p.date.Equals(date)).FirstOrDefault().count);
                }
                else
                {
                    if (counterweb == "[")
                    {
                        counterweb = counterweb + 0;

                    }
                    counterweb = counterweb + "," + 0;
                    webCount.Add(0);
                }
            }
            #endregion

            counterfb = counterfb + "]";
            countertw = countertw + "]";
            counterweb = counterweb + "]";

            ViewBag.facebookChartCount = counterfb;
            ViewBag.twitterChartCount = countertw;
            ViewBag.websiteChartCount = counterweb;
            ViewBag.fbCount = fbCount;
            ViewBag.twCount = twCount;
            ViewBag.webCount = webCount;
            // Facebook Chart ///
            string queryFbPos = string.Format("select count(*) as count,Convert(date,post.UpdateTime) as date from dbo.[Facebook.Posts] post inner join dbo.[Facebook.Post.Person] person on post.ID = person.PostID and (person.IsDeleted is null or person.IsDeleted<>1) where CONVERT(date, getdate()) >= Convert(date,post.UpdateTime) and Convert(date,post.UpdateTime) > DATEADD(DD, -30, CONVERT(date, getdate())) and person.PersonID = '{0}' and person.Sentiment = 'Positive' group by Convert(date, post.UpdateTime) order by Convert(date, post.UpdateTime) desc;", id);
            string queryFbNeg = string.Format("select count(*) as count,Convert(date,post.UpdateTime) as date from dbo.[Facebook.Posts] post inner join dbo.[Facebook.Post.Person] person on post.ID = person.PostID and (person.IsDeleted is null or person.IsDeleted<>1)  where CONVERT(date, getdate()) >= Convert(date,post.UpdateTime) and Convert(date,post.UpdateTime) > DATEADD(DD, -30, CONVERT(date, getdate())) and person.PersonID = '{0}' and person.Sentiment = 'Negative' group by Convert(date, post.UpdateTime) order by Convert(date, post.UpdateTime) desc;", id);
            string queryFbNeu = string.Format("select count(*) as count,Convert(date,post.UpdateTime) as date from dbo.[Facebook.Posts] post inner join dbo.[Facebook.Post.Person] person on post.ID = person.PostID and (person.IsDeleted is null or person.IsDeleted<>1)  where CONVERT(date, getdate()) >= Convert(date,post.UpdateTime) and Convert(date,post.UpdateTime) > DATEADD(DD, -30, CONVERT(date, getdate())) and person.PersonID = '{0}' and person.Sentiment = 'Neutral' group by Convert(date, post.UpdateTime) order by Convert(date, post.UpdateTime) desc;", id);

            var resultsFbPos = monitoringDB.Database.SqlQuery<graphResult>(queryFbPos).ToList<graphResult>();
            var resultsFbNeg = monitoringDB.Database.SqlQuery<graphResult>(queryFbNeg).ToList<graphResult>();
            var resultsFbNeu = monitoringDB.Database.SqlQuery<graphResult>(queryFbNeu).ToList<graphResult>();

            string counterfbPos = "[";
            string counterfbNeg = "[";
            string counterfbNeu = "[";

            #region FbPosloop
            for (int i = 0; i < dates.Count; i++)
            {
                DateTime date = dates.ElementAt(i).Date;
                //
                if (resultsFbPos.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterfbPos == "[")
                    {
                        counterfbPos = counterfbPos + resultsFbPos.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    counterfbPos = counterfbPos + "," + resultsFbPos.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                }
                else
                {
                    if (counterfbPos == "[")
                    {
                        counterfbPos = counterfbPos + 0;

                    }
                    counterfbPos = counterfbPos + "," + 0;
                }
            }
            #endregion           
            #region FbNegloop
            for (int i = 0; i < dates.Count; i++)
            {
                DateTime date = dates.ElementAt(i).Date;
                //
                if (resultsFbNeg.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterfbNeg == "[")
                    {
                        counterfbNeg = counterfbNeg + resultsFbNeg.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    counterfbNeg = counterfbNeg + "," + resultsFbNeg.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                }
                else
                {
                    if (counterfbNeg == "[")
                    {
                        counterfbNeg = counterfbNeg + 0;
                    }
                    counterfbNeg = counterfbNeg + "," + 0;
                }
            }
            #endregion   
            #region FbNeuloop
            for (int i = 0; i < dates.Count; i++)
            {
                DateTime date = dates.ElementAt(i).Date;
                //
                if (resultsFbNeu.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterfbNeu == "[")
                    {
                        counterfbNeu = counterfbNeu + resultsFbNeu.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    counterfbNeu = counterfbNeu + "," + resultsFbNeu.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                }
                else
                {
                    if (counterfbNeu == "[")
                    {
                        counterfbNeu = counterfbNeu + 0;
                    }
                    counterfbNeu = counterfbNeu + "," + 0;
                }
            }
            #endregion 

            counterfbPos = counterfbPos + "]";
            counterfbNeg = counterfbNeg + "]";
            counterfbNeu = counterfbNeu + "]";

            ViewBag.FbPosChartCount = counterfbPos;
            ViewBag.FbNegChartCount = counterfbNeg;
            ViewBag.FbNeuChartCount = counterfbNeu;
            // End Facebook Chart //

            // Facebook Chart ///
            string queryTwPos = string.Format("select count(*) as count,Convert(date,tweet.CreatedAt) as date from dbo.[Twitter.Tweets] tweet inner join dbo.[Twitter.Tweet.Person] person on tweet.ID = person.TweetID  and (person.IsDeleted is null or person.IsDeleted<>1)  where CONVERT(date, getdate()) >= Convert(date,tweet.CreatedAt) and Convert(date,tweet.CreatedAt) > DATEADD(DD, -30, CONVERT(date, getdate())) and person.PersonID = '{0}' and person.Sentiment = 'Positive' group by Convert(date, tweet.CreatedAt) order by Convert(date, tweet.CreatedAt) desc;", id);
            string queryTwNeg = string.Format("select count(*) as count,Convert(date,tweet.CreatedAt) as date from dbo.[Twitter.Tweets] tweet inner join dbo.[Twitter.Tweet.Person] person on tweet.ID = person.TweetID  and (person.IsDeleted is null or person.IsDeleted<>1)  where CONVERT(date, getdate()) >= Convert(date,tweet.CreatedAt) and Convert(date,tweet.CreatedAt) > DATEADD(DD, -30, CONVERT(date, getdate())) and person.PersonID = '{0}' and person.Sentiment = 'Negative' group by Convert(date, tweet.CreatedAt) order by Convert(date, tweet.CreatedAt) desc;", id);
            string queryTwNeu = string.Format("select count(*) as count,Convert(date,tweet.CreatedAt) as date from dbo.[Twitter.Tweets] tweet inner join dbo.[Twitter.Tweet.Person] person on tweet.ID = person.TweetID  and (person.IsDeleted is null or person.IsDeleted<>1)  where CONVERT(date, getdate()) >= Convert(date,tweet.CreatedAt) and Convert(date,tweet.CreatedAt) > DATEADD(DD, -30, CONVERT(date, getdate())) and person.PersonID = '{0}' and person.Sentiment = 'Neutral' group by Convert(date, tweet.CreatedAt) order by Convert(date, tweet.CreatedAt) desc;", id);

            var resultsTwPos = monitoringDB.Database.SqlQuery<graphResult>(queryTwPos).ToList<graphResult>();
            var resultsTwNeg = monitoringDB.Database.SqlQuery<graphResult>(queryTwNeg).ToList<graphResult>();
            var resultsTwNeu = monitoringDB.Database.SqlQuery<graphResult>(queryTwNeu).ToList<graphResult>();

            string counterTwPos = "[";
            string counterTwNeg = "[";
            string counterTwNeu = "[";

            #region TwPosloop
            for (int i = 0; i < dates.Count; i++)
            {
                DateTime date = dates.ElementAt(i).Date;
                //
                if (resultsTwPos.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterTwPos == "[")
                    {
                        counterTwPos = counterTwPos + resultsTwPos.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    counterTwPos = counterTwPos + "," + resultsTwPos.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                }
                else
                {
                    if (counterTwPos == "[")
                    {
                        counterTwPos = counterTwPos + 0;

                    }
                    counterTwPos = counterTwPos + "," + 0;
                }
            }
            #endregion           
            #region TwNegloop
            for (int i = 0; i < dates.Count; i++)
            {
                DateTime date = dates.ElementAt(i).Date;
                //
                if (resultsTwNeg.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterTwNeg == "[")
                    {
                        counterTwNeg = counterTwNeg + resultsTwNeg.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    counterTwNeg = counterTwNeg + "," + resultsTwNeg.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                }
                else
                {
                    if (counterTwNeg == "[")
                    {
                        counterTwNeg = counterTwNeg + 0;
                    }
                    counterTwNeg = counterTwNeg + "," + 0;
                }
            }
            #endregion   
            #region TwNeuloop
            for (int i = 0; i < dates.Count; i++)
            {
                DateTime date = dates.ElementAt(i).Date;
                //
                if (resultsTwNeu.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterTwNeu == "[")
                    {
                        counterTwNeu = counterTwNeu + resultsTwNeu.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    counterTwNeu = counterTwNeu + "," + resultsTwNeu.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                }
                else
                {
                    if (counterTwNeu == "[")
                    {
                        counterTwNeu = counterTwNeu + 0;
                    }
                    counterTwNeu = counterTwNeu + "," + 0;
                }
            }
            #endregion 

            counterTwPos = counterTwPos + "]";
            counterTwNeg = counterTwNeg + "]";
            counterTwNeu = counterTwNeu + "]";

            ViewBag.TwPosChartCount = counterTwPos;
            ViewBag.TwNegChartCount = counterTwNeg;
            ViewBag.TwNeuChartCount = counterTwNeu;
            // End Twitter Chart //

            // Facebook Chart ///
            string queryWebPos = string.Format("select count(*) as count,Convert(date,post.Date) as date from dbo.[WebSite.Posts] post inner join dbo.[Website.Post.Person] person on post.ID = person.PostID  and (person.IsDeleted is null or person.IsDeleted<>1)  where CONVERT(date, getdate()) >= Convert(date,post.Date) and Convert(date,post.Date) > DATEADD(DD, -30, CONVERT(date, getdate())) and person.PersonID = '{0}' and person.Sentiment = 'Positive' group by Convert(date, post.Date) order by Convert(date, post.Date) desc;", id);
            string queryWebNeg = string.Format("select count(*) as count,Convert(date,post.Date) as date from dbo.[WebSite.Posts] post inner join dbo.[Website.Post.Person] person on post.ID = person.PostID  and (person.IsDeleted is null or person.IsDeleted<>1)  where CONVERT(date, getdate()) >= Convert(date,post.Date) and Convert(date,post.Date) > DATEADD(DD, -30, CONVERT(date, getdate())) and person.PersonID = '{0}' and person.Sentiment = 'Negative' group by Convert(date, post.Date) order by Convert(date, post.Date) desc;", id);
            string queryWebNeu = string.Format("select count(*) as count,Convert(date,post.Date) as date from dbo.[WebSite.Posts] post inner join dbo.[Website.Post.Person] person on post.ID = person.PostID  and (person.IsDeleted is null or person.IsDeleted<>1) where CONVERT(date, getdate()) >= Convert(date,post.Date) and Convert(date,post.Date) > DATEADD(DD, -30, CONVERT(date, getdate())) and person.PersonID = '{0}' and person.Sentiment = 'Neutral' group by Convert(date, post.Date) order by Convert(date, post.Date) desc;", id);

            var resultsWebPos = monitoringDB.Database.SqlQuery<graphResult>(queryWebPos).ToList<graphResult>();
            var resultsWebNeg = monitoringDB.Database.SqlQuery<graphResult>(queryWebNeg).ToList<graphResult>();
            var resultsWebNeu = monitoringDB.Database.SqlQuery<graphResult>(queryWebNeu).ToList<graphResult>();

            string counterWebPos = "[";
            string counterWebNeg = "[";
            string counterWebNeu = "[";

            #region WebPosloop
            for (int i = 0; i < dates.Count; i++)
            {
                DateTime date = dates.ElementAt(i).Date;
                //
                if (resultsWebPos.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterWebPos == "[")
                    {
                        counterWebPos = counterWebPos + resultsWebPos.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    counterWebPos = counterWebPos + "," + resultsWebPos.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                }
                else
                {
                    if (counterWebPos == "[")
                    {
                        counterWebPos = counterWebPos + 0;

                    }
                    counterWebPos = counterWebPos + "," + 0;
                }
            }
            #endregion           
            #region WebNegloop
            for (int i = 0; i < dates.Count; i++)
            {
                DateTime date = dates.ElementAt(i).Date;
                //
                if (resultsWebNeu.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterWebNeg == "[")
                    {
                        counterWebNeg = counterWebNeg + resultsWebNeu.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    counterWebNeg = counterWebNeg + "," + resultsWebNeu.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                }
                else
                {
                    if (counterWebNeg == "[")
                    {
                        counterWebNeg = counterWebNeg + 0;
                    }
                    counterWebNeg = counterWebNeg + "," + 0;
                }
            }
            #endregion   
            #region WebNeuloop
            for (int i = 0; i < dates.Count; i++)
            {
                DateTime date = dates.ElementAt(i).Date;
                //
                if (resultsWebNeu.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterWebNeu == "[")
                    {
                        counterWebNeu = counterWebNeu + resultsWebNeu.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    counterWebNeu = counterWebNeu + "," + resultsWebNeu.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                }
                else
                {
                    if (counterWebNeu == "[")
                    {
                        counterWebNeu = counterWebNeu + 0;
                    }
                    counterWebNeu = counterWebNeu + "," + 0;
                }
            }
            #endregion 

            counterWebPos = counterWebPos + "]";
            counterWebNeg = counterWebNeg + "]";
            counterWebNeu = counterWebNeu + "]";

            ViewBag.WebPosChartCount = counterWebPos;
            ViewBag.WebNegChartCount = counterWebNeg;
            ViewBag.WebNeuChartCount = counterWebNeu;
            return PartialView();
        }
        //2022-05-11 bulgaa end Analyze main graph
        public string FacebookDataSend(string fbImage, string fbDescription, string fbLink, string personID, string postID, string sentiment, string fromName, string UserID, string DateTime, string FileName)
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();
            Guid id = Guid.NewGuid();
            Facebook_Posts FBnew = new Facebook_Posts();
            if (fbImage != "")
            {
                FBnew.ID = id;
                FBnew.PostID = postID;
                FBnew.FromName = fromName;
                FBnew.Message = fbDescription;
                FBnew.UpdateTime = Convert.ToDateTime(DateTime);
                FBnew.PermalinkUrl = fbLink;
                FBnew.Type = "Posts";
                fbImage = fbImage.Replace("data:image/png;base64,", "").Replace("data:image/jpeg;base64,", "").Replace("data:image/jpg;base64,", "");
                byte[] data = System.Convert.FromBase64String(fbImage);
                string folderPath = Server.MapPath("~/Content/Images/FBimage/");  //Create a Folder in your Root directory on your solution.
                string fileName = Guid.NewGuid() + ".jpg";
                string imagePath = folderPath + fileName;
                MemoryStream ms = new MemoryStream(data);
                System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
                img.Save(imagePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                FBnew.FullPicture = "Content/Images/FBimage/" + fileName;
            }
            else
            {
                /*FBnew.FullPicture = "Content/Images/noImage.jpg";*/
                FBnew.ID = id;
                FBnew.PostID = postID;
                FBnew.FromName = fromName;
                FBnew.Message = fbDescription;
                FBnew.UpdateTime = Convert.ToDateTime(DateTime);
                FBnew.PermalinkUrl = fbLink;
                FBnew.Type = "Posts";
            }

            monitoringDB.Facebook_Posts.Add(FBnew);
            monitoringDB.SaveChanges();
            Facebook_Post_Person FBperson = new Facebook_Post_Person();
            FBperson.ID = Guid.NewGuid();
            FBperson.PostID = id;
            FBperson.PersonID = new Guid(UserID);
            FBperson.Sentiment = sentiment;
            FBperson.IsDeleted = null;
            monitoringDB.Facebook_Post_Person.Add(FBperson);
            monitoringDB.SaveChanges();


            return "succeed";
        }

        public string TwitterDataSend(string twitterDescription, string twitterLink, string sentiment, string statusID, string personID, string screenName, string Twimage, string UserID, DateTime CreatedAt)
        {

            MonitoringEntities monitoringDB = new MonitoringEntities();

            Twitter_User_Details TWdetails = new Twitter_User_Details();
            Twitter_Users TWUsers = new Twitter_Users();

            Guid twitterUserID = Guid.NewGuid();
            Guid twitterDetailsUserID = Guid.NewGuid();
            var userDetails = monitoringDB.Twitter_User_Details.Where(s => s.ScreenName == screenName).ToList();
            /*   if (userDetails.Count == 0)
               {
   */
            TWdetails.ID = twitterDetailsUserID;
            TWdetails.TwitterUserID = twitterUserID;
            TWdetails.ScreenName = screenName;
            monitoringDB.Twitter_User_Details.Add(TWdetails);

            TWUsers.ID = twitterUserID;
            TWUsers.UserID = "https://twitter.com/" + screenName;
            TWUsers.UserName = screenName;
            monitoringDB.Twitter_User_Details.GetType();
            /*  }
              else
              {
                  twitterUserID = userDetails.FirstOrDefault().ID;
              }*/

            Guid id = Guid.NewGuid();
            Twitter_Tweets TWnew = new Twitter_Tweets();




            TWnew.ID = id;
            TWnew.CreatedAt = Convert.ToDateTime(CreatedAt);
            TWnew.StatusID = statusID;
            TWnew.RetweetCount = 1;
            TWnew.TweetID = statusID;
            TWnew.Retweeted = false;
            TWnew.FullText = twitterDescription;
            TWnew.Language = "Russian";
            TWnew.IsManual = "True";
            TWnew.Text = twitterDescription;
            TWnew.Truncated = false;
            TWnew.RegisteredDate = DateTime.Now;

            Twimage = Twimage.Replace("data:image/png;base64,", "").Replace("data:image/jpeg;base64,", "").Replace("data:image/jpg;base64,", "");

            byte[] data = System.Convert.FromBase64String(Twimage);
            string folderPath = Server.MapPath("~/Content/Images/Twimage/");
            string fileName = Guid.NewGuid() + ".jpg";
            string imagePath = folderPath + fileName;
            MemoryStream ms = new MemoryStream(data);
            System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
            img.Save(imagePath, System.Drawing.Imaging.ImageFormat.Jpeg);

            TWnew.TwFullPicture = "Content/Images/Twimage/" + fileName;
            TWnew.UrlEntity1 = twitterLink;
            TWnew.TwitterUserID = twitterUserID;
            TWnew.ScreenName = screenName;
            monitoringDB.Twitter_Tweets.Add(TWnew);

            Twitter_Tweet_Person TWperson = new Twitter_Tweet_Person();
            TWperson.ID = Guid.NewGuid();
            TWperson.TweetID = id;
            TWperson.PersonID = new Guid(UserID);
            TWperson.Sentiment = sentiment;
            TWperson.IsDeleted = null;
            monitoringDB.Twitter_Tweet_Person.Add(TWperson);
            monitoringDB.SaveChanges();

            return "succeed";
        }

        public string WebDataSend(string webDescription, string Wbimage, string webTitle, string webLink, string sentiment, string Reporter, string personID, string UserID, DateTime WebCreatAt, string webCoverImageLink)
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();
            Guid id = Guid.NewGuid();
            WebSite_Posts WBnew = new WebSite_Posts();
            WBnew.ID = id;
            WBnew.Link = webLink;
            WBnew.Reporter = Reporter;
            WBnew.Title = webTitle;
            WBnew.Date = DateTime.Now;
            WBnew.DateTime = WebCreatAt;
            WBnew.Url = webLink;
            WBnew.Body = webDescription;
            WBnew.Text = webDescription;

            Wbimage = Wbimage.Replace("data:image/png;base64,", "").Replace("data:image/jpeg;base64,", "").Replace("data:image/jpg;base64,", "");

            byte[] data = Convert.FromBase64String(Wbimage);
            string folderPath = Server.MapPath("~/Content/Images/Wbimage/");  //Create a Folder in your Root directory on your solution.
            string fileName = Guid.NewGuid() + ".jpg";
            string imagePath = folderPath + fileName;
            MemoryStream ms = new MemoryStream(data);
            System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
            img.Save(imagePath, System.Drawing.Imaging.ImageFormat.Jpeg);

            WBnew.WbFullPicture = "Content/Images/Wbimage/" + fileName;
            WBnew.CoverUrl = webCoverImageLink;
            monitoringDB.WebSite_Posts.Add(WBnew);
            Website_Post_Person WBperson = new Website_Post_Person();
            WBperson.ID = Guid.NewGuid();
            WBperson.PostID = id;
            WBperson.PersonID = new Guid(UserID);
            WBperson.Sentiment = sentiment;
            WBperson.IsDeleted = false;
            monitoringDB.Website_Post_Person.Add(WBperson);
            monitoringDB.SaveChanges();
            return "succeed";
        }
        public ActionResult DynamicMenu()
        {
            var id = new Guid(User.Identity.GetUserId());
            MonitoringEntities monitoringDB = new MonitoringEntities();
            /*            var model = (from a in monitoringDB.UserMenus
                                    where a.userID == id
                                    orderby a.Level descending
                                    select new Models.MenuModels
                                    {
                                        ID = a.ID,
                                        Level = a.Level,
                                        Name = a.name,
                                        parentID = a.parentID,
                                        childMenu = a.childMenu,
                                    }).ToList();*/
            var model = (from a in monitoringDB.System_Person
                         where a.userID == id
                         orderby a.Level descending
                         select new Models.MenuModels
                         {
                             ID = a.ID,
                             Level = a.Level,
                             Name = a.Name,
                             parentID = a.ParentID,
                             childMenu = a.childMenu,
                         }).ToList();
            return PartialView(model);
        }
        public ActionResult RenderMenu()
        {
            /*            var id = new Guid(User.Identity.GetUserId());
                        MonitoringEntities monitoringDB = new MonitoringEntities();
                        var model = (from a in monitoringDB.UserMenus
                                     where a.userID == id
                                     orderby a.Level descending
                                     select new Models.MenuModels
                                     {
                                         ID = a.ID,
                                         Level = a.Level,
                                         Name = a.name,
                                         parentID = a.parentID,
                                         childMenu = a.childMenu,
                                     }).ToList();
                        return PartialView("DynamicMenu", model);*/
            var id = new Guid(User.Identity.GetUserId());
            MonitoringEntities monitoringDB = new MonitoringEntities();
            var model = (from a in monitoringDB.System_Person
                         where a.userID == id
                         orderby a.Level descending
                         select new Models.MenuModels
                         {
                             ID = a.ID,
                             Level = a.Level,
                             Name = a.Name,
                             parentID = a.ParentID,
                             childMenu = a.childMenu,
                         }).ToList();
            return PartialView("DynamicMenu", model);
        }

        /* public ActionResult WebDataSend(string webImage, string webDescription, string webTitle, string webLink, string sentiment, string Reporter, string personID)
         {
             MonitoringEntities monitoringDB = new MonitoringEntities();
             Guid id = Guid.NewGuid();
             WebSite_Posts WBnew = new WebSite_Posts();
             WBnew.ID = id;
             WBnew.Link = webLink;
             WBnew.Reporter = Reporter;
             WBnew.Title = webTitle;
             WBnew.Text = webDescription;
             WBnew.Date = DateTime.Now;
             WBnew.DateTime = DateTime.UtcNow;
             WBnew.Url = webLink;
             WBnew.Body = webDescription;
             monitoringDB.WebSite_Posts.Add(WBnew);
             Website_Post_Person WBperson = new Website_Post_Person();
             WBperson.ID = new Guid();
             WBperson.PostID = id;
             WBperson.PersonID = new Guid(personID);
             WBperson.Sentiment = sentiment;
             WBperson.IsDeleted = false;
             monitoringDB.Website_Post_Person.Add(WBperson);
             monitoringDB.SaveChanges();
             return View();
         }*/

        public string NewMenu(string menuLevel, string Name, string Email, string PhoneNumber, string Website, string Picture)
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();
            /*SystemUserModels NM = new SystemUserModels();*/
            System_Person NM = new System_Person();




            var b = monitoringDB.System_Person.Where(a => a.ID.ToString().Equals(menuLevel)).Select(a => a.Level).FirstOrDefault();
            NM.ID = Guid.NewGuid();
            NM.Name = Name;
            NM.Email = Email;
            NM.Picture = Picture;
            NM.PhoneNumber = PhoneNumber;
            NM.Website = Website;
            if (menuLevel == "0")
            {
                NM.ParentID = null;
                NM.Level = 0;
            }
            else
            {
                NM.ParentID = new Guid(menuLevel);
                NM.Level = b + 1;
                System_Person NM2 = monitoringDB.System_Person.Where(a => a.ID.ToString().Equals(menuLevel)).FirstOrDefault();
                NM2.childMenu = 1;
            }
            var CurrentUserIDnew = (User.Identity.GetUserId().ToString());
            NM.userID = new Guid(CurrentUserIDnew);
            monitoringDB.System_Person.Add(NM);
            /*System_Person NM2 = monitoringDB.System_Person.Where(a => a.ID.ToString().Equals(menuLevel)).FirstOrDefault();*/
            /*            NM2.childMenu = 1;*/
            monitoringDB.SaveChanges();
            return "succeed";
        }

        public ActionResult MenuDataTake()
        {
            var id = new Guid(User.Identity.GetUserId());
            MonitoringEntities monitoringDB = new MonitoringEntities();
            var data = (from a in monitoringDB.System_Person
                        where a.userID == id && a.Level < 2
                        orderby a.Level descending
                        select new Models.MenuModels
                        {
                            ID = a.ID,
                            Name = a.Name,
                        }).ToList();
            return Json(new { success = true, data }, JsonRequestBehavior.AllowGet);
        }
        /*javhaa analyze, fb, twitter, web post save end*/
        /*bulgaa Only Website Data 2022-04-26 Begin*/
        public ActionResult SmartNewsCusOnlyListWebData(string UserID, string counter)
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();
            int skip = Int32.Parse(counter) * 6;
            var WebData = (from a in monitoringDB.Website_Post_Person
                           join b in monitoringDB.WebSite_Posts on a.PostID equals b.ID
                           join c in monitoringDB.System_Person on a.PersonID equals c.ID
                           where c.ID.ToString() == UserID && a.IsDeleted != true
                           orderby b.Date descending
                           select new Models.WebSitePost
                           {
                               ID = b.ID,
                               Link = b.Link,
                               Title = b.Title,
                               Name = c.Name,
                               Text = b.Text,
                               PicturePerson = c.Picture,
                               Sentiment = a.Sentiment,
                               Url = b.Url,
                               Body = b.Body.Substring(0, 300) + "...",
                               Reporter = b.Reporter,
                               CoverUrl = b.CoverUrl,
                               WbFullPicture = b.WbFullPicture,
                               //bulgaa 2022-05-05 
                               /*StringUrl = b.Url.Substring(6, 17) + "...",*/
                               StringUrl = b.Url.Replace(@"//", "").Substring(0, 22) + "...",
                               DateTime = b.DateTime
                           }).Skip(skip).Take(6).ToList();
            return PartialView(WebData);
        }
        /*bulgaa Only Website Data 2022-04-26 End*/

        /*bulgaa Only Facebook Data 2022-04-26 Begin*/
        public ActionResult SmartNewsCusOnlyListFacebookData(string UserID, string counter)
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();
            int skip = Int32.Parse(counter) * 6;
            var FbData = (from a in monitoringDB.Facebook_Post_Person
                          join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
                          join c in monitoringDB.System_Person on a.PersonID equals c.ID
                          where c.ID.ToString() == UserID && a.IsDeleted != true
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
                          }).Skip(skip).Take(6).ToList();
            return PartialView(FbData);
        }
        /*bulgaa Only Facebook Data 2022-04-26 End*/

        /*bulgaa Only Twitter Data 2022-04-26 Begin*/
        public ActionResult SmartNewsCusOnlyListTwitterData(string UserID, string counter)
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();
            int skip = Int32.Parse(counter) * 6;
            var TwData = (from a in monitoringDB.Twitter_Tweet_Person
                          join tweet in monitoringDB.Twitter_Tweets on a.TweetID equals tweet.ID
                          join c in monitoringDB.System_Person on a.PersonID equals c.ID
                          join user in monitoringDB.Twitter_User_Details on tweet.TwitterUserID equals user.TwitterUserID
                          where c.ID.ToString() == UserID && a.IsDeleted != true
                          orderby tweet.CreatedAt descending
                          select new Models.Twitter
                          {
                              ID = tweet.ID,
                              TwitterUserID = tweet.TwitterUserID,
                              TweetID = tweet.TweetID,
                              CreatedAt = tweet.CreatedAt,
                              ScreenName = user.ScreenName,
                              UrlEntity1 = tweet.UrlEntity1,
                              Source = tweet.Source,
                              TwFullPicture = tweet.TwFullPicture,
                              StatusID = tweet.StatusID,
                              UserID = tweet.UserID,
                              FullText = tweet.FullText,
                              Sentiment = a.Sentiment,
                          }).Skip(skip).Take(6).ToList();
            return PartialView(TwData);
        }
        /*bulgaa Only Twitter Data 2022-04-26 End*/

        public ActionResult Comment()
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();
            var comments = (from comment in monitoringDB.Facebook_Post_Comments
                            orderby comment.RegisteredDate descending
                            select new Models.FbComment
                            {
                                ID = comment.ID,
                                CommentID = comment.CommentID,
                                FromName = comment.FromName,
                                Message = comment.Message,
                                CreateTime = comment.CreateTime
                            }).Take(1).ToList();
            if (Session["latestComment"] != null)
            {
                if (Session["latestComment"].ToString() != comments.FirstOrDefault().ID.ToString())
                {
                    Session["latestComment"] = comments.FirstOrDefault().ID.ToString();
                    ViewBag.Comments = comments;
                }
                else
                {
                    ViewBag.Comments = null;
                }
            }
            else
            {
                Session["latestComment"] = comments.FirstOrDefault().ID.ToString();
                ViewBag.Comments = comments;
            }
            return PartialView();
        }
        public ActionResult Tweets()
        {
            ViewBag.Message = "Ухаалаг мэдээ";
            MonitoringEntities monitoringDB = new MonitoringEntities();

            var tweets =
               (from tweet in monitoringDB.Twitter_Tweets
                join user in monitoringDB.Twitter_User_Details on tweet.TwitterUserID equals user.TwitterUserID
                orderby tweet.RegisteredDate descending
                select new Models.Twitter
                {
                    ScreenName = user.ScreenName,
                    StatusID = tweet.StatusID,
                    Text = tweet.Text,
                    Name = user.Name,
                    ID = tweet.ID
                    //SortOrder = b.SortOrder,
                    //PicturePerson = c.Picture,
                }).Take(1).ToList();
            String ID = tweets.FirstOrDefault().ID.ToString();

            if (Session["latestTweet"] != null)
            {
                if (Session["latestTweet"].ToString() != ID)
                {
                    Session["latestTweet"] = tweets.FirstOrDefault().ID.ToString();
                    ViewBag.twitters = tweets;
                }
                else
                {
                    ViewBag.twitters = null;
                }
            }
            else
            {
                Session["latestTweet"] = tweets.FirstOrDefault().ID.ToString();
                ViewBag.twitters = tweets;
            }
            return PartialView();
        }

        public ActionResult FbLoadCus(string id)
        {
            ViewBag.Message = "Ухаалаг мэдээ";
            MonitoringEntities monitoringDB = new MonitoringEntities();
            int FbLastID = 0;

            ViewBag.FbData =
                (from a in monitoringDB.Facebook_Post_Person
                 join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
                 join c in monitoringDB.System_Person on a.PersonID equals c.ID
                 where c.ID.ToString() == id && a.IsDeleted != true
                 orderby b.UpdateTime descending

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
                     Name = c.Name,
                     Picture = b.Picture,
                     Icon = b.Icon,
                     ObjectID = b.ObjectID,
                     ParentID = b.ParentID,
                     PerName = b.Name
                 }).Take(200).ToList();

            foreach (var post in ViewBag.FbData)
            {
                FbLastID++;
                if (FbLastID == 1)
                {
                    ViewBag.FbLastID = post.ID;

                }
            }
            return PartialView();
        }
        public ActionResult WebSiteLoadCus(string id)
        {
            ViewBag.Message = "Ухаалаг мэдээ";
            MonitoringEntities monitoringDB = new MonitoringEntities();

            int WebLastID = 0;
            ViewBag.WebSiteData =
            (from a in monitoringDB.Website_Post_Person
             join b in monitoringDB.WebSite_Posts on a.PostID equals b.ID
             join c in monitoringDB.System_Person on a.PersonID equals c.ID
             where c.ID.ToString() == id && a.IsDeleted != true
             orderby b.Date descending
             select new Models.WebSitePost
             {
                 ID = b.ID,
                 Link = b.Link,
                 Title = b.Title,
                 Text = b.Text
             }).Take(200).ToList();

            foreach (var post in ViewBag.WebSiteData)
            {
                WebLastID++;
                if (WebLastID == 1)
                {
                    ViewBag.WebLastID = post.ID;
                }
            }
            return PartialView();
        }

        public ActionResult TwitterLoadCus(string id)
        {
            ViewBag.Message = "Ухаалаг мэдээ";
            MonitoringEntities monitoringDB = new MonitoringEntities();

            int TwitterLastID = 0;
            ViewBag.TwitterData =
               (from a in monitoringDB.Twitter_Tweet_Person
                join tweet in monitoringDB.Twitter_Tweets on a.TweetID equals tweet.ID
                join c in monitoringDB.System_Person on a.PersonID equals c.ID
                join user in monitoringDB.Twitter_User_Details on tweet.TwitterUserID equals user.TwitterUserID
                where c.ID.ToString() == id && a.IsDeleted != true
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
                    Verified = user.Verified
                }).Take(200).ToList();

            foreach (var post in ViewBag.TwitterData)
            {
                TwitterLastID++;
                if (TwitterLastID == 1)
                {
                    ViewBag.TwitterLastID = post.ID;
                }
            }
            return PartialView();
        }

        public ActionResult FbLastIDCus(string PostID, string id)
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();
            ViewBag.FbLastID = PostID;
            ViewBag.FbLastData =
                (from a in monitoringDB.Facebook_Post_Person
                 join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
                 join c in monitoringDB.System_Person on a.PersonID equals c.ID
                 where c.ID.ToString() == id && a.IsDeleted != true
                 orderby b.UpdateTime descending
                 select new Models.FbPost
                 {
                     ID = b.ID,

                 }).Take(200).ToList();

            if (ViewBag.FbLastID == "")
            {
                return Content("ok");
            }
            else
            {
                if (ViewBag.FbLastID == ViewBag.FbLastData[0].ID.ToString())
                {
                    return Content("ok");
                }
            }
            return Content("no");
        }

        public ActionResult TwitterLastIDCus(string PostID, string id)
        {

            MonitoringEntities monitoringDB = new MonitoringEntities();
            ViewBag.TwitterLastID = PostID;
            ViewBag.TwitterLastData =
                (from a in monitoringDB.Twitter_Tweet_Person
                 join b in monitoringDB.Twitter_Tweets on a.TweetID equals b.ID
                 join c in monitoringDB.System_Person on a.PersonID equals c.ID
                 where c.ID.ToString() == id && a.IsDeleted != true
                 orderby b.CreatedAt descending
                 select new Models.Twitter
                 {
                     ID = b.ID,

                 }).Take(200).ToList();

            if (ViewBag.TwitterLastID == "")
            {
                return Content("ok");
            }
            else
            {
                if (ViewBag.TwitterLastID == ViewBag.TwitterLastData[0].ID.ToString())
                {
                    return Content("ok");
                }
            }
            return Content("no");
        }

        public ActionResult WebLastIDCus(string PostID, string id)
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();
            ViewBag.WebLastID = PostID;
            ViewBag.WebLastData =
                (from a in monitoringDB.Website_Post_Person
                 join b in monitoringDB.WebSite_Posts on a.PostID equals b.ID
                 join c in monitoringDB.System_Person on a.PersonID equals c.ID
                 where c.ID.ToString() == id
                 orderby b.Date descending
                 select new Models.WebSitePost
                 {
                     ID = b.ID,

                 }).Take(200).ToList();

            if (ViewBag.WebLastID == "")
            {
                return Content("ok");
            }
            else
            {
                if (ViewBag.WebLastID == ViewBag.WebLastData[0].ID.ToString())
                {
                    return Content("ok");
                }
            }
            return Content("no");
        }

        public IQueryable<Models.Group> GetAllGroups()
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();
            var groups = (from a in monitoringDB.System_Groups
                          where a.ParentID == null
                          select new Models.Group
                          {
                              ID = a.ID,
                              Name = a.Name,
                              ParentID = a.ParentID,
                              Level = a.Level,
                              ChildGroup = monitoringDB.System_Groups.Where(x => x.ParentID == a.ID)
                          }).ToList();
            var queryable = groups.AsQueryable();
            return queryable;
        }

        public ActionResult Group()
        {
            ViewBag.Message = "Групп";
            var model = GetAllGroups();

            MonitoringEntities monitoringDB = new MonitoringEntities();
            ViewBag.GroupCombo = (from a in monitoringDB.System_Groups
                                  select new Models.Group
                                  {
                                      ID = a.ID,
                                      Name = a.Name,
                                      ParentID = a.ParentID,
                                      Level = a.Level,
                                      ChildGroup = monitoringDB.System_Groups.Where(x => x.ParentID == a.ID)
                                  }).ToList();


            return View(model);
        }

        [HttpPost]
        public ActionResult GroupPersonList(String ID)
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();
            if (ID != "0")
            {
                //string query1 = string.Format("select a.* from [System.Person] a join  [System.Groups] b on a.GroupID = b.ID where b.ID IN ({0})", ID);
                //string query2 = string.Format("select a.* from [System.Person] a join  [System.Groups] b on a.GroupID = b.ID where b.ID IN ({0})", ID);

                //ViewBag.persons = monitoringDB.Database.SqlQuery<Person>(query).ToList<Person>();

                var person =
                (
                    from per in monitoringDB.System_Person
                    where per.GroupID.ToString() == ID
                    select per
                ).ToList();

                var personToGroup =
                (
                    from pers in monitoringDB.System_Person
                    join groupToPerson in monitoringDB.System_GroupToPerson on pers.ID equals groupToPerson.PersonID
                    where groupToPerson.GroupID.ToString() == ID
                    select pers
                ).ToList();

                var personList = person.Union(personToGroup).Distinct();
                ViewBag.persons = personList.ToList();
            }
            else
            {
                ViewBag.persons = null;
            }
            return PartialView();
        }

        //insert person//
        public ActionResult Insert(String SureName, String Name, String GroupID)
        {
            using (MonitoringEntities monitoringDB = new MonitoringEntities())
            {
                string queryIns = string.Format("INSERT INTO [System.Person] (ID,Name,Surename,GroupID) values (NEWID(),N'{0}',N'{1}','{2}');", Name, SureName, Guid.Parse(GroupID));
                var resultsIns = monitoringDB.Database.SqlQuery<Person>(queryIns).ToList<Person>();
                return Json(resultsIns, JsonRequestBehavior.AllowGet);
            }

        }
        // end insert person //


        // update person //
        public ActionResult Update(String SureName, String Name, String GroupID, String ID)
        {
            using (MonitoringEntities monitoringDB = new MonitoringEntities())
            {
                string queryUpd = string.Format("UPDATE [System.Person] SET SureName=N'{0}', Name=N'{1}', GroupID='{2}' WHERE ID = '{3}';", SureName, Name, Guid.Parse(GroupID), Guid.Parse(ID));
                var resultsUpd = monitoringDB.Database.SqlQuery<Person>(queryUpd).ToList<Person>();
                return Json(resultsUpd, JsonRequestBehavior.AllowGet);
            }
        }
        // end update person //

        // insert key //
        public ActionResult InsertKey(String Key1, String Key2, String Key3, String Key4, String Latin1, String Latin2, String Latin3, String Latin4, String Type, String PersonID)
        {
            using (MonitoringEntities monitoringDB = new MonitoringEntities())
            {
                string queryIns = string.Format("INSERT INTO [System.Keys] (ID,Key1,Key2,Key3,Key4,Lattin1,Lattin2,Lattin3,Lattin4,KeyTypeID,PersonID) values " +
                    "(NEWID(),N'{0}',N'{1}',N'{2}',N'{3}',N'{4}',N'{5}',N'{6}',N'{7}','{8}','{9}');", Key1.Trim().Replace("'", "''"), Key2.Trim().Replace("'", "''"), Key3.Trim().Replace("'", "''"), Key4.Trim().Replace("'", "''"), Latin1.Trim().Replace("'", "''"), Latin2.Trim().Replace("'", "''"), Latin3.Trim().Replace("'", "''"), Latin4.Trim().Replace("'", "''"), Guid.Parse(Type), Guid.Parse(PersonID));
                var resultsIns = monitoringDB.Database.SqlQuery<Key>(queryIns).ToList<Key>();
                return Json(resultsIns, JsonRequestBehavior.AllowGet);
            }

        }
        // end insert key//

        // update key //
        public ActionResult UpdateKey(String Key1, String Key2, String Key3, String Key4, String Latin1, String Latin2, String Latin3, String Latin4, String Type, String PersonID, String ID)
        {
            using (MonitoringEntities monitoringDB = new MonitoringEntities())
            {
                string queryUpd = string.Format("UPDATE [System.Keys] SET Key1=N'{0}', Key2=N'{1}', Key3=N'{2}'" +
                    ", Key4=N'{3}', Lattin1='{4}', Lattin2='{5}', Lattin3='{6}', Lattin4='{7}', KeyTypeID='{8}' " +
                    "WHERE ID = '{9}';", Key1, Key2, Key3, Key4, Latin1, Latin2, Latin3, Latin4, Guid.Parse(Type), Guid.Parse(ID));
                var resultsUpd = monitoringDB.Database.SqlQuery<Key>(queryUpd).ToList<Key>();
                return Json(resultsUpd, JsonRequestBehavior.AllowGet);
            }
        }
        // end update key //

        // Delete key //
        public ActionResult DeleteKey(String ID)
        {
            using (MonitoringEntities monitoringDB = new MonitoringEntities())
            {
                string queryDelete = string.Format("DELETE FROM [System.Keys] WHERE ID = '{0}';", Guid.Parse(ID));
                var resultsDel = monitoringDB.Database.SqlQuery<Key>(queryDelete).ToList<Key>();
                return Json(resultsDel, JsonRequestBehavior.AllowGet);
            }
        }
        // end delete key //

        // Delete Person //
        public ActionResult DeletePerson(String ID)
        {
            using (MonitoringEntities monitoringDB = new MonitoringEntities())
            {
                string queryDelete = string.Format("DELETE FROM [System.Person] WHERE ID = '{0}';", Guid.Parse(ID));
                var resultsDel = monitoringDB.Database.SqlQuery<Person>(queryDelete).ToList<Person>();
                return Json(resultsDel, JsonRequestBehavior.AllowGet);
            }
        }
        // end person key //

        public class graphResult
        {
            public int count { get; set; }
            public DateTime date { get; set; }
        }

        public ActionResult SmartNewsCustomer(string id, string Sentiment)
        {
            ViewBag.Message = "Ухаалаг мэдээ";
            MonitoringEntities monitoringDB = new MonitoringEntities();

            ViewBag.CusID = id;

            if (Sentiment != "All")
            {
                ViewBag.FbData =
               (from a in monitoringDB.Facebook_Post_Person
                join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
                join c in monitoringDB.System_Person on a.PersonID equals c.ID
                where c.ID.ToString() == id && a.Sentiment == Sentiment && a.IsDeleted != true
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
                }).Take(200).ToList();

                ViewBag.TwitterData =
                   (from a in monitoringDB.Twitter_Tweet_Person
                    join tweet in monitoringDB.Twitter_Tweets on a.TweetID equals tweet.ID
                    join c in monitoringDB.System_Person on a.PersonID equals c.ID
                    join user in monitoringDB.Twitter_User_Details on tweet.TwitterUserID equals user.TwitterUserID
                    where c.ID.ToString() == id && a.Sentiment == Sentiment && a.IsDeleted != true
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
                   where c.ID.ToString() == id & a.Sentiment == Sentiment
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
                where c.ID.ToString() == id && a.IsDeleted != true
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
                }).Take(200).ToList();

                ViewBag.TwitterData =
                   (from a in monitoringDB.Twitter_Tweet_Person
                    join tweet in monitoringDB.Twitter_Tweets on a.TweetID equals tweet.ID
                    join c in monitoringDB.System_Person on a.PersonID equals c.ID
                    join user in monitoringDB.Twitter_User_Details on tweet.TwitterUserID equals user.TwitterUserID
                    where c.ID.ToString() == id && a.IsDeleted != true
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
                   where c.ID.ToString() == id
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
            return View();
        }

        public ActionResult Comments()
        {
            ViewBag.Message = "Коммэнт";
            MonitoringEntities monitoringDB = new MonitoringEntities();
            ViewBag.FbCommData =
                (from a in monitoringDB.Facebook_Post_Comments
                 orderby a.CreateTime descending
                 select new Models.FbComment
                 {
                     ID = a.ID,
                     PostID = a.PostID,
                     CommentID = a.CommentID,
                     ObjectID = a.ObjectID,
                     FromID = a.FromID,
                     FromName = a.FromName,
                     Message = a.Message,
                     CreateTime = a.CreateTime,
                     PositiveCount = a.PositiveCount,
                     NegativeCount = a.NegativeCount,
                     Sentiment = a.Sentiment,
                     Sentiment_Confirmed = a.Sentiment_Confirmed,
                     Question = a.Question,
                     Likes = a.Likes
                 }).Take(200).ToList();

            return View();
        }

        public ActionResult CommentSearch(string txt)
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();
            ViewBag.FbCommData =
                (from a in monitoringDB.Facebook_Post_Comments
                 where a.FromName.Contains(txt) || a.Message.Contains(txt)

                 orderby a.CreateTime descending
                 select new Models.FbComment
                 {
                     ID = a.ID,
                     PostID = a.PostID,
                     CommentID = a.CommentID,
                     ObjectID = a.ObjectID,
                     FromID = a.FromID,
                     FromName = a.FromName,
                     Message = a.Message,
                     CreateTime = a.CreateTime,
                     PositiveCount = a.PositiveCount,
                     NegativeCount = a.NegativeCount,
                     Sentiment = a.Sentiment,
                     Sentiment_Confirmed = a.Sentiment_Confirmed,
                     Question = a.Question,
                     Likes = a.Likes
                 }).Take(200).ToList();
            return PartialView();
        }

        public ActionResult CommentSearchCus(string txt)
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();
            ViewBag.Comment =
                (from a in monitoringDB.Facebook_Post_Comments
                 where a.FromName.Contains(txt) || a.Message.Contains(txt)
                 orderby a.CreateTime descending
                 select new Models.FbComment
                 {
                     ID = a.ID,
                     PostID = a.PostID,
                     CommentID = a.CommentID,
                     ObjectID = a.ObjectID,
                     FromID = a.FromID,
                     FromName = a.FromName,
                     Message = a.Message,
                     CreateTime = a.CreateTime,
                     PositiveCount = a.PositiveCount,
                     NegativeCount = a.NegativeCount,
                     Sentiment = a.Sentiment,
                     Sentiment_Confirmed = a.Sentiment_Confirmed,
                     Question = a.Question,
                     Likes = a.Likes
                 }).ToList();
            return PartialView();
        }

        public ActionResult PersonKeyList(String ID)
        {
            if (User.Identity.IsAuthenticated == false)
            {
                return Redirect("/Account/Login");
            }
            MonitoringEntities monitoringDB = new MonitoringEntities();

            ViewBag.KeyPersonID = ID;
            ViewBag.PersonData =
            (from a in monitoringDB.System_Person
             where a.ID.ToString() == ID
             select new Models.Person
             {
                 ID = a.ID,
                 Name = a.Name,
                 SureName = a.Surename,
                 GroupID = a.GroupID

             }).ToList();

            ViewBag.PersonKeyData =
                (from a in monitoringDB.System_Keys
                 join b in monitoringDB.System_KeyTypes on a.KeyTypeID equals b.ID
                 where a.PersonID.ToString() == ID

                 select new Models.Key
                 {
                     ID = a.ID,
                     Key1 = a.Key1,
                     Key2 = a.Key2,
                     Key3 = a.Key3,
                     Key4 = a.Key4,
                     Type = a.KeyTypeID,
                     PersonID = a.PersonID,
                     GroupID = a.GroupID,
                     TypeName = b.Description,
                     Latin1 = a.Lattin1,
                     Latin3 = a.Lattin3,
                     Latin4 = a.Lattin4,
                     Latin2 = a.Lattin2
                 }).ToList();

            ViewBag.PersonKeyTypes =
                    (from a in monitoringDB.System_KeyTypes
                     orderby a.Order
                     select a).ToList<DAL.System_KeyTypes>();
            return PartialView();
        }

        public ActionResult KeyLists(String ID)
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();
            ViewBag.PersonKeyData =
                (from a in monitoringDB.System_Keys
                 join b in monitoringDB.System_KeyTypes on a.KeyTypeID equals b.ID
                 where a.PersonID.ToString() == ID

                 select new Models.Key
                 {
                     ID = a.ID,
                     Key1 = a.Key1,
                     Key2 = a.Key2,
                     Key3 = a.Key3,
                     Key4 = a.Key4,
                     Type = a.KeyTypeID,
                     PersonID = a.PersonID,
                     GroupID = a.GroupID,
                     TypeName = b.Description,
                     Latin1 = a.Lattin1,
                     Latin3 = a.Lattin3,
                     Latin4 = a.Lattin4,
                     Latin2 = a.Lattin2

                 }).ToList();
            return PartialView();
        }

        public ActionResult Rating()
        {
            if (User.Identity.IsAuthenticated == false)
            {
                return Redirect("/Account/Login");
            }

            ViewBag.Message = "Групп";
            var model = GetAllGroups();
            return View(model);
        }

        [HttpPost]
        public ActionResult RatingPersonList(String ID, String Date)
        {
            if (User.Identity.IsAuthenticated == false)
            {
                return Redirect("/Account/Login");
            }

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

            using (MonitoringEntities monitoringDB = new MonitoringEntities())
            {
                if (ID != "0")
                {

                    string query = string.Format(
    "SELECT SUM(alldata.FBpositive) as FBpositive, SUM(alldata.FBnegative) as FBnegative, SUM(alldata.FBneutral) as FBneutral, SUM(alldata.Twpositive) as Twpositive, " +
    " SUM(alldata.Twnegative) as Twnegative, SUM(alldata.Twneutral) as Twneutral, SUM(alldata.Webpositive) as Webpositive, " +
    " SUM(alldata.Webnegative) as Webnegative, SUM(alldata.Webneutral) as Webneutral, alldata.Surename, alldata.Name, alldata.Picture, alldata.GroupID, alldata.ID," +
    " SUM(alldata.FBnegative + alldata.FBneutral + alldata.FBpositive) as countFb, " +
    " SUM(alldata.Twpositive + alldata.Twnegative + alldata.Twneutral) as countTw, " +
    " SUM(alldata.Webpositive + alldata.Webnegative + alldata.Webneutral) as countWeb, " +
    " SUM(alldata.Webpositive + alldata.Webnegative + alldata.Webneutral + alldata.Twpositive + alldata.Twnegative + alldata.Twneutral + alldata.FBnegative + alldata.FBneutral + alldata.FBpositive) as countAll " +
    " FROM( "
    +
    " select SUM(dd.PosCnt) as FBpositive, SUM(dd.NegCnt) as FBnegative, SUM(dd.NeuCnt) as FBneutral, 0 as Twpositive, 0 as Twnegative, " +
    " 0 as Twneutral, 0 as Webpositive, 0 as Webnegative, 0 as Webneutral, dd.Surename, dd.Name, dd.Picture, dd.GroupID, dd.ID " +
    "  from( "
    +
     " select count(b.Sentiment) as PosCnt, 0 as NegCnt, 0 as NeuCnt, c.Surename, c.Name, c.Picture, b.Sentiment as PosSentiment, null as NegSentiment, null as NeuSentiment, c.GroupID, c.ID " +
    " from [Facebook.Posts] a join [Facebook.Post.Person] b on b.PostID = a.ID and (b.IsDeleted is null or b.IsDeleted <> 1) " +
    " join [System.Person] c on c.ID = b.PersonID " +
    " where (c.GroupID IN ({0}) or (select count(*) from [System.GroupToPerson] where GroupId={0} and PersonID=c.ID)>0) and b.Sentiment = 'Positive' and a.UpdateTime >= '{1}' and a.UpdateTime <= '{2}' " +
    " group by b.Sentiment, c.Name, c.Surename, c.Picture, c.GroupID, c.ID " +
    " union "
    +
    " select 0 as PosCnt, count(b.Sentiment) as NegCnt, 0 as NeuCnt, c.Surename, c.Name, c.Picture, null as PosSentiment, b.Sentiment as NegSentiment, null as NeuSentiment, c.GroupID, c.ID" +
    " from [Facebook.Posts] a join [Facebook.Post.Person] b on b.PostID = a.ID  and (b.IsDeleted is null or b.IsDeleted <> 1)" +
    " join [System.Person] c on c.ID = b.PersonID " +
    " where (c.GroupID IN ({0}) or (select count(*) from [System.GroupToPerson] where GroupId={0} and PersonID=c.ID)>0) and b.Sentiment = 'Negative' and a.UpdateTime >= '{1}' and a.UpdateTime <= '{2}' " +
    " group by b.Sentiment, c.Name, c.Surename, c.Picture, c.GroupID, c.ID " +
    " union "
    +
    " select 0 as PosCnt, 0 as NegCnt, count(b.Sentiment) as NeuCnt, c.Surename, c.Name, c.Picture, null as PosSentiment, null as NegSentiment, b.Sentiment as NeuSentiment, c.GroupID, c.ID " +
    " from [Facebook.Posts] a join [Facebook.Post.Person] b on b.PostID = a.ID  and (b.IsDeleted is null or b.IsDeleted <> 1)" +
    " join [System.Person] c on c.ID = b.PersonID " +
    " where (c.GroupID IN ({0}) or (select count(*) from [System.GroupToPerson] where GroupId={0} and PersonID=c.ID)>0) and b.Sentiment = 'Neutral' and a.UpdateTime >= '{1}' and a.UpdateTime <= '{2}' " +
    " group by b.Sentiment, c.Name, c.Surename, c.Picture, c.GroupID, c.ID" +
    " ) dd "
    +
    " group by dd.Surename, dd.Name, dd.Picture, dd.GroupID, dd.ID " +
    " union "
    +
    " select 0 as FBpositive, 0 as FBnegative, 0 as FBneutral, SUM(tw.PosCnt) as Twpositive, SUM(tw.NegCnt) as Twnegative, " +
    " SUM(tw.NeuCnt) as Twneutral, 0 as Webpositive, 0 as Webnegative, 0 as Webneutral, tw.Surename, tw.Name, tw.Picture, tw.GroupID, tw.ID " +
    " from( "
    +
    " select count(b.Sentiment) as PosCnt, 0 as NegCnt, 0 as NeuCnt, c.Surename, c.Name, c.Picture, b.Sentiment as PosSentiment, null as NegSentiment, null as NeuSentiment, c.GroupID, c.ID " +
    " from [Twitter.Tweets] a join [Twitter.Tweet.Person] b on b.TweetID = a.ID  and (b.IsDeleted is null or b.IsDeleted <> 1) " +
    " join [System.Person] c on c.ID = b.PersonID " +
    " where (c.GroupID IN ({0}) or (select count(*) from [System.GroupToPerson] where GroupId={0} and PersonID=c.ID)>0) and b.Sentiment = 'Positive' and a.CreatedAt >= '{1}' and a.CreatedAt <= '{2}' " +
    " group by b.Sentiment, c.Name, c.Surename, c.Picture, c.GroupID, c.ID " +
    " union "
    +
    " select 0 as PosCnt, count(b.Sentiment) as NegCnt, 0 as NeuCnt, c.Surename, c.Name, c.Picture, null as PosSentiment, b.Sentiment as NegSentiment, null as NeuSentiment, c.GroupID, c.ID " +
    " from [Twitter.Tweets] a join [Twitter.Tweet.Person] b on b.TweetID = a.ID  and (b.IsDeleted is null or b.IsDeleted <> 1) " +
    " join [System.Person] c on c.ID = b.PersonID " +
    " where (c.GroupID IN ({0}) or (select count(*) from [System.GroupToPerson] where GroupId={0} and PersonID=c.ID)>0) and b.Sentiment = 'Negative' and a.CreatedAt >= '{1}' and a.CreatedAt <= '{2}' " +
    " group by b.Sentiment, c.Name, c.Surename, c.Picture, c.GroupID, c.ID " +
    " union "
    +
    " select 0 as PosCnt, 0 as NegCnt, count(b.Sentiment) as NeuCnt, c.Surename, c.Name, c.Picture, null as PosSentiment, null as NegSentiment, b.Sentiment as NeuSentiment, c.GroupID, c.ID " +
    " from [Twitter.Tweets] a join [Twitter.Tweet.Person] b on b.TweetID = a.ID  and (b.IsDeleted is null or b.IsDeleted <> 1) " +
    " join [System.Person] c on c.ID = b.PersonID " +
    " where (c.GroupID IN ({0}) or (select count(*) from [System.GroupToPerson] where GroupId={0} and PersonID=c.ID)>0) and b.Sentiment = 'Neutral' and a.CreatedAt >= '{1}' and a.CreatedAt <= '{2}' " +
    " group by b.Sentiment, c.Name, c.Surename, c.Picture, c.GroupID, c.ID " +
    " ) tw "
    +
    " group by tw.Surename, tw.Name, tw.Picture, tw.GroupID, tw.ID " +
    " union "
    +
    " select 0 as FBpositive, 0 as FBnegative, 0 as FBneutral, 0 as Twpositive, 0 as Twnegative,  " +
    " 0 as Twneutral, SUM(PosCnt) as Webpositive, SUM(NegCnt) as Webnegative, SUM(NeuCnt) as Webneutral, web.Surename, web.Name, web.Picture, web.GroupID, web.ID " +
    " from( "
    +
    " select count(b.Sentiment) as PosCnt, 0 as NegCnt, 0 as NeuCnt, c.Surename, c.Name, c.Picture, b.Sentiment as PosSentiment, null as NegSentiment, null as NeuSentiment, c.GroupID, c.ID " +
    " from [WebSite.Posts] a join [Website.Post.Person] b on b.PostID = a.ID  and (b.IsDeleted is null or b.IsDeleted <> 1) " +
    " join [System.Person] c on c.ID = b.PersonID " +
    " where (c.GroupID IN ({0}) or (select count(*) from [System.GroupToPerson] where GroupId={0} and PersonID=c.ID)>0) and b.Sentiment = 'Positive' and a.Date >= '{1}' and a.Date <= '{2}' " +
    " group by b.Sentiment, c.Name,c.Surename, c.Picture, c.GroupID, c.ID " +
    " union select 0 as PosCnt, count(b.Sentiment) as NegCnt, 0 as NeuCnt, c.Surename, c.Name, c.Picture, null as PosSentiment, b.Sentiment as NegSentiment, null as NeuSentiment, c.GroupID, c.ID " +
    " from [WebSite.Posts] a join [Website.Post.Person] b on b.PostID = a.ID  and (b.IsDeleted is null or b.IsDeleted <> 1) " +
    " join [System.Person] c on c.ID = b.PersonID " +
    " where (c.GroupID IN ({0}) or (select count(*) from [System.GroupToPerson] where GroupId={0} and PersonID=c.ID)>0) and b.Sentiment = 'Negative'  and a.Date >= '{1}' and a.Date <= '{2}' " +
    " group by b.Sentiment, c.Name, c.Surename, c.Picture, c.GroupID, c.ID " +
    " union "
    +
    "  select 0 as PosCnt, 0 as NegCnt, count(b.Sentiment) as NeuCnt, c.Surename, c.Name, c.Picture, null as PosSentiment, null as NegSentiment, b.Sentiment as NeuSentiment, c.GroupID, c.ID " +
    " from [WebSite.Posts] a join [Website.Post.Person] b on b.PostID = a.ID  and (b.IsDeleted is null or b.IsDeleted <> 1) " +
    " join [System.Person] c on c.ID = b.PersonID " +
    " where (c.GroupID IN ({0}) or (select count(*) from [System.GroupToPerson] where GroupId={0} and PersonID=c.ID)>0) and b.Sentiment = 'Neutral'  and a.Date >= '{1}' and a.Date <= '{2}'" +
    " group by b.Sentiment, c.Name, c.Surename, c.Picture, c.GroupID, c.ID  " +
    ") web group by web.Name, web.Surename, web.Picture, web.GroupID, web.ID ) alldata group by alldata.Surename, alldata.Name, alldata.Picture, alldata.GroupID, alldata.ID order by countAll desc", ID, dt1, dt);

                    ViewBag.RatingData = monitoringDB.Database.SqlQuery<ratingResult>(query).ToList<ratingResult>();

                }
                else
                {
                    ViewBag.RatingData = null;
                }
            }

            return PartialView();
        }

        public ActionResult RatingResultDtl(string id, string Sentiment, string type, string groupID)
        {
            ViewBag.Message = "RatingResultDtl";
            ViewBag.Type = type;
            MonitoringEntities monitoringDB = new MonitoringEntities();
            ViewBag.GroupID = groupID;
            ViewBag.CusID = id;

            if (Sentiment != "All")
            {
                ViewBag.FbData =
               (from a in monitoringDB.Facebook_Post_Person
                join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
                join c in monitoringDB.System_Person on a.PersonID equals c.ID
                where c.ID.ToString() == id && a.Sentiment == Sentiment && a.IsDeleted != true
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
                }).ToList();

                ViewBag.TwitterData =
                   (from a in monitoringDB.Twitter_Tweet_Person
                    join tweet in monitoringDB.Twitter_Tweets on a.TweetID equals tweet.ID
                    join c in monitoringDB.System_Person on a.PersonID equals c.ID
                    join user in monitoringDB.Twitter_User_Details on tweet.TwitterUserID equals user.TwitterUserID
                    where c.ID.ToString() == id && a.Sentiment == Sentiment && a.IsDeleted != true
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
                    }).ToList();

                ViewBag.WebSiteData =
                  (from a in monitoringDB.Website_Post_Person
                   join b in monitoringDB.WebSite_Posts on a.PostID equals b.ID
                   join c in monitoringDB.System_Person on a.PersonID equals c.ID
                   where c.ID.ToString() == id & a.Sentiment == Sentiment
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
                   }).ToList();
            }
            else
            {
                ViewBag.FbData =
               (from a in monitoringDB.Facebook_Post_Person
                join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
                join c in monitoringDB.System_Person on a.PersonID equals c.ID
                where c.ID.ToString() == id && a.IsDeleted != true
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
                }).ToList();

                ViewBag.TwitterData =
                   (from a in monitoringDB.Twitter_Tweet_Person
                    join tweet in monitoringDB.Twitter_Tweets on a.TweetID equals tweet.ID
                    join c in monitoringDB.System_Person on a.PersonID equals c.ID
                    join user in monitoringDB.Twitter_User_Details on tweet.TwitterUserID equals user.TwitterUserID
                    where c.ID.ToString() == id && a.IsDeleted != true
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
                        Verified = user.Verified,

                    }).ToList();

                ViewBag.WebSiteData =
                  (from a in monitoringDB.Website_Post_Person
                   join b in monitoringDB.WebSite_Posts on a.PostID equals b.ID
                   join c in monitoringDB.System_Person on a.PersonID equals c.ID
                   where c.ID.ToString() == id
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

            var rating = monitoringDB.System_Person.Where(s => s.ID.ToString().Equals(id)).FirstOrDefault();
            ViewBag.RatingImage = rating.Picture;
            ViewBag.RatingName = rating.Name;

            return PartialView();
        }

        public IQueryable<Models.Group> GetGroups()
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();
            var groups = (from a in monitoringDB.System_Groups
                          where a.ParentID == null
                          select new Models.Group
                          {
                              ID = a.ID,
                              Name = a.Name,
                              ParentID = a.ParentID,
                              Level = a.Level,
                              ChildGroup = monitoringDB.System_Groups.Where(x => x.ParentID == a.ID)
                          }).ToList();
            var queryable = groups.AsQueryable();
            return queryable;
        }

        public ActionResult ChildList(String ID)
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();
            string query = string.Format("select  m.ID, m.Name, m.ParentID, m.Level, (select count(*) from [System.Groups] where ParentID = m.ID) as ChildCount from [System.Groups] m where m.ParentID = '{0}'", ID);
            ViewBag.ChildList = monitoringDB.Database.SqlQuery<SocialMonster.Models.Group>(query).ToList<SocialMonster.Models.Group>();
            return PartialView();
        }

        public ActionResult TreeView()
        {
            ViewBag.Message = "Групп";
            var model = GetGroups();
            return View(model);
        }

        public class ratingResult
        {
            public int Fbpositive { get; set; }
            public int Fbnegative { get; set; }
            public int Fbneutral { get; set; }


            public int Twpositive { get; set; }
            public int Twnegative { get; set; }
            public int Twneutral { get; set; }

            public int countFb { get; set; }
            public int countTw { get; set; }
            public int countWeb { get; set; }
            public int countAll { get; set; }

            public int Webpositive { get; set; }
            public int Webnegative { get; set; }
            public int Webneutral { get; set; }

            public String Name { get; set; }
            public String Picture { get; set; }
            public String Surename { get; set; }
            public Nullable<System.Guid> GroupID { get; set; }
            public Nullable<System.Guid> ID { get; set; }
        }

        public ActionResult HotTopicRating()
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();

            ViewBag.HotTopicData =
                   (from a in monitoringDB.System_Hottopics
                    orderby a.Title
                    select new Models.HotTopic
                    {
                        ID = a.ID,
                        Title = a.Title,
                        Description = a.Description,
                        ImageURL = a.ImageURL,
                    }).ToList();
            var model = GetAllGroups();
            return View(model);
        }

        public ActionResult HotTopicRatingResult(String ID, String Date)
        {
            if (User.Identity.IsAuthenticated == false)
            {
                return Redirect("/Account/Login");
            }

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

            using (MonitoringEntities monitoringDB = new MonitoringEntities())
            {
                if (ID != "0")
                {
                    ViewBag.TopicID = ID;
                    string query = string.Format("select SUM(alldata.FBpositive) as FBpositive, SUM(alldata.FBnegative) as FBnegative, SUM(alldata.FBneutral) as FBneutral, SUM(alldata.Twpositive) as Twpositive, " +
                        "SUM(alldata.Twnegative) as Twnegative, SUM(alldata.Twneutral) as Twneutral, SUM(alldata.Webpositive) as Webpositive, " +
                        "SUM(alldata.Webnegative) as Webnegative, SUM(alldata.Webneutral) as Webneutral, " +
                        "SUM(alldata.FBnegative + alldata.FBneutral + alldata.FBpositive) as countFb, " +
                        "SUM(alldata.Twpositive + alldata.Twnegative + alldata.Twneutral) as countTw, " +
                        "SUM(alldata.Webpositive + alldata.Webnegative + alldata.Webneutral) as countWeb, " +
                        "(SUM(alldata.FBpositive) + SUM(alldata.FBnegative) + SUM(alldata.FBneutral) + SUM(alldata.Twpositive) + SUM(alldata.Twnegative) + SUM(alldata.Twneutral) + SUM(alldata.Webpositive) + SUM(alldata.Webnegative) + SUM(alldata.Webneutral)) as countAll, " +
                        "alldata.ImageURL, alldata.Title, alldata.Description, alldata.ID, alldata.GroupID FROM( " +
                            "select SUM(fb.PosCnt) as FBpositive, SUM(fb.NegCnt) as FBnegative, SUM(fb.NeuCnt) as FBneutral, 0 as Twpositive, 0 as Twnegative, " +
                            "0 as Twneutral, 0 as Webpositive, 0 as Webnegative, 0 as Webneutral, fb.ImageURL, fb.Title, fb.Description, fb.ID, fb.GroupID " +
                            "from( " +
                                "select count(b.Sentiment) as PosCnt, 0 as NegCnt, 0 as NeuCnt, b.Sentiment as PosSentiment, null as NegSentiment, null as NeuSentiment, c.ImageURL, c.Title, c.Description, c.ID, c.GroupID " +
                                "from[Facebook.Posts] a join[Facebook.Post.Hottopic] b on b.PostID = a.ID " +
                                "join[System.Hottopics] c on c.ID = b.HottopicID " +
                                "where c.GroupID IN ({0})  and b.Sentiment = 'Positive' and a.UpdateTime >= '{1}' and a.UpdateTime <= '{2}' " +
                                "group by b.Sentiment, c.ImageURL, c.Title, c.Description, c.ID, c.GroupID " +
                                "union " +
                                "select 0 as PosCnt, count(b.Sentiment) as NegCnt, 0 as NeuCnt, null PosSentiment, b.Sentiment as NegSentiment, null as NeuSentiment, c.ImageURL, c.Title, c.Description, c.ID,c.GroupID " +
                                "from[Facebook.Posts] a join[Facebook.Post.Hottopic] b on b.PostID = a.ID " +
                                "join[System.Hottopics] c on c.ID = b.HottopicID " +
                                " where c.GroupID IN ({0}) and b.Sentiment = 'Negative' and a.UpdateTime >= '{1}' and a.UpdateTime <= '{2}' " +
                                "group by b.Sentiment, c.ImageURL, c.Title, c.Description, c.ID, c.GroupID " +
                                "union " +
                                "select 0 as PosCnt, 0 as NegCnt, count(b.Sentiment) as NeuCnt, null PosSentiment, null as NegSentiment, b.Sentiment as NeuSentiment, c.ImageURL, c.Title, c.Description, c.ID, c.GroupID " +
                                "from[Facebook.Posts] a join[Facebook.Post.Hottopic] b on b.PostID = a.ID " +
                                "join[System.Hottopics] c on c.ID = b.HottopicID " +
                                "where c.GroupID IN ({0}) and b.Sentiment = 'Neutral' and a.UpdateTime >= '{1}' and a.UpdateTime <= '{2}' " +
                                "group by b.Sentiment, c.ImageURL, c.Title, c.Description, c.ID, c.GroupID " +
                                ") fb group by fb.ImageURL, fb.Title, fb.Description, fb.ID, fb.GroupID " +
                                "union " +
                            "select 0 as FBpositive, 0 as FBnegative, 0 as FBneutral, SUM(tw.PosCnt) as Twpositive, SUM(tw.NegCnt) as Twnegative, " +
                            "SUM(tw.NeuCnt) as Twneutral, 0 as Webpositive, 0 as Webnegative, 0 as Webneutral, tw.ImageURL, tw.Title, tw.Description, tw.ID, tw.GroupID " +
                            "from( " +
                                "select count(b.Sentiment) as PosCnt, 0 as NegCnt, 0 as NeuCnt, b.Sentiment as PosSentiment, null as NegSentiment, null as NeuSentiment, c.ImageURL, c.Title, c.Description, c.ID,c.GroupID " +
                                "from[Twitter.Tweets] a join[Twitter.Tweet.Hottopic] b on b.TweetID = a.ID " +
                                "join[System.Hottopics] c on c.ID = b.HottopicID " +
                                "where c.GroupID IN ({0}) and b.Sentiment = 'Positive' and a.CreatedAt >= '{1}' and a.CreatedAt <= '{2}' " +
                                "group by b.Sentiment, c.ImageURL, c.Title, c.Description, c.ID, c.GroupID " +
                                "union " +
                                "select 0 as PosCnt, count(b.Sentiment) as NegCnt, 0 as NeuCnt, null as PosSentiment, b.Sentiment as NegSentiment, null as NeuSentiment, c.ImageURL, c.Title, c.Description, c.ID, c.GroupID " +
                                "from[Twitter.Tweets] a join[Twitter.Tweet.Hottopic] b on b.TweetID = a.ID " +
                                "join[System.Hottopics] c on c.ID = b.HottopicID " +
                                "where c.GroupID IN ({0}) and b.Sentiment = 'Negative' and a.CreatedAt >= '{1}' and a.CreatedAt <= '{2}' " +
                                "group by b.Sentiment, c.ImageURL, c.Title, c.Description, c.ID, c.GroupID " +
                                "union " +
                                "select 0 as PosCnt, 0 as NegCnt, count(b.Sentiment) as NeuCnt, null as PosSentiment, null as NegSentiment, b.Sentiment as NeuSentiment, c.ImageURL, c.Title, c.Description, c.ID, c.GroupID " +
                                "from[Twitter.Tweets] a join[Twitter.Tweet.Hottopic] b on b.TweetID = a.ID " +
                                "join[System.Hottopics] c on c.ID = b.HottopicID " +
                                "where c.GroupID IN ({0}) and b.Sentiment = 'Neutral' and a.CreatedAt >= '{1}' and a.CreatedAt <= '{2}' " +
                                "group by b.Sentiment, c.ImageURL, c.Title, c.Description, c.ID, c.GroupID " +
                                ") tw group by tw.ImageURL, tw.Title, tw.Description, tw.ID, tw.GroupID " +
                                " union " +
                            "select 0 as FBpositive, 0 as FBnegative, 0 as FBneutral, 0 as Twpositive, 0 as Twnegative, " +
                            "0 as Twneutral, SUM(PosCnt) as Webpositive, SUM(NegCnt) as Webnegative, SUM(NeuCnt) as Webneutral, web.ImageURL, web.Title, web.Description, web.ID, web.GroupID " +
                            "from( " +
                                "select count(b.Sentiment) as PosCnt, 0 as NegCnt, 0 as NeuCnt, b.Sentiment as PosSentiment, null as NegSentiment, null as NeuSentiment, c.ImageURL, c.Title, c.Description, c.ID, c.GroupID " +
                                "from[WebSite.Posts] a join[Website.Post.Hottopic] b on b.PostID = a.ID " +
                                "join[System.Hottopics] c on c.ID = b.HottopicID " +
                                "where c.GroupID IN ({0}) and b.Sentiment = 'Positive' and a.Date >= '{1}' and a.Date <= '{2}' " +
                                "group by b.Sentiment, c.ImageURL, c.Title, c.Description, c.ID, c.GroupID " +
                                "union " +
                                "select 0 as PosCnt, count(b.Sentiment) as NegCnt, 0 as NeuCnt, null as PosSentiment, b.Sentiment as NegSentiment, null as NeuSentiment, c.ImageURL, c.Title, c.Description, c.ID, c.GroupID " +
                                "from[WebSite.Posts] a join[Website.Post.Hottopic] b on b.PostID = a.ID " +
                                "join[System.Hottopics] c on c.ID = b.HottopicID " +
                                "where c.GroupID IN ({0}) and b.Sentiment = 'Negative' and a.Date >= '{1}' and a.Date <= '{2}' " +
                                "group by b.Sentiment, c.ImageURL, c.Title, c.Description, c.ID, c.GroupID " +
                                "union " +
                                "select 0 as PosCnt, 0 as NegCnt, count(b.Sentiment) as NeuCnt, null as PosSentiment, null as NegSentiment, b.Sentiment as NeuSentiment, c.ImageURL, c.Title, c.Description, c.ID, c.GroupID " +
                                "from[WebSite.Posts] a join[Website.Post.Hottopic] b on b.PostID = a.ID " +
                                "join[System.Hottopics] c on c.ID = b.HottopicID " +
                                "where c.GroupID IN ({0}) and b.Sentiment = 'Neutral' and a.Date >= '{1}' and a.Date <= '{2}' " +
                                "group by b.Sentiment, c.ImageURL, c.Title, c.Description, c.ID, c.GroupID " +
                                ") web group by web.ImageURL, web.Title, web.Description, web.ID, web.GroupID" +
                            ") alldata " +
                            "group by alldata.ImageURL, alldata.Title, alldata.Description, alldata.ID, alldata.GroupID " +
                            "order by countAll DESC", ID, dt1, dt);

                    ViewBag.TopicRatingData = monitoringDB.Database.SqlQuery<hotTopicRatingResult>(query).ToList<hotTopicRatingResult>();

                }
                else
                {
                    ViewBag.TopicRatingData = null;
                }
            }
            return PartialView();
        }

        public class hotTopicRatingResult
        {
            public int Fbpositive { get; set; }
            public int Fbnegative { get; set; }
            public int Fbneutral { get; set; }


            public int Twpositive { get; set; }
            public int Twnegative { get; set; }
            public int Twneutral { get; set; }

            public int countFb { get; set; }
            public int countTw { get; set; }
            public int countWeb { get; set; }
            public int countAll { get; set; }

            public int Webpositive { get; set; }
            public int Webnegative { get; set; }
            public int Webneutral { get; set; }

            public String ImageURL { get; set; }
            public String Title { get; set; }
            public String Description { get; set; }
            public Nullable<System.Guid> ID { get; set; }
            public Nullable<System.Guid> GroupID { get; set; }
        }

        public ActionResult HotTopic()
        {
            if (User.Identity.IsAuthenticated == false)
            {
                return Redirect("/Account/Login");
            }
            ViewBag.Message = "HotTopics";

            var model = GetAllGroups();

            MonitoringEntities monitoringDB = new MonitoringEntities();
            ViewBag.GroupCombo = (from a in monitoringDB.System_Groups
                                  select new Models.Group
                                  {
                                      ID = a.ID,
                                      Name = a.Name,
                                      ParentID = a.ParentID,
                                      Level = a.Level,
                                      ChildGroup = monitoringDB.System_Groups.Where(x => x.ParentID == a.ID)
                                  }).ToList();
            return View(model);
        }

        public JsonResult GetNewsByKeyword(String text)
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();
            string query = string.Format("select * from searchNewsByKeyword(N'{0}')", '"' + text + '"');
            var result = monitoringDB.Database.SqlQuery<Models.News>(query).ToList<Models.News>();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult HotTopicList(String ID)
        {
            if (ID != "0")
            {
                MonitoringEntities monitoringDB = new MonitoringEntities();

                string query = string.Format("select * from [System.Hottopics] where GroupID IN ({0})", ID);
                ViewBag.HotTopic = monitoringDB.Database.SqlQuery<Models.HotTopic>(query).ToList<Models.HotTopic>();
            }
            else
            {
                ViewBag.HotTopic = null;
            }
            return PartialView();
        }

        //insert Hottopic//
        public ActionResult InsertHotTopic(String Title, String Description, String ImageURL, String groupID)
        {
            using (MonitoringEntities monitoringDB = new MonitoringEntities())
            {


                string queryIns = string.Format("INSERT INTO [System.Hottopics] (ID,Title,Description,ImageURL,GroupID) values (NEWID(),N'{0}',N'{1}',N'{2}',N'{3}');", Title, Description, ImageURL, groupID);

                var resultsIns = monitoringDB.Database.SqlQuery<Models.HotTopic>(queryIns).ToList<Models.HotTopic>();
                return Json(resultsIns, JsonRequestBehavior.AllowGet);
            }

        }
        // end insert Hottopic //

        // update Hottopic //
        public ActionResult UpdateHotTopic(String Title, String Description, String ImageURL, String ID, String groupID)
        {
            using (MonitoringEntities monitoringDB = new MonitoringEntities())
            {
                string queryUpd = string.Format("UPDATE [System.Hottopics] SET Title=N'{0}', Description=N'{1}', GroupID='{4}', ImageURL='{2}' WHERE ID = '{3}';", Title, Description, ImageURL, Guid.Parse(ID), Guid.Parse(groupID));
                var resultsUpd = monitoringDB.Database.SqlQuery<Models.HotTopic>(queryUpd).ToList<Models.HotTopic>();
                return Json(resultsUpd, JsonRequestBehavior.AllowGet);
            }
        }
        // end update Hottopic //

        // Delete Hottopic //
        public ActionResult DeleteHotTopic(String ID)
        {
            using (MonitoringEntities monitoringDB = new MonitoringEntities())
            {
                string queryDelete = string.Format("DELETE FROM [System.Hottopics] WHERE ID = '{0}';", Guid.Parse(ID));
                var resultsDel = monitoringDB.Database.SqlQuery<Models.HotTopic>(queryDelete).ToList<Models.HotTopic>();
                return Json(resultsDel, JsonRequestBehavior.AllowGet);
            }
        }
        // end delete Hottopic //

        // Hottopic Social //
        public ActionResult HottopicSocial(String ID)
        {
            ViewBag.Message = "HottopicSocial";
            MonitoringEntities monitoringDB = new MonitoringEntities();

            ViewBag.FbData =
                               (from a in monitoringDB.Facebook_Post_Hottopic
                                join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
                                join c in monitoringDB.System_Hottopics on a.HottopicID equals c.ID
                                where c.ID.ToString() == ID
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
               (from a in monitoringDB.Twitter_Tweet_Hottopic
                join tweet in monitoringDB.Twitter_Tweets on a.TweetID equals tweet.ID
                join c in monitoringDB.System_Hottopics on a.HottopicID equals c.ID
                join user in monitoringDB.Twitter_User_Details on tweet.TwitterUserID equals user.TwitterUserID
                where (c.ID.ToString() == ID)
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
                    Sentiment = a.Sentiment
                    //,
                    //SortOrder = b.SortOrder
                }).Take(200).ToList();

            ViewBag.WebSiteData =
              (from a in monitoringDB.Website_Post_Hottopic
               join b in monitoringDB.WebSite_Posts on a.PostID equals b.ID
               join c in monitoringDB.System_Hottopics on a.HottopicID equals c.ID
               where (c.ID.ToString() == ID)
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

            return PartialView();
        }
        // end Hottopic Social //

        public ActionResult HotTopicKey(string topicID)
        {
            if (User.Identity.IsAuthenticated == false)
            {
                return Redirect("/Account/Login");
            }

            MonitoringEntities monitoringDB = new MonitoringEntities();
            ViewBag.KeyTopicID = topicID;

            ViewBag.HotTopicData =
                (from a in monitoringDB.System_Hottopics
                 where a.ID.ToString() == topicID
                 select new Models.HotTopic
                 {
                     ID = a.ID,
                     Title = a.Title,
                     Description = a.Description,
                     ImageURL = a.ImageURL,
                 }).ToList();

            ViewBag.HotTopicKeyData =
                (from a in monitoringDB.System_Keys
                 join b in monitoringDB.System_KeyTypes on a.KeyTypeID equals b.ID
                 where a.HoptopicID.ToString() == topicID

                 select new Models.Key
                 {
                     ID = a.ID,
                     Key1 = a.Key1,
                     Key2 = a.Key2,
                     Key3 = a.Key3,
                     Key4 = a.Key4,
                     Type = a.KeyTypeID,
                     PersonID = a.PersonID,
                     GroupID = a.GroupID,
                     TypeName = b.Description,
                     Latin1 = a.Lattin1,
                     Latin3 = a.Lattin3,
                     Latin4 = a.Lattin4,
                     Latin2 = a.Lattin2

                 }).ToList();

            ViewBag.HotTopicKeyTypes =
                    (from a in monitoringDB.System_KeyTypes
                     select new Models.Key
                     {
                         ID = a.ID,
                         TypeName = a.Description,
                     }).ToList();

            return PartialView();
        }

        // insert hottopic key //
        public ActionResult InsertTopicKey(String Key1, String Key2, String Key3, String Key4, String Latin1, String Latin2, String Latin3, String Latin4, String Type, String TopicID)
        {
            using (MonitoringEntities monitoringDB = new MonitoringEntities())
            {
                string queryIns = string.Format("INSERT INTO [System.Keys] (ID,Key1,Key2,Key3,Key4,Lattin1,Lattin2,Lattin3,Lattin4,KeyTypeID,HoptopicID) values " +
                    "(NEWID(),N'{0}',N'{1}',N'{2}',N'{3}',N'{4}',N'{5}',N'{6}',N'{7}','{8}','{9}');", Key1, Key2, Key3, Key4, Latin1, Latin2, Latin3, Latin4, Guid.Parse(Type), Guid.Parse(TopicID));
                var resultsIns = monitoringDB.Database.SqlQuery<Key>(queryIns).ToList<Key>();
                return Json(resultsIns, JsonRequestBehavior.AllowGet);
            }
        }
        // end insert hottopic key//

        // update hottopic key //
        public ActionResult UpdateTopicKey(String Key1, String Key2, String Key3, String Key4, String Latin1, String Latin2, String Latin3, String Latin4, String Type, String TopicID, String ID)
        {
            using (MonitoringEntities monitoringDB = new MonitoringEntities())
            {
                string queryUpd = string.Format("UPDATE [System.Keys] SET Key1=N'{0}', Key2=N'{1}', Key3=N'{2}'" +
                    ", Key4=N'{3}', Lattin1=N'{4}', Lattin2=N'{5}', Lattin3=N'{6}', Lattin4=N'{7}', KeyTypeID=N'{8}' " +
                    "WHERE ID = '{9}';", Key1, Key2, Key3, Key4, Latin1, Latin2, Latin3, Latin4, Guid.Parse(Type), Guid.Parse(ID));
                var resultsUpd = monitoringDB.Database.SqlQuery<Key>(queryUpd).ToList<Key>();
                return Json(resultsUpd, JsonRequestBehavior.AllowGet);
            }
        }
        // end update hottopic key //

        // Delete hottopic key //
        public ActionResult DeleteTopicKey(String ID)
        {
            using (MonitoringEntities monitoringDB = new MonitoringEntities())
            {
                string queryDelete = string.Format("DELETE FROM [System.Keys] WHERE ID = '{0}';", Guid.Parse(ID));
                var resultsDel = monitoringDB.Database.SqlQuery<Key>(queryDelete).ToList<Key>();
                return Json(resultsDel, JsonRequestBehavior.AllowGet);
            }
        }
        // end delete hottopic key //

        public ActionResult KeyTopicLists(String ID)
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();
            ViewBag.TopicKeyData =
                (from a in monitoringDB.System_Keys
                 join b in monitoringDB.System_KeyTypes on a.KeyTypeID equals b.ID
                 where a.HoptopicID.ToString() == ID

                 select new Models.Key
                 {
                     ID = a.ID,
                     Key1 = a.Key1,
                     Key2 = a.Key2,
                     Key3 = a.Key3,
                     Key4 = a.Key4,
                     Type = a.KeyTypeID,
                     PersonID = a.PersonID,
                     GroupID = a.GroupID,
                     TypeName = b.Description,
                     Latin1 = a.Lattin1,
                     Latin3 = a.Lattin3,
                     Latin4 = a.Lattin4,
                     Latin2 = a.Lattin2

                 }).ToList();
            return PartialView();
        }

        public ActionResult HotTopicDtl(string id)
        {
            if (User.Identity.IsAuthenticated == false)
            {
                return Redirect("/Account/Login");
            }

            ViewBag.Message = "HotTopicDtl";
            MonitoringEntities monitoringDB = new MonitoringEntities();
            int FbLastID = 0;
            ViewBag.TopicID = id;

            ViewBag.FbData =
           (from a in monitoringDB.Facebook_Post_Hottopic
            join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
            join c in monitoringDB.System_Hottopics on a.HottopicID equals c.ID
            where c.ID.ToString() == id
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
                Name = c.Title,
                Picture = c.ImageURL,
                Icon = b.Icon,
                ObjectID = b.ObjectID,
                ParentID = b.ParentID,
                PerName = b.Name,
                Sentiment = a.Sentiment
            }).ToList();

            ViewBag.TwitterData =
               (from a in monitoringDB.Twitter_Tweet_Hottopic
                join tweet in monitoringDB.Twitter_Tweets on a.TweetID equals tweet.ID
                join c in monitoringDB.System_Hottopics on a.HottopicID equals c.ID
                join user in monitoringDB.Twitter_User_Details on tweet.TwitterUserID equals user.TwitterUserID
                where c.ID.ToString() == id
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
                    FullText = tweet.Text,
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
                    //,
                    //SortOrder = b.SortOrder,
                    //PicturePerson = c.Picture,
                }).Take(200).ToList();

            ViewBag.WebSiteData =
              (from a in monitoringDB.Website_Post_Hottopic
               join b in monitoringDB.WebSite_Posts on a.PostID equals b.ID
               join c in monitoringDB.System_Hottopics on a.HottopicID equals c.ID
               where c.ID.ToString() == id
               orderby b.Date descending
               select new Models.WebSitePost
               {
                   ID = b.ID,
                   Link = b.Link,
                   Title = b.Title,
                   Name = c.Title,
                   Text = b.Text,
                   PicturePerson = c.ImageURL,
                   Sentiment = a.Sentiment,
                   Url = b.Url,
                   Body = b.Body.Substring(0, 300) + "...",
                   Reporter = b.Reporter,
                   CoverUrl = b.CoverUrl,
                   DateTime = b.DateTime
               }).Take(200).ToList();

            // making date list
            DateTime last = new DateTime();
            last = DateTime.Now.AddDays(-30);
            DateTime real = DateTime.Now;
            List<DateTime> dates = new List<DateTime>();
            do
            {
                dates.Add(last);
                last = last.AddDays(+1);
            } while (real != last);


            string queryFb = string.Format("select count(*) as count,Convert(date,post.UpdateTime) as date from dbo.[Facebook.Posts] post inner join dbo.[Facebook.Post.Hottopic] b on post.ID = b.PostID  where CONVERT(date, getdate()) >= Convert(date,post.UpdateTime) and Convert(date,post.UpdateTime) > DATEADD(DD, -30, CONVERT(date, getdate())) and b.HottopicID = '{0}' group by Convert(date, post.UpdateTime) order by Convert(date, post.UpdateTime) desc;", id);
            string queryTw = string.Format("select count(*) as count,Convert(date,post.CreatedAt) as date from dbo.[Twitter.Tweets] post inner join dbo.[Twitter.Tweet.Hottopic] b on post.ID = b.HottopicID where CONVERT(date, getdate()) >= Convert(date,post.CreatedAt) and Convert(date,post.CreatedAt) > DATEADD(DD, -30, CONVERT(date, getdate())) and b.HottopicID = '{0}' group by Convert(date, post.CreatedAt) order by Convert(date, post.CreatedAt) desc;", id);
            string queryWb = string.Format("select count(*) as count,Convert(date,post.Date) as date from dbo.[WebSite.Posts] post inner join dbo.[Website.Post.Hottopic] b on post.ID = b.PostID where CONVERT(date, getdate()) >= Convert(date,post.Date) and Convert(date,post.Date) > DATEADD(DD, -30, CONVERT(date, getdate())) and b.HottopicID = '{0}' group by Convert(date, post.Date) order by Convert(date, post.Date) desc;", id);

            var resultsFb = monitoringDB.Database.SqlQuery<graphResult>(queryFb).ToList<graphResult>();
            var resultsTw = monitoringDB.Database.SqlQuery<graphResult>(queryTw).ToList<graphResult>();
            var resultsWb = monitoringDB.Database.SqlQuery<graphResult>(queryWb).ToList<graphResult>();

            string counterfb = "[";
            string countertw = "[";
            string counterweb = "[";

            #region facebookloop
            for (int i = 0; i < dates.Count; i++)
            {
                DateTime date = dates.ElementAt(i).Date;
                //
                if (resultsFb.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterfb == "[")
                    {
                        counterfb = counterfb + resultsFb.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    counterfb = counterfb + "," + resultsFb.Where(p => p.date.Equals(date)).FirstOrDefault().count;

                }
                else
                {
                    if (counterfb == "[")
                    {
                        counterfb = counterfb + 0;

                    }
                    counterfb = counterfb + "," + 0;

                }
            }
            #endregion

            #region twitterloop
            for (int i = 0; i < dates.Count; i++)
            {
                DateTime date = dates.ElementAt(i).Date;
                //
                if (resultsTw.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (countertw == "[")
                    {
                        countertw = countertw + resultsTw.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }
                    countertw = countertw + "," + resultsTw.Where(p => p.date.Equals(date)).FirstOrDefault().count;

                }
                else
                {
                    if (countertw == "[")
                    {
                        countertw = countertw + 0;

                    }
                    countertw = countertw + "," + 0;

                }
            }
            #endregion

            #region webloop
            for (int i = 0; i < dates.Count; i++)
            {
                DateTime date = dates.ElementAt(i).Date;
                //
                if (resultsWb.Where(p => p.date.Equals(date)).Count() > 0)
                {
                    if (counterweb == "[")
                    {
                        counterweb = counterweb + resultsWb.Where(p => p.date.Equals(date)).FirstOrDefault().count;
                    }

                    counterweb = counterweb + "," + resultsWb.Where(p => p.date.Equals(date)).FirstOrDefault().count;

                }
                else
                {
                    if (counterweb == "[")
                    {
                        counterweb = counterweb + 0;

                    }
                    counterweb = counterweb + "," + 0;

                }
            }
            #endregion

            counterfb = counterfb + "]";
            countertw = countertw + "]";
            counterweb = counterweb + "]";
            ViewBag.facebookChartCount = counterfb;
            ViewBag.twitterChartCount = countertw;
            ViewBag.websiteChartCount = counterweb;

            foreach (var post in ViewBag.FbData)
            {
                FbLastID++;
                if (FbLastID == 1)
                {
                    ViewBag.FbLastID = post.ID;
                    ViewBag.FbCusName = post.Name;
                    ViewBag.FbPicture = post.PicturePerson;
                }
            }


            var hotTopic = monitoringDB.System_Hottopics.Where(s => s.ID.ToString().Equals(id)).FirstOrDefault();
            ViewBag.TopicImage = hotTopic.ImageURL;
            ViewBag.TopicTitle = hotTopic.Title;



            return View();
        }

        public ActionResult HotTopicDtlLoad(string id, string Sentiment)
        {
            ViewBag.Message = "HotTopicDtlLoad";
            MonitoringEntities monitoringDB = new MonitoringEntities();

            ViewBag.CusID = id;

            if (Sentiment != "All")
            {
                ViewBag.FbData =
               (from a in monitoringDB.Facebook_Post_Hottopic
                join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
                join c in monitoringDB.System_Hottopics on a.HottopicID equals c.ID
                where c.ID.ToString() == id & a.Sentiment == Sentiment
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
                    Name = c.Title,
                    Picture = b.Picture,
                    PicturePerson = c.ImageURL,
                    Icon = b.Icon,
                    ObjectID = b.ObjectID,
                    ParentID = b.ParentID,
                    PerName = b.Name,
                    Sentiment = a.Sentiment
                }).Take(200).ToList();

                ViewBag.TwitterData =
                   (from a in monitoringDB.Twitter_Tweet_Hottopic
                    join tweet in monitoringDB.Twitter_Tweets on a.TweetID equals tweet.ID
                    join c in monitoringDB.System_Hottopics on a.HottopicID equals c.ID
                    join user in monitoringDB.Twitter_User_Details on tweet.TwitterUserID equals user.TwitterUserID
                    where c.ID.ToString() == id & a.Sentiment == Sentiment
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
                  (from a in monitoringDB.Website_Post_Hottopic
                   join b in monitoringDB.WebSite_Posts on a.PostID equals b.ID
                   join c in monitoringDB.System_Hottopics on a.HottopicID equals c.ID
                   where c.ID.ToString() == id & a.Sentiment == Sentiment
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
               (from a in monitoringDB.Facebook_Post_Hottopic
                join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
                join c in monitoringDB.System_Hottopics on a.HottopicID equals c.ID
                where c.ID.ToString() == id
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
                    Name = c.Title,
                    Picture = b.Picture,
                    PicturePerson = c.ImageURL,
                    Icon = b.Icon,
                    ObjectID = b.ObjectID,
                    ParentID = b.ParentID,
                    PerName = b.Name,
                    Sentiment = a.Sentiment
                }).Take(200).ToList();

                ViewBag.TwitterData =
                   (from a in monitoringDB.Twitter_Tweet_Hottopic
                    join tweet in monitoringDB.Twitter_Tweets on a.TweetID equals tweet.ID
                    join c in monitoringDB.System_Hottopics on a.HottopicID equals c.ID
                    join user in monitoringDB.Twitter_User_Details on tweet.TwitterUserID equals user.TwitterUserID
                    where c.ID.ToString() == id
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
                        Verified = user.Verified,

                    }).Take(200).ToList();

                ViewBag.WebSiteData =
                  (from a in monitoringDB.Website_Post_Hottopic
                   join b in monitoringDB.WebSite_Posts on a.PostID equals b.ID
                   join c in monitoringDB.System_Hottopics on a.HottopicID equals c.ID
                   where c.ID.ToString() == id
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

        public ActionResult HotTopicRatingResultDtl(string id, string Sentiment, string type, string groupID)
        {
            ViewBag.Message = "HotTopicRatingResultDtl";
            ViewBag.Type = type;
            MonitoringEntities monitoringDB = new MonitoringEntities();
            ViewBag.GroupID = groupID;
            ViewBag.CusID = id;

            if (Sentiment != "All")
            {
                ViewBag.FbData =
               (from a in monitoringDB.Facebook_Post_Hottopic
                join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
                join c in monitoringDB.System_Hottopics on a.HottopicID equals c.ID
                where c.ID.ToString() == id & a.Sentiment == Sentiment
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
                    Name = c.Title,
                    Picture = b.Picture,
                    PicturePerson = c.ImageURL,
                    Icon = b.Icon,
                    ObjectID = b.ObjectID,
                    ParentID = b.ParentID,
                    PerName = b.Name,
                    Sentiment = a.Sentiment
                }).Take(200).ToList();

                ViewBag.TwitterData =
                   (from a in monitoringDB.Twitter_Tweet_Hottopic
                    join tweet in monitoringDB.Twitter_Tweets on a.TweetID equals tweet.ID
                    join c in monitoringDB.System_Hottopics on a.HottopicID equals c.ID
                    join user in monitoringDB.Twitter_User_Details on tweet.TwitterUserID equals user.TwitterUserID
                    where c.ID.ToString() == id & a.Sentiment == Sentiment
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
                  (from a in monitoringDB.Website_Post_Hottopic
                   join b in monitoringDB.WebSite_Posts on a.PostID equals b.ID
                   join c in monitoringDB.System_Hottopics on a.HottopicID equals c.ID
                   where c.ID.ToString() == id & a.Sentiment == Sentiment
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
               (from a in monitoringDB.Facebook_Post_Hottopic
                join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
                join c in monitoringDB.System_Hottopics on a.HottopicID equals c.ID
                where c.ID.ToString() == id
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
                    Name = c.Title,
                    Picture = b.Picture,
                    PicturePerson = c.ImageURL,
                    Icon = b.Icon,
                    ObjectID = b.ObjectID,
                    ParentID = b.ParentID,
                    PerName = b.Name,
                    Sentiment = a.Sentiment
                }).Take(200).ToList();

                ViewBag.TwitterData =
                   (from a in monitoringDB.Twitter_Tweet_Hottopic
                    join tweet in monitoringDB.Twitter_Tweets on a.TweetID equals tweet.ID
                    join c in monitoringDB.System_Hottopics on a.HottopicID equals c.ID
                    join user in monitoringDB.Twitter_User_Details on tweet.TwitterUserID equals user.TwitterUserID
                    where c.ID.ToString() == id
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
                        Verified = user.Verified,

                    }).Take(200).ToList();

                ViewBag.WebSiteData =
                  (from a in monitoringDB.Website_Post_Hottopic
                   join b in monitoringDB.WebSite_Posts on a.PostID equals b.ID
                   join c in monitoringDB.System_Hottopics on a.HottopicID equals c.ID
                   where c.ID.ToString() == id
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

            var hotTopic = monitoringDB.System_Hottopics.Where(s => s.ID.ToString().Equals(id)).FirstOrDefault();
            ViewBag.TopicImage = hotTopic.ImageURL;
            ViewBag.TopicTitle = hotTopic.Title;

            return PartialView();
        }

        public ActionResult HotTopicDtlFilter(String id, String Filter, String Sentiment)
        {
            ViewBag.Message = " HotTopic";
            MonitoringEntities monitoringDB = new MonitoringEntities();
            if (Filter == "Date")
            {
                if (Sentiment == "All" || Sentiment == "default" || Sentiment == "")
                {
                    ViewBag.FbData =
                                    (from a in monitoringDB.Facebook_Post_Hottopic
                                     join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
                                     join c in monitoringDB.System_Hottopics on a.HottopicID equals c.ID
                                     where (c.ID.ToString() == id)
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
                       (from a in monitoringDB.Twitter_Tweet_Hottopic
                        join tweet in monitoringDB.Twitter_Tweets on a.TweetID equals tweet.ID
                        join c in monitoringDB.System_Hottopics on a.HottopicID equals c.ID
                        join user in monitoringDB.Twitter_User_Details on tweet.TwitterUserID equals user.TwitterUserID
                        where (c.ID.ToString() == id)
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
                      (from a in monitoringDB.Website_Post_Hottopic
                       join b in monitoringDB.WebSite_Posts on a.PostID equals b.ID
                       join c in monitoringDB.System_Hottopics on a.HottopicID equals c.ID
                       where (c.ID.ToString() == id)
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
                                    (from a in monitoringDB.Facebook_Post_Hottopic
                                     join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
                                     join c in monitoringDB.System_Hottopics on a.HottopicID equals c.ID
                                     where c.ID.ToString() == id & a.Sentiment == Sentiment
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
                        where (c.ID.ToString() == id) && a.Sentiment == Sentiment && a.IsDeleted != true
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
                      (from a in monitoringDB.Website_Post_Hottopic
                       join b in monitoringDB.WebSite_Posts on a.PostID equals b.ID
                       join c in monitoringDB.System_Hottopics on a.HottopicID equals c.ID
                       where (c.ID.ToString() == id) & a.Sentiment == Sentiment
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
                if (Sentiment == "All" || Sentiment == "default")
                {
                    ViewBag.FbData =
                                    (from a in monitoringDB.Facebook_Post_Hottopic
                                     join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
                                     join c in monitoringDB.System_Hottopics on a.HottopicID equals c.ID
                                     where (c.ID.ToString() == id)
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
                       (from a in monitoringDB.Twitter_Tweet_Hottopic
                        join tweet in monitoringDB.Twitter_Tweets on a.TweetID equals tweet.ID
                        join c in monitoringDB.System_Hottopics on a.HottopicID equals c.ID
                        join user in monitoringDB.Twitter_User_Details on tweet.TwitterUserID equals user.TwitterUserID
                        where (c.ID.ToString() == id)
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
                            Sentiment = a.Sentiment
                            //,
                            //SortOrder = b.SortOrder
                        }).Take(200).ToList();

                    ViewBag.WebSiteData =
                      (from a in monitoringDB.Website_Post_Hottopic
                       join b in monitoringDB.WebSite_Posts on a.PostID equals b.ID
                       join c in monitoringDB.System_Hottopics on a.HottopicID equals c.ID
                       where (c.ID.ToString() == id)
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
                                    (from a in monitoringDB.Facebook_Post_Hottopic
                                     join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
                                     join c in monitoringDB.System_Hottopics on a.HottopicID equals c.ID
                                     where c.ID.ToString() == id & a.Sentiment == Sentiment
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
                       (from a in monitoringDB.Twitter_Tweet_Hottopic
                        join tweet in monitoringDB.Twitter_Tweets on a.TweetID equals tweet.ID
                        join c in monitoringDB.System_Hottopics on a.HottopicID equals c.ID
                        join user in monitoringDB.Twitter_User_Details on tweet.TwitterUserID equals user.TwitterUserID
                        where (c.ID.ToString() == id) & a.Sentiment == Sentiment
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
                            Verified = user.Verified
                        }).Take(200).ToList();

                    ViewBag.WebSiteData =
                      (from a in monitoringDB.Website_Post_Hottopic
                       join b in monitoringDB.WebSite_Posts on a.PostID equals b.ID
                       join c in monitoringDB.System_Hottopics on a.HottopicID equals c.ID
                       where (c.ID.ToString() == id) & a.Sentiment == Sentiment
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
            return PartialView();
        }

        public ActionResult Notification()
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();

            ViewBag.AspUserData =
                (from a in monitoringDB.AspNetUsers
                 select new Models.AspNetUser
                 {
                     Id = a.Id,
                     SureName = a.SureName,
                     Name = a.Name,
                     Email = a.Email,
                     PhoneNumber = a.PhoneNumber,
                     UserName = a.UserName
                 }).Take(200).ToList();

            return View();
        }

        public ActionResult NotfUser(string ID)
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();

            ViewBag.NotfPersonID = ID;

            ViewBag.AspUserData =
              (from a in monitoringDB.AspNetUsers
               where a.Id == ID
               select new Models.AspNetUser
               {
                   Id = a.Id,
                   SureName = a.SureName,
                   Name = a.Name,
                   Email = a.Email,
                   PhoneNumber = a.PhoneNumber,
                   UserName = a.UserName
               }).ToList();

            ViewBag.PersonData =
              (from a in monitoringDB.AspNetUserToSystemUsers
               join b in monitoringDB.System_Person on a.SystemPersonID equals b.ID
               where a.AspNetUserID.ToString() == ID
               select new Models.AspUserToSysUser
               {
                   ID = a.ID,
                   PersonID = b.ID,
                   AspNetUserID = a.AspNetUserID.ToString(),
                   SureName = b.Surename,
                   Name = b.Name,
                   Mobile = a.Mobile,
                   Email = a.Email
               }).ToList();

            string query = string.Format("select * from[System.Person] WHERE ID NOT IN( select SystemPersonID from AspNetUserToSystemUser WHERE AspNetUserID = '{0}') ", ID);
            ViewBag.SysPersonData = monitoringDB.Database.SqlQuery<Person>(query).ToList<Person>();

            string query1 = string.Format("select * from[System.Person] ");
            ViewBag.SysPersonData1 = monitoringDB.Database.SqlQuery<Person>(query1).ToList<Person>();

            return View();
        }

        public ActionResult NotfInsert(String persons, String userID, String phone, String email)
        {
            using (MonitoringEntities monitoringDB = new MonitoringEntities())
            {
                string queryIns = string.Format("INSERT INTO [AspNetUserToSystemUser] (ID,AspNetUserID,SystemPersonID, Mobile, Email) values (NEWID(),N'{0}',N'{1}','{2}', N'{3}');", userID, persons, phone, email);
                var resultsIns = monitoringDB.Database.SqlQuery<AspUserToSysUser>(queryIns).ToList<AspUserToSysUser>();
                return Json(resultsIns, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult NotfUpdate(String ID, String persons, String userID, String phone, String email)
        {
            using (MonitoringEntities monitoringDB = new MonitoringEntities())
            {
                string queryUpd = string.Format("UPDATE [AspNetUserToSystemUser] SET AspNetUserID=N'{1}', SystemPersonID=N'{2}', Mobile='{3}'" +
                    ", Email='{4}' WHERE ID = '{0}';", Guid.Parse(ID), userID, persons, phone, email);
                var resultsUpd = monitoringDB.Database.SqlQuery<AspUserToSysUser>(queryUpd).ToList<AspUserToSysUser>();
                return Json(resultsUpd, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult NotfDelete(String ID)
        {
            using (MonitoringEntities monitoringDB = new MonitoringEntities())
            {
                string queryDelete = string.Format("DELETE FROM [AspNetUserToSystemUser] WHERE ID = '{0}';", Guid.Parse(ID));
                var resultsDel = monitoringDB.Database.SqlQuery<AspUserToSysUser>(queryDelete).ToList<AspUserToSysUser>();
                return Json(resultsDel, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult NotfUserList(String ID)
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();

            ViewBag.PersonData =
             (from a in monitoringDB.AspNetUserToSystemUsers
              join b in monitoringDB.System_Person on a.SystemPersonID equals b.ID
              where a.AspNetUserID.ToString() == ID
              select new Models.AspUserToSysUser
              {
                  ID = a.ID,
                  SureName = b.Surename,
                  Name = b.Name,
                  Mobile = a.Mobile,
                  Email = a.Email
              }).Take(200).ToList();

            return PartialView();
        }

        public bool Control(string postID, string type, string sentiment, string isEdit, string UserId)
        {
            //using (MonitoringEntities monitoringDB = new MonitoringEntities())
            //{
            //    string queryDelete = string.Format("DELETE FROM [AspNetUserToSystemUser] WHERE ID = '{0}';", Guid.Parse(ID));
            //    var resultsDel = monitoringDB.Database.SqlQuery<AspUserToSysUser>(queryDelete).ToList<AspUserToSysUser>();
            //    return Json(resultsDel, JsonRequestBehavior.AllowGet);
            //}
            try
            {
                MonitoringEntities monitoringDB = new MonitoringEntities();
                switch (type)
                {
                    case "Facebook":
                        var facebookPost = monitoringDB.Facebook_Post_Person.Where(x => x.PostID == new Guid(postID) && x.PersonID.ToString() == UserId).FirstOrDefault();
                        if (facebookPost != null)
                        {
                            if (isEdit == "true")
                                facebookPost.Sentiment = sentiment;
                            else
                                facebookPost.IsDeleted = true;
                            monitoringDB.Entry(facebookPost).State = System.Data.Entity.EntityState.Modified;

                        }
                        break;
                    case "Twitter":
                        var twitterPost = monitoringDB.Twitter_Tweet_Person.Where(x => x.TweetID == new Guid(postID) && x.PersonID.ToString() == UserId).FirstOrDefault();
                        if (twitterPost != null)
                        {
                            if (isEdit == "true")
                                twitterPost.Sentiment = sentiment;
                            else
                                twitterPost.IsDeleted = true;
                            monitoringDB.Entry(twitterPost).State = System.Data.Entity.EntityState.Modified;
                        }
                        break;
                    case "WebSite":
                        var webSitePost = monitoringDB.Website_Post_Person.Where(x => x.PostID == new Guid(postID) && x.PersonID.ToString() == UserId).FirstOrDefault();
                        if (webSitePost != null)
                        {
                            if (isEdit == "true")
                                webSitePost.Sentiment = sentiment;
                            else
                                webSitePost.IsDeleted = true;
                            monitoringDB.Entry(webSitePost).State = System.Data.Entity.EntityState.Modified;
                        }
                        break;
                }
                monitoringDB.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public ActionResult WordCloud(string ID)
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();
            //var myPostIDs = monitoringDB.Facebook_Post_Person.Where(a => a.PersonID.ToString() == ID).Select(a => a.PostID).ToList();
            //string LargeText="";
            //foreach(var postID in myPostIDs)
            //{
            //    var postComments = monitoringDB.Facebook_Post_Comments.Where(a => a.PostID.ToString() == postID.ToString()).ToList();
            //    foreach(var comment in postComments)
            //    {
            //        LargeText = LargeText + " " + comment.Message.ToLower();
            //    }
            //}
            //ViewBag.Text = LargeText;
            var comments = (from comment in monitoringDB.Facebook_Post_Comments
                            join post in monitoringDB.Facebook_Posts on comment.PostID equals post.ID
                            join per in monitoringDB.Facebook_Post_Person on post.ID equals per.PostID
                            orderby comment.RegisteredDate descending
                            where per.PersonID.ToString() == ID
                            select new Models.FbComment
                            {
                                Message = comment.Message
                            }).ToList();
            string AllText = string.Concat(comments.Select(n => n.Message));
            //foreach (var comment in comments)
            //{
            //    AllText = AllText + " " + comment.Message.ToLower();
            //}
            var Words = Regex.Split(AllText.ToLower(), @"\W+")
                .Where(s => s.Length > 3 && s != "байна" && s != "байгаа" && s != "байх" && s != "байсан" && s != "байдаг" && s != "shig" && s != "бгаа" && s != "чинь" && s != "юм" && s != "bgaa" && s != "болж" && s != "байж" && s != "болсон" && s != "гээд" && s != "гэсэн" && s != "минь" && s != "bgaa" && s != "shuu" && s != "https")
                .GroupBy(s => s)
                .Select(s => new Models.LargeTextModels
                {
                    word = s.Key,
                    count = s.Count()
                })
                .OrderByDescending(g => g.count).Take(40).ToList();
            //List<string> Word = new List<string>();
            //int[] WordCount = new int[10];
            //string[] WordName = new string[10];
            //int i = 0;
            //foreach (var word in Words)
            //{
            //    WordCount[i] = word.count;
            //    WordName[i] = word.word;
            //    i++;
            //}
            //ViewBag.Word1 = WordName[0];
            //ViewBag.Count1 = WordCount[0];
            //ViewBag.Words = Words;
            return View(Words);
        }

        public ActionResult FacebookPostCloud(string ID)
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();
            var posts =
                (from a in monitoringDB.Facebook_Post_Person
                 join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
                 join c in monitoringDB.System_Person on a.PersonID equals c.ID
                 where (c.ID.ToString() == ID) && a.IsDeleted != true
                 select new Models.FbPost
                 {
                     Message = b.Message,

                 }).ToList();
            string AllText = string.Concat(posts.Select(n => n.Message));
            var Words = Regex.Split(AllText.ToLower(), @"\W+")
                .Where(s => s.Length > 3 && s != "байна" && s != "байгаа" && s != "байх" && s != "байсан" && s != "байдаг" && s != "shig" && s != "бгаа" && s != "чинь" && s != "юм" && s != "bgaa" && s != "болж" && s != "байж" && s != "болсон" && s != "гээд" && s != "гэсэн" && s != "минь" && s != "bgaa" && s != "shuu" && s != "https")
                .GroupBy(s => s)
                .Select(s => new Models.LargeTextModels
                {
                    word = s.Key,
                    count = s.Count()
                })
                .OrderByDescending(g => g.count).Take(40).ToList();
            return View(Words);
        }

        public ActionResult TwitterCloud(string ID)
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();
            var posts =
                (from a in monitoringDB.Twitter_Tweet_Person
                 join b in monitoringDB.Twitter_Tweets on a.TweetID equals b.ID
                 join c in monitoringDB.System_Person on a.PersonID equals c.ID
                 where (c.ID.ToString() == ID) && a.IsDeleted != true
                 select new Models.Twitter
                 {
                     Text = b.Text,

                 }).ToList();
            string AllText = string.Concat(posts.Select(n => n.Text));
            var Words = Regex.Split(AllText.ToLower(), @"\W+")
                .Where(s => s.Length > 3 && s != "байна" && s != "байгаа" && s != "байх" && s != "байсан" && s != "байдаг" && s != "shig" && s != "бгаа" && s != "чинь" && s != "юм" && s != "bgaa" && s != "болж" && s != "байж" && s != "болсон" && s != "гээд" && s != "гэсэн" && s != "минь" && s != "bgaa" && s != "shuu" && s != "https")
                .GroupBy(s => s)
                .Select(s => new Models.LargeTextModels
                {
                    word = s.Key,
                    count = s.Count()
                })
                .OrderByDescending(g => g.count).Take(40).ToList();
            return View(Words);
        }

        public JsonResult ListSubMenu(string ID)
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();
            var list = (from person in monitoringDB.System_Person
                        join groupToPerson in monitoringDB.System_GroupToPerson on person.ID equals groupToPerson.PersonID
                        where groupToPerson.GroupID.ToString() == ID
                        orderby person.Name ascending
                        select new Models.Person
                        {
                            Name = person.Name.ToUpper(),
                            SureName = person.Surename.Substring(0, 1),
                            ID = person.ID
                        }).ToList();
            //string id = '8D809E4E-28DA-4B29-8ED1-1D536DAC8F20';
            //var list = monitoringDB.System_Person.Where(a => a.GroupID.ToString() == '8D809E4E-28DA-4B29-8ED1-1D536DAC8F20').ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SmartNewsCusList(string ID)
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();
            var currentUser = monitoringDB.System_Person.Where(a => a.ID.ToString().Equals(ID)).Select(n => new Models.SystemUserModels
            {
                ID = n.ID.ToString(),
                Name = n.Name,
                Surename = n.Surename,
                Tittlename = n.Tittlename,
                Picture = n.Picture,
                Description = n.Description,
                FacebookAccount = n.FacebookAccount.ToString(),
                TwitterAccount = n.TwitterAccount.ToString()
            }).FirstOrDefault();
            //Guid personID = Guid.NewGuid(ID);
            //from a in monitoringDB.Facebook_Post_Person
            //join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
            //join c in monitoringDB.System_Person on a.PersonID equals c.ID
            var fb = (from a in monitoringDB.Facebook_Post_Person
                      join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
                      join c in monitoringDB.System_Person on a.PersonID equals c.ID
                      where c.ID.ToString() == ID && a.IsDeleted != true
                      orderby b.UpdateTime descending
                      select new Models.mixedDataModel
                      {
                          ID = b.ID,
                          FromID = b.FromID,
                          From = b.FromName,
                          Text = b.Message.Substring(0, 300) + "...",
                          Type = "Facebook",
                          Date = b.UpdateTime,
                          FullPicture = b.FullPicture,
                          URL = b.PermalinkUrl,
                          FromPicture = c.Picture,
                          Icon = b.Icon,
                          Sentiment = a.Sentiment
                      }).ToList();
            var tw = (from tweetPerson in monitoringDB.Twitter_Tweet_Person
                      join tweet in monitoringDB.Twitter_Tweets on tweetPerson.TweetID equals tweet.ID
                      join sysPerson in monitoringDB.System_Person on tweetPerson.PersonID equals sysPerson.ID
                      join tweetUser in monitoringDB.Twitter_User_Details on tweet.TwitterUserID equals tweetUser.TwitterUserID
                      where sysPerson.ID.ToString() == ID && tweetPerson.IsDeleted != true
                      orderby tweet.CreatedAt descending
                      select new Models.mixedDataModel
                      {
                          ID = tweet.ID,
                          FromID = tweet.TwitterUserID.ToString(),
                          From = tweetUser.ScreenName,
                          Text = tweet.FullText,
                          Type = "Twitter",
                          Date = tweet.CreatedAt,
                          TwFullPicture = tweet.TwFullPicture,
                          URL = tweet.StatusID,
                          FromURL = tweetUser.ScreenName,
                          Sentiment = tweetPerson.Sentiment,
                      }).ToList();
            var wb = (from a in monitoringDB.Website_Post_Person
                      join b in monitoringDB.WebSite_Posts on a.PostID equals b.ID
                      join c in monitoringDB.System_Person on a.PersonID equals c.ID
                      where c.ID.ToString() == ID && a.IsDeleted != true
                      orderby b.Date descending
                      select new Models.mixedDataModel
                      {
                          ID = b.ID,
                          Title = b.Title,
                          From = b.Reporter,
                          Text = b.Text.Substring(0, 300) + "...",
                          Type = "Website",
                          Date = b.DateTime,
                          FullPicture = b.CoverUrl,
                          WbFullPicture = b.WbFullPicture,
                          URL = b.Url,
                          FromPicture = c.Picture,
                          Sentiment = a.Sentiment
                      }).ToList();
            IEnumerable<mixedDataModel> alldata = fb.Union(tw.Union(wb)).OrderByDescending(n => n.Date);
            int dataCount = alldata.Count();
            int pageCount = dataCount / 20;
            ViewBag.pageCount = pageCount;
            ViewBag.alldata = alldata.Take(20);
            return View(currentUser);
        }
        public ActionResult SmartNewsCusList_Result(string userID, string text, string type, string sentiment, string start, string end)
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();
            //IEnumerable <mixedDataModel> alldata= new List<mixedDataModel>();
            var fb = (from a in monitoringDB.Facebook_Post_Person
                      join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
                      join c in monitoringDB.System_Person on a.PersonID equals c.ID
                      where c.ID.ToString() == userID && a.IsDeleted != true
                      orderby b.UpdateTime descending
                      select new Models.mixedDataModel
                      {
                          ID = b.ID,
                          FromID = b.FromID,
                          From = b.FromName,
                          Text = b.Message.Substring(0, 300) + "...",
                          Type = "Facebook",
                          Date = b.UpdateTime,
                          FullPicture = b.FullPicture,
                          URL = b.PermalinkUrl,
                          FromPicture = c.Picture,
                          Icon = b.Icon,
                          Sentiment = a.Sentiment
                      }).ToList();
            var tw = (from tweetPerson in monitoringDB.Twitter_Tweet_Person
                      join tweet in monitoringDB.Twitter_Tweets on tweetPerson.TweetID equals tweet.ID
                      join sysPerson in monitoringDB.System_Person on tweetPerson.PersonID equals sysPerson.ID
                      join tweetUser in monitoringDB.Twitter_User_Details on tweet.TwitterUserID equals tweetUser.TwitterUserID
                      where sysPerson.ID.ToString() == userID && tweetPerson.IsDeleted != true
                      orderby tweet.CreatedAt descending
                      select new Models.mixedDataModel
                      {
                          ID = tweet.ID,
                          FromID = tweet.TwitterUserID.ToString(),
                          From = tweetUser.ScreenName,
                          Text = tweet.FullText,
                          Type = "Twitter",
                          Date = tweet.CreatedAt,
                          TwFullPicture = tweet.TwFullPicture,
                          FullPicture = tweet.MediaEntitiy1,
                          URL = tweet.StatusID,
                          FromURL = tweetUser.ScreenName,
                          Sentiment = tweetPerson.Sentiment,
                      }).ToList();
            var wb = (from a in monitoringDB.Website_Post_Person
                      join b in monitoringDB.WebSite_Posts on a.PostID equals b.ID
                      join c in monitoringDB.System_Person on a.PersonID equals c.ID
                      where c.ID.ToString() == userID && a.IsDeleted != true
                      orderby b.Date descending
                      select new Models.mixedDataModel
                      {
                          ID = b.ID,
                          Title = b.Title,
                          From = b.Reporter,
                          Text = b.Text.Substring(0, 300) + "...",
                          Type = "Website",
                          Date = b.DateTime,
                          FullPicture = b.CoverUrl,
                          WbFullPicture = b.WbFullPicture,
                          URL = b.Url,
                          FromPicture = c.Picture,
                          Sentiment = a.Sentiment
                      }).ToList();
            IEnumerable<mixedDataModel> alldata = fb.Union(tw.Union(wb)).OrderByDescending(n => n.Date);
            if (sentiment != "0")
            {
                alldata = alldata.Where(a => a.Sentiment == sentiment);
            }
            if (type != "0")
            {
                alldata = alldata.Where(a => a.Type == type);
            }
            if (start != "")
            {
                DateTime d_start = DateTime.Parse(start.Replace('T', ' '));
                alldata = alldata.Where(a => a.Date > d_start);
            }
            if (end != "")
            {
                DateTime d_end = DateTime.Parse(end.Replace('T', ' '));
                alldata = alldata.Where(a => a.Date < d_end);
            }
            alldata = alldata.Where(a => a.Text.Contains(text));

            //if (sentiment!="0" && type!="0")
            //{
            //    var fb = (from a in monitoringDB.Facebook_Post_Person
            //              join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
            //              join c in monitoringDB.System_Person on a.PersonID equals c.ID
            //              where c.ID.ToString() == userID && a.IsDeleted != true && (b.Message.Contains(text) || b.Name.Contains(text))
            //              orderby b.UpdateTime descending
            //              select new Models.mixedDataModel
            //              {
            //                  ID = b.ID,
            //                  FromID = b.FromID,
            //                  From = b.FromName,
            //                  Text = b.Message.Substring(0, 300) + "...",
            //                  Type = "Facebook",
            //                  Date = b.UpdateTime,
            //                  FullPicture = b.FullPicture,
            //                  URL = b.PermalinkUrl,
            //                  FromPicture = c.Picture,
            //                  Icon = b.Icon,
            //                  Sentiment = a.Sentiment
            //              }).ToList();
            //    var tw = (from tweetPerson in monitoringDB.Twitter_Tweet_Person
            //              join tweet in monitoringDB.Twitter_Tweets on tweetPerson.TweetID equals tweet.ID
            //              join sysPerson in monitoringDB.System_Person on tweetPerson.PersonID equals sysPerson.ID
            //              join tweetUser in monitoringDB.Twitter_User_Details on tweet.TwitterUserID equals tweetUser.TwitterUserID
            //              where sysPerson.ID.ToString() == userID && tweetPerson.IsDeleted != true && (tweet.Text.Contains(text))
            //              orderby tweet.CreatedAt descending
            //              select new Models.mixedDataModel
            //              {
            //                  ID = tweet.ID,
            //                  FromID = tweet.TwitterUserID.ToString(),
            //                  From = tweetUser.ScreenName,
            //                  Text = tweet.FullText,
            //                  Type = "Twitter",
            //                  Date = tweet.CreatedAt,
            //                  URL = tweet.StatusID,
            //                  FromURL = tweetUser.ScreenName,
            //                  Sentiment = tweetPerson.Sentiment,
            //              }).ToList();
            //    var wb = (from a in monitoringDB.Website_Post_Person
            //              join b in monitoringDB.WebSite_Posts on a.PostID equals b.ID
            //              join c in monitoringDB.System_Person on a.PersonID equals c.ID
            //              where c.ID.ToString() == userID && a.IsDeleted != true && (b.Text.Contains(text) || b.Reporter.Contains(text))
            //              orderby b.Date descending
            //              select new Models.mixedDataModel
            //              {
            //                  ID = b.ID,
            //                  Title = b.Title,
            //                  From = b.Reporter,
            //                  Text = b.Text.Substring(0, 300) + "...",
            //                  Type = "Website",
            //                  Date = b.DateTime,
            //                  FullPicture = b.CoverUrl,
            //                  URL = b.Url,
            //                  FromPicture = c.Picture,
            //                  Sentiment = a.Sentiment
            //              }).ToList();
            //    alldata = fb.Union(tw.Union(wb)).OrderByDescending(n => n.Date).Take(20);
            //}
            //else
            //{
            //    if (type == "facebook")
            //    {
            //        alldata = (from a in monitoringDB.Facebook_Post_Person
            //                  join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
            //                  join c in monitoringDB.System_Person on a.PersonID equals c.ID
            //                  where c.ID.ToString() == userID && a.IsDeleted != true && (b.Message.Contains(text) || b.Name.Contains(text) && a.Sentiment == sentiment)
            //                  orderby b.UpdateTime descending
            //                  select new Models.mixedDataModel
            //                  {
            //                      ID = b.ID,
            //                      FromID = b.FromID,
            //                      From = b.FromName,
            //                      Text = b.Message.Substring(0, 300) + "...",
            //                      Type = "Facebook",
            //                      Date = b.UpdateTime,
            //                      FullPicture = b.FullPicture,
            //                      URL = b.PermalinkUrl,
            //                      FromPicture = c.Picture,
            //                      Icon = b.Icon,
            //                      Sentiment = a.Sentiment
            //                  }).Take(10).ToList();
            //    }
            //    else if(type == "twitter")
            //    {
            //        alldata = (from tweetPerson in monitoringDB.Twitter_Tweet_Person
            //                  join tweet in monitoringDB.Twitter_Tweets on tweetPerson.TweetID equals tweet.ID
            //                  join sysPerson in monitoringDB.System_Person on tweetPerson.PersonID equals sysPerson.ID
            //                  join tweetUser in monitoringDB.Twitter_User_Details on tweet.TwitterUserID equals tweetUser.TwitterUserID
            //                  where sysPerson.ID.ToString() == userID && tweetPerson.IsDeleted != true
            //                  orderby tweet.CreatedAt descending
            //                  select new Models.mixedDataModel
            //                  {
            //                      ID = tweet.ID,
            //                      FromID = tweet.TwitterUserID.ToString(),
            //                      From = tweetUser.ScreenName,
            //                      Text = tweet.FullText,
            //                      Type = "Twitter",
            //                      Date = tweet.CreatedAt,
            //                      URL = tweet.StatusID,
            //                      FromURL = tweetUser.ScreenName,
            //                      Sentiment = tweetPerson.Sentiment,
            //                  }).Take(10).ToList();
            //    }
            //    else if(type=="website")
            //    {

            //        alldata = (from a in monitoringDB.Website_Post_Person
            //                  join b in monitoringDB.WebSite_Posts on a.PostID equals b.ID
            //                  join c in monitoringDB.System_Person on a.PersonID equals c.ID
            //                  where c.ID.ToString() == userID && a.IsDeleted != true
            //                  orderby b.Date descending
            //                  select new Models.mixedDataModel
            //                  {
            //                      ID = b.ID,
            //                      Title = b.Title,
            //                      From = b.Reporter,
            //                      Text = b.Text.Substring(0, 300) + "...",
            //                      Type = "Website",
            //                      Date = b.DateTime,
            //                      FullPicture = b.CoverUrl,
            //                      URL = b.Url,
            //                      FromPicture = c.Picture,
            //                      Sentiment = a.Sentiment
            //                  }).Take(10).ToList();
            //    }
            //}
            return PartialView(alldata);
        }
        public ActionResult SmartNewsCusList_page(string ID, int page)
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();
            int skip = 20 * page;
            //Guid personID = Guid.NewGuid(ID);
            //from a in monitoringDB.Facebook_Post_Person
            //join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
            //join c in monitoringDB.System_Person on a.PersonID equals c.ID
            var fb = (from a in monitoringDB.Facebook_Post_Person
                      join b in monitoringDB.Facebook_Posts on a.PostID equals b.ID
                      join c in monitoringDB.System_Person on a.PersonID equals c.ID
                      where c.ID.ToString() == ID && a.IsDeleted != true
                      orderby b.UpdateTime descending
                      select new Models.mixedDataModel
                      {
                          ID = b.ID,
                          FromID = b.FromID,
                          From = b.FromName,
                          Text = b.Message.Substring(0, 300) + "...",
                          Type = "Facebook",
                          Date = b.UpdateTime,
                          FullPicture = b.FullPicture,
                          URL = b.PermalinkUrl,
                          FromPicture = c.Picture,
                          Icon = b.Icon,
                          Sentiment = a.Sentiment
                      }).ToList();
            var tw = (from tweetPerson in monitoringDB.Twitter_Tweet_Person
                      join tweet in monitoringDB.Twitter_Tweets on tweetPerson.TweetID equals tweet.ID
                      join sysPerson in monitoringDB.System_Person on tweetPerson.PersonID equals sysPerson.ID
                      join tweetUser in monitoringDB.Twitter_User_Details on tweet.TwitterUserID equals tweetUser.TwitterUserID
                      where sysPerson.ID.ToString() == ID && tweetPerson.IsDeleted != true
                      orderby tweet.CreatedAt descending
                      select new Models.mixedDataModel
                      {
                          ID = tweet.ID,
                          FromID = tweet.TwitterUserID.ToString(),
                          From = tweetUser.ScreenName,
                          Text = tweet.FullText,
                          Type = "Twitter",
                          Date = tweet.CreatedAt,
                          FullPicture = tweet.MediaEntitiy1,
                          URL = tweet.StatusID,
                          FromURL = tweetUser.ScreenName,
                          Sentiment = tweetPerson.Sentiment,
                      }).ToList();
            var wb = (from a in monitoringDB.Website_Post_Person
                      join b in monitoringDB.WebSite_Posts on a.PostID equals b.ID
                      join c in monitoringDB.System_Person on a.PersonID equals c.ID
                      where c.ID.ToString() == ID && a.IsDeleted != true
                      orderby b.Date descending
                      select new Models.mixedDataModel
                      {
                          ID = b.ID,
                          Title = b.Title,
                          From = b.Reporter,
                          Text = b.Text.Substring(0, 300) + "...",
                          Type = "Website",
                          Date = b.DateTime,
                          FullPicture = b.CoverUrl,
                          URL = b.Url,
                          FromPicture = c.Picture,
                          Sentiment = a.Sentiment
                      }).ToList();
            IEnumerable<mixedDataModel> alldata = fb.Union(tw.Union(wb)).OrderByDescending(n => n.Date).Skip(skip).Take(20);
            return PartialView(alldata);
        }

        [HttpGet]
        public ActionResult SearchByMultipleFields(string personID, string searchText, DateTime startDate, DateTime endDate, string Sentiment)
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();
            dynamic mixedModel = new ExpandoObject();

            var fbPosts =
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
                });
            //var testfbpost = fbPosts.ToList();

            var twPosts =
               (from tweetPerson in monitoringDB.Twitter_Tweet_Person
                join tweet in monitoringDB.Twitter_Tweets on tweetPerson.TweetID equals tweet.ID
                join sysPerson in monitoringDB.System_Person on tweetPerson.PersonID equals sysPerson.ID
                join tweetUser in monitoringDB.Twitter_User_Details on tweet.TwitterUserID equals tweetUser.TwitterUserID
                where sysPerson.ID.ToString() == personID && tweetPerson.IsDeleted != true
                orderby tweet.CreatedAt descending
                select new Models.Twitter
                {
                    ID = tweet.ID,
                    TwitterUserID = tweet.TwitterUserID,
                    Text = tweet.Text,
                    TweetID = tweet.TweetID,
                    CreatedAt = tweet.CreatedAt,
                    ScreenName = tweetUser.ScreenName,
                    Source = tweet.Source,
                    StatusID = tweet.StatusID,
                    UserID = tweet.UserID,
                    UrlEntity1 = tweet.UrlEntity1,
                    Sentiment = tweetPerson.Sentiment,
                    TwFullPicture = tweet.TwFullPicture,
                    FullText = tweet.FullText,
                });
            var temptwPosts = twPosts.ToList();

            var wbPosts =
              (from a in monitoringDB.Website_Post_Person
               join b in monitoringDB.WebSite_Posts on a.PostID equals b.ID
               join c in monitoringDB.System_Person on a.PersonID equals c.ID
               where c.ID.ToString() == personID && a.IsDeleted != true
               orderby b.Date descending
               select new Models.WebSitePost
               {
                   ID = b.ID,
                   Link = b.Link,
                   Title = b.Title,
                   Name = c.Name,
                   Text = b.Text,
                   PicturePerson = c.Picture,
                   Sentiment = a.Sentiment,
                   WbFullPicture = b.WbFullPicture,
                   Url = b.Url,
                   Body = b.Body.Substring(0, 300) + "...",
                   Reporter = b.Reporter,
                   CoverUrl = b.CoverUrl,
                   DateTime = b.DateTime
               });
            //var tempwbPosts = wbPosts.ToList();
            if (Sentiment != null && !Sentiment.Equals(""))
            {
                string tempSentiment = null;
                switch (Sentiment)
                {
                    case "Эерэг": tempSentiment = "Positive"; break;
                    case "Сөрөг": tempSentiment = "Negative"; break;
                    case "Саармаг": tempSentiment = "Neutral"; break;
                    default: tempSentiment = null; break;
                }

                if (tempSentiment != null)
                {
                    fbPosts = fbPosts.Where(a => a.Sentiment == tempSentiment);
                    twPosts = twPosts.Where(a => a.Sentiment == tempSentiment);
                    wbPosts = wbPosts.Where(a => a.Sentiment == tempSentiment);
                    var aa = twPosts.Count();
                }
            }

            if (startDate != null && !startDate.Equals(""))
            {
                DateTime tempDate = Convert.ToDateTime(startDate);
                //tempDate.AddHours(23);
                //tempDate.AddMinutes(59);
                tempDate = tempDate.AddSeconds(1);
                fbPosts = fbPosts.Where(a => a.UpdateTime > tempDate);
                twPosts = twPosts.Where(a => a.CreatedAt > tempDate);
                wbPosts = wbPosts.Where(a => a.DateTime > tempDate);

            }
            //testfbpost = fbPosts.ToList();
            //temptwPosts = twPosts.ToList();
            //tempwbPosts = wbPosts.ToList();


            if (endDate != null && !endDate.Equals(""))
            {
                DateTime tempDate = Convert.ToDateTime(endDate);
                tempDate = tempDate.AddHours(23);
                tempDate = tempDate.AddMinutes(59);
                tempDate = tempDate.AddSeconds(59);
                fbPosts = fbPosts.Where(a => a.UpdateTime < tempDate);
                twPosts = twPosts.Where(a => a.CreatedAt < tempDate);
                wbPosts = wbPosts.Where(a => a.DateTime < tempDate);
            }
            //testfbpost = fbPosts.ToList();
            //temptwPosts = twPosts.ToList();
            //tempwbPosts = wbPosts.ToList();



            //testfbpost = fbPosts.ToList();
            //temptwPosts = twPosts.ToList();
            //tempwbPosts = wbPosts.ToList();

            if (searchText != null && !searchText.Equals(""))
            {
                fbPosts = fbPosts.Where(a => a.Description.Contains(searchText) || a.Message.Contains(searchText) || a.Name.Contains(searchText) || a.FromName.Contains(searchText));
                twPosts = twPosts.Where(a => a.Text.Contains(searchText));
                wbPosts = wbPosts.Where(a => a.Title.Contains(searchText) || a.Text.Contains(searchText));
            }

            //testfbpost = fbPosts.ToList();
            //temptwPosts = twPosts.ToList();
            //tempwbPosts = wbPosts.ToList();

            var PositiveFb = fbPosts.Where(a => a.Sentiment.ToString() == "Positive").Select(n => new Models.SystemUserModels
            {
                ID = n.ID.ToString()
            }).Count();

            var NegativeFb = fbPosts.Where(a => a.Sentiment.ToString() == "Negative").Select(n => new Models.SystemUserModels
            {
                ID = n.ID.ToString()
            }).Count();
            var NeutralFB = fbPosts.Where(a => a.Sentiment.ToString() == "Neutral").Select(n => new Models.SystemUserModels
            {
                ID = n.ID.ToString()
            }).Count();
            var PositiveWb = wbPosts.Where(a => a.Sentiment.ToString() == "Positive").Select(n => new Models.SystemUserModels
            {
                ID = n.ID.ToString()
            }).Count();
            var NegativeWb = wbPosts.Where(a => a.Sentiment.ToString() == "Negative").Select(n => new Models.SystemUserModels
            {
                ID = n.ID.ToString()
            }).Count();
            var NeutralWB = wbPosts.Where(a => a.Sentiment.ToString() == "Neutral").Select(n => new Models.SystemUserModels
            {
                ID = n.ID.ToString()
            }).Count();
            var PositiveTW = twPosts.Where(a => a.Sentiment.ToString() == "Positive").Select(n => new Models.SystemUserModels
            {
                ID = n.ID.ToString()
            }).Count();
            var NegativeTW = twPosts.Where(a => a.Sentiment.ToString() == "Negative").Select(n => new Models.SystemUserModels
            {
                ID = n.ID.ToString()
            }).Count();
            var NeutralTW = twPosts.Where(a => a.Sentiment.ToString() == "Neutral").Select(n => new Models.SystemUserModels
            {
                ID = n.ID.ToString()
            }).Count();
            var SearchPositive = PositiveFb + PositiveWb + PositiveTW;
            var SearchNegative = NegativeFb + NegativeWb + NegativeTW;
            var SearchNeutral = NeutralFB + NeutralWB + NeutralTW;
            var SearchFBCount = PositiveFb + NegativeFb + NeutralFB;
            var SearchTWCount = PositiveTW + NegativeTW + NeutralTW;
            var SearchWBCount = PositiveWb + NegativeWb + NeutralWB;


            try
            {
                mixedModel.fbPosts = fbPosts.ToList();
            }
            catch { };

            try
            {
                mixedModel.twPosts = twPosts.ToList();
            }
            catch { };

            try
            {
                mixedModel.wbPosts = wbPosts.ToList();
            }
            catch { };
            try
            {
                mixedModel.SearchPositive = SearchPositive;
            }
            catch { };

            try
            {
                mixedModel.SearchNegative = SearchNegative;
            }
            catch { };

            try
            {
                mixedModel.SearchNeutral = SearchNeutral;
            }
            catch { };
            try
            {

                mixedModel.SearchFBCount = SearchFBCount;
            }
            catch { };

            try
            {
                mixedModel.SearchTWCount = SearchTWCount;
            }
            catch { };

            try
            {
                mixedModel.SearchWBCount = SearchWBCount;
            }
            catch { };


            var jsonResult = Json(mixedModel, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;



            return jsonResult;

            //var person = monitoringDB.System_Person.Where(s => s.ID.ToString().Equals(id)).FirstOrDefault();
            //ViewBag.ProfileImage = person.Picture;
            //ViewBag.PersonName = person.Name;
            //ViewBag.Person = person;
        }
    }
}