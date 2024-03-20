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
	 SELECT [SurveyGuid],' + @cols  + N' FROM 
				 (
						SELECT A.[SurveyGuid],CAST(A.[InputValue] AS INT) [InputValue], V.[Name]
						FROM [UserAnswers] AS A
						INNER JOIN dbo.Variables V
							ON V.Id=A.VariableId
						WHERE A.Id IN (SELECT Id FROM dbo.UserAnswers WHERE SheetId=@SheetId1)
							AND A.SurveyVersion=(SELECT MAX(Version) FROM UserSurveys WHERE [Guid]=(SELECT TOP(1) [Guid] FROM UserSurveys WHERE Id=A.SurveyId))   
							AND V.Type=0 AND A.QuestionType NOT IN (3,4,7,8,9)
				) X
				PIVOT 
				(
					SUM([InputValue])
					FOR [Name] IN (' + @cols + N')
				) P ';
	 PRINT @query;
	 exec sp_executesql @query;
END
GO

CREATE  PROCEDURE [dbo].[sp_SurveyVariableData]
	-- Add the parameters for the stored procedure here
	@_surveyGuid NVARCHAR(50)
AS
BEGIN
DECLARE @SheetId VARCHAR(50)=(SELECT TOP(1) SheetId FROM [dbo].[UserSurveys] WHERE Guid=@_surveyGuid);
DECLARE @SheetVersion INT = (SELECT TOP(1) MAX(SheetVersion) FROM [dbo].[UserSurveys] WHERE Guid=@_surveyGuid);

;WITH cte1
AS 
(
	SELECT
		V.Id,
		V.Name,
		V.Type,
		SUM(CAST(UA.InputValue AS INT)) [Sum],
		CAST(1 AS BIT) ReadOnly
	FROM dbo.Variables V
	INNER JOIN dbo.UserAnswers UA
		ON UA.VariableId = V.Id
	WHERE V.Id IN ((SELECT Id FROM dbo.Variables WHERE SheetId=@SheetId AND SheetVersion=@SheetVersion AND Deleted=0))
	GROUP BY V.Id,V.Name,V.Type
), cte2
AS
(
	SELECT
		V.Id,
		V.Name,
		V.Type,
		0 [Sum],
		CAST(0 AS BIT) ReadOnly
	FROM dbo.Variables V
	right OUTER JOIN dbo.UserAnswers UA
		ON UA.VariableId = V.Id
	WHERE V.Id IN ((SELECT Id FROM dbo.Variables WHERE SheetId=@SheetId AND SheetVersion=@SheetVersion AND Deleted=0))
),variable_cte
AS(
	SELECT
		V.Id,
		V.Name,
		V.Type,
		0 [Sum],
		CAST(0 AS BIT) ReadOnly
	FROM dbo.Variables V
	WHERE V.Id IN ((SELECT Id FROM dbo.Variables WHERE SheetId=@SheetId AND SheetVersion=@SheetVersion AND Deleted=0))
)
SELECT * FROM cte1
UNION 
SELECT * FROM variable_cte
EXCEPT 
SELECT * FROM cte2
END
GO