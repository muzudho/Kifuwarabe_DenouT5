#if DEBUG
using kifuwarabe_shogithink.pure;
using kifuwarabe_shogithink.pure.com;
using kifuwarabe_shogithink.pure.control;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.listen;
using kifuwarabe_shogithink.pure.listen.ikkyoku;
using kifuwarabe_shogithink.pure.logger;
using System;
#else
using System;
using kifuwarabe_shogithink.pure;
using kifuwarabe_shogithink.pure.com;
using kifuwarabe_shogithink.pure.com.hyoka;
using kifuwarabe_shogithink.pure.com.sasiteorder;
using kifuwarabe_shogithink.pure.genkyoku;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.ky.bb;
using kifuwarabe_shogithink.pure.listen;
using kifuwarabe_shogithink.pure.listen.ikkyoku;
using kifuwarabe_shogithink.pure.logger;
using kifuwarabe_shogithink.pure.control;
using kifuwarabe_shogiwin.consolegame.machine;
#endif

namespace kifuwarabe_shogiwin.consolegame.console
{
    public static class ConvAppli
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="line">コマンドライン</param>
        /// <param name="ky2"></param>
        /// <param name="hyoji"></param>
        public static void Set(
            string line, IHyojiMojiretu hyoji
            )
        {
            int caret = 0;
            if (Util_String.MatchAndNext("set", line, ref caret))
            {
                #region BanTateHaba
                if (Util_String.MatchAndNext("BanTateHaba", line, ref caret))
                {
                    // うしろに続く文字は☆（＾▽＾）
                    string rest = line.Substring(caret);
                    if (int.TryParse(rest, out int val))
                    {
                        PureSettei.banTateHabaOld = PureSettei.banTateHaba;
                        PureSettei.banTateHaba = val;

                        // 縦幅と横幅は続けて変えることが多いんで、
                        // 駒の動きの作り直しは別コマンドに分けて行えだぜ☆（＾～＾）
                    }
                }
                #endregion
                #region BanYokoHaba
                else if (Util_String.MatchAndNext("BanYokoHaba", line, ref caret))
                {
                    // うしろに続く文字は☆（＾▽＾）
                    string rest = line.Substring(caret);
                    if (int.TryParse(rest, out int val))
                    {
                        PureSettei.banYokoHabaOld = PureSettei.banYokoHaba;
                        PureSettei.banYokoHaba = val;

                        // 縦幅と横幅は続けて変えることが多いんで、
                        // 駒の動きの作り直しは別コマンドに分けて行えだぜ☆（＾～＾）
                    }
                }
                #endregion
                #region FEN
                else if (Util_String.MatchAndNext("FEN", line, ref caret))
                {
                    // うしろに続く文字は☆（＾▽＾）
                    string rest = line.Substring(caret);
                    switch (rest)
                    {
                        case "dfen": PureSettei.fenSyurui = FenSyurui.dfe_n; break;
                        case "sfen": PureSettei.fenSyurui = FenSyurui.sfe_n; break;
                        default:
                            throw new Exception(string.Format("未定義 {0}", rest));
                    }
                }
                #endregion
                #region GameRule
                else if (Util_String.MatchAndNext("GameRule", line, ref caret))
                {
                    if (Util_String.MatchAndNext("DobutuShogi", line, ref caret))
                    {
                        PureSettei.gameRule = GameRule.DobutuShogi;
                    }
                    else if (Util_String.MatchAndNext("HonShogi", line, ref caret))
                    {
                        PureSettei.gameRule = GameRule.HonShogi;
                    }
                }
                #endregion
                #region HimodukiHyokaTukau
                else if (Util_String.MatchAndNext("HimodukiHyokaTukau", line, ref caret))
                {
                    // うしろに続く文字は☆（＾▽＾）
                    string rest = line.Substring(caret);
                    if (bool.TryParse(rest, out bool val))
                    {
                        ComSettei.himodukiHyokaTukau = val;
                    }
                }
                #endregion
                #region IttedumeTukau
                else if (Util_String.MatchAndNext("IttedumeTukau", line, ref caret))
                {
                    // うしろに続く文字は☆（＾▽＾）
                    string rest = line.Substring(caret);
                    if (bool.TryParse(rest, out bool val))
                    {
                        PureSettei.ittedumeTukau = val;
                    }
                }
                #endregion
                #region JohoJikan
                else if (Util_String.MatchAndNext("JohoJikan", line, ref caret))
                {
                    // うしろに続く文字は☆（＾▽＾）
                    string rest = line.Substring(caret);
                    if (int.TryParse(rest, out int val))
                    {
                        ComSettei.johoJikan = val;
                    }
                }
                #endregion
                #region P1Char
                else if (Util_String.MatchAndNext("P1Char", line, ref caret))
                {
                    // うしろに続く文字は☆（＾▽＾）
                    PureSettei.char_playerN[(int)Taikyokusya.T1] = LisSasiteCharacter.Parse(line, ref caret);
                }
                #endregion
                #region P1Com
                else if (Util_String.MatchAndNext("P1Com", line, ref caret))
                {
                    // うしろに続く文字は☆（＾▽＾）
                    string rest = line.Substring(caret);
                    if (bool.TryParse(rest, out bool val))
                    {
                        PureSettei.p1Com = val;
                    }
                }
                #endregion
                #region P1Name
                else if (Util_String.MatchAndNext("P1Name", line, ref caret))
                {
                    // うしろに続く文字は☆（＾▽＾）
                    PureSettei.name_playerN[(int)Taikyokusya.T1] = line.Substring(caret);
                }
                #endregion
                #region P2Char
                else if (Util_String.MatchAndNext("P2Char", line, ref caret))
                {
                    // うしろに続く文字は☆（＾▽＾）
                    PureSettei.char_playerN[(int)Taikyokusya.T2] = LisSasiteCharacter.Parse(line, ref caret);
                }
                #endregion
                #region P2Com
                else if (Util_String.MatchAndNext("P2Com", line, ref caret))
                {
                    // うしろに続く文字は☆（＾▽＾）
                    string rest = line.Substring(caret);
                    if (bool.TryParse(rest, out bool val))
                    {
                        PureSettei.p2Com = val;
                    }
                }
                #endregion
                #region P2Name
                else if (Util_String.MatchAndNext("P2Name", line, ref caret))
                {
                    // うしろに続く文字は☆（＾▽＾）
                    PureSettei.name_playerN[(int)Taikyokusya.T2] = line.Substring(caret);
                }
                #endregion
                #region RenzokuTaikyoku
                else if (Util_String.MatchAndNext("RenzokuTaikyoku", line, ref caret))
                {
                    // うしろに続く文字は☆（＾▽＾）
                    string rest = line.Substring(caret);
                    if (bool.TryParse(rest, out bool val))
                    {
                        ConsolegameSettei.renzokuTaikyoku = val;
                    }
                }
                #endregion
                #region SaidaiFukasa
                else if (Util_String.MatchAndNext("SaidaiFukasa", line, ref caret))
                {
                    // うしろに続く文字は☆（＾▽＾）
                    string rest = line.Substring(caret);
                    if (int.TryParse(rest, out int val))
                    {
                        ComSettei.saidaiFukasa = val;

                        if (PureMemory.SAIDAI_SASITE_FUKASA - 1 < ComSettei.saidaiFukasa)
                        {
                            ComSettei.saidaiFukasa = PureMemory.SAIDAI_SASITE_FUKASA - 1;
                            hyoji.AppendLine("(^q^)実装の上限の [" + (PureMemory.SAIDAI_SASITE_FUKASA - 1) + "] に下げたぜ☆");
                        }
                    }
                }
                #endregion
                #region SikoJikan
                else if (Util_String.MatchAndNext("SikoJikan", line, ref caret))
                {
                    // うしろに続く文字は☆（＾▽＾）
                    string rest = line.Substring(caret);
                    if (long.TryParse(rest, out long val))
                    {
                        ComSettei.sikoJikan = val;
                    }
                }
                #endregion
                #region SikoJikanRandom
                else if (Util_String.MatchAndNext("SikoJikanRandom", line, ref caret))
                {
                    // うしろに続く文字は☆（＾▽＾）
                    string rest = line.Substring(caret);
                    if (int.TryParse(rest, out int val))
                    {
                        ComSettei.sikoJikanRandom = val;
                    }
                }
                #endregion
                #region TobikikiTukau
                else if (Util_String.MatchAndNext("TobikikiTukau", line, ref caret))
                {
                    // うしろに続く文字は☆（＾▽＾）
                    string rest = line.Substring(caret);
                    if (bool.TryParse(rest, out bool val))
                    {
                        PureSettei.tobikikiTukau = val;
                    }
                }
                #endregion
                #region UseTimeOver
                else if (Util_String.MatchAndNext("UseTimeOver", line, ref caret))
                {
                    // うしろに続く文字は☆（＾▽＾）
                    string rest = line.Substring(caret);
                    if (bool.TryParse(rest, out bool val))
                    {
                        ComSettei.useTimeOver = val;
                    }
                }
                #endregion
                #region USI
                else if (Util_String.MatchAndNext("USI", line, ref caret))
                {
                    // うしろに続く文字は☆（＾▽＾）
                    string rest = line.Substring(caret);
                    if (bool.TryParse(rest, out bool val))
                    {
                        PureSettei.usi = val;
                    }
                }
                #endregion
            }
            else
            {
                // 該当しないものは無視だぜ☆（＾▽＾）
            }
        }
    }
}
