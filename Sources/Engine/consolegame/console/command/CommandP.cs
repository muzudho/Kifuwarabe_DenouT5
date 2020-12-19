#if DEBUG
using kifuwarabe_shogithink.pure.accessor;
using kifuwarabe_shogithink.pure.control;
using kifuwarabe_shogithink.pure.listen;
using kifuwarabe_shogithink.pure.listen.genkyoku;
using kifuwarabe_shogithink.pure.logger;
using kifuwarabe_shogiwin.consolegame.machine;
using System;
using kifuwarabe_shogithink.pure.com;
#else
using kifuwarabe_shogithink.pure.com;
using kifuwarabe_shogithink.pure;
using kifuwarabe_shogithink.pure.accessor;
using kifuwarabe_shogithink.pure.control;
using kifuwarabe_shogithink.pure.listen;
using kifuwarabe_shogithink.pure.listen.genkyoku;
using kifuwarabe_shogithink.pure.logger;
using kifuwarabe_shogithink.pure.speak.genkyoku;
using kifuwarabe_shogiwin.consolegame.machine;
using System;
using Grayscale.Kifuwarabi.Entities.Logging;
#endif

namespace kifuwarabe_shogiwin.consolegame.console.command
{
    public static class CommandP
    {
        /// <summary>
        /// 初期局面と、そこからの棋譜をセットするものだぜ☆（＾～＾）
        /// </summary>
        /// <param name="f"></param>
        /// <param name="line"></param>
        /// <param name="hyoji"></param>
        public static void Position(FenSyurui f, string line, IHyojiMojiretu hyoji)
        {
            int caret = 0;
            if (Util_String.MatchAndNext("position", line, ref caret))
            {
                // 初期局面を変更するぜ☆！（＾▽＾）
                if (LisGenkyoku.TryFail_MatchPositionvalue(
                    f,
                    line, ref caret, out string moves
#if DEBUG
                    , (IDebugMojiretu)hyoji
#endif
                    ))
                {
                    hyoji.AppendLine("パースに失敗だぜ☆（＾～＾）！ #黒牛");
                    // 例外で出力したいので、フラッシュされる前に退避だぜ☆（＾～＾）
                    string msg = hyoji.ToContents();
                    Logger.Flush(hyoji);
                    throw new Exception(msg);
                }


                if ("" != moves)
                {
                    // moves が続いていたら☆（＾～＾）

                    // 頭の moves を取り除くぜ☆（*＾～＾*）
                    moves = moves.Substring("moves ".Length);
#if DEBUG
                    hyoji.AppendLine(string.Format("棋譜再生 moves={0}",
                        moves
                        ));
#endif

                    // 「手目」が最後まで進んでしまうぜ☆（＾～＾）
                    MoveGenAccessor.Tukurinaosi_RemakeKifuByMoves(moves);

                    // 棋譜の通り指すぜ☆（＾～＾）
                    if (!MoveGenAccessor.Try_PlayMoves_0ToPreTeme(f, hyoji))
                    {
                        Logger.Flush(hyoji);
                        throw new Exception(hyoji.ToContents());
                    }
                }

                // 初回は「position startpos」しか送られてこない☆（＾～＾）
            }
        }

        public static void PreGo(string line, IHyojiMojiretu hyoji)
        {
            Util_Tansaku.PreGo();
        }
    }
}
