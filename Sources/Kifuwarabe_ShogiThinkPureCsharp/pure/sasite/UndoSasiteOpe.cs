#if DEBUG
using kifuwarabe_shogithink.pure.accessor;
using kifuwarabe_shogithink.pure.control;
using kifuwarabe_shogithink.pure.conv.genkyoku.play;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.logger;
using System.Diagnostics;
#else
using kifuwarabe_shogithink.pure.accessor;
using kifuwarabe_shogithink.pure.conv.genkyoku.play;
using kifuwarabe_shogithink.pure.ky;
using System.Diagnostics;
#endif


namespace kifuwarabe_shogithink.pure.sasite
{
    public static class UndoSasiteOpe
    {


        /// <summary>
        /// 移動先の手番の駒を取り除くぜ☆（＾▽＾）
        /// </summary>
        /// <returns></returns>
        public static bool TryFail_Tejun1_IdosakiNoTebanNoKomaWoTorinozoku(
#if DEBUG
            FenSyurui f
            , IDebugMojiretu dbg_reigai
#endif
            )
        {
            //────────────────────────────────────────
            //  Ｔ１　［１］      移動先に　手番の駒　が在る
            //────────────────────────────────────────

            //────────────────────────────────────────
            //  Ｔ１　［遷移］   移動先の　手番の駒　を除外する
            //────────────────────────────────────────

            // ハッシュ、駒割、二駒
            
            // TODO:駒割り評価値を減らすならここだぜ☆（＾～＾）

            // 駒を取り除く
            Debug.Assert(Conv_Koma.IsOk(PureMemory.umv_km_t1), "km_t1 can not remove");
            if (PureMemory.gky_ky.shogiban.TryFail_TorinozokuKoma(
                PureMemory.umv_ms_t1, // 移動先の升
                PureMemory.umv_km_t1, // 移動先の駒
                PureMemory.umv_ms_t0,// 移動元の升 (2017-05-02 22:44 Add) 未来に駒があるのは、元の場所なのでここなんだが☆（＾～＾）？
                        // 未指定の場合があるが、飛び利きに使ってるだけなんで関係無い☆（＾～＾）
                      // × ms_t1,
                      // × Sindan.MASU_ERROR,
                      // × ms_t0
                true                
#if DEBUG
                , dbg_reigai
#endif
                ))
            {
                return Pure.FailTrue("TryFail_Torinozoku(3)");
            }

            //────────────────────────────────────────
            //  Ｔ１　［２］     移動先に　手番の駒　が無い
            //────────────────────────────────────────
            return Pure.SUCCESSFUL_FALSE;
        }


        /// <summary>
        /// 移動元の盤の升に、手番の駒を戻すぜ☆（＾▽＾）
        /// </summary>
        /// <returns></returns>
        public static bool TryFail_Tejun2Sasu_IdomotoniTebannoKomawoModosu(
#if DEBUG
            FenSyurui dbg_f
            , IDebugMojiretu dbg_reigai
#endif
            )
        {
            //────────────────────────────────────────
            //  Ｔ０  ［１］     移動元に　駒　が無い
            //────────────────────────────────────────

            //────────────────────────────────────────
            //  Ｔ０  ［遷移］    移動元に　駒　が現れる☆
            //────────────────────────────────────────
            {
                // ハッシュを差分更新

                if (PureMemory.gky_ky.shogiban.TryFail_OkuKoma(// 手順２ 移動元に手番の駒を戻す
                    PureMemory.umv_ms_t0, PureMemory.umv_km_t0, true
#if DEBUG
                    , dbg_reigai
#endif
                    ))
                {
                    return Pure.FailTrue("undo-tj2s gky.ky.shogiban.Try_Oku");
                }
                // TODO: 駒割り評価値を増やすならここだぜ☆（＾～＾）
            }

            //────────────────────────────────────────
            //  Ｔ０  ［２］     移動元に　駒　が在る
            //────────────────────────────────────────
            return Pure.SUCCESSFUL_FALSE;
        }
        /// <summary>
        /// 移動元の盤の升に、手番の駒を戻すぜ☆（＾▽＾）
        /// </summary>
        /// <returns></returns>
        public static bool TryFail_Tejun2Utu_IdomotoniTebannoKomawoModosu(
#if DEBUG
            FenSyurui dbg_f
            , IDebugMojiretu dbg_reigai
#endif
            )
        {
            //────────────────────────────────────────
            //  Ｔ０  ［１］     移動元に　駒　が無い
            //────────────────────────────────────────

            //────────────────────────────────────────
            //  Ｔ０  ［遷移］    移動元に　駒　が現れる☆
            //────────────────────────────────────────
            {
                // 今の駒台の駒数は消える ※駒台だけ、このステップが多い
                PureMemory.gky_ky.motigomaItiran.Fuyasu(PureMemory.umv_mk_t0);

                // TODO: 駒割り評価値を増やすならここだぜ☆（＾～＾）
            }

            //────────────────────────────────────────
            //  Ｔ０  ［２］     移動元に　駒　が在る
            //────────────────────────────────────────

            return Pure.SUCCESSFUL_FALSE;
        }


