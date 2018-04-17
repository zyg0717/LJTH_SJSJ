
using System;
using System.Collections.Generic;
using Framework.Data;
using Framework.Data.AppBase;
using Lib.Model;
using System.Linq;
using System.Data.SqlClient;
using System.Data;
using Lib.DAL;
using Lib.Common;
using Framework.Web.Security.Authentication;

namespace Lib.DAL
{



    /// <summary>
    /// Template对象的数据访问适配器
    /// </summary>
    public class TemplateAdapter : AppBaseAdapterT<Template>
    {

        public IList<Template> GetList()
        {
            string sql = ORMapping.GetSelectSql<Template>(TSqlBuilder.Instance);

            sql += "WHERE " + base.NotDeleted;

            return ExecuteQuery(sql);
        }

        public IList<Template> GetListByLoginName(string loginName)
        {
            //string sql = ORMapping.GetSelectSql<Template>(TSqlBuilder.Instance);

            //sql += "WHERE loginName=@loginName and" + base.NotDeleted;
            return base.Load(p => { p.AppendItem("CreatorLoginName", loginName); p.AppendItem("isdeleted", "0"); p.AppendItem("IsPrivate", true); p.AppendItem("Status", 0); });
            //return ExecuteQuery(sql, new SqlParameter() { ParameterName = "@loginName", datat DbType.String, loginName });
        }

        

        public int UpdateRalationByID(string templateID)
        {
            string SQL = "DELETE FROM dbo.Template WHERE ID=@ID ";
            SQL += "DELETE FROM dbo.TemplateSheet  WHERE TemplateID=@ID ";
            SQL += "DELETE FROM dbo.TemplateConfig  WHERE TemplateID=@ID ";
            SQL += "DELETE FROM dbo.TemplateConfigSelect  WHERE TemplateID=@ID ";


            return ExecuteSql(SQL, CreateSqlParameter("@ID", DbType.String, templateID));
        }

        public List<Template> SearchTemplate(string name, string loginName, bool isWithIsImport = true)
        {
            string SQL = ORMapping.GetSelectSql<Template>(TSqlBuilder.Instance);
            SQL += string.Format("Where {0} ", NotDeleted);
            if (!string.IsNullOrEmpty(name))
            {
                SQL += string.Format(" and TemplateName like '%{0}%'", SafeQuote(name));
            }

            if (!string.IsNullOrEmpty(loginName))
            {
                SQL += string.Format(" and CreatorLoginName='{0}'", SafeQuote(loginName));
            }
            if (!isWithIsImport)
            {
                SQL += string.Format(" and IsImport=0");
            }
            SQL += " AND Status=0";

            return ExecuteQuery(SQL);
        }
        public List<Template> SearchTemplateSelf(string name, string loginName)
        {
            string SQL = @"select distinct t.*  from Template t join TemplateConfigInstance tm on t.ID=tm.TemplateID join TemplateTask tk on tm.ID=tk.TemplateConfigInstanceID 
 where t.IsDeleted=0 and tm.IsDeleted=0 and tk.IsDeleted=0";
            if (!string.IsNullOrEmpty(name))
            {
                SQL += string.Format(" and t.TemplateName like '%{0}%'", SafeQuote(name));
            }

            if (!string.IsNullOrEmpty(loginName))
            {
                SQL += string.Format(" and t.CreatorLoginName='{0}'", SafeQuote(loginName));
            }
            SQL += " AND Status=0";
            return ExecuteQuery(SQL);
        }


        public List<Template> GetMyModelList()
        {
            string sql = @"with cte as
                (
                    select orgID,parentUnitID from wd_org
	                
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
                select * from Template where IsDeleted=0 and IsPrivate=1 and CreatorLoginName=@loginName and Status=0

                drop table #torg";
            var user = Framework.Web.Utility.WebHelper.GetCurrentUser();
            //where orgID=@orgid
            //return ExecuteQuery(sql, CreateSqlParameter("@orgid", DbType.Int32, user.UnitID), CreateSqlParameter("@loginName", DbType.String, user.LoginName));
            return ExecuteQuery(sql, CreateSqlParameter("@loginName", DbType.String, user.LoginName));
        }

        public bool CheckExist(string loginName, string templateName)
        {
            string sql = "SELECT COUNT(1) FROM dbo.Template WHERE IsDeleted=0 AND Status=0 AND TemplateName=@TemplateName AND CreatorLoginName=@CreatorLoginName";

            return Convert.ToInt32(DbHelper.RunSqlReturnScalar(sql, this.ConnectionName
                 , CreateSqlParameter("@TemplateName", DbType.String, templateName)
                 , CreateSqlParameter("@CreatorLoginName", DbType.String, loginName)

                 )) > 0;
        }
    }
}

