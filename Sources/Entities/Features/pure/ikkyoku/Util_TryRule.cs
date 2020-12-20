#if DEBUG
using kifuwarabe_shogithink.pure.project;
using kifuwarabe_shogithink.pure.genkyoku;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.ky.bb;

using kifuwarabe_shogithink.pure.accessor;
using System;
#else
using kifuwarabe_shogithink.pure.accessor;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.ky.bb;
using System;
#endif

namespace kifuwarabe_shogithink.pure.ikkyoku
{
    public abstract class Util_TryRule
    {
        static Util_TryRule()
        {
            m_trySakiBB_ = new Bitboard();
        }

        /// <summary>
        /// トライしていれば真☆
        /// </summary>
        /// <returns></returns>
        public static bool IsTried(Taikyokusya ts)
        {
            Koma km = Med_Koma.KomasyuruiAndTaikyokusyaToKoma(Komasyurui.R, ts);
            switch (ts)
            {
                case Taikyokusya.T1: return PureMemory.gky_ky.yomiKy.yomiShogiban.yomiIbashoBan.ToIsIntersect_Koma(km, BitboardsOmatome.bb_danArray[0]);
                case Taikyokusya.T2: return PureMemory.gky_ky.yomiKy.yomiShogiban.yomiIbashoBan.ToIsIntersect_Koma(km, BitboardsOmatome.bb_danArray[PureSettei.banTateHaba - 1]);
                default: throw new Exception("未定義の手番");
            }
        }
        /// <summary>
        /// トライできる先。
        /// </summary>
        /// <param name="ky">局面</param>
        /// <param name="kikiBB">手番らいおんの利きビットボード</param>
        /// <param name="tb">手番</param>
        /// <param name="ms1">手番らいおんがいる升</param>
        /// <returns></returns>
        public static Bitboard GetTrySaki(Bitboard kikiBB, Masu ms1)
        {
#if DEBUG
            Util_Test.AppendLine("テスト：　トライルール", Pure.Sc.dbgMojiretu);
#endif
            m_trySakiBB_.Clear();

            // 自分はＮ段目にいる☆
            int dan = Conv_Masu.ToDanO1_JibunSiten(PureMemory.kifu_teban, ms1);
            bool nidanme = 2 == dan;
#if DEBUG
            Util_Test.AppendLine("２段目にいるか☆？[" + nidanme + "]　わたしは[" + dan + "]段目にいるぜ☆", Pure.Sc.dbgMojiretu);
#endif
            if (!nidanme)
            {
#if DEBUG
                Util_Test.AppendLine("むりだぜ☆", Pure.Sc.dbgMojiretu);
                //Util_Test.Flush(reigai1);
#endif
                return m_trySakiBB_;
            }

            // １段目に移動できる升☆

            m_trySakiBB_.Set(kikiBB);
            m_trySakiBB_.Siborikomi(BitboardsOmatome.bb_try[PureMemory.kifu_nTeban]);

#if DEBUG
            Interproject.project.Dbg_TryRule1(kikiBB, m_trySakiBB_);
#endif

            // 味方の駒がないところ☆
            Bitboard spaceBB = new Bitboard();
            spaceBB.Set(BitboardsOmatome.bb_boardArea);
            PureMemory.gky_ky.shogiban.ibashoBan_yk00.yomiIbashoBan.ToSitdown_KomaZenbu(PureMemory.kifu_teban, spaceBB);
            m_trySakiBB_.Siborikomi( spaceBB);

#if DEBUG
            Interproject.project.Dbg_TryRule2(spaceBB, m_trySakiBB_);
#endif
            if (m_trySakiBB_.IsEmpty())
            {
#if DEBUG
                Util_Test.AppendLine("むりだぜ☆", Pure.Sc.dbgMojiretu);
                //Util_Test.Flush(reigai1);
#endif
                return m_trySakiBB_;
            }

            // 相手の利きが届いていないところ☆
            Bitboard safeBB = new Bitboard();
            safeBB.Set(BitboardsOmatome.bb_boardArea);
            PureMemory.gky_ky.yomiKy.yomiShogiban.yomiKikiBan.ToSitdown_BBKikiZenbu(PureMemory.kifu_aiteban, safeBB);
            m_trySakiBB_.Siborikomi( safeBB);

#if DEBUG
            Interproject.project.Dbg_TryRule3(safeBB, m_trySakiBB_);
#endif
            if (m_trySakiBB_.IsEmpty())
            {
#if DEBUG
                Util_Test.AppendLine("むりだぜ☆", Pure.Sc.dbgMojiretu);
                //Util_Test.Flush(reigai1);
#endif
                return m_trySakiBB_;
            }

#if DEBUG
            Util_Test.AppendLine("トライできるぜ☆", Pure.Sc.dbgMojiretu);
            //Util_Test.Flush(reigai1);
#endif

            return m_trySakiBB_;
        }
        static Bitboard m_trySakiBB_;
    }
}
