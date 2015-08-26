using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace J7.IO
{
    /// <summary>
    /// 文件操作类
    /// </summary>
    public class FileExtend
    {
        /// <summary>
        /// 写一个Txt文本文件
        /// </summary>
        /// <param name="path">存放文件的完整路径，包含文件名</param>
        /// <param name="content">写内容</param>
        /// <param name="isAppend">是否追加</param>
        public static void WriteTxt(string path, string content, bool isAppend)
        {
            try
            {
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(path, isAppend, Encoding.UTF8))
                {
                    sw.Write(content);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
