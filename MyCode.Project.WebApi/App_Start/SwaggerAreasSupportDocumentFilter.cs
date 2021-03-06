﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Description;
using System.Web.Http.Dispatcher;
using Swashbuckle.Swagger;

namespace MyCode.Project.WebApi.App_Start
{
    public class SwaggerAreasSupportDocumentFilter : IDocumentFilter
    {
        /// <summary>
        /// 申请处理
        /// </summary>
        /// <param name="swaggerDoc"></param>
        /// <param name="schemaRegistry"></param>
        /// <param name="apiExplorer"></param>
        public void Apply(SwaggerDocument swaggerDoc, SchemaRegistry schemaRegistry, IApiExplorer apiExplorer)
        {
            IDictionary<string, PathItem> replacePaths = new ConcurrentDictionary<string, PathItem>();
            foreach (var item in swaggerDoc.paths)
            {
                string key = item.Key;
                if (key.Contains("/token"))
                {
                    replacePaths.Add(item.Key, item.Value);
                    continue;
                }
                var value = item.Value;
                var keys = key.Split('/');

                if (keys[3].IndexOf('.') != -1)
                {
                    // 区域路径
                    string areasName = keys[2];
                    string namespaceFullName = keys[3];
                    var directoryNames = namespaceFullName.Split('.');
                    string namespaceName = directoryNames[4];
                    if (areasName.Equals(namespaceName, StringComparison.OrdinalIgnoreCase))
                    {
                        string controllerName = directoryNames[6];
                        replacePaths.Add(
                            item.Key.Replace(namespaceFullName,
                                controllerName.Substring(0,
                                    controllerName.Length - DefaultHttpControllerSelector.ControllerSuffix.Length)),
                            value);
                    }
                }
                else if (keys[2].IndexOf('.') != -1)
                {
                    // 基础路径
                    string namespaceFullName = keys[2];
                    var directoryNames = namespaceFullName.Split('.');
                    bool isControllers = directoryNames[3].Equals("Controllers", StringComparison.OrdinalIgnoreCase);
                    string controllerName = directoryNames[4];
                    if (isControllers)
                    {
                        replacePaths.Add(
                        item.Key.Replace(namespaceFullName,
                            controllerName.Substring(0,
                                controllerName.Length - DefaultHttpControllerSelector.ControllerSuffix.Length)), value);
                    }
                }
            }
            swaggerDoc.paths = replacePaths;
        }
    }
}