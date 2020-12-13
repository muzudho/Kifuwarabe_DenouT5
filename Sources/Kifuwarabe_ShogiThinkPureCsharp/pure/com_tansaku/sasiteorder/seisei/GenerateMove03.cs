#if DEBUG
using kifuwarabe_shogithink.pure.ikkyoku;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.ky.bb;
#else
using kifuwarabe_shogithink.pure.ikkyoku;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.ky.bb;

#endif

namespace kifuwarabe_shogithink.pure.com.MoveOrder.seisei
{
    public static class GenerateMove03
    {
        static GenerateMove03()
        {
            bbTmp_nifu = new Bitboard();
        }

        #region 二歩防止
        static Bitboard bbTmp_nifu;
        /// <summary>
        /// 二歩防止
        /// </summary>
        public static void SiborikomiByNifu()
        {
            bbTmp_nifu.Clear();
            Koma hiyoko = Med_Koma.KomasyuruiAndTaikyokusyaToKoma(Komasyurui.H, PureMemory.kifu_teban);
            for (int iSuji = 0; iSuji < PureSettei.banYokoHaba; iSuji++)
            {
                PureMemory.gky_ky.shogiban.yomiIbashoBan_yoko.ToSet_Koma(hiyoko, bbTmp_nifu);//自分の歩
                bbTmp_nifu.Siborikomi(BitboardsOmatome.bb_sujiArray[iSuji]);//調べる筋だけ残す
                if (!bbTmp_nifu.IsEmpty())
                {
                    PureMemory.ssss_bbVar_idosaki_narazu.Sitdown(BitboardsOmatome.bb_sujiArray[iSuji]);
                }
            }

        }
        #endregion

        /// <summary>
        /// 仲間を支えていた駒が離れたら真だぜ☆（＾▽＾）ｗｗｗ
        /// 
        /// 利きテーブルをいじっているが、更新したいわけではないので、使い終わったら元に戻すぜ☆（＾～＾）
        /// </summary>
        /// <param name="out_ret"></param>
        /// <param name="ms_t0">移動元</param>
        /// <param name="ms_t1">移動先</param>
        /// <returns></returns>
        public static bool IsMisuteruUgoki()
        {
            // 升にある駒種類
            Komasyurui ks_t0;
            if (PureMemory.gky_ky.shogiban.ibashoBan_yk00.yomiIbashoBan.ExistsKoma(PureMemory.kifu_teban, PureMemory.ssss_ugoki_ms_src, out ks_t0))
            {

            }

            Koma km_t0 = Med_Koma.KomasyuruiAndTaikyokusyaToKoma(ks_t0, PureMemory.kifu_teban);
            Koma km_t1 = km_t0;//FIXME:成りを考慮してないぜ☆（＞＿＜）

            int sasaeBeforeMove = 0; // 移動前に、動こうとしている駒を支えている味方の数
            int sasaeAfterMove = 0; // 移動後の、動いた駒を支えている味方の数

            // 移動前の駒の動き（２個作っておく）
            Bitboard bbConst_kiki0 = BitboardsOmatome.KomanoUgokikataYk00.Clone_Merge(km_t0, PureMemory.ssss_ugoki_ms_src);
            Bitboard bbVar_kiki = bbConst_kiki0.Clone();
            if (!bbConst_kiki0.IsEmpty())
            {
                // 移動する前の、利き先の　利きの数を数えておこうぜ☆（＾▽＾）
                Masu ms_kiki;
                while (bbVar_kiki.Ref_PopNTZ(out ms_kiki))
                {
                    // 0以上なら駒の取り合いで勝つぜ☆（＾▽＾）
                    // 利き重ね　＝　自分の利き数　－　相手の利き数
                    int kasaneGake = PureMemory.gky_ky.shogiban.kikiBan.yomiKikiBan.CountKikisuZenbu(PureMemory.kifu_teban, ms_kiki) - PureMemory.gky_ky.shogiban.kikiBan.yomiKikiBan.CountKikisuZenbu(PureMemory.kifu_aiteban, ms_kiki);
                    if (0 < kasaneGake)
                    {
                        sasaeBeforeMove++;
                    }
                }

                //────────────────────────────────────────
                // 駒は動かさず、移動前の利きを消す（減らす）ぜ☆（＾▽＾）
                //────────────────────────────────────────
                bbVar_kiki.Set(bbConst_kiki0);
                //ビットボード使い回し
                PureMemory.gky_ky.shogiban.kikiBan.TorinozokuKiki(km_t0, bbVar_kiki);
            }


            // 移動先での駒の動き
            Bitboard bbConst_kiki1 = BitboardsOmatome.KomanoUgokikataYk00.Clone_Merge(km_t1, PureMemory.ssss_ugoki_ms_dst);
            if (!bbConst_kiki1.IsEmpty())
            {
                // 駒を移動させた想定で、利きの数を増やすぜ☆（＾▽＾）
                bbVar_kiki.Set(bbConst_kiki1);
                //ビットボード使い回し
                PureMemory.gky_ky.shogiban.kikiBan.OkuKiki(km_t1, bbVar_kiki);

                // 移動した後の、利き先の　利きの数を数えておこうぜ☆（＾▽＾）
                bbVar_kiki.Set(bbConst_kiki1);
                Masu ms_kiki;
                while (bbVar_kiki.Ref_PopNTZ(out ms_kiki))
                {
                    // 0以上なら駒の取り合いで勝つぜ☆（＾▽＾）
                    // 利き重ね　＝　自分の利き数　－　相手の利き数
                    int kasaneGake = PureMemory.gky_ky.shogiban.kikiBan.yomiKikiBan.CountKikisuZenbu(PureMemory.kifu_teban, ms_kiki) - PureMemory.gky_ky.shogiban.kikiBan.yomiKikiBan.CountKikisuZenbu(PureMemory.kifu_aiteban, ms_kiki);
                    if (0 < kasaneGake)
                    {
                        sasaeAfterMove++;
                    }
                }
            }

            //────────────────────────────────────────
            // 利き数表をいじったんで、元に戻しておこうぜ☆（＾▽＾）
            //────────────────────────────────────────
            {
                bbVar_kiki.Set(bbConst_kiki1);
                if (!bbVar_kiki.IsEmpty())
                {
                    // 増やした分の重ね利きを減らして、元に戻すぜ☆（＾▽＾）
                    //ビットボード使い回し
                    PureMemory.gky_ky.shogiban.kikiBan.TorinozokuKiki(km_t1, bbVar_kiki);
                }
            }

            {
                bbVar_kiki.Set(bbConst_kiki0);
                // 減らした分の重ね利きを増やして、元に戻すぜ☆（＾▽＾）
                //ビットボード使い回し
                PureMemory.gky_ky.shogiban.kikiBan.OkuKiki(km_t0, bbVar_kiki);
            }

            return sasaeBeforeMove - sasaeAfterMove < 0;
        }

