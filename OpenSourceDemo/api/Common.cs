using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;


namespace OpenSourceDemo.api
{
    public class Common
    {

        private static HttpWebResponse myResponse;

        private static HttpWebRequest myRequest;
        /// <summary>
        /// 获得Url或表单参数的值, 先判断Url参数是否为空字符串, 如为空则返回表单参数的值
        /// </summary>
        public static string GetString(string strName)
        {
            if ("".Equals(GetQueryString(strName)))
                return GetFormString(strName).Trim();
            else
                return GetQueryString(strName).Trim();
        }

        #region 表单传值
        /// <summary>
        /// 获得指定表单参数的值
        /// </summary>
        public static string GetFormString(string strName)
        {
            if (HttpContext.Current.Request.Form[strName] == null)
                return "";

            return HttpContext.Current.Request.Form[strName];
        }

        /// <summary>
        /// 获得指定表单参数的int类型值,缺省值0
        /// </summary>
        /// <param name="strName">表单参数</param>
        public static int GetFormInt(string strName)
        {
            return GetFormInt(strName, 0);
        }

        /// <summary>
        /// 获得指定表单参数的int类型值
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <param name="defValue">缺省值</param>
        public static int GetFormInt(string strName, int defValue)
        {
            return ConvertHelper.StrToInt(HttpContext.Current.Request.Form[strName], defValue);
        }

        /// <summary>
        /// 返回指定表单的参数值(String型)
        /// </summary>
        public static string GetFormStringValue(string strName)
        {
            return GetQueryStringValue(strName, string.Empty);
        }
        /// <summary>
        /// 返回指定表单的参数值(String型),如果该参数不存在则返回默认值
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <param name="defaultvalue">默认值</param>
        public static string GetFormStringValue(string strName, string defaultvalue)
        {
            if (HttpContext.Current.Request.Form[strName] == null || HttpContext.Current.Request.Form[strName].ToString() == string.Empty)
                return defaultvalue;
            else
            {
                Regex obj = new Regex("\\w+");
                Match objmach = obj.Match(HttpContext.Current.Request.Form[strName].ToString());
                if (objmach.Success)
                    return objmach.Value;
                else
                    return defaultvalue;
            }
        }

        /// <summary>
        /// 获得指定表单参数的decimal类型值
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <param name="defValue">缺省值</param>
        public static decimal GetFormDecimal(string strName, decimal defValue)
        {
            return ConvertHelper.StrToDecimal(HttpContext.Current.Request.Form[strName], defValue);
        }

        /// <summary>
        /// 获得指定表单参数的float类型值
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>表单参数的float类型值</returns>
        public static float GetFormFloat(string strName, float defValue)
        {
            return ConvertHelper.StrToFloat(HttpContext.Current.Request.Form[strName], defValue);
        }
        #endregion

        #region URL传值
        /// <summary>
        /// 获得指定Url参数的值
        /// </summary> 
        /// <param name="strName">Url参数</param>
        public static string GetQueryString(string strName)
        {
            if (HttpContext.Current.Request.QueryString[strName] == null)
                return "";

            return HttpContext.Current.Request.QueryString[strName];
        }

        /// <summary>
        /// 获得指定Url参数的int类型值,缺省值0
        /// </summary>
        /// <param name="strName">Url参数</param>
        public static int GetQueryInt(string strName)
        {
            return ConvertHelper.StrToInt(HttpContext.Current.Request.QueryString[strName], 0);
        }

        /// <summary>
        /// 获得指定Url参数的int类型值
        /// </summary>
        /// <param name="strName">Url参数</param>
        /// <param name="defValue">缺省值</param>
        public static int GetQueryInt(string strName, int defValue)
        {
            return ConvertHelper.StrToInt(HttpContext.Current.Request.QueryString[strName], defValue);
        }

        /// <summary>
        /// 返回指定URL的参数值(String型),如果不存在返回空
        /// </summary>
        /// <param name="strName">URL参数</param>
        public static string GetQueryStringValue(string strName)
        {
            return GetQueryStringValue(strName, string.Empty);
        }

