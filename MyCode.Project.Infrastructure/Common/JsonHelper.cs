using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCode.Project.Infrastructure.Common
{
    public class JsonHelper
    {
        /// <summary>
        /// Json序列化器
        /// </summary>
        static readonly JsonSerializer JsonSerializer = new JsonSerializer();

        #region ToObject<T>(将Json字符串转换为对象)
        /// <summary>
        /// 将Json字符串转换为对象
        /// </summary>
        /// <param name="json">Json字符串</param>
        public static T ToObject<T>(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return default(T);
            return JsonConvert.DeserializeObject<T>(json);
        }
        #endregion

        #region ObjectToByte(将对象转换成字节数组)
        public static byte[] ObjectToByte(object obj)
        {

            var typeName = Reflection.GetTypeName(obj.GetType());

            using (var ms = new MemoryStream())
            {
                using (var tw = new StreamWriter(ms))
                {
                    using (var jw = new JsonTextWriter(tw))
                    {
                        jw.WriteStartArray();// [
                        jw.WriteValue(typeName);// "type"
                        JsonSerializer.Serialize(jw, obj);// obj

                        jw.WriteEndArray();// ]
                        jw.Flush();

                        return ms.ToArray();
                    }
                }
            }
        }
        #endregion

        #region ToObject(字节数组转成对象)
        /// <summary>
        /// 反序列化对象
        /// </summary>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static object ToObject(byte[] value)
        {

            using (var ms = new MemoryStream(value, writable: false))
            {
                using (var tr = new StreamReader(ms))
                {
                    using (var jr = new JsonTextReader(tr))
                    {
                        jr.Read();
                        if (jr.TokenType == JsonToken.StartArray)
                        {
                            // 读取类型
                            var typeName = jr.ReadAsString();
                            var type = Type.GetType(typeName, throwOnError: true);// 获取类型
                            // 读取对象
                            jr.Read();
                            return JsonSerializer.Deserialize(jr, type);
                        }
                        else
                        {
                            throw new InvalidDataException("JsonTranscoder 仅支持 [\"TypeName\", object]");
                        }
                    }
                }
            }
        }
        #endregion

        #region ToJson(将对象转换为Json字符串)
        /// <summary>
        /// 将对象转换为Json字符串
        /// </summary>
        /// <param name="target">目标对象</param>
        /// <param name="isConvertToSingleQuotes">是否将双引号转成单引号</param>
        public static string ToJson(object target, bool isConvertToSingleQuotes = false)
        {
            if (target == null)
                return "{}";
            var result = JsonConvert.SerializeObject(target);
            if (isConvertToSingleQuotes)
                result = result.Replace("\"", "'");
            return result;
        }
        #endregion
    }
}
