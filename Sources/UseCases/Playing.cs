using System;
using System.Collections.Generic;
using System.IO;
using Grayscale.Kifuwarabi.Entities;
using Grayscale.Kifuwarabi.Entities.Logging;
using kifuwarabe_shogithink.fen;
using kifuwarabe_shogithink.pure;
using kifuwarabe_shogithink.pure.accessor;
using kifuwarabe_shogithink.pure.com;
using kifuwarabe_shogithink.pure.control;
using kifuwarabe_shogithink.pure.genkyoku;
using kifuwarabe_shogithink.pure.ikkyoku;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.ky.bb;
using kifuwarabe_shogithink.pure.ky.tobikiki;
using kifuwarabe_shogithink.pure.listen;
using kifuwarabe_shogithink.pure.listen.ky;
using kifuwarabe_shogithink.pure.listen.play;
using kifuwarabe_shogithink.pure.logger;
using kifuwarabe_shogithink.pure.move;
using kifuwarabe_shogithink.pure.speak.ky;
using kifuwarabe_shogithink.pure.speak.play;
using kifuwarabe_shogiwin.consolegame.console;
using kifuwarabe_shogiwin.consolegame.machine;
using kifuwarabe_shogiwin.speak.ban;
using Nett;

namespace Grayscale.Kifuwarabi.UseCases
{
    public class Playing : IPlaying
    {
        public Playing()
        {
            this.commandBufferName = "";
            commandBuffer = new List<string>(0);
        }

        /// <summary>
        /// コマンドを複数ためる仕組み。
        /// </summary>
        public string commandBufferName { get; set; }
        public List<string> commandBuffer { get; set; }

        public void UsiOk(string line, string engineName, string engineAuthor, IHyojiMojiretu hyoji)
        {
            Logger.Flush(hyoji);

            hyoji.AppendLine($"id name {engineName}");
            hyoji.AppendLine($"id author {engineAuthor}");
            hyoji.AppendLine("option name SikoJikan type spin default 500 min 100 max 10000000");
            hyoji.AppendLine("option name SikoJikanRandom type spin default 1000 min 0 max 10000000");
            hyoji.AppendLine("option name Comment type string default Jikan is milli seconds.");
            hyoji.AppendLine("usiok");
            Logger.Flush_USI(hyoji);
        }

        public void ReadOk(string commandline, IHyojiMojiretu hyoji)
        {
            Logger.Flush(hyoji);
            hyoji.AppendLine("readyok");
            Logger.Flush_USI(hyoji);
        }

        public void Atmark(string commandline, IHyojiMojiretu hyoji)
        {
            // 頭の「@」を取って、末尾に「.txt」を付けた文字は☆（＾▽＾）
            this.commandBufferName = commandline.Substring("@".Length);

            var profilePath = System.Configuration.ConfigurationManager.AppSettings["Profile"];
            var toml = Toml.ReadFile(Path.Combine(profilePath, "Engine.toml"));
            var commandPath = toml.Get<TomlTable>("Resources").Get<string>("Command");
            string file = Path.Combine(profilePath, commandPath, $"{this.commandBufferName}.txt");

            this.commandBuffer.Clear();
            if (File.Exists(file)) // Visual Studioで「Unity」とか新しい構成を新規作成した場合は、出力パスも合わせろだぜ☆（＾▽＾）
            {
                this.commandBuffer.AddRange(new List<string>(File.ReadAllLines(file)));
            }
            // 該当しないものは無視だぜ☆（＾▽＾）
        }

