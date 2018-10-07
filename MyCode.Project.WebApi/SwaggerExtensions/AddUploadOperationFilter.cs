using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Description;
using Swashbuckle.Swagger;

namespace MyCode.Project.WebApi.SwaggerExtensions
{
    /// <summary>
    /// 添加上传过滤
    /// </summary>
    public class AddUploadOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            var upload = apiDescription.ActionDescriptor.GetCustomAttributes<UploadAttribute>().Any();
            if (upload)
            {
                operation.consumes.Add("application/form-data");
                operation.parameters.Add(new Parameter()
                {
                    name = "file",
                    @in = "formData",
                    required = true,
                    type = "file"
                });
            }
        }
    }
}