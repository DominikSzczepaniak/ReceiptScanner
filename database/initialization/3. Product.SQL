CREATE SEQUENCE ProductIdSeq START WITH 1 INCREMENT BY 1;

CREATE TABLE Product (
    ID BIGINT nextval('ProductIdSeq') PRIMARY KEY,
    Name VARCHAR(255) NOT NULL,
    QuantityWeight DECIMAL(10, 2) NOT NULL,
    Price DECIMAL(10, 2) NOT NULL,
    OwnerID INT NOT NULL REFERENCES Users(ID),
    Category VARCHAR(255)
);