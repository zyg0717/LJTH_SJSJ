using Framework.Data;
using Framework.Data.AppBase;
using Framework.Web.Security.Authentication;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.Common;
using Lib.ViewModel;
using Framework.Web.Utility;

namespace Lib.DAL
{
    public class TempOrgAdapter : AppBaseCompositionAdapterT<TempTaskOrgView>
    {
        public List<TempTaskOrgView> GetList()
        {
            string sql = @"with cte as
                (
                    select orgID,parentUnitID from wd_org
	                where orgID=@orgid
                    union all
                    select d.orgID,d.parentUnitID from cte c inner join wd_org d
                    on c.parentUnitID = d.orgID
                )
                select * INTO #torg  from cte 

                select t.[ID],t.[TemplateName],t.[TemplateNo],t.[OrgID],t.[OrgName],t.[UnitID],t.[UnitName],t.[ActualUnitID],t.[ActualUnitName],t.[IsImport],t.[IsPrivate],t.[TemplatePath],
                t.[Range],t.[Status],t.[CreatorLoginName],t.[CreatorName],t.[CreatorTime],t.[ModifierLoginName],t.[ModifierName],t.[ModifyTime],t.[IsDeleted] from (
                select  
                     a.*,b.tRange 
                 from  
                     (select *,Range1=convert(xml,' <root> <v>'+replace(Range,',',' </v> <v>')+' </v> </root>') from Template where IsDeleted=0 and IsPrivate=0)a 
                 outer apply 
                     (select tRange=C.v.value('.','nvarchar(100)') from a.Range1.nodes('/root/v')C(v))b ) t
                where t.tRange in (
                select orgid from #torg
                )
                union 
                select * from Template where IsDeleted=0 and IsPrivate=1 and CreatorLoginName=@loginName

                drop table #torg";
            var user = WebHelper.GetCurrentUser();;
            return ExecuteQuery(sql, CreateSqlParameter("@orgid", DbType.Int32, user.UnitID), CreateSqlParameter("@loginName", DbType.String, user.LoginName));
        }
    }
}
