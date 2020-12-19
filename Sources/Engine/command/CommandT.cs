#if DEBUG
using kifuwarabe_shogithink.pure;
using kifuwarabe_shogithink.pure.com;
using kifuwarabe_shogithink.pure.com.moveorder.hioute;
using kifuwarabe_shogithink.pure.control;
using kifuwarabe_shogithink.pure.project;
using kifuwarabe_shogithink.pure.genkyoku;
using kifuwarabe_shogithink.pure.ikkyoku;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.ky.bb;
using kifuwarabe_shogithink.pure.listen;
using kifuwarabe_shogithink.pure.listen.genkyoku;
using kifuwarabe_shogithink.pure.listen.ky;
using kifuwarabe_shogithink.pure.logger;
using kifuwarabe_shogithink.pure.speak.ky;
using kifuwarabe_shogithink.pure.speak.ky.bb;
using kifuwarabe_shogiwin.consolegame.gakusyu;
using kifuwarabe_shogiwin.consolegame.machine;
using kifuwarabe_shogiwin.consolegame.tumeshogi;
using kifuwarabe_shogiwin.speak.ban;
using System;
using System.Collections.Generic;
#else
using kifuwarabe_shogithink.pure.listen.genkyoku;
using System.Collections.Generic;
using kifuwarabe_shogithink.pure.accessor;
using kifuwarabe_shogithink.pure;
using kifuwarabe_shogithink.pure.com;
using kifuwarabe_shogithink.pure.com.MoveOrder.hioute;
using kifuwarabe_shogithink.pure.control;
using kifuwarabe_shogithink.pure.genkyoku;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.ky.bb;
using kifuwarabe_shogithink.pure.listen;
using kifuwarabe_shogithink.pure.listen.ky;
using kifuwarabe_shogithink.pure.logger;
using kifuwarabe_shogithink.pure.speak.ky;
using kifuwarabe_shogithink.pure.speak.ky.bb;
using kifuwarabe_shogiwin.consolegame.gakusyu;
using kifuwarabe_shogiwin.consolegame.machine;
using kifuwarabe_shogiwin.consolegame.tumeshogi;
using kifuwarabe_shogiwin.speak.ban;
using System;
using Grayscale.Kifuwarabi.Entities.Logging;
#endif

namespace kifuwarabe_shogiwin.consolegame.console.command
{
    public static class CommandT
    {
        /// <summary>
        /// Taikyokusyaクラスと名前が被ると面倒なんで _cmd を付けているぜ☆（＾～＾）
        /// </summary>
        /// <param name="line"></param>
        /// <param name="hyoji"></param>
        public static void Taikyokusya_cmd(string line, IHyojiMojiretu hyoji)
        {
            int caret = 0;
            if (Util_String.MatchAndNext("taikyokusya", line, ref caret))
            {
                SpkTaikyokusya.AppendSetumeiName(PureMemory.kifu_teban, hyoji);
                hyoji.AppendLine();
                return;
            }
            else
            {
            }
        }

        public static void Tansaku(string line, IHyojiMojiretu hyoji)
        {
            int caret = 0;
            if (Util_String.MatchAndNext("tansaku", line, ref caret))
            {
                hyoji.AppendLine(string.Format("tansaku: kaisiTeme={0} kaisiTaikyokusya={1}",
                    PureMemory.tnsk_kaisiTeme,
                    PureMemory.tnsk_kaisiTaikyokusya
                    ));
                return;
            }
            else
            {
            }
        }

