using System;
using System.Collections.Generic;
using System.Linq;

namespace Blog.Core.Converters
{
    public static class StringToUrlStandard
    {
        private static readonly Dictionary<Char, String> Map;

        static StringToUrlStandard()
        {
            Map = new Dictionary<Char, String>
                  {
                      {'а', "a"},
                      {'б', "b"},
                      {'в', "v"},
                      {'г', "g"},
                      {'д', "d"},
                      {'е', "e"},
                      {'ё', "e"},
                      {'ж', "zh"},
                      {'з', "z"},
                      {'и', "i"},
                      {'й', "i"},
                      {'к', "k"},
                      {'л', "l"},
                      {'м', "m"},
                      {'н', "n"},
                      {'о', "o"},
                      {'п', "p"},
                      {'р', "r"},
                      {'с', "s"},
                      {'т', "t"},
                      {'у', "u"},
                      {'ф', "f"},
                      {'х', "kh"},
                      {'ц', "ts"},
                      {'ч', "ch"},
                      {'ш', "sh"},
                      {'щ', "sch"},
                      {'ъ', String.Empty},
                      {'ы', "y"},
                      {'ь', String.Empty},
                      {'э', "e"},
                      {'ю', "yu"},
                      {'я', "ya"},
                      {' ', "-"},
                      {'\"', String.Empty},
                      {'\'', String.Empty},
                      {'<', String.Empty},
                      {'>', String.Empty},
                      {'@', String.Empty},
                      {'#', String.Empty},
                      {'$', String.Empty},
                      {'!', String.Empty},
                      {'?', String.Empty},
                      {'(', String.Empty},
                      {')', String.Empty},
                      {':', String.Empty},
                      {';', String.Empty},
                      {'=', String.Empty},
                      {'~', String.Empty},
                      {'{', String.Empty},
                      {'}', String.Empty},
                      {']', String.Empty},
                      {'[', String.Empty},
                      {'*', String.Empty},
                      {'\\', String.Empty},
                      {'/', String.Empty},
                      {'|', String.Empty},
                      {'^', String.Empty},
                  };
        }

        public static String Convert(String input)
        {
            var result = input;

            foreach (var c in input)
            {
                if (!Map.ContainsKey(c))
                {
                    continue;
                }

                result = result.Replace(c.ToString(), Map[c]);
            }

            return result;
        }
    }
}