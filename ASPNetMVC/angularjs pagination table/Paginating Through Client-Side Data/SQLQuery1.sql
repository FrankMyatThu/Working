DECLARE @TotalRecordCount INT
SELECT @TotalRecordCount = COUNT(*) FROM tbl_Country 
SELECT
	@TotalRecordCount AS TotalRecordCount,
	ROW_NUMBER() OVER (ORDER BY CreatedDate ASC) AS SrNo,
	*
FROM tbl_Country