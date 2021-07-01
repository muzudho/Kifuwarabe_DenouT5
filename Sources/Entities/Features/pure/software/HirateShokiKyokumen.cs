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
                Piece.R2, Piece.K2, Piece.B2,
                Piece.Kuhaku, Piece.P2, Piece.Kuhaku,
                Piece.Kuhaku, Piece.P1, Piece.Kuhaku,
                Piece.B1, Piece.K1, Piece.R1
            };
        }

        /// <summary>
        /// FIXME: 本将棋に対応してない
        /// </summary>
        public static Piece[] banjo;

    }
}
