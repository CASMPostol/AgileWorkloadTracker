if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'Partners')
  drop table  Partners;
CREATE TABLE [dbo].[Partners] (
    [Author]                 NVARCHAR(max)   NULL,
    [Body]                   NVARCHAR(max)   NULL,
    [CellPhone]              NVARCHAR(max)   NULL,
    [Created]                DATETIME        NULL,
    [Editor]                 NVARCHAR(max)   NULL,
    [EMail]                  NVARCHAR(max)   NULL,
    [ID]                     INT             NOT NULL,
    [Modified]               DATETIME        NULL,
    [owshiddenversion]       INT             NULL,
    [Title]                  NVARCHAR(max)   NOT NULL,
    [WorkAddress]            NVARCHAR(max)   NULL,
    [WorkCity]               NVARCHAR(max)   NULL,
    [WorkCountry]            NVARCHAR(max)   NULL,
    [WorkFax]                NVARCHAR(max)   NULL,
    [WorkPhone]              NVARCHAR(max)   NULL,
    [WorkZip]                NVARCHAR(max)   NULL,
    [OnlySQL]				 BIT			 NOT NULL,	
	CONSTRAINT [PK_Partners_ID] PRIMARY KEY CLUSTERED ([ID] ASC) 
);
