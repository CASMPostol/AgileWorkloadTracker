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
    [owshiddenversion]       INT             NULL,
    [Title]                  NVARCHAR(max)   NOT NULL,
    [OnlySQL]				 BIT			 NOT NULL,	
	CONSTRAINT [PK_Contracts_ID] PRIMARY KEY CLUSTERED ([ID] ASC) ,
    CONSTRAINT [FK_Contracts_Partners] FOREIGN KEY ([Contracts2PartnersTitle]) REFERENCES [dbo].[Partners] ([ID]),
);
