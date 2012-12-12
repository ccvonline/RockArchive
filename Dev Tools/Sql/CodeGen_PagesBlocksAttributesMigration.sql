-- Note:  Right Click | Results To > Results To Text

begin

    -- pages
    SELECT 
        CONCAT('AddPage("',
        [parentPage].[Guid], '","', 
        [p].[Name],  '","',  
        [p].[Description],  '","',
        [p].[Guid], '");') [Pages]
    FROM 
      [Page] p
    join [Page] [parentPage] on [p].[ParentPageId] = [parentPage].[Id]
    where [p].[IsSystem] = 0
    order by [p].[Id]

    -- block types
    select 
        CONCAT('AddBlockType("',
        [Name], '","',  
        [Description], '","',  
        [Path], '","',  
        [Guid], '");"') AddBlockType
    from [BlockType]
    where [IsSystem] = 0
    order by [Id]

    -- blocks
    select 
        CONCAT('AddBlock("',
        [p].[Guid], '","', 
        [bt].[Guid], '","',
        [b].[Name], '","',
        [b].[Zone], '","',
        [b].[Guid], '","',
        [b].[Order], '");"') AddBlock
    from [Block] [b]
    join [Page] [p] on [p].[Id] = [b].[PageId]
    join [BlockType] [bt] on [bt].[Id] = [b].[BlockTypeId]
    where 
      [b].[IsSystem] = 0
    order by [b].[Id]

    -- attributes
    if object_id('tempdb..#attributeIds') is not null
    begin
      drop table #attributeIds
    end

    select * into #attributeIds from (select [Id] from [dbo].[Attribute] where [IsSystem] = 0) [newattribs]

    select
        CONCAT('AddBlockAttribute("', 
        b.Guid, '","',   
        ft.Guid, '","',   
        a.name, '","',  
        a.[Key], '","', 
        a.Category, '","', 
        a.Description, '","', 
        a.[Order], '","', 
        a.DefaultValue, '","', 
        a.Guid, '");"') [AddBlockAttribute]
    from [Attribute] [a]
    join [FieldType] [ft] on [ft].[Id] = [a].[FieldTypeId]
    join [Block] [b] on [b].[Id] = [a].[EntityTypeQualifierValue]
    where [a].[id] in (select [Id] from #attributeIds)

    -- attributes values    
    select 
        CONCAT('AddBlockAttributeValue("',     
        b.Guid, '","', 
        a.Guid, '","', 
        av.Value, '"); //"', 
        b.Name, '":"', 
        a.Name ) [AddBlockAttributeValue]
    from [AttributeValue] [av]
    join Block b on b.Id = av.EntityId
    join Attribute a on a.id = av.AttributeId
    where [av].[AttributeId] in (select [Id] from #attributeIds)

    drop table #attributeIds

end