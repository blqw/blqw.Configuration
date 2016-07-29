using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Configuration
{
    /// <summary> 
    /// 用于操作Config文件
    /// </summary>
    public static class Configs
    {
        static Configs()
        {

            //ConnectionStrings = new NameValueCollection();
            //ConnectionProviders = new NameValueCollection();
            //foreach (ConnectionStringSettings item in System.Configuration.ConfigurationManager.ConnectionStrings)
            //{
            //    ConnectionStrings[item.Name] = item.ConnectionString;
            //    ConnectionProviders[item.Name] = item.ProviderName;
            //}
            //AppSettings = System.Configuration.ConfigurationManager.AppSettings;
            //IsDebug = string.Equals(AppSettings["DEBUG"], "true", StringComparison.OrdinalIgnoreCase);
        }
        /// <summary> 获取当前应用程序默认配置的 System.Configuration.AppSettingsSection 数据。
        /// </summary>
        public readonly static Config AppSettings;

        /// <summary> 获取当前应用程序默认配置的 连接字符串。
        /// </summary>
        public readonly static Config ConnectionStrings;

        /// <summary> 获取当前应用程序默认配置的 提供程序名称属性。
        /// </summary>
        public readonly static Config ConnectionProviders;

        /// <summary> 是否处于Debug模式
        /// </summary>
        public readonly static bool IsDebug;
    }
}