        /// <summary>
        /// ビットボードのテスト用だぜ☆（*＾～＾*）
        /// </summary>
        /// <param name="line">コマンドライン</param>
        public bool TryFail_Bitboard(
            string line,
            IHyojiMojiretu hyoji)
        {
            #region bitboard
            if (line == "bitboard")
            {
                // ビットボード表示☆

                // 筋
                {
                    for (int iSuji = 0; iSuji < PureSettei.banYokoHaba; iSuji++)
                    {
                        SpkBan_1Column.Setumei_Bitboard("筋" + iSuji, BitboardsOmatome.bb_sujiArray[iSuji], hyoji);
                    }
                    hyoji.AppendLine();
                }
                // 段
                {
                    for (int iDan = 0; iDan < PureSettei.banTateHaba; iDan++)
                    {
                        SpkBan_1Column.Setumei_Bitboard("段" + iDan, BitboardsOmatome.bb_danArray[iDan], hyoji);
                    }
                    hyoji.AppendLine();
                }
                // トライ
                {
                    SpkBan_MultiColumn.Setumei_Bitboard(new string[] { "対局者１", "対局者２（トライ）" },
                        new YomiBitboard[] { new YomiBitboard(BitboardsOmatome.bb_try[(int)Taikyokusya.T1]), new YomiBitboard(BitboardsOmatome.bb_try[(int)Taikyokusya.T2]) },
                        " △ ", "　　",
                        hyoji);
                    hyoji.AppendLine();
                }

                SpkBan_1Column.ToHyojiIbasho("TryFail_Bitboard", hyoji);// 駒の居場所☆
                SpkBan_MultiRow.HyojiKomanoKikiSu(PureMemory.gky_ky.yomiKy.yomiShogiban.yomiKikiBan, hyoji);// 駒の重ね利き数☆
                SpkBan_MultiRow.HyojiKomanoKiki(PureMemory.gky_ky.yomiKy.yomiShogiban.yomiKikiBan, hyoji);// 駒の利き☆
                SpkBan_MultiRow.HyojiKomanoUgoki(PureMemory.gky_ky.yomiKy.yomiShogiban.yomiKikiBan, PureSettei.banHeimen, hyoji);// 駒の動き☆
                return Pure.SUCCESSFUL_FALSE;
            }
            #endregion

            // うしろに続く文字は☆（＾▽＾）
            int caret = 0;
            Util_String.TobasuTangoToMatubiKuhaku(line, ref caret, "bitboard ");

            if (Util_String.MatchAndNext("has", line, ref caret))
            {
                // hidari agari suji の略
                for (int iMs = 0; iMs < PureSettei.banNanameDanLen; iMs++)
                {
                    SpkBan_1Column.Setumei_Bitboard(
                        string.Format("左上筋{0}", iMs),
                        BitboardsOmatome.bb_hidariAgariSujiArray[iMs],
                        hyoji
                    );
                }
            }
            else if (Util_String.MatchAndNext("hss", line, ref caret))
            {
                // hidari sagari suji の略
                for (int iMs = 0; iMs < PureSettei.banNanameDanLen; iMs++)
                {
                    SpkBan_1Column.Setumei_Bitboard(
                        string.Format("左下筋{0}", iMs),
                        BitboardsOmatome.bb_hidariSagariSujiArray[iMs],
                        hyoji
                    );
                }
            }
            else if (Util_String.MatchAndNext("kiki", line, ref caret))
            {
                // 重ね利きビットボード表示☆

                // 利き
                hyoji.AppendLine("利き:　対局者別、駒別");
                SpkBan_MultiRow.HyojiKomanoKiki(PureMemory.gky_ky.yomiKy.yomiShogiban.yomiKikiBan, hyoji);

                // 利き数
                hyoji.AppendLine("利き数:　対局者別、駒別");
                SpkBan_MultiRow.HyojiKomanoKikiSu(PureMemory.gky_ky.yomiKy.yomiShogiban.yomiKikiBan, hyoji);

                return Pure.SUCCESSFUL_FALSE;
            }
            else if (Util_String.MatchAndNext("kt", line, ref caret))
            {
                // 「bitboard ky」
                // kirin tate の略
                for (int iTai = 0; iTai < Conv_Taikyokusya.itiran.Length; iTai++)
                {
                    Taikyokusya tai = (Taikyokusya)iTai;
                    for (int iMs = 0; iMs < PureSettei.banHeimen; iMs++)
                    {
                        for (int iOjhsh = 0; iOjhsh < Util_Tobikiki.tateToriSu; iOjhsh++)
                        {
                            SpkBan_1Column.Setumei_Bitboard(
                                string.Format("{0}きりん縦{1} {2}",
                                SpkTaikyokusya.sankaku[iTai],
                                iMs,
                                iOjhsh
                                ),
                                BitboardsOmatome.KomanoUgokikataYk00.CloneElement((Taikyokusya)iTai, TobikikiDirection.KT, (Masu)iMs, iOjhsh),
                                hyoji
                            );
                        }
                    }
                }
            }
            else if (Util_String.MatchAndNext("ky", line, ref caret))
            {
                // kirin yoko の略
                for (int iTai = 0; iTai < Conv_Taikyokusya.itiran.Length; iTai++)
                {
                    Taikyokusya tai = (Taikyokusya)iTai;
                    for (int iMs = 0; iMs < PureSettei.banHeimen; iMs++)
                    {
                        for (int iOjhsh = 0; iOjhsh < Util_Tobikiki.yokoToriSu; iOjhsh++)
                        {
                            SpkBan_1Column.Setumei_Bitboard(
                                string.Format("{0}きりん横{1} {2}",
                                SpkTaikyokusya.sankaku[iTai],
                                iMs,
                                iOjhsh)
                                ,
                                BitboardsOmatome.KomanoUgokikataYk00.CloneElement(
                                    (Taikyokusya)iTai, TobikikiDirection.KY, (Masu)iMs, iOjhsh),
                                hyoji
                            );
                        }
                    }
                }

            }
            // マスク表
            else if (Util_String.MatchAndNext("mask", line, ref caret))
            {
                int exp;
                if (LisInt.MatchInt(line, ref caret, out exp))
                {
                    hyoji.AppendLine(string.Format("bitboard mask ?? 数字を付けろだぜ☆ｍ９（＾～＾） line=[{0}] caret={1}", line, caret));
                    return Pure.SUCCESSFUL_FALSE;
                }

                if (-1 < exp && exp < BitboardsOmatome.maskHyo.Length)
                {
                    hyoji.AppendLine(string.Format("value063={0}", Convert.ToString((int)BitboardsOmatome.maskHyo[exp].value063, 2)));
                }
                else
                {
                    hyoji.AppendLine(string.Format("bitboard mask {0} 数字が範囲外だぜ☆ｍ９（＾～＾）", exp));
                    return Pure.SUCCESSFUL_FALSE;
                }
            }

            //            // 「bitboard remake」は廃止して、「updaterule」に移行
            //            else if (Util_String.MatchAndNext("remake", line, ref caret))
            //            {
            //                // 駒の動き方を作り直し
            //                Util_Control.UpdateRule(
            //                    gky.ky,
            //#if DEBUG
            //                    "CommandB.remake"
            //#endif
            //                    );
            //            }
            else if (Util_String.MatchAndNext("s", line, ref caret))
            {
                for (int iTai = 0; iTai < Conv_Taikyokusya.itiran.Length; iTai++)
                {
                    Taikyokusya tai = (Taikyokusya)iTai;
                    // inosisi の略
                    for (int iMs = 0; iMs < PureSettei.banHeimen; iMs++)
                    {
                        for (int iOjhsh = 0; iOjhsh < Util_Tobikiki.tateToriSu; iOjhsh++)
                        {
                            SpkBan_1Column.Setumei_Bitboard(
                                string.Format("{0}いのしし{1} {2}",
                                SpkTaikyokusya.sankaku[iTai],
                                iMs, iOjhsh),
                                BitboardsOmatome.KomanoUgokikataYk00.CloneElement(
                                    (Taikyokusya)iTai, TobikikiDirection.S, (Masu)iMs, iOjhsh),
                                hyoji
                            );
                        }
                    }
                }
            }
            else if (Util_String.MatchAndNext("usagitobi", line, ref caret))
            {
#if DEBUG
                // うさぎ跳びの表示
                SpkBan_Hisigata.Setumei_yk00("▲うさぎ右跳び", BitboardsOmatome.bb_usagigaMiginiToberu[(int)Taikyokusya.T1], hyoji);
                SpkBan_Hisigata.Setumei_yk00("▲うさぎ左跳び", BitboardsOmatome.bb_usagigaHidariniToberu[(int)Taikyokusya.T1], hyoji);
                SpkBan_Hisigata.Setumei_yk00("△うさぎ右跳び", BitboardsOmatome.bb_usagigaMiginiToberu[(int)Taikyokusya.T2], hyoji);
                SpkBan_Hisigata.Setumei_yk00("△うさぎ左跳び", BitboardsOmatome.bb_usagigaHidariniToberu[(int)Taikyokusya.T2], hyoji);
#endif
            }
            else if (Util_String.MatchAndNext("zha", line, ref caret))
            {
                // kirin yoko の略
                for (int iTai = 0; iTai < Conv_Taikyokusya.itiran.Length; iTai++)
                {
                    Taikyokusya tai = (Taikyokusya)iTai;

                    // zou hidari agari の略
                    for (int iMs = 0; iMs < PureSettei.banHeimen; iMs++)
                    {
                        for (int iOjhsh = 0; iOjhsh < Util_Tobikiki.nanameToriSu; iOjhsh++)
                        {
                            SpkBan_1Column.Setumei_Bitboard(
                                string.Format("{0}ぞう左上{0} {1}",
                                SpkTaikyokusya.sankaku[iTai],
                                iMs, iOjhsh),
                                BitboardsOmatome.KomanoUgokikataYk00.CloneElement(
                                    (Taikyokusya)iTai, TobikikiDirection.ZHa, (Masu)iMs, iOjhsh),
                                    hyoji
                                );
                        }
                    }
                }

            }
            else if (Util_String.MatchAndNext("zhs", line, ref caret))
            {
                // kirin yoko の略
                for (int iTai = 0; iTai < Conv_Taikyokusya.itiran.Length; iTai++)
                {
                    Taikyokusya tai = (Taikyokusya)iTai;
                    // zou hidari sagari の略
                    for (int iMs = 0; iMs < PureSettei.banHeimen; iMs++)
                    {
                        for (int iOjhsh = 0; iOjhsh < Util_Tobikiki.nanameToriSu; iOjhsh++)
                        {
                            SpkBan_1Column.Setumei_Bitboard(
                                string.Format("{0}ぞう左下{1} {2}",
                                SpkTaikyokusya.sankaku[iTai],
                                iMs, iOjhsh),
                                BitboardsOmatome.KomanoUgokikataYk00.CloneElement(
                                    (Taikyokusya)iTai, TobikikiDirection.ZHs, (Masu)iMs, iOjhsh),
                                hyoji
                            );
                        }
                    }
                }
            }

            return Pure.SUCCESSFUL_FALSE;
        }

