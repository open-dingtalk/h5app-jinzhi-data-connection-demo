using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Configuration;
using System.Web.SessionState;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections;

namespace OpenSourceDemo.api
{
    /// <summary>
    /// DataManage 的摘要说明
    /// </summary>
    public class DataManage : IHttpHandler, IRequiresSessionState
    {
        private HttpContext _context;

        string basePath = ConfigurationManager.AppSettings["basePath"];

        private Dictionary<string, string> dicGetNextData = new Dictionary<string, string>();

        /// <summary>
        /// 统一调用webapi接口的处理程序
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {
            //从返回值中，取下一级数据的字段
            dicGetNextData.Add("data_userid", "title");
            dicGetNextData.Add("kh_pkhid", "title");
            dicGetNextData.Add("kh_yopuser", "title");
            dicGetNextData.Add("ht_xshid", "title");
            dicGetNextData.Add("ht_customerid", "title");
            dicGetNextData.Add("ht_wesub", "title");
            dicGetNextData.Add("ht_preside", "title");
            dicGetNextData.Add("fh_shipper", "title");
            dicGetNextData.Add("fh_htorder", "title");
            dicGetNextData.Add("fh_customerid", "title");
            dicGetNextData.Add("xsh_customerid", "title");
            dicGetNextData.Add("bj_customerid", "title");
            dicGetNextData.Add("bj_bjren", "title");
            dicGetNextData.Add("gysid", "title");

            string action = Common.GetString("action");
            this._context = context;

            switch (action)
            {
                case "login":
                    Login();
                    break;
                case "GetDataList":
                    GetDataList();
                    break;
                case "GetTitle":
                    GetTitle();
                    break;
                case "UpdateData":
                    UpdateData();
                    break;
                case "GetDataInfo":
                    GetDataInfo();
                    break;
                case "GetUserList":
                    GetUserList();
                    break;
            }
        }


        public void Login()
        {
            JObject ret = GetToken();

            this._context.Response.Write(ret);
            this._context.Response.End();
        }

        private JObject GetToken()
        {
            string appkey = ConfigurationManager.AppSettings["appkey"];
            string appsecret = ConfigurationManager.AppSettings["appsecret"];
            string uri = "gettoken?appkey=" + appkey + "&appsecret=" + appsecret;
            string ret = Common.Get(uri, "https://oapi.dingtalk.com", null);
            JObject jret = (JObject)JsonConvert.DeserializeObject(ret);

            if (jret["errcode"].ToString() == "0")
            {
                _context.Session["token"] = jret["access_token"].ToString();
                _context.Session.Timeout = 120;
            }

            return jret;
        }

        private JObject GetUserList()
        {
            string token = string.Empty;
            if (_context.Session["token"] == null)
            {
                JObject jToken = GetToken();
                if (jToken["errcode"].ToString() != "0")
                {
                    this._context.Response.Write(new JObject { });
                    this._context.Response.End();
                }
            }
            token = _context.Session["token"].ToString();

            string uri = "topapi/user/listsimple?access_token=" + token;
            string data = "{\"dept_id\":1,\"cursor\":0,\"size\":10}";
            string ret = Common.Post(data, uri, "https://oapi.dingtalk.com", token);

            JObject jret = (JObject)JsonConvert.DeserializeObject(ret);
            return jret;
        }

        /// <summary>
        /// 时间戳计时开始时间
        /// </summary>
        private static DateTime timeStampStartTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// DateTime转换为10位时间戳（单位：秒）
        /// </summary>
        /// <param name="dateTime"> DateTime</param>
        /// <returns>10位时间戳（单位：秒）</returns>
        public static long DateTimeToTimeStamp(DateTime dateTime)
        {
            return (long)(dateTime.ToUniversalTime() - timeStampStartTime).TotalSeconds;
        }

        private void GetDataList()
        {

            string formKey = Common.GetString("formKey");
            string page = Common.GetString("page");
            string pagesize = Common.GetString("pagesize");
            JObject retData = GetData(formKey, page, pagesize);

            this._context.Response.Write(retData);
            this._context.Response.End();
        }

