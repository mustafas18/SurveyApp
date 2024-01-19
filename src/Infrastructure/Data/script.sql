CREATE VIEW [dbo].[vw_UserCategories]
AS
SELECT 
UC.Id,
UC.NameFa,
UC.NameEn,
(SELECT COUNT(1) FROM UserInfos WHERE CategoryId = UC.Id) UserCount
FROM UserCategories UC WHERE UC.IsDelete = 0

