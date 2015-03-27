if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'Type')
  drop table  Type;
CREATE TABLE [dbo].[Type] (
    [Author]                 NVARCHAR(max)   NULL,
    [Created]                DATETIME        NULL,
    [Editor]                 NVARCHAR(max)   NULL,
    [ID]                     INT             NOT NULL,
    [Modified]               DATETIME        NULL,
    [owshiddenversion]       INT             NULL,
    [Title]                  NVARCHAR(max)   NOT NULL,
    [OnlySQL]				 BIT			 NOT NULL,	
	CONSTRAINT [PK_Type_ID] PRIMARY KEY CLUSTERED ([ID] ASC) 
);
