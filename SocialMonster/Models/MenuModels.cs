using System;
using System.Collections.Generic;
using System.Linq;
using SocialMonster.DAL;

namespace SocialMonster.Models
{
    public partial class MenuModels
    {
        public Nullable<System.Guid> ID { get; set; }
        public int? Level { get; set; }
        public string Name { get; set; }
        public Nullable<System.Guid> parentID { get; set; }
        public int? childMenu { get; set; }
    }
}