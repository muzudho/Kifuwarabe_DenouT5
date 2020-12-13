#if DEBUG
using kifuwarabe_shogithink.pure.accessor;
using kifuwarabe_shogithink.pure.control;
using kifuwarabe_shogithink.pure.conv.genkyoku.play;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.logger;
using System.Diagnostics;
#else
using kifuwarabe_shogithink.pure.control;
using kifuwarabe_shogithink.pure.accessor;
using kifuwarabe_shogithink.pure.conv.genkyoku.play;
using kifuwarabe_shogithink.pure.ikkyoku;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.move;
using System.Diagnostics;
#endif

namespace kifuwarabe_shogithink.pure.move
{
    public static class DoMoveOpe
    {


        /// <summary>
        /// 指したあとの、次の局面へと更新するぜ☆
        /// ハッシュも差分変更するぜ☆
        /// 
        /// 手番を進める処理は、分けるぜ☆（＾～＾）
        /// </summary>
        /// <param name="ss">指し手☆</param>
        public static bool TryFailDoMoveAll(
            Move ss,
            MoveType ssType
#if DEBUG
            , FenSyurui f
            , IDebugMojiretu reigai1
            , bool isAssertYokusei // 駒の取り合いは呼び出し回数が多いので、アサートを抑制したいときに真
            , string hint
#endif
            )
        {
#if DEBUG
            isAssertYokusei = false;//FIXME:
#endif



            // 投了なら、なにも更新せず終了☆（＾▽＾）
            if (Move.Toryo == ss)
            {
                PureMemory.dmv_ks_c = Komasyurui.Yososu;
                goto gt_EndMethod;
            }
            MoveGenAccessor.BunkaiMoveDmv(ss);

            Debug.Assert(Conv_Koma.IsOk(PureMemory.dmv_km_t0), string.Format("Ｄｏ km_t0={0}", PureMemory.dmv_km_t0));
            Debug.Assert(Conv_Koma.IsOk(PureMemory.dmv_km_t1), "Ｄｏ");
            Debug.Assert(Conv_Masu.IsBanjoOrError(PureMemory.dmv_ms_t1), "");
            Debug.Assert(Conv_Koma.IsOkOrKuhaku(PureMemory.dmv_km_c), "Ｄｏ");

            if (AbstractConvMove.IsUtta(ss))
            {
                // 打った場合☆（＾～＾）

                // 駒台から駒を減らすんだぜ☆（＾～＾）
                if (TryFail_DaiOff(
                    PureMemory.dmv_ms_t0,// 打ち、の場合は使わないので、エラー値を入れておく
                    PureMemory.dmv_km_t0,// 打つ駒
                    PureMemory.dmv_mk_t0,// 持駒
                    PureMemory.dmv_ms_t1 // 移動先升

#if DEBUG
                    , f
                    , reigai1
#endif
                ))
                {
                    return Pure.FailTrue("TryFail_Tejun3_IdomotoJibunnoKomaTorinozoku");
                }
            }
            else
            {
                // 盤上の駒を動かした場合☆（＾～＾）


                // 移動先に駒があれば取る
                if (CanDstOff(PureMemory.dmv_km_c))
                {
                    if (TryFail_DstOff(
                        PureMemory.dmv_ms_t1, // 移動先升
                        PureMemory.dmv_km_c,// あれば、移動先の相手の駒（取られる駒; capture）
                        PureMemory.dmv_ks_c
#if DEBUG
                        , f
                        , reigai1
#endif
                    ))
                    {
                        return Pure.FailTrue("TryFail_Tejun1_IdosakiNoKomaWoToru");
                    }

                    // 取った駒が有れば駒台に増やすぜ☆（＾～＾）
                    if (TryFail_DaiOn(
                        PureMemory.dmv_km_c,// あれば、移動先の相手の駒（取られる駒; capture）
                        PureMemory.dmv_ks_c,
                        PureMemory.dmv_mk_c
#if DEBUG
                        , f
                        , reigai1
#endif
                    ))
                    {
                        return Pure.FailTrue("TryFail_Tejun2_TottaKomaWoKomadainiOku");
                    }
                }


                // 移動元から自分の駒を取り除くぜ☆（＾～＾）
                if (TryFail_SrcOff(
                    ss,
                    PureMemory.dmv_ms_t0,
                    PureMemory.dmv_km_t0,
                    PureMemory.dmv_mk_t0,
                    PureMemory.dmv_ms_t1 // 移動先升

    #if DEBUG
                    , f
                    , reigai1
    #endif
                ))
                {
                    return Pure.FailTrue("TryFail_Tejun3_IdomotoJibunnoKomaTorinozoku");
                }
            }

            // 移動先に手番の駒を置くぜ☆（＾～＾）
            if (TryFail_DstOn(
                PureMemory.dmv_ms_t0,
                PureMemory.dmv_km_t1,
                PureMemory.dmv_ms_t1 // 移動先升

#if DEBUG
                , f
                , reigai1
#endif
                ))
            {
                return Pure.FailTrue("TryFail_Tejun4_IdosakiNiTebanonKomawoOku");
            }


            //────────────────────────────────────────
            // 最後に診断
            //────────────────────────────────────────
            gt_EndMethod:
            return Pure.SUCCESSFUL_FALSE;
        }


