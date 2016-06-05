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

-- ---------------------------------------------------------------------------------------------------------------------------------------------------------------
-- Select All 
-- ---------------------------------------------------------------------------------------------------------------------------------------------------------------
SELECT * FROM tbl_FlatTable 