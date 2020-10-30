//USE [DataIngestion]
//GO

///****** Object:  Table [dbo].[Artist]    Script Date: 2020-10-29 22:12:17 ******/
//IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Artist]') AND type in (N'U'))
//DROP TABLE [dbo].[Artist]
//GO

///****** Object:  Table [dbo].[Artist]    Script Date: 2020-10-29 22:12:17 ******/
//SET ANSI_NULLS ON
//GO

//SET QUOTED_IDENTIFIER ON
//GO

//CREATE TABLE [dbo].[Artist](
//	[exportDate] [varchar](100) NULL,
//	[artistId] [varchar](100) NOT NULL,
//	[name] [varchar](max) NULL,
//	[isActualArtist] [varchar](100) NULL,
//	[viewUrl] [varchar](100) NULL,
//	[artistTypeId] [varchar](100) NULL,
//PRIMARY KEY CLUSTERED 
//(
//	[artistId] ASC
//)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
//) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
//GO




//USE [DataIngestion]
//GO

///****** Object:  Table [dbo].[ArtistCollection]    Script Date: 2020-10-29 22:12:52 ******/
//IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ArtistCollection]') AND type in (N'U'))
//DROP TABLE [dbo].[ArtistCollection]
//GO

///****** Object:  Table [dbo].[ArtistCollection]    Script Date: 2020-10-29 22:12:52 ******/
//SET ANSI_NULLS ON
//GO

//SET QUOTED_IDENTIFIER ON
//GO

//CREATE TABLE [dbo].[ArtistCollection](
//	[exportDate] [varchar](100) NULL,
//	[artistId] [varchar](100) NOT NULL,
//	[collectionId] [varchar](100) NOT NULL,
//	[isPrimaryArtist] [varchar](100) NULL,
//	[roleId] [varchar](100) NOT NULL,
//PRIMARY KEY CLUSTERED 
//(
//	[artistId] ASC,
//	[collectionId] ASC,
//	[roleId] ASC
//)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
//) ON [PRIMARY]
//GO


//USE [DataIngestion]
//GO

///****** Object:  Table [dbo].[Collection]    Script Date: 2020-10-29 22:13:20 ******/
//IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Collection]') AND type in (N'U'))
//DROP TABLE [dbo].[Collection]
//GO

///****** Object:  Table [dbo].[Collection]    Script Date: 2020-10-29 22:13:20 ******/
//SET ANSI_NULLS ON
//GO

//SET QUOTED_IDENTIFIER ON
//GO

//CREATE TABLE [dbo].[Collection](
//	[exportDate] [varchar](100) NULL,
//	[collectionId] [varchar](100) NOT NULL,
//	[name] [varchar](max) NULL,
//	[titleVersion] [varchar](max) NULL,
//	[searchTerms] [varchar](max) NULL,
//	[parentalAdvisoryId] [varchar](100) NULL,
//	[artistDisplayName] [varchar](max) NULL,
//	[viewUrl] [varchar](max) NULL,
//	[artworkUrl] [varchar](max) NULL,
//	[originalReleaseDate] [varchar](100) NULL,
//	[itunesReleaseDate] [varchar](100) NULL,
//	[labelStudio] [varchar](max) NULL,
//	[contentProviderName] [varchar](max) NULL,
//	[copyright] [varchar](max) NULL,
//	[pLine] [varchar](max) NULL,
//	[mediaTypeId] [varchar](100) NULL,
//	[isCompilation] [varchar](100) NULL,
//	[collectionTypeId] [varchar](100) NULL,
//PRIMARY KEY CLUSTERED 
//(
//	[collectionId] ASC
//)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
//) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
//GO


//USE [DataIngestion]
//GO

///****** Object:  Table [dbo].[CollectionMatch]    Script Date: 2020-10-29 22:13:45 ******/
//IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CollectionMatch]') AND type in (N'U'))
//DROP TABLE [dbo].[CollectionMatch]
//GO

///****** Object:  Table [dbo].[CollectionMatch]    Script Date: 2020-10-29 22:13:45 ******/
//SET ANSI_NULLS ON
//GO

//SET QUOTED_IDENTIFIER ON
//GO

//CREATE TABLE [dbo].[CollectionMatch](
//	[exportDate] [varchar](100) NULL,
//	[collectionId] [varchar](100) NOT NULL,
//	[upc] [varchar](max) NULL,
//	[grid] [varchar](max) NULL,
//	[amgAlbumId] [varchar](100) NULL,
//PRIMARY KEY CLUSTERED 
//(
//	[collectionId] ASC
//)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
//) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
//GO




