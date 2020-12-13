#if DEBUG
using kifuwarabe_shogithink.pure;
using kifuwarabe_shogithink.pure.genkyoku;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.ky.bb;
using kifuwarabe_shogithink.pure.listen;
using kifuwarabe_shogithink.pure.listen.play;
using kifuwarabe_shogithink.pure.logger;
using kifuwarabe_shogithink.pure.move;
using kifuwarabe_shogiwin.consolegame.machine;
using kifuwarabe_shogiwin.speak.ban;
using System;
using kifuwarabe_shogithink.pure.control;
#else
using kifuwarabe_shogithink.pure;
using kifuwarabe_shogithink.pure.control;
using kifuwarabe_shogithink.pure.genkyoku;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.ky.bb;
using kifuwarabe_shogithink.pure.listen;
using kifuwarabe_shogithink.pure.listen.play;
using kifuwarabe_shogithink.pure.logger;
using kifuwarabe_shogithink.pure.move;
using kifuwarabe_shogiwin.consolegame.machine;
using kifuwarabe_shogiwin.speak.ban;
using System;
#endif


namespace kifuwarabe_shogiwin.consolegame.console.command
{
    public static class CommandC
    {
        public static void CanDo(FenSyurui f, string line,
            CommandMode commandMode, IHyojiMojiretu hyoji)
        {
            // うしろに続く文字は☆（＾▽＾）
            int caret = 0;
            if (Util_String.MatchAndNext("cando", line, ref caret))
            {
                Move ss;
                if (line.Length<=caret)
                {
                    return;
                }
                else if (!LisPlay.MatchFenMove(f, line, ref caret, out ss))
                {
                    throw new Exception("パースエラー [" + line + "]");
                }

                if (GenkyokuOpe.CanDoMove( ss, out MoveMatigaiRiyu riyu))
                {
                    hyoji.AppendLine("cando, true");
                }
                else
                {
                    hyoji.Append("cando, false, ");
                    hyoji.AppendLine(riyu.ToString());
                }
            }
        }

        /// <summary>
        /// 置換表
        /// </summary>
        /// <param name="line"></param>
        /// <param name="hyoji"></param>
        public static bool TryFail_ChikanHyo(string line,
            IHyojiMojiretu hyoji)
        {
            // うしろに続く文字は☆（＾▽＾）
            int caret = 0;
            Util_String.TobasuTangoToMatubiKuhaku(line, ref caret, "chikanhyo ");

            // 左肩上がり    の置換表
            if (Util_String.MatchAndNext("ha45", line, ref caret))
            {
                // hidarikata agari 45 chikan の略
                SpkBan_MultiColumn.Setumei_MasuHyo(
                    new string[] { "左肩上４５" },
                    new Masu[][] { RotateChikanhyo.chikanHyo_ha45 },
                    false,
                    hyoji
                    );
            }
            // 左肩下がり    の置換表
            else if (Util_String.MatchAndNext("hs45", line, ref caret))
            {
                // hidarikata sagari 45 chikan の略
                SpkBan_MultiColumn.Setumei_MasuHyo(
                    new string[] { "左肩下４５" },
                    new Masu[][] { RotateChikanhyo.chikanHyo_hs45 },
                    false,
                    hyoji
                    );
            }
            // ９０°回転    の置換表
            else if (Util_String.MatchAndNext("ht90", line, ref caret))
            {
                // han tokei mawari 90 chikan の略
                SpkBan_MultiColumn.Setumei_MasuHyo(
                    new string[] { "反時回90" },
                    new Masu[][] { RotateChikanhyo.chikanHyo_ht90 },
                    true, // 横、縦の長さを反転
                    hyoji
                    );
            }

            return Pure.SUCCESSFUL_FALSE;
        }

        public static void Clear()
        {
            Util_Machine.Clear();
        }

    }
}
