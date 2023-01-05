using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialMonster.Models
{
    public class WebSitePost
    {
        public Nullable<System.Guid> ID { get; set; }
     
        public string Link { get; set; }
        public string Title { get; set; }
        public int CommentCount { get; set; }
        public string Text { get; set; }
        public string PicturePerson { get; set; }
        public int SortOrder { get; set; }
        public Nullable<DateTime> Date { get; set; }
        public Nullable<DateTime> DateTime { get; set; }
        public string Sentiment { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Body { get; set; }
        public string Reporter { get; set; }
        public string WbFullPicture { get; set; }
        public string CoverUrl { get; set; }
        public string PerName { get; set; }
        public string StringUrl { get; set; }
    }
}