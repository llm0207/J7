using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Net;

namespace J7.Net
{
    /// <summary>
    /// 网页采集类
    /// </summary>
    public class WebPick
    {
        /// <summary>
        /// 页面请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="method">请求方式 POST 或 GET</param>
        /// <param name="data">表单提交的值</param>
        /// <param name="encoding">页面编码格式</param>
        /// <returns></returns>
        public static string WebRequest(string url, string method = "GET", string data = null, string encoding = "UTF-8")
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                request.Method = method == "GET" ? method : "POST";
                request.Timeout = 3000;
                request.AllowAutoRedirect = true;
                request.Headers.Add("Accept-Encoding", "gzip");
                if (method == "POST" && !string.IsNullOrEmpty(data))
                {
                    request.ContentType = "application/x-www-form-urlencoded";
                    using (StreamWriter streamWriter = new StreamWriter(request.GetRequestStream()))
                    {
                        streamWriter.Write(data);
                    }
                }
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (Stream stream = response.GetResponseStream())
                {
                    bool hasGzipCompress = response.Headers["Content-Encoding"] != null && response.Headers["Content-Encoding"].ToLower() == "gzip";
                    GZipStream gzip = hasGzipCompress ? new GZipStream(stream, System.IO.Compression.CompressionMode.Decompress) : null;
                    using (StreamReader streamReader = new StreamReader(hasGzipCompress ? gzip : stream, Encoding.GetEncoding(encoding)))
                    {
                        return streamReader.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 判断该网络资源是否存在
        /// </summary>
        /// <param name="url">网络资源地址</param>
        /// <returns></returns>
        public static bool IsExistNetResources(string url)
        {
            int num = 200;
            HttpWebRequest request = (HttpWebRequest)System.Net.WebRequest.Create(new Uri(url));
            ServicePointManager.Expect100Continue = false;
            try
            {
                ((HttpWebResponse)request.GetResponse()).Close();
            }
            catch (WebException exception)
            {
                if (exception.Status != WebExceptionStatus.ProtocolError)
                {
                    num = 200;
                }
                if (exception.Message.IndexOf("500 ") > 0)
                {
                    num = 500;
                }
                if (exception.Message.IndexOf("401 ") > 0)
                {
                    num = 401;
                }
                if (exception.Message.IndexOf("404") > 0)
                {
                    num = 404;
                }
            }
            return num == 200;
        }
    }
}