        public void CanDo(FenSyurui f, string line,
    CommandMode commandMode, IHyojiMojiretu hyoji)
        {
            // うしろに続く文字は☆（＾▽＾）
            int caret = 0;
            if (Util_String.MatchAndNext("cando", line, ref caret))
            {
                Move ss;
                if (line.Length <= caret)
                {
                    return;
                }
                else if (!LisPlay.MatchFenMove(f, line, ref caret, out ss))
                {
                    throw new Exception("パースエラー [" + line + "]");
                }

                if (GenkyokuOpe.CanDoMove(ss, out MoveMatigaiRiyu riyu))
                {
                    hyoji.AppendLine("cando, true");
                }
                else
                {
                    hyoji.Append("cando, false, ");
                    hyoji.AppendLine(riyu.ToString());
                }
            }
        }

        /// <summary>
        /// 置換表
        /// </summary>
        /// <param name="line"></param>
        /// <param name="hyoji"></param>
        public bool TryFail_ChikanHyo(string line,
            IHyojiMojiretu hyoji)
        {
            // うしろに続く文字は☆（＾▽＾）
            int caret = 0;
            Util_String.TobasuTangoToMatubiKuhaku(line, ref caret, "chikanhyo ");

            // 左肩上がり    の置換表
            if (Util_String.MatchAndNext("ha45", line, ref caret))
            {
                // hidarikata agari 45 chikan の略
                SpkBan_MultiColumn.Setumei_MasuHyo(
                    new string[] { "左肩上４５" },
                    new Masu[][] { RotateChikanhyo.chikanHyo_ha45 },
                    false,
                    hyoji
                    );
            }
            // 左肩下がり    の置換表
            else if (Util_String.MatchAndNext("hs45", line, ref caret))
            {
                // hidarikata sagari 45 chikan の略
                SpkBan_MultiColumn.Setumei_MasuHyo(
                    new string[] { "左肩下４５" },
                    new Masu[][] { RotateChikanhyo.chikanHyo_hs45 },
                    false,
                    hyoji
                    );
            }
            // ９０°回転    の置換表
            else if (Util_String.MatchAndNext("ht90", line, ref caret))
            {
                // han tokei mawari 90 chikan の略
                SpkBan_MultiColumn.Setumei_MasuHyo(
                    new string[] { "反時回90" },
                    new Masu[][] { RotateChikanhyo.chikanHyo_ht90 },
                    true, // 横、縦の長さを反転
                    hyoji
                    );
            }

            return Pure.SUCCESSFUL_FALSE;
        }

