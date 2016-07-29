using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Configuration
{
    /// <summary>
    /// 配置节点类型
    /// </summary>
    [Flags]
    enum ConfigNodeType
    {
        Null = 0,
        /// <summary>
        /// 包含值
        /// </summary>
        Value = 1,
        /// <summary>
        /// 包含键值对
        /// </summary>
        Dictionary = 2,
        /// <summary>
        /// 包含列表
        /// </summary>
        List = 4,
    }
}
