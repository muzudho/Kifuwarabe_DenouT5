#if DEBUG
using kifuwarabe_shogithink.pure;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.listen;
using kifuwarabe_shogithink.pure.logger;
using kifuwarabe_shogiwin.speak.ban;
#else
using kifuwarabe_shogithink.pure.ky.tobikiki;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogiwin.speak.ban;
using kifuwarabe_shogithink.pure.genkyoku;
using kifuwarabe_shogiwin.consolegame.machine;
using kifuwarabe_shogithink.pure;
using kifuwarabe_shogithink.pure.listen;
using kifuwarabe_shogithink.pure.logger;
#endif

namespace kifuwarabe_shogiwin.consolegame.console.command
{
    public static class CommandF
    {
        /// <summary>
        /// 符号
        /// </summary>
        /// <param name="line"></param>
        /// <param name="hyoji"></param>
        public static bool TryFail_Fugo(string line,
            IHyojiMojiretu hyoji)
        {
            // うしろに続く文字は☆（＾▽＾）
            int caret = 0;
            Util_String.TobasuTangoToMatubiKuhaku(line, ref caret, "fugo ");

            // 段の符号
            if (Util_String.MatchAndNext("dan", line, ref caret))
            {
                int[][] numberHyoHairetu = new int[1][];
                numberHyoHairetu[0] = new int[PureSettei.banHeimen];
                for (int iMs = 0; iMs < PureSettei.banHeimen; iMs++)
                {
                    numberHyoHairetu[0][iMs] = Conv_Masu.ToDanO1_WithoutErrorCheck(iMs);
                }

                SpkBan_MultiColumn.Setumei_Masutbl(
                    new string[] { "段符号" },
                    numberHyoHairetu,
                    false,
                    hyoji
                    );
            }
            // 左上筋の符号
            else if (Util_String.MatchAndNext("ha", line, ref caret))
            {
                int[][] numberHyoHairetu = new int[1][];
                numberHyoHairetu[0] = new int[PureSettei.banHeimen];
                for (int iMs = 0; iMs < PureSettei.banHeimen; iMs++)
                {
                    numberHyoHairetu[0][iMs] = Conv_Masu.ToHidariAgariSujiO1_WithoutErrorCheck(iMs);
                }

                SpkBan_MultiColumn.Setumei_Masutbl(
                    new string[] { "左上筋符号" },
                    numberHyoHairetu,
                    false,
                    hyoji
                    );
            }
            // 左下筋の符号
            else if (Util_String.MatchAndNext("hs", line, ref caret))
            {
                int[][] numberHyoHairetu = new int[1][];
                numberHyoHairetu[0] = new int[PureSettei.banHeimen];
                for (int iMs = 0; iMs < PureSettei.banHeimen; iMs++)
                {
                    numberHyoHairetu[0][iMs] = Conv_Masu.ToHidariSagariSujiO1_WithoutErrorCheck(iMs);
                }

                SpkBan_MultiColumn.Setumei_Masutbl(
                    new string[] { "左下筋符号" },
                    numberHyoHairetu,
                    false,
                    hyoji
                    );
            }
            // 筋の符号
            else if (Util_String.MatchAndNext("suji", line, ref caret))
            {
                int[][] numberHyoHairetu = new int[1][];
                numberHyoHairetu[0] = new int[PureSettei.banHeimen];
                for (int iMs=0; iMs<PureSettei.banHeimen; iMs++)
                {
                    numberHyoHairetu[0][iMs] = Conv_Masu.ToSujiO1_WithoutErrorCheck(iMs);
                }

                SpkBan_MultiColumn.Setumei_Masutbl(
                    new string[] { "筋符号" },
                    numberHyoHairetu,
                    false,
                    hyoji
                    );
            }

            return Pure.SUCCESSFUL_FALSE;
        }
    }
}
