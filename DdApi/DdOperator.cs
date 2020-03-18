using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DdApi.Model;
using DdApi.Util;
using DingTalk.Api;
using DingTalk.Api.Request;
using DingTalk.Api.Response;

namespace DdApi
{
    public class DdOperator : IDdOper
    {
        private static DdOperator _instance = null; //单列对象
        private string accessToken, jsApiTicket;
        //private string jsApiTicket { get; set; }
        private DateTime accessTokenTime = DateTime.Parse("1900-01-01");
        public List<Dept> DeptList { get; set; } = new List<Dept>();
        /// <summary>
        /// 设置应用ID
        /// </summary>
        public string AgentID { get; set; }
        /// <summary>
        /// 设置企业ID
        /// </summary>
        public string CorpId { get; set; }
        /// <summary>
        /// 设置企业密钥
        /// </summary>
        public string CorpSecret { get; set; }
        /// <summary>
        /// 应用程序key
        /// </summary>
        public string AppKey { get; set; }
        /// <summary>
        /// 应用程序密钥
        /// </summary>
        public string AppSecret { get; set; }
    
        private DdOperator()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
            System.Net.ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, error) =>
            {
                return true;
            };

        }

        ///取得单例类实例
        public static DdOperator GetDdOperator()
        {
            if (_instance == null)
            {
                _instance = new DdOperator();
            }
            return _instance;
        }

        /// <summary>
        /// 返回AccessToken
        /// </summary>
        public string AccessToken
        {
            get
            {
                TimeSpan ts = DateTime.Now - accessTokenTime;
                if (ts.TotalSeconds < 7000 & accessToken != "")
                {
                    return accessToken; //在有效期内，直接返回
                }
                else
                {
                    GetAccessTokenAndJsTicket();  //重新生成并返回
                    return accessToken;
                }
            }
        }

        /// <summary>
        /// 返回钉钉免登签名
        /// </summary>
        /// <param name="nonceStr"></param>
        /// <param name="timeStamp"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public string GetSignature(string nonceStr, string timeStamp, string url)
        {
            string str1 = string.Format("jsapi_ticket={0}&noncestr={1}&timestamp={2}&url={3}", JsApiTicket, nonceStr, timeStamp, url);
            string signature = SH1Helper.GetSwcSH1(str1).ToLower();
            return signature;
        }

        /// <summary>
        /// 返回jsapiticket
        /// </summary>
        public string JsApiTicket
        {
            get
            {
                TimeSpan ts = DateTime.Now - accessTokenTime;
                if (ts.TotalSeconds < 6000)
                {
                    return jsApiTicket; //在有效期内，直接返回
                }
                else
                {
                    GetAccessTokenAndJsTicket();  //重新生成并返回
                    return jsApiTicket;
                }
            }
        }

      
        /// <summary>
        /// 生成accessToken
        /// </summary>
        private void GetAccessTokenAndJsTicket()
        {
            accessToken = GetAccessToken();
            jsApiTicket = GetJsApiTicket(accessToken);
            accessTokenTime = DateTime.Now;

        }

        /// <summary>
        /// 生成JsAipTicket
        /// </summary>
        /// <param name="_accessToken"></param>
        /// <returns></returns>
        private string GetJsApiTicket(string _accessToken)
        {
            DefaultDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/get_jsapi_ticket");
            OapiGetJsapiTicketRequest req = new OapiGetJsapiTicketRequest();
            req.SetHttpMethod("GET");
            OapiGetJsapiTicketResponse response = client.Execute(req, _accessToken);
            if (response.Errcode == 0)
            {
                return response.Ticket;
            }
            else
            {
                throw new Exception("获取jsTicket错误:"+response.ErrMsg);
            }
         
        }
       
               
        /// <summary>
        /// 根据企业尖,和密码返回 AccessToken
        /// </summary>
        /// <param name="CorpId"></param>
        /// <param name="CorpSecret"></param>
        /// <returns></returns>
        private string GetAccessToken()
        {

            IDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/gettoken");
            OapiGettokenRequest request = new OapiGettokenRequest();
            request.Appkey = AppKey;
            request.Appsecret = AppSecret;
            request.SetHttpMethod("GET");
            OapiGettokenResponse response = client.Execute(request);
            return response.AccessToken;
        }

        /// <summary>
        /// 生成时间戳
        /// </summary>
        /// <returns></returns>
        public string TimeStamp()
        {
            DateTime dt1 = Convert.ToDateTime("1970-01-01 00:00:00");
            TimeSpan ts = DateTime.Now - dt1;
            return Math.Ceiling(ts.TotalSeconds).ToString();
        }


        /// <summary>
        /// 根据部门ID取得部部门名字
        /// </summary>
        /// <param name="DeptId"></param>
        /// <returns></returns>
        public string GetDeptName(long DeptId)
        {
            var re = DeptList.SingleOrDefault(p => p.DeptID == DeptId);
            if (re != null) return re.DeptName;
            return "";
        }
        /// <summary>
        /// 设置部门
        /// </summary>
        /// <param name="depts"></param>
        public void SetDept(List<Dept> depts)
        {
            DeptList.Clear();
            depts.ForEach(p => DeptList.Add(p));
        }

    }
}
