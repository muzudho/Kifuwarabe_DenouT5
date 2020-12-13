#if DEBUG
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.ky.bb;
#else
using kifuwarabe_shogithink.pure.accessor;
using kifuwarabe_shogithink.pure.com.MoveOrder.hioute;
using kifuwarabe_shogithink.pure.genkyoku;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.ky.bb;
#endif

namespace kifuwarabe_shogithink.pure.com.MoveOrder.seisei
{
    /// <summary>
    /// 経験
    /// 逃げ道を上げ渡してしまう緩手
    /// </summary>
    public static class NigemitiWatasuKansyu
    {
        /// <summary>
        /// 相手番らいおん　の逃げ道を開けてしまう、手番側の悪手かどうか調べるぜ☆（＾▽＾）
        /// </summary>
        /// <returns></returns>
        public static bool IsNigemitiWoAkeru()
        {
            if (PureMemory.hot_bb_nigemitiWoFusaideiruAiteNoKomaAr[PureMemory.kifu_nAiteban].IsOff(PureMemory.ssss_ugoki_ms_src))
            {
                // 逃げ道を塞いでいる駒ではないのなら、スルーするぜ☆（＾▽＾）
                return false;//正常
            }


            // 手番らいおん　の８近傍　のどこかに、重ね利きの数　０　が出来ていれば、
            // 逃げ道を開けると判定するぜ☆（＾▽＾）
            bool akeru = false;
            Koma km0_teban = Med_Koma.KomasyuruiAndTaikyokusyaToKoma(PureMemory.ssss_ugoki_ks, PureMemory.kifu_teban);
            Koma km1_teban = km0_teban;// FIXME: 成りを考慮していない


            //────────────────────────────────────────
            // （１）利き表をいじる前☆（＾▽＾）
            //────────────────────────────────────────

            // 2回以上使う
            Bitboard bbConst_kiki0 = BitboardsOmatome.KomanoUgokikataYk00.Clone_Merge(km0_teban, PureMemory.ssss_ugoki_ms_src);
            Bitboard bbVar_kiki = bbConst_kiki0.Clone();
            if (!bbVar_kiki.IsEmpty())
            {
                //────────────────────────────────────────
                // （２）動く前の、駒の利きを、利き表から減らすぜ☆（＾▽＾）
                //────────────────────────────────────────
                //ビットボード使い回し
                PureMemory.gky_ky.shogiban.kikiBan.TorinozokuKiki(km0_teban, bbVar_kiki);
                // FIXME: ここで　利きの数が減っている必要がある
            }

            Bitboard kikiBB1_const = BitboardsOmatome.KomanoUgokikataYk00.Clone_Merge(km1_teban, PureMemory.ssss_ugoki_ms_dst);
            if (!kikiBB1_const.IsEmpty())
            {
                bbVar_kiki.Set(kikiBB1_const);
                //────────────────────────────────────────
                // （３）動いた後の、駒の動きを、利き表へ足すぜ☆（＾▽＾）
                //────────────────────────────────────────
                //ビットボード使い回し
                PureMemory.gky_ky.shogiban.kikiBan.OkuKiki(km1_teban, bbVar_kiki);
            }

            // 動いたことで、らいおんの逃げ道を塞いでいた駒が、らいおんの逃げ道を空けてしまうか☆（＾～＾）
            Bitboard nigemitiBB = new Bitboard();
            nigemitiBB.Set(PureMemory.hot_bb_raion8KinboAr[PureMemory.kifu_nAiteban]);
            PureMemory.gky_ky.shogiban.ibashoBan_yk00.yomiIbashoBan.ToSitdown_KomaZenbu(PureMemory.kifu_aiteban, nigemitiBB);

            Masu ms_nigemiti;
            while (nigemitiBB.Ref_PopNTZ(out ms_nigemiti))
            {
                // 手番の利きが無くなったか☆（＾▽＾）
                if (0 == PureMemory.gky_ky.yomiKy.yomiShogiban.yomiKikiBan.CountKikisuZenbu(PureMemory.kifu_teban, ms_nigemiti))
                {

                    akeru = true; // （＾▽＾）相手らいおんの逃げ道が開いたぜ☆！
                    goto gt_EndLoop;
                }
            }
            gt_EndLoop:
            ;

            bbVar_kiki.Set(kikiBB1_const);
            if (!bbVar_kiki.IsEmpty())
            {
                //────────────────────────────────────────
                // （４）増やした重ね利きの数を減らして、元に戻すぜ☆（＾▽＾）
                //────────────────────────────────────────
                //ビットボード使い回し
                PureMemory.gky_ky.shogiban.kikiBan.TorinozokuKiki(km1_teban, bbVar_kiki);
            }

            // 利き数は、まだ戻している途中

            bbVar_kiki.Set(bbConst_kiki0);
            if (!bbVar_kiki.IsEmpty())
            {
                //────────────────────────────────────────
                // （５）減らした重ね利きの数を増やして、元に戻すぜ☆（＾▽＾）
                //────────────────────────────────────────
                //ビットボード使い回し
                PureMemory.gky_ky.shogiban.kikiBan.OkuKiki(km0_teban, bbVar_kiki);
            }

            //────────────────────────────────────────
            // ここで、利きの数は現局面と合ってるはずだぜ☆（＾～＾）
            //────────────────────────────────────────
            return akeru;
        }
    }
}
