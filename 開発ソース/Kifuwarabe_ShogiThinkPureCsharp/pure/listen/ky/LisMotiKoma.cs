#if DEBUG
using kifuwarabe_shogithink.cui.ky;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.logger;
using kifuwarabe_shogithink.pure.control;
using System;
#else
using System;
using kifuwarabe_shogithink.cui.ky;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.logger;
using kifuwarabe_shogithink.pure.control;
#endif

namespace kifuwarabe_shogithink.pure.listen.ky
{
    public static class LisMotiKoma
    {
        public static bool TryParseFen(FenSyurui f, char moji, out Motigoma out_koma)
        {
            switch (f)
            {
                case FenSyurui.sfe_n:
                    {
                        switch (moji)
                        {
                            // 角（対局者１、対局者２）
                            case 'B': out_koma = Motigoma.Z; return true;
                            case 'b': out_koma = Motigoma.z; return true;

                            case 'R': out_koma = Motigoma.K; return true;
                            case 'r': out_koma = Motigoma.k; return true;

                            case 'P': out_koma = Motigoma.H; return true;
                            case 'p': out_koma = Motigoma.h; return true;

                            case 'G': out_koma = Motigoma.I; return true;
                            case 'g': out_koma = Motigoma.i; return true;

                            case 'S': out_koma = Motigoma.N; return true;
                            case 's': out_koma = Motigoma.n; return true;

                            case 'N': out_koma = Motigoma.U; return true;
                            case 'n': out_koma = Motigoma.u; return true;

                            case 'L': out_koma = Motigoma.S; return true;
                            case 'l': out_koma = Motigoma.s; return true;

                            default: out_koma = Motigoma.Yososu; return false;
                        }
                    }
                case FenSyurui.dfe_n:
                    {
                        switch (moji)
                        {
                            // ぞう（対局者１、対局者２）
                            case 'Z': out_koma = Motigoma.Z; return true;
                            case 'z': out_koma = Motigoma.z; return true;

                            case 'K': out_koma = Motigoma.K; return true;
                            case 'k': out_koma = Motigoma.k; return true;

                            case 'H': out_koma = Motigoma.H; return true;
                            case 'h': out_koma = Motigoma.h; return true;

                            case 'I': out_koma = Motigoma.I; return true;
                            case 'i': out_koma = Motigoma.i; return true;

                            case 'N': out_koma = Motigoma.N; return true;
                            case 'n': out_koma = Motigoma.n; return true;

                            case 'U': out_koma = Motigoma.U; return true;
                            case 'u': out_koma = Motigoma.u; return true;

                            case 'S': out_koma = Motigoma.S; return true;
                            case 's': out_koma = Motigoma.s; return true;

                            default: out_koma = Motigoma.Yososu; return false;
                        }
                    }
                default:
                    throw new Exception(string.Format("未定義 {0}", f));
            }
        }

        public static Motigoma Yomu_Motikoma(FenSyurui f, string line, ref int caret, ref bool sippai, IHyojiMojiretu hyoji)
        {
            if (sippai)
            {
                hyoji.AppendLine("failure 持ち駒");
                return Motigoma.Yososu;
            }

            foreach (Motigoma mk in Conv_Motigoma.itiran)
            {
                if (Util_String.MatchAndNext(SpkMotiKoma.GetFen(f, mk), line, ref caret))
                {
                    return mk;
                }
            }

            sippai = true;
            hyoji.AppendLine("failure 持ち駒");
            return Motigoma.Yososu;
        }
    }
}
