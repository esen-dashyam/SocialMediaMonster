
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 05/14/2019 17:08:43
-- Generated from EDMX file: D:\SocialMonster\SocialMonster\SocialMonster\DAL\MainModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [Monitoring];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Facebook.Groups]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Facebook.Groups];
GO
IF OBJECT_ID(N'[dbo].[System.Groups]', 'U') IS NOT NULL
    DROP TABLE [dbo].[System.Groups];
GO
IF OBJECT_ID(N'[dbo].[System.Keys]', 'U') IS NOT NULL
    DROP TABLE [dbo].[System.Keys];
GO
IF OBJECT_ID(N'[dbo].[System.KeyTypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[System.KeyTypes];
GO
IF OBJECT_ID(N'[dbo].[System.Person]', 'U') IS NOT NULL
    DROP TABLE [dbo].[System.Person];
GO
IF OBJECT_ID(N'[MonitoringModelStoreContainer].[Facebook_Pages]', 'U') IS NOT NULL
    DROP TABLE [MonitoringModelStoreContainer].[Facebook_Pages];
GO
IF OBJECT_ID(N'[MonitoringModelStoreContainer].[Facebook_Posts]', 'U') IS NOT NULL
    DROP TABLE [MonitoringModelStoreContainer].[Facebook_Posts];
GO
IF OBJECT_ID(N'[MonitoringModelStoreContainer].[System_View]', 'U') IS NOT NULL
    DROP TABLE [MonitoringModelStoreContainer].[System_View];
GO
IF OBJECT_ID(N'[MonitoringModelStoreContainer].[System_View_Type]', 'U') IS NOT NULL
    DROP TABLE [MonitoringModelStoreContainer].[System_View_Type];
GO
IF OBJECT_ID(N'[MonitoringModelStoreContainer].[Twitter_Tweets]', 'U') IS NOT NULL
    DROP TABLE [MonitoringModelStoreContainer].[Twitter_Tweets];
GO
IF OBJECT_ID(N'[MonitoringModelStoreContainer].[Twitter_Users]', 'U') IS NOT NULL
    DROP TABLE [MonitoringModelStoreContainer].[Twitter_Users];
GO
IF OBJECT_ID(N'[MonitoringModelStoreContainer].[WebSite_Posts]', 'U') IS NOT NULL
    DROP TABLE [MonitoringModelStoreContainer].[WebSite_Posts];
GO
IF OBJECT_ID(N'[MonitoringModelStoreContainer].[Websites]', 'U') IS NOT NULL
    DROP TABLE [MonitoringModelStoreContainer].[Websites];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Facebook_Groups'
CREATE TABLE [dbo].[Facebook_Groups] (
    [ID] uniqueidentifier  NOT NULL,
    [ObjectID] nvarchar(50)  NULL,
    [ObjectName] nvarchar(200)  NULL,
    [ProfileID] nvarchar(50)  NULL,
    [ProfileName] nvarchar(200)  NULL,
    [SourceName] nvarchar(200)  NULL,
    [CategorySectorID] uniqueidentifier  NULL,
    [CategoryOperationID] uniqueidentifier  NULL,
    [IsRealTimeMonitor] bit  NULL
);
GO

-- Creating table 'System_Groups'
CREATE TABLE [dbo].[System_Groups] (
    [ID] uniqueidentifier  NOT NULL,
    [Name] nvarchar(50)  NULL,
    [ParentID] uniqueidentifier  NULL
);
GO

-- Creating table 'System_Keys'
CREATE TABLE [dbo].[System_Keys] (
    [ID] uniqueidentifier  NOT NULL,
    [Text] nvarchar(100)  NULL,
    [KeyTypeID] uniqueidentifier  NULL,
    [PeopleID] uniqueidentifier  NULL
);
GO

-- Creating table 'System_KeyTypes'
CREATE TABLE [dbo].[System_KeyTypes] (
    [ID] uniqueidentifier  NOT NULL,
    [Name] nvarchar(100)  NULL
);
GO

-- Creating table 'System_Person'
CREATE TABLE [dbo].[System_Person] (
    [ID] uniqueidentifier  NOT NULL,
    [Name] nvarchar(100)  NULL,
    [GroupID] uniqueidentifier  NULL,
    [FBGroupID1] uniqueidentifier  NULL,
    [FBGroupID2] uniqueidentifier  NULL,
    [FBGroupID3] uniqueidentifier  NULL,
    [FBPagesID1] uniqueidentifier  NULL,
    [FBPagesID2] uniqueidentifier  NULL,
    [FBPagesID3] uniqueidentifier  NULL,
    [WebsitesID1] uniqueidentifier  NULL,
    [WebsitesID2] uniqueidentifier  NULL,
    [WebsitesID3] uniqueidentifier  NULL,
    [TWUsersID] uniqueidentifier  NULL,
    [TWUsersID1] uniqueidentifier  NULL,
    [Image] nvarchar(100)  NULL
);
GO

-- Creating table 'Facebook_Pages'
CREATE TABLE [dbo].[Facebook_Pages] (
    [ID] uniqueidentifier  NOT NULL,
    [Name] nvarchar(100)  NULL,
    [ObjectID] nvarchar(20)  NULL,
    [CreateTime] datetime  NULL,
    [CheckedDate] datetime  NULL,
    [PoliticalPartyID] uniqueidentifier  NULL,
    [IsRealTimeMonitor] bit  NULL
);
GO

-- Creating table 'Facebook_Posts'
CREATE TABLE [dbo].[Facebook_Posts] (
    [ID] uniqueidentifier  NOT NULL,
    [ParrentObjectID] nvarchar(50)  NULL,
    [PageID] uniqueidentifier  NULL,
    [GroupID] uniqueidentifier  NULL,
    [ObjectID] nvarchar(50)  NULL,
    [Link] nvarchar(400)  NULL,
    [Text] nvarchar(max)  NULL,
    [Story] nvarchar(200)  NULL,
    [TotalReactionCount] int  NULL,
    [WowCount] int  NULL,
    [AngryCount] int  NULL,
    [SadCount] int  NULL,
    [YayCount] int  NULL,
    [HahaCount] int  NULL,
    [LoveCount] int  NULL,
    [LikesCount] int  NULL,
    [SharesCount] int  NULL,
    [CheckedDate] datetime  NULL,
    [CreatedDate] datetime  NULL,
    [Sentiment] nvarchar(10)  NULL
);
GO

-- Creating table 'System_View'
CREATE TABLE [dbo].[System_View] (
    [ID] uniqueidentifier  NOT NULL,
    [Name] nvarchar(50)  NULL,
    [PeopleID1] uniqueidentifier  NOT NULL,
    [PeopleID2] uniqueidentifier  NOT NULL,
    [PeopleID3] uniqueidentifier  NOT NULL,
    [System_View_typeID] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'System_View_Type'
CREATE TABLE [dbo].[System_View_Type] (
    [ID] uniqueidentifier  NOT NULL,
    [Name] nvarchar(50)  NULL
);
GO

-- Creating table 'Twitter_Tweets'
CREATE TABLE [dbo].[Twitter_Tweets] (
    [ID] uniqueidentifier  NOT NULL,
    [Tittle] nvarchar(200)  NULL,
    [Text] nvarchar(500)  NULL,
    [ReTweetCount] int  NULL,
    [SortOrder] int  NULL,
    [Date] datetime  NULL,
    [Settlement] nvarchar(50)  NULL,
    [CandidateID] uniqueidentifier  NULL
);
GO

-- Creating table 'Twitter_Users'
CREATE TABLE [dbo].[Twitter_Users] (
    [ID] uniqueidentifier  NOT NULL,
    [UserID] nvarchar(300)  NULL,
    [UserName] nvarchar(100)  NULL,
    [SureName] nvarchar(100)  NULL,
    [Followers] int  NULL,
    [Following] int  NULL,
    [Likes] int  NULL,
    [TweetNumber] int  NULL,
    [Image] varbinary(max)  NULL,
    [Country] nvarchar(50)  NULL,
    [BirthDay] nvarchar(50)  NULL,
    [Url] nvarchar(50)  NULL,
    [JoinedDate] nvarchar(50)  NULL,
    [isChecked] bit  NULL,
    [ReadDate] datetime  NULL,
    [Level] int  NULL,
    [Status] nvarchar(50)  NULL
);
GO

-- Creating table 'WebSite_Posts'
CREATE TABLE [dbo].[WebSite_Posts] (
    [ID] uniqueidentifier  NOT NULL,
    [Link] nvarchar(800)  NULL,
    [Title] nvarchar(800)  NULL,
    [CommentNumber] int  NULL,
    [Text] nvarchar(max)  NULL,
    [SortOrder] int  NULL,
    [Date] datetime  NULL,
    [Settlement] nvarchar(50)  NULL,
    [CandidateID] uniqueidentifier  NULL
);
GO

-- Creating table 'Websites'
CREATE TABLE [dbo].[Websites] (
    [ID] uniqueidentifier  NOT NULL,
    [SiteName] nvarchar(100)  NULL,
    [Url] nvarchar(300)  NOT NULL,
    [Level] int  NOT NULL,
    [SiteColorCode] nvarchar(10)  NULL,
    [NewListTagID] nvarchar(100)  NULL,
    [HtmlTagID] nvarchar(100)  NULL,
    [TitleTagID] nvarchar(100)  NULL,
    [CoverTagID] nvarchar(100)  NULL,
    [BodyTagID] nvarchar(100)  NULL,
    [DateTimeTagID] nvarchar(100)  NULL,
    [ReporterTagID] nvarchar(100)  NULL,
    [CommentTagID] nvarchar(100)  NULL,
    [CommentInput] nvarchar(100)  NULL,
    [CommentSubmit] nvarchar(100)  NULL,
    [SortOrder] int  NULL,
    [Logo] varbinary(max)  NULL,
    [isHiddenHTML] bit  NULL,
    [isContainComments] bit  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [ID] in table 'Facebook_Groups'
ALTER TABLE [dbo].[Facebook_Groups]
ADD CONSTRAINT [PK_Facebook_Groups]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'System_Groups'
ALTER TABLE [dbo].[System_Groups]
ADD CONSTRAINT [PK_System_Groups]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'System_Keys'
ALTER TABLE [dbo].[System_Keys]
ADD CONSTRAINT [PK_System_Keys]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'System_KeyTypes'
ALTER TABLE [dbo].[System_KeyTypes]
ADD CONSTRAINT [PK_System_KeyTypes]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'System_Person'
ALTER TABLE [dbo].[System_Person]
ADD CONSTRAINT [PK_System_Person]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Facebook_Pages'
ALTER TABLE [dbo].[Facebook_Pages]
ADD CONSTRAINT [PK_Facebook_Pages]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Facebook_Posts'
ALTER TABLE [dbo].[Facebook_Posts]
ADD CONSTRAINT [PK_Facebook_Posts]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID], [PeopleID1], [PeopleID2], [PeopleID3], [System_View_typeID] in table 'System_View'
ALTER TABLE [dbo].[System_View]
ADD CONSTRAINT [PK_System_View]
    PRIMARY KEY CLUSTERED ([ID], [PeopleID1], [PeopleID2], [PeopleID3], [System_View_typeID] ASC);
GO

-- Creating primary key on [ID] in table 'System_View_Type'
ALTER TABLE [dbo].[System_View_Type]
ADD CONSTRAINT [PK_System_View_Type]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Twitter_Tweets'
ALTER TABLE [dbo].[Twitter_Tweets]
ADD CONSTRAINT [PK_Twitter_Tweets]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Twitter_Users'
ALTER TABLE [dbo].[Twitter_Users]
ADD CONSTRAINT [PK_Twitter_Users]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'WebSite_Posts'
ALTER TABLE [dbo].[WebSite_Posts]
ADD CONSTRAINT [PK_WebSite_Posts]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID], [Url], [Level] in table 'Websites'
ALTER TABLE [dbo].[Websites]
ADD CONSTRAINT [PK_Websites]
    PRIMARY KEY CLUSTERED ([ID], [Url], [Level] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------