        /// <summary>
        /// タダ捨ての動き
        /// </summary>
        /// <param name="ky2"></param>
        /// <param name="tai1"></param>
        /// <param name="ms_src"></param>
        /// <param name="ms_dst"></param>
        /// <param name="da">打の場合</param>
        /// <returns></returns>
        public static bool TadasuteNoUgoki()
        {
            // 主なケース
            // ・移動先の升には、味方の利き（動かす駒の利き除く）がない。
            // ・敵の利きに指す、打ち込む

            // タダ捨ての特殊なケース
            //
            // ・移動先の升は　空白☆
            // ・移動先の升には、手番らいおん　の利きがある☆（味方の利きはあるが、それが　らいおん　だった場合☆）
            // ・移動先の升には、相手番の駒の　重ね利きが　２つ以上ある☆
            // 
            // これはタダ捨てになる☆　らいおんでは取り返せないので☆
            // 「らいおんの利きを除いた利きビットボード」とかあれば便利だろうか☆（＞＿＜）？
            // 

            if (PureMemory.gky_ky.shogiban.kikiBan.yomiKikiBan.ExistsKikiZenbu(PureMemory.kifu_aiteban, PureMemory.ssss_ugoki_ms_dst)) // 相手の利きがあるところに放り込む
            {
                // 移動先の升の、味方の重ね利き　の数（これから動かす駒を除く）
                int kiki_ts1 = PureMemory.gky_ky.shogiban.kikiBan.yomiKikiBan.CountKikisuZenbu(PureMemory.kifu_teban, PureMemory.ssss_ugoki_ms_dst);
                if (!PureMemory.ssss_ugoki_kakuteiDa)
                {
                    // 「指」だと、自分の利きの数はカウントしないぜ☆（＾▽＾）ｗｗｗこれから動くからな☆（＾▽＾）ｗｗｗｗ
                    // 「打」だと、数字を－１してはいけないぜ☆
                    kiki_ts1--;
                }
                int kiki_ts2 = PureMemory.gky_ky.shogiban.kikiBan.yomiKikiBan.CountKikisuZenbu(PureMemory.kifu_aiteban, PureMemory.ssss_ugoki_ms_dst);

                if (0 == kiki_ts1 && 0 < kiki_ts2)//味方の利きがなくて、敵の利きがあれば、タダ捨てだぜ☆（＾▽＾）ｗｗｗ
                {
                    return true;
                }

                if (1 == kiki_ts1 && 1 < kiki_ts2//味方の利きがあり、敵の利きが２以上あり、
                    &&
                    //その味方はらいおんだった場合、タダ捨てだぜ☆（＾▽＾）ｗｗｗ
                    PureMemory.gky_ky.shogiban.kikiBan.yomiKikiBan.ExistsBBKiki(Med_Koma.KomasyuruiAndTaikyokusyaToKoma(Komasyurui.R, PureMemory.kifu_teban), PureMemory.ssss_ugoki_ms_dst)
                    )
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 成れる場所だけに絞り込むぜ☆（＾～＾）
        /// </summary>
        /// <param name="dstMs"></param>
        /// <param name="tai"></param>
        /// <param name="yomiKy"></param>
        /// <returns></returns>
        public static void SiborikomiNareruZone()
        {
            // 成り駒なら
            if (Conv_Koma.nari[(int)PureMemory.ssss_ugoki_km])
            {
                // 成れないぜ☆（＾～＾）ｗｗ
                PureMemory.ssss_bbVar_idosaki_nari.Clear();
            }
            else
            {
                // 移動元が「成る権利がもらえるゾーン」
                if (BitboardsOmatome.bb_nareruZone[PureMemory.kifu_nTeban].IsIntersect(PureMemory.ssss_ugoki_ms_src))
                {
                    // どこでも、成れるぜ☆（＾～＾） 絞り込みなし☆（＾～＾）
                }
                else
                {
                    // 「成る権利がもらえるゾーン」の中だけ成れるぜ☆（＾～＾）
                    PureMemory.ssss_bbVar_idosaki_nari.Siborikomi(BitboardsOmatome.bb_nareruZone[PureMemory.kifu_nTeban]);
                }
            }
        }

        /// <summary>
        /// 王手の手を消すぜ☆（＾～＾）
        /// </summary>
        public static void KesuOte()
        {
            BitboardsOmatome.KomanoUgokikataYk00.ToSitdown_Merge(
                PureMemory.ssss_ugoki_sakasaKm,
                PureMemory.hot_ms_raionAr[PureMemory.kifu_nAiteban],
                PureMemory.ssss_bbVar_idosaki_narazu// 移動先のうち、相手の　らいおん　が居るマスを除外するぜ☆（＾～＾）
            );
        }

        /// <summary>
        /// トライを消すぜ☆（＾～＾）
        /// </summary>
        public static void KesuTry()
        {
            Pure.Sc.Push("トライ除去", PureMemory.tnsk_hyoji);
            PureMemory.ssss_bbVar_idosaki_narazu.Sitdown(Util_TryRule.GetTrySaki(PureMemory.ssss_bbVar_idosaki_narazu, PureMemory.ssss_ugoki_ms_src));
            Pure.Sc.Pop();
        }

        /// <summary>
        /// らいおんの自殺手を消すぜ☆（＾～＾）
        /// </summary>
        public static void KesuRaionJisatusyu()
        {
            // らいおん　が自分から　相手の利きに飛び込むのを防ぐぜ☆（＾▽＾）ｗｗｗ
            PureMemory.gky_ky.yomiKy.yomiShogiban.yomiKikiBan.ToSitdown_BBKikiZenbu(PureMemory.kifu_aiteban, PureMemory.ssss_bbVar_idosaki_narazu);
        }

        /// <summary>
        /// 王手に絞り込むぜ☆（＾～＾）
        /// </summary>
        public static void SiborikomiOte()
        {
            // らいおんのいる升に、先後逆の自分の駒があると考えれば、その利きの場所と、今いる場所からの利きが重なれば、王手だぜ☆（＾▽＾）
            BitboardsOmatome.KomanoUgokikataYk00.ToSelect_MergeShogiban(PureMemory.ssss_ugoki_sakasaKm, PureMemory.hot_ms_raionAr[PureMemory.kifu_nAiteban], PureMemory.ssss_bbVar_idosaki_narazu);
        }
        /// <summary>
        /// 紐を付ける手に絞り込むぜ☆（＾～＾）
        /// </summary>
        public static void SiborikomiHimoduke()
        {
            // 紐を付ける☆
            PureMemory.ssss_bbVar_idosaki_narazu.Siborikomi(Util_Bitboard.CreateBBTebanKikiZenbu_1KomaNozoku(PureMemory.ssss_ugoki_ms_src));
        }
        /// <summary>
        /// 紐を付ける手は削るぜ☆（＾～＾）
        /// </summary>
        public static void KesuHimoduke()
        {
            // 紐を付けない☆
            PureMemory.ssss_bbVar_idosaki_narazu.Sitdown(Util_Bitboard.CreateBBTebanKikiZenbu_1KomaNozoku(PureMemory.ssss_ugoki_ms_src));
        }
    }
}
