
--�Ƿ�ʽ
IF NOT EXISTS ( SELECT  1
                FROM    sysobjects T1
                        INNER JOIN syscolumns T2 ON T1.id = T2.id
                WHERE   T1.name = 'TemplateConfig'
                        AND T2.name = 'IsFormula' )
    BEGIN
        ALTER TABLE TemplateConfig
        ADD IsFormula SMALLINT;
    END;
   
   --ʾ����ʽ
IF NOT EXISTS ( SELECT  1
                FROM    sysobjects T1
                        INNER JOIN syscolumns T2 ON T1.id = T2.id
                WHERE   T1.name = 'TemplateConfig'
                        AND T2.name = 'TempFormula' )
    BEGIN
        ALTER TABLE TemplateConfig
        ADD TempFormula NVARCHAR(4000);
    END;
	--ͨ�ù�ʽ
IF NOT EXISTS ( SELECT  1
                FROM    sysobjects T1
                        INNER JOIN syscolumns T2 ON T1.id = T2.id
                WHERE   T1.name = 'TemplateConfig'
                        AND T2.name = 'CellFormula' )
    BEGIN
        ALTER TABLE TemplateConfig
        ADD CellFormula  NVARCHAR(4000);
    END;