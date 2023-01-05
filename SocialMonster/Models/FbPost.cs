using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialMonster.Models
{
    public class FbPost
    {
        public Nullable<System.Guid> ID { get; set; }
        public Nullable<System.Guid> PageID { get; set; }
        public Nullable<System.Guid> GroupID { get; set; }
        public string PostID { get; set; }
        public string FromID { get; set; }
        public string FromName { get; set; }
        public string Message { get; set; }
        public string Story { get; set; }
        public string Type { get; set; }
        public Nullable<DateTime>  UpdateTime { get; set; }
        public Nullable<Int32> SharedCount { get; set; }
        public string PermalinkUrl { get; set; }
        public string Caption { get; set; }
        public string Description { get; set; }
        public string FullPicture { get; set; }
        public string Link { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
        public string PicturePerson { get; set; }
        public string Icon { get; set; }
        public string ObjectID { get; set; }
        public string ParentID { get; set; }
        public string Sentiment { get; set; }
        public string PerName { get; set; }
        public int CommentCount { get; set; }
    }
}