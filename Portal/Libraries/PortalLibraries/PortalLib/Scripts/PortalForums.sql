IF EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'PortalForums')
	DROP DATABASE [PortalForums]
GO

CREATE DATABASE [PortalForums]
COLLATE Cyrillic_General_CI_AS
GO

exec sp_dboption N'PortalForums', N'autoclose', N'false'
GO

exec sp_dboption N'PortalForums', N'bulkcopy', N'false'
GO

exec sp_dboption N'PortalForums', N'trunc. log', N'false'
GO

exec sp_dboption N'PortalForums', N'torn page detection', N'true'
GO

exec sp_dboption N'PortalForums', N'read only', N'false'
GO

exec sp_dboption N'PortalForums', N'dbo use', N'false'
GO

exec sp_dboption N'PortalForums', N'single', N'false'
GO

exec sp_dboption N'PortalForums', N'autoshrink', N'false'
GO

exec sp_dboption N'PortalForums', N'ANSI null default', N'false'
GO

exec sp_dboption N'PortalForums', N'recursive triggers', N'false'
GO

exec sp_dboption N'PortalForums', N'ANSI nulls', N'false'
GO

exec sp_dboption N'PortalForums', N'concat null yields null', N'false'
GO

exec sp_dboption N'PortalForums', N'cursor close on commit', N'false'
GO

exec sp_dboption N'PortalForums', N'default to local cursor', N'false'
GO

exec sp_dboption N'PortalForums', N'quoted identifier', N'false'
GO

exec sp_dboption N'PortalForums', N'ANSI warnings', N'false'
GO

exec sp_dboption N'PortalForums', N'auto create statistics', N'true'
GO

exec sp_dboption N'PortalForums', N'auto update statistics', N'true'
GO

use [PortalForums]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ForumUserAttributes]') AND type in (N'U'))
DROP TABLE [dbo].[ForumUserAttributes]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Posts_Forums]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Posts] DROP CONSTRAINT FK_Posts_Forums
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_PrivateForums_UserRoles]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[PrivateForums] DROP CONSTRAINT FK_PrivateForums_UserRoles
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_UsersInRoles_UserRoles]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[UsersInRoles] DROP CONSTRAINT FK_UsersInRoles_UserRoles
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Moderators_Users]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Moderators] DROP CONSTRAINT FK_Moderators_Users
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Posts_Users]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Posts] DROP CONSTRAINT FK_Posts_Users
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_ThreadTrackings_Users]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[ThreadTrackings] DROP CONSTRAINT FK_ThreadTrackings_Users
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_UsersInRoles_Users]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[UsersInRoles] DROP CONSTRAINT FK_UsersInRoles_Users
GO

