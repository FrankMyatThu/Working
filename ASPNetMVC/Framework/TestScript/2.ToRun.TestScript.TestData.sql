-- ---------------------------------------------------------------------------------------------------------------------------------------------------------
-- Create Test data for Products
-- ---------------------------------------------------------------------------------------------------------------------------------------------------------

DECLARE @i INT
SET @i = 1
WHILE (@i <= 10000)
BEGIN
	INSERT INTO Products
	(
	    --ProductID - this column value is auto-generated
	    ProductName,
	    Description,
	    Price,
	    Active
	)
	VALUES
	(
	    -- ProductID - int
	    N'Test Product Name ' + CONVERT(VARCHAR(255), @i) + '', -- ProductName - nvarchar
	    N'Test Product Description ' + CONVERT(VARCHAR(255), @i) + '', -- Description - nvarchar
	    @i, -- Price - decimal
	    1 -- Active - bit
	)
	SET @i = @i + 1
END
-- ---------------------------------------------------------------------------------------------------------------------------------------------------------
-- Create Test data for Orders
-- ---------------------------------------------------------------------------------------------------------------------------------------------------------

INSERT INTO Orders
(
    --OrderId - this column value is auto-generated
    Description,
    OrderDate
)
VALUES
(
    -- OrderId - int
    N'Order One', -- Description - nvarchar
    '2016-01-27 18:23:04' -- OrderDate - datetime
);

INSERT INTO Orders
(
    --OrderId - this column value is auto-generated
    Description,
    OrderDate
)
VALUES
(
    -- OrderId - int
    N'Order Two', -- Description - nvarchar
    '2016-01-29 18:23:42' -- OrderDate - datetime
);

-- ---------------------------------------------------------------------------------------------------------------------------------------------------------
-- Create Test data for OrderDetails
-- ---------------------------------------------------------------------------------------------------------------------------------------------------------
-- For Order One
INSERT INTO OrderDetails
(
    --OrderDetailId - this column value is auto-generated
    OrderId,
    ProductID,
    Quantity,
    Total,
    TotalGST
)
VALUES
(
    -- OrderDetailId - int
    1, -- OrderId - int
    1, -- ProductID - int
    1, -- Quantity - int
    1, -- Total - decimal
    1.07 -- TotalGST - decimal
);

INSERT INTO OrderDetails
(
    --OrderDetailId - this column value is auto-generated
    OrderId,
    ProductID,
    Quantity,
    Total,
    TotalGST
)
VALUES
(
    -- OrderDetailId - int
    1, -- OrderId - int
    2, -- ProductID - int
    3, -- Quantity - int
    6, -- Total - decimal
    6.42 -- TotalGST - decimal
);

-- For Order Two
INSERT INTO OrderDetails
(
    --OrderDetailId - this column value is auto-generated
    OrderId,
    ProductID,
    Quantity,
    Total,
    TotalGST
)
VALUES
(
    -- OrderDetailId - int
    2, -- OrderId - int
    1, -- ProductID - int
    1, -- Quantity - int
    1, -- Total - decimal
    1.07 -- TotalGST - decimal
);

INSERT INTO OrderDetails
(
    --OrderDetailId - this column value is auto-generated
    OrderId,
    ProductID,
    Quantity,
    Total,
    TotalGST
)
VALUES
(
    -- OrderDetailId - int
    2, -- OrderId - int
    3, -- ProductID - int
    5, -- Quantity - int
    15, -- Total - decimal
    16.05 -- TotalGST - decimal
);

-- ---------------------------------------------------------------------------------------------------------------------------------------------------------
-- Select ...
-- ---------------------------------------------------------------------------------------------------------------------------------------------------------

SELECT * FROM Products 
SELECT * FROM Orders 
SELECT * FROM OrderDetails 


    
