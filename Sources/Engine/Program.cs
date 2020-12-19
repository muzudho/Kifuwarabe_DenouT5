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

            var programSupport = new ProgramSupport();

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
            Util_ConsoleGame.ReadCommandline(programSupport, hyoji);
            if (programSupport.commandline == "usi")
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

                playing.UsiOk(programSupport.commandline, $"{engineName} {version.Major}.{version.Minor}.{version.Build}", engineAuthor, hyoji);
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
                Util_ConsoleGame.Begin_Mainloop(playing, programSupport, hyoji);
                if (programSupport.commandline != null)
                {
                    // コマンド・バッファーにコマンドラインが残っていたようなら、そのまま使うぜ☆（＾▽＾）
                }
                else if (
                    GameMode.Game == PureAppli.gameMode // ゲームモードの場合☆
                    &&
                    Console02.IsComputerNoBan() // コンピューターの番の場合☆
                    )
                {
                    programSupport.ClearCommandline(); // コマンドラインは消しておくぜ☆（＾▽＾）
                }
                else
                {

                    Util_ConsoleGame.ReadCommandline(programSupport, hyoji);// コンソールからのキー入力を受け取るぜ☆（＾▽＾）（コンソール・ゲーム用）
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
                        if (programSupport.caret != programSupport.commandline.IndexOf("do ", programSupport.caret))
                        {
                            // do以外のコマンドであれば、コマンドラインを保持したまま、そのまま続行
                        }
                        // 以下、do コマンドの場合☆
                        else if (!programSupport.ParseDoMove( out Move inputMove))
                        {
                            // do コマンドのパースエラー表示（コンソール・ゲーム用）☆（＾～＾）
                            SpkMove.AppendSetumei(MoveMatigaiRiyu.ParameterSyosikiMatigai, hyoji);
                            hyoji.AppendLine();
                            Logger.Flush(hyoji);
                            programSupport.CommentCommandline();// コマンドの誤発動防止
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
                            playing.Result(hyoji, CommandMode.NingenYoConsoleGame);
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
                        programSupport.CommentCommandline();
                    }
                    #endregion
                }
                #endregion
                #region （手順６）ゲーム用の指し手以外のコマンドライン実行
                //────────────────────────────────────────
                // （手順６）ゲーム用の指し手以外のコマンドライン実行
                //────────────────────────────────────────
                bool result2 = Pure.SUCCESSFUL_FALSE;
                string cmdline = programSupport.commandline;
                int caret = programSupport.caret;
                programSupport.isQuit = false;
                programSupport.isKyokumenEcho1 = false; // ゲーム・モードの場合、特に指示がなければ　コマンド終了後、局面表示を返すぜ☆

                if (playing.isMultipleLineCommand)
                {
                    // TODO: 複数行コマンド中☆（＾～＾）
                    //syuturyoku.AppendLine("TODO: ky set 複数行コマンド中☆（＾～＾）(2) commandline="+ commandline);
                    //isKyokumenEcho1 = false;
                    if (cmdline == ".")
                    {
                        // 「.」だけの行が来たら終了だぜ☆（＾～＾）
                        playing.isMultipleLineCommand = false;
                        // 実行☆（＾～＾）
                        playing.dlgt_multipleLineCommand(playing.multipleLineCommand);
                        playing.multipleLineCommand.Clear();
                        //syuturyoku.AppendLine("TODO: 複数行コマンドは=" + sbMultipleLineCommand.ToString());
                    }
                    else
                    {
                        playing.multipleLineCommand.Add(cmdline);
                    }
                    goto gt_EndCommand;
                }

                if (null == cmdline)
                {
                    // 未設定
                    programSupport.isKyokumenEcho1 = true;
                }
                else if (cmdline == "")
                {
                    programSupport.isKyokumenEcho1 = true;
                    // 空打ちは無視するか、からっぽモードでは、ゲームモードに切り替えるぜ☆（＾▽＾）
                    if (GameMode.Karappo == PureAppli.gameMode)// 感想戦での発動防止☆
                    {
                        // ゲームモード（対局開始）
                        PureAppli.gameMode = GameMode.Game;
                    }
                }
                // なるべく、アルファベット順☆（＾▽＾）同じつづりで始まる単語の場合、語句の長い単語を優先にしないと if 文が通らないぜ☆ｗｗｗ
                else if (caret == cmdline.IndexOf("@", caret)) { playing.Atmark(cmdline, hyoji); }
                else if (caret == cmdline.IndexOf("#", caret)) { }// 受け付けるが、何もしないぜ☆（＾▽＾）ｗｗｗ
                else if (caret == cmdline.IndexOf("bitboard", caret))
                {
                    // ビットボードの表示テスト用だぜ☆（＾～＾）
                    if (playing.TryFail_Bitboard(cmdline, hyoji))
                    {
                        result2 = Pure.FailTrue("TryFail_Bitboard");
                        goto gt_EndCommand;
                    }
                }
                else if (caret == cmdline.IndexOf("cando", caret))
                {
                    playing.CanDo(PureSettei.fenSyurui, cmdline, GameMode.Game == PureAppli.gameMode ? CommandMode.NingenYoConsoleGame : CommandMode.NigenYoConsoleKaihatu, hyoji);
                    programSupport.isKyokumenEcho1 = true;
                }
                else if (caret == cmdline.IndexOf("chikanhyo", caret))
                {
                    if (playing.TryFail_ChikanHyo(cmdline, hyoji
                        ))
                    {
                        result2 = Pure.FailTrue("TryFail_ChikanHyo");
                        goto gt_EndCommand;
                    }
                }
                else if (caret == cmdline.IndexOf("clear", caret)) { playing.Clear(); }
                else if (caret == cmdline.IndexOf("dosub", caret))
                {
                    if (playing.TryFail_DoSub(cmdline, hyoji))
                    {
                        result2 = Pure.FailTrue("TryFail_Do");
                        goto gt_EndCommand;
                    }
                    programSupport.isKyokumenEcho1 = true;
                }
                else if (caret == cmdline.IndexOf("do", caret))
                {
                    if (playing.TryFail_Do(
                        PureSettei.fenSyurui,
                        cmdline,
                        GameMode.Game == PureAppli.gameMode ? CommandMode.NingenYoConsoleGame : CommandMode.NigenYoConsoleKaihatu, hyoji
                        ))
                    {
                        result2 = Pure.FailTrue("TryFail_Do");
                        goto gt_EndCommand;
                    }
                    programSupport.isKyokumenEcho1 = true;
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
                    if (playing.TryFail_Fugo(cmdline, hyoji
                        ))
                    {
                        result2 = Pure.FailTrue("TryFail_Fugo");
                        goto gt_EndCommand;
                    }
                }
                else if (caret == cmdline.IndexOf("gameover", caret)) { playing.Gameover(cmdline, hyoji); programSupport.isKyokumenEcho1 = true; }
                else if (caret == cmdline.IndexOf("go", caret))
                {
                    var result3 = playing.TryFail_Go(
                        PureSettei.usi,
                        PureSettei.fenSyurui,
                        CommandMode.NigenYoConsoleKaihatu
                        , hyoji
                        );
                    if (result3)
                    {
                        result2 = Pure.FailTrue("Try_Go");
                        goto gt_EndCommand;
                    }
                }
                else if (caret == cmdline.IndexOf("hirate", caret)) { playing.Hirate(cmdline, hyoji); programSupport.isKyokumenEcho1 = true; }
                else if (caret == cmdline.IndexOf("honyaku", caret)) { playing.Honyaku(cmdline, hyoji); }
                else if (caret == cmdline.IndexOf("hyoka", caret)) { playing.Hyoka(cmdline, hyoji); }
                else if (caret == cmdline.IndexOf("ojama", caret))
                {
                    if (playing.TryFail_Ojama(cmdline, hyoji
                        ))
                    {
                        result2 = Pure.FailTrue("TryFail_Ojama");
                        goto gt_EndCommand;
                    }
                }
                else if (caret == cmdline.IndexOf("isready", caret)) { playing.ReadOk(cmdline, hyoji); }
                else if (caret == cmdline.IndexOf("jokyo", caret)) { playing.Jokyo(cmdline, hyoji); }
                else if (caret == cmdline.IndexOf("kansosen", caret)) { playing.Kansosen(PureSettei.fenSyurui, cmdline, hyoji); }// 駒の場所を表示するぜ☆（＾▽＾）
                else if (caret == cmdline.IndexOf("kifu", caret)) { playing.Kifu(PureSettei.fenSyurui, cmdline, hyoji); }// 駒の場所を表示するぜ☆（＾▽＾）
                else if (caret == cmdline.IndexOf("kikisu", caret))
                {
                    // 利きの数を調べるぜ☆（＾▽＾）
                    // 旧名「kikikazu」→「kikisu」
                    playing.Kikisu(cmdline, hyoji);
                }
                else if (caret == cmdline.IndexOf("kiki", caret))
                {
                    // 利きを調べるぜ☆（＾▽＾）
                    var result3 = playing.TryFail_Kiki(cmdline, hyoji);
                    if (result3)
                    {
                        result2 = Pure.FailTrue("TryFail_Kiki");
                        goto gt_EndCommand;
                    }
                }
                else if (caret == cmdline.IndexOf("koma", caret))
                {
                    Pure.Sc.Push("komaコマンド");
                    playing.Koma_cmd( PureSettei.fenSyurui, cmdline, hyoji);
                    Pure.Sc.Pop();
                }// 駒の場所を表示するぜ☆（＾▽＾）
                else if (caret == cmdline.IndexOf("ky", caret))
                {
                    // 局面をクリアーしてやり直すときもここを通るので、ここで局面アサートを入れてはいけないぜ☆（＾～＾）

                    if (playing.TryFail_Ky(cmdline, hyoji))
                    {
                        result2 = Pure.FailTrue("Try_Ky");
                        goto gt_EndCommand;
                    }

                }// 局面を表示するぜ☆（＾▽＾）
                else if (caret == cmdline.IndexOf("manual", caret)) { playing.Man(hyoji); }// "man" と同じ☆（＾▽＾）
                else if (caret == cmdline.IndexOf("man", caret)) { playing.Man(hyoji); }// "manual" と同じ☆（＾▽＾）
                else if (caret == cmdline.IndexOf("masu", caret)) { playing.Masu_cmd(cmdline, hyoji); }
                else if (caret == cmdline.IndexOf("nanamedan", caret))
                {
                    if (playing.TryFail_Nanamedan(cmdline, hyoji
                        ))
                    {
                        result2 = Pure.FailTrue("TryFail_Nanamedan");
                        goto gt_EndCommand;
                    }
                }
                else if (caret == cmdline.IndexOf("nisinsu", caret))
                {
                    if (playing.TryFail_Nisinsu(cmdline, hyoji
                        ))
                    {
                        result2 = Pure.FailTrue("TryFail_Nisinsu");
                        goto gt_EndCommand;
                    }
                }
                else if (caret == cmdline.IndexOf("position", caret)) { playing.Position(PureSettei.fenSyurui, cmdline, hyoji); }
                else if (caret == cmdline.IndexOf("prego", caret)) { playing.PreGo(cmdline, hyoji); }
                else if (caret == cmdline.IndexOf("quit", caret)) { programSupport.isQuit = true; programSupport.isKyokumenEcho1 = true; }
                else if (caret == cmdline.IndexOf("result", caret)) { playing.Result(hyoji, CommandMode.NigenYoConsoleKaihatu); }
                else if (caret == cmdline.IndexOf("rnd", caret))
                {
                    if (!playing.Try_Rnd(
#if DEBUG
                    (IDebugMojiretu)hyoji
#endif
                ))
                    {
                        result2 = Pure.FailTrue("commandline");
                        goto gt_EndCommand;
                    }
                    programSupport.isKyokumenEcho1 = true;
                }
                else if (caret == cmdline.IndexOf("move", caret))
                {
                    if (playing.TryFail_Move_cmd(cmdline, hyoji))
                    {
                        result2 = Pure.FailTrue("TryFail_Move_cmd");
                        goto gt_EndCommand;
                    }
                }
                else if (caret == cmdline.IndexOf("setoption", caret)) { playing.Setoption(cmdline, hyoji); }
                else if (caret == cmdline.IndexOf("set", caret)) { playing.Set(cmdline, hyoji); }
                else if (caret == cmdline.IndexOf("taikyokusya", caret)) { playing.Taikyokusya_cmd(cmdline, hyoji); }
                else if (caret == cmdline.IndexOf("tansaku", caret)) { playing.Tansaku(cmdline, hyoji); }
                else if (caret == cmdline.IndexOf("test", caret))
                {
                    if (playing.TryFail_Test(cmdline, hyoji))
                    {
                        result2 = Pure.FailTrue("TryFail_Test");
                        goto gt_EndCommand;
                    }
                }
                else if (caret == cmdline.IndexOf("tonarikiki", caret))
                {
                    if (playing.TryFail_Tonarikiki(cmdline, hyoji))
                    {
                        result2 = Pure.FailTrue("TryFail_Tonarikiki");
                        goto gt_EndCommand;
                    }
                }
                else if (caret == cmdline.IndexOf("tumeshogi", caret)) { playing.TumeShogi(PureSettei.fenSyurui, cmdline, hyoji); }// "tu" と同じ☆（＾▽＾）
                else if (caret == cmdline.IndexOf("tu", caret)) { playing.TumeShogi(PureSettei.fenSyurui, cmdline, hyoji); }// "tumeshogi" と同じ☆（＾▽＾）
                else if (caret == cmdline.IndexOf("ugokikata", caret))
                {
                    if (playing.TryFail_Ugokikata(cmdline, hyoji))
                    {
                        result2 = Pure.FailTrue("TryFail_Ugokikata");
                        goto gt_EndCommand;
                    }
                }
                else if (caret == cmdline.IndexOf("undo", caret))
                {
                    playing.Undo(cmdline, hyoji);
                }
                else if (caret == cmdline.IndexOf("updaterule", caret))
                {
                    playing.UpdateRule(cmdline, hyoji);
                }
                else if (caret == cmdline.IndexOf("usinewgame", caret)) { playing.UsiNewGame(cmdline, hyoji); }
                else if (caret == cmdline.IndexOf("usi", caret))
                {
                    //ここは普通、来ない☆（＾～＾）
                    var profilePath = System.Configuration.ConfigurationManager.AppSettings["Profile"];
                    var toml = Toml.ReadFile(Path.Combine(profilePath, "Engine.toml"));

                    var engineName = toml.Get<TomlTable>("Engine").Get<string>("Name");
                    Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                    var engineAuthor = toml.Get<TomlTable>("Engine").Get<string>("Author");

                    playing.UsiOk(cmdline, $"{engineName} {version.Major}.{version.Minor}.{version.Build}", engineAuthor, hyoji);
                }
                else
                {
                    // 表示（コンソール・ゲーム用）
                    hyoji.Append("「");
                    hyoji.Append(cmdline);
                    hyoji.AppendLine("」☆？（＾▽＾）");

                    hyoji.AppendLine("そんなコマンドは無いぜ☆（＞＿＜） man で調べろだぜ☆（＾▽＾）");
                    Logger.Flush(hyoji);
                    programSupport.isKyokumenEcho1 = true;
                }
            gt_EndCommand:

                if (result2)
                {
                    result1 = Pure.FailTrue("Try_DoCommandline");
                    goto gt_EndLoop1;
                }

                if (programSupport.isQuit)
                {
                    break;//goto gt_EndLoop1;
                }

                // 次の入力を促す表示をしてるだけだぜ☆（＾～＾）
                programSupport.ShowPrompt(playing, PureSettei.fenSyurui, hyoji);

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
