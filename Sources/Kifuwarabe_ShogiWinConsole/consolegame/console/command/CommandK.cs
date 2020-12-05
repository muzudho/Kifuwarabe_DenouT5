#if DEBUG
using kifuwarabe_shogithink.pure;
using kifuwarabe_shogithink.pure.accessor;
using kifuwarabe_shogithink.pure.control;
using kifuwarabe_shogithink.pure.genkyoku;
using kifuwarabe_shogithink.pure.ikkyoku;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.ky.bb;
using kifuwarabe_shogithink.pure.listen;
using kifuwarabe_shogithink.pure.listen.genkyoku;
using kifuwarabe_shogithink.pure.listen.ky;
using kifuwarabe_shogithink.pure.logger;
using kifuwarabe_shogithink.pure.speak.genkyoku;
using kifuwarabe_shogiwin.consolegame.machine;
using kifuwarabe_shogiwin.project.speak;
using kifuwarabe_shogiwin.speak;
using kifuwarabe_shogiwin.speak.ban;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using kifuwarabe_shogithink.fen;
#else
using kifuwarabe_shogithink.pure;
using kifuwarabe_shogithink.pure.accessor;
using kifuwarabe_shogithink.pure.control;
using kifuwarabe_shogithink.pure.genkyoku;
using kifuwarabe_shogithink.pure.ikkyoku;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.ky.bb;
using kifuwarabe_shogithink.pure.listen;
using kifuwarabe_shogithink.pure.listen.genkyoku;
using kifuwarabe_shogithink.pure.listen.ky;
using kifuwarabe_shogithink.pure.logger;
using kifuwarabe_shogithink.pure.speak.genkyoku;
using kifuwarabe_shogiwin.consolegame.machine;
using kifuwarabe_shogiwin.speak;
using kifuwarabe_shogiwin.speak.ban;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using kifuwarabe_shogithink.fen;
#endif

namespace kifuwarabe_shogiwin.consolegame.console.command
{
    public static class CommandK
    {

        /// <summary>
        /// 初期局面まで undo したいが☆（＾～＾）
        /// </summary>
        /// <param name="f"></param>
        /// <param name="line"></param>
        /// <param name="hyoji"></param>
        public static void Kansosen(FenSyurui f, string line, IHyojiMojiretu hyoji)
        {
            int caret = 0;
            if (Util_String.MatchAndNext( "kansosen", line, ref caret))
            {
                if ("" == PureMemory.kifu_syokiKyokumenFen)
                {
                    hyoji.AppendLine("棋譜がないぜ☆（＞＿＜）");
                    return;
                }
                PureAppli.gameMode = GameMode.Kansosen;

//                // 初期局面に戻すぜ☆（＾～＾）
//                string moves_notUse;
//                if (LisGenkyoku.TryFail_MatchPositionvalue(f,
//                    PureMemory.kifu_syokiKyokumenFen, ref caret,
//                    true//適用
//                    , false, out moves_notUse
//#if DEBUG
//                , (IDebugMojiretu)hyoji
//#endif
//                ))
//                {
//                    string msg = hyoji.ToContents();
//                    Util_Machine.Flush(hyoji);
//                    throw new Exception(msg);
//                }

                //// 終局図まで進めるぜ☆（＾～＾）
                //if (!SasiteSeiseiAccessor.Try_PlayToFinish(f, hyoji))
                //{
                //    string msg = hyoji.ToContents();
                //    Util_Machine.Flush(hyoji);
                //    throw new Exception(msg);
                //}

                if (!SpkKifu_WinConsole.Try_SetumeiAll( hyoji))
                {
                    string msg = hyoji.ToContents();
                    Util_Machine.Flush(hyoji);
                    throw new Exception(msg);
                }

                hyoji.AppendLine("終局図");
                SpkBan_1Column.Setumei_NingenGameYo(PureMemory.kifu_endTeme, hyoji);

                return;
            }
        }

