/************************************************************************************
 * Copyright (c) 2017 All Rights Reserved. 
 * CLR版本：4.0.30319.42000
 * 机器名称：JIAN
 * 命名空间：MyCode.Project.Infrastructure.Imports
 * 文件名：ImportListProperty
 * 版本号：v1.0.0.0
 * 唯一标识：7ee2c39b-a61c-4851-ae9f-67f24d552148
 * 当前的用户域：JIAN
 * 创建人：简玄冰
 * 电子邮箱：jianxuanhuo1@126.com
 * 创建时间：2017/6/21 11:48:32
 * 描述：
 *
 * =====================================================================
 * 修改标记：
 * 修改时间：2017/6/21 11:48:32
 * 修改人：简玄冰
 * 版本号：v1.0.0.0
 * 描述：
 *
/************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCode.Project.Infrastructure.Imports
{
    /// <summary>
    /// 子表数据导入
    /// </summary>
    public class ImportListProperty:ImportProperty
    {
        /// <summary>
        /// 复杂属性的名称
        /// </summary>
        public string ComplexPropName { get; set; }

        /// <summary>
        /// 复杂属性的类型
        /// </summary>
        public Type ComplexPropType { get; set; }

        /// <summary>
        /// 子属性类型
        /// </summary>
        public ImportChildProperty ChildProperty { get; set; }

        /// <summary>
        /// 主表的主键字段，即唯一标记
        /// </summary>
        public string PrimaryKey { get; set; }

        /// <summary>
        /// 主表的主键字段在Excel中的标题，即唯一标记
        /// </summary>
        public string PrimaryKeyCaption { get; set; }
    }
}
