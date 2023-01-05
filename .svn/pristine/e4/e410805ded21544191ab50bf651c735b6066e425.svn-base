using SocialMonster.DAL;
using SocialMonster.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace SocialMonster.Controllers
{
    public class KeyWordController : Controller
    {
        // GET: KeyWord
        public ActionResult Index(int? KeyDate)
        {
            if (User.Identity.IsAuthenticated == false)
            {
                return Redirect("/Account/Login");
            }
            MonitoringEntities monitoringDB = new MonitoringEntities();
            if (KeyDate == null)
            {
                KeyDate = 1;
            }
            ViewBag.Date = KeyDate;
            DateTime dt = DateTime.Now;
            DateTime dt1 = DateTime.Now;
            DateTime dt2 = DateTime.Now;
            //int looper = 0;

            List<Keyword> oneWord = new List<Keyword>();
            List<Keyword> twoWord = new List<Keyword>();
            List<Keyword> threeWord = new List<Keyword>();
            
            //start Facebook keyword
            for (int j=0;j<KeyDate;j++)
            {
                double tmpDate = j;
                dt1 = dt.AddDays(-tmpDate);
                dt2 = dt1.AddHours(-3);
                var totalData = monitoringDB.Facebook_Posts.Where(a => a.UpdateTime >= dt2 && a.UpdateTime <= dt1);
                //string BigText = "";
                string BigText = string.Concat(totalData.Select(n => n.Message));
                foreach (var row in totalData)
                {
                    BigText = BigText + " " + row.Name;
                }
                BigText = BigText.ToLower();
                //BigText = BigText.Replace("/", " ");
                BigText = BigText.Replace("?", " ")
                                  .Replace("....", " ")
                                  .Replace("...", " ")
                                  .Replace("..", " ")
                                  .Replace(". ", " ")
                                  .Replace("!", " ")
                                  .Replace("[", " ")
                                  .Replace("]", " ")
                                  .Replace("(", " ")
                                  .Replace(")", " ")
                                  .Replace(":", " ")
                                  .Replace("*", " ")
                                  .Replace("~", " ")
                                  .Replace("^", " ")
                                  .Replace("`", " ")
                                  .Replace("+", " ")
                                  .Replace("<", " ")
                                  .Replace(">", " ")
                                  .Replace("{", " ")
                                  .Replace("}", " ")
                                  .Replace("|", " ")
                                  .Replace("     ", " ")
                                  .Replace("    ", " ")
                                  .Replace("   ", " ")
                                  .Replace("  ", " ")
                                  .Replace(@"\", " ")
                                  .Replace(" ийн", " ")
                                  .Replace(" руу", " ")
                                  .Replace(" даа", " ");

                string[] split = BigText.Split(' ').Distinct().ToArray();
                if(split.Length>2)
                {
                    //1 ҮГ
                    int[] keyOneWordNumber = new int[split.Length];
                    for (int i = 0; i < split.Length; i++)
                    {
                        if (split[i] != " " && split[i] != "" && split[i] != "бай" && split[i].Length > 2)
                        {
                            keyOneWordNumber[i] = Regex.Matches(BigText, split[i]).Count + 1;
                        }
                        else
                        {
                            keyOneWordNumber[i] = 0;
                        }
                    }
                    for (int i = 0; i < 200; i++)
                    {
                        if (i >= keyOneWordNumber.Length)
                        {
                            break;
                        }
                        int maxValue = keyOneWordNumber.Max();
                        int maxIndex = keyOneWordNumber.ToList().IndexOf(maxValue);
                        //oneWordValue[j,i] = maxValue;
                        //oneWordText[j,i] = split[maxIndex];
                        Keyword key = new Keyword();
                        key.Word = split[maxIndex];
                        key.Count = maxValue;
                        oneWord.Add(key);
                        keyOneWordNumber[maxIndex] = 0;
                    }
                    //2 ҮГ
                    string[] splitNoDistinct = BigText.Split(' ').ToArray();
                    string[] split2word = new string[splitNoDistinct.Length - 1];
                    for (int i = 0; i < splitNoDistinct.Length - 1; i++)
                    {
                        if (splitNoDistinct[i] != " " && splitNoDistinct[i] != "" && splitNoDistinct[i].Length > 2 && splitNoDistinct[i + 1] != " " && splitNoDistinct[i + 1] != "" && splitNoDistinct[i + 1].Length > 2)
                        {
                            split2word[i] = splitNoDistinct[i] + " " + splitNoDistinct[i + 1];
                        }
                        else
                        {
                            split2word[i] = "x x";
                        }
                    }
                    split2word = split2word.Distinct().ToArray();
                    int[] keyTwoWordNumber = new int[split2word.Length];
                    for (int i = 0; i < split2word.Length; i++)
                    {
                        keyTwoWordNumber[i] = Regex.Matches(BigText, split2word[i]).Count + 1;
                    }
                    for (int i = 0; i < 100; i++)
                    {
                        if (i >= keyTwoWordNumber.Length)
                        {
                            break;
                        }
                        int maxValue = keyTwoWordNumber.Max();
                        int maxIndex = keyTwoWordNumber.ToList().IndexOf(maxValue);
                        //twoWordValue[j,i] = maxValue;
                        //twoWordText[j,i] = split2word[maxIndex];
                        Keyword key = new Keyword();
                        key.Word = split2word[maxIndex];
                        key.Count = maxValue;
                        twoWord.Add(key);
                        keyTwoWordNumber[maxIndex] = 0;

                    }
                    //3 ҮГ
                    string[] split3word = new string[splitNoDistinct.Length - 2];
                    for (int i = 0; i < splitNoDistinct.Length - 2; i++)
                    {
                        if (splitNoDistinct[i] != " " && splitNoDistinct[i] != "" && splitNoDistinct[i].Length > 2 && splitNoDistinct[i + 1] != " " && splitNoDistinct[i + 1] != "" && splitNoDistinct[i + 1].Length > 2 && splitNoDistinct[i + 2] != " " && splitNoDistinct[i + 2] != "" && splitNoDistinct[i + 2].Length > 2)
                        {
                            split3word[i] = splitNoDistinct[i] + " " + splitNoDistinct[i + 1] + " " + splitNoDistinct[i + 2];
                        }
                        else
                        {
                            split3word[i] = "x x x";
                        }
                    }
                    split3word = split3word.Distinct().ToArray();
                    int[] keyThreeWordNumber = new int[split3word.Length];
                    for (int i = 0; i < split3word.Length; i++)
                    {
                        keyThreeWordNumber[i] = Regex.Matches(BigText, split3word[i]).Count + 1;
                    }
                    for (int i = 0; i < 100; i++)
                    {
                        if (i > keyThreeWordNumber.Length)
                        {
                            break;
                        }
                        int maxValue = keyThreeWordNumber.Max();
                        int maxIndex = keyThreeWordNumber.ToList().IndexOf(maxValue);
                        //threeWordValue[j,i] = maxValue;
                        //threeWordText[j,i] = split3word[maxIndex];
                        Keyword key = new Keyword();
                        key.Word = split3word[maxIndex];
                        key.Count = maxValue;
                        threeWord.Add(key);
                        keyThreeWordNumber[maxIndex] = 0;

                        //int test3 = 1;
                        //foreach (var row in threeWord.ToList())
                        //{
                        //    if (row.Word == split3word[maxIndex] && test3 < threeWord.Count())
                        //    {
                        //        split3word[maxIndex] = "";
                        //        maxValue = 0;
                        //        threeWord.RemoveAt(threeWord.Count - 1);
                        //        i--;
                        //        break;
                        //        //keyTwoWordNumber[maxIndex] = twoWord.LastOrDefault().Count;

                        //        //i--;
                        //        //keyTwoWordNumber[maxIndex] = 0;
                        //        //break;
                        //    }
                        //    test3++;
                        //}
                    }
                }
                
            }
            ViewBag.topOneWord = oneWord.GroupBy(a => a.Word).Select(n => new Models.Keyword
            {
                    topWord = n.FirstOrDefault().Word,
                    topCount = n.Sum(s => s.Count)
            }
                ).OrderByDescending(a => a.topCount).Take(40).ToList();
            ViewBag.topTwoWord = twoWord.GroupBy(a => a.Word).Select(n => new Models.Keyword
            {
                    topWord = n.FirstOrDefault().Word,
                    topCount = n.Sum(s=>s.Count)
            }
                ).OrderByDescending(a => a.topCount).Take(40).ToList();
            ViewBag.topThreeWord = threeWord.GroupBy(a => a.Word).Select(n => new Models.Keyword
            {
                    topWord = n.FirstOrDefault().Word,
                    topCount = n.Sum(s => s.Count)
            }
            ).OrderByDescending(a => a.topCount).Take(40).ToList();
            //end Facebook keyword

            //start Web keyword
            List<Keyword> wbOneWord = new List<Keyword>();
            List<Keyword> wbTwoWord = new List<Keyword>();
            List<Keyword> wbThreeWord = new List<Keyword>();
            for (int j = 0; j < KeyDate*8; j++)
            {
                double tmpDate = j * 0.125;
                dt1 = dt.AddDays(-tmpDate);
                dt2 = dt1.AddHours(-3);
                var totalData = monitoringDB.WebSite_Posts.Where(a => a.Date >= dt2 && a.Date <= dt1);
                string BigText = "";
                foreach (var row in totalData)
                {
                    BigText = BigText + row.Title;
                }
                BigText = BigText.ToLower();
                //BigText = BigText.Replace("/", " ");
                BigText = BigText.Replace("?", " ")
                                  .Replace("....", " ")
                                  .Replace("...", " ")
                                  .Replace("..", " ")
                                  .Replace(". ", " ")
                                  .Replace("!", " ")
                                  .Replace("[", " ")
                                  .Replace("]", " ")
                                  .Replace("(", " ")
                                  .Replace(")", " ")
                                  .Replace(":", " ")
                                  .Replace("*", " ")
                                  .Replace("~", " ")
                                  .Replace("^", " ")
                                  .Replace("`", " ")
                                  .Replace("+", " ")
                                  .Replace("<", " ")
                                  .Replace(">", " ")
                                  .Replace("{", " ")
                                  .Replace("}", " ")
                                  .Replace("|", " ")
                                  .Replace("     ", " ")
                                  .Replace("    ", " ")
                                  .Replace("   ", " ")
                                  .Replace("  ", " ")
                                  .Replace(@"\", " ");
                string[] split = BigText.Split(' ').Distinct().ToArray();
                if(split.Length>2)
                {
                    //1 ҮГ
                    int[] keyOneWordNumber = new int[split.Length];
                    for (int i = 0; i < split.Length; i++)
                    {
                        if (split[i] != " " && split[i] != "" && split[i] != "бай" && split[i].Length > 2)
                        {
                            keyOneWordNumber[i] = Regex.Matches(BigText, split[i]).Count + 1;
                        }
                        else
                        {
                            keyOneWordNumber[i] = 0;
                        }
                    }
                    for (int i = 0; i < 200; i++)
                    {
                        if (i >= keyOneWordNumber.Length)
                        {
                            break;
                        }
                        int maxValue = keyOneWordNumber.Max();
                        int maxIndex = keyOneWordNumber.ToList().IndexOf(maxValue);
                        //oneWordValue[j,i] = maxValue;
                        //oneWordText[j,i] = split[maxIndex];
                        Keyword key = new Keyword();
                        key.Word = split[maxIndex];
                        key.Count = maxValue;
                        wbOneWord.Add(key);
                        keyOneWordNumber[maxIndex] = 0;
                    }
                    //2 ҮГ
                    string[] splitNoDistinct = BigText.Split(' ').ToArray();
                    string[] split2word = new string[splitNoDistinct.Length - 1];
                    for (int i = 0; i < splitNoDistinct.Length - 1; i++)
                    {
                        if (splitNoDistinct[i] != " " && splitNoDistinct[i] != "" && splitNoDistinct[i].Length > 2 && splitNoDistinct[i + 1] != " " && splitNoDistinct[i + 1] != "" && splitNoDistinct[i + 1].Length > 2)
                        {
                            split2word[i] = splitNoDistinct[i] + " " + splitNoDistinct[i + 1];
                        }
                        else
                        {
                            split2word[i] = "x x";
                        }
                    }
                    split2word = split2word.Distinct().ToArray();
                    int[] keyTwoWordNumber = new int[split2word.Length];
                    for (int i = 0; i < split2word.Length; i++)
                    {
                        keyTwoWordNumber[i] = Regex.Matches(BigText, split2word[i]).Count + 1;
                    }
                    for (int i = 0; i < 100; i++)
                    {
                        if (i >= keyTwoWordNumber.Length)
                        {
                            break;
                        }
                        int maxValue = keyTwoWordNumber.Max();
                        int maxIndex = keyTwoWordNumber.ToList().IndexOf(maxValue);
                        //twoWordValue[j,i] = maxValue;
                        //twoWordText[j,i] = split2word[maxIndex];
                        Keyword key = new Keyword();
                        key.Word = split2word[maxIndex];
                        key.Count = maxValue;
                        wbTwoWord.Add(key);
                        keyTwoWordNumber[maxIndex] = 0;
                    }
                    //3 ҮГ
                    string[] split3word = new string[splitNoDistinct.Length - 2];
                    for (int i = 0; i < splitNoDistinct.Length - 2; i++)
                    {
                        if (splitNoDistinct[i] != " " && splitNoDistinct[i] != "" && splitNoDistinct[i].Length > 2 && splitNoDistinct[i + 1] != " " && splitNoDistinct[i + 1] != "" && splitNoDistinct[i + 1].Length > 2 && splitNoDistinct[i + 2] != " " && splitNoDistinct[i + 2] != "" && splitNoDistinct[i + 2].Length > 2)
                        {
                            split3word[i] = splitNoDistinct[i] + " " + splitNoDistinct[i + 1] + " " + splitNoDistinct[i + 2];
                        }
                        else
                        {
                            split3word[i] = "x x x";
                        }
                    }
                    split3word = split3word.Distinct().ToArray();
                    int[] keyThreeWordNumber = new int[split3word.Length];
                    for (int i = 0; i < split3word.Length; i++)
                    {
                        keyThreeWordNumber[i] = Regex.Matches(BigText, split3word[i]).Count + 1;
                    }
                    for (int i = 0; i < 100; i++)
                    {
                        if (i > keyThreeWordNumber.Length)
                        {
                            break;
                        }
                        int maxValue = keyThreeWordNumber.Max();
                        int maxIndex = keyThreeWordNumber.ToList().IndexOf(maxValue);
                        //threeWordValue[j,i] = maxValue;
                        //threeWordText[j,i] = split3word[maxIndex];
                        Keyword key = new Keyword();
                        key.Word = split3word[maxIndex];
                        key.Count = maxValue;
                        wbThreeWord.Add(key);
                        keyThreeWordNumber[maxIndex] = 0;
                    }

                }
            }
            ViewBag.webOneWord = wbOneWord.GroupBy(a => a.Word).Select(n => new Models.Keyword
            {
                topWord = n.FirstOrDefault().Word,
                topCount = n.Sum(s => s.Count)
            }
                ).OrderByDescending(a => a.topCount).Take(20).ToList();
            ViewBag.webTwoWord = wbTwoWord.GroupBy(a => a.Word).Select(n => new Models.Keyword
            {
                topWord = n.FirstOrDefault().Word,
                topCount = n.Sum(s => s.Count)
            }
                ).OrderByDescending(a => a.topCount).Take(20).ToList();
            ViewBag.webThreeWord = wbThreeWord.GroupBy(a => a.Word).Select(n => new Models.Keyword
            {
                topWord = n.FirstOrDefault().Word,
                topCount = n.Sum(s => s.Count)
            }
            ).OrderByDescending(a => a.topCount).Take(20).ToList();
            //end Web keyword

            //string[] allOneWordText = new string[100];
            //int[] allOneWordValue = new int[100];
            //for (int i=0;i<KeyDate;i++)
            //{
            //    for(int j=0;j<100;j++)
            //    {
            //        allOneWordText[i] = allOneWordText[i] + oneWordText[i, j];
            //        allOneWordValue[i] = allOneWordValue[i] + oneWordValue[i, j];
            //    }
            //}
            //switch (KeyDate)
            //{
            //    case "1":
            //        dt1 = dt.AddDays(-1);
            //        break;

            //    case "5":
            //        dt1 = dt.AddDays(-5);
            //        break;

            //    case "7":
            //        dt1 = dt.AddDays(-7);
            //        break;
            //    case "10":
            //        dt1 = dt.AddDays(-10);
            //        break;
            //    case "14":
            //        dt1 = dt.AddDays(-14);
            //        break;
            //    case "100":
            //        dt1 = dt.AddMonths(-1);
            //        break;
            //    case "1000":
            //        dt1 = dt.AddYears(-1);
            //        break;
            //    case "0":
            //        dt1 = dt.AddYears(-100);
            //        break;
            //    default:
            //        dt1 = dt.AddYears(-100);
            //        break;
            //}
            //var totalData = monitoringDB.Facebook_Posts.Where(a => a.UpdateTime >= dt1 && a.UpdateTime <= dt);
            ////var totalData = monitoringDB.Facebook_Posts;
            //string BigText = "";
            //foreach (var row in totalData)
            //{
            //    BigText = BigText + row.Message;
            //}
            //string[] split = BigText.Split(' ').Distinct().ToArray();
            ////1 ҮГ
            //int[] keyOneWordNumber = new int[split.Length];
            //for (int i = 0; i < split.Length; i++)
            //{
            //    if (split[i] != " " && split[i] != "" && split[i].Length > 2)
            //    {
            //        keyOneWordNumber[i] = Regex.Matches(BigText, split[i]).Count;
            //    }
            //    else
            //    {
            //        keyOneWordNumber[i] = 0;
            //    }
            //}
            //int[] top10Value = new int[10];
            //string[] top10Word = new string[10];
            //for (int i = 0; i < 10; i++)
            //{
            //    int maxValue = keyOneWordNumber.Max();
            //    int maxIndex = keyOneWordNumber.ToList().IndexOf(maxValue);
            //    top10Value[i] = maxValue;
            //    top10Word[i] = split[maxIndex];
            //    keyOneWordNumber[maxIndex] = 0;
            //}
            //ViewBag.word1count = top10Value;
            //ViewBag.word1text = top10Word;

            ////2 ҮГ
            //string[] split2word = new string[split.Length - 1];

            //int[] keyTwoWordNumber = new int[split.Length - 1];
            //for (int i = 0; i < split.Length - 1; i++)
            //{
            //    if (split[i] != " " && split[i] != "" && split[i].Length > 1 && split[i + 1] != " " && split[i + 1] != "" && split[i + 1].Length > 1)
            //    {
            //        split2word[i] = split[i] + " " + split[i + 1];
            //        keyTwoWordNumber[i] = Regex.Matches(BigText, split2word[i]).Count;
            //    }
            //    else
            //    {
            //        keyOneWordNumber[i] = 0;
            //    }
            //}
            //int[] top10Value2 = new int[10];
            //string[] top10Word2 = new string[10];
            //for (int i = 0; i < 10; i++)
            //{
            //    int maxValue = keyTwoWordNumber.Max();
            //    int maxIndex = keyTwoWordNumber.ToList().IndexOf(maxValue);
            //    top10Value2[i] = maxValue;
            //    top10Word2[i] = split2word[maxIndex];
            //    keyTwoWordNumber[maxIndex] = 0;
            //}
            //ViewBag.word2count = top10Value2;
            //ViewBag.word2text = top10Word2;

            ////3 ҮГ
            //string[] split3word = new string[split.Length - 2];

            //int[] keyThreeWordNumber = new int[split.Length - 2];
            //for (int i = 0; i < split.Length - 2; i++)
            //{
            //    split3word[i] = split[i] + " " + split[i + 1] + " " + split[i + 2];
            //    keyThreeWordNumber[i] = Regex.Matches(BigText, split3word[i]).Count;
            //}
            //int[] top10Value3 = new int[10];
            //string[] top10Word3 = new string[10];
            //for (int i = 0; i < 10; i++)
            //{
            //    int maxValue = keyThreeWordNumber.Max();
            //    int maxIndex = keyThreeWordNumber.ToList().IndexOf(maxValue);
            //    top10Value3[i] = maxValue;
            //    top10Word3[i] = split3word[maxIndex];
            //    keyThreeWordNumber[maxIndex] = 0;
            //}
            //ViewBag.word3count = top10Value3;
            //ViewBag.word3text = top10Word3;

            return View();
        }
        public ActionResult ResultList(string Value, int? date)
        {
            if (User.Identity.IsAuthenticated == false)
            {
                return Redirect("/Account/Login");
            }
            MonitoringEntities monitoringDB = new MonitoringEntities();
            DateTime dt = DateTime.Now;
            int tmpDate = date ?? default(int);
            DateTime dt1 = dt.AddDays(-tmpDate);
            string res = Value;
            //List<TopUser> facebookList = new List<TopUser>();
            //List<TopUser> twitterList = new List<TopUser>();
            //List<TopUser> webList = new List<TopUser>();
            //facebookList = monitoringDB.Facebook_Posts.Where(a => a.Message.Contains(res))
            //    .Select(a=> new Models.TopUser
            //    {
            //        fb_ID=a.FromName,
            //        fb_Name=a.Message
            //    }).ToList();
            //foreach(var row in facebookList)
            //{
            //    if(row.fb_ID.Length>15)
            //    {
            //        row.fb_ID = row.fb_ID.Substring(0, 15) + "...";
            //    }
            //}
            //twitterList = monitoringDB.Twitter_Tweets.Where(a => a.Text.Contains(res))
            //    .Select(a => new Models.TopUser
            //    {
            //        fb_ID = a.ScreenName,
            //        fb_Name = a.Text
            //    }).ToList();
            //foreach (var row in twitterList)
            //{
            //    if (row.fb_ID!=null)
            //    {
            //        if (row.fb_ID.Length > 15)
            //        {
            //            row.fb_ID = row.fb_ID.Substring(0, 15) + "...";
            //        }
            //    }
            //}
            //webList = monitoringDB.WebSite_Posts.Where(a => a.Title.Contains(res))
            //    .Select(a => new Models.TopUser
            //    {
            //        fb_ID = a.Url,
            //        fb_Name = a.Title
            //    }).ToList();
            //foreach (var row in webList)
            //{
            //    if (row.fb_ID.Length > 15)
            //    {
            //        row.fb_ID = row.fb_ID.Substring(0, 15) + "...";
            //    }
            //}
            //ViewBag.fbList = facebookList;
            //ViewBag.twList = twitterList;
            //ViewBag.wbList = webList;
            ViewBag.Fb_Data = monitoringDB.Facebook_Posts.Where(a =>a.UpdateTime >=dt1 && a.UpdateTime<=dt && a.Message.Contains(res) || a.Name.Contains(res))
                                     .Select(b=> new Models.FbPost
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
                                         //Sentiment = a.Sentiment

                                     }).OrderByDescending(a=>a.UpdateTime).Take(200).ToList();

            ViewBag.TwitterData = (from tweet in monitoringDB.Twitter_Tweets
                                   join user in monitoringDB.Twitter_User_Details on tweet.TwitterUserID equals user.TwitterUserID
                                   where (tweet.CreatedAt >= dt1) & tweet.CreatedAt <= dt & tweet.Text.Contains(res) 
                                   orderby tweet.CreatedAt descending
                                   select new Models.Twitter
                                   {
                                    TweetID = tweet.TweetID,
                                    StatusID = tweet.StatusID,
                                    ScreenNameResponse=user.ScreenNameResponse
                            //,
                            //SortOrder = b.SortOrder
                        }).Take(200).ToList();

            ViewBag.WebSiteData = monitoringDB.WebSite_Posts.Where(a => a.Date >= dt1 && a.Date <= dt && a.Title.Contains(res))
               .Select(b=> new Models.WebSitePost
               {
                   ID = b.ID,
                   Link = b.Link,
                   Title = b.Title,
                   Text = b.Text,
                   //Sentiment = a.Sentiment,
                   Url = b.Url,
                   Body = b.Body.Substring(0, 300) + "...",
                   Reporter = b.Reporter,
                   CoverUrl = b.CoverUrl,
                   DateTime = b.DateTime
               }).OrderByDescending(a=>a.DateTime.Value).Take(200).ToList();
            return View();
        }
    }
}