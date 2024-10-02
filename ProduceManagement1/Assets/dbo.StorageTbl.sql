CREATE TABLE [dbo].[StorageTbl] (
    [ProductID]       INT          IDENTITY (1, 1) NOT NULL,
    [ProductName]     VARCHAR (50) NOT NULL,
    [ProductStorage]  INT          NOT NULL,
    [ProductRecieved] DATE         NOT NULL,
    [ProductBBE]      DATE         NOT NULL,
    CONSTRAINT [PK_StorageTbl] PRIMARY KEY CLUSTERED ([ProductID] ASC),
    CHECK ([ProductStorage]=(2) OR [ProductStorage]=(1))
);

