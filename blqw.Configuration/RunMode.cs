using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Configuration
{
    /// <summary>
    /// 运行模式
    /// </summary>
    public enum RunMode
    {
        /// <summary>
        /// 发布模式,默认
        /// </summary>
        Release = 0,
        /// <summary>
        /// 附加到进程调试模式
        /// </summary>
        Attached = 1,
        /// <summary>
        /// 本地开发模式
        /// </summary>
        Local = 2,
        /// <summary>
        /// 调试模式
        /// </summary>
        Debug = 3,
        /// <summary>
        /// 测试模式
        /// </summary>
        Test = 4,
        /// <summary>
        /// 集成发布模式
        /// </summary>
        Integration = 5,
        /// <summary>
        /// 预览模式
        /// </summary>
        Preview = 6,
        /// <summary>
        /// 内测模式
        /// </summary>
        Alpha = 7,
        /// <summary>
        /// 公测模式
        /// </summary>
        Beta = 8,
    }
}
