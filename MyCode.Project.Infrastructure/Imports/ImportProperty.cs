using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MyCode.Project.Infrastructure.Extensions;

namespace MyCode.Project.Infrastructure.Imports
{
    /// <summary>
    /// 基本类型数据属性类
    /// </summary>
    public class ImportProperty
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Caption { get; set; }

        /// <summary>
        /// 实体的属性名称
        /// </summary>
        public string EntityProp { get; set; }

        /// <summary>
        /// 是否允许为空，默认允许
        /// </summary>
        public bool IsNullable { get; set; }

        /// <summary>
        /// 最大长度
        /// </summary>
        public int MaxLength { get; set; }

        /// <summary>
        /// 数据类型，默认为字符串，只有string、int、decimal三个类型，其他类型不做检查
        /// </summary>
        public Type ValueType { get; set; }

        public ImportProperty()
        {
            this.IsNullable = true;
            this.ValueType = typeof(string);
        }

        /// <summary>
        /// 验证数据格式是否合法
        /// </summary>
        /// <param name="input">输入数据</param>
        /// <param name="message">错误消息</param>
        /// <returns></returns>
        public bool ValidateInput(object input, ref string message)
        {
            if (input == null && this.IsNullable)
            {
                return true;
            }
            if (input == null && !this.IsNullable)
            {
                message=string.Format("[{0}]列不允许为空，必须填写数据!",this.Caption);
                return false;
            }
            var str = input.ToString();
            if (string.IsNullOrEmpty(str) && !this.IsNullable)
            {
                message = string.Format("[{0}]列不允许为空，必须填写数据!", this.Caption);
                return false;
            }
            if (string.IsNullOrEmpty(str) && this.IsNullable)
            {
                return true;
            }
            if (str.Length > this.MaxLength)
            {
                message = string.Format("[{0}]列中的[{1}]数据超出最大长度!", this.Caption, input);
                return false;
            }
            if (this.ValueType == typeof(decimal))
            {
                if (!Regex.IsMatch(str, @"^([0-9]+\.[0-9]+)|[0-9]+$"))
                {
                    message=string.Format("[{0}]列中的[{1}]数据不是合法的数值!",this.Caption,input);
                    return false;
                }
            }
            if (this.ValueType == typeof(int))
            {
                if (!Regex.IsMatch(str, @"^[0-9]+$"))
                {
                    message = string.Format("[{0}]列中的[{1}]数据不是合法的整数!", this.Caption, input);
                    return false;
                }
            }
            if (this.ValueType == typeof(DateTime))
            {
                DateTime value=DateTime.Now;
                if (!DateTime.TryParse(str, out value))
                {
                    message = string.Format("[{0}]列中的[{1}]数据不是合法的时间!", this.Caption, input);
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 获取对应类型的数据
        /// </summary>
        /// <param name="input">输入数据</param>
        /// <returns></returns>
        public object GetValue(object input)
        {
            if (input == null)
            {
                return null;
            }
            if (input.GetType() == this.ValueType)
            {
                return input;
            }
            return Convert.ChangeType(input, this.ValueType);
        }


    }
}
