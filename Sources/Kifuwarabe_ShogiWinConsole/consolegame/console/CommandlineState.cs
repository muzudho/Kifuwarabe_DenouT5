#if DEBUG
using kifuwarabe_shogithink.pure;
using kifuwarabe_shogithink.pure.genkyoku;
using kifuwarabe_shogithink.pure.ikkyoku;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.logger;
using kifuwarabe_shogiwin.consolegame.console.command;
using kifuwarabe_shogiwin.consolegame.machine;
using kifuwarabe_shogiwin.speak.ban;
using System;
using System.Collections.Generic;
using kifuwarabe_shogithink.pure.accessor;
using kifuwarabe_shogithink.pure.control;
#else
using kifuwarabe_shogithink.pure.control;
using kifuwarabe_shogithink.pure;
using kifuwarabe_shogithink.pure.accessor;
using kifuwarabe_shogithink.pure.ikkyoku;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.logger;
using kifuwarabe_shogiwin.consolegame.console.command;
using kifuwarabe_shogiwin.consolegame.machine;
using kifuwarabe_shogiwin.speak.ban;
using System;
using System.Collections.Generic;
#endif

namespace kifuwarabe_shogiwin.consolegame.console
{
    public abstract class CommandlineState
    {
        static CommandlineState()
        {
            commandBufferName = "";
            commandBuffer = new List<string>(0);
            multipleLineCommand = new List<string>();
        }

        public static string commandline { get; set; }
        public static int caret { get; set; }
        public static string commandBufferName { get; set; }
        public static List<string> commandBuffer { get; set; }
        public static bool isQuit { get; set; }
        public static bool isKyokumenEcho1 { get; set; }

        #region 複数行コマンド
        /// <summary>
        /// TODO: 複数行コマンドモード☆（＾～＾）
        /// 「.」だけの行になるまで続く予定☆（＾～＾）
        /// </summary>
        static bool isMultipleLineCommand;
        public static List<string> multipleLineCommand;
        public static void DoMultipleLineCommand(DLGT_MultipleLineCommand dlgt_multipleLineCommand)
        {
            isMultipleLineCommand = true;
            //isKyokumenEcho1 = false;
            CommandlineState.dlgt_multipleLineCommand = dlgt_multipleLineCommand;
        }
        public delegate void DLGT_MultipleLineCommand(List<string> multipleLineCommand);
        static DLGT_MultipleLineCommand dlgt_multipleLineCommand;
        #endregion

        public static void InitCommandline()
        {
            commandline = null;// 空行とは区別するぜ☆（＾▽＾）
            caret = 0;
        }
        public static void ClearCommandline()
        {
            commandline = "";
            caret = 0;
        }
        /// <summary>
        /// コマンドの誤発動防止
        /// </summary>
        public static void CommentCommandline()
        {
            commandline = "#";
            caret = 0;
        }
        /// <summary>
        /// コマンド・バッファーから１行読取り。
        /// </summary>
        public static void ReadCommandBuffer(IHyojiMojiretu hyoji)
        {
            if (0 < commandBuffer.Count)
            {
                commandline = commandBuffer[0];
                caret = 0;
                commandBuffer.RemoveAt(0);
                hyoji.AppendLine(commandline);
            }
        }
        public static void SetCommandline(string commandline)
        {
            CommandlineState.commandline = commandline;
            caret = 0;
        }