/****** Object:  User Defined Function dbo.HasReadPost    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[HasReadPost]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[HasReadPost]
GO

/****** Object:  Stored Procedure dbo.Reports_UserVisitsByDay    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Reports_UserVisitsByDay]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Reports_UserVisitsByDay]
GO

/****** Object:  Stored Procedure dbo.Statistics_GetMostActiveUsers    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Statistics_GetMostActiveUsers]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Statistics_GetMostActiveUsers]
GO

/****** Object:  Stored Procedure dbo.forums_AddForumGroup    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_AddForumGroup]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_AddForumGroup]
GO

/****** Object:  Stored Procedure dbo.forums_AddForumToRole    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_AddForumToRole]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_AddForumToRole]
GO

/****** Object:  Stored Procedure dbo.forums_AddModeratedForumForUser    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_AddModeratedForumForUser]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_AddModeratedForumForUser]
GO

/****** Object:  Stored Procedure dbo.forums_AddPost    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_AddPost]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_AddPost]
GO

/****** Object:  Stored Procedure dbo.forums_AddPostETC    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_AddPostETC]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_AddPostETC]
GO

/****** Object:  Stored Procedure dbo.forums_AddUserToRole    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_AddUserToRole]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_AddUserToRole]
GO

/****** Object:  Stored Procedure dbo.forums_ApproveModeratedPost    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_ApproveModeratedPost]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_ApproveModeratedPost]
GO

/****** Object:  Stored Procedure dbo.forums_ApprovePost    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_ApprovePost]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_ApprovePost]
GO

/****** Object:  Stored Procedure dbo.forums_CanModerate    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_CanModerate]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_CanModerate]
GO

/****** Object:  Stored Procedure dbo.forums_CanModerateForum    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_CanModerateForum]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_CanModerateForum]
GO

/****** Object:  Stored Procedure dbo.forums_DeleteForum    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_DeleteForum]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_DeleteForum]
GO

/****** Object:  Stored Procedure dbo.forums_DeleteModeratedPost    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_DeleteModeratedPost]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_DeleteModeratedPost]
GO

/****** Object:  Stored Procedure dbo.forums_DeletePost    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_DeletePost]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_DeletePost]
GO

/****** Object:  Stored Procedure dbo.forums_DeletePostAndChildren    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_DeletePostAndChildren]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_DeletePostAndChildren]
GO

/****** Object:  Stored Procedure dbo.forums_DeleteRole    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_DeleteRole]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_DeleteRole]
GO

/****** Object:  Stored Procedure dbo.forums_GetAllForumGroups    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_GetAllForumGroups]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_GetAllForumGroups]
GO

/****** Object:  Stored Procedure dbo.forums_GetAllForumGroupsForModeration    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_GetAllForumGroupsForModeration]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_GetAllForumGroupsForModeration]
GO

/****** Object:  Stored Procedure dbo.forums_GetAllForums    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_GetAllForums]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_GetAllForums]
GO

/****** Object:  Stored Procedure dbo.forums_GetForumByPostID    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_GetForumByPostID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_GetForumByPostID]
GO

/****** Object:  Stored Procedure dbo.forums_GetForumByThreadID    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_GetForumByThreadID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_GetForumByThreadID]
GO

/****** Object:  Stored Procedure dbo.forums_GetForumInfo    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_GetForumInfo]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_GetForumInfo]
GO

/****** Object:  Stored Procedure dbo.forums_GetForumModerators    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_GetForumModerators]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_GetForumModerators]
GO

/****** Object:  Stored Procedure dbo.forums_GetForumsByForumGroupId    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_GetForumsByForumGroupId]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_GetForumsByForumGroupId]
GO

/****** Object:  Stored Procedure dbo.forums_GetForumsForModerationByForumGroupId    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_GetForumsForModerationByForumGroupId]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_GetForumsForModerationByForumGroupId]
GO

/****** Object:  Stored Procedure dbo.forums_GetForumsModeratedByUser    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_GetForumsModeratedByUser]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_GetForumsModeratedByUser]
GO

/****** Object:  Stored Procedure dbo.forums_GetForumsNotModeratedByUser    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_GetForumsNotModeratedByUser]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_GetForumsNotModeratedByUser]
GO

/****** Object:  Stored Procedure dbo.forums_GetModeratedForums    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_GetModeratedForums]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_GetModeratedForums]
GO

/****** Object:  Stored Procedure dbo.forums_GetModeratedPosts    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_GetModeratedPosts]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_GetModeratedPosts]
GO

/****** Object:  Stored Procedure dbo.forums_GetModeratorsForEmailNotification    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_GetModeratorsForEmailNotification]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_GetModeratorsForEmailNotification]
GO

/****** Object:  Stored Procedure dbo.forums_GetPostInfo    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_GetPostInfo]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_GetPostInfo]
GO

/****** Object:  Stored Procedure dbo.forums_GetRolesByForum    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_GetRolesByForum]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_GetRolesByForum]
GO

/****** Object:  Stored Procedure dbo.forums_GetRolesByUser    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_GetRolesByUser]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_GetRolesByUser]
GO

/****** Object:  Stored Procedure dbo.forums_GetSingleMessage    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_GetSingleMessage]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_GetSingleMessage]
GO

/****** Object:  Stored Procedure dbo.forums_GetStatistics    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_GetStatistics]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_GetStatistics]
GO

/****** Object:  Stored Procedure dbo.forums_GetThreadByPostID    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_GetThreadByPostID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_GetThreadByPostID]
GO

/****** Object:  Stored Procedure dbo.forums_GetThreadByPostIDPaged    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_GetThreadByPostIDPaged]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_GetThreadByPostIDPaged]
GO

/****** Object:  Stored Procedure dbo.forums_GetThreadByPostIDPaged2    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_GetThreadByPostIDPaged2]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_GetThreadByPostIDPaged2]
GO

/****** Object:  Stored Procedure dbo.forums_GetTop25NewPosts    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_GetTop25NewPosts]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_GetTop25NewPosts]
GO

/****** Object:  Stored Procedure dbo.forums_GetTopicsUserIsTracking    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_GetTopicsUserIsTracking]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_GetTopicsUserIsTracking]
GO

/****** Object:  Stored Procedure dbo.forums_GetTrackingEmailsForThread    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_GetTrackingEmailsForThread]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_GetTrackingEmailsForThread]
GO

/****** Object:  Stored Procedure dbo.forums_GetUserInfo    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_GetUserInfo]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_GetUserInfo]
GO

/****** Object:  Stored Procedure dbo.forums_GetUsersByFirstCharacter    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_GetUsersByFirstCharacter]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_GetUsersByFirstCharacter]
GO

/****** Object:  Stored Procedure dbo.forums_GetUsersOnline    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_GetUsersOnline]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_GetUsersOnline]
GO

/****** Object:  Stored Procedure dbo.forums_IsUserTrackingPost    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_IsUserTrackingPost]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_IsUserTrackingPost]
GO

/****** Object:  Stored Procedure dbo.forums_MovePost    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_MovePost]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_MovePost]
GO

/****** Object:  Stored Procedure dbo.forums_RemoveForumFromRole    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_RemoveForumFromRole]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_RemoveForumFromRole]
GO

/****** Object:  Stored Procedure dbo.forums_RemoveModeratedForumForUser    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_RemoveModeratedForumForUser]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_RemoveModeratedForumForUser]
GO

/****** Object:  Stored Procedure dbo.forums_RemoveUserFromRole    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_RemoveUserFromRole]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_RemoveUserFromRole]
GO

/****** Object:  Stored Procedure dbo.forums_ReverseTrackingOption    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_ReverseTrackingOption]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_ReverseTrackingOption]
GO

/****** Object:  Stored Procedure dbo.forums_UpdateForum    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_UpdateForum]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_UpdateForum]
GO

/****** Object:  Stored Procedure dbo.forums_UserHasPostsAwaitingModeration    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_UserHasPostsAwaitingModeration]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_UserHasPostsAwaitingModeration]
GO

/****** Object:  Stored Procedure dbo.statistics_clearposts    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[statistics_clearposts]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[statistics_clearposts]
GO

/****** Object:  Stored Procedure dbo.statistics_updateUsageStats    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[statistics_updateUsageStats]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[statistics_updateUsageStats]
GO

/****** Object:  Stored Procedure dbo.Maintenance_CleanForumsRead    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Maintenance_CleanForumsRead]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Maintenance_CleanForumsRead]
GO

/****** Object:  Stored Procedure dbo.Maintenance_ResetForumGroupsForInsert    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Maintenance_ResetForumGroupsForInsert]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Maintenance_ResetForumGroupsForInsert]
GO

/****** Object:  Stored Procedure dbo.Moderate_GetPostHistory    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Moderate_GetPostHistory]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Moderate_GetPostHistory]
GO

/****** Object:  Stored Procedure dbo.Statistics_GetModerationActions    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Statistics_GetModerationActions]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Statistics_GetModerationActions]
GO

/****** Object:  Stored Procedure dbo.Statistics_GetMostActiveModerators    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Statistics_GetMostActiveModerators]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Statistics_GetMostActiveModerators]
GO

/****** Object:  Stored Procedure dbo.Statistics_ResetForumStatistics    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Statistics_ResetForumStatistics]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Statistics_ResetForumStatistics]
GO

/****** Object:  Stored Procedure dbo.Statistics_UpdateForumStatistics    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Statistics_UpdateForumStatistics]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Statistics_UpdateForumStatistics]
GO

/****** Object:  Stored Procedure dbo.forums_AddForum    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_AddForum]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_AddForum]
GO

/****** Object:  Stored Procedure dbo.forums_ChangeForumGroupSortOrder    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_ChangeForumGroupSortOrder]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_ChangeForumGroupSortOrder]
GO

/****** Object:  Stored Procedure dbo.forums_ChangeUserPassword    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_ChangeUserPassword]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_ChangeUserPassword]
GO

/****** Object:  Stored Procedure dbo.forums_CheckUserCredentials    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_CheckUserCredentials]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_CheckUserCredentials]
GO

/****** Object:  Stored Procedure dbo.forums_CreateNewRole    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_CreateNewRole]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_CreateNewRole]
GO

/****** Object:  Stored Procedure dbo.forums_CreateNewUser    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_CreateNewUser]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_CreateNewUser]
GO

/****** Object:  Stored Procedure dbo.forums_GetAllButOneForum    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_GetAllButOneForum]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_GetAllButOneForum]
GO

/****** Object:  Stored Procedure dbo.forums_GetAllForumsByForumGroupId    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_GetAllForumsByForumGroupId]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_GetAllForumsByForumGroupId]
GO

/****** Object:  Stored Procedure dbo.forums_GetAllMessages    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_GetAllMessages]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_GetAllMessages]
GO

/****** Object:  Stored Procedure dbo.forums_GetAllRoles    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_GetAllRoles]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_GetAllRoles]
GO

/****** Object:  Stored Procedure dbo.forums_GetAllTopicsPaged    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_GetAllTopicsPaged]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_GetAllTopicsPaged]
GO

/****** Object:  Stored Procedure dbo.forums_GetAnonymousUsersOnline    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_GetAnonymousUsersOnline]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_GetAnonymousUsersOnline]
GO

/****** Object:  Stored Procedure dbo.forums_GetBannedUsers    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_GetBannedUsers]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_GetBannedUsers]
GO

/****** Object:  Stored Procedure dbo.forums_GetEmailInfo    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_GetEmailInfo]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_GetEmailInfo]
GO

/****** Object:  Stored Procedure dbo.forums_GetEmailList    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_GetEmailList]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_GetEmailList]
GO

/****** Object:  Stored Procedure dbo.forums_GetForumGroupByForumID    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_GetForumGroupByForumID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_GetForumGroupByForumID]
GO

/****** Object:  Stored Procedure dbo.forums_GetForumGroupNameByID    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_GetForumGroupNameByID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_GetForumGroupNameByID]
GO

/****** Object:  Stored Procedure dbo.forums_GetForumMessageTemplateList    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_GetForumMessageTemplateList]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_GetForumMessageTemplateList]
GO

/****** Object:  Stored Procedure dbo.forums_GetForumViewByUsername    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_GetForumViewByUsername]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_GetForumViewByUsername]
GO

/****** Object:  Stored Procedure dbo.forums_GetMessage    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_GetMessage]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_GetMessage]
GO

/****** Object:  Stored Procedure dbo.forums_GetPostRead    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_GetPostRead]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_GetPostRead]
GO

/****** Object:  Stored Procedure dbo.forums_GetRoleDescription    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_GetRoleDescription]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_GetRoleDescription]
GO

/****** Object:  Stored Procedure dbo.forums_GetSummaryInfo    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_GetSummaryInfo]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_GetSummaryInfo]
GO

/****** Object:  Stored Procedure dbo.forums_GetTimezoneByUsername    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_GetTimezoneByUsername]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_GetTimezoneByUsername]
GO

/****** Object:  Stored Procedure dbo.forums_GetTotalNumberOfForums    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_GetTotalNumberOfForums]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_GetTotalNumberOfForums]
GO

/****** Object:  Stored Procedure dbo.forums_GetTotalUsers    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_GetTotalUsers]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_GetTotalUsers]
GO

/****** Object:  Stored Procedure dbo.forums_GetUsernameByEmail    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_GetUsernameByEmail]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_GetUsernameByEmail]
GO

/****** Object:  Stored Procedure dbo.forums_GetVoteResults    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_GetVoteResults]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_GetVoteResults]
GO

/****** Object:  Stored Procedure dbo.forums_MarkAllThreadsRead    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_MarkAllThreadsRead]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_MarkAllThreadsRead]
GO

/****** Object:  Stored Procedure dbo.forums_MarkPostAsRead    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_MarkPostAsRead]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_MarkPostAsRead]
GO

/****** Object:  Stored Procedure dbo.forums_ToggleOptions    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_ToggleOptions]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_ToggleOptions]
GO

/****** Object:  Stored Procedure dbo.forums_TopicCountForForum    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_TopicCountForForum]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_TopicCountForForum]
GO

/****** Object:  Stored Procedure dbo.forums_TrackAnonymousUsers    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_TrackAnonymousUsers]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_TrackAnonymousUsers]
GO

/****** Object:  Stored Procedure dbo.forums_UnbanUser    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_UnbanUser]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_UnbanUser]
GO

/****** Object:  Stored Procedure dbo.forums_UpdateEmailTemplate    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_UpdateEmailTemplate]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_UpdateEmailTemplate]
GO

/****** Object:  Stored Procedure dbo.forums_UpdateForumGroup    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_UpdateForumGroup]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_UpdateForumGroup]
GO

/****** Object:  Stored Procedure dbo.forums_UpdateMessageTemplateList    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_UpdateMessageTemplateList]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_UpdateMessageTemplateList]
GO

/****** Object:  Stored Procedure dbo.forums_UpdatePost    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_UpdatePost]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_UpdatePost]
GO

/****** Object:  Stored Procedure dbo.forums_UpdateRoleDescription    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_UpdateRoleDescription]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_UpdateRoleDescription]
GO

/****** Object:  Stored Procedure dbo.forums_UpdateUserFromAdminPage    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_UpdateUserFromAdminPage]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_UpdateUserFromAdminPage]
GO

/****** Object:  Stored Procedure dbo.forums_UpdateUserInfo    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_UpdateUserInfo]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_UpdateUserInfo]
GO

/****** Object:  Stored Procedure dbo.forums_Vote    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_Vote]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_Vote]
GO

/****** Object:  Stored Procedure dbo.Search_ForUser    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Search_ForUser]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Search_ForUser]
GO

/****** Object:  Stored Procedure dbo.Statistics_ResetTopPosters    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Statistics_ResetTopPosters]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Statistics_ResetTopPosters]
GO

/****** Object:  Stored Procedure dbo.forums_FindUsersByName    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_FindUsersByName]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_FindUsersByName]
GO

/****** Object:  Stored Procedure dbo.forums_GetAllUnmoderatedTopicsPaged    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_GetAllUnmoderatedTopicsPaged]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_GetAllUnmoderatedTopicsPaged]
GO

/****** Object:  Stored Procedure dbo.forums_GetAllUsers    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_GetAllUsers]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_GetAllUsers]
GO

/****** Object:  Stored Procedure dbo.forums_GetNextPostID    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_GetNextPostID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_GetNextPostID]
GO

/****** Object:  Stored Procedure dbo.forums_GetNextThreadID    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_GetNextThreadID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_GetNextThreadID]
GO

/****** Object:  Stored Procedure dbo.forums_GetParentID    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_GetParentID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_GetParentID]
GO

/****** Object:  Stored Procedure dbo.forums_GetPrevNextThreadID    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_GetPrevNextThreadID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_GetPrevNextThreadID]
GO

/****** Object:  Stored Procedure dbo.forums_GetPrevPostID    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_GetPrevPostID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_GetPrevPostID]
GO

/****** Object:  Stored Procedure dbo.forums_GetPrevThreadID    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_GetPrevThreadID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_GetPrevThreadID]
GO

/****** Object:  Stored Procedure dbo.forums_GetSearchResults    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_GetSearchResults]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_GetSearchResults]
GO

/****** Object:  Stored Procedure dbo.forums_GetThread    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_GetThread]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_GetThread]
GO

/****** Object:  Stored Procedure dbo.forums_GetThreadByParentID    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_GetThreadByParentID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_GetThreadByParentID]
GO

/****** Object:  Stored Procedure dbo.forums_GetThreadByPostIDPaged_BackUp    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_GetThreadByPostIDPaged_BackUp]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_GetThreadByPostIDPaged_BackUp]
GO

/****** Object:  Stored Procedure dbo.forums_GetTopicsUserMostRecentlyParticipatedIn    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_GetTopicsUserMostRecentlyParticipatedIn]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_GetTopicsUserMostRecentlyParticipatedIn]
GO

/****** Object:  Stored Procedure dbo.forums_GetTotalPostCount    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_GetTotalPostCount]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_GetTotalPostCount]
GO

/****** Object:  Stored Procedure dbo.forums_GetTotalPostsForThread    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_GetTotalPostsForThread]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_GetTotalPostsForThread]
GO

/****** Object:  Stored Procedure dbo.forums_GetUnmoderatedPostStatus    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_GetUnmoderatedPostStatus]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_GetUnmoderatedPostStatus]
GO

/****** Object:  Stored Procedure dbo.forums_GetUserGroups    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_GetUserGroups]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_GetUserGroups]
GO

/****** Object:  Stored Procedure dbo.forums_GetUserNameFromPostID    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_GetUserNameFromPostID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_GetUserNameFromPostID]
GO

/****** Object:  Stored Procedure dbo.forums_IsDuplicatePost    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_IsDuplicatePost]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_IsDuplicatePost]
GO

/****** Object:  Stored Procedure dbo.forums_SetLastCount  ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_SetLastCount]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_SetLastCount]
GO

/****** Object:  Stored Procedure dbo.forums_GetLastCount  ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[forums_GetLastCount]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[forums_GetLastCount]
GO

/****** Объект:  StoredProcedure [dbo].[forums_GetTotalPinnedPostsForThread]    Дата сценария: 10/31/2007 16:28:19 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[forums_GetTotalPinnedPostsForThread]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[forums_GetTotalPinnedPostsForThread]
GO
/****** Object:  Table [dbo].[Moderators]    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Moderators]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Moderators]
GO

/****** Object:  Table [dbo].[Posts]    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Posts]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Posts]
GO

/****** Object:  Table [dbo].[PrivateForums]    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[PrivateForums]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[PrivateForums]
GO

/****** Object:  Table [dbo].[ThreadTrackings]    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ThreadTrackings]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[ThreadTrackings]
GO

/****** Object:  Table [dbo].[UsersInRoles]    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UsersInRoles]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[UsersInRoles]
GO

/****** Object:  Table [dbo].[AnonymousUsers]    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[AnonymousUsers]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[AnonymousUsers]
GO

/****** Object:  Table [dbo].[Emails]    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Emails]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Emails]
GO

/****** Object:  Table [dbo].[ForumGroups]    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ForumGroups]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[ForumGroups]
GO

/****** Object:  Table [dbo].[Forums]    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Forums]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Forums]
GO

/****** Object:  Table [dbo].[ForumsRead]    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ForumsRead]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[ForumsRead]
GO

/****** Object:  Table [dbo].[Messages]    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Messages]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Messages]
GO

/****** Object:  Table [dbo].[ModerationAction]    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ModerationAction]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[ModerationAction]
GO

/****** Object:  Table [dbo].[ModerationAudit]    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ModerationAudit]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[ModerationAudit]
GO

/****** Object:  Table [dbo].[Post_Archive]    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Post_Archive]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Post_Archive]
GO

/****** Object:  Table [dbo].[PostsRead]    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[PostsRead]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[PostsRead]
GO

/****** Object:  Table [dbo].[UserRoles]    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UserRoles]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[UserRoles]
GO

/****** Object:  Table [dbo].[ForumUsers]    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ForumUsers]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[ForumUsers]
GO

/****** Object:  Table [dbo].[Vote]    Script Date: 20.02.2003 20:33:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Vote]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Vote]
GO









-- СОЗДАЁМ





/****** Объект:  Table [dbo].[ForumUserAttributes]    Дата сценария: 09/28/2007 15:15:47 ******/

