using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace blqw.Configuration
{
    /// <summary>
    /// 方便的获取一个单例对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    static class Singleton<T>
    {
        private static IList<T> _IList;

        private static IEnumerator<T> _Enumerator;

        private static T _Instance;
        private static bool _Created;

        public static IList<T> IList
        {
            get
            {
                return _IList ?? (_IList = new List<T>(0).AsReadOnly());
            }
        }

        public static IEnumerator<T> Enumerator
        {
            get
            {
                return _Enumerator ?? (_Enumerator = new List<T>(0).GetEnumerator());
            }
        }

        public static T Instance
        {
            get
            {
                if (_Created)
                {
                    return _Instance;
                }
                try
                {
                    object obj = _Instance;
                    var instance = Activator.CreateInstance<T>();
                    Interlocked.CompareExchange<object>(ref obj, instance, null);
                    _Instance = (T)obj;
                }
                catch (Exception)
                {
                    _Instance = default(T);
                };
                
                _Created = true;
                return _Instance;
            }
        }
        
    }
}
