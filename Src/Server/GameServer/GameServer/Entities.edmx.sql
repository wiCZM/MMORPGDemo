
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 09/22/2023 01:27:35
-- Generated from EDMX file: D:\T2\Unity\Demo\Mymmorpg\Src\Server\GameServer\GameServer\Entities.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [ExtremeWorld];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_UserPlayer]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Users] DROP CONSTRAINT [FK_UserPlayer];
GO
IF OBJECT_ID(N'[dbo].[FK_PlayerCharacter]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Characters] DROP CONSTRAINT [FK_PlayerCharacter];
GO
IF OBJECT_ID(N'[dbo].[FK_CharacterItem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CharacterItems] DROP CONSTRAINT [FK_CharacterItem];
GO
IF OBJECT_ID(N'[dbo].[FK_CharacterBag]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Characters] DROP CONSTRAINT [FK_CharacterBag];
GO
IF OBJECT_ID(N'[dbo].[FK_CharacterQuest]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CharacterQuests] DROP CONSTRAINT [FK_CharacterQuest];
GO
IF OBJECT_ID(N'[dbo].[FK_CharacterFriend]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CharacterFriends] DROP CONSTRAINT [FK_CharacterFriend];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Users]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users];
GO
IF OBJECT_ID(N'[dbo].[Players]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Players];
GO
IF OBJECT_ID(N'[dbo].[Characters]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Characters];
GO
IF OBJECT_ID(N'[dbo].[CharacterItems]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CharacterItems];
GO
IF OBJECT_ID(N'[dbo].[CharacterBags]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CharacterBags];
GO
IF OBJECT_ID(N'[dbo].[CharacterQuests]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CharacterQuests];
GO
IF OBJECT_ID(N'[dbo].[CharacterFriends]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CharacterFriends];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [ID] bigint IDENTITY(1,1) NOT NULL,
    [Username] nvarchar(50)  NOT NULL,
    [Password] nvarchar(50)  NOT NULL,
    [RegisterDate] datetime  NULL,
    [Player_ID] int  NOT NULL
);
GO

-- Creating table 'Players'
CREATE TABLE [dbo].[Players] (
    [ID] int IDENTITY(1,1) NOT NULL
);
GO

-- Creating table 'Characters'
CREATE TABLE [dbo].[Characters] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [TID] int  NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Class] int  NOT NULL,
    [MapID] int  NOT NULL,
    [MapPosX] int  NOT NULL,
    [MapPosY] int  NOT NULL,
    [MapPosZ] int  NOT NULL,
    [Gold] bigint  NOT NULL,
    [Equips] binary(28)  NOT NULL,
    [Level] int  NOT NULL,
    [Exp] bigint  NOT NULL,
    [Player_ID] int  NOT NULL,
    [Bag_Id] int  NOT NULL
);
GO

-- Creating table 'CharacterItems'
CREATE TABLE [dbo].[CharacterItems] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ItemID] int  NOT NULL,
    [ItemCount] int  NOT NULL,
    [CharacterID] int  NOT NULL
);
GO

-- Creating table 'CharacterBags'
CREATE TABLE [dbo].[CharacterBags] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Items] varbinary(max)  NOT NULL,
    [Unlocked] int  NOT NULL
);
GO

-- Creating table 'CharacterQuests'
CREATE TABLE [dbo].[CharacterQuests] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CharacterID] int  NOT NULL,
    [QuestID] int  NOT NULL,
    [Target1] int  NOT NULL,
    [Target2] int  NOT NULL,
    [Target3] int  NOT NULL,
    [Status] int  NOT NULL
);
GO

-- Creating table 'CharacterFriends'
CREATE TABLE [dbo].[CharacterFriends] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [FriendID] int  NOT NULL,
    [FriendName] nvarchar(max)  NOT NULL,
    [Class] int  NOT NULL,
    [Level] int  NOT NULL,
    [CharacterID] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [ID] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Players'
ALTER TABLE [dbo].[Players]
ADD CONSTRAINT [PK_Players]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Characters'
ALTER TABLE [dbo].[Characters]
ADD CONSTRAINT [PK_Characters]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [Id] in table 'CharacterItems'
ALTER TABLE [dbo].[CharacterItems]
ADD CONSTRAINT [PK_CharacterItems]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CharacterBags'
ALTER TABLE [dbo].[CharacterBags]
ADD CONSTRAINT [PK_CharacterBags]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CharacterQuests'
ALTER TABLE [dbo].[CharacterQuests]
ADD CONSTRAINT [PK_CharacterQuests]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CharacterFriends'
ALTER TABLE [dbo].[CharacterFriends]
ADD CONSTRAINT [PK_CharacterFriends]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Player_ID] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [FK_UserPlayer]
    FOREIGN KEY ([Player_ID])
    REFERENCES [dbo].[Players]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserPlayer'
CREATE INDEX [IX_FK_UserPlayer]
ON [dbo].[Users]
    ([Player_ID]);
GO

-- Creating foreign key on [Player_ID] in table 'Characters'
ALTER TABLE [dbo].[Characters]
ADD CONSTRAINT [FK_PlayerCharacter]
    FOREIGN KEY ([Player_ID])
    REFERENCES [dbo].[Players]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PlayerCharacter'
CREATE INDEX [IX_FK_PlayerCharacter]
ON [dbo].[Characters]
    ([Player_ID]);
GO

-- Creating foreign key on [CharacterID] in table 'CharacterItems'
ALTER TABLE [dbo].[CharacterItems]
ADD CONSTRAINT [FK_CharacterItem]
    FOREIGN KEY ([CharacterID])
    REFERENCES [dbo].[Characters]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CharacterItem'
CREATE INDEX [IX_FK_CharacterItem]
ON [dbo].[CharacterItems]
    ([CharacterID]);
GO

-- Creating foreign key on [Bag_Id] in table 'Characters'
ALTER TABLE [dbo].[Characters]
ADD CONSTRAINT [FK_CharacterBag]
    FOREIGN KEY ([Bag_Id])
    REFERENCES [dbo].[CharacterBags]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CharacterBag'
CREATE INDEX [IX_FK_CharacterBag]
ON [dbo].[Characters]
    ([Bag_Id]);
GO

-- Creating foreign key on [CharacterID] in table 'CharacterQuests'
ALTER TABLE [dbo].[CharacterQuests]
ADD CONSTRAINT [FK_CharacterQuest]
    FOREIGN KEY ([CharacterID])
    REFERENCES [dbo].[Characters]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CharacterQuest'
CREATE INDEX [IX_FK_CharacterQuest]
ON [dbo].[CharacterQuests]
    ([CharacterID]);
GO

-- Creating foreign key on [CharacterID] in table 'CharacterFriends'
ALTER TABLE [dbo].[CharacterFriends]
ADD CONSTRAINT [FK_CharacterFriend]
    FOREIGN KEY ([CharacterID])
    REFERENCES [dbo].[Characters]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CharacterFriend'
CREATE INDEX [IX_FK_CharacterFriend]
ON [dbo].[CharacterFriends]
    ([CharacterID]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------