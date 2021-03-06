USE [DataIngestion]
GO
/****** Object:  Table [dbo].[Artist]    Script Date: 2020-10-31 23:04:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Artist](
	[exportDate] [varchar](100) NULL,
	[artistId] [bigint] NOT NULL,
	[name] [varchar](max) NULL,
	[isActualArtist] [varchar](100) NULL,
	[viewUrl] [varchar](max) NULL,
	[artistTypeId] [varchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[artistId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ArtistCollection]    Script Date: 2020-10-31 23:04:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ArtistCollection](
	[exportDate] [varchar](100) NULL,
	[artistId] [bigint] NOT NULL,
	[collectionId] [bigint] NOT NULL,
	[isPrimaryArtist] [varchar](100) NULL,
	[roleId] [varchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[artistId] ASC,
	[collectionId] ASC,
	[roleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Collection]    Script Date: 2020-10-31 23:04:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Collection](
	[exportDate] [varchar](100) NULL,
	[collectionId] [bigint] NOT NULL,
	[name] [varchar](max) NULL,
	[titleVersion] [varchar](max) NULL,
	[searchTerms] [varchar](max) NULL,
	[parentalAdvisoryId] [varchar](100) NULL,
	[artistDisplayName] [varchar](max) NULL,
	[viewUrl] [varchar](max) NULL,
	[artworkUrl] [varchar](max) NULL,
	[originalReleaseDate] [varchar](100) NULL,
	[itunesReleaseDate] [varchar](100) NULL,
	[labelStudio] [varchar](max) NULL,
	[contentProviderName] [varchar](max) NULL,
	[copyright] [varchar](max) NULL,
	[pLine] [varchar](max) NULL,
	[mediaTypeId] [varchar](100) NULL,
	[isCompilation] [varchar](100) NULL,
	[collectionTypeId] [varchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[collectionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CollectionMatch]    Script Date: 2020-10-31 23:04:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CollectionMatch](
	[exportDate] [varchar](100) NULL,
	[collectionId] [bigint] NOT NULL,
	[upc] [varchar](max) NULL,
	[grid] [varchar](max) NULL,
	[amgAlbumId] [varchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[collectionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
