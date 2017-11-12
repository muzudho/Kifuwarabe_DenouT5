#if DEBUG
using kifuwarabe_shogithink.pure;
using kifuwarabe_shogithink.pure.com;
using kifuwarabe_shogithink.pure.control;
using kifuwarabe_shogithink.pure.genkyoku;
using kifuwarabe_shogithink.pure.ikkyoku;
using kifuwarabe_shogithink.pure.logger;
using kifuwarabe_shogithink.pure.sasite;
using kifuwarabe_shogithink.pure.speak.play;
using kifuwarabe_shogiwin.consolegame.machine;
using kifuwarabe_shogiwin.speak;
using kifuwarabe_shogiwin.speak.ban;
#else
using kifuwarabe_shogithink.pure;
using kifuwarabe_shogithink.pure.com;
using kifuwarabe_shogithink.pure.control;
using kifuwarabe_shogithink.pure.genkyoku;
using kifuwarabe_shogithink.pure.ikkyoku;
using kifuwarabe_shogithink.pure.logger;
using kifuwarabe_shogithink.pure.sasite;
using kifuwarabe_shogithink.pure.speak.play;
using kifuwarabe_shogiwin.consolegame.machine;
using kifuwarabe_shogiwin.speak;
using kifuwarabe_shogiwin.speak.ban;
#endif

