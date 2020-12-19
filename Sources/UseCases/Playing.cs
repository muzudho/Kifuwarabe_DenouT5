using Grayscale.Kifuwarabi.Entities;
using Grayscale.Kifuwarabi.Entities.Logging;
using kifuwarabe_shogithink.pure.logger;

namespace Grayscale.Kifuwarabi.UseCases
{
    public class Playing : IPlaying
    {
        public void UsiOk(string line, string engineName, string engineAuthor, IHyojiMojiretu hyoji)
        {
            Logger.Flush(hyoji);

            hyoji.AppendLine($"id name {engineName}");
            hyoji.AppendLine($"id author {engineAuthor}");
            hyoji.AppendLine("option name SikoJikan type spin default 500 min 100 max 10000000");
            hyoji.AppendLine("option name SikoJikanRandom type spin default 1000 min 0 max 10000000");
            hyoji.AppendLine("option name Comment type string default Jikan is milli seconds.");
            hyoji.AppendLine("usiok");
            Logger.Flush_USI(hyoji);
        }

        public void ReadOk(string commandline, IHyojiMojiretu hyoji)
        {
            Logger.Flush(hyoji);
            hyoji.AppendLine("readyok");
            Logger.Flush_USI(hyoji);
        }

    }
}
