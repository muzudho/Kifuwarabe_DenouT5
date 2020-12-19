#if DEBUG
using kifuwarabe_shogithink.pure;
using kifuwarabe_shogithink.pure.accessor;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.listen.genkyoku;
using kifuwarabe_shogithink.pure.logger;
using kifuwarabe_shogithink.pure.speak.play;
using kifuwarabe_shogiwin.speak.ban;
#else
using kifuwarabe_shogithink.pure;
using kifuwarabe_shogithink.pure.accessor;
using kifuwarabe_shogithink.pure.logger;
using kifuwarabe_shogithink.pure.speak.play;
#endif

namespace kifuwarabe_shogiwin.speak
{
    public static class SpkKifu_WinConsole
    {
        /// <summary>
        /// 棋譜データをそのままダンプ★（＾～＾）
        /// </summary>
        /// <param name="kf"></param>
        /// <param name="isSfen"></param>
        /// <param name="hyoji"></param>
        /// <returns></returns>
        public static bool Try_SetumeiAll( IHyojiMojiretu hyoji)
        {
            hyoji.AppendLine(string.Format("棋譜カーソル： 手目={0} 手番={1}",
                PureMemory.kifu_endTeme,
                PureMemory.kifu_teban
                ));

            MoveGenAccessor.ScanKifu_0ToPreTeme((int iKifu, ref bool toBreak) =>
            {
                hyoji.AppendLine(ToString_ByTeme(iKifu));
            });
            return true;
        }
        public static bool Try_SetumeiTebanAll(IHyojiMojiretu hyoji)
        {
            hyoji.AppendLine("手番、最初の数件：");
            for (int iTeme=0; iTeme<10 && iTeme<PureMemory.KIFU_SIZE; iTeme++)
            {
                hyoji.AppendLine( PureMemory.ToString_KifuCursor(iTeme));
            }
            return true;
        }

        /// <summary>
        /// 棋譜データをそのままダンプ★（＾～＾）
        /// </summary>
        /// <param name="isSfen"></param>
        /// <param name="hyoji"></param>
        /// <returns></returns>
        public static string ToString_ByTeme(int iTeme)
        {
            return string.Format("curr[{0,3}] next: ss={1} ssType={2} cap={3}",
                iTeme,
                SpkMove.ToString_Fen(PureSettei.fenSyurui, PureMemory.kifu_moveArray[iTeme]),
                PureMemory.kifu_moveTypeArray[iTeme],
                PureMemory.kifu_toraretaKsAr[iTeme]
                );
        }
    }
}
