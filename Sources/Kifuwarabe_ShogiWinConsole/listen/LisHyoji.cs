#if DEBUG
using System;
using System.Text;
#else
using System;
using System.Text;
#endif

namespace kifuwarabe_shogiwin.listen
{
    public static class LisHyoji
    {
        /// <summary>
        /// ２進数にして返します
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string ToNisinsu(ulong number)
        {
            // ulong を2進数文字列表記に変換できない
            // 精度が下がってしまうが？
            //return Convert.ToString((long)number, 2);

            // 一旦、１６進に変換
            string x16 = number.ToString("x");
            char[] chArr = x16.ToCharArray();

            StringBuilder sb = new StringBuilder();
            foreach (char ch in chArr)
            {
                switch (ch)
                {
                    case '0': sb.Append("0000"); break;
                    case '1': sb.Append("0001"); break;
                    case '2': sb.Append("0010"); break;
                    case '3': sb.Append("0011"); break;
                    case '4': sb.Append("0100"); break;
                    case '5': sb.Append("0101"); break;
                    case '6': sb.Append("0110"); break;
                    case '7': sb.Append("0111"); break;
                    case '8': sb.Append("1000"); break;
                    case '9': sb.Append("1001"); break;
                    case 'a': sb.Append("1010"); break;
                    case 'b': sb.Append("1011"); break;
                    case 'c': sb.Append("1100"); break;
                    case 'd': sb.Append("1101"); break;
                    case 'e': sb.Append("1110"); break;
                    case 'f': sb.Append("1111"); break;
                    default:throw new Exception(string.Format("想定しない文字 ch={0}",ch));
                }
            }
            string bi = sb.ToString();

            // 頭に連続する 0 を省く
            int index = bi.IndexOf("1");
            if (-1==index)
            {
                bi = "0";
            }
            else if (0 < index)
            {
                bi = bi.Substring(index);
            }

            return bi;
        }
    }
}
