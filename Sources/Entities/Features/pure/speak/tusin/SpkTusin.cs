using System.Text;
using kifuwarabe_shogithink.pure.ikkyoku;


namespace kifuwarabe_shogithink.pure.speak.tusin
{
    public static class SpkTusin
    {
        public static void SpeakGameMode_TusinYo(GameMode gameMode, StringBuilder syuturyoku)
        {
            syuturyoku.Append("gameMode, ");
            syuturyoku.AppendLine(gameMode.ToString());
        }

    }
}
