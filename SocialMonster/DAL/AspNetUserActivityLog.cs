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
    
    public partial class AspNetUserActivityLog
    {
        public System.Guid ID { get; set; }
        public string UserName { get; set; }
        public string Activity { get; set; }
        public Nullable<System.DateTime> Time { get; set; }
    }
}