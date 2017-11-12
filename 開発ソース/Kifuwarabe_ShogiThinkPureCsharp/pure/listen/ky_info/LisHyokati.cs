using kifuwarabe_shogithink.pure.listen.ky;
using kifuwarabe_shogithink.pure.logger;
//using kifuwarabe_shogiwin.consolegame.machine;
using System;
using System.Text.RegularExpressions;

namespace kifuwarabe_shogithink.pure.listen.ky_info
{
    public static class LisHyokati
    {
        /// <summary>
        /// 評価値以外にも、数字のパーサーとしてよく使うぜ☆（＾～＾）
        /// </summary>
        /// <param name="out_restLine"></param>
        /// <param name="commandline"></param>
        /// <param name="out_hyokati"></param>
        /// <returns></returns>
        public static bool TryParse(string commandline, ref int caret, out int out_hyokati
#if DEBUG
            , IDebugMojiretu reigai1
#endif
            )
        {
            Match m = Itiran_FenParser.HyokatiPattern.Match(commandline, caret);
            if (m.Success)
            {
                //if(""== m.Groups[1].Value)
                //{
                //    //*
                //    // FIXME:
                //    string msg = "パースに失敗だぜ☆（＾～＾）！  commandline=[" + commandline + "]caret(" + caret + ") .Value=[" + m.Groups[1].Value + "] m.Index=["+ m.Index+ "] m.Length=["+ m.Length + "]";
                //    Util_Machine.AppendLine(msg);
                //    Util_Machine.Flush();
                //    throw new Exception(msg);
                //    // */
                //}

                // キャレットを進めるぜ☆（＾▽＾）
                Util_String.SkipMatch(commandline, ref caret, m);

                // moji1 = m.Groups[1].Value;
                if (int.TryParse(m.Groups[1].Value, out out_hyokati))
                {
                    return true;
                }
                else
                {
                    //*
                    // FIXME:
#if DEBUG
                    string msg = "パースに失敗だぜ☆（＾～＾）！ #鱒 commandline=[" + commandline + "]caret(" + caret + ") .Value=[" + m.Groups[1].Value + "]";
                    reigai1.AppendLine(msg);
#endif
                    return false;
                    // */
                }
            }

            /*
            {
                // FIXME:
                string msg = "パースに失敗だぜ☆（＾～＾）！  commandline=[" + commandline + "]caret(" + caret + ")";
                Util_Machine.AppendLine(msg);
                Util_Machine.Flush();
                throw new Exception(msg);
            }
            // */
            out_hyokati = 0;
            return false;
        }
    }
}