        public static bool TryFail_Test(string line, IHyojiMojiretu hyoji)
        {
            if (line == "test")
            {
                return Pure.SUCCESSFUL_FALSE;
            }

            // うしろに続く文字は☆（＾▽＾）
            int caret = 0;
            Util_String.TobasuTangoToMatubiKuhaku(line, ref caret, "test ");

            if (Util_String.MatchAndNext("bit-shift", line, ref caret))
            {
                //────────────────────────────────────────
                // ビット演算のテスト
                //────────────────────────────────────────
                #region ビット演算のテスト☆
                Bitboard bb = new Bitboard();
                int i = 0; bb.Set(0UL); SpkBan_1Column.Setumei_Bitboard(i.ToString(), bb, hyoji);
                i = 1; bb.Set(1UL); SpkBan_1Column.Setumei_Bitboard(i.ToString(), bb, hyoji);
                for (i = 2; i < 131; i++) // 128より少し多め
                {
                    bb.LeftShift(1); SpkBan_1Column.Setumei_Bitboard(i.ToString(), bb, hyoji);
                }

                // 逆回転
                bb.Standup((Masu)127);
                for (i = 126; -1 < i; i--)
                {
                    bb.RightShift(1); SpkBan_1Column.Setumei_Bitboard(i.ToString(), bb, hyoji);
                }

                Logger.Flush(hyoji);
                #endregion
            }
            // 旧「test bit-ntz」→「test ntz」
            else if (Util_String.MatchAndNext("ntz", line, ref caret))
            {
                //────────────────────────────────────────
                // ビット演算のテスト
                //────────────────────────────────────────
                #region ビット演算のテスト☆
                Bitboard x = new Bitboard();
                Masu ntz;
                x.Set(0); x.GetNTZ(out ntz); hyoji.AppendLine("    0  0000 0000 0000 0000 の NTZ =[" + ntz + "]");
                x.Set(1); x.GetNTZ(out ntz); hyoji.AppendLine("    1  0000 0000 0000 0001 の NTZ =[" + ntz + "]");
                x.Set(2); x.GetNTZ(out ntz); hyoji.AppendLine("    2  0000 0000 0000 0010 の NTZ =[" + ntz + "]");
                x.Set(4); x.GetNTZ(out ntz); hyoji.AppendLine("    4  0000 0000 0000 0100 の NTZ =[" + ntz + "]");
                x.Set(8); x.GetNTZ(out ntz); hyoji.AppendLine("    8  0000 0000 0000 1000 の NTZ =[" + ntz + "]");
                x.Set(16); x.GetNTZ(out ntz); hyoji.AppendLine("   16  0000 0000 0001 0000 の NTZ =[" + ntz + "]");
                x.Set(32); x.GetNTZ(out ntz); hyoji.AppendLine("   32  0000 0000 0010 0000 の NTZ =[" + ntz + "]");
                x.Set(64); x.GetNTZ(out ntz); hyoji.AppendLine("   64  0000 0000 0100 0000 の NTZ =[" + ntz + "]");
                x.Set(128); x.GetNTZ(out ntz); hyoji.AppendLine("  128  0000 0000 1000 0000 の NTZ =[" + ntz + "]");
                x.Set(256); x.GetNTZ(out ntz); hyoji.AppendLine("  256  0000 0001 0000 0000 の NTZ =[" + ntz + "]");
                x.Set(512); x.GetNTZ(out ntz); hyoji.AppendLine("  512  0000 0010 0000 0000 の NTZ =[" + ntz + "]");
                x.Set(1024); x.GetNTZ(out ntz); hyoji.AppendLine(" 1024  0000 0100 0000 0000 の NTZ =[" + ntz + "]");
                x.Set(2048); x.GetNTZ(out ntz); hyoji.AppendLine(" 2048  0000 1000 0000 0000 の NTZ =[" + ntz + "]");
                x.Set(4096); x.GetNTZ(out ntz); hyoji.AppendLine(" 4096  0001 0000 0000 0000 の NTZ =[" + ntz + "]");
                x.Set(8192); x.GetNTZ(out ntz); hyoji.AppendLine(" 8192  0010 0000 0000 0000 の NTZ =[" + ntz + "]");
                x.Set(16384); x.GetNTZ(out ntz); hyoji.AppendLine("16384  0100 0000 0000 0000 の NTZ =[" + ntz + "]");
                x.Set(32768); x.GetNTZ(out ntz); hyoji.AppendLine("32768  1000 0000 0000 0000 の NTZ =[" + ntz + "]");
                //for (int i = 17; i < 67; i++) // [63]以降は 64 のようだぜ☆（＾▽＾）
                for (int i = 17; i < 131; i++) // [63]以降は 64 のようだぜ☆（＾▽＾）
                {
                    x.LeftShift(1);
                    x.GetNTZ(out ntz);
                    hyoji.AppendLine("(" + i + ")                         NTZ =[" + ntz + "] Contents=[" + x.ToContents() + "]");
                }
                Logger.Flush(hyoji);
                #endregion
            }
            else if (Util_String.MatchAndNext("bit-kiki", line, ref caret))
            {
                //────────────────────────────────────────
                // ビット演算のテスト（利き）
                //────────────────────────────────────────
                #region ビット演算のテスト（利き）☆
                Bitboard maskBB = new Bitboard();//使いまわす
                for (int iKs = 0; iKs < Conv_Komasyurui.itiran.Length; iKs++)
                {
                    //if ((Komasyurui)iKs!=Komasyurui.N)
                    //{
                    //    continue;
                    //}
                    // 例
                    // らいおん
                    //     先手
                    //         [ 0] [ 1] [ 2] [ 3] [ 4] [ 5] [ 6] [ 7] [ 8] [ 9] [10] [11]
                    //          000  000  000  000  000  000  000  000  000  000  000  000
                    //          000  000  000  000  000  000  000  000  000  000  000  000
                    //          000  000  000  000  000  000  000  000  000  000  000  000
                    //          000  000  000  000  000  000  000  000  000  000  000  000
                    hyoji.Append(Med_Koma.GetKomasyuruiNamae(Taikyokusya.T1, (Komasyurui)iKs));
                    //Conv_Komasyurui.GetNamae((Komasyurui)iKs, syuturyoku);
                    hyoji.AppendLine();
                    for (int iTb = 0; iTb < Conv_Taikyokusya.itiran.Length; iTb++)
                    {
                        hyoji.Append("    ");
                        SpkTaikyokusya.AppendSetumeiName((Taikyokusya)iTb, hyoji);
                        hyoji.AppendLine();
                        hyoji.AppendLine("        [ 0] [ 1] [ 2] [ 3] [ 4] [ 5] [ 6] [ 7] [ 8] [ 9] [10] [11]");

                        // 0～2
                        hyoji.Append("         ");
                        for (int iMs = 0; iMs < (int)Conv_Masu.masu_error; iMs++)
                        {
                            maskBB.Set(0x01);// 0x01,0x02,0x04
                            for (int iShift = 0; iShift < 3; iShift++)
                            {
                                Koma km = Med_Koma.KomasyuruiAndTaikyokusyaToKoma((Komasyurui)iKs, (Taikyokusya)iTb);
                                SpkBitboard.AppendSyuturyokuTo(BitboardsOmatome.KomanoUgokikataYk00.Clone_Merge(km, (Masu)iMs).Siborikomi(maskBB).RightShift(iShift), hyoji);
                                maskBB.LeftShift(1);
                            }

                            if (iMs + 1 < (int)Conv_Masu.masu_error)
                            {
                                hyoji.Append("  ");
                            }
                            else
                            {
                                hyoji.AppendLine();
                            }
                        }
                        // 3～5
                        hyoji.Append("         ");
                        for (int iMs = 0; iMs < (int)Conv_Masu.masu_error; iMs++)
                        {
                            maskBB.Set(0x08);// 0x08,0x10,0x20
                            for (int iShift = 3; iShift < 6; iShift++)
                            {
                                Koma km = Med_Koma.KomasyuruiAndTaikyokusyaToKoma((Komasyurui)iKs, (Taikyokusya)iTb);
                                SpkBitboard.AppendSyuturyokuTo(BitboardsOmatome.KomanoUgokikataYk00.Clone_Merge(km, (Masu)iMs).Siborikomi(maskBB).RightShift(iShift), hyoji);
                                maskBB.LeftShift(1);
                            }

                            if (iMs + 1 < (int)Conv_Masu.masu_error)
                            {
                                hyoji.Append("  ");
                            }
                            else
                            {
                                hyoji.AppendLine();
                            }
                        }
                        // 6～8
                        hyoji.Append("         ");
                        for (int iMs = 0; iMs < (int)Conv_Masu.masu_error; iMs++)
                        {
                            maskBB.Set(0x40);// 0x40,0x80,0x100
                            for (int iShift = 6; iShift < 9; iShift++)
                            {
                                Koma km = Med_Koma.KomasyuruiAndTaikyokusyaToKoma((Komasyurui)iKs, (Taikyokusya)iTb);
                                SpkBitboard.AppendSyuturyokuTo(BitboardsOmatome.KomanoUgokikataYk00.Clone_Merge(km, (Masu)iMs).Siborikomi(maskBB).RightShift(iShift), hyoji);
                                maskBB.LeftShift(1);
                            }

                            if (iMs + 1 < (int)Conv_Masu.masu_error)
                            {
                                hyoji.Append("  ");
                            }
                            else
                            {
                                hyoji.AppendLine();
                            }
                        }
                        // 9～11
                        hyoji.Append("         ");
                        for (int iMs = 0; iMs < (int)Conv_Masu.masu_error; iMs++)
                        {
                            maskBB.Set(0x200);// 0x200,0x400,0x800
                            for (int iShift = 9; iShift < 12; iShift++)
                            {
                                Koma km = Med_Koma.KomasyuruiAndTaikyokusyaToKoma((Komasyurui)iKs, (Taikyokusya)iTb);
                                SpkBitboard.AppendSyuturyokuTo(BitboardsOmatome.KomanoUgokikataYk00.Clone_Merge(km, (Masu)iMs).Siborikomi(maskBB).RightShift(iShift), hyoji);
                                maskBB.LeftShift(1);
                            }

                            if (iMs + 1 < (int)Conv_Masu.masu_error)
                            {
                                hyoji.Append("  ");
                            }
                            else
                            {
                                hyoji.AppendLine();
                            }
                        }

                        //*
                        int max = 64;// 32: 32bit
                        for (int i = 12; i < max; i += 3)
                        {
                            hyoji.AppendLine("i=[" + i + "]");
                            hyoji.Append("         ");
                            maskBB.Set(0x200);
                            Koma km = Med_Koma.KomasyuruiAndTaikyokusyaToKoma((Komasyurui)iKs, (Taikyokusya)iTb);
                            SpkBitboard.AppendSyuturyokuTo(BitboardsOmatome.KomanoUgokikataYk00.Clone_Merge(km, Conv_Masu.A1).Siborikomi(maskBB).RightShift(i + 0), hyoji);
                            if (i + 1 < max)
                            {
                                maskBB.Set(0x400);
                                SpkBitboard.AppendSyuturyokuTo(BitboardsOmatome.KomanoUgokikataYk00.Clone_Merge(km, Conv_Masu.A1).Siborikomi(maskBB).RightShift(i + 1), hyoji);
                            }
                            else
                            {
                                hyoji.Append(" ");
                            }

                            if (i + 2 < max)
                            {
                                maskBB.Set(0x800);
                                SpkBitboard.AppendSyuturyokuTo(BitboardsOmatome.KomanoUgokikataYk00.Clone_Merge(km, Conv_Masu.A1).Siborikomi(maskBB).RightShift(i + 2), hyoji);
                            }
                            else
                            {
                                hyoji.Append(" ");
                            }

                            hyoji.Append("  ");
                            maskBB.Set(0x200);
                            SpkBitboard.AppendSyuturyokuTo(BitboardsOmatome.KomanoUgokikataYk00.Clone_Merge(km, (Masu)1).Siborikomi(maskBB).RightShift(i + 0), hyoji);

                            if (i + 1 < max)
                            {
                                maskBB.Set(0x400);
                                SpkBitboard.AppendSyuturyokuTo(BitboardsOmatome.KomanoUgokikataYk00.Clone_Merge(km, (Masu)1).Siborikomi(maskBB).RightShift(i + 1), hyoji);
                            }
                            else
                            {
                                hyoji.Append(" ");
                            }

                            if (i + 2 < max)
                            {
                                maskBB.Set(0x800);
                                SpkBitboard.AppendSyuturyokuTo(BitboardsOmatome.KomanoUgokikataYk00.Clone_Merge(km, (Masu)1).Siborikomi(maskBB).RightShift(i + 2), hyoji);
                            }
                            else
                            {
                                hyoji.Append(" ");
                            }

                            hyoji.Append("  ");
                            maskBB.Set(0x200);
                            SpkBitboard.AppendSyuturyokuTo(BitboardsOmatome.KomanoUgokikataYk00.Clone_Merge(km, (Masu)2).Siborikomi(maskBB).RightShift(i + 0), hyoji);

                            if (i + 1 < max)
                            {
                                maskBB.Set(0x400);
                                SpkBitboard.AppendSyuturyokuTo(BitboardsOmatome.KomanoUgokikataYk00.Clone_Merge(km, (Masu)2).Siborikomi(maskBB).RightShift(i + 1), hyoji);
                            }
                            else
                            {
                                hyoji.Append(" ");
                            }

                            if (i + 2 < max)
                            {
                                maskBB.Set(0x800);
                                SpkBitboard.AppendSyuturyokuTo(BitboardsOmatome.KomanoUgokikataYk00.Clone_Merge(km, (Masu)2).Siborikomi(maskBB).RightShift(i + 2), hyoji);
                            }
                            else
                            {
                                hyoji.Append(" ");
                            }

                            hyoji.Append("  ");
                            maskBB.Set(0x200);
                            SpkBitboard.AppendSyuturyokuTo(BitboardsOmatome.KomanoUgokikataYk00.Clone_Merge(km, (Masu)3).Siborikomi(maskBB).RightShift(i + 0), hyoji);

                            if (i + 1 < max)
                            {
                                maskBB.Set(0x400);
                                SpkBitboard.AppendSyuturyokuTo(BitboardsOmatome.KomanoUgokikataYk00.Clone_Merge(km, (Masu)3).Siborikomi(maskBB).RightShift(i + 1), hyoji);
                            }
                            else
                            {
                                hyoji.Append(" ");
                            }

                            if (i + 2 < max)
                            {
                                maskBB.Set(0x800);
                                SpkBitboard.AppendSyuturyokuTo(BitboardsOmatome.KomanoUgokikataYk00.Clone_Merge(km, (Masu)3).Siborikomi(maskBB).RightShift(i + 2), hyoji);
                            }
                            else
                            {
                                hyoji.Append(" ");
                            }

                            hyoji.Append("  ");
                            maskBB.Set(0x200);
                            SpkBitboard.AppendSyuturyokuTo(BitboardsOmatome.KomanoUgokikataYk00.Clone_Merge(km, (Masu)4).Siborikomi(maskBB).RightShift(i + 0), hyoji);

                            if (i + 1 < max)
                            {
                                maskBB.Set(0x400);
                                SpkBitboard.AppendSyuturyokuTo(BitboardsOmatome.KomanoUgokikataYk00.Clone_Merge(km, (Masu)4).Siborikomi(maskBB).RightShift(i + 1), hyoji);
                            }
                            else
                            {
                                hyoji.Append(" ");
                            }

                            if (i + 2 < max)
                            {
                                maskBB.Set(0x800);
                                SpkBitboard.AppendSyuturyokuTo(BitboardsOmatome.KomanoUgokikataYk00.Clone_Merge(km, (Masu)4).Siborikomi(maskBB).RightShift(i + 2), hyoji);
                            }
                            else
                            {
                                hyoji.Append(" ");
                            }

                            hyoji.Append("  ");
                            maskBB.Set(0x200);
                            SpkBitboard.AppendSyuturyokuTo(BitboardsOmatome.KomanoUgokikataYk00.Clone_Merge(km, (Masu)5).Siborikomi(maskBB).RightShift(i + 0), hyoji);

                            if (i + 1 < max)
                            {
                                maskBB.Set(0x400);
                                SpkBitboard.AppendSyuturyokuTo(BitboardsOmatome.KomanoUgokikataYk00.Clone_Merge(km, (Masu)5).Siborikomi(maskBB).RightShift(i + 1), hyoji);
                            }
                            else
                            {
                                hyoji.Append(" ");
                            }

                            if (i + 2 < max)
                            {
                                maskBB.Set(0x800);
                                SpkBitboard.AppendSyuturyokuTo(BitboardsOmatome.KomanoUgokikataYk00.Clone_Merge(km, (Masu)5).Siborikomi(maskBB).RightShift(i + 2), hyoji);
                            }
                            else
                            {
                                hyoji.Append(" ");
                            }

                            hyoji.Append("  ");
                            maskBB.Set(0x200);
                            SpkBitboard.AppendSyuturyokuTo(BitboardsOmatome.KomanoUgokikataYk00.Clone_Merge(km, (Masu)6).Siborikomi(maskBB).RightShift(i + 0), hyoji);

                            if (i + 1 < max)
                            {
                                maskBB.Set(0x400);
                                SpkBitboard.AppendSyuturyokuTo(BitboardsOmatome.KomanoUgokikataYk00.Clone_Merge(km, (Masu)6).Siborikomi(maskBB).RightShift(i + 1), hyoji);
                            }
                            else
                            {
                                hyoji.Append(" ");
                            }

                            if (i + 2 < max)
                            {
                                maskBB.Set(0x800);
                                SpkBitboard.AppendSyuturyokuTo(BitboardsOmatome.KomanoUgokikataYk00.Clone_Merge(km, (Masu)6).Siborikomi(maskBB).RightShift(i + 2), hyoji);
                            }
                            else
                            {
                                hyoji.Append(" ");
                            }

                            hyoji.Append("  ");
                            maskBB.Set(0x200);
                            SpkBitboard.AppendSyuturyokuTo(BitboardsOmatome.KomanoUgokikataYk00.Clone_Merge(km, (Masu)7).Siborikomi(maskBB).RightShift(i + 0), hyoji);

                            if (i + 1 < max)
                            {
                                maskBB.Set(0x400);
                                SpkBitboard.AppendSyuturyokuTo(BitboardsOmatome.KomanoUgokikataYk00.Clone_Merge(km, (Masu)7).Siborikomi(maskBB).RightShift(i + 1), hyoji);
                            }
                            else
                            {
                                hyoji.Append(" ");
                            }

                            if (i + 2 < max)
                            {
                                maskBB.Set(0x800);
                                SpkBitboard.AppendSyuturyokuTo(BitboardsOmatome.KomanoUgokikataYk00.Clone_Merge(km, (Masu)7).Siborikomi(maskBB).RightShift(i + 2), hyoji);
                            }
                            else
                            {
                                hyoji.Append(" ");
                            }

                            hyoji.Append("  ");
                            maskBB.Set(0x200);
                            SpkBitboard.AppendSyuturyokuTo(BitboardsOmatome.KomanoUgokikataYk00.Clone_Merge(km, (Masu)8).Siborikomi(maskBB).RightShift(i + 0), hyoji);

                            if (i + 1 < max)
                            {
                                maskBB.Set(0x400);
                                SpkBitboard.AppendSyuturyokuTo(BitboardsOmatome.KomanoUgokikataYk00.Clone_Merge(km, (Masu)8).Siborikomi(maskBB).RightShift(i + 1), hyoji);
                            }
                            else
                            {
                                hyoji.Append(" ");
                            }

                            if (i + 2 < max)
                            {
                                maskBB.Set(0x800);
                                SpkBitboard.AppendSyuturyokuTo(BitboardsOmatome.KomanoUgokikataYk00.Clone_Merge(km, (Masu)8).Siborikomi(maskBB).RightShift(i + 2), hyoji);
                            }
                            else
                            {
                                hyoji.Append(" ");
                            }

                            hyoji.Append("  ");
                            maskBB.Set(0x200);
                            SpkBitboard.AppendSyuturyokuTo(BitboardsOmatome.KomanoUgokikataYk00.Clone_Merge(km, (Masu)9).Siborikomi(maskBB).RightShift(i + 0), hyoji);

                            if (i + 1 < max)
                            {
                                maskBB.Set(0x400);
                                SpkBitboard.AppendSyuturyokuTo(BitboardsOmatome.KomanoUgokikataYk00.Clone_Merge(km, (Masu)9).Siborikomi(maskBB).RightShift(i + 1), hyoji);
                            }
                            else
                            {
                                hyoji.Append(" ");
                            }

                            if (i + 2 < max)
                            {
                                maskBB.Set(0x800);
                                SpkBitboard.AppendSyuturyokuTo(BitboardsOmatome.KomanoUgokikataYk00.Clone_Merge(km, (Masu)9).Siborikomi(maskBB).RightShift(i + 2), hyoji);
                            }
                            else
                            {
                                hyoji.Append(" ");
                            }

                            hyoji.Append("  ");
                            maskBB.Set(0x200);
                            SpkBitboard.AppendSyuturyokuTo(BitboardsOmatome.KomanoUgokikataYk00.Clone_Merge(km, (Masu)10).Siborikomi(maskBB).RightShift(i + 0), hyoji);

                            if (i + 1 < max)
                            {
                                maskBB.Set(0x400);
                                SpkBitboard.AppendSyuturyokuTo(BitboardsOmatome.KomanoUgokikataYk00.Clone_Merge(km, (Masu)10).Siborikomi(maskBB).RightShift(i + 1), hyoji);
                            }
                            else
                            {
                                hyoji.Append(" ");
                            }

                            if (i + 2 < max)
                            {
                                maskBB.Set(0x800);
                                SpkBitboard.AppendSyuturyokuTo(BitboardsOmatome.KomanoUgokikataYk00.Clone_Merge(km, (Masu)10).Siborikomi(maskBB).RightShift(i + 2), hyoji);
                            }
                            else
                            {
                                hyoji.Append(" ");
                            }

                            hyoji.Append("  ");
                            maskBB.Set(0x200);
                            SpkBitboard.AppendSyuturyokuTo(BitboardsOmatome.KomanoUgokikataYk00.Clone_Merge(km, (Masu)11).Siborikomi(maskBB).RightShift(i + 0), hyoji);

                            if (i + 1 < max)
                            {
                                maskBB.Set(0x400);
                                SpkBitboard.AppendSyuturyokuTo(BitboardsOmatome.KomanoUgokikataYk00.Clone_Merge(km, (Masu)11).Siborikomi(maskBB).RightShift(i + 1), hyoji);
                            }
                            else
                            {
                                hyoji.Append(" ");
                            }

                            if (i + 2 < max)
                            {
                                maskBB.Set(0x800);
                                SpkBitboard.AppendSyuturyokuTo(BitboardsOmatome.KomanoUgokikataYk00.Clone_Merge(km, (Masu)11).Siborikomi(maskBB).RightShift(i + 2), hyoji);
                            }
                            else
                            {
                                hyoji.Append(" ");
                            }

                            hyoji.AppendLine();
                        }
                        //*/
                    }
                }
                Logger.Flush(hyoji);
                #endregion
            }
            if (Util_String.MatchAndNext("bit-popcnt", line, ref caret))
            {
                //────────────────────────────────────────
                // ビット演算のpopcntのテスト
                //────────────────────────────────────────
                #region ビット演算のpopcntのテスト
                Bitboard tmp = new Bitboard();
                for (ulong x = 0; x < 32UL; x++)
                {
                    tmp.Set(x);
                    hyoji.AppendLine(Convert.ToString((long)x, 2) + " の PopCnt =[" + tmp.PopCnt() + "]");
                }
                // 64bitあたり
                tmp.Set(1UL);
                tmp.LeftShift(63);
                for (int x = 0; x < 10; x++)
                {
                    hyoji.AppendLine(Convert.ToString((long)tmp.value64127, 2) + "_" + Convert.ToString((long)tmp.value063, 2) + " の PopCnt =[" + tmp.PopCnt() + "]");
                    tmp.LeftShift(1);
                }
                Logger.Flush(hyoji);
                #endregion
            }
            else if (Util_String.MatchAndNext("bitboard", line, ref caret))
            {
                //────────────────────────────────────────
                // 固定ビットボードの確認
                //────────────────────────────────────────
                // 段
                {
                    for (int iDan = 0; iDan < PureSettei.banTateHaba; iDan++)
                    {
                        SpkBan_1Column.Setumei_Bitboard("段" + iDan, BitboardsOmatome.bb_danArray[iDan], hyoji);
                    }
                    hyoji.AppendLine();
                }
                Logger.Flush(hyoji);
            }
            else if (Util_String.MatchAndNext("ittedume", line, ref caret))
            {
                //────────────────────────────────────────
                // 一手詰めのテスト
                //────────────────────────────────────────
                #region 一手詰めのテスト
                LisGenkyoku.SetRule(
                    GameRule.DobutuShogi, 3, 4,
                    "　ラ　" +
                    "き　き" +
                    "　にら" +
                    "ぞひぞ"
                    , new Dictionary<Motigoma, int>()
                    {
                        { Motigoma.K,0 },
                        { Motigoma.Z,0 },
                        { Motigoma.H,0 },
                        { Motigoma.k,0 },
                        { Motigoma.z,0 },
                        { Motigoma.h,0 },
                    }
                );

                SpkBan_1Column.Setumei_Kyokumen(PureMemory.kifu_endTeme, PureAppli.syuturyoku1);
                hyoji.AppendLine();
                Logger.Flush(hyoji);
#endregion
            }
            else if (Util_String.MatchAndNext("jisatusyu", line, ref caret))
            {
                //────────────────────────────────────────
                // 自殺手のテスト
                //────────────────────────────────────────

                if (!LisMasu.MatchMasu( line, ref caret, out Masu masu
#if DEBUG
                    ,(IDebugMojiretu) hyoji
#endif
                    ))
                {
                    return Pure.FailTrue("jisatusyuコマンド解析失敗☆？");
                }
                else
                {
                    if (Util_Hioute.IsJisatusyu(masu))
                    {
                        hyoji.AppendLine("自殺手だぜ☆");
                        Logger.Flush(hyoji);
                    }
                    else
                    {
                        hyoji.AppendLine("セーフ☆");
                        Logger.Flush(hyoji);
                    }
                }
            }
            else if (Util_String.MatchAndNext("sigmoid", line, ref caret))
            {
                //────────────────────────────────────────
                // シグモイド曲線のテスト
                //────────────────────────────────────────
                Util_Sigmoid.Test(hyoji);
            }
            else if (Util_String.MatchAndNext("tryrule", line, ref caret))
            {
                // うしろに続く文字は☆（＾▽＾）
                string rest = line.Substring(caret).Trim();

                //────────────────────────────────────────
                // トライルールのテスト
                //────────────────────────────────────────
#region トライルールのテスト

#if !DEBUG
                hyoji.AppendLine("デバッグモードで実行してくれだぜ☆（＾▽＾）");
                Logger.Flush(hyoji);
#endif

                if (int.TryParse(rest, out int testNo))
                {
                    testNo = PureSettei.random.Next(4);
                }

                // テストケースの作成（ランダム）
                switch (testNo)
                {
                    case 0:
                        {
                            LisGenkyoku.SetRule(
                                GameRule.DobutuShogi, 3, 4,
                                "　　　" +
                                "　キら" +
                                "ラ　　" +
                                "　　　"
                                , new Dictionary<Motigoma, int>()
                                {
                                    { Motigoma.K,0 },
                                    { Motigoma.Z,0 },
                                    { Motigoma.H,0 },
                                    { Motigoma.k,0 },
                                    { Motigoma.z,0 },
                                    { Motigoma.h,0 },
                                }
                            );
                        }
                        break;
                    case 1:
                        {
                            LisGenkyoku.SetRule(
                                GameRule.DobutuShogi, 3, 4,
                                "　　キ" +
                                "ラ　ら" +
                                "　　　" +
                                "　　　"
                                , new Dictionary<Motigoma, int>()
                                {
                                    { Motigoma.K,0 },
                                    { Motigoma.Z,0 },
                                    { Motigoma.H,0 },
                                    { Motigoma.k,0 },
                                    { Motigoma.z,0 },
                                    { Motigoma.h,0 },
                                }
                            );
                        }
                        break;
                    case 2:
                        {
                            LisGenkyoku.SetRule(
                                GameRule.DobutuShogi, 3, 4,
                                "ひ　　" +
                                "ら　ラ" +
                                "　　　" +
                                "　　　"
                                , new Dictionary<Motigoma, int>()
                                {
                                    { Motigoma.K,0 },
                                    { Motigoma.Z,0 },
                                    { Motigoma.H,0 },
                                    { Motigoma.k,0 },
                                    { Motigoma.z,0 },
                                    { Motigoma.h,0 },
                                }
                            );
                        }
                        break;
                    case 3:
                        {
                            LisGenkyoku.SetRule(
                                GameRule.DobutuShogi, 3, 4,
                                "　　　" +
                                "キらゾ" +
                                "　　　" +
                                "　　　"
                                , new Dictionary<Motigoma, int>()
                                {
                                    { Motigoma.K,0 },
                                    { Motigoma.Z,0 },
                                    { Motigoma.H,0 },
                                    { Motigoma.k,0 },
                                    { Motigoma.z,0 },
                                    { Motigoma.h,0 },
                                }
                            );
                        }
                        break;
                    default:
                        {
                            LisGenkyoku.SetRule(
                                GameRule.DobutuShogi, 3, 4,
                                "　　　" +
                                "　　ら" +
                                "ラ　　" +
                                "　　　"
                                , new Dictionary<Motigoma, int>()
                                {
                                    { Motigoma.K,0 },
                                    { Motigoma.Z,0 },
                                    { Motigoma.H,0 },
                                    { Motigoma.k,0 },
                                    { Motigoma.z,0 },
                                    { Motigoma.h,0 },
                                }
                            );
                        }
                        break;
                }

//                // 先手後手をランダムで変更☆（＾▽＾）
//                if (0 == PureSettei.random.Next(2))
//                {
//                    if (!KyokumenOperation.Try_Hanten(
//#if DEBUG
//                    (IDebugMojiretu)hyoji
//#endif
//                    ))
//                    {
//                        Util_Machine.Flush(hyoji);
//                        throw new Exception(hyoji.ToContents());
//                    }

//                    PureMemory.gky_ky.SetTeban( Taikyokusya.T2);
//                }

                Util_Control.UpdateRule(
#if DEBUG
                    "test tryrule"
#endif
                );
                // 駒を配置したあとで使えだぜ☆（＾～＾）
                PureMemory.gky_ky.shogiban.Tukurinaosi_RemakeKiki();
                SpkBan_1Column.Setumei_Kyokumen(PureMemory.kifu_endTeme, PureAppli.syuturyoku1);
                hyoji.AppendLine();
                Logger.Flush(hyoji);

                Koma raionKm = (PureMemory.kifu_teban == Taikyokusya.T1 ? Koma.R : Koma.r);
                Masu ms1 = GenkyokuOpe.Lookup( raionKm);
                Bitboard kikiBB = new Bitboard();
                BitboardsOmatome.KomanoUgokikataYk00.ToSet_Merge(
                    Med_Koma.KomasyuruiAndTaikyokusyaToKoma(Komasyurui.R, PureMemory.kifu_teban),
                    ms1, kikiBB);

#if DEBUG
                // なんだぜ、このコード☆（＾～＾）？
                {
                    bool tmp = Util_Test.TestMode;
                    Util_Test.TestMode = true;
                    Pure.Sc.Push("テスト", hyoji);
                    Util_TryRule.GetTrySaki( kikiBB, ms1);
                    Pure.Sc.Pop();
                    Util_Test.TestMode = tmp;
                }
#endif
#endregion
            }
            else
            {
                //────────────────────────────────────────
                // それ以外のテスト
                //────────────────────────────────────────
                /*
                ky.SetBanjo(
                    "ラ　　" +
                    "　ひひ" +
                    "ヒヒ　" +
                    "　　ら");
                syuturyoku.AppendLine(ky.Setumei());
                Util_Machine.Flush();
                */

                /*
                Util_Logger.WriteLine("posp>");
                Util_Logger.WriteLine(ApplicationImpl.Kyokumen.Setumei());

                Move ss = AbstractConvMove.ToMove((Masu)7, (Masu)4, Komasyurui.H, Komasyurui.H, Komasyurui.H);
                Debug.Assert((int)ss != -1, "");

                Util_Logger.WriteLine("> " + AbstractConvMove.Setumei_Fen(ss));
                Util_Logger.WriteLine("src masu > " + AbstractConvMove.GetSrcMasu(ss));
                Util_Logger.WriteLine("src suji > " + Conv_Kihon.ToAlphabetLarge(AbstractConvMove.GetSrcSuji(ss)));
                Util_Logger.WriteLine("src dan  > " + AbstractConvMove.GetSrcDan(ss));
                Util_Logger.WriteLine("src uttKs> " + Conv_Komasyurui.Setumei(AbstractConvMove.GetUttaKomasyurui(ss)));
                Util_Logger.WriteLine("dst masu > " + AbstractConvMove.GetDstMasu(ss));
                Util_Logger.WriteLine("dst suji > " + Conv_Kihon.ToAlphabetLarge(AbstractConvMove.GetDstSuji(ss)));
                Util_Logger.WriteLine("dst dan  > " + AbstractConvMove.GetDstDan(ss));
                Util_Logger.WriteLine("torareta > " + Conv_Komasyurui.Setumei(ApplicationImpl.Kyokumen.Konoteme.ToraretaKs));

                Nanteme nanteme = new NantemeImpl();
                ApplicationImpl.Kyokumen.DoMove(ss, ref nanteme);
                Util_Logger.WriteLine("DoMove >");
                Util_Logger.WriteLine(ApplicationImpl.Kyokumen.Setumei());

                Util_Logger.WriteLine("torareta > " + Conv_Komasyurui.Setumei(ApplicationImpl.Kyokumen.Konoteme.ToraretaKs));

                Util_Logger.WriteLine("UndoMove>");
                ApplicationImpl.Kyokumen.UndoMove(ss);
                Util_Logger.WriteLine(ApplicationImpl.Kyokumen.Setumei());
                Util_Logger.Flush();
                */
            }

            return Pure.SUCCESSFUL_FALSE;
        }

