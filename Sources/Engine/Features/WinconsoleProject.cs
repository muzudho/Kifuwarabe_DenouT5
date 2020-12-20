#if DEBUG
using kifuwarabe_shogithink.pure;
using kifuwarabe_shogithink.pure.ky.bb;

using kifuwarabe_shogithink.pure.project;
using kifuwarabe_shogithink.pure.speak.ky.bb;
using kifuwarabe_shogithink.pure.speak.play;
using kifuwarabe_shogiwin.consolegame.machine;
using kifuwarabe_shogiwin.project.speak;
using kifuwarabe_shogiwin.speak.ban;
#else
using System.Text;
using Grayscale.Kifuwarabi.Entities.Logging;
using kifuwarabe_shogithink.pure;
using kifuwarabe_shogithink.pure.ky.bb;

using kifuwarabe_shogithink.pure.project;
using kifuwarabe_shogithink.pure.speak.ky.bb;
using kifuwarabe_shogithink.pure.speak.play;
using kifuwarabe_shogiwin.consolegame.console;
using kifuwarabe_shogiwin.consolegame.machine;
using kifuwarabe_shogiwin.project.speak;
using kifuwarabe_shogiwin.speak.ban;
#endif

namespace kifuwarabe_shogiwin.project
{
    public class WinconsoleProject : PureProject
    {
        public override string Owata(string hint, StringBuilder hyoji)
        {
            string msg = $"{hint} {hyoji.ToString()}";
            Logger.Flush(hyoji);
            return msg;
        }

        public override void HyojiKikiItiran(StringBuilder hyoji)
        {
            SpkDump.HyojiKikiItiran(hyoji);
        }
        public override void SnapshotTansaku(StringBuilder hyoji)
        {
            // 「手目」に関する情報
            hyoji.AppendLine(string.Format("curr[{0,3}] next: ss={1} ssType={2} cap={3}",
                PureMemory.kifu_endTeme,
                SpkMove.ToString_Fen(PureSettei.fenSyurui, PureMemory.kifu_moveArray[PureMemory.kifu_endTeme]),
                PureMemory.kifu_moveTypeArray[PureMemory.kifu_endTeme],
                PureMemory.kifu_toraretaKsAr[PureMemory.kifu_endTeme]
                ));
        }
        public override void HyojiIbasho(string header, StringBuilder hyoji)
        {
            // 駒の居場所表示☆
            SpkBan_1Column.ToHyojiIbasho(header, hyoji);
        }
        public override void HyojiKyokumen(int teme, StringBuilder hyoji)
        {
            SpkBan_1Column.Setumei_NingenGameYo(teme, hyoji);
        }
        public override void HyojiBitboard(string header, Bitboard bb, StringBuilder hyoji)
        {
            SpkBan_Hisigata.Setumei_yk00(header, bb, hyoji);
        }
        public override string HyojiBitboard(string header, Bitboard bb)
        {
            StringBuilder hyoji = new StringBuilder();
            SpkBan_Hisigata.Setumei_yk00(header, bb, hyoji);
            return hyoji.ToString();
        }
        /*
        /// <summary>
        /// なんでもかんでも出力させたいとき
        /// </summary>
        /// <param name="hint"></param>
        /// <returns></returns>
        public override string Dump()
        {
            StringBuilder mojiretu = new StringBuilder();

            SpkDump.TryFail_Dump(programSupport, mojiretu);

            return mojiretu.ToContents();
        }
        */

#if DEBUG
        public override void Dbg_TryRule1(Bitboard kikiBB, Bitboard trySakiBB)
        {
            SpkBan_MultiColumn.Setumei_Bitboard(
                new string[] { "らいおんの利き", "１段目に移動できる升" },
                new YomiBitboard[] { new YomiBitboard(kikiBB), new YomiBitboard(trySakiBB) },
                " △ ", "　　",
                Pure.Sc.dbgMojiretu
            );
        }
        public override void Dbg_TryRule2(Bitboard spaceBB, Bitboard trySakiBB)
        {
            SpkBan_MultiColumn.Setumei_Bitboard(new string[] { "味方駒無い所", "トライ先" },
                new YomiBitboard[] { new YomiBitboard(spaceBB), new YomiBitboard(trySakiBB) },
                " △ ", "　　",
                Pure.Sc.dbgMojiretu
            );
        }
        public override void Dbg_TryRule3(Bitboard safeBB, Bitboard trySakiBB)
        {
            SpkBan_MultiColumn.Setumei_Bitboard(new string[] { "相手利き無い所", "トライ先" },
                new YomiBitboard[] { new YomiBitboard(safeBB), new YomiBitboard(trySakiBB) },
                " △ ", "　　",
                Pure.Sc.dbgMojiretu
            );
        }
#endif //DEBUG
    }
}
