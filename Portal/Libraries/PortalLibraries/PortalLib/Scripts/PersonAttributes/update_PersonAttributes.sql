USE portal
GO

UPDATE PersonAttributes
SET KeyWord = (
			SELECT ID FROM PersonAttributeTypes
			WHERE AttributeName = KeyWord 
			)
   
ALTER TABLE PersonAttributes
	ALTER COLUMN KeyWord int NOT NULL
	
EXEC sp_rename '[Portal].[dbo].[PersonAttributes].[KeyWord]', 
				'AttributeID', 'COLUMN'	 
GO

ALTER TABLE PersonAttributes
WITH CHECK 
	ADD CONSTRAINT FK_PersonAttributes_PersonAttributes FOREIGN KEY(AttributeID)
REFERENCES PersonAttributeTypes (ID) ON DELETE CASCADE
GO

ALTER TABLE PersonAttributes
	CHECK CONSTRAINT FK_PersonAttributes_PersonAttributes