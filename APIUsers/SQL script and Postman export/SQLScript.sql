DROP TABLE IF EXISTS dbo.Token;
DROP TABLE IF EXISTS dbo.Users;

CREATE TABLE Users
(
	userID int IDENTITY(1,1) PRIMARY KEY,
	email varchar(50) UNIQUE NOT NULL,
	password varchar(50),
	name varchar(50),
	title varchar(100),
	country varchar(100)
);

INSERT INTO Users VALUES ('admin@admin.com', 'Password123!', 'Admin', 'Admin User', 'Romania');
INSERT INTO Users VALUES ('eduard@gmail.com', 'ImE5KBUOWA808zYhwgY2nA==', 'Eduard Decu', 'Full stack Developer', 'Romania');
INSERT INTO Users VALUES ('john@gmail.com', 'ImE5KBUOWA808zYhwgY2nA==', 'John Peterson', 'Back-end Developer', 'UK');
INSERT INTO Users Values ('maria@gmail.com', 'ImE5KBUOWA808zYhwgY2nA==', 'Ana Maria', 'Front-end Developer', 'Germany');

CREATE TABLE Token(
	id int IDENTITY(1,1) PRIMARY KEY,
	refresh varchar(100),
	userID int FOREIGN KEY REFERENCES Users(userID) ON DELETE CASCADE
);

SELECT * FROM Users;
SELECT * FROM Token;

GO
CREATE TRIGGER delete_refresh ON Token
AFTER INSERT
AS
	BEGIN
		declare @userID int;
		declare @tokenR varchar(100);

		select @userID = userObj.userID from inserted userObj
		select @tokenR = userObj.refresh from inserted userObj
		
		delete from Token where Token.userID = @userID and Token.refresh != @tokenR
	END;
GO