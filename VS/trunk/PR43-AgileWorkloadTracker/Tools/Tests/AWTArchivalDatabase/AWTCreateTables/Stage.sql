if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'Stage')
  drop table  Stage;
CREATE TABLE [dbo].[Stage] (
    [Author]                 NVARCHAR(max)   NULL,
    [Created]                DATETIME        NULL,
    [Editor]                 NVARCHAR(max)   NULL,
    [ID]                     INT             NOT NULL,
    [Modified]               DATETIME        NULL,
    [owshiddenversion]       INT             NULL,
    [Title]                  NVARCHAR(max)   NOT NULL,
    [OnlySQL]				 BIT			 NOT NULL,	
	CONSTRAINT [PK_Stage_ID] PRIMARY KEY CLUSTERED ([ID] ASC) 
);
