#if DEBUG
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.logger;
using kifuwarabe_shogiwin.consolegame.machine;
#else
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.logger;
using kifuwarabe_shogiwin.consolegame.machine;
#endif

namespace kifuwarabe_shogiwin.consolegame.console
{
    /// <summary>
    /// Unityでは使わないだろうもの☆（＾～＾）
    /// </summary>
    public abstract class Util_ConsoleGame
    {
        /// <summary>
        /// メインループ開始時☆（＾▽＾）
        /// </summary>
        public static void Begin_Mainloop(IHyojiMojiretu hyoji)
        {
            CommandlineState.InitCommandline();// コマンド・ライン初期化☆
            CommandlineState.ReadCommandBuffer(hyoji);// コマンド・バッファー読取り☆
        }

        public static void ReadCommandline(IHyojiMojiretu hyoji)
        {
            Util_Machine.Flush(hyoji);
            CommandlineState.SetCommandline(Util_Machine.ReadLine());
            hyoji.AppendLine(CommandlineState.commandline);
            Util_Machine.Flush_NoEcho(hyoji);
        }

        #region 定跡登録
        public static string KyFen_before { get; set; }
        public static Taikyokusya KyTaikyokusya_before { get; set; }

#endregion
    }
}
