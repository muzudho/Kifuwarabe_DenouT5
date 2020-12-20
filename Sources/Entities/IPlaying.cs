

using System.Text;

namespace Grayscale.Kifuwarabi.Entities
{
    public interface IPlaying
    {
        void UsiOk(string line, string engineName, string engineAuthor, StringBuilder hyoji);

        void ReadOk(string commandline, StringBuilder hyoji);
    }
}
