#if DEBUG
using kifuwarabe_shogithink.pure;
using kifuwarabe_shogithink.pure.control;
using kifuwarabe_shogithink.pure.genkyoku;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.ky.bb;
using kifuwarabe_shogithink.pure.ky.tobikiki;
using kifuwarabe_shogithink.pure.listen;
using kifuwarabe_shogithink.pure.listen.ky;
using kifuwarabe_shogithink.pure.logger;
using kifuwarabe_shogiwin.consolegame.machine;
using kifuwarabe_shogiwin.speak.ban;
using System;
using System.Configuration;
using System.IO;
using Nett;
#else
using kifuwarabe_shogithink.pure;
using kifuwarabe_shogithink.pure.control;
using kifuwarabe_shogithink.pure.genkyoku;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.ky.bb;
using kifuwarabe_shogithink.pure.ky.tobikiki;
using kifuwarabe_shogithink.pure.listen;
using kifuwarabe_shogithink.pure.listen.ky;
using kifuwarabe_shogithink.pure.logger;
using kifuwarabe_shogiwin.consolegame.machine;
using kifuwarabe_shogiwin.speak.ban;
using System;
using System.Configuration;
using System.IO;
using Nett;
using Grayscale.Kifuwarabi.Entities.Logging;
#endif

namespace kifuwarabe_shogiwin.consolegame.console.command
{
    public static class CommandU
    {
        /// <summary>
        /// 駒の動き方
        /// </summary>
        /// <param name="line"></param>
        /// <param name="hyoji"></param>
        public static bool TryFail_Ugokikata(string line, IHyojiMojiretu hyoji)
        {
            int caret = 0;
            if (Util_String.MatchAndNext("ugokikata", line, ref caret))
            {
                // 先後駒
                Koma km;
                {
                    string koma;
                    Util_String.YomuTangoTobasuMatubiKuhaku(line, ref caret, out koma);
                    if (!LisKoma.Try_ParseFen(PureSettei.fenSyurui, koma, out km))
                    {
                        hyoji.AppendLine(string.Format("error koma={0}", koma));
                        return Pure.FailTrue("TryFail_Ugokikata");
                    }
                }
                Taikyokusya tai = Med_Koma.KomaToTaikyokusya(km);

                // 升（0～）
                int iMs;
                if (LisInt.MatchInt(line, ref caret, out iMs))
                {
                    return Pure.FailTrue("TryFail_Ugokikata iMs");
                }

                // 長い利きの方向（1,2）
                TobikikiDirection kikiDir;
                if (LisNagaikiki.TryFail_ParseNagaikikiDir(line, ref caret, out kikiDir))
                {
                    return Pure.FailTrue(string.Format("TryFail_Ugokikata iKikiDir line=[{0}] caret={1}", line, caret));
                }

                // ブロック駒の配置パターン（0～）
                int iBlocks;
                if (LisInt.MatchInt(line, ref caret, out iBlocks))
                {
                    return Pure.FailTrue("TryFail_Ugokikata iBlocks");
                }

                Bitboard bb;
                switch (km)
                {
                    case Koma.K:
                    case Koma.k:
                    case Koma.PK:
                    case Koma.pk:
                    case Koma.Z:
                    case Koma.z:
                    case Koma.PZ:
                    case Koma.pz:
                    case Koma.S:
                    case Koma.s:
                        bb = BitboardsOmatome.KomanoUgokikataYk00.CloneElement(
                            tai, kikiDir, (Masu)iMs, iBlocks);
                        break;
                    default:
                        bb = BitboardsOmatome.KomanoUgokikataYk00.CloneElement(km, (Masu)iMs);
                        break;
                }
                hyoji.AppendLine(string.Format("km={0} masu={1} kikiDir={2} blocks={3}", km, iMs, kikiDir, iBlocks));
                SpkBan_1Column.Setumei_Bitboard("長い利き", bb, hyoji);
            }
            else
            {

            }

            return Pure.SUCCESSFUL_FALSE;
        }

        public static void Undo(string line, IHyojiMojiretu hyoji)
        {
            if (!Util_Control.Try_Undo(line, hyoji))
            {
                Logger.Flush(hyoji);
                throw new Exception(hyoji.ToContents());
            }
            SpkBan_1Column.Setumei_Kyokumen(PureMemory.kifu_endTeme, hyoji);
        }

        /// <summary>
        /// 駒の動き、ビットボード等更新
        /// </summary>
        /// <param name="line"></param>
        /// <param name="hyoji"></param>
        public static void UpdateRule(string line, IHyojiMojiretu hyoji)
        {
            int caret = 0;
            if (Util_String.MatchAndNext("updaterule", line, ref caret))
            {
                Util_Control.UpdateRule(
#if DEBUG
                    "UpdateRuleコマンド"
#endif
                );
            }
        }

        public static void Usinewgame(string line, IHyojiMojiretu hyoji)
        {
            // 設定を何か変更して、確定したければ、ここでやれだぜ☆（＾～＾）
        }
    }
}
