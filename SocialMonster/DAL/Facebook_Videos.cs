//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SocialMonster.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class Facebook_Videos
    {
        public System.Guid ID { get; set; }
        public string ParrentID { get; set; }
        public string ParrentName { get; set; }
        public string PostID { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> Updated_Time { get; set; }
        public Nullable<int> TotalReactionCount { get; set; }
        public Nullable<int> WowCount { get; set; }
        public Nullable<int> AngryCount { get; set; }
        public Nullable<int> SadCount { get; set; }
        public Nullable<int> YayCount { get; set; }
        public Nullable<int> HahaCount { get; set; }
        public Nullable<int> LoveCount { get; set; }
        public Nullable<int> LikesCount { get; set; }
        public Nullable<int> SharesCount { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
    }
}
