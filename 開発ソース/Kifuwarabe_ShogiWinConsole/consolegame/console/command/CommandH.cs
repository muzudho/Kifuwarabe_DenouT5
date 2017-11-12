#if DEBUG
using kifuwarabe_shogithink.pure;
using kifuwarabe_shogithink.pure.accessor;
using kifuwarabe_shogithink.pure.control;
using kifuwarabe_shogithink.pure.ikkyoku;
using kifuwarabe_shogithink.pure.listen;
using kifuwarabe_shogithink.pure.listen.genkyoku;
using kifuwarabe_shogithink.pure.logger;
using kifuwarabe_shogithink.pure.med.ky;
using kifuwarabe_shogithink.pure.speak.genkyoku;
using kifuwarabe_shogithink.pure.speak.ky_info;
using kifuwarabe_shogiwin.consolegame.machine;
using kifuwarabe_shogiwin.speak.ban;
using System;
#else
using kifuwarabe_shogithink.pure.accessor;
using kifuwarabe_shogithink.pure;
using kifuwarabe_shogithink.pure.control;
using kifuwarabe_shogithink.pure.ikkyoku;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.listen;
using kifuwarabe_shogithink.pure.listen.genkyoku;
using kifuwarabe_shogithink.pure.logger;
using kifuwarabe_shogithink.pure.med.ky;
using kifuwarabe_shogithink.pure.speak.genkyoku;
using kifuwarabe_shogithink.pure.speak.ky_info;
using kifuwarabe_shogiwin.consolegame.machine;
using kifuwarabe_shogiwin.speak.ban;
using System;
using System.Collections.Generic;
#endif

namespace kifuwarabe_shogiwin.consolegame.console.command
{
    public static class CommandH
    {
        public static void Hirate(string line, IHyojiMojiretu hyoji)
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
                    SasiteSeiseiAccessor.BackTemeToFirst_AndClearTeme();
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
        public static void Honyaku(string line, IHyojiMojiretu hyoji)
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
                            Util_Machine.Flush(hyoji);
                            throw new Exception(hyoji.ToContents());
                        }

                        hyoji.AppendLine(PureMemory.kifu_syokiKyokumenFen);
                        Util_Machine.Flush(hyoji);
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
                            Util_Machine.Flush(hyoji);
                            throw new Exception(hyoji.ToContents());
                        }

                        ICommandMojiretu sfen = new MojiretuImpl();
                        SpkGenkyokuOpe.AppendFenTo( FenSyurui.dfe_n, sfen);// 局面は、棋譜を持っていない

                        //if ("" != moves)
                        //{
                        //    moves = moves.Substring("moves ".Length);
                        //    Kifu kifu2 = new Kifu();
                        //    kifu2.AddMoves(true, moves, gky.yomiKy); // ky2 は空っぽなんで、 ky の診断を使う。盤サイズを見る。
                        //    sfen.Append(" moves ");
                        //    kifu2.AppendMovesTo(true, sfen);
                        //}

                        sfen.AppendLine();
                        Util_Machine.Flush((IHyojiMojiretu)sfen);
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

        public static void Hyoka(string line, IHyojiMojiretu hyoji)
        {
            int caret = 0;
            if (Util_String.MatchAndNext("hyoka",line, ref caret))
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

    }
}
