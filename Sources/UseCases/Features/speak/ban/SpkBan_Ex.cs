﻿#if DEBUG
using kifuwarabe_shogiwin.speak.ban;
using kifuwarabe_shogithink.pure;
using kifuwarabe_shogithink.pure.ky;

#else
using System.Text;
using kifuwarabe_shogithink.pure;
using kifuwarabe_shogithink.pure.ky;

#endif

namespace kifuwarabe_shogiwin.speak.ban
{
    public static class SpkBan_Ex
    {
        /// <summary>
        /// 今、どんな局面か見たい場合は、説明をダンプすることができるぜ☆（＾～＾）
        /// </summary>
        /// <returns></returns>
        public static string Setumei_Kyokumen_NingenYo()
        {
            StringBuilder str = new StringBuilder();
            SpkBan_1Column.Setumei_NingenGameYo(PureMemory.kifu_endTeme, str);
            return str.ToString();
        }

        /// <summary>
        /// 利き
        /// </summary>
        /// <param name="yomiKy"></param>
        /// <param name="tai"></param>
        /// <param name="hyoji"></param>
        public static void Setumei_GenkoKiki( Taikyokusya tai, StringBuilder hyoji)
        {
            hyoji.AppendLine("利き：（現行）");
            SpkBan_MultiColumn.Setumei_Bitboard(
                Med_Koma.GetKomasyuruiNamaeItiran(tai),
                PureMemory.gky_ky.yomiKy.yomiShogiban.yomiKikiBan.GetBB_WhereKiki(tai),
                " ＋ ", "　　",
                hyoji
                );
        }
    }
}
