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
    
    public partial class XBACKUP_Facebook_Comments_Old
    {
        public System.Guid ID { get; set; }
        public string UserID { get; set; }
        public string From { get; set; }
        public Nullable<System.Guid> PostID { get; set; }
        public string ObjectID { get; set; }
        public string Message { get; set; }
        public Nullable<System.DateTime> UpdateTime { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<int> PositiveCount { get; set; }
        public Nullable<int> NegativeCount { get; set; }
        public string Sentiment { get; set; }
        public Nullable<bool> Sentiment_Confirmed { get; set; }
        public Nullable<bool> Question { get; set; }
    }
}