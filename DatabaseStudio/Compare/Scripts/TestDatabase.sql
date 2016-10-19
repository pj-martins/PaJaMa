use master

if not exists(select 1 from sys.server_principals
where name = 'testlogin')
begin
CREATE LOGIN [testlogin] WITH PASSWORD=N'P@ssw0rd'
, DEFAULT_DATABASE=[master], DEFAULT_LANGUAGE=[us_english], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
end
go

if not exists(select 1 from sys.server_principals
where name = 'testlogin2')
begin
CREATE LOGIN [testlogin2] WITH PASSWORD=N'P@ssw0rd'
, DEFAULT_DATABASE=[master], DEFAULT_LANGUAGE=[us_english], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
end
go

--if exists (select 1 from sys.databases where name = 'FromDatabase')
--alter database FromDatabase set offline with rollback immediate
--go

if exists (select 1 from sys.databases where name = 'FromDatabase')
drop database FromDatabase
go

--if exists (select 1 from sys.databases where name = 'ToDatabase')
--alter database ToDatabase set offline with rollback immediate
--go

if exists (select 1 from sys.databases where name = 'ToDatabase')
drop database ToDatabase
go

create database FromDatabase
go

create database ToDatabase
go

use FromDatabase
go

create schema [TestSchema]
go

EXEC sp_addextendedproperty N'SchProp1', N'SchValue0', 'SCHEMA', N'TestSchema', NULL, NULL, NULL, NULL
GO

EXEC sp_addextendedproperty N'SchProp2', N'SchValue2', 'SCHEMA', N'TestSchema', NULL, NULL, NULL, NULL
GO


EXEC sp_addextendedproperty N'SchProp4', N'SchValue2', 'SCHEMA', N'TestSchema', NULL, NULL, NULL, NULL
GO


