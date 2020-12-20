using System.Text;
using kifuwarabe_shogithink.pure.conv;
using kifuwarabe_shogithink.pure.genkyoku;
using kifuwarabe_shogithink.pure.ky;


namespace kifuwarabe_shogithink.pure.speak.ky
{
    public static class SpkMasu
    {

        public static void AppendSetumei(Masu ms, StringBuilder hyoji)
        {
            hyoji.Append(Conv_Kihon.ToAlphabetLarge(GenkyokuOpe.ToSuji_WithError( ms)));
            hyoji.Append(GenkyokuOpe.ToDan_WithError(ms).ToString());
        }

        public static string ToSetumei_New(Masu ms)
        {
            StringBuilder tmp = new StringBuilder();
            AppendSetumei(ms, tmp);
            return tmp.ToString();
        }
    }
}
