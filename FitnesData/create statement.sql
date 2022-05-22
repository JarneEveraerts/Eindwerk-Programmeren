CREATE TABLE [dbo].[beheerder] (
  [B_Id] [int] IDENTITY(1,1) NOT NULL UNIQUE,
  [B_Name] [varchar](45) NOT NULL UNIQUE,
  [B_Password] [varchar](45) NOT NULL,
  PRIMARY KEY CLUSTERED
(
[B_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY];

CREATE TABLE [dbo].[interesse] (
  [I_Id] [int] IDENTITY(1,1) NOT NULL UNIQUE,
  [I_Name] [varchar](45) NOT NULL UNIQUE,
PRIMARY KEY CLUSTERED
(
[I_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY];

CREATE TABLE [dbo].[klant] (
  [K_Id] [int] IDENTITY(1,1) NOT NULL UNIQUE,
  [K_FirstName] [varchar](45) NOT NULL,
  [K_Name] [varchar](45) NOT NULL,
  [K_Email] [varchar](45) NOT NULL UNIQUE,
  [K_Gemeente] [varchar](45) NOT NULL,
  [K_GeboorteDatum] [date] NOT NULL,
  [K_Intresse] [int] DEFAULT NULL,
  [K_Subscription] [int] NOT NULL,
PRIMARY KEY CLUSTERED
(
[K_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY];

CREATE TABLE [dbo].[status] (
  [S_Id] [int] IDENTITY(1,1) NOT NULL UNIQUE,
  [S_Name] [varchar](45) NOT NULL,
PRIMARY KEY CLUSTERED
(
[S_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY];

CREATE TABLE [dbo].[subscription] (
  [S_Id] [int] IDENTITY(1,1) NOT NULL UNIQUE,
  [S_Name] [varchar](45) NOT NULL,
PRIMARY KEY CLUSTERED
(
[S_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY];

CREATE TABLE [dbo].[toestellen] (
  [T_Id] [int] NOT NULL UNIQUE,
  [T_Name] [varchar](45) NOT NULL,
  [T_Status] [int] NOT NULL
PRIMARY KEY CLUSTERED
(
[T_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY];

CREATE TABLE [dbo].[reservatie] (
  [R_Id] [int] IDENTITY(1,1) NOT NULL UNIQUE,
  [R_KId] [varchar](45) NOT NULL,
  [R_FirstName] [varchar](45) NOT NULL,
  [R_Name] [varchar](45) NOT NULL,
  [R_Email] [varchar](45) NOT NULL,
  [R_Toestel] [varchar](45) NOT NULL,
  [R_Date] [DATE] NOT NULL,
  [R_Slot] [int] NOT NULL
PRIMARY KEY CLUSTERED
(
[R_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY];

INSERT INTO beheerder(B_Name,B_Password)
VALUES ('admin','admin')

INSERT INTO status (S_Name)
VALUES ('Beschikbaar')
INSERT INTO status (S_Name)
VALUES ('Onderhoud')

INSERT INTO interesse(I_Name)
VALUES('Cycling')
INSERT INTO interesse(I_Name)
VALUES('Fun')
INSERT INTO interesse(I_Name)
VALUES('Rowing')
INSERT INTO interesse(I_Name)
VALUES('Running')

INSERT INTO subscription(S_Name)
VALUES ('Bronze')
INSERT INTO subscription(S_Name)
VALUES ('Silver')
INSERT INTO subscription(S_Name)
VALUES ('Gold')