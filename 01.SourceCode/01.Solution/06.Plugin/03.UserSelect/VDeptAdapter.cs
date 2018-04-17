



using System;
using System.Collections.Generic;
using Framework.Data;
using Framework.Data.AppBase;

namespace Plugin.UserSelect
{
    /// <summary>
    /// AutoProcess对象的数据访问适配器
    /// </summary>
    public class VDeptAdapter : AppBaseAdapterT<VDeptEntity>
    {
        public List<VDeptEntity> LoaDeptList(int pid)
        {
            string sql = "SELECT * FROM VDept WHERE IsDeleted=0 AND ParentID=@ParentID ORDER BY OrderLevel asc ";
            return ExecuteQuery(sql
                , CreateSqlParameter("@ParentID", System.Data.DbType.Int32, pid)
                );
        }

        internal object LoadPreDeptList(int oid)
        {
            string sql = "SELECT * FROM Dept P WHERE  CHARINDEX(P.DeptPath,(SELECT DeptPath FROM Dept I WHERE I.ID=@id))=1 AND  IsDeleted=0  ORDER BY LEN(DeptPath)-LEN(REPLACE(DeptPath, '/',''))";
            return ExecuteQuery(sql
                , CreateSqlParameter("@id", System.Data.DbType.Int32, oid)
                );
        }
    }
}

