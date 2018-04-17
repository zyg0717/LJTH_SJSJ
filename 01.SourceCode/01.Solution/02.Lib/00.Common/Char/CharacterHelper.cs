
using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.Common
{
    ///   <summary>   
    ///   汉字拼音声母计算类   
    ///   Write   by   WangZhenlong   at   2003/11/29   
    ///   </summary>   
    public static class CharacterHelper
    {

        private static string CombinePinyinToString(List<string> Pinyins)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string item in Pinyins)
            {
                sb.Append(item);
                sb.Append("|");
            }

            return sb.ToString().TrimEnd('|');
        }

        private static List<string> CombineArray(List<string> source, List<string> add)
        {
            List<string> result = new List<string>();
            if (source.Count == 0)
            {
                return add;
            }
            foreach (string s in source)
            {
                foreach (string a in add)
                {
                    result.Add(string.Format("{0}{1}", s, a));
                }
            }
            return result;
        }
        ///   <summary>   
        ///   获取一串汉字的拼音声母   
        ///   </summary>   
        ///   <param   name="chinese">Unicode格式的汉字字符串</param>   
        ///   <returns>拼音声母字符串</returns>  
        public static String Convert(String chinese)
        {
            char[] buffer = new char[chinese.Length];
            for (int i = 0; i < chinese.Length; i++)
            {
                buffer[i] = Convert(chinese[i]);
            }
            return new String(buffer);
        }

        ///   <summary>   
        ///   获取一个汉字的拼音声母   
        ///   </summary>   
        ///   <param   name="chinese">Unicode格式的一个汉字</param>   
        ///   <returns>汉字的声母</returns>   
        private static char Convert(Char chinese)
        {
            Encoding gb2312 = Encoding.GetEncoding("GB2312");
            Encoding unicode = Encoding.Unicode;

            //   Convert   the   string   into   a   byte[].   
            byte[] unicodeBytes = unicode.GetBytes(new Char[] { chinese });
            //   Perform   the   conversion   from   one   encoding   to   the   other.   
            byte[] asciiBytes = Encoding.Convert(unicode, gb2312, unicodeBytes);

            char defaultChar = 'a';

            if (asciiBytes.Length < 2)
            {
                return defaultChar;
            }

            //   计算该汉字的GB-2312编码   
            int n = (int)asciiBytes[0] << 8;
            n += (int)asciiBytes[1];

            //   根据汉字区域码获取拼音声母   
            if (In(0xB0A1, 0xB0C4, n)) return 'a';
            if (In(0XB0C5, 0XB2C0, n)) return 'b';
            if (In(0xB2C1, 0xB4ED, n)) return 'c';
            if (In(0xB4EE, 0xB6E9, n)) return 'd';
            if (In(0xB6EA, 0xB7A1, n)) return 'e';
            if (In(0xB7A2, 0xB8c0, n)) return 'f';
            if (In(0xB8C1, 0xB9FD, n)) return 'g';
            if (In(0xB9FE, 0xBBF6, n)) return 'h';
            if (In(0xBBF7, 0xBFA5, n)) return 'j';
            if (In(0xBFA6, 0xC0AB, n)) return 'k';
            if (In(0xC0AC, 0xC2E7, n)) return 'l';
            if (In(0xC2E8, 0xC4C2, n)) return 'm';
            if (In(0xC4C3, 0xC5B5, n)) return 'n';
            if (In(0xC5B6, 0xC5BD, n)) return 'o';
            if (In(0xC5BE, 0xC6D9, n)) return 'p';
            if (In(0xC6DA, 0xC8BA, n)) return 'q';
            if (In(0xC8BB, 0xC8F5, n)) return 'r';
            if (In(0xC8F6, 0xCBF0, n)) return 's';
            if (In(0xCBFA, 0xCDD9, n)) return 't';
            if (In(0xCDDA, 0xCEF3, n)) return 'w';
            if (In(0xCEF4, 0xD188, n)) return 'x';
            if (In(0xD1B9, 0xD4D0, n)) return 'y';
            if (In(0xD4D1, 0xD7F9, n)) return 'z';
            return defaultChar;
        }

        private static bool In(int Lp, int Hp, int Value)
        {
            return ((Value <= Hp) && (Value >= Lp));
        }

        #region reference
        //// -------------
        ////将汉字转成拼音字头的方法“中华人民共和国”-->"ZHRMGHG"     

        ////   是采用对应的区位的方法，但有些汉字不在这个范围里，大家试一下   

        //private string hz2py(string hz)     //获得汉字的区位码   
        //{
        //    byte[] sarr = System.Text.Encoding.Default.GetBytes(hz);
        //    int len = sarr.Length;
        //    if (len > 1)
        //    {
        //        byte[] array = new byte[2];
        //        array = System.Text.Encoding.Default.GetBytes(hz);

        //        int i1 = (short)(array[0] - '\0');
        //        int i2 = (short)(array[1] - '\0');

        //        //unicode解码方式下的汉字码   
        //        //                         array   =   System.Text.Encoding.Unicode.GetBytes(hz);   
        //        //                         int   i1   =   (short)(array[0]   -   '\0');   
        //        //                         int   i2   =   (short)(array[1]   -   '\0');   
        //        //                         int   t1   =   Convert.ToInt32(i1,16);   
        //        //                         int   t2   =   Convert.ToInt32(i2,16);   

        //        int tmp = i1 * 256 + i2;
        //        string getpychar = "*";//找不到拼音码的用*补位   

        //        if (tmp >= 45217 && tmp <= 45252) { getpychar = "A"; }
        //        else if (tmp >= 45253 && tmp <= 45760) { getpychar = "B"; }
        //        else if (tmp >= 47761 && tmp <= 46317) { getpychar = "C"; }
        //        else if (tmp >= 46318 && tmp <= 46825) { getpychar = "D"; }
        //        else if (tmp >= 46826 && tmp <= 47009) { getpychar = "E"; }
        //        else if (tmp >= 47010 && tmp <= 47296) { getpychar = "F"; }
        //        else if (tmp >= 47297 && tmp <= 47613) { getpychar = "G"; }
        //        else if (tmp >= 47614 && tmp <= 48118) { getpychar = "H"; }
        //        else if (tmp >= 48119 && tmp <= 49061) { getpychar = "J"; }
        //        else if (tmp >= 49062 && tmp <= 49323) { getpychar = "K"; }
        //        else if (tmp >= 49324 && tmp <= 49895) { getpychar = "L"; }
        //        else if (tmp >= 49896 && tmp <= 50370) { getpychar = "M"; }
        //        else if (tmp >= 50371 && tmp <= 50613) { getpychar = "N"; }
        //        else if (tmp >= 50614 && tmp <= 50621) { getpychar = "O"; }
        //        else if (tmp >= 50622 && tmp <= 50905) { getpychar = "P"; }
        //        else if (tmp >= 50906 && tmp <= 51386) { getpychar = "Q"; }
        //        else if (tmp >= 51387 && tmp <= 51445) { getpychar = "R"; }
        //        else if (tmp >= 51446 && tmp <= 52217) { getpychar = "S"; }
        //        else if (tmp >= 52218 && tmp <= 52697) { getpychar = "T"; }
        //        else if (tmp >= 52698 && tmp <= 52979) { getpychar = "W"; }
        //        else if (tmp >= 52980 && tmp <= 53640) { getpychar = "X"; }
        //        else if (tmp >= 53689 && tmp <= 54480) { getpychar = "Y"; }
        //        else if (tmp >= 54481 && tmp <= 55289) { getpychar = "Z"; }
        //        return getpychar;
        //    }
        //    else
        //    {
        //        return hz;
        //    }
        //}

        //public string transpy(string strhz)     //把汉字字符串转换成拼音码   
        //{
        //    string strtemp = "";
        //    int strlen = strhz.Length;
        //    for (int i = 0; i <= strlen - 1; i++)
        //    {
        //        strtemp += hz2py(strhz.Substring(i, 1));
        //    }
        //    return strtemp;
        //}


        /// <summary>
        /// 截取字符串,中文算2个字符 li jing guang 2013-05-29
        /// </summary>
        /// <param name="inputString">原字符串</param>
        /// <param name="len">长度</param>
        /// <param name="tailString">字符串末尾显示</param>
        /// <returns>截取后的字符串</returns>
        public static string GetSubString(string inputString, int len, string tailString)
        {
            string subString = CutString(inputString, len);
            if (inputString != subString)
                return subString + tailString;
            return subString;
        }

        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="inputString">要被截取的字符串</param>
        /// <param name="len">长度</param>
        /// <returns>截取后的字符串</returns>
        public static string CutString(string inputString, int len)
        {
            if (string.IsNullOrEmpty(inputString))
                return string.Empty;
            if (inputString.Length * 2 <= len)
                return inputString;
            if (inputString.Length > len)
                inputString = inputString.Substring(0, len);

            System.Text.UnicodeEncoding unicode = new System.Text.UnicodeEncoding();
            byte[] s = unicode.GetBytes(inputString);
            int realLen = 0;
            int i = 0;

            for (i = 0; i < inputString.Length; i++)
            {
                if (s[i * 2 + 1] == 0 && s[i * 2] <= 127)
                    realLen += 1;
                else
                    realLen += 2;

                if (realLen > len) break;
            }
            return inputString.Substring(0, i);
        }
        #endregion

        /// <summary>
        /// 当字符串为空串的时候， 返回为null；否则返回去掉头尾空格的字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string NullWhenEmpty(this string value)
        {
            string result = value.Trim();
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }
            return result;
        }
    }
}
