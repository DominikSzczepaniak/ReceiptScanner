CREATE SEQUENCE UserIdSeq START WITH 1 INCREMENT BY 1;

CREATE TABLE Users (
    ID BIGINT DEFAULT nextval('UserIdSeq') PRIMARY KEY,
    Username VARCHAR(255) NOT NULL,
    Password VARCHAR(255) NOT NULL
);