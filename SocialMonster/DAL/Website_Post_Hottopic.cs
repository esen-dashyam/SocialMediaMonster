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
    
    public partial class Website_Post_Hottopic
    {
        public System.Guid ID { get; set; }
        public Nullable<System.Guid> PostID { get; set; }
        public Nullable<System.Guid> HottopicID { get; set; }
        public string Sentiment { get; set; }
        public Nullable<System.Guid> GroupID { get; set; }
    }
}
