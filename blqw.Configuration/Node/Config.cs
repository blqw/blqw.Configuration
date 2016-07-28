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
    }
}
