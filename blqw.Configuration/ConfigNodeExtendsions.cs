using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Configuration
{
    /// <summary>
    /// <see cref="ConfigNode"/> 拓展方法
    /// </summary>
    public static class ConfigNodeExtendsions
    {
        /// <summary>
        /// 根据路径查询配置节点
        /// </summary>
        /// <param name="config">需要查询的配置的根节点</param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static ConfigNode Select(this Config config, string path)
        {
            if (config == null)
            {
                return null;
            }
            if (string.IsNullOrEmpty(path))
            {
                return ConfigNode.Invalid;
            }
            var index = new IndexerArgsParser(path);
            var node = (ConfigNode)config;
            foreach (var i in index)
            {
                node = i.IsKey ? node[i.Key] : node[i.Index];
            }
            return node;
        }


    }
}
