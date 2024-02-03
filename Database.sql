--drop database hackishssharp;

if (not exists (select null from sys.databases where name = 'hackishssharp'))
    create database hackishssharp;
GO

use hackishssharp;

if (not exists(select * from sys.tables where name = 'User'))
    create table [User] (
        UserId int identity(1, 1) primary key,
        Email nvarchar(50),
        Password nvarchar(100),
        Salt nvarchar(50),
        FirstName nvarchar(50),
        LastName nvarchar(50),
        ProfileImage nvarchar(200),
        Status int
    )
GO

if (not exists(select * from sys.columns where name = 'NormilizedEmail' and Object_Id = Object_Id('User'))) 
    alter table [User] add NormilizedEmail varchar(255);
GO

if (not exists(select * from sys.indexes where name = 'IX_User_NormilizedEmail'))
    create index IX_User_NormilizedEmail on [User] (Email) 
GO
if (not exists(select * from sys.indexes where name = 'IX_User_Email'))
    create index IX_User_Email on [User] (Email)
GO


if (not exists(select * from sys.tables where name = 'FailedAttempt'))
    create table [FailedAttempt] (
        FailedAttemptId bigint identity(1, 1) primary key,
        Email nvarchar(50),
        UserId int null,
        IP nvarchar(100),
        Created datetime
    )
GO

if (not exists(select * from sys.indexes where name = 'IX_FailedAttempt_Email'))
    create index IX_FailedAttempt_Email on FailedAttempt (Email) include (Created)
GO


if (not exists(select * from sys.indexes where name = 'IX_FailedAttempt_Ip'))
    create index IX_FailedAttempt_Ip on FailedAttempt (Ip) include (Created)
GO

if (not exists(select * from sys.tables where name = 'UserSecurity'))
    create table UserSecurity (
        UserSecurityId int identity(1, 1) primary key,
        UserId int, 
        VerificationCode nvarchar(50)
    )
GO

if (not exists(select * from sys.indexes where name = 'IX_UserSecurity_UserId'))
    create index IX_UserSecurity_UserId on UserSecurity (UserId)
GO

if (not exists(select * from sys.tables where name = 'EmailQueue'))
    create table EmailQueue (
        EmailQueueId int identity(1, 1) primary key,
        EmailTo nvarchar(200),
        EmailFrom nvarchar(200),
        EmailSubject nvarchar(200), 
        EmailBody ntext,
        Created datetime,
        ProcessingId nvarchar(100),
        Retry int
    )
GO

if (not exists(select * from sys.tables where name = 'Blog'))
    create table Blog (
        BlogId int identity(1, 1) primary key,
        Title nvarchar(200),
        Content ntext,
        Created datetime,
        UserId nvarchar(100),
        Status int
    )
GO


if (not exists(select * from sys.tables where name = 'Session'))
    create table Session (
        SessionId UNIQUEIDENTIFIER primary key,
        Content ntext,
        Created datetime,
        LastAccessed datetime,
        UserId int
    )
GO


if (not exists(select * from sys.columns where name = 'ImageFile' and object_id = object_id('Blog')))
    alter table Blog add ImageFile varchar(255)
GO