using kifuwarabe_shogithink.pure.sasite;

namespace kifuwarabe_shogithink.pure.listen.ikkyoku
{
    public static class LisSasiteCharacter
    {
        public static SasiteCharacter Parse(string line, ref int caret)
        {
            // うしろに続く文字は☆（＾▽＾）
            if (Util_String.MatchAndNext("HyokatiYusen", line, ref caret))
            {
                return SasiteCharacter.HyokatiYusen;
            }
            else if (Util_String.MatchAndNext("SyorituYusen", line, ref caret))
            {
                return SasiteCharacter.SyorituYusen;
            }
            else if (Util_String.MatchAndNext("SyorituNomi", line, ref caret))
            {
                return SasiteCharacter.SinteYusen;
            }
            else if (Util_String.MatchAndNext("SinteYusen", line, ref caret))
            {
                return SasiteCharacter.SinteYusen;
            }
            else if (Util_String.MatchAndNext("SinteNomi", line, ref caret))
            {
                return SasiteCharacter.SinteNomi;
            }
            else if (Util_String.MatchAndNext("TansakuNomi", line, ref caret))
            {
                return SasiteCharacter.TansakuNomi;
            }

            return SasiteCharacter.Yososu;// パース・エラー☆（＾▽＾）
        }
    }
}