        /// <summary>
        /// 返回指定URL的参数值(String型),如果该参数不存在则返回默认值
        /// </summary>
        /// <param name="strName">URL参数</param>
        /// <param name="defaultvalue">默认值</param>
        public static string GetQueryStringValue(string strName, string defaultvalue)
        {
            if (HttpContext.Current.Request.QueryString[strName] == null || HttpContext.Current.Request.QueryString[strName].ToString() == string.Empty)
                return defaultvalue;
            else
            {
                Regex obj = new Regex("\\w+");
                Match objmach = obj.Match(HttpContext.Current.Request.QueryString[strName].ToString());
                if (objmach.Success)
                    return objmach.Value;
                else
                    return defaultvalue;
            }
        }

        /// <summary>
        /// 获得指定Url参数的decimal类型值
        /// </summary>
        /// <param name="strName">Url参数</param>
        /// <param name="defValue">缺省值</param>
        public static decimal GetQueryDecimal(string strName, decimal defValue)
        {
            return ConvertHelper.StrToDecimal(HttpContext.Current.Request.QueryString[strName], defValue);
        }

        /// <summary>
        /// 获得指定Url参数的float类型值
        /// </summary>
        /// <param name="strName">Url参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>Url参数的int类型值</returns>
        public static float GetQueryFloat(string strName, float defValue)
        {
            return ConvertHelper.StrToFloat(HttpContext.Current.Request.QueryString[strName], defValue);
        }

        /// <summary>
        /// 获得指定Url参数的int类型值,缺省值0
        /// </summary>
        /// <param name="strName">Url参数</param>
        public static float GetQueryFloat(string strName)
        {
            return ConvertHelper.StrToFloat(HttpContext.Current.Request.QueryString[strName], 0);
        }

        #endregion

        #region 数据类型校验及转换操作类
        /// <summary>
        /// 数据类型校验及转换类
        /// </summary>
        public abstract class ConvertHelper
        {
            /// <summary>
            /// 获取安全返回值
            /// </summary>
            /// <param name="value">可空值</param>
            public static T SafeValue<T>(T? value) where T : struct
            {
                return value ?? default(T);
            }

            #region 数据类型判断
            /// <summary>
            /// 判断是否为Int32类型的数字
            /// </summary>
            /// <param name="Expression">要验证的字符串</param>
            public static bool IsNumeric(object obj)
            {
                if (obj != null)
                    return IsNumeric(ObjToStr(obj));

                return false;

            }
            /// <summary>
            /// 判断是否为Int32类型的数字
            /// </summary>
            /// <param name="Expression">要验证的字符串</param>
            public static bool IsNumeric(string expression)
            {
                if (expression != null)
                {
                    string str = expression;
                    if (str.Length > 0 && str.Length <= 11 && Regex.IsMatch(str, @"^[-]?[0-9]*[.]?[0-9]*$"))
                    {
                        if ((str.Length < 10) || (str.Length == 10 && str[0] == '1') || (str.Length == 11 && str[0] == '-' && str[1] == '1'))
                            return true;
                    }
                }
                return false;
            }

            /// <summary>
            /// 是否为Double类型
            /// </summary>
            /// <param name="expression">要验证的字符串</param>
            public static bool IsDouble(object obj)
            {
                if (obj != null)
                    return IsDouble(ObjToStr(obj));

                return false;
            }
            /// <summary>
            /// 是否为Double类型
            /// </summary>
            /// <param name="expression">要验证的字符串</param>
            public static bool IsDouble(string expression)
            {
                if (expression != null)
                    return Regex.IsMatch(expression, @"^([0-9])[0-9]*(\.\w*)?$");

                return false;
            }
            #endregion

