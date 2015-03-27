if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'Category')
  drop table  Category;
CREATE TABLE [dbo].[Category] (
    [Author]                 NVARCHAR(max)   NULL,
    [Category2ProjectsTitle] INT             NULL,
    [Created]                DATETIME        NULL,
    [Editor]                 NVARCHAR(max)   NULL,
    [ID]                     INT             NOT NULL,
    [Modified]               DATETIME        NULL,
    [owshiddenversion]       INT             NULL,
    [Title]                  NVARCHAR(max)   NOT NULL,
    [OnlySQL]				 BIT			 NOT NULL,	
	CONSTRAINT [PK_Category_ID] PRIMARY KEY CLUSTERED ([ID] ASC) ,
    CONSTRAINT [FK_Category_Projects] FOREIGN KEY ([Category2ProjectsTitle]) REFERENCES [dbo].[Projects] ([ID]),
);
