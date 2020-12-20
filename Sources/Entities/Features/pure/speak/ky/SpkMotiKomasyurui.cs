using kifuwarabe_shogithink.pure.ky;

using kifuwarabe_shogithink.pure.control;
using System;
using System.Text;

namespace kifuwarabe_shogithink.pure.speak.ky
{
    public static class SpkMotiKomasyurui
    {
        /// <summary>
        /// 改造Fen用の文字列を返すぜ☆（＾▽＾）
        /// </summary>
        /// <param name="ks"></param>
        /// <returns></returns>
        public static void AppendFenTo(FenSyurui f, MotigomaSyurui mks, StringBuilder syuturyoku)
        {
            switch (f)
            {
                case FenSyurui.sfe_n:
                    syuturyoku.Append(Conv_MotigomaSyurui.m_sfen_[(int)mks]);
                    break;
                case FenSyurui.dfe_n:
                    syuturyoku.Append(Conv_MotigomaSyurui.m_dfen_[(int)mks]);
                    break;
                default:
                    throw new Exception(string.Format("未定義 {0}", f));
            }
        }
    }
}
