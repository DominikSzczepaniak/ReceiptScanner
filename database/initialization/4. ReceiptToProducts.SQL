CREATE TABLE ReceiptToProducts (
    ReceiptID INT NOT NULL,
    ProductID INT NOT NULL,
    PRIMARY KEY (ReceiptID, ProductID),
    FOREIGN KEY (ReceiptID) REFERENCES Receipt(ID),
    FOREIGN KEY (ProductID) REFERENCES Product(ID)
);