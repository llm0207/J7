using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace J7.Net
{
    /// <summary>
    /// 调用实例：
    /// UserAgent userAgent = new UserAgent("Mozilla/5.0+(Windows+NT+6.1;+WOW64)+AppleWebKit/537.36+(KHTML,+like+Gecko)+Chrome/29.0.1547.66+Safari/537.36+LBBROWSER");
    /// var brower = userAgent.ClientBrowser;
    /// </summary>
    public class UserAgent
    {
        #region 属性设置
        /// <summary>
        /// 客户端操作系统
        /// </summary>
        public string ClientSystem { get; set; }
        /// <summary>
        /// 客户端浏览器
        /// </summary>
        public string ClientBrowser { get; set; }
        /// <summary>
        /// 浏览器版本
        /// </summary>
        public string BrowserVersion { get; set; }
        /// <summary>
        /// 是否是移动设备，包括阅读器、PS3、XBox等移动设备
        /// </summary>
        public bool IsMobileDevice { get; set; }
        #endregion

        /// <summary>
        /// 根据客户端的User-Agent，获取客户端的系统及浏览器相关信息
        /// </summary>
        /// <param name="userAgentString">客户端的User-Agent</param>
        public UserAgent(string userAgentString)
        {
            userAgentString = userAgentString.ToLower();

            #region 客户端系统

            if (userAgentString.IndexOf("windows") != -1)
            {
                ClientSystem = "Windows";
            }
            else if (userAgentString.IndexOf("ubuntu") != -1)
            {
                ClientSystem = "Ubuntu";
            }
            else if (userAgentString.IndexOf("silk") != -1 || userAgentString.IndexOf("kindle") != -1) { } //Duplicate used to block Macintosh
            else if (userAgentString.IndexOf("macintosh") != -1)
            {
                ClientSystem = "Macintosh";
            }
            else if (userAgentString.IndexOf("blackberry") != -1)
            {
                IsMobileDevice = true;
                ClientSystem = "Blackberry";                
            }
            else if (userAgentString.IndexOf("xbox") != -1)
            {
                IsMobileDevice = true;
                ClientSystem = "XBox";
            }
            else if (userAgentString.IndexOf("playstation") != -1)
            {
                IsMobileDevice = true;
                ClientSystem = "PlayStation";
            }

            if (userAgentString.IndexOf("ipad") != -1)
            {
                IsMobileDevice = true;
                ClientSystem = "iPad";
            }
            else if (userAgentString.IndexOf("iphone") != -1)
            {
                IsMobileDevice = true;
                ClientSystem = "iPhone";
            }
            else if (userAgentString.IndexOf("kindle") != -1)
            {
                IsMobileDevice = true;
                ClientSystem = "Kindle";
            }
            else if (userAgentString.IndexOf("silk") != -1)
            {
                IsMobileDevice = true;
                ClientSystem = "Kindle";
            }
            else if (userAgentString.IndexOf("android") != -1)
            {
                IsMobileDevice = true;
                ClientSystem = "Android";
            }
            #endregion

            #region 客户端浏览器

            if (userAgentString.IndexOf("msie") != -1)
            {
                ClientBrowser = "MSIE";     //IE
            }
            else if (userAgentString.IndexOf("firefox") != -1)
            {
                ClientBrowser = "FireFox";
            }
            else if (userAgentString.IndexOf("opera") != -1)
            {
                ClientBrowser = "Opera";
            }
            else if (userAgentString.IndexOf("android") != -1)
            {
                ClientBrowser = "Android";
            }
            else if (userAgentString.IndexOf("chrome") != -1 || userAgentString.IndexOf("crios") != -1)
            {
                ClientBrowser = "Chrome";
            }
            else if (userAgentString.IndexOf("silk") != -1 || userAgentString.IndexOf("kindle") != -1)
            {
                ClientBrowser = "Silk";
            }
            else if (userAgentString.IndexOf("safari") != -1)
            {
                ClientBrowser = "Safari";
            }
            #endregion
        }
    }
}
