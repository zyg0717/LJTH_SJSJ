using Framework.Data.AppBase;
using Framework.Web.Security.Authentication;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.Model;

namespace Lib.DAL
{
    public class UserInfoAdapter : CommonAdapter
    {

        public LoginUserInfo GetWDUserInfoByEmployeeID(string userId)
        {
            string sql = @"
SELECT employeeStatus,employeeCode,employeeID,username,employeeName,a.jobName,unitID,unitName,a.orgID,a.orgName,actualUnitID,b.orgName as ActualUnitName
FROM dbo.WD_User a INNER JOIN Wd_Org b on a.actualUnitID=b.orgID
    where employeeID='" + SafeQuote(userId) + "'";
            DataTable dt = ExecuteReturnTable(sql);
            LoginUserInfo model = null;
            if (dt != null && dt.Rows.Count > 0)
            {

                model = new LoginUserInfo();
                model.EmployeeID = !string.IsNullOrEmpty(dt.Rows[0]["employeeID"].ToString()) ? dt.Rows[0]["employeeID"].ToString() : "";
                model.EmployeeCode = !string.IsNullOrEmpty(dt.Rows[0]["employeeCode"].ToString()) ? dt.Rows[0]["employeeCode"].ToString() : "";
                model.LoginName = !string.IsNullOrEmpty(dt.Rows[0]["username"].ToString()) ? dt.Rows[0]["username"].ToString() : "";
                model.CNName = !string.IsNullOrEmpty(dt.Rows[0]["employeeName"].ToString()) ? dt.Rows[0]["employeeName"].ToString() : "";
                model.UnitID = !string.IsNullOrEmpty(dt.Rows[0]["unitID"].ToString()) ? int.Parse(dt.Rows[0]["unitID"].ToString()) : 0;
                model.UnitName = !string.IsNullOrEmpty(dt.Rows[0]["unitName"].ToString()) ? dt.Rows[0]["unitName"].ToString() : "";
                model.OrgID = !string.IsNullOrEmpty(dt.Rows[0]["orgID"].ToString()) ? int.Parse(dt.Rows[0]["orgID"].ToString()) : 0;
                model.OrgName = !string.IsNullOrEmpty(dt.Rows[0]["orgName"].ToString()) ? dt.Rows[0]["orgName"].ToString() : "";
                model.actualUnitID = !string.IsNullOrEmpty(dt.Rows[0]["actualUnitID"].ToString()) ? int.Parse(dt.Rows[0]["actualUnitID"].ToString().Trim()) : 0;
                model.ActualUnitName = !string.IsNullOrEmpty(dt.Rows[0]["ActualUnitName"].ToString()) ? dt.Rows[0]["ActualUnitName"].ToString() : "";
                model.jobName = !string.IsNullOrEmpty(dt.Rows[0]["jobName"].ToString()) ? dt.Rows[0]["jobName"].ToString() : "";
                model.EmployeeStatus = !string.IsNullOrEmpty(dt.Rows[0]["employeeStatus"].ToString()) ? int.Parse(dt.Rows[0]["employeeStatus"].ToString()) : 0;
            }
            return model;

        }
        /// <summary>
        /// 获取用户详细信息根据用户Id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public LoginUserInfo GetWDUserInfoByID(string userId)
        {
            string sql = @"
SELECT employeeStatus,employeeCode,username,employeeName,a.jobName,unitID,unitName,a.orgID,a.orgName,actualUnitID,b.orgName as ActualUnitName
FROM dbo.WD_User a INNER JOIN Wd_Org b on a.actualUnitID=b.orgID
    where employeeCode='" + SafeQuote(userId) + "'";
            DataTable dt = ExecuteReturnTable(sql);
            LoginUserInfo model = null;
            if (dt != null && dt.Rows.Count > 0)
            {

                model = new LoginUserInfo();
                model.EmployeeCode = !string.IsNullOrEmpty(dt.Rows[0]["employeeCode"].ToString()) ? dt.Rows[0]["employeeCode"].ToString() : "";
                model.LoginName = !string.IsNullOrEmpty(dt.Rows[0]["username"].ToString()) ? dt.Rows[0]["username"].ToString() : "";
                model.CNName = !string.IsNullOrEmpty(dt.Rows[0]["employeeName"].ToString()) ? dt.Rows[0]["employeeName"].ToString() : "";
                model.UnitID = !string.IsNullOrEmpty(dt.Rows[0]["unitID"].ToString()) ? int.Parse(dt.Rows[0]["unitID"].ToString()) : 0;
                model.UnitName = !string.IsNullOrEmpty(dt.Rows[0]["unitName"].ToString()) ? dt.Rows[0]["unitName"].ToString() : "";
                model.OrgID = !string.IsNullOrEmpty(dt.Rows[0]["orgID"].ToString()) ? int.Parse(dt.Rows[0]["orgID"].ToString()) : 0;
                model.OrgName = !string.IsNullOrEmpty(dt.Rows[0]["orgName"].ToString()) ? dt.Rows[0]["orgName"].ToString() : "";
                model.actualUnitID = !string.IsNullOrEmpty(dt.Rows[0]["actualUnitID"].ToString()) ? int.Parse(dt.Rows[0]["actualUnitID"].ToString().Trim()) : 0;
                model.ActualUnitName = !string.IsNullOrEmpty(dt.Rows[0]["ActualUnitName"].ToString()) ? dt.Rows[0]["ActualUnitName"].ToString() : "";
                model.jobName = !string.IsNullOrEmpty(dt.Rows[0]["jobName"].ToString()) ? dt.Rows[0]["jobName"].ToString() : "";
                model.EmployeeStatus = !string.IsNullOrEmpty(dt.Rows[0]["employeeStatus"].ToString()) ? int.Parse(dt.Rows[0]["employeeStatus"].ToString()) : 0;
            }
            return model;

        }


