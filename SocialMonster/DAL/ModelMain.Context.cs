﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class MonitoringEntities : DbContext
    {
        public MonitoringEntities()
            : base("name=MonitoringEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<Facebook_CommentRemover> Facebook_CommentRemover { get; set; }
        public virtual DbSet<Facebook_Groups> Facebook_Groups { get; set; }
        public virtual DbSet<Facebook_Pages> Facebook_Pages { get; set; }
        public virtual DbSet<Facebook_Post_Comments> Facebook_Post_Comments { get; set; }
        public virtual DbSet<Facebook_Post_Hottopic> Facebook_Post_Hottopic { get; set; }
        public virtual DbSet<Facebook_Post_Person> Facebook_Post_Person { get; set; }
        public virtual DbSet<Facebook_Post_SubAttachment> Facebook_Post_SubAttachment { get; set; }
        public virtual DbSet<Facebook_Posts> Facebook_Posts { get; set; }
        public virtual DbSet<System_Groups> System_Groups { get; set; }
        public virtual DbSet<System_Hottopics> System_Hottopics { get; set; }
        public virtual DbSet<System_Keys> System_Keys { get; set; }
        public virtual DbSet<System_KeyTypes> System_KeyTypes { get; set; }
        public virtual DbSet<System_Person> System_Person { get; set; }
        public virtual DbSet<System_View> System_View { get; set; }
        public virtual DbSet<Twitter_Tweet_Hottopic> Twitter_Tweet_Hottopic { get; set; }
        public virtual DbSet<Twitter_Tweet_Person> Twitter_Tweet_Person { get; set; }
        public virtual DbSet<Twitter_Tweets> Twitter_Tweets { get; set; }
        public virtual DbSet<Twitter_User_Details> Twitter_User_Details { get; set; }
        public virtual DbSet<Twitter_Users> Twitter_Users { get; set; }
        public virtual DbSet<WebSite_Post_Comments> WebSite_Post_Comments { get; set; }
        public virtual DbSet<Website_Post_Hottopic> Website_Post_Hottopic { get; set; }
        public virtual DbSet<Website_Post_Person> Website_Post_Person { get; set; }
        public virtual DbSet<WebSite_PostList> WebSite_PostList { get; set; }
        public virtual DbSet<WebSite_Posts> WebSite_Posts { get; set; }
        public virtual DbSet<WebSites> WebSites { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual DbSet<Google_SearchResult> Google_SearchResult { get; set; }
        public virtual DbSet<System_Questions> System_Questions { get; set; }
    }
}
