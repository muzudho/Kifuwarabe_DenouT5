#if DEBUG
using kifuwarabe_shogithink.pure.conv;
using kifuwarabe_shogithink.pure.logger;
using System;
using System.Text.RegularExpressions;
using kifuwarabe_shogithink.pure.control;
using kifuwarabe_shogithink.fen;
#else
using kifuwarabe_shogithink.pure.control;
using kifuwarabe_shogithink.pure.conv;
using kifuwarabe_shogithink.pure.genkyoku;
using kifuwarabe_shogithink.pure.ky;
using System;
using System.Text.RegularExpressions;
using kifuwarabe_shogithink.pure.logger;
using kifuwarabe_shogithink.fen;
#endif


namespace kifuwarabe_shogithink.pure.listen.ky
{
    public static class LisInt
    {
        /// <summary>
        /// 持駒の枚数等
        /// </summary>
        /// <param name="isSfen"></param>
        /// <param name="suji"></param>
        /// <returns></returns>
        public static bool MatchInt(string line, ref int caret, out int out_int)
        {
            Match m = Itiran_FenParser.GetIntPattern().Match(line, caret);
            if (m.Success)
            {
                Util_String.SkipMatch(line, ref caret, m);

                if (!int.TryParse(m.Groups[1].Value, out out_int))
                {
                    return Pure.FailTrue(string.Format("パース失敗 m.Groups[1].Value=[{0}]", m.Groups[1].Value));
                }
                return Pure.SUCCESSFUL_FALSE;
            }
            else
            {
                out_int = 0;
                return Pure.FailTrue("MatchInt not success1");
            }
        }

        /// <summary>
        /// 筋
        /// </summary>
        /// <param name="f"></param>
        /// <param name="suji"></param>
        /// <returns></returns>
        public static int FenSuji_Int(FenSyurui f, string suji)
        {
            switch (f)
            {
                case FenSyurui.sfe_n:
                    {
                        int iSuji;
                        if (!int.TryParse(suji, out iSuji))
                        {
                            throw new Exception("パース失敗 suji=[" + suji + "]");
                        }
                        return PureSettei.banYokoHaba + 1 - iSuji;
                    }
                case FenSyurui.dfe_n:
                    {
                        return Conv_Kihon.AlphabetToInt(suji);
                    }
                default:
                    throw new Exception(string.Format("未定義 {0}", f));
            }
        }
        /// <summary>
        /// 段
        /// </summary>
        /// <param name="f"></param>
        /// <param name="suji"></param>
        /// <returns></returns>
        public static int FenDan_Int(FenSyurui f, string dan)
        {
            switch (f)
            {
                case FenSyurui.sfe_n:
                    return Conv_Kihon.AlphabetToInt(dan);
                case FenSyurui.dfe_n:
                    {
                        int iDan;
                        if (!int.TryParse(dan, out iDan))
                        {
                            throw new Exception("パース失敗 dan=[" + dan + "]");
                        }
                        return iDan;
                    }
                default:
                    throw new Exception(string.Format("未定義 {0}", f));
            }
        }
    }
}
