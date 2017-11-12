#if DEBUG
using kifuwarabe_shogithink.pure.conv;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.logger;
using System;
using System.Text.RegularExpressions;
using kifuwarabe_shogithink.pure.control;
#else
using kifuwarabe_shogithink.pure.control;
using kifuwarabe_shogithink.pure.conv;
using kifuwarabe_shogithink.pure.genkyoku;
using kifuwarabe_shogithink.pure.ky;
using System;
using System.Text.RegularExpressions;
using kifuwarabe_shogithink.pure.logger;
#endif

namespace kifuwarabe_shogithink.pure.listen.ky
{
    public abstract class Med_Parser
    {
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="out_restString"></param>
        ///// <param name="commandline">B3といった文字列☆</param>
        ///// <returns></returns>
        //public static bool TryParseMs(
        //    string commandline,
        //    Kyokumen ky,
        //    ref int caret,
        //    out Masu result
        //)
        //{
        //    //「B4」形式と想定
        //    // テキスト形式の符号「A4 …」の最初の１要素を、切り取ってトークンに分解します。

        //    //------------------------------------------------------------
        //    // リスト作成
        //    //------------------------------------------------------------

        //    Match m = Itiran_FenParser.GetMasuSasitePattern(Option_Application.Optionlist.USI).Match(commandline, caret);
        //    if (m.Success)
        //    {
        //        Util_String.SkipMatch(commandline, ref caret, m);

        //        // 符号１「B4」を元に、Masu を作ります。

        //        // 盤上の駒を動かしたぜ☆
        //        result = Med_Parser.FenSujiDan_Masu(
        //            Option_Application.Optionlist.USI,
        //            m.Groups[1].Value, //ABCabc か、 ZKH
        //            m.Groups[2].Value //1234   か、 *
        //            );
        //        return true;
        //    }

        //    // 「B4B3」形式ではなかった☆（＾△＾）！？　次の一手が読めない☆
        //    result = ky.MASU_ERROR;
        //    return false;
        //}

        /// <summary>
        /// A1 を 0 に。
        /// </summary>
        /// <param name="f"></param>
        /// <param name="suji"></param>
        /// <param name="dan"></param>
        /// <returns></returns>
        public static Masu FenSujiDan_Masu(FenSyurui f, string suji, string dan)
        {
            return Conv_Masu.ToMasu(LisInt.FenSuji_Int(f, suji), LisInt.FenDan_Int(f, dan));
        }




        /// <summary>
        /// "1" を 対局者１、 "2" を 対局者２ にするぜ☆（＾～＾）
        /// </summary>
        /// <param name="moji1"></param>
        /// <returns></returns>
        public static bool Try_MojiToTaikyokusya(FenSyurui f, string moji1, out Taikyokusya out_tai)
        {
            switch (f)
            {
                case FenSyurui.sfe_n:
                    {
                        switch (moji1)
                        {
                            case "b": out_tai = Taikyokusya.T1; return true;
                            case "w": out_tai = Taikyokusya.T2; return true;
                            default: out_tai = Taikyokusya.Yososu; return false;
                        }
                    }
                case FenSyurui.dfe_n:
                    {
                        switch (moji1)
                        {
                            case "1": out_tai = Taikyokusya.T1; return true;
                            case "2": out_tai = Taikyokusya.T2; return true;
                            default: out_tai = Taikyokusya.Yososu; return false;
                        }
                    }
                default:
                    throw new Exception(string.Format("未定義 {0}", f));
            }
        }

        /// <summary>
        /// 持ち駒の種類
        /// </summary>
        /// <param name="moji">改造Fen</param>
        /// <returns></returns>
        public static MotigomaSyurui MojiToMotikomaSyurui(FenSyurui f, string moji)
        {
            switch (f)
            {
                case FenSyurui.sfe_n:
                    {
                        switch (moji)
                        {
                            case "B": return MotigomaSyurui.Z;
                            case "R": return MotigomaSyurui.K;
                            case "P": return MotigomaSyurui.H;
                            case "G": return MotigomaSyurui.I;
                            case "S": return MotigomaSyurui.Neko;
                            case "N": return MotigomaSyurui.U;
                            case "L": return MotigomaSyurui.S;
                            default: return MotigomaSyurui.Yososu;
                        }
                    }
                case FenSyurui.dfe_n:
                    {
                        switch (moji)
                        {
                            case "Z": return MotigomaSyurui.Z;
                            case "K": return MotigomaSyurui.K;
                            case "H": return MotigomaSyurui.H;
                            case "I": return MotigomaSyurui.I;
                            case "N": return MotigomaSyurui.Neko;
                            case "U": return MotigomaSyurui.U;
                            case "S": return MotigomaSyurui.S;
                            default: return MotigomaSyurui.Yososu;
                        }
                    }
                default:
                    throw new Exception(string.Format("未定義 {0}", f));
            }
        }

        /// <summary>
        /// 駒の種類
        /// </summary>
        /// <param name="moji">改造Fen</param>
        /// <returns></returns>
        public static Komasyurui MojiToKomasyurui(FenSyurui f, string moji)
        {
            switch (f)
            {
                case FenSyurui.sfe_n:
                    {
                        switch (moji)
                        {
                            case "K": return Komasyurui.R;
                            case "B": return Komasyurui.Z;
                            case "+B": return Komasyurui.PZ;
                            case "R": return Komasyurui.K;
                            case "+R": return Komasyurui.PK;
                            case "P": return Komasyurui.H;
                            case "+P": return Komasyurui.PH;
                            case "G": return Komasyurui.I;
                            case "S": return Komasyurui.N;
                            case "+S": return Komasyurui.PN;
                            case "N": return Komasyurui.U;
                            case "+N": return Komasyurui.PU;
                            case "L": return Komasyurui.S;
                            case "+L": return Komasyurui.PS;
                            default: return Komasyurui.Yososu;
                        }
                    }
                case FenSyurui.dfe_n:
                    {
                        switch (moji)
                        {
                            case "R": return Komasyurui.R;
                            case "Z": return Komasyurui.Z;
                            case "+Z": return Komasyurui.PZ;
                            case "K": return Komasyurui.K;
                            case "+K": return Komasyurui.PK;
                            case "H": return Komasyurui.H;
                            case "+H": return Komasyurui.PH;
                            case "I": return Komasyurui.I;
                            case "N": return Komasyurui.N;
                            case "+N": return Komasyurui.PN;
                            case "U": return Komasyurui.U;
                            case "+U": return Komasyurui.PU;
                            case "S": return Komasyurui.S;
                            case "+S": return Komasyurui.PS;
                            default: return Komasyurui.Yososu;
                        }
                    }
                default:
                    throw new Exception(string.Format("未定義 {0}", f));
            }
        }
    }
}
