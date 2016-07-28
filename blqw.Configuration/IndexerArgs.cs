using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Configuration
{
    /// <summary>
    /// 表示一个索引器参数
    /// </summary>
    struct IndexerArgs
    {
        /// <summary>
        /// 初始化一个带有键的索引器参数
        /// </summary>
        /// <param name="key"></param>
        public IndexerArgs(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }
            _index = null;
            Key = key;
            IsKey = true;
            IsIndex = false;
        }
        /// <summary>
        /// 初始化一个带有索引的索引器参数
        /// </summary>
        /// <param name="index"></param>
        public IndexerArgs(int index)
        {
            _index = index;
            Key = null;
            IsKey = false;
            IsIndex = true;
        }
        /// <summary>
        /// 键
        /// </summary>
        public string Key { get; }
        private int? _index;
        /// <summary>
        /// 索引
        /// </summary>
        public int Index { get { return _index.Value; } }
        /// <summary>
        /// 参数是否是一个键
        /// </summary>
        public bool IsKey { get; }
        /// <summary>
        /// 参数是否是一个索引
        /// </summary>
        public bool IsIndex { get; }
    }
}
