using System;
using System.Collections.Generic;
using System.Linq;
using SocialMonster.DAL;

namespace SocialMonster.Models
{
    public partial class Group
    {
        public Nullable<System.Guid> ID { get; set; }
        public string Name { get; set; }
        public Nullable<System.Guid> ParentID { get; set; }
        public Nullable<int> Level { get; set; }
        public IQueryable<System_Groups> ChildGroup { get; internal set; }
        public Nullable<int> ChildCount { get; set; }
    }
}