////Scaffold-DbContext "Server=(localdb)\MSSQLLocalDB;Database=DataIngestion;Integrated Security=True" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models
////Scaffold-DbContext "Server=DESKTOP-RAISED2\SQLEXPRESS;Database=DataIngestion;Integrated Security=True" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models

//            if (!optionsBuilder.IsConfigured)
//            {
//                //TODO:move to vault or to appsettings.json
//                //optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=DataIngestion;Integrated Security=True");
//                optionsBuilder.UseSqlServer("Server=DESKTOP-RAISED2\SQLEXPRESS;Database=DataIngestion;Integrated Security=True");
//            }




//USE [master]
//GO

///****** Object:  Database [DataIngestion]    Script Date: 2020-10-30 08:06:02 ******/
//CREATE DATABASE [DataIngestion]
// CONTAINMENT = NONE
// ON  PRIMARY 
//( NAME = N'DataIngestion', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\DataIngestion.mdf' , SIZE = 1122304KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
// LOG ON 
//( NAME = N'DataIngestion_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\DataIngestion_log.ldf' , SIZE = 73728KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
// WITH CATALOG_COLLATION = DATABASE_DEFAULT
//GO

//IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
//begin
//EXEC [DataIngestion].[dbo].[sp_fulltext_database] @action = 'enable'
//end
//GO

//ALTER DATABASE [DataIngestion] SET ANSI_NULL_DEFAULT OFF 
//GO

//ALTER DATABASE [DataIngestion] SET ANSI_NULLS OFF 
//GO

//ALTER DATABASE [DataIngestion] SET ANSI_PADDING OFF 
//GO

//ALTER DATABASE [DataIngestion] SET ANSI_WARNINGS OFF 
//GO

//ALTER DATABASE [DataIngestion] SET ARITHABORT OFF 
//GO

//ALTER DATABASE [DataIngestion] SET AUTO_CLOSE OFF 
//GO

//ALTER DATABASE [DataIngestion] SET AUTO_SHRINK OFF 
//GO

//ALTER DATABASE [DataIngestion] SET AUTO_UPDATE_STATISTICS ON 
//GO

//ALTER DATABASE [DataIngestion] SET CURSOR_CLOSE_ON_COMMIT OFF 
//GO

//ALTER DATABASE [DataIngestion] SET CURSOR_DEFAULT  GLOBAL 
//GO

//ALTER DATABASE [DataIngestion] SET CONCAT_NULL_YIELDS_NULL OFF 
//GO

//ALTER DATABASE [DataIngestion] SET NUMERIC_ROUNDABORT OFF 
//GO

//ALTER DATABASE [DataIngestion] SET QUOTED_IDENTIFIER OFF 
//GO

//ALTER DATABASE [DataIngestion] SET RECURSIVE_TRIGGERS OFF 
//GO

//ALTER DATABASE [DataIngestion] SET  DISABLE_BROKER 
//GO

//ALTER DATABASE [DataIngestion] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
//GO

//ALTER DATABASE [DataIngestion] SET DATE_CORRELATION_OPTIMIZATION OFF 
//GO

//ALTER DATABASE [DataIngestion] SET TRUSTWORTHY OFF 
//GO

//ALTER DATABASE [DataIngestion] SET ALLOW_SNAPSHOT_ISOLATION OFF 
//GO

//ALTER DATABASE [DataIngestion] SET PARAMETERIZATION SIMPLE 
//GO

//ALTER DATABASE [DataIngestion] SET READ_COMMITTED_SNAPSHOT OFF 
//GO

//ALTER DATABASE [DataIngestion] SET HONOR_BROKER_PRIORITY OFF 
//GO

//ALTER DATABASE [DataIngestion] SET RECOVERY SIMPLE 
//GO

//ALTER DATABASE [DataIngestion] SET  MULTI_USER 
//GO

//ALTER DATABASE [DataIngestion] SET PAGE_VERIFY CHECKSUM  
//GO

//ALTER DATABASE [DataIngestion] SET DB_CHAINING OFF 
//GO

//ALTER DATABASE [DataIngestion] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
//GO

//ALTER DATABASE [DataIngestion] SET TARGET_RECOVERY_TIME = 60 SECONDS 
//GO

//ALTER DATABASE [DataIngestion] SET DELAYED_DURABILITY = DISABLED 
//GO

//ALTER DATABASE [DataIngestion] SET ACCELERATED_DATABASE_RECOVERY = OFF  
//GO

//ALTER DATABASE [DataIngestion] SET QUERY_STORE = OFF
//GO

//ALTER DATABASE [DataIngestion] SET  READ_WRITE 
//GO


