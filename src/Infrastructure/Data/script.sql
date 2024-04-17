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
	IF (@SheetVersion IS NULL OR @SheetVersion=0) 
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
	 FROM [dbo].[Variables] V
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


ALTER  PROCEDURE [dbo].[sp_SurveyVariableData]
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
	 CAST(1 AS BIT)  ReadOnly
	FROM dbo.Variables V
	INNER JOIN dbo.UserAnswers UA
		ON UA.VariableId = V.Id
	WHERE V.Id IN ((SELECT Id FROM dbo.Variables WHERE SheetId=@SheetId AND SheetVersion=@SheetVersion AND Deleted=0))
		AND UA.QuestionType NOT IN (3,4,7,8,9) AND UA.InputLabel<>'$DynamicVariable$'
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
	AND UA.InputLabel<>'$DynamicVariable$'
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
	UNION 
	SELECT
		V.Id,
		V.Name,
		V.Type,
		SUM(CAST(UA.InputValue AS INT)) [Sum],
	 CAST(0 AS BIT)  ReadOnly
	FROM dbo.Variables V
	INNER JOIN dbo.UserAnswers UA
		ON UA.VariableId = V.Id
	WHERE V.Id IN ((SELECT Id FROM dbo.Variables WHERE SheetId=@SheetId AND SheetVersion=@SheetVersion AND Deleted=0))
		AND UA.QuestionType NOT IN (3,4,7,8,9) AND UA.InputLabel='$DynamicVariable$'
	GROUP BY V.Id,V.Name,V.Type
),result_cte
AS(
SELECT * FROM cte1
UNION 
SELECT * FROM variable_cte
EXCEPT 
SELECT * FROM cte2
)
SELECT Id,Name,Type,SUM([Sum]) [Sum],[ReadOnly] FROM result_cte
GROUP BY Id,Name,Type,ReadOnly
	END
GO

CREATE  PROCEDURE [dbo].[sp_UpdateVariable]
	-- Add the parameters for the stored procedure here
	@GuidId NVARCHAR(50),
	@VariableId INT,
	@InputValue INT 
AS
BEGIN
IF EXISTS(SELECT 1 FROM dbo.UserAnswers  WHERE [VariableId]=@VariableId AND [SurveyGuid]=@GuidId AND [SurveyVersion] = (SELECT MAX([SurveyVersion]) FROM UserAnswers WHERE [SurveyGuid]=@GuidId))
BEGIN 
	UPDATE UserAnswers SET [InputValue]=@InputValue
                                WHERE [VariableId]=@VariableId AND [SurveyGuid]=@GuidId AND [SurveyVersion] = (SELECT MAX([SurveyVersion]) FROM UserAnswers WHERE [SurveyGuid]=@GuidId);
      
END 
ELSE
BEGIN
DECLARE @SheetId INT,@SurveyVersion INT,@QuestionId INT,@AnswerId INT ,@QuestionType INT;
DECLARE @SurveyId INT , @UserResponseTime INT;
DECLARE @SurveyGuid varchar(50),@UserName varchar(50),@InputLabel varchar(50);

SET @InputLabel='$DynamicVariable$';

SELECT @SheetId=SheetId,@SurveyId=[SurveyId],
			@SurveyGuid=[SurveyGuid]
           ,@SurveyVersion=[SurveyVersion]
           ,@QuestionId=0
           ,@AnswerId=0
           ,@QuestionType=0
           ,@UserName=UserName
           ,@UserResponseTime=0
	FROM UserAnswers WHERE [SurveyGuid]=@GuidId AND [SurveyVersion] = (SELECT MAX([SurveyVersion]) FROM UserAnswers WHERE [SurveyGuid]=@GuidId);
	
	INSERT INTO [dbo].[UserAnswers]
           ([SheetId]
           ,[SurveyId]
           ,[SurveyGuid]
           ,[SurveyVersion]
           ,[QuestionId]
           ,[AnswerId]
           ,[QuestionType]
           ,[VariableId]
           ,[UserName]
           ,[InputLabel]
           ,[InputValue]
           ,[UserResponseTime])
     VALUES
           (@SheetId
           ,@SurveyId
           ,@SurveyGuid
           ,@SurveyVersion
           ,@QuestionId
           ,@AnswerId
           ,@QuestionType
           ,@VariableId
           ,@UserName
           ,@InputLabel
           ,@InputValue
           ,@UserResponseTime)
END
END
GO