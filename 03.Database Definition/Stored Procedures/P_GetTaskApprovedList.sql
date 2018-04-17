

IF OBJECT_ID('dbo.P_GetTaskApprovedList') IS NOT NULL
    DROP PROC P_GetTaskApprovedList;
GO

CREATE PROC P_GetTaskApprovedList
    (
      @TemplateConfigInstanceID NVARCHAR(36)
    )
AS
    BEGIN

        SELECT  ROW_NUMBER() OVER ( ORDER BY tmp.ID ) AS '序号' ,
                tmp.UnitName '接收人部门',
                tmp.EmployeeUserName '接收人',
                tmp.SubmitTime '填报日期',
                tmp.AuthTime '审批日期',
                dbo.FN_GetApproveList(tmp.LastTaskID) AS '审批流程'
        FROM    ( SELECT    dcu.ID ,
                            dcu.UnitName ,
                            dcu.EmployeeName + '（' + dcu.UserName + '）' AS EmployeeUserName ,
                            CASE WHEN tt.SubmitTime IS NULL THEN '- -'
                                 ELSE CONVERT(VARCHAR(100), tt.SubmitTime, 20)
                            END AS SubmitTime ,
                            CASE WHEN tt.AuthTime IS NULL THEN '- -'
                                 ELSE CONVERT(VARCHAR(100), tt.AuthTime, 20)
                            END AS AuthTime ,
                            tt.ID AS LastTaskID ,
                            dcu.TemplateConfigInstanceID
                  FROM      dbo.DataCollectUser AS dcu
                            LEFT JOIN dbo.TemplateTask tt ON tt.DataCollectUserID = dcu.ID
                                                             AND tt.TemplateConfigInstanceID = dcu.TemplateConfigInstanceID
                                                             AND tt.IsDeleted = 0
                            INNER JOIN dbo.V_Employee u ON u.username = dcu.UserName
                  WHERE     dcu.IsDeleted = 0
                            AND ( EXISTS ( SELECT   DataCollectUserID ,
                                                    TemplateConfigInstanceID ,
                                                    MAX(itt.CreatorTime) AS CreateDate
                                           FROM     dbo.TemplateTask AS itt
                                           WHERE    itt.IsDeleted = 0
                                                    AND itt.ProcessStatus = 0
                                                    AND itt.TemplateConfigInstanceID = tt.TemplateConfigInstanceID
                                                    AND itt.DataCollectUserID = tt.DataCollectUserID
                                                    AND itt.CreatorTime = tt.CreatorTime
                                           GROUP BY DataCollectUserID ,
                                                    TemplateConfigInstanceID )
                                  OR tt.TemplateConfigInstanceID IS NULL
                                )
                ) AS tmp
        WHERE   TemplateConfigInstanceID = @TemplateConfigInstanceID;
    END;
