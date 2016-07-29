using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace blqw.Configuration
{
    /// <summary> 
    /// 用于操作Config文件
    /// </summary>
    public static class Configs
    {
        static Configs()
        {
            //var xml = new XmlDocument();
            //xml.LoadXml(File.ReadAllText(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile));
            //var nodes = xml.SelectNodes("/configuration/connectionStrings/*");
            var appsettings = System.Configuration.ConfigurationManager.AppSettings;
            AppSettings = new Config();
            var builder = new System.Data.Common.DbConnectionStringBuilder(false);
            foreach (string name in appsettings)
            {
                var value = appsettings[name];
                var node = AppSettings[name];
                AddOrSet(node, value);
                try
                {
                    builder.ConnectionString = value;
                    foreach (string key in builder.Keys)
                    {
                        AddOrSet(node[key], builder[key]);
                    }
                }
                catch { }
            }


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


        /// <summary>
        /// 添加或设置值
        /// </summary>
        public static void AddOrSet(ConfigNode node, object value)
        {
            if (node.HasList)
            {
                node.Add(value);
            }
            else if (node.HasValue)
            {
                node.Add(node.Value);
                node.Add(value);
            }
            node.Value = value;
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
