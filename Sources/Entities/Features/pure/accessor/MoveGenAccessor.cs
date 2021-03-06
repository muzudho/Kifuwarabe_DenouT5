﻿#if DEBUG
using kifuwarabe_shogithink.pure.com;
using kifuwarabe_shogithink.pure.com.moveorder.hioute;
using kifuwarabe_shogithink.pure.control;
using kifuwarabe_shogithink.pure.conv.genkyoku.play;
using kifuwarabe_shogithink.pure.genkyoku;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.ky.bb;
using kifuwarabe_shogithink.pure.listen.genkyoku;
using kifuwarabe_shogithink.pure.listen.play;
using kifuwarabe_shogithink.pure.move;
using kifuwarabe_shogithink.pure.speak.ky.bb;
using kifuwarabe_shogithink.pure.speak.play;
using System;
using System.Diagnostics;
using Grayscale.Kifuwarabi.Entities.Take1Base;
#else
using kifuwarabe_shogithink.pure.com;
using kifuwarabe_shogithink.pure.com.MoveOrder.hioute;
using kifuwarabe_shogithink.pure.control;
using kifuwarabe_shogithink.pure.conv.genkyoku.play;
using kifuwarabe_shogithink.pure.genkyoku;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.ky.bb;
using kifuwarabe_shogithink.pure.listen.genkyoku;
using kifuwarabe_shogithink.pure.listen.play;
using kifuwarabe_shogithink.pure.move;
using kifuwarabe_shogithink.pure.speak.play;
using System;
using System.Diagnostics;
using System.Text;
using Grayscale.Kifuwarabi.Entities.Take1Base;
#endif

namespace kifuwarabe_shogithink.pure.accessor
{
    /// <summary>
    /// 指し手生成メモリー
    /// 
    /// とりあえずシングル・スレッドで使うことを想定した静的変数。
    /// 
    /// 
    /// 対局終了時の記録用だぜ☆
    /// (2017-04-27 08:37) USI用にも拡張するぜ☆（*＾～＾*）
    /// </summary>
    public static class MoveGenAccessor
    {
#if DEBUG
        public static void DumpMoveGen(StringBuilder hyoji)
        {
            
            SpkBan_Hisigata.Setumei_yk00("チェッカー(手番へ)", PureMemory.hot_bb_checkerAr[PureMemory.kifu_nTeban], hyoji);
            SpkBan_Hisigata.Setumei_yk00("チェッカー(相手番へ)", PureMemory.hot_bb_checkerAr[PureMemory.kifu_nAiteban], hyoji);

            SpkBan_Hisigata.Setumei_yk00("王手を掛けてきている駒", PureMemory.ssss_bbBase_idosaki01_checker, hyoji);
            SpkBan_Hisigata.Setumei_yk00("らいおんキャッチ", PureMemory.ssss_bbBase_idosaki02_raionCatch, hyoji);

            SpkBan_Hisigata.Setumei_yk00("逃げろ手", PureMemory.ssss_bbBase_idosaki03_nigeroTe, hyoji);
            SpkBan_Hisigata.Setumei_yk00("トライ", PureMemory.ssss_bbBase_idosaki04_try, hyoji);

            SpkBan_Hisigata.Setumei_yk00("駒を取る手", PureMemory.ssss_bbBase_idosaki05_komaWoToruTe, hyoji);
            SpkBan_Hisigata.Setumei_yk00("紐付き王手指し", PureMemory.ssss_bbBase_idosaki06_himodukiOteZasi, hyoji);
            SpkBan_Hisigata.Setumei_yk00("捨て王手指し", PureMemory.ssss_bbBase_idosaki07_suteOteZasi, hyoji);
            SpkBan_Hisigata.Setumei_yk00("捨て王手打", PureMemory.ssss_bbBase_idosaki08_suteOteDa, hyoji);
            SpkBan_Hisigata.Setumei_yk00("紐付き王手打", PureMemory.ssss_bbBase_idosaki09_himodukiOteDa, hyoji);
            SpkBan_Hisigata.Setumei_yk00("紐付き緩慢打", PureMemory.ssss_bbBase_idosaki10_himodukiKanmanDa, hyoji);
            SpkBan_Hisigata.Setumei_yk00("紐付き緩慢指し", PureMemory.ssss_bbBase_idosaki11_himodukiKanmanZasi, hyoji);
            SpkBan_Hisigata.Setumei_yk00("ぼっち緩慢指し", PureMemory.ssss_bbBase_idosaki12_bottiKanmanZasi, hyoji);
            SpkBan_Hisigata.Setumei_yk00("ぼっち緩慢打", PureMemory.ssss_bbBase_idosaki13_bottiKanmanDa, hyoji);
            SpkBan_Hisigata.Setumei_yk00("捨て緩慢指し", PureMemory.ssss_bbBase_idosaki14_suteKanmanZasi, hyoji);
            SpkBan_Hisigata.Setumei_yk00("捨て緩慢打", PureMemory.ssss_bbBase_idosaki15_suteKanmanDa, hyoji);
        }
#endif

        public static void AppendMovesTo(FenSyurui f, StringBuilder syuturyoku)
        {
            ScanKifu_0ToPreTeme((int iKifu, ref bool toBreak) =>
            {
                Move ss = PureMemory.kifu_moveArray[iKifu];
                SpkMove.AppendFenTo(f, ss, syuturyoku);
                syuturyoku.Append(" ");
            });
        }

