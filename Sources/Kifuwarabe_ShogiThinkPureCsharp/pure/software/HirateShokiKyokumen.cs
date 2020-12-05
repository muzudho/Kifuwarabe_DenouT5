#if DEBUG
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.com.hyoka;
#else
using kifuwarabe_shogithink.pure.ky;
#endif

namespace kifuwarabe_shogithink.pure.software
{
    /// <summary>
    /// 平手初期局面
    /// </summary>
    public static class HirateShokiKyokumen
    {
        static HirateShokiKyokumen()
        {
            // どうぶつしょうぎの平手初期局面
            banjo = new Koma[]
            {
                Koma.k, Koma.r, Koma.z,
                Koma.Kuhaku, Koma.h, Koma.Kuhaku,
                Koma.Kuhaku, Koma.H, Koma.Kuhaku,
                Koma.Z, Koma.R, Koma.K
            };
        }

        /// <summary>
        /// FIXME: 本将棋に対応してない
        /// </summary>
        public static Koma[] banjo;

    }
}
