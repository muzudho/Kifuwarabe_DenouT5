#if DEBUG
using kifuwarabe_shogithink.pure;
using kifuwarabe_shogiwin.consolegame.yomisuji;
#else
using kifuwarabe_shogithink.pure;
using kifuwarabe_shogiwin.consolegame.yomisuji;
#endif

namespace kifuwarabe_shogiwin.consolegame
{
    public static class ConsolegameSettei
    {
        static ConsolegameSettei()
        {
            renzokuTaikyoku = false;
        }

        /// <summary>
        /// 設定の上書き
        /// </summary>
        public static void Init_PureAppliOverride()
        {
            // 読み筋出力
            PureAppli.dlgt_CreateJoho = Util_JohoWinconsole.Dlgt_WriteYomisujiJoho;
        }

        /// <summary>
        /// アプリケーションを強制終了するまで、ノンストップの対局だぜ☆（＾▽＾）
        /// </summary>
        public static bool renzokuTaikyoku { get; set; }
    }
}
