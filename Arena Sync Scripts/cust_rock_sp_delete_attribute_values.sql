
CREATE PROC [dbo].[cust_rock_sp_delete_attribute_values]
@PersonId int

AS

DECLARE @EntityId int

SELECT
	 @EntityId = [foreign_key]
FROM [core_person] WITH (NOLOCK)
WHERE [person_id] = @PersonId

IF @EntityId IS NOT NULL
BEGIN

	DELETE [RockChMS].[dbo].[coreAttributeValue] 
	WHERE [AttributeId] IN (
		SELECT [Id]
		FROM [RockChMS].[dbo].[coreAttribute] WITH (NOLOCK)
		WHERE [Entity] = 'Rock.CRM.Person'
	)
	AND [EntityId] = @EntityId
	
END


