using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialMonster.Models
{
    public class fbModel
    {
        public Nullable<System.Guid> ID { get; set; }
        public string Text { get; set; }
        public Nullable<DateTime> Date { get; set; }
        public string PerName { get; set; }
    }
}