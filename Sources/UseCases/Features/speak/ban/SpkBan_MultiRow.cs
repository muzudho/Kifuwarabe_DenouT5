#if DEBUG
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.ky.bb;
using kifuwarabe_shogithink.pure.speak.ky;
using System.Diagnostics;
using kifuwarabe_shogithink.pure;
using Grayscale.Kifuwarabi.Entities.Take1Base;
#else
using kifuwarabe_shogithink.pure;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.ky.bb;
using kifuwarabe_shogithink.pure.speak.ky;
using System.Diagnostics;
using System.Text;
using Grayscale.Kifuwarabi.Entities.Take1Base;
#endif

namespace kifuwarabe_shogiwin.speak.ban
{
    public static class SpkBan_MultiRow
    {
        /// <summary>
        /// 駒の利き数☆（＾～＾）
        /// 対局者別と、駒別
        /// </summary>
        /// <returns></returns>
        public static void HyojiKomanoKikiSu(KikiBan.YomiKikiBan yomiKikiBan, StringBuilder hyoji)
        {
            hyoji.AppendLine("重ね利き数全部");
            hyoji.AppendLine(string.Format("差分更新トータル ▲{0} △{1}", yomiKikiBan.CountKikisuTotalZenbu(Taikyokusya.T1), yomiKikiBan.CountKikisuTotalZenbu(Taikyokusya.T2)));
            // 対局者別　全部
            {
                // 見出し
                SpkBanWaku.Setumei_Headers(Conv_Taikyokusya.namaeItiran, hyoji);

                SpkBanWaku.AppendLine_TopBar(Conv_Taikyokusya.itiran.Length, PureSettei.banYokoHaba, hyoji); // ┌──┬──┬──┐みたいな線☆
                for (int dan = 0; dan < PureSettei.banTateHaba; dan++)
                {
                    // データ表示
                    SpkBanWaku.AppendLine_Record_Cell4Hankakus1(
                        (Taikyokusya tai, Masu ms) =>
                        {
                            int kikisuZenbu = yomiKikiBan.CountKikisuZenbu(tai, ms);
                            return 0 < kikisuZenbu ? string.Format(" {0,2} ", kikisuZenbu) : "　　";
                        },
                        dan,
                        hyoji
                        );

                    if (dan + 1 < PureSettei.banTateHaba)
                    {
                        SpkBanWaku.AppendLine_MiddleBar(Conv_Taikyokusya.itiran.Length, PureSettei.banYokoHaba, hyoji); // ├──┼──┼──┤みたいな線☆
                    }
                }
                SpkBanWaku.AppendLine_BottomBar(Conv_Taikyokusya.itiran.Length, PureSettei.banYokoHaba, hyoji); // └──┴──┴──┘みたいな線☆
            }
            // 駒別
            foreach (Taikyokusya tai in Conv_Taikyokusya.itiran) // 対局者１、対局者２
            {
                foreach (Piece km_tai in Conv_Koma.itiranTai[(int)tai])
                {
                    hyoji.Append(SpkBanWaku.CutHeaderBanWidthZenkaku(Conv_Koma.GetName(km_tai)));
                }
                hyoji.AppendLine();

                SpkBanWaku.AppendLine_TopBar(Conv_Komasyurui.itiran.Length, PureSettei.banYokoHaba, hyoji);

                for (int dan = 0; dan < PureSettei.banTateHaba; dan++)
                {
                    SpkBanWaku.AppendLine_Record_Cell4Hankakus3(
                        (Taikyokusya tai1, Komasyurui ks, Masu ms) => {
                            int kikisuKomabetu = yomiKikiBan.CountKikisuKomabetu(Med_Koma.KomasyuruiAndTaikyokusyaToKoma(ks, tai1), ms);
                            return 0 < kikisuKomabetu ? string.Format(" {0,2} ", kikisuKomabetu) : "　　";
                        },
                        tai, dan, hyoji);

                    if (dan + 1 < PureSettei.banTateHaba)
                    {
                        SpkBanWaku.AppendLine_MiddleBar(Conv_Komasyurui.itiran.Length, PureSettei.banYokoHaba, hyoji);
                    }
                }
                SpkBanWaku.AppendLine_BottomBar(Conv_Komasyurui.itiran.Length, PureSettei.banYokoHaba, hyoji);
            }
        }

        /// <summary>
        /// 駒の利き
        /// </summary>
        /// <param name="bbItiran_kikiZenbu"></param>
        /// <param name="bbItiran_kikiKomabetu"></param>
        /// <param name="hyoji"></param>
        public static void HyojiKomanoKiki(KikiBan.YomiKikiBan yomiKikiBan, StringBuilder hyoji)
        {
            Debug.Assert(yomiKikiBan.IsActiveBBKiki(), "");

            // 対局者別
            {
                hyoji.AppendLine("利き（対局者別）");
                YomiBitboard[] bbHairetu = new YomiBitboard[Conv_Taikyokusya.itiran.Length];
                foreach (Taikyokusya tai in Conv_Taikyokusya.itiran)
                {
                    bbHairetu[(int)tai] = new YomiBitboard(yomiKikiBan.CloneBBKikiZenbu(tai));
                }
                SpkBan_MultiColumn.Setumei_Bitboard(Conv_Taikyokusya.namaeItiran, bbHairetu,
                    " ＋ ", "　　",
                    hyoji);
            }
            // 駒別
            {
                hyoji.AppendLine("利き（駒別）");
                foreach (Taikyokusya tai in Conv_Taikyokusya.itiran)// 対局者１、対局者２
                {
                    // 盤上
                    YomiBitboard[] bbHairetu = new YomiBitboard[Conv_Komasyurui.itiran.Length];
                    foreach (Komasyurui ks in Conv_Komasyurui.itiran)
                    {
                        bbHairetu[(int)ks] = new YomiBitboard(yomiKikiBan.CloneBBKiki(Med_Koma.KomasyuruiAndTaikyokusyaToKoma(ks, tai)));
                    }

                    SpkBan_MultiColumn.Setumei_Bitboard(Med_Koma.GetKomasyuruiNamaeItiran(tai), bbHairetu,
                        " ＋ ", "　　",
                        hyoji);
                }
            }
        }

        /// <summary>
        /// 駒の動き☆
        /// </summary>
        /// <param name="komanoUgokikata"></param>
        /// <param name="hyoji"></param>
        public static void HyojiKomanoUgoki(KikiBan.YomiKikiBan yomiKikiBan, int masuYososu, StringBuilder hyoji)
        {
            for (int ms = 0; ms < masuYososu; ms++)
            {
                hyoji.AppendLine($"ます{ ms}");
                foreach (Taikyokusya tai in Conv_Taikyokusya.itiran)
                {
                    // 盤上
                    YomiBitboard[] bbHairetu = new YomiBitboard[Conv_Komasyurui.itiran.Length];
                    foreach (Komasyurui ks in Conv_Komasyurui.itiran)
                    {
                        bbHairetu[(int)ks] = new YomiBitboard(BitboardsOmatome.KomanoUgokikataYk00.Clone_Merge(
                            Med_Koma.KomasyuruiAndTaikyokusyaToKoma(ks, tai), (Masu)ms));
                    }
                    SpkBan_MultiColumn.Setumei_Bitboard(Med_Koma.GetKomasyuruiNamaeItiran(tai), bbHairetu,
                        " ＋ ", "　　",
                        hyoji);
                    hyoji.AppendLine();
                }
            }
        }

    }
}
