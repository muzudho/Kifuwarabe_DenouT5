#if DEBUG
using System;
using System.Diagnostics;
using kifuwarabe_shogithink.pure.ky.tobikiki;
#else
using System;
using System.Diagnostics;
using kifuwarabe_shogithink.pure.ky.tobikiki;
#endif

namespace kifuwarabe_shogithink.pure.ky.bb
{
    /// <summary>
    /// ローテート・ビットボードの置換表
    /// </summary>
    public static class RotateChikanhyo
    {
        /// <summary>
        /// ローテート・ビットボード用の置換表
        /// </summary>
        public static Masu[] chikanHyo_ha45;
        public static Masu[] chikanHyo_hs45;
        public static Masu[] chikanHyo_ht90;

        public static void Tukurinaosi()
        {
            //────────────────────
            // 左上がり筋一列ビットボード
            //────────────────────
            // o---
            // -o--
            // --o-
            // ---o
            {
                chikanHyo_ha45 = new Masu[PureSettei.banHeimen];
                int dst = 0;
                BitboardsOmatome.ScanHidariAgariSuji((int diagonals, Masu ms) =>
                {
                    chikanHyo_ha45[(int)ms] = (Masu)dst;
                    dst++;
                });
            }

            //────────────────────
            // 左下がり筋一列ビットボード
            //────────────────────
            // ---o
            // --o-
            // -o--
            // o---
            {
                chikanHyo_hs45 = new Masu[PureSettei.banHeimen];
                int dst = 0;
                BitboardsOmatome.ScanHidariSagariSuji((int diagonals, Masu ms) =>
                {
                    chikanHyo_hs45[(int)ms] = (Masu)dst;
                    dst++;
                });
            }

            //────────────────────
            // 反時計回り一列ビットボード
            //────────────────────
            // ---o
            // ---o
            // ---o
            // ---o
            {
                chikanHyo_ht90 = new Masu[PureSettei.banHeimen];
                BitboardsOmatome.ScanHanTokei90((int diagonals, Masu ms,  Masu dst) => {
                    chikanHyo_ht90[(int)ms] = (Masu)dst;
                });
            }

        }
    }
}
