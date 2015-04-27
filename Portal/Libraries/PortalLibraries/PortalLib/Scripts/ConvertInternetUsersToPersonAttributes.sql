USE Portal
GO

--------------------------
set quoted_identifier off
DECLARE
  @PersonID				INT,
  @Password				NVARCHAR(50),
  @PublicUserGroupID    INT

SET @PublicUserGroupID = (SELECT ID FROM Groups 
								WHERE GroupID LIKE 'PublicUser')

DECLARE AlterTable_Cursor CURSOR FOR
SELECT
	UserID				AS [userid],
	Password			AS [password]
FROM dbo.InternetUsers

OPEN AlterTable_Cursor

FETCH NEXT FROM AlterTable_Cursor
  INTO @PersonID, @Password

WHILE @@FETCH_STATUS = 0 
  BEGIN 

----------------- Add IN PersonAttributes -------------------------------
		IF NOT EXISTS (SELECT * FROM PersonAttributes 
						WHERE PersonID LIKE @PersonID
						AND KeyWord LIKE 'PublicPassword'
					   )
		BEGIN
			INSERT INTO PersonAttributes (PersonID, InsertionDate, KeyWord, ValueType, StringField)
			VALUES 
			(
				@PersonID,
				getdate(), 
				'PublicPassword', 
				'System.String, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089',
				@Password
			)
		END
	
		IF NOT EXISTS (SELECT * FROM Person2Group 
						WHERE PersonID LIKE @PersonID
						AND GroupID LIKE @PublicUserGroupID
					   )	
		BEGIN
			INSERT INTO Person2Group (PersonID, GroupID)
				VALUES (@PersonID, @PublicUserGroupID)
		END

-------------------------------------------------------------------------
	FETCH NEXT FROM AlterTable_Cursor
		INTO @PersonID, @Password
  END 

CLOSE AlterTable_Cursor
DEALLOCATE AlterTable_Cursor
set quoted_identifier on