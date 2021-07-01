#if DEBUG
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.com.hyoka;
using Grayscale.Kifuwarabi.Entities.Take1Base;
#else
using kifuwarabe_shogithink.pure.ky;
using Grayscale.Kifuwarabi.Entities.Take1Base;
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
            banjo = new Piece[]
            {
                Piece.k, Piece.r, Piece.z,
                Piece.Kuhaku, Piece.h, Piece.Kuhaku,
                Piece.Kuhaku, Piece.H, Piece.Kuhaku,
                Piece.Z, Piece.R, Piece.K
            };
        }

        /// <summary>
        /// FIXME: 本将棋に対応してない
        /// </summary>
        public static Piece[] banjo;

    }
}