        /// <summary>
        /// 次の入力を促す表示をしてるだけだぜ☆（＾～＾）
        /// </summary>
        public static void ShowPrompt(FenSyurui f, IHyojiMojiretu hyoji)
        {
            if (0 < commandBuffer.Count)
            {
                // コマンド・バッファーの実行中だぜ☆（＾▽＾）
                hyoji.Append(commandBufferName + "> ");
                Util_Machine.Flush(hyoji);
            }
            else if (GameMode.Game == PureAppli.gameMode)
            {
                // 表示（コンソール・ゲーム用）　局面、あれば勝敗☆（＾～＾）
                {
                    if (isKyokumenEcho1)
                    {
                        SpkBan_1Column.Setumei_NingenGameYo(PureMemory.kifu_endTeme, hyoji);

                        //#if DEBUG
                        //                        CommandK.Ky(isSfen, "ky fen", gky, syuturyoku);// 参考：改造FEN表示
                        //                        CommandS.Move_cmd(isSfen, "move", gky, syuturyoku);// 参考：指し手表示
                        //                        if (false){
                        //                            SpkShogiban.HyojiKomanoIbasho(gky.ky.shogiban, syuturyoku);// 参考：駒の表示
                        //                            SpkShogiban.HyojiKomanoKikiSu(gky.ky.shogiban, syuturyoku);// 参考：利きの数
                        //                        }
                        //                        CommandS.Move_cmd(isSfen, "move seisei", gky, syuturyoku);// 参考：指し手表示 詳細
                        //                        Util_Machine.Flush(syuturyoku);
                        //#endif

                        CommandR.Result(hyoji, CommandMode.NingenYoConsoleGame);
                    }
                    Util_Machine.Flush(hyoji);
                }

                if (!isMultipleLineCommand // 複数行コマンド読み取り中はプロンプトを出さないぜ☆（＾～＾）
                    &&
                    (PureMemory.kifu_teban == Taikyokusya.T1 && !PureSettei.p1Com)
                    ||
                    (PureMemory.kifu_teban == Taikyokusya.T2 && !PureSettei.p2Com)
                    )
                {
                    // 人間の手番が始まるところで☆
                    hyoji.Append(
                        "指し手を入力してください。一例　do B3B2　※ do b3b2 も同じ" + Environment.NewLine +
                        "> ");
                    Util_Machine.Flush(hyoji);
                }
            }
            else
            {
                // 表示（コンソール・ゲーム用）
                hyoji.Append("> ");
                Util_Machine.Flush(hyoji);
            }
        }

