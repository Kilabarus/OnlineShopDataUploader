CREATE DATABASE OnlineShop_DB;
USE OnlineShop_DB;

CREATE TABLE Customer (
	Customer_ID  INT 		   CONSTRAINT PK_Customer_ID PRIMARY KEY IDENTITY,
	Fullname 	 NVARCHAR(80)  NOT NULL,
	Email 		 VARCHAR(255)  CONSTRAINT UQ_Customer_Email UNIQUE NOT NULL
);

CREATE TABLE Purchase (
	Purchase_ID		   INT	  CONSTRAINT PK_Purchase_ID PRIMARY KEY,
	Customer_ID		   INT	  CONSTRAINT FK_Customer_ID FOREIGN KEY REFERENCES Customer (Customer_ID),
	Registration_Date  DATE	  NOT NULL,
	Total_Sum		   MONEY  NOT NULL
);

CREATE TABLE Product (
	Product_ID  INT 		   CONSTRAINT PK_Product_ID PRIMARY KEY IDENTITY,
	Title 		NVARCHAR(255)  NOT NULL,
	Price 		MONEY		   NOT NULL
);

CREATE TABLE Purchase_Product (
	Purchase_ID  INT  CONSTRAINT FK_Purchase_ID FOREIGN KEY REFERENCES Purchase (Purchase_ID),
	Product_ID 	 INT  CONSTRAINT FK_Product_ID FOREIGN KEY REFERENCES Product (Product_ID),
	Quantity 	 INT  CONSTRAINT CK_Quantity_Greater_Than_0 CHECK (Quantity > 0) NOT NULL,
		
	CONSTRAINT PK_Purchase_Product_ID PRIMARY KEY (Purchase_ID, Product_ID)
);