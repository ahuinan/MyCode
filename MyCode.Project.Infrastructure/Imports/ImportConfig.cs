using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCode.Project.Infrastructure.Common;
using MyCode.Project.Infrastructure.Exceptions;

namespace MyCode.Project.Infrastructure.Imports
{
    /// <summary>
    /// 导入配置类
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    public class ImportConfig<T> where T : class, new()
    {
        /// <summary>
        /// 导入的数据文件，转换为Excel操作实体
        /// </summary>
        public ExcelUtils ExcelUtil { get; set; }

        /// <summary>
        /// Excel中的标题和实体属性中的字段map，例如：标题->Title
        /// </summary>
        public List<ImportProperty> Properties { get; private set; }

        /// <summary>
        /// 默认的属性列，这些列不需要Excel导入，而是程序中或者导入逻辑中直接设定
        /// </summary>
        public List<DefaultValueProperty> DefaultProperties { get; private set; }

        /// <summary>
        /// 复杂类型的属性列
        /// </summary>
        public List<ImportComplexProperty> ComplexProperties { get; private set; }

        /// <summary>
        /// 二维数据的属性列
        /// </summary>
        public List<ImportSplitProperty> SplitProperties { get; private set; }

        /// <summary>
        /// 子表数据的属性列
        /// </summary>
        public List<ImportListProperty> ListProperties { get; private set; }

        /// <summary>
        /// 初始化一个<see cref="ImportConfig{T}"/>类型的实例
        /// </summary>
        public ImportConfig()
        {
            Properties=new List<ImportProperty>();
            DefaultProperties=new List<DefaultValueProperty>();
            ComplexProperties=new List<ImportComplexProperty>();
            SplitProperties=new List<ImportSplitProperty>();
            ListProperties=new List<ImportListProperty>();
        }

        /// <summary>
        /// 验证Excel的导入格式是否正确
        /// </summary>
        /// <param name="message">消息</param>
        /// <returns></returns>
        public bool ValidFormat(ref string message)
        {
            //获取Excel的第一行数据，即标题
            var columns = this.ExcelUtil.ExportFirstRowData();
            foreach (var item in this.Properties)
            {
                //检查是否存在对应列
                if (columns.All(x => x.Trim() != item.Caption))
                {
                    message = string.Format("Excel中找不到[{0}]列!", item.Caption);
                    return false;
                }
            }
            foreach (var item in this.ComplexProperties)
            {
                //检查是否存在对应列
                if (columns.All(x => x.Trim() != item.Caption))
                {
                    message = string.Format("Excel中找不到[{0}]列!", item.Caption);
                    return false;
                }
            }
            foreach (var item in this.SplitProperties)
            {
                //检查是否存在对应列
                if (columns.All(x => x.Trim() != item.Caption))
                {
                    message = string.Format("Excel中找不到[{0}]列!", item.Caption);
                    return false;
                }
            }
            foreach (var item in this.ListProperties)
            {
                //检查是否存在对应列
                if (columns.All(x => x.Trim() != item.Caption))
                {
                    message = string.Format("Excel中找不到[{0}]列!", item.Caption);
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 将Excel数据转换为实体列表
        /// </summary>
        /// <param name="sourceData">数据源</param>
        /// <returns></returns>
        public List<T> GetListData(List<Dictionary<string, object>> sourceData)
        {
            var type = typeof(T);
            List<T> resultData=new List<T>();

            foreach (var item in sourceData)
            {
                T entity=new T();                
                object listItem = null;// 该对象用于存储子表数据                
                bool isAddList = false;// 判断是否增加子表数据
                foreach (var prop in this.ListProperties)
                {
                    var input = item[prop.Caption];
                    var keyValue = item[prop.PrimaryKeyCaption].ToString();
                    //验证格式
                    string message = "";
                    if (!prop.ValidateInput(input, ref message))
                    {
                        throw new BaseException(message);
                    }
                    //获取对应类型的值
                    var value = prop.GetValue(input);

                    var matersEntity =
                        resultData.FirstOrDefault(
                            p => type.GetProperty(prop.PrimaryKey).GetValue(p).ToString() == keyValue);
                    IList procList = null;
                    if (matersEntity != null)
                    {
                        procList = type.GetProperty(prop.ComplexPropName).GetValue(matersEntity, null) as IList;
                        //因为主表数据已经存在，所以这个时候只需要增加子表数据。后面的数据直接跳过
                        isAddList = true;
                    }
                    else
                    {
                        procList = type.GetProperty(prop.ComplexPropName).GetValue(entity, null) as IList;
                        if (procList == null)
                        {
                            //创建子表集合对象
                            var procType = prop.ComplexPropType;
                            var listType = typeof(List<>).MakeGenericType(procType);
                            procList = Activator.CreateInstance(listType) as IList;
                            //赋值
                            type.GetProperty(prop.ComplexPropName).SetValue(entity,procList,null);
                        }
                    }

                    if (listItem == null)
                    {
                        //创建一个目标类型的空实例
                        listItem = prop.ComplexPropType.Assembly.CreateInstance(prop.ComplexPropType.FullName);
                        //添加到子表集合中
                        procList.Add(listItem);
                    }

                    if (prop.ChildProperty != null)
                    {
                        var childEntity =
                            prop.ChildProperty.PropType.Assembly.CreateInstance(prop.ChildProperty.PropType.FullName);
                        prop.ChildProperty.PropType.GetProperty(prop.ChildProperty.EntityProp).SetValue(childEntity,value,null);
                        prop.ComplexPropType.GetProperty(prop.EntityProp).SetValue(listItem, childEntity, null);
                    }
                    else
                    {
                        prop.ComplexPropType.GetProperty(prop.EntityProp).SetValue(listItem,value,null);
                    }
                }

                if (isAddList)
                {
                    //因为主表数据已经存在，所以这个时候只需要增加子表数据。后面的数据直接跳过
                    continue;
                }
                //设置基础值
                foreach (var prop in this.Properties)
                {
                    var input = item[prop.Caption];
                    //验证格式
                    string message = "";
                    if (!prop.ValidateInput(input, ref message))
                    {
                        throw new BaseException(message);
                    }
                    //获取对应类型的值
                    var value = prop.GetValue(input);
                    type.GetProperty(prop.EntityProp).SetValue(entity,value,null);
                }

                //设置默认值
                foreach (var prop in this.DefaultProperties)
                {
                    type.GetProperty(prop.EntityProp).SetValue(entity,prop.DefaultValue,null);
                }

                //设置复杂类型属性
                foreach (var prop in this.ComplexProperties)
                {
                    var input = item[prop.Caption];
                    //验证格式
                    string message = "";
                    if (!prop.ValidateInput(input, ref message))
                    {
                        throw new BaseException(message);
                    }

                    //获取对应类型的值
                    var value = prop.GetValue(input);

                    var propEntity = type.GetProperty(prop.ComplexPropName).GetValue(entity);
                    if (propEntity == null)
                    {
                        //创建一个目标类型的空实例
                        propEntity = prop.ComplexPropType.Assembly.CreateInstance(prop.ComplexPropType.FullName);
                        if (prop.ChildProperty != null)
                        {
                            var childEntity =
                                prop.ChildProperty.PropType.Assembly.CreateInstance(prop.ChildProperty.PropType.FullName);
                            prop.ChildProperty.PropType.GetProperty(prop.ChildProperty.EntityProp).SetValue(childEntity,value,null);
                            prop.ComplexPropType.GetProperty(prop.EntityProp).SetValue(propEntity, childEntity, null);

                        }
                        else
                        {
                            prop.ComplexPropType.GetProperty(prop.EntityProp).SetValue(propEntity,value,null);                            
                        }
                        //添加到实体中
                        type.GetProperty(prop.ComplexPropName).SetValue(entity,propEntity,null);                        
                    }
                    else
                    {
                        if (prop.ChildProperty != null)
                        {
                            var childEntity =
                                prop.ChildProperty.PropType.Assembly.CreateInstance(prop.ChildProperty.PropType.FullName);
                            prop.ChildProperty.PropType.GetProperty(prop.ChildProperty.EntityProp).SetValue(childEntity,value,null);
                            prop.ComplexPropType.GetProperty(prop.EntityProp).SetValue(propEntity, childEntity, null);
                        }
                        else
                        {
                            prop.ComplexPropType.GetProperty(prop.EntityProp).SetValue(propEntity, value, null);
                        }
                    }
                }

                //设置二维数据类型属性
                foreach (var prop in this.SplitProperties)
                {
                    var arrayData = item[prop.Caption].ToString().Split(',');

                    //创建对应的类型集合
                    var listType = typeof(List<>).MakeGenericType(prop.ComplexPropType);
                    var procList = Activator.CreateInstance(listType) as IList;

                    foreach (var propValue in arrayData)
                    {
                        var input = propValue;
                        //验证格式
                        string message = "";
                        if (!prop.ValidateInput(input, ref message))
                        {
                            throw new BaseException(message);
                        }
                        //获取对应类型的值
                        var value = prop.GetValue(input);
                        if (prop.ChildProperty != null)
                        {
                            var propEntity = prop.ComplexPropType.Assembly.CreateInstance(prop.ComplexPropType.FullName);
                            var childEntity =
                                prop.ChildProperty.PropType.Assembly.CreateInstance(prop.ChildProperty.PropType.FullName);

                            prop.ChildProperty.PropType.GetProperty(prop.ChildProperty.EntityProp).SetValue(childEntity, value, null);
                            prop.ComplexPropType.GetProperty(prop.EntityProp).SetValue(propEntity, childEntity, null);
                            procList.Add(propEntity);
                        }
                        else
                        {
                            var propEntity = prop.ComplexPropType.Assembly.CreateInstance(prop.ComplexPropType.FullName);
                            prop.ComplexPropType.GetProperty(prop.EntityProp).SetValue(propEntity, value, null);
                            procList.Add(propEntity);
                        }
                    }

                    //将集合添加到实体中
                    type.GetProperty(prop.ComplexPropName).SetValue(entity,procList,null);
                }
                resultData.Add(entity);
            }
            return resultData;
        }
    }
}
