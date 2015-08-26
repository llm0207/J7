using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace J7.Utility
{
    /// <summary>
    /// 文本工具类
    /// </summary>
    public class TextUtility
    {

        /// <summary>
        /// 将Unix时间戳格式转换为DateTime时间格式
        /// </summary>
        /// <param name="timeStamp">Unix时间戳</param>
        /// <returns></returns>
        public static DateTime TimeSpanToDateTime(int timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp.ToString() + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }

        /// <summary>
        /// 将DateTime时间格式转换为Unix时间戳格式
        /// </summary>
        /// <param name="time">DateTime时间格式</param>
        /// <returns></returns> 
        public static int DateTimeToTimeSpan(DateTime time)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }

        /// <summary>
        /// 防注入检测，用于验证 SQL 语句中的参数值
        /// </summary>
        /// <param name="input">SQL 语句中的参数值</param>
        /// <returns></returns>
        public static string SqlFilter(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return "";
            }

            input = input.ToLower();
            string[] immit = { "'", "%24", "%27", "%3a", "%3b", "%3c", ";", "'", "-", "*", "\\", "&", ";", "{", "}", "(", ")", "--", "update", "delete", "create", "alter", "drop", "exec", "insert", "truncate", "and", "or", "count", "master", "declare" };
            for (int i = 0; i < immit.Length; i++)
            {
                input = input.Replace(immit[i], "");
            }
            return input;
        }

        /// <summary>
        /// 将字符串中的部分字符用*号代替
        /// </summary>
        /// <param name="input"></param>
        /// <param name="length">左右保留的字符长度</param>
        /// <returns></returns>
        public static string StringToAsterisk(string input, int length = 1)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return "";
            }

            string result = "";
            if (input.Length >= (length * 2 + 1))
            {
                result = input.Substring(0, length).PadRight(input.Length - length, '*') + input.Substring(input.Length - length);
            }
            else if (input.Length > length)
            {
                result = input.Substring(0, length);
                result = result.PadRight(input.Length, '*');
            }
            else
            {
                result = input;
            }

            return result;
        }

        /// <summary>
        /// 生成随机字符串，一般用于随机生成验证码
        /// </summary>
        /// <param name="length">验证码长度</param>
        /// <returns>随机字符串</returns>
        public static string GenerateRandomNumber(int length)
        {
            char[] constant = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
            StringBuilder result = new System.Text.StringBuilder(62);
            Random rd = new Random();
            for (int i = 0; i < length; i++)
            {
                result.Append(constant[rd.Next(62)]);
            }
            return result.ToString();
        }
        /// <summary>
        /// 获取帖子的发布时间间隔
        /// </summary>
        /// <param name="publishTime"></param>
        /// <returns></returns>
        public static string GetPublishTimeFormat(DateTime publishTime)
        {
            string showFormat = "";
            TimeSpan span = DateTime.Now.Subtract(publishTime);
            if (span.Days == 0 && span.Hours == 0 && span.Minutes < 1) { showFormat = "刚刚"; }
            else if (span.Days == 0 && span.Hours == 0 && span.Minutes >= 1) { showFormat = span.Minutes.ToString() + "分钟前"; }
            else if (span.Days == 0 && span.Hours > 0) { showFormat = span.Hours.ToString() + "小时前"; }
            else if (span.Days == 1) { showFormat = "昨天"; }
            else if (span.Days >= 2) { showFormat = publishTime.ToString("yyyy.MM.dd"); }
            return showFormat;
        }

        #region Json 相关

        /// <summary>
        /// 对Json格式的字符串进行特符字符过滤
        /// </summary>
        /// <param name="input">要过滤的源字符串</param>
        /// <returns>返回过滤后的字符串</returns>
        public static string String2Json(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < input.Length; i++)
            {
                char c = input.ToCharArray()[i];
                switch (c)
                {
                    case '\"':
                        result.Append("\\\""); break;
                    case '\\':
                        result.Append("\\\\"); break;
                    case '/':
                        result.Append("\\/"); break;
                    case '\b':
                        result.Append("\\b"); break;
                    case '\f':
                        result.Append("\\f"); break;
                    case '\n':
                        result.Append("\\n"); break;
                    case '\r':
                        result.Append("\\r"); break;
                    case '\t':
                        result.Append("\\t"); break;
                    default:
                        result.Append(c); break;
                }
            }
            return result.ToString();
        }

        /// <summary>
        /// 对Json格式的字符串进行特符字符过滤
        /// </summary>
        /// <param name="input">要过滤的源字符串</param>
        /// <returns>返回过滤后的字符串</returns>
        public static string StringToJson(string input)
        {
            input = input.Replace("\\", "\\\\");
            input = input.Replace("/", "\\/");
            input = input.Replace("\b", "\\\b");
            input = input.Replace("\t", "\\\t");
            input = input.Replace("\n", "\\\n");
            input = input.Replace("\f", "\\\f");
            input = input.Replace("\r", "\\\r");
            return input.Replace("\"", "\\\"");
        }

        #endregion
    }
}
