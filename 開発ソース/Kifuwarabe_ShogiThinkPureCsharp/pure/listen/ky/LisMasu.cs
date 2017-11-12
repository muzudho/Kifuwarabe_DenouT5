#if DEBUG
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.logger;
using System.Text.RegularExpressions;
#else
using kifuwarabe_shogithink.pure.conv;
using kifuwarabe_shogithink.pure.genkyoku;
using kifuwarabe_shogithink.pure.ky;
using System;
using System.Text.RegularExpressions;
using kifuwarabe_shogithink.pure.logger;
#endif


namespace kifuwarabe_shogithink.pure.listen.ky
{
    public static class LisMasu
    {
        /// <summary>
        /// 2017-04-19 作成
        /// 
        /// パースに失敗してもエラーではない。
        /// </summary>
        /// <param name="line"></param>
        /// <param name="caret"></param>
        /// <param name="out_ms"></param>
        /// <returns></returns>
        public static bool MatchMasu
            (string line, ref int caret, out Masu out_ms
#if DEBUG
            ,IDebugMojiretu dbg_reigai
#endif
            )
        {
            Match m = Itiran_FenParser.GetMasuPattern(PureSettei.fenSyurui).Match(line, caret);
            if (m.Success)
            {
                // キャレットを進める
                Util_String.SkipMatch(line, ref caret, m);

                int suji = LisInt.FenSuji_Int(PureSettei.fenSyurui, m.Groups[1].Value);
                int dan = LisInt.FenDan_Int(PureSettei.fenSyurui, m.Groups[2].Value);

                // 升を返す
                out_ms = Conv_Masu.ToMasu(suji, dan);

                return true;
            }
            else
            {
                // 該当なし（エラーではない）
                out_ms = Conv_Masu.masu_error;
                return false;
            }
        }

    }
}
