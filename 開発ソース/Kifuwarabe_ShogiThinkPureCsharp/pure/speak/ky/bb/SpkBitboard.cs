using kifuwarabe_shogithink.pure.ky.bb;
using kifuwarabe_shogithink.pure.logger;

namespace kifuwarabe_shogithink.pure.speak.ky.bb
{
    public static class SpkBitboard
    {
        public static void AppendSyuturyokuTo(Bitboard bb, IHyojiMojiretu hyoji)
        {
            if (0UL < bb.value64127)
            {
                hyoji.Append(bb.value64127);
                hyoji.Append("_");
            }
            hyoji.Append(bb.value063);
        }
    }
}
