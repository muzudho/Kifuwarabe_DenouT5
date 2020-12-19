#if DEBUG
using kifuwarabe_shogithink.pure.com.hyoka;
using kifuwarabe_shogithink.pure.ky;
#else
using kifuwarabe_shogithink.pure.com.hyoka;
using kifuwarabe_shogithink.pure.ky;
#endif

namespace kifuwarabe_shogithink.pure.ikkyoku
{
    public abstract class Util_Taikyoku
    {
        static Util_Taikyoku()
        {
            Clear();
        }

        /// <summary>
        /// 対局者Ｎ側に　何手詰め、または　何手詰められ　の評価が出たときの　手目数。
        /// </summary>
        public static int[] nantedumeTeme_playerN { get; set; }
        public static int[] nantedumeHyokati_playerN { get; set; }

        public static void Clear()
        {
            nantedumeTeme_playerN = new int[] { int.MaxValue, int.MaxValue };
            nantedumeHyokati_playerN = new int[] { Conv_Hyokati.Hyokati_Rei, Conv_Hyokati.Hyokati_Rei };
        }

        public static void Update(Hyokati hyokaSu, Taikyokusya taikyokusya)
        {
            if (Conv_Tumesu.None != hyokaSu.tumeSu)
            {
                // 詰め手数が表示されているぜ☆

                if (Util_Taikyoku.nantedumeTeme_playerN[(int)taikyokusya]==int.MaxValue)
                {
                    // 詰め手数が新たに表示されたようだぜ☆
                    Util_Taikyoku.nantedumeTeme_playerN[(int)taikyokusya] = PureMemory.kifu_endTeme;
                }
                // 前から表示されていたのなら、そのままだぜ☆（＾▽＾）
            }
            else
            {
                // 詰め手数は、表示されていないぜ☆

                if (Util_Taikyoku.nantedumeTeme_playerN[(int)taikyokusya] != int.MaxValue)
                {
                    // 詰め手数が消えたようだぜ☆
                    Util_Taikyoku.nantedumeTeme_playerN[(int)taikyokusya] = int.MaxValue;
                }
                // もともと表示されていなかったのなら、そのままだぜ☆（＾▽＾）
            }
        }
    }
}