            #region String转换成其他数据类型
            /// <summary>
            /// string型转换为bool型(为true时返回true，为false时返回false,其他则返回缺省值)
            /// </summary>
            /// <param name="strValue">要转换的字符串</param>
            /// <param name="defValue">缺省值</param>
            public static bool StrToBool(string expression, bool defValue)
            {
                if (expression != null)
                {
                    if (string.Compare(expression, "true", true) == 0)
                        return true;
                    else if (string.Compare(expression, "false", true) == 0)
                        return false;
                }
                return defValue;
            }
            /// <summary>
            /// string型转换为bool型，默认返回false
            /// </summary>
            /// <param name="strValue">要转换的字符串</param>
            public static bool StrToBool(string expression)
            {
                return StrToBool(expression, false);
            }

            /// <summary>
            /// 将字符串转换为Int32类型
            /// </summary>
            /// <param name="expression">要转换的字符串</param>
            /// <param name="defValue">缺省值</param>
            public static int StrToInt(string expression, int defValue)
            {
                if (string.IsNullOrEmpty(expression) || expression.Trim().Length >= 11 || !Regex.IsMatch(expression.Trim(), @"^([-]|[0-9])[0-9]*(\.\w*)?$"))
                    return defValue;

                int rv;
                if (Int32.TryParse(expression, out rv))
                    return rv;

                return Convert.ToInt32(StrToFloat(expression, defValue));
            }
            /// <summary>
            /// 将字符串转换为Int32类型(转换失败则返回0)
            /// </summary>
            /// <param name="expression">要转换的字符串</param>
            public static int StrToInt(string expression)
            {
                return StrToInt(expression, 0);
            }

            /// <summary>
            /// string型转换为decimal型
            /// </summary>
            /// <param name="strValue">要转换的字符串</param>
            /// <param name="defValue">缺省值</param>
            public static decimal StrToDecimal(string expression, decimal defValue)
            {
                if ((expression == null) || (expression.Length > 10))
                    return defValue;

                decimal intValue = defValue;
                if (expression != null)
                {
                    bool IsDecimal = Regex.IsMatch(expression, @"^([-]|[0-9])[0-9]*(\.\w*)?$");
                    if (IsDecimal)
                        decimal.TryParse(expression, out intValue);
                }
                return intValue;
            }
            /// <summary>
            /// string型转换为decimal型(转换失败则返回0)
            /// </summary>
            /// <param name="strValue">要转换的字符串</param>
            public static decimal StrToDecimal(string expression)
            {
                return StrToDecimal(expression, 0);
            }

            /// <summary>
            /// string型转换为float型
            /// </summary>
            /// <param name="strValue">要转换的字符串</param>
            /// <param name="defValue">缺省值</param>
            public static float StrToFloat(string expression, float defValue)
            {
                if ((expression == null) || (expression.Length > 10))
                    return defValue;

                float intValue = defValue;
                if (expression != null)
                {
                    bool IsFloat = Regex.IsMatch(expression, @"^([-]|[0-9])[0-9]*(\.\w*)?$");
                    if (IsFloat)
                        float.TryParse(expression, out intValue);
                }
                return intValue;
            }
            /// <summary>
            /// string型转换为float型(转换失败则返回0)
            /// </summary>
            /// <param name="strValue">要转换的字符串</param>
            public static float StrToFloat(string expression)
            {
                return StrToFloat(expression, 0);
            }

            /// <summary>
            /// 将对象转换为日期时间类型
            /// </summary>
            /// <param name="str">要转换的字符串</param>
            /// <param name="defValue">缺省值</param>
            public static DateTime StrToDateTime(string str, DateTime defValue)
            {
                if (!string.IsNullOrEmpty(str))
                {
                    DateTime dateTime;
                    if (DateTime.TryParse(str, out dateTime))
                        return dateTime;
                }
                return defValue;
            }
            /// <summary>
            /// 将对象转换为日期时间类型(转换失败则返回当前时间)
            /// </summary>
            /// <param name="str">要转换的字符串</param>
            public static DateTime StrToDateTime(string str)
            {
                return StrToDateTime(str, DateTime.Now);
            }

            #endregion