        public void Clear()
        {
            Util_Machine.Clear();
        }

        /// <summary>
        /// 「dosub」Doを分解したサブルーチン
        /// </summary>
        /// <param name="isSfen"></param>
        /// <param name="line"></param>
        /// <param name="ky"></param>
        /// <param name="commandMode"></param>
        /// <param name="hyoji"></param>
        /// <returns></returns>
        public bool TryFail_DoSub(string line,
            IHyojiMojiretu hyoji)
        {
            int caret = 0;
            if (Util_String.MatchAndNext("dosub", line, ref caret))
            {
                if (Util_String.MatchAndNext("daiOff", line, ref caret))
                {
                    if (!LisPlay.MatchFenMove(PureSettei.fenSyurui, line, ref caret, out Move ss))
                    {
                        return Pure.FailTrue(string.Format("指し手のパースエラー [{0}]", line));
                    }

                    MoveGenAccessor.BunkaiMoveDmv(ss);

                    DoMoveOpe.TryFail_DaiOff(
                        PureMemory.dmv_ms_t0,
                        PureMemory.dmv_km_t0,
                        PureMemory.dmv_mk_t0,
                        PureMemory.dmv_ms_t1
#if DEBUG
                        , PureSettei.fenSyurui
                        , (IDebugMojiretu)hyoji
#endif
                        );
                }
                // 取った駒を、駒台に置くぜ☆（＾▽＾）
                else if (Util_String.MatchAndNext("daiOn", line, ref caret))
                {
                    if (!LisPlay.MatchFenMove(PureSettei.fenSyurui, line, ref caret, out Move ss))
                    {
                        return Pure.FailTrue(string.Format("指し手のパースエラー [{0}]", line));
                    }

                    MoveGenAccessor.BunkaiMoveDmv(ss);

                    DoMoveOpe.TryFail_DaiOn(
                        PureMemory.dmv_km_c,
                        PureMemory.dmv_ks_c,
                        PureMemory.dmv_mk_c
#if DEBUG
                        , PureSettei.fenSyurui
                        , (IDebugMojiretu)hyoji
#endif
                        );
                }
                // 移動先に駒があれば消すぜ☆（＾～＾）
                else if (Util_String.MatchAndNext("dstOff", line, ref caret))
                {
                    if (!LisPlay.MatchFenMove(PureSettei.fenSyurui, line, ref caret, out Move ss))
                    {
                        return Pure.FailTrue(string.Format("指し手のパースエラー [{0}]", line));
                    }

                    MoveGenAccessor.BunkaiMoveDmv(ss);

                    DoMoveOpe.TryFail_DstOff(
                        PureMemory.dmv_ms_t1,
                        PureMemory.dmv_km_c,
                        PureMemory.dmv_ks_c
#if DEBUG
                        , PureSettei.fenSyurui
                        , (IDebugMojiretu)hyoji
#endif
                        );
                }
                // 移動先に駒を置くぜ☆（＾～＾）
                else if (Util_String.MatchAndNext("dstOn", line, ref caret))
                {
                    // dosub dstOn 移動元升 移動先升 駒

                    // 移動元升
                    Masu ms_t0;
                    if (!Itiran_FenParser.MatchSrcMs(line, ref caret, out ms_t0
#if DEBUG
                            , (IDebugMojiretu)hyoji
#endif
                        ))
                    {
                        // エラーの場合、「打ち」の可能性
                    }
                    // 移動先升
                    Masu ms_t1;
                    Itiran_FenParser.MatchSrcMs(line, ref caret, out ms_t1
#if DEBUG
                            , (IDebugMojiretu)hyoji
#endif
                        );
                    // 駒
                    Koma km_t1;
                    Itiran_FenParser.MatchKoma(line, ref caret, out km_t1);

                    DoMoveOpe.TryFail_DstOn(
                        ms_t0, // 移動元の升
                        km_t1, // 駒
                        ms_t1 // 打ち先の升
#if DEBUG
                        , PureSettei.fenSyurui
                        , (IDebugMojiretu)hyoji
#endif
                        );
                }
                else if (Util_String.MatchAndNext("srcOff", line, ref caret))
                {
                    if (!LisPlay.MatchFenMove(PureSettei.fenSyurui, line, ref caret, out Move ss))
                    {
                        return Pure.FailTrue(string.Format("指し手のパースエラー [{0}]", line));
                    }

                    MoveGenAccessor.BunkaiMoveDmv(ss);

                    DoMoveOpe.TryFail_SrcOff(
                        ss,
                        PureMemory.dmv_ms_t0,
                        PureMemory.dmv_km_t1,
                        PureMemory.dmv_mk_t0,
                        PureMemory.dmv_ms_t1
#if DEBUG
                        , PureSettei.fenSyurui
                        , (IDebugMojiretu)hyoji
#endif
                        );
                }
                // 指し手の終わりに☆（＾～＾）
                else if (Util_String.MatchAndNext("addMoveToKifu", line, ref caret))
                {
                    // 「dosub addMoveToKifu K*E5 -」
                    // 指し手、取った駒種類
                    if (!LisPlay.MatchFenMove(PureSettei.fenSyurui, line, ref caret, out Move ss))
                    {
                        return Pure.FailTrue(string.Format("指し手のパースエラー [{0}]", line));
                    }

                    Komasyurui ks_c;
                    if (Util_String.MatchAndNext("-", line, ref caret))
                    {
                        ks_c = Komasyurui.Yososu;
                    }
                    else if (LisKomasyurui.MatchKomasyurui(line, ref caret, out ks_c))
                    {
                    }
                    else
                    {
                        return Pure.FailTrue(string.Format("指し手のパースエラー [{0}]", line));
                    }

                    MoveGenAccessor.AddKifu(ss, MoveType.N00_Karappo, PureMemory.dmv_ks_c);
                    //#if DEBUG
                    //                    Util_Tansaku.Snapshot("DoSub(1)", (IDebugMojiretu)hyoji);
                    //#endif
                }
                else
                {
                    return Pure.FailTrue(string.Format("コマンドのパースエラー☆（＾～＾） [{0}]", line));
                }
            }

            return Pure.SUCCESSFUL_FALSE;
        }

