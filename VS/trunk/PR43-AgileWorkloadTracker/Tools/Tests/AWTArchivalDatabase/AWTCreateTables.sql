USE AWTARCHIV
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
if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'Contracts')
  drop table  Contracts;
CREATE TABLE [dbo].[Contracts] (
    [Author]                 NVARCHAR(max)   NULL,
    [Body]                   NVARCHAR(max)   NULL,
    [ContractDate]           DATETIME        NULL,
    [ContractEndDate]        DATETIME        NULL,
    [ContractNumber]         NVARCHAR(max)   NULL,
    [ContractOffer]          NVARCHAR(max)   NULL,
    [Contracts2PartnersTitle] INT             NULL,
    [ContractSubject]        NVARCHAR(max)   NULL,
    [ContractValue]          FLOAT           NULL,
    [ContractWarrantyDate]   DATETIME        NULL,
    [Created]                DATETIME        NULL,
    [Currency]               NVARCHAR(max)   NULL,
    [Editor]                 NVARCHAR(max)   NULL,
    [ID]                     INT             NOT NULL,
    [Modified]               DATETIME        NULL,
    [Title]                  NVARCHAR(max)   NOT NULL,
    [OnlySQL]				 BIT			 NOT NULL,	
	CONSTRAINT [PK_Contracts_ID] PRIMARY KEY CLUSTERED ([ID] ASC) ,
    CONSTRAINT [FK_Contracts_Partners] FOREIGN KEY ([Contracts2PartnersTitle]) REFERENCES [dbo].[Partners] ([ID]),
);
if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'Stage')
  drop table  Stage;
CREATE TABLE [dbo].[Stage] (
    [Author]                 NVARCHAR(max)   NULL,
    [Created]                DATETIME        NULL,
    [Editor]                 NVARCHAR(max)   NULL,
    [ID]                     INT             NOT NULL,
    [Modified]               DATETIME        NULL,
    [Title]                  NVARCHAR(max)   NOT NULL,
    [OnlySQL]				 BIT			 NOT NULL,	
	CONSTRAINT [PK_Stage_ID] PRIMARY KEY CLUSTERED ([ID] ASC) 
);
if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'Projects')
  drop table  Projects;
CREATE TABLE [dbo].[Projects] (
    [AcceptedHours]          FLOAT           NULL,
    [Active]                 BIT             NULL,
    [AssignedTo]             NVARCHAR(max)   NULL,
    [Author]                 NVARCHAR(max)   NULL,
    [BaselineEnd]            DATETIME        NULL,
    [BaselineStart]          DATETIME        NULL,
    [Body]                   NVARCHAR(max)   NULL,
    [Created]                DATETIME        NULL,
    [Currency]               NVARCHAR(max)   NULL,
    [Editor]                 NVARCHAR(max)   NULL,
    [EstimatedHours]         FLOAT           NULL,
    [ID]                     INT             NOT NULL,
    [MilestoneHours]         FLOAT           NULL,
    [Modified]               DATETIME        NULL,
    [PONumber]               NVARCHAR(max)   NULL,
    [Project2ContractTitle]  INT             NULL,
    [Project2PartnersTitle]  INT             NULL,
    [Project2StageTitle]     INT             NULL,
    [ProjectBudget]          FLOAT           NULL,
    [ProjectEndDate]         DATETIME        NULL,
    [ProjectHours]           FLOAT           NULL,
    [ProjectNumber]          NVARCHAR(max)   NULL,
    [ProjectStartDate]       DATETIME        NULL,
    [ProjectType]            NVARCHAR(max)   NULL,
    [ProjectWarrantyDate]    DATETIME        NULL,
    [Title]                  NVARCHAR(max)   NOT NULL,
    [OnlySQL]				 BIT			 NOT NULL,	
	CONSTRAINT [PK_Projects_ID] PRIMARY KEY CLUSTERED ([ID] ASC) ,
    CONSTRAINT [FK_Projects_Contracts] FOREIGN KEY ([Project2ContractTitle]) REFERENCES [dbo].[Contracts] ([ID]),
    CONSTRAINT [FK_Projects_Partners] FOREIGN KEY ([Project2PartnersTitle]) REFERENCES [dbo].[Partners] ([ID]),
    CONSTRAINT [FK_Projects_Stage] FOREIGN KEY ([Project2StageTitle]) REFERENCES [dbo].[Stage] ([ID]),
);
if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'Category')
  drop table  Category;