            #region Object转换成其他数据类型
            /// <summary>
            /// 将对象转换为字符串
            /// </summary>
            /// <param name="obj">要转换的对象</param>
            public static string ObjToStr(object obj)
            {
                try
                {
                    if (obj == null) return "";
                    else return obj.ToString();
                }
                catch (Exception ex)
                {
                    return "";
                }
            }

            /// <summary>
            /// object型转换为bool型(为true时返回true，为false时返回false,其他则返回缺省值)
            /// </summary>
            /// <param name="strValue">要转换的字符串</param>
            /// <param name="defValue">缺省值</param>
            public static bool ObjToBool(object obj, bool defValue)
            {
                if (obj != null)
                    return StrToBool(ObjToStr(obj), defValue);

                return defValue;
            }

            /// <summary>
            /// 将对象转换为Int32类型
            /// </summary>
            /// <param name="expression">要转换的字符串</param>
            /// <param name="defValue">缺省值</param>
            public static int ObjToInt(object obj, int defValue)
            {
                if (obj != null)
                    return StrToInt(ObjToStr(obj), defValue);

                return defValue;
            }
            /// <summary>
            /// 将对象转换为Int32类型
            /// </summary>
            /// <param name="expression">要转换的字符串</param>
            public static int ObjToInt(object obj)
            {
                if (obj != null)
                    return StrToInt(ObjToStr(obj), 0);

                return 0;
            }

            /// <summary>
            /// Object型转换为decimal型
            /// </summary>
            /// <param name="strValue">要转换的字符串</param>
            /// <param name="defValue">缺省值</param>
            public static decimal ObjToDecimal(object obj, decimal defValue)
            {
                if (obj != null)
                    return StrToDecimal(ObjToStr(obj), defValue);

                return defValue;
            }
            /// <summary>
            /// 将对象转换为decimal类型
            /// </summary>
            /// <param name="expression">要转换的字符串</param>
            public static decimal ObjToDecimal(object obj)
            {
                if (obj != null)
                    return StrToDecimal(ObjToStr(obj), 0);

                return 0;
            }

            /// <summary>
            /// Object型转换为float型
            /// </summary>
            /// <param name="strValue">要转换的字符串</param>
            /// <param name="defValue">缺省值</param>
            public static float ObjToFloat(object obj, float defValue)
            {
                if (obj != null)
                    return StrToFloat(ObjToStr(obj), defValue);

                return defValue;
            }
            /// <summary>
            /// 将对象转换为float类型
            /// </summary>
            /// <param name="expression">要转换的字符串</param>
            public static float ObjToFloat(object obj)
            {
                if (obj != null)
                    return StrToFloat(ObjToStr(obj), 0);

                return 0;
            }

            /// <summary>
            /// 将对象转换为日期时间类型
            /// </summary>
            /// <param name="obj">要转换的对象</param>
            public static DateTime ObjToDateTime(object obj)
            {
                return StrToDateTime(ObjToStr(obj));
            }
            /// <summary>
            /// 将对象转换为日期时间类型
            /// </summary>
            /// <param name="obj">要转换的对象</param>
            /// <param name="defValue">缺省值</param>
            public static DateTime ObjToDateTime(object obj, DateTime defValue)
            {
                return StrToDateTime(ObjToStr(obj), defValue);
            }
            #endregion

            #region 清除HTML标记
            /// <summary>
            /// 清除HTML标记
            /// </summary>
            /// <param name="Htmlstring">HTML字符串</param>
            public static string DropHTML(string Htmlstring)
            {
                if (string.IsNullOrEmpty(Htmlstring)) return "";
                //删除脚本  
                Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
                //删除HTML  
                Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);

                Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);
                Htmlstring.Replace("<", "");
                Htmlstring.Replace(">", "");
                Htmlstring.Replace("\r\n", "");
                Htmlstring.Replace("&emsp;", "");
                Htmlstring = HttpContext.Current.Server.HtmlEncode(Htmlstring).Trim();
                return Htmlstring;
            }
            #endregion

