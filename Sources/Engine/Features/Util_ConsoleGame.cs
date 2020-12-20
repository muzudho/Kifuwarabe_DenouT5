#if DEBUG
using kifuwarabe_shogithink.pure.ky;

using kifuwarabe_shogiwin.consolegame.machine;
#else
using System.Text;
using Grayscale.Kifuwarabi.Entities.Logging;
using Grayscale.Kifuwarabi.UseCases;
using kifuwarabe_shogithink.pure.ky;

using kifuwarabe_shogiwin.consolegame.machine;
#endif

namespace kifuwarabe_shogiwin.consolegame.console
{
    /// <summary>
    /// コンソール画面用☆（＾～＾）
    /// </summary>
    public abstract class Util_ConsoleGame
    {
        /// <summary>
        /// メインループ開始時☆（＾▽＾）
        /// </summary>
        public static void Begin_Mainloop(Playing playing, ProgramSupport programSupport, StringBuilder hyoji)
        {
            programSupport.InitCommandline();// コマンド・ライン初期化☆
            programSupport.ReadCommandBuffer(playing, hyoji);// コマンド・バッファー読取り☆
        }

        public static void ReadCommandline(ProgramSupport programSupport, StringBuilder hyoji)
        {
            Logger.Flush(hyoji);
            programSupport.SetCommandline(Util_Machine.ReadLine());
            hyoji.AppendLine(programSupport.commandline);
            Logger.Flush_NoEcho(hyoji);
        }

        #region 定跡登録
        public static string KyFen_before { get; set; }
        public static Taikyokusya KyTaikyokusya_before { get; set; }

#endregion
    }
}
