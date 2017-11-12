#if DEBUG
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.control;
#else
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.control;
#endif

namespace kifuwarabe_shogithink.cui.ky
{

    public class SpkMotiKoma
    {
        public static string GetFen(FenSyurui f, Motigoma mk)
        {
            return f==FenSyurui.sfe_n ? Conv_Motigoma.m_sfen_[(int)mk] : Conv_Motigoma.m_dfen_[(int)mk];
        }
    }
}
