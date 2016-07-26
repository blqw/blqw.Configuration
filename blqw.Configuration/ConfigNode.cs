using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Configuration
{
    /// <summary>
    /// 表示一个配置节点
    /// </summary>
    public sealed class ConfigNode : IList<ConfigNode>, IDictionary<string, ConfigNode>
    {
        public ConfigNode(object value)
        {
            Value = value;
        }

        /// <summary>
        /// 用于存储集合数据
        /// </summary>
        private List<ConfigNode> _list;

        /// <summary>
        /// 用于存储键值对数据
        /// </summary>
        private Dictionary<string, ConfigNode> _dict;


        /// <summary>
        /// 表示一个值为<see cref="null"/>的节点
        /// </summary>
        public static readonly ConfigNode Null = new ConfigNode(null);

        /// <summary>
        /// 节点类型
        /// </summary>
        public ConfigNodeType Type { get; }

        /// <summary>
        /// 当前节点是否是一个数组
        /// </summary>
        public bool IsArray { get { return Type == ConfigNodeType.Array; } }
        /// <summary>
        /// 当前节点是否是一个对象(键值对)
        /// </summary>
        public bool IsObject { get { return Type == ConfigNodeType.Object; } }
        /// <summary>
        /// 当前节点是否是一个值
        /// </summary>
        public bool IsValue { get { return Type == ConfigNodeType.Value; } }
        /// <summary>
        /// 当前节点是否是null
        /// </summary>
        public bool IsNull { get { return Type == ConfigNodeType.Null; } }

        public int Count { get { return _list?.Count ?? _dict?.Count ?? 0; } }

        /// <summary>
        /// 根据索引获取或设置数组中的值,如果<seealso cref="Type"/> 不等于 <see cref="ConfigNodeType.Array"/> 则返回 <seealso cref="Null"/>
        /// </summary>
        /// <param name="index">索引</param>
        /// <returns></returns>
        /// <exception cref="">set操作时<paramref name="index"/>小于<see cref="0"/>或大于</exception>
        public ConfigNode this[int index]
        {
            get { /* return the specified index here */ }
            set { /* set the specified index to value here */ }
        }

        /// <summary>
        /// 根据键获取或设置对象中的值,如果<seealso cref="Type"/> 不等于 <see cref="ConfigNodeType.Object"/> 则返回 <seealso cref="Null"/>
        /// </summary>
        /// <param name="key">用于查找值的键</param>
        /// <returns></returns>
        /// <exception cref="">set操作时<paramref name="indexOrKey"/>为<see cref="null"/></exception>
        public ConfigNode this[string key]
        {
            get { /* return the specified index here */ }
            set { /* set the specified index to value here */ }
        }

        /// <summary>
        /// 根据键(<see cref="string"/>)或索引(<see cref="int"/>)获取或设置对象中的值,找不到时返回<seealso cref="Null"/>
        /// </summary>
        /// <param name="indexOrKey">键或索引</param>
        /// <returns></returns>
        /// <exception cref=""><paramref name="indexOrKey"/>不是<see cref="string"/>也不是<see cref="int"/></exception>
        /// <exception cref="">set操作时<paramref name="indexOrKey"/>为<see cref="null"/></exception>
        public ConfigNode this[object indexOrKey]
        {
            get { /* return the specified index here */ }
            set { /* set the specified index to value here */ }
        }

        /// <summary>
        /// 根据键获取或设置对象中的值,如果<seealso cref="Type"/> 不等于 <see cref="ConfigNodeType.Value"/> 则返回 <seealso cref="Null"/>
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// 移除指定的子节点,节点不存在返回<see cref="false"/>
        /// </summary>
        /// <param name="key">子节点的键</param>
        public bool Remove(string key)
        {

        }

        /// <summary>
        /// 移除指定的子节点,节点不存在返回<see cref="false"/>
        /// </summary>
        /// <param name="index">子节点的索引或键</param>
        public bool Remove(int index)
        {

        }

        /// <summary>
        /// 移除指定的子节点,节点不存在返回<see cref="false"/>
        /// </summary>
        /// <param name="indexOrKey">子节点的索引或键</param>
        public bool Remove(string indexOrKey)
        {

        }

        public void Add(string name, object value)
        {

        }

        public void Add(object value)
        {

        }


        #region IList
        bool ICollection<ConfigNode>.IsReadOnly
        {
            get
            {
                return false;
            }
        }

        int IList<ConfigNode>.IndexOf(ConfigNode item)
        {
            return this._list.IndexOf(item);
        }

        void IList<ConfigNode>.Insert(int index, ConfigNode item)
        {
            this._list.Insert(index, item);
        }

        void IList<ConfigNode>.RemoveAt(int index)
        {
            this._list.RemoveAt(index);
        }

        void ICollection<ConfigNode>.Add(ConfigNode item)
        {
            this._list.Add(item);
        }

        void ICollection<ConfigNode>.Clear()
        {
            this._list.Clear();
        }

        bool ICollection<ConfigNode>.Contains(ConfigNode item)
        {
            return this._list.Contains(item);
        }

        void ICollection<ConfigNode>.CopyTo(ConfigNode[] array, int arrayIndex)
        {
            this._list.CopyTo(array, arrayIndex);
        }

        bool ICollection<ConfigNode>.Remove(ConfigNode item)
        {
            return this._list.Remove(item);
        }

        IEnumerator<ConfigNode> IEnumerable<ConfigNode>.GetEnumerator()
        {
            return this._list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            if (IsArray)
            {
                return this._list?.GetEnumerator();
            }
            if (IsObject)
            {
                return this._dict?.GetEnumerator();
            }
            return null;
        }

        #endregion

        public bool ContainsKey(string key)
        {
            return ((IDictionary<string, ConfigNode>)this._dict).ContainsKey(key);
        }

        public void Add(string key, ConfigNode value)
        {
            ((IDictionary<string, ConfigNode>)this._dict).Add(key, value);
        }

        public bool TryGetValue(string key, out ConfigNode value)
        {
            return ((IDictionary<string, ConfigNode>)this._dict).TryGetValue(key, out value);
        }

        public void Add(KeyValuePair<string, ConfigNode> item)
        {
            ((IDictionary<string, ConfigNode>)this._dict).Add(item);
        }

        public void Clear()
        {
            ((IDictionary<string, ConfigNode>)this._dict).Clear();
        }

        public bool Contains(KeyValuePair<string, ConfigNode> item)
        {
            return ((IDictionary<string, ConfigNode>)this._dict).Contains(item);
        }

        public void CopyTo(KeyValuePair<string, ConfigNode>[] array, int arrayIndex)
        {
            ((IDictionary<string, ConfigNode>)this._dict).CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<string, ConfigNode> item)
        {
            return ((IDictionary<string, ConfigNode>)this._dict).Remove(item);
        }

        public IEnumerator<KeyValuePair<string, ConfigNode>> GetEnumerator()
        {
            return ((IDictionary<string, ConfigNode>)this._dict).GetEnumerator();
        }

        public ICollection<string> Keys
        {
            get
            {
                return ((IDictionary<string, ConfigNode>)this._dict).Keys;
            }
        }

        public ICollection<ConfigNode> Values
        {
            get
            {
                return ((IDictionary<string, ConfigNode>)this._dict).Values;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return ((IDictionary<string, ConfigNode>)this._dict).IsReadOnly;
            }
        }

    }
}
