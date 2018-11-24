using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data.Common;
using System.Data;
using SqlSugar;

namespace MyCode.Project.Infrastructure.Search
{
    public class SearchCondition
    {
        private Hashtable conditionTable = new Hashtable();
        public Hashtable ConditionTable
        {
            get { return this.conditionTable; }
        }

        private List<SugarParameter> parameters = null;

        public SearchCondition()
        {
            parameters = new List<SugarParameter>(); ;
        }

        /// <summary>
        /// 为查询添加条件
        /// <example>
        /// 用法一：
        /// SearchCondition searchObj = new SearchCondition();
        /// searchObj.AddCondition("Test", 1, SqlOperator.NotEqual);
        /// searchObj.AddCondition("Test2", "Test2Value", SqlOperator.Like);
        /// string conditionSql = searchObj.BuildConditionSql();
        /// 
        /// 用法二：AddCondition函数可以串起来添加多个条件
        /// SearchCondition searchObj = new SearchCondition();
        /// searchObj.AddCondition("Test", 1, SqlOperator.NotEqual).AddCondition("Test2", "Test2Value", SqlOperator.Like);
        /// string conditionSql = searchObj.BuildConditionSql();
        /// </example>
        /// </summary>
        /// <param name="fielName">字段名称</param>
        /// <param name="fieldValue">字段值</param>
        /// <param name="sqlOperator">SqlOperator枚举类型</param>
        /// <returns>增加条件后的Hashtable</returns>
        //public SearchCondition AddCondition(string fielName, object fieldValue, SqlOperator sqlOperator)
        //{
        //    this.conditionTable.Add(fielName, new SearchInfo(fielName, fieldValue, sqlOperator));
        //    return this;
        //}

       
        #region AddCondition(为查询添加条件)
        /// <summary>
        /// 为查询添加条件
        /// <example>
        /// 用法一：
        /// SearchCondition searchObj = new SearchCondition();
        /// searchObj.AddCondition("Test", 1, SqlOperator.NotEqual, false);
        /// searchObj.AddCondition("Test2", "Test2Value", SqlOperator.Like, true);
        /// string conditionSql = searchObj.BuildConditionSql();
        /// 
        /// 用法二：AddCondition函数可以串起来添加多个条件
        /// SearchCondition searchObj = new SearchCondition();
        /// searchObj.AddCondition("Test", 1, SqlOperator.NotEqual, false).AddCondition("Test2", "Test2Value", SqlOperator.Like, true);
        /// string conditionSql = searchObj.BuildConditionSql();
        /// </example>
        /// </summary>
        /// <param name="fieldName">字段名称</param>
        /// <param name="fieldValue">字段值</param>
        /// <param name="sqlOperator">SqlOperator枚举类型</param>
        /// <param name="IfExclude">是否排除，比如!string.isNullOrEmpty(key)</param>
        /// <returns></returns>
        public SearchCondition AddCondition(string fieldName, object fieldValue, SqlOperator sqlOperator, bool ifExclude)
        {
            this.conditionTable.Add(fieldName, new SearchInfo(fieldName, fieldValue, sqlOperator, ifExclude));
            return this;
        }
        #endregion




        #region AddSqlCondition(增加SQL的条件语句)
        /// <summary>
        /// 增加SQL的条件语句
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="excludeIfEmpty"></param>
        /// <returns></returns>
        public SearchCondition AddSqlCondition(string sql, bool excludeIfEmpty, params SugarParameter[] parameters)
        {

            //对关键字如果是使用了like搜索，则"%" + searchInfo.FieldValue + "%"

            this.parameters.AddRange(parameters);

            this.conditionTable.Add(sql, new SearchInfo(sql, "", SqlOperator.Sql, excludeIfEmpty));
            return this;
        }
        #endregion

