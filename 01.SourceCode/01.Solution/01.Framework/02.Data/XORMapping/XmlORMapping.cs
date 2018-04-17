using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Framework.Core;
using System.Reflection;
using System.Data;
using Framework.Data.Properties;

namespace Framework.Data.XORMapping
{
    public static class XmlORMapping
    {
        public static DataModel GetMappingInfo<T>()
        {
            return InnerGetMappingInfo(typeof(T));
        }

        public static WhereSqlClauseBuilder GetWhereSqlClauseBuilderByPrimaryKey<T>(T graph, params string[] ignorProperties)
        {
            DataModel mapping = InnerGetMappingInfo(typeof(T));
            return GetWhereSqlClauseBuilderByPrimaryKey(graph, mapping, ignorProperties);
        }

        #region Select

        private delegate void DoSelectClauseBuilder(SelectSqlClauseBuilder builder, DataProperty property);

        public static string GetSelectByPrimarySql<T>(T graph, ISqlBuilder builder, params string[] ignorProperties)
        {
            DataModel mapping = InnerGetMappingInfo(graph);
            WhereSqlClauseBuilder whereBuilder = GetWhereSqlClauseBuilderByPrimaryKey(graph, ignorProperties);

            return string.Format("{0} WHERE {1}", GetSelectSql(mapping, builder, ignorProperties), whereBuilder.ToSqlString(builder));
        }

        public static string GetSelectSql(System.Type type, ISqlBuilder builder, params string[] ignorProperties)
        {
            DataModel mapping = InnerGetMappingInfo(type);

            return GetSelectSql(mapping, builder, ignorProperties);
        }

        private static string GetSelectSql(DataModel mapping, ISqlBuilder builder, string[] ignorProperties)
        {
            ExceptionHelper.FalseThrow<ArgumentNullException>(mapping != null, "mapping");
            ExceptionHelper.FalseThrow<ArgumentNullException>(builder != null, "builder");

            SelectSqlClauseBuilder selectBuilder = GetSelectSqlClauseBuilder(mapping, ignorProperties);

            return string.Format("SELECT {0} FROM {1} ", selectBuilder.ToSqlString(builder), mapping.TableName);
        }

        private static SelectSqlClauseBuilder GetSelectSqlClauseBuilder(DataModel mapping, string[] ignorProperties)
        {
            ExceptionHelper.FalseThrow<ArgumentNullException>(mapping != null, "mapping");

            SelectSqlClauseBuilder builder = new SelectSqlClauseBuilder();
            FillSqlClauseBuilder(builder, mapping, ClauseBindingFlags.Select, new DoSelectClauseBuilder(DoSelectSqlClauseBuilder), ignorProperties);

            return builder;
        }

        private static void FillSqlClauseBuilder(SelectSqlClauseBuilder builder, DataModel mapping,
            ClauseBindingFlags bindingFlags, DoSelectClauseBuilder builderDelegate, params string[] ignorProperties)
        {
            mapping.Properties.ForEach(property =>
            {
                if (!ignorProperties.Contains(property.Name))
                {
                    if (property.OutputMapping)
                        builderDelegate(builder, property);
                }
            });
        }

        private static void DoSelectSqlClauseBuilder(SelectSqlClauseBuilder builder, DataProperty property)
        {
            builder.AppendItem(property.FieldName);
        }

        #endregion

        #region Insert & Update & Delete

        #region Insert

        public static string GetInsertSql<T>(T graph, ISqlBuilder builder, params string[] ignorProperties)
        {
            DataModel mapping = InnerGetMappingInfo(graph);

            return GetInsertSql<T>(graph, mapping, builder, ignorProperties);
        }

        private static string GetInsertSql<T>(T graph, DataModel mapping, ISqlBuilder builder, string[] ignorProperties)
        {
            ExceptionHelper.FalseThrow<ArgumentNullException>(graph != null, "graph");
            ExceptionHelper.FalseThrow<ArgumentNullException>(mapping != null, "mapping");
            ExceptionHelper.FalseThrow<ArgumentNullException>(builder != null, "builder");

            InsertSqlClauseBuilder insertBuilder = GetInsertSqlClauseBuilder(graph, mapping, ignorProperties);

            return string.Format("INSERT INTO {0} {1}", mapping.TableName, insertBuilder.ToSqlString(builder));
        }