CREATE TABLE [dbo].[Category] (
    [Author]                 NVARCHAR(max)   NULL,
    [Category2ProjectsTitle] INT             NULL,
    [Created]                DATETIME        NULL,
    [Editor]                 NVARCHAR(max)   NULL,
    [ID]                     INT             NOT NULL,
    [Modified]               DATETIME        NULL,
    [Title]                  NVARCHAR(max)   NOT NULL,
    [OnlySQL]				 BIT			 NOT NULL,	
	CONSTRAINT [PK_Category_ID] PRIMARY KEY CLUSTERED ([ID] ASC) ,
    CONSTRAINT [FK_Category_Projects] FOREIGN KEY ([Category2ProjectsTitle]) REFERENCES [dbo].[Projects] ([ID]),
);
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
    [Title]                  NVARCHAR(max)   NOT NULL,
    [OnlySQL]				 BIT			 NOT NULL,	
	CONSTRAINT [PK_Estimation_ID] PRIMARY KEY CLUSTERED ([ID] ASC) ,
    CONSTRAINT [FK_Estimation_Projects] FOREIGN KEY ([Estimation2ProjectTitle]) REFERENCES [dbo].[Projects] ([ID]),
);
if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'Milestone')
  drop table  Milestone;
CREATE TABLE [dbo].[Milestone] (
    [AcceptedHours]          FLOAT           NULL,
    [Active]                 BIT             NULL,
    [Author]                 NVARCHAR(max)   NULL,
    [BaselineEnd]            DATETIME        NULL,
    [BaselineStart]          DATETIME        NULL,
    [Created]                DATETIME        NULL,
    [Default]                BIT             NULL,
    [Editor]                 NVARCHAR(max)   NULL,
    [EstimatedHours]         FLOAT           NULL,
    [ID]                     INT             NOT NULL,
    [Milestone2ProjectTitle] INT             NULL,
    [Milestone2StageTitle]   INT             NULL,
    [MilestoneDescription]   NVARCHAR(max)   NULL,
    [MilestoneEnd]           DATETIME        NULL,
    [MilestoneHours]         FLOAT           NULL,
    [MilestoneStart]         DATETIME        NULL,
    [Modified]               DATETIME        NULL,
    [SortOrder]              FLOAT           NULL,
    [Title]                  NVARCHAR(max)   NOT NULL,
    [OnlySQL]				 BIT			 NOT NULL,	
	CONSTRAINT [PK_Milestone_ID] PRIMARY KEY CLUSTERED ([ID] ASC) ,
    CONSTRAINT [FK_Milestone_Projects] FOREIGN KEY ([Milestone2ProjectTitle]) REFERENCES [dbo].[Projects] ([ID]),
    CONSTRAINT [FK_Milestone_Stage] FOREIGN KEY ([Milestone2StageTitle]) REFERENCES [dbo].[Stage] ([ID]),
);
if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'Priority')
  drop table  Priority;
CREATE TABLE [dbo].[Priority] (
    [Author]                 NVARCHAR(max)   NULL,
    [Created]                DATETIME        NULL,
    [Editor]                 NVARCHAR(max)   NULL,
    [ID]                     INT             NOT NULL,
    [Modified]               DATETIME        NULL,
    [Title]                  NVARCHAR(max)   NOT NULL,
    [OnlySQL]				 BIT			 NOT NULL,	
	CONSTRAINT [PK_Priority_ID] PRIMARY KEY CLUSTERED ([ID] ASC) 
);
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
if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'Resolution')
  drop table  Resolution;
