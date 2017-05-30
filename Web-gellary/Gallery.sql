CREATE DATABASE Gallery;

USE Gallery;

CREATE TABLE [Users] (
    [Id]           INT           IDENTITY (1, 1) NOT NULL,
    [UserURL]      AS            (right('000000000'+CONVERT([varchar](9),[Id]),(9))) PERSISTED,
    [Email]        VARCHAR (40)  NOT NULL,
    [Nick]         VARCHAR (30)  NOT NULL,
    [Password]     VARCHAR (255) NOT NULL,
    [Avatar]       VARCHAR (255) DEFAULT ('http://www.teniteatr.ru/assets/no_avatar-e557002f44d175333089815809cf49ce.png') NULL,
    [State]        VARCHAR (20)  DEFAULT ('online') NULL,
    [Permission]   INT           DEFAULT ((10)) NOT NULL,
    [Role]         VARCHAR (20)  DEFAULT ('User') NOT NULL,
    [Status]       VARCHAR (255) NULL,
    [CodeLanguage] VARCHAR (5)   DEFAULT ('en') NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    UNIQUE NONCLUSTERED ([Email] ASC)
);

CREATE TABLE [PicturesWaiting] (
    [Id]         INT          IDENTITY (1, 1) NOT NULL,
    [Name]       AS           (right('000000000'+CONVERT([varchar](9),[Id]),(9))) PERSISTED,
    [Status]     INT          NOT NULL,
    [UserId]     INT          NOT NULL,
    [DateUpload] DATE         DEFAULT (getdate()) NULL,
    [Expansion]  VARCHAR (10) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [Answers] (
    [Id]        INT           IDENTITY (1, 1) NOT NULL,
    [UserId]    INT           NOT NULL,
    [PictureId] INT           NOT NULL,
    [Text]      VARCHAR (200) NOT NULL,
    [Date]      DATE          DEFAULT (getdate()) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    UNIQUE NONCLUSTERED ([PictureId] ASC),
    FOREIGN KEY ([PictureId]) REFERENCES [PicturesWaiting] ([Id]),
    FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [Images] (
    [Id]         INT          IDENTITY (1, 1) NOT NULL,
    [Name]       AS           (right('000000000'+CONVERT([varchar](9),[Id]),(9))) PERSISTED,
    [Expansion]  VARCHAR (10) NOT NULL,
    [DateUpload] DATE         DEFAULT (getdate()) NULL,
    [CountView]  INT          DEFAULT ((0)) NOT NULL,
    [UserId]     INT          NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [LikesToImages] (
    [Id]      INT IDENTITY (1, 1) NOT NULL,
    [UserId]  INT,
    [ImageId] INT NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    UNIQUE NONCLUSTERED ([UserId] ASC, [ImageId] ASC),
    FOREIGN KEY ([ImageId]) REFERENCES [Images] ([Id]) ON DELETE CASCADE,
    FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) 
);

CREATE TABLE [CommentsToImages] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [UserId]      INT           NOT NULL,
    [ImageId]     INT           NOT NULL,
    [Comment]     VARCHAR (300) NOT NULL,
    [DateComment] DATE          DEFAULT (getdate()) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([ImageId]) REFERENCES [Images] ([Id]),
    FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [Verification] (
    [Id]               INT         IDENTITY (1, 1) NOT NULL,
    [UserId]           INT         NOT NULL,
    [VerificationCode] VARCHAR (8) NOT NULL,
    [NumberAttempts] INT DEFAULT(4),
    [DateRegistration] DATE         DEFAULT (getdate()) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    UNIQUE([UserId]),
    FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
);
