#if DEBUG
using kifuwarabe_shogithink.pure;
using kifuwarabe_shogithink.pure.accessor;
using kifuwarabe_shogithink.pure.com;
using kifuwarabe_shogithink.pure.com.moveorder;
using kifuwarabe_shogithink.pure.com.moveorder.hioute;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.listen;
using kifuwarabe_shogithink.pure.logger;
using kifuwarabe_shogithink.pure.move;
using kifuwarabe_shogithink.pure.speak.ky.bb;
using kifuwarabe_shogithink.pure.speak.play;
using kifuwarabe_shogiwin.consolegame.machine;
using kifuwarabe_shogiwin.speak;
using kifuwarabe_shogiwin.speak.ban;
using System;
using System.Text;
#else
using kifuwarabe_shogithink.pure;
using kifuwarabe_shogithink.pure.accessor;
using kifuwarabe_shogithink.pure.com;
using kifuwarabe_shogithink.pure.com.MoveOrder;
using kifuwarabe_shogithink.pure.com.MoveOrder.hioute;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.listen;
using kifuwarabe_shogithink.pure.logger;
using kifuwarabe_shogithink.pure.move;
using kifuwarabe_shogithink.pure.speak.play;
using kifuwarabe_shogiwin.consolegame.machine;
using kifuwarabe_shogiwin.speak;
using kifuwarabe_shogiwin.speak.ban;
using System;
using System.Text;
#endif

