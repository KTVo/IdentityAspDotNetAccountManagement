use DemoDb

CREATE TABLE Products (
ProductID INT IDENTITY (1,1) PRIMARY KEY, -- Auto-incrementing primary key
ProductName VARCHAR (255) NOT NULL,
-- Product name (required)
SupplierID INT,
-- Foreign key to Suppliers table (nullable)
CategoryID INT,
-- Foreign key to Categories table (nullable)
UnitPrice DECIMAL (18, 2),
-- Price with precision and scale
UnitsInStock INT,
-- Number of units in stock
UnitsOnOrder INT,
-- Number of units on order
Discontinued BIT NOT NULL,
-- Boolean indicating if discontinued
DiscontinuedDate DATETIME2,
-- Date when discontinued (nullable)
-- Optional: Add foreign key constraints if you have Suppliers and Categories tables
-- FOREIGN KEY (SupplierID) REFERENCES Suppliers(SupplierID),
-- FOREIGN KEY (CategoryID) REFERENCES Categories
);

INSERT INTO Products (ProductName, SupplierID, CategoryID, UnitPrice, UnitsInStock, UnitsOnOrder, Discontinued, DiscontinuedDate)
VALUES
('Organic Apples', 1, 1, 1.50, 150, 20, 0, NULL), -- Supplier 1, Category 1
('Whole Wheat Bread', 2, 2, 2.75, 80, 15, 0, NULL), -- Supplier 2, Category 2
('Cheddar Cheese', 3, 3, 5.00, 60, 10, 0, NULL),
-- Supplier 3, Category 3
('Chicken Breast (1kg)', 4, 4, 8.50, 40, 5, 0, NULL), -- Supplier 4, Category 4
('Salmon Fillet (500g)', 5, 4, 12.00, 25, 0, 0, NULL), -- Supplier 5, Category 4
('Broccoli (500g)', 1, 1, 1.25, 100, 15, 0, NULL), -- Supplier 1, Category 1 ('Greek Yogurt', 2, 2, 3.25, 75, 10, 0, NULL),
-- Supplier 2, Category 2
('Swiss Cheese', 3, 3, 6.00, 50, 5, 0, NULL),
-- Supplier 3, Category 3
('Ground Beef (1kg)', 4, 4, 9.00, 35, 0, 0, NULL), -- Supplier 4, Category 4
('Tuna (5 cans)', 5, 4, 7.50, 60, 10, 0, NULL);
-- Sunnlier 5.
-- Supplier 5, Category 4


-- Example with a discontinued product:
INSERT INTO Products (ProductName, SupplierID, CategoryID, UnitPrice, UnitsInStock, UnitsOnOrder, Discontinued, DiscontinuedDate)
VALUES ('Discontinued Product', 1, 1, 10.00, 0, 0, 1, '2023-10-26');