namespace kifuwarabe_shogiwin.consolegame.console
{
    public static class Console01
    {
        /// <summary>
        /// コマンドラインを実行するぜ☆（＾▽＾）
        /// </summary>
        /// <param name="line">コマンドライン☆</param>
        /// <param name="hyoji">実行結果☆</param>
        public static bool TryFail_Execute(string line, IHyojiMojiretu hyoji)
        {
            // このプログラムでは（Ａ）コマンド・モード、（Ｂ）ゲーム・モード　の２種類があるぜ☆
            // 最初は　コマンド・モードになっている☆（＾～＾）
            //
            // ゲームモード
            //      （１）手番
            //              人間、コンピューターの設定が有効になり、
            //              人間の手番のときにしかコマンドが打てなくなるぜ☆
            //      （２）合法手
            //              指し手の合法手チェックを行うぜ☆
            //      （３）自動着手
            //              コンピューターは自分の手番で 指すぜ☆
            //      （４）決着
            //              決着すると　ゲームモード　を抜けるぜ☆ 連続対局設定の場合は抜けない☆（＾▽＾）
            //
            // コマンドモード
            //      （１）手番
            //              ＭＡＮ vs ＭＡＮ扱い
            //      （２）合法手
            //              チェックしない☆　ひよこをナナメに進めるのも、ワープするのも可能☆
            //      （３）自動着手
            //              しない☆
            //      （４）決着
            //              しない☆ [Enter]キーを空打ちすると、ゲームモードに変わるぜ☆（＾▽＾）

            for (;;)//メインループ（無限ループ）
            {
#region （手順２）ユーザー入力
                //────────────────────────────────────────
                // （手順２）ユーザー入力
                //────────────────────────────────────────
                Util_ConsoleGame.Begin_Mainloop(hyoji);
                if (CommandlineState.commandline != null)
                {
                    // コマンド・バッファーにコマンドラインが残っていたようなら、そのまま使うぜ☆（＾▽＾）
                }
                else if (
                    GameMode.Game == PureAppli.gameMode // ゲームモードの場合☆
                    &&
                    Console02.IsComputerNoBan() // コンピューターの番の場合☆
                    )
                {
                    CommandlineState.ClearCommandline(); // コマンドラインは消しておくぜ☆（＾▽＾）
                }
                else
                {

                    Util_ConsoleGame.ReadCommandline(hyoji);// コンソールからのキー入力を受け取るぜ☆（＾▽＾）（コンソール・ゲーム用）
                }
#endregion

#region ゲームセクション
                if (GameMode.Game == PureAppli.gameMode)
                {
                    #region 手番の開始時
                    //────────────────────────────────────────
                    // 手番の開始時
                    //────────────────────────────────────────

                    // 手番の開始時に　何かやることがあれば　ここに書けだぜ☆（＾～＾）
                    #endregion

                    #region （手順３）人間の手番
                    //────────────────────────────────────────
                    // （手順３）人間の手番
                    //────────────────────────────────────────
                    if (Console02.IsNingenNoBan()) // 人間の手番
                    {
                        // ゲームモードでの人間の手番では、さらにコマンド解析

                        // ここで do コマンド（do b3b2 等）を先行して解析するぜ☆（＾▽＾）
                        if (CommandlineState.caret != CommandlineState.commandline.IndexOf("do ", CommandlineState.caret))
                        {
                            // do以外のコマンドであれば、コマンドラインを保持したまま、そのまま続行
                        }
                        // 以下、do コマンドの場合☆
                        else if (!Console02.ParseDoSasite(out Sasite inputSasite))
                        {
                            // do コマンドのパースエラー表示（コンソール・ゲーム用）☆（＾～＾）
                            SpkSasite.AppendSetumei(SasiteMatigaiRiyu.ParameterSyosikiMatigai, hyoji);
                            hyoji.AppendLine();
                            Util_Machine.Flush(hyoji);
                            CommandlineState.CommentCommandline();// コマンドの誤発動防止
                        }
                        else if (!GenkyokuOpe.CanDoSasite( inputSasite, out SasiteMatigaiRiyu reason))// 指し手の合否チェック
                        {
                            // イリーガル・ムーブなどの、エラー理由表示☆（＾～＾）
                            SpkSasite.AppendSetumei(reason, hyoji);
                            hyoji.AppendLine();
                            Util_Machine.Flush(hyoji);
                        }
                        else
                        {
                            // do コマンドを実行するぜ☆（＾▽＾）

                            // １手指す☆！（＾▽＾）
                            if (!Util_Control.Try_DoSasite_Input(inputSasite
#if DEBUG
                                , PureSettei.fenSyurui
                                , (IDebugMojiretu)hyoji
#endif
                                ))
                            {
                                return Pure.FailTrue("Try_DoSasite_Input");
                            }
                            // 勝敗判定☆（＾▽＾）
                            if (!Util_Kettyaku.Try_JudgeKettyaku(inputSasite
#if DEBUG
                                , hyoji
#endif
                                ))
                            {
                                return Pure.FailTrue("Try_JudgeKettyaku");
                            }

                            // 局面出力
                            SpkBan_1Column.Setumei_NingenGameYo(PureMemory.kifu_endTeme, hyoji);
                        }

                    }// 人間おわり☆（＾▽＾）
#endregion
#region （手順４）コンピューターの手番
                    //────────────────────────────────────────
                    // （手順４）コンピューターの手番
                    //────────────────────────────────────────
                    else if (Console02.IsComputerNoBan())//コンピューターの番☆
                    {
                        SpkNaration.Speak_ComputerSikochu( hyoji);// 表示（コンピューター思考中☆）
                        Util_Machine.Flush(hyoji);

                        // コンピューターに１手指させるぜ☆
                        Util_Tansaku.PreGo();
                        if (Util_Tansaku.TryFail_Go(hyoji))
                        {
                            return Pure.FailTrue("Try_Go");
                        }
                        // 勝敗判定☆（＾▽＾）
                        if (!Util_Kettyaku.Try_JudgeKettyaku(PureMemory.tnsk_kohoSasite
#if DEBUG
                            , hyoji
#endif
                            ))
                        {
                            return Pure.FailTrue("Try_JudgeKettyaku");
                        }

                        SpkNaration.Speak_KettyakuJi(hyoji);// 決着していた場合はメッセージ表示☆（＾～＾）
                        Util_Machine.Flush(hyoji);
                    }// コンピューターの手番おわり☆（＾～＾）
#endregion
                    #region （手順５）決着時
                    //────────────────────────────────────────
                    // （手順５）決着時
                    //────────────────────────────────────────
                    if (Genkyoku.IsKettyaku())// 決着が付いているなら☆
                    {
                        Console02.DoTejun5_SyuryoTaikyoku1( hyoji);// 対局終了時
                    }
#endregion
                }
#endregion
#region （手順６）ゲーム用の指し手以外のコマンドライン実行
                //────────────────────────────────────────
                // （手順６）ゲーム用の指し手以外のコマンドライン実行
                //────────────────────────────────────────
                if(CommandlineState.TryFail_DoCommandline( hyoji))
                {
                    return Pure.FailTrue("Try_DoCommandline");
                }

                if (CommandlineState.isQuit)
                {
                    break;//goto gt_EndLoop1;
                }

                // 次の入力を促す表示をしてるだけだぜ☆（＾～＾）
                CommandlineState.ShowPrompt(PureSettei.fenSyurui, hyoji);

#endregion

            }//無限ループ
            //gt_EndLoop1:
            //;
            return Pure.SUCCESSFUL_FALSE;
        }


    }
}
