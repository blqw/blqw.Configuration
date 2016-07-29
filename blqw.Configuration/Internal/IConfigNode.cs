using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Configuration
{
    /// <summary>
    /// 描述一个配置节点
    /// </summary>
    interface IConfigNode : ICloneable, IEnumerable<ConfigNode>
    {
        /// <summary>
        /// 是否包含键
        /// </summary>
        bool HasDictionary { get; }
        /// <summary>
        /// 是否包含列表
        /// </summary>
        bool HasList { get; }
        /// <summary>
        /// 是否包含值
        /// </summary>
        bool HasValue { get; }
        /// <summary>
        /// 节点路径
        /// </summary>
        string Path { get; }
        /// <summary>
        /// 父节点
        /// </summary>
        ConfigNode Parent { get; }
        /// <summary>
        /// 是否只读
        /// </summary>
        bool IsReadOnly { get; }
        /// <summary>
        /// 克隆节点,克隆出的对象不包含父节点和路径
        /// </summary>
        new ConfigNode Clone();
        /// <summary>
        /// 通过索引获取或设置子节点
        /// </summary>
        /// <param name="index">索引</param>
        /// <returns></returns>
        ConfigNode this[int index] { get; set; }
        /// <summary>
        /// 通过键获取或设置子节点
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        ConfigNode this[string key] { get; set; }
        /// <summary>
        /// 获取或设置当前节点的值
        /// </summary>
        object Value { get; set; }
        /// <summary>
        /// 当前节点的名称
        /// </summary>
        string Key { get; }
        /// <summary>
        /// 枚举所有的Key
        /// </summary>
        IEnumerator<string> Keys { get; }
        /// <summary>
        /// 从父节点中移除这个节点
        /// </summary>
        void Remove();
        /// <summary>
        /// 删除当前节点以及子节点中的所有内容
        /// </summary>
        void Delete();

        /// <summary>
        /// 添加值到当前的节点的列表
        /// </summary>
        void Add(object value);
    }
}
