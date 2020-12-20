#if DEBUG
using kifuwarabe_shogithink.pure.ky;

using kifuwarabe_shogithink.pure.control;
#else
using kifuwarabe_shogithink.pure.ky;

using kifuwarabe_shogithink.pure.control;
using System.Text;
#endif


namespace kifuwarabe_shogithink.pure.speak.ky
{
    public static class SpkKoma
    {
        public static string ToSetumei(Koma km) { return Conv_Koma.m_itimojiKoma_[(int)km]; }
        public static void AppendSetumei(Koma km, StringBuilder hyoji) { hyoji.Append(ToSetumei(km)); }

        public static void AppendTusinYo(Koma km, StringBuilder syuturyoku) { syuturyoku.Append(Conv_Koma.m_dfen_[(int)km]); }

        /// <summary>
        /// 目視確認用の文字列を返すぜ☆（＾▽＾）
        /// </summary>
        /// <param name="mk"></param>
        /// <returns></returns>
        public static void AppendSetumei(Motigoma mk, StringBuilder hyoji)
        {
            hyoji.Append(Conv_Motigoma.m_setumeiMojiretu_[(int)mk]);
        }

        /// <summary>
        /// 改造Fen用の文字列を返すぜ☆（＾▽＾）
        /// </summary>
        /// <param name="km"></param>
        /// <returns></returns>
        public static void AppendFenTo(FenSyurui f, Koma km, StringBuilder syuturyoku)
        {
            syuturyoku.Append(f==FenSyurui.sfe_n ? Conv_Koma.m_sfen_[(int)km] : Conv_Koma.m_dfen_[(int)km]);
        }

        public static string Koma_To_Zen1(Koma km)
        {
            switch (km)
            {
                // らいおん（対局者１、対局者２）
                case Koma.R: return Conv_Koma.ZEN1_RAION1;
                case Koma.r: return Conv_Koma.ZEN1_RAION2;

                case Koma.Z: return Conv_Koma.ZEN1_ZOU1;
                case Koma.z: return Conv_Koma.ZEN1_ZOU2;

                case Koma.PZ: return Conv_Koma.ZEN1_POW_ZOU1;
                case Koma.pz: return Conv_Koma.ZEN1_POW_ZOU2;

                case Koma.K: return Conv_Koma.ZEN1_KIRIN1;
                case Koma.k: return Conv_Koma.ZEN1_KIRIN2;

                case Koma.PK: return Conv_Koma.ZEN1_POW_KIRIN1;
                case Koma.pk: return Conv_Koma.ZEN1_POW_KIRIN2;

                case Koma.H: return Conv_Koma.ZEN1_HIYOKO1;
                case Koma.h: return Conv_Koma.ZEN1_HIYOKO2;

                case Koma.PH: return Conv_Koma.ZEN1_POW_HIYOKO1;
                case Koma.ph: return Conv_Koma.ZEN1_POW_HIYOKO2;

                case Koma.I: return Conv_Koma.ZEN1_INU1;
                case Koma.i: return Conv_Koma.ZEN1_INU2;

                case Koma.N: return Conv_Koma.ZEN1_NEKO1;
                case Koma.n: return Conv_Koma.ZEN1_NEKO2;

                case Koma.PN: return Conv_Koma.ZEN1_POW_NEKO1;
                case Koma.pn: return Conv_Koma.ZEN1_POW_NEKO2;

                case Koma.U: return Conv_Koma.ZEN1_USAGI1;
                case Koma.u: return Conv_Koma.ZEN1_USAGI2;

                case Koma.PU: return Conv_Koma.ZEN1_POW_USAGI1;
                case Koma.pu: return Conv_Koma.ZEN1_POW_USAGI2;

                case Koma.S: return Conv_Koma.ZEN1_SISI1;
                case Koma.s: return Conv_Koma.ZEN1_SISI2;

                case Koma.PS: return Conv_Koma.ZEN1_POW_SISI1;
                case Koma.ps: return Conv_Koma.ZEN1_POW_SISI2;

                case Koma.Kuhaku: return Conv_Koma.ZEN1_KUHAKU_ZEN;//空白は全角で
                default: return "×";
            }
        }

    }
}
