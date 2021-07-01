#if DEBUG
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.control;
using System;
using kifuwarabe_shogithink.fen;
using Grayscale.Kifuwarabi.Entities.Take1Base;
#else
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.control;
using System;
using kifuwarabe_shogithink.fen;
using Grayscale.Kifuwarabi.Entities.Take1Base;
#endif

namespace kifuwarabe_shogithink.pure.listen.ky
{
    public static class LisKoma
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="moji"></param>
        /// <param name="out_koma"></param>
        /// <returns></returns>
        public static bool Try_ParseFen(FenSyurui f, string moji, out Piece out_koma)
        {
            switch (f)
            {
                case FenSyurui.sfe_n:
                    {
                        // 本将棋駒
                        switch (moji)
                        {
                            // 玉（対局者１、対局者２）
                            case "K": out_koma = Piece.R; return true;
                            case "k": out_koma = Piece.r; return true;

                            case "B": out_koma = Piece.Z; return true;
                            case "b": out_koma = Piece.z; return true;

                            case "+B": out_koma = Piece.PZ; return true;
                            case "+b": out_koma = Piece.pz; return true;

                            case "R": out_koma = Piece.K; return true;
                            case "r": out_koma = Piece.k; return true;

                            case "+R": out_koma = Piece.PK; return true;
                            case "+r": out_koma = Piece.pk; return true;

                            case "P": out_koma = Piece.H; return true;
                            case "p": out_koma = Piece.h; return true;

                            case "+P": out_koma = Piece.PH; return true;
                            case "+p": out_koma = Piece.ph; return true;

                            case "G": out_koma = Piece.I; return true;
                            case "g": out_koma = Piece.i; return true;

                            case "S": out_koma = Piece.N; return true;
                            case "s": out_koma = Piece.n; return true;

                            case "+S": out_koma = Piece.PN; return true;
                            case "+s": out_koma = Piece.pn; return true;

                            case "N": out_koma = Piece.U; return true;
                            case "n": out_koma = Piece.u; return true;

                            case "+N": out_koma = Piece.PU; return true;
                            case "+n": out_koma = Piece.pu; return true;

                            case "L": out_koma = Piece.S; return true;
                            case "l": out_koma = Piece.s; return true;

                            case "+L": out_koma = Piece.PS; return true;
                            case "+l": out_koma = Piece.ps; return true;

                            case " ": out_koma = Piece.Kuhaku; return true;
                            default: out_koma = Piece.Yososu; return false;
                        }
                    }
                case FenSyurui.dfe_n:
                    {
                        //どうぶつしょうぎ駒
                        switch (moji)
                        {
                            // らいおん（対局者１、対局者２）
                            case "R": out_koma = Piece.R; return true;
                            case "r": out_koma = Piece.r; return true;

                            case "Z": out_koma = Piece.Z; return true;
                            case "z": out_koma = Piece.z; return true;

                            case "+Z": out_koma = Piece.PZ; return true;
                            case "+z": out_koma = Piece.pz; return true;

                            case "K": out_koma = Piece.K; return true;
                            case "k": out_koma = Piece.k; return true;

                            case "+K": out_koma = Piece.PK; return true;
                            case "+k": out_koma = Piece.pk; return true;

                            case "H": out_koma = Piece.H; return true;
                            case "h": out_koma = Piece.h; return true;

                            case "+H": out_koma = Piece.PH; return true;
                            case "+h": out_koma = Piece.ph; return true;

                            case "I": out_koma = Piece.I; return true;
                            case "i": out_koma = Piece.i; return true;

                            case "N": out_koma = Piece.N; return true;
                            case "n": out_koma = Piece.n; return true;

                            case "+N": out_koma = Piece.PN; return true;
                            case "+n": out_koma = Piece.pn; return true;

                            case "U": out_koma = Piece.U; return true;
                            case "u": out_koma = Piece.u; return true;

                            case "+U": out_koma = Piece.PU; return true;
                            case "+u": out_koma = Piece.pu; return true;

                            case "S": out_koma = Piece.S; return true;
                            case "s": out_koma = Piece.s; return true;

                            case "+S": out_koma = Piece.PS; return true;
                            case "+s": out_koma = Piece.ps; return true;

                            case " ": out_koma = Piece.Kuhaku; return true;
                            default: out_koma = Piece.Yososu; return false;
                        }
                    }
                default:
                    throw new Exception(string.Format("未定義 {0}", f));
            }
        }

