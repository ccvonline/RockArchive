/* List each Entity's Attributes' Values and which Entity ID it is associated with order by most recently added values first */
SELECT 
       [v].[Value] [AttributeValue.Value],
       [a].[Key] [AttributeKey],       
       [ft].[Name] [FieldType.Name],       
       [p].[Name] [Page.Name],
       [b].[Name] [Block.Name],
       [aud].[DateTime] [Audit/LastUpdated DateTime],
       isnull([e].[Name], 'Global') [EntityName],
       [a].EntityTypeId,
       [a].[EntityTypeQualifierColumn],
       [a].[EntityTypeQualifierValue],
       isnull(cast([v].[EntityId] as nvarchar(100)), 'n/a') [Entity's Id Value],
       [v].[Guid] [AttributeValue.Guid],
       [a].[Guid] [Attribute.Guid],
       [ft].[Guid] [FieldType.Guid],
       case e.Name
         when 'Rock.Model.Block' then b.Guid
         else null
       end 'Block.Guid',
       case e.Name
         when 'Rock.Model.Block' then b.Name
         else null
       end 'Block.Name'
  FROM [AttributeValue] [v]
  join [Attribute] [a] on [a].[Id] = [v].[AttributeId]
  left join [EntityType] [e] on [e].[Id] = [a].[EntityTypeId]
  join [FieldType] [ft] on [ft].[Id] = [a].[FieldTypeId]
  left join [Block] [b] on b.Id = [v].[EntityId]
  left outer join [Audit] [aud] on [aud].[EntityTypeId] = 
   (select [Id] from [EntityType] where [Name] = 'Rock.Model.AttributeValue') 
    and [aud].[EntityId] = [v].[Id]
  left outer join [Page] [p] on [b].[PageId] = [p].[Id]
order by [v].[Id] desc /* To sort by recently Added either by Migration/SQL or within Rock*/
--order by isnull([aud].[DateTime], convert(DATETIME, '1901-01-01')) desc /* To sort by recently updated from within Rock*/

