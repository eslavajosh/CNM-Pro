USE [CnmPro]
GO
/****** Object:  Table [dbo].[FAQs]    Script Date: 8/1/2022 3:29:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FAQs](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Question] [nvarchar](255) NOT NULL,
	[Answer] [nvarchar](2000) NOT NULL,
	[CategoryId] [int] NOT NULL,
	[SortOrder] [int] NOT NULL,
	[DateCreated] [datetime2](7) NOT NULL,
	[DateModified] [datetime2](7) NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[ModifiedBy] [int] NOT NULL,
 CONSTRAINT [PK_FAQs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FAQsCategories]    Script Date: 8/1/2022 3:29:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FAQsCategories](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_FAQsCategories] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[FAQs] ADD  CONSTRAINT [DF_FAQ_DateCreated]  DEFAULT (getutcdate()) FOR [DateCreated]
GO
ALTER TABLE [dbo].[FAQs] ADD  CONSTRAINT [DF_FAQ_DateModified]  DEFAULT (getutcdate()) FOR [DateModified]
GO
ALTER TABLE [dbo].[FAQs]  WITH CHECK ADD  CONSTRAINT [FK_FAQs_FAQsCategories] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[FAQsCategories] ([Id])
GO
ALTER TABLE [dbo].[FAQs] CHECK CONSTRAINT [FK_FAQs_FAQsCategories]
GO
ALTER TABLE [dbo].[FAQs]  WITH CHECK ADD  CONSTRAINT [FK_FAQs_Users] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[FAQs] CHECK CONSTRAINT [FK_FAQs_Users]
GO
ALTER TABLE [dbo].[FAQs]  WITH CHECK ADD  CONSTRAINT [FK_FAQs_Users1] FOREIGN KEY([ModifiedBy])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[FAQs] CHECK CONSTRAINT [FK_FAQs_Users1]
GO
/****** Object:  StoredProcedure [dbo].[FAQs_Delete_ById]    Script Date: 8/1/2022 3:29:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: Joshua Eslava
-- Create date: 7/19/2022
-- Description: Faq_Delete_ById
-- Code Reviewer: Thomas Sauer

-- MODIFIED BY: author
-- MODIFIED DATE:12/1/2020
-- Code Reviewer:
-- Note:
-- =============================================

CREATE PROC [dbo].[FAQs_Delete_ById]
@Id int 


as
/*
Declare @Id int = 3

Execute dbo.Faq_Delete_ById @Id

Select * from dbo.Faq
*/



BEGIN

DELETE FROM [dbo].[Faqs]
      WHERE @Id = Id

END


GO
/****** Object:  StoredProcedure [dbo].[FAQs_Insert]    Script Date: 8/1/2022 3:29:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: Joshua Eslava
-- Create date: 7/19/2022
-- Description: <FAQ_insert>
-- Code Reviewer: Thomas Sauer

-- MODIFIED BY: author
-- MODIFIED DATE:12/1/2020
-- Code Reviewer:
-- Note:
-- =============================================

CREATE proc [dbo].[FAQs_Insert]

@Question nvarchar(255)
,@Answer nvarchar(2000) 
,@CategoryId int
,@SortOrder int
,@CreatedBy int
,@ModifiedBy int
,@Id int OUTPUT


AS
/*


DECLARE @Question nvarchar(255) = 'question3'
		,@Answer nvarchar(2000) = 'answer3'
		,@CategoryId int = 3
		,@SortOrder int = 3
		,@CreatedBy int = 3
		,@ModifiedBy int = 3
		,@Id int = 3 

EXECUTE [dbo].[Faq_Insert]
		@Question 
		,@Answer 
		,@CategoryId 
		,@SortOrder 
		,@CreatedBy 
		,@ModifiedBy 
		,@Id OUTPUT

	Select *
	From [dbo].[Faq]
	Where Id = @Id

*/


BEGIN

INSERT INTO [dbo].[FAQs]
               ([Question]
               ,[Answer]
               ,[CategoryId]
               ,[SortOrder]
               ,[CreatedBy]
               ,[ModifiedBy])
         VALUES
               (@Question
               ,@Answer
               ,@CategoryId
               ,@SortOrder
               ,@CreatedBy
               ,@ModifiedBy)

        SELECT @Id = SCOPE_IDENTITY()

END


GO
/****** Object:  StoredProcedure [dbo].[FAQs_Select_ByCreatedBy]    Script Date: 8/1/2022 3:29:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: Joshua Eslava
-- Create date: 07/19/2022
-- Description: Paginated list returned of FAQs by CreatedBy
-- Code Reviewer: Thomas Sauer

-- MODIFIED BY: author
-- MODIFIED DATE:
-- Code Reviewer:
-- Note:
-- =============================================

CREATE PROC [dbo].[FAQs_Select_ByCreatedBy]
				@PageIndex	int
				,@PageSize	int
				,@CreatedBy	int

AS
/* ====================Test code====================

    Declare     @PageIndex	int = 0
				,@PageSize	int = 5
				,@CreatedBy	int = 52

    Execute [dbo].[FAQs_Select_ByCreatedBy]
				
                @PageIndex
				,@PageSize
				,@CreatedBy	

				Select * from dbo.FAQs
   ====================Test code==================== */
 
