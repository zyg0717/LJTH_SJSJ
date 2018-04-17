



--任务类型1
IF NOT EXISTS ( SELECT  1
                FROM    sysobjects T1
                        INNER JOIN syscolumns T2 ON T1.id = T2.id
                WHERE   T1.name = 'DataCollectUser'
                        AND T2.name = 'TaskTemplateType' )
    BEGIN
        ALTER TABLE DataCollectUser
        ADD  TaskTemplateType INT;
    END;

	
--更新区域
IF NOT EXISTS ( SELECT  1
                FROM    sysobjects T1
                        INNER JOIN syscolumns T2 ON T1.id = T2.id
                WHERE   T1.name = 'DataCollectUser'
                        AND T2.name = 'UpdateArea' )
    BEGIN
        ALTER TABLE DataCollectUser
        ADD  UpdateArea NVARCHAR(200);
    END;

	
--区域值
IF NOT EXISTS ( SELECT  1
                FROM    sysobjects T1
                        INNER JOIN syscolumns T2 ON T1.id = T2.id
                WHERE   T1.name = 'DataCollectUser'
                        AND T2.name = 'AreaValue' )
    BEGIN
        ALTER TABLE DataCollectUser
        ADD  AreaValue NVARCHAR(500);
    END;