        private static InsertSqlClauseBuilder GetInsertSqlClauseBuilder<T>(T graph, DataModel mapping, string[] ignorProperties)
        {
            ExceptionHelper.FalseThrow<ArgumentNullException>(graph != null, "graph");
            ExceptionHelper.FalseThrow<ArgumentNullException>(mapping != null, "mapping");

            InsertSqlClauseBuilder builder = new InsertSqlClauseBuilder();

            FillSqlClauseBuilder(builder, graph, mapping, ClauseBindingFlags.Insert,
                new DoSqlClauseBuilder<T>(DoInsertUpdateSqlClauseBuilder<T>), ignorProperties);

            return builder;
        }

        #endregion

        #region Update

        public static string GetUpdateSql<T>(T graph, ISqlBuilder builder, params string[] ignorProperties)
        {
            DataModel mapping = InnerGetMappingInfo(graph);

            return GetUpdateSql<T>(graph, mapping, builder, ignorProperties);
        }

        private static string GetUpdateSql<T>(T graph, DataModel mapping, ISqlBuilder builder, string[] ignorProperties)
        {
            ExceptionHelper.FalseThrow<ArgumentNullException>(graph != null, "graph");
            ExceptionHelper.FalseThrow<ArgumentNullException>(mapping != null, "mapping");
            ExceptionHelper.FalseThrow<ArgumentNullException>(builder != null, "builder");

            UpdateSqlClauseBuilder updateBuilder = GetUpdateSqlClauseBuilder(graph, mapping, ignorProperties);
            WhereSqlClauseBuilder whereBuilder = GetWhereSqlClauseBuilderByPrimaryKey(graph, mapping);

            return string.Format("UPDATE {0} SET {1} WHERE {2}",
                mapping.TableName,
                updateBuilder.ToSqlString(builder),
                whereBuilder.ToSqlString(builder));
        }

        private static WhereSqlClauseBuilder GetWhereSqlClauseBuilderByPrimaryKey<T>(T graph, DataModel mapping, params string[] ignorProperties)
        {
            ExceptionHelper.FalseThrow<ArgumentNullException>(graph != null, "graph");
            ExceptionHelper.FalseThrow<ArgumentNullException>(mapping != null, "mapping");

            WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();

            FillSqlClauseBuilder(builder, graph, mapping, ClauseBindingFlags.Where,
                new DoSqlClauseBuilder<T>(DoWhereSqlClauseBuilderByPrimaryKey<T>), ignorProperties);

            return builder;
        }

        private static UpdateSqlClauseBuilder GetUpdateSqlClauseBuilder<T>(T graph, DataModel mapping, string[] ignorProperties)
        {
            ExceptionHelper.FalseThrow<ArgumentNullException>(graph != null, "graph");
            ExceptionHelper.FalseThrow<ArgumentNullException>(mapping != null, "mapping");

            UpdateSqlClauseBuilder builder = new UpdateSqlClauseBuilder();

            FillSqlClauseBuilder(builder, graph, mapping, ClauseBindingFlags.Update,
                new DoSqlClauseBuilder<T>(DoInsertUpdateSqlClauseBuilder<T>), ignorProperties);

            return builder;
        }

        private static void DoWhereSqlClauseBuilderByPrimaryKey<T>(SqlClauseBuilderIUW builder, DataProperty item, T graph)
        {
            if (item.Primary)
            {
                object data = GetPropertyValue(item, graph);

                if ((data == null || data == DBNull.Value))
                    builder.AppendItem(item.FieldName, data, SqlClauseBuilderBase.Is);
                else
                    builder.AppendItem(item.FieldName, data);
            }
        }

        #endregion

        #region Delete

        public static string GetDeleteSql<T>(T graph, ISqlBuilder builder, params string[] ignorProperties)
        {
            DataModel mapping = InnerGetMappingInfo(graph);

            return GetDeleteSql<T>(graph, mapping, builder, ignorProperties);
        }

        private static string GetDeleteSql<T>(T graph, DataModel mapping, ISqlBuilder builder, string[] ignorProperties)
        {
            ExceptionHelper.FalseThrow<ArgumentNullException>(graph != null, "graph");
            ExceptionHelper.FalseThrow<ArgumentNullException>(mapping != null, "mapping");
            ExceptionHelper.FalseThrow<ArgumentNullException>(builder != null, "builder");

            WhereSqlClauseBuilder whereBuilder = GetWhereSqlClauseBuilderByPrimaryKey(graph, mapping);

            return string.Format("DELETE FROM {0} WHERE {1}", mapping.TableName, whereBuilder.ToSqlString(builder));
        }

