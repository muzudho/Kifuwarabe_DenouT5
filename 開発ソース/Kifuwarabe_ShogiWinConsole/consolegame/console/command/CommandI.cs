#if DEBUG
using kifuwarabe_shogiwin.consolegame.machine;
using kifuwarabe_shogithink.pure;
using kifuwarabe_shogithink.pure.genkyoku;
using kifuwarabe_shogithink.pure.listen;
using kifuwarabe_shogithink.pure.logger;
using kifuwarabe_shogiwin.speak.ban;
using kifuwarabe_shogithink.pure.ky;
#else
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogiwin.speak.ban;
using kifuwarabe_shogithink.pure.genkyoku;
using kifuwarabe_shogiwin.consolegame.machine;
using kifuwarabe_shogithink.pure;
using kifuwarabe_shogithink.pure.listen;
using kifuwarabe_shogithink.pure.logger;
#endif

namespace kifuwarabe_shogiwin.consolegame.console.command
{
    public static class CommandI
    {
        public static void Isready(string commandline, IHyojiMojiretu hyoji)
        {
            Util_Machine.Flush(hyoji);
            hyoji.AppendLine("readyok");
            Util_Machine.Flush_USI(hyoji);
        }

    }
}
