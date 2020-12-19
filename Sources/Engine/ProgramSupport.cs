﻿#if DEBUG
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
using System;
using Grayscale.Kifuwarabi.Entities.Logging;
using kifuwarabe_shogithink.pure;
using kifuwarabe_shogithink.pure.control;
using kifuwarabe_shogithink.pure.ikkyoku;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.logger;
using kifuwarabe_shogiwin.consolegame.console;
using kifuwarabe_shogiwin.consolegame.console.command;
using kifuwarabe_shogiwin.speak.ban;
#else
using System;
using System.Collections.Generic;
using Grayscale.Kifuwarabi.Entities.Logging;
using kifuwarabe_shogithink.pure;
using kifuwarabe_shogithink.pure.control;
using kifuwarabe_shogithink.pure.ikkyoku;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.logger;
using kifuwarabe_shogiwin.consolegame.console.command;
using kifuwarabe_shogiwin.speak.ban;
#endif

namespace kifuwarabe_shogiwin.consolegame.console
{
    public class ProgramSupport
    {
        public ProgramSupport()
        {
            commandBufferName = "";
            commandBuffer = new List<string>(0);
            multipleLineCommand = new List<string>();
        }

        public string commandline { get; set; }
        public int caret { get; set; }
        public string commandBufferName { get; set; }
        public List<string> commandBuffer { get; set; }
        public bool isQuit { get; set; }
        public bool isKyokumenEcho1 { get; set; }

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
        public void ReadCommandBuffer(IHyojiMojiretu hyoji)
        {
            if (0 < commandBuffer.Count)
            {
                commandline = commandBuffer[0];
                caret = 0;
                commandBuffer.RemoveAt(0);
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
        public void ShowPrompt(FenSyurui f, IHyojiMojiretu hyoji)
        {
            if (0 < this.commandBuffer.Count)
            {
                // コマンド・バッファーの実行中だぜ☆（＾▽＾）
                hyoji.Append(this.commandBufferName + "> ");
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

                        CommandR.Result(hyoji, CommandMode.NingenYoConsoleGame);
                    }
                    Logger.Flush(hyoji);
                }

                if (!this.isMultipleLineCommand // 複数行コマンド読み取り中はプロンプトを出さないぜ☆（＾～＾）
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
    }
}