namespace kifuwarabe_shogiwin.consolegame.console.command
{
    public static class CommandS
    {
        public static bool TryFail_Move_cmd(string line, IHyojiMojiretu hyoji)
        {
            int caret = 0;
            if (Util_String.MatchAndNext( "move", line, ref caret))
            {
                #region 指し手一覧「move」
                if (line.Length <= caret)
                {
                    if (UtilMove.TryFailMoveCmd1(hyoji))
                    {
                        return Pure.FailTrue("TryFail_Move_cmd1");
                    }
                    SpkMoveList.Setumei(PureSettei.fenSyurui, "指し手全部", PureMemory.ssss_moveList[PureMemory.FUKASA_MANUAL], hyoji);
                    hyoji.AppendLine();
                    return Pure.SUCCESSFUL_FALSE;
                }
                #endregion
                else
                {
                    #region 指し手の件数出力「move su」
                    if (Util_String.MatchAndNext("su", line, ref caret))
                    {
                        if (UtilMove.TryFailMoveCmd1(hyoji))
                        {
                            return Pure.FailTrue("TryFail_Move_cmd1");
                        }
                        hyoji.AppendLine("指し手 件数=[" + PureMemory.ssss_moveList[PureMemory.FUKASA_MANUAL].listCount + "]");
                        return Pure.SUCCESSFUL_FALSE;
                    }
                    #endregion
                    #region 指し手生成チェック「move seisei」
                    else if (Util_String.MatchAndNext("seisei", line, ref caret))
                    {
                        //MovePicker01.DoMovePickerBegin();
                        const bool NO_MERGE = false;

                        #region はじめに
                        if (Util_String.MatchAndNext("begin", line, ref caret))
                        {
                            Util_Hioute.Tukurinaosi();

                            // 先手らいおん
                            {
                                hyoji.AppendLine(string.Format("▲らいおんの場所: {0}", PureMemory.hot_ms_raionAr[(int)Taikyokusya.T1]));
#if DEBUG
                        SpkBan_Hisigata.Setumei_yk00("▲らいおん８近傍↓", PureMemory.hot_bb_raion8KinboAr[(int)Taikyokusya.T1], hyoji);
                        SpkBan_Hisigata.Setumei_yk00("▲らいおん通り道↓", PureMemory.hot_bb_nigereruAr[(int)Taikyokusya.T1], hyoji);
                        SpkBan_Hisigata.Setumei_yk00("▲への刺客↓", PureMemory.hot_bb_checkerAr[(int)Taikyokusya.T1], hyoji);
#endif
                                hyoji.AppendLine(string.Format("▲への王手駒数: {0}", PureMemory.hot_outeKomasCountAr[(int)Taikyokusya.T1]));
#if DEBUG
                        SpkBan_Hisigata.Setumei_yk00("▲を阻止する駒↓", PureMemory.hot_bb_nigemitiWoFusaideiruAiteNoKomaAr[(int)Taikyokusya.T1], hyoji);
                        SpkBan_Hisigata.Setumei_yk00("▲は逃げろ↓", PureMemory.hot_bb_nigeroAr[(int)Taikyokusya.T1], hyoji);
#endif
                            }
                            // 後手らいおん
                            {
                                hyoji.AppendLine(string.Format("△らいおんの場所: {0}", PureMemory.hot_ms_raionAr[(int)Taikyokusya.T2]));
#if DEBUG
                        SpkBan_Hisigata.Setumei_yk00("▽らいおん８近傍↓", PureMemory.hot_bb_raion8KinboAr[(int)Taikyokusya.T2], hyoji);
                        SpkBan_Hisigata.Setumei_yk00("▽らいおん通り道↓", PureMemory.hot_bb_nigereruAr[(int)Taikyokusya.T2], hyoji);
                        SpkBan_Hisigata.Setumei_yk00("▽への刺客↓", PureMemory.hot_bb_checkerAr[(int)Taikyokusya.T2], hyoji);
#endif
                                hyoji.AppendLine(string.Format("▽への王手駒数: {0}", PureMemory.hot_outeKomasCountAr[(int)Taikyokusya.T2]));
#if DEBUG
                        SpkBan_Hisigata.Setumei_yk00("▽を阻止する駒↓", PureMemory.hot_bb_nigemitiWoFusaideiruAiteNoKomaAr[(int)Taikyokusya.T2], hyoji);
                        SpkBan_Hisigata.Setumei_yk00("▽は逃げろ↓", PureMemory.hot_bb_nigeroAr[(int)Taikyokusya.T2], hyoji);
#endif
                            }
                            hyoji.AppendLine();
                        }
                        #endregion
                        #region 逼迫返討手(hkt)
                        else if (Util_String.MatchAndNext("hkt", line, ref caret))
                        {
                            PureMemory.SetTnskFukasa(PureMemory.FUKASA_MANUAL);
                            MoveGenAccessor.DoMovePickerBegin(MoveType.N13_HippakuKaeriutiTe);
                            MovePicker01.MovePickerN01(MoveType.N13_HippakuKaeriutiTe, NO_MERGE);
                            SpkMoveList.Setumei(PureSettei.fenSyurui, "逼迫返討手", PureMemory.ssss_moveList[PureMemory.FUKASA_MANUAL], hyoji);
                            hyoji.AppendLine();
                        }
                        #endregion
                        else
                        {
                            // 詰んでいる状況かどうか調べるぜ☆（＾▽＾）
                            // 相手番側が、王手回避が必要かどうか調べたいぜ☆（＾～＾）
                            {
                                Util_Hioute.Tukurinaosi_1(PureMemory.kifu_aiteban, PureMemory.kifu_teban, true);//手番をひっくり返して判定

                                hyoji.AppendLine("・相手　らいおん　の逃げ道を調べようぜ☆（＾▽＾）");
#if DEBUG
                                SpkBan_1Column.Setumei_Bitboard("NigeroBB", PureMemory.hot_bb_nigeroAr[PureMemory.kifu_nAiteban], hyoji);
                                SpkBan_1Column.Setumei_Bitboard("NigereruBB", PureMemory.hot_bb_nigereruAr[PureMemory.kifu_nAiteban], hyoji);
#endif
                                SpkBan_1Column.Setumei_Bitboard("逃げ道を塞いでいる駒",
                                    PureMemory.hot_bb_nigemitiWoFusaideiruAiteNoKomaAr[PureMemory.kifu_nAiteban],
                                    hyoji);
                            }


                            //逼迫返討手(hkt)
                            CommandS.TryFail_Move_cmd("move seisei hkt", hyoji);
                            #region 余裕返討手
                            {
                                PureMemory.SetTnskFukasa(PureMemory.FUKASA_MANUAL);
                                MoveGenAccessor.DoMovePickerBegin(MoveType.N14_YoyuKaeriutiTe);
                                MovePicker01.MovePickerN01(MoveType.N14_YoyuKaeriutiTe, NO_MERGE);
                                SpkMoveList.Setumei(PureSettei.fenSyurui, "余裕返討手", PureMemory.ssss_moveList[PureMemory.FUKASA_MANUAL], hyoji);
                                hyoji.AppendLine();
                            }
                            #endregion
                            #region らいおんキャッチ
                            {
                                PureMemory.SetTnskFukasa(PureMemory.FUKASA_MANUAL);
                                MoveGenAccessor.DoMovePickerBegin(MoveType.N12_RaionCatch);
                                MovePicker01.MovePickerN01(MoveType.N12_RaionCatch, NO_MERGE);
                                SpkMoveList.Setumei(PureSettei.fenSyurui, "らいおんキャッチ", PureMemory.ssss_moveList[PureMemory.FUKASA_MANUAL], hyoji);
#if DEBUG
                        SpkBan_Hisigata.Setumei_yk00("らいおんキャッチ", PureMemory.ssss_bbBase_idosaki02_raionCatch, hyoji);
#endif
                                hyoji.AppendLine();
                            }
                            #endregion
                            #region 逃げろ手
                            {
                                PureMemory.SetTnskFukasa(PureMemory.FUKASA_MANUAL);
                                MoveGenAccessor.DoMovePickerBegin(MoveType.N15_NigeroTe);
                                MovePicker01.MovePickerN01(MoveType.N15_NigeroTe, NO_MERGE);
                                SpkMoveList.Setumei(PureSettei.fenSyurui, "逃げろ手", PureMemory.ssss_moveList[PureMemory.FUKASA_MANUAL], hyoji);
                                hyoji.AppendLine();
                            }
                            #endregion
                            #region トライ
                            {
                                PureMemory.SetTnskFukasa(PureMemory.FUKASA_MANUAL);
                                MoveGenAccessor.DoMovePickerBegin(MoveType.N16_Try);
                                MovePicker01.MovePickerN01(MoveType.N16_Try, NO_MERGE);
                                SpkMoveList.Setumei(PureSettei.fenSyurui, "トライ", PureMemory.ssss_moveList[PureMemory.FUKASA_MANUAL], hyoji);
                                hyoji.AppendLine();
                            }
                            #endregion

                            #region 駒を取る手（逃げ道を開けない手）
                            {
                                PureMemory.SetTnskFukasa(PureMemory.FUKASA_MANUAL);
                                MoveGenAccessor.DoMovePickerBegin(MoveType.N01_KomaWoToruTe);
                                MovePicker01.MovePickerN01(MoveType.N01_KomaWoToruTe, NO_MERGE);
                                SpkMoveList.Setumei(PureSettei.fenSyurui, "駒を取る手（逃げ道を開けない手）", PureMemory.ssss_moveList[PureMemory.FUKASA_MANUAL], hyoji);
                                hyoji.AppendLine();
                            }
                            #endregion
                            #region 駒を取る手（逃げ道を開ける手）
                            {
                                PureMemory.SetTnskFukasa(PureMemory.FUKASA_MANUAL);
                                MoveGenAccessor.DoMovePickerBegin(MoveType.N01_KomaWoToruTe);
                                MovePicker01.MovePickerN01(MoveType.N01_KomaWoToruTe, NO_MERGE);
                                SpkMoveList.Setumei(PureSettei.fenSyurui, "駒を取る手（逃げ道を開ける手）", PureMemory.ssss_moveListBad[PureMemory.FUKASA_MANUAL], hyoji);
                                hyoji.AppendLine();
                            }
                            #endregion

                            // ぼっち　と　王手　は組み合わないぜ☆(＾◇＾)　捨て王手、または　紐付王手　になるからな☆（＾▽＾）

                            #region 紐付王手指（逃げ道を開けない手）
                            {
                                PureMemory.SetTnskFukasa(PureMemory.FUKASA_MANUAL);
                                MoveGenAccessor.DoMovePickerBegin(MoveType.N10_HimozukiOteZasi);
                                MovePicker01.MovePickerN01(MoveType.N10_HimozukiOteZasi, NO_MERGE);
                                SpkMoveList.Setumei(PureSettei.fenSyurui, "紐付王手指（逃げ道を開けない手）", PureMemory.ssss_moveList[PureMemory.FUKASA_MANUAL], hyoji);
                                hyoji.AppendLine();
                            }
                            #endregion
                            #region 捨て王手指（逃げ道を開けない手）
                            {
                                PureMemory.SetTnskFukasa(PureMemory.FUKASA_MANUAL);
                                MoveGenAccessor.DoMovePickerBegin(MoveType.N06_SuteOteZasi);
                                MovePicker01.MovePickerN01(MoveType.N06_SuteOteZasi, NO_MERGE);
                                SpkMoveList.Setumei(PureSettei.fenSyurui, "捨て王手指（逃げ道を開けない手）", PureMemory.ssss_moveList[PureMemory.FUKASA_MANUAL], hyoji);
                                hyoji.AppendLine();
                            }
                            #endregion
                            #region 捨て王手打（逃げ道を開けない手）
                            {
                                PureMemory.SetTnskFukasa(PureMemory.FUKASA_MANUAL);
                                MoveGenAccessor.DoMovePickerBegin(MoveType.N07_SuteOteDa);
                                MovePicker01.MovePickerN01(MoveType.N07_SuteOteDa, NO_MERGE);
                                SpkMoveList.Setumei(PureSettei.fenSyurui, "捨て王手打（逃げ道を開けない手）", PureMemory.ssss_moveList[PureMemory.FUKASA_MANUAL], hyoji);
                                hyoji.AppendLine();
                            }
                            #endregion
                            #region 紐付王手打（逃げ道を開けない手）
                            {
                                PureMemory.SetTnskFukasa(PureMemory.FUKASA_MANUAL);
                                MoveGenAccessor.DoMovePickerBegin(MoveType.N11_HimodukiOteDa);
                                MovePicker01.MovePickerN01(MoveType.N11_HimodukiOteDa, NO_MERGE);
                                SpkMoveList.Setumei(PureSettei.fenSyurui, "紐付王手打（逃げ道を開けない手）", PureMemory.ssss_moveList[PureMemory.FUKASA_MANUAL], hyoji);
                                hyoji.AppendLine();
                            }
                            #endregion

                            #region 紐付王手指（逃げ道を開ける手）
                            {
                                PureMemory.SetTnskFukasa(PureMemory.FUKASA_MANUAL);
                                MoveGenAccessor.DoMovePickerBegin(MoveType.N10_HimozukiOteZasi);
                                MovePicker01.MovePickerN01(MoveType.N10_HimozukiOteZasi, NO_MERGE);
                                SpkMoveList.Setumei(PureSettei.fenSyurui, "紐付王手指（逃げ道を開ける手）", PureMemory.ssss_moveListBad[PureMemory.FUKASA_MANUAL], hyoji);
                                hyoji.AppendLine();
                            }
                            #endregion
                            #region 捨て王手指（逃げ道を開ける手）
                            {
                                PureMemory.SetTnskFukasa(PureMemory.FUKASA_MANUAL);
                                MoveGenAccessor.DoMovePickerBegin(MoveType.N06_SuteOteZasi);
                                MovePicker01.MovePickerN01(MoveType.N06_SuteOteZasi, NO_MERGE);
                                SpkMoveList.Setumei(PureSettei.fenSyurui, "捨て王手指（逃げ道を開ける手）", PureMemory.ssss_moveListBad[PureMemory.FUKASA_MANUAL], hyoji);
                                hyoji.AppendLine();
                            }
                            #endregion
                            #region 捨て王手打（逃げ道を開ける手）
                            {
                                PureMemory.SetTnskFukasa(PureMemory.FUKASA_MANUAL);
                                MoveGenAccessor.DoMovePickerBegin(MoveType.N07_SuteOteDa);
                                MovePicker01.MovePickerN01(MoveType.N07_SuteOteDa, NO_MERGE);
                                SpkMoveList.Setumei(PureSettei.fenSyurui, "捨て王手打（逃げ道を開ける手）", PureMemory.ssss_moveListBad[PureMemory.FUKASA_MANUAL], hyoji);
                                hyoji.AppendLine();
                            }
                            #endregion
                            #region 紐付王手打（逃げ道を開ける手）
                            {
                                PureMemory.SetTnskFukasa(PureMemory.FUKASA_MANUAL);
                                MoveGenAccessor.DoMovePickerBegin(MoveType.N11_HimodukiOteDa);
                                MovePicker01.MovePickerN01(MoveType.N11_HimodukiOteDa, NO_MERGE);
                                SpkMoveList.Setumei(PureSettei.fenSyurui, "紐付王手打（逃げ道を開ける手）", PureMemory.ssss_moveListBad[PureMemory.FUKASA_MANUAL], hyoji);
                                hyoji.AppendLine();
                            }
                            #endregion

                            #region 紐付緩慢打
                            {
                                PureMemory.SetTnskFukasa(PureMemory.FUKASA_MANUAL);
                                MoveGenAccessor.DoMovePickerBegin(MoveType.N09_HimotukiKanmanDa);
                                MovePicker01.MovePickerN01(MoveType.N09_HimotukiKanmanDa, NO_MERGE);
                                SpkMoveList.Setumei(PureSettei.fenSyurui, "紐付緩慢打", PureMemory.ssss_moveList[PureMemory.FUKASA_MANUAL], hyoji);
                                hyoji.AppendLine();
                            }
                            #endregion

                            #region 紐付緩慢指（仲間を見捨てない動き）
                            {
                                PureMemory.SetTnskFukasa(PureMemory.FUKASA_MANUAL);
                                MoveGenAccessor.DoMovePickerBegin(MoveType.N08_HimotukiKanmanSasi);
                                MovePicker01.MovePickerN01(MoveType.N08_HimotukiKanmanSasi, NO_MERGE);
                                SpkMoveList.Setumei(PureSettei.fenSyurui, "紐付緩慢指（仲間を見捨てない動き）", PureMemory.ssss_moveList[PureMemory.FUKASA_MANUAL], hyoji);
                                hyoji.AppendLine();
                            }
                            #endregion
                            #region ぼっち緩慢指（仲間を見捨てない動き）
                            {
                                PureMemory.SetTnskFukasa(PureMemory.FUKASA_MANUAL);
                                MoveGenAccessor.DoMovePickerBegin(MoveType.N02_BottiKanmanSasi);
                                MovePicker01.MovePickerN01(MoveType.N02_BottiKanmanSasi, NO_MERGE);
                                SpkMoveList.Setumei(PureSettei.fenSyurui, "ぼっち緩慢指（仲間を見捨てない動き）", PureMemory.ssss_moveList[PureMemory.FUKASA_MANUAL], hyoji);
                                hyoji.AppendLine();
                            }
                            #endregion
                            #region ぼっち緩慢打（仲間を見捨てない動き）
                            {
                                PureMemory.SetTnskFukasa(PureMemory.FUKASA_MANUAL);
                                MoveGenAccessor.DoMovePickerBegin(MoveType.N03_BottiKanmanDa);
                                MovePicker01.MovePickerN01(MoveType.N03_BottiKanmanDa, NO_MERGE);
                                SpkMoveList.Setumei(PureSettei.fenSyurui, "ぼっち緩慢打（仲間を見捨てない動き）", PureMemory.ssss_moveList[PureMemory.FUKASA_MANUAL], hyoji);
                                hyoji.AppendLine();
                            }
                            #endregion

                            #region 紐付緩慢指（仲間を見捨てる動き）
                            {
                                PureMemory.SetTnskFukasa(PureMemory.FUKASA_MANUAL);
                                MoveGenAccessor.DoMovePickerBegin(MoveType.N08_HimotukiKanmanSasi);
                                MovePicker01.MovePickerN01(MoveType.N08_HimotukiKanmanSasi, NO_MERGE);
                                SpkMoveList.Setumei(PureSettei.fenSyurui, "紐付緩慢指（仲間を見捨てる動き）", PureMemory.ssss_moveListBad[PureMemory.FUKASA_MANUAL], hyoji);
                                hyoji.AppendLine();
                            }
                            #endregion
                            #region ぼっち緩慢指（仲間を見捨てる動き）
                            {
                                PureMemory.SetTnskFukasa(PureMemory.FUKASA_MANUAL);
                                MoveGenAccessor.DoMovePickerBegin(MoveType.N02_BottiKanmanSasi);
                                MovePicker01.MovePickerN01(MoveType.N02_BottiKanmanSasi, NO_MERGE);
                                SpkMoveList.Setumei(PureSettei.fenSyurui, "ぼっち緩慢指（仲間を見捨てる動き）", PureMemory.ssss_moveListBad[PureMemory.FUKASA_MANUAL], hyoji);
                                hyoji.AppendLine();
                            }
                            #endregion
                            #region ぼっち緩慢打（仲間を見捨てる動き）
                            {
                                PureMemory.SetTnskFukasa(PureMemory.FUKASA_MANUAL);
                                MoveGenAccessor.DoMovePickerBegin(MoveType.N03_BottiKanmanDa);
                                MovePicker01.MovePickerN01(MoveType.N03_BottiKanmanDa, NO_MERGE);
                                SpkMoveList.Setumei(PureSettei.fenSyurui, "ぼっち緩慢打（仲間を見捨てる動き）", PureMemory.ssss_moveListBad[PureMemory.FUKASA_MANUAL], hyoji);
                                hyoji.AppendLine();
                            }
                            #endregion

                            #region 捨て緩慢指し（タダ捨て指し）
                            {
                                PureMemory.SetTnskFukasa(PureMemory.FUKASA_MANUAL);
                                MoveGenAccessor.DoMovePickerBegin(MoveType.N04_SuteKanmanSasi);
                                MovePicker01.MovePickerN01(MoveType.N04_SuteKanmanSasi, NO_MERGE);
                                SpkMoveList.Setumei(PureSettei.fenSyurui, "捨て緩慢指し（タダ捨て指し）", PureMemory.ssss_moveList[PureMemory.FUKASA_MANUAL], hyoji);
                                hyoji.AppendLine();
                            }
                            #endregion
                            #region 捨て緩慢打（タダ捨て打）
                            {
                                PureMemory.SetTnskFukasa(PureMemory.FUKASA_MANUAL);
                                MoveGenAccessor.DoMovePickerBegin(MoveType.N05_SuteKanmanDa);
                                MovePicker01.MovePickerN01(MoveType.N05_SuteKanmanDa, NO_MERGE);
                                SpkMoveList.Setumei(PureSettei.fenSyurui, "捨て緩慢打（タダ捨て打）", PureMemory.ssss_moveList[PureMemory.FUKASA_MANUAL], hyoji);
                                hyoji.AppendLine();
                            }
                            #endregion

                            if (!NO_MERGE)
                            {
                                // マージを忘れるなだぜ☆（＾▽＾）
                                PureMemory.SetTnskFukasa(PureMemory.FUKASA_MANUAL);
                                MoveGenAccessor.MergeMoveListGoodBad(
#if DEBUG
                                    "マージを忘れるなだぜ☆（＾▽＾）"
#endif
                    );
                            }
                        }
                    }
                    #endregion
                    else
                    {
                        if (!UtilMove.TryMoveCmd2(out Move ss, line))// move 912 とか☆
                        {
                            // パース・エラー時
                            hyoji.AppendLine("指し手文字列　解析失敗☆");
                        }
                        SpkMove.AppendSetumei(PureSettei.fenSyurui, ss, hyoji);
                        hyoji.Append(" (");
                        hyoji.Append((int)ss);
                        hyoji.Append(")");
                        hyoji.AppendLine();
                    }

                }
            }
            else
            {

            }


            return Pure.SUCCESSFUL_FALSE;
        }