CREATE TABLE [dbo].[Resolution] (
    [Author]                 NVARCHAR(max)   NULL,
    [Created]                DATETIME        NULL,
    [Editor]                 NVARCHAR(max)   NULL,
    [ID]                     INT             NOT NULL,
    [Modified]               DATETIME        NULL,
    [Title]                  NVARCHAR(max)   NOT NULL,
    [OnlySQL]				 BIT			 NOT NULL,	
	CONSTRAINT [PK_Resolution_ID] PRIMARY KEY CLUSTERED ([ID] ASC) 
);
if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'SPStatus')
  drop table  SPStatus;
CREATE TABLE [dbo].[SPStatus] (
    [Active]                 BIT             NULL,
    [Author]                 NVARCHAR(max)   NULL,
    [Created]                DATETIME        NULL,
    [Editor]                 NVARCHAR(max)   NULL,
    [ID]                     INT             NOT NULL,
    [Modified]               DATETIME        NULL,
    [Title]                  NVARCHAR(max)   NOT NULL,
    [OnlySQL]				 BIT			 NOT NULL,	
	CONSTRAINT [PK_SPStatus_ID] PRIMARY KEY CLUSTERED ([ID] ASC) 
);
if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'Type')
  drop table  Type;
CREATE TABLE [dbo].[Type] (
    [Author]                 NVARCHAR(max)   NULL,
    [Created]                DATETIME        NULL,
    [Editor]                 NVARCHAR(max)   NULL,
    [ID]                     INT             NOT NULL,
    [Modified]               DATETIME        NULL,
    [Title]                  NVARCHAR(max)   NOT NULL,
    [OnlySQL]				 BIT			 NOT NULL,	
	CONSTRAINT [PK_Type_ID] PRIMARY KEY CLUSTERED ([ID] ASC) 
);
if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'Task')
  drop table  Task;
CREATE TABLE [dbo].[Task] (
    [Active]                 BIT             NULL,
    [AssignedTo]             NVARCHAR(max)   NULL,
    [Author]                 NVARCHAR(max)   NULL,
    [BaselineEnd]            DATETIME        NULL,
    [BaselineStart]          DATETIME        NULL,
    [Body]                   NVARCHAR(max)   NULL,
    [Created]                DATETIME        NULL,
    [Editor]                 NVARCHAR(max)   NULL,
    [Hours]                  FLOAT           NULL,
    [ID]                     INT             NOT NULL,
    [Modified]               DATETIME        NULL,
    [Task2CategoryTitle]     INT             NULL,
    [Task2MilestoneDefinedInTitle] INT             NULL,
    [Task2MilestoneResolvedInTitle] INT             NULL,
    [Task2ProjectTitle]      INT             NULL,
    [Task2RequirementsTitle] INT             NULL,
    [Task2SPriorityTitle]    INT             NULL,
    [Task2SResolutionTitle]  INT             NULL,
    [Task2StatusTitle]       INT             NULL,
    [Task2TypeTitle]         INT             NULL,
    [TaskDueDate]            DATETIME        NULL,
    [TaskEnd]                DATETIME        NULL,
    [TaskStart]              DATETIME        NULL,
    [Title]                  NVARCHAR(max)   NOT NULL,
    [OnlySQL]				 BIT			 NOT NULL,	
	CONSTRAINT [PK_Task_ID] PRIMARY KEY CLUSTERED ([ID] ASC) ,
    CONSTRAINT [FK_Task_Category] FOREIGN KEY ([Task2CategoryTitle]) REFERENCES [dbo].[Category] ([ID]),
    CONSTRAINT [FK_Task_MilestoneDefined] FOREIGN KEY ([Task2MilestoneDefinedInTitle]) REFERENCES [dbo].[Milestone] ([ID]),
    CONSTRAINT [FK_Task_MilestoneResolved] FOREIGN KEY ([Task2MilestoneResolvedInTitle]) REFERENCES [dbo].[Milestone] ([ID]),
    CONSTRAINT [FK_Task_Projects] FOREIGN KEY ([Task2ProjectTitle]) REFERENCES [dbo].[Projects] ([ID]),
    CONSTRAINT [FK_Task_Requirements] FOREIGN KEY ([Task2RequirementsTitle]) REFERENCES [dbo].[Requirements] ([ID]),
    CONSTRAINT [FK_Task_Priority] FOREIGN KEY ([Task2SPriorityTitle]) REFERENCES [dbo].[Priority] ([ID]),
    CONSTRAINT [FK_Task_Resolution] FOREIGN KEY ([Task2SResolutionTitle]) REFERENCES [dbo].[Resolution] ([ID]),
    CONSTRAINT [FK_Task_SPStatus] FOREIGN KEY ([Task2StatusTitle]) REFERENCES [dbo].[SPStatus] ([ID]),
    CONSTRAINT [FK_Task_Type] FOREIGN KEY ([Task2TypeTitle]) REFERENCES [dbo].[Type] ([ID]),
);
if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'Workload')
  drop table  Workload;
