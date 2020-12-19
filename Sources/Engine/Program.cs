#if DEBUG
using kifuwarabe_shogithink.pure;
using kifuwarabe_shogithink.pure.com;
using kifuwarabe_shogithink.pure.control;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.listen.genkyoku;
using kifuwarabe_shogithink.pure.logger;
using kifuwarabe_shogiwin.consolegame;
using kifuwarabe_shogiwin.consolegame.console;
using kifuwarabe_shogiwin.consolegame.console.command;
using kifuwarabe_shogiwin.consolegame.machine;
using kifuwarabe_shogiwin.project;
using kifuwarabe_shogiwin.speak;
using System;
using System.Collections.Generic;
#else
using System;
using System.Collections.Generic;
using System.IO;
using Grayscale.Kifuwarabi.Entities.Logging;
using Grayscale.Kifuwarabi.UseCases;
using kifuwarabe_shogithink.pure;
using kifuwarabe_shogithink.pure.accessor;
using kifuwarabe_shogithink.pure.com;
using kifuwarabe_shogithink.pure.control;
using kifuwarabe_shogithink.pure.genkyoku;
using kifuwarabe_shogithink.pure.ikkyoku;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.listen.genkyoku;
using kifuwarabe_shogithink.pure.logger;
using kifuwarabe_shogithink.pure.move;
using kifuwarabe_shogithink.pure.speak.genkyoku;
using kifuwarabe_shogithink.pure.speak.play;
using kifuwarabe_shogiwin.consolegame;
using kifuwarabe_shogiwin.consolegame.console;
using kifuwarabe_shogiwin.consolegame.console.command;
using kifuwarabe_shogiwin.consolegame.machine;
using kifuwarabe_shogiwin.project;
using kifuwarabe_shogiwin.speak;
using kifuwarabe_shogiwin.speak.ban;
using Nett;
#endif

namespace kifuwarabe_shogiwin
{
    public class Program
    {

        /// <summary>
        /// ここからコンソール・アプリケーションが始まるぜ☆（＾▽＾）
        /// 
        /// ＰＣのコンソール画面のプログラムなんだぜ☆（＾▽＾）
        /// Ｕｎｉｔｙでは中身は要らないぜ☆（＾～＾）
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            var playing = new Playing();

            // （手順２）きふわらべの応答は、文字列になって　ここに入るぜ☆（＾▽＾）
            // syuturyoku.ToContents() メソッドで中身を取り出せるぜ☆（＾～＾）
            IHyojiMojiretu hyoji = PureAppli.syuturyoku1;
            Interproject.project = new WinconsoleProject();

            if (PureAppli.TryFail_Init())
            {
                Logger.Flush(hyoji);
                throw new Exception(hyoji.ToContents());
            }

            // コンソールゲーム用の設定上書き
            ConsolegameSettei.Init_PureAppliOverride();



