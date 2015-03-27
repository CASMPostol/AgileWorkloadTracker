if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'ArchivingOperationLogs')
  drop table  ArchivingOperationLogs;
CREATE TABLE [dbo].[ArchivingOperationLogs] (
    [ID]                     INT IDENTITY(1,1) NOT NULL,
    [Operation]              NVARCHAR(255)   NOT NULL,
    [Date]                   DATETIME        NOT NULL,
    [UserName]               NVARCHAR(255)   NOT NULL,
    [OnlySQL]				 BIT			 NOT NULL,	
	CONSTRAINT [PK_ArchivingOperationLogs_ID] PRIMARY KEY CLUSTERED ([ID] ASC) 
);
