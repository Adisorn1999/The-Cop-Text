using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Services;

public static class KeyboardLayoutHelper
{
    private static readonly Dictionary<char, char> EnToTh =
        new()
        {
            ['a'] = 'ฟ',
            ['b'] = 'ิ',
            ['c'] = 'แ',
            ['d'] = 'ก',
            ['e'] = 'ำ',
            ['f'] = 'ด',
            ['g'] = 'เ',
            ['h'] = '้',
            ['i'] = 'ร',
            ['j'] = '่',
            ['k'] = 'า',
            ['l'] = 'ส',
            ['m'] = 'ท',
            ['n'] = 'ื',
            ['o'] = 'น',
            ['p'] = 'ย',
            ['q'] = 'ๆ',
            ['r'] = 'พ',
            ['s'] = 'ห',
            ['t'] = 'ะ',
            ['u'] = 'ี',
            ['v'] = 'อ',
            ['w'] = 'ไ',
            ['x'] = 'ป',
            ['y'] = 'ั',
            ['z'] = 'ผ'
        };

    private static readonly Dictionary<char, char> ThToEn =
        EnToTh.ToDictionary(
            x => x.Value,
            x => x.Key);

    public static string ConvertLayout(
        string text)
    {
        char[] result =
            new char[text.Length];

        for (int i = 0;
             i < text.Length;
             i++)
        {
            char c =
                char.ToLower(text[i]);

            if (EnToTh.ContainsKey(c))
            {
                result[i] =
                    EnToTh[c];
            }
            else if (ThToEn.ContainsKey(c))
            {
                result[i] =
                    ThToEn[c];
            }
            else
            {
                result[i] =
                    c;
            }
        }

        return new string(result);
    }
}
