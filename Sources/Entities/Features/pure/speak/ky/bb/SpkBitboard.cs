using System.Text;
using kifuwarabe_shogithink.pure.ky.bb;


namespace kifuwarabe_shogithink.pure.speak.ky.bb
{
    public static class SpkBitboard
    {
        public static void AppendSyuturyokuTo(Bitboard bb, StringBuilder hyoji)
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
