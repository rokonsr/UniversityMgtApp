USE [master]
GO
/****** Object:  Database [UniversityMgtDb]    Script Date: 5/29/2017 10:09:13 PM ******/
CREATE DATABASE [UniversityMgtDb]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'UniversityMgtDb', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\UniversityMgtDb.mdf' , SIZE = 3136KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'UniversityMgtDb_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\UniversityMgtDb_log.ldf' , SIZE = 832KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [UniversityMgtDb] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [UniversityMgtDb].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [UniversityMgtDb] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [UniversityMgtDb] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [UniversityMgtDb] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [UniversityMgtDb] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [UniversityMgtDb] SET ARITHABORT OFF 
GO
ALTER DATABASE [UniversityMgtDb] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [UniversityMgtDb] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [UniversityMgtDb] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [UniversityMgtDb] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [UniversityMgtDb] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [UniversityMgtDb] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [UniversityMgtDb] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [UniversityMgtDb] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [UniversityMgtDb] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [UniversityMgtDb] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [UniversityMgtDb] SET  ENABLE_BROKER 
GO
ALTER DATABASE [UniversityMgtDb] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [UniversityMgtDb] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [UniversityMgtDb] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [UniversityMgtDb] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [UniversityMgtDb] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [UniversityMgtDb] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [UniversityMgtDb] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [UniversityMgtDb] SET RECOVERY FULL 
GO
ALTER DATABASE [UniversityMgtDb] SET  MULTI_USER 
GO
ALTER DATABASE [UniversityMgtDb] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [UniversityMgtDb] SET DB_CHAINING OFF 
GO
ALTER DATABASE [UniversityMgtDb] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [UniversityMgtDb] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
EXEC sys.sp_db_vardecimal_storage_format N'UniversityMgtDb', N'ON'
GO
USE [UniversityMgtDb]
GO
/****** Object:  StoredProcedure [dbo].[uspAllocationClassRoom]    Script Date: 5/29/2017 10:09:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[uspAllocationClassRoom]
	-- Add the parameters for the stored procedure here
	@DepartmentId int,
	@CourseId int,
	@RoomId int,
	@Day nvarchar(50),
	@StartTime nvarchar(50),
	@EndTime nvarchar(50)
AS
BEGIN
	
	INSERT INTO ClassRoomAllocation(DepartmentId, CourseId, ClassRoomId, [Day], StartTime, EndTime, IsAllocate)
	VALUES(@DepartmentId, @CourseId, @RoomId, @Day, @StartTime, @EndTime, 1)

END



GO
/****** Object:  StoredProcedure [dbo].[uspClassScheduleInfo]    Script Date: 5/29/2017 10:09:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[uspClassScheduleInfo]
	-- Add the parameters for the stored procedure here
	@DepartmentId int = null
AS
BEGIN
	
	SELECT c.CourseId, CourseCode, CourseName, cr.RoomNumber, cra.[Day], format(convert(datetime, StartTime), N'hh:mm tt') + ' - ' + format(convert(datetime, EndTime), N'hh:mm tt') AS ClassTime INTO #ClassSchedule FROM Course c
	LEFT JOIN ClassRoomAllocation cra ON c.CourseId = cra.CourseId
	LEFT JOIN ClassRoom cr ON cra.ClassRoomId = cr.ClassRoomId
	WHERE cra.IsAllocate = 1

	SELECT c.CourseCode, c.CourseName, ISNULL(
	STUFF((
	SELECT '; ' + 'R. No : ' + RoomNumber + ', ' + [Day] + ' ' + ClassTime
	FROM #ClassSchedule b
	WHERE (b.CourseId = c.CourseId) 
	FOR XML PATH(''),TYPE).value('(./text())[1]','VARCHAR(MAX)')
	,1,2,''), 'Not Scheduled Yet') AS ScheduleInfo 
	FROM Course c
	LEFT JOIN #ClassSchedule cs ON c.CourseId = cs.CourseId
	WHERE c.DepartmentId = @DepartmentId
	GROUP BY c.CourseCode, c.CourseName, c.CourseId

END



GO
/****** Object:  StoredProcedure [dbo].[uspGetAllCourseDetails]    Script Date: 5/29/2017 10:09:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[uspGetAllCourseDetails]
	@DepartmentId int = null
	
AS
BEGIN
	
	IF(@DepartmentId IS NOT NULL)
	BEGIN
		SELECT c.CourseId, c.CourseCode, c.CourseName, s.SemesterName, ISNULL(t.TeacherName, 'Not Assigned Yet') AS TeacherName FROM Course c
		INNER JOIN Semester s ON c.SemesterId = s.SemesterId
		LEFT JOIN CourseAssign ca ON c.CourseId = ca.CourseId AND ca.IsAssign = 1
		LEFT JOIN Teacher t ON ca.TeacherId = t.TeacherId
		WHERE c.DepartmentId = @DepartmentId
	END
	ELSE
	BEGIN
		SELECT c.CourseId, c.CourseCode, c.CourseName, s.SemesterName, ISNULL(t.TeacherName, 'Not Assigned Yet') AS TeacherName FROM Course c
		INNER JOIN Semester s ON c.SemesterId = s.SemesterId
		LEFT JOIN CourseAssign ca ON c.CourseId = ca.CourseId AND ca.IsAssign = 1
		LEFT JOIN Teacher t ON ca.TeacherId = t.TeacherId
	END
END


GO
/****** Object:  StoredProcedure [dbo].[uspGetCourseDetail]    Script Date: 5/29/2017 10:09:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[uspGetCourseDetail]
	-- Add the parameters for the stored procedure here
	@StudentId int = null
AS
BEGIN
	
	SELECT * FROM Course c
	INNER JOIN Department d ON c.DepartmentId = d.DepartmentId
	INNER JOIN Student s ON s.DepartmentId = d.DepartmentId
	WHERE StudentId = @StudentId

END



GO
/****** Object:  StoredProcedure [dbo].[uspGetEnrollCourseDetail]    Script Date: 5/29/2017 10:09:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[uspGetEnrollCourseDetail]
	-- Add the parameters for the stored procedure here
	@StudentId int = null
AS
BEGIN
	
	SELECT * FROM Course c
	INNER JOIN CourseEnrollment ce ON c.CourseId = ce.CourseId
	WHERE StudentId = @StudentId

END



GO
/****** Object:  StoredProcedure [dbo].[uspGetRegistrationNumber]    Script Date: 5/29/2017 10:09:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[uspGetRegistrationNumber]
	-- Add the parameters for the stored procedure here
	@DepartmentCode nvarchar(50)
AS
BEGIN
	
	SELECT @DepartmentCode +'-' + RIGHT(Convert(varchar,YEAR(GETDATE())), 4) + '-' + RIGHT('000'+ Convert(varchar,ISNULL(Max(Convert(integer, RIGHT(RegNumber, 3))),0)+ 1), 3) AS RegNumber FROM Student WHERE  LEFT(RegNumber, CHARINDEX('-', RegNumber) - 1)= @DepartmentCode

END



GO
/****** Object:  StoredProcedure [dbo].[uspGetStudentDetail]    Script Date: 5/29/2017 10:09:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[uspGetStudentDetail]
	-- Add the parameters for the stored procedure here
	@StudentId int = null
AS
BEGIN
	
	SELECT * FROM Student s
	INNER JOIN Department d ON s.DepartmentId = d.DepartmentId
	WHERE StudentId = @StudentId

END



GO
/****** Object:  StoredProcedure [dbo].[uspGetStudentResult]    Script Date: 5/29/2017 10:09:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[uspGetStudentResult]
	-- Add the parameters for the stored procedure here
	@StudentId int = null
AS
BEGIN
	
	SELECT CourseCode, CourseName, ISNULL(sr.GradeLetter, 'Not Graded Yet') AS GradeLetter
	FROM CourseEnrollment ce
	INNER JOIN Course c ON ce.CourseId = c.CourseId
	LEFT JOIN StudentResult sr ON ce.CourseId = sr.CourseId AND ce.StudentId = sr.StudentId
	WHERE ce.StudentId = @StudentId

END



GO
/****** Object:  StoredProcedure [dbo].[uspGetTeacherAssignCredit]    Script Date: 5/29/2017 10:09:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[uspGetTeacherAssignCredit]
	@TeacherId int = null
AS
BEGIN
	
	SELECT TeacherId, SUM(CourseCredit) AS AssignCredit FROM CourseAssign
	GROUP BY TeacherId, IsAssign
	HAVING TeacherId = @TeacherId AND IsAssign = 1

END



GO
/****** Object:  StoredProcedure [dbo].[uspIsClassRoomAllocated]    Script Date: 5/29/2017 10:09:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[uspIsClassRoomAllocated]
	-- Add the parameters for the stored procedure here
	@ScheduleDate nvarchar(50),
	@StartTime nvarchar(50),
	@EndTime nvarchar(50),
	@Day nvarchar(50),
	@RoomNumber int
AS
BEGIN
	
	SELECT * FROM ClassRoomAllocation 
	WHERE (((@StartTime BETWEEN StartTime AND EndTime) OR (@EndTime BETWEEN StartTime AND EndTime)) 
	OR (StartTime BETWEEN @StartTime AND @EndTime) OR (EndTime BETWEEN @StartTime AND @EndTime))
	AND [Day] = @ScheduleDate AND IsAllocate = 1 AND [Day] = @Day AND ClassRoomId = @RoomNumber

END



GO
/****** Object:  StoredProcedure [dbo].[uspSaveAssignCourse]    Script Date: 5/29/2017 10:09:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[uspSaveAssignCourse] 
	-- Add the parameters for the stored procedure here
	@DepartmentId int,
	@TeacherId int,
	@CourseId int,
	@CourseCredit numeric(5,2)
AS
BEGIN
	
	INSERT INTO CourseAssign(DepartmentId, TeacherId, CourseId, CourseCredit, IsAssign)
	VALUES(@DepartmentId, @TeacherId, @CourseId, @CourseCredit, 1)

END



GO
/****** Object:  StoredProcedure [dbo].[uspSaveCourse]    Script Date: 5/29/2017 10:09:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[uspSaveCourse]
	-- Add the parameters for the stored procedure here
	@Code nvarchar(50),
	@Name nvarchar(50),
	@Credit Numeric(3,2),
	@Description nvarchar(500) = null,
	@DepartmentId int,
	@SemesterId int
AS
BEGIN
	
	INSERT INTO Course(CourseCode, CourseName, Credit, [Description], DepartmentId, SemesterId)
	VALUES(@Code, @Name, @Credit, @Description, @DepartmentId, @SemesterId)

END



GO
/****** Object:  StoredProcedure [dbo].[uspSaveCourseEnroll]    Script Date: 5/29/2017 10:09:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[uspSaveCourseEnroll]
	-- Add the parameters for the stored procedure here
	@StudentId int,
	@CourseId int,
	@EnrollDate datetime
AS
BEGIN
	
	INSERT INTO CourseEnrollment(StudentId, CourseId, EnrollDate)
	VALUES(@StudentId, @CourseId, @EnrollDate)

END



GO
/****** Object:  StoredProcedure [dbo].[uspSaveDepartment]    Script Date: 5/29/2017 10:09:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[uspSaveDepartment]
	-- Add the parameters for the stored procedure here
	@Code nvarchar(50),
	@Name nvarchar(50)
AS
BEGIN
	
	INSERT INTO Department(DepartmentCode, DepartmentName)
	VALUES(@Code, @Name)

END



GO
/****** Object:  StoredProcedure [dbo].[uspSaveResult]    Script Date: 5/29/2017 10:09:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[uspSaveResult] 
	-- Add the parameters for the stored procedure here
	@StudentId int,
	@CourseId int,
	@GradeLetter nvarchar(50)
AS
BEGIN
	
	INSERT INTO StudentResult(StudentId, CourseId, GradeLetter)
	VALUES(@StudentId, @CourseId, @GradeLetter)

END



GO
/****** Object:  StoredProcedure [dbo].[uspSaveStudent]    Script Date: 5/29/2017 10:09:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[uspSaveStudent]
	-- Add the parameters for the stored procedure here
	@RegNumber nvarchar(50),
	@StudentName nvarchar(50),
	@Email nvarchar(50),
	@ContactNumber nvarchar(100),
	@Date datetime,
	@Address nvarchar(200),
	@DepartmentCode nvarchar(50)
AS
BEGIN
	
	Declare @DepartmentId int;

	SELECT @DepartmentId = DepartmentId FROM Department WHERE DepartmentCode = @DepartmentCode;

	INSERT INTO Student(RegNumber, StudentName, Email, ContactNumber, RegistrationDate, [Address], DepartmentId)
	VALUES(@RegNumber, @StudentName, @Email, @ContactNumber, @Date, @Address, @DepartmentId)

END



GO
/****** Object:  StoredProcedure [dbo].[uspSaveTeacher]    Script Date: 5/29/2017 10:09:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[uspSaveTeacher]
	-- Add the parameters for the stored procedure here
	@Name nvarchar(50),
	@Address nvarchar(200),
	@Email nvarchar(50),
	@ContactNumber nvarchar(50),
	@DesignationId int,
	@DepartmentId int,
	@Credit numeric(5,2)
AS
BEGIN
	
	INSERT INTO Teacher(TeacherName, Address, Email, ContactNumber, DesignationId, DepartmentId, CreditToBeTaken)
	VALUES(@Name, @Address, @Email, @ContactNumber, @DesignationId, @DepartmentId, @Credit)

END



GO
/****** Object:  Table [dbo].[ClassRoom]    Script Date: 5/29/2017 10:09:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ClassRoom](
	[ClassRoomId] [int] IDENTITY(1,1) NOT NULL,
	[RoomNumber] [nvarchar](50) NOT NULL,
	[RoomName] [nvarchar](50) NULL,
 CONSTRAINT [PK_ClassRoom] PRIMARY KEY CLUSTERED 
(
	[ClassRoomId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ClassRoomAllocation]    Script Date: 5/29/2017 10:09:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ClassRoomAllocation](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DepartmentId] [int] NOT NULL,
	[CourseId] [int] NOT NULL,
	[ClassRoomId] [int] NOT NULL,
	[Day] [nvarchar](50) NOT NULL,
	[StartTime] [time](7) NOT NULL,
	[EndTime] [time](7) NOT NULL,
	[IsAllocate] [bit] NOT NULL,
 CONSTRAINT [PK_ClassRoomAllocation] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Course]    Script Date: 5/29/2017 10:09:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Course](
	[CourseId] [int] IDENTITY(1,1) NOT NULL,
	[CourseCode] [nvarchar](50) NOT NULL,
	[CourseName] [nvarchar](50) NOT NULL,
	[Credit] [numeric](3, 2) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[DepartmentId] [int] NOT NULL,
	[SemesterId] [int] NOT NULL,
 CONSTRAINT [PK_Course] PRIMARY KEY CLUSTERED 
(
	[CourseId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CourseAssign]    Script Date: 5/29/2017 10:09:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CourseAssign](
	[CourseAssignId] [int] IDENTITY(1,1) NOT NULL,
	[DepartmentId] [int] NOT NULL,
	[TeacherId] [int] NOT NULL,
	[CourseId] [int] NOT NULL,
	[CourseCredit] [numeric](5, 2) NOT NULL,
	[IsAssign] [bit] NOT NULL,
 CONSTRAINT [PK_CourseAssign] PRIMARY KEY CLUSTERED 
(
	[CourseAssignId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CourseEnrollment]    Script Date: 5/29/2017 10:09:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CourseEnrollment](
	[EnrollmentId] [int] IDENTITY(1,1) NOT NULL,
	[StudentId] [int] NOT NULL,
	[CourseId] [int] NOT NULL,
	[EnrollDate] [date] NOT NULL,
 CONSTRAINT [PK_CourseEnrollment] PRIMARY KEY CLUSTERED 
(
	[EnrollmentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Department]    Script Date: 5/29/2017 10:09:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Department](
	[DepartmentId] [int] IDENTITY(1,1) NOT NULL,
	[DepartmentCode] [nvarchar](50) NOT NULL,
	[DepartmentName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Department] PRIMARY KEY CLUSTERED 
(
	[DepartmentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Designation]    Script Date: 5/29/2017 10:09:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Designation](
	[DesignationId] [int] IDENTITY(1,1) NOT NULL,
	[DesignationName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Designation] PRIMARY KEY CLUSTERED 
(
	[DesignationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Semester]    Script Date: 5/29/2017 10:09:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Semester](
	[SemesterId] [int] IDENTITY(1,1) NOT NULL,
	[SemesterName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Semester] PRIMARY KEY CLUSTERED 
(
	[SemesterId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Student]    Script Date: 5/29/2017 10:09:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Student](
	[StudentId] [int] IDENTITY(1,1) NOT NULL,
	[RegNumber] [nvarchar](50) NOT NULL,
	[StudentName] [nvarchar](50) NOT NULL,
	[Email] [nvarchar](50) NOT NULL,
	[ContactNumber] [nvarchar](100) NOT NULL,
	[RegistrationDate] [datetime] NOT NULL,
	[Address] [nvarchar](200) NOT NULL,
	[DepartmentId] [int] NOT NULL,
 CONSTRAINT [PK_Student] PRIMARY KEY CLUSTERED 
(
	[StudentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[StudentResult]    Script Date: 5/29/2017 10:09:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StudentResult](
	[StudentResultId] [int] IDENTITY(1,1) NOT NULL,
	[StudentId] [int] NOT NULL,
	[CourseId] [int] NOT NULL,
	[GradeLetter] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_StudentResult] PRIMARY KEY CLUSTERED 
(
	[StudentResultId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Teacher]    Script Date: 5/29/2017 10:09:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Teacher](
	[TeacherId] [int] IDENTITY(1,1) NOT NULL,
	[TeacherName] [nvarchar](50) NOT NULL,
	[Address] [nvarchar](200) NOT NULL,
	[Email] [nvarchar](50) NOT NULL,
	[ContactNumber] [nvarchar](50) NOT NULL,
	[DesignationId] [int] NOT NULL,
	[DepartmentId] [int] NOT NULL,
	[CreditToBeTaken] [numeric](5, 2) NOT NULL,
 CONSTRAINT [PK_Teacher] PRIMARY KEY CLUSTERED 
(
	[TeacherId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[ClassRoomAllocation]  WITH CHECK ADD  CONSTRAINT [FK_ClassRoomAllocation_ClassRoom] FOREIGN KEY([ClassRoomId])
REFERENCES [dbo].[ClassRoom] ([ClassRoomId])
GO
ALTER TABLE [dbo].[ClassRoomAllocation] CHECK CONSTRAINT [FK_ClassRoomAllocation_ClassRoom]
GO
ALTER TABLE [dbo].[ClassRoomAllocation]  WITH CHECK ADD  CONSTRAINT [FK_ClassRoomAllocation_Course] FOREIGN KEY([CourseId])
REFERENCES [dbo].[Course] ([CourseId])
GO
ALTER TABLE [dbo].[ClassRoomAllocation] CHECK CONSTRAINT [FK_ClassRoomAllocation_Course]
GO
ALTER TABLE [dbo].[ClassRoomAllocation]  WITH CHECK ADD  CONSTRAINT [FK_ClassRoomAllocation_Department] FOREIGN KEY([DepartmentId])
REFERENCES [dbo].[Department] ([DepartmentId])
GO
ALTER TABLE [dbo].[ClassRoomAllocation] CHECK CONSTRAINT [FK_ClassRoomAllocation_Department]
GO
ALTER TABLE [dbo].[Course]  WITH CHECK ADD  CONSTRAINT [FK_Course_Department] FOREIGN KEY([DepartmentId])
REFERENCES [dbo].[Department] ([DepartmentId])
GO
ALTER TABLE [dbo].[Course] CHECK CONSTRAINT [FK_Course_Department]
GO
ALTER TABLE [dbo].[Course]  WITH CHECK ADD  CONSTRAINT [FK_Course_Semester] FOREIGN KEY([SemesterId])
REFERENCES [dbo].[Semester] ([SemesterId])
GO
ALTER TABLE [dbo].[Course] CHECK CONSTRAINT [FK_Course_Semester]
GO
ALTER TABLE [dbo].[CourseAssign]  WITH CHECK ADD  CONSTRAINT [FK_CourseAssign_Course] FOREIGN KEY([CourseId])
REFERENCES [dbo].[Course] ([CourseId])
GO
ALTER TABLE [dbo].[CourseAssign] CHECK CONSTRAINT [FK_CourseAssign_Course]
GO
ALTER TABLE [dbo].[CourseAssign]  WITH CHECK ADD  CONSTRAINT [FK_CourseAssign_Department] FOREIGN KEY([DepartmentId])
REFERENCES [dbo].[Department] ([DepartmentId])
GO
ALTER TABLE [dbo].[CourseAssign] CHECK CONSTRAINT [FK_CourseAssign_Department]
GO
ALTER TABLE [dbo].[CourseAssign]  WITH CHECK ADD  CONSTRAINT [FK_CourseAssign_Teacher] FOREIGN KEY([TeacherId])
REFERENCES [dbo].[Teacher] ([TeacherId])
GO
ALTER TABLE [dbo].[CourseAssign] CHECK CONSTRAINT [FK_CourseAssign_Teacher]
GO
ALTER TABLE [dbo].[CourseEnrollment]  WITH CHECK ADD  CONSTRAINT [FK_CourseEnrollment_Course] FOREIGN KEY([CourseId])
REFERENCES [dbo].[Course] ([CourseId])
GO
ALTER TABLE [dbo].[CourseEnrollment] CHECK CONSTRAINT [FK_CourseEnrollment_Course]
GO
ALTER TABLE [dbo].[CourseEnrollment]  WITH CHECK ADD  CONSTRAINT [FK_CourseEnrollment_Student] FOREIGN KEY([StudentId])
REFERENCES [dbo].[Student] ([StudentId])
GO
ALTER TABLE [dbo].[CourseEnrollment] CHECK CONSTRAINT [FK_CourseEnrollment_Student]
GO
ALTER TABLE [dbo].[Student]  WITH CHECK ADD  CONSTRAINT [FK_Student_Department] FOREIGN KEY([DepartmentId])
REFERENCES [dbo].[Department] ([DepartmentId])
GO
ALTER TABLE [dbo].[Student] CHECK CONSTRAINT [FK_Student_Department]
GO
ALTER TABLE [dbo].[StudentResult]  WITH CHECK ADD  CONSTRAINT [FK_StudentResult_Course] FOREIGN KEY([CourseId])
REFERENCES [dbo].[Course] ([CourseId])
GO
ALTER TABLE [dbo].[StudentResult] CHECK CONSTRAINT [FK_StudentResult_Course]
GO
ALTER TABLE [dbo].[StudentResult]  WITH CHECK ADD  CONSTRAINT [FK_StudentResult_Student] FOREIGN KEY([StudentId])
REFERENCES [dbo].[Student] ([StudentId])
GO
ALTER TABLE [dbo].[StudentResult] CHECK CONSTRAINT [FK_StudentResult_Student]
GO
ALTER TABLE [dbo].[Teacher]  WITH CHECK ADD  CONSTRAINT [FK_Teacher_Department] FOREIGN KEY([DepartmentId])
REFERENCES [dbo].[Department] ([DepartmentId])
GO
ALTER TABLE [dbo].[Teacher] CHECK CONSTRAINT [FK_Teacher_Department]
GO
ALTER TABLE [dbo].[Teacher]  WITH CHECK ADD  CONSTRAINT [FK_Teacher_Designation] FOREIGN KEY([DepartmentId])
REFERENCES [dbo].[Designation] ([DesignationId])
GO
ALTER TABLE [dbo].[Teacher] CHECK CONSTRAINT [FK_Teacher_Designation]
GO
USE [master]
GO
ALTER DATABASE [UniversityMgtDb] SET  READ_WRITE 
GO
