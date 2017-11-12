using kifuwarabe_shogithink.pure.ikkyoku;
using kifuwarabe_shogithink.pure.logger;

namespace kifuwarabe_shogithink.pure.speak.tusin
{
    public static class SpkTusin
    {
        public static void SpeakGameMode_TusinYo(GameMode gameMode, ICommandMojiretu syuturyoku)
        {
            syuturyoku.Append("gameMode, ");
            syuturyoku.AppendLine(gameMode.ToString());
        }

    }
}
