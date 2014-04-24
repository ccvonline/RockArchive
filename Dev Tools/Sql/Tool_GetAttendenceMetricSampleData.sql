/* Creates INSERT Sql from data from another database for metrics */
SELECT concat(
  'INSERT INTO [MetricValue] ([MetricId], [MetricValueType], [XValue], [YValue], [Order], [Note], [MetricValueDateTime], [Guid])
    VALUES (@metricId, 1, ''', metric_x_value, ''',',  metric_value, ',', '0', ',''', replace(note, '''', ''''''), ''',''', collection_date, ''',''', newid(), ''')')
  FROM [Arena].[dbo].[mtrc_metric_item]
where metric_id=158

