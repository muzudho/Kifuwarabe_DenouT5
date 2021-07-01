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
                case Piece.R: return Conv_Koma.ZEN1_RAION1;
                case Piece.r: return Conv_Koma.ZEN1_RAION2;

                case Piece.Z: return Conv_Koma.ZEN1_ZOU1;
                case Piece.z: return Conv_Koma.ZEN1_ZOU2;

                case Piece.PZ: return Conv_Koma.ZEN1_POW_ZOU1;
                case Piece.pz: return Conv_Koma.ZEN1_POW_ZOU2;

                case Piece.K: return Conv_Koma.ZEN1_KIRIN1;
                case Piece.k: return Conv_Koma.ZEN1_KIRIN2;

                case Piece.PK: return Conv_Koma.ZEN1_POW_KIRIN1;
                case Piece.pk: return Conv_Koma.ZEN1_POW_KIRIN2;

                case Piece.H: return Conv_Koma.ZEN1_HIYOKO1;
                case Piece.h: return Conv_Koma.ZEN1_HIYOKO2;

                case Piece.PH: return Conv_Koma.ZEN1_POW_HIYOKO1;
                case Piece.ph: return Conv_Koma.ZEN1_POW_HIYOKO2;

                case Piece.I: return Conv_Koma.ZEN1_INU1;
                case Piece.i: return Conv_Koma.ZEN1_INU2;

                case Piece.N: return Conv_Koma.ZEN1_NEKO1;
                case Piece.n: return Conv_Koma.ZEN1_NEKO2;

                case Piece.PN: return Conv_Koma.ZEN1_POW_NEKO1;
                case Piece.pn: return Conv_Koma.ZEN1_POW_NEKO2;

                case Piece.U: return Conv_Koma.ZEN1_USAGI1;
                case Piece.u: return Conv_Koma.ZEN1_USAGI2;

                case Piece.PU: return Conv_Koma.ZEN1_POW_USAGI1;
                case Piece.pu: return Conv_Koma.ZEN1_POW_USAGI2;

                case Piece.S: return Conv_Koma.ZEN1_SISI1;
                case Piece.s: return Conv_Koma.ZEN1_SISI2;

                case Piece.PS: return Conv_Koma.ZEN1_POW_SISI1;
                case Piece.ps: return Conv_Koma.ZEN1_POW_SISI2;

                case Piece.Kuhaku: return Conv_Koma.ZEN1_KUHAKU_ZEN;//空白は全角で
                default: return "×";
            }
        }

    }
}