        public static bool MatchKoma(string line, ref int caret, out Piece out_km)
        {
            return Itiran_FenParser.MatchKoma(line, ref caret, out out_km);
        }

        public static bool TryParse_ZenkakuKanaNyuryoku(string zenkakuKana, out Piece out_koma)
        {
            switch (zenkakuKana)
            {
                // らいおん（対局者１、対局者２）
                case Conv_Koma.ZEN1_RAION1: out_koma = Piece.R; return true;
                case Conv_Koma.ZEN1_RAION2: out_koma = Piece.r; return true;

                case Conv_Koma.ZEN1_ZOU1: out_koma = Piece.Z; return true;
                case Conv_Koma.ZEN1_ZOU2: out_koma = Piece.z; return true;

                case Conv_Koma.ZEN1_POW_ZOU1: out_koma = Piece.PZ; return true;
                case Conv_Koma.ZEN1_POW_ZOU2: out_koma = Piece.pz; return true;

                case Conv_Koma.ZEN1_KIRIN1: out_koma = Piece.K; return true;
                case Conv_Koma.ZEN1_KIRIN2: out_koma = Piece.k; return true;

                case Conv_Koma.ZEN1_POW_KIRIN1: out_koma = Piece.PK; return true;
                case Conv_Koma.ZEN1_POW_KIRIN2: out_koma = Piece.pk; return true;

                case Conv_Koma.ZEN1_HIYOKO1: out_koma = Piece.H; return true;
                case Conv_Koma.ZEN1_HIYOKO2: out_koma = Piece.h; return true;

                case Conv_Koma.ZEN1_POW_HIYOKO1: out_koma = Piece.PH; return true;
                case Conv_Koma.ZEN1_POW_HIYOKO2: out_koma = Piece.ph; return true;

                case Conv_Koma.ZEN1_INU1: out_koma = Piece.I; return true;
                case Conv_Koma.ZEN1_INU2: out_koma = Piece.i; return true;

                case Conv_Koma.ZEN1_NEKO1: out_koma = Piece.N; return true;
                case Conv_Koma.ZEN1_NEKO2: out_koma = Piece.n; return true;

                case Conv_Koma.ZEN1_POW_NEKO1: out_koma = Piece.PN; return true;
                case Conv_Koma.ZEN1_POW_NEKO2: out_koma = Piece.pn; return true;

                case Conv_Koma.ZEN1_USAGI1: out_koma = Piece.U; return true;
                case Conv_Koma.ZEN1_USAGI2: out_koma = Piece.u; return true;

                case Conv_Koma.ZEN1_POW_USAGI1: out_koma = Piece.PU; return true;
                case Conv_Koma.ZEN1_POW_USAGI2: out_koma = Piece.pu; return true;

                case Conv_Koma.ZEN1_SISI1: out_koma = Piece.S; return true;
                case Conv_Koma.ZEN1_SISI2: out_koma = Piece.s; return true;

                case Conv_Koma.ZEN1_POW_SISI1: out_koma = Piece.PS; return true;
                case Conv_Koma.ZEN1_POW_SISI2: out_koma = Piece.ps; return true;

                case Conv_Koma.ZEN1_KUHAKU_HAN: out_koma = Piece.Kuhaku; return true;
                case Conv_Koma.ZEN1_KUHAKU_ZEN: out_koma = Piece.Kuhaku; return true;//2017-09-22追加
                default: out_koma = Piece.Yososu; return false;
            }
        }

    }
}