        #endregion

        #region Common

        private delegate void DoSqlClauseBuilder<T>(SqlClauseBuilderIUW builder, DataProperty property, T graph);

        private static void FillSqlClauseBuilder<T>(SqlClauseBuilderIUW builder, T graph, DataModel mapping,
           ClauseBindingFlags bindingFlags, DoSqlClauseBuilder<T> builderDelegate, string[] ignorProperties)
        {
            ExceptionHelper.FalseThrow<ArgumentNullException>(graph != null, "graph");

            mapping.Properties.ForEach(property =>
            {
                if (!ignorProperties.Contains(property.Name))
                {
                    if (property.InputMapping)
                        builderDelegate(builder, property, graph);
                }
            });
        }

        private static void DoInsertUpdateSqlClauseBuilder<T>(SqlClauseBuilderIUW builder, DataProperty property, T graph)
        {
            object data = GetPropertyValue(property, graph);

            if (data == null || data == DBNull.Value || data.Equals(TypeCreator.GetTypeDefaultValue(data.GetType())))
            {
                if (string.IsNullOrWhiteSpace(property.DefaultValue))
                {
                    data = DBNull.Value;
                }
                else
                {
                    builder.AppendItem(property.FieldName, property.DefaultValue, SqlClauseBuilderBase.EqualTo, true);
                }
            }
            else
                builder.AppendItem(property.FieldName, data);
        }


        #endregion

        #endregion Insert & Update & Delete

        #region DataRow To Object

        public static void DataRowToObject<T>(DataRow row, T graph)
        {
            DataRowToObject(row, InnerGetMappingInfo(graph), graph);
        }


        public static void DataRowToObject<T>(DataRow row, T graph, DataToObjectDeligations dod)
        {
            DataRowToObject(row, InnerGetMappingInfo(graph), graph, dod);
        }


        public static void DataRowToObject<T>(DataRow row, DataModel mapping, T graph)
        {
            DataRowToObject(row, mapping, graph, null);
        }


        public static void DataRowToObject<T>(DataRow row, DataModel mapping, T graph, DataToObjectDeligations dod)
        {
            ExceptionHelper.FalseThrow<ArgumentNullException>(row != null, "row");
            ExceptionHelper.FalseThrow<ArgumentNullException>(mapping != null, "items");
            ExceptionHelper.FalseThrow<ArgumentNullException>(graph != null, "graph");
            ExceptionHelper.FalseThrow<ArgumentNullException>(row.Table != null, "row.Table");

            foreach (DataColumn column in row.Table.Columns)
            {
                if (mapping.Properties.Contains(column.ColumnName))
                {
                    DataProperty property = mapping.Properties[column.ColumnName];
                    System.Type realType = GetRealType(property.MemberInfo);
                    object data = row[column];
                    if (Convertible(realType, data))
                        SetValueToObject(property, graph, ConvertData(property, data));
                }
            }
        }

        #endregion Data To Object

        #region DataReader to Object

        public static void DataReaderToObject<T>(IDataReader dr, T graph)
        {
            DataReaderToObject(dr, InnerGetMappingInfo(graph), graph);
        }

        public static void DataReaderToObject<T>(IDataReader dr, DataModel mapping, T graph)
        {
            DataReaderToObject(dr, mapping, graph, null);
        }

        public static void DataReaderToObject<T>(IDataReader dr, DataModel mapping, T graph, DataToObjectDeligations dod)
        {
            ExceptionHelper.FalseThrow<ArgumentNullException>(dr != null, "dr");
            ExceptionHelper.FalseThrow<ArgumentNullException>(mapping != null, "items");
            ExceptionHelper.FalseThrow<ArgumentNullException>(graph != null, "graph");

            DataTable schemaTable = dr.GetSchemaTable();

            foreach (DataRow row in schemaTable.Rows)
            {
                string columnName = row["ColumnName"].ToString();
                if (mapping.Properties.Contains(columnName))
                {
                    DataProperty property = mapping.Properties[row["ColumnName"].ToString()];
                    SetMemberInfo(property, graph);
                    System.Type realType = GetRealType(property.MemberInfo);

                    object data = dr[columnName];
                    if (Convertible(realType, data))
                        SetValueToObject(property, graph, ConvertData(property, data));
                }
            }
        }
        #endregion


        #region Private