            #region TXT代码转换成HTML格式
            /// <summary>
            /// 字符串字符处理
            /// </summary>
            /// <param name="chr">等待处理的字符串</param>
            public static String ToHtml(string Input)
            {
                StringBuilder sb = new StringBuilder(Input);
                sb.Replace("&", "&amp;");
                sb.Replace("<", "&lt;");
                sb.Replace(">", "&gt;");
                sb.Replace("\r\n", "<br />");
                sb.Replace("\n", "<br />");
                sb.Replace("\t", " ");
                //sb.Replace(" ", "&nbsp;");
                return sb.ToString();
            }
            #endregion

            #region HTML代码转换成TXT格式
            /// <summary>
            /// 字符串字符处理
            /// </summary>
            /// <param name="chr">等待处理的字符串</param>
            public static String ToTxt(String Input)
            {
                StringBuilder sb = new StringBuilder(Input);
                sb.Replace("&nbsp;", " ");
                sb.Replace("<br>", "\r\n");
                sb.Replace("<br>", "\n");
                sb.Replace("<br />", "\n");
                sb.Replace("<br />", "\r\n");
                sb.Replace("&lt;", "<");
                sb.Replace("&gt;", ">");
                sb.Replace("&amp;", "&");
                return sb.ToString();
            }
            #endregion

