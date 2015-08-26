using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace J7.Net
{
    /// <summary>
    /// 获取js执行之后的网页html标签body部分的代码
    /// 示例代码：
    /// string url = "http://item.taobao.com/item.htm?id=39116126552";
    /// FinalHtml html = new FinalHtml();
    /// if (html.Run(url))
    /// {
    ///    FileStream stream = File.OpenWrite("out.html");
    ///    StreamWriter writer = new StreamWriter(stream);
    ///    List<String> linkList = html.LinkList;
    ///    List<String> imageList = html.ImageList;                
    ///    writer.WriteLine("Html Body:");
    ///    writer.WriteLine(html.HtmlBody);
    ///    writer.Close();
    /// }
    public class FinalHtml
    {
        private String htmlString;
        private String url;
        private String htmlTitle;
        // 获得html title标签的内容
        public String HtmlTitle
        {
            get
            {
                if (success == false) return null;
                return htmlTitle;
            }
        }
        private List<String> linkList;
        private List<String> imageList;
        private bool success; // 是否成功运行
        /// <summary>
        /// 获得网页所有链接的链表， 一定要在Run之后进行
        /// </summary>
        public List<String> LinkList
        {
            get
            {
                if (success == false) return null;
                return linkList;
            }
        }
        /// <summary>
        /// 获得所有图像的标签， 一定要在Run之后进行
        /// </summary>
        public List<String> ImageList
        {
            get
            {
                if (success == false) return null;
                return imageList;
            }
        }
        /// <summary>
        /// 获得执行完js之后的网页body 部分的html代码
        /// </summary>
        public String HtmlBody
        {
            get
            {
                if (success == false) return null;
                return htmlString;
            }
        }
        public FinalHtml()
        {
            linkList = new List<String>();
            imageList = new List<String>();
            htmlString = "";
            success = false;
        }
        /// <summary>
        /// 检查并补充设置url
        /// </summary>
        /// <param name="url"></param>
        private void CheckURL(String url)
        {
            if (!url.StartsWith("http://") && !url.StartsWith("https://") && !url.StartsWith("file:///"))
                url = "http://" + url;
            this.url = url;
        }
        /// <summary>
        /// 加载指定文件
        /// </summary>
        /// <param name="url">文件URL</param>
        /// <param name="timeOut">超时时限</param>
        /// <returns>是否成功运行，没有超时</returns>
        public bool Run(String url, int timeOut = 10000)
        {
            CheckURL(url);
            Thread newThread = new Thread(NewThread);
            newThread.SetApartmentState(ApartmentState.STA);/// 为了创建WebBrowser类的实例 必须将对应线程设为单线程单元
            newThread.Start();
            //监督子线程运行时间
            while (newThread.IsAlive && timeOut > 0)
            {
                Thread.Sleep(100);
                timeOut -= 100;
            }
            // 超时处理
            if (newThread.IsAlive)
            {
                if (success) return true;
                newThread.Abort();
                return false;
            }
            return true;
        }

        private void NewThread()
        {
            new FinalHtmlPerThread(this);
            Application.Run();// 循环等待webBrowser 加载完毕 调用 DocumentCompleted 事件
        }
        /// <summary>
        ///  用于处理一个url的核心类
        /// </summary>
        class FinalHtmlPerThread : IDisposable
        {
            FinalHtml master;
            WebBrowser browser;

            public FinalHtmlPerThread(FinalHtml master)
            {
                this.master = master;
                DealWithUrl();
            }
            private void DealWithUrl()
            {
                String url = master.url;
                browser = new WebBrowser();
                bool success = false;
                try
                {
                    browser.ScriptErrorsSuppressed = true;
                    browser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(web_DocumentCompleted);
                    browser.Navigating += new WebBrowserNavigatingEventHandler(browser_Navigating);
                    browser.Navigate(url);
                    while (browser.ReadyState != WebBrowserReadyState.Complete)
                    {
                        Application.DoEvents();
                    }
                    //while (hitCount < 16)
                    //{
                    //    Application.DoEvents();
                    //}
                    master.htmlTitle = browser.Document.Title;
                    ToList(browser.Document.Links, master.linkList);
                    ToList(browser.Document.Images, master.imageList);
                    var htmldocument = (mshtml.HTMLDocument)browser.Document.DomDocument;
                    htmldocument.defaultCharset = "GBK";
                    master.htmlString = htmldocument.documentElement.outerHTML;
                    master.success = true;
                    Thread.CurrentThread.Abort();
                    success = true;
                }
                finally
                {
                    if (!success)
                        Dispose();
                }

            }
            public void Dispose()
            {
                if (!browser.IsDisposed)
                    browser.Dispose();
            }
            private void ToList(HtmlElementCollection collection, List<String> list)
            {
                System.Collections.IEnumerator it = collection.GetEnumerator();
                while (it.MoveNext())
                {
                    HtmlElement htmlElement = (HtmlElement)it.Current;
                    list.Add(htmlElement.OuterHtml);
                }
            }
            private void web_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
            {
                //微软官方回答 一个网页有多个Ifram元素就有可能触发多次此事件， 并且提到了
                // vb 和 C++ 的解决方案， C# 没有提及， 经本人尝试，发现下面的语句可以判断成功
                // 如果未完全加载 web.ReadyState = WebBrowserReadyState.Interactive

            }
            static void browser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
            {

            }
        }
    }
}
