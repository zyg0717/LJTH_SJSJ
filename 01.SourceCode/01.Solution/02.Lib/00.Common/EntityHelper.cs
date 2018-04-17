using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Common
{
    /// <summary>
    /// 数据实体转换类
    /// </summary>
    public static partial class EntityHelper
    {
        /// <summary>
        /// 根据数据表生成相应的实体对象列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static T ToList<T>(this DataTable dt)
        {
            List<T> list = GetEntityListByDT<T>(dt);
            if (list != null && list.Count > 0) return list[0];
            return default(T);
        }

        /// <summary>
        /// 根据数据表生成相应的实体对象列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static T ToList<T>(this DataSet ds)
        {
            return GetEntityListByDT<T>(ds);
        }

        /// <summary>
        /// 根据数据表生成相应的实体对象列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static List<T> ToLists<T>(this DataSet ds)
        {
            if (ds == null || ds.Tables.Count <= 0) return null;
            return GetEntityListByDT<T>(ds.Tables[0]);
        }

        /// <summary>
        /// 根据数据表生成相应的实体对象列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<T> ToLists<T>(this DataTable dt)
        {
            return GetEntityListByDT<T>(dt);
        }

        /// <summary>
        /// 根据数据表生成相应的实体对象列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static List<T> ToLists<T>(this DataTable dt, string fieldName)
        {
            return GetEntityListByDT<T>(dt, fieldName);
        }

        /// <summary>
        /// 根据数据表生成相应的实体对象列表
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="srcDT">数据</param>
        /// <param name="relation">数据库表列名与对象属性名对应关系；如果列名与实体对象属性名相同，该参数可为空</param>
        /// <returns>对象列表</returns>
        public static List<T> GetEntityListByDT<T>(DataTable srcDT)
        {
            List<T> list = new List<T>();
            T destObj = default(T);

            if (srcDT != null && srcDT.Rows.Count > 0)
            {
                list = new List<T>();
                foreach (DataRow row in srcDT.Rows)
                {
                    destObj = GetEntityListByDT<T>(row);
                    list.Add(destObj);
                }
            }

            return list;
        }

        /// <summary>
        /// 根据数据表生成相应的实体对象列表
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="srcDT">数据</param>
        /// <param name="fieldName">数据库表列名与对象属性名对应关系；如果列名与实体对象属性名相同，该参数可为空</param>
        /// <returns>对象列表</returns>
        public static List<T> GetEntityListByDT<T>(DataTable srcDT, string fieldName)
        {
            List<T> list = null;
            T destObj = default(T);
            if (srcDT != null && srcDT.Rows.Count > 0)
            {
                list = new List<T>();
                foreach (DataRow row in srcDT.Rows)
                {
                    destObj = GetEntityListByDT<T>(row, fieldName);
                    list.Add(destObj);
                }
            }
            return list;
        }

        /// <summary>
        ///  将SqlDataReader转换成数据实体 add by trenhui
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="row"></param>
        /// <param name="relation"></param>
        /// <returns></returns>
        public static T GetEntityListByDT<T>(IDataReader dr)
        {
            Type type = typeof(T);
            T destObj = Activator.CreateInstance<T>();
            foreach (PropertyInfo prop in type.GetProperties())
            {
                try
                {
                    if (dr[prop.Name] != null && dr[prop.Name] != DBNull.Value)
                    {
                        SetPropertyValue(prop, destObj, dr[prop.Name]);
                    }
                }
                catch (Exception ex) { }
            }
            return destObj;
        }

        /// <summary>
        ///  将数据行转换成数据实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="row"></param>
        /// <returns></returns>
        public static T GetEntityListByDT<T>(DataRow row)
        {
            Type type = typeof(T);
            T destObj = Activator.CreateInstance<T>();
            foreach (PropertyInfo prop in type.GetProperties())
            {
                if (row.Table.Columns.Contains(prop.Name) &&
                    row[prop.Name] != DBNull.Value)
                {
                    SetPropertyValue(prop, destObj, row[prop.Name]);
                }
            }
            return destObj;
        }

        /// <summary>
        ///  将数据行转换成数据实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="row"></param>
        /// <param name="relation"></param>
        /// <returns></returns>
        public static T GetEntityListByDT_Back<T>(DataRow row, Hashtable relation)
        {
            Type type = typeof(T);
            T destObj = Activator.CreateInstance<T>();
            PropertyInfo temp = null;
            foreach (PropertyInfo prop in type.GetProperties())
            {
                if (row.Table.Columns.Contains(prop.Name) &&
                    row[prop.Name] != DBNull.Value)
                {
                    SetPropertyValue(prop, destObj, row[prop.Name]);
                }
            }

            if (relation != null && relation.Count > 0)
            {
                foreach (string name in relation.Keys)
                {
                    temp = type.GetProperty(relation[name].ToString());
                    if (temp != null &&
                        row[name] != DBNull.Value)
                    {
                        SetPropertyValue(temp, destObj, row[name]);
                    }
                }
            }

            return destObj;
        }

        /// <summary>
        ///  将数据行转换成数据实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="row"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static T GetEntityListByDT<T>(DataRow row, string fieldName)
        {
            Type type = typeof(T);
            T destObj = Activator.CreateInstance<T>();
            foreach (PropertyInfo prop in type.GetProperties())
            {
                string columnName = string.Empty;
                switch (prop.Name)
                {
                    case "ID":
                        columnName = prop.Name;
                        break;
                    default:
                        columnName = string.Format("{0}{1}", fieldName, prop.Name);
                        break;
                }
                if (row.Table.Columns.Contains(columnName) &&
                    row[columnName] != DBNull.Value)
                {
                    SetPropertyValue(prop, destObj, row[columnName]);
                }
            }
            return destObj;
        }

        /// <summary>
        ///  将数据行转换成数据实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static T GetEntityListByDT<T>(DataSet ds)
        {
            Type type = typeof(T);
            T destObj = Activator.CreateInstance<T>();

            try
            {
                DataTableCollection dts = ds.Tables;
                if (dts.Count > 0 && dts[0].Rows.Count > 0)
                {
                    DataRow row = ds.Tables[0].Rows[0];
                    foreach (PropertyInfo prop in type.GetProperties())
                    {
                        if (row.Table.Columns.Contains(prop.Name) &&
                            row[prop.Name] != DBNull.Value)
                        {
                            SetPropertyValue(prop, destObj, row[prop.Name]);
                        }
                    }
                }
            }
            catch { }

            return destObj;
        }

        /// <summary>
        /// 为对象的属性赋值
        /// </summary>
        /// <param name="prop">属性</param>
        /// <param name="destObj">目标对象</param>
        /// <param name="value">源值</param>
        private static void SetPropertyValue(PropertyInfo prop, object destObj, object value)
        {
            object temp = ChangeType(prop.PropertyType, value);
            prop.SetValue(destObj, temp, null);
        }

        /// <summary>
        /// 用于类型数据的赋值
        /// </summary>
        /// <param name="type">目标类型</param>
        /// <param name="value">原值</param>
        /// <returns></returns>
        private static object ChangeType(Type type, object value)
        {
            int temp = 0;
            if ((value == null) && type.IsGenericType)
            {
                return Activator.CreateInstance(type);
            }
            if (value == null)
            {
                return null;
            }
            if (type == value.GetType())
            {
                return value;
            }
            if (type.IsEnum)
            {
                if (value is string)
                {
                    return Enum.Parse(type, value as string);
                }
                return Enum.ToObject(type, value);
            }
            //if (type == typeof(bool) && typeof(int).IsInstanceOfType(value))
            if (type == typeof(bool))
            {
                try
                {
                    return Convert.ToBoolean(value);
                }
                catch { }
                try
                {
                    return int.Parse(value.ToString()) != 0;
                }
                catch { }
            }
            if (!type.IsInterface && type.IsGenericType)
            {
                Type type1 = type.GetGenericArguments()[0];
                object obj1 = ChangeType(type1, value);
                return Activator.CreateInstance(type, new object[] { obj1 });
            }
            if ((value is string) && (type == typeof(Guid)))
            {
                return new Guid(value as string);
            }
            if ((value is string) && (type == typeof(Version)))
            {
                return new Version(value as string);
            }
            if (!(value is IConvertible))
            {
                return value;
            }
            return Convert.ChangeType(value, type);
        }

        public static List<T> DataTableToList<T>(DataTable dt) where T : new()
        {
            var list = new List<T>();
            if (dt == null) return list;
            var len = dt.Rows.Count;

            for (var i = 0; i < len; i++)
            {
                var info = new T();
                foreach (DataColumn dc in dt.Rows[i].Table.Columns)
                {
                    var field = dc.ColumnName;
                    var value = dt.Rows[i][field].ToString();
                    if (string.IsNullOrEmpty(value)) continue;
                    //if (IsDate(value))
                    //{
                    //    value = DateTime.Parse(value).ToString();
                    //}

                    var p = info.GetType().GetProperty(field);

                    try
                    {
                        if (p.PropertyType == typeof(string))
                        {
                            p.SetValue(info, value, null);
                        }
                        else if (p.PropertyType == typeof(int))
                        {
                            p.SetValue(info, int.Parse(value), null);
                        }
                        else if (p.PropertyType == typeof(bool))
                        {
                            p.SetValue(info, bool.Parse(value), null);
                        }
                        else if (p.PropertyType == typeof(DateTime))
                        {
                            p.SetValue(info, DateTime.Parse(value), null);
                        }
                        else if (p.PropertyType == typeof(float))
                        {
                            p.SetValue(info, float.Parse(value), null);
                        }
                        else if (p.PropertyType == typeof(double))
                        {
                            p.SetValue(info, double.Parse(value), null);
                        }
                        else
                        {
                            p.SetValue(info, value, null);
                        }
                    }
                    catch (Exception)
                    {
                        //p.SetValue(info, ex.Message, null);   
                    }
                }
                list.Add(info);
            }
            dt.Dispose(); dt = null;
            return list;
        }

        public static string DataTableToJson(DataTable dt)
        {
            StringBuilder jsonBuilder = new StringBuilder();
            jsonBuilder.Append("[");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                jsonBuilder.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    jsonBuilder.Append("\"");
                    jsonBuilder.Append(dt.Columns[j].ColumnName);
                    jsonBuilder.Append("\":\"");
                    jsonBuilder.Append(dt.Rows[i][j].ToString());
                    jsonBuilder.Append("\",");
                }
                jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                jsonBuilder.Append("},");
            }
            jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
            jsonBuilder.Append("]");
            return jsonBuilder.ToString();
        }

        /// <summary> 
        /// DataTable转为List 
        /// </summary> 
        /// <param name="dt">DataTable</param> 
        /// <returns>List</returns> 
        public static List<Dictionary<string, object>> ToList(DataTable dt)
        {
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            foreach (DataRow dr in dt.Rows)
            {
                Dictionary<string, object> result = new Dictionary<string, object>();
                foreach (DataColumn dc in dt.Columns)
                {
                    result.Add(dc.ColumnName, dr[dc]);
                }
                list.Add(result);
            }

            return list;
        }
    }
}