        private static DataModel InnerGetMappingInfo<T>(T graph)
        {
            ExceptionHelper.FalseThrow<ArgumentNullException>(graph != null, "graph");

            System.Type type = null;

            if (typeof(T).IsInterface)
                type = typeof(T);
            else
                type = graph.GetType();

            return InnerGetMappingInfo(type);
        }

        private static DataModel InnerGetMappingInfo(Type type)
        {
            //添加Cache
            //DataModelCache

            return XORMappingSerializer.Deserialize(type);
        }

        private static void SetMemberInfo(DataProperty property, object graph)
        {
            if (property.MemberInfo == null)
                property.MemberInfo = graph.GetType().GetProperty(property.Name);
            
            if (property.MemberInfo == null)
                property.MemberInfo = graph.GetType().GetField(property.Name);
        }

        private static object GetPropertyValue(DataProperty property, object graph)
        {
            object data = null;

            if (graph != null)
            {
                SetMemberInfo(property, graph);

                if (property.MemberInfo != null)
                {
                    data = GetMemberValue(property.MemberInfo, graph);
                }
            }

            return data;
        }

        private static object GetMemberValue(MemberInfo mi, object graph)
        {
            try
            {
                object data = null;

                switch (mi.MemberType)
                {
                    case MemberTypes.Property:
                        PropertyInfo pi = (PropertyInfo)mi;
                        if (pi.CanRead)
                            data = pi.GetValue(graph, null);
                        break;
                    case MemberTypes.Field:
                        FieldInfo fi = (FieldInfo)mi;
                        data = fi.GetValue(graph);
                        break;
                    default:
                        break;
                }

                return data;
            }
            catch (System.Exception ex)
            {
                System.Exception realEx = ExceptionHelper.GetRealException(ex);

                throw new ApplicationException(string.Format("读取属性{0}值的时候出错，{1}", mi.Name, realEx.Message));
            }
        }

        private static void SetMemberValue(MemberInfo mi, object graph, object data)
        {
            data = DecorateDate(data);

            switch (mi.MemberType)
            {
                case MemberTypes.Property:
                    PropertyInfo pi = (PropertyInfo)mi;
                    if (pi.CanWrite)
                        pi.SetValue(graph, data, null);
                    break;
                case MemberTypes.Field:
                    FieldInfo fi = (FieldInfo)mi;
                    fi.SetValue(graph, data);
                    break;
                default:
                    //ThrowInvalidMemberInfoTypeException(mi);
                    break;
            }
        }

        private static object DecorateDate(object data)
        {
            object result = data;

            if (data is DateTime)
            {
                DateTime dt = (DateTime)data;

                if (dt.Kind == DateTimeKind.Unspecified)
                    result = DateTime.SpecifyKind(dt, DateTimeKind.Local);
            }

            return result;
        }

        private static System.Type GetRealType(MemberInfo mi)
        {
            System.Type type = null;

            switch (mi.MemberType)
            {
                case MemberTypes.Property:
                    type = ((PropertyInfo)mi).PropertyType;
                    break;
                case MemberTypes.Field:
                    type = ((FieldInfo)mi).FieldType;
                    break;
                default:
                    break;
            }

            if (type.IsGenericType && type.GetGenericTypeDefinition().FullName == "System.Nullable`1")
                type = type.GetGenericArguments()[0];

            return type;
        }

        private static bool Convertible(System.Type targetType, object data)
        {
            bool result = true;

            if (data == null && targetType.IsValueType)
                result = false;
            else
            {
                if (data == DBNull.Value)
                {
                    if (targetType != typeof(DBNull) && targetType != typeof(string))
                        result = false;
                }
            }

            return result;
        }

        private static void SetValueToObject(DataProperty property, object graph, object data)
        {
            SetMemberValue(property.MemberInfo, graph, data);
        }

        /// <summary>
        /// 将从DataRow或DataReader读取出来的Object值转换成其原始类型
        /// </summary>
        /// <param name="property">DataProperty 映射关系</param>
        /// <param name="data"> DataRow["ColName"] OR DataReader["ColName"]</param>
        /// <returns></returns>
        private static object ConvertData(DataProperty property, object data)
        {
            try
            {
                System.Type realType = GetRealType(property.MemberInfo);

                return DataConverter.ChangeType(data, realType);
            }
            catch (System.Exception ex)
            {
                throw new SystemSupportException(
                    string.Format(Resource.ConvertDataFieldToPropertyError,
                        property.FieldName, property.Name, ex.Message),
                    ex
                    );
            }
        }

        #endregion
    }
}
