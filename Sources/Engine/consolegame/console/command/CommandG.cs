#if DEBUG
using kifuwarabe_shogithink.pure;
using kifuwarabe_shogithink.pure.com;
using kifuwarabe_shogithink.pure.control;
using kifuwarabe_shogithink.pure.ikkyoku;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.listen;
using kifuwarabe_shogithink.pure.logger;
using kifuwarabe_shogithink.pure.speak.play;
using kifuwarabe_shogiwin.consolegame.machine;
using kifuwarabe_shogiwin.speak.ban;
#else
using Grayscale.Kifuwarabi.Entities.Logging;
using kifuwarabe_shogithink.pure;
using kifuwarabe_shogithink.pure.com;
using kifuwarabe_shogithink.pure.control;
using kifuwarabe_shogithink.pure.ikkyoku;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.listen;
using kifuwarabe_shogithink.pure.logger;
using kifuwarabe_shogithink.pure.speak.play;
using kifuwarabe_shogiwin.consolegame.machine;
using kifuwarabe_shogiwin.speak.ban;
#endif

namespace kifuwarabe_shogiwin.consolegame.console.command
{
    public static class CommandG
    {
        public static void Gameover(string line, IHyojiMojiretu hyoji)
        {
            int caret = 0;
            if (Util_String.MatchAndNext("gameover", line, ref caret))
            {
                if (Util_String.MatchAndNext("lose", line, ref caret))
                {
                    // コンピューターは止めるぜ☆（*＾～＾*）次のイリーガルな指し手を指してしまうからなｗｗｗｗ☆（＾▽＾）
                    switch (PureMemory.kifu_teban)
                    {
                        case Taikyokusya.T1: PureSettei.p1Com = false; break;
                        case Taikyokusya.T2: PureSettei.p2Com = false; break;
                        default: break;
                    }
                }
                else
                {

                }
            }
            else
            {

            }
        }

        /// <summary>
        /// USI でも go を受信するぜ☆（＾～＾）
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="ky"></param>
        /// <param name="hyoji"></param>
        public static bool TryFail_Go(bool isUsi, FenSyurui f, CommandMode mode, IHyojiMojiretu hyoji)
        {
            Util_Tansaku.PreGo();
            if (Util_Tansaku.TryFail_Go(hyoji))
            {
                return Pure.FailTrue("PurePlay.Try_Go");
            }

            // 勝敗判定☆（＾▽＾）
            if (!Util_Kettyaku.Try_JudgeKettyaku(PureMemory.tnsk_kohoMove
#if DEBUG
                , hyoji
#endif
                ))
            {
                return Pure.FailTrue("Try_JudgeKettyaku");
            }

            if (isUsi)
            {
                Logger.Flush(hyoji);
                hyoji.Append("bestmove ");
                SpkMove.AppendFenTo(f, PureMemory.tnsk_kohoMove, hyoji);
                hyoji.AppendLine();
                Logger.Flush_USI(hyoji);
            }
            else if (mode == CommandMode.NigenYoConsoleKaihatu)
            {
                // 開発モードでは、指したあとに盤面表示を返すぜ☆（＾▽＾）
                SpkBan_1Column.Setumei_Kyokumen(PureMemory.kifu_endTeme, hyoji);
            }
            // ゲームモードでは表示しないぜ☆（＾▽＾）

            return Pure.SUCCESSFUL_FALSE;
        }

    }
}