        #region BuildConditionSql(生成Sql条件语句)
        /// <summary>
        /// 根据对象构造相关的条件语句（不使用参数），如返回的语句是:
        /// <![CDATA[
        /// Where (1=1)  AND Test4  <  'Value4' AND Test6  >=  'Value6' AND Test7  <=  'value7' AND Test  <>  '1' AND Test5  >  'Value5' AND Test2  Like  '%Value2%' AND Test3  =  'Value3'
        /// ]]>
        /// </summary>
        /// <returns></returns> 
        public BuildSqlReturnModel BuildConditionSql()
        {
            string sql = " (1=1) ";
            string fieldName = string.Empty;
            SearchInfo searchInfo = null;



            StringBuilder sb = new StringBuilder();

            foreach (DictionaryEntry de in this.conditionTable)
            {
                searchInfo = (SearchInfo)de.Value;

                //如果选择ExcludeIfEmpty为True,并且该字段为空值的话,跳过
                //if (searchInfo.ExcludeIfEmpty && string.IsNullOrEmpty((string)searchInfo.FieldValue))
                if (!searchInfo.ExcludeIfEmpty) { continue; }

                //直接执行的Sql语句
                if (searchInfo.SqlOperator == SqlOperator.Sql)
                {
                    sb.AppendFormat(" AND {0} ", searchInfo.FieldName);
                    continue;
                }

                //如果为null或者空字符串则不执行
                if (string.IsNullOrEmpty(Convert.ToString(searchInfo.FieldValue))) { continue; }

                var fieldType = GetFieldDbType(searchInfo.FieldValue);

                if (fieldType == System.Data.DbType.Guid && (Guid)searchInfo.FieldValue == Guid.Empty) { continue; }

                if (searchInfo.SqlOperator == SqlOperator.Like)
                {
                    if (!string.IsNullOrEmpty(searchInfo.FieldValue.ToString()))
                    {
                        searchInfo.FieldValue = searchInfo.FieldValue.ToString();
                        //sb.AppendFormat(" AND {0} {1} '{2}'", searchInfo.FieldName,this.ConvertSqlOperator(searchInfo.SqlOperator), string.Format("%{0}%", searchInfo.FieldValue));

                        sb.AppendFormat(" AND {0} {1} {2}", searchInfo.FieldName, this.ConvertSqlOperator(searchInfo.SqlOperator), $"@{searchInfo.FieldName}");

                        parameters.Add(new SugarParameter(searchInfo.FieldName, "%" + searchInfo.FieldValue + "%"));
                    }

                }
                else if (searchInfo.SqlOperator == SqlOperator.In)
                {
                    var list = (IList)searchInfo.FieldValue;
                    StringBuilder sbInSQL = new StringBuilder();
                    foreach (var obj in list)
                    {
                        sbInSQL.AppendFormat("'{0}',", obj.ToString());
                    }
                    string inSQL = sbInSQL.ToString().TrimEnd(',');
                    sb.AppendFormat(" AND {0} {1} ({2})", searchInfo.FieldName, this.ConvertSqlOperator(searchInfo.SqlOperator), inSQL);

                }
                else
                {

                    if (fieldType == System.Data.DbType.Int32 || fieldType == System.Data.DbType.Int64)
                    {
                        sb.AppendFormat(" AND {0} {1} {2}", searchInfo.FieldName,
                            this.ConvertSqlOperator(searchInfo.SqlOperator), searchInfo.FieldValue);
                    }
                    else
                    {
                        //sb.AppendFormat(" AND {0} {1} '{2}'", searchInfo.FieldName,this.ConvertSqlOperator(searchInfo.SqlOperator), searchInfo.FieldValue);

                        sb.AppendFormat(" AND {0} {1} {2}", searchInfo.FieldName, this.ConvertSqlOperator(searchInfo.SqlOperator), $"@{searchInfo.FieldName}");

                        parameters.Add(new SugarParameter(searchInfo.FieldName, searchInfo.FieldValue));
                    }
                }
            }

            sql += sb.ToString();

            return new BuildSqlReturnModel() { Sql = sql, ListParameter = parameters };
        }
        #endregion

        #region ConvertSqlOperator(操作符转换为对应的Sql)

        /// <summary>
        /// 转换枚举类型为对应的Sql语句操作符号
        /// </summary>
        /// <param name="sqlOperator">SqlOperator枚举对象</param>
        /// <returns><![CDATA[对应的Sql语句操作符号（如 ">" "<>" ">=")]]></returns>
        private string ConvertSqlOperator(SqlOperator sqlOperator)
        {
            string stringOperator = " = ";
            switch (sqlOperator)
            {
                case SqlOperator.Equal:
                    stringOperator = " = ";
                    break;
                case SqlOperator.LessThan:
                    stringOperator = " < ";
                    break;
                case SqlOperator.LessThanOrEqual:
                    stringOperator = " <= ";
                    break;
                case SqlOperator.Like:
                    stringOperator = " Like ";
                    break;
                case SqlOperator.MoreThan:
                    stringOperator = " > ";
                    break;
                case SqlOperator.MoreThanOrEqual:
                    stringOperator = " >= ";
                    break;
                case SqlOperator.NotEqual:
                    stringOperator = " <> ";
                    break;
                case SqlOperator.In:
                    stringOperator = " in ";
                    break;
                default:
                    break;
            }

            return stringOperator;
        }
        #endregion

        #region GetFieldDbType(根据传入对象的值类型获取其对应的DbType类型)
        /// <summary>
        /// 根据传入对象的值类型获取其对应的DbType类型
        /// </summary>
        /// <param name="fieldValue">对象的值</param>
        /// <returns>DbType类型</returns>
        private System.Data.DbType GetFieldDbType(object fieldValue)
        {
            System.Data.DbType type = System.Data.DbType.String;

            switch (fieldValue.GetType().ToString())
            {
                case "System.Int16":
                    type = System.Data.DbType.Int16;
                    break;
                case "System.UInt16":
                    type = System.Data.DbType.UInt16;
                    break;
                case "System.Single":
                    type = System.Data.DbType.Single;
                    break;
                case "System.UInt32":
                    type = System.Data.DbType.UInt32;
                    break;
                case "System.Int32":
                    type = System.Data.DbType.Int32;
                    break;
                case "System.UInt64":
                    type = System.Data.DbType.UInt64;
                    break;
                case "System.Int64":
                    type = System.Data.DbType.Int64;
                    break;
                case "System.String":
                    type = System.Data.DbType.String;
                    break;
                case "System.Double":
                    type = System.Data.DbType.Double;
                    break;
                case "System.Decimal":
                    type = System.Data.DbType.Decimal;
                    break;
                case "System.Byte":
                    type = System.Data.DbType.Byte;
                    break;
                case "System.Boolean":
                    type = System.Data.DbType.Boolean;
                    break;
                case "System.DateTime":
                    type = System.Data.DbType.DateTime;
                    break;
                case "System.Guid":
                    type = System.Data.DbType.Guid;
                    break;
                default:
                    break;
            }
            return type;
        }
        #endregion
    }
}