        public bool TryFail_Do(FenSyurui f, string line,
    CommandMode commandMode, IHyojiMojiretu hyoji)
        {
            int caret = 0;
            if (Util_String.MatchAndNext("do", line, ref caret))//"do "
            {
                // 一体型☆（＾～＾）
                if (!LisPlay.MatchFenMove(f, line, ref caret, out Move ss))
                {
                    return Pure.FailTrue(string.Format("指し手のパースエラー [{0}]", line));
                }

                MoveType ssType = MoveType.N00_Karappo;
                if (DoMoveOpe.TryFailDoMoveAll(
                    ss,
                    ssType
#if DEBUG
                    , f
                    , (IDebugMojiretu)hyoji
                    , false
                    , "CommandD#Do"
#endif
                ))
                {
                    return Pure.FailTrue("TryFail_DoMove_All");
                }
                // 手番を進めるぜ☆（＾～＾）
                MoveGenAccessor.AddKifu(ss, ssType, PureMemory.dmv_ks_c);
                //#if DEBUG
                //                Util_Tansaku.Snapshot("Doコマンド", (IDebugMojiretu)hyoji);
                //#endif

                // FIXME: 局面表示
                switch (commandMode)
                {
                    case CommandMode.NigenYoConsoleKaihatu: SpkBan_1Column.Setumei_Kyokumen(PureMemory.kifu_endTeme, hyoji); break;
                    case CommandMode.NingenYoConsoleGame: SpkBan_1Column.Setumei_NingenGameYo(PureMemory.kifu_endTeme, hyoji); break;
                }
            }
            else
            {

            }

            return Pure.SUCCESSFUL_FALSE;
        }
#if DEBUG
        /// <summary>
        /// ダンプ
        /// </summary>
        /// <param name="line"></param>
        /// <param name="ky2"></param>
        /// <param name="hyoji"></param>
        public static bool TryFail_Dump(string line, IHyojiMojiretu hyoji)
        {
            int caret = 0;
            if (Util_String.MatchAndNext("dump", line, ref caret))
            {
                if (line.Length<=caret)
                {
                    Pure.Sc.Push("dumpコマンド");
                    bool ret = SpkDump.TryFail_Dump(hyoji);
                    Pure.Sc.Pop();
                    return ret;
                }
#if DEBUG
                else if (Util_String.MatchAndNext("move", line, ref caret))
                {
                    MoveSeiseiAccessor.DumpMoveSeisei(hyoji);
                }
#endif
            }

            return false;
        }
#endif

