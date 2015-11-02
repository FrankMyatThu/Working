-- generate test data 1 million
DECLARE @i INT
SET @i = 1
WHILE (@i <= 250000)
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
		'2015-11-02 17:39:18', -- CreatedDate - datetime
		N'Developer', -- CreatedBy - nvarchar
		'2015-11-02 17:39:18', -- UpdatedDate - datetime
		N'Looping' -- UpdatedBy - nvarchar
	)
	SET @i = @i + 1
END
SELECT * FROM dbo.tbl_Country 