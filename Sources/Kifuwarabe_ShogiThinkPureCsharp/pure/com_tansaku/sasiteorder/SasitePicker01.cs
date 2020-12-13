#if DEBUG
using kifuwarabe_shogithink.pure.accessor;
using kifuwarabe_shogithink.pure.com.sasiteorder.hioute;
using kifuwarabe_shogithink.pure.com.sasiteorder.seisei;
using kifuwarabe_shogithink.pure.conv.genkyoku.play;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.ky.bb;
using kifuwarabe_shogithink.pure.move;
using System;
using System.Diagnostics;
#else
using kifuwarabe_shogithink.pure.accessor;
using kifuwarabe_shogithink.pure.com.sasiteorder.hioute;
using kifuwarabe_shogithink.pure.com.sasiteorder.seisei;
using kifuwarabe_shogithink.pure.conv.genkyoku.play;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.ky.bb;
using kifuwarabe_shogithink.pure.move;
using System;
using System.Diagnostics;
#endif

namespace kifuwarabe_shogithink.pure.com.sasiteorder
{
    public static class SasitePicker01
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fukasa">指し手リストは、深さ別で、配列を再利用しているぜ☆（＾～＾）</param>
        /// <param name="ky"></param>
        /// <param name="flag"></param>
        /// <param name="sasitelistMerge"></param>
        /// <returns></returns>
        public static void SasitePicker_01(MoveType flag, bool sasitelistMerge)
        {
            #region 前準備
            Debug.Assert(0 <= PureMemory.tnsk_fukasa && PureMemory.tnsk_fukasa < PureMemory.ssss_sasitelist.Length, "");

            //────────────────────────────────────────
            // 勝負無し調査☆
            //────────────────────────────────────────
            // 次の状態では、指し次いではいけないぜ☆（＾～＾）
            if (PureMemory.ssss_isSyobuNasi)
            {
#if DEBUG
                PureMemory.ssss_sasitePickerWoNuketaBasho1 = "無勝負";
#endif
                return;
            }

            #endregion

            #region 詰んでいれば終わり☆
            //────────────────────────────────────────
            // 詰んでいれば終わり☆（＾▽＾）
            //────────────────────────────────────────
            if (Util_Hioute.IsTunderu(PureMemory.kifu_teban))
            {
                goto gt_FlushSasite; // 空っぽで投了だぜ☆（＾▽＾）
            }
            #endregion

            // 変数名短縮
            Kyokumen.YomiKy yomiKy = PureMemory.gky_ky.yomiKy;
            IbashoBan.YomiIbashoBan yomiIbashoBan = PureMemory.gky_ky.yomiKy.yomiShogiban.yomiIbashoBan;
            KikiBan.YomiKikiBan yomiKikiBan = PureMemory.gky_ky.yomiKy.yomiShogiban.yomiKikiBan;

            //──────────
            // よく使う数
            //──────────
            bool hasMotiKoma = !PureMemory.gky_ky.yomiKy.yomiMotigomaItiran.IsEmpty();//持ち駒を持っているなら真

            #region 逼迫返討手
#if DEBUG
            // 絶対に取らなければならない駒があるフラグだぜ☆（＾～＾）
            if (PureMemory.hot_isNigerarenaiCheckerAr[PureMemory.kifu_nTeban])
            {
                int a = 0;
            }
#endif
            //────────────────────────────────────────
            // 絶対に駒を取らないといけない場合で、その駒を取りにいく手☆（略して「返討手」）
            //────────────────────────────────────────
            // ・正当防衛　専門なので、逃げろ手　がある場合は　駒を取りにいかないぜ☆　らいおんが取れても取らないぜ☆
            // ・返り討ちで斬った相手が　らいおん　かどうかまで見てないぜ☆　らいおん斬ったらラッキーということで☆（＾～＾）
            PureMemory.SetKakuteiSsType( MoveType.N13_HippakuKaeriutiTe, false);
            if (flag.HasFlag(PureMemory.ssss_ugoki_kakuteiSsType))
            {
                if (!PureMemory.ssss_bbBase_idosaki01_checker.IsEmpty())
                {
                    foreach (Koma iKm_teban in Conv_Koma.itiranTuyoimonoJun[PureMemory.kifu_nTeban])// 駒について、弱い駒から順
                    {
                        PureMemory.SetSsssUgokiKm(iKm_teban);
                        PureMemory.gky_ky.yomiKy.yomiShogiban.yomiIbashoBan.ToSet_Koma(PureMemory.ssss_ugoki_km, PureMemory.ssssTmp_bbVar_ibasho);
                        while (PureMemory.ssssTmp_bbVar_ibasho.Ref_PopNTZ(out PureMemory.ssss_ugoki_ms_src))
                        {
                            if (MoveGenAccessor.CheckUtikiri())
                            {
#if DEBUG
                                PureMemory.ssss_sasitePickerWoNuketaBasho1 = "逼迫返討手";
#endif
                                goto gt_FlushSasite;
                            }// 指し手生成終了☆

                            // 移動先リセット
                            PureMemory.ssss_bbVar_idosaki_narazu.Set(PureMemory.ssss_bbBase_idosaki01_checker);
                            BitboardsOmatome.KomanoUgokikataYk00.ToSelect_MergeShogiban(PureMemory.ssss_ugoki_km, PureMemory.ssss_ugoki_ms_src, PureMemory.ssss_bbVar_idosaki_narazu);
                            PureMemory.ssss_bbVar_idosaki_nari.Set(PureMemory.ssss_bbVar_idosaki_narazu);
                            // 進んで構わない場所に絞り込むぜ☆（＾～＾）
                            PureMemory.ssss_bbVar_idosaki_narazu.Siborikomi(BitboardsOmatome.bb_uteruZone[(int)PureMemory.ssss_ugoki_km]);
                            GenerateSasite03.SiborikomiNareruZone();

                            switch (PureMemory.ssss_ugoki_ks)
                            {
                                case Komasyurui.R:
                                    GenerateSasite02.GenerateRaion_HippakuKaeriutiTe();
                                    break;

                                case Komasyurui.Z: // thru
                                case Komasyurui.K:
                                case Komasyurui.H:
                                case Komasyurui.N:
                                case Komasyurui.U:
                                case Komasyurui.S:
                                    GenerateSasite02.GenerateNk_HippakuKaeriutiTe();
                                    break;

                                case Komasyurui.PZ: // thru
                                case Komasyurui.PK:
                                case Komasyurui.I:
                                case Komasyurui.PH:
                                case Komasyurui.PN:
                                case Komasyurui.PU:
                                case Komasyurui.PS:
                                    GenerateSasite02.GenerateXk_HippakuKaeriutiTe();
                                    break;
                            }
                        }
                    }
                }
            }

            // 絶対に取らなければならない駒があるフラグだぜ☆（＾～＾）
            if (PureMemory.hot_isNigerarenaiCheckerAr[PureMemory.kifu_nTeban])
            {
                // このフラグが立っているのに　ここに来たということは、
                // TODO: 先に　らいおん　を取れるか、
                // あるいは、投了だぜ☆（＾～＾）
                goto gt_FlushSasite;
            }
            #endregion

            #region 余裕返討手
            // 逃げることもできるが、王手をしてきた駒を取る手☆
            PureMemory.SetKakuteiSsType( MoveType.N14_YoyuKaeriutiTe, false);
            if (flag.HasFlag(PureMemory.ssss_ugoki_kakuteiSsType))
            {
                if (!PureMemory.ssss_bbBase_idosaki01_checker.IsEmpty())
                {
                    foreach (Koma iKm_teban in Conv_Koma.itiranTuyoimonoJun[PureMemory.kifu_nTeban])// 弱い駒から順
                    {
                        PureMemory.SetSsssUgokiKm(iKm_teban);
                        yomiIbashoBan.ToSet_Koma(PureMemory.ssss_ugoki_km, PureMemory.ssssTmp_bbVar_ibasho);
                        while (PureMemory.ssssTmp_bbVar_ibasho.Ref_PopNTZ(out PureMemory.ssss_ugoki_ms_src))
                        {
                            if (MoveGenAccessor.CheckUtikiri())
                            {
#if DEBUG
                                PureMemory.ssss_sasitePickerWoNuketaBasho1 = "余裕返討手";
#endif
                                goto gt_FlushSasite;
                            }// 指し手生成終了☆

                            // 移動先リセット
                            PureMemory.ssss_bbVar_idosaki_narazu.Set(PureMemory.ssss_bbBase_idosaki01_checker);
                            BitboardsOmatome.KomanoUgokikataYk00.ToSelect_MergeShogiban(PureMemory.ssss_ugoki_km, PureMemory.ssss_ugoki_ms_src, PureMemory.ssss_bbVar_idosaki_narazu);
                            PureMemory.ssss_bbVar_idosaki_nari.Set(PureMemory.ssss_bbVar_idosaki_narazu);
                            // 進んで構わない場所に絞り込むぜ☆（＾～＾）
                            PureMemory.ssss_bbVar_idosaki_narazu.Siborikomi(BitboardsOmatome.bb_uteruZone[(int)PureMemory.ssss_ugoki_km]);
                            GenerateSasite03.SiborikomiNareruZone();

                            switch (PureMemory.ssss_ugoki_ks)
                            {
                                case Komasyurui.R:
                                    GenerateSasite02.GenerateRaion_YoyuKaeriutiTe();
                                    break;

                                case Komasyurui.Z: // thru
                                case Komasyurui.K:
                                case Komasyurui.H:
                                case Komasyurui.N:
                                case Komasyurui.U:
                                case Komasyurui.S:
                                    GenerateSasite02.GenerateNk_YoyuKaeriutiTe();
                                    break;

                                case Komasyurui.PZ: // thru
                                case Komasyurui.PK:
                                case Komasyurui.I:
                                case Komasyurui.PH:
                                case Komasyurui.PN:
                                case Komasyurui.PU:
                                case Komasyurui.PS:
                                    GenerateSasite02.GenerateXk_YoyuKaeriutiTe();
                                    break;
                            }
                        }
                    }
                }
            }
            #endregion

            #region らいおんキャッチ
            //────────────────────────────────────────
            // らいおんを取る手☆
            //────────────────────────────────────────
            // どの　指し手タイプ　でも、らいおんを捕まえる手があるかどうかは調べたいぜ☆（＾～＾）
            {
                if (flag.HasFlag(MoveType.N12_RaionCatch))
                {
                    PureMemory.SetKakuteiSsType( MoveType.N12_RaionCatch,false);
                }
                else
                {
                    // らいおんを取る手　以外のタイプでは、調査するだけだぜ☆（＾～＾）
                    PureMemory.SetKakuteiSsType( MoveType.N17_RaionCatchChosa,false);
                    PureMemory.hot_raionCatchChosaAr[PureMemory.kifu_nTeban] = false;
                }

                // らいおんと、らいおんが向かい合っている場合は、返討手でも　らいおんキャッチ扱いにする。
                if (!PureMemory.ssss_bbBase_idosaki02_raionCatch.IsEmpty())
                {
                    int ssCount_old = PureMemory.ssss_sasitelist[PureMemory.tnsk_fukasa].listCount;

                    foreach (Koma iKm_teban in Conv_Koma.itiranTuyoimonoJun[PureMemory.kifu_nTeban])// 弱い駒から順
                    {
                        PureMemory.SetSsssUgokiKm(iKm_teban);
                        yomiIbashoBan.ToSet_Koma(PureMemory.ssss_ugoki_km, PureMemory.ssssTmp_bbVar_ibasho);
                        while (PureMemory.ssssTmp_bbVar_ibasho.Ref_PopNTZ(out PureMemory.ssss_ugoki_ms_src))
                        {
                            if (MoveGenAccessor.CheckUtikiri())
                            {
#if DEBUG
                                PureMemory.ssss_sasitePickerWoNuketaBasho1 = "らいおんキャッチ";
#endif
                                goto gt_FlushSasite;
                            }// 指し手生成終了☆

                            // 移動先リセット
                            PureMemory.ssss_bbVar_idosaki_narazu.Set(PureMemory.ssss_bbBase_idosaki02_raionCatch);
                            BitboardsOmatome.KomanoUgokikataYk00.ToSelect_MergeShogiban(PureMemory.ssss_ugoki_km, PureMemory.ssss_ugoki_ms_src, PureMemory.ssss_bbVar_idosaki_narazu);
                            PureMemory.ssss_bbVar_idosaki_nari.Set(PureMemory.ssss_bbVar_idosaki_narazu);
                            // 進んで構わない場所に絞り込むぜ☆（＾～＾）
                            PureMemory.ssss_bbVar_idosaki_narazu.Siborikomi(BitboardsOmatome.bb_uteruZone[(int)PureMemory.ssss_ugoki_km]);
                            GenerateSasite03.SiborikomiNareruZone();

                            switch (PureMemory.ssss_ugoki_ks)
                            {
                                case Komasyurui.Z: // thru
                                case Komasyurui.K:
                                case Komasyurui.H:
                                case Komasyurui.I:
                                case Komasyurui.N:
                                case Komasyurui.U:
                                case Komasyurui.S:
                                    GenerateSasite02.GenerateNk_RaionCatch();
                                    break;

                                case Komasyurui.R: // thru
                                case Komasyurui.PZ:
                                case Komasyurui.PK:
                                case Komasyurui.PH:
                                case Komasyurui.PN:
                                case Komasyurui.PU:
                                case Komasyurui.PS:
                                    GenerateSasite02.GenerateRaionXk_RaionCatch();
                                    break;
                            }

                            // らいおんを捕まえる手が１手でもあれば十分☆ これ以降の手は作らないぜ☆（＾～＾）
                            if (ssCount_old < PureMemory.ssss_sasitelist[PureMemory.tnsk_fukasa].listCount || PureMemory.hot_raionCatchChosaAr[PureMemory.kifu_nTeban])
                            {
#if DEBUG
                                PureMemory.ssss_sasitePickerWoNuketaBasho1 = "らいおんキャッチ";
#endif
                                goto gt_FlushSasite;
                            }
                        }
                    }
                }
            }
            #endregion

            #region 逃げろ手
            //────────────────────────────────────────
            // 逃げろ手☆
            //────────────────────────────────────────
            if (!PureMemory.hot_bb_nigeroAr[PureMemory.kifu_nTeban].IsEmpty()// 逃げなければいけないのなら、逃げろだぜ☆（＾▽＾）
                                               //&& TansakuUtikiri.RaionTukamaeta != hioute.TansakuUtikiri
                                               // FIXME: ただし、返討手の中に「らいおんを取る手」がある場合、逃げてる場合じゃないぜ☆（＾～＾）；；
                                               // FIXME: 返討手は、「らいおんを取れ」じゃないんだぜ☆（＞＿＜）
                )
            {
                PureMemory.SetKakuteiSsType( MoveType.N15_NigeroTe,false);
                if (flag.HasFlag(PureMemory.ssss_ugoki_kakuteiSsType))
                {
                    if (!PureMemory.ssss_bbBase_idosaki03_nigeroTe.IsEmpty())
                    {
                        // 移動先リセット
                        PureMemory.ssss_bbVar_idosaki_narazu.Set(PureMemory.ssss_bbBase_idosaki03_nigeroTe);
                        PureMemory.ssss_bbVar_idosaki_nari.Clear();
                        // らいおんに、「成り」も「移動できない場所」も無いぜ☆（＾～＾）

                        while (PureMemory.ssss_bbVar_idosaki_narazu.Ref_PopNTZ(out PureMemory.ssss_ugoki_ms_dst))
                        {
                            Move ss = AbstractConvMove.ToSasite_01a_NarazuSasi(
                                PureMemory.hot_ms_raionAr[PureMemory.kifu_nTeban], PureMemory.ssss_ugoki_ms_dst
                                );
                            PureMemory.ssss_sasitelist[PureMemory.tnsk_fukasa].AddList(ss, MoveType.N15_NigeroTe);
                        }
                    }
                }
#if DEBUG
                PureMemory.ssss_sasitePickerWoNuketaBasho1 = "逃げろ手";
#endif
                goto gt_FlushSasite;
            }
            #endregion

            #region トライ
            //────────────────────────────────────────
            // トライする手☆　（らいおん　のみ）
            //────────────────────────────────────────
            PureMemory.SetKakuteiSsType( MoveType.N16_Try,false);
            if (flag.HasFlag(PureMemory.ssss_ugoki_kakuteiSsType))
            {
                if (!PureMemory.ssss_bbBase_idosaki04_try.IsEmpty())
                {
                    // らいおん
                    PureMemory.SetSsssUgokiKm(Med_Koma.KomasyuruiAndTaikyokusyaToKoma(Komasyurui.R, PureMemory.kifu_teban));
                    yomiIbashoBan.ToSet_Koma(Med_Koma.ToRaion(PureMemory.kifu_teban), PureMemory.ssssTmp_bbVar_ibasho);
                    while (PureMemory.ssssTmp_bbVar_ibasho.Ref_PopNTZ(out PureMemory.ssss_ugoki_ms_src))
                    {
                        if (MoveGenAccessor.CheckUtikiri())
                        {
#if DEBUG
                            PureMemory.ssss_sasitePickerWoNuketaBasho1 = "トライ";
#endif
                            goto gt_FlushSasite;
                        }// 指し手生成終了☆

                        // 移動先リセット
                        PureMemory.ssss_bbVar_idosaki_narazu.Set(PureMemory.ssss_bbBase_idosaki04_try);
                        BitboardsOmatome.KomanoUgokikataYk00.ToSelect_MergeShogiban(PureMemory.ssss_ugoki_km, PureMemory.ssss_ugoki_ms_src, PureMemory.ssss_bbVar_idosaki_narazu);
                        PureMemory.ssss_bbVar_idosaki_nari.Set(PureMemory.ssss_bbVar_idosaki_narazu);
                        // 進んで構わない場所に絞り込むぜ☆（＾～＾）
                        PureMemory.ssss_bbVar_idosaki_narazu.Siborikomi(BitboardsOmatome.bb_uteruZone[(int)PureMemory.ssss_ugoki_km]);
                        GenerateSasite03.SiborikomiNareruZone();

                        GenerateSasite02.GenerateRaion_Try();

                        // トライする手が１手でもあれば十分☆ 指し手生成終了☆（＾▽＾）
                        if (0 < PureMemory.ssss_sasitelist[PureMemory.tnsk_fukasa].listCount)
                        {
                            goto gt_FlushSasite;
                        }
                    }
                }
            }
            #endregion

            #region 駒を取る手（Good 逃げ道を開ける手、Bad 逃げ道を開けない手）
            //────────────────────────────────────────
            // 駒を取る手☆（Good 逃げ道を開ける手、Bad 逃げ道を開けない手）
            //────────────────────────────────────────
            PureMemory.SetKakuteiSsType( MoveType.N01_KomaWoToruTe,false);
            if (flag.HasFlag(PureMemory.ssss_ugoki_kakuteiSsType))
            {
                if (!PureMemory.ssss_bbBase_idosaki05_komaWoToruTe.IsEmpty())
                {
                    foreach (Koma iKm_teban in Conv_Koma.itiranTuyoimonoJun[PureMemory.kifu_nTeban])// 弱い駒から順
                    {
                        PureMemory.SetSsssUgokiKm(iKm_teban);
                        yomiIbashoBan.ToSet_Koma(PureMemory.ssss_ugoki_km, PureMemory.ssssTmp_bbVar_ibasho);
                        while (PureMemory.ssssTmp_bbVar_ibasho.Ref_PopNTZ(out PureMemory.ssss_ugoki_ms_src))
                        {
                            if (TansakuUtikiri.Karappo != PureMemory.ssss_tansakuUtikiri)
                            {
#if DEBUG
                                PureMemory.ssss_sasitePickerWoNuketaBasho1 = "駒を取る手";
#endif
                                goto gt_FlushSasite;
                            }// 指し手生成終了☆

                            // 移動先リセット
                            PureMemory.ssss_bbVar_idosaki_narazu.Set(PureMemory.ssss_bbBase_idosaki05_komaWoToruTe);
                            BitboardsOmatome.KomanoUgokikataYk00.ToSelect_MergeShogiban(PureMemory.ssss_ugoki_km, PureMemory.ssss_ugoki_ms_src, PureMemory.ssss_bbVar_idosaki_narazu);
                            PureMemory.ssss_bbVar_idosaki_nari.Set(PureMemory.ssss_bbVar_idosaki_narazu);
                            // 進んで構わない場所に絞り込むぜ☆（＾～＾）
                            PureMemory.ssss_bbVar_idosaki_narazu.Siborikomi(BitboardsOmatome.bb_uteruZone[(int)PureMemory.ssss_ugoki_km]);
                            GenerateSasite03.SiborikomiNareruZone();

                            switch (PureMemory.ssss_ugoki_ks)
                            {
                                case Komasyurui.R:
                                    GenerateSasite02.GenerateRaion_KomaWoToruTe();
                                    break;

                                case Komasyurui.Z: // thuru
                                case Komasyurui.PZ:
                                case Komasyurui.K:
                                case Komasyurui.PK:
                                case Komasyurui.H:
                                case Komasyurui.I:
                                case Komasyurui.N:
                                case Komasyurui.U:
                                case Komasyurui.S:
                                    GenerateSasite02.GenerateNk_KomaWoToruTe();
                                    break;

                                case Komasyurui.PH: // thru
                                case Komasyurui.PN:
                                case Komasyurui.PU:
                                case Komasyurui.PS:
                                    GenerateSasite02.GenerateXk_KomaWoToruTe();
                                    break;

                                default: throw new Exception("未定義の駒種類 km=" + PureMemory.ssss_ugoki_km);
                            }
                        }
                    }
                }
            }
            #endregion

            #region GoodBadマージ　駒を取る手（Good 逃げ道を開ける手、Bad 逃げ道を開けない手）
            if (flag.HasFlag(MoveType.N18_Option_MergeGoodBad))
            {
                // マージをするぜ☆（＾▽＾）
                MoveGenAccessor.MergeSasitelistGoodBad(
#if DEBUG
                    "マージ　盤上駒で紐付王手（逃げ道を開ける手）"
#endif
                    );
            }
            #endregion

            // ぼっち　と　王手　は組み合わないぜ☆(＾◇＾)　捨て王手、または　紐付王手　になるからな☆（＾▽＾）

            #region 紐付王手指（Good 逃げ道を開けない手、Bad 逃げ道を開ける手）
            //────────────────────────────────────────
            // 紐付王手指☆（Good 逃げ道を開けない手、Bad 逃げ道を開ける手）（らいおんを除く☆）
            //────────────────────────────────────────
            PureMemory.SetKakuteiSsType( MoveType.N10_HimozukiOteZasi,false);
            if (flag.HasFlag(PureMemory.ssss_ugoki_kakuteiSsType))
            {
                if (!PureMemory.ssss_bbBase_idosaki06_himodukiOteZasi.IsEmpty())
                {
                    foreach (Koma iKm_teban in Conv_Koma.itiranTuyoimonoJun[PureMemory.kifu_nTeban])// 弱い駒から順
                    {
                        PureMemory.SetSsssUgokiKm(iKm_teban);
                        yomiIbashoBan.ToSet_Koma(PureMemory.ssss_ugoki_km, PureMemory.ssssTmp_bbVar_ibasho);
                        while (PureMemory.ssssTmp_bbVar_ibasho.Ref_PopNTZ(out PureMemory.ssss_ugoki_ms_src))
                        {
                            if (MoveGenAccessor.CheckUtikiri())
                            {
#if DEBUG
                                PureMemory.ssss_sasitePickerWoNuketaBasho1 = "紐付王手指";
#endif
                                goto gt_FlushSasite;
                            }// 指し手生成終了☆

                            // 移動先リセット
                            PureMemory.ssss_bbVar_idosaki_narazu.Set(PureMemory.ssss_bbBase_idosaki06_himodukiOteZasi);
                            PureMemory.ssss_bbVar_idosaki_narazu.Siborikomi(Util_Bitboard.CreateBBTebanKikiZenbu_1KomaNozoku(PureMemory.ssss_ugoki_ms_src));// 自分以外の味方の駒の利き（紐）を付ける☆
                            BitboardsOmatome.KomanoUgokikataYk00.ToSelect_MergeShogiban(PureMemory.ssss_ugoki_km, PureMemory.ssss_ugoki_ms_src, PureMemory.ssss_bbVar_idosaki_narazu);
                            PureMemory.ssss_bbVar_idosaki_nari.Set(PureMemory.ssss_bbVar_idosaki_narazu);
                            // 進んで構わない場所に絞り込むぜ☆（＾～＾）
                            PureMemory.ssss_bbVar_idosaki_narazu.Siborikomi(BitboardsOmatome.bb_uteruZone[(int)PureMemory.ssss_ugoki_km]);
                            GenerateSasite03.SiborikomiNareruZone();

                            switch (PureMemory.ssss_ugoki_ks)
                            {
                                case Komasyurui.R:
                                    break;// らいおんは　王手しないぜ☆（＾▽＾）

                                case Komasyurui.Z: // thru
                                case Komasyurui.K:
                                case Komasyurui.H:
                                case Komasyurui.N:
                                case Komasyurui.U:
                                case Komasyurui.S:
                                    GenerateSasite02.GenerateNk_HimodukiOteZasi();
                                    break;

                                case Komasyurui.PZ: // thru
                                case Komasyurui.PK:
                                case Komasyurui.I:
                                case Komasyurui.PH:
                                case Komasyurui.PN:
                                case Komasyurui.PU:
                                case Komasyurui.PS:
                                    GenerateSasite02.GenerateXk_HimodukiOteZasi();
                                    break;
                            }
                        }
                    }
                }
            }
            #endregion

            #region 捨て王手指（Good 逃げ道を開けない手、Bad 逃げ道を開ける手）
            //────────────────────────────────────────
            // 捨て王手指☆（らいおんを除く☆）
            //────────────────────────────────────────
            PureMemory.SetKakuteiSsType( MoveType.N06_SuteOteZasi,false);
            if (flag.HasFlag(PureMemory.ssss_ugoki_kakuteiSsType))
            {
                if (!PureMemory.ssss_bbBase_idosaki07_suteOteZasi.IsEmpty())
                {
                    // 2016-12-22 捨てだからと言って、紐を付けないとは限らない☆

                    foreach (Koma iKm_teban in Conv_Koma.itiranTuyoimonoJun[PureMemory.kifu_nTeban])// 弱い駒から順
                    {
                        PureMemory.SetSsssUgokiKm(iKm_teban);
                        yomiIbashoBan.ToSet_Koma(PureMemory.ssss_ugoki_km, PureMemory.ssssTmp_bbVar_ibasho);
                        while (PureMemory.ssssTmp_bbVar_ibasho.Ref_PopNTZ(out PureMemory.ssss_ugoki_ms_src))
                        {
                            if (MoveGenAccessor.CheckUtikiri())
                            {
#if DEBUG
                                PureMemory.ssss_sasitePickerWoNuketaBasho1 = "捨て王手指";
#endif
                                goto gt_FlushSasite;
                            }// 指し手生成終了☆

                            // 移動先リセット
                            PureMemory.ssss_bbVar_idosaki_narazu.Set(PureMemory.ssss_bbBase_idosaki07_suteOteZasi);
                            BitboardsOmatome.KomanoUgokikataYk00.ToSelect_MergeShogiban(PureMemory.ssss_ugoki_km, PureMemory.ssss_ugoki_ms_src, PureMemory.ssss_bbVar_idosaki_narazu);
                            PureMemory.ssss_bbVar_idosaki_nari.Set(PureMemory.ssss_bbVar_idosaki_narazu);
                            // 進んで構わない場所に絞り込むぜ☆（＾～＾）
                            PureMemory.ssss_bbVar_idosaki_narazu.Siborikomi(BitboardsOmatome.bb_uteruZone[(int)PureMemory.ssss_ugoki_km]);
                            GenerateSasite03.SiborikomiNareruZone();

                            switch (PureMemory.ssss_ugoki_ks)
                            {
                                case Komasyurui.R:// らいおんは　王手しないぜ☆（＾▽＾）
                                    break;

                                case Komasyurui.Z: // thru
                                case Komasyurui.K:
                                case Komasyurui.H:
                                case Komasyurui.N:
                                case Komasyurui.U:
                                case Komasyurui.S:
                                    GenerateSasite02.GenerateNk_SuteOteZasi();
                                    break;

                                case Komasyurui.PZ: // thru
                                case Komasyurui.PK:
                                case Komasyurui.I:
                                case Komasyurui.PH:
                                case Komasyurui.PN:
                                case Komasyurui.PU:
                                case Komasyurui.PS:
                                    GenerateSasite02.GenerateNx_SuteOteZasi();
                                    break;
                            }
                        }
                    }
                }
            }
            #endregion

            if (hasMotiKoma)
            {
                #region 捨て王手打（Good 逃げ道を開けない手、Bad 逃げ道を開ける手）
                //────────────────────────────────────────
                // 捨て王手打☆
                //────────────────────────────────────────
                PureMemory.SetKakuteiSsType( MoveType.N07_SuteOteDa,true);
                if (flag.HasFlag(PureMemory.ssss_ugoki_kakuteiSsType))
                {
                    // 2016-12-22 捨てだからと言って、紐を付けないとは限らない☆
                    //& ~ky.BB_KikiZenbu[(int)teban]// 紐を付けない☆
                    if (!PureMemory.ssss_bbBase_idosaki08_suteOteDa.IsEmpty())
                    {
                        // 順を付けて、指し手を調べようぜ☆（＾▽＾）
                        foreach (MotigomaSyurui iMks in Conv_MotigomaSyurui.itiranTuyoimonoJun)
                        {
                            if(PureMemory.SetSsssMotMks_AndHasMotigoma(iMks))
                            {
                                // 移動先リセット
                                PureMemory.ssss_bbVar_idosaki_narazu.Set(PureMemory.ssss_bbBase_idosaki08_suteOteDa);
                                PureMemory.ssss_bbVar_idosaki_nari.Clear();// 「打」に「成り」は無いぜ☆（＾～＾）

                                // 二歩防止
                                if (MotigomaSyurui.H == PureMemory.ssss_mot_mks) { GenerateSasite03.SiborikomiByNifu(); }
                                // 打って構わない場所に絞り込むぜ☆（＾～＾）
                                PureMemory.ssss_bbVar_idosaki_narazu.Siborikomi(BitboardsOmatome.bb_uteruZone[(int)PureMemory.ssss_mot_km]);

                                GenerateSasite02.GenerateMotigoma_SuteDa();
                            }
                        }

#if DEBUG
                        PureMemory.ssss_sasitePickerWoNuketaBasho1 = "捨て王手打";
#endif
                    }
                }
                #endregion

                #region 紐付王手打（Good 逃げ道を開けない手、Bad 逃げ道を開ける手）
                //────────────────────────────────────────
                // 紐付王手打☆
                //────────────────────────────────────────
                PureMemory.SetKakuteiSsType( MoveType.N11_HimodukiOteDa,true);
                if (flag.HasFlag(PureMemory.ssss_ugoki_kakuteiSsType))
                {
                    if (!PureMemory.ssss_bbBase_idosaki09_himodukiOteDa.IsEmpty())
                    {
                        // 順を付けて、指し手を調べようぜ☆（＾▽＾）
                        foreach (MotigomaSyurui iMks in Conv_MotigomaSyurui.itiranTuyoimonoJun)
                        {
                            if (PureMemory.SetSsssMotMks_AndHasMotigoma(iMks))
                            {
                                // 移動先リセット
                                PureMemory.ssss_bbVar_idosaki_narazu.Set(PureMemory.ssss_bbBase_idosaki09_himodukiOteDa);
                                PureMemory.ssss_bbVar_idosaki_nari.Clear();// 「打」に「成り」は無いぜ☆（＾～＾）

                                // 王手に限る。らいおんのいる升に、先後逆の自分の駒があると考えれば、その利きの場所と、今いる場所からの利きが重なれば、王手だぜ☆（＾▽＾）
                                BitboardsOmatome.KomanoUgokikataYk00.ToSelect_MergeShogiban(Med_Koma.KomasyuruiAndTaikyokusyaToKoma(PureMemory.ssss_mot_ks, PureMemory.kifu_aiteban), PureMemory.hot_ms_raionAr[PureMemory.kifu_nAiteban], PureMemory.ssss_bbVar_idosaki_narazu);

                                // 二歩防止
                                if (MotigomaSyurui.H == PureMemory.ssss_mot_mks) { GenerateSasite03.SiborikomiByNifu(); }
                                // 打って構わない場所に絞り込むぜ☆（＾～＾）
                                PureMemory.ssss_bbVar_idosaki_narazu.Siborikomi(BitboardsOmatome.bb_uteruZone[(int)PureMemory.ssss_mot_km]);
                                GenerateSasite02.GenerateMotigoma_NormalDa();
                            }
                        }

#if DEBUG
                        PureMemory.ssss_sasitePickerWoNuketaBasho1 = "紐付王手打";
#endif
                    }
                }
                #endregion
            }

            #region GoodBadマージ　紐付王手指（Good 逃げ道を開ける手、Bad 逃げ道を開けない手）
            if (flag.HasFlag(MoveType.N18_Option_MergeGoodBad))
            {
                // マージをするぜ☆（＾▽＾）
                MoveGenAccessor.MergeSasitelistGoodBad(
#if DEBUG
                    "GoodBadマージ　紐付王手指（Good 逃げ道を開ける手、Bad 逃げ道を開けない手）"
#endif
                    );
            }
            #endregion

            if (hasMotiKoma)
            {
                #region 紐付緩慢打
                //────────────────────────────────────────
                // 紐付緩慢打☆
                //────────────────────────────────────────
                if (flag.HasFlag(MoveType.N09_HimotukiKanmanDa))
                {
                    // 持ち駒
                    PureMemory.ssss_bbBase_idosaki10_himodukiKanmanDa.Set(BitboardsOmatome.bb_boardArea);
                    PureMemory.gky_ky.shogiban.yomiIbashoBan_yoko.ToSitdown_KomaZenbu(Taikyokusya.T1, PureMemory.ssss_bbBase_idosaki10_himodukiKanmanDa);// 持ち駒の打てる場所　＝　駒が無いところ☆
                    PureMemory.gky_ky.shogiban.yomiIbashoBan_yoko.ToSitdown_KomaZenbu(Taikyokusya.T2, PureMemory.ssss_bbBase_idosaki10_himodukiKanmanDa);
                    yomiKikiBan.ToSelect_BBKikiZenbu(PureMemory.kifu_teban, PureMemory.ssss_bbBase_idosaki10_himodukiKanmanDa);// 紐を付ける☆
                }
                PureMemory.SetKakuteiSsType( MoveType.N09_HimotukiKanmanDa,true);
                if (flag.HasFlag(PureMemory.ssss_ugoki_kakuteiSsType))
                {
                    //utuBB &= ~ky.BB_KikiZenbu[(int)相手];// 敵の利きが利いていない場所に打つぜ☆（＾▽＾）
                    if (!PureMemory.ssss_bbBase_idosaki10_himodukiKanmanDa.IsEmpty())
                    {
                        // 順を付けて、指し手を調べようぜ☆（＾▽＾）
                        foreach (MotigomaSyurui iMks in Conv_MotigomaSyurui.itiranTuyoimonoJun)
                        {
                            if (PureMemory.SetSsssMotMks_AndHasMotigoma(iMks))
                            {
                                // 移動先リセット
                                PureMemory.ssss_bbVar_idosaki_narazu.Set(PureMemory.ssss_bbBase_idosaki10_himodukiKanmanDa);
                                PureMemory.ssss_bbVar_idosaki_nari.Clear();// 「打」に「成り」は無いぜ☆（＾～＾）

                                // 王手を除く☆ らいおんのいる升に、先後逆の自分の駒があると考えれば、その利きの場所と、今いる場所からの利きが重なれば、王手だぜ☆（＾▽＾）
                                BitboardsOmatome.KomanoUgokikataYk00.ToSitdown_Merge(
                                    Med_Koma.KomasyuruiAndTaikyokusyaToKoma(PureMemory.ssss_mot_ks, PureMemory.kifu_aiteban),
                                    PureMemory.hot_ms_raionAr[PureMemory.kifu_nAiteban], PureMemory.ssss_bbVar_idosaki_narazu);

                                // 二歩防止
                                if (MotigomaSyurui.H == PureMemory.ssss_mot_mks) { GenerateSasite03.SiborikomiByNifu(); }
                                // 打って構わない場所に絞り込むぜ☆（＾～＾）
                                PureMemory.ssss_bbVar_idosaki_narazu.Siborikomi(BitboardsOmatome.bb_uteruZone[(int)PureMemory.ssss_mot_km]);

                                GenerateSasite02.GenerateMotigoma_NormalDa();
                            }
                        }
#if DEBUG
                        PureMemory.ssss_sasitePickerWoNuketaBasho1 = "紐付緩慢打";
#endif
                    }
                }
                #endregion
            }


            #region 紐付緩慢指☆（Good 仲間を見捨てない動き、Bad 仲間を見捨てる動き）
            //────────────────────────────────────────
            // 紐付緩慢指☆（Good 仲間を見捨てない動き、Bad 仲間を見捨てる動き）
            //────────────────────────────────────────
            PureMemory.SetKakuteiSsType( MoveType.N08_HimotukiKanmanSasi,false);
            if (flag.HasFlag(PureMemory.ssss_ugoki_kakuteiSsType))
            {
                if (!PureMemory.ssss_bbBase_idosaki11_himodukiKanmanZasi.IsEmpty())
                {
                    foreach (Koma iKm_teban in Conv_Koma.itiranTuyoimonoJun[PureMemory.kifu_nTeban])// 弱い駒から順
                    {
                        PureMemory.SetSsssUgokiKm(iKm_teban);
                        yomiIbashoBan.ToSet_Koma(PureMemory.ssss_ugoki_km, PureMemory.ssssTmp_bbVar_ibasho);
                        while (PureMemory.ssssTmp_bbVar_ibasho.Ref_PopNTZ(out PureMemory.ssss_ugoki_ms_src))
                        {
                            if (MoveGenAccessor.CheckUtikiri())
                            {
#if DEBUG
                                PureMemory.ssss_sasitePickerWoNuketaBasho1 = string.Format(
                                    "紐付緩慢指Nuke utikiri={0}", PureMemory.ssss_tansakuUtikiri);
#endif
                                goto gt_FlushSasite;
                            }// 指し手生成終了☆

                            // 移動先リセット
                            PureMemory.ssss_bbVar_idosaki_narazu.Set(PureMemory.ssss_bbBase_idosaki11_himodukiKanmanZasi);
                            // 自分以外の味方の駒の利き（紐）を付ける☆
                            PureMemory.ssss_bbVar_idosaki_narazu.Siborikomi(Util_Bitboard.CreateBBTebanKikiZenbu_1KomaNozoku(PureMemory.ssss_ugoki_ms_src));
                            BitboardsOmatome.KomanoUgokikataYk00.ToSelect_MergeShogiban(PureMemory.ssss_ugoki_km, PureMemory.ssss_ugoki_ms_src, PureMemory.ssss_bbVar_idosaki_narazu);
                            PureMemory.ssss_bbVar_idosaki_nari.Set(PureMemory.ssss_bbVar_idosaki_narazu);
                            // 進んで構わない場所に絞り込むぜ☆（＾～＾）
                            PureMemory.ssss_bbVar_idosaki_narazu.Siborikomi(BitboardsOmatome.bb_uteruZone[(int)PureMemory.ssss_ugoki_km]);
                            GenerateSasite03.SiborikomiNareruZone();

                            switch (PureMemory.ssss_ugoki_ks)
                            {
                                case Komasyurui.R: GenerateSasite02.GenerateRaion_BottiKanmanZasi_HimodukiKanmanZasi(); break;
                                case Komasyurui.Z: GenerateSasite02.Generate02ZouKirinNado_bottiKanmanZasi_himodukiKanmanZasi(); break;
                                case Komasyurui.PZ: GenerateSasite02.Generate02ZouKirinNado_bottiKanmanZasi_himodukiKanmanZasi(); break;
                                case Komasyurui.K: GenerateSasite02.Generate02ZouKirinNado_bottiKanmanZasi_himodukiKanmanZasi(); break;
                                case Komasyurui.PK: GenerateSasite02.Generate02ZouKirinNado_bottiKanmanZasi_himodukiKanmanZasi(); break;
                                case Komasyurui.H: GenerateSasite02.Generate02HiyokoNado_BottiKanmanZasi_HimodukiKanmanZasi(); break;
                                case Komasyurui.PH:
                                    GenerateSasite02.Generate02NiwatoriNado_BottiKanmanZasi_HimodukiKanmanZasi();
                                    break;
                                case Komasyurui.I: GenerateSasite02.Generate02HiyokoNado_BottiKanmanZasi_HimodukiKanmanZasi(); break;
                                case Komasyurui.N: GenerateSasite02.Generate02HiyokoNado_BottiKanmanZasi_HimodukiKanmanZasi(); break;
                                case Komasyurui.PN:
                                    GenerateSasite02.Generate02NiwatoriNado_BottiKanmanZasi_HimodukiKanmanZasi();
                                    break;
                                case Komasyurui.U: GenerateSasite02.Generate02HiyokoNado_BottiKanmanZasi_HimodukiKanmanZasi(); break;
                                case Komasyurui.PU:
                                    GenerateSasite02.Generate02NiwatoriNado_BottiKanmanZasi_HimodukiKanmanZasi();
                                    break;
                                case Komasyurui.S: GenerateSasite02.Generate02HiyokoNado_BottiKanmanZasi_HimodukiKanmanZasi(); break;
                                case Komasyurui.PS:
                                    GenerateSasite02.Generate02NiwatoriNado_BottiKanmanZasi_HimodukiKanmanZasi();
                                    break;
                            }
                        }
                    }
                }
            }
            #endregion

            #region ぼっち緩慢指☆（Good 仲間を見捨てない動き、Bad 仲間を見捨てる動き）
            //────────────────────────────────────────
            // ぼっち緩慢指☆（Good 仲間を見捨てない動き、Bad 仲間を見捨てる動き）
            //────────────────────────────────────────
            PureMemory.SetKakuteiSsType( MoveType.N02_BottiKanmanSasi,false);
            if (flag.HasFlag(PureMemory.ssss_ugoki_kakuteiSsType))
            {
                if (!PureMemory.ssss_bbBase_idosaki12_bottiKanmanZasi.IsEmpty())
                {
                    foreach (Koma iKm_teban in Conv_Koma.itiranTuyoimonoJun[PureMemory.kifu_nTeban])// 弱い駒から順
                    {
                        PureMemory.SetSsssUgokiKm(iKm_teban);
                        yomiIbashoBan.ToSet_Koma(PureMemory.ssss_ugoki_km, PureMemory.ssssTmp_bbVar_ibasho);
                        while (PureMemory.ssssTmp_bbVar_ibasho.Ref_PopNTZ(out PureMemory.ssss_ugoki_ms_src))
                        {
                            if (MoveGenAccessor.CheckUtikiri())
                            {
#if DEBUG
                                PureMemory.ssss_sasitePickerWoNuketaBasho1 = "ぼっち緩慢指";
#endif
                                goto gt_FlushSasite;
                            }// 指し手生成終了☆

                            // 移動先リセット
                            PureMemory.ssss_bbVar_idosaki_narazu.Set(PureMemory.ssss_bbBase_idosaki12_bottiKanmanZasi);
                            PureMemory.ssss_bbVar_idosaki_narazu.Sitdown(Util_Bitboard.CreateBBTebanKikiZenbu_1KomaNozoku(PureMemory.ssss_ugoki_ms_src));// 自分以外の味方の駒の利き（紐）を付ける☆
                            BitboardsOmatome.KomanoUgokikataYk00.ToSelect_MergeShogiban(PureMemory.ssss_ugoki_km, PureMemory.ssss_ugoki_ms_src, PureMemory.ssss_bbVar_idosaki_narazu);
                            PureMemory.ssss_bbVar_idosaki_nari.Set(PureMemory.ssss_bbVar_idosaki_narazu);
                            // 進んで構わない場所に絞り込むぜ☆（＾～＾）
                            PureMemory.ssss_bbVar_idosaki_narazu.Siborikomi(BitboardsOmatome.bb_uteruZone[(int)PureMemory.ssss_ugoki_km]);
                            GenerateSasite03.SiborikomiNareruZone();

                            switch (PureMemory.ssss_ugoki_ks)
                            {
                                case Komasyurui.R: GenerateSasite02.GenerateRaion_BottiKanmanZasi_HimodukiKanmanZasi(); break;
                                case Komasyurui.Z: GenerateSasite02.Generate02ZouKirinNado_bottiKanmanZasi_himodukiKanmanZasi(); break;
                                case Komasyurui.PZ: GenerateSasite02.Generate02ZouKirinNado_bottiKanmanZasi_himodukiKanmanZasi(); break;
                                case Komasyurui.K: GenerateSasite02.Generate02ZouKirinNado_bottiKanmanZasi_himodukiKanmanZasi(); break;
                                case Komasyurui.PK: GenerateSasite02.Generate02ZouKirinNado_bottiKanmanZasi_himodukiKanmanZasi(); break;
                                case Komasyurui.H: GenerateSasite02.Generate02HiyokoNado_BottiKanmanZasi_HimodukiKanmanZasi(); break;
                                case Komasyurui.PH: GenerateSasite02.Generate02NiwatoriNado_BottiKanmanZasi_HimodukiKanmanZasi(); break;
                                case Komasyurui.I: GenerateSasite02.Generate02HiyokoNado_BottiKanmanZasi_HimodukiKanmanZasi(); break;
                                case Komasyurui.N: GenerateSasite02.Generate02HiyokoNado_BottiKanmanZasi_HimodukiKanmanZasi(); break;
                                case Komasyurui.PN: GenerateSasite02.Generate02NiwatoriNado_BottiKanmanZasi_HimodukiKanmanZasi(); break;
                                case Komasyurui.U: GenerateSasite02.Generate02HiyokoNado_BottiKanmanZasi_HimodukiKanmanZasi(); break;
                                case Komasyurui.PU: GenerateSasite02.Generate02NiwatoriNado_BottiKanmanZasi_HimodukiKanmanZasi(); break;
                                case Komasyurui.S: GenerateSasite02.Generate02HiyokoNado_BottiKanmanZasi_HimodukiKanmanZasi(); break;
                                case Komasyurui.PS: GenerateSasite02.Generate02NiwatoriNado_BottiKanmanZasi_HimodukiKanmanZasi(); break;
                            }
                        }
                    }
                }
            }
            #endregion

            if (hasMotiKoma)
            {
                #region ぼっち緩慢打☆（Good 仲間を見捨てない動き、Bad 仲間を見捨てる動き）
                //────────────────────────────────────────
                // ぼっち緩慢打☆（Good 仲間を見捨てない動き、Bad 仲間を見捨てる動き）
                //────────────────────────────────────────
                PureMemory.SetKakuteiSsType( MoveType.N03_BottiKanmanDa,true);
                if (flag.HasFlag(PureMemory.ssss_ugoki_kakuteiSsType))
                {
                    if (!PureMemory.ssss_bbBase_idosaki13_bottiKanmanDa.IsEmpty())
                    {
                        // 順を付けて、指し手を調べようぜ☆（＾▽＾）
                        foreach (MotigomaSyurui iMks in Conv_MotigomaSyurui.itiranTuyoimonoJun)
                        {
                            if (PureMemory.SetSsssMotMks_AndHasMotigoma(iMks))
                            {
                                PureMemory.ssss_bbVar_idosaki_narazu.Set(PureMemory.ssss_bbBase_idosaki13_bottiKanmanDa);
                                PureMemory.ssss_bbVar_idosaki_nari.Clear();// 「打」に「成り」は無いぜ☆（＾～＾）

                                // 二歩防止
                                if (MotigomaSyurui.H == PureMemory.ssss_mot_mks) { GenerateSasite03.SiborikomiByNifu(); }
                                // 打って構わない場所に絞り込むぜ☆（＾～＾）
                                PureMemory.ssss_bbVar_idosaki_narazu.Siborikomi(BitboardsOmatome.bb_uteruZone[(int)PureMemory.ssss_mot_km]);

                                GenerateSasite02.GenerateMotigoma_NormalDa();
                            }
                        }
#if DEBUG
                        PureMemory.ssss_sasitePickerWoNuketaBasho1 = "ぼっち緩慢打";
#endif
                    }
                }
                #endregion
            }


            #region GoodBadマージ　ぼっち緩慢指☆（Good 仲間を見捨てない動き、Bad 仲間を見捨てる動き）
            if (flag.HasFlag(MoveType.N18_Option_MergeGoodBad))
            {
                // マージをするぜ☆（＾▽＾）
                MoveGenAccessor.MergeSasitelistGoodBad(
#if DEBUG
                    "GoodBadマージ　緩慢な手☆（Good 仲間を見捨てない動き、Bad 仲間を見捨てる動き）"
#endif
                    );
            }
            #endregion

            #region 捨て緩慢指（タダ捨て指し）
            //────────────────────────────────────────
            // 捨て緩慢指☆（タダ捨て指し）
            //────────────────────────────────────────
            PureMemory.SetKakuteiSsType( MoveType.N04_SuteKanmanSasi,false);
            if (flag.HasFlag(PureMemory.ssss_ugoki_kakuteiSsType))
            {
                if (!PureMemory.ssss_bbBase_idosaki14_suteKanmanZasi.IsEmpty())
                {
                    // 2016-12-22 捨てだからと言って、紐を付けないとは限らない☆

                    foreach (Koma iKm_teban in Conv_Koma.itiranTuyoimonoJun[PureMemory.kifu_nTeban])// 弱い駒から順
                    {
                        PureMemory.SetSsssUgokiKm(iKm_teban);
                        yomiIbashoBan.ToSet_Koma(PureMemory.ssss_ugoki_km, PureMemory.ssssTmp_bbVar_ibasho);
                        while (PureMemory.ssssTmp_bbVar_ibasho.Ref_PopNTZ(out PureMemory.ssss_ugoki_ms_src))
                        {
                            if (MoveGenAccessor.CheckUtikiri())
                            {
#if DEBUG
                                PureMemory.ssss_sasitePickerWoNuketaBasho1 = "捨て緩慢指";
#endif
                                goto gt_FlushSasite;
                            }// 指し手生成終了☆

                            // 移動先リセット
                            PureMemory.ssss_bbVar_idosaki_narazu.Set(PureMemory.ssss_bbBase_idosaki14_suteKanmanZasi);
                            BitboardsOmatome.KomanoUgokikataYk00.ToSelect_MergeShogiban(PureMemory.ssss_ugoki_km, PureMemory.ssss_ugoki_ms_src, PureMemory.ssss_bbVar_idosaki_narazu);
                            PureMemory.ssss_bbVar_idosaki_nari.Set(PureMemory.ssss_bbVar_idosaki_narazu);
                            // 進んで構わない場所に絞り込むぜ☆（＾～＾）
                            PureMemory.ssss_bbVar_idosaki_narazu.Siborikomi(BitboardsOmatome.bb_uteruZone[(int)PureMemory.ssss_ugoki_km]);
                            GenerateSasite03.SiborikomiNareruZone();

                            switch (PureMemory.ssss_ugoki_ks)
                            {
                                case Komasyurui.R:// らいおんは　捨て緩慢指し　をやらないぜ☆（*＾～＾*）
                                    break;

                                case Komasyurui.Z: // thuru
                                case Komasyurui.K:
                                case Komasyurui.H:
                                case Komasyurui.N:
                                case Komasyurui.U:
                                case Komasyurui.S:
                                    GenerateSasite02.GenerateNk_SuteKanmanZasi();
                                    break;

                                case Komasyurui.PZ: // thru
                                case Komasyurui.PK:
                                case Komasyurui.PH:
                                case Komasyurui.I:
                                case Komasyurui.PN:
                                case Komasyurui.PU:
                                case Komasyurui.PS:
                                    GenerateSasite02.GenerateXk_SuteKanmanZasi();
                                    break;
                            }

                        }
                    }
                }
            }
            #endregion

            if (hasMotiKoma)
            {
                #region 捨て緩慢打（タダ捨て打）
                //────────────────────────────────────────
                // 捨て緩慢打（タダ捨て打）☆
                //────────────────────────────────────────
                PureMemory.SetKakuteiSsType( MoveType.N05_SuteKanmanDa,true);
                if (flag.HasFlag(PureMemory.ssss_ugoki_kakuteiSsType))
                {
                    if (!PureMemory.ssss_bbBase_idosaki15_suteKanmanDa.IsEmpty())
                    {
                        // 順を付けて、指し手を調べようぜ☆（＾▽＾）
                        foreach (MotigomaSyurui iMks in Conv_MotigomaSyurui.itiranTuyoimonoJun)
                        {
                            if (PureMemory.SetSsssMotMks_AndHasMotigoma(iMks))
                            {
                                // 移動先リセット
                                PureMemory.ssss_bbVar_idosaki_narazu.Set(PureMemory.ssss_bbBase_idosaki15_suteKanmanDa);
                                PureMemory.ssss_bbVar_idosaki_nari.Clear();// 「打」に「成り」は無いぜ☆（＾～＾）

                                // 二歩防止
                                if (MotigomaSyurui.H == PureMemory.ssss_mot_mks) { GenerateSasite03.SiborikomiByNifu(); }
                                // 打って構わない場所に絞り込むぜ☆（＾～＾）
                                PureMemory.ssss_bbVar_idosaki_narazu.Siborikomi(BitboardsOmatome.bb_uteruZone[(int)PureMemory.ssss_mot_km]);
                                GenerateSasite02.GenerateMotigoma_SuteDa();
                            }
                        }

#if DEBUG
                        PureMemory.ssss_sasitePickerWoNuketaBasho1 = "捨て緩慢打";
#endif
                    }
                }
                #endregion
            }

        gt_FlushSasite:
            ;


            if (sasitelistMerge)
            {
                // マージを忘れるなだぜ☆（＾▽＾）
                MoveGenAccessor.MergeSasitelistGoodBad(
#if DEBUG
                    "マージを忘れるなだぜ☆（＾▽＾）"
#endif
                );

            }
            return;
        }
    }
}
