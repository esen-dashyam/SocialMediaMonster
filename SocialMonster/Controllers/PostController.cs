using Nest;
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
    public class PostController : System.Web.Http.ApiController
    {
        [HttpPost]
        [AllowAnonymous]
        [JwtAuthentication]
        public IHttpActionResult GetNewsByPerson(string PersonID, int Days, int Skip)
        {
            MonitoringEntities monitoringDB = new MonitoringEntities();
            DateTime From = DateTime.Today.AddDays(-5);
            DateTime To = DateTime.Today.AddDays(1);
            var fbPost = (from fpost in monitoringDB.Facebook_Posts
                          join person in monitoringDB.Facebook_Post_Person on fpost.ID equals person.PostID
                          where person.PersonID == new Guid(PersonID) && (From <= fpost.UpdateTime && To >= fpost.UpdateTime)
                          select new Models.ApiNews
                          {
                              ID = fpost.PostID.ToString(),
                              url = fpost.PermalinkUrl,
                              title = fpost.Caption,
                              text = fpost.Message,
                              publisher = fpost.FromName,
                              author = fpost.FromID,
                              image = fpost.FullPicture,
                              date = fpost.UpdateTime.ToString(),
                              type = "facebook"

                          }
                         ).ToList<Models.ApiNews>();
;
            var twTweet = (from tpost in monitoringDB.Twitter_Tweets
                            join person in monitoringDB.Twitter_Tweet_Person on tpost.ID equals person.TweetID
                            join userDetails in monitoringDB.Twitter_User_Details on tpost.TwitterUserID  equals userDetails.TwitterUserID
                            where person.PersonID == new Guid(PersonID) && (From <= tpost.RegisteredDate && To >= tpost.RegisteredDate)
                            select new Models.ApiNews
                            {
                                ID = tpost.StatusID,
                                url = "https://twitter.com/statuses/" + tpost.StatusID,
                                title = null,
                                text = tpost.Text,
                                //publisher = post.,
                                author = userDetails.ScreenName,
                                //image = tpost.ima,
                                date = tpost.RegisteredDate.ToString(),
                                type = "twitter"

                            }
             ).ToList<Models.ApiNews>();

            var wbPost = (from wpost in monitoringDB.WebSite_Posts
                            join person in monitoringDB.Twitter_Tweet_Person on wpost.ID equals person.TweetID
                            where person.PersonID == new Guid(PersonID) && (From <= wpost.DateTime && To >= wpost.DateTime)
                            select new Models.ApiNews
                            {
                                ID = wpost.ID.ToString(),
                                url = wpost.Url,
                                title = wpost.Title,
                                text = wpost.Text,
                                //publisher = post.,
                                //author = post.FromName,
                                image = wpost.CoverUrl,
                                date = wpost.DateTime.ToString(),
                                type = "twitter"

                            }
             ).ToList<Models.ApiNews>();

            var post = fbPost.Union<Models.ApiNews>(twTweet).Union<Models.ApiNews>(wbPost).OrderByDescending(a=>a.date).Take(4);

            var json = JsonConvert.SerializeObject(post);
            return Ok
                (
                   json
                );
        }
     
    }
}