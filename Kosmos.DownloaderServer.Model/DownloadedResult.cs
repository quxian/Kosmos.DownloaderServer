using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kosmos.DownloaderServer.Model {
    /// <summary>
    /// 下载结果
    /// </summary>
    public class DownloadedResult {
        /// <summary>
        /// 把下载结果的HashCode作为ID
        /// </summary>
        public string ResultHashCode { get; set; }
        /// <summary>
        /// 下载结果所属站点
        /// </summary>
        public string Domain { get; set; }
        /// <summary>
        /// 从该URL下载内容
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// URI对用的内容
        /// </summary>
        public string Result { get; set; }
        /// <summary>
        /// 下载时间
        /// </summary>
        public DateTime? DownloadDate { get; set; }
        /// <summary>
        /// 是否已经提取
        /// </summary>
        public bool IsExtracted { get; set; }
        /// <summary>
        /// 最后一次提取时间
        /// </summary>
        public DateTime? LastExtractDate { get; set; }

        public override bool Equals(object obj) {
            if (ReferenceEquals(obj, null)) return false;

            if (ReferenceEquals(this, obj)) return true;

            var o = obj as DownloadedResult;
            return ResultHashCode.Equals(o.ResultHashCode);
        }

        public override int GetHashCode() {
            return ResultHashCode.GetHashCode();
        }
    }
}
