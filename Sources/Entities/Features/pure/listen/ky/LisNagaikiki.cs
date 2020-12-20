#if DEBUG
using kifuwarabe_shogithink.pure.ky.tobikiki;

#else
using kifuwarabe_shogithink.pure.ky.tobikiki;
#endif

namespace kifuwarabe_shogithink.pure.listen.ky
{
    public static class LisNagaikiki
    {
        public static bool TryFail_ParseNagaikikiDir(string commandline, ref int caret, out TobikikiDirection out_kikiDir)
        {
            if (caret == commandline.IndexOf("KT", caret))
            {
                caret += 2;
                out_kikiDir = TobikikiDirection.KT;
                return Pure.SUCCESSFUL_FALSE;
            }
            else if (caret == commandline.IndexOf("KY", caret))
            {
                caret += 2;
                out_kikiDir = TobikikiDirection.KY;
                return Pure.SUCCESSFUL_FALSE;
            }
            else if (caret == commandline.IndexOf("None", caret))
            {
                caret += 4;
                out_kikiDir = TobikikiDirection.None;
                return Pure.SUCCESSFUL_FALSE;
            }
            else if (caret == commandline.IndexOf("S", caret))
            {
                caret += 1;
                out_kikiDir = TobikikiDirection.S;
                return Pure.SUCCESSFUL_FALSE;
            }
            else if (caret == commandline.IndexOf("ZHa", caret))
            {
                caret += 3;
                out_kikiDir = TobikikiDirection.ZHa;
                return Pure.SUCCESSFUL_FALSE;
            }
            else if (caret == commandline.IndexOf("ZHs", caret))
            {
                caret += 3;
                out_kikiDir = TobikikiDirection.ZHs;
                return Pure.SUCCESSFUL_FALSE;
            }
            else
            {
                out_kikiDir = TobikikiDirection.None;
                return Pure.FailTrue(string.Format("commandline=[{0}] caret={1}", commandline, caret));
            }
        }
    }
}
