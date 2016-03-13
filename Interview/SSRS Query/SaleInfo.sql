SELECT * FROM Sales.SalesOrderDetail
SELECT * FROM Sales.SalesOrderHeader
SELECT * FROM Sales.SalesTerritory

-- Get Sale Info
SELECT 
Sales.SalesTerritory.Name,
DATEPART(yy, Sales.SalesOrderHeader.OrderDate) AS OrderdYear,
DATEPART(mm, Sales.SalesOrderHeader.OrderDate) AS OrderdMonth,
DATENAME(mm, Sales.SalesOrderHeader.OrderDate) AS OrderdMonthName,
Sales.SalesOrderDetail.LineTotal
FROM Sales.SalesOrderHeader
INNER JOIN Sales.SalesOrderDetail ON SalesOrderDetail.SalesOrderID = SalesOrderHeader.SalesOrderID
INNER JOIN Sales.SalesTerritory ON SalesTerritory.TerritoryID = SalesOrderHeader.TerritoryID
WHERE DATEPART(yy, Sales.SalesOrderHeader.OrderDate) = 2001
GROUP BY Sales.SalesTerritory.Name,
DATEPART(yy, Sales.SalesOrderHeader.OrderDate),
DATEPART(mm, Sales.SalesOrderHeader.OrderDate),
DATENAME(mm, Sales.SalesOrderHeader.OrderDate),
Sales.SalesOrderDetail.LineTotal
ORDER BY DATEPART(yy, Sales.SalesOrderHeader.OrderDate),
DATEPART(mm, Sales.SalesOrderHeader.OrderDate)


-- Get Sale Territory
SELECT 
Sales.SalesTerritory.Name AS SalesTerritoryValue,
Sales.SalesTerritory.Name AS SalesTerritoryName
FROM Sales.SalesTerritory
GROUP BY Sales.SalesTerritory.Name