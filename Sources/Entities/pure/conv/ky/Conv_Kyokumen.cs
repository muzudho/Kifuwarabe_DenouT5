using kifuwarabe_shogithink.pure.ky;

namespace kifuwarabe_shogithink.pure.conv.ky
{
    public static class Conv_Kyokumen
    {
        /// <summary>
        /// 局面を駒の配列で表現
        /// </summary>
        /// <param name="yomiKy"></param>
        /// <returns></returns>
        public static Koma[] ToKomaHairetu()
        {
            Koma[] ret = new Koma[PureSettei.banHeimen];
            for (int iMs = 0; iMs < PureSettei.banHeimen; iMs++)
            {
                ret[iMs] = PureMemory.gky_ky.yomiKy.yomiShogiban.yomiIbashoBan.GetBanjoKoma((Masu)iMs);
            }
            return ret;
        }

    }
}
