using System;
using System.Text;
using Framework.Core;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Framework.Data
{
    /// <summary>
    /// 提供一组字段和值的集合，帮助生成WHERE语句
    /// </summary>
    [Serializable]
    public class WhereSqlClauseBuilder : SqlClauseBuilderIUW, IConnectiveSqlClause
    {
        private LogicOperatorDefine logicOperator = LogicOperatorDefine.And;

        /// <summary>
        /// 构造方法
        /// </summary>
        public WhereSqlClauseBuilder()
            : base()
        {
        }

        /// <summary>
        /// 构造方法，可以指定生成条件表达式时的逻辑运算符
        /// </summary>
        /// <param name="lod">逻辑运算符</param>
        public WhereSqlClauseBuilder(LogicOperatorDefine lod)
            : base()
        {
            this.logicOperator = lod;
        }

        /// <summary>
        /// 条件表达式之间的逻辑运算符
        /// </summary>
        public LogicOperatorDefine LogicOperator
        {
            get
            {
                return this.logicOperator;
            }
            set
            {
                this.logicOperator = value;
            }
        }

        /// <summary>
        /// 判断是否不存在任何条件表达式
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return this.Count == 0;
            }
        }

        public override void AppendItem<T>(string dataField, T data, string op, bool isExpression, bool filterNull)
        {
            if (!isExpression && data != null)
            {
                if (op.Trim().Equals(SqlClauseBuilderBase.In, StringComparison.InvariantCultureIgnoreCase))
                {

                    if ((data is IEnumerable) == false || (data.GetType().IsGenericType == false))
                    {
                        throw new InvalidCastException("in 操作符对应的数据应该是个可枚举的泛型类(IEnumabl，Generic )对象");
                    }
                    else
                    {
                        if (data is List<string>)
                        {
                            var dataList = (data as List<string>).Select(x => string.Format("'{0}'", x));
                            if (dataList.Count() == 0)
                            {
                                return;
                            }
                            base.AppendItem<string>(dataField, string.Format("({0})",
                        string.Join(",", dataList)),
                        SqlClauseBuilderBase.In,
                        true,
                        filterNull);
                            return;
                        }
                        if (data is List<int>)
                        {
                            var dataList = data as List<int>;
                            if (dataList.Count() == 0)
                            {
                                return;
                            }
                            base.AppendItem<string>(dataField, string.Format("({0})",
                        string.Join(",", dataList)),
                        SqlClauseBuilderBase.In,
                        true,
                        filterNull);
                            return;
                        }
                        return;
                    }




                }
                else if (op.Trim().Equals(SqlClauseBuilderBase.Like, StringComparison.InvariantCultureIgnoreCase))
                {
                    base.AppendItem<string>(dataField, string.Format("%{0}%",
                        EscapeLikeString(data.ToString(), false)),
                        SqlClauseBuilderBase.Like,
                        isExpression,
                        filterNull);
                    return;
                }
                else if (op.Trim().Equals("startwith", StringComparison.InvariantCultureIgnoreCase))
                {
                    base.AppendItem<string>(dataField, string.Format("{0}%",
                        EscapeLikeString(data.ToString(), false)),
                        SqlClauseBuilderBase.Like,
                        isExpression,
                        filterNull);
                    return;
                }
                else if (op.Trim().Equals("endwith", StringComparison.InvariantCultureIgnoreCase))
                {
                    base.AppendItem<string>(dataField, string.Format("%{0}",
                        EscapeLikeString(data.ToString(), false)),
                        SqlClauseBuilderBase.Like,
                        isExpression,
                        filterNull);
                    return;
                }
            }

            base.AppendItem<T>(dataField, data, op, isExpression, filterNull);
        }

        /// <summary>
        /// 帮助生成WHERE语句
        /// </summary>
        /// <param name="sqlBuilder">语句构造器</param>
        /// <returns>构造的Where子句(不含where部分)</returns>
        public override string ToSqlString(ISqlBuilder sqlBuilder)
        {
            ExceptionHelper.FalseThrow<ArgumentNullException>(sqlBuilder != null, "sqlBuilder");

            StringBuilder strB = new StringBuilder(256);

            foreach (SqlClauseBuilderItemIUW item in List)
            {
                string value = item.GetDataDesp(sqlBuilder);
                if (value == "NULL" && item.FilterNull)
                    continue;

                if (strB.Length > 0)
                    strB.AppendFormat(" {0} ", EnumItemDescriptionAttribute.GetAttribute(this.logicOperator).ShortName);

                string[] fields = item.DataField.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (fields.Length > 1)
                {
                    strB.Append("(");
                    for (int i = 0; i < fields.Length; i++)
                    {
                        if (i > 0)
                            strB.Append(" OR ");

                        strB.Append(fields[i]);
                        strB.AppendFormat(" {0} ", item.Operation);
                        strB.Append(item.GetDataDesp(sqlBuilder));
                    }
                    strB.Append(")");
                }
                else
                {
                    strB.Append(item.DataField);
                    strB.AppendFormat(" {0} ", item.Operation);
                    strB.Append(item.GetDataDesp(sqlBuilder));
                }
            }
            return strB.ToString();
        }

        /// <summary>
        /// 将LIKE对应的查询子句转义。将语句中的%、[、_转义
        /// </summary>
        /// <param name="likeString"></param>
        /// <param name="omitStartAndEnd">true 表示忽略likeString首尾的特殊字符的转义</param>
        /// <returns></returns>
        public static string EscapeLikeString(string likeString, bool omitStartAndEnd = true)
        {
            string result = likeString;

            if (omitStartAndEnd)
            {
                char[] delimiters = "%_".ToCharArray();

                int startTrimedLength, endTrimedLength;
                result = result.TrimStart(delimiters);
                startTrimedLength = likeString.Length - result.Length;

                result = result.TrimEnd(delimiters);
                endTrimedLength = likeString.Length - result.Length - startTrimedLength;

                // 保留首尾的特殊字符
                result = likeString.Substring(0, startTrimedLength)
                    + InnerEscapeLikeString(result)
                    + likeString.Substring(likeString.Length - endTrimedLength, endTrimedLength);
            }

            else
            {
                result = InnerEscapeLikeString(result);
            }

            return result;
        }

        private static string InnerEscapeLikeString(string likeString)
        {
            string result = likeString;
            if (string.IsNullOrEmpty(result) == false)
            {
                result = result.Replace("[", "[[]");
                result = result.Replace("%", "[%]");
                result = result.Replace("_", "[_]");
            }

            return result;
        }
    }

}
