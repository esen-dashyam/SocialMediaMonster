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
    
    public partial class Twitter_Users
    {
        public System.Guid ID { get; set; }
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string SureName { get; set; }
        public Nullable<int> Followers { get; set; }
        public Nullable<int> Following { get; set; }
        public Nullable<int> Likes { get; set; }
        public Nullable<int> TweetNumber { get; set; }
        public byte[] Image { get; set; }
        public string Country { get; set; }
        public string BirthDay { get; set; }
        public string Url { get; set; }
        public string JoinedDate { get; set; }
        public Nullable<bool> isChecked { get; set; }
        public Nullable<System.DateTime> ReadDate { get; set; }
        public Nullable<int> Level { get; set; }
        public string Status { get; set; }
        public Nullable<System.DateTime> CommentCheckedDate { get; set; }
    }
}