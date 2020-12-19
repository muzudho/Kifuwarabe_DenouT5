using kifuwarabe_shogithink.pure.move;

namespace kifuwarabe_shogithink.pure.listen.ikkyoku
{
    public static class LisMoveCharacter
    {
        public static MoveCharacter Parse(string line, ref int caret)
        {
            // うしろに続く文字は☆（＾▽＾）
            if (Util_String.MatchAndNext("HyokatiYusen", line, ref caret))
            {
                return MoveCharacter.HyokatiYusen;
            }
            else if (Util_String.MatchAndNext("SyorituYusen", line, ref caret))
            {
                return MoveCharacter.SyorituYusen;
            }
            else if (Util_String.MatchAndNext("SyorituNomi", line, ref caret))
            {
                return MoveCharacter.SinteYusen;
            }
            else if (Util_String.MatchAndNext("SinteYusen", line, ref caret))
            {
                return MoveCharacter.SinteYusen;
            }
            else if (Util_String.MatchAndNext("SinteNomi", line, ref caret))
            {
                return MoveCharacter.SinteNomi;
            }
            else if (Util_String.MatchAndNext("TansakuNomi", line, ref caret))
            {
                return MoveCharacter.TansakuNomi;
            }

            return MoveCharacter.Yososu;// パース・エラー☆（＾▽＾）
        }
    }
}
