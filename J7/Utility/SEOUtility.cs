using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Text.RegularExpressions;

namespace J7.Utility
{
    /// <summary>
    /// SEO相关工具类
    /// </summary>
    public class SEOUtility
    {
        /// <summary>
        /// 获取Canonical标签，如：<link rel="canonical" href="http://www.fanhuan.com/" />
        /// </summary>
        /// <param name="isTemplet">是否是模板，默认为false，为true时不输出任何东西</param>
        /// <returns></returns>
        public static string GetCanonicalUrl(bool isTemplet = false)
        {
            if (isTemplet)
            {
                return "";
            }
            else
            {
                Regex reg = new Regex(@"\?(.*)|index(/|)");
                string canonicalTemp = "<link rel=\"canonical\" href=\"{0}\" />";
                return string.Format(canonicalTemp, reg.Replace(HttpContext.Current.Request.Url.AbsoluteUri, ""));
            }
        }
    }
}
