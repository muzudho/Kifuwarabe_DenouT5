namespace kifuwarabe_shogithink.pure.conv.ky
{
    using kifuwarabe_shogithink.pure.ky;
    using Grayscale.Kifuwarabi.Entities.Take1Base;

    public static class Conv_Kyokumen
    {
        /// <summary>
        /// 局面を駒の配列で表現
        /// </summary>
        /// <param name="yomiKy"></param>
        /// <returns></returns>
        public static Piece[] ToKomaHairetu()
        {
            Piece[] ret = new Piece[PureSettei.banHeimen];
            for (int iMs = 0; iMs < PureSettei.banHeimen; iMs++)
            {
                ret[iMs] = PureMemory.gky_ky.yomiKy.yomiShogiban.yomiIbashoBan.GetBanjoKoma((Masu)iMs);
            }
            return ret;
        }

    }
}
