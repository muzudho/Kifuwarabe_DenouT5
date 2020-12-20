using kifuwarabe_shogithink.pure.ky;

using kifuwarabe_shogithink.pure.control;
using System;
using System.Text;

namespace kifuwarabe_shogithink.pure.speak.ky
{
    public static class SpkKomasyurui
    {
        /// <summary>
        /// 目視確認用の文字列を返すぜ☆（＾▽＾）
        /// </summary>
        /// <param name="ks"></param>
        /// <returns></returns>
        public static void GetNingenyoMijikaiFugo(Komasyurui ks, StringBuilder hyoji)
        {
            hyoji.Append(Conv_Komasyurui.m_ningenyoMijikaiFugo_[(int)ks]);
        }

        /// <summary>
        /// 改造Fen用の文字列を返すぜ☆（＾▽＾）
        /// </summary>
        /// <param name="ks"></param>
        /// <returns></returns>
        public static string ToFen(FenSyurui f, Komasyurui ks)
        {
            switch (f)
            {
                case FenSyurui.sfe_n:
                    {
                        return Conv_Komasyurui.m_sfen_[(int)ks];
                    }
                case FenSyurui.dfe_n:
                    {
                        return Conv_Komasyurui.m_dfen_[(int)ks];
                    }
                default:
                    throw new Exception(string.Format("未定義 {0}", f));
            }
        }

        /// <summary>
        /// 改造Fen用の文字列を返すぜ☆（＾▽＾）
        /// </summary>
        /// <param name="ks"></param>
        /// <returns></returns>
        public static void AppendFenTo(FenSyurui f, Komasyurui ks, StringBuilder syuturyoku)
        {
            syuturyoku.Append(ToFen(f, ks));
        }
    }
}
