CustomerRequirement{
	Expectations: Production code quality, clean, decoupled design, highly testable and mock-able.
	Tools: .NET 4.5, MVC, Entity Framework, IOC/DI, Loggers, Diagnostics, etc.

	Order Entry Database
	Use SQL server (any version you prefer, or even express) and design the following tables:

	CREATE TABLE Products
	(
		ProductID INT IDENTITY(1,1) NOT NULL,
		ProductName NVARCHAR(250),
		Description NVARCHAR(250),
		Price DECIMAL(10,2),
		Active BIT,
		PRIMARY KEY (ProductID)
	)

	CREATE TABLE Orders
	(
		OrderId INT IDENTITY(1,1) NOT NULL,
		Description NVARCHAR(250),
		OrderDate DATETIME,
		PRIMARY KEY (OrderId)
	)

	CREATE TABLE OrderDetails
	(
		OrderDetailId INT IDENTITY(1,1) NOT NULL,
		OrderId INT,
		ProductID INT,
		Quantity INT,
		Total DECIMAL(10,2),
		TotalGST DECIMAL(10,2),
		
		PRIMARY KEY(OrderDetailId),

		FOREIGN KEY(OrderId) REFERENCES Orders(OrderId)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION,

		FOREIGN KEY(ProductID) REFERENCES Products(ProductID)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
	)

	*"GST" stands for Goods and Services Tax, fixed at 7%, calculated field.
	 Please showcase how you’re going to build this data entry application 
	 using MVC and .NET – 
	 feel free to use any tools you see fit.
	-          A data entry for products and Order entry form, 
	-          including a list of orders
	-          Unit tests
	-          Login page if you have time – External identity management if you have time, you can check Okta
	-          If you can publish this in cloud – Azure or AWS – much better
}
ProgrammerRequirement{
	OrderListing{
		
	}
	OrderDetail{
		
	}
	
	ProductListing{
	
	}
	ProductDetail{
	
	}
	
	UserLogin{}
}