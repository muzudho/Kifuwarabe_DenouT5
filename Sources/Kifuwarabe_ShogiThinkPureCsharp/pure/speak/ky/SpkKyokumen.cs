#if DEBUG
using kifuwarabe_shogithink.pure.ky;
using System.Text;
#else
using kifuwarabe_shogithink.pure.ky;
using System.Text;
#endif

namespace kifuwarabe_shogithink.pure.speak.ky
{
    public static class SpkKyokumen
    {
        /// <summary>
        /// 局面を全角１文字の配列で表現
        /// </summary>
        /// <param name="yomiKy"></param>
        /// <returns></returns>
        public static string[] ToZen1Hairetu()
        {
            string[] ret = new string[PureSettei.banHeimen];
            for (int iMs = 0; iMs < PureSettei.banHeimen; iMs++)
            {
                Koma km = PureMemory.gky_ky.yomiKy.yomiShogiban.yomiIbashoBan.GetBanjoKoma((Masu)iMs);
                ret[iMs] = SpkKoma.Koma_To_Zen1(km);
            }
            return ret;
        }

        /// <summary>
        /// 局面を全角１文字の列で表現
        /// </summary>
        /// <param name="yomiKy"></param>
        /// <returns></returns>
        public static string ToZen1Mojiretu()
        {
            StringBuilder sb = new StringBuilder();
            for (int iMs = 0; iMs < PureSettei.banHeimen; iMs++)
            {
                Koma km = PureMemory.gky_ky.yomiKy.yomiShogiban.yomiIbashoBan.GetBanjoKoma((Masu)iMs);
                sb.Append(SpkKoma.Koma_To_Zen1(km));

                // 改行
                if(iMs%PureSettei.banYokoHaba== PureSettei.banYokoHaba - 1)
                {
                    sb.AppendLine();
                }
            }
            return sb.ToString();
        }
        /// <summary>
        /// 局面を全角１文字の列で表現
        /// </summary>
        /// <param name="yomiKy"></param>
        /// <returns></returns>
        public static string ToZen1Mojiretu(Koma[] komaHairetu)
        {
            StringBuilder sb = new StringBuilder();
            for (int iMs = 0; iMs < komaHairetu.Length; iMs++)
            {
                Koma km = komaHairetu[iMs];
                sb.Append(SpkKoma.Koma_To_Zen1(km));

                // 改行
                if (iMs % PureSettei.banYokoHaba == PureSettei.banYokoHaba - 1)
                {
                    sb.AppendLine();
                }
            }
            return sb.ToString();
        }
    }
}
