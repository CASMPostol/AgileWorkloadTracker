if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'Estimation')
  drop table  Estimation;
CREATE TABLE [dbo].[Estimation] (
    [AssignedTo]             NVARCHAR(max)   NULL,
    [Author]                 NVARCHAR(max)   NULL,
    [Created]                DATETIME        NULL,
    [Editor]                 NVARCHAR(max)   NULL,
    [EstimatedWorkload]      FLOAT           NULL,
    [Estimation2ProjectTitle] INT             NULL,
    [ID]                     INT             NOT NULL,
    [Modified]               DATETIME        NULL,
    [owshiddenversion]       INT             NULL,
    [Title]                  NVARCHAR(max)   NOT NULL,
    [OnlySQL]				 BIT			 NOT NULL,	
	CONSTRAINT [PK_Estimation_ID] PRIMARY KEY CLUSTERED ([ID] ASC) ,
    CONSTRAINT [FK_Estimation_Projects] FOREIGN KEY ([Estimation2ProjectTitle]) REFERENCES [dbo].[Projects] ([ID]),
);
