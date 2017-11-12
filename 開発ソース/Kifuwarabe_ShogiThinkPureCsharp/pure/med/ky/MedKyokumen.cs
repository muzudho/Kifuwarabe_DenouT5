using kifuwarabe_shogithink.pure.conv.ky;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.software;

namespace kifuwarabe_shogithink.pure.med.ky
{
    public static class MedKyokumen
    {
        /// <summary>
        /// 現局面を、平手初期局面として登録します
        /// </summary>
        public static void TorokuHirate()
        {
            HirateShokiKyokumen.banjo = Conv_Kyokumen.ToKomaHairetu();
        }
    }
}
