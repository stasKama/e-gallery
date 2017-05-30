<h1>E-Gallery project</h1>
<p>This program is designed to download, to view images with the opportunity to comment and like them. </p>
<p>This program uses the MS SQL database:</p>
<h4>Table Users</h4>
<pre>
  <code>
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
  </code>
</pre>

<p>User Email must be unique.</p>
<p>Password must contain at least 8 characters (Letters upper and lower register and digits).</p>
<p>This program has 2 types of user: Moderator and User.</p>
<p>The Moderator is responsible for filtering the images uploaded by the Users, gives permission to display new uploaded images.</p>
<p>The Moderator can delete images, that are placed in the public view.</p>
<p>Images uploaded by the Moderators  immediately are available to the public view.</p>
<p>The User has limit of the number of blocked and deleted images by the Moderator.</p>
<p>Each user can place a status on his page.</p>
<p>User can  edit CodeLanguage, Nick, Avatar, Status and Password.</p>
<p>User can view all users with a search by Nick, filter for the availability of Avatar or who is online at the moment and sort by popularity (Popularity calculates the total number of all the likes divided by the number of images uploaded by the user).</p>
<br/>

<h4>Table Pictures Waiting (image upload by the Users)</h4>
<pre>
  <code>
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
  </code>
</pre>
<b>Images have 3 status:</b>
<ul>
  <li>WAITING = 1 - The image uploaded by the User and waiting approval of the Moderator;</li>
  <li>VIEW = 2 - Image approved by the Moderator for public viewing;</li>
  <li>BLOCK = 3 - Image disapproved or deleted by the Moderator from public view.</li>
</ul>
<br/>

<h4>Table Answers</h4>
<pre>
  <code>
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
  </code>
</pre>
<p>This table is necessary to notify the user that his image was approved, blocked or deleted by the Moderator.</p>
<p>After reading the answer, it is deleted.</p>
<br/>

<h4>Table Images (Images admitted to public view)</h4>
<pre>
  <code>
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
  </code>
</pre>
<p>Every new image preview is recorded in the [CountView]text box.</p>
<p>It is not only possible to view all the pictures of all the users, but it is also possible to view all the pictures of only the one user on his personal page.</p>
<br/>

<h4>Table Likes Images</h4>
<pre>
  <code>
  CREATE TABLE [LikesToImages] (
    [Id]      INT IDENTITY (1, 1) NOT NULL,
    [UserId]  INT,
    [ImageId] INT NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    UNIQUE NONCLUSTERED ([UserId] ASC, [ImageId] ASC),
    FOREIGN KEY ([ImageId]) REFERENCES [Images] ([Id]) ON DELETE CASCADE,
    FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) 
  );
  </code>
</pre>

<p>User can put only the one like on the one image.</p>
<br/>

<h4>Table Comments Images</h4>
<pre>
  <code>
  CREATE TABLE [CommentsToImages] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [UserId]      INT           NOT NULL,
    [ImageId]     INT           NOT NULL,
    [Comment]     VARCHAR (300) NOT NULL,
    [DateComment] DATE          DEFAULT (getdate()) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([ImageId]) REFERENCES [Images] ([Id],
    FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
  );
  </code>
</pre>

<p>Any image can be commented any number of times by any user.</p>
<br/>

<h4>Table Verification</h4>
<pre>
  <code>
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
  </code>
</pre>

<p>When the user registers, the verification code is sent to the user email.</p>
<p>The user will not be active until the code is entered.</p>

<br/>

<h4>Information</h4>

<p>This project uses EntityFramework to interact with the database, Ajax to send data from the client to the server.</p>
<br/>

<h4>Screenshot project</h4>

<b>Login Page</b>
<img src="https://pp.userapi.com/c836322/v836322646/39ceb/1TypI2qAfVA.jpg"/>

<b>Registration Page</b>
<img src="https://pp.userapi.com/c836322/v836322646/39cf4/apuf_W5XOWI.jpg"/>

<b>Varification Page</b>
<img src="https://pp.userapi.com/c837138/v837138646/53d93/kcSZ9iisLUU.jpg"/>

<b>Users Page</b>
<img src="https://pp.userapi.com/c836322/v836322646/39d08/GBlCJb0BWSQ.jpg"/>

<b>History (Gallery All Users) Page</b>
<img src="https://pp.userapi.com/c836322/v836322646/39ce2/QsG4iFZrVco.jpg"/>

<b>Upload Image Page</b>
<img src="https://pp.userapi.com/c836322/v836322646/39cfe/feUw93yA3YQ.jpg"/>

<b>Edit Page</b>
<img src="https://pp.userapi.com/c837138/v837138646/539b2/bByKaMrxYOU.jpg"/>

<b>Edit Page if select Belarusian Language</b>
<img src="https://pp.userapi.com/c837138/v837138646/539bc/JKqJb2EbSHU.jpg"/>

<b>View Image</b>
<img src="https://pp.userapi.com/c836322/v836322646/39d12/6JrMooAVUFU.jpg"/>

<b>Answers Page</b>
<img src="https://cs7066.userapi.com/c837537/v837537646/3e539/huRDL3NgV1o.jpg"/>













