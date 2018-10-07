using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using MyCode.Project.Infrastructure.Common;

namespace MyCode.Project.Infrastructure.Extensions
{
    public static class JsonExtensions
    {
        public static bool IsNullOrEmpty(this JToken token)
        {
            return (token == null) ||
                   (token.Type == JTokenType.Array && !token.HasValues) ||
                   (token.Type == JTokenType.Object && !token.HasValues) ||
                   (token.Type == JTokenType.String && token.ToString() == String.Empty) ||
                   (token.Type == JTokenType.Null);
        }

        #region GetSingle(取得单个的数据)
        public static T GetSingle<T>(this JObject jObject,string propertyeName,string propertype2Name = "")
        { 

            var token = jObject[propertyeName];

            if((token.IsNullOrEmpty()))
            {
                return default(T);
            }

            if (!string.IsNullOrEmpty(propertype2Name))
            {
                token = token[propertype2Name];
            }

            if (token.IsNullOrEmpty())
            {
                return default(T);
            }

            return token.Value<T>();
        }
        #endregion

        #region GetList(取得列表数据，适用于字典的情况)
        public static List<T> GetList<T>(this JObject jObject, string propertyeeName)
        {
            var token = jObject[propertyeeName];

            if ((token.IsNullOrEmpty()))
            {
                return null;
            }

            return token.Values<T>().ToList();
        }
        #endregion

        #region 
        public static List<T> GetList<T>(this JObject jObject,string propertyeeName, string fieldName)
        {
            var sourceTags = jObject[propertyeeName].ToList();

            var listTags = new List<T>();

            foreach (var tag in sourceTags)
            {
                var name = tag[fieldName].Value<T>();

                listTags.Add(name);
            }

            return listTags;
        }
        #endregion

       
    }
}
