using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialMonster.Models
{
    public class SystemUserModels
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Surename { get; set; }
        public string Picture { get; set; }
        public string Description { get; set; }
        public string FacebookAccount { get; set; }
        public string TwitterAccount { get; set; }
        public string Tittlename { get; set; }

        //bulgaa 2022-04-28 begin
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Website { get; set; }

        public string userID { get; set; }
        public int? Level { get; set; }
        public Nullable<System.Guid> parentID { get; set; }
        public int? childMenu { get; set; }

        //bulgaa 2022-04-28 end
    }
}