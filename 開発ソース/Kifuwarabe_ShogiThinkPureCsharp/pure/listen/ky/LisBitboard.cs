using kifuwarabe_shogithink.pure.ky.bb;

namespace kifuwarabe_shogithink.pure.listen.ky
{
    public static class LisBitboard
    {
        public static bool TryParse(string text, out Bitboard result)
        {
            ulong number;
            if (ulong.TryParse(text, out number))
            {
                result = new Bitboard();
                result.Set(number);
                return true;
            }
            result = null; return false;
        }
    }
}
