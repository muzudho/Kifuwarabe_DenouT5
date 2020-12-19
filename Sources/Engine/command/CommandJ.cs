#if DEBUG
using kifuwarabe_shogithink.pure;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.genkyoku;
using kifuwarabe_shogithink.pure.logger;
using kifuwarabe_shogiwin.consolegame.machine;
using System;
using kifuwarabe_shogithink.pure.control;
#else
using kifuwarabe_shogithink.pure.control;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure;
using kifuwarabe_shogithink.pure.genkyoku;
using kifuwarabe_shogithink.pure.logger;
#endif

namespace kifuwarabe_shogiwin.consolegame.console.command
{
    public static class CommandJ
    {
        public static void Jokyo(string line, IHyojiMojiretu hyoji)
        {
            if (line == "jokyo")
            {
                hyoji.AppendLine("GameMode = " + PureAppli.gameMode);
                hyoji.AppendLine("Kekka    = " + PureMemory.gky_kekka);
                hyoji.AppendLine("Kettyaku = " + Genkyoku.IsKettyaku());
                return;
            }
        }


    }
}
