using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Configuration
{
    /// <summary>
    /// 表示一个配置节点
    /// </summary>
    public class ConfigNode : IConfigNode
    {
        protected ConfigNode()
        {

        }

        /// <summary>
        /// 无效节点
        /// </summary>
        public static readonly ConfigNode Invalid = new ConfigNode { IsReadOnly = true };

        /// <summary>
        /// 用于存储实际值
        /// </summary>
        private object _value;
        /// <summary>
        /// 临时节点保存的索引
        /// </summary>
        private int _index;
        /// <summary>
        /// 这是一个临时节点
        /// </summary>
        public bool IsTemporary { get; private set; } = true;
        /// <summary>
        /// 通过键获取或设置子节点
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public virtual ConfigNode this[string key]
        {
            get
            {
                if (string.IsNullOrEmpty(key))
                {
                    return Invalid;
                }
                ConfigNode c;
                if (Dictionary != null && Dictionary.TryGetValue(key, out c))
                {
                    return c;
                }
                c = CreateTemporary();
                c.Key = key;
                c.Parent = this;
                return c;
            }
            set
            {
                if (IsReadOnly)
                {
                    throw new NotSupportedException("当前节点是只读的");
                }
                if (string.IsNullOrEmpty(key))
                {
                    throw new ArgumentNullException(nameof(key));
                }
                ConfigNode node;
                if (Dictionary == null)
                {
                    Dictionary = new Dictionary<string, ConfigNode>();
                }
                else if (Dictionary.TryGetValue(key, out node))
                {
                    if (node == value)
                    {
                        return;
                    }
                    node.Remove();
                }
                Dictionary[key] = value;
                Self(value);
            }
        }
        /// <summary>
        /// 通过索引获取或设置子节点
        /// </summary>
        /// <param name="index">索引</param>
        /// <returns></returns>
        public virtual ConfigNode this[int index]
        {
            get
            {
                if (index < 0)
                {
                    return Invalid;
                }
                if (this.List == null || index >= this.List.Count)
                {

                    var node = CreateTemporary();
                    node.IsTemporary = true;
                    node.Parent = this;
                    node._index = index;
                    return node;
                }
                return List[index];
            }
            set
            {
                if (IsReadOnly)
                {
                    throw new NotSupportedException("当前节点是只读的");
                }
                if (index < 0)
                    throw new ArgumentOutOfRangeException(nameof(index), $"`{nameof(index)}`={index} 无效");
                if (this.List == null && index == 0)
                {
                    this.List = new List<ConfigNode>();
                    this.List.Add(value);
                }
                else if (this.List != null && index <= this.List.Count)
                {
                    if (index == this.List.Count)
                    {
                        this.List.Add(value);
                    }
                    else
                    {
                        this.List[index] = value;
                    }
                }
                else
                {
                    throw new NotSupportedException("暂时不能插入这个节点,这可能是因为索引过大");
                }
                Self(value);
            }
        }
        /// <summary>
        /// 将节点保存到自己名下
        /// </summary>
        /// <param name="node"></param>
        private void Self(ConfigNode node)
        {
            if (node == this)
            {
                throw new NotSupportedException("自己不能成为自己的子节点");
            }
            var p = this.Parent;
            while (p != null)
            {
                if (p == node)
                {
                    throw new NotSupportedException("自己的父节点不能成为自己的子节点");
                }
                p = p.Parent;
            }
            if (node.Parent != null && node.IsTemporary == false)
            {
                node.Remove();
            }
            node.Parent = this;
            node.IsTemporary = false;
            if (IsTemporary && Parent != null)
            {
                if (Key != null)
                {
                    Parent[Key] = this;
                }
                else
                {
                    Parent[this._index] = this;
                }
            }
        }

        /// <summary>
        /// 获取或设置当前节点的值
        /// </summary>
        public virtual object Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (IsReadOnly)
                {
                    throw new NotSupportedException("当前节点是只读的");
                }
                if (value is ConfigNode)
                {
                    throw new NotSupportedException($"`{nameof(value)}`不能是`{nameof(ConfigNode)}`类型");
                }
                if (IsTemporary && Parent != null)
                {
                    if (Key != null)
                    {
                        Parent[Key] = this;
                    }
                    else
                    {
                        Parent[this._index] = this;
                    }
                }

                if (value is DBNull)
                {
                    this._value = null;
                }
                else
                {
                    this._value = value;
                }

            }
        }
        
        /// <summary>
        /// 用于存储集合数据
        /// </summary>
        protected List<ConfigNode> List { get; private set; }
        /// <summary>
        /// 用于存储键值对数据
        /// </summary>
        protected Dictionary<string, ConfigNode> Dictionary { get; private set; }

        /// <summary>
        /// 是否只读
        /// </summary>
        public virtual bool IsReadOnly { get; protected set; }
        /// <summary>
        /// 父节点
        /// </summary>
        public virtual ConfigNode Parent { get; protected set; }
        /// <summary>
        /// 节点路径
        /// </summary>
        public string Path
        {
            get
            {
                if (Parent == null)
                {
                    return null;
                }
                else if (Key == null)
                {
                    if (IsTemporary)
                    {
                        return $"{Parent.Path}[{_index}]?";
                    }
                    var index = Parent.List.IndexOf(this);
                    return $"{Parent.Path}[{index}]";
                }
                else if (Parent.Path == null)
                {
                    if (IsTemporary)
                    {
                        return $"{Key}?";
                    }
                    return Key;
                }
                else if (IsTemporary)
                {
                    return $"{Parent.Path}.{Key}?";
                }
                return $"{Parent.Path}.{Key}";
            }
        }
        /// <summary>
        /// 当前节点的名称
        /// </summary>
        public virtual string Key { get; protected set; }

        /// <summary>
        /// 克隆节点,克隆出的对象不包含父节点和路径
        /// </summary>
        public ConfigNode Clone()
        {
            var clone = (ConfigNode)MemberwiseClone();
            if (this.List != null)
            {
                clone.List = new List<ConfigNode>();
                clone.List.AddRange(this.List.Select(it => it.Clone()));
            }
            if (this.Dictionary != null)
            {
                clone.Dictionary = new Dictionary<string, ConfigNode>();
                foreach (var node in this.Dictionary.Values)
                {
                    clone.Dictionary.Add(node.Key, node.Clone());
                }
            }
            return clone;
        }

        /// <summary>
        /// 从父节点中移除这个节点
        /// </summary>
        public void Remove()
        {
            if (Parent == null) return;
            if (Key != null)
            {
                Parent.Dictionary.Remove(Key);
            }
            else
            {
                Parent.List.Remove(this);
            }
            Parent = null;
        }

        /// <summary>
        /// 删除当前节点以及子节点中的所有内容
        /// </summary>
        public void Delete()
        {
            Remove();
            this.List?.All(it =>
            {
                it.Parent = null;
                it.Delete();
                return true;
            });
            this.List?.Clear();
            this.Dictionary?.All(it =>
            {
                it.Value.Parent = null;
                it.Value.Delete();
                return true;
            });
            this.Dictionary?.Clear();
            this.IsReadOnly = true;
        }

        object ICloneable.Clone()
        {
            return Clone();
        }

        public IEnumerator<ConfigNode> GetEnumerator()
        {
            return this.List?.GetEnumerator() ?? Singleton<ConfigNode>.Enumerator;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// 是否包含键
        /// </summary>
        public bool HasDictionary { get { return this.Dictionary?.Count > 0; } }
        /// <summary>
        /// 是否包含列表
        /// </summary>
        public bool HasList { get { return this.List?.Count > 0; } }
        /// <summary>
        /// 是否包含值
        /// </summary>
        public bool HasValue { get { return this.Value != null; } }
        /// <summary>
        /// 是否是null
        /// </summary>
        public bool IsNull
        {
            get
            {
                return !(List?.Count > 0 || Dictionary?.Count > 0 || Value != null);
            }
        }

        public IEnumerator<string> Keys
        {
            get
            {
                return (IEnumerator<string>)Dictionary?.Keys.GetEnumerator() ?? Singleton<string>.Enumerator;
            }
        }

        protected virtual ConfigNode CreateTemporary()
        {
            return new ConfigNode();
        }

        public override string ToString()
        {
            if (ReferenceEquals(this , Invalid))
            {
                return "<Invalid>";
            }
            var type = ConfigNodeType.Null;
            if (HasDictionary)
                type |= ConfigNodeType.Dictionary;
            if (HasList)
                type |= ConfigNodeType.List;
            if (HasValue)
                type |= ConfigNodeType.Value;
            return $"{Path} Type={type}";
        }


    }
}
