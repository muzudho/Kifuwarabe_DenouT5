#if DEBUG
using kifuwarabe_shogithink.pure.control;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.ky.bb;
using System.Diagnostics;
#else
using kifuwarabe_shogithink.pure.accessor;
using kifuwarabe_shogithink.pure.control;
using kifuwarabe_shogithink.pure.genkyoku;
using kifuwarabe_shogithink.pure.ikkyoku;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.ky.bb;
using System.Diagnostics;
#endif


namespace kifuwarabe_shogithink.pure.com.sasiteorder.hioute
{
    /// <summary>
    /// 王手されるのはいやだな☆（＾▽＾）ｗｗｗ
    /// 被王手情報☆
    /// 
    /// （１）２つ以上の駒から王手されている場合　→　らいおん　が逃げるしかないぜ☆
    /// （２）１つの駒から王手されている場合　→　らいおん　が逃げるか、攻撃駒を取る方法があるぜ☆
    /// 
    /// 玉の位置も覚えておこうぜ……動くか☆（＾～＾）常に　相手のらいおん　の位置だけ覚えておけばいいか☆
    /// </summary>
    public static class Util_Hioute
    {
        static Util_Hioute()
        {
            bbVar_checker = new Bitboard();
            bbVar_nigemiti = new Bitboard();
            bbVar_kikasiteiruKoma = new Bitboard();
        }
        /// <summary>
        /// 相手番の駒がいる升
        /// </summary>
        static Bitboard bbVar_checker;
        static Bitboard bbVar_nigemiti;
        static Bitboard bbVar_kikasiteiruKoma;


        /// <summary>
        /// 指し手生成の開始時に呼び出されるぜ☆（＾～＾）
        /// </summary>
        /// <param name="gky"></param>
        public static void Tukurinaosi()
        {
            // らいおんの場所（らいおんは０～１匹という前提）
            foreach (Taikyokusya tai in Conv_Taikyokusya.itiran)
            {
                Masu ms_tmp;
                if (PureMemory.gky_ky.yomiKy.yomiShogiban.yomiIbashoBan.GetNTZ(Med_Koma.ToRaion(tai), out ms_tmp))
                {
                    PureMemory.hot_ms_raionAr[(int)tai] = ms_tmp;// ここに、らいおんのマスを覚えておくぜ☆（＾～＾）

                    // <編集>
                    // ライオンの利き＝８近傍
                    BitboardsOmatome.KomanoUgokikataYk00.ToSet_Merge(
                        Med_Koma.ToRaion(tai),
                        PureMemory.hot_ms_raionAr[(int)tai],
                        PureMemory.hot_bb_raion8KinboAr[(int)tai]// ここに、らいおんの８近傍を覚えておくぜ☆（＾～＾）
                        );
                }
                else
                {
                    // らいおんがいない場合
                    PureMemory.hot_ms_raionAr[(int)tai] = Conv_Masu.masu_error;
                    PureMemory.hot_bb_raion8KinboAr[(int)tai].Clear();
                }
            }

            // 手番側が、王手回避が必要かどうか調べたいぜ☆（＾～＾）
            Tukurinaosi_1(PureMemory.kifu_teban, PureMemory.kifu_aiteban, false);
            // 相手番側が、王手回避が必要かどうか調べたいぜ☆（＾～＾）
            Tukurinaosi_1(PureMemory.kifu_aiteban, PureMemory.kifu_teban, true);//手番を　ひっくり返す
        }