CREATE TABLE [dbo].[ForumUserAttributes](
	[UserID] [int] NOT NULL,
	[IsForumAdmin] [bit] NOT NULL,
	[IsBanned] [bit] NOT NULL,
	[LastCount] [int],
 CONSTRAINT [PK_ForumUserAttributes] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO



/****** Object:  Table [dbo].[Forums]    Script Date: 20.02.2003 20:33:47 ******/
CREATE TABLE [dbo].[Forums] (
	[ForumID] [int] IDENTITY (1, 1) NOT NULL ,
	[Name] [nvarchar] (100) COLLATE Cyrillic_General_CI_AS NOT NULL ,
	[Description] [nvarchar] (3000) COLLATE Cyrillic_General_CI_AS NOT NULL ,
	[DateCreated] [datetime] NOT NULL ,
	[TotalPosts] [int] NOT NULL ,
	[TotalThreads] [int] NOT NULL ,
	[MostRecentPostID] [int] NOT NULL ,
	[MostRecentThreadID] [int] NOT NULL ,
	[MostRecentPostDate] [datetime] NULL ,
	[MostRecentPostAuthorID] [int]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[ForumsRead]    Script Date: 20.02.2003 20:33:48 ******/
CREATE TABLE [dbo].[ForumsRead] (
	[ForumId] [int] NOT NULL ,
	[UserID] [int] NOT NULL ,
	[MarkReadAfter] [int] NOT NULL ,
	[LastActivity] [datetime] NOT NULL 
) ON [PRIMARY]
GO


/****** Object:  Table [dbo].[PostsRead]    Script Date: 20.02.2003 20:33:48 ******/
CREATE TABLE [dbo].[PostsRead] (
	[UserID] [int] NOT NULL ,
	[PostId] [int] NOT NULL ,
	[HasRead] [bit] NOT NULL 
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Posts]    Script Date: 20.02.2003 20:33:50 ******/
CREATE TABLE [dbo].[Posts] (
	[PostID] [int] IDENTITY (1, 1) NOT NULL ,
	[ThreadID] [int] NOT NULL ,
	[ParentID] [int] NOT NULL ,
	[Subject] [nvarchar] (256) COLLATE Cyrillic_General_CI_AS NOT NULL ,
	[PostDate] [datetime] NOT NULL ,
	[ForumID] [int] NOT NULL ,
	[UserID] [int] NOT NULL ,
	[ThreadDate] [datetime] NOT NULL ,
	[TotalViews] [int] NOT NULL ,
	[IsLocked] [bit] NOT NULL ,
	[IsPinned] [bit] NOT NULL ,
	[IsPostPinned] [bit] NOT NULL,
	[PinnedDate] [datetime] NULL ,
	[Body] [nvarchar] (max) COLLATE Cyrillic_General_CI_AS NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

/****** Object:  Table [dbo].[ThreadTrackings]    Script Date: 20.02.2003 20:33:50 ******/
CREATE TABLE [dbo].[ThreadTrackings] (
	[ThreadID] [int] NOT NULL ,
	[UserID] [int] NOT NULL 
) ON [PRIMARY]
GO






-- ОГРАНИЧЕНИЯ








ALTER TABLE [dbo].[Forums] WITH NOCHECK ADD 
	CONSTRAINT [PK_Forums] PRIMARY KEY  CLUSTERED 
	(
		[ForumID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[Posts] WITH NOCHECK ADD 
	CONSTRAINT [PK_Posts] PRIMARY KEY  CLUSTERED 
	(
		[PostID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[ThreadTrackings] WITH NOCHECK ADD 
	CONSTRAINT [PK_ThreadTrackings] PRIMARY KEY  CLUSTERED 
	(
		[ThreadID],
		[UserID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[Forums] WITH NOCHECK ADD 
	CONSTRAINT [DF_Forums_DateCreated] DEFAULT (getdate()) FOR [DateCreated],
	CONSTRAINT [DF_Forums_TotalPosts] DEFAULT (0) FOR [TotalPosts],
	CONSTRAINT [DF_Forums_TotalThreads] DEFAULT (0) FOR [TotalThreads],
	CONSTRAINT [DF_Forums_MostRecentPostID] DEFAULT (0) FOR [MostRecentPostID],
	CONSTRAINT [DF_Forums_MostRecentThreadID] DEFAULT (0) FOR [MostRecentThreadID],
	CONSTRAINT [DF_Forums_MostRecentPostAuthorID] DEFAULT (0) FOR [MostRecentPostAuthorID]
GO

ALTER TABLE [dbo].[ForumsRead] WITH NOCHECK ADD 
	CONSTRAINT [DF_ForumsReadByDate_MarkReadAfter] DEFAULT (0) FOR [MarkReadAfter],
	CONSTRAINT [DF_ForumsRead_LastActivity] DEFAULT (getdate()) FOR [LastActivity]
GO

ALTER TABLE [dbo].[PostsRead] WITH NOCHECK ADD 
	CONSTRAINT [DF_PostsReadDateByUser_HasRead] DEFAULT (1) FOR [HasRead]
GO

ALTER TABLE [dbo].[Posts] WITH NOCHECK ADD 
	CONSTRAINT [DF_Posts_ThreadID] DEFAULT (0) FOR [ThreadID],
	CONSTRAINT [DF_Posts_ParentID] DEFAULT (0) FOR [ParentID],
	CONSTRAINT [DF_Posts_PostDate] DEFAULT (getdate()) FOR [PostDate],
	CONSTRAINT [DF_Posts_ForumID] DEFAULT (0) FOR [ForumID],
	CONSTRAINT [DF_Posts_ThreadDate] DEFAULT (getdate()) FOR [ThreadDate],
	CONSTRAINT [DF_Posts_TotalViews] DEFAULT (0) FOR [TotalViews],
	CONSTRAINT [DF_Posts_IsLocked] DEFAULT (0) FOR [IsLocked],
	CONSTRAINT [DF_Posts_IsPinned] DEFAULT (0) FOR [IsPinned]
GO

 CREATE  INDEX [IX_ForumsReadByDate] ON [dbo].[ForumsRead]([ForumId]) ON [PRIMARY]
GO

 CREATE  INDEX [IX_ForumsReadByDate_1] ON [dbo].[ForumsRead]([UserID]) ON [PRIMARY]
GO

 CREATE  INDEX [IX_PostsRead] ON [dbo].[PostsRead]([PostId]) ON [PRIMARY]
GO

 CREATE  INDEX [IX_PostsRead_1] ON [dbo].[PostsRead]([UserID]) ON [PRIMARY]
GO

 CREATE  INDEX [IX_PostsRead_2] ON [dbo].[PostsRead]([UserID]) ON [PRIMARY]
GO

 CREATE  INDEX [IX_Posts_ParentID] ON [dbo].[Posts]([ParentID]) ON [PRIMARY]
GO

 CREATE  INDEX [IX_Posts_ThreadID] ON [dbo].[Posts]([ThreadID]) ON [PRIMARY]
GO

 CREATE  INDEX [IX_Posts_ForumID] ON [dbo].[Posts]([ForumID]) ON [PRIMARY]
GO

 CREATE  INDEX [IX_Posts_UserID] ON [dbo].[Posts]([UserID]) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Posts] ADD 
	CONSTRAINT [FK_Posts_Forums] FOREIGN KEY 
	(
		[ForumID]
	) REFERENCES [dbo].[Forums] (
		[ForumID]
	)

GO



SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO












-- Хранимые процедуры


GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored Procedure dbo.Statistics_UpdateForumStatistics    Script Date: 20.02.2003 20:33:52 ******/

CREATE  procedure Statistics_UpdateForumStatistics
(
	@ForumID int,
	@ThreadID int,
	@PostID int
)
AS
BEGIN
--DECLARE @Username nvarchar(50)
DECLARE @UserID int
DECLARE @PostDate datetime
DECLARE @TotalPosts int
DECLARE @TotalThreads int

-- Get values necessary to update the forum statistics
SELECT
	@UserID = UserID,
	@PostDate = PostDate,
	@TotalPosts = (SELECT COUNT(*) FROM Posts P2 (nolock) WHERE P2.ForumID = P.ForumID),
	@TotalThreads = (SELECT COUNT(*) FROM Posts P2 (nolock) WHERE P2.ForumID = P.ForumID AND P2.ParentID=0)
FROM
	Posts P
WHERE
	PostID = @PostID
	IF @TotalPosts IS NULL
	SET @TotalPosts=0

IF @TotalThreads IS NULL
	SET @TotalThreads=0

IF @PostID IS NULL
	SET @PostID=0

IF @ThreadID IS NULL
	SET @ThreadID=0

IF @UserID IS NULL
	SET @UserID=0

-- Do the update within a transaction
BEGIN TRAN

	UPDATE 
		Forums
	SET
		TotalPosts = @TotalPosts,
		TotalThreads = @TotalThreads,
		MostRecentPostID = @PostID,
		MostRecentThreadID = @ThreadID,
		MostRecentPostDate = @PostDate,
		MostRecentPostAuthorID = @UserID
	WHERE
		ForumID = @ForumID

COMMIT TRAN

END



GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO






/****** Object:  Stored Procedure dbo.Statistics_ResetForumStatistics    Script Date: 20.02.2003 20:33:52 ******/

CREATE  procedure Statistics_ResetForumStatistics
(
@ForumID int
)
AS
BEGIN
DECLARE @ForumCount int
DECLARE @ThreadID int
DECLARE @PostID int
set @ForumCount = 1


IF @ForumID = 0
  WHILE @ForumCount < (SELECT Max(ForumID) FROM FORUMS)
  BEGIN


	IF EXISTS(SELECT ForumID FROM Forums WHERE ForumID = @ForumCount)
	BEGIN
		SELECT TOP 1 @ThreadID = ThreadID, @PostID = PostID FROM Posts WHERE ForumID = @ForumCount ORDER BY PostID DESC
		IF @ThreadID IS NOT NULL
			exec Statistics_UpdateForumStatistics @ForumCount, @ThreadID, @PostID
	END

	SET @ForumCount = @ForumCount + 1
	SET @ThreadID = NULL


  END
ELSE
	SELECT TOP 1 @ThreadID = ThreadID, @PostID = PostID FROM Posts WHERE ForumID = @ForumID ORDER BY PostID DESC
	exec Statistics_UpdateForumStatistics @ForumID, @ThreadID, @PostID
END



GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


/****** Object:  Stored Procedure dbo.forums_DeletePostAndChildren    Script Date: 20.02.2003 20:33:53 ******/



CREATE   PROCEDURE forums_DeletePostAndChildren
(
	@PostID	int
)
 AS
	DECLARE @ChildPostID int
	DECLARE @UserID int
	DECLARE @ForumID int

	-- Build a cursor to loop through all the children of this post
	DECLARE c1 CURSOR LOCAL FOR
		SELECT PostID 
		FROM Posts
		WHERE ParentID = @PostID
	OPEN c1
	FETCH NEXT FROM c1
	INTO @ChildPostID
	WHILE @@FETCH_STATUS = 0
	  BEGIN
		exec dbo.forums_DeletePostAndChildren @ChildPostID
		FETCH NEXT FROM c1
		INTO @ChildPostID
	  END

	-- now, go ahead and delete the post
	SELECT 
		@UserID = UserID,
		@ForumID = ForumID
	FROM 
		Posts
	WHERE
		PostID = @PostID


	-- Now, delete the post
	DELETE 
		Posts 
	WHERE 
		PostID = @PostID

	-- Update the forum statistics
	exec Statistics_ResetForumStatistics @ForumID



GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.forums_SetLastCount    Script Date: 20.02.2003 20:33:51 ******/



create procedure forums_SetLastCount
(
	@UserID int,
	@Count int
)
 AS
BEGIN

IF NOT EXISTS (SELECT UserID FROM ForumUserAttributes WHERE UserID = @UserID)
BEGIN
	BEGIN TRAN
        INSERT INTO 
            ForumUserAttributes
        VALUES
        (
            @UserID,
            0,
			0,
			@Count
        )
    COMMIT TRAN
END

	UPDATE
		ForumUserAttributes
	SET 
		LastCount = @Count
	WHERE
		UserID = @UserID


END





GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.forums_GetLastCount    Script Date: 20.02.2003 20:33:51 ******/



create procedure forums_GetLastCount
(
	@UserID int
)
 AS
BEGIN


IF (SELECT COUNT(*) FROM ForumUserAttributes WHERE UserID = @UserID) = 0
	SELECT 25
ELSE
	SELECT LastCount FROM ForumUserAttributes WHERE UserID = @UserID

END






GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.forums_GetParentID    Script Date: 20.02.2003 20:33:51 ******/



create procedure forums_GetParentID
(
	@PostID	int
)
 AS
	SELECT ParentID
	FROM Posts (nolock)
	WHERE PostID = @PostID






GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored Procedure dbo.forums_GetSearchResults    Script Date: 20.02.2003 20:33:51 ******/

CREATE   PROCEDURE forums_GetSearchResults
(
	@SearchTerms	nvarchar(500),
	@Page int,
	@RecsPerPage int,
	@UserID int
)
 AS
	CREATE TABLE #tmp
	(
		ID int IDENTITY,
		PostID int
	)
	DECLARE @sql nvarchar(1000)
	SET NOCOUNT ON
	SELECT @sql = 'INSERT INTO #tmp(PostID) SELECT PostID ' + 
			'FROM Posts P (nolock) INNER JOIN Forums F (nolock) ON F.ForumID = P.ForumID ' +
			@SearchTerms + ' ORDER BY ThreadDate DESC'
	EXEC(@sql)
	-- ok, all of the rows are inserted into the table.
	-- now, select the correct subset
	DECLARE @FirstRec int, @LastRec int
	SELECT @FirstRec = (@Page - 1) * @RecsPerPage
	SELECT @LastRec = (@Page * @RecsPerPage + 1)
	DECLARE @MoreRecords int
	SELECT @MoreRecords = COUNT(*)  FROM #tmp -- WHERE ID >= @LastRec


	-- Select the data out of the temporary table
	IF @UserID IS NOT NULL
		SELECT
			T.PostID,
			P.ParentID,
			P.ThreadID,
			P.UserID,
			P.Subject,
			P.PostDate,
			P.ThreadDate,
			P.ForumID,
			F.Name As ForumName,
			MoreRecords = @MoreRecords,
			Replies = (SELECT COUNT(*) FROM Posts P2 (nolock) WHERE P2.ParentID = P.PostID ),
			P.Body,
			P.TotalViews,
			P.IsLocked,
			P.IsPostPinned,
			HasRead = 0 -- not used
		FROM 
			#tmp T
			INNER JOIN Posts P (nolock) ON
				P.PostID = T.PostID
			INNER JOIN Forums F (nolock) ON
				F.ForumID = P.ForumID
		WHERE 
			T.ID > @FirstRec AND T.ID < @LastRec
	ELSE
		SELECT
			T.PostID,
			P.ParentID,
			P.ThreadID,
			P.UserID,
			P.Subject,
			P.PostDate,
			P.ThreadDate,
			P.ForumID,
			F.Name As ForumName,
			MoreRecords = @MoreRecords,
			Replies = (SELECT COUNT(*) FROM Posts P2 (nolock) WHERE P2.ParentID = P.PostID),
			P.Body,
			P.TotalViews,
			P.IsLocked,
			HasRead = 0 -- not used
		FROM 
			#tmp T
			INNER JOIN Posts P (nolock) ON
				P.PostID = T.PostID
			INNER JOIN Forums F (nolock) ON
				F.ForumID = P.ForumID
		WHERE 
			T.ID > @FirstRec AND T.ID < @LastRec

	SET NOCOUNT OFF


























GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.forums_GetThread    Script Date: 20.02.2003 20:33:51 ******/



create procedure forums_GetThread
(
	@ThreadID int
) AS
SELECT
	P.PostID,
	P.ForumID,
	P.Subject,
	P.ParentID,
	P.ThreadID,
	P.PostDate,
	P.ThreadDate,
	P.UserID,
	Replies = (SELECT COUNT(*) FROM Posts P2 (nolock) WHERE P2.ParentID = P.PostID),
	P.Body
FROM Posts P (nolock)
WHERE P.ThreadID = @ThreadID
ORDER BY P.PostDate















GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored Procedure dbo.forums_GetThreadByParentID    Script Date: 20.02.2003 20:33:51 ******/



CREATE    PROCEDURE forums_GetThreadByParentID
(
	@ParentID	int
)
 AS
BEGIN
	SELECT 
		PostID,
		ThreadID,
		ForumID,
		Subject,
		ParentID,
		PostDate,
		ThreadDate,
		P.UserID,
		Replies = (SELECT COUNT(*) FROM Posts P2 (nolock) WHERE P2.ParentID = P.PostID),
		Body,
		TotalMessagesInThread = 0, -- not used
		TotalViews,
		IsLocked
	FROM
		Posts P
	WHERE
		ParentID = @ParentID

END








GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored Procedure dbo.forums_GetTopicsUserMostRecentlyParticipatedIn    Script Date: 20.02.2003 20:33:51 ******/



create procedure forums_GetTopicsUserMostRecentlyParticipatedIn
(
@UserID int,
@Count int
)
AS

-- Create a temp table
CREATE Table #ThreadsUserParticipatedIn (
	ThreadID int,
	ThreadDate datetime
)

-- Insert into temp table
INSERT INTO #ThreadsUserParticipatedIn
SELECT DISTINCT TOP (@Count)
	ThreadID, 
	ThreadDate 
FROM 
	Posts
WHERE 
	UserID = @UserID 
ORDER BY
	ThreadDate DESC

SELECT 
	Subject,
	Body,
	P.PostID,
	P.ThreadID,
	ParentID,
	PostDate = (SELECT Max(PostDate) FROM Posts WHERE ThreadID = P.ThreadID),
	P.ThreadDate,
	PinnedDate,
	P.UserID,
	Replies = (SELECT COUNT(*) FROM Posts WHERE P.ThreadID = ThreadID AND  PostID != ThreadID),
	Body,
	TotalViews,
	IsLocked,
	IsPinned,
	HasRead = dbo.HasReadPost(@UserID, P.PostID, P.ForumID),
	MostRecentPostAuthorID = (SELECT TOP 1 UserID FROM Posts WHERE P.ThreadID = ThreadID ORDER BY PostDate DESC),
	MostRecentPostID = (SELECT TOP 1 PostID FROM Posts WHERE P.ThreadID = ThreadID ORDER BY PostDate DESC)
FROM
	Posts P,
	#ThreadsUserParticipatedIn T
WHERE
    PostID = T.ThreadID AND 
	UserID=@UserID







GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.forums_GetTotalPostCount    Script Date: 20.02.2003 20:33:51 ******/



Create  PROCEDURE forums_GetTotalPostCount
 AS
	SELECT
	  Count(*)
	FROM
	  Posts








GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.forums_GetTotalPostsForThread    Script Date: 20.02.2003 20:33:52 ******/



Create  PROCEDURE forums_GetTotalPostsForThread
(
	@PostID	int
)
 AS

	BEGIN

	DECLARE @T int
	SELECT @T = (SELECT ThreadID FROM Posts WHERE PostID = @PostID)

	-- Get the count of posts for a given thread
	SELECT
		TotalPostsForThread = COUNT(PostID)
	FROM
		Posts (nolock)
	WHERE
		ThreadID = @T
		
	END













GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO




/****** Object:  Stored Procedure dbo.forums_IsDuplicatePost    Script Date: 20.02.2003 20:33:52 ******/



create procedure forums_IsDuplicatePost
(
	@UserID int,
	@Body ntext
)
 AS
	SELECT COUNT(*)
	FROM Posts (nolock)
	WHERE UserID=@UserID AND Body LIKE @Body




GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored Procedure dbo.Maintenance_CleanForumsRead    Script Date: 20.02.2003 20:33:52 ******/

-- for all other [Portal].[dbo].[Users] mark forum as not read
create procedure Maintenance_CleanForumsRead
(
	@ForumID int
)
AS
BEGIN
	DELETE
		ForumsRead
	WHERE
		MarkReadAfter = 0 AND
		ForumID = @ForumID

END



GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.forums_AddForum    Script Date: 20.02.2003 20:33:52 ******/



CREATE  PROCEDURE forums_AddForum
(
	@Name		nvarchar(100),
	@Description	nvarchar(3000)
)
 AS
	-- insert a new forum
	INSERT INTO Forums (Name, Description)
	VALUES (@Name, @Description)







GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO



/****** Object:  Stored Procedure dbo.forums_GetAllTopicsPaged    Script Date: 20.02.2003 20:33:52 ******/

CREATE                   PROCEDURE forums_GetAllTopicsPaged
(
	@ForumID int,
	@PageSize int,
	@PageIndex int, 
	@DateFilter Datetime,    -- Filter returned records by date 
	@UserID int,
	@UnReadTopicsOnly bit,    -- 0 All / 1 Unread only
	@SortBy int,
	@SortOrder int
)
AS
BEGIN

DECLARE @PageLowerBound int
DECLARE @PageUpperBound int

-- Set the page bounds
SET @PageLowerBound = @PageSize * @PageIndex
SET @PageUpperBound = @PageLowerBound + @PageSize + 1

-- Create a temp table to store the select results
CREATE TABLE #PageIndex 
(
	IndexId int IDENTITY (1, 1) NOT NULL,
	PostID int,
	PostDate datetime,
	Views int,
	Replies int,
	Auth int
)


IF (@SortOrder = 0)
BEGIN
	IF(@SortBy = 0)
		INSERT INTO #PageIndex
		SELECT 
			PostID,
			PinnedDate = (SELECT Max(PinnedDate) FROM Posts WHERE ThreadID = P.ThreadID),
			TotalViews as Views,
			Replies = (SELECT COUNT(*) FROM Posts WHERE P.ThreadID = ThreadID AND  ParentID != 0),
			Auth = P.UserID
		FROM 
			Posts P 
		WHERE 
			ParentID = 0 AND 
			ForumID = @ForumID AND 
			
			ThreadDate >= @DateFilter
		ORDER BY 
			Subject ASC

	IF(@SortBy = 1)
		INSERT INTO #PageIndex
		SELECT 
			PostID,
			PinnedDate = (SELECT Max(PinnedDate) FROM Posts WHERE ThreadID = P.ThreadID),
			TotalViews as Views,
			Replies = (SELECT COUNT(*) FROM Posts WHERE P.ThreadID = ThreadID AND  ParentID != 0),
			Auth = P.UserID
		FROM 
			Posts P 
		WHERE 
			ParentID = 0 AND 
			ForumID = @ForumID AND 
			
			ThreadDate >= @DateFilter
		ORDER BY 
			Auth ASC

IF(@SortBy = 2)
		INSERT INTO #PageIndex
		SELECT 
			PostID,
			PinnedDate = (SELECT Max(PinnedDate) FROM Posts WHERE ThreadID = P.ThreadID),
			TotalViews as Views,
			Replies = (SELECT COUNT(*) FROM Posts WHERE P.ThreadID = ThreadID AND  ParentID != 0),
			Auth = P.UserID
		FROM 
			Posts P 
		WHERE 
			ParentID = 0 AND 
			ForumID = @ForumID AND 
			
			ThreadDate >= @DateFilter
		ORDER BY 
			Replies ASC

IF(@SortBy = 3)
		INSERT INTO #PageIndex
		SELECT 
			PostID,
			PinnedDate = (SELECT Max(PinnedDate) FROM Posts WHERE ThreadID = P.ThreadID),
			TotalViews as Views,
			Replies = (SELECT COUNT(*) FROM Posts WHERE P.ThreadID = ThreadID AND  ParentID != 0),
			Auth = P.UserID
		FROM 
			Posts P 
		WHERE 
			ParentID = 0 AND 
			ForumID = @ForumID AND 
			
			ThreadDate >= @DateFilter
		ORDER BY 
			TotalViews ASC

IF(@SortBy = 4)
		INSERT INTO #PageIndex
		SELECT 
			PostID,
			PinnedDate = (SELECT Max(PinnedDate) FROM Posts WHERE ThreadID = P.ThreadID),
			TotalViews as Views,
			Replies = (SELECT COUNT(*) FROM Posts WHERE P.ThreadID = ThreadID AND  ParentID != 0),
			Auth = P.UserID
		FROM 
			Posts P 
		WHERE 
			ParentID = 0 AND 
			ForumID = @ForumID AND 
			
			ThreadDate >= @DateFilter
		ORDER BY 
			PinnedDate ASC

END
ELSE
BEGIN
	IF(@SortBy = 0)
		INSERT INTO #PageIndex
		SELECT 
			PostID,
			PinnedDate = (SELECT Max(PinnedDate) FROM Posts WHERE ThreadID = P.ThreadID),
			TotalViews as Views,
			Replies = (SELECT COUNT(*) FROM Posts WHERE P.ThreadID = ThreadID AND  ParentID != 0),
			Auth = P.UserID
		FROM 
			Posts P 
		WHERE 
			ParentID = 0 AND 
			ForumID = @ForumID AND 
			
			ThreadDate >= @DateFilter
		ORDER BY 
			Subject DESC

	IF(@SortBy = 1)
		INSERT INTO #PageIndex
		SELECT 
			PostID,
			PinnedDate = (SELECT Max(PinnedDate) FROM Posts WHERE ThreadID = P.ThreadID),
			TotalViews as Views,
			Replies = (SELECT COUNT(*) FROM Posts WHERE P.ThreadID = ThreadID AND  ParentID != 0),
			Auth = P.UserID
		FROM 
			Posts P 
		WHERE 
			ParentID = 0 AND 
			ForumID = @ForumID AND 
			
			ThreadDate >= @DateFilter
		ORDER BY 
			Auth DESC

IF(@SortBy = 2)
		INSERT INTO #PageIndex
		SELECT 
			PostID,
			PinnedDate = (SELECT Max(PinnedDate) FROM Posts WHERE ThreadID = P.ThreadID),
			TotalViews as Views,
			Replies = (SELECT COUNT(*) FROM Posts WHERE P.ThreadID = ThreadID AND  ParentID != 0),
			Auth = P.UserID
		FROM 
			Posts P 
		WHERE 
			ParentID = 0 AND 
			ForumID = @ForumID AND 
			
			ThreadDate >= @DateFilter
		ORDER BY 
			Replies DESC

IF(@SortBy = 3)
		INSERT INTO #PageIndex
		SELECT 
			PostID,
			PinnedDate = (SELECT Max(PinnedDate) FROM Posts WHERE ThreadID = P.ThreadID),
			TotalViews as Views,
			Replies = (SELECT COUNT(*) FROM Posts WHERE P.ThreadID = ThreadID AND  ParentID != 0),
			Auth = P.UserID
		FROM 
			Posts P 
		WHERE 
			ParentID = 0 AND 
			ForumID = @ForumID AND 
			
			ThreadDate >= @DateFilter
		ORDER BY 
			TotalViews DESC

IF(@SortBy = 4)
		INSERT INTO #PageIndex
		SELECT 
			PostID,
			PinnedDate = (SELECT Max(PinnedDate) FROM Posts WHERE ThreadID = P.ThreadID),
			TotalViews as Views,
			Replies = (SELECT COUNT(*) FROM Posts WHERE P.ThreadID = ThreadID AND  ParentID != 0),
			Auth = P.UserID
		FROM 
			Posts P 
		WHERE 
			ParentID = 0 AND 
			ForumID = @ForumID AND 
			
			ThreadDate >= @DateFilter
			ORDER BY 
			PinnedDate DESC

END


	BEGIN
  		SELECT 
			Subject,
			Body,
			P.PostID,
			ThreadID,
			ParentID,
			PostDate = (SELECT Max(PostDate) FROM Posts WHERE ThreadID = P.ThreadID),
			ThreadDate,
			PinnedDate,
			UserID,	
			Replies = (SELECT COUNT(*) FROM Posts WHERE P.ThreadID = ThreadID AND  ParentID != 0),
			Body,
			TotalViews,
			IsLocked,
			IsPinned,
			HasRead = dbo.HasReadPost(@UserID, P.PostID, P.ForumID),
			MostRecentPostAuthorID = (SELECT TOP 1 P2.UserID FROM Posts P2 WHERE P.ThreadID = ThreadID ORDER BY PostDate DESC),
			MostRecentPostID = (SELECT TOP 1 PostID FROM Posts WHERE P.ThreadID = ThreadID ORDER BY PostDate DESC)
		FROM 
			Posts P (nolock),
			#PageIndex PageIndex
		WHERE 
			P.PostID = PageIndex.PostID AND
			PageIndex.IndexID > @PageLowerBound AND
			PageIndex.IndexID < @PageUpperBound 
		ORDER BY 
			PageIndex.IndexID
	END

	-- Update Forum View date
	IF EXISTS (SELECT ForumID FROM ForumsRead WHERE ForumID = @ForumID AND UserID=@UserID)
		-- Row exists, update
		UPDATE 
			ForumsRead
		SET
			LastActivity = GetDate()
		WHERE
			ForumID = @ForumID AND
			UserID = @UserID
	ELSE
		-- Row does not exist, insert
		INSERT INTO
			ForumsRead
			(ForumID, UserID, MarkReadAfter, LastActivity)
		VALUES
			(@ForumID, @UserID, 0, GetDate())

END









GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.forums_GetPostRead    Script Date: 20.02.2003 20:33:52 ******/



CREATE    PROCEDURE forums_GetPostRead
(
	@PostID int,
	@UserID int
)
 AS
BEGIN
	DECLARE @HasRead bit
	SET @HasRead = 0

	IF EXISTS 
	(
		SELECT
			HasRead
		FROM
			PostsRead
		WHERE
			PostID = @PostID AND
			UserID=@UserID
	)
		SET @HasRead = 1

	SELECT HasRead = @HasRead
END




GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO





/****** Object:  Stored Procedure dbo.forums_GetTotalNumberOfForums    Script Date: 20.02.2003 20:33:52 ******/



Create   PROCEDURE forums_GetTotalNumberOfForums
AS

	SELECT
		COUNT (*)
	FROM
		Forums




GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO



/****** Object:  Stored Procedure dbo.forums_MarkAllThreadsRead    Script Date: 20.02.2003 20:33:53 ******/



CREATE       PROCEDURE forums_MarkAllThreadsRead
(
	@ForumID int,
	@UserID int
)
 AS
BEGIN
	DECLARE @PostID int

	-- first find the max post id for the given forum
	SELECT @PostID = MAX(PostID) FROM Posts WHERE ForumID = @ForumID

	-- Do we need to performa an INSERT or an UPDATE?
	IF EXISTS (SELECT ForumID FROM ForumsRead WHERE ForumID = @ForumID AND UserID=@UserID)
		UPDATE 
			ForumsRead
		SET
			MarkReadAfter = @PostID
		WHERE
			ForumID = @ForumID AND
			UserID = @UserID
	ELSE
		INSERT INTO
			ForumsRead
			(ForumId, UserID, MarkReadAfter)
		VALUES
			(@ForumID, @UserID, @PostID)

	-- Do some clean up
	DELETE PostsRead WHERE PostID < @PostID AND UserID = @UserID

END
		





GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.forums_MarkPostAsRead    Script Date: 20.02.2003 20:33:53 ******/



Create       PROCEDURE forums_MarkPostAsRead
(
	@PostID	int,
	@UserID int
)
 AS
BEGIN

	-- If @UserName is null it is an anonymous user
	IF @UserID IS NOT NULL
	BEGIN
		DECLARE @ForumID int
		DECLARE @PostDate datetime
		-- Mark the post as read
		-- *********************

		-- Only for ParentID = 0
		IF EXISTS (SELECT PostID FROM Posts WHERE PostID = @PostID AND ParentID = 0)
			IF NOT EXISTS (SELECT HasRead FROM PostsRead WHERE UserID = @UserID and PostID = @PostID)
				INSERT INTO PostsRead (UserID, PostID) VALUES (@UserID, @PostID)

	END

END





GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO



/****** Object:  Stored Procedure dbo.forums_TopicCountForForum    Script Date: 20.02.2003 20:33:53 ******/



CREATE      PROCEDURE forums_TopicCountForForum
(
	@ForumID int,
	@MaxDate datetime,
	@MinDate datetime
)
 AS
	--IF @UserName IS NULL OR @UnReadTopicsOnly = 0
		SELECT 
			TotalTopics = COUNT(*) 
		FROM 
			Posts 
		WHERE 
			ParentID = 0 AND 
			forumid = @ForumID AND 
			
			ThreadDate >= @MinDate AND 
			ThreadDate <= @MaxDate





GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO



/****** Object:  Stored Procedure dbo.forums_UpdatePost    Script Date: 20.02.2003 20:33:53 ******/



CREATE    PROCEDURE forums_UpdatePost
(
	@PostID	int,
	@Subject	nvarchar(256),
	@Body		ntext,
	@IsLocked	bit,
	@IsPinnedPost bit,
	@Pinned datetime,
	@ChangePin bit
)
AS
DECLARE @IsPinned bit
-- Is the post pinned?

If(@ChangePin='True')
BEGIN
	IF @Pinned IS NULL
	BEGIN
		SET @IsPinned = 0
		SET @Pinned = GetDate()
	END
	ELSE
		SET @IsPinned = 1

		UPDATE 
			Posts 
		SET
			PinnedDate = @Pinned,
			IsPinned = @IsPinned
		WHERE 
			PostID = @PostID
END

	-- this sproc updates a post (called from the moderate/admin page)
	UPDATE 
		Posts 
	SET
		Subject = @Subject,
		Body = @Body,
		IsLocked = @IsLocked,
		IspostPinned = @IsPinnedPost
	WHERE 
		PostID = @PostID




GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored Procedure dbo.forums_AddPost    Script Date: 20.02.2003 20:33:53 ******/




CREATE              PROCEDURE forums_AddPost 
(
	@ForumID int,
	@ReplyToPostID int, 
	@Subject nvarchar(256),
	@UserID int,
	@Body ntext,
	@IsLocked bit,
	@IsPinnedPost bit,
	@Pinned datetime
) AS
DECLARE @ParentLevel int
DECLARE @ThreadID int
DECLARE @NewPostID int
DECLARE @ModeratedForum bit
DECLARE @IsPinned bit


-- Is the post pinned?
IF @Pinned IS NULL
BEGIN
	SET @IsPinned = 0
	SET @Pinned = GetDate()
END
ELSE
	SET @IsPinned = 1

-- Is this forum moderated?
IF @ForumID = 0 AND @ReplyToPostID <> 0
	-- we need to get the forum ID
	SELECT @ForumID = ForumID FROM Posts (nolock) WHERE PostID = @ReplyToPostID
--SELECT @ModeratedForum = Moderated FROM Forums (nolock) WHERE ForumID = @ForumID


-- Determine if this post will be approved
-- if the forum is NOT moderated, then the post will be approved
SET NOCOUNT ON
BEGIN TRAN


IF @ReplyToPostID = 0 -- New Post
  BEGIN
	
    -- Do INSERT into Posts table
    INSERT 
	Posts ( ForumID, ThreadID, ParentID, Subject, PinnedDate, IsPinned, UserID, Body, IsLocked, IsPostPinned )
    VALUES 
	(@ForumID, 0, 0, @Subject, @Pinned, @IsPinned, @UserID, @Body, @IsLocked, @IsPinnedPost)

    -- Get the new post id
    SELECT 
	@NewPostID = @@IDENTITY

    -- update posts with the new post id
    UPDATE 
	Posts
    SET 
	ThreadID = @NewPostID
    WHERE 
	PostID = @NewPostID

   -- do we need to track the threads for this user?
   SELECT @ThreadID = @NewPostID

  END
ELSE -- @ReplyToID <> 0 means reply to an existing post
  BEGIN
    -- Get Post Information for what we are replying to
    SELECT 
           @ThreadID = ThreadID,
           @ForumID = ForumID
    FROM 
	   Posts
    WHERE 
       PostID = @ReplyToPostID

     BEGIN

	-- Insert the new post
		INSERT 
		Posts (ForumID, ThreadID, ParentID, Subject, PinnedDate, IsPinned, UserID, Body, IsLocked, IsPostPinned  )
    	VALUES 
		(@ForumID, @ThreadID, @ReplyToPostID, @Subject, @Pinned, @IsPinned, @UserID, @Body, @IsLocked, @IsPinnedPost )
	-- Clean up PostsRead
	DELETE PostsRead WHERE PostID = @ThreadID AND UserID != @UserID

     END 

     SELECT 
	@NewPostID = @@IDENTITY FROM Posts

     -- if this message is approved, update the thread date

	UPDATE 
		Posts 
	SET 
		ThreadDate = getdate()
	WHERE 
		ThreadID = @ThreadID
  END

COMMIT TRAN

 -- Update the forum statitics

   exec Statistics_UpdateForumStatistics @ForumID, @ThreadID, @NewPostID

 -- Clean up unnecessary columns in forumsread
 exec Maintenance_CleanForumsRead @ForumID

	-- Row does not exist, insert
	INSERT INTO
		ForumsRead
		(ForumID, UserID, MarkReadAfter, LastActivity)
	VALUES
		(@ForumID, @UserID, 0, GetDate())

SET NOCOUNT OFF
SELECT PostID = @NewPostID



GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


/****** Object:  Stored Procedure dbo.forums_DeleteForum    Script Date: 20.02.2003 20:33:53 ******/



create procedure forums_DeleteForum
(
	@ForumID	int
)
 AS
	-- delete the specified forum and all of its posts
	-- first we must remove all the thread tracking rows
	DELETE FROM ThreadTrackings
	WHERE ThreadID IN (SELECT DISTINCT ThreadID FROM Posts WHERE ForumID = @ForumID)
	-- now we must remove all of the posts
	DELETE FROM Posts
	WHERE ForumID = @ForumID
	-- finally we can delete the actual forum
	DELETE FROM Forums
	WHERE ForumID = @ForumID




GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored Procedure dbo.forums_DeleteModeratedPost    Script Date: 20.02.2003 20:33:53 ******/



CREATE     PROCEDURE forums_DeleteModeratedPost
(
	@PostID	int
) AS
	-- we must delete all of the posts and replies
	-- first things first, determine if this is the parent of the thread
	DECLARE @ThreadID int
	DECLARE @ForumID int
	DECLARE @UserID int

	SELECT @ThreadID = ThreadID, @ForumID = ForumID, @UserID = UserID FROM Posts (nolock) WHERE PostID = @PostID
	IF @ThreadID = @PostID
	  BEGIN
		-- we are dealing with the parent fo the thread
		-- delete all of the thread tracking
		DELETE 
			ThreadTrackings
		WHERE 
			ThreadID = @ThreadID

		-- Delete the entire thread
		DELETE 
			Posts
		WHERE 
			ThreadID = @ThreadID

		-- Clean up the forum statistics
		exec Statistics_ResetForumStatistics @ForumID

	  END
	ELSE
		-- we must recursively delete this post and all of its children
		exec dbo.forums_DeletePostAndChildren @PostID















GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.forums_DeletePost    Script Date: 20.02.2003 20:33:53 ******/



CREATE   PROCEDURE forums_DeletePost
(
	@PostID	int
) AS
	-- we must delete all of the posts and replies
	-- first things first, determine if this is the parent of the thread
	DECLARE @ThreadID int
	DECLARE @ForumID int
	DECLARE @UserID int

	SELECT @ThreadID = ThreadID, @ForumID = ForumID, @UserID = UserID FROM Posts (nolock) WHERE PostID = @PostID
	IF @ThreadID = @PostID
	  BEGIN
		-- we are dealing with the parent fo the thread
		-- delete all of the thread tracking
		DELETE FROM 
			ThreadTrackings
		WHERE 
			ThreadID = @ThreadID

		-- delete the entire thread
		DELETE FROM 
			Posts
		WHERE 
			ThreadID = @ThreadID

		-- Update the forum statistics
		exec Statistics_ResetForumStatistics @ForumID
	  END
	ELSE
		-- we must recursively delete this post and all of its children
		exec dbo.forums_DeletePostAndChildren @PostID




GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO



/****** Object:  Stored Procedure dbo.forums_GetAllForums    Script Date: 20.02.2003 20:33:53 ******/


CREATE                  PROCEDURE forums_GetAllForums
(
	@UserID int

)
AS
	-- return all of the columns in all of the forums

                -- Is a User Specified?
    IF @UserID IS NOT NULL

		-- get all of the forums
		SELECT
			ForumID,
			Name,
			Description,
			DateCreated,

			TotalPosts,
			TotalTopics = TotalThreads,
			MostRecentPostID,
			MostRecentThreadID,
			MostRecentPostDate,
			MostRecentPostAuthorID,

			LastUserActivity = (SELECT LastActivity FROM ForumsRead WHERE ForumID = F.ForumID AND UserID=@UserID)
		FROM 
			Forums F (nolock)
		
		ORDER BY
			Name

    ELSE
		-- get JUST the active forums
		SELECT
			ForumID,
			Name,
			Description,
			DateCreated,

			TotalPosts,
			TotalTopics = TotalThreads,
			MostRecentPostID,
			MostRecentThreadID,
			MostRecentPostDate,
			MostRecentPostAuthorID,
			LastUserActivity = NULL
		FROM 
			Forums F (nolock)

		ORDER BY
			Name




GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.forums_GetForumByPostID    Script Date: 20.02.2003 20:33:53 ******/




CREATE     PROCEDURE forums_GetForumByPostID
(
	@PostID int
)
 AS
	-- get the Forum ID for a particular post
	SELECT
		ForumID,
		Name,
		Description,
		DateCreated
	FROM Forums F (nolock)
	WHERE ForumID = (SELECT ForumID FROM Posts (nolock) WHERE PostID = @PostID)






GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.forums_GetForumByThreadID    Script Date: 20.02.2003 20:33:53 ******/



CREATE      PROCEDURE forums_GetForumByThreadID
(
	@ThreadID int
)
 AS
	-- get the Forum ID for a particular post
	SELECT
		ForumID,
		Name,
		Description,
		DateCreated,
		IsPrivate = 0
	FROM Forums F (nolock)
	WHERE ForumID = (SELECT DISTINCT ForumID FROM Posts (nolock) WHERE ThreadID = @ThreadID)







GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.forums_GetForumInfo    Script Date: 20.02.2003 20:33:54 ******/



CREATE           PROCEDURE forums_GetForumInfo
(
	@ForumID int,
	@UserID int
)
AS

	SELECT
		ForumID = @ForumID,
		Name,
		Description,
		DateCreated,
		TotalTopics = TotalThreads
	FROM 
		Forums F (nolock)
	WHERE 
		ForumID = @ForumID




GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


/****** Object:  Stored Procedure dbo.forums_GetPostInfo    Script Date: 20.02.2003 20:33:54 ******/

CREATE        PROCEDURE forums_GetPostInfo
(
	@PostID	int,
	@TrackViews bit,
	@UserID int
)
 AS
BEGIN

	IF @TrackViews = 1
	BEGIN
		DECLARE @views int
	
		-- Update the counter for the number of times this post is viewed
		SELECT @views = TotalViews FROM Posts WHERE PostID = @PostID
		UPDATE Posts SET TotalViews = (@views + 1) WHERE PostID = @PostID
	END

	-- If @UserName is null it is an anonymous user
	IF @UserID IS NOT NULL
	BEGIN
		DECLARE @ForumID int
		DECLARE @PostDate datetime

		-- Mark the post as read
		-- *********************

		-- Only for ParentID = 0
		IF EXISTS (SELECT PostID FROM Posts WHERE PostID = @PostID AND ParentID = 0)
			IF NOT EXISTS (SELECT HasRead FROM PostsRead WHERE  PostID = @PostID AND UserID=@UserID)
				INSERT INTO PostsRead (UserID, PostID) VALUES (@UserID, @PostID)

	END

	IF @UserID IS NOT NULL
		SELECT
			Subject,
			PostID,
			UserID,
			P.ForumID,
			ForumName = (SELECT Name FROM Forums F (nolock) WHERE F.ForumID = P.ForumID),
			ParentID,
			ThreadID,
			PostDate,
			ThreadDate,
			Replies = (SELECT COUNT(*) FROM Posts P2 (nolock) WHERE P2.ParentID = P.PostID AND P2.ParentID != 0),
			Body,
			TotalMessagesInThread = 0, -- not used
			TotalViews,
			IsLocked,
			IsPostPinned,
			HasRead = 1
		FROM 
			Posts P (nolock)
		WHERE 
			P.PostID = @PostID
	ELSE
		SELECT
			Subject,
			PostID,
			UserID,
			P.ForumID,
			ForumName = (SELECT Name FROM Forums F (nolock) WHERE F.ForumID = P.ForumID),
			ParentID,
			ThreadID,
			PostDate,
			ThreadDate,
			Replies = (SELECT COUNT(*) FROM Posts P2 (nolock) WHERE P2.ParentID = P.PostID AND P2.ParentID != 0),
			Body,
			TotalMessagesInThread = 0, -- not used
			TotalViews,
			IsLocked,
			IsPostPinned,
			HasRead = 1
		FROM 
			Posts P (nolock)
		WHERE 
			P.PostID = @PostID

END




GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored Procedure dbo.forums_GetThreadByPostIDPaged    Script Date: 20.02.2003 20:33:54 ******/


CREATE              PROCEDURE forums_GetThreadByPostIDPaged
(
	@PostID	int,
	@PageIndex int,
	@PageSize int,
	@UserID int
)
 AS
BEGIN

DECLARE @PageLowerBound int
DECLARE @PageUpperBound int
DECLARE @ThreadID int
DECLARE @ForumID int
DECLARE @PrivateForumID int
DECLARE @PinnedCount int

-- Get the ForumID, the PrivateForumID, and the ThreadID
SELECT @ForumID = ForumID, @ThreadID = ThreadID FROM Posts WHERE PostID = @PostID

SELECT @PinnedCount = COUNT(*) FROM Posts WHERE IsPostPinned = 'True' AND ThreadID = @ThreadID
SET @PageSize = @PageSize - @PinnedCount;

-- Set the page bounds
SET @PageLowerBound = @PageSize * @PageIndex
SET @PageUpperBound = @PageLowerBound + @PageSize + 1

-- Create a temp table to store the select results
CREATE TABLE #PageIndex 
(
	IndexId int IDENTITY (1, 1) NOT NULL,
	PostID int
)


    INSERT INTO #PageIndex (PostID)
    SELECT PostID FROM Posts P (nolock) WHERE IsPostPinned = 'False' AND ThreadID = @ThreadID ORDER BY PostDate

SELECT
	P.PostID,
	ThreadID,
	ForumID,
	ForumName = (SELECT Name FROM Forums F (nolock) WHERE F.ForumID = P.ForumID),
	Subject,
	ParentID,
	PostDate,
	ThreadDate,
	UserID,
	Replies = (SELECT COUNT(*) FROM Posts P2 (nolock) WHERE P2.ParentID = P.PostID AND P2.ParentID != 0),
	Body,
	TotalViews,
	IsLocked,
	IsPostPinned,
	TotalMessagesInThread = 0, -- not used
	HasRead = 0 -- not used
FROM 
	Posts P (nolock)
WHERE
	P.ThreadID = @ThreadID AND
	P.IsPostPinned='True'

UNION ALL

SELECT
	P.PostID,
	ThreadID,
	ForumID,
	ForumName = (SELECT Name FROM Forums F (nolock) WHERE F.ForumID = P.ForumID),
	Subject,
	ParentID,
	PostDate,
	ThreadDate,
	UserID,
	Replies = (SELECT COUNT(*) FROM Posts P2 (nolock) WHERE P2.ParentID = P.PostID AND P2.ParentID != 0),
	Body,
	TotalViews,
	IsLocked,
	IsPostPinned,
	TotalMessagesInThread = 0, -- not used
	HasRead = 0 -- not used
FROM 
	Posts P (nolock),
	#PageIndex
WHERE
	P.PostID = #PageIndex.PostID AND
	P.IsPostPinned='False' AND	#PageIndex.IndexID > @PageLowerBound AND
	#PageIndex.IndexID < @PageUpperBound
	
END




GO



SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[forums_GetTotalPinnedPostsForThread]
(
	@PostID	int
)
 AS

	BEGIN

	DECLARE @T int
	SELECT @T = (SELECT ThreadID FROM Posts WHERE PostID = @PostID)

	-- Get the count of posts for a given thread
	SELECT
		TotalPinnedPostsForThread = COUNT(PostID)
	FROM
		Posts (nolock)
	WHERE
		ThreadID = @T AND
		IsPostPinned = 'True'
		
	END

GO



SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


/****** Object:  Stored Procedure dbo.forums_GetTopicsUserIsTracking    Script Date: 20.02.2003 20:33:54 ******/


create procedure forums_GetTopicsUserIsTracking
(
@UserID int
)
AS
BEGIN

SELECT 
	Subject,
	Body,
	P.PostID,
	P.ThreadID,
	ParentID,
	PostDate = (SELECT Max(PostDate) FROM Posts WHERE ThreadID = P.ThreadID),
	ThreadDate,
	PinnedDate,
	P.UserID,
	Replies = (SELECT COUNT(*) FROM Posts WHERE P.ThreadID = ThreadID AND  ParentID != 0),
	Body,
	TotalViews,
	IsLocked,
	IsPinned,
	HasRead = dbo.HasReadPost(@UserID, P.PostID, P.ForumID),
	MostRecentPostAuthorID = (SELECT TOP 1 UserID FROM Posts P2 WHERE P.ThreadID = ThreadID ORDER BY PostDate DESC),
	MostRecentPostID = (SELECT TOP 1 PostID FROM Posts WHERE P.ThreadID = ThreadID ORDER BY PostDate DESC)
FROM
	Posts P,
	ThreadTrackings T
WHERE
	ParentID = 0 AND
	
	P.ThreadID = T.ThreadID AND
    T.UserID = @UserID
	
END




GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO




/****** Object:  Stored Procedure dbo.forums_IsUserTrackingPost    Script Date: 20.02.2003 20:33:54 ******/

CREATE procedure forums_IsUserTrackingPost
(
	@ThreadID int,
	@UserID int
)
AS
DECLARE @TrackingThread bit

IF EXISTS(SELECT ThreadID FROM ThreadTrackings (nolock) WHERE ThreadID = @ThreadID AND UserID=@UserID)
	SELECT IsUserTrackingPost = 1
ELSE
	SELECT IsUserTrackingPost = 0


GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored Procedure dbo.forums_MovePost    Script Date: 20.02.2003 20:33:54 ******/



CREATE     PROCEDURE forums_MovePost
(
	@PostID		int,
	@MoveToForumID	int,
	@UserID	int
)
 AS
DECLARE @CurrentForum int
DECLARE @ApproveSetting bit
DECLARE @ForumName nvarchar(100)
		
	-- only allow top-level messages to be moved
	IF (SELECT ParentID FROM Posts (nolock) WHERE PostID = @PostID) <> 0
		SELECT 0
	ELSE
	  BEGIN

		-- Get the forum we are moving from
		SELECT
			@CurrentForum = ForumID
		FROM
			Posts
		WHERE
			PostID = @PostID	

		-- Update the post with a new forum id
		UPDATE 
			Posts
		SET 
			ForumID = @MoveToForumID
		WHERE 
			ThreadID = @PostID

		-- Update the forum statistics for the from forum
		exec Statistics_ResetForumStatistics @CurrentForum

		-- Update the forum statistics for the to forum
		exec Statistics_ResetForumStatistics @MoveToForumID

		IF @ApproveSetting = 0
			-- the post was moved but not approved
			SELECT 1
		ELSE
			-- the post was moved AND approved
			SELECT 2
	  END





GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


/****** Object:  Stored Procedure dbo.forums_ReverseTrackingOption    Script Date: 20.02.2003 20:33:54 ******/



create procedure forums_ReverseTrackingOption 
(
	@UserID int,
	@PostID	int
)
AS

	-- reverse the user's tracking options for a particular thread
	-- first get the threadID of the Post
	DECLARE @ThreadID int
	SELECT @ThreadID = ThreadID FROM Posts (nolock) WHERE PostID = @PostID
	IF EXISTS(SELECT ThreadID FROM ThreadTrackings WHERE ThreadID = @ThreadID AND UserID=@UserID)
		-- the user is tracking this thread, delete this row
		DELETE FROM ThreadTrackings
		WHERE ThreadID = @ThreadID AND UserID=@UserID
	ELSE
		-- this user isn't tracking the thread, so add her
		INSERT INTO ThreadTrackings (ThreadID, UserID)
		VALUES(@ThreadID, @UserID)







GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.forums_UpdateForum    Script Date: 20.02.2003 20:33:54 ******/



CREATE  PROCEDURE forums_UpdateForum
(
	@ForumID	int,
	@Name		nvarchar(100),
	@Description	nvarchar(3000)
)
 AS
	-- update the forum information
	UPDATE Forums SET
		Name = @Name,
		Description = @Description
	WHERE ForumID = @ForumID




GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  User Defined Function dbo.HasReadPost    Script Date: 20.02.2003 20:33:54 ******/

CREATE  function HasReadPost(@UserID int, @PostID int, @ForumID int)
RETURNS bit
AS
BEGIN
DECLARE @HasRead bit
DECLARE @ReadAfter int
SET @HasRead = 0


	-- Do we have topics marked as read?
	SELECT @ReadAfter = MarkReadAfter FROM ForumsRead WHERE ForumID = @ForumID AND UserID=@UserID

	IF @ReadAfter IS NOT NULL
	BEGIN
		IF @ReadAfter >= @PostID
			RETURN 1
	END
	
	
	IF EXISTS (SELECT HasRead FROM PostsRead  WHERE PostID = @PostID AND UserID=@UserID)
	  SET @HasRead = 1

RETURN @HasRead
END

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

-- Create a new forum
SET IDENTITY_INSERT Forums ON; 
INSERT INTO Forums (ForumID, Name, Description, 
					DateCreated, TotalPosts, TotalThreads,
					MostRecentPostID, MostRecentThreadID, 
					MostRecentPostDate, MostRecentPostAuthorID) 
		VALUES (1 , 'Обсуждение новостей', 'Обсуждение новостей портала', 
				GetDate(), 0, 0, 
				0, 0, null, 0);

SET IDENTITY_INSERT Forums OFF;