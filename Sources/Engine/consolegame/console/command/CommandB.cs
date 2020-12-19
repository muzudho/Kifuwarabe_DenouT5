#if DEBUG
using kifuwarabe_shogithink.pure;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.ky.bb;
using kifuwarabe_shogithink.pure.ky.tobikiki;
using kifuwarabe_shogithink.pure.listen;
using kifuwarabe_shogithink.pure.listen.ky;
using kifuwarabe_shogithink.pure.logger;
using kifuwarabe_shogithink.pure.speak.ky;
using kifuwarabe_shogithink.pure.speak.ky.bb;
using kifuwarabe_shogiwin.speak.ban;
using System;
#else
using kifuwarabe_shogithink.pure.control;
using kifuwarabe_shogithink.pure.speak.ky;
using kifuwarabe_shogithink.pure;
using kifuwarabe_shogithink.pure.genkyoku;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.ky.bb;
using kifuwarabe_shogithink.pure.ky.tobikiki;
using kifuwarabe_shogithink.pure.listen;
using kifuwarabe_shogithink.pure.listen.ky;
using kifuwarabe_shogithink.pure.logger;
using kifuwarabe_shogiwin.speak.ban;
using System;
#endif

namespace kifuwarabe_shogiwin.consolegame.console.command
{
    public static class CommandB
    {
        /// <summary>
        /// ビットボードのテスト用だぜ☆（*＾～＾*）
        /// </summary>
        /// <param name="line">コマンドライン</param>
        public static bool TryFail_Bitboard(
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
                for (int iTai=0; iTai<Conv_Taikyokusya.itiran.Length; iTai++)
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

                if (-1<exp && exp<BitboardsOmatome.maskHyo.Length)
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
    }
}
