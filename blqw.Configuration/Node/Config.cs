using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Configuration
{
    /// <summary>
    /// 表示一个配置的根节点
    /// </summary>
    public sealed class Config : ConfigNode
    {
        private bool _singleNode;

        public Config() { }

        public Config(bool singleNode)
        {
            this._singleNode = singleNode;
        }

        public override ConfigNode Parent
        {
            get
            {
                return null;
            }
            protected set
            {
                throw new NotSupportedException($"根节点不允许设置{nameof(Parent)}属性");
            }
        }

        public override string Key
        {
            get
            {
                return null;
            }
            protected set
            {
            }
        }

        protected override ConfigNode CreateTemporary()
        {
            if (_singleNode)
            {
                return new SingleNode();
            }
            return base.CreateTemporary();
        }

        public override ConfigNode this[string path]
        {
            get
            {
                if (string.IsNullOrEmpty(path))
                {
                    return Invalid;
                }
                var index = new IndexerArgsParser(path);
                var ee = index.GetEnumerator();
                if (ee.MoveNext())
                {
                    var i = ee.Current;
                    var node = i.IsKey ? base[i.Key] : base[i.Index];
                    while (ee.MoveNext())
                    {
                        i = ee.Current;
                        node = i.IsKey ? node[i.Key] : node[i.Index];
                    }
                    return node;
                }
                return Invalid;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                if (string.IsNullOrEmpty(path))
                {
                    throw new ArgumentException(nameof(path), "路径无效");
                }
                var index = new IndexerArgsParser(path);
                var ee = index.GetEnumerator();
                if (ee.MoveNext())
                {
                    var i = ee.Current;
                    var node = i.IsKey ? base[i.Key] : base[i.Index];
                    if (ee.MoveNext() == false)
                    {
                        i = ee.Current;
                        if (i.IsKey)
                        {
                            base[i.Key] = value;
                        }
                        else
                        {
                            base[i.Index] = value;
                        }
                        return;
                    }

                    do
                    {
                        i = ee.Current;
                        node = i.IsKey ? node[i.Key] : node[i.Index];
                    } while (ee.MoveNext());

                    if (i.IsKey)
                    {
                        node.Parent[i.Key] = value;
                    }
                    else
                    {
                        node.Parent[i.Index] = value;
                    }
                    return;
                }
                throw new ArgumentException(nameof(path), "路径无效");
            }
        }
    }
}
