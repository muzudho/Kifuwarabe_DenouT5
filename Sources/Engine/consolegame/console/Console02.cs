#if DEBUG
using kifuwarabe_shogithink.pure;
using kifuwarabe_shogithink.pure.accessor;
using kifuwarabe_shogithink.pure.com;
using kifuwarabe_shogithink.pure.conv.genkyoku.play;
using kifuwarabe_shogithink.pure.genkyoku;
using kifuwarabe_shogithink.pure.ikkyoku;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.listen;
using kifuwarabe_shogithink.pure.listen.play;
using kifuwarabe_shogithink.pure.logger;
using kifuwarabe_shogithink.pure.move;
using kifuwarabe_shogithink.pure.speak.genkyoku;
using kifuwarabe_shogiwin.consolegame.console.command;
using kifuwarabe_shogiwin.consolegame.machine;
using System;
using kifuwarabe_shogithink.pure.listen.genkyoku;
using System.Collections.Generic;
using kifuwarabe_shogithink.pure.control;
#else
using Grayscale.Kifuwarabi.Entities.Logging;
using kifuwarabe_shogithink.pure;
using kifuwarabe_shogithink.pure.accessor;
using kifuwarabe_shogithink.pure.com;
using kifuwarabe_shogithink.pure.ikkyoku;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.listen;
using kifuwarabe_shogithink.pure.listen.play;
using kifuwarabe_shogithink.pure.logger;
using kifuwarabe_shogithink.pure.move;
using kifuwarabe_shogithink.pure.speak.genkyoku;
using kifuwarabe_shogiwin.consolegame.console.command;
using kifuwarabe_shogiwin.consolegame.machine;
#endif

namespace kifuwarabe_shogiwin.consolegame.console
{
    public static class Console02
    {
        /// <summary>
        /// 対局終了
        /// </summary>
        public static void DoTejun5_SyuryoTaikyoku1( IHyojiMojiretu hyoji)
        {
            // 表示（コンソール・ゲーム用）
            {
                CommandR.Result( hyoji, CommandMode.NingenYoConsoleGame);
                hyoji.AppendLine("終わったぜ☆（＾▽＾）");
                Logger.Flush(hyoji);
            }



            // 棋譜の初期局面を更新☆
            {
                ICommandMojiretu kyFen_temp = new MojiretuImpl();
                SpkGenkyokuOpe.AppendFenTo( PureSettei.fenSyurui, kyFen_temp);
                PureMemory.kifu_syokiKyokumenFen = kyFen_temp.ToContents();
            }


            // TODO: 成績は保存しないにしても、棋譜は欲しいときもあるぜ☆（＾～＾）
            // 棋譜を作ろうぜ☆
            hyoji.AppendLine( "感想戦を行う場合は kansosen と打てだぜ☆（＾▽＾）　そのあと kifu 1 とか打て☆（＾▽＾）");
            hyoji.AppendLine( "終わるときは hirate な☆（＾▽＾）");
            Logger.Flush(hyoji);

            // 初期局面に戻すぜ☆（＾▽＾）
            Util_Taikyoku.Clear();

            // 棋譜カーソルを０にすれば、初期局面に戻るだろ☆ｗｗｗ（＾▽＾）
            MoveGenAccessor.BackTemeToFirst_AndClearTeme();


            if (Util_Machine.IsRenzokuTaikyokuStop())
            {
                // 連続対局を止めるぜ☆（＾▽＾）
                ConsolegameSettei.renzokuTaikyoku = false;
                hyoji.AppendLine(Util_Machine.RENZOKU_TAIKYOKU_STOP_FILE + "> done");
            }

            if (!ConsolegameSettei.renzokuTaikyoku)
            {
                // ゲームモードを解除するぜ☆（＾～＾）
                if (GameMode.Game == PureAppli.gameMode)// 感想戦での発動防止☆
                {
                    PureAppli.gameMode = GameMode.Karappo;
                }
            }
            else
            {
                // 連続対局中☆（＾～＾）

            }

            // コマンドの誤発動防止
            CommandlineState.CommentCommandline();
        }
        public static void DoTejun7_FlushAll2(IHyojiMojiretu hyoji)
        {
            // 表示・ログ出力（コンソール・ゲーム用）
            {
                // ファイルに書き出していないログが溜まっていれば、これで全部書き出します。
                Logger.Flush(hyoji);
            }
        }