        /// <summary>
        /// 被王手されている情報を調べるぜ☆（＾～＾）
        /// 手番側を設定するんだぜ☆（＾～＾）
        /// </summary>
        /// <param name="gky"></param>
        /// <param name="isHouimouCheck">包囲網チェック</param>
        /// <returns></returns>
        public static void Tukurinaosi_1(
            Taikyokusya irekaeTeban,// 入れ替える
            Taikyokusya irekaeAiteban,// 入れ替える
            bool isHouimouCheck
            )
        {
            bbVar_nigemiti.Clear();
            PureMemory.hot_bb_checkerAr[(int)irekaeTeban].Clear();
            PureMemory.hot_bb_nigereruAr[(int)irekaeTeban].Clear();
            PureMemory.hot_outeKomasCountAr[(int)irekaeTeban] = 0;
            PureMemory.hot_isNigerarenaiCheckerAr[(int)irekaeTeban] = false;
            PureMemory.hot_bb_nigemitiWoFusaideiruAiteNoKomaAr[(int)irekaeTeban].Clear();

            //らいおんが盤上にいないこともあるぜ☆（＾▽＾）
            if (Conv_Masu.masu_error != PureMemory.hot_ms_raionAr[(int)irekaeTeban])
            {
                // 手番らいおんの８近傍の升☆（＾▽＾）
                Debug.Assert((int)Komasyurui.R < Conv_Komasyurui.itiran.Length, "");
                Debug.Assert((int)irekaeTeban < Conv_Taikyokusya.itiran.Length, "");
                Debug.Assert((int)irekaeAiteban < Conv_Taikyokusya.itiran.Length, "");
                Debug.Assert((int)PureMemory.hot_ms_raionAr[(int)irekaeTeban] < PureSettei.banHeimen, "");

                // らいおんが逃げれる８近傍の升☆（＾▽＾）
                // <編集>
                PureMemory.hot_bb_nigereruAr[(int)irekaeTeban].Set(PureMemory.hot_bb_raion8KinboAr[(int)irekaeTeban]);
                // <影響>
                PureMemory.gky_ky.yomiKy.yomiShogiban.yomiIbashoBan.ToSitdown_KomaZenbu(irekaeTeban, PureMemory.hot_bb_nigereruAr[(int)irekaeTeban]);
                PureMemory.gky_ky.yomiKy.yomiShogiban.yomiKikiBan.ToSitdown_BBKikiZenbu(irekaeAiteban, PureMemory.hot_bb_nigereruAr[(int)irekaeTeban]);

                // 手番の　らいおん　の
                // - ８近傍
                // - タテ筋
                // - ヨコ筋
                // - 左上がり筋
                // - 左下がり筋
                // を調べて、王手をかけている駒を一覧するぜ☆（＾▽＾）
                Masu ms_checker;
                bbVar_checker.Set(PureMemory.hot_bb_raion8KinboAr[(int)irekaeTeban]);
                // きりんとぞうは、先後同形☆（＾～＾）
                BitboardsOmatome.KomanoUgokikataYk00.ToStandup_Merge(Koma.K, PureMemory.hot_ms_raionAr[(int)irekaeTeban], bbVar_checker);
                BitboardsOmatome.KomanoUgokikataYk00.ToStandup_Merge(Koma.Z, PureMemory.hot_ms_raionAr[(int)irekaeTeban], bbVar_checker);
                // いのししは先後別☆（＾～＾）
                switch (irekaeAiteban)
                {
                    case Taikyokusya.T1:
                        BitboardsOmatome.KomanoUgokikataYk00.ToStandup_Merge(Koma.S, PureMemory.hot_ms_raionAr[(int)irekaeTeban], bbVar_checker);
                        break;
                    case Taikyokusya.T2:
                        BitboardsOmatome.KomanoUgokikataYk00.ToStandup_Merge(Koma.s, PureMemory.hot_ms_raionAr[(int)irekaeTeban], bbVar_checker);
                        break;
                }
                while (bbVar_checker.Ref_PopNTZ(out ms_checker))// 立っているビットを降ろすぜ☆
                {
                    if (IsTobikondaKiki(irekaeAiteban, ms_checker, PureMemory.hot_ms_raionAr[(int)irekaeTeban]))
                    {
                        // <編集>
                        PureMemory.hot_bb_checkerAr[(int)irekaeTeban].Standup(ms_checker);
                        PureMemory.hot_outeKomasCountAr[(int)irekaeTeban]++;
                    }
                }

                // ８方向に逃げ場がない場合は、王手を掛けてきている駒（チェッカー）を、
                // ~~~~~~~~~~~~~~~~~~~~~~~~
                // 必ず取り返して、その場にいようぜ☆
                PureMemory.hot_isNigerarenaiCheckerAr[(int)irekaeTeban] = (0 < PureMemory.hot_outeKomasCountAr[(int)irekaeTeban] && PureMemory.hot_bb_nigereruAr[(int)irekaeTeban].IsEmpty());

                if (isHouimouCheck)
                {
                    // 編集
                    // らいおんが逃げようとする８マス☆（＾～＾）
                    bbVar_nigemiti.Set(PureMemory.hot_bb_raion8KinboAr[(int)irekaeTeban]);
                    // 影響
                    // 相手の利きで塞がれているところを消す☆（＾～＾）
                    PureMemory.gky_ky.yomiKy.yomiShogiban.yomiKikiBan.ToSelect_BBKikiZenbu(irekaeAiteban, bbVar_nigemiti);
                    // 自分の駒があるところを消す☆（＾～＾）
                    PureMemory.gky_ky.shogiban.ibashoBan_yk00.yomiIbashoBan.ToSitdown_KomaZenbu(irekaeTeban, bbVar_nigemiti);

                    Masu ms_fusagiMiti;
                    while (bbVar_nigemiti.Ref_PopNTZ(out ms_fusagiMiti))
                    {
                        // 塞がれている升の８近傍に、塞いでいる駒がいるだろう☆
                        foreach (Komasyurui ks_fusagi8Kinbo in Conv_Komasyurui.itiran)
                        {
                            // <編集>
                            // ８近傍のそれぞれのマスについて、
                            // 相手の駒がそこに利きを利かしているかどうかを調べるには、
                            // ８近傍に自分の駒を置いて利きを調べれば、
                            // そこに利かしている相手の駒の場所が分かるぜ☆（＾▽＾）
                            BitboardsOmatome.KomanoUgokikataYk00.ToSet_Merge(
                                Med_Koma.KomasyuruiAndTaikyokusyaToKoma(ks_fusagi8Kinbo, irekaeTeban),
                                ms_fusagiMiti,
                                bbVar_kikasiteiruKoma
                                );
                            // <影響>
                            PureMemory.gky_ky.shogiban.ibashoBan_yk00.yomiIbashoBan.ToSelect_Koma(Med_Koma.KomasyuruiAndTaikyokusyaToKoma(ks_fusagi8Kinbo,irekaeAiteban), bbVar_kikasiteiruKoma);

                            Masu ms_fusagi8KinboKoma;
                            while (bbVar_kikasiteiruKoma.Ref_PopNTZ(out ms_fusagi8KinboKoma))
                            {
                                PureMemory.hot_bb_nigemitiWoFusaideiruAiteNoKomaAr[(int)irekaeTeban].Standup(ms_fusagi8KinboKoma);
                            }
                        }
                    }
                }
            }

            // 王手を掛けている駒を数えるぜ☆（＾～＾）
            if (PureMemory.hot_outeKomasCountAr[(int)irekaeTeban] < 1)
            {
                // 王手を掛けられていないか、
                // 王手は掛けられているが逃げれる場合は、
                // クリアー
                PureMemory.hot_bb_nigeroAr[(int)irekaeTeban].Clear();
            }
        }

