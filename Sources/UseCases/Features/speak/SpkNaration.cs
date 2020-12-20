#if DEBUG
using kifuwarabe_shogiwin.speak.ban;
using kifuwarabe_shogithink.pure;
using kifuwarabe_shogithink.pure.accessor;
using kifuwarabe_shogithink.pure.genkyoku;
using kifuwarabe_shogithink.pure.ikkyoku;

using kifuwarabe_shogithink.pure.speak.ky;
using System;
#else
using kifuwarabe_shogithink.pure.accessor;
using kifuwarabe_shogithink.pure;
using kifuwarabe_shogithink.pure.ikkyoku;
using kifuwarabe_shogithink.pure.ky;

using kifuwarabe_shogithink.pure.speak.ky;
using kifuwarabe_shogiwin.speak.ban;
using System;
using System.Text;
#endif


namespace kifuwarabe_shogiwin.speak
{
    public static class SpkNaration
    {
        /// <summary>
        /// タイトル画面表示☆（＾～＾）
        /// </summary>
        public static void Speak_TitleGamen(StringBuilder hyoji)
        {
            hyoji.Append(
                $@"┌─────────────────────────────────────┐
│ら　ぞ　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　き　ぞ│
│　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　│
│ぞ　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　き│
│　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　│
│　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　│
│　　　　　　　　し　ょ　う　ぎ　　　　さ　ん　　　　よ　ん　　　　　　　　│
│　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　│
│　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　│
│　　　　　　　　　　　　かいはつしゃ　　　むずでょ　　　　　　　　　　　　│
│　　　　　　　　　　　　さーくる　　ぐれーすけーる　　　　　　　　　　　　│
│ひ　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　に│
│　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　│
│き　ひ　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　に　ひ│
└─────────────────────────────────────┘
……ようこそ、将棋３４へ☆（＾▽＾）ｗｗｗ


　　　　　　　　[Enter]　　　　……　対局開始
　　　　　　　　man [Enter]　　……　コマンド説明
　　　　　　　　quit [Enter]　　……　終了



");
#if DEBUG
            hyoji.Append("**デバッグ・モード**");//注意喚起☆（＾▽＾）
#endif
            hyoji.Append("> ");
        }

        /// <summary>
        /// コンピューター思考中表示☆（＾～＾）
        /// </summary>
        public static void Speak_ComputerSikochu( StringBuilder hyoji)
        {
#if DEBUG
            hyoji.Append("**デバッグ・モード** ");//注意喚起☆（＾▽＾）
#endif
            SpkTaikyokusya.AppendSetumeiName(PureMemory.kifu_teban, hyoji);
            hyoji.Append("（");
            hyoji.Append(PureSettei.char_playerN[PureMemory.kifu_nTeban].ToString());
            hyoji.Append("）の思考中（＾～＾）");
        }

        /// <summary>
        /// 決着時のメッセージ表示☆
        /// </summary>
        public static void Speak_KettyakuJi( StringBuilder hyoji)
        {
            if (TaikyokuKekka.Karappo != PureMemory.gky_kekka)
            {
                // 表示（コンソール・ゲーム用）　勝敗☆（＾～＾）”””
                hyoji.AppendLine("決着図");
                SpkBan_1Column.Setumei_NingenGameYo(PureMemory.kifu_endTeme, hyoji);

                // 表示（コンソール・ゲーム用）　勝敗☆（＾～＾）”””
                switch (PureMemory.gky_kekka)
                {
                    case TaikyokuKekka.Taikyokusya1NoKati:
                        if (PureSettei.p2Com)
                        {
                            hyoji.AppendLine("まいったぜ☆（＞＿＜）");
                        }
                        break;
                    case TaikyokuKekka.Taikyokusya2NoKati:
                        if (PureSettei.p2Com)
                        {
                            hyoji.AppendLine("やったぜ☆（＾▽＾）！");
                        }
                        break;
                    case TaikyokuKekka.Hikiwake:
                        {
                            hyoji.AppendLine("決着を付けたかったぜ☆（＾～＾）");
                        }
                        break;
                    case TaikyokuKekka.Karappo://thru
                    default:
                        break;
                }
            }
        }
    }
}
