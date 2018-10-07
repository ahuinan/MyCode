/************************************************************************************
 * Copyright (c) 2017 All Rights Reserved. 
 * CLR版本：4.0.30319.42000
 * 机器名称：JIAN
 * 命名空间：MyCode.Project.Domain.Message.Common
 * 文件名：IdRequest
 * 版本号：v1.0.0.0
 * 唯一标识：c03a97d1-ffd9-4b98-a049-f15dfea6361d
 * 当前的用户域：JIAN
 * 创建人：简玄冰
 * 电子邮箱：jianxuanhuo1@126.com
 * 创建时间：2017/6/8 10:35:56
 * 描述：
 *
 * =====================================================================
 * 修改标记：
 * 修改时间：2017/6/8 10:35:56
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

namespace MyCode.Project.Domain.Message.Common
{
    /// <summary>
    /// ID请求相关
    /// </summary>
    public class IdAct
    {
        /// <summary>
        /// 系统编号,具体传递看注释
        /// </summary>
        public Guid Id { get; set; }
    }
}