        public static bool TryFail_Tonarikiki(string line, IHyojiMojiretu hyoji)
        {
            int caret = 0;
            if (Util_String.MatchAndNext("tonarikiki", line, ref caret))
            {
                Koma km;
                if (LisKoma.MatchKoma(line, ref caret, out km))
                {

                    Masu ms;
                    if (LisMasu.MatchMasu(line, ref caret, out ms
#if DEBUG
                        ,(IDebugMojiretu)hyoji
#endif
                        ))
                    {
                        Bitboard bb = BitboardsOmatome.KomanoUgokikataYk00.CloneElement(km, ms);
                        SpkBan_Hisigata.Setumei_yk00("隣利き", bb, hyoji);
                    }

                }
            }
            return Pure.SUCCESSFUL_FALSE;
        }

        /// <summary>
        /// 詰将棋だぜ☆（＾▽＾）
        /// </summary>
        public static void TumeShogi(FenSyurui f, string line, IHyojiMojiretu hyoji)
        {
            // "tu" に統一するぜ☆（＾▽＾）
            line = line.Replace("tumeshogi", "tu");

            int bango;
            if (line == "tu")
            {
                bango = PureSettei.random.Next(2);
            }
            else
            {
                // うしろに続く文字は☆（＾▽＾）
                int caret = 0;
                Util_String.TobasuTangoToMatubiKuhaku(line, ref caret, "tu ");
                string line2 = line.Substring(caret);

                if (!int.TryParse(line2, out bango))
                {
                    return;
                }
            }

            // （＾～＾）詰将棋しようぜ☆
            if (!Util_TumeShogi.Try_TumeShogi(f, bango, out int nantedume
#if DEBUG
                , (IDebugMojiretu)hyoji
#endif
                ))
            {
                Logger.Flush(hyoji);
                throw new Exception(hyoji.ToContents());
            }

            ComSettei.saidaiFukasa = nantedume + 1;
            hyoji.AppendLine(string.Format("# {0}手詰め", nantedume));
            SpkBan_1Column.Setumei_Kyokumen(PureMemory.kifu_endTeme, hyoji);
        }

    }
}
