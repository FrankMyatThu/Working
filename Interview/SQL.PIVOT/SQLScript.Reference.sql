USE InterviewDB
-- ---------------------------------------------------------------------------------------------------------------------------------------------------------------
-- Create 
-- ---------------------------------------------------------------------------------------------------------------------------------------------------------------
CREATE TABLE tbl_FlatTable
(
	EmployeeName VARCHAR(100) NOT NULL,
	ConfirmedDate DATETIME,
	Field_Type VARCHAR(100) NOT NULL,
	Field_Value VARCHAR(100) NOT NULL
)

-- ---------------------------------------------------------------------------------------------------------------------------------------------------------------
-- Insert 
-- ---------------------------------------------------------------------------------------------------------------------------------------------------------------
INSERT INTO tbl_FlatTable 
(EmployeeName,ConfirmedDate,Field_Type,Field_Value)
VALUES 
('John', '2009/01/01', 'Title', 'Programmer'), 
('John', '2009/01/01', 'Department', 'IT Department'), 
('John', '2011/01/01', 'Title', 'Sr. Programmer'), 
('David', '2010/01/01', 'Title', 'Software Engineer'), 
('David', '2010/01/01', 'Department', 'IT Department');
-- ---------------------------------------------------------------------------------------------------------------------------------------------------------------
-- select table using PIVOT function
-- ---------------------------------------------------------------------------------------------------------------------------------------------------------------
;WITH _tbl_FlatTable AS
(
	SELECT *, 
		MAX(ConfirmedDate) OVER (PARTITION BY EmployeeName, Field_Type) AS MAXDate_OVER_EMP_FIELDTYPE,
		MAX(ConfirmedDate) OVER (PARTITION BY EmployeeName) AS MAXDate_OVER_EMP
	FROM tbl_FlatTable
)

SELECT ConfirmedDate = MAXDate_OVER_EMP, EmployeeName, Title, Department
FROM
(
	SELECT 
		EmployeeName,
		MAXDate_OVER_EMP,
		Field_Type,
		Field_Value
	FROM 
		_tbl_FlatTable
	WHERE ConfirmedDate = MAXDate_OVER_EMP_FIELDTYPE 	-- /// This line kick out junior programmer row.
) _tbl_FlatTable
PIVOT (MAX(Field_Value) FOR  Field_Type IN (Title, Department)) AS _PIVOT
ORDER BY ConfirmedDate DESC
-- ---------------------------------------------------------------------------------------------------------------------------------------------------------------
-- Select All 
-- ---------------------------------------------------------------------------------------------------------------------------------------------------------------
SELECT * FROM tbl_FlatTable 