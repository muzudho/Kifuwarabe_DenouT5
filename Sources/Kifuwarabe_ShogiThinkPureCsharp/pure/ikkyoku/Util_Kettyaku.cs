#if DEBUG
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.logger;
using kifuwarabe_shogithink.pure.move;
using kifuwarabe_shogithink.pure.accessor;
#else
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.move;
#endif

namespace kifuwarabe_shogithink.pure.ikkyoku
{
    public abstract class Util_Kettyaku
    {
        /// <summary>
        /// 決着判定
        /// </summary>
        /// <param name="bestSasite">投了かどうか調べるだけだぜ☆（＾▽＾）</param>
        public static bool Try_JudgeKettyaku(Move bestSasite
#if DEBUG
            , IHyojiMojiretu hyoji
#endif
            )
        {
            Taikyokusya tb2 = Conv_Taikyokusya.Hanten(PureMemory.kifu_teban);
            if (Move.Toryo == bestSasite)
            {
                switch (PureMemory.kifu_teban)// 投了した時点で、次の手番に移っているぜ☆
                {
                    case Taikyokusya.T2:
                        // 対局者１が投了して、対局者２の手番になったということだぜ☆
                        // だから対局者２の勝ちだぜ☆
                        PureMemory.gky_kekka = TaikyokuKekka.Taikyokusya2NoKati; break;
                    case Taikyokusya.T1: PureMemory.gky_kekka = TaikyokuKekka.Taikyokusya1NoKati; break;
                    default:
#if DEBUG
                        hyoji.AppendLine("未定義の手番");
#endif
                        return false;
                }
            }
            // トライルール
            else if (Util_TryRule.IsTried(
                tb2//手番が進んでいるので、相手番のトライを判定☆
                )
                )
            {
                switch (tb2)
                {
                    case Taikyokusya.T1: PureMemory.gky_kekka = TaikyokuKekka.Taikyokusya1NoKati; break;
                    case Taikyokusya.T2: PureMemory.gky_kekka = TaikyokuKekka.Taikyokusya2NoKati; break;
                    default:
#if DEBUG
                        hyoji.AppendLine("未定義の手番");
#endif
                        return false;
                }
            }
            else
            {
                // らいおんがいるか☆
                bool raion1Vanished = PureMemory.gky_ky.shogiban.ibashoBan_yk00.yomiIbashoBan.IsEmptyKoma(Koma.R);
                bool raion2Vanished = PureMemory.gky_ky.shogiban.ibashoBan_yk00.yomiIbashoBan.IsEmptyKoma(Koma.r);

                if (raion1Vanished && raion2Vanished)
                {
                    // らいおんが２匹ともいない場合（エラー）
                    PureMemory.gky_kekka = TaikyokuKekka.Hikiwake;
                }
                else if (raion2Vanished)
                {
                    PureMemory.gky_kekka = TaikyokuKekka.Taikyokusya1NoKati;
                }
                else if (raion1Vanished)
                {
                    PureMemory.gky_kekka = TaikyokuKekka.Taikyokusya2NoKati;
                }
            }

            return true;
        }
    }
}
