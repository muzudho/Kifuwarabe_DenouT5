using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.logger;
using kifuwarabe_shogithink.pure.control;

namespace kifuwarabe_shogithink.pure.speak.ky
{
    public static class SpkTaikyokusya
    {
        /// <summary>
        /// 先後。
        /// </summary>
        /// <param name="tai"></param>
        /// <returns></returns>
        public static string ToSetumeiName(Taikyokusya tai)
        {
            switch (tai)
            {
                case Taikyokusya.T1: return PureSettei.name_playerN[(int)Taikyokusya.T1];
                case Taikyokusya.T2: return PureSettei.name_playerN[(int)Taikyokusya.T2];
                default: return "×";
            }
        }
        /// <summary>
        /// 先後。
        /// </summary>
        /// <param name="ts"></param>
        /// <returns></returns>
        public static void AppendSetumeiName(Taikyokusya ts, IHyojiMojiretu hyoji)
        {
            hyoji.Append(ToSetumeiName(ts));
        }
        public static readonly string[] sankaku = new string[] {
            "▲",
            "△",
            "×",
        };

        /// <summary>
        /// 先後。
        /// </summary>
        /// <param name="ts"></param>
        /// <returns></returns>
        public static void AppendSetumeiSankaku(Taikyokusya ts, IHyojiMojiretu hyoji)
        {
            hyoji.Append(sankaku[(int)ts]);
        }
        private static string[] m_tusinYo_ = new string[]
        {
            "1",
            "2",
            "x"
        };
        public static void AppendTusinYo(Taikyokusya ts, ICommandMojiretu syuturyoku)
        {
            syuturyoku.Append(m_tusinYo_[(int)ts]);
        }
        /// <summary>
        /// 先後。
        /// </summary>
        /// <param name="tb"></param>
        /// <returns></returns>
        public static void AppendSetumeiNagame(Ninsyo tb, IHyojiMojiretu hyoji)
        {
            switch (tb)
            {
                case Ninsyo.Watasi: hyoji.Append("手番"); break;
                case Ninsyo.Anata: hyoji.Append("相手番"); break;
                default: hyoji.Append("×"); break;
            }
        }

        public static string ToFen(FenSyurui f, Taikyokusya tb)
        {
            return f == FenSyurui.sfe_n ? Conv_Taikyokusya.m_sfen_[(int)tb] : Conv_Taikyokusya.m_dfen_[(int)tb];
        }

        //public static Taikyokusya Yomu_Player(string commandline, ref int caret, ref bool sippai, Mojiretu syuturyoku)
        //{
        //    if (caret == commandline.IndexOf("1", caret))// 視点　対局者１
        //    {
        //        Util_String.TobasuTangoToMatubiKuhaku(commandline, ref caret, "1");
        //        return Taikyokusya.T1;
        //    }
        //    else if (caret == commandline.IndexOf("2", caret))// 視点　対局者２
        //    {
        //        Util_String.TobasuTangoToMatubiKuhaku(commandline, ref caret, "2");
        //        return Taikyokusya.T2;
        //    }

        //    sippai = true;
        //    syuturyoku.AppendLine("failure 対局者視点");
        //    return Taikyokusya.Yososu;
        //}

    }
}
