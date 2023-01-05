using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialMonster.Models
{
    public class TopUser
    {
        public string fb_ID { get; set;}
        public string fb_Name { get; set;}
        public int fb_Count { get; set;}
        public int fb_shareCount { get; set; }
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string tw_ScreenResponse { get; set; }
        public int tw_FollowersNumber { get; set; }
        public int TweetNumber { get; set; }
        public string web_name { get; set; }
        public string web_id { get; set; }
        public int web_count { get; set; }
        public string cmt_name { get; set; }
        public string cmt_id { get; set; }
        public int cmt_count { get; set; }
        public int cmt_index { get; set; }
    }
}