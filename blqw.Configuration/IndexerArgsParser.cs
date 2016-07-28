using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Configuration
{
    /// <summary>
    /// 用于解析复杂的索引
    /// </summary>
    struct IndexerArgsParser : IEnumerable<IndexerArgs>
    {
        public IndexerArgsParser(string str)
        {
            if (string.IsNullOrEmpty(str))
                throw new ArgumentNullException(nameof(str));
            _index = 0;
            _chars = str.ToCharArray();
            _length = str.Length;
        }

        int _index;
        int _length;
        char[] _chars;

        public string Finished
        {
            get
            {
                return new string(_chars, 0, _index);
            }
        }

        public string Unfinished
        {
            get
            {
                return new string(_chars, _index + 1, _length - _index);
            }
        }


        public IEnumerator<IndexerArgs> GetEnumerator()
        {
            _index = 0;
            var saved = 0;
            for (; _index < _length; _index++)
            {
                var c = _chars[_index];
                if (c == '.')
                {
                    if (_index == saved)
                    {
                        throw new NotSupportedException($"路径`{Finished}`有误");
                    }
                    yield return new IndexerArgs(new string(_chars, saved, _index - saved));
                    saved = _index + 1;
                }
                else if (c == '[')
                {
                    if (_index == saved)
                    {
                        throw new NotSupportedException($"路径`{Finished}`有误");
                    }
                    yield return new IndexerArgs(new string(_chars, saved, _index - saved));
                    _index++;
                    yield return new IndexerArgs(GetNumber(_chars));
                    saved = _index + 1;
                }
            }
            if (saved != _index)
            {
                yield return new IndexerArgs(new string(_chars, saved, _index - saved));
            }
        }

        private int GetNumber(char[] chars)
        {
            int number = 0;
            for (; _index < _length; _index++)
            {
                switch (chars[_index])
                {
                    case '1':
                    case '１':
                        number = number * 10 + 1;
                        break;
                    case '2':
                    case '２':
                        number = number * 10 + 2;
                        break;
                    case '3':
                    case '３':
                        number = number * 10 + 3;
                        break;
                    case '4':
                    case '４':
                        number = number * 10 + 4;
                        break;
                    case '5':
                    case '５':
                        number = number * 10 + 5;
                        break;
                    case '6':
                    case '６':
                        number = number * 10 + 6;
                        break;
                    case '7':
                    case '７':
                        number = number * 10 + 7;
                        break;
                    case '8':
                    case '８':
                        number = number * 10 + 8;
                        break;
                    case '9':
                    case '９':
                        number = number * 10 + 9;
                        break;
                    case '0':
                    case '０':
                        number = number * 10;
                        break;
                    case ']':
                        _index++;
                        return number;
                    default:
                        throw new NotImplementedException("`[]` 中只能是数字");
                }
            }
            throw new NotImplementedException("缺少 `]`");
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
