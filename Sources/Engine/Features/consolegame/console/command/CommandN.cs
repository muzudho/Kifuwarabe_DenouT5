#if DEBUG
using kifuwarabe_shogithink.pure;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.ky.bb;
using kifuwarabe_shogithink.pure.ky.tobikiki;
using kifuwarabe_shogithink.pure.listen;
using kifuwarabe_shogithink.pure.listen.ky;
using kifuwarabe_shogithink.pure.logger;
using kifuwarabe_shogiwin.listen;
using kifuwarabe_shogiwin.speak.ban;
#else
using kifuwarabe_shogiwin.listen;
using kifuwarabe_shogiwin.speak.ban;
using kifuwarabe_shogithink.pure;
using kifuwarabe_shogithink.pure.genkyoku;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.ky.bb;
using kifuwarabe_shogithink.pure.ky.tobikiki;
using kifuwarabe_shogithink.pure.listen;
using kifuwarabe_shogithink.pure.listen.ky;
using kifuwarabe_shogithink.pure.logger;
using System.Text;
#endif

namespace kifuwarabe_shogiwin.consolegame.console.command
{
    public static class CommandN
    {

        /// <summary>
        /// ナナメ段
        /// </summary>
        /// <param name="line"></param>
        /// <param name="hyoji"></param>
        public static bool TryFail_Nanamedan(string line, IHyojiMojiretu hyoji)
        {
            // うしろに続く文字は☆（＾▽＾）
            int caret = 0;
            Util_String.TobasuTangoToMatubiKuhaku(line, ref caret, "nanamedan ");

            // ナナメ段の符号
            if (line.Length<caret)
            {

            }
            else if (Util_String.MatchAndNext("atama", line, ref caret))
            {
                TobikikiDirection[] kikiDirs = new TobikikiDirection[]
                {
                    TobikikiDirection.KT,
                    TobikikiDirection.KY,
                    TobikikiDirection.S,
                    TobikikiDirection.ZHa,
                    TobikikiDirection.ZHs,
                };
                string[] headers_beforeRotate = new string[]
                {
                    "Ｂき縦頭",
                    "Ｂき横頭",
                    "Ｂし縦頭",
                    "Ｂぞ左上ナ頭",
                    "Ｂぞ左下ナ頭",
                };
                string[] headers_afterRotate = new string[]
                {
                    "Ａき縦頭",
                    "Ａき横頭",
                    "Ａし縦頭",
                    "Ａぞ左上ナ頭",
                    "Ａぞ左下ナ頭",
                };
                int[] dArray = new int[] {
                    PureSettei.banYokoHaba,
                    PureSettei.banTateHaba,
                    PureSettei.banYokoHaba,
                    PureSettei.banNanameDanLen,
                    PureSettei.banNanameDanLen,
                };
                // Before
                for (int iKikiDir = 0; iKikiDir < kikiDirs.Length; iKikiDir++)
                {
                    TobikikiDirection kikiDir = kikiDirs[iKikiDir];
                    hyoji.Append(headers_beforeRotate[iKikiDir]);
                    hyoji.Append(" ");
                    for (int iD = 0; iD < dArray[iKikiDir]; iD++)
                    {
                        hyoji.Append((int)Util_Tobikiki.GetNanameDanAtama_ReverseRotateChikanhyo(iD, kikiDir));
                        hyoji.Append(" ");
                    }
                    hyoji.AppendLine();
                }
                // After
                for (int iKikiDir = 0; iKikiDir < kikiDirs.Length; iKikiDir++)
                {
                    TobikikiDirection kikiDir = kikiDirs[iKikiDir];
                    hyoji.Append(headers_afterRotate[iKikiDir]);
                    hyoji.Append(" ");
                    for (int iD = 0; iD < dArray[iKikiDir]; iD++)
                    {
                        hyoji.Append((int)Util_Tobikiki.GetNanameDanAtama_NoRotateMotohyo(iD, kikiDir));
                        hyoji.Append(" ");
                    }
                    hyoji.AppendLine();
                }
            }
            else if (Util_String.MatchAndNext("d", line, ref caret))//diagonals
            {
                TobikikiDirection[] kikiDirs = new TobikikiDirection[]
                {
                    TobikikiDirection.KT,
                    TobikikiDirection.KY,
                    TobikikiDirection.S,
                    TobikikiDirection.ZHa,
                    TobikikiDirection.ZHs,
                };
                string[] headers = new string[]
                {
                    "き縦段",
                    "き横段",
                    "し縦段",
                    "ぞ左上ナ段",
                    "ぞ左下ナ段",
                };
                bool[] yokoTateHantenHairetu = new bool[] {
                    true,
                    false,
                    true,
                    false,
                    false,
                };

                for(int iKikiDir=0; iKikiDir<kikiDirs.Length; iKikiDir++)
                {
                    TobikikiDirection kikiDir = kikiDirs[iKikiDir];
                    string name = headers[iKikiDir];

                    int[][] numberHyoHairetu = new int[1][];
                    numberHyoHairetu[0] = new int[PureSettei.banHeimen];
                    for (int iMs = 0; iMs < PureSettei.banHeimen; iMs++)
                    {
                        numberHyoHairetu[0][iMs] = Util_Tobikiki.GetNanameDan((Masu)iMs, kikiDir);
                    }

                    SpkBan_MultiColumn.Setumei_Masutbl(
                        new string[] { name },
                        numberHyoHairetu,
                        yokoTateHantenHairetu[iKikiDir],
                        hyoji
                        );
                }
            }
            else if (Util_String.MatchAndNext("haba", line, ref caret))
            {
                TobikikiDirection[] kikiDirs = new TobikikiDirection[]
                {
                    TobikikiDirection.KT,
                    TobikikiDirection.KY,
                    TobikikiDirection.S,
                    TobikikiDirection.ZHa,
                    TobikikiDirection.ZHs,
                };
                int[] dArray = new int[] {
                    PureSettei.banYokoHaba,
                    PureSettei.banTateHaba,
                    PureSettei.banYokoHaba,
                    PureSettei.banNanameDanLen,
                    PureSettei.banNanameDanLen,
                };
                string[] headers = new string[]
                {
                    "き縦幅",
                    "き横幅",
                    "し縦幅",
                    "ぞ左上ナ幅",
                    "ぞ左下ナ幅",
                };
                for (int iKikiDir = 0; iKikiDir < kikiDirs.Length; iKikiDir++)
                {
                    TobikikiDirection kikiDir = kikiDirs[iKikiDir];
                    string name = headers[iKikiDir];
                    hyoji.Append(name);
                    hyoji.Append(" ");
                    for (int iD = 0; iD < dArray[iKikiDir]; iD++)
                    {
                        hyoji.Append(Util_Tobikiki.GetNanameDanHaba(iD, kikiDir));
                        hyoji.Append(" ");
                    }
                    hyoji.AppendLine();
                }
            }
            // 情報表示
            else if (Util_String.MatchAndNext("masu", line, ref caret))
            {
                Masu ms1;
                if (!LisMasu.MatchMasu( line, ref caret, out ms1
#if DEBUG
                        , (IDebugMojiretu)hyoji
#endif
                    ))
                {
                    return Pure.FailTrue(string.Format("nanamedan masu103 パースエラー103 line=[{0}] caret={1}", line, caret));
                }

                TobikikiDirection[] kikiDirs = new TobikikiDirection[]
                {
                    TobikikiDirection.KT,
                    TobikikiDirection.KY,
                    TobikikiDirection.S,
                    TobikikiDirection.ZHa,
                    TobikikiDirection.ZHs,
                };
                int[] masuSpans = new int[]
                {
                    Util_Tobikiki.MasSpanKT,
                    Util_Tobikiki.MasSpanKY,
                    Util_Tobikiki.MasSpanKT,
                    Util_Tobikiki.MasSpanZHa,
                    Util_Tobikiki.MasSpanZHs,
                };
                bool[] sakasa_forZHs = new bool[]
                {
                    false,
                    false,
                    false,
                    false,
                    true
                };
                string[] headers = new string[]
                {
                    "き縦頭",
                    "き横頭",
                    "し縦頭",
                    "ぞ左上ナ頭",
                    "ぞ左下ナ頭",
                };
                for (int iKikiDir = 0; iKikiDir < kikiDirs.Length; iKikiDir++)
                {
                    TobikikiDirection kikiDir = kikiDirs[iKikiDir];
                    int nanamedanD;
                    Masu atama_reverseRotateChikanhyo;
                    Masu atama_noRotateMotohyo;
                    Masu siri_noRotateMotohyo;
                    int haba;
                    Util_Tobikiki.FromMasu(kikiDir, masuSpans[iKikiDir], sakasa_forZHs[iKikiDir],
                        ms1, out nanamedanD,
                        out atama_reverseRotateChikanhyo,
                        out atama_noRotateMotohyo,
                        out siri_noRotateMotohyo,
                        out haba);

                    hyoji.AppendLine(string.Format("{0} d={1} atama={2} haba={3} ", headers[iKikiDir], nanamedanD, atama_reverseRotateChikanhyo, haba));
                }
            }

            return Pure.SUCCESSFUL_FALSE;
        }


        /// <summary>
        /// 二進数
        /// </summary>
        /// <param name="line"></param>
        /// <param name="hyoji"></param>
        public static bool TryFail_Nisinsu(string line, IHyojiMojiretu hyoji)
        {
            // うしろに続く文字は☆（＾▽＾）
            int caret = 0;
            Util_String.TobasuTangoToMatubiKuhaku(line, ref caret, "nisinsu");

            int number;
            if(LisInt.MatchInt(line, ref caret, out number))
            {
                return Pure.FailTrue("TryFail_ParseInt");
            }

            hyoji.AppendLine(string.Format("二進数:{0}", LisHyoji.ToNisinsu((ulong)number)));

            return Pure.SUCCESSFUL_FALSE;
        }
    }
}