BEGIN
   DECLARE @Offset int = @PageIndex * @PageSize
	SELECT 
		  f.Id
		  ,f.Question
		  ,f.Answer
		  ,fc.Id as CategoryId
		  ,f.SortOrder
		  ,f.DateCreated
		  ,f.DateModified
		  ,f.CreatedBy
		  ,f.ModifiedBy
		  ,TotalCount = COUNT(1) OVER()
	  FROM [dbo].[FAQs] as f inner join [dbo].[FAQsCategories] as fc
		   on f.CategoryId = fc.Id
	  WHERE CreatedBy = @CreatedBy
	  ORDER BY f.Id
	  OFFSET @Offset ROWS
	  FETCH NEXT @PageSize ROWS ONLY
END
GO
/****** Object:  StoredProcedure [dbo].[FAQs_Select_ById]    Script Date: 8/1/2022 3:29:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: Joshua Eslava
-- Create date: 7/19/2022
-- Description: Faq_Select_ById
-- Code Reviewer: Thomas Sauer

-- MODIFIED BY: author
-- MODIFIED DATE:12/1/2020
-- Code Reviewer:
-- Note:
-- =============================================


CREATE proc [dbo].[FAQs_Select_ById]
@Id int

as

/*
Declare @Id int = 1 

Execute dbo.Faqs_Select_ById @Id

*/

BEGIN

SELECT [Id]
      ,[Question]
      ,[Answer]
      ,[CategoryId]
      ,[SortOrder]
      ,[DateCreated]
      ,[DateModified]
      ,[CreatedBy]
      ,[ModifiedBy]
  FROM [dbo].[Faqs]
  WHERE Id = @Id 

END


GO
/****** Object:  StoredProcedure [dbo].[FAQs_SelectAll]    Script Date: 8/1/2022 3:29:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: Joshua Eslava
-- Create date: 07/19/2022
-- Description: Paginated list returned of FAQs
-- Code Reviewer: Thomas Sauer

-- MODIFIED BY: author
-- MODIFIED DATE:
-- Code Reviewer: 
-- Note:
-- =============================================

CREATE PROC [dbo].[FAQs_SelectAll]
				@PageIndex	int
				,@PageSize	int
AS
/* ====================Test code====================

    Declare     @PageIndex	int = 0
				,@PageSize	int = 1

    Execute [dbo].[FAQs_SelectAll]
                @PageIndex
				,@PageSize

   ====================Test code==================== */

BEGIN

   DECLARE @Offset int = @PageIndex * @PageSize

	SELECT
		  f.Id
		  ,f.Question
		  ,f.Answer
		  ,fc.Id as CategoryId
		  ,f.SortOrder
		  ,f.DateCreated
		  ,f.DateModified
		  ,f.CreatedBy
		  ,f.ModifiedBy
		  ,TotalCount = COUNT(1) OVER()

	  FROM [dbo].[FAQs] as f inner join [dbo].[FAQsCategories] as fc
			on f.CategoryId = fc.Id
	  ORDER BY f.Id
	  OFFSET @Offset ROWS
	  FETCH NEXT @PageSize ROWS ONLY
END
GO
/****** Object:  StoredProcedure [dbo].[FAQs_Update]    Script Date: 8/1/2022 3:29:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: Joshua Eslava
-- Create date: 07/19/2022
-- Description: Updating FAQs
-- Code Reviewer: Thomas Sauer

-- MODIFIED BY: author
-- MODIFIED DATE: 
-- Code Reviewer: 
-- Note:
-- =============================================

CREATE PROC [dbo].[FAQs_Update]
			   @Id              int
               ,@Question       nvarchar(255)
               ,@Answer         nvarchar(2000)
               ,@CategoryId     int
               ,@SortOrder      int
               ,@CreatedBy      int
               ,@ModifiedBy     int


AS
/* ====================Test code====================

    Declare     @Id				int				= 1
				,@Question      nvarchar(255)   = 'question1updated'
                ,@Answer        nvarchar(2000)  = 'answer1updated'
				,@CategoryId    int				= 1
				,@SortOrder     int             = 1
                ,@CreatedBy     int             = 2
                ,@ModifiedBy    int             = 2

	Execute [dbo].[FAQs_Select_ById]	@Id

    Execute [dbo].[FAQs_Update]

                @Id
                ,@Question
                ,@Answer
                ,@CategoryId
                ,@SortOrder
                ,@CreatedBy
                ,@ModifiedBy

	Execute [dbo].[FAQs_Select_ById]	@Id

   ====================Test code==================== */

BEGIN


DECLARE @DateModified datetime2(7) = GETUTCDATE()

		UPDATE [dbo].[FAQs]
			SET [Question]		= @Question
				,[Answer]			= @Answer
				,[CategoryId]		= @CategoryId
				,[SortOrder]		= @SortOrder
				,[DateModified]	= @DateModified
				,[CreatedBy]		= @CreatedBy
				,[ModifiedBy]		= @ModifiedBy


	WHERE Id = @Id



END


GO
/****** Object:  StoredProcedure [dbo].[FAQsCategories_SelectAll]    Script Date: 8/1/2022 3:29:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[FAQsCategories_SelectAll]


/*
execute dbo.FAQsCategories_SelectAll

*/

as 

begin

	SELECT [Id]
		  ,[Name]
	  FROM [dbo].[FAQsCategories]

end

GO