            #region 人民币转中文
            /// <summary> 
            /// 人民币转中文 
            /// </summary> 
            /// <param name="num">金额</param> 
            /// <returns>返回大写形式</returns> 
            public static string CmycurD(decimal num)
            {
                string str1 = "零壹贰叁肆伍陆柒捌玖";            //0-9所对应的汉字 
                string str2 = "万仟佰拾亿仟佰拾万仟佰拾元角分"; //数字位所对应的汉字 
                string str3 = "";    //从原num值中取出的值 
                string str4 = "";    //数字的字符串形式 
                string str5 = "";  //人民币大写金额形式 
                int i;    //循环变量 
                int j;    //num的值乘以100的字符串长度 
                string ch1 = "";    //数字的汉语读法 
                string ch2 = "";    //数字位的汉字读法 
                int nzero = 0;  //用来计算连续的零值是几个 
                int temp;            //从原num值中取出的值 

                num = Math.Round(Math.Abs(num), 2);    //将num取绝对值并四舍五入取2位小数 
                str4 = ((long)(num * 100)).ToString();        //将num乘100并转换成字符串形式 
                j = str4.Length;      //找出最高位 
                if (j > 15) { return "溢出"; }
                str2 = str2.Substring(15 - j);   //取出对应位数的str2的值。如：200.55,j为5所以str2=佰拾元角分 

                //循环取出每一位需要转换的值 
                for (i = 0; i < j; i++)
                {
                    str3 = str4.Substring(i, 1);          //取出需转换的某一位的值 
                    temp = Convert.ToInt32(str3);      //转换为数字 
                    if (i != (j - 3) && i != (j - 7) && i != (j - 11) && i != (j - 15))
                    {
                        //当所取位数不为元、万、亿、万亿上的数字时 
                        if (str3 == "0")
                        {
                            ch1 = "";
                            ch2 = "";
                            nzero = nzero + 1;
                        }
                        else
                        {
                            if (str3 != "0" && nzero != 0)
                            {
                                ch1 = "零" + str1.Substring(temp * 1, 1);
                                ch2 = str2.Substring(i, 1);
                                nzero = 0;
                            }
                            else
                            {
                                ch1 = str1.Substring(temp * 1, 1);
                                ch2 = str2.Substring(i, 1);
                                nzero = 0;
                            }
                        }
                    }
                    else
                    {
                        //该位是万亿，亿，万，元位等关键位 
                        if (str3 != "0" && nzero != 0)
                        {
                            ch1 = "零" + str1.Substring(temp * 1, 1);
                            ch2 = str2.Substring(i, 1);
                            nzero = 0;
                        }
                        else
                        {
                            if (str3 != "0" && nzero == 0)
                            {
                                ch1 = str1.Substring(temp * 1, 1);
                                ch2 = str2.Substring(i, 1);
                                nzero = 0;
                            }
                            else
                            {
                                if (str3 == "0" && nzero >= 3)
                                {
                                    ch1 = "";
                                    ch2 = "";
                                    nzero = nzero + 1;
                                }
                                else
                                {
                                    if (j >= 11)
                                    {
                                        ch1 = "";
                                        nzero = nzero + 1;
                                    }
                                    else
                                    {
                                        ch1 = "";
                                        ch2 = str2.Substring(i, 1);
                                        nzero = nzero + 1;
                                    }
                                }
                            }
                        }
                    }
                    if (i == (j - 11) || i == (j - 3))
                    {
                        //如果该位是亿位或元位，则必须写上 
                        ch2 = str2.Substring(i, 1);
                    }
                    str5 = str5 + ch1 + ch2;

                    if (i == j - 1 && str3 == "0")
                    {
                        //最后一位（分）为0时，加上“整” 
                        str5 = str5 + '整';
                    }
                }
                if (num == 0)
                {
                    str5 = "零元整";
                }
                return str5;
            }
            /// <summary> 
            /// 人民币转中文
            /// </summary> 
            /// <param name="num">金额</param> 
            /// <returns>返回大写形式</returns>  
            public static string CmycurD(string numstr)
            {
                try
                {
                    decimal num = Convert.ToDecimal(numstr);
                    return CmycurD(num);
                }
                catch
                {
                    return "非数字形式！";
                }
            }
            #endregion

        }
        #endregion

        #region HTTP请求
        #region Get请求
        public static string Get(string uri, string BaseUri, string token)
        {
            try
            {
                //先根据用户请求的uri构造请求地址
                string serviceUrl = string.Format("{0}/{1}", BaseUri, uri);
                //创建Web访问对  象

                myRequest = (HttpWebRequest)WebRequest.Create(serviceUrl);
                if (!string.IsNullOrEmpty(token))
                {
                    myRequest.Headers["x-acs-dingtalk-access-token"] = token;
                }
                myResponse = (HttpWebResponse)myRequest.GetResponse();
                //通过响应内容流创建StreamReader对象，因为StreamReader更高级更快
                StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
                //string returnXml = HttpUtility.UrlDecode(reader.ReadToEnd());//如果有编码问题就用这个方法
                string returnJson = reader.ReadToEnd();//利用StreamReader就可以从响应内容从头读到尾
                reader.Close();
                myResponse.Close();
                return returnJson;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion

        #region Post请求
        public static string Post(string data, string uri, string BaseUri, string token)
        {
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(ValidateServerCertificate);
                //先根据用户请求的uri构造请求地址
                string serviceUrl = string.Format("{0}/{1}", BaseUri, uri);
                //创建Web访问对象
                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(serviceUrl);
                //把用户传过来的数据转成“UTF-8”的字节流
                byte[] buf = System.Text.Encoding.GetEncoding("UTF-8").GetBytes(data);

                myRequest.Method = "POST";
                myRequest.ContentLength = buf.Length;
                myRequest.ContentType = "application/json";
                myRequest.MaximumAutomaticRedirections = 1;
                myRequest.AllowAutoRedirect = true;
                myRequest.Headers["x-acs-dingtalk-access-token"] = token;
                //发送请求
                Stream stream = myRequest.GetRequestStream();
                stream.Write(buf, 0, buf.Length);
                stream.Close();

                //获取接口返回值
                //通过Web访问对象获取响应内容
                HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();
                //通过响应内容流创建StreamReader对象，因为StreamReader更高级更快
                StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
                //string returnXml = HttpUtility.UrlDecode(reader.ReadToEnd());//如果有编码问题就用这个方法
                string returnJson = reader.ReadToEnd();//利用StreamReader就可以从响应内容从头读到尾
                reader.Close();
                myResponse.Close();
                return returnJson;
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        #endregion
        #endregion

        private static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
        
    }
}