        public static void Kifu(FenSyurui f, string line, IHyojiMojiretu hyoji)
        {
            int caret = 0;
            if (Util_String.MatchAndNext("kifu", line, ref caret))
            {
                int temeMade;

                if (line.Length<=caret)
                {
                    if (!SpkKifu_WinConsole.Try_SetumeiAll( hyoji))
                    {
                        Util_Machine.Flush(hyoji);
                        throw new Exception(hyoji.ToContents());
                    }
                    return;
                }
                else if (Util_String.MatchAndNext("goto", line, ref caret))
                {
                    if (LisInt.MatchInt(line, ref caret, out temeMade))// kifu goto 10 など☆
                    {
                        // 指定の手目まで進めるぜ☆（＾～＾）
                        if (!SasiteSeiseiAccessor.Try_GoToTememade(f, temeMade, hyoji))
                        {
                            Util_Machine.Flush(hyoji);
                            throw new Exception(hyoji.ToContents());
                        }

                        hyoji.AppendLine("指定局面図");
                        SpkBan_1Column.Setumei_NingenGameYo(PureMemory.kifu_endTeme, hyoji);
                    }
                    else
                    {

                    }
                }
                else if(Util_String.MatchAndNext("teban", line, ref caret))
                {
                    if (!SpkKifu_WinConsole.Try_SetumeiTebanAll(hyoji))
                    {
                        Util_Machine.Flush(hyoji);
                        throw new Exception(hyoji.ToContents());
                    }
                    return;
                }
                else
                {

                }
            }
        }

        /// <summary>
        /// 駒の利きの数
        /// 旧名「kikikazu」
        /// </summary>
        /// <param name="line"></param>
        public static void Kikisu(string line, IHyojiMojiretu hyoji)
        {
            int caret = 0;
            if (Util_String.MatchAndNext("kikisu",line,ref caret))
            {
                if (Util_String.MatchAndNext("genko", line, ref caret))
                {
                    // 現行
                    hyoji.AppendLine("利き数:全部（現行）全部、駒別");
                    SpkBan_MultiRow.HyojiKomanoKikiSu(PureMemory.gky_ky.yomiKy.yomiShogiban.yomiKikiBan, hyoji);
                }
                else
                {
                    // カウントボード表示☆
                    SpkBan_MultiRow.HyojiKomanoKikiSu(PureMemory.gky_ky.yomiKy.yomiShogiban.yomiKikiBan, hyoji);
                    hyoji.AppendLine();
                }
            }
        }