        private JObject GetData(string formKey, string page, string pagesize)
        {
            string token = string.Empty;
            if (_context.Session["token"] == null)
            {
                JObject jToken = GetToken();
                if (jToken["errcode"].ToString() != "0")
                {
                    this._context.Response.Write(new JObject { });
                    this._context.Response.End();
                }
            }
            token = _context.Session["token"].ToString();

            string uri = "v1.0/jzcrm/data?datatype=" + formKey + "&page=" + page + "&pagesize=" + pagesize;

            string ret = Common.Get(uri, basePath, token);

            JObject data = (JObject)JsonConvert.DeserializeObject(ret);
            JObject retData = new JObject();

            if (data["dataname"] != null)
            {
                #region 获取列表数据
                JObject dataList = new JObject();
                JArray jData = new JArray();

                dataList["code"] = 0;
                dataList["msg"] = "";
                dataList["count"] = Convert.ToInt32(data["totalCount"]);
                JArray socData = (JArray)data["data"];
                for (int i = 0; i < socData.Count; i++)
                {
                    JObject jaData = new JObject();
                    JObject detail = (JObject)socData[i]["detail"];
                    foreach (JProperty item in detail.Properties())
                    {
                        string key = item.Name;
                        string value = "";
                        if (dicGetNextData.ContainsKey(key))
                        {
                            JObject jnextValue = (JObject)item.Value;
                            value = jnextValue[dicGetNextData[key]].ToString();
                        }
                        else
                        {
                            value = item.Value.ToString();
                        }
                        jaData[key] = value;
                    }
                    jData.Add(jaData);
                }
                dataList["data"] = jData;
                retData["dataList"] = dataList;
                #endregion

                #region 获取表头数据
                JArray titleList = new JArray();
                titleList.Add(new JObject() { { "title", "操作/状态" }, { "minWidth", 150 }, { "toolbar", "#currentTableBar" }, { "align", "center" } });
                JObject jSoruTitle = (JObject)data["dataname"];
                foreach (JProperty item in jSoruTitle.Properties())
                {
                    JObject jTitle = new JObject() { { "field", item.Name }, { "width", 150 }, { "title", item.Value } };
                    titleList.Add(jTitle);
                }
                retData["headList"] = titleList;
                retData["code"] = 0;
                #endregion
            }
            else
            {
                retData["code"] = 1;
            }

            return retData;
        }

        /// <summary>
        /// 读取JSON文件
        /// </summary>
        /// <param name="key">JSON文件中的key值</param>
        /// <returns>JSON文件中的value值</returns>
        public JObject Readjson(string path)
        {
            string jsonfile = path;//JSON文件路径

            using (System.IO.StreamReader file = System.IO.File.OpenText(jsonfile))
            {
                using (JsonTextReader reader = new JsonTextReader(file))
                {
                    JObject o = (JObject)JToken.ReadFrom(reader);
                    return o;
                }
            }
        }

        public void UpdateData()
        {
            string formKey = Common.GetString("formKey");
            string msgid = Common.GetString("msgid");
            string data = Common.GetString("data");

            JObject jdata = (JObject)JsonConvert.DeserializeObject(data);

            if (formKey == "154")
            {
                //{"stamp":"1641817098","datatype":"154","data":{"data_userid":"","cp_parentid":"","cpname":"请问11","typeid":"","cpno":"","cparea":"","cpbrand":"","cbprice":"","issnmanage":"","ispicimanage":"","gysid":"","cpimg":"","cpunit":"个","unitrate":"1","isstock":"","cpbarcode":"","cptype":"","cpguige":""}}
                jdata["cp_parentid"] = "";
                jdata["typeid"] = "";
                jdata["cpno"] = "";
                jdata["cparea"] = "";
                jdata["cpbrand"] = "";
                jdata["cbprice"] = "";
                jdata["issnmanage"] = "";
                jdata["ispicimanage"] = "";
                jdata["gysid"] = "";
                jdata["cpimg"] = "";
                jdata["isstock"] = "";
                jdata["cpbarcode"] = "";
                jdata["cptype"] = "";
                jdata["cpguige"] = "";
            }

            JObject requestData = new JObject();
            requestData["datatype"] = formKey;
            requestData["stamp"] = DateTimeToTimeStamp(DateTime.Now);
            requestData["msgid"] = msgid;
            requestData["data"] = jdata;

            string token = string.Empty;
            if (_context.Session["token"] == null)
            {
                JObject jToken = GetToken();
                if (jToken["errcode"].ToString() != "0")
                {
                    this._context.Response.Write(new JObject { });
                    this._context.Response.End();
                }
            }
            token = _context.Session["token"].ToString();

            string updataInterfaceName = ConfigurationManager.AppSettings[formKey];

            string uri = "v1.0/jzcrm/" + updataInterfaceName;
            string dataStr = requestData.ToString();
            string ret = Common.Post(dataStr, uri, basePath, token);

            this._context.Response.Write(ret);
            this._context.Response.End();
        }


