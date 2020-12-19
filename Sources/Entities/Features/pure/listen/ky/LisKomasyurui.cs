#if DEBUG
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.fen;
#else
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.fen;
#endif

namespace kifuwarabe_shogithink.pure.listen.ky
{
    public static class LisKomasyurui
    {
        public static bool MatchKomasyurui(string line, ref int caret, out Komasyurui out_ks)
        {
            return Itiran_FenParser.MatchKomasyurui(line, ref caret, out out_ks);
        }

    }
}