        /// <summary>
        /// 駒の利き全集
        /// </summary>
        /// <param name="line"></param>
        public static bool TryFail_Kiki(string line, IHyojiMojiretu hyoji)
        {
            #region kiki

            int caret = 0;
            if (Util_String.MatchAndNext("kiki", line, ref caret))
            {
                if (line.Length<=caret)
                {
                    // 重ね利きビットボード表示☆

                    if (CommandK.TryFail_Kiki("kiki genko", hyoji))
                    {
                        // なんらかのエラーの場合
                        return Pure.FailTrue("TryFail_Kiki");
                    }
                    return Pure.SUCCESSFUL_FALSE;
                }
                // アタック駒の要る升を指定しての、利き（リーガル・ムーブ）表示
                else if (Util_String.MatchAndNext("atk", line, ref caret))
                {
                    // もともと付いていなかったが、「atk」を追加

                    Bitboard bb_kiki = new Bitboard();

                    // アタック駒が居る升☆
                    Masu attackerMs;
                    if (!LisMasu.MatchMasu(line, ref caret, out attackerMs
#if DEBUG
                        , (IDebugMojiretu)hyoji
#endif
                    ))
                    {
                        // パースエラーの場合
                        return Pure.FailTrue("升まで入力してくれだぜ☆（＾～＾）");
                    }
                    else
                    {
                        // 駒種類で絞り込み
                        Komasyurui ks;
                        if (Itiran_FenParser.MatchKomasyurui(line, ref caret, out ks))
                        {
                            // 対局者で絞り込み
                            Taikyokusya tai;
                            if (Itiran_FenParser.MatchTaikyokusya(line, ref caret, out tai
#if DEBUG
                                , (IDebugMojiretu)hyoji
#endif
                            ))
                            {
                                // 対局者まで一致
                                // 例「kiki b3 R 1」
                                Koma km = Med_Koma.KomasyuruiAndTaikyokusyaToKoma(ks, tai);

                                BitboardsOmatome.KomanoUgokikataYk00.ToSet_Merge(
                                    km,// 駒
                                    attackerMs,// 升
                                    bb_kiki // リーガルムーブをここに入れるぜ☆（＾～＾）
                                    );
                            }
                            else
                            {
                                // パースエラーの場合
                                return Pure.FailTrue("対局者まで入力してくれだぜ☆（＾～＾）");
                            }
                        }
                        else
                        {
                            // 升まで一致
                            // 旧「kiki b3」→新「kiki atk b3」

                            Taikyokusya tai;
                            PureMemory.gky_ky.yomiKy.yomiShogiban.yomiIbashoBan.ExistsKomaZenbu(attackerMs, out tai);
                            PureMemory.gky_ky.yomiKy.yomiShogiban.yomiIbashoBan.ExistsKoma(tai, attackerMs, out ks);
                            Koma km = Med_Koma.KomasyuruiAndTaikyokusyaToKoma(ks, tai);

                            BitboardsOmatome.KomanoUgokikataYk00.ToSet_Merge(
                                km,// 駒
                                attackerMs,// 升
                                bb_kiki // リーガルムーブの盤面を、ここに入れるぜ☆（＾～＾）
                                );
                        }
                    }

                    // 利きを表示するぜ☆
                    SpkBan_1Column.Setumei_Bitboard("利き", bb_kiki, hyoji);
                    hyoji.AppendLine();
                    return Pure.SUCCESSFUL_FALSE;
                }
                else if (Util_String.MatchAndNext("discovered", line, ref caret))
                {
                    // 次は升
                    if (!LisMasu.MatchMasu( line, ref caret, out Masu ms2
#if DEBUG
                        , (IDebugMojiretu)hyoji
#endif
                        ))
                    {
                        return Pure.FailTrue("discoverdコマンドのマス解析失敗☆？");
                    }
                    else
                    {
                        SpkBan_1Column.Setumei_Discovered(ms2, hyoji);
                        hyoji.AppendLine();
                    }
                    return Pure.SUCCESSFUL_FALSE;
                }
                // 利き表示（現行）
                else if (Util_String.MatchAndNext("genko", line, ref caret))
                {
                    Interproject.project.HyojiKikiItiran(hyoji);
                }
                else
                {
                    return Pure.FailTrue("コマンド解析失敗☆？ コマンド変更「kiki b1」→「kiki atk b1」");
                }
            }
            return Pure.SUCCESSFUL_FALSE;
#endregion
        }
        public static void Koma_cmd(FenSyurui f, string line, IHyojiMojiretu hyoji)
        {
            if (line == "koma")
            {
                // 駒のある場所を表示☆
                SpkBan_1Column.ToHyojiIbasho("Koma_cmd", hyoji);
                return;
            }

            // うしろに続く文字は☆（＾▽＾）
            int caret = 0;
            Util_String.TobasuTangoToMatubiKuhaku(line, ref caret, "koma ");

            if (caret == line.IndexOf("zenbu", caret))
            {
                // 駒全部（だけ）を表示
                SpkBan_MultiColumn.Setumei_Bitboard(new string[] { "Ｐ１", "Ｐ２" },
                    new YomiBitboard[] {
                            PureMemory.gky_ky.shogiban.ibashoBan_yk00.yomiIbashoBan.GetKomaZenbu(Taikyokusya.T1),
                            PureMemory.gky_ky.shogiban.ibashoBan_yk00.yomiIbashoBan.GetKomaZenbu(Taikyokusya.T2)
                    },
                    " 〇 ", "　　",
                    hyoji);
            }
            else
            {
                // raion,zou,kirin,hiyoko,niwatori は廃止
                foreach (Koma km_all in Conv_Koma.itiran)
                {
                    if (caret == line.IndexOf(Conv_Koma.GetFen(f, km_all)))
                    {
                        Komasyurui ks = Med_Koma.KomaToKomasyurui(km_all);
                        // 対局者１、２の らいおん のいる場所を表示☆
                        SpkBan_MultiColumn.Setumei_Bitboard(new string[] { "Ｐ１", "Ｐ２" },
                            new YomiBitboard[] {
                                // FIXME: ここはクローンじゃなくてGetYomi とかにしたいぜ☆（＾～＾）
                            new YomiBitboard(PureMemory.gky_ky.shogiban.ibashoBan_yk00.CloneBb_Koma(Med_Koma.KomasyuruiAndTaikyokusyaToKoma(ks,Taikyokusya.T1))),
                            new YomiBitboard(PureMemory.gky_ky.shogiban.ibashoBan_yk00.CloneBb_Koma(Med_Koma.KomasyuruiAndTaikyokusyaToKoma(ks,Taikyokusya.T2)))
                            },
                            " 〇 ", "　　",
                            hyoji);
                    }
                }
            }

        }