        public static bool TryFail_DoCommandline( IHyojiMojiretu hyoji)
        {
            string cmdline = CommandlineState.commandline;
            int caret = CommandlineState.caret;
            isQuit = false;
            isKyokumenEcho1 = false; // ゲーム・モードの場合、特に指示がなければ　コマンド終了後、局面表示を返すぜ☆

            if (isMultipleLineCommand)
            {
                // TODO: 複数行コマンド中☆（＾～＾）
                //syuturyoku.AppendLine("TODO: ky set 複数行コマンド中☆（＾～＾）(2) commandline="+ commandline);
                //isKyokumenEcho1 = false;
                if (cmdline == ".")
                {
                    // 「.」だけの行が来たら終了だぜ☆（＾～＾）
                    isMultipleLineCommand = false;
                    // 実行☆（＾～＾）
                    dlgt_multipleLineCommand(multipleLineCommand);
                    multipleLineCommand.Clear();
                    //syuturyoku.AppendLine("TODO: 複数行コマンドは=" + sbMultipleLineCommand.ToString());
                }
                else
                {
                    multipleLineCommand.Add(cmdline);
                }
                goto gt_EndCommand;
            }

            if (null==cmdline)
            {
                // 未設定
                isKyokumenEcho1 = true;
            }
            else if (cmdline == "")
            {
                isKyokumenEcho1 = true;
                // 空打ちは無視するか、からっぽモードでは、ゲームモードに切り替えるぜ☆（＾▽＾）
                if (GameMode.Karappo == PureAppli.gameMode)// 感想戦での発動防止☆
                {
                    // ゲームモード（対局開始）
                    PureAppli.gameMode = GameMode.Game;
                }
            }
            // なるべく、アルファベット順☆（＾▽＾）同じつづりで始まる単語の場合、語句の長い単語を優先にしないと if 文が通らないぜ☆ｗｗｗ
            else if (caret == cmdline.IndexOf("@", caret)) { CommandA.Atmark(cmdline, hyoji); }
            else if (caret == cmdline.IndexOf("#", caret)) { }// 受け付けるが、何もしないぜ☆（＾▽＾）ｗｗｗ
            else if (caret == cmdline.IndexOf("bitboard", caret)) {
                // ビットボードの表示テスト用だぜ☆（＾～＾）
                if (CommandB.TryFail_Bitboard(cmdline, hyoji))
                {
                    return Pure.FailTrue("TryFail_Bitboard");
                }
            }
            else if (caret == cmdline.IndexOf("cando", caret)) {
                CommandC.CanDo(PureSettei.fenSyurui, cmdline, GameMode.Game == PureAppli.gameMode ? CommandMode.NingenYoConsoleGame : CommandMode.NigenYoConsoleKaihatu, hyoji); isKyokumenEcho1 = true;
            }
            else if (caret == cmdline.IndexOf("chikanhyo", caret))
            {
                if (CommandC.TryFail_ChikanHyo(cmdline, hyoji
                    ))
                {
                    return Pure.FailTrue("TryFail_ChikanHyo");
                }
            }
            else if (caret == cmdline.IndexOf("clear", caret)) { CommandC.Clear(); }
            else if (caret == cmdline.IndexOf("dosub", caret))
            {
                if (CommandD.TryFail_DoSub(cmdline, hyoji))
                {
                    return Pure.FailTrue("TryFail_Do");
                }
                isKyokumenEcho1 = true;
            }
            else if (caret == cmdline.IndexOf("do", caret)) {
                if (CommandD.TryFail_Do(
                    PureSettei.fenSyurui,
                    cmdline,
                    GameMode.Game == PureAppli.gameMode ? CommandMode.NingenYoConsoleGame : CommandMode.NigenYoConsoleKaihatu, hyoji
                    ))
                {
                    return Pure.FailTrue("TryFail_Do");
                }
                isKyokumenEcho1 = true;
            }
#if DEBUG
            else if (caret == cmdline.IndexOf("dump", caret))
            {
                if (CommandD.TryFail_Dump(cmdline, hyoji
                    ))
                {
                    return Pure.FailTrue("TryFail_Dump");
                }
            }
#endif
            else if (caret == cmdline.IndexOf("fugo", caret))
            {
                if (CommandF.TryFail_Fugo(cmdline, hyoji
                    ))
                {
                    return Pure.FailTrue("TryFail_Fugo");
                }
            }
            else if (caret == cmdline.IndexOf("gameover", caret)) { CommandG.Gameover(cmdline, hyoji); isKyokumenEcho1 = true; }
            else if (caret == cmdline.IndexOf("go", caret)) {
                if (CommandG.TryFail_Go(
                    PureSettei.usi,
                    PureSettei.fenSyurui,
                    CommandMode.NigenYoConsoleKaihatu
                    , hyoji
                    ))
                {
                    return Pure.FailTrue("Try_Go");
                }
            }
            else if (caret == cmdline.IndexOf("hirate", caret)) { CommandH.Hirate(cmdline, hyoji); isKyokumenEcho1 = true; }
            else if (caret == cmdline.IndexOf("honyaku", caret)) { CommandH.Honyaku(cmdline, hyoji); }
            else if (caret == cmdline.IndexOf("hyoka", caret)) { CommandH.Hyoka(cmdline, hyoji); }
            else if (caret == cmdline.IndexOf("ojama", caret)) {
                if (CommandO.TryFail_Ojama(cmdline, hyoji
                    ))
                {
                    return Pure.FailTrue("TryFail_Ojama");
                }
            }
            else if (caret == cmdline.IndexOf("isready", caret)) { CommandI.Isready(cmdline, hyoji); }
            else if (caret == cmdline.IndexOf("jokyo", caret)) { CommandJ.Jokyo( cmdline, hyoji); }
            else if (caret == cmdline.IndexOf("kansosen", caret)) { CommandK.Kansosen(PureSettei.fenSyurui, cmdline, hyoji); }// 駒の場所を表示するぜ☆（＾▽＾）
            else if (caret == cmdline.IndexOf("kifu", caret)) { CommandK.Kifu(PureSettei.fenSyurui, cmdline, hyoji); }// 駒の場所を表示するぜ☆（＾▽＾）
            else if (caret == cmdline.IndexOf("kikisu", caret)) {
                // 利きの数を調べるぜ☆（＾▽＾）
                // 旧名「kikikazu」→「kikisu」
                CommandK.Kikisu(cmdline, hyoji);
            }
            else if (caret == cmdline.IndexOf("kiki", caret)) {
                // 利きを調べるぜ☆（＾▽＾）
                if (CommandK.TryFail_Kiki(cmdline, hyoji))
                {
                    return Pure.FailTrue("TryFail_Kiki");
                }
            }
            else if (caret == cmdline.IndexOf("koma", caret)) {
                Pure.Sc.Push("komaコマンド");
                CommandK.Koma_cmd(PureSettei.fenSyurui, cmdline, hyoji);
                Pure.Sc.Pop();
            }// 駒の場所を表示するぜ☆（＾▽＾）
            else if (caret == cmdline.IndexOf("ky", caret)) {
                // 局面をクリアーしてやり直すときもここを通るので、ここで局面アサートを入れてはいけないぜ☆（＾～＾）

                if (CommandK.TryFail_Ky(cmdline, hyoji))
                {
                    return Pure.FailTrue("Try_Ky");
                }

            }// 局面を表示するぜ☆（＾▽＾）
            else if (caret == cmdline.IndexOf("manual", caret)) { CommandM.Man(hyoji); }// "man" と同じ☆（＾▽＾）
            else if (caret == cmdline.IndexOf("man", caret)) { CommandM.Man(hyoji); }// "manual" と同じ☆（＾▽＾）
            else if (caret == cmdline.IndexOf("masu", caret)) { CommandM.Masu_cmd(cmdline, hyoji); }
            else if (caret == cmdline.IndexOf("nanamedan", caret))
            {
                if (CommandN.TryFail_Nanamedan(cmdline, hyoji
                    ))
                {
                    return Pure.FailTrue("TryFail_Nanamedan");
                }
            }
            else if (caret == cmdline.IndexOf("nisinsu", caret))
            {
                if (CommandN.TryFail_Nisinsu(cmdline, hyoji
                    ))
                {
                    return Pure.FailTrue("TryFail_Nisinsu");
                }
            }
            else if (caret == cmdline.IndexOf("position", caret)) { CommandP.Position(PureSettei.fenSyurui, cmdline, hyoji); }
            else if (caret == cmdline.IndexOf("prego", caret)) { CommandP.PreGo( cmdline, hyoji); }
            else if (caret == cmdline.IndexOf("quit", caret)) { isQuit = true; isKyokumenEcho1 = true; }
            else if (caret == cmdline.IndexOf("result", caret)) { CommandR.Result( hyoji, CommandMode.NigenYoConsoleKaihatu); }
            else if (caret == cmdline.IndexOf("rnd", caret)) {
                if(!CommandR.Try_Rnd(
#if DEBUG
                    (IDebugMojiretu)hyoji
#endif
                ))
                {
                    return Pure.FailTrue("commandline");
                }
                isKyokumenEcho1 = true;
            }
            else if (caret == cmdline.IndexOf("move", caret)) {
                if (CommandS.TryFail_Move_cmd(cmdline, hyoji))
                {
                    return Pure.FailTrue("TryFail_Move_cmd");
                }
            }
            else if (caret == cmdline.IndexOf("setoption", caret)) { CommandS.Setoption(cmdline, hyoji); }
            else if (caret == cmdline.IndexOf("set", caret)) { CommandS.Set(cmdline, hyoji); }
            else if (caret == cmdline.IndexOf("taikyokusya", caret)) { CommandT.Taikyokusya_cmd(cmdline, hyoji); }
            else if (caret == cmdline.IndexOf("tansaku", caret)) { CommandT.Tansaku(cmdline, hyoji); }
            else if (caret == cmdline.IndexOf("test", caret)) {
                if (CommandT.TryFail_Test(cmdline, hyoji))
                {
                    return Pure.FailTrue("TryFail_Test");
                }
            }
            else if (caret == cmdline.IndexOf("tonarikiki", caret))
            {
                if (CommandT.TryFail_Tonarikiki(cmdline, hyoji))
                {
                    return Pure.FailTrue("TryFail_Tonarikiki");
                }
            }
            else if (caret == cmdline.IndexOf("tumeshogi", caret)) { CommandT.TumeShogi(PureSettei.fenSyurui, cmdline, hyoji); }// "tu" と同じ☆（＾▽＾）
            else if (caret == cmdline.IndexOf("tu", caret)) { CommandT.TumeShogi(PureSettei.fenSyurui, cmdline, hyoji); }// "tumeshogi" と同じ☆（＾▽＾）
            else if (caret == cmdline.IndexOf("ugokikata", caret))
            {
                if (CommandU.TryFail_Ugokikata(cmdline, hyoji))
                {
                    return Pure.FailTrue("TryFail_Ugokikata");
                }
            }
            else if (caret == cmdline.IndexOf("undo", caret)) {
                CommandU.Undo(cmdline, hyoji);
            }
            else if (caret == cmdline.IndexOf("updaterule", caret))
            {
                CommandU.UpdateRule(cmdline, hyoji);
            }
            else if (caret == cmdline.IndexOf("usinewgame", caret)) { CommandU.Usinewgame(cmdline, hyoji); }
            else if (caret == cmdline.IndexOf("usi", caret)) { CommandU.Usi(cmdline, hyoji); }//ここは普通、来ない☆（＾～＾）
            else
            {
                // 表示（コンソール・ゲーム用）
                hyoji.Append("「");
                hyoji.Append(cmdline);
                hyoji.AppendLine("」☆？（＾▽＾）");

                hyoji.AppendLine("そんなコマンドは無いぜ☆（＞＿＜） man で調べろだぜ☆（＾▽＾）");
                Util_Machine.Flush(hyoji);
                isKyokumenEcho1 = true;
            }
            gt_EndCommand:

            return Pure.SUCCESSFUL_FALSE;
        }
    }
}