        /// <summary>
        /// 移動元の自分の駒台から、駒を取るぜ☆（＾▽＾）
        /// </summary>
        /// <returns></returns>
        public static bool TryFail_DaiOff(
            Masu ms_t0,
            Koma km_t0,
            Motigoma mk_t0,
            Masu ms_t1 // 移動先升

#if DEBUG
            , FenSyurui f
            , IDebugMojiretu dbg_reigai
#endif
            )
        {
            //────────────────────────────────────────
            // Ｔ１   ［１］  移動元に　手番の駒　が在る
            //────────────────────────────────────────

            //────────────────────────────────────────
            // Ｔ１   ［遷移］    移動元の　手番の駒　を除外する
            //────────────────────────────────────────
            {

                MotigomaItiran motiKomaItiranImpl;
                if (!PureMemory.gky_ky.motigomaItiran.Try_Herasu(out motiKomaItiranImpl, mk_t0
#if DEBUG
                        , (IDebugMojiretu)dbg_reigai
#endif
                        ))
                {
                    return Pure.FailTrue("do-tj3u gky.ky.motiKomas.Try_Herasu");
                }

                // TODO: 駒割り評価値を減らすならここだぜ☆（＾～＾）

                // 駒台はこのステップが１つ多い
            }

            //DoMove1( isSfen, ss, ssType, ref konoTeme, syuturyoku, out gt_EndMethod);


            //────────────────────────────────────────
            // Ｔ１   ［２］      移動元に　手番の駒　が無い
            //────────────────────────────────────────
            return Pure.SUCCESSFUL_FALSE;
        }

        /// <summary>
        /// 移動先に駒があって、その駒を取ることができるか☆（＾～＾）？
        /// </summary>
        /// <returns></returns>
        public static bool CanDstOff(Koma km_c)
        {
            return km_c != Koma.Kuhaku;
        }

        /// <summary>
        /// 移動先の相手番の駒を取るぜ☆（＾▽＾）
        /// </summary>
        /// <returns></returns>
        public static bool TryFail_DstOff(
            Masu ms_t1, // 移動先升
            Koma km_c,// あれば、移動先の相手の駒（取られる駒; capture）
            Komasyurui ks_c // 取られた駒の種類
#if DEBUG
            , FenSyurui f
            , IDebugMojiretu dbg_reigai
#endif
            )
        {
            // 移動先に駒があるかどうかのチェックは先に終わらせておくこと☆（＾～＾）
            //────────────────────────────────────────
            // Ｔ２Ｃ  ［１］ 移動先に　相手の駒　が在る
            //────────────────────────────────────────

            //────────────────────────────────────────
            // Ｔ２Ｃ  ［遷移］    移動先の　相手の駒　を除外する
            //────────────────────────────────────────
            Debug.Assert(Conv_Koma.IsOk(km_c), "km_c can not remove");

            if (PureMemory.gky_ky.shogiban.TryFail_TorinozokuKoma(
                ms_t1,
                km_c,
                Conv_Masu.masu_error,
                true                
#if DEBUG
                , dbg_reigai
#endif
                ))
            {
                return Pure.FailTrue("TryFail_Torinozoku(1)");
            }

            //────────────────────────────────────────
            // Ｔ２Ｃ  ［２］     移動先に　相手の駒　が無い
            //────────────────────────────────────────

            // ビットボードの駒の数は合っていないからチェックしないぜ☆
            return Pure.SUCCESSFUL_FALSE;
        }


