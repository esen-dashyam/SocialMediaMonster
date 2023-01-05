using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialMonster.Models
{
    public class Twitter
    {
        public System.Guid ID { get; set; }
        public Nullable<System.Guid> TwitterUserID { get; set; }
        public string TweetID { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public string ScreenName { get; set; }
        public string Source { get; set; }
        public string StatusID { get; set; }
        public Nullable<int> RetweetCount { get; set; }
        public Nullable<bool> Retweeted { get; set; }
        public string FullText { get; set; }
        public string Language { get; set; }
        public string Text { get; set; }
        public Nullable<bool> TrimUser { get; set; }
        public Nullable<bool> Truncated { get; set; }
        public Nullable<long> UserID { get; set; }
        public string HashEntity1 { get; set; }
        public string HashEntity2 { get; set; }
        public string HashEntity3 { get; set; }
        public string SymbolEntity1 { get; set; }
        public string SymbolEntity2 { get; set; }
        public string SymbolEntity3 { get; set; }
        public string UrlEntity1 { get; set; }
        public string UrlEntityDisplayUrl1 { get; set; }
        public string UrlEntityExpandedUrl1 { get; set; }
        public string UrlEntity2 { get; set; }
        public string UrlEntityDisplayUrl2 { get; set; }
        public string UrlEntityExpandedUrl2 { get; set; }
        public string UrlEntity3 { get; set; }
        public string UrlEntityDisplayUrl3 { get; set; }
        public string UrlEntityExpandedUrl3 { get; set; }
        public string MediaEntitiy1 { get; set; }
        public string MediaEntitiyDisplayUrl1 { get; set; }
        public string MediaEntitiyMediaUrl1 { get; set; }
        public string MediaEntitiyVideoInfo1 { get; set; }
        public string MediaEntity2 { get; set; }
        public string MediaEntitiyDisplayUrl2 { get; set; }
        public string MediaEntitiyMediaUrl2 { get; set; }
        public string MediaEntitiyVideoInfo2 { get; set; }
        public string MediaEntity3 { get; set; }
        public string MediaEntitiyDisplayUrl3 { get; set; }
        public string MediaEntitiyMediaUrl3 { get; set; }
        public string MediaEntitiyVideoInfo3 { get; set; }
        public string UserMentionEntity1 { get; set; }
        public string UserMentionEntityScreenName1 { get; set; }
        public string UserMentionEntity2 { get; set; }
        public string UserMentionEntityScreenName2 { get; set; }
        public string UserMentionEntity3 { get; set; }
        public string UserMentionEntityScreenName3 { get; set; }

        //user
        public Nullable<bool> ContributorsEnabled { get; set; }
        public Nullable<int> Count { get; set; }
        public Nullable<long> Cursor { get; set; }
        public Nullable<bool> DefaultProfile { get; set; }
        public Nullable<bool> DefaultProfileImage { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public Nullable<int> FavoritesCount { get; set; }
        public Nullable<bool> FollowRequestSent { get; set; }
        public Nullable<int> FollowersCount { get; set; }
        public Nullable<bool> Following { get; set; }
        public Nullable<int> FriendsCount { get; set; }
        public Nullable<bool> GeoEnabled { get; set; }
        public string ImageSize { get; set; }
        public Nullable<bool> IncludeEntities { get; set; }
        public Nullable<bool> IsTranslator { get; set; }
        public string Lang { get; set; }
        public string LangResponse { get; set; }
        public Nullable<int> ListedCount { get; set; }
        public string Location { get; set; }
        public string Name { get; set; }
        public Nullable<bool> Notifications { get; set; }
        public Nullable<int> Page { get; set; }
        public string ProfileBackgroundColor { get; set; }
        public string ProfileBackgroundImageUrl { get; set; }
        public string ProfileBackgroundImageUrlHttps { get; set; }
        public Nullable<bool> ProfileBackgroundTile { get; set; }
        public string ProfileBannerUrl { get; set; }
        public string ProfileImage { get; set; }
        public string ProfileImageUrl { get; set; }
        public string ProfileImageUrlHttps { get; set; }
        public string ProfileLinkColor { get; set; }
        public string ProfileSidebarBorderColor { get; set; }
        public string ProfileSidebarFillColor { get; set; }
        public string ProfileTextColor { get; set; }
        public Nullable<bool> ProfileUseBackgroundImage { get; set; }
        public Nullable<bool> Protected { get; set; }
        public string Query { get; set; }
        public string ScreenNameList { get; set; }
        public string ScreenNameResponse { get; set; }
        public Nullable<bool> ShowAllInlineMedia { get; set; }
        public Nullable<bool> SkipStatus { get; set; }
        public string Slug { get; set; }
        public Nullable<int> StatusesCount { get; set; }
        public string TimeZone { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }
        public string UserIDResponse { get; set; }
        public string UserIdList { get; set; }
        public Nullable<int> UtcOffset { get; set; }
        public Nullable<bool> Verified { get; set; }
        public string Sentiment { get; set; }
        public string Link { get; set; }
        public string TwFullPicture { get; set; }


        public string PerName { get; set; }
    }
}