        public LoginUserInfo GetWDUserInfoByLoginName(string loginName)
        {
            string sql = @"
SELECT * from V_Employee
    where username='" + SafeQuote(loginName) + "'";
            DataTable dt = ExecuteReturnTable(sql);
            LoginUserInfo model = null;
            if (dt != null && dt.Rows.Count > 0)
            {

                model = new LoginUserInfo();
                model.UnitFullPath = !string.IsNullOrEmpty(dt.Rows[0]["unitFullPath"].ToString()) ? dt.Rows[0]["unitFullPath"].ToString() : "";
                model.EmployeeID = !string.IsNullOrEmpty(dt.Rows[0]["employeeID"].ToString()) ? dt.Rows[0]["employeeID"].ToString() : "";
                model.EmployeeCode = !string.IsNullOrEmpty(dt.Rows[0]["employeeCode"].ToString()) ? dt.Rows[0]["employeeCode"].ToString() : "";
                model.LoginName = !string.IsNullOrEmpty(dt.Rows[0]["username"].ToString()) ? dt.Rows[0]["username"].ToString() : "";
                model.CNName = !string.IsNullOrEmpty(dt.Rows[0]["employeeName"].ToString()) ? dt.Rows[0]["employeeName"].ToString() : "";
                model.UnitID = !string.IsNullOrEmpty(dt.Rows[0]["unitID"].ToString()) ? int.Parse(dt.Rows[0]["unitID"].ToString()) : 0;
                model.UnitName = !string.IsNullOrEmpty(dt.Rows[0]["unitName"].ToString()) ? dt.Rows[0]["unitName"].ToString() : "";
                model.OrgID = !string.IsNullOrEmpty(dt.Rows[0]["orgID"].ToString()) ? int.Parse(dt.Rows[0]["orgID"].ToString()) : 0;
                model.OrgName = !string.IsNullOrEmpty(dt.Rows[0]["orgName"].ToString()) ? dt.Rows[0]["orgName"].ToString() : "";
                model.actualUnitID = !string.IsNullOrEmpty(dt.Rows[0]["actualUnitID"].ToString()) ? int.Parse(dt.Rows[0]["actualUnitID"].ToString().Trim()) : 0;
                model.ActualUnitName = !string.IsNullOrEmpty(dt.Rows[0]["ActualUnitName"].ToString()) ? dt.Rows[0]["ActualUnitName"].ToString() : "";
                model.jobName = !string.IsNullOrEmpty(dt.Rows[0]["jobName"].ToString()) ? dt.Rows[0]["jobName"].ToString() : "";
                model.EmployeeStatus = !string.IsNullOrEmpty(dt.Rows[0]["employeeStatus"].ToString()) ? int.Parse(dt.Rows[0]["employeeStatus"].ToString()) : 0;
            }
            return model;

        }

        public List<LoginUserInfo> GetWDUserInfoByLoginNameList(List<string> loginUserList)
        {
            List<LoginUserInfo> result = new List<LoginUserInfo>();
            if (loginUserList == null || loginUserList.Count == 0)
            {
                return result;
            }
            string sql = string.Format(@"
SELECT * from V_Employee
    where username in ({0})", string.Join(",", loginUserList.Select(x => string.Format("'{0}'", SafeQuote(x))).ToList()));
            DataTable dt = ExecuteReturnTable(sql);
            LoginUserInfo model = null;
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    model = new LoginUserInfo();
                    model.UnitFullPath = !string.IsNullOrEmpty(row["unitFullPath"].ToString()) ? row["unitFullPath"].ToString() : "";
                    model.EmployeeID = !string.IsNullOrEmpty(row["employeeID"].ToString()) ? row["employeeID"].ToString() : "";
                    model.EmployeeCode = !string.IsNullOrEmpty(row["employeeCode"].ToString()) ? row["employeeCode"].ToString() : "";
                    model.LoginName = !string.IsNullOrEmpty(row["username"].ToString()) ? row["username"].ToString() : "";
                    model.CNName = !string.IsNullOrEmpty(row["employeeName"].ToString()) ? row["employeeName"].ToString() : "";
                    model.UnitID = !string.IsNullOrEmpty(row["unitID"].ToString()) ? int.Parse(row["unitID"].ToString()) : 0;
                    model.UnitName = !string.IsNullOrEmpty(row["unitName"].ToString()) ? row["unitName"].ToString() : "";
                    model.OrgID = !string.IsNullOrEmpty(row["orgID"].ToString()) ? int.Parse(row["orgID"].ToString()) : 0;
                    model.OrgName = !string.IsNullOrEmpty(row["orgName"].ToString()) ? row["orgName"].ToString() : "";
                    model.actualUnitID = !string.IsNullOrEmpty(row["actualUnitID"].ToString()) ? int.Parse(row["actualUnitID"].ToString().Trim()) : 0;
                    model.ActualUnitName = !string.IsNullOrEmpty(row["ActualUnitName"].ToString()) ? row["ActualUnitName"].ToString() : "";
                    model.jobName = !string.IsNullOrEmpty(row["jobName"].ToString()) ? row["jobName"].ToString() : "";
                    model.EmployeeStatus = !string.IsNullOrEmpty(row["employeeStatus"].ToString()) ? int.Parse(row["employeeStatus"].ToString()) : 0;
                    result.Add(model);
                }

            }
            return result;
        }

    }
}
