#if DEBUG
using kifuwarabe_shogiwin.speak.ban;
using kifuwarabe_shogithink.pure;
using kifuwarabe_shogithink.pure.accessor;
using kifuwarabe_shogithink.pure.genkyoku;
using kifuwarabe_shogithink.pure.ikkyoku;
using kifuwarabe_shogithink.pure.logger;
using kifuwarabe_shogithink.pure.speak.ky;
using System;
#else
using kifuwarabe_shogithink.pure.accessor;
using kifuwarabe_shogithink.pure;
using kifuwarabe_shogithink.pure.ikkyoku;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.logger;
using kifuwarabe_shogithink.pure.speak.ky;
using kifuwarabe_shogiwin.speak.ban;
using System;
#endif


namespace kifuwarabe_shogiwin.speak
{
    public static class SpkNaration
    {
        /// <summary>
        /// タイトル画面表示☆（＾～＾）
        /// </summary>
        public static void Speak_TitleGamen(IHyojiMojiretu hyoji)
        {
            hyoji.Append(
                "┌─────────────────────────────────────┐" + Environment.NewLine +
                "│ら　ぞ　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　き　ぞ│" + Environment.NewLine +
                "│　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　│" + Environment.NewLine +
                "│ぞ　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　き│" + Environment.NewLine +
                "│　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　│" + Environment.NewLine +
                "│　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　│" + Environment.NewLine +
                "│　　　　　　　　し　ょ　う　ぎ　　　　さ　ん　　　　よ　ん　　　　　　　　│" + Environment.NewLine +
                "│　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　│" + Environment.NewLine +
                "│　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　│" + Environment.NewLine +
                "│　　　　　　　　　　　　かいはつしゃ　　　むずでょ　　　　　　　　　　　　│" + Environment.NewLine +
                "│　　　　　　　　　　　　さーくる　　ぐれーすけーる　　　　　　　　　　　　│" + Environment.NewLine +
                "│ひ　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　に│" + Environment.NewLine +
                "│　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　│" + Environment.NewLine +
                "│き　ひ　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　に　ひ│" + Environment.NewLine +
                "└─────────────────────────────────────┘" + Environment.NewLine +
                "……ようこそ、将棋３４へ☆（＾▽＾）ｗｗｗ" + Environment.NewLine +
                Environment.NewLine +
                Environment.NewLine +
                "　　　　　　　　[Enter]　　　　……　対局開始" + Environment.NewLine +
                "　　　　　　　　man [Enter]　　……　コマンド説明" + Environment.NewLine +
                "　　　　　　　　quit [Enter]　　……　終了" + Environment.NewLine +
                Environment.NewLine +
                Environment.NewLine +
                Environment.NewLine +
                "");
#if DEBUG
            hyoji.Append("**デバッグ・モード**");//注意喚起☆（＾▽＾）
#endif
            hyoji.Append("> ");
        }

        /// <summary>
        /// コンピューター思考中表示☆（＾～＾）
        /// </summary>
        public static void Speak_ComputerSikochu( IHyojiMojiretu hyoji)
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
        public static void Speak_KettyakuJi( IHyojiMojiretu hyoji)
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