        public static bool ParseDoMove( out Move out_move)
        {
            // コンソールからのキー入力を解析するぜ☆（＾▽＾）
            int caret = CommandlineState.caret;
            int oldCaret = CommandlineState.caret;

            Util_String.TobasuTangoToMatubiKuhaku(CommandlineState.commandline, ref caret, "do ");

            // うしろに続く文字は☆（＾▽＾）
            if (!LisPlay.MatchFenMove(PureSettei.fenSyurui, CommandlineState.commandline, ref caret, out out_move))
            {
                CommandlineState.caret = oldCaret;

                //String2 str = new String2Impl();
                //str.Append("指し手のパースに失敗だぜ☆（＾～＾）！ #鷺 commandline=[");
                //str.Append(commandline);
                //str.Append("] caret=[");
                //str.Append(caret);
                //str.Append("]");
                //syuturyoku.AppendLine(str.ToContents());
                //Util_Machine.Flush();
                //throw new Exception(str.ToContents());
                return false;
            }

            // do コマンドだった場合☆
            CommandlineState.caret = caret;
            CommandlineState.CommentCommandline();// コマンドの誤発動防止
            return true;
        }

        #region コンソールゲーム用の機能☆
        /// <summary>
        /// アプリケーション終了時に呼び出せだぜ☆（＾▽＾）！
        /// </summary>
        /// <param name="hyoji"></param>
        public static void End_Application(IHyojiMojiretu hyoji)
        {
            #region （手順７）保存して終了
            //────────────────────────────────────────
            // （手順７）保存して終了
            //────────────────────────────────────────
            // 保存していないものを保存するぜ☆（＾▽＾）
            Console02.DoTejun7_FlushAll2(hyoji);
            #endregion
        }

        /// <summary>
        /// 人間の番☆
        /// </summary>
        /// <returns></returns>
        public static bool IsNingenNoBan()
        {
            return (PureMemory.kifu_teban == Taikyokusya.T1 && !PureSettei.p1Com) // コンピューターでない場合
                    ||
                    (PureMemory.kifu_teban == Taikyokusya.T2 && !PureSettei.p2Com) // コンピューターでない場合
                    ;
        }
        /// <summary>
        /// コンピューターの番☆
        /// </summary>
        /// <returns></returns>
        public static bool IsComputerNoBan()
        {
            return (PureMemory.kifu_teban == Taikyokusya.T1 && PureSettei.p1Com) // 対局者１でコンピューター☆
                        ||
                        (PureMemory.kifu_teban == Taikyokusya.T2 && PureSettei.p2Com) // 対局者２でコンピューター☆
                        ;
        }



        /// <summary>
        /// 定跡等外部ファイルの保存間隔の調整だぜ☆　もう保存していいなら真だぜ☆（＾▽＾）
        /// </summary>
        /// <returns></returns>
        public static bool IsOk_SavefileTimeSpan()
        {
            return ComSettei.timeManager.IsTimeOver_Savefile();
        }
        /// <summary>
        /// 保存間隔の調整だぜ☆　保存が終わったら呼び出せだぜ☆（＾▽＾）
        /// </summary>
        public static void Restart_SavefileTimeSpan()
        {
            ComSettei.timeManager.RestartStopwatch_Savefile();
        }

        /// <summary>
        /// 連続対局時のルール変更間隔の調整だぜ☆　もう変更していいなら真だぜ☆（＾▽＾）
        /// 
        /// ルールを変更するときに必要となる、ファイルの読み書き時間を回避するためのものだぜ☆（＾▽＾）
        /// </summary>
        /// <returns></returns>
        public static bool IsTimeOver_RenzokuRandomRule()
        {
            return ComSettei.timeManager.IsTimeOver_RenzokuRandomRule();
        }
        /// <summary>
        /// 変更間隔の調整だぜ☆　変更が終わったら呼び出せだぜ☆（＾▽＾）
        /// </summary>
        public static void Restart_RenzokuRandomRuleTimeSpan()
        {
            ComSettei.timeManager.RestartStopwatch_RenzokuRandomRule();
        }
        #endregion


    }
}