        public static void ScanBestYomisuji(DLGT_scanKifu dlgt_scanKifu)
        {
            bool toBreak = false;
            for (int iKifu = PureMemory.tnsk_kaisiTeme; iKifu < PureMemory.tnsk_happaTeme; iKifu++)
            {
                dlgt_scanKifu(iKifu, ref toBreak);
                if (toBreak)
                {
                    break;
                }
            }
        }

        public delegate void DLGT_scanKifu(int iTeme, ref bool toBreak);
        public static void ScanMoves_0ToPreTeme(DLGT_scanKifu dlgt_scanKifu)
        {
            bool toBreak = false;
            for (int iTeme = 0; iTeme < PureMemory.mvs_endTeme; iTeme++)
            {
                dlgt_scanKifu(iTeme, ref toBreak);
                if (toBreak)
                {
                    break;
                }
            }
        }
        public static void ScanKifu_0ToPreTeme(DLGT_scanKifu dlgt_scanKifu)
        {
            bool toBreak = false;
            for (int iTeme = 0; iTeme < PureMemory.kifu_endTeme; iTeme++)
            {
                dlgt_scanKifu(iTeme, ref toBreak);
                if (toBreak)
                {
                    break;
                }
            }
        }
        public static void ScanKifu_PreTemeTo0(DLGT_scanKifu dlgt_scanKifu)
        {
            bool toBreak = false;
            for (int iTeme = PureMemory.kifu_endTeme-1; -1<iTeme; iTeme--)
            {
                dlgt_scanKifu(iTeme, ref toBreak);
                if (toBreak)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// カーソル位置を最初に戻すだけだぜ☆（＾～＾）
        /// </summary>
        public static void BackTemeToFirst_AndClearTeme()
        {
            PureMemory.ClearTeme();
            // 最初の要素をリセットするぜ☆（＾～＾）
            PureMemory.kifu_moveArray[PureMemory.kifu_endTeme] = Move.Toryo;
            PureMemory.kifu_moveTypeArray[PureMemory.kifu_endTeme] = MoveType.N00_Karappo;
            PureMemory.kifu_toraretaKsAr[PureMemory.kifu_endTeme] = Komasyurui.Yososu;
        }

        /// <summary>
        /// カーソル位置に挿入。
        /// カーソルは１進み、エンドは進んだカーソル位置に合わせるぜ☆（＾～＾）
        /// </summary>
        /// <param name="ss"></param>
        /// <param name="ssType"></param>
        public static void AddKifu(Move ss, MoveType ssType, Komasyurui toraretaKs)
        {
            PureMemory.kifu_moveArray[PureMemory.kifu_endTeme] = ss;
            PureMemory.kifu_moveTypeArray[PureMemory.kifu_endTeme] = ssType;

            // 取った駒の種類を覚えておくぜ☆（＾▽＾）
            PureMemory.kifu_toraretaKsAr[PureMemory.kifu_endTeme] = toraretaKs;

            // 棋譜を進めるぜ☆（＾～＾）
            PureMemory.AddTeme();
        }
        #region 作り直し
        /// <summary>
        /// moves 以降の符号を指定しろだぜ☆（＾～＾）
        /// 「手目」は最後まで進んでしまうぜ☆（＾～＾）
        /// </summary>
        /// <param name="moves"></param>
        public static void Tukurinaosi_RemakeKifuByMoves(string moves)
        {
            PureMemory.mvs_endTeme = 0;

            string[] fugoItiran = moves.Split(' ');
            foreach (string fugo in fugoItiran)
            {
                int caret = 0;
                Move move;
                if (!LisPlay.MatchFenMove(PureSettei.fenSyurui, fugo, ref caret, out move))
                {
                    throw new System.Exception($"指し手のパースエラー fugo=[{ fugo }]");
                }
                //MoveSeiseiAccessor.AddKifu(
                //    move,
                //    MoveType.N00_Karappo,
                //    Komasyurui.Yososu // FIXME: 取られた駒も調べたい
                //    );
                PureMemory.mvs_ssAr[PureMemory.mvs_endTeme] = move;
                PureMemory.mvs_endTeme++;
            }
        }
        #endregion

        /// <summary>
        /// 指定の手目まで進めるぜ☆（＾～＾）
        /// </summary>
        /// <param name="temeMade"></param>
        /// <param name="ky2"></param>
        /// <param name="hyoji"></param>
        public static bool Try_GoToTememade(FenSyurui f, int temeMade, StringBuilder hyoji)
        {
            // 棋譜を元に、局面データを再現するぜ☆

            int caret = 0;
            string moves_notUse;
            // 初期局面のセット☆（＾～＾）
            if (LisGenkyoku.TryFail_MatchPositionvalue(f,PureMemory.kifu_syokiKyokumenFen, ref caret, out moves_notUse
#if DEBUG
                , (IDebugMojiretu)hyoji
#endif
                ))
            {
                return false;
            }
            // 指定の手目まで進めるぜ☆（＾～＾）
            MoveGenAccessor.ScanKifu_0ToPreTeme((int iKifu, ref bool toBreak) =>
            {
                Move ss = PureMemory.kifu_moveArray[iKifu];
                if (temeMade < iKifu)
                {
                    toBreak = true;
                    return;
                }

                MoveType ssType = MoveType.N00_Karappo;
                if (DoMoveOpe.TryFailDoMoveAll(ss, ssType
#if DEBUG
                    , f
                    , (IDebugMojiretu)hyoji
                    , false
                    , "Try_GoToTememade"
#endif
                    ))
                {
                    toBreak = true;
                    return;
                }
                // 手番を進めるぜ☆（＾～＾）
                MoveGenAccessor.AddKifu(ss, ssType, PureMemory.dmv_ks_c);
//#if DEBUG
//                Util_Tansaku.Snapshot("GoToTememade", (IDebugMojiretu)hyoji);
//#endif

                temeMade--;
            });
            return true;
        }

        /// <summary>
        /// 棋譜に沿って、終局図まで進めるぜ☆（＾～＾）
        /// 局面は初期局面に設定し、「手目」は０に戻してあるものとするぜ☆（＾～＾）
        /// </summary>
        /// <param name="ky2"></param>
        /// <param name="hyoji"></param>
        public static bool Try_PlayMoves_0ToPreTeme(FenSyurui f, StringBuilder hyoji)
        {
            // 棋譜を元に、局面データを再現するぜ☆
            MoveGenAccessor.ScanMoves_0ToPreTeme((int iTeme, ref bool toBreak) => {
                Move ss = PureMemory.mvs_ssAr[iTeme];

#if DEBUG
                hyoji.AppendLine(string.Format("[{0}] 指すぜ☆（＾～＾） ss={1}",
                    iTeme,
                    SpkMove.ToString_Fen(PureSettei.fenSyurui, ss)
                    ));
                int a = 0;
#endif


                if (DoMoveOpe.TryFailDoMoveAll(
                    ss,
                    MoveType.N00_Karappo
#if DEBUG
                    , f
                    , (IDebugMojiretu)hyoji
                    , false
                    , "Try_GoToFinish"
#endif
                    ))
                {
                    toBreak = true;
                    return;
                }
                MoveGenAccessor.AddKifu(ss, MoveType.N00_Karappo, PureMemory.dmv_ks_c);
#if DEBUG
                // 局面表示☆
                Interproject.project.HyojiKyokumen(iTeme, hyoji);
                // 駒の居場所表示☆
                Interproject.project.HyojiIbasho("Try_PlayMoves_0ToPreTeme", hyoji);
#endif
            });

            return true;
        }

        /// <summary>
        /// 指し手情報を分解するぜ☆（＾～＾）
        /// 駒を動かす方を手番、相手を相手番と考えるぜ☆（＾～＾）
        /// </summary>
        public static void BunkaiMoveDmv(Move ss)
        {
            //
            // 動かす駒を t0 と呼ぶとする。
            //      移動元を t0、移動先を t1 と呼ぶとする。
            // 取られる駒を c と呼ぶとする。
            //      取られる駒の元位置は t1 、駒台は 3 と呼ぶとする。
            //

            // 変数をグローバルに一時退避
            // 移動先升
            PureMemory.dmv_ms_t1 = AbstractConvMove.GetDstMasu_WithoutErrorCheck((int)ss);
            // あれば、移動先の相手の駒（取られる駒; capture）
            PureMemory.dmv_km_c = PureMemory.gky_ky.yomiKy.yomiShogiban.yomiIbashoBan.GetBanjoKoma(PureMemory.kifu_aiteban, PureMemory.dmv_ms_t1);
            PureMemory.dmv_ks_c = Med_Koma.KomaToKomasyurui(PureMemory.dmv_km_c);
            PureMemory.dmv_mk_c = Med_Koma.BanjoKomaToMotiKoma(PureMemory.dmv_km_c);

            if (AbstractConvMove.IsUtta(ss))
            {
                // 打
                PureMemory.dmv_ms_t0 = Conv_Masu.masu_error;

                // 指し手から「持駒」を判別
                PureMemory.dmv_mks_t0 = AbstractConvMove.GetUttaKomasyurui(ss);
                PureMemory.dmv_mk_t0 = Med_Koma.MotiKomasyuruiAndTaikyokusyaToMotiKoma(PureMemory.dmv_mks_t0, PureMemory.kifu_teban);
                // 「持駒」から「駒」へ変換
                PureMemory.dmv_km_t0 = Med_Koma.MotiKomasyuruiAndTaikyokusyaToKoma(PureMemory.dmv_mks_t0, PureMemory.kifu_teban);

                // 持ち駒は t0 も t1 も同じ。
                PureMemory.dmv_km_t1 = PureMemory.dmv_km_t0;
                PureMemory.dmv_ks_t0 = Med_Koma.MotiKomasyuruiToKomasyrui(PureMemory.dmv_mks_t0);//おまとめ☆（＾～＾）
                PureMemory.dmv_ks_t1 = PureMemory.dmv_ks_t0;//追加
                //#if DEBUG
                //                if (!gky.ky.motiKomas.sindanMK.HasMotiKoma(mk_t0))
                //                {
                //                    CommandK.Ky(isSfen, "ky", gky, syuturyoku);
                //                    Util_Machine.Flush(syuturyoku);
                //                }
                //#endif
                Debug.Assert(PureMemory.gky_ky.motigomaItiran.yomiMotigomaItiran.HasMotigoma(PureMemory.dmv_mk_t0),
                    $"持っていない駒を打つのか☆（＾～＾）！？ mks_src=[{ PureMemory.dmv_mks_t0 }] mk_utu=[{ PureMemory.dmv_mk_t0 }]");
            }
            else
            {
                // 指し
                PureMemory.dmv_ms_t0 = AbstractConvMove.GetSrcMasu_WithoutErrorCheck((int)ss);
                PureMemory.dmv_km_t0 = PureMemory.gky_ky.yomiKy.yomiShogiban.yomiIbashoBan.GetBanjoKoma(PureMemory.dmv_ms_t0);
                PureMemory.dmv_ks_t0 = Med_Koma.KomaToKomasyurui(PureMemory.dmv_km_t0);//移動元の駒の種類
                PureMemory.dmv_mks_t0 = MotigomaSyurui.Yososu;
                PureMemory.dmv_mk_t0 = Motigoma.Yososu;
                if (AbstractConvMove.IsNatta(ss)) // 駒が成るケース
                {
                    PureMemory.dmv_ks_t1 = Conv_Komasyurui.ToNariCase(PureMemory.dmv_ks_t0);
                    PureMemory.dmv_km_t1 = Med_Koma.KomasyuruiAndTaikyokusyaToKoma(PureMemory.dmv_ks_t1, PureMemory.kifu_teban);
                }
                else // 駒が成らないケース
                {
                    PureMemory.dmv_km_t1 = PureMemory.gky_ky.yomiKy.yomiShogiban.yomiIbashoBan.GetBanjoKoma(PureMemory.dmv_ms_t0);
                    PureMemory.dmv_ks_t1 = PureMemory.dmv_ks_t0;
                }
            }
        }

        public static void BunkaiMoveUmv()
        {
            PureMemory.umv_ss = PureMemory.kifu_moveArray[PureMemory.kifu_endTeme];

            // 駒がないところを指していることがないか？
            PureMemory.umv_ms_t1 = AbstractConvMove.GetDstMasu_WithoutErrorCheck((int)PureMemory.umv_ss);
            PureMemory.umv_km_t1 = PureMemory.gky_ky.yomiKy.yomiShogiban.yomiIbashoBan.GetBanjoKoma(PureMemory.umv_ms_t1);
            PureMemory.umv_ks_t1 = Med_Koma.KomaToKomasyurui(PureMemory.umv_km_t1);// 成っているかもしれない☆
            Debug.Assert(Conv_Masu.IsBanjoOrError(PureMemory.umv_ms_t1), "error Undo-Begin-6");
            Debug.Assert(Conv_Koma.IsOk(PureMemory.umv_km_t1), "error Undo-Begin-7");


            if (!AbstractConvMove.IsUtta(PureMemory.umv_ss))// 指す
            {
                PureMemory.umv_ms_t0 = AbstractConvMove.GetSrcMasu_WithoutErrorCheck((int)PureMemory.umv_ss);// 戻し先。
                Debug.Assert(Conv_Masu.IsBanjo(PureMemory.umv_ms_t0), "error Undo-Begin-21 #金魚 戻し先が盤上でない？");

                PureMemory.umv_mk_t0 = Motigoma.Yososu;
                if (AbstractConvMove.IsNatta(PureMemory.umv_ss))// 成っていたとき
                {
                    PureMemory.umv_ks_t0 = Conv_Komasyurui.ToNarazuCase(PureMemory.umv_ks_t1);// 成る前
                }
                else
                {
                    PureMemory.umv_ks_t0 = PureMemory.umv_ks_t1;// 成る前、あるいは、成っていない、あるいは　もともと　にわとり☆
                }
                PureMemory.umv_km_t0 = Med_Koma.KomasyuruiAndTaikyokusyaToKoma(PureMemory.umv_ks_t0, PureMemory.kifu_teban);
                Debug.Assert(Conv_Koma.IsOk(PureMemory.umv_km_t0), "error Undo-Begin-9 #羊");
                Debug.Assert(Conv_Masu.IsBanjoOrError(PureMemory.umv_ms_t0), "error Undo-Begin-8 #颪");
            }
            else// 打つ
            {
                PureMemory.umv_ms_t0 = Conv_Masu.masu_error;
                PureMemory.umv_km_t0 = Piece.Yososu;
                PureMemory.umv_ks_t0 = Komasyurui.Yososu;
                PureMemory.umv_mk_t0 = Med_Koma.KomasyuruiAndTaikyokusyaToMotiKoma(PureMemory.umv_ks_t1, PureMemory.kifu_teban);
            }


            PureMemory.umv_ks_c = PureMemory.kifu_toraretaKsAr[PureMemory.kifu_endTeme];

            if (Komasyurui.Yososu != PureMemory.umv_ks_c)
            {
                PureMemory.umv_km_c = Med_Koma.KomasyuruiAndTaikyokusyaToKoma(PureMemory.umv_ks_c, PureMemory.kifu_aiteban);
                PureMemory.umv_mk_c = Med_Koma.BanjoKomaToMotiKoma(PureMemory.umv_km_c);
                Debug.Assert(Conv_Koma.IsOk(PureMemory.umv_km_c), "error Undo-Begin-10 #竜巻");
            }
            else
            {
                PureMemory.umv_km_c = Piece.Yososu;
                PureMemory.umv_mk_c = Motigoma.Yososu;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static void DoMovePickerBegin(MoveType flag)
        {
            MoveGenAccessor.Clear_SsssUtikiri();
            // 空っぽにしておくぜ☆　何か入れないと投了だぜ☆（＾▽＾）ｗｗｗ
            MoveGenAccessor.ClearMoveList();

            PureMemory.ssss_isSyobuNasi = GenkyokuOpe.IsSyobuNasi();

            PureMemory.ssss_bbBase_idosaki01_checker.Clear();
            PureMemory.ssss_bbBase_idosaki02_raionCatch.Clear();

            PureMemory.ssss_bbBase_idosaki03_nigeroTe.Clear();
            PureMemory.ssss_bbBase_idosaki04_try.Clear();

            PureMemory.ssss_bbBase_idosaki05_komaWoToruTe.Clear();
            PureMemory.ssss_bbBase_idosaki06_himodukiOteZasi.Clear();
            PureMemory.ssss_bbBase_idosaki07_suteOteZasi.Clear();
            PureMemory.ssss_bbBase_idosaki08_suteOteDa.Clear();
            PureMemory.ssss_bbBase_idosaki09_himodukiOteDa.Clear();
            PureMemory.ssss_bbBase_idosaki10_himodukiKanmanDa.Clear();
            PureMemory.ssss_bbBase_idosaki11_himodukiKanmanZasi.Clear();
            PureMemory.ssss_bbBase_idosaki12_bottiKanmanZasi.Clear();
            PureMemory.ssss_bbBase_idosaki13_bottiKanmanDa.Clear();
            PureMemory.ssss_bbBase_idosaki14_suteKanmanZasi.Clear();
            PureMemory.ssss_bbBase_idosaki15_suteKanmanDa.Clear();

            PureMemory.ssss_bbVar_idosaki_narazu.Clear();
            PureMemory.ssss_bbVar_idosaki_nari.Clear();
            PureMemory.ssssTmp_bbVar_ibasho.Clear();

            // 変数名短縮
            Kyokumen.YomiKy yomiKy = PureMemory.gky_ky.yomiKy;
            IbashoBan.YomiIbashoBan yomiIbashoBan = PureMemory.gky_ky.yomiKy.yomiShogiban.yomiIbashoBan;
            KikiBan.YomiKikiBan yomiKikiBan = PureMemory.gky_ky.yomiKy.yomiShogiban.yomiKikiBan;

            //────────────────────────────────────────
            // 被王手
            //────────────────────────────────────────

            Util_Hioute.Tukurinaosi();

            //────────────────────────────────────────
            // 移動先
            //────────────────────────────────────────

            if (
                flag.HasFlag(MoveType.N13_HippakuKaeriutiTe)
                ||
                flag.HasFlag(MoveType.N14_YoyuKaeriutiTe)
                )
            {
                // 移動先は、王手をかけてきている駒☆（＾～＾）
                PureMemory.ssss_bbBase_idosaki01_checker.Set(PureMemory.hot_bb_checkerAr[PureMemory.kifu_nTeban]);
            }

            if (flag.HasFlag(MoveType.N12_RaionCatch) || flag.HasFlag(MoveType.N17_RaionCatchChosa))
            {
                // 相手らいおん　を取る手のみ生成するぜ☆（＾▽＾）
                yomiIbashoBan.ToSet_Koma(Med_Koma.ToRaion(PureMemory.kifu_aiteban), PureMemory.ssss_bbBase_idosaki02_raionCatch);
            }

            if (flag.HasFlag(MoveType.N15_NigeroTe))
            {
                // 移動先
                PureMemory.ssss_bbBase_idosaki03_nigeroTe.Set(BitboardsOmatome.bb_boardArea);
                PureMemory.ssss_bbBase_idosaki03_nigeroTe.Siborikomi(PureMemory.hot_bb_nigeroAr[PureMemory.kifu_nTeban]);
                PureMemory.ssss_bbBase_idosaki03_nigeroTe.Sitdown(PureMemory.hot_bb_checkerAr[PureMemory.kifu_nTeban]);// （逼迫／余裕）返討手は除外するぜ☆（＾▽＾）
                yomiIbashoBan.ToSitdown_Koma(Med_Koma.ToRaion(PureMemory.kifu_aiteban), PureMemory.ssss_bbBase_idosaki03_nigeroTe);// 利きのうち、らいおんを取る手　は、除外するぜ☆（＾▽＾）
                PureMemory.ssss_bbBase_idosaki03_nigeroTe.Sitdown(PureMemory.hot_bb_checkerAr[PureMemory.kifu_nTeban]); // 返討手　は除外するぜ☆（＾▽＾）
            }

            if (flag.HasFlag(MoveType.N16_Try))
            {
                // トライは　どうぶつしょうぎ用　だぜ☆（＾～＾）
                if (PureSettei.gameRule == GameRule.DobutuShogi)
                {
                    PureMemory.ssss_bbBase_idosaki04_try.Set(BitboardsOmatome.bb_boardArea);
                    yomiIbashoBan.ToSitdown_KomaZenbu(PureMemory.kifu_teban, PureMemory.ssss_bbBase_idosaki04_try);// 味方の駒があるところには移動できないぜ☆（＾▽＾）
                    yomiIbashoBan.ToSitdown_Koma(Med_Koma.ToRaion(PureMemory.kifu_aiteban), PureMemory.ssss_bbBase_idosaki04_try);// 利きのうち、らいおん　を取る手は、除外するぜ☆（＾▽＾）
                    PureMemory.ssss_bbBase_idosaki04_try.Sitdown(PureMemory.hot_bb_checkerAr[PureMemory.kifu_nTeban]); // 返討手　は除外するぜ☆（＾▽＾）
                }
            }

            if (flag.HasFlag(MoveType.N01_KomaWoToruTe))
            {
                // 移動先
                yomiIbashoBan.ToSet_KomaZenbu(PureMemory.kifu_aiteban, PureMemory.ssss_bbBase_idosaki05_komaWoToruTe);// 相手の駒があるところだけ☆（＾▽＾）
                yomiIbashoBan.ToSitdown_Koma(Med_Koma.ToRaion(PureMemory.kifu_aiteban), PureMemory.ssss_bbBase_idosaki05_komaWoToruTe);// らいおんキャッチ　は除外するぜ☆（＾▽＾）
                PureMemory.ssss_bbBase_idosaki05_komaWoToruTe.Sitdown(PureMemory.hot_bb_checkerAr[PureMemory.kifu_nTeban]); // 返討手　は除外するぜ☆（＾▽＾）
            }

            if (flag.HasFlag(MoveType.N10_HimozukiOteZasi))
            {
                // 移動先
                PureMemory.ssss_bbBase_idosaki06_himodukiOteZasi.Set(BitboardsOmatome.bb_boardArea);
                yomiIbashoBan.ToSitdown_KomaZenbu(PureMemory.kifu_teban, PureMemory.ssss_bbBase_idosaki06_himodukiOteZasi);// 味方の駒があるところには移動できないぜ☆（＾▽＾）
                yomiIbashoBan.ToSitdown_KomaZenbu(PureMemory.kifu_aiteban, PureMemory.ssss_bbBase_idosaki06_himodukiOteZasi);// 相手の駒がある升　は除外するぜ☆（＾▽＾）
                PureMemory.ssss_bbBase_idosaki06_himodukiOteZasi.Sitdown(PureMemory.hot_bb_checkerAr[PureMemory.kifu_nTeban]); // 返討手　は除外するぜ☆（＾▽＾）
            }

            if (flag.HasFlag(MoveType.N06_SuteOteZasi))
            {
                // 移動先
                PureMemory.ssss_bbBase_idosaki07_suteOteZasi.Set(BitboardsOmatome.bb_boardArea);
                yomiIbashoBan.ToSitdown_KomaZenbu(PureMemory.kifu_teban, PureMemory.ssss_bbBase_idosaki07_suteOteZasi);// 味方の駒がある升　は除外☆（＾▽＾）
                yomiIbashoBan.ToSitdown_KomaZenbu(PureMemory.kifu_aiteban, PureMemory.ssss_bbBase_idosaki07_suteOteZasi);// 相手の駒がある升　は除外☆（＾▽＾）
                PureMemory.ssss_bbBase_idosaki07_suteOteZasi.Sitdown(PureMemory.hot_bb_checkerAr[PureMemory.kifu_nTeban]); // 返討手　は除外☆（＾▽＾）
            }

            if (flag.HasFlag(MoveType.N07_SuteOteDa))
            {
                // 持ち駒
                PureMemory.ssss_bbBase_idosaki08_suteOteDa.Set(BitboardsOmatome.bb_boardArea);
                yomiIbashoBan.ToSitdown_KomaZenbu(Taikyokusya.T1, PureMemory.ssss_bbBase_idosaki08_suteOteDa);// 持ち駒の打てる場所　＝　駒が無いところ☆
                yomiIbashoBan.ToSitdown_KomaZenbu(Taikyokusya.T2, PureMemory.ssss_bbBase_idosaki08_suteOteDa);
            }

            if (flag.HasFlag(MoveType.N11_HimodukiOteDa))
            {
                // 持ち駒
                PureMemory.ssss_bbBase_idosaki09_himodukiOteDa.Set(BitboardsOmatome.bb_boardArea);
                yomiIbashoBan.ToSitdown_KomaZenbu(Taikyokusya.T1, PureMemory.ssss_bbBase_idosaki09_himodukiOteDa);// 持ち駒の打てる場所　＝　駒が無いところ☆
                yomiIbashoBan.ToSitdown_KomaZenbu(Taikyokusya.T2, PureMemory.ssss_bbBase_idosaki09_himodukiOteDa);
                PureMemory.gky_ky.yomiKy.yomiShogiban.yomiKikiBan.ToSelect_BBKikiZenbu(PureMemory.kifu_teban, PureMemory.ssss_bbBase_idosaki09_himodukiOteDa);// 紐を付ける☆
            }

            if (flag.HasFlag(MoveType.N08_HimotukiKanmanSasi))
            {
                // 盤面全体
                PureMemory.ssss_bbBase_idosaki11_himodukiKanmanZasi.Set(BitboardsOmatome.bb_boardArea);
                // －　味方の駒がある升　※味方の駒があるところには移動できないぜ☆（＾▽＾）
                yomiIbashoBan.ToSitdown_KomaZenbu(PureMemory.kifu_teban, PureMemory.ssss_bbBase_idosaki11_himodukiKanmanZasi);
                // －　相手の駒がある升　※除外するぜ☆（＾▽＾）
                yomiIbashoBan.ToSitdown_KomaZenbu(PureMemory.kifu_aiteban, PureMemory.ssss_bbBase_idosaki11_himodukiKanmanZasi);
                // －　利きのうち、らいおんを取る手　※除外するぜ☆（＾▽＾）
                yomiIbashoBan.ToSitdown_Koma(Med_Koma.ToRaion(PureMemory.kifu_aiteban), PureMemory.ssss_bbBase_idosaki11_himodukiKanmanZasi);
            }

            if (flag.HasFlag(MoveType.N02_BottiKanmanSasi))
            {
                // 移動先
                PureMemory.ssss_bbBase_idosaki12_bottiKanmanZasi.Set(BitboardsOmatome.bb_boardArea);
                yomiIbashoBan.ToSitdown_KomaZenbu(PureMemory.kifu_teban, PureMemory.ssss_bbBase_idosaki12_bottiKanmanZasi);// 味方の駒があるところには移動できないぜ☆（＾▽＾）
                yomiIbashoBan.ToSitdown_KomaZenbu(PureMemory.kifu_aiteban, PureMemory.ssss_bbBase_idosaki12_bottiKanmanZasi);// 相手の駒がある升　は除外するぜ☆（＾▽＾）
                yomiIbashoBan.ToSitdown_Koma(Med_Koma.ToRaion(PureMemory.kifu_aiteban), PureMemory.ssss_bbBase_idosaki12_bottiKanmanZasi);// 利きのうち、らいおんを取る手　は、除外するぜ☆（＾▽＾）
                PureMemory.ssss_bbBase_idosaki12_bottiKanmanZasi.Sitdown(PureMemory.hot_bb_checkerAr[PureMemory.kifu_nTeban]); // 返討手　は除外するぜ☆（＾▽＾）
            }

            if (flag.HasFlag(MoveType.N03_BottiKanmanDa))
            {
                // 持ち駒
                PureMemory.ssss_bbBase_idosaki13_bottiKanmanDa.Set(BitboardsOmatome.bb_boardArea);
                yomiIbashoBan.ToSitdown_KomaZenbu(Taikyokusya.T1, PureMemory.ssss_bbBase_idosaki13_bottiKanmanDa);// 自駒が無いところ☆
                yomiIbashoBan.ToSitdown_KomaZenbu(Taikyokusya.T2, PureMemory.ssss_bbBase_idosaki13_bottiKanmanDa);// 相手駒が無いところ☆
                yomiKikiBan.ToSitdown_BBKikiZenbu(PureMemory.kifu_teban, PureMemory.ssss_bbBase_idosaki13_bottiKanmanDa);// 味方の利きが利いていない場所☆（＾▽＾）
                yomiKikiBan.ToSitdown_BBKikiZenbu(PureMemory.kifu_aiteban, PureMemory.ssss_bbBase_idosaki13_bottiKanmanDa);// 敵の利きが利いていない場所☆（＾▽＾）
            }

            if (flag.HasFlag(MoveType.N04_SuteKanmanSasi))
            {
                PureMemory.ssss_bbBase_idosaki14_suteKanmanZasi.Set(BitboardsOmatome.bb_boardArea);
                yomiIbashoBan.ToSitdown_KomaZenbu(PureMemory.kifu_teban, PureMemory.ssss_bbBase_idosaki14_suteKanmanZasi);// 味方の駒があるところには移動できないぜ☆（＾▽＾）
                yomiIbashoBan.ToSitdown_KomaZenbu(PureMemory.kifu_aiteban, PureMemory.ssss_bbBase_idosaki14_suteKanmanZasi);// 相手の駒がある升　は除外するぜ☆（＾▽＾）
                yomiIbashoBan.ToSitdown_Koma(Med_Koma.ToRaion(PureMemory.kifu_aiteban), PureMemory.ssss_bbBase_idosaki14_suteKanmanZasi);// 利きのうち、らいおん　を取る手は、除外するぜ☆（＾▽＾）
                PureMemory.ssss_bbBase_idosaki14_suteKanmanZasi.Sitdown(PureMemory.hot_bb_checkerAr[PureMemory.kifu_nTeban]); // 返討手　は除外するぜ☆（＾▽＾）
            }

            if (flag.HasFlag(MoveType.N05_SuteKanmanDa))
            {
                PureMemory.ssss_bbBase_idosaki15_suteKanmanDa.Set(BitboardsOmatome.bb_boardArea);
                yomiIbashoBan.ToSitdown_KomaZenbu(Taikyokusya.T1, PureMemory.ssss_bbBase_idosaki15_suteKanmanDa);// 味方の駒がない升
                yomiIbashoBan.ToSitdown_KomaZenbu(Taikyokusya.T2, PureMemory.ssss_bbBase_idosaki15_suteKanmanDa);// 相手の駒がない升// 2016-12-22 捨てだからと言って、紐を付けないとは限らない☆
                yomiKikiBan.ToSelect_BBKikiZenbu(PureMemory.kifu_aiteban, PureMemory.ssss_bbBase_idosaki15_suteKanmanDa);// 敵の利きが利いている場所に打つぜ☆（＾▽＾）
            }

        }

        public static void Clear_SnapshotBb()
        {
            PureMemory.ssss_movePickerWoNuketaBasho1 = "";
        }

        public static void Clear_SsssUtikiri()
        {
            PureMemory.ssss_tansakuUtikiri_ = TansakuUtikiri.Karappo;
        }

        public static bool CheckUtikiri()
        {
            if (TansakuUtikiri.Karappo != PureMemory.ssss_tansakuUtikiri)
            {
                // 指し手生成を打ち切り
                return true;
            }

            return false;
        }


        public static void ClearMoveList()
        {
            PureMemory.ssss_moveList[PureMemory.tnsk_fukasa].ClearList();
            PureMemory.ssss_moveListBad[PureMemory.tnsk_fukasa].ClearList();
        }


        public static void MergeMoveListGoodBad(
#if DEBUG
            string dbg_hint
#endif
            )
        {
            if (0 < PureMemory.ssss_moveListBad[PureMemory.tnsk_fukasa].listCount)
            {
                /*
#if DEBUG
                Util_Machine.AppendLine("指し手リストのGood,Bad をマージするぜ☆（＾～＾）hint=[{ hint }]");
                Util_Machine.Flush();
#endif
                // */

                Array.Copy(PureMemory.ssss_moveListBad[PureMemory.tnsk_fukasa].moveList, 0, PureMemory.ssss_moveList[PureMemory.tnsk_fukasa].moveList, PureMemory.ssss_moveList[PureMemory.tnsk_fukasa].listCount, PureMemory.ssss_moveListBad[PureMemory.tnsk_fukasa].listCount);
                Array.Copy(PureMemory.ssss_moveListBad[PureMemory.tnsk_fukasa].moveTypeList, 0, PureMemory.ssss_moveList[PureMemory.tnsk_fukasa].moveTypeList, PureMemory.ssss_moveList[PureMemory.tnsk_fukasa].listCount, PureMemory.ssss_moveListBad[PureMemory.tnsk_fukasa].listCount);
                PureMemory.ssss_moveList[PureMemory.tnsk_fukasa].listCount += PureMemory.ssss_moveListBad[PureMemory.tnsk_fukasa].listCount;
                Array.Clear(PureMemory.ssss_moveListBad[PureMemory.tnsk_fukasa].moveList, 0, PureMemory.ssss_moveListBad[PureMemory.tnsk_fukasa].listCount);
                Array.Clear(PureMemory.ssss_moveListBad[PureMemory.tnsk_fukasa].moveTypeList, 0, PureMemory.ssss_moveListBad[PureMemory.tnsk_fukasa].listCount);
                PureMemory.ssss_moveListBad[PureMemory.tnsk_fukasa].listCount = 0;
            }
        }

        public static void AddMoveNariGood()
        {
            if (PureMemory.ssss_genk_tume1) { MoveGenAccessor.ClearMoveList(); }//他の指し手を消し飛ばすぜ☆（＾▽＾）

            Move ss = AbstractConvMove.ToMove01bNariSasi(PureMemory.ssss_ugoki_ms_src, PureMemory.ssss_ugoki_ms_dst);
            Debug.Assert(Move.Toryo != ss, "");
            PureMemory.ssss_moveList[PureMemory.tnsk_fukasa].AddList(ss, PureMemory.ssss_ugoki_kakuteiSsType);
        }
        public static void AddMoveNarazuGood()
        {
            if (PureMemory.ssss_genk_tume1) { MoveGenAccessor.ClearMoveList(); }//他の指し手を消し飛ばすぜ☆（＾▽＾）

            Move ss = AbstractConvMove.ToMove01aNarazuSasi(PureMemory.ssss_ugoki_ms_src, PureMemory.ssss_ugoki_ms_dst);
            Debug.Assert(Move.Toryo != ss, "");
            PureMemory.ssss_moveList[PureMemory.tnsk_fukasa].AddList(ss, PureMemory.ssss_ugoki_kakuteiSsType);
        }

        public static void AddMoveNarazuGoodXorBad()
        {
            Move ss = AbstractConvMove.ToMove01aNarazuSasi(PureMemory.ssss_ugoki_ms_src, PureMemory.ssss_ugoki_ms_dst);

            if (PureMemory.ssss_genk_tume1) { MoveGenAccessor.ClearMoveList(); }//他の指し手を消し飛ばすぜ☆（＾▽＾）

            Debug.Assert(Move.Toryo != ss, "");
            if (PureMemory.IsAkusyu_Ssss)
            {
                PureMemory.ssss_moveListBad[PureMemory.tnsk_fukasa].AddList(ss, PureMemory.ssss_ugoki_kakuteiSsType | PureMemory.GetAkusyuType_Ssss());
            }
            else { PureMemory.ssss_moveList[PureMemory.tnsk_fukasa].AddList(ss, PureMemory.ssss_ugoki_kakuteiSsType); }
        }
        public static void AddMoveNariGoodXorBad()
        {
            Move ss = AbstractConvMove.ToMove01bNariSasi(PureMemory.ssss_ugoki_ms_src, PureMemory.ssss_ugoki_ms_dst);

            if (PureMemory.ssss_genk_tume1) { MoveGenAccessor.ClearMoveList(); }//他の指し手を消し飛ばすぜ☆（＾▽＾）

            Debug.Assert(Move.Toryo != ss, "");
            if (PureMemory.IsAkusyu_Ssss)
            {
                PureMemory.ssss_moveListBad[PureMemory.tnsk_fukasa].AddList(ss, PureMemory.ssss_ugoki_kakuteiSsType | PureMemory.GetAkusyuType_Ssss());
            }
            else { PureMemory.ssss_moveList[PureMemory.tnsk_fukasa].AddList(ss, PureMemory.ssss_ugoki_kakuteiSsType); }
        }
        /// <summary>
        /// 「打」で悪手判定はしていないぜ☆（＾～＾）
        /// </summary>
        public static void AddMoveUttaGood()
        {
            if (PureMemory.ssss_genk_tume1) { MoveGenAccessor.ClearMoveList(); }//他の指し手を消し飛ばすぜ☆（＾▽＾）

            Move ss = AbstractConvMove.ToMove01cUtta(PureMemory.ssss_ugoki_ms_dst, PureMemory.ssss_mot_mks);
            Debug.Assert(Move.Toryo != ss, "");
            PureMemory.ssss_moveList[PureMemory.tnsk_fukasa].AddList(ss, PureMemory.ssss_ugoki_kakuteiSsType);
        }
    }
}