            // まず最初に「USI\n」が届くかどうかを判定☆（＾～＾）
            Util_ConsoleGame.ReadCommandline(hyoji);
            if (CommandlineState.commandline=="usi")
            {
                // 「将棋所」で本将棋を指す想定☆（＾～＾）
                // CommandA.Atmark("@USI9x9", hyoji);

                PureSettei.usi = true;
                PureSettei.fenSyurui = FenSyurui.sfe_n;

                PureSettei.p1Com = false;
                PureSettei.p2Com = false;
                PureSettei.tobikikiTukau = true; // FIXME: 飛び利きはまだ不具合修正されていないぜ☆（＾～＾）
                ComSettei.himodukiHyokaTukau = true; // FIXME: 紐付き評価は、使うとしておこう☆（＾～＾）
                // ルールを確定してから　局面を作れだぜ☆（＾～＾）
                LisGenkyoku.SetRule(
                    GameRule.HonShogi, 9, 9,
@"シウネイライネウシ
　キ　　　　　ゾ　
ヒヒヒヒヒヒヒヒヒ
　　　　　　　　　
　　　　　　　　　
　　　　　　　　　
ひひひひひひひひひ
　ぞ　　　　　き　
しうねいらいねうし"
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

                var profilePath = System.Configuration.ConfigurationManager.AppSettings["Profile"];
                var toml = Toml.ReadFile(Path.Combine(profilePath, "Engine.toml"));

                var engineName = toml.Get<TomlTable>("Engine").Get<string>("Name");
                Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                var engineAuthor = toml.Get<TomlTable>("Engine").Get<string>("Author");

                playing.UsiOk(CommandlineState.commandline, $"{engineName} {version.Major}.{version.Minor}.{version.Build}", engineAuthor, hyoji);
            }
            else
            {
                SpkNaration.Speak_TitleGamen(hyoji);// とりあえず、タイトル画面表示☆（＾～＾）
                Logger.Flush(hyoji);
            }

            //Face_Kifuwarabe.Execute("", Option_Application.Kyokumen, syuturyoku); // 空打ちで、ゲームモードに入るぜ☆（＾▽＾）
            // 空打ちで、ゲームモードに入るぜ☆（＾▽＾）
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

            bool result1 = Pure.SUCCESSFUL_FALSE;
            for (; ; )//メインループ（無限ループ）
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
                        else if (!Console02.ParseDoMove(out Move inputMove))
                        {
                            // do コマンドのパースエラー表示（コンソール・ゲーム用）☆（＾～＾）
                            SpkMove.AppendSetumei(MoveMatigaiRiyu.ParameterSyosikiMatigai, hyoji);
                            hyoji.AppendLine();
                            Logger.Flush(hyoji);
                            CommandlineState.CommentCommandline();// コマンドの誤発動防止
                        }
                        else if (!GenkyokuOpe.CanDoMove(inputMove, out MoveMatigaiRiyu reason))// 指し手の合否チェック
                        {
                            // イリーガル・ムーブなどの、エラー理由表示☆（＾～＾）
                            SpkMove.AppendSetumei(reason, hyoji);
                            hyoji.AppendLine();
                            Logger.Flush(hyoji);
                        }
                        else
                        {
                            // do コマンドを実行するぜ☆（＾▽＾）

                            // １手指す☆！（＾▽＾）
                            if (!Util_Control.Try_DoMove_Input(inputMove
#if DEBUG
                                , PureSettei.fenSyurui
                                , (IDebugMojiretu)hyoji
#endif
                                ))
                            {
                                result1 = Pure.FailTrue("Try_DoMove_Input");
                                goto gt_EndLoop1;
                            }
                            // 勝敗判定☆（＾▽＾）
                            if (!Util_Kettyaku.Try_JudgeKettyaku(inputMove
#if DEBUG
                                , hyoji
#endif
                                ))
                            {
                                result1 = Pure.FailTrue("Try_JudgeKettyaku");
                                goto gt_EndLoop1;
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
                        SpkNaration.Speak_ComputerSikochu(hyoji);// 表示（コンピューター思考中☆）
                        Logger.Flush(hyoji);

                        // コンピューターに１手指させるぜ☆
                        Util_Tansaku.PreGo();
                        if (Util_Tansaku.TryFail_Go(hyoji))
                        {
                            result1 = Pure.FailTrue("Try_Go");
                            goto gt_EndLoop1;
                        }
                        // 勝敗判定☆（＾▽＾）
                        if (!Util_Kettyaku.Try_JudgeKettyaku(PureMemory.tnsk_kohoMove
#if DEBUG
                            , hyoji
#endif
                            ))
                        {
                            result1 = Pure.FailTrue("Try_JudgeKettyaku");
                            goto gt_EndLoop1;
                        }

                        SpkNaration.Speak_KettyakuJi(hyoji);// 決着していた場合はメッセージ表示☆（＾～＾）
                        Logger.Flush(hyoji);
                    }// コンピューターの手番おわり☆（＾～＾）
                    #endregion
                    #region （手順５）決着時
                    //────────────────────────────────────────
                    // （手順５）決着時
                    //────────────────────────────────────────
                    if (Genkyoku.IsKettyaku())// 決着が付いているなら☆
                    {
                        // 対局終了時
                        // 表示（コンソール・ゲーム用）
                        {
                            CommandR.Result(hyoji, CommandMode.NingenYoConsoleGame);
                            hyoji.AppendLine("終わったぜ☆（＾▽＾）");
                            Logger.Flush(hyoji);
                        }



                        // 棋譜の初期局面を更新☆
                        {
                            ICommandMojiretu kyFen_temp = new MojiretuImpl();
                            SpkGenkyokuOpe.AppendFenTo(PureSettei.fenSyurui, kyFen_temp);
                            PureMemory.kifu_syokiKyokumenFen = kyFen_temp.ToContents();
                        }


                        // TODO: 成績は保存しないにしても、棋譜は欲しいときもあるぜ☆（＾～＾）
                        // 棋譜を作ろうぜ☆
                        hyoji.AppendLine("感想戦を行う場合は kansosen と打てだぜ☆（＾▽＾）　そのあと kifu 1 とか打て☆（＾▽＾）");
                        hyoji.AppendLine("終わるときは hirate な☆（＾▽＾）");
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
                    #endregion
                }
                #endregion
                #region （手順６）ゲーム用の指し手以外のコマンドライン実行
                //────────────────────────────────────────
                // （手順６）ゲーム用の指し手以外のコマンドライン実行
                //────────────────────────────────────────
                if (CommandlineState.TryFail_DoCommandline(playing, hyoji))
                {
                    result1 = Pure.FailTrue("Try_DoCommandline");
                    goto gt_EndLoop1;
                }

                if (CommandlineState.isQuit)
                {
                    break;//goto gt_EndLoop1;
                }

                // 次の入力を促す表示をしてるだけだぜ☆（＾～＾）
                CommandlineState.ShowPrompt(PureSettei.fenSyurui, hyoji);

                #endregion

            }//無限ループ
            gt_EndLoop1:
            ;

            if (result1)
            {
                Logger.Flush(hyoji);
                Console.WriteLine("おわり☆（＾▽＾）");
                Console.ReadKey();
                //throw new Exception(syuturyoku.ToContents());
            }
            // 開発モードでは、ユーザー入力を待機するぜ☆（＾▽＾）

            //────────────────────────────────────────
            // （手順７）保存して終了
            //────────────────────────────────────────
            // 保存していないものを保存するぜ☆（＾▽＾）
            // ファイルに書き出していないログが溜まっていれば、これで全部書き出します。
            Logger.Flush(PureAppli.syuturyoku1);
        }

    }
}
