SELECT * FROM tbl_FlatTable 

SELECT 
*, MAX(ConfirmedDate) OVER (PARTITION BY EmployeeName, Field_Type) AS MAXDateByTitle
FROM tbl_FlatTable;
GO

WITH _tbl_FlatTable AS
(
	SELECT 
	*, 
	MAX(ConfirmedDate) OVER (PARTITION BY EmployeeName, Field_Type) AS MAXDateByTitle,
	MAX(ConfirmedDate) OVER (PARTITION BY EmployeeName) AS MAXDateByName
	FROM tbl_FlatTable 
)

SELECT 
_Pivot.MAXDateByName AS ConfirmedDate, 
_Pivot.EmployeeName,
_Pivot.Title,
_Pivot.Department
FROM
(
	SELECT 
		_tbl_FlatTable.MAXDateByName,
		_tbl_FlatTable.EmployeeName,
		_tbl_FlatTable.Field_Type,
		_tbl_FlatTable.Field_Value
	FROM _tbl_FlatTable 
	WHERE ConfirmedDate = MAXDateByTitle
) AS UpSizeDown
PIVOT (MAX(Field_Value) FOR Field_Type IN (Title, Department)) AS _Pivot

-- ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------
-- ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------
-- ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------

SELECT * FROM tbl_Array 
DELETE FROM tbl_Array

DECLARE @Count AS INT, @Value AS VARCHAR(255);
SET @Count = 1;
SET @Value = 'asiatechnic@gmail.com';
WHILE @Count <= LEN(@Value)
BEGIN

INSERT INTO tbl_Array
(
    Id,
    ColumnA
)
VALUES
(
    @Count, -- Id - int
    SUBSTRING(@Value, @Count, 1) -- ColumnA - varchar
)

SET @Count = @Count + 1;

END

-- ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------
-- ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------
-- ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------

SELECT * FROM tbl_EmpDep 
SELECT * FROM tbl_EmpDep WHERE JoinDate IN (SELECT MAX(tbl_EmpDep.JoinDate) FROM tbl_EmpDep GROUP BY tbl_EmpDep.DepartmentID)
SELECT * FROM 
(SELECT *, ROW_NUMBER() OVER (PARTITION BY tbl_EmpDep.DepartmentID ORDER BY tbl_EmpDep.JoinDate DESC) AS RowNumber FROM tbl_EmpDep) _tbl_EmpDep
WHERE RowNumber = 1

-- ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------
-- ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------
-- ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------

SELECT * FROM tbl_Table 
SELECT DENSE_RANK() OVER (ORDER BY tbl_Table.ColumnA), * FROM tbl_Table 

-- ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------
-- ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------
-- ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------