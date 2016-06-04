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
	MAX(tbl_FlatTable.ConfirmedDate) OVER (PARTITION BY tbl_FlatTable.EmployeeName, tbl_FlatTable.Field_Type) AS Max_Date_By_Title,
	MAX(tbl_FlatTable.ConfirmedDate) OVER (PARTITION BY tbl_FlatTable.EmployeeName) AS Max_Date_By_Name
	FROM tbl_FlatTable 
)
SELECT 
	_PIVOT.Max_Date_By_Name AS ConfirmedDate,  -- /// This line is fake ConfirmDate because different ConfirmedDate in same group (PARTITION BY EmployeeName) can create more than one record(s) using null.
	_PIVOT.EmployeeName, 
	_PIVOT.Title, 
	_PIVOT.Department  
FROM (
	SELECT 
		Max_Date_By_Name, 
		EmployeeName, 
		Field_Type, 
		Field_Value 
	FROM _tbl_FlatTable 
	WHERE ConfirmedDate = Max_Date_By_Title -- /// This line kick out junior programmer row.
) _UpSizeDown
PIVOT (MAX(Field_Value) FOR Field_Type IN (Department, Title) ) AS _PIVOT
ORDER BY ConfirmedDate DESC
-- ---------------------------------------------------------------------------------------------------------------------------------------------------------------
-- Select All 
-- ---------------------------------------------------------------------------------------------------------------------------------------------------------------
SELECT * FROM tbl_FlatTable 