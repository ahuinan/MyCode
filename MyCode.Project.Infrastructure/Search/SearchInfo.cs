using System;
using System.Collections.Generic;
using System.Text;

namespace MyCode.Project.Infrastructure.Search
{
    /// <summary>
    /// 查询信息实体类
    /// </summary>
    public class SearchInfo
    {
        public SearchInfo() {}

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="fieldName">字段名称</param>
        /// <param name="fieldValue">字段的值</param>
        /// <param name="sqlOperator">字段的Sql操作符号</param>
        public SearchInfo(string fieldName, object fieldValue, SqlOperator sqlOperator)
            : this(fieldName, fieldValue, sqlOperator, false)
        { }

		
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="fieldName">字段名称</param>
        /// <param name="fieldValue">字段的值</param>
        /// <param name="sqlOperator">字段的Sql操作符号</param>
        /// <param name="excludeIfEmpty">如果字段为空或者Null则不作为查询条件</param>
        public SearchInfo(string fieldName, object fieldValue, SqlOperator sqlOperator, bool excludeIfEmpty) 
        {
            this.fieldName = fieldName;
            this.fieldValue = fieldValue;
            this.sqlOperator = sqlOperator; 
            this.excludeIfEmpty = excludeIfEmpty;
        }

        private string fieldName;
        private object fieldValue;
        private SqlOperator sqlOperator;
        private bool excludeIfEmpty = false;


        /// <summary>
        /// 字段名称
        /// </summary>
        public string FieldName
        {
            get { return fieldName; }
            set { fieldName = value; }
        }

        /// <summary>
        /// 字段的值
        /// </summary>
        public object FieldValue
        {
            get { return fieldValue; }
            set { fieldValue = value; }
        }

        /// <summary>
        /// 字段的Sql操作符号
        /// </summary>
        public SqlOperator SqlOperator
        {
            get { return sqlOperator; }
            set { sqlOperator = value; }
        }

        /// <summary>
        /// 如果字段为空或者Null则不作为查询条件
        /// </summary>
        public bool ExcludeIfEmpty
        {
            get { return excludeIfEmpty; }
            set { excludeIfEmpty = value; }
        }
    }
}
