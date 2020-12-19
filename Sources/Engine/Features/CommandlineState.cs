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
using Nett;
using System.IO;
using Grayscale.Kifuwarabi.Entities.Logging;
using Grayscale.Kifuwarabi.Entities;
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
        public static bool isMultipleLineCommand;
        public static List<string> multipleLineCommand;
        public static void DoMultipleLineCommand(DLGT_MultipleLineCommand dlgt_multipleLineCommand)
        {
            isMultipleLineCommand = true;
            //isKyokumenEcho1 = false;
            CommandlineState.dlgt_multipleLineCommand = dlgt_multipleLineCommand;
        }
        public delegate void DLGT_MultipleLineCommand(List<string> multipleLineCommand);
        public static DLGT_MultipleLineCommand dlgt_multipleLineCommand;
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
    }
}
