﻿#if DEBUG
using kifuwarabe_shogithink.pure;
using kifuwarabe_shogithink.pure.ky;

using kifuwarabe_shogiwin.consolegame.console.command;
using kifuwarabe_shogiwin.speak.ban;
#else
using System.Text;
using Grayscale.Kifuwarabi.UseCases;
using kifuwarabe_shogithink.pure;

using kifuwarabe_shogiwin.speak.ban;
#endif

namespace kifuwarabe_shogiwin.project.speak
{
    /// <summary>
    /// ダンプ
    /// </summary>
    public static class SpkDump
    {
        public static bool TryFail_Dump(Playing playing, StringBuilder hyoji)
        {
            // FIXME: 再計算はいったん廃止☆（＾～＾）
            hyoji.AppendLine("──────────再計算──────────");
            // 駒の配置
            playing.Koma_cmd( PureSettei.fenSyurui, "koma", hyoji);

            // 利き
            HyojiKikiItiran(hyoji);
            //CommandK.TryFail_Kiki("kiki", gky, hyoji);

            // ojhsh お邪魔ハッシュキー
            playing.TryFail_Nanamedan("ojama ojhsh KT", hyoji);
            playing.TryFail_Nanamedan("ojama ojhsh KY", hyoji);
            playing.TryFail_Nanamedan("ojama ojhsh S", hyoji);
            playing.TryFail_Nanamedan("ojama ojhsh ZHa", hyoji);
            playing.TryFail_Nanamedan("ojama ojhsh ZHs", hyoji);

            return Pure.SUCCESSFUL_FALSE;
        }

        /// <summary>
        /// 利きの表示
        /// </summary>
        public static void HyojiKikiItiran(StringBuilder hyoji)
        {
            // 現行
            hyoji.AppendLine("利き一覧");//（現行）:全部、駒別
            SpkBan_MultiRow.HyojiKomanoKiki(PureMemory.gky_ky.shogiban.yomiShogiban.yomiKikiBan, hyoji);
        }
    }
}