        /// <summary>
        /// 駒台から、取った駒を除外するぜ☆（＾▽＾）
        /// </summary>
        /// <returns></returns>
        public static bool TryFail_Tejun3_KomadaiKaraTottakomawoJogai(
#if DEBUG
            FenSyurui dbg_f
            , IDebugMojiretu dbg_reigai
#endif
            )
        {
            if (Komasyurui.Yososu != PureMemory.umv_ks_c)
            {
                //------------------------------------------------------------
                // 取った駒を戻す
                //------------------------------------------------------------

                if (PureMemory.umv_ks_c != Komasyurui.R)// らいおん を盤に戻しても、持駒の数は変わらないぜ☆（＾▽＾）
                {
                    //────────────────────────────────────────
                    //  Ｃ   [１]      取ったあとの　持駒の数　の駒台が在る
                    //────────────────────────────────────────

                    //────────────────────────────────────────
                    //  Ｃ   [遷移]    取った　持駒　を除外する
                    //────────────────────────────────────────
                    // 消して

                    // 増やす
                    MotigomaItiran motiKomaItiranImpl;
                    if (!PureMemory.gky_ky.motigomaItiran.Try_Herasu(out motiKomaItiranImpl, PureMemory.umv_mk_c
#if DEBUG
                        , (IDebugMojiretu)dbg_reigai
#endif
                        ))
                    {
                        return Pure.FailTrue("Try_Herasu");
                    }

                    // TODO: 駒割り評価値を減らすならここだぜ☆（＾～＾）


                    //────────────────────────────────────────
                    // Ｃ   ［２］     戻したあとの　持駒の数　の駒台が在る
                    //────────────────────────────────────────
                }

            }
            return Pure.SUCCESSFUL_FALSE;
        }


        /// <summary>
        /// 移動先に、取った駒を戻すぜ☆（＾▽＾）
        /// </summary>
        /// <returns></returns>
        public static bool TryFail_Tejun4_IdosakiniTottakomawoModosu(
#if DEBUG
            FenSyurui dbg_f
            , IDebugMojiretu dbg_reigai
#endif
            )
        {
            if (Komasyurui.Yososu != PureMemory.umv_ks_c)
            {

                //────────────────────────────────────────
                // Ｔ１Ｃ  ［１］  移動先に、取っていた駒が現れる
                //────────────────────────────────────────

                //────────────────────────────────────────
                // Ｔ１Ｃ  ［遷移］     駒が増える
                //────────────────────────────────────────
                if (PureMemory.gky_ky.shogiban.TryFail_OkuKoma(//手順4 移動先に取った駒を戻す
                    PureMemory.umv_ms_t1, PureMemory.umv_km_c, true
#if DEBUG
                    , dbg_reigai
#endif
                    ))
                {
                    return Pure.FailTrue("gky.ky.shogiban.Try_Oku");
                }

                // TODO: 駒割り評価値を増やすならここだぜ☆（＾～＾）


                //────────────────────────────────────────
                // Ｔ１Ｃ  ［２］
                //────────────────────────────────────────
            }

            return Pure.SUCCESSFUL_FALSE;
        }

        /// <summary>
        /// 指定した指し手をやりなおす動きをするぜ☆（＾▽＾）
        /// </summary>
        /// <param name="ss"></param>
        public static bool TryFail_UndoSasite(
#if DEBUG
            FenSyurui dbg_f
            , IDebugMojiretu dbg_reigai
#endif
            )
        {
            //────────────────────────────────────────
            // 手番
            //────────────────────────────────────────
            // 事前に戻すぜ☆（＾▽＾）
            PureMemory.RemoveTeme();

            //────────────────────────────────────────
            // まず最初に整合性を確認だぜ☆（＾～＾）
            //────────────────────────────────────────

            //────────────────────────────────────────
            // グローバル変数に、結果を入れておくぜ☆（＾～＾）
            //────────────────────────────────────────
            SasiteSeiseiAccessor.BunkaiSasite_Umv();

            if (Sasite.Toryo == PureMemory.umv_ss) { goto gt_EndMethod; }// なにも更新せず終了☆（＾▽＾）


            if (TryFail_Tejun1_IdosakiNoTebanNoKomaWoTorinozoku(
#if DEBUG
                dbg_f
                , dbg_reigai
#endif
                ))
            {
                return Pure.FailTrue("TryFail_Tejun1_IdosakiNoTebanNoKomaWoTorinozoku");
            }

            if (Conv_Sasite.IsUtta(PureMemory.umv_ss))
            {
                // 打つ
                if (TryFail_Tejun2Utu_IdomotoniTebannoKomawoModosu(
#if DEBUG
                    dbg_f
                    , dbg_reigai
#endif
                ))
                {
                    return Pure.FailTrue("TryFail_Tejun2_IdomotoniTebannoKomawoModosu");
                }
            }
            else
            {
                // 指す
                if (TryFail_Tejun2Sasu_IdomotoniTebannoKomawoModosu(
#if DEBUG
                dbg_f
                , dbg_reigai
#endif
                ))
                {
                    return Pure.FailTrue("TryFail_Tejun2_IdomotoniTebannoKomawoModosu");
                }
            }


            if (TryFail_Tejun3_KomadaiKaraTottakomawoJogai(
#if DEBUG
                dbg_f
                , dbg_reigai
#endif
                ))
            {
                return Pure.FailTrue("TryFail_Tejun3_KomadaiKaraTottakomawoJogai");
            }

            if (TryFail_Tejun4_IdosakiniTottakomawoModosu(
#if DEBUG
                dbg_f
                , dbg_reigai
#endif
                ))
            {
                return Pure.FailTrue("TryFail_Tejun4_IdosakiniTottakomawoModosu");
            }

            //────────────────────────────────────────
            // 最後に一括更新
            //────────────────────────────────────────

            gt_EndMethod:
            //────────────────────────────────────────
            // 最後に整合性を確認だぜ☆（＾～＾）
            //────────────────────────────────────────
            return Pure.SUCCESSFUL_FALSE;
        }

    }
}