        /// <summary>
        /// 符号
        /// </summary>
        /// <param name="line"></param>
        /// <param name="hyoji"></param>
        public bool TryFail_Fugo(string line,
            IHyojiMojiretu hyoji)
        {
            // うしろに続く文字は☆（＾▽＾）
            int caret = 0;
            Util_String.TobasuTangoToMatubiKuhaku(line, ref caret, "fugo ");

            // 段の符号
            if (Util_String.MatchAndNext("dan", line, ref caret))
            {
                int[][] numberHyoHairetu = new int[1][];
                numberHyoHairetu[0] = new int[PureSettei.banHeimen];
                for (int iMs = 0; iMs < PureSettei.banHeimen; iMs++)
                {
                    numberHyoHairetu[0][iMs] = Conv_Masu.ToDanO1_WithoutErrorCheck(iMs);
                }

                SpkBan_MultiColumn.Setumei_Masutbl(
                    new string[] { "段符号" },
                    numberHyoHairetu,
                    false,
                    hyoji
                    );
            }
            // 左上筋の符号
            else if (Util_String.MatchAndNext("ha", line, ref caret))
            {
                int[][] numberHyoHairetu = new int[1][];
                numberHyoHairetu[0] = new int[PureSettei.banHeimen];
                for (int iMs = 0; iMs < PureSettei.banHeimen; iMs++)
                {
                    numberHyoHairetu[0][iMs] = Conv_Masu.ToHidariAgariSujiO1_WithoutErrorCheck(iMs);
                }

                SpkBan_MultiColumn.Setumei_Masutbl(
                    new string[] { "左上筋符号" },
                    numberHyoHairetu,
                    false,
                    hyoji
                    );
            }
            // 左下筋の符号
            else if (Util_String.MatchAndNext("hs", line, ref caret))
            {
                int[][] numberHyoHairetu = new int[1][];
                numberHyoHairetu[0] = new int[PureSettei.banHeimen];
                for (int iMs = 0; iMs < PureSettei.banHeimen; iMs++)
                {
                    numberHyoHairetu[0][iMs] = Conv_Masu.ToHidariSagariSujiO1_WithoutErrorCheck(iMs);
                }

                SpkBan_MultiColumn.Setumei_Masutbl(
                    new string[] { "左下筋符号" },
                    numberHyoHairetu,
                    false,
                    hyoji
                    );
            }
            // 筋の符号
            else if (Util_String.MatchAndNext("suji", line, ref caret))
            {
                int[][] numberHyoHairetu = new int[1][];
                numberHyoHairetu[0] = new int[PureSettei.banHeimen];
                for (int iMs = 0; iMs < PureSettei.banHeimen; iMs++)
                {
                    numberHyoHairetu[0][iMs] = Conv_Masu.ToSujiO1_WithoutErrorCheck(iMs);
                }

                SpkBan_MultiColumn.Setumei_Masutbl(
                    new string[] { "筋符号" },
                    numberHyoHairetu,
                    false,
                    hyoji
                    );
            }

            return Pure.SUCCESSFUL_FALSE;
        }

