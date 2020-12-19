using kifuwarabe_shogithink.pure.conv;
using kifuwarabe_shogithink.pure.genkyoku;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.logger;

namespace kifuwarabe_shogithink.pure.speak.ky
{
    public static class SpkMasu
    {

        public static void AppendSetumei(Masu ms, IHyojiMojiretu hyoji)
        {
            hyoji.Append(Conv_Kihon.ToAlphabetLarge(GenkyokuOpe.ToSuji_WithError( ms)));
            hyoji.Append(GenkyokuOpe.ToDan_WithError(ms).ToString());
        }

        public static string ToSetumei_New(Masu ms)
        {
            IHyojiMojiretu tmp = new MojiretuImpl();
            AppendSetumei(ms, tmp);
            return tmp.ToContents();
        }
    }
}
