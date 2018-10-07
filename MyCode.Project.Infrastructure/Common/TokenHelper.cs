using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCode.Project.Infrastructure.Exceptions;

namespace MyCode.Project.Infrastructure.Common
{
    public class TokenHelper
    {
        #region CreateToken(创建token)
        public static string CreateToken(string key ,string objName,object t,int expireMinute=300)
        {

            var payload = new Dictionary<string, object>
            {
                { "exp", DateTimeOffset.UtcNow.AddMinutes(expireMinute).ToUnixTimeSeconds() },
                { objName,t}
            };

            //采用HS256加密算法
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

            return encoder.Encode(payload, key);
        }
        #endregion

        #region Get(根据token得到登陆信息)
    public static object Get(string token,string tokenKey,string jsonKey)
    {
        try
        {
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IDateTimeProvider provider = new UtcDateTimeProvider();
            IJwtValidator validator = new JwtValidator(serializer, provider);
            IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder);

            string json = decoder.Decode(token, tokenKey, verify: true);

            var dic = decoder.DecodeToObject<Dictionary<string, object>>(token);

            return dic[jsonKey];
        }
        catch (TokenExpiredException ex)
        {
            throw new BaseException("请重新登陆，token已失效");
        }
        catch (SignatureVerificationException ex)
        {
            throw new BaseException("请重新登陆，签名错误");
        }
    }
        #endregion
    }
}
