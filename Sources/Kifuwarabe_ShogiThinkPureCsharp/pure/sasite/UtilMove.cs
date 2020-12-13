#if DEBUG
using kifuwarabe_shogithink.pure.accessor;
using kifuwarabe_shogithink.pure.com.moveorder;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.listen;
using kifuwarabe_shogithink.pure.listen.play;
using kifuwarabe_shogithink.pure.logger;
#else
using kifuwarabe_shogithink.pure.accessor;
using kifuwarabe_shogithink.pure.com.MoveOrder;
using kifuwarabe_shogithink.pure.listen;
using kifuwarabe_shogithink.pure.listen.play;
using kifuwarabe_shogithink.pure.logger;
#endif

namespace kifuwarabe_shogithink.pure.move
{
    public static class UtilMove
    {
        /// <summary>
        /// グローバル変数に結果を入れるぜ☆（＾～＾）
        /// </summary>
        /// <param name="out_list"></param>
        /// <param name="hyoji"></param>
        /// <returns></returns>
        public static bool TryFailMoveCmd1(IHyojiMojiretu hyoji)
        {
            //グローバル変数に指し手がセットされるぜ☆（＾▽＾）
            PureMemory.SetTnskFukasa(PureMemory.FUKASA_MANUAL);
            MoveGenAccessor.DoMovePickerBegin(MoveType.N21_All);
            MovePicker01.MovePickerN01(MoveType.N21_All, true);
            return Pure.SUCCESSFUL_FALSE;
        }

        public static bool TryMoveCmd2(out Move out_move, string line)
        {
            // うしろに続く文字は☆（＾▽＾）
            int caret = 0;
            Util_String.TobasuTangoToMatubiKuhaku(line, ref caret, "move ");
            string line2 = line.Substring(caret).Trim();

            // move 912 といった数字かどうか☆（＾～＾）
            int ssSuji;
            if (int.TryParse(line2, out ssSuji))
            {
                out_move = (Move)ssSuji;
                return true;
            }

            // 数字でなければ、 move B2B3 といった文字列か☆（＾～＾）
            if (!LisPlay.MatchFenMove(PureSettei.fenSyurui, line, ref caret, out out_move))
            {
                out_move = Move.Toryo;
                return false;
            }

            return true;
        }
    }
}
