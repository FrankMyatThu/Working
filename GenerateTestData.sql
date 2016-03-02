-- generate test data 100 thousand
DECLARE @i INT
SET @i = 1 
WHILE (@i <= 100000)
BEGIN
	--INSERT INTO tbl_Array (Id, ColumnA) VALUES (@i, SUBSTRING(@Value, @i, 1));
	INSERT INTO dbo.tbl_Country
	(
		Id,
		Name,		
		IsActive,
		CreatedDate,
		CreatedBy,
		UpdatedDate,
		UpdatedBy
	)
	VALUES
	(
		NEWID(), -- Id - uniqueidentifier
		N'Test Data ' + CONVERT(VARCHAR(255), @i) + ' value', -- Name - nvarchar		
		0, -- IsActive - bit
		GETDATE(), -- CreatedDate - datetime
		N'B59D79E2-C4B8-4E9F-9B63-E4E51ABE6E4B', -- CreatedBy - nvarchar
		GETDATE(), -- UpdatedDate - datetime
		N'B59D79E2-C4B8-4E9F-9B63-E4E51ABE6E4B' -- UpdatedBy - nvarchar
	)
	SET @i = @i + 1
END
SELECT * FROM dbo.tbl_Country 