#if DEBUG
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.ky.bb;

using System;
using System.Diagnostics;
#else
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.ky.bb;
using System;
using System.Diagnostics;
#endif


namespace kifuwarabe_shogithink.pure.com.MoveOrder
{
    /// <summary>
    /// 一手詰め判定
    /// </summary>
    public static class Tume1Hantei
    {
        public static bool CheckEnd_Tume1()
        {
            if (PureSettei.ittedumeTukau && PureMemory.ssss_genk_tume1)
            {
                PureMemory.SetTansakuUtikiri(TansakuUtikiri.RaionTukamaeta);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 一手詰めの局面かどうか調べるぜ☆（＾▽＾）
        /// 
        /// 自分が一手詰めを掛けられるから、この指し手は作らないでおこう、といった使い方がされるぜ☆（＾▽＾）
        /// 
        /// FIXME: 持ち駒の打ちにも使えないか☆（＾～＾）？
        /// </summary>
        /// <param name="fukasa"></param>
        /// <param name="ky2"></param>
        /// <param name="ms_src">持ち駒の場合、エラー値</param>
        /// <param name="mks">持ち駒の場合、持駒の種類</param>
        /// <param name="ms_t1"></param>
        /// <param name="tebanHioute"></param>
        /// <returns></returns>
        public static bool CheckBegin_Ittedume_MotiKoma()
        {
            if (!PureSettei.ittedumeTukau)
            {
                return false;
            }

            if (!Conv_Motigoma.IsOk(PureMemory.ssss_mot_mg))
            {
                throw new Exception("持ち駒じゃないじゃないか☆（＾▽＾）ｗｗｗ");
            }

            // Ａ Ｂ  Ｃ
            //　┌──┬──┬──┐
            //１│　　│▽ら│　　│
            //　├──┼──┼──┤
            //２│▲き│　　│▲き│
            //　├──┼──┼──┤
            //３│　　│▲に│▲ら│
            //　├──┼──┼──┤
            //４│▲ぞ│▲ひ│▲ぞ│
            //　└──┴──┴──┘

            // 盤上の駒を動かすのか、駒台の駒を打つのかによって、利き　の形が異なるぜ☆（＾～＾）
            Bitboard bb_kiki_t1 = BitboardsOmatome.KomanoUgokikataYk00.Clone_Merge(Med_Koma.MotiKomaToKoma(PureMemory.ssss_mot_mg), PureMemory.ssss_ugoki_ms_dst);

            if (!PureMemory.gky_ky.shogiban.yomiIbashoBan_yoko.IsIntersect(Med_Koma.ToRaion(PureMemory.kifu_aiteban),// 相手らいおんの場所☆
                bb_kiki_t1))// 相手らいおん　が、動かした駒の、利きの中に居ないんだったら、一手詰め　にはならないぜ☆（＾～＾）
            {
                // FIXME: ステイルメイトは考えてないぜ☆（＞＿＜）
                return false;
            }

            Koma km_t1 = Med_Koma.MotiKomaToKoma(PureMemory.ssss_mot_mg);//t0も同じ

            // FIXME: ↓駒移動後の、利きを取る必要がある
            Bitboard bb_kikiNewJibun = new Bitboard();
            {
                // 打による、重ね利きの数を差分更新するぜ☆（＾▽＾）
                //, ky.BB_KikiZenbu
                // 移動先の駒の利きを増やすぜ☆（＾▽＾）
                PureMemory.gky_ky.shogiban.kikiBan.OkuKiki(km_t1, PureMemory.ssss_ugoki_ms_dst);

                // こっちの利きを作り直し
                bb_kikiNewJibun = PureMemory.gky_ky.shogiban.kikiBan.RefBB_FromKikisuZenbuPositiveNumber(Med_Koma.MotiKomaToTaikyokusya(PureMemory.ssss_mot_mg));

                // 打による、重ね利きの数の差分更新を元に戻すぜ☆（＾▽＾）
                // 移動先の駒の利きを減らすぜ☆（＾▽＾）
                PureMemory.gky_ky.shogiban.kikiBan.TorinozokuKiki(km_t1, PureMemory.ssss_ugoki_ms_dst);
            }

            // 相手らいおんが逃げようとしていて。
            Bitboard bb1 = PureMemory.hot_bb_raion8KinboAr[PureMemory.kifu_nAiteban].Clone();// 相手らいおんの８近傍
            PureMemory.gky_ky.shogiban.ibashoBan_yk00.yomiIbashoBan.ToSitdown_KomaZenbu(PureMemory.kifu_aiteban, bb1);// 相手の駒がない升
            return 
            bb1.Sitdown(bb_kikiNewJibun)// こっちの利きがない升
            .IsEmpty();// 相手らいおん　が逃げれる升がない場合、詰み☆
        }

        /// <summary>
        /// 一手詰めの局面かどうか調べるぜ☆（＾▽＾）
        /// 
        /// 盤上の駒を動かすのか、駒台の駒を打つのかによって、利き　の形が異なるぜ☆（＾～＾）
        /// 
        /// 自分が一手詰めを掛けられるから、この指し手は作らないでおこう、といった使い方がされるぜ☆（＾▽＾）
        /// 
        /// FIXME: 成りを考慮してない
        /// </summary>
        /// <param name="ky2"></param>
        /// <param name="ms_t0">移動元。持ち駒の場合、エラー値</param>
        /// <param name="ms_t1">移動先</param>
        /// <param name="tebanHioute"></param>
        /// <returns></returns>
        public static bool CheckBegin_Tume1_BanjoKoma()
        {
            Debug.Assert(Conv_Masu.IsBanjo(PureMemory.ssss_ugoki_ms_dst), "升エラー");

            if (!PureSettei.ittedumeTukau)
            {
                return false;
            }

            // 動かす駒
            Komasyurui ks_t0;
            if (!PureMemory.gky_ky.yomiKy.yomiShogiban.yomiIbashoBan.ExistsKoma(PureMemory.kifu_teban, PureMemory.ssss_ugoki_ms_src, out ks_t0))
            {
                // エラー。盤上の駒ではないのかも☆（＾～＾）
                throw new Exception(Interproject.project.Owata("TryFail_Ittedume_BanjoKoma gky.ky.shogiban.ibashoBan.ExistsBBKoma(1)", PureMemory.tnsk_hyoji));
            }

            Koma km_t0 = Med_Koma.KomasyuruiAndTaikyokusyaToKoma(ks_t0, PureMemory.kifu_teban);
            Koma km_t1 = km_t0; // FIXME: 成りを考慮していないぜ☆（＞＿＜）

            // Ａ Ｂ  Ｃ
            //　┌──┬──┬──┐
            //１│　　│▽ら│　　│
            //　├──┼──┼──┤
            //２│▲き│　　│▲き│
            //　├──┼──┼──┤
            //３│　　│▲に│▲ら│
            //　├──┼──┼──┤
            //４│▲ぞ│▲ひ│▲ぞ│
            //　└──┴──┴──┘

            // 動かしたばかりの駒を　取り返されるようでは、一手詰めは成功しないぜ☆（＾～＾）（ステイルメイト除く）
            if (1 < PureMemory.gky_ky.shogiban.kikiBan.yomiKikiBan.CountKikisuZenbu(PureMemory.kifu_aiteban, PureMemory.ssss_ugoki_ms_dst))
            {
                // 移動先升は、相手らいおん　の利きも　１つ　あるはず。
                // 移動先升に　相手の利きが２つあれば、駒を取り返される☆
                return false;
            }

            if (!PureMemory.gky_ky.shogiban.yomiIbashoBan_yoko.IsIntersect(Med_Koma.ToRaion(PureMemory.kifu_aiteban),// 相手のらいおん
                    BitboardsOmatome.KomanoUgokikataYk00.Clone_Merge(km_t0, PureMemory.ssss_ugoki_ms_dst) // 移動先での駒の利き
                ))// 相手らいおん　が、移動先での駒の利きの中に居ないんだったら、一手詰め　にはならないぜ☆（＾～＾）
            {
                // FIXME: ステイルメイトは考えてないぜ☆（＞＿＜）
                return false;
            }

            Bitboard bb_idogoKikiNew = new Bitboard();// 移動後の、利き
            {
                // 盤上の重ね利きの数を差分更新するぜ☆（＾▽＾）
                // 移動元の駒の利きを消すぜ☆（＾▽＾）
                PureMemory.gky_ky.shogiban.kikiBan.TorinozokuKiki(km_t0, PureMemory.ssss_ugoki_ms_src);

                // 現局面より　利き　が減っているのが正しい


                // 移動先の駒の利きを増やすぜ☆（＾▽＾）
                PureMemory.gky_ky.shogiban.kikiBan.OkuKiki(km_t1, PureMemory.ssss_ugoki_ms_dst);

                // ここで、現行の利きは変更されているぜ☆（＾～＾）駒は移動していないので、再計算の駒配置は　現行の利きと異なるぜ☆（＾～＾）

                // 移動後の利きを作り直し
                bb_idogoKikiNew = PureMemory.gky_ky.shogiban.kikiBan.RefBB_FromKikisuZenbuPositiveNumber(PureMemory.kifu_teban);

                // 盤上の重ね利きの数の差分更新を元に戻すぜ☆（＾▽＾）
                {
                    // 移動先の駒の利きを減らすぜ☆（＾▽＾）
                    PureMemory.gky_ky.shogiban.kikiBan.TorinozokuKiki(km_t1, PureMemory.ssss_ugoki_ms_dst);

                    // 駒の利きを減らしているぜ☆（＾～＾）　駒は減っていないので、再計算すると結果が異なるぜ☆（＾～＾）

                    // 移動元の駒の利きを増やすぜ☆（＾▽＾）
                    PureMemory.gky_ky.shogiban.kikiBan.OkuKiki(km_t0, PureMemory.ssss_ugoki_ms_src);
                }
            }



            Bitboard bb1 = PureMemory.hot_bb_raion8KinboAr[PureMemory.kifu_nAiteban].Clone();// 相手らいおん　が逃げれる、相手らいおんの周りの空白
            PureMemory.gky_ky.shogiban.ibashoBan_yk00.yomiIbashoBan.ToSitdown_KomaZenbu(PureMemory.kifu_aiteban, bb1);// 相手の駒がない升
            return 
                bb1.Sitdown(bb_idogoKikiNew)// こっちの利きがない升
                .IsEmpty();// がない場合、詰み☆
            ;
        }

    }
}