        /// <summary>
        /// 王手回避が必要なら真。
        /// </summary>
        /// <returns></returns>
        public static bool IsHituyoOteKaihi(Taikyokusya tai)
        {
            return !PureMemory.hot_bb_checkerAr[(int)tai].IsEmpty();
        }

        /// <summary>
        /// 詰んでるか☆（＾▽＾）
        /// </summary>
        /// <returns></returns>
        public static bool IsTunderu(Taikyokusya tai)
        {
            // どうぶつしょうぎ用の詰み判定
            switch (PureSettei.gameRule)
            {
                case GameRule.DobutuShogi:
                    {
                        return 1 < PureMemory.hot_outeKomasCountAr[(int)tai] // 両王手で、
                            && PureMemory.hot_bb_nigeroAr[(int)tai].IsEmpty(); // 逃げ道がない場合は、回避不能だぜ☆（＾▽＾）
                    }
                case GameRule.HonShogi:
                    {
                        return 1 < PureMemory.hot_outeKomasCountAr[(int)tai] // 両王手で、
                            && PureMemory.hot_bb_nigeroAr[(int)tai].IsEmpty(); // 逃げ道がない場合は、回避不能だぜ☆（＾▽＾）
                    }
                default:
                    {
                        // ルールが分からないので、とりあえず詰んではいないことにするぜ☆（＾～＾）
                        return false;
                    }
            }
        }

        /// <summary>
        /// 手番の駒　の８近傍を調べて、利きに飛び込んでいたら真顔で真だぜ☆（＾▽＾）
        /// </summary>
        /// <param name="gky"></param>
        /// <param name="ms_attacker">相手の攻撃駒の居場所</param>
        /// <param name="ms_target">狙っている升</param>
        /// <returns></returns>
        public static bool IsTobikondaKiki(Taikyokusya irekaeAiteban, Masu ms_attacker, Masu ms_target)
        {
            if (PureMemory.gky_ky.yomiKy.yomiShogiban.yomiIbashoBan.ExistsKomaZenbu(irekaeAiteban, ms_attacker)) // 攻撃を仕掛けてくるだろう場所(attackerMs)に相手の駒があることを確認
            {
                Komasyurui ks;
                if (PureMemory.gky_ky.yomiKy.yomiShogiban.yomiIbashoBan.ExistsKoma(irekaeAiteban, ms_attacker, out ks))// 駒があれば、その駒の種類を確認
                {
                    Koma km_attacker = Med_Koma.KomasyuruiAndTaikyokusyaToKoma(ks, irekaeAiteban);
                    // 相手の攻撃駒の利き
                    // ここで飛び利きを判定できるか？
                    return BitboardsOmatome.KomanoUgokikataYk00.IsIntersect(
                        km_attacker,// 王手してくる駒
                        ms_attacker,// その駒がいる升
                        ms_target// 調べている升（王手されている升）
                        );
                }
            }
            return false;
        }

        /// <summary>
        /// 自殺手チェック☆
        /// 相手番の利きに入っていたら真顔で真だぜ☆（＾▽＾）
        /// </summary>
        /// <param name="gky"></param>
        /// <param name="targetMs"></param>
        /// <returns></returns>
        public static bool IsJisatusyu(Masu targetMs)
        {
            return PureMemory.gky_ky.yomiKy.yomiShogiban.yomiKikiBan.IsIntersect_KikiZenbu(
                PureMemory.kifu_aiteban,// 相手の駒の利き☆
                targetMs//調べる升
                );
        }

    }
}
