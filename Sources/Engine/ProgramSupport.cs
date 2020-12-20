#if DEBUG
using kifuwarabe_shogithink.pure;
using kifuwarabe_shogithink.pure.genkyoku;
using kifuwarabe_shogithink.pure.ikkyoku;
using kifuwarabe_shogithink.pure.ky;

using kifuwarabe_shogiwin.consolegame.console.command;
using kifuwarabe_shogiwin.consolegame.machine;
using kifuwarabe_shogiwin.speak.ban;
using System;
using System.Collections.Generic;
using kifuwarabe_shogithink.pure.accessor;
using kifuwarabe_shogithink.pure.control;
using System;
using Grayscale.Kifuwarabi.Entities.Logging;
using kifuwarabe_shogithink.pure;
using kifuwarabe_shogithink.pure.control;
using kifuwarabe_shogithink.pure.ikkyoku;
using kifuwarabe_shogithink.pure.ky;

using kifuwarabe_shogiwin.consolegame.console;
using kifuwarabe_shogiwin.consolegame.console.command;
using kifuwarabe_shogiwin.speak.ban;
#else
using System;
using System.Text;
using Grayscale.Kifuwarabi.Entities.Logging;
using Grayscale.Kifuwarabi.UseCases;
using kifuwarabe_shogithink.pure;
using kifuwarabe_shogithink.pure.control;
using kifuwarabe_shogithink.pure.ikkyoku;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.listen;
using kifuwarabe_shogithink.pure.listen.play;

using kifuwarabe_shogithink.pure.move;
using kifuwarabe_shogiwin.speak.ban;
#endif

namespace kifuwarabe_shogiwin.consolegame.console
{
    public class ProgramSupport
    {
        public ProgramSupport()
        {
        }

        public string commandline { get; set; }
        public int caret { get; set; }

        public bool isQuit { get; set; }
        public bool isKyokumenEcho1 { get; set; }


        public void InitCommandline()
        {
            commandline = null;// 空行とは区別するぜ☆（＾▽＾）
            caret = 0;
        }
        public void ClearCommandline()
        {
            commandline = "";
            caret = 0;
        }
        /// <summary>
        /// コマンドの誤発動防止
        /// </summary>
        public void CommentCommandline()
        {
            commandline = "#";
            caret = 0;
        }
        /// <summary>
        /// コマンド・バッファーから１行読取り。
        /// </summary>
        public void ReadCommandBuffer(Playing playing, StringBuilder hyoji)
        {
            if (0 < playing.commandBuffer.Count)
            {
                commandline = playing.commandBuffer[0];
                caret = 0;
                playing.commandBuffer.RemoveAt(0);
                hyoji.AppendLine(commandline);
            }
        }
        public void SetCommandline(string commandline)
        {
            this.commandline = commandline;
            caret = 0;
        }

        /// <summary>
        /// 次の入力を促す表示をしてるだけだぜ☆（＾～＾）
        /// </summary>
        public void ShowPrompt(Playing playing, FenSyurui f, StringBuilder hyoji)
        {
            if (0 < playing.commandBuffer.Count)
            {
                // コマンド・バッファーの実行中だぜ☆（＾▽＾）
                hyoji.Append(playing.commandBufferName + "> ");
                Logger.Flush(hyoji);
            }
            else if (GameMode.Game == PureAppli.gameMode)
            {
                // 表示（コンソール・ゲーム用）　局面、あれば勝敗☆（＾～＾）
                {
                    if (this.isKyokumenEcho1)
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

                        playing.Result(hyoji, CommandMode.NingenYoConsoleGame);
                    }
                    Logger.Flush(hyoji);
                }

                if (!playing.isMultipleLineCommand // 複数行コマンド読み取り中はプロンプトを出さないぜ☆（＾～＾）
                    &&
                    (PureMemory.kifu_teban == Taikyokusya.T1 && !PureSettei.p1Com)
                    ||
                    (PureMemory.kifu_teban == Taikyokusya.T2 && !PureSettei.p2Com)
                    )
                {
                    // 人間の手番が始まるところで☆
                    hyoji.Append(@"指し手を入力してください。一例　do B3B2　※ do b3b2 も同じ
> ");
                    Logger.Flush(hyoji);
                }
            }
            else
            {
                // 表示（コンソール・ゲーム用）
                hyoji.Append("> ");
                Logger.Flush(hyoji);
            }
        }

        public bool ParseDoMove(out Move out_move)
        {
            // コンソールからのキー入力を解析するぜ☆（＾▽＾）
            int caret = this.caret;
            int oldCaret = this.caret;

            Util_String.TobasuTangoToMatubiKuhaku(this.commandline, ref caret, "do ");

            // うしろに続く文字は☆（＾▽＾）
            if (!LisPlay.MatchFenMove(PureSettei.fenSyurui, this.commandline, ref caret, out out_move))
            {
                this.caret = oldCaret;

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
            this.caret = caret;
            this.CommentCommandline();// コマンドの誤発動防止
            return true;
        }

    }
}
