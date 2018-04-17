

--任务类型1
IF NOT EXISTS ( SELECT  1
                FROM    sysobjects T1
                        INNER JOIN syscolumns T2 ON T1.id = T2.id
                WHERE   T1.name = 'TemplateConfigInstance'
                        AND T2.name = 'TaskTemplateType' )
    BEGIN
        ALTER TABLE TemplateConfigInstance
        ADD  TaskTemplateType INT;
    END;

	