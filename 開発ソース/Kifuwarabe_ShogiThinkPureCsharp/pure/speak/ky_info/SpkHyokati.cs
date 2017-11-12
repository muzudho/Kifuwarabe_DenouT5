#if DEBUG
using kifuwarabe_shogithink.pure.com.hyoka;
using kifuwarabe_shogithink.pure.logger;
using System;
using kifuwarabe_shogithink.pure.control;
#else
using kifuwarabe_shogithink.pure.control;
using kifuwarabe_shogithink.pure.com.hyoka;
using kifuwarabe_shogithink.pure.logger;
using System;
#endif

namespace kifuwarabe_shogithink.pure.speak.ky_info
{
    public static class SpkHyokati
    {
        public static string ToContents(Hyokati hyokaSu)
        {
            MojiretuImpl hyoji = new MojiretuImpl();
            Setumei(hyokaSu, hyoji);
            return hyoji.ToContents();
        }

        /// <summary>
        /// 評価値の表示。
        /// 基本的に数字なんだが、数字の前に説明がつくことがあるぜ☆（＾～＾）
        /// 説明は各括弧で囲んであるぜ☆（＾▽＾）
        /// </summary>
        /// <param name="hyokaSu"></param>
        /// <param name="hyoji"></param>
        public static void Setumei(Hyokati hyokaSu, IHyojiMojiretu hyoji)
        {
            if (hyokaSu.tumeSu==Conv_Tumesu.None)
            {
                // スルー
            }
            else if (hyokaSu.IsKatu())
            {
                // 詰み手数が見えたときだぜ☆（＾▽＾）
                switch (PureSettei.fenSyurui)
                {
                    case FenSyurui.sfe_n:
                        {
                            hyoji.Append("mate ");
                            hyoji.Append(hyokaSu.GetKatu().ToString());
                        }
                        break;
                    case FenSyurui.dfe_n:
                        {
                            hyoji.Append("[katu ");
                            hyoji.Append(hyokaSu.GetKatu().ToString());
                            hyoji.Append("] ");
                            hyoji.Append(hyokaSu.ToString_Ten());
                        }
                        break;
                    default:
                        throw new Exception(string.Format("未定義 {0}", PureSettei.fenSyurui));
                }
                return;
            }
            else if(hyokaSu.IsMakeru())
            {
                // 詰めを食らうぜ☆（＞＿＜）
                switch (PureSettei.fenSyurui)
                {
                    case FenSyurui.sfe_n:
                        {
                            hyoji.Append("mate ");
                            hyoji.Append(hyokaSu.GetMakeru().ToString());
                        }
                        break;
                    case FenSyurui.dfe_n:
                        {
                            // 負数で出てくるのを、正の数に変換して表示するぜ☆（＾▽＾）
                            hyoji.Append("[makeru ");
                            hyoji.Append(hyokaSu.GetMakeru().ToString());
                            hyoji.Append("] ");
                            hyoji.Append(hyokaSu.ToString_Ten());
                        }
                        break;
                    default:
                        throw new Exception(string.Format("未定義 {0}", PureSettei.fenSyurui));
                }
                return;
            }

            // 評価値☆
            hyoji.Append("cp ");
            hyoji.Append(hyokaSu.ToString_Ten());//enum型の名前が出ないように一旦int型に変換
        }



    }
}
