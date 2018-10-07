using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MyCode.Project.Infrastructure.WebPost
{
    public class RongCloudServer
    {
        private static String buildQueryStr(Dictionary<String, String> dicList)
        {
            String postStr = "";

            foreach (var item in dicList)
            {
                postStr += item.Key + "=" + HttpUtility.UrlEncode(item.Value, Encoding.UTF8) + "&";
            }
            postStr = postStr.Substring(0, postStr.LastIndexOf('&'));
            return postStr;
        }

        public static String GetToken(String appkey, String appSecret, String userId, String name, String portraitUri)
        {
            Dictionary<String, String> dicList = new Dictionary<String, String>();
            dicList.Add("userId", userId);
            dicList.Add("name", name);
            dicList.Add("portraitUri", portraitUri);

            String postStr = buildQueryStr(dicList);

            RongHttpClient client = new RongHttpClient(appkey, appSecret, InterfaceUrl.getTokenUrl, postStr);

            return client.ExecutePost();

        }
    }

    class InterfaceUrl
    {
        public static String server_addr = "http://api.cn.ronghub.com";

        public static String getTokenUrl = server_addr + "/user/getToken.json";
        public static String joinGroupUrl = server_addr + "/group/join.json";
        public static String quitGroupUrl = server_addr + "/group/quit.json";
        public static String dismissUrl = server_addr + "/group/dismiss.json";
        public static String syncGroupUrl = server_addr + "/group/sync.json";
        public static String sendMsgUrl = server_addr + "/message/publish.json";
        public static String sendSysMsgUrl = server_addr + "/message/system/publish.json";
        public static String broadcastUrl = server_addr + "/message/broadcast.json";
        public static String createChatroomUrl = server_addr + "/chatroom/create.json";
        public static String destroyChatroomUrl = server_addr + "/chatroom/destroy.json";
        public static String queryChatroomUrl = server_addr + "/chatroom/query.json";

        //public static String getTokenUrl = server_addr + "/user/getToken.xml";
        //public static String joinGroupUrl = server_addr + "/group/join.xml";
        //public static String quitGroupUrl = server_addr + "/group/quit.xml";
        //public static String dismissUrl = server_addr + "/group/dismiss.xml";
        //public static String syncGroupUrl = server_addr + "/group/sync.xml";
        //public static String SendMsgUrl = server_addr + "/message/publish.xml";
        //public static String sendSysMsgUrl = server_addr + "/message/system/publish.xml";
        //public static String broadcastUrl = server_addr + "/message/broadcast.xml";
        //public static String createChatroomUrl = server_addr + "/chatroom/create.xml";
        //public static String destroyChatroomUrl = server_addr + "/chatroom/destroy.xml";
        //public static String queryChatroomUrl = server_addr + "/chatroom/query.xml";

    }
}
