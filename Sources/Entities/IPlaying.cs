using kifuwarabe_shogithink.pure.logger;

namespace Grayscale.Kifuwarabi.Entities
{
    public interface IPlaying
    {
        void UsiOk(string line, string engineName, string engineAuthor, IHyojiMojiretu hyoji);

        void ReadOk(string commandline, IHyojiMojiretu hyoji);
    }
}