        public void Gameover(string line, IHyojiMojiretu hyoji)
        {
            int caret = 0;
            if (Util_String.MatchAndNext("gameover", line, ref caret))
            {
                if (Util_String.MatchAndNext("lose", line, ref caret))
                {
                    // コンピューターは止めるぜ☆（*＾～＾*）次のイリーガルな指し手を指してしまうからなｗｗｗｗ☆（＾▽＾）
                    switch (PureMemory.kifu_teban)
                    {
                        case Taikyokusya.T1: PureSettei.p1Com = false; break;
                        case Taikyokusya.T2: PureSettei.p2Com = false; break;
                        default: break;
                    }
                }
                else
                {

                }
            }
            else
            {

            }
        }

        /// <summary>
        /// USI でも go を受信するぜ☆（＾～＾）
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="ky"></param>
        /// <param name="hyoji"></param>
        public bool TryFail_Go(bool isUsi, FenSyurui f, CommandMode mode, IHyojiMojiretu hyoji)
        {
            Util_Tansaku.PreGo();
            if (Util_Tansaku.TryFail_Go(hyoji))
            {
                return Pure.FailTrue("PurePlay.Try_Go");
            }

            // 勝敗判定☆（＾▽＾）
            if (!Util_Kettyaku.Try_JudgeKettyaku(PureMemory.tnsk_kohoMove
#if DEBUG
                , hyoji
#endif
                ))
            {
                return Pure.FailTrue("Try_JudgeKettyaku");
            }

            if (isUsi)
            {
                Logger.Flush(hyoji);
                hyoji.Append("bestmove ");
                SpkMove.AppendFenTo(f, PureMemory.tnsk_kohoMove, hyoji);
                hyoji.AppendLine();
                Logger.Flush_USI(hyoji);
            }
            else if (mode == CommandMode.NigenYoConsoleKaihatu)
            {
                // 開発モードでは、指したあとに盤面表示を返すぜ☆（＾▽＾）
                SpkBan_1Column.Setumei_Kyokumen(PureMemory.kifu_endTeme, hyoji);
            }
            // ゲームモードでは表示しないぜ☆（＾▽＾）

            return Pure.SUCCESSFUL_FALSE;
        }

    }
}