        public static void Setoption(string line, IHyojiMojiretu hyoji)
        {
            // // とりあえず無視☆（*＾～＾*）

            // 「setoption name 名前 value 値」といった書式なので、
            // 「set 名前 値」に変えたい。

            // うしろに続く文字は☆（＾▽＾）
            int caret = 0;
            Util_String.TobasuTangoToMatubiKuhaku(line, ref caret, "setoption ");
            Util_String.TobasuTangoToMatubiKuhaku(line, ref caret, "name ");
            int end = line.IndexOf("value ", caret);
            if (-1 != end)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("set ");
                sb.Append(line.Substring(caret, end - caret));//名前
                caret = end + "value ".Length;
                sb.Append(line.Substring(caret));//値

                Set(sb.ToString(), hyoji);
            }
        }

        public static void Set(string line, IHyojiMojiretu hyoji)
        {
            if (line == "set")
            {
                hyoji.AppendLine("BanTateHaba              = " + PureSettei.banTateHaba);
                hyoji.AppendLine("BanYokoHaba              = " + PureSettei.banYokoHaba);
                hyoji.AppendLine("FEN                      = " + PureSettei.fenSyurui);
                hyoji.AppendLine("GameRule                 = " + PureSettei.gameRule);
                hyoji.AppendLine("HimodukiHyokaTukau       = " + ComSettei.himodukiHyokaTukau);
                hyoji.AppendLine("IttedumeTukau            = " + PureSettei.ittedumeTukau);
                hyoji.AppendLine("JohoJikan                = " + ComSettei.johoJikan);
                hyoji.AppendLine("P1Char                   = " + PureSettei.char_playerN[(int)Taikyokusya.T1]);
                hyoji.AppendLine("P1Com                    = " + PureSettei.p1Com);
                hyoji.AppendLine("P1Name                   = " + PureSettei.name_playerN[(int)Taikyokusya.T1]);
                hyoji.AppendLine("P2Char                   = " + PureSettei.char_playerN[(int)Taikyokusya.T2]);
                hyoji.AppendLine("P2Com                    = " + PureSettei.p2Com);
                hyoji.AppendLine("P2Name                   = " + PureSettei.name_playerN[(int)Taikyokusya.T2]);
                hyoji.AppendLine("RenzokuTaikyoku          = " + ConsolegameSettei.renzokuTaikyoku);
                hyoji.AppendLine("SaidaiFukasa             = " + ComSettei.saidaiFukasa);
                hyoji.AppendLine("SikoJikan                = " + ComSettei.sikoJikan);
                hyoji.AppendLine("SikoJikanRandom          = " + ComSettei.sikoJikanRandom);
                hyoji.AppendLine("TobikikiTukau            = " + PureSettei.tobikikiTukau);
                hyoji.AppendLine("UseTimeOver              = " + ComSettei.useTimeOver);
                hyoji.AppendLine("USI                      = " + PureSettei.usi);
                return;
            }

            ConvAppli.Set(line, hyoji);
        }

    }
}
