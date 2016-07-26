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
    public enum ConfigNodeType
    {
        Null = 0,
        /// <summary>
        /// 值
        /// </summary>
        Value = 1,
        /// <summary>
        /// 对象(键值对)
        /// </summary>
        Object = 2,
        /// <summary>
        /// 数组
        /// </summary>
        Array = 3,
    }
}