        public static bool TryFail_Ky(
            string line,
            IHyojiMojiretu hyoji
            )
        {
            int caret = 0;
            if (Util_String.MatchAndNext("ky:",line,ref caret))
            {
                SpkGenkyokuOpe.TusinYo_Line( PureSettei.fenSyurui, hyoji);
                return Pure.SUCCESSFUL_FALSE;
            }
            else if (Util_String.MatchAndNext( "ky", line, ref caret))
            {
                if (Util_String.MatchAndNext("fen ", line, ref caret))
                {
                    // 局面を作成するぜ☆（＾▽＾）
                    if (LisGenkyoku.TryFail_MatchPositionvalue(
                        PureSettei.fenSyurui,
                        line, ref caret, out string moves
#if DEBUG
                    , (IDebugMojiretu)hyoji
#endif
                    ))
                    {
                        string msg = "パースに失敗だぜ☆（＾～＾）！ #河馬";
                        hyoji.AppendLine(msg);
                        return Pure.FailTrue("fen ");
                    }
                }
                else if (Util_String.MatchAndNext("fen", line, ref caret))
                {
                    // fenを出力するぜ☆（＾▽＾）
                    SpkGenkyokuOpe.AppendFenTo( PureSettei.fenSyurui, hyoji);
                    hyoji.AppendLine();
                }
                else if (Util_String.MatchAndNext("hanten", line, ref caret))
                {
                    // 盤を１８０度回転☆
                    KyokumenOperation.Hanten();
                    // 駒を配置したあとで使えだぜ☆（＾～＾）
                    PureMemory.gky_ky.shogiban.Tukurinaosi_RemakeKiki();
                    SpkBan_1Column.Setumei_Kyokumen(PureMemory.kifu_endTeme, hyoji);
                    hyoji.AppendLine();
                }
                else if (Util_String.MatchAndNext("kesu ", line, ref caret))
                {
                    string line2 = line.Substring(caret).Trim();

                    if (line2.Length == 2)// ky kesu C4
                    {
                        // 指定した升を空白にするぜ☆（＾▽＾）
                        if (!LisMasu.MatchMasu(line, ref caret, out Masu ms
#if DEBUG
                            , (IDebugMojiretu)hyoji
#endif
                        ))
                        {
                            return Pure.FailTrue("パースエラー101 kesu(1) ");
                        }
                        Koma km_remove = PureMemory.gky_ky.yomiKy.yomiShogiban.yomiIbashoBan.GetBanjoKoma(ms);
                        Debug.Assert(Conv_Koma.IsOk(km_remove), "km_remove can not remove");
                        if (PureMemory.gky_ky.shogiban.TryFail_TorinozokuKoma(
                            ms,
                            km_remove,
                            Conv_Masu.masu_error, true
#if DEBUG
                            , (IDebugMojiretu)hyoji
#endif
                        ))
                        {
                            return Pure.FailTrue("kesu(2) ");
                        }

                        // 駒を配置したあとで使えだぜ☆（＾～＾）
                        PureMemory.gky_ky.shogiban.Tukurinaosi_RemakeKiki();
                    }
                }
                else if (Util_String.MatchAndNext("motu ", line, ref caret))// ky motu I 2
                {
                    bool failure = false;

                    // 次のスペースまで読み取る（駒）
                    string token;
                    Util_String.YomuTangoTobasuMatubiKuhaku(line, ref caret, out token);
                    if (!LisMotiKoma.TryParseFen(PureSettei.fenSyurui, token.ToCharArray()[0], out Motigoma mk))
                    {
                        mk = Motigoma.Yososu;
                        failure = true;
                    }

                    int maisu;
                    if (failure)
                    {
                        maisu = 0;
                        failure = true;
                    }
                    else
                    {
                        if (LisInt.MatchInt(line, ref caret, out maisu))
                        {
                            hyoji.AppendLine("パースエラー103 commandline=[" + line + "]");
                            return Pure.FailTrue("motu(1) ");
                        }
                    }

                    if (failure)
                    {
                    }
                    else
                    {
                        // 持ち駒をセットするぜ☆（＾▽＾）
                        PureMemory.gky_ky.motigomaItiran.Set(mk, maisu);
                    }
                }
                else if (Util_String.MatchAndNext("oku ", line, ref caret))// ky oku R A1
                {
                    // 次のスペースまで読み取る（駒）
                    string token;
                    Util_String.YomuTangoTobasuMatubiKuhaku(line, ref caret, out token);

                    bool failure = false;
                    if (!LisKoma.Try_ParseFen(PureSettei.fenSyurui, token, out Koma km1))
                    {
                        km1 = Koma.Kuhaku;
                        failure = true;
                    }

                    Masu ms1;
                    if (failure)
                    {
                        ms1 = Conv_Masu.masu_error;
                        failure = true;
                    }
                    else
                    {
                        if (!LisMasu.MatchMasu(line, ref caret, out ms1
#if DEBUG
                            , (IDebugMojiretu)hyoji
#endif
                        ))
                        {
                            return Pure.FailTrue(string.Format("oku(1) パースエラー103 commandline=[{0}]", line));
                        }
                        Debug.Assert(Conv_Masu.IsBanjo(ms1), "");
                    }

                    if (failure)
                    {
                    }
                    else
                    {
                        // 指定した升に、指定した駒を置くぜ☆（＾▽＾）

                        if (PureMemory.gky_ky.shogiban.TryFail_OkuKoma(// kyコマンド
                            ms1, km1, true
#if DEBUG
                            , (IDebugMojiretu)hyoji
#endif
                        ))
                        {
                            return Pure.FailTrue("oku(3)");
                        }

                    }
                }
                else if (Util_String.MatchAndNext("set", line, ref caret))
                {
                    CommandlineState.DoMultipleLineCommand((List<string> multipleLineCommand) => {
                        StringBuilder sb = new StringBuilder();
                        foreach (string line1 in multipleLineCommand)
                        {
                            sb.Append(line1);
                        }

                        // 文字列を元に局面を作成
                        LisGenkyoku.SetRule(
                            GameRule.DobutuShogi, PureSettei.banYokoHaba, PureSettei.banTateHaba,
                            sb.ToString()
                            , new Dictionary<Motigoma, int>()
                            {
                                // TODO: 持駒をどうやってセットするかだぜ☆（＾～＾）？
                                    { Motigoma.K,0 },
                                    { Motigoma.Z,0 },
                                    { Motigoma.H,0 },
                                    { Motigoma.k,0 },
                                    { Motigoma.z,0 },
                                    { Motigoma.h,0 },
                            }
                        );

                        //syuturyoku.AppendLine("TODO: 複数行コマンドは=" + sb.ToString());
                    });

                }
                else if (Util_String.MatchAndNext("mazeru", line, ref caret))
                {
                    // 駒配置を適当に入れ替えるぜ☆
                    if (GenkyokuOpe.TryFail_Mazeru( PureSettei.fenSyurui
#if DEBUG
                    , (IDebugMojiretu)hyoji
#endif
                    ))
                    {
                        return Pure.FailTrue("mazeru(1)");
                    }
                    SpkBan_1Column.Setumei_Kyokumen(PureMemory.kifu_endTeme, hyoji);
                }
                else if (Util_String.MatchAndNext("tekiyo", line, ref caret))
                {
                    // 駒を配置したあとで使えだぜ☆（＾～＾）
                    PureMemory.gky_ky.shogiban.Tukurinaosi_RemakeKiki();
                }
                else
                {
                    SpkBan_1Column.Setumei_Kyokumen(PureMemory.kifu_endTeme, hyoji);
                    return Pure.SUCCESSFUL_FALSE;
                }
            }

            return Pure.SUCCESSFUL_FALSE;
        }

    }
}
