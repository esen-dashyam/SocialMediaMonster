using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialMonster.Models
{
    public class HotTopic
    {
        public Nullable<System.Guid> ID { get; set; }
        public Nullable<System.Guid> GroupID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageURL { get; set; }
    }
}