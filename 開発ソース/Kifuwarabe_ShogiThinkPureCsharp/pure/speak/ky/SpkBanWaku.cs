#if DEBUG
using kifuwarabe_shogithink.pure.conv;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.ky.tobikiki;
using kifuwarabe_shogithink.pure.ky.bb;
using kifuwarabe_shogithink.pure.logger;
using System.Text;
#else
using kifuwarabe_shogithink.pure.ky.tobikiki;
using kifuwarabe_shogithink.pure.conv;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.ky.bb;
using kifuwarabe_shogithink.pure.logger;
using System.Text;
#endif

namespace kifuwarabe_shogithink.pure.speak.ky
{
    /// <summary>
    /// 盤の枠
    /// </summary>
    public static class SpkBanWaku
    {
        #region ヘッダー部
        /// <summary>
        /// 全角しか使っていないと想定して、
        /// ヘッダーを表示盤の横幅サイズに切り抜く。
        /// 右の空いたところには全角空白１個分の半角空白を埋める。
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        public static string CutHeaderBanWidthZenkaku(string header)
        {
            // 盤の横幅は「┌」「──┬」「──┐」の数。
            // 全角で3。
            int banZenkakuWidth = PureSettei.banYokoHaba * 3 + 1;
            StringBuilder sb = new StringBuilder();
            if (banZenkakuWidth == header.Length)
            {
                sb.Append(header.Substring(0, banZenkakuWidth));
            }
            else if (banZenkakuWidth < header.Length)
            {
                sb.Append(header.Substring(0, banZenkakuWidth - 1));
                sb.Append("…");
            }
            else
            {
                sb.Append(header);
                for (int i = banZenkakuWidth - header.Length; 0 < i; i--)
                {
                    sb.Append("  ");
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 将棋盤の見出しをコンソールへ出力するぜ☆（＾▽＾）
        /// </summary>
        /// <returns></returns>
        public static void Setumei_Headers(string[] headers, IHyojiMojiretu hyoji)
        {
            foreach (string header in headers)
            {
                hyoji.Append(SpkBanWaku.CutHeaderBanWidthZenkaku(header));
            }
            hyoji.AppendLine();
        }
        #endregion

        #region 筋符号部
        /// <summary>
        /// 筋符号
        /// </summary>
        /// <param name="hyoji"></param>
        public static void AppendLine_SujiFugo(int banYokoHaba, IHyojiMojiretu hyoji)
        {
            hyoji.Append("   ");
            for (int iSuji = 0; iSuji < banYokoHaba; iSuji++)
            {
                hyoji.Append(Conv_Kihon.ZenkakuAlphabet[iSuji % banYokoHaba]);
                if (iSuji + 1 < banYokoHaba)
                {
                    hyoji.Append("    ");
                }
            }
            hyoji.AppendLine("    ");
        }
        #endregion

        #region トップバー
        /// <summary>
        /// 罫線
        /// 複数列組みに対応☆（＾～＾）
        /// </summary>
        /// <param name="kumiRetuSu"></param>
        /// <param name="hyoji"></param>
        public static void AppendLine_TopBar(int kumiRetuSu, int banYokoHaba, IHyojiMojiretu hyoji)
        {
            for (int ban = 0; ban < kumiRetuSu; ban++)
            {
                hyoji.Append("┌");
                for (int j = 0; j < banYokoHaba; j++)
                {
                    if (j + 1 < banYokoHaba)
                    {
                        hyoji.Append("──┬");
                    }
                    else
                    {
                        hyoji.Append("──┐");
                    }
                }
            }
            hyoji.AppendLine();
        }
        #endregion

        #region レコード部
        public delegate string DLGT_GetCellData1(Taikyokusya tai, Masu ms);
        /// <summary>
        /// [対局者,マス]
        /// </summary>
        /// <param name="dlgt_GetCellData"></param>
        /// <param name="dan"></param>
        /// <param name="hyoji"></param>
        public static void AppendLine_Record_Cell4Hankakus1(DLGT_GetCellData1 dlgt_GetCellData, int dan, IHyojiMojiretu hyoji)
        {
            for (int iTai = 0; iTai < Conv_Taikyokusya.itiran.Length; iTai++)
            {
                hyoji.Append("│");
                for (int iMs_offset = 0; iMs_offset < PureSettei.banYokoHaba; iMs_offset++)
                {
                    hyoji.Append(dlgt_GetCellData((Taikyokusya)iTai, (Masu)(dan * PureSettei.banYokoHaba + iMs_offset)));
                    hyoji.Append("│");
                }
            }
            hyoji.AppendLine();
        }

        public delegate string DLGT_GetCellData2(Masu ms);
        /// <summary>
        /// [マス]
        /// </summary>
        /// <param name="dlgt_GetCellData"></param>
        /// <param name="dan"></param>
        /// <param name="hyoji"></param>
        public static void AppendLine_Record_Cell4Hankakus2(
            DLGT_GetCellData2 dlgt_GetCellData, int dan, int banYokoHaba, IHyojiMojiretu hyoji)
        {
            hyoji.Append("│");
            for (int iMs_offset = 0; iMs_offset < banYokoHaba; iMs_offset++)
            {
                hyoji.Append(dlgt_GetCellData((Masu)(dan * banYokoHaba + iMs_offset)));
                hyoji.Append("│");
            }
            hyoji.AppendLine();
        }

        public delegate string DLGT_GetCellData3(Taikyokusya tai, Komasyurui ks, Masu ms);
        /// <summary>
        /// 駒種類、対局者
        /// </summary>
        /// <param name="dlgt_GetCellData"></param>
        /// <param name="tai"></param>
        /// <param name="ms_hidariHasi"></param>
        /// <param name="hyoji"></param>
        public static void AppendLine_Record_Cell4Hankakus3(
            DLGT_GetCellData3 dlgt_GetCellData, Taikyokusya tai, int dan, IHyojiMojiretu hyoji)
        {
            for (int iKs = 0; iKs < Conv_Komasyurui.itiran.Length; iKs++)
            {
                hyoji.Append("│");
                for (int iMs_offset = 0; iMs_offset < PureSettei.banYokoHaba; iMs_offset++)
                {
                    hyoji.Append(dlgt_GetCellData(tai, (Komasyurui)iKs, (Masu)(dan * PureSettei.banYokoHaba + iMs_offset)));
                    hyoji.Append("│");
                }
            }
            hyoji.AppendLine();
        }

        public delegate string DLGT_GetCellData4(int tableIndex, Masu ms);
        /// <summary>
        /// 〇、×　表示したりするビットボードを横に並べるぜ☆（＾～＾）
        /// </summary>
        /// <param name="dLGT_GetCellData4"></param>
        /// <param name="hyoHairetu"></param>
        /// <param name="dan"></param>
        /// <param name="hyoji"></param>
        public static void AppendLine_MultiTableRecord_Cell4Hankakus4(
            DLGT_GetCellData4 dLGT_GetCellData4,
            YomiBitboard[] hyoHairetu, int dan, IHyojiMojiretu hyoji)
        {
            for (int iHyo = 0; iHyo < hyoHairetu.Length; iHyo++)
            {
                hyoji.Append("│");
                for (int i = 0; i < PureSettei.banYokoHaba; i++)
                {
                    hyoji.Append(dLGT_GetCellData4(iHyo, (Masu)(dan * PureSettei.banYokoHaba + i)));
                    hyoji.Append("│");
                }
            }
            hyoji.AppendLine();
        }
        /// <summary>
        /// 〇、×　表示したりするビットボードを横に並べるぜ☆（＾～＾）
        /// </summary>
        /// <param name="dLGT_GetCellData4"></param>
        /// <param name="hyoHairetu"></param>
        /// <param name="dan"></param>
        /// <param name="hyoji"></param>
        public static void AppendLine_MultiTableRecord_Cell4Hankakus4(
            DLGT_GetCellData4 dLGT_GetCellData4,
            Bitboard[] hyoHairetu, int dan, IHyojiMojiretu hyoji)
        {
            for (int iHyo = 0; iHyo < hyoHairetu.Length; iHyo++)
            {
                hyoji.Append("│");
                for (int i = 0; i < PureSettei.banYokoHaba; i++)
                {
                    hyoji.Append(dLGT_GetCellData4(iHyo, (Masu)(dan * PureSettei.banYokoHaba + i)));
                    hyoji.Append("│");
                }
            }
            hyoji.AppendLine();
        }

        public delegate string DLGT_GetCellData5(Masu[] masuHyo, Masu ms);
        /// <summary>
        /// お邪魔ブロックを横に並べるぜ☆（＾～＾）
        /// </summary>
        /// <param name="dLGT_GetCellData4"></param>
        /// <param name="bbHairetu"></param>
        /// <param name="dan"></param>
        /// <param name="hyoji"></param>
        public static void AppendLine_MultiTableRecord_Cell4Hankakus5(
            DLGT_GetCellData5 dLGT_GetCellData5,
            Masu[][] masuHyoHairetu, int dan, int banYokoHaba, IHyojiMojiretu hyoji)
        {
            for (int iBb = 0; iBb < masuHyoHairetu.Length; iBb++)
            {
                Masu[] masuHyo = masuHyoHairetu[iBb];

                hyoji.Append("│");
                for (int i = 0; i < banYokoHaba; i++)
                {
                    hyoji.Append(dLGT_GetCellData5(masuHyo, (Masu)(dan * banYokoHaba + i)));
                    hyoji.Append("│");
                }
            }
            hyoji.AppendLine();
        }

        public delegate string DLGT_GetCellData6(int tableNumber, Masu ms);
        /// <summary>
        /// 数字を横に並べるぜ☆（＾～＾）
        /// </summary>
        /// <param name="dLGT_GetCellData4"></param>
        /// <param name="bbHairetu"></param>
        /// <param name="dan"></param>
        /// <param name="hyoji"></param>
        public static void AppendLine_MultiTableRecord_Cell4Hankakus5(
            DLGT_GetCellData6 dLGT_GetCellData6,
            int tableSu, int dan, int banYokoHaba, IHyojiMojiretu hyoji)
        {
            for (int iTable = 0; iTable < tableSu; iTable++)
            {
                hyoji.Append("│");
                for (int i = 0; i < banYokoHaba; i++)
                {
                    hyoji.Append(dLGT_GetCellData6(iTable, (Masu)(dan * banYokoHaba + i)));
                    hyoji.Append("│");
                }
            }
            hyoji.AppendLine();
        }
        #endregion


        #region ミドルバー
        /// <summary>
        /// 盤の罫線
        /// </summary>
        /// <param name="banSu"></param>
        /// <param name="hyoji"></param>
        public static void AppendLine_MiddleBar(int banSu, int banYokoHaba, IHyojiMojiretu hyoji)
        {
            for (int ban = 0; ban < banSu; ban++)
            {
                hyoji.Append("├");
                for (int j = 0; j < banYokoHaba; j++)
                {
                    if (j + 1 < banYokoHaba)
                    {
                        hyoji.Append("──┼");
                    }
                    else
                    {
                        hyoji.Append("──┤");
                    }
                }
            }
            hyoji.AppendLine();
        }
        #endregion

        #region ボトムバー
        /// <summary>
        /// 罫線
        /// </summary>
        /// <param name="banSu"></param>
        /// <param name="hyoji"></param>
        public static void AppendLine_BottomBar(int banSu, int banYokoHaba, IHyojiMojiretu hyoji)
        {
            for (int ban = 0; ban < banSu; ban++)
            {
                hyoji.Append("└");
                for (int j = 0; j < banYokoHaba; j++)
                {
                    if (j + 1 < banYokoHaba)
                    {
                        hyoji.Append("──┴");
                    }
                    else
                    {
                        hyoji.Append("──┘");
                    }
                }
            }
            hyoji.AppendLine();
        }
        #endregion
    }
}