CREATE TABLE [dbo].[Workload] (
    [AssignedTo]             NVARCHAR(max)   NULL,
    [Author]                 NVARCHAR(max)   NULL,
    [Comments]               NVARCHAR(max)   NULL,
    [Created]                DATETIME        NULL,
    [Editor]                 NVARCHAR(max)   NULL,
    [EndDate]                DATETIME        NULL,
    [ID]                     INT             NOT NULL,
    [Modified]               DATETIME        NULL,
    [ReadOnly]               BIT             NULL,
    [StartDate]              DATETIME        NULL,
    [Title]                  NVARCHAR(max)   NOT NULL,
    [Workload2ProjectTitle]  INT             NULL,
    [Workload2StageTitle]    INT             NULL,
    [Workload2TaskID]        INT             NULL,
    [WorkloadHours]          FLOAT           NULL,
    [OnlySQL]				 BIT			 NOT NULL,	
	CONSTRAINT [PK_Workload_ID] PRIMARY KEY CLUSTERED ([ID] ASC) ,
    CONSTRAINT [FK_Workload_Projects] FOREIGN KEY ([Workload2ProjectTitle]) REFERENCES [dbo].[Projects] ([ID]),
    CONSTRAINT [FK_Workload_Stage] FOREIGN KEY ([Workload2StageTitle]) REFERENCES [dbo].[Stage] ([ID]),
    CONSTRAINT [FK_Workload_Task] FOREIGN KEY ([Workload2TaskID]) REFERENCES [dbo].[Task] ([ID]),
);
if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'History')
  drop table  History;
CREATE TABLE [dbo].[History] (
    [ID]                     INT IDENTITY(1,1) NOT NULL,
    [ListName]               NVARCHAR(255)   NOT NULL,
    [ItemID]                 INT             NOT NULL,
    [FieldName]              NVARCHAR(255)   NOT NULL,
    [FieldValue]             NVARCHAR(255)   NOT NULL,
    [Modified]               DATETIME        NOT NULL,
    [ModifiedBy]             NVARCHAR(255)   NOT NULL,
    [OnlySQL]				 BIT			 NOT NULL,	
	CONSTRAINT [PK_History_ID] PRIMARY KEY CLUSTERED ([ID] ASC) 
);
if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'ArchivingLogs')
  drop table  ArchivingLogs;
CREATE TABLE [dbo].[ArchivingLogs] (
    [ID]                     INT IDENTITY(1,1) NOT NULL,
    [ListName]               NVARCHAR(255)   NOT NULL,
    [ItemID]                 INT             NOT NULL,
    [Date]                   DATETIME        NOT NULL,
    [UserName]               NVARCHAR(255)   NOT NULL,
    [OnlySQL]				 BIT			 NOT NULL,	
	CONSTRAINT [PK_ArchivingLogs_ID] PRIMARY KEY CLUSTERED ([ID] ASC) 
);
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
