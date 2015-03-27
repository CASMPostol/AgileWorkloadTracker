if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'Requirements')
  drop table  Requirements;
CREATE TABLE [dbo].[Requirements] (
    [Author]                 NVARCHAR(max)   NULL,
    [Body]                   NVARCHAR(max)   NULL,
    [Created]                DATETIME        NULL,
    [Editor]                 NVARCHAR(max)   NULL,
    [EstimatedHours]         FLOAT           NULL,
    [Hours]                  FLOAT           NULL,
    [ID]                     INT             NOT NULL,
    [Modified]               DATETIME        NULL,
    [owshiddenversion]       INT             NULL,
    [RequirementPriority]    INT             NULL,
    [Requirements2MilestoneTitle] INT             NULL,
    [Requirements2ProjectsTitle] INT             NULL,
    [RequirementsType]       NVARCHAR(max)   NULL,
    [Title]                  NVARCHAR(max)   NOT NULL,
    [OnlySQL]				 BIT			 NOT NULL,	
	CONSTRAINT [PK_Requirements_ID] PRIMARY KEY CLUSTERED ([ID] ASC) ,
    CONSTRAINT [FK_Requirements_Milestone] FOREIGN KEY ([Requirements2MilestoneTitle]) REFERENCES [dbo].[Milestone] ([ID]),
    CONSTRAINT [FK_Requirements_Projects] FOREIGN KEY ([Requirements2ProjectsTitle]) REFERENCES [dbo].[Projects] ([ID]),
);
