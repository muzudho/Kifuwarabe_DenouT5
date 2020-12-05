#if DEBUG
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.ky.tobikiki;
using kifuwarabe_shogithink.pure.ky.bb;
using kifuwarabe_shogithink.pure.logger;
using kifuwarabe_shogithink.pure.speak.ky;
using System.Diagnostics;
using kifuwarabe_shogithink.pure;
#else
using kifuwarabe_shogithink.pure;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.ky.tobikiki;
using kifuwarabe_shogithink.pure.ky.bb;
using kifuwarabe_shogithink.pure.logger;
using kifuwarabe_shogithink.pure.speak.ky;
using System.Diagnostics;
#endif

namespace kifuwarabe_shogiwin.speak.ban
{
    public static class SpkBan_MultiColumn
    {

        /// <summary>
        /// ビットボードをコンソールへ出力するぜ☆（＾▽＾）
        /// </summary>
        /// <returns></returns>
        public static void Setumei_Bitboard(string[] headers, Bitboard[] bbHairetu, IHyojiMojiretu hyoji)
        {
            // 見出し
            if(null!= headers)
            {
                SpkBanWaku.Setumei_Headers(headers, hyoji);
            }

            SpkBanWaku.AppendLine_TopBar(bbHairetu.Length, PureSettei.banYokoHaba, hyoji);
            for (int dan = 0; dan < PureSettei.banTateHaba; dan++)
            {
                SpkBanWaku.AppendLine_MultiTableRecord_Cell4Hankakus4(
                    (int iTable, Masu ms) =>
                    {
                        return bbHairetu[iTable].IsOn(ms) ? " 〇 " : "　　";
                    },
                    bbHairetu, dan, hyoji);

                if (dan + 1 < PureSettei.banTateHaba)
                {
                    SpkBanWaku.AppendLine_MiddleBar(bbHairetu.Length, PureSettei.banYokoHaba, hyoji);
                }
            }
            SpkBanWaku.AppendLine_BottomBar(bbHairetu.Length, PureSettei.banYokoHaba, hyoji);
        }

        /// <summary>
        /// ビットボードを横に並べるぜ☆（＾▽＾）
        /// </summary>
        /// <param name="headers">見出しを使わない場合はヌル</param>
        /// <param name="yomiBbHairetu"></param>
        /// <param name="hyoji"></param>
        public static void Setumei_Bitboard(string[] headers, YomiBitboard[] yomiBbHairetu, string yes, string no, IHyojiMojiretu hyoji)
        {
            Debug.Assert(0 < yomiBbHairetu.Length && null != yomiBbHairetu[0], "");

            // 見出し
            if(null!= headers)
            {
                SpkBanWaku.Setumei_Headers(headers, hyoji);
            }

            // 盤上
            SpkBanWaku.AppendLine_TopBar(yomiBbHairetu.Length, PureSettei.banYokoHaba, hyoji);
            for (int dan = 0; dan < PureSettei.banTateHaba; dan++)
            {
                SpkBanWaku.AppendLine_MultiTableRecord_Cell4Hankakus4(
                    (int iTable, Masu ms) =>
                    {
                        return yomiBbHairetu[iTable].IsOn(ms) ? yes : no ;// " 〇 " : "　　";
                    },
                    yomiBbHairetu, dan, hyoji);

                if (dan + 1 < PureSettei.banTateHaba)
                {
                    SpkBanWaku.AppendLine_MiddleBar(yomiBbHairetu.Length, PureSettei.banYokoHaba, hyoji);
                }
            }
            SpkBanWaku.AppendLine_BottomBar(yomiBbHairetu.Length, PureSettei.banYokoHaba, hyoji);
        }

        /// <summary>
        /// お邪魔ボードを横に並べるぜ☆（＾▽＾）
        /// 見出し有り
        /// </summary>
        /// <returns></returns>
        public static void Setumei_MasuHyo(
            string[] headers, Masu[][] masuHyoHairetu, bool yokoTateHanten, IHyojiMojiretu hyoji)
        {
            Debug.Assert(0 < masuHyoHairetu.Length, "");
            int banYokoHaba_tmp;
            int banTateHaba_tmp;
            if (yokoTateHanten)
            {
                banYokoHaba_tmp = PureSettei.banTateHaba;
                banTateHaba_tmp = PureSettei.banYokoHaba;
            }
            else
            {
                banYokoHaba_tmp = PureSettei.banYokoHaba;
                banTateHaba_tmp = PureSettei.banTateHaba;
            }

            // 見出し
            SpkBanWaku.Setumei_Headers(headers, hyoji);

            // 盤上
            SpkBanWaku.AppendLine_TopBar(masuHyoHairetu.Length, banYokoHaba_tmp, hyoji);
            for (int dan = 0; dan < banTateHaba_tmp; dan++)
            {
                SpkBanWaku.AppendLine_MultiTableRecord_Cell4Hankakus5(
                    (Masu[] masuHyo, Masu ms) =>
                    {
                        return string.Format(" {0,2} ", masuHyo[(int)ms]);
                    },
                    masuHyoHairetu, dan, banYokoHaba_tmp, hyoji);

                if (dan + 1 < banTateHaba_tmp)
                {
                    SpkBanWaku.AppendLine_MiddleBar(masuHyoHairetu.Length, banYokoHaba_tmp, hyoji);
                }
            }
            SpkBanWaku.AppendLine_BottomBar(masuHyoHairetu.Length, banYokoHaba_tmp, hyoji);
        }

        /// <summary>
        /// 番号ボードを横に並べるぜ☆（＾▽＾）
        /// 見出し有り
        /// </summary>
        /// <returns></returns>
        public static void Setumei_Masutbl(
            string[] headers, int[][] numberHyoHairetu, bool yokoTateHanten, IHyojiMojiretu hyoji)
        {
            Debug.Assert(0 < numberHyoHairetu.Length, "");
            int banYokoHaba_tmp;
            int banTateHaba_tmp;
            if (yokoTateHanten)
            {
                banYokoHaba_tmp = PureSettei.banTateHaba;
                banTateHaba_tmp = PureSettei.banYokoHaba;
            }
            else
            {
                banYokoHaba_tmp = PureSettei.banYokoHaba;
                banTateHaba_tmp = PureSettei.banTateHaba;
            }

            // 見出し
            SpkBanWaku.Setumei_Headers(headers, hyoji);

            // 盤上
            SpkBanWaku.AppendLine_TopBar(numberHyoHairetu.Length, banYokoHaba_tmp, hyoji);
            for (int dan = 0; dan < banTateHaba_tmp; dan++)
            {
                SpkBanWaku.AppendLine_MultiTableRecord_Cell4Hankakus5(
                    (int tableNumber, Masu ms) =>
                    {
                        return string.Format(" {0,2} ", numberHyoHairetu[tableNumber][(int)ms]);
                    },
                    1, dan, banYokoHaba_tmp, hyoji);

                if (dan + 1 < banTateHaba_tmp)
                {
                    SpkBanWaku.AppendLine_MiddleBar(numberHyoHairetu.Length, banYokoHaba_tmp, hyoji);
                }
            }
            SpkBanWaku.AppendLine_BottomBar(numberHyoHairetu.Length, banYokoHaba_tmp, hyoji);
        }
    }
}
