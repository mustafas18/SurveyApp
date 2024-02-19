CREATE VIEW [dbo].[vw_UserCategories]
AS
SELECT 
UC.Id,
UC.NameFa,
UC.NameEn,
(SELECT COUNT(1) FROM UserInfos WHERE CategoryId1 = UC.Id) UserCount
FROM UserCategories UC WHERE UC.IsDelete = 0
GO

CREATE PROCEDURE [dbo].[sp_GetVariableAnswers]
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
INNER JOIN [VariableValueLabel] BV
ON B.Id=BV.VariableId
WHERE B.Id IN ((SELECT Id FROM dbo.Variables WHERE SheetId=@SheetId AND SheetVersion=@SheetVersion AND Deleted=0))
)
SELECT VariableId,[InputValue],AnswerLabel,SUM([Count]) AS [AnswerCount] FROM cte
GROUP BY VariableId,[InputValue],AnswerLabel

--ORDER BY [AnswerCount] DESC


END
GO

CREATE PROCEDURE [dbo].[sp_GetSheetVariables]
	-- Add the parameters for the stored procedure here
	@SheetId NVARCHAR(50), 
	@SheetVersion INT
AS
BEGIN
	IF (@SheetVersion IS NULL) 
		SET @SheetVersion= (SELECT MAX(Version) FROM Sheets WHERE SheetId=@SheetId);

		SELECT DISTINCT V.[Id]
                ,[Name]
                ,V.[Type]
                ,CASE WHEN [Label]='' THEN Q.Text ELSE V.Label END AS Label
				,(SELECT CONCAT('{', STRING_AGG(CONCAT([Label],':',[Value]), ','),'}') FROM [VariableValueLabel] WHERE VariableId=V.Id) AS valuesAsString
                ,[MaxValue]
                ,[Messure]
                ,V.[SheetId]
                ,V.[SheetVersion]
                ,V.[Deleted]
        FROM Variables AS V
		LEFT JOIN Questions Q
			ON V.Id=Q.VariableId
		WHERE V.Deleted=0 AND V.SheetId=@SheetId AND V.SheetVersion=@SheetVersion
END
GO

CREATE PROCEDURE [dbo].[sp_GetSurveyData]
	-- Add the parameters for the stored procedure here
	@SheetId NVARCHAR(50)
AS
BEGIN
	DECLARE @cols NVARCHAR(MAX);
	DECLARE @query NVARCHAR(MAX);

	select @cols=STUFF((SELECT ',' + QUOTENAME([Name])
	 FROM [iSurveyApp].[dbo].[Variables] V
	  WHERE V.SheetId=@SheetId AND  V.SheetVersion=(SELECT MAX(Version) FROM dbo.Sheets WHERE SheetId=V.SheetId) AND V.Deleted=0
	FOR XML PATH(''), TYPE
				).value('.', 'NVARCHAR(MAX)') 
			,1,1,'');


	 SET @query = N'
	 DECLARE @SheetId NVARCHAR(10) =''' + @SheetId + N''';
	 DECLARE @SheetId1 INT = (SELECT Id FROM dbo.Sheets WHERE SheetId=@SheetId AND Version=(SELECT MAX(Version) FROM dbo.Sheets WHERE SheetId=@SheetId));
	 SELECT [UserName],' + @cols  + N' FROM 
				 (
					SELECT A.[UserName],A.[InputValue], V.[Name]
					FROM [UserAnswers] AS A
					INNER JOIN dbo.Variables V
						ON V.Id=A.VariableId
					WHERE A.Id IN (SELECT Id FROM dbo.UserAnswers WHERE SheetId=@SheetId1)
						AND A.SurveyVersion=(SELECT MAX(Version) FROM UserSurveys WHERE [Guid]=(SELECT TOP(1) [Guid] FROM UserSurveys WHERE Id=A.SurveyId))       
				) X
				PIVOT 
				(
					MAX([InputValue])
					FOR [Name] IN (' + @cols + N')
				) P ';
	 PRINT @query;
	 exec sp_executesql @query;
END
GO