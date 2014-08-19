/* List DefinedTypeValues*/
SELECT
    t.Category, 
	t.Id as [Type ID],
    t.Name as [DefinedType.Name],
	v.Id as [Value ID],
    v.Value as Value, 
    v.Description,
    cast( 
        (select 
            a.Name
            ,av.Value
            ,a.Guid [Attribute.Guid] 
            ,av.Guid [AttributeValue.Guid] 
        from 
            AttributeValue av 
        join 
            Attribute a on av.AttributeId = a.Id 
        join 
            EntityType et on a.EntityTypeId = et.Id
        where 
            et.Name = 'Rock.Model.DefinedValue' 
        and 
            av.EntityId = v.Id
        FOR XML PATH ('Attribute'), root ('root' ) ) as XML) [AttributeValues],
    t.Guid [DefinedType.Guid],
    v.Guid [DefinedValue.Guid]
FROM            
    DefinedValue AS v 
INNER JOIN
    DefinedType AS t ON t.Id = v.DefinedTypeId
ORDER BY t.Category, [DefinedType.Name], Value