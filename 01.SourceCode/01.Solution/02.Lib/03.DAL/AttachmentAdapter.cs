
using System;
using System.Collections.Generic;
using Framework.Data;
using Framework.Data.AppBase;
using Lib.Model;
using System.Linq;
using Framework.Web.Security.Authentication;
using Lib.Common;
using Framework.Web.Utility;

namespace Lib.DAL
{



    /// <summary>
    /// TemplateAttachment对象的数据访问适配器
    /// </summary>
    public class AttachmentAdapter : AppBaseAdapterT<Attachment>
    {
        public List<Attachment> GetAttachmentListByTask(string TemplateConfigInstanceID, string businessType)
        {
            string sql = @"SELECT * FROM Attachment WHERE IsDeleted=0 
AND BusinessID IN(
SELECT CAST(ID AS NVARCHAR(40)) FROM[dbo].[TemplateTask] TT WHERE TT.IsDeleted = 0 AND
TT.TemplateConfigInstanceID = @TemplateConfigInstanceID AND TT.ProcessStatus=0 and TT.Status=2
)
 AND BusinessType = @BusinessType";
            return ExecuteQuery(sql,
                CreateSqlParameter("@TemplateConfigInstanceID", System.Data.DbType.String, TemplateConfigInstanceID)
                ,
                CreateSqlParameter("@BusinessType", System.Data.DbType.String, businessType)
                );
        }

        public Attachment GetLastModel(string bussinessType, string bussinessId)
        {
            string sql = "select top 1 * from Attachment";

            sql += " WHERE " + base.NotDeleted;
            sql += " AND BusinessID=@BusinessID AND BusinessType=@BusinessType order by CreatorTime desc";
            return ExecuteQuery(sql
                , CreateSqlParameter("@BusinessID", System.Data.DbType.String, bussinessId)
                , CreateSqlParameter("@BusinessType", System.Data.DbType.String, bussinessType)
                ).FirstOrDefault();

        }

        public Attachment GetModelByCode(string fileCode)
        {
            string sql = ORMapping.GetSelectSql<Attachment>(TSqlBuilder.Instance);

            sql += "WHERE " + base.NotDeleted;
            sql += " AND AttachmentPath=@AttachmentPath";

            return ExecuteQuery(sql, CreateSqlParameter("@AttachmentPath", System.Data.DbType.String, fileCode)).FirstOrDefault();
        }

        public List<Attachment> GetList(string bussinessType, string bussinessId)
        {
            string sql = "select  * from Attachment";

            sql += " WHERE " + base.NotDeleted;
            sql += " AND BusinessID=@BusinessID AND BusinessType=@BusinessType order by CreatorTime desc";
            return ExecuteQuery(sql
                , CreateSqlParameter("@BusinessID", System.Data.DbType.String, bussinessId)
                , CreateSqlParameter("@BusinessType", System.Data.DbType.String, bussinessType)
                );

        }
        public IList<Attachment> GetList()
        {
            string sql = ORMapping.GetSelectSql<Attachment>(TSqlBuilder.Instance);

            sql += "WHERE " + base.NotDeleted;

            return ExecuteQuery(sql);
        }

        public IList<Attachment> GetModel(string businessType, string businessID)
        {
            return ExecuteQuery("SELECT * FROM Attachment WHERE BusinessType=@BusinessType AND BusinessID=@BusinessID AND IsDeleted=0"
                , CreateSqlParameter("@BusinessType", System.Data.DbType.String, businessType)
                , CreateSqlParameter("@BusinessID", System.Data.DbType.String, businessID)
                );
            //return base.Load(p => { p.AppendItem("BusinessType", businessType); p.AppendItem("BusinessID", SafeQuote(businessID)); p.AppendItem("isdeleted", 0); });
        }

        public void AddAndUpdateModel(Attachment attach)
        {
            var atts = base.Load(p => { p.AppendItem("BusinessType", attach.BusinessType); p.AppendItem("BusinessID", SafeQuote(attach.BusinessID)); p.AppendItem("isdeleted", 0); });
            LoginUserInfo userinfo = WebHelper.GetCurrentUser();
            foreach (var item in atts)
            {
                item.IsDeleted = true;
                item.ModifierLoginName = userinfo.LoginName;
                item.ModifierName = userinfo.CNName;
                item.ModifyTime = DateTime.Now;
                base.Update(item);
            }

            base.Insert(attach);
        }
    }
}

