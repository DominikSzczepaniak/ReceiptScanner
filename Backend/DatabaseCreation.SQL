-- Create the Product table
CREATE TABLE Product (
    ID SERIAL PRIMARY KEY,
    Name VARCHAR(255) NOT NULL,
    Quantity_Weight DECIMAL(10, 2) NOT NULL,
    Price DECIMAL(10, 2) NOT NULL,
    OwnerID INT NOT NULL,
    Category VARCHAR(255)
);

-- Create the Receipt table
CREATE TABLE Receipt (
    ID SERIAL PRIMARY KEY,
    Date DATE NOT NULL,
    Shopname VARCHAR(255) NOT NULL,
    OwnerID INT NOT NULL
);

-- Create the ReceiptToProducts table
CREATE TABLE ReceiptToProducts (
    ReceiptID INT NOT NULL,
    ProductID INT NOT NULL,
    PRIMARY KEY (ReceiptID, ProductID),
    FOREIGN KEY (ReceiptID) REFERENCES Receipt(ID),
    FOREIGN KEY (ProductID) REFERENCES Product(ID)
);

-- Create the User table
CREATE TABLE Users (
    ID SERIAL PRIMARY KEY,
    Username VARCHAR(255) NOT NULL,
    Password VARCHAR(255) NOT NULL
);
