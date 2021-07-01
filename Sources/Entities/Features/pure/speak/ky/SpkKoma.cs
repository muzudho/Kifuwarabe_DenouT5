#if DEBUG
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.control;
using Grayscale.Kifuwarabi.Entities.Take1Base;
#else
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.control;
using System.Text;
using Grayscale.Kifuwarabi.Entities.Take1Base;
#endif


namespace kifuwarabe_shogithink.pure.speak.ky
{
    public static class SpkKoma
    {
        public static string ToSetumei(Piece km) { return Conv_Koma.m_itimojiKoma_[(int)km]; }
        public static void AppendSetumei(Piece km, StringBuilder hyoji) { hyoji.Append(ToSetumei(km)); }

        public static void AppendTusinYo(Piece km, StringBuilder syuturyoku) { syuturyoku.Append(Conv_Koma.m_dfen_[(int)km]); }

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
        public static void AppendFenTo(FenSyurui f, Piece km, StringBuilder syuturyoku)
        {
            syuturyoku.Append(f==FenSyurui.sfe_n ? Conv_Koma.m_sfen_[(int)km] : Conv_Koma.m_dfen_[(int)km]);
        }

        public static string Koma_To_Zen1(Piece km)
        {
            switch (km)
            {
                // らいおん（対局者１、対局者２）
                case Piece.K1: return Conv_Koma.ZEN1_RAION1;
                case Piece.K2: return Conv_Koma.ZEN1_RAION2;

                case Piece.B1: return Conv_Koma.ZEN1_ZOU1;
                case Piece.B2: return Conv_Koma.ZEN1_ZOU2;

                case Piece.PB1: return Conv_Koma.ZEN1_POW_ZOU1;
                case Piece.PB2: return Conv_Koma.ZEN1_POW_ZOU2;

                case Piece.R1: return Conv_Koma.ZEN1_KIRIN1;
                case Piece.R2: return Conv_Koma.ZEN1_KIRIN2;

                case Piece.PR1: return Conv_Koma.ZEN1_POW_KIRIN1;
                case Piece.PR2: return Conv_Koma.ZEN1_POW_KIRIN2;

                case Piece.P1: return Conv_Koma.ZEN1_HIYOKO1;
                case Piece.P2: return Conv_Koma.ZEN1_HIYOKO2;

                case Piece.PP1: return Conv_Koma.ZEN1_POW_HIYOKO1;
                case Piece.PP2: return Conv_Koma.ZEN1_POW_HIYOKO2;

                case Piece.G1: return Conv_Koma.ZEN1_INU1;
                case Piece.G2: return Conv_Koma.ZEN1_INU2;

                case Piece.S1: return Conv_Koma.ZEN1_NEKO1;
                case Piece.S2: return Conv_Koma.ZEN1_NEKO2;

                case Piece.PS1: return Conv_Koma.ZEN1_POW_NEKO1;
                case Piece.PS2: return Conv_Koma.ZEN1_POW_NEKO2;

                case Piece.N1: return Conv_Koma.ZEN1_USAGI1;
                case Piece.N2: return Conv_Koma.ZEN1_USAGI2;

                case Piece.PN1: return Conv_Koma.ZEN1_POW_USAGI1;
                case Piece.PN2: return Conv_Koma.ZEN1_POW_USAGI2;

                case Piece.L1: return Conv_Koma.ZEN1_SISI1;
                case Piece.L2: return Conv_Koma.ZEN1_SISI2;

                case Piece.PL1: return Conv_Koma.ZEN1_POW_SISI1;
                case Piece.PL2: return Conv_Koma.ZEN1_POW_SISI2;

                case Piece.Kuhaku: return Conv_Koma.ZEN1_KUHAKU_ZEN;//空白は全角で
                default: return "×";
            }
        }

    }
}
