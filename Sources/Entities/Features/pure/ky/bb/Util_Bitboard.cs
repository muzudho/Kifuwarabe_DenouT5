#if DEBUG
using kifuwarabe_shogithink.pure.accessor;
using Grayscale.Kifuwarabi.Entities.Take1Base;
#else
using kifuwarabe_shogithink.pure.accessor;
using Grayscale.Kifuwarabi.Entities.Take1Base;
#endif

namespace kifuwarabe_shogithink.pure.ky.bb
{
    public abstract class Util_Bitboard
    {
        /// <summary>
        /// ヌルにするのではなく、使える状態にすること。
        /// </summary>
        /// <param name="bbHairetu"></param>
        public static void ClearBitboards(Bitboard[] bbHairetu)
        {
            for (int i = 0; i < bbHairetu.Length; i++)
            {
                if (null == bbHairetu[i]) { bbHairetu[i] = new Bitboard(); }
                else { bbHairetu[i].Clear(); }
            }
        }

        public static void BitOff(ref long bitboard, long removeBit)
        {
            bitboard &= (long)(~0UL ^ (ulong)removeBit);// 立っているビットを降ろすぜ☆
        }

        /// <summary>
        /// 指定の升にいる駒を除く、味方全部の利き☆
        /// 
        /// 盤上の駒を指す場合、自分自身が動いてしまうので利きが変わってしまうので、
        /// 全部の利きを合成したＢＢが使えないので、代わりにこの関数を使うんだぜ☆（＾～＾）
        /// </summary>
        /// <param name="ky"></param>
        /// <param name="ms_nozoku">除きたい駒がいる升</param>
        /// <returns></returns>
        public static Bitboard CreateBBTebanKikiZenbu_1KomaNozoku( Masu ms_nozoku)
        {
            Bitboard kikiZenbuBB = new Bitboard();

            // 味方の駒（変数使いまわし）
            Bitboard mikataBB = new Bitboard();

            foreach (Komasyurui ks in Conv_Komasyurui.itiran)
            {
                Piece km_teban = Med_Koma.KomasyuruiAndTaikyokusyaToKoma(ks, PureMemory.kifu_teban);
                PureMemory.gky_ky.shogiban.yomiIbashoBan_yoko.ToSet_Koma(km_teban, mikataBB);
                Masu ms;
                while (mikataBB.Ref_PopNTZ(out ms))
                {
                    if (ms_nozoku != ms)//この駒を除く
                    {
                        BitboardsOmatome.KomanoUgokikataYk00.ToStandup_Merge( km_teban, ms, kikiZenbuBB);
                    }
                }

            }
            return kikiZenbuBB;
        }
    }
}