        /// <summary>
        /// 取った駒を、駒台に置くぜ☆（＾▽＾）
        /// </summary>
        /// <returns></returns>
        public static bool TryFail_DaiOn(
            Koma km_c,// あれば、移動先の相手の駒（取られる駒; capture）
            Komasyurui ks_c,
            Motigoma mk_c

#if DEBUG
            , FenSyurui f
            , IDebugMojiretu dbg_reigai
#endif
            )
        {
            //────────────────────────────────────────
            // 状況：
            //          移動先に駒があれば……。
            //────────────────────────────────────────
            #region 駒を取る
            if (km_c != Koma.Kuhaku)
            {
                // 駒取るぜ☆（＾▽＾）！

                // ただし、らいおんを除く
                if (ks_c != Komasyurui.R) // らいおん を取っても、持駒は増えないぜ☆
                {
                    //────────────────────────────────────────
                    // Ｃ    ［１］     取る前の　持ち駒が増える前の　駒台が在る
                    //────────────────────────────────────────

                    //────────────────────────────────────────
                    //  Ｃ    ［遷移］    取った持ち駒を増やす
                    //────────────────────────────────────────
                    // 取る前の持ち駒をリカウントする
                    // 増やす
                    PureMemory.gky_ky.motigomaItiran.Fuyasu(mk_c);

                    // TODO: 駒割りを増やすならここだぜ☆（＾～＾）


                    //────────────────────────────────────────
                    // Ｃ   ［３］  取った後の　持駒が１つ増えた　駒台が在る
                    //────────────────────────────────────────
                }
            }
            #endregion

            return Pure.SUCCESSFUL_FALSE;
        }

        /// <summary>
        /// 移動元の盤の升から、自分の駒を取るぜ☆（＾▽＾）
        /// </summary>
        /// <returns></returns>
        public static bool TryFail_SrcOff(
            Move ss,
            Masu ms_t0,
            Koma km_t0,
            Motigoma mk_t0,
            Masu ms_t1 // 移動先升
#if DEBUG
            , FenSyurui f
            , IDebugMojiretu dbg_reigai
#endif
            )
        {
            //────────────────────────────────────────
            // Ｔ１   ［１］  移動元に　手番の駒　が在る
            //────────────────────────────────────────

            //────────────────────────────────────────
            // Ｔ１   ［遷移］    移動元の　手番の駒　を除外する
            //────────────────────────────────────────
            {

                // TODO: 駒割りを評価値減らすならここだぜ☆（＾～＾）

                // FIXME: ここに問題のコードがあった★★★★★★★★★★★★★★★★★★★
            }

            //DoMove1( isSfen, ss, ssType, ref konoTeme, syuturyoku, out gt_EndMethod);


            // ローカル変数はグローバル変数に移動した。
            {
                // この下の HerasuBanjoKoma で指し手件数が動くようだ。

                // 盤上はこのステップが多い
                Debug.Assert(Conv_Koma.IsOk(km_t0), "km_t0 can not remove");
                if (PureMemory.gky_ky.shogiban.TryFail_TorinozokuKoma(
                    ms_t0, km_t0,
                    ms_t1, // (2017-05-02 22:19 Add)移動先の升（将来駒を置く升）を指定しておくぜ☆（＾～＾）
                    true
#if DEBUG
                    , dbg_reigai
#endif
                    ))
                {
                    return Pure.FailTrue("TryFail_Torinozoku(2)");
                }
                // 駒が無かった、というキャッシュは取らないぜ☆（＾▽＾）

                // この上の HerasuBanjoKoma で指し手件数が動くようだ。
            }

            //────────────────────────────────────────
            // Ｔ１   ［２］      移動元に　手番の駒　が無い
            //────────────────────────────────────────

            return Pure.SUCCESSFUL_FALSE;
        }


        /// <summary>
        /// 移動先に手番の駒を置くぜ☆（＾▽＾）
        /// </summary>
        /// <returns></returns>
        public static bool TryFail_DstOn(
            Masu ms_t0,
            Koma km_t1,
            Masu ms_t1 // 移動先升

#if DEBUG
            , FenSyurui f
            , IDebugMojiretu dbg_reigai
#endif
            )
        {
            //────────────────────────────────────────
            // Ｔ２    [１］    移動先に　手番の駒　が無い
            //────────────────────────────────────────

            //────────────────────────────────────────
            // Ｔ２    [遷移］   移動先に　手番の駒　を増やす
            //────────────────────────────────────────

            // FIXME:(2017-05-02 23:14)
            if (PureMemory.gky_ky.shogiban.TryFail_OkuKoma(//手順4 移動先に手番の駒を置く
                ms_t1, km_t1, true
#if DEBUG
                , dbg_reigai
#endif
                ))
            {
                return Pure.FailTrue("gky.ky.shogiban.Try_Oku");
            }

            //────────────────────────────────────────
            // Ｔ２   ［２］     移動先に　手番の駒　が在る
            //────────────────────────────────────────

            return Pure.SUCCESSFUL_FALSE;
        }

    }
}
