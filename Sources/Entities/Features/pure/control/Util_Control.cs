#if DEBUG
using kifuwarabe_shogithink.pure.accessor;
using kifuwarabe_shogithink.pure.ky.bb;
using kifuwarabe_shogithink.pure.listen;

using kifuwarabe_shogithink.pure.move;
#else
using System.Text;
using kifuwarabe_shogithink.pure.accessor;
using kifuwarabe_shogithink.pure.ky.bb;
using kifuwarabe_shogithink.pure.listen;

using kifuwarabe_shogithink.pure.move;
#endif

namespace kifuwarabe_shogithink.pure.control
{
    public static class Util_Control
    {
        /// <summary>
        /// ルール更新
        /// </summary>
        /// <returns></returns>
        public static void UpdateRule(
#if DEBUG
            string dbg_hint // どこで駒の動き方を初期化しようとしたか調べるもの
#endif
            )
        {
            //────────────────────────────────────────
            // 盤のサイズを作り直すぜ☆（＾～＾）
            //────────────────────────────────────────

            if (PureSettei.banTateHabaOld != PureSettei.banTateHaba
                ||
                PureSettei.banYokoHabaOld != PureSettei.banYokoHaba
            )
            {
                // 盤のサイズが変わったら

                // まっさきに、疑似の定数を更新するんだぜ☆（＾～＾）！
                PureMemory.gky_ky.OnBanResized1(PureSettei.banYokoHaba, PureSettei.banTateHaba);


                // 盤サイズ変更に伴い、作り直し
                PureMemory.gky_ky.shogiban.kikiBan.TukurinaosiBanOkisaHenko4();
            }

            //────────────────────────────────────────
            // ビットボードを作り直すぜ☆（＾～＾）
            //────────────────────────────────────────
            // 駒の動き方を作り直し
            // 飛び利きを作りたい駒もあるので、全居場所の空っぽビットボードは先に生成しておく。
            BitboardsOmatome.Tukurinaosi(
#if DEBUG
                    dbg_hint // どこで駒の動き方を初期化しようとしたか調べるもの
#endif
                    );
        }

        public static bool Try_DoMove_Input(Move ss
#if DEBUG
            , FenSyurui f
            , IDebugMojiretu dbg_reigai
#endif
            )
        {

            MoveType ssType = MoveType.N00_Karappo;
            if (DoMoveOpe.TryFailDoMoveAll(ss, ssType
#if DEBUG
                , f
                , dbg_reigai
                , false
                , "Try_DoMove_Input"
#endif
                ))
            {
                return false;
            }
            // 手番を進めるぜ☆（＾～＾）
            MoveGenAccessor.AddKifu(ss, ssType, PureMemory.dmv_ks_c);
//#if DEBUG
//            Util_Tansaku.Snapshot("Try_DoMove_Input", dbg_reigai);
//#endif
            return true;
        }



//        /// <summary>
//        /// コンピューターに思考させます。
//        /// ここが入り口です。
//        /// </summary>
//        /// <param name="gky"></param>
//        /// <param name="out_hyokatiUtiwake"></param>
//        /// <param name="dlgt_CreateJoho"></param>
//        /// <param name="hyoji"></param>
//        /// <returns></returns>
//        public static bool TryFail_Go(
//            out Move out_move,
//            Genkyoku gky,
//            out HyokatiAb out_hyokatiUtiwake,
//            StringBuilder hyoji
//            )
//        {
//            if (Util_Tansaku.TryFail_Go(out out_move, gky, out out_hyokatiUtiwake, hyoji))
//            {
//                return Pure.FailTrue("Util_Tansaku.Try_Go"
//#if DEBUG
//                                        , (IDebugMojiretu)hyoji
//#endif
//                    );
//            }

//            return Pure.SUCCESSFUL_FALSE;
//        }

        /// <summary>
        /// 2017-11-08 undo の後ろには 符号を付けないように変更☆（＾～＾）
        /// </summary>
        /// <param name="line"></param>
        /// <param name="hyoji"></param>
        /// <returns></returns>
        public static bool Try_Undo(string line, StringBuilder hyoji)
        {
            // うしろに続く文字は☆（＾▽＾）
            int caret = 0;
            if (Util_String.MatchAndNext("undo", line, ref caret))
            {
                if (UndoMoveOpe.TryFailUndoMove(
#if DEBUG
                    PureSettei.fenSyurui
                    , (IDebugMojiretu)hyoji
#endif
                ))
                {
                    return false;
                }
            }

            return true;
        }

    }
}