        public void GetDataInfo()
        {
            string formKey = Common.GetString("formKey");
            string msgid = Common.GetString("msgid");
            JObject jret = GetDataInfoMx(formKey, msgid);

            JObject details = (JObject)jret["data"]["detail"];
            JArray newDetails = new JArray();

            foreach (JProperty item in details.Properties())
            {
                if (dicGetNextData.ContainsKey(item.Name))
                {
                    JObject nextData = (JObject)item.Value;
                    JObject detail = new JObject() { { "label", item.Name }, { "value", nextData[dicGetNextData[item.Name]] } };
                    newDetails.Add(detail);
                }
                else
                {
                    JObject detail = new JObject() { { "label", item.Name }, { "value", item.Value } };
                    newDetails.Add(detail);
                }
            }
            jret["data"]["detail"] = newDetails;

            this._context.Response.Write(jret);
            this._context.Response.End();
        }

        private JObject GetDataInfoMx(string formKey, string msgid)
        {
            string token = string.Empty;
            if (_context.Session["token"] == null)
            {
                JObject jToken = GetToken();
                if (jToken["errcode"].ToString() != "0")
                {
                    this._context.Response.Write(new JObject { });
                    this._context.Response.End();
                }
            }
            token = _context.Session["token"].ToString();

            string uri = "v1.0/jzcrm/dataView?datatype=" + formKey + "&msgid=" + msgid;
            string ret = Common.Get(uri, basePath, token);

            JObject jret = (JObject)JsonConvert.DeserializeObject(ret);
            return jret;
        }

        public void GetTitle()
        {
            JObject userList = GetUserList();
            JArray juser = (JArray)userList["result"]["list"];
            JArray jselectUser = new JArray();

            for (int i = 0; i < juser.Count; i++)
            {
                jselectUser.Add(new JObject { { "label", juser[i]["userid"].ToString() }, { "value", juser[i]["name"].ToString() } });
            }

            JObject custList = GetData("148", "1", "100");
            JArray jcust = (JArray)custList["dataList"]["data"];
            JArray jselectCust = new JArray();
            for (int i = 0; i < jcust.Count; i++)
            {
                jselectCust.Add(new JObject { { "label", jcust[i]["kh_id"].ToString() }, { "value", jcust[i]["kh_name"].ToString() } });
            }

            string formKey = Common.GetString("formKey");
            string msgid = Common.GetString("msgid");
            string opType = Common.GetString("optype");

            JObject thisDataList = null;
            if (opType == "edit")
            {
                thisDataList = GetDataInfoMx(formKey, msgid);
            }

            JObject ret = Readjson(System.AppDomain.CurrentDomain.BaseDirectory + @"api/" + formKey + ".json");

            for (int i = 0; i < ret["dataname"].Count(); i++)
            {
                if (ret["dataname"][i]["label"].ToString() == "data_userid" || ret["dataname"][i]["label"].ToString() == "ht_preside" || ret["dataname"][i]["label"].ToString() == "bj_bjren")
                {
                    ret["dataname"][i]["option"] = jselectUser;
                }
                else if (ret["dataname"][i]["label"].ToString() == "ht_customerid" || ret["dataname"][i]["label"].ToString() == "fh_customerid" || ret["dataname"][i]["label"].ToString() == "xsh_customerid" || ret["dataname"][i]["label"].ToString() == "bj_customerid" || ret["dataname"][i]["label"].ToString() == "gysid")
                {
                    ret["dataname"][i]["option"] = jselectCust;
                }

                if (opType == "edit")
                {
                    string zdname = ret["dataname"][i]["label"].ToString();
                    string valueName = "";
                    if (dicGetNextData.ContainsKey(zdname))
                    {
                        valueName = thisDataList["data"]["detail"][zdname][dicGetNextData[zdname]].ToString();
                    }
                    else
                    {
                        valueName = thisDataList["data"]["detail"][zdname].ToString();
                    }
                    ret["dataname"][i]["defVal"] = valueName;
                }
            }
            this._context.Response.Write(ret);
            this._context.Response.End();
        }



        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}