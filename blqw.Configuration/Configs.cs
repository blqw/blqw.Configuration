using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using blqw.IOC;

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
            AppSettings.AsReadOnly();

            ConnectionStrings = new Config();
            ConnectionProviders = new Config();
            foreach (ConnectionStringSettings item in ConfigurationManager.ConnectionStrings)
            {
                ConnectionStrings[item.Name].Value = item.ConnectionString;
                ConnectionProviders[item.Name].Value = item.ProviderName;
            }
            ConnectionStrings.AsReadOnly();
            ConnectionProviders.AsReadOnly();

            if (Debugger.IsAttached)
            {
                RunMode = RunMode.Attached;
            }
            else if (Enum.TryParse(AppSettings["RunMode"].Value?.ToString(), false, out RunMode) == false)
            {
                RunMode = RunMode.Release;
            }
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

        /// <summary> 
        /// 获取当前应用程序默认配置的 System.Configuration.AppSettingsSection 数据。
        /// </summary>
        public static readonly Config AppSettings;

        /// <summary> 
        /// 获取当前应用程序默认配置的 连接字符串。
        /// </summary>
        public static readonly Config ConnectionStrings;

        /// <summary> 
        /// 获取当前应用程序默认配置的 提供程序名称属性。
        /// </summary>
        public static readonly Config ConnectionProviders;

        /// <summary> 
        /// 获取当前开发模式
        /// </summary>
        public static readonly RunMode RunMode;

        /// <summary>
        /// 应用设置到当前实例的属性
        /// </summary>
        /// <param name="obj"></param>
        public static void ApplyAppSettings(object obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            var type = obj as Type;
            if (type != null)
            {
                ApplyAppSettings(type);
                return;
            }
            ApplyAppSettings(obj.GetType(), BindingFlags.Instance, obj);
        }

        /// <summary>
        /// 应用设置到当前对象的静态属性
        /// </summary>
        /// <param name="type"></param>
        public static void ApplyAppSettings(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            ApplyAppSettings(type, BindingFlags.Static, null);
        }

        private static void ApplyAppSettings(Type type, BindingFlags flags, object instance)
        {
            var className = type.GetCustomAttribute<AppSettsingAttribute>()?.Name;
            foreach (var p in type.GetProperties(flags | BindingFlags.Public | BindingFlags.NonPublic))
            {
                var name = p.GetCustomAttribute<AppSettsingAttribute>()?.Name ?? p.Name;
                
                if (className != null)
                {
                    SetValue(instance, p,
                        $"{className}.{name}",
                        name
                        );
                }
                else
                {
                    SetValue(instance, p,
                        $"{type.FullName.Replace("+", ".")}.{name}",
                        $"{type.Name}.{name}",
                        name
                        );
                }
            }
        }

        private static void SetValue(object instance, PropertyInfo p, params string[] names)
        {
            
            for (int i = 0, length = names.Length; i < length; i++)
            {
                var node = AppSettings[names[i]];
                if (!node.HasValue) continue;
                try
                {
                    var value = ComponentServices.Converter.Convert(node.Value, p.PropertyType);
                    ComponentServices.GetSeter(p)(instance, value);
                }
                catch (Exception ex)
                {
                    Debug.Assert(true, ex.Message);
                    // ignored
                }
                return;
            }
        }
    }
}
