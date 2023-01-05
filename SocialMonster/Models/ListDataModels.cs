using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialMonster.Models
{
    public class mixedDataModel
    {
        public Nullable<System.Guid> ID { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public Nullable<DateTime> Date { get; set; }
        public string URL { get; set; }
        public string FullPicture { get; set; }
        public string TwFullPicture { get; set; }
        public string WbFullPicture { get; set; }
        public string Icon { get; set; }
        public string Sentiment { get; set; }
        public string From { get; set; }
        public string FromID { get; set; }
        public string FromURL { get; set; }
        public string FromPicture { get; set; }

    }
    public class ListDataModel
    {
        public Nullable<System.Guid> ID { get; set; }
        public string Type { get; set; }
        public string fb_text { get; set; }
        public string tw_text { get; set; }
        public string wb_text { get; set; }

        public Nullable<DateTime> fb_date { get; set; }
        public Nullable<DateTime> tw_date { get; set; }
        public Nullable<DateTime> wb_date { get; set; }
        public string fb_url { get; set; }
        public string tw_url { get; set; }
        public string wb_url { get; set; }
        public string fb_picture { get; set; }
        public string tw_picture { get; set; }
        public string wb_picture { get; set; }
        public string fb_icon { get; set; }
        public string tw_icon { get; set; }
        public string wb_icon { get; set; }
        public string fb_sentiment { get; set; }
        public string tw_sentiment { get; set; }
        public string wb_sentiment { get; set; }
        public string fb_person { get; set; }
        public string tw_person { get; set; }
        public string wb_person { get; set; }
    }
}