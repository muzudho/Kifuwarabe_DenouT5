#if DEBUG
using kifuwarabe_shogithink.pure.accessor;
using kifuwarabe_shogithink.pure.com.sasiteorder;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.listen;
using kifuwarabe_shogithink.pure.listen.play;
using kifuwarabe_shogithink.pure.logger;
#else
using kifuwarabe_shogithink.pure.accessor;
using kifuwarabe_shogithink.pure.com.sasiteorder;
using kifuwarabe_shogithink.pure.listen;
using kifuwarabe_shogithink.pure.listen.play;
using kifuwarabe_shogithink.pure.logger;
#endif

namespace kifuwarabe_shogithink.pure.sasite
{
    public static class UtilSasite
    {
        /// <summary>
        /// グローバル変数に結果を入れるぜ☆（＾～＾）
        /// </summary>
        /// <param name="out_list"></param>
        /// <param name="hyoji"></param>
        /// <returns></returns>
        public static bool TryFail_Sasite_cmd1(IHyojiMojiretu hyoji)
        {
            //グローバル変数に指し手がセットされるぜ☆（＾▽＾）
            PureMemory.SetTnskFukasa(PureMemory.FUKASA_MANUAL);
            SasiteSeiseiAccessor.DoSasitePickerBegin(SasiteType.N21_All);
            SasitePicker01.SasitePicker_01(SasiteType.N21_All, true);
            return Pure.SUCCESSFUL_FALSE;
        }

        public static bool Try_Sasite_cmd2(out Sasite out_sasite, string line)
        {
            // うしろに続く文字は☆（＾▽＾）
            int caret = 0;
            Util_String.TobasuTangoToMatubiKuhaku(line, ref caret, "sasite ");
            string line2 = line.Substring(caret).Trim();

            // sasite 912 といった数字かどうか☆（＾～＾）
            int ssSuji;
            if (int.TryParse(line2, out ssSuji))
            {
                out_sasite = (Sasite)ssSuji;
                return true;
            }

            // 数字でなければ、 sasite B2B3 といった文字列か☆（＾～＾）
            if (!LisPlay.MatchFenSasite(PureSettei.fenSyurui, line, ref caret, out out_sasite))
            {
                out_sasite = Sasite.Toryo;
                return false;
            }

            return true;
        }
    }
}
