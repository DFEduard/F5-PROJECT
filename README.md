# F5-PROJECT
F5-PROJECT-BACKEND

Backend build. 
MS SQL Server, C#, Web API, REST Services

Front end: https://github.com/DFEduard/F5-PROJECT-FRONTEND

# APIS
- Get access token and refresh token:
  - POST: {{base_url}}/token/
  - Payload: {
    "email": "{{user email}}",
    "password": {{user password}}
  }
  - - Return: access token (lifetime 5 minutes) refresh token (used to get a new access token)
  
- Get new access token
  - POST: {{base_url}}/token/refresh/
  - Payload: {
    "refresh": "{{refresh token here}}"
  }
  - Return: new access token (lifetime 24 hours)
  
  
- Create user: 
  - POST: {{base_url}}/api/users
  - Payload: {
    "email": "{{email here}}",
    "password": "{{password here}}",
    "name": "{{name here}}",
    "title": "{{job title here}}",
    "country": "{{country here}}"
}
  - Return: user data

- Get users: 
  - GET: {{base_url}}/api/users
  - Return: list of users
  
- Get user: 
  - GET: {{base_url}}/api/users/{{user_id}}/
  - Returns: user
  
- Update user: 
  - PUT: {{base_url}}/api/users/{{user_id}}/
  - Returns: updated user
  
- Delete user: 
  - DELETE: {{base_url}}/api/users/{{user_id}}/
  - Returns: 200
 
# SQL script to create the db for this project
- Database name used to develop: WEB DB
- Connection string has to be changed with new DB 
- APIUsers > appsettings.json > appsettings.Development.json - line 10
```
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
INSERT INTO Users VALUES ('eduard@gmail.com', 'ImE5KBUOWA808zYhwgY2nA==', 'Eduard Edd', 'Full stack Developer', 'Romania');
INSERT INTO Users VALUES ('john@gmail.com', 'ImE5KBUOWA808zYhwgY2nA==', 'John Peter', 'Back-end Developer', 'UK');
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
```
