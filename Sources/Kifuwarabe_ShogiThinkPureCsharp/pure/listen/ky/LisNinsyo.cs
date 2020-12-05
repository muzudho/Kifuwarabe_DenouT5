using kifuwarabe_shogithink.pure.ky;

namespace kifuwarabe_shogithink.pure.listen.ky
{
    public static class LisNinsyo
    {

        /// <summary>
        /// "friend" を 手番、 "opponent" を 相手番 にするぜ☆（＾～＾）
        /// </summary>
        /// <param name="moji1"></param>
        /// <returns></returns>
        public static Ninsyo Parse(string moji1)
        {
            switch (moji1)
            {
                case "friend": return Ninsyo.Watasi;
                case "opponent": return Ninsyo.Anata;
                default: return Ninsyo.Yososu;
            }
        }

    }
}
