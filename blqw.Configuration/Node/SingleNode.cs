using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Configuration
{
    public class SingleNode : ConfigNode
    {
        public override ConfigNode this[int index]
        {
            get
            {
                return base[index];
            }
            set
            {
                base[index] = value;
                if (HasDictionary) ClearDictionary();
                if (HasValue) ClearValue();
            }
        }

        public override ConfigNode this[string key]
        {
            get
            {
                return base[key];
            }
            set
            {
                base[key] = value;
                if (HasList) ClearList();
                if (HasValue) ClearValue();
            }
        }

        public override object Value
        {
            get
            {
                return base.Value;
            }

            set
            {
                base.Value = value;
                if (HasDictionary) ClearDictionary();
                if (HasList) ClearList();
            }
        }

        private void ClearDictionary()
        {
            if (Dictionary?.Count > 0)
            {
                var arr = Dictionary.Values.ToArray();
                Dictionary.Clear();
                Array.ForEach(arr, it => it.Delete());
            }
        }
        private void ClearList()
        {
            if (List?.Count > 0)
            {
                var arr = List?.ToArray();
                List?.Clear();
                Array.ForEach(arr, it => it.Delete());
            }
        }
        private void ClearValue()
        {
            if (Value != null)
            {
                Value = null;
            }
        }
    }
}