CREATE TABLE [dbo].[FromOnly](
	[FromOnlyID] [int] IDENTITY(1,1) NOT NULL,
	[ColNumeric] [numeric](18, 2) NOT NULL,
	[ColBigInt] [bigint] NOT NULL CONSTRAINT [DF_FromOnly_BigInt]  DEFAULT ((0)),
	[ColBinary] [binary](50) NOT NULL,
	[ColBit] [bit] NOT NULL,
	[ColChar] [char](10) NOT NULL,
	[ColDateTime] [datetime] NOT NULL CONSTRAINT [DF_FromOnly_DateTime]  DEFAULT (getdate()),
	[ColDecimal] [decimal](18, 2) NOT NULL,
	[ColFloat] [float] NOT NULL,
	[ColImage] [image] NOT NULL,
	[ColInt] [int] NOT NULL,
	[ColMoney] [money] NOT NULL,
	[ColNChar] [nchar](10) NOT NULL,
	[ColNText] [ntext] NOT NULL,
	[ColNVarChar] [nvarchar](50) NOT NULL,
	[ColReal] [real] NOT NULL,
	[ColUniqueIdentifier] [uniqueidentifier] NOT NULL,
	[ColSmallDateTime] [smalldatetime] NOT NULL CONSTRAINT [DF_FromOnly_SmallDateTime]  DEFAULT (getdate()),
	[ColSmallInt] [smallint] NOT NULL,
	[ColSmallMoney] [smallmoney] NOT NULL,
	[ColText] [text] NOT NULL,
	[ColTimestamp] [timestamp] NOT NULL,
	[ColTinyInt] [tinyint] NOT NULL,
	[ColVarBinary] [varbinary](50) NOT NULL,
	[ColVarChar] [varchar](50) NOT NULL,
	[ColXml] [xml] NOT NULL,
	[ColDate] [date] NOT NULL,
	[ColTime] [time](7) NOT NULL,
	[ColDateTime2] [datetime2](7) NOT NULL,
	[ColDateTimeOffset] [datetimeoffset](7) NOT NULL,
 CONSTRAINT [PK_FromOnly] PRIMARY KEY CLUSTERED 
(
	[FromOnlyID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
go


CREATE TABLE [TestSchema].[NullToNotNull](
	[NullToNotNullID] [int] IDENTITY(1,1) NOT NULL,
	[ColNumeric] [numeric](18, 2) NOT NULL,
	[ColBigInt] [bigint] NOT NULL,
	[ColBinary] [binary](50) NOT NULL,
	[ColBit] [bit] NOT NULL,
	[ColChar] [char](10) NOT NULL,
	[ColDateTime] [datetime] NOT NULL,
	[ColDecimal] [decimal](18, 2) NOT NULL,
	[ColFloat] [float] NOT NULL,
	[ColImage] [image] NOT NULL,
	[ColInt] [int] NOT NULL,
	[ColMoney] [money] NOT NULL,
	[ColNChar] [nchar](10) NOT NULL,
	[ColNText] [ntext] NOT NULL,
	[ColNVarChar] [nvarchar](50) NOT NULL,
	[ColReal] [real] NOT NULL,
	[ColUniqueIdentifier] [uniqueidentifier] NOT NULL,
	[ColSmallDateTime] [smalldatetime] NOT NULL,
	[ColSmallInt] [smallint] NOT NULL,
	[ColSmallMoney] [smallmoney] NOT NULL,
	[ColText] [text] NOT NULL,
	[ColTimestamp] [timestamp] NOT NULL,
	[ColTinyInt] [tinyint] NOT NULL,
	[ColVarBinary] [varbinary](50) NOT NULL,
	[ColVarChar] [varchar](50) NOT NULL,
	[ColXml] [xml] NOT NULL,
	[ColDate] [date] NOT NULL,
	[ColTime] [time](7) NOT NULL,
	[ColDateTime2] [datetime2](7) NOT NULL,
	[ColDateTimeOffset] [datetimeoffset](7) NOT NULL,
 CONSTRAINT [PK_NullToNotNull] PRIMARY KEY CLUSTERED 
(
	[NullToNotNullID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
go

CREATE TABLE [dbo].[ColMismatch](
	[ColMismatchID] [int] IDENTITY(1,1) NOT NULL,
	[ColNumeric] [numeric](10, 0) NOT NULL,
	[ColBit] [bit] NOT NULL CONSTRAINT [DF_ColMismatch_ColBit]  default(0),
	[ColChar] [char](10) NOT NULL,
	[ColDateTime] [datetime] NOT NULL,
	[ColDecimal] [decimal](18, 2) NOT NULL,
	[ColFloat] [float] NOT NULL,
	[ColImage] [image] NOT NULL,
	[ColInt] [int] NOT NULL,
	[ColFormula] AS [ColInt] * 2,
	[ColNVarChar] [nvarchar](50) NULL,
	[ColReal] [real] NOT NULL CONSTRAINT [DF_ColMismatch_ColReal]  default(0),
	[ColUniqueIdentifier] [uniqueidentifier] NOT NULL,
	[ColSmallDateTime] [smalldatetime] NOT NULL,
	[ColSmallInt] [smallint] NOT NULL,
	[ColSmallMoney] [smallmoney] NOT NULL,
	[ColText] [text] NOT NULL,
	[ColTimestamp] [timestamp] NOT NULL,
	[ColTinyInt] [tinyint] NOT NULL,
	[ColVarBinary] [varbinary](20) NOT NULL,
	[ColVarChar] [varchar](20) NOT NULL,
	[ColXml] [xml] NOT NULL,
	[ColDate] [date] NOT NULL,
	[ColTime] [time](7) NOT NULL,
	[ColDateTime2] [datetime2](7) NOT NULL,
	[ColDateTimeOffset] [datetimeoffset](7) NOT NULL,
 CONSTRAINT [PK_ColMismatch] PRIMARY KEY CLUSTERED 
(
	[ColMismatchID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
go



CREATE TABLE [dbo].[PropsMismatch](
	[PropsMismatchID] [int] IDENTITY(1,1) NOT NULL,
	[ColBit] [int] NULL default(0),
	[ColDateTime] [datetime] NULL,
	[ColInt] [int] NULL default(1),
	[ColMoney] [money] NULL,
	[ColReal] [real] NULL CONSTRAINT [DF_PropsMismatch_ColReal]  default(1),
	[ColSmallDateTime] [smalldatetime] NULL,
	[ColSmallInt] [smallint] NULL,
	[ColDate] [date] NULL,
	[ColTime] [time](7) NULL,
 CONSTRAINT [PK_PropsMismatch] PRIMARY KEY CLUSTERED 
(
	[PropsMismatchID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
go


CREATE TABLE [dbo].[KeyMismatch](
	[Key1] [int] NOT NULL,
	[Key2] [int] NOT NULL,
 CONSTRAINT [PK_KeyMismatch] PRIMARY KEY CLUSTERED 
(
	[Key1] ASC,
	[Key2] DESC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
go


CREATE TABLE [dbo].[Exact](
	[ExactID] [int] IDENTITY(1,1) NOT NULL,
	[ColNumeric] [numeric](18, 2) NOT NULL,
	[ColBigInt] [bigint] NOT NULL,
	[ColBinary] [binary](50) NOT NULL,
	[ColBit] [bit] NOT NULL,
	[ColChar] [char](10) NOT NULL,
	[ColDateTime] [datetime] NOT NULL,
	[ColDecimal] [decimal](18, 2) NOT NULL,
	[ColFloat] [float] NOT NULL,
	[ColImage] [image] NOT NULL,
	[ColInt] [int] NOT NULL,
	[ColMoney] [money] NOT NULL,
	[ColNChar] [nchar](10) NOT NULL,
	[ColNText] [ntext] NOT NULL,
	[ColNVarChar] [nvarchar](50) NOT NULL,
	[ColReal] [real] NOT NULL,
	[ColUniqueIdentifier] [uniqueidentifier] NOT NULL,
	[ColSmallDateTime] [smalldatetime] NOT NULL,
	[ColSmallInt] [smallint] NOT NULL,
	[ColSmallMoney] [smallmoney] NOT NULL,
	[ColText] [text] NOT NULL,
	[ColTimestamp] [timestamp] NOT NULL,
	[ColTinyInt] [tinyint] NOT NULL,
	[ColVarBinary] [varbinary](50) NOT NULL,
	[ColVarChar] [varchar](50) NOT NULL,
	[ColXml] [xml] NOT NULL,
	[ColDate] [date] NOT NULL,
	[ColTime] [time](7) NOT NULL,
	[ColDateTime2] [datetime2](7) NOT NULL,
	[ColDateTimeOffset] [datetimeoffset](7) NOT NULL,
 CONSTRAINT [PK_Exact] PRIMARY KEY CLUSTERED 
(
	[ExactID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
go


CREATE TABLE [dbo].[Parent](
	[ParentID] [int] IDENTITY(1,1) NOT NULL,
	[ColNVarChar] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Parent] PRIMARY KEY CLUSTERED 
(
	[ParentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
go

CREATE TABLE [dbo].[Child](
	[ChildID] [int] IDENTITY(1,1) NOT NULL,
	[ParentID] [int] NOT NULL,
	[ColNVarChar] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Child] PRIMARY KEY CLUSTERED 
(
	[ChildID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
go

CREATE TABLE [dbo].[GrandChild](
	[GrandChildID] [int] IDENTITY(1,1) NOT NULL,
	[ChildID] [int] NOT NULL,
	[ColNVarChar] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_GrandChild] PRIMARY KEY CLUSTERED 
(
	[GrandChildID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
go

ALTER TABLE [dbo].[Child]  WITH CHECK ADD  CONSTRAINT [FK_Child_Parent] FOREIGN KEY([ParentID])
REFERENCES [dbo].[Parent] ([ParentID])
GO

ALTER TABLE [dbo].[Child] CHECK CONSTRAINT [FK_Child_Parent]
GO

ALTER TABLE [dbo].[GrandChild]  WITH CHECK ADD  CONSTRAINT [FK_GrandChild_Child] FOREIGN KEY([ChildID])
REFERENCES [dbo].[Child] ([ChildID])
GO

ALTER TABLE [dbo].[GrandChild] CHECK CONSTRAINT [FK_GrandChild_Child]
GO

CREATE TABLE [dbo].[DropChild](
	[DropChildID] [int] IDENTITY(1,1) NOT NULL,
	[DropParentID] [int] NOT NULL,
	[ColNVarChar] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_DropChild] PRIMARY KEY CLUSTERED 
(
	[DropChildID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
go


CREATE TABLE [dbo].[ParentAction](
	[ParentActionID] [int] IDENTITY(1,1) NOT NULL,
	[ColNVarChar] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_ParentAction] PRIMARY KEY CLUSTERED 
(
	[ParentActionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
go

CREATE TABLE [dbo].[ChildAction](
	[ChildActionID] [int] IDENTITY(1,1) NOT NULL,
	[ParentActionID] [int] NULL,
	[ColNVarChar] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_ChildAction] PRIMARY KEY CLUSTERED 
(
	[ChildActionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
go

ALTER TABLE [dbo].[ChildAction]  WITH NOCHECK ADD  CONSTRAINT [FK_ChildAction_ParentAction] FOREIGN KEY([ParentActionID])
REFERENCES [dbo].[ParentAction] ([ParentActionID])
ON DELETE NO ACTION
ON UPDATE SET NULL
GO



CREATE TABLE [dbo].[ParentMulti1](
	[Parent1ID] [int] NOT NULL,
	[Parent2ID] [int] NOT NULL,
	[ColNVarChar] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_ParentMulti1] PRIMARY KEY CLUSTERED 
(
	[Parent1ID] ASC,
	[Parent2ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
go

CREATE TABLE [dbo].[ChildMulti1](
	[ChildMultiID] [int] IDENTITY(1,1) NOT NULL,
	[Parent1ID] [int] NOT NULL,
	[Parent2ID] [int] NOT NULL,
	[ColNVarChar] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_ChildMulti1] PRIMARY KEY CLUSTERED 
(
	[ChildMultiID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
go

ALTER TABLE [dbo].[ChildMulti1]  WITH NOCHECK ADD  CONSTRAINT [FK_ChildMulti_ParentMulti1] FOREIGN KEY([Parent1ID],[Parent2ID])
REFERENCES [dbo].[ParentMulti1] ([Parent1ID],[Parent2ID])
GO


CREATE TABLE [dbo].[ParentMulti2](
	[Parent1ID] [int] NOT NULL,
	[Parent2ID] [int] NOT NULL,
	[ColNVarChar] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_ParentMulti2] PRIMARY KEY CLUSTERED 
(
	[Parent1ID] ASC,
	[Parent2ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
go

CREATE TABLE [dbo].[ChildMulti2](
	[ChildMultiID] [int] IDENTITY(1,1) NOT NULL,
	[Parent1ID] [int] NOT NULL,
	[Parent2ID] [int] NOT NULL,
	[ColNVarChar] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_ChildMulti2] PRIMARY KEY CLUSTERED 
(
	[ChildMultiID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
go

ALTER TABLE [dbo].[ChildMulti2]  WITH NOCHECK ADD  CONSTRAINT [FK_ChildMulti_ParentMulti2] FOREIGN KEY([Parent1ID],[Parent2ID])
REFERENCES [dbo].[ParentMulti2] ([Parent1ID],[Parent2ID])
GO


CREATE TABLE [dbo].[ParentMulti3](
	[Parent1ID] [int] NOT NULL,
	[Parent2ID] [int] NOT NULL,
	[ColNVarChar] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_ParentMulti3] PRIMARY KEY CLUSTERED 
(
	[Parent1ID] ASC,
	[Parent2ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
go

CREATE TABLE [dbo].[ChildMulti3](
	[ChildMultiID] [int] IDENTITY(1,1) NOT NULL,
	[Parent1ID] [int] NOT NULL,
	[Parent2ID] [int] NOT NULL,
	[ColNVarChar] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_ChildMulti3] PRIMARY KEY CLUSTERED 
(
	[ChildMultiID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
go

ALTER TABLE [dbo].[ChildMulti3]  WITH NOCHECK ADD  CONSTRAINT [FK_ChildMulti_ParentMulti3] FOREIGN KEY([Parent1ID],[Parent2ID])
REFERENCES [dbo].[ParentMulti3] ([Parent1ID],[Parent2ID])
GO



CREATE TABLE [dbo].[FromIdentity](
	[FromIdentityID] [int] IDENTITY(1,1) NOT NULL,
	[ColNVarChar] [nvarchar](20) NOT NULL,
 CONSTRAINT [PK_FromIdentity] PRIMARY KEY CLUSTERED 
(
	[FromIdentityID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
go

CREATE TABLE [dbo].[ToIdentity](
	[ToIdentityID] [int] NOT NULL,
	[ColNVarChar] [nvarchar](20) NOT NULL,
 CONSTRAINT [PK_ToIdentity] PRIMARY KEY CLUSTERED 
(
	[ToIdentityID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
go

CREATE TABLE [TestSchema].[DiffSchema](
	[DiffSchemaID] [int] IDENTITY(1,1) NOT NULL,
	[ColNVarChar] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_DiffSchema] PRIMARY KEY CLUSTERED 
(
	[DiffSchemaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
go


CREATE TABLE [dbo].[ParentSchema](
	[ParentSchemaID] [int] IDENTITY(1,1) NOT NULL,
	[ColNVarChar] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_ParentSchema] PRIMARY KEY CLUSTERED 
(
	[ParentSchemaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
go

CREATE TABLE [TestSchema].[ChildSchema](
	[ChildSchemaID] [int] IDENTITY(1,1) NOT NULL,
	[ParentSchemaID] [int] NULL,
	[ColNVarChar] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_ChildSchema] PRIMARY KEY CLUSTERED 
(
	[ChildSchemaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
go

ALTER TABLE [TestSchema].[ChildSchema]  WITH NOCHECK ADD  CONSTRAINT [FK_ChildSchema_ParentSchema] FOREIGN KEY([ParentSchemaID])
REFERENCES [dbo].[ParentSchema] ([ParentSchemaID])
ON DELETE CASCADE
GO

CREATE TABLE [dbo].[NonClusteredPK] (
		[Column1]       [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
		[Column2]      [varchar](30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
		[Column3]     [varchar](1000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)
GO

ALTER TABLE [dbo].[NonClusteredPK]
	ADD
	CONSTRAINT [pk_NonClusteredPK]
	PRIMARY KEY
	NONCLUSTERED
	([Column1], [Column2])

GO

CREATE CLUSTERED INDEX [IX_Clustered]

	ON [dbo].[NonClusteredPK] ([Column1], [Column2])

GO

CREATE NONCLUSTERED INDEX [IX_NonClustered]

	ON [dbo].[NonClusteredPK] ([Column1])

GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_UniqueValues]

	ON [dbo].[NonClusteredPK] ([Column1], [Column2])

GO




CREATE TABLE [dbo].[DiffNonClusteredPK] (
		[Column1]       [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
		[Column2]      [varchar](30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
		[Column3]     [varchar](1000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)
GO

ALTER TABLE [dbo].[DiffNonClusteredPK]
	ADD
	CONSTRAINT [pk_DiffNonClusteredPK]
	PRIMARY KEY
	NONCLUSTERED
	([Column1], [Column2])

GO

CREATE CLUSTERED INDEX [IX_DiffClustered]

	ON [dbo].[DiffNonClusteredPK] ([Column1], [Column2])

GO

CREATE NONCLUSTERED INDEX [IX_DiffNonClustered]

	ON [dbo].[DiffNonClusteredPK] ([Column1])

GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_DiffUniqueValues]

	ON [dbo].[DiffNonClusteredPK] ([Column1], [Column2])

GO





CREATE TABLE [dbo].[DiffNonClusteredPK2] (
		[Column1]       [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
		[Column2]      [varchar](30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
		[Column3]     [varchar](1000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)
GO

ALTER TABLE [dbo].[DiffNonClusteredPK2]
	ADD
	CONSTRAINT [pk_DiffNonClusteredPK2]
	PRIMARY KEY
	NONCLUSTERED
	([Column1], [Column2])

GO

CREATE CLUSTERED INDEX [IX_DiffClustered2]

	ON [dbo].[DiffNonClusteredPK2] ([Column1], [Column2])

GO

CREATE NONCLUSTERED INDEX [IX_DiffNonClustered2]

	ON [dbo].[DiffNonClusteredPK2] ([Column1])

GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_DiffUniqueValues2]

	ON [dbo].[DiffNonClusteredPK2] ([Column1], [Column2])

GO





CREATE TABLE [dbo].[DiffNonClusteredPK3] (
		[Column1]       [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
		[Column2]      [varchar](30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
		[Column3]     [varchar](1000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)
GO

ALTER TABLE [dbo].[DiffNonClusteredPK3]
	ADD
	CONSTRAINT [pk_DiffNonClusteredPK3]
	PRIMARY KEY
	NONCLUSTERED
	([Column1], [Column2])

GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_DiffUniqueValues3]

	ON [dbo].[DiffNonClusteredPK3] ([Column1], [Column2])

GO


create schema [TestSchemaFromOnly]
go

CREATE TABLE [TestSchemaFromOnly].[FromParent](
	[FromParentID] [int] IDENTITY(1,1) NOT NULL,
	[ColNVarChar] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_FromParent] PRIMARY KEY CLUSTERED 
(
	[FromParentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
go

CREATE TABLE [dbo].[FromChild](
	[FromChildID] [int] IDENTITY(1,1) NOT NULL,
	[FromParentID] [int] NOT NULL,
	[ColNVarChar] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_FromChild] PRIMARY KEY CLUSTERED 
(
	[FromChildID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
go

CREATE TABLE [TestSchemaFromOnly].[FromGrandChild](
	[FromGrandChildID] [int] IDENTITY(1,1) NOT NULL,
	[FromChildID] [int] NOT NULL,
	[ColNVarChar] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_FromGrandChild] PRIMARY KEY CLUSTERED 
(
	[FromGrandChildID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
go

ALTER TABLE [dbo].[FromChild]  WITH CHECK ADD  CONSTRAINT [FK_FromChild_FromParent] FOREIGN KEY([FromParentID])
REFERENCES [TestSchemaFromOnly].[FromParent] ([FromParentID])
GO

ALTER TABLE [dbo].[FromChild] CHECK CONSTRAINT [FK_FromChild_FromParent]
GO

ALTER TABLE [TestSchemaFromOnly].[FromGrandChild]  WITH CHECK ADD  CONSTRAINT [FK_GrandChild_Child] FOREIGN KEY([FromChildID])
REFERENCES [dbo].[FromChild] ([FromChildID])
GO

ALTER TABLE [TestSchemaFromOnly].[FromGrandChild] CHECK CONSTRAINT [FK_GrandChild_Child]
GO


CREATE TABLE [TestSchemaFromOnly].[FromParent2](
	[FromParent2ID] [int] IDENTITY(1,1) NOT NULL,
	[ColNVarChar] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_FromParent2] PRIMARY KEY CLUSTERED 
(
	[FromParent2ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
go

CREATE TABLE [dbo].[FromChild2](
	[FromChild2ID] [int] IDENTITY(1,1) NOT NULL,
	[MyFromParentID] [int] NOT NULL,
	[ColNVarChar] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_FromChild2] PRIMARY KEY CLUSTERED 
(
	[FromChild2ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
go

ALTER TABLE [dbo].[FromChild2]  WITH CHECK ADD  CONSTRAINT [FK_FromChild2_FromParent2] FOREIGN KEY([MyFromParentID])
REFERENCES [TestSchemaFromOnly].[FromParent2] ([FromParent2ID])
GO

ALTER TABLE [dbo].[FromChild2] CHECK CONSTRAINT [FK_FromChild2_FromParent2]
GO

CREATE TABLE [TestSchemaFromOnly].[SchemaTable](
	[SchemaTableID] [int] IDENTITY(1,1) NOT NULL,
	[ColNVarChar] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_SchemaTable] PRIMARY KEY CLUSTERED 
(
	[SchemaTableID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
go

CREATE TABLE [dbo].[SchemaTable](
	[SchemaTableID] [int] IDENTITY(1,1) NOT NULL,
	[ColNVarChar] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_SchemaTable] PRIMARY KEY CLUSTERED 
(
	[SchemaTableID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
go

EXEC sp_addextendedproperty N'TableProp', N'TestValue2', 'SCHEMA', N'dbo', 'TABLE', N'SchemaTable', NULL, NULL

GO


EXEC sp_addextendedproperty N'TableProp1', N'TestValue0', 'SCHEMA', N'dbo', 'TABLE', N'SchemaTable', NULL, NULL

GO

EXEC sp_addextendedproperty N'ColumnProp', N'TestValue1', 'SCHEMA', N'dbo', 'TABLE', N'ColMismatch', 'COLUMN', N'ColBit'

GO

EXEC sp_addextendedproperty N'TableProp', N'TestValue2', 'SCHEMA', N'dbo', 'TABLE', N'ColMismatch', NULL, NULL

GO


EXEC sp_addextendedproperty N'TableProp1', N'TestValue0', 'SCHEMA', N'dbo', 'TABLE', N'ColMismatch', NULL, NULL

GO


EXEC sp_addextendedproperty N'ColumnProp', N'TestValue1', 'SCHEMA', N'dbo', 'TABLE', N'FromOnly', 'COLUMN', N'ColBit'

GO

EXEC sp_addextendedproperty N'TableProp', N'TestValue2', 'SCHEMA', N'dbo', 'TABLE', N'FromOnly', NULL, NULL

GO



CREATE NONCLUSTERED INDEX [IX_PropsMismatch] ON [dbo].[PropsMismatch]
(
	[ColSmallInt] ASC,
	[ColSmallDateTime] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO


CREATE NONCLUSTERED INDEX [IX_PropsMismatch1] ON [dbo].[PropsMismatch]
(
	[ColSmallDateTime] DESC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO


CREATE NONCLUSTERED INDEX [IX_PropsMismatch2] ON [dbo].[PropsMismatch]
(
	[ColSmallInt] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO


CREATE NONCLUSTERED INDEX [IX_PropsMismatch4] ON [dbo].[PropsMismatch]
(
	[ColBit] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO



CREATE NONCLUSTERED INDEX [IX_PropsMismatch6] ON [dbo].[PropsMismatch]
(
	[ColSmallDateTime] DESC,
	[ColSmallInt] DESC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO


CREATE SYNONYM [dbo].[Exact_Syn] FOR [ChildSchema].[Exact]
GO


EXEC sp_addextendedproperty N'SynProp', N'SynValue', 'SCHEMA', N'dbo', 'SYNONYM', N'Exact_Syn', NULL, NULL
GO


CREATE SYNONYM [TestSchema].[FromOnly_Syn] FOR [dbo].[FromOnly]
GO


CREATE SYNONYM [TestSchema].[Diff_Syn] FOR [dbo].[PropsMismatch]
GO

EXEC sp_addextendedproperty N'SynProp1', N'SynValue0', 'SCHEMA', N'TestSchema', 'SYNONYM', N'Diff_Syn', NULL, NULL
GO

EXEC sp_addextendedproperty N'SynProp3', N'SynValue', 'SCHEMA', N'TestSchema', 'SYNONYM', N'Diff_Syn', NULL, NULL
GO



CREATE SYNONYM [TestSchema].[Schem_Syn] FOR [dbo].[ColMisMatch]
GO


CREATE VIEW [TestSchema].[OnlyView]
AS
SELECT        *
FROM            [dbo].[FromOnly]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'TestSchema', @level1type=N'VIEW',@level1name=N'OnlyView'
GO




CREATE VIEW [TestSchema].[ExactView]
AS
SELECT        *
FROM           [dbo].[Exact]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'TestSchema', @level1type=N'VIEW',@level1name=N'ExactView'
GO



create PROCEDURE [dbo].[ExactProcedure]
	@input nvarchar(50)
AS
BEGIN
	print @input
END
GO

EXEC sp_addextendedproperty N'ProcProp', N'ProcValue', 'SCHEMA', N'dbo', 'PROCEDURE', N'ExactProcedure', NULL, NULL
GO



create PROCEDURE [dbo].[FromOnlyProcedure]
	@input nvarchar(50)
AS
BEGIN
	print @input
END
GO



create PROCEDURE [TestSchema].[DiffProcedure]
	@input nvarchar(50)
AS
BEGIN
	print @input
END
GO


EXEC sp_addextendedproperty N'ProcProp', N'ProcValue', 'SCHEMA', N'TestSchema', 'PROCEDURE', N'DiffProcedure', NULL, NULL
GO

EXEC sp_addextendedproperty N'ProcProp3', N'ProcValue1', 'SCHEMA', N'TestSchema', 'PROCEDURE', N'DiffProcedure', NULL, NULL
GO


create PROCEDURE [dbo].[DiffProcedure]
	@input nvarchar(50)
AS
BEGIN
	print @input
END
GO


create PROCEDURE [TestSchema].[SchemaDiffProcedure]
	@input nvarchar(50)
AS
BEGIN
	print 'testing ' + @input
END
GO



create Function [dbo].[ExactFunction] ()
returns nvarchar(50)
AS
BEGIN
	return 'test'
END
GO

EXEC sp_addextendedproperty N'FuncProp', N'FuncValue', 'SCHEMA', N'dbo', 'FUNCTION', N'ExactFunction', NULL, NULL
GO



create Function [dbo].[FromOnlyFunction] ()
returns nvarchar(50)
AS
BEGIN
	return 'test'
END
GO


create Function [TestSchema].[DiffFunction] ()
returns nvarchar(50)
AS
BEGIN
	return 'test'
END
GO

EXEC sp_addextendedproperty N'FuncProp1', N'FuncValue', 'SCHEMA', N'TestSchema', 'FUNCTION', N'DiffFunction', NULL, NULL
GO

EXEC sp_addextendedproperty N'FuncProp2', N'FuncValue', 'SCHEMA', N'TestSchema', 'FUNCTION', N'DiffFunction', NULL, NULL
GO


create Function [dbo].[DiffFunction] ()
returns nvarchar(50)
AS
BEGIN
	return 'test'
END
GO

create Function [dbo].[DiffFunction2] (@test varchar(100))
returns nvarchar(50)
AS
BEGIN
	return 'test'
END
GO

create Function [dbo].[DiffFunction3] (@test varchar(100))
returns nvarchar(50)
AS
BEGIN
	return 'test'
END
GO

create Function [dbo].[DiffFunction4] (@test varchar(100))
returns nvarchar(50)
AS
BEGIN
	return 'test'
END
GO

create Function [TestSchema].[SchemaDiffFunction] ()
returns nvarchar(50)
AS
BEGIN
	return 'test'
END
GO



CREATE VIEW [TestSchema].[DiffView]
AS
select c.ChildID, p.ColNVarChar as ParentNVarChar, c.ColNVarChar as ChildNVarChar
from Child c
join Parent p on p.ParentID = c.ParentID
GO


EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'TestSchema', @level1type=N'VIEW',@level1name=N'DiffView'
GO



CREATE TRIGGER dbo.FromOnlyTrigger
   ON  dbo.FromOnly
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
	select * from FromOnly where 2 = 5
END
GO




CREATE TRIGGER dbo.ExactTrigger
   ON  dbo.Exact
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
	select * from dbo.Exact where 2 = 5
END
GO


CREATE TRIGGER TestSchema.DiffTrigger
   ON  TestSchema.ChildSchema
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
	select * from  TestSchema.ChildSchema where 2 = 5
END
GO




CREATE TRIGGER dbo.DiffTrigger
   ON  dbo.PropsMismatch
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
	select * from dbo.Exact where 2 = 5
END
GO



CREATE TRIGGER dbo.DiffTrigger2
   ON  dbo.PropsMismatch
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
	select * from dbo.Exact where 2 = 5
END
GO

CREATE TRIGGER dbo.DiffTrigger3
   ON  dbo.PropsMisMatch
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
	select * from dbo.Exact where 2 = 5
END
GO

CREATE TRIGGER dbo.DiffTrigger4
   ON  dbo.PropsMisMatch
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
	select * from dbo.Exact where 2 = 5
END
GO

DISABLE TRIGGER dbo.DiffTrigger4 on dbo.PropsMisMatch
go

CREATE TRIGGER dbo.DiffTrigger5
   ON  dbo.PropsMisMatch
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
	select * from dbo.Exact where 2 = 5
END
GO


CREATE TRIGGER dbo.DiffSchemaTrigger
   ON  dbo.PropsMisMatch
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
	select * from dbo.Exact where 2 = 5
END
GO

CREATE USER [fromuser] WITHOUT LOGIN
GO

EXEC sp_addextendedproperty N'fromuserprop', N'fromuservalue', 'USER', N'fromuser', NULL, NULL, NULL, NULL

GO

CREATE USER [exactuser] WITHOUT LOGIN
GO

CREATE USER [zzzTestSchema2Owner] WITHOUT LOGIN
GO

create schema [aaaTestSchema2] authorization [zzzTestSchema2Owner]
go

create schema [TestSchemaOwner] authorization [exactuser]
go

CREATE TABLE [TestSchemaOwner].[Exact](
	[ExactID] [int] IDENTITY(1,1) NOT NULL,
	[ColNumeric] [numeric](18, 2) NOT NULL,
	[ColBigInt] [bigint] NOT NULL,
	[ColBinary] [binary](50) NOT NULL,
 CONSTRAINT [PK_Exact] PRIMARY KEY CLUSTERED 
(
	[ExactID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
go




DENY CONTROL ON [dbo].[Parent] TO [exactuser] 
GRANT DELETE ON [dbo].[Parent] TO [exactuser] 
GO

GRANT SELECT ON [TestSchema].[ChildSchema] TO [exactuser] 
GO

CREATE USER [testuserprincipal] FOR LOGIN [testlogin2] WITH DEFAULT_SCHEMA=[dbo]
GO

EXEC sp_addextendedproperty N'testuserprincipalprop1', N'testuserprincipalvalue', 'USER', N'testuserprincipal', NULL, NULL, NULL, NULL

GO

EXEC sp_addextendedproperty N'testuserprincipalprop2', N'testuserprincipalvalue', 'USER', N'testuserprincipal', NULL, NULL, NULL, NULL

GO

EXEC sp_addextendedproperty N'testuserprincipalprop3', N'testuserprincipalvalue', 'USER', N'testuserprincipal', NULL, NULL, NULL, NULL

GO


create role [fromrole]
go

alter role [fromrole] add member [fromuser]
go

alter role [fromrole] add member [testuserprincipal]
go

EXEC sp_addextendedproperty N'fromroleprop', N'fromroleval', 'USER', N'fromrole', NULL, NULL, NULL, NULL

GO


create role [exact] authorization [testuserprincipal]

alter role [exact] add member [exactuser]
go

alter role [exact] add member [testuserprincipal]
go

create role [diff1] authorization [testuserprincipal]

alter role [diff1] add member [testuserprincipal]
go

create role [diff2] authorization [testuserprincipal]

alter role [diff2] add member [testuserprincipal]
go

create role [diff3] authorization [testuserprincipal]

alter role [diff3] add member [testuserprincipal]
go

create role [diff4] authorization [testuserprincipal]

alter role [diff4] add member [testuserprincipal]
go

alter role [diff4] add member [exact]
go


create role [diff5]

alter role [diff5] add member [testuserprincipal]
go


create role [diff6] authorization [testuserprincipal]

alter role [diff6] add member [testuserprincipal]
go

alter role [diff6] add member [exact]
go


EXEC sp_addextendedproperty N'diff6prop1', N'diff6val', 'USER', N'diff6', NULL, NULL, NULL, NULL

GO

EXEC sp_addextendedproperty N'diff6prop2', N'diff6val', 'USER', N'diff6', NULL, NULL, NULL, NULL

GO


EXEC sp_addextendedproperty N'diff6prop3', N'diff6val', 'USER', N'diff6', NULL, NULL, NULL, NULL

GO

use ToDatabase
go

create schema [TestSchema]
go

EXEC sp_addextendedproperty N'SchProp1', N'SchValue0', 'SCHEMA', N'TestSchema', NULL, NULL, NULL, NULL
GO

EXEC sp_addextendedproperty N'SchProp2', N'SchValue1', 'SCHEMA', N'TestSchema', NULL, NULL, NULL, NULL
GO


EXEC sp_addextendedproperty N'SchProp3', N'SchValue2', 'SCHEMA', N'TestSchema', NULL, NULL, NULL, NULL
GO




CREATE TABLE [dbo].[ToOnly](
	[ToOnlyID] [int] IDENTITY(1,1) NOT NULL,
	[ColNumeric] [numeric](18, 2) NOT NULL,
	[ColBigInt] [bigint] NOT NULL,
	[ColBinary] [binary](50) NOT NULL,
	[ColBit] [bit] NOT NULL,
	[ColChar] [char](10) NOT NULL,
	[ColDateTime] [datetime] NOT NULL,
	[ColDecimal] [decimal](18, 2) NOT NULL,
	[ColFloat] [float] NOT NULL,
	[ColImage] [image] NOT NULL,
	[ColInt] [int] NOT NULL,
	[ColMoney] [money] NOT NULL,
	[ColNChar] [nchar](10) NOT NULL,
	[ColNText] [ntext] NOT NULL,
	[ColNVarChar] [nvarchar](50) NOT NULL,
	[ColReal] [real] NOT NULL,
	[ColUniqueIdentifier] [uniqueidentifier] NOT NULL,
	[ColSmallDateTime] [smalldatetime] NOT NULL,
	[ColSmallInt] [smallint] NOT NULL,
	[ColSmallMoney] [smallmoney] NOT NULL,
	[ColText] [text] NOT NULL,
	[ColTimestamp] [timestamp] NOT NULL,
	[ColTinyInt] [tinyint] NOT NULL,
	[ColVarBinary] [varbinary](50) NOT NULL,
	[ColVarChar] [varchar](50) NOT NULL,
	[ColXml] [xml] NOT NULL,
	[ColDate] [date] NOT NULL,
	[ColTime] [time](7) NOT NULL,
	[ColDateTime2] [datetime2](7) NOT NULL,
	[ColDateTimeOffset] [datetimeoffset](7) NOT NULL,
 CONSTRAINT [PK_ToOnly] PRIMARY KEY CLUSTERED 
(
	[ToOnlyID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
go


CREATE TABLE [TestSchema].[NullToNotNull](
	[NullToNotNullID] [int] IDENTITY(1,1) NOT NULL,
	[ColNumeric] [numeric](18, 2) NULL,
	[ColBigInt] [bigint] NULL,
	[ColBinary] [binary](50) NULL,
	[ColBit] [bit] NULL,
	[ColChar] [char](10) NULL,
	[ColDateTime] [datetime] NULL,
	[ColDecimal] [decimal](18, 2) NULL,
	[ColFloat] [float] NULL,
	[ColImage] [image] NULL,
	[ColInt] [int] NULL,
	[ColMoney] [money] NULL,
	[ColNChar] [nchar](10) NULL,
	[ColNText] [ntext] NULL,
	[ColNVarChar] [nvarchar](50) NULL,
	[ColReal] [real] NULL,
	[ColUniqueIdentifier] [uniqueidentifier] NULL,
	[ColSmallDateTime] [smalldatetime] NULL,
	[ColSmallInt] [smallint] NULL,
	[ColSmallMoney] [smallmoney] NULL,
	[ColText] [text] NULL,
	[ColTimestamp] [timestamp] NULL,
	[ColTinyInt] [tinyint] NULL,
	[ColVarBinary] [varbinary](50) NULL,
	[ColVarChar] [varchar](50) NULL,
	[ColXml] [xml] NULL,
	[ColDate] [date] NULL,
	[ColTime] [time](7) NULL,
	[ColDateTime2] [datetime2](7) NULL,
	[ColDateTimeOffset] [datetimeoffset](7) NULL,
 CONSTRAINT [PK_NullToNotNull] PRIMARY KEY CLUSTERED 
(
	[NullToNotNullID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
go


CREATE TABLE [dbo].[ColMismatch](
	[ColMismatchID] [int] IDENTITY(1,1) NOT NULL,
	[ColBit] [int] NULL default(0),
	[ColChar] [char](10) NULL,
	[ColDateTime] [datetime] NULL,
	[ColDecimal] [decimal](18, 2) NULL,
	[ColFloat] [float] NULL,
	[ColImage] [image] NULL,
	[ColInt] [int] NULL default(1),
	[ColFormula] AS [ColInt] * 3,
	[ColMoney] [money] NULL,
	[ColNChar] [nchar](10) NULL,
	[ColNText] [ntext] NULL,
	[ColNVarChar] [ntext] NULL,
	[ColReal] [real] NULL CONSTRAINT [DF_ColMismatch_ColReal]  default(1),
	[ColUniqueIdentifier] [uniqueidentifier] NULL,
	[ColSmallDateTime] [smalldatetime] NULL,
	[ColSmallInt] [smallint] NULL,
	[ColVarBinary] [varbinary](50) NULL,
	[ColVarChar] [varchar](50) NULL,
	[ColXml] [xml] NULL,
	[coldate] [date] NULL,
	[ColTime] [time](7) NULL,
	[ColDateTime2] [datetime2](7) NULL,
	[ColDateTimeOffset] [datetimeoffset](7) NULL,
 CONSTRAINT [PK_ColMismatch] PRIMARY KEY CLUSTERED 
(
	[ColMismatchID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
go


CREATE TABLE [dbo].[PropsMismatch](
	[PropsMismatchID] [int] IDENTITY(1,1) NOT NULL,
	[ColBit] [int] NULL default(0),
	[ColDateTime] [datetime] NULL,
	[ColInt] [int] NULL default(1),
	[ColMoney] [money] NULL,
	[ColReal] [real] NULL CONSTRAINT [DF_PropsMismatch_ColReal]  default(0),
	[ColSmallDateTime] [smalldatetime] NULL,
	[ColSmallInt] [smallint] NULL,
	[ColDate] [date] NULL,
	[ColTime] [time](7) NULL,
 CONSTRAINT [PK_PropsMismatch] PRIMARY KEY CLUSTERED 
(
	[PropsMismatchID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
go



CREATE TABLE [dbo].[KeyMismatch](
	[Key1] [int] NOT NULL,
	[Key2] [int] NOT NULL,
 CONSTRAINT [PK_KeyMismatch] PRIMARY KEY CLUSTERED 
(
	[Key1] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
go



CREATE TABLE [dbo].[Exact](
	[ExactID] [int] IDENTITY(1,1) NOT NULL,
	[ColNumeric] [numeric](18, 2) NOT NULL,
	[ColBigInt] [bigint] NOT NULL,
	[ColBinary] [binary](50) NOT NULL,
	[ColBit] [bit] NOT NULL,
	[ColChar] [char](10) NOT NULL,
	[ColDateTime] [datetime] NOT NULL,
	[ColDecimal] [decimal](18, 2) NOT NULL,
	[ColFloat] [float] NOT NULL,
	[ColImage] [image] NOT NULL,
	[ColInt] [int] NOT NULL,
	[ColMoney] [money] NOT NULL,
	[ColNChar] [nchar](10) NOT NULL,
	[ColNText] [ntext] NOT NULL,
	[ColNVarChar] [nvarchar](50) NOT NULL,
	[ColReal] [real] NOT NULL,
	[ColUniqueIdentifier] [uniqueidentifier] NOT NULL,
	[ColSmallDateTime] [smalldatetime] NOT NULL,
	[ColSmallInt] [smallint] NOT NULL,
	[ColSmallMoney] [smallmoney] NOT NULL,
	[ColText] [text] NOT NULL,
	[ColTimestamp] [timestamp] NOT NULL,
	[ColTinyInt] [tinyint] NOT NULL,
	[ColVarBinary] [varbinary](50) NOT NULL,
	[ColVarChar] [varchar](50) NOT NULL,
	[ColXml] [xml] NOT NULL,
	[ColDate] [date] NOT NULL,
	[ColTime] [time](7) NOT NULL,
	[ColDateTime2] [datetime2](7) NOT NULL,
	[ColDateTimeOffset] [datetimeoffset](7) NOT NULL,
 CONSTRAINT [PK_Exact] PRIMARY KEY CLUSTERED 
(
	[ExactID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
go





CREATE TABLE [dbo].[Parent](
	[ParentID] [int] IDENTITY(1,1) NOT NULL,
	[ColNVarChar] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Parent] PRIMARY KEY CLUSTERED 
(
	[ParentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
go

CREATE TABLE [dbo].[Child](
	[ChildID] [int] IDENTITY(1,1) NOT NULL,
	[ParentID] [int] NOT NULL,
	[ColNVarChar] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Child] PRIMARY KEY CLUSTERED 
(
	[ChildID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
go

CREATE TABLE [dbo].[GrandChild](
	[GrandChildID] [int] IDENTITY(1,1) NOT NULL,
	[ChildID] [int] NOT NULL,
	[ColNVarChar] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_GrandChild] PRIMARY KEY CLUSTERED 
(
	[GrandChildID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
go

ALTER TABLE [dbo].[GrandChild]  WITH CHECK ADD  CONSTRAINT [FK_GrandChild_Parent] FOREIGN KEY([ChildID])
REFERENCES [dbo].[Parent] ([ParentID])
GO

ALTER TABLE [dbo].[GrandChild] CHECK CONSTRAINT [FK_GrandChild_Parent]
GO


ALTER TABLE [dbo].[GrandChild]  WITH CHECK ADD  CONSTRAINT [FK_GrandChild_Child] FOREIGN KEY([ChildID])
REFERENCES [dbo].[Parent] ([ParentID])
GO

ALTER TABLE [dbo].[GrandChild] CHECK CONSTRAINT [FK_GrandChild_Child]
GO

CREATE TABLE [dbo].[DropParent](
	[DropParentID] [int] IDENTITY(1,1) NOT NULL,
	[ColNVarChar] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_DropParent] PRIMARY KEY CLUSTERED 
(
	[DropParentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
go

CREATE TABLE [dbo].[DropChild](
	[DropChildID] [int] IDENTITY(1,1) NOT NULL,
	[DropParentID] [int] NOT NULL,
	[ColNVarChar] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_DropChild] PRIMARY KEY CLUSTERED 
(
	[DropChildID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
go

ALTER TABLE [dbo].[DropChild]  WITH CHECK ADD  CONSTRAINT [FK_DropChild_DropParent] FOREIGN KEY([DropParentID])
REFERENCES [dbo].[DropParent] ([DropParentID])
GO

ALTER TABLE [dbo].[DropChild] CHECK CONSTRAINT [FK_DropChild_DropParent]
GO


CREATE TABLE [dbo].[ParentAction](
	[ParentActionID] [int] IDENTITY(1,1) NOT NULL,
	[ColNVarChar] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_ParentAction] PRIMARY KEY CLUSTERED 
(
	[ParentActionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
go

CREATE TABLE [dbo].[ChildAction](
	[ChildActionID] [int] IDENTITY(1,1) NOT NULL,
	[ParentActionID] [int] NULL,
	[ColNVarChar] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_ChildAction] PRIMARY KEY CLUSTERED 
(
	[ChildActionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
go

ALTER TABLE [dbo].[ChildAction]  WITH CHECK ADD  CONSTRAINT [FK_ChildAction_ParentAction] FOREIGN KEY([ParentActionID])
REFERENCES [dbo].[ParentAction] ([ParentActionID])
ON DELETE CASCADE
ON UPDATE NO ACTION
GO





CREATE TABLE [dbo].[ParentMulti1](
	[Parent1ID] [int] NOT NULL,
	[Parent2ID] [int] NOT NULL,
	[ColNVarChar] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_ParentMulti1] PRIMARY KEY CLUSTERED 
(
	[Parent1ID] ASC,
	[Parent2ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
go

CREATE TABLE [dbo].[ChildMulti1](
	[ChildMultiID] [int] IDENTITY(1,1) NOT NULL,
	[Parent1ID] [int] NOT NULL,
	[Parent2ID] [int] NOT NULL,
	[ColNVarChar] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_ChildMulti1] PRIMARY KEY CLUSTERED 
(
	[ChildMultiID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
go

ALTER TABLE [dbo].[ChildMulti1]  WITH NOCHECK ADD  CONSTRAINT [FK_ChildMulti_ParentMulti1] FOREIGN KEY([Parent1ID],[Parent2ID])
REFERENCES [dbo].[ParentMulti1] ([Parent1ID],[Parent2ID])
GO


CREATE TABLE [dbo].[ParentMulti2](
	[Parent1ID] [int] NOT NULL,
	[Parent2ID] [int] NOT NULL,
	[ColNVarChar] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_ParentMulti2] PRIMARY KEY CLUSTERED 
(
	[Parent1ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
go

CREATE TABLE [dbo].[ChildMulti2](
	[ChildMultiID] [int] IDENTITY(1,1) NOT NULL,
	[Parent1ID] [int] NOT NULL,
	[Parent2ID] [int] NOT NULL,
	[ColNVarChar] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_ChildMulti2] PRIMARY KEY CLUSTERED 
(
	[ChildMultiID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
go

ALTER TABLE [dbo].[ChildMulti2]  WITH NOCHECK ADD  CONSTRAINT [FK_ChildMulti_ParentMulti2] FOREIGN KEY([Parent1ID])
REFERENCES [dbo].[ParentMulti2] ([Parent1ID])
GO


CREATE TABLE [dbo].[FromIdentity](
	[FromIdentityID] [int] NOT NULL,
	[ColNVarChar] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_FromIdentity] PRIMARY KEY CLUSTERED 
(
	[FromIdentityID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
go

CREATE TABLE [dbo].[ToIdentity](
	[ToIdentityID] [int] IDENTITY(1,1) NOT NULL,
	[ColNVarChar] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_ToIdentity] PRIMARY KEY CLUSTERED 
(
	[ToIdentityID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
go

CREATE TABLE [dbo].[DiffSchema](
	[DiffSchemaID] [int] IDENTITY(1,1) NOT NULL,
	[ColNVarChar] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_DiffSchema] PRIMARY KEY CLUSTERED 
(
	[DiffSchemaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
go


CREATE TABLE [dbo].[ParentSchema](
	[ParentSchemaID] [int] IDENTITY(1,1) NOT NULL,
	[ColNVarChar] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_ParentSchema] PRIMARY KEY CLUSTERED 
(
	[ParentSchemaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
go

CREATE TABLE [TestSchema].[ChildSchema](
	[ChildSchemaID] [int] IDENTITY(1,1) NOT NULL,
	[ParentSchemaID] [int] NULL,
	[ColNVarChar] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_ChildSchema] PRIMARY KEY CLUSTERED 
(
	[ChildSchemaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
go

CREATE TABLE [dbo].[SchemaTable](
	[SchemaTableID] [int] IDENTITY(1,1) NOT NULL,
	[ColNVarChar] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_SchemaTable] PRIMARY KEY CLUSTERED 
(
	[SchemaTableID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
go

create schema [TestSchemaToOnly]
go

CREATE TABLE [TestSchemaToOnly].[SchemaTable](
	[SchemaTableID] [int] IDENTITY(1,1) NOT NULL,
	[ColNVarChar] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_SchemaTable] PRIMARY KEY CLUSTERED 
(
	[SchemaTableID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
go

EXEC sp_addextendedproperty N'TableProp', N'TestValue2', 'SCHEMA', N'TestSchemaToOnly', 'TABLE', N'SchemaTable', NULL, NULL

GO


EXEC sp_addextendedproperty N'TableProp1', N'TestValue0', 'SCHEMA', N'TestSchemaToOnly', 'TABLE', N'SchemaTable', NULL, NULL

GO
EXEC sp_addextendedproperty N'ColumnProp', N'TestValue', 'SCHEMA', N'dbo', 'TABLE', N'ColMismatch', 'COLUMN', N'ColNVarChar'

GO

EXEC sp_addextendedproperty N'TableProp', N'TestValue', 'SCHEMA', N'dbo', 'TABLE', N'ColMismatch', NULL, NULL

GO

EXEC sp_addextendedproperty N'TableProp1', N'TestValue0', 'SCHEMA', N'dbo', 'TABLE', N'ColMismatch', NULL, NULL

GO

CREATE NONCLUSTERED INDEX [IX_PropsMismatch] ON [dbo].[PropsMismatch]
(
	[ColSmallDateTime] ASC,
	[ColSmallInt] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO


CREATE NONCLUSTERED INDEX [IX_PropsMismatch1] ON [dbo].[PropsMismatch]
(
	[ColSmallDateTime] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO


CREATE NONCLUSTERED INDEX [IX_PropsMismatch2] ON [dbo].[PropsMismatch]
(
	[ColSmallInt] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO


CREATE NONCLUSTERED INDEX [IX_PropsMismatch3] ON [dbo].[PropsMismatch]
(
	[ColBit] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO


CREATE NONCLUSTERED INDEX [IX_PropsMismatch6] ON [dbo].[PropsMismatch]
(
	[ColSmallDateTime] ASC,
	[ColSmallInt] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE SYNONYM [dbo].[Exact_Syn] FOR [ChildSchema].[Exact]
GO

EXEC sp_addextendedproperty N'SynProp', N'SynValue', 'SCHEMA', N'dbo', 'SYNONYM', N'Exact_Syn', NULL, NULL
GO

CREATE SYNONYM [dbo].[ToOnly_Syn] FOR [dbo].[ToOnly]
GO


CREATE SYNONYM [TestSchema].[Diff_Syn] FOR [dbo].[ColMisMatch]
GO

EXEC sp_addextendedproperty N'SynProp1', N'SynValue', 'SCHEMA', N'TestSchema', 'SYNONYM', N'Diff_Syn', NULL, NULL
GO

EXEC sp_addextendedproperty N'SynProp2', N'SynValue', 'SCHEMA', N'TestSchema', 'SYNONYM', N'Diff_Syn', NULL, NULL
GO



CREATE SYNONYM [dbo].[Schem_Syn] FOR [dbo].[ColMisMatch]
GO





CREATE VIEW [dbo].[OnlyView]
AS
SELECT        *
FROM            [dbo].[ToOnly]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'OnlyView'
GO




CREATE VIEW [TestSchema].[ExactView]
AS
SELECT        *
FROM           [dbo].[Exact]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'TestSchema', @level1type=N'VIEW',@level1name=N'ExactView'
GO




CREATE VIEW [TestSchema].[DiffView]
AS
select c.ChildID, c.ColNVarChar
from Child c
join Parent p on p.ParentID = c.ParentID
GO


EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount111', @value=1 , @level0type=N'SCHEMA',@level0name=N'TestSchema', @level1type=N'VIEW',@level1name=N'DiffView'
GO





create PROCEDURE [dbo].[ExactProcedure]
	@input nvarchar(50)
AS
BEGIN
	print @input
END
GO

EXEC sp_addextendedproperty N'ProcProp', N'ProcValue', 'SCHEMA', N'dbo', 'PROCEDURE', N'ExactProcedure', NULL, NULL
GO



create PROCEDURE [dbo].[ToOnlyProcedure]
	@input nvarchar(50)
AS
BEGIN
	print @input
END
GO



create PROCEDURE [TestSchema].[DiffProcedure]
	@input nvarchar(20)
AS
BEGIN
	print @input
END
GO

EXEC sp_addextendedproperty N'ProcProp', N'ProcValue1', 'SCHEMA', N'TestSchema', 'PROCEDURE', N'DiffProcedure', NULL, NULL
GO

EXEC sp_addextendedproperty N'ProcProp2', N'ProcValue1', 'SCHEMA', N'TestSchema', 'PROCEDURE', N'DiffProcedure', NULL, NULL
GO



create PROCEDURE [dbo].[DiffProcedure]
	@input nvarchar(50)
AS
BEGIN
	print 'test ' + @input
END
GO


create PROCEDURE [dbo].[SchemaDiffProcedure]
	@input nvarchar(50)
AS
BEGIN
	print @input
END
GO




create Function [dbo].[ExactFunction] ()
returns nvarchar(50)
AS
BEGIN
	return 'test'
END
GO


EXEC sp_addextendedproperty N'FuncProp', N'FuncValue', 'SCHEMA', N'dbo', 'FUNCTION', N'ExactFunction', NULL, NULL
GO



create Function [dbo].[ToOnlyFunction] ()
returns nvarchar(50)
AS
BEGIN
	return 'test'
END
GO


create Function [TestSchema].[DiffFunction] ()
returns nvarchar(50)
AS
BEGIN
	return 'test2'
END
GO

EXEC sp_addextendedproperty N'FuncProp1', N'FuncValue0', 'SCHEMA', N'TestSchema', 'FUNCTION', N'DiffFunction', NULL, NULL
GO

EXEC sp_addextendedproperty N'FuncProp3', N'FuncValue', 'SCHEMA', N'TestSchema', 'FUNCTION', N'DiffFunction', NULL, NULL
GO


create Function [dbo].[DiffFunction] ()
returns nvarchar(10)
AS
BEGIN
	return 'test'
END
GO


create Function [dbo].[SchemaDiffFunction] ()
returns nvarchar(50)
AS
BEGIN
	return 'test'
END
GO

create Function [dbo].[DiffFunction2] ()
returns nvarchar(50)
AS
BEGIN
	return 'test'
END
GO

create Function [dbo].[DiffFunction3] (@test2 varchar(100))
returns nvarchar(50)
AS
BEGIN
	return 'test'
END
GO

create Function [dbo].[DiffFunction4] (@test varchar(10))
returns nvarchar(50)
AS
BEGIN
	return 'test'
END
GO


CREATE TRIGGER dbo.ToOnlyTrigger
   ON  dbo.ToOnly
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
	select * from ToOnly where 2 = 5
END
GO




CREATE TRIGGER dbo.ExactTrigger
   ON  dbo.Exact
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
	select * from dbo.Exact where 2 = 5
END
GO


CREATE TRIGGER TestSchema.DiffTrigger
   ON  TestSchema.ChildSchema
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
	select * from  TestSchema.ChildSchema where 2 = 5
END
GO




CREATE TRIGGER dbo.DiffTrigger
   ON  dbo.PropsMisMatch
   AFTER INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
	select * from dbo.Exact where 2 = 5
END
GO



CREATE TRIGGER dbo.DiffTrigger2
   ON  dbo.Parent
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
	select * from dbo.Exact where 2 = 5
END
GO

CREATE TRIGGER dbo.DiffTrigger3
   ON  dbo.PropsMisMatch
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
	select * from dbo.Exact where 2 = 1
END
GO

CREATE TRIGGER dbo.DiffTrigger4
   ON  dbo.PropsMisMatch
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
	select * from dbo.Exact where 2 = 5
END
GO

CREATE TRIGGER dbo.DiffTrigger5
   ON  dbo.PropsMisMatch
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
	select * from dbo.Exact where 2 = 5
END
GO

DISABLE TRIGGER dbo.DiffTrigger5 on dbo.PropsMisMatch
go


CREATE TRIGGER TestSchema.DiffSchemaTrigger
   ON  TestSchema.NullToNotNull
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
	select * from TestSchema.NullToNotNull where 2 = 5
END
GO

CREATE USER [atouser] WITHOUT LOGIN
GO

CREATE USER [exactuser] WITHOUT LOGIN
GO


GRANT CONTROL ON [dbo].[Parent] TO [exactuser] 
DENY DELETE ON [dbo].[Parent] TO [exactuser] 
GO

DENY SELECT ON [TestSchema].[ChildSchema] TO [exactuser] 
GO

CREATE USER [testuserprincipal] FOR LOGIN [testlogin] WITH DEFAULT_SCHEMA=[TestSchema]
GO

EXEC sp_addextendedproperty N'testuserprincipalprop1', N'testuserprincipalvalue', 'USER', N'testuserprincipal', NULL, NULL, NULL, NULL

GO

EXEC sp_addextendedproperty N'testuserprincipalprop2', N'testuserprincipalvalue2', 'USER', N'testuserprincipal', NULL, NULL, NULL, NULL

GO

EXEC sp_addextendedproperty N'testuserprincipalprop4', N'testuserprincipalvalue', 'USER', N'testuserprincipal', NULL, NULL, NULL, NULL

GO
create role [torole]
go

alter role [torole] add member [atouser]
go

alter role [torole] add member [testuserprincipal]
go


create role [exact] authorization [testuserprincipal]

alter role [exact] add member [exactuser]
go

alter role [exact] add member [testuserprincipal]
go

create role [diff1] authorization [testuserprincipal]

alter role [diff1] add member [exact]
go

create role [diff2] authorization [exact]

alter role [diff2] add member [testuserprincipal]
go

create role [diff3] authorization [testuserprincipal]

alter role [diff3] add member [testuserprincipal]
go

alter role [diff3] add member [exact]
go

create role [diff4] authorization [testuserprincipal]

alter role [diff4] add member [testuserprincipal]
go


create role [diff5] authorization [testuserprincipal]

alter role [diff5] add member [testuserprincipal]
go

alter role [diff5] add member [exact]
go


create role [diff6]

alter role [diff6] add member [testuserprincipal]
go

EXEC sp_addextendedproperty N'diff6prop1', N'diff6val', 'USER', N'diff6', NULL, NULL, NULL, NULL

GO

EXEC sp_addextendedproperty N'diff6prop2', N'diff6val1', 'USER', N'diff6', NULL, NULL, NULL, NULL

GO


EXEC sp_addextendedproperty N'diff6prop4', N'diff6val', 'USER', N'diff6', NULL, NULL, NULL, NULL

GO

create schema [TestSchemaOwner] authorization [testuserprincipal]
go

CREATE TABLE [TestSchemaOwner].[Exact](
	[ExactID] [int] IDENTITY(1,1) NOT NULL,
	[ColNumeric] [numeric](18, 2) NOT NULL,
	[ColBigInt] [bigint] NOT NULL,
	[ColBinary] [binary](50) NOT NULL,
 CONSTRAINT [PK_Exact] PRIMARY KEY CLUSTERED 
(
	[ExactID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
go




CREATE TABLE [dbo].[DiffNonClusteredPK] (
		[Column1]       [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
		[Column2]      [varchar](30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
		[Column3]     [varchar](1000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)
GO

ALTER TABLE [dbo].[DiffNonClusteredPK]
	ADD
	CONSTRAINT [pk_DiffNonClusteredPK]
	PRIMARY KEY
	NONCLUSTERED
	([Column1])

GO

CREATE CLUSTERED INDEX [IX_DiffClustered]

	ON [dbo].[DiffNonClusteredPK] ([Column1])

GO

CREATE NONCLUSTERED INDEX [IX_DiffNonClustered]

	ON [dbo].[DiffNonClusteredPK] ([Column1], [Column2])

GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_DiffUniqueValues]

	ON [dbo].[DiffNonClusteredPK] ([Column1])

GO



CREATE TABLE [dbo].[DiffNonClusteredPK2] (
		[Column1]       [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
		[Column2]      [varchar](30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
		[Column3]     [varchar](1000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)
GO

ALTER TABLE [dbo].[DiffNonClusteredPK2]
	ADD
	CONSTRAINT [pk_DiffNonClusteredPK2]
	PRIMARY KEY
	NONCLUSTERED
	([Column2], [Column1])

GO

CREATE CLUSTERED INDEX [IX_DiffClustered2]

	ON [dbo].[DiffNonClusteredPK2] ([Column2], [Column1])

GO

CREATE NONCLUSTERED INDEX [IX_DiffNonClustered2]

	ON [dbo].[DiffNonClusteredPK2] ([Column2])

GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_DiffUniqueValues2]

	ON [dbo].[DiffNonClusteredPK2] ([Column2], [Column1])

GO

CREATE TABLE [dbo].[DiffNonClusteredPK3] (
		[Column1]       [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
		[Column2]      [varchar](30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
		[Column3]     [varchar](1000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)
GO

ALTER TABLE [dbo].[DiffNonClusteredPK3]
	ADD
	CONSTRAINT [pk_DiffNonClusteredPK3]
	PRIMARY KEY
	NONCLUSTERED
	([Column1] DESC, [Column2])

GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_DiffUniqueValues3]

	ON [dbo].[DiffNonClusteredPK3] ([Column1], [Column2] DESC)

GO
