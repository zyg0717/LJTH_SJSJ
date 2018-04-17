
IF OBJECT_ID('dbo.FN_GetApproveList') IS NOT NULL
    DROP FUNCTION FN_GetApproveList;
GO



CREATE FUNCTION FN_GetApproveList ( @AppBizID NVARCHAR(36) )
RETURNS NVARCHAR(MAX)
AS
    BEGIN
        DECLARE @Result NVARCHAR(max)= '';
        DECLARE @NaxtNodeId NVARCHAR(36)= '';
        DECLARE @NodeName NVARCHAR(200)= '';
        DECLARE @UserName NVARCHAR(36)= '';
        DECLARE @CurrentStatus INT = 0;
        DECLARE @NodeId NVARCHAR(36)= '';
        DECLARE @Flag INT = 1; --���ڿ��ƶԺ��Ƿ���ʾ


        SELECT  @NaxtNodeId = NextNodeID ,
                @NodeName = NodeName ,
                @UserName = UserName ,
                @CurrentStatus = Status
        FROM    dbo.SN_WWF_Node
        WHERE   AppBizID = @AppBizID
                AND NodeType = 0;
        IF @CurrentStatus = 1
            SET @Flag = 0;

        IF ( @NodeName <> '' )
            BEGIN	
                IF @CurrentStatus = 1
                    SET @Result = @NodeName + '��' + @UserName + '��';
                ELSE
                    SET @Result = @NodeName + '[' + @UserName + ']';
            END;
        IF @Flag = 1
            SET @Result += ' �� ';

        WHILE @NaxtNodeId <> ''
            BEGIN
                SELECT  @NodeId = NodeId ,
                        @NaxtNodeId = NextNodeID ,
                        @NodeName = NodeName ,
                        @UserName = LTRIM(RTRIM(ISNULL(UserName, ''))) ,
                        @CurrentStatus = Status
                FROM    dbo.SN_WWF_Node
                WHERE   AppBizID = @AppBizID
                        AND NodeID = @NaxtNodeId;
                IF @CurrentStatus = 1
                    SET @Flag = 0;
                IF @UserName = ''
                    BEGIN
						--�ϲ��ڵ㶼�������ʱ�����������潫������ʾ�Ժ�
                        IF ( SELECT COUNT(1)
                             FROM   dbo.SN_WWF_Node
                             WHERE  status <> 2
                                    AND parentnodeid = @NodeId
                           ) > 0
                            SET @UserName = ISNULL(STUFF(( SELECT
                                                              '��' + username
                                                              + CASE
                                                              WHEN status = 2
                                                              THEN ' ��'
                                                              ELSE ''
                                                              END
                                                           FROM
                                                              dbo.SN_WWF_Node
                                                           WHERE
                                                              parentnodeid = @NodeId
                                                         FOR
                                                           XML
                                                              PATH('')
                                                         ), 1, 1, ''), '');
                        ELSE
                            SET @UserName = ISNULL(STUFF(( SELECT
                                                              '��' + username
                                                           FROM
                                                              dbo.SN_WWF_Node
                                                           WHERE
                                                              parentnodeid = @NodeId
                                                         FOR
                                                           XML
                                                              PATH('')
                                                         ), 1, 1, ''), '');
                    END;
                    
		
                IF @CurrentStatus = 1
                    SET @Result += '-> ' + @NodeName + '��' + @UserName + '��';
                ELSE
                    SET @Result += '-> ' + @NodeName + '[' + @UserName + ']';
                IF @Flag = 1
                    AND @NodeName <> ''
                    SET @Result += ' �� ';
            END;
        RETURN @Result;
    END;