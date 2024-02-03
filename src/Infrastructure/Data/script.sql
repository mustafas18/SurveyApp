﻿CREATE VIEW [dbo].[vw_UserCategories]
AS
SELECT 
UC.Id,
UC.NameFa,
UC.NameEn,
(SELECT COUNT(1) FROM UserInfos WHERE CategoryId1 = UC.Id) UserCount
FROM UserCategories UC WHERE UC.IsDelete = 0
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
	-- Add the parameters for the stored procedure here
	@SheetId NVARCHAR(50), 
	@SheetVersion INT = 1
AS
BEGIN

WITH cte 
AS (
 SELECT  A.[VariableId]
        ,A.[InputValue]
       ,A.InputLabel AS AnswerLabel,
COUNT(1) AS [Count]
FROM [UserAnswers]   AS A
WHERE A.SheetId=(SELECT TOP (1) Id FROM dbo.Sheets WHERE SheetId=@SheetId AND Version=@SheetVersion)
    AND A.SurveyVersion=(SELECT MAX(Version) FROM UserSurveys WHERE [Guid]=(SELECT TOP(1) [Guid] FROM UserSurveys WHERE Id=A.SurveyId))       
	GROUP BY VariableId,[InputValue],A.InputLabel

UNION 
SELECT B.Id
,BV.Value [InputValue]
,BV.Label AnswerLabel,
0 AS [Count]
FROM dbo.Variables B
LEFT JOIN [VariableValueLabel] BV
ON B.Id=BV.VariableId
WHERE B.Id IN ((SELECT Id FROM dbo.Variables WHERE SheetId=@SheetId AND SheetVersion=@SheetVersion AND Deleted=0))
)
SELECT VariableId,[InputValue],AnswerLabel,SUM([Count]) AS [AnswerCount] FROM cte
GROUP BY VariableId,[InputValue],AnswerLabel

END
