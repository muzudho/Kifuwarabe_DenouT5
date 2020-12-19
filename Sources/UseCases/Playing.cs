using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Grayscale.Kifuwarabi.Entities;
using Grayscale.Kifuwarabi.Entities.Logging;
using kifuwarabe_shogithink.fen;
using kifuwarabe_shogithink.pure;
using kifuwarabe_shogithink.pure.accessor;
using kifuwarabe_shogithink.pure.com;
using kifuwarabe_shogithink.pure.com.MoveOrder;
using kifuwarabe_shogithink.pure.com.MoveOrder.hioute;
using kifuwarabe_shogithink.pure.control;
using kifuwarabe_shogithink.pure.genkyoku;
using kifuwarabe_shogithink.pure.ikkyoku;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.ky.bb;
using kifuwarabe_shogithink.pure.ky.tobikiki;
using kifuwarabe_shogithink.pure.listen;
using kifuwarabe_shogithink.pure.listen.genkyoku;
using kifuwarabe_shogithink.pure.listen.ky;
using kifuwarabe_shogithink.pure.listen.play;
using kifuwarabe_shogithink.pure.logger;
using kifuwarabe_shogithink.pure.med.ky;
using kifuwarabe_shogithink.pure.move;
using kifuwarabe_shogithink.pure.speak.genkyoku;
using kifuwarabe_shogithink.pure.speak.ky;
using kifuwarabe_shogithink.pure.speak.ky_info;
using kifuwarabe_shogithink.pure.speak.play;
using kifuwarabe_shogiwin.consolegame;
using kifuwarabe_shogiwin.consolegame.console;
using kifuwarabe_shogiwin.consolegame.machine;
using kifuwarabe_shogiwin.listen;
using kifuwarabe_shogiwin.speak;
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

            multipleLineCommand = new List<string>();
        }

        /// <summary>
        /// コマンドを複数ためる仕組み。
        /// </summary>
        public string commandBufferName { get; set; }
        public List<string> commandBuffer { get; set; }

        #region 複数行コマンド
        /// <summary>
        /// TODO: 複数行コマンドモード☆（＾～＾）
        /// 「.」だけの行になるまで続く予定☆（＾～＾）
        /// </summary>
        public bool isMultipleLineCommand;
        public List<string> multipleLineCommand;
        public void DoMultipleLineCommand(DLGT_MultipleLineCommand dlgt_multipleLineCommand)
        {
            isMultipleLineCommand = true;
            //isKyokumenEcho1 = false;
            this.dlgt_multipleLineCommand = dlgt_multipleLineCommand;
        }
        public delegate void DLGT_MultipleLineCommand(List<string> multipleLineCommand);
        public DLGT_MultipleLineCommand dlgt_multipleLineCommand;
        #endregion

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

        public void Hirate(string line, IHyojiMojiretu hyoji)
        {
            int caret = 0;
            if (Util_String.MatchAndNext("hirate", line, ref caret))
            {
                if (Util_String.MatchAndNext("toroku", line, ref caret))
                {
                    //────────────────────────────────────────
                    // 現局面を、平手初期局面として登録します
                    //────────────────────────────────────────
                    MedKyokumen.TorokuHirate();
                }
                else
                {
                    //────────────────────────────────────────
                    // 感想戦モードのときに hirate コマンドを打ち込んだ場合、感想戦モードを解除します
                    //────────────────────────────────────────
                    if (GameMode.Kansosen == PureAppli.gameMode)
                    {
                        PureAppli.gameMode = GameMode.Karappo;
                    }

                    //────────────────────────────────────────
                    // 平手初期局面に戻します
                    //────────────────────────────────────────
                    // 棋譜カーソルを０にすれば、初期局面に戻るだろ☆ｗｗｗ（＾▽＾）
                    MoveGenAccessor.BackTemeToFirst_AndClearTeme();
                    //// どうぶつしょうぎの平手初期局面に変更するぜ☆ｗｗｗ（＾▽＾）
                    //LisGenkyoku.SetRule(
                    //    GameRule.DobutuShogi, 3, 4,
                    //    "キラゾ" +
                    //    "　ヒ　" +
                    //    "　ひ　" +
                    //    "ぞらき"
                    //    , new Dictionary<Motigoma, int>()
                    //    {
                    //        { Motigoma.K,0 },
                    //        { Motigoma.Z,0 },
                    //        { Motigoma.H,0 },
                    //        { Motigoma.k,0 },
                    //        { Motigoma.z,0 },
                    //        { Motigoma.h,0 },
                    //    }
                    //);

                    //────────────────────────────────────────
                    // 局面を表示します
                    //────────────────────────────────────────
                    SpkBan_1Column.Setumei_Kyokumen(PureMemory.kifu_endTeme, hyoji);
                }
            }
            else
            {

            }
        }

        /// <summary>
        /// 現在の将棋盤をクリアーして、翻訳結果の将棋盤を作るぜ☆（＾～＾）
        /// </summary>
        /// <param name="line"></param>
        /// <param name="hyoji"></param>
        public void Honyaku(string line, IHyojiMojiretu hyoji)
        {
            int caret = 0;
            if (Util_String.MatchAndNext("honyaku", line, ref caret))
            {
                if (Util_String.MatchAndNext("fen", line, ref caret))
                {
                    if (Util_String.MatchAndNext("sfen", line, ref caret))
                    {
                        string positionvalue = line.Substring(caret);

                        int caret2 = 0;
                        if (LisGenkyoku.TryFail_MatchPositionvalue(
                            FenSyurui.dfe_n,
                            positionvalue, ref caret2, out string moves
#if DEBUG
                        , (IDebugMojiretu)hyoji
#endif
                        ))
                        {
                            Logger.Flush(hyoji);
                            throw new Exception(hyoji.ToContents());
                        }

                        hyoji.AppendLine(PureMemory.kifu_syokiKyokumenFen);
                        Logger.Flush(hyoji);
                    }
                    else
                    {

                    }
                }
                else if (Util_String.MatchAndNext("sfen", line, ref caret))
                {
                    if (Util_String.MatchAndNext("fen", line, ref caret))
                    {
                        string positionvalue = line.Substring(caret);
                        int caret2 = 0;
                        if (LisGenkyoku.TryFail_MatchPositionvalue(
                            FenSyurui.sfe_n,
                            positionvalue, ref caret2, out string moves
#if DEBUG
                        , (IDebugMojiretu)hyoji
#endif
                        ))
                        {
                            Logger.Flush(hyoji);
                            throw new Exception(hyoji.ToContents());
                        }

                        ICommandMojiretu sfen = new MojiretuImpl();
                        SpkGenkyokuOpe.AppendFenTo(FenSyurui.dfe_n, sfen);// 局面は、棋譜を持っていない

                        //if ("" != moves)
                        //{
                        //    moves = moves.Substring("moves ".Length);
                        //    Kifu kifu2 = new Kifu();
                        //    kifu2.AddMoves(true, moves, gky.yomiKy); // ky2 は空っぽなんで、 ky の診断を使う。盤サイズを見る。
                        //    sfen.Append(" moves ");
                        //    kifu2.AppendMovesTo(true, sfen);
                        //}

                        sfen.AppendLine();
                        Logger.Flush((IHyojiMojiretu)sfen);
                    }
                    else
                    {

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

        public void Hyoka(string line, IHyojiMojiretu hyoji)
        {
            int caret = 0;
            if (Util_String.MatchAndNext("hyoka", line, ref caret))
            {
                //                Conv_HyokatiAb.Hyoka_Remake(
                //                    gky,
                //                    out HyokatiAb hyokatiUtiwake,
                //                    true //ランダムな局面の可能性もあるぜ☆（＾～＾）
                //#if DEBUG
                //                    ,HyokaRiyu.Yososu
                //#endif
                //                );

                if (Util_String.MatchAndNext("genkyoku", line, ref caret))
                {
                    // 現局面

                    // 棋譜を使って現局面まで指すか、計算で求めるか？
                }
                else
                {
                    // 探索後は、末端局面評価値になる
                    hyoji.Append(string.Format("手番から見た評価値 {0} ",
                        SpkHyokati.ToContents(PureMemory.gky_hyokati)));
                }

                return;
            }
        }

        public void Jokyo(string line, IHyojiMojiretu hyoji)
        {
            if (line == "jokyo")
            {
                hyoji.AppendLine("GameMode = " + PureAppli.gameMode);
                hyoji.AppendLine("Kekka    = " + PureMemory.gky_kekka);
                hyoji.AppendLine("Kettyaku = " + Genkyoku.IsKettyaku());
                return;
            }
        }

        /// <summary>
        /// 初期局面まで undo したいが☆（＾～＾）
        /// </summary>
        /// <param name="f"></param>
        /// <param name="line"></param>
        /// <param name="hyoji"></param>
        public void Kansosen(FenSyurui f, string line, IHyojiMojiretu hyoji)
        {
            int caret = 0;
            if (Util_String.MatchAndNext("kansosen", line, ref caret))
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
                //if (!MoveSeiseiAccessor.Try_PlayToFinish(f, hyoji))
                //{
                //    string msg = hyoji.ToContents();
                //    Util_Machine.Flush(hyoji);
                //    throw new Exception(msg);
                //}

                if (!SpkKifu_WinConsole.Try_SetumeiAll(hyoji))
                {
                    string msg = hyoji.ToContents();
                    Logger.Flush(hyoji);
                    throw new Exception(msg);
                }

                hyoji.AppendLine("終局図");
                SpkBan_1Column.Setumei_NingenGameYo(PureMemory.kifu_endTeme, hyoji);

                return;
            }
        }

        public void Kifu(FenSyurui f, string line, IHyojiMojiretu hyoji)
        {
            int caret = 0;
            if (Util_String.MatchAndNext("kifu", line, ref caret))
            {
                int temeMade;

                if (line.Length <= caret)
                {
                    if (!SpkKifu_WinConsole.Try_SetumeiAll(hyoji))
                    {
                        Logger.Flush(hyoji);
                        throw new Exception(hyoji.ToContents());
                    }
                    return;
                }
                else if (Util_String.MatchAndNext("goto", line, ref caret))
                {
                    if (LisInt.MatchInt(line, ref caret, out temeMade))// kifu goto 10 など☆
                    {
                        // 指定の手目まで進めるぜ☆（＾～＾）
                        if (!MoveGenAccessor.Try_GoToTememade(f, temeMade, hyoji))
                        {
                            Logger.Flush(hyoji);
                            throw new Exception(hyoji.ToContents());
                        }

                        hyoji.AppendLine("指定局面図");
                        SpkBan_1Column.Setumei_NingenGameYo(PureMemory.kifu_endTeme, hyoji);
                    }
                    else
                    {

                    }
                }
                else if (Util_String.MatchAndNext("teban", line, ref caret))
                {
                    if (!SpkKifu_WinConsole.Try_SetumeiTebanAll(hyoji))
                    {
                        Logger.Flush(hyoji);
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
        public void Kikisu(string line, IHyojiMojiretu hyoji)
        {
            int caret = 0;
            if (Util_String.MatchAndNext("kikisu", line, ref caret))
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
        public bool TryFail_Kiki(string line, IHyojiMojiretu hyoji)
        {
            #region kiki

            int caret = 0;
            if (Util_String.MatchAndNext("kiki", line, ref caret))
            {
                if (line.Length <= caret)
                {
                    // 重ね利きビットボード表示☆

                    if (this.TryFail_Kiki("kiki genko", hyoji))
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
                    if (!LisMasu.MatchMasu(line, ref caret, out Masu ms2
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

        public bool TryFail_Ky(
    string line,
    IHyojiMojiretu hyoji
    )
        {
            int caret = 0;
            if (Util_String.MatchAndNext("ky:", line, ref caret))
            {
                SpkGenkyokuOpe.TusinYo_Line(PureSettei.fenSyurui, hyoji);
                return Pure.SUCCESSFUL_FALSE;
            }
            else if (Util_String.MatchAndNext("ky", line, ref caret))
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
                    SpkGenkyokuOpe.AppendFenTo(PureSettei.fenSyurui, hyoji);
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
                    this.DoMultipleLineCommand((List<string> multipleLineCommand) =>
                    {
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
                    if (GenkyokuOpe.TryFail_Mazeru(PureSettei.fenSyurui
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

        public void Koma_cmd(FenSyurui f, string line, IHyojiMojiretu hyoji)
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

        public void Man(IHyojiMojiretu hyoji)
        {
            // なるべく、アルファベット順☆（＾▽＾）
            hyoji.AppendLine("(空っぽEnter)   : ゲームモードのフラグを ON にするぜ☆");
            hyoji.AppendLine("@例             : 「例.txt」ファイル読込んでコマンド実行だぜ☆(UTF-8 BOM有り)");
            hyoji.AppendLine("#コメント       : なんにもしないぜ☆");
            hyoji.AppendLine("bitboard        : ビットボードのテスト用だぜ☆");
            hyoji.AppendLine("bitboard kiki   : 駒の動きを表示するぜ☆");
            hyoji.AppendLine("bitboard remake : 駒の動きを作り直すぜ☆");
#if DEBUG
            hyoji.AppendLine("bitboard usagitobi: 桂馬跳びの表示☆");
#endif
            hyoji.AppendLine("cando B4B3      : B4にある駒をB3へ動かせるなら true を返すぜ☆");
            hyoji.AppendLine("                : 動かせないなら「false, 理由」を返すぜ☆");

            hyoji.AppendLine("chikanhyo ha45  : 左肩上がり４５°の盤面表示☆");
            hyoji.AppendLine("chikanhyo hs45  : 左肩下がり４５°の盤面表示☆");
            hyoji.AppendLine("chikanhyo ht90  : 反時計回り９０°の盤面表示☆");

            hyoji.AppendLine("clear           : コンソールをクリアーするぜ☆");
            hyoji.AppendLine("do B4B3         : B4にある駒をB3へ動かしたあと ky するぜ☆");
            hyoji.AppendLine("                : アルファベットは小文字でも構わない☆");
            hyoji.AppendLine("do Z*A2         : 持ち駒の ぞう をA2へ打ったあと ky するぜ☆");
            hyoji.AppendLine("                : Z* ぞう打　K* きりん打　H* ひよこ打☆");
            hyoji.AppendLine("do C2C1+        : C2にある駒をC1へ動かしたあと成って ky するぜ☆");
            hyoji.AppendLine("do toryo        : 投了するぜ☆");
#if DEBUG
            hyoji.AppendLine("dump            : 変数をたくさん出力☆");
            hyoji.AppendLine("dump move     : 指し手生成の変数をたくさん出力☆");
#endif

            hyoji.AppendLine("fugo suji       : 筋符号一覧表☆");

            hyoji.AppendLine("go              : コンピューターが１手指したあと ky するぜ☆");
            hyoji.AppendLine("hirate          : 平手初期局面にしたあと ky するぜ☆");
            hyoji.AppendLine("honyaku fen sfen startpos : 翻訳☆ どうぶつしょうぎfenから");
            hyoji.AppendLine("                          : 本将棋fenへ変換☆ 4単語目以降を☆");
            hyoji.AppendLine("hyoka           : 現局面を（読み無しで）形成判断するぜ☆");
            hyoji.AppendLine("hyoka komawari  : 現局面を　駒割りだけで　形成判断するぜ☆");

            hyoji.AppendLine("jokyo           : 現在の内部の状況を表示☆");
            hyoji.AppendLine("kansosen        : 終わった直後の局面データを復活させるぜ☆（＾～＾）");
            hyoji.AppendLine("kifu            : 終わった直後の局面の棋譜を表示するぜ☆");
            hyoji.AppendLine("kifu 10         : 終わった直後の局面の10手目までの図を表示するぜ☆");
            hyoji.AppendLine("kiki            : 味方の駒の利きを一覧するぜ☆");
            hyoji.AppendLine("kiki atk B3     : 現局面の B3 にあるアタック駒の利きを一覧するぜ☆");
            hyoji.AppendLine("                : 旧「kiki B3」");
            hyoji.AppendLine("kiki B3 R 1     : 升と、駒の種類と、手番を指定すると、利きを表示するぜ☆");
            hyoji.AppendLine("kikisu          : 重ね利きの数を一覧するぜ☆");
            hyoji.AppendLine("koma            : 対局者１、２の駒の場所を表示☆");
            hyoji.AppendLine("koma zenbu      : 対局者１、２の駒全部盤だけを表示☆");
            hyoji.AppendLine("koma R          : 対局者１、２の　らいおん　の場所を表示☆");
            hyoji.AppendLine("koma +z         : 対局者１、２の　パワーゾウ　の場所を表示☆ 他同様☆");
            hyoji.AppendLine("ky              : 将棋盤をグラフィカル表示☆ KYokumen");
            hyoji.AppendLine("ky:             : 将棋盤を１行表示☆");
            hyoji.AppendLine("                : fen krz/1h1/1H1/ZRK - 1 0 1");
            hyoji.AppendLine("                : fen 盤上の駒配置 持ち駒の数 手番の対局者 何手目 同形反復の回数");
            hyoji.AppendLine("ky fen krz/1h1/1H1/ZRK - 1 : fen を打ち込んで局面作成☆");
            hyoji.AppendLine("ky fen          : 現局面の fen を表示☆");
            hyoji.AppendLine("ky hanten       : 将棋盤を１８０度回転☆ 反転☆");
            hyoji.AppendLine("ky kesu C4      : C4升に置いてある駒を消すぜ☆");
            hyoji.AppendLine("ky mazeru       : 将棋盤をごちゃごちゃに混ぜるぜ☆ シャッフル☆");
            hyoji.AppendLine("ky motu I 2     : 持ち駒の▲犬を２にするぜ☆");
            hyoji.AppendLine("ky oku K C3     : きりんをC3升に置くぜ☆ 最後に ky tekiyo 必要☆");
            hyoji.AppendLine("ky tekiyo       : 編集した盤面を使えるようにするぜ☆");
            hyoji.AppendLine("man,manual      : これ");
            hyoji.AppendLine("masu 3          : A1,B1升の位置を表す盤を返すぜ☆");
            hyoji.AppendLine("nanamedan atama : ナナメ段の行頭のマス番号（マス毎）☆");
            hyoji.AppendLine("nanamedan d     : ナナメ段の符号（マス毎）☆");
            hyoji.AppendLine("nanamedan haba  : ナナメ段の幅（マス毎）☆");
            hyoji.AppendLine("nanamedan nukidasi: 全マスのナナメ段のビットボード抜出し☆");
            hyoji.AppendLine("nanamedan nukidasi KT: きりん縦☆");
            hyoji.AppendLine("nanamedan nukidasi KY: きりん横☆");
            hyoji.AppendLine("nanamedan nukidasi S: いのしし☆");
            hyoji.AppendLine("nanamedan nukidasi ZHa: ぞう左上がり☆");
            hyoji.AppendLine("nanamedan nukidasi ZHs: ぞう左下がり☆");
            hyoji.AppendLine("nanamedan masu A1: A1マスに紐づくナナメ段情報表示☆");

            hyoji.AppendLine("ojama ha45         : 左肩上がり４５°の盤面表示☆");
            hyoji.AppendLine("ojama hs45         : 左肩下がり４５°の盤面表示☆");
            hyoji.AppendLine("ojama ht90         : 反時計回り９０°の盤面表示☆");

            hyoji.AppendLine("quit            : アプリケーション終了。保存してないものは保存する☆");
            hyoji.AppendLine("rnd             : ランダムに１手指すぜ☆");
            hyoji.AppendLine("move          : 味方の指し手を一覧するぜ☆");
            hyoji.AppendLine("move 1361     : 指し手コード 1361 を翻訳するぜ☆");
            hyoji.AppendLine("move seisei   : 指し手生成のテストだぜ☆");
            hyoji.AppendLine("move su       : 指し手の件数を出力するぜ☆");
            hyoji.AppendLine("set             : 設定を一覧表示するぜ☆");
            hyoji.AppendLine("set BanTateHaba 9       : 盤の縦幅☆");
            hyoji.AppendLine("set BanYokoHaba 9       : 盤の横幅☆");
            hyoji.AppendLine("set JohoJikan 3000      : 読み筋表示を 3000 ミリ秒間隔で行うぜ☆ 負数で表示なし☆");
            hyoji.AppendLine("set Learn true          : 機械学習を行うぜ☆");
            hyoji.AppendLine("set P1Char HyokatiYusen : 対局者１の指し手設定。評価値優先☆");
            hyoji.AppendLine("set P1Char SinteYusen   : 対局者１の指し手設定。新手優先☆");
            hyoji.AppendLine("set P1Char SinteNomi    : 対局者１の指し手設定。新手最優先☆");
            hyoji.AppendLine("set P1Char SyorituYusen : 対局者１の指し手設定。勝率優先☆");
            hyoji.AppendLine("set P1Char SyorituNomi  : 対局者１の指し手設定。勝率最優先☆");
            hyoji.AppendLine("set P1Char TansakuNomi  : 対局者１の指し手設定。探索のみ☆");
            hyoji.AppendLine("set P1Com true          : 対局者１をコンピューターに指させるぜ☆");
            hyoji.AppendLine("set P2Char 略           : P1Char 参照☆");
            hyoji.AppendLine("set P2Com true          : 対局者２をコンピューターに指させるぜ☆");
            hyoji.AppendLine("set P1Name きふわらべ    : 対局者１の表示名を きふわらべ に変更☆");
            hyoji.AppendLine("set P2Name きふわらべ    : 対局者２の表示名を きふわらべ に変更☆");
            hyoji.AppendLine("set RenzokuRandomRule true : 連続対局をランダムにルール変えてやる☆");
            hyoji.AppendLine("set RenzokuTaikyoku true: 強制終了するまで連続対局だぜ☆");
            hyoji.AppendLine("set SagareruHiyoko true : 下がれるひよこモード☆普通のひよこはいなくなる☆");
            hyoji.AppendLine("set SaidaiFukasa 3      : コンピューターの探索深さの最大を3に設定するぜ☆");
            hyoji.AppendLine("set SikoJikan 4000      : コンピューターが一手に思考する時間を 4秒 に設定するぜ☆");
            hyoji.AppendLine("set SikoJikanRandom 1000: 探索毎に 0～0.999秒 の範囲で思考時間を多めに取るぜ☆");
            hyoji.AppendLine("set TobikikiTukau true  : 飛び利きのＯＮ／ＯＦＦ☆");
            hyoji.AppendLine("set UseTimeOver false   : 思考時間の時間切れ判定を無視するぜ☆");
            hyoji.AppendLine("set USI false           : USI通信モードを途中でやめたくなったとき☆");
            hyoji.AppendLine("tantaitest        : 色んなテストを一気にするぜ☆");
            hyoji.AppendLine("taikyokusya       : 手番を表示するんだぜ☆");
            hyoji.AppendLine("test bit-shift    : ビットシフト の動作テスト☆");
            hyoji.AppendLine("test bit-ntz      : ビット演算 NTZ の動作テスト☆");
            hyoji.AppendLine("test bit-kiki     : ビット演算の利きの動作テスト☆");
            hyoji.AppendLine("test bitboard     : 固定ビットボードの確認☆");
            hyoji.AppendLine("test downSizing   : 定跡ファイルの内容を減らすテストだぜ☆");
            hyoji.AppendLine("test ittedume     : 一手詰めの動作テスト☆");
            hyoji.AppendLine("test jisatusyu B3 : B3升に駒を動かすのは自殺手かテスト☆");
            hyoji.AppendLine("test tryrule      : トライルールの動作テスト☆");
            hyoji.AppendLine("tu                : tumeshogi と同じだぜ☆");
            hyoji.AppendLine("tumeshogi         : 詰将棋が用意されるぜ☆");
            hyoji.AppendLine("ugokikata R 0 0 0 : 動き方。[駒][升][長い利き方向][邪魔ブロック]☆");
            hyoji.AppendLine("undo B4B3         : B3にある駒をB4へ動かしたあと ky するぜ☆");
            Logger.Flush(hyoji);
        }

        public void Masu_cmd(string line, IHyojiMojiretu hyoji)
        {
            // うしろに続く文字は☆（＾▽＾）
            int caret = 0;
            Util_String.TobasuTangoToMatubiKuhaku(line, ref caret, "masu ");
            string line2 = line.Substring(caret).Trim();

            // masu 2468 といった数字かどうか☆（＾～＾）
            if (!LisBitboard.TryParse(line2, out Bitboard bitboard))
            {
                hyoji.AppendLine("masuコマンド解析失敗？");
            }
            else
            {
                SpkBan_1Column.Setumei_Bitboard("升の位置", bitboard, hyoji);
                //int masu;
                //while(0!= masuBango && Util_BitEnzan.PopNTZ(ref masuBango, out masu))
                //{

                //}
            }
        }

        /// <summary>
        /// ナナメ段
        /// </summary>
        /// <param name="line"></param>
        /// <param name="hyoji"></param>
        public bool TryFail_Nanamedan(string line, IHyojiMojiretu hyoji)
        {
            // うしろに続く文字は☆（＾▽＾）
            int caret = 0;
            Util_String.TobasuTangoToMatubiKuhaku(line, ref caret, "nanamedan ");

            // ナナメ段の符号
            if (line.Length < caret)
            {

            }
            else if (Util_String.MatchAndNext("atama", line, ref caret))
            {
                TobikikiDirection[] kikiDirs = new TobikikiDirection[]
                {
                    TobikikiDirection.KT,
                    TobikikiDirection.KY,
                    TobikikiDirection.S,
                    TobikikiDirection.ZHa,
                    TobikikiDirection.ZHs,
                };
                string[] headers_beforeRotate = new string[]
                {
                    "Ｂき縦頭",
                    "Ｂき横頭",
                    "Ｂし縦頭",
                    "Ｂぞ左上ナ頭",
                    "Ｂぞ左下ナ頭",
                };
                string[] headers_afterRotate = new string[]
                {
                    "Ａき縦頭",
                    "Ａき横頭",
                    "Ａし縦頭",
                    "Ａぞ左上ナ頭",
                    "Ａぞ左下ナ頭",
                };
                int[] dArray = new int[] {
                    PureSettei.banYokoHaba,
                    PureSettei.banTateHaba,
                    PureSettei.banYokoHaba,
                    PureSettei.banNanameDanLen,
                    PureSettei.banNanameDanLen,
                };
                // Before
                for (int iKikiDir = 0; iKikiDir < kikiDirs.Length; iKikiDir++)
                {
                    TobikikiDirection kikiDir = kikiDirs[iKikiDir];
                    hyoji.Append(headers_beforeRotate[iKikiDir]);
                    hyoji.Append(" ");
                    for (int iD = 0; iD < dArray[iKikiDir]; iD++)
                    {
                        hyoji.Append((int)Util_Tobikiki.GetNanameDanAtama_ReverseRotateChikanhyo(iD, kikiDir));
                        hyoji.Append(" ");
                    }
                    hyoji.AppendLine();
                }
                // After
                for (int iKikiDir = 0; iKikiDir < kikiDirs.Length; iKikiDir++)
                {
                    TobikikiDirection kikiDir = kikiDirs[iKikiDir];
                    hyoji.Append(headers_afterRotate[iKikiDir]);
                    hyoji.Append(" ");
                    for (int iD = 0; iD < dArray[iKikiDir]; iD++)
                    {
                        hyoji.Append((int)Util_Tobikiki.GetNanameDanAtama_NoRotateMotohyo(iD, kikiDir));
                        hyoji.Append(" ");
                    }
                    hyoji.AppendLine();
                }
            }
            else if (Util_String.MatchAndNext("d", line, ref caret))//diagonals
            {
                TobikikiDirection[] kikiDirs = new TobikikiDirection[]
                {
                    TobikikiDirection.KT,
                    TobikikiDirection.KY,
                    TobikikiDirection.S,
                    TobikikiDirection.ZHa,
                    TobikikiDirection.ZHs,
                };
                string[] headers = new string[]
                {
                    "き縦段",
                    "き横段",
                    "し縦段",
                    "ぞ左上ナ段",
                    "ぞ左下ナ段",
                };
                bool[] yokoTateHantenHairetu = new bool[] {
                    true,
                    false,
                    true,
                    false,
                    false,
                };

                for (int iKikiDir = 0; iKikiDir < kikiDirs.Length; iKikiDir++)
                {
                    TobikikiDirection kikiDir = kikiDirs[iKikiDir];
                    string name = headers[iKikiDir];

                    int[][] numberHyoHairetu = new int[1][];
                    numberHyoHairetu[0] = new int[PureSettei.banHeimen];
                    for (int iMs = 0; iMs < PureSettei.banHeimen; iMs++)
                    {
                        numberHyoHairetu[0][iMs] = Util_Tobikiki.GetNanameDan((Masu)iMs, kikiDir);
                    }

                    SpkBan_MultiColumn.Setumei_Masutbl(
                        new string[] { name },
                        numberHyoHairetu,
                        yokoTateHantenHairetu[iKikiDir],
                        hyoji
                        );
                }
            }
            else if (Util_String.MatchAndNext("haba", line, ref caret))
            {
                TobikikiDirection[] kikiDirs = new TobikikiDirection[]
                {
                    TobikikiDirection.KT,
                    TobikikiDirection.KY,
                    TobikikiDirection.S,
                    TobikikiDirection.ZHa,
                    TobikikiDirection.ZHs,
                };
                int[] dArray = new int[] {
                    PureSettei.banYokoHaba,
                    PureSettei.banTateHaba,
                    PureSettei.banYokoHaba,
                    PureSettei.banNanameDanLen,
                    PureSettei.banNanameDanLen,
                };
                string[] headers = new string[]
                {
                    "き縦幅",
                    "き横幅",
                    "し縦幅",
                    "ぞ左上ナ幅",
                    "ぞ左下ナ幅",
                };
                for (int iKikiDir = 0; iKikiDir < kikiDirs.Length; iKikiDir++)
                {
                    TobikikiDirection kikiDir = kikiDirs[iKikiDir];
                    string name = headers[iKikiDir];
                    hyoji.Append(name);
                    hyoji.Append(" ");
                    for (int iD = 0; iD < dArray[iKikiDir]; iD++)
                    {
                        hyoji.Append(Util_Tobikiki.GetNanameDanHaba(iD, kikiDir));
                        hyoji.Append(" ");
                    }
                    hyoji.AppendLine();
                }
            }
            // 情報表示
            else if (Util_String.MatchAndNext("masu", line, ref caret))
            {
                Masu ms1;
                if (!LisMasu.MatchMasu(line, ref caret, out ms1
#if DEBUG
                        , (IDebugMojiretu)hyoji
#endif
                    ))
                {
                    return Pure.FailTrue(string.Format("nanamedan masu103 パースエラー103 line=[{0}] caret={1}", line, caret));
                }

                TobikikiDirection[] kikiDirs = new TobikikiDirection[]
                {
                    TobikikiDirection.KT,
                    TobikikiDirection.KY,
                    TobikikiDirection.S,
                    TobikikiDirection.ZHa,
                    TobikikiDirection.ZHs,
                };
                int[] masuSpans = new int[]
                {
                    Util_Tobikiki.MasSpanKT,
                    Util_Tobikiki.MasSpanKY,
                    Util_Tobikiki.MasSpanKT,
                    Util_Tobikiki.MasSpanZHa,
                    Util_Tobikiki.MasSpanZHs,
                };
                bool[] sakasa_forZHs = new bool[]
                {
                    false,
                    false,
                    false,
                    false,
                    true
                };
                string[] headers = new string[]
                {
                    "き縦頭",
                    "き横頭",
                    "し縦頭",
                    "ぞ左上ナ頭",
                    "ぞ左下ナ頭",
                };
                for (int iKikiDir = 0; iKikiDir < kikiDirs.Length; iKikiDir++)
                {
                    TobikikiDirection kikiDir = kikiDirs[iKikiDir];
                    int nanamedanD;
                    Masu atama_reverseRotateChikanhyo;
                    Masu atama_noRotateMotohyo;
                    Masu siri_noRotateMotohyo;
                    int haba;
                    Util_Tobikiki.FromMasu(kikiDir, masuSpans[iKikiDir], sakasa_forZHs[iKikiDir],
                        ms1, out nanamedanD,
                        out atama_reverseRotateChikanhyo,
                        out atama_noRotateMotohyo,
                        out siri_noRotateMotohyo,
                        out haba);

                    hyoji.AppendLine(string.Format("{0} d={1} atama={2} haba={3} ", headers[iKikiDir], nanamedanD, atama_reverseRotateChikanhyo, haba));
                }
            }

            return Pure.SUCCESSFUL_FALSE;
        }

        /// <summary>
        /// 二進数
        /// </summary>
        /// <param name="line"></param>
        /// <param name="hyoji"></param>
        public bool TryFail_Nisinsu(string line, IHyojiMojiretu hyoji)
        {
            // うしろに続く文字は☆（＾▽＾）
            int caret = 0;
            Util_String.TobasuTangoToMatubiKuhaku(line, ref caret, "nisinsu");

            int number;
            if (LisInt.MatchInt(line, ref caret, out number))
            {
                return Pure.FailTrue("TryFail_ParseInt");
            }

            hyoji.AppendLine(string.Format("二進数:{0}", LisHyoji.ToNisinsu((ulong)number)));

            return Pure.SUCCESSFUL_FALSE;
        }

        /// <summary>
        /// お邪魔駒の配置をビットボードで表示するもの
        /// </summary>
        /// <param name="line"></param>
        /// <param name="hyoji"></param>
        public bool TryFail_Ojama(string line, IHyojiMojiretu hyoji)
        {
            // うしろに続く文字は☆（＾▽＾）
            int caret = 0;
            Util_String.TobasuTangoToMatubiKuhaku(line, ref caret, "ojama ");

            // 左肩上がり    の置換表
            if (Util_String.MatchAndNext("ha45", line, ref caret))
            {
                // hidarikata agari 45 の略
#if DEBUG
                SpkBan_Hisigata.Setumei( TobikikiDirection.ZHa, hyoji);
#else
                SpkBan_1Column.Setumei_Kyokumen(PureMemory.kifu_endTeme, OjamaBanSyurui.Ha45, hyoji);
#endif
                return Pure.SUCCESSFUL_FALSE;
            }
            // お邪魔ハッシュキーの表示
            else if (Util_String.MatchAndNext("ojhsh", line, ref caret))
            {
                if (Util_String.MatchAndNext("KT", line, ref caret))
                {
                    Masu ms;
                    if (Itiran_FenParser.MatchSrcMs(line, ref caret, out ms
#if DEBUG
                    , (IDebugMojiretu)hyoji
#endif
                        ))
                    {
                        // 升で絞り込み
                        HyojiOjamaHsh("きりん縦", TobikikiDirection.KT, ms, hyoji
#if DEBUG
                            , false
#endif
                        );
                    }
                    else
                    {
                        // 全マス
                        HyojiOjamaHsh("きりん縦", TobikikiDirection.KT, hyoji
#if DEBUG
                            , false
#endif
                        );
                    }
                }
                else if (Util_String.MatchAndNext("KY", line, ref caret))
                {
                    Masu ms;
                    if (Itiran_FenParser.MatchSrcMs(line, ref caret, out ms
#if DEBUG
                    , (IDebugMojiretu)hyoji
#endif
                        ))
                    {
                        // 升で絞り込み
                        HyojiOjamaHsh("きりん横", TobikikiDirection.KY, ms, hyoji
#if DEBUG
                            , false
#endif
                        );
                    }
                    else
                    {
                        // 全マス
                        HyojiOjamaHsh("きりん横", TobikikiDirection.KY, hyoji
#if DEBUG
                            , false
#endif
                        );
                    }
                }
                else if (Util_String.MatchAndNext("S", line, ref caret))
                {
                    Masu ms;
                    if (Itiran_FenParser.MatchSrcMs(line, ref caret, out ms
#if DEBUG
                    , (IDebugMojiretu)hyoji
#endif
                        ))
                    {
                        // 升で絞り込み
                        HyojiOjamaHsh("いのしし縦", TobikikiDirection.S, ms, hyoji
#if DEBUG
                            , false
#endif
                        );
                    }
                    else
                    {
                        // 全マス
                        HyojiOjamaHsh("いのしし縦", TobikikiDirection.S, hyoji
#if DEBUG
                            , false
#endif
                        );
                    }
                }
                else if (Util_String.MatchAndNext("ZHa", line, ref caret))
                {
                    Masu ms;
                    if (Itiran_FenParser.MatchSrcMs(line, ref caret, out ms
#if DEBUG
                    , (IDebugMojiretu)hyoji
#endif
                        ))
                    {
                        // 升で絞り込み
                        HyojiOjamaHsh("ぞう左上がり", TobikikiDirection.ZHa, ms, hyoji
#if DEBUG
                            , false
#endif
                        );
                    }
                    else
                    {
                        // 全マス
                        HyojiOjamaHsh("ぞう左上がり", TobikikiDirection.ZHa, hyoji
#if DEBUG
                            , false
#endif
                        );
                    }
                }
                else if (Util_String.MatchAndNext("ZHs", line, ref caret))
                {
                    Masu ms;
                    if (Itiran_FenParser.MatchSrcMs(line, ref caret, out ms
#if DEBUG
                    , (IDebugMojiretu)hyoji
#endif
                        ))
                    {
                        // 升で絞り込み
                        HyojiOjamaHsh("ぞう左下がり", TobikikiDirection.ZHs, ms, hyoji
#if DEBUG
                            , false
#endif
                        );
                    }
                    else
                    {
                        // 全マス
                        HyojiOjamaHsh("ぞう左下がり", TobikikiDirection.ZHs, hyoji
#if DEBUG
                            , false
#endif
                        );
                    }
                }

                return Pure.SUCCESSFUL_FALSE;
            }
            // 左肩下がり    の置換表
            else if (Util_String.MatchAndNext("hs45", line, ref caret))
            {
                // hidarikata sagari 45 の略
#if DEBUG
                SpkBan_Hisigata.Setumei( TobikikiDirection.ZHs, hyoji);
#else
                SpkBan_1Column.Setumei_Kyokumen(PureMemory.kifu_endTeme, OjamaBanSyurui.Hs45, hyoji);
#endif
                return Pure.SUCCESSFUL_FALSE;
            }
            // ９０°回転    の置換表
            else if (Util_String.MatchAndNext("ht90", line, ref caret))
            {
                // han tokei mawari 90 の略
#if DEBUG
                SpkBan_Hisigata.Setumei( TobikikiDirection.KT, hyoji);
#else
                SpkBan_1Column.Setumei_Kyokumen(PureMemory.kifu_endTeme, OjamaBanSyurui.Ht90, hyoji);
#endif
                return Pure.SUCCESSFUL_FALSE;
            }
            // 横型
            else if (Util_String.MatchAndNext("yk00", line, ref caret))
            {
                // yoko 90 の略
#if DEBUG
                SpkBan_Hisigata.Setumei( TobikikiDirection.KY, hyoji);
#else
                //SpkBan_1Column.Setumei_Kyokumen(gky.yomiGky, OjamaBanSyurui.None, hyoji);
#endif
                return Pure.SUCCESSFUL_FALSE;
            }

            return Pure.SUCCESSFUL_FALSE;
        }
        /// <summary>
        /// 升から「お邪魔駒配置ハッシュキー」盤面表示
        /// </summary>
        /// <param name="header"></param>
        /// <param name="kikiDir"></param>
        /// <param name="ms"></param>
        /// <param name="ky2"></param>
        /// <param name="hyoji"></param>
        /// <param name="dbg_hisigata"></param>
        static void HyojiOjamaHsh(
            string header,
            TobikikiDirection kikiDir,
            Masu ms,
            IHyojiMojiretu hyoji
#if DEBUG
            , bool dbg_hisigata
#endif
            )
        {
            hyoji.Append(string.Format("({0}{1}) ", header, (int)ms));
            Bitboard bb = Util_Tobikiki.GetOjhsh(kikiDir, ms);

            {
#if DEBUG
                // 指定「利きの方向」
                hyoji.Append(string.Format("kikiDir={0} ",
                    kikiDir
                    ));
                hyoji.Append(string.Format("msSpan={0} sakasa_forZHs={1} ",
                    PureMemory.ssssDbg_masuSpan, PureMemory.ssssDbg_sakasa_forZHs
                    ));
                hyoji.Append(string.Format("d={0} atama(dst={1} ms={2}) siriMs={3} ",
                    PureMemory.ssssDbg_nanamedanD,
                    PureMemory.ssssDbg_atama_reverseRotateChikanhyo,
                    PureMemory.ssssDbg_atama_noRotateMotohyo,
                    PureMemory.ssssDbg_siri_noRotateMotohyo
                    ));

#if DEBUG
                if (dbg_hisigata)
                {
                    hyoji.AppendLine("ojama ");
                    SpkBan_Hisigata.ScanAndHyojiHisigata((Masu ms2) =>
                    {
                        return PureMemory.ssssDbg_bb_ojamaTai.IsOn(ms2) ? "〇" : "・";
                    },
                    kikiDir, hyoji);
                }
                else
                {
#endif
                    // 元の盤
                    hyoji.Append(string.Format("ojama(Big={0}, Small={1}) ",
                        LisHyoji.ToNisinsu(PureMemory.ssssDbg_bb_ojamaTai.value64127),
                        LisHyoji.ToNisinsu(PureMemory.ssssDbg_bb_ojamaTai.value063)
                        ));
#if DEBUG
                }
#endif

                // 右シフト
                hyoji.Append(string.Format("rsft(val={0} Big={1}, Small={2}) ",
                    PureMemory.ssssDbg_rightShift,
                    LisHyoji.ToNisinsu(PureMemory.ssssDbg_bb_rightShifted.value64127),
                    LisHyoji.ToNisinsu(PureMemory.ssssDbg_bb_rightShifted.value063)
                    ));
                // マスク
                hyoji.Append(string.Format("haba={0} msk(Big={1} Small={2}) ",
                    PureMemory.ssssDbg_haba,
                    LisHyoji.ToNisinsu(PureMemory.ssssDbg_bb_mask.value64127),
                    LisHyoji.ToNisinsu(PureMemory.ssssDbg_bb_mask.value063)
                    ));
#endif
                hyoji.Append(string.Format("bb(Big={0} Small={1}) ",
                    LisHyoji.ToNisinsu(bb.value64127),
                    LisHyoji.ToNisinsu(bb.value063)
                    ));
            }


            StringBuilder sb2 = new StringBuilder();
            for (int iMs2 = 0; iMs2 < PureSettei.banHeimen; iMs2++)
            {
                sb2.Append(bb.IsOn((Masu)iMs2) ? "〇" : " ");
            }
            hyoji.AppendLine(sb2.ToString().TrimEnd().Replace(" ", "・"));
        }
        /// <summary>
        /// 升から「お邪魔駒配置ハッシュキー」盤面表示
        /// 全マス
        /// </summary>
        /// <param name="header"></param>
        /// <param name="kikiDir"></param>
        /// <param name="ky2"></param>
        /// <param name="hyoji"></param>
        /// <param name="dbg_hisigata"></param>
        static void HyojiOjamaHsh(
            string header,
            TobikikiDirection kikiDir,
            IHyojiMojiretu hyoji
#if DEBUG
            , bool dbg_hisigata
#endif
            )
        {
            for (int iMs = 0; iMs < PureSettei.banHeimen; iMs++)
            {
                HyojiOjamaHsh(
                    header,
                    kikiDir,
                    (Masu)iMs,
                    hyoji
#if DEBUG
                        , dbg_hisigata
#endif
                        );
            }
        }

        /// <summary>
        /// 初期局面と、そこからの棋譜をセットするものだぜ☆（＾～＾）
        /// </summary>
        /// <param name="f"></param>
        /// <param name="line"></param>
        /// <param name="hyoji"></param>
        public void Position(FenSyurui f, string line, IHyojiMojiretu hyoji)
        {
            int caret = 0;
            if (Util_String.MatchAndNext("position", line, ref caret))
            {
                // 初期局面を変更するぜ☆！（＾▽＾）
                if (LisGenkyoku.TryFail_MatchPositionvalue(
                    f,
                    line, ref caret, out string moves
#if DEBUG
                    , (IDebugMojiretu)hyoji
#endif
                    ))
                {
                    hyoji.AppendLine("パースに失敗だぜ☆（＾～＾）！ #黒牛");
                    // 例外で出力したいので、フラッシュされる前に退避だぜ☆（＾～＾）
                    string msg = hyoji.ToContents();
                    Logger.Flush(hyoji);
                    throw new Exception(msg);
                }


                if ("" != moves)
                {
                    // moves が続いていたら☆（＾～＾）

                    // 頭の moves を取り除くぜ☆（*＾～＾*）
                    moves = moves.Substring("moves ".Length);
#if DEBUG
                    hyoji.AppendLine(string.Format("棋譜再生 moves={0}",
                        moves
                        ));
#endif

                    // 「手目」が最後まで進んでしまうぜ☆（＾～＾）
                    MoveGenAccessor.Tukurinaosi_RemakeKifuByMoves(moves);

                    // 棋譜の通り指すぜ☆（＾～＾）
                    if (!MoveGenAccessor.Try_PlayMoves_0ToPreTeme(f, hyoji))
                    {
                        Logger.Flush(hyoji);
                        throw new Exception(hyoji.ToContents());
                    }
                }

                // 初回は「position startpos」しか送られてこない☆（＾～＾）
            }
        }

        public  void PreGo(string line, IHyojiMojiretu hyoji)
        {
            Util_Tansaku.PreGo();
        }

        public void Result(IHyojiMojiretu hyoji, CommandMode commandMode)
        {
            switch (commandMode)
            {
                case CommandMode.NingenYoConsoleGame:
                    {
                        switch (PureMemory.gky_kekka)
                        {
                            case TaikyokuKekka.Hikiwake:
                                {
                                    hyoji.AppendLine("┌─────────────────結　果─────────────────┐");
                                    hyoji.AppendLine("│　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　│");
                                    hyoji.AppendLine("│　　　　　　　　　　　　　　　　 引き分け 　　　　　　　　　　　　　　　　　│");
                                    hyoji.AppendLine("│　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　│");
                                    hyoji.AppendLine("└─────────────────────────────────────┘");
                                }
                                break;
                            case TaikyokuKekka.Taikyokusya1NoKati:
                                {
                                    hyoji.AppendLine("┌─────────────────結　果─────────────────┐");
                                    hyoji.AppendLine("│　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　│");
                                    hyoji.AppendLine("│　　　　　　　　　　　　　　　対局者１の勝ち　　　　　　　　　　　　　　　│");
                                    hyoji.AppendLine("│　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　│");
                                    hyoji.AppendLine("└─────────────────────────────────────┘");
                                }
                                break;
                            case TaikyokuKekka.Taikyokusya2NoKati:
                                {
                                    hyoji.AppendLine("┌─────────────────結　果─────────────────┐");
                                    hyoji.AppendLine("│　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　│");
                                    hyoji.AppendLine("│　　　　　　　　　　　　　　　対局者２の勝ち　　　　　　　　　　　　　　　│");
                                    hyoji.AppendLine("│　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　│");
                                    hyoji.AppendLine("└─────────────────────────────────────┘");
                                }
                                break;
                            case TaikyokuKekka.Karappo://thru
                            default:
                                break;
                        }
                    }
                    break;
                case CommandMode.TusinYo:
                    {
                        // 列挙型をそのまま出力するぜ☆（＾▽＾）
                        hyoji.Append("< result, kekka = ");
                        hyoji.AppendLine(PureMemory.gky_kekka.ToString());
                    }
                    break;
                default://thru
                case CommandMode.NigenYoConsoleKaihatu:
                    {
                        switch (PureMemory.gky_kekka)
                        {
                            case TaikyokuKekka.Hikiwake: hyoji.AppendLine("結果：　引き分け"); break;
                            case TaikyokuKekka.Taikyokusya1NoKati: hyoji.AppendLine("結果：　対局者１の勝ち"); break;
                            case TaikyokuKekka.Taikyokusya2NoKati: hyoji.AppendLine("結果：　対局者２の勝ち"); break;
                            case TaikyokuKekka.Karappo://thru
                            default:
                                break;
                        }
                    }
                    break;
            }
        }

        public bool Try_Rnd(
#if DEBUG
            IDebugMojiretu reigai1
#endif
            )
        {
            if (!IkkyokuOpe.Try_Rnd(
#if DEBUG
                reigai1
#endif
                ))
            {
                return false;
            }
            return true;
        }

        public bool TryFail_Move_cmd(string line, IHyojiMojiretu hyoji)
        {
            int caret = 0;
            if (Util_String.MatchAndNext("move", line, ref caret))
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
                            this.TryFail_Move_cmd("move seisei hkt", hyoji);
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

        public void Setoption(string line, IHyojiMojiretu hyoji)
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

                this.Set(sb.ToString(), hyoji);
            }
        }

        public void Set(string line, IHyojiMojiretu hyoji)
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
