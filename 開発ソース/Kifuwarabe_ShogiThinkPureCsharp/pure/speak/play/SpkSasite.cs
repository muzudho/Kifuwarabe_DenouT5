#if DEBUG
using kifuwarabe_shogithink.pure.conv;
using kifuwarabe_shogithink.pure.conv.genkyoku.play;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.listen.ky;
using kifuwarabe_shogithink.pure.logger;
using kifuwarabe_shogithink.pure.sasite;
using kifuwarabe_shogithink.pure.speak.ky;
using kifuwarabe_shogithink.pure.control;
using System;
#else
using kifuwarabe_shogithink.pure.control;
using kifuwarabe_shogithink.pure.conv;
using kifuwarabe_shogithink.pure.conv.genkyoku.play;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.listen.ky;
using kifuwarabe_shogithink.pure.logger;
using kifuwarabe_shogithink.pure.sasite;
using kifuwarabe_shogithink.pure.speak.ky;
using System;
#endif

namespace kifuwarabe_shogithink.pure.speak.play
{
    public static class SpkSasite
    {
        public static string ToString_Fen(FenSyurui f, Sasite ss)
        {
            MojiretuImpl hyoji = new MojiretuImpl();
            AppendFenTo(f, ss, hyoji);
            return hyoji.ToContents();
        }
        /// <summary>
        /// 改造FEN符号表記
        /// </summary>
        /// <returns></returns>
        public static void AppendFenTo(FenSyurui f, Sasite ss, ICommandMojiretu syuturyoku)
        {
            if (Sasite.Toryo == ss) { syuturyoku.Append(Itiran_FenParser.GetToryo(f)); return; }

            int v = (int)ss;//バリュー（ビットフィールド）

            // 打った駒の種類（取り出すのは難しいので関数を使う☆）
            MotigomaSyurui mksUtta = Conv_Sasite.GetUttaKomasyurui(ss);

            if (MotigomaSyurui.Yososu != mksUtta)//指定があれば
            {
                // 打でした。

                // (自)筋・(自)段は書かずに、「P*」といった表記で埋めます。
                SpkMotiKomasyurui.AppendFenTo(f, mksUtta, syuturyoku);
                syuturyoku.Append("*");
            }
            else
            {
                //------------------------------------------------------------
                // (自)筋
                //------------------------------------------------------------
                //Option_Application.Optionlist.USI
                switch (f)
                {
                    case FenSyurui.sfe_n:
                        {
                            syuturyoku.Append(PureSettei.banYokoHaba + 1 - Conv_Sasite.GetSrcSuji_WithoutErrorCheck(v));
                        }
                        break;
                    case FenSyurui.dfe_n:
                        {
                            syuturyoku.Append(Conv_Kihon.ToAlphabetLarge(Conv_Sasite.GetSrcSuji_WithoutErrorCheck(v)));
                        }
                        break;
                    default:
                        throw new Exception(string.Format("未定義 {0}", f));
                }

                //------------------------------------------------------------
                // (自)段
                //------------------------------------------------------------
                //Option_Application.Optionlist.USI
                switch (f)
                {
                    case FenSyurui.sfe_n:
                        {
                            syuturyoku.Append(Conv_Kihon.ToAlphabetSmall(Conv_Sasite.GetSrcDan_WithoutErrorCheck(v)));
                        }
                        break;
                    case FenSyurui.dfe_n:
                        {
                            syuturyoku.Append(Conv_Sasite.GetSrcDan_WithoutErrorCheck(v).ToString());
                        }
                        break;
                    default:
                        throw new Exception(string.Format("未定義 {0}", f));
                }
            }

            //------------------------------------------------------------
            // (至)筋
            //------------------------------------------------------------
            //Option_Application.Optionlist.USI
            switch (f)
            {
                case FenSyurui.sfe_n:
                    {
                        syuturyoku.Append(PureSettei.banYokoHaba + 1 - Conv_Sasite.GetDstSuji_WithoutErrorCheck(v));
                    }
                    break;
                case FenSyurui.dfe_n:
                    {
                        syuturyoku.Append(Conv_Kihon.ToAlphabetLarge(Conv_Sasite.GetDstSuji_WithoutErrorCheck(v)));
                    }
                    break;
                default:
                    throw new Exception(string.Format("未定義 {0}", f));
            }

            //------------------------------------------------------------
            // (至)段
            //------------------------------------------------------------
            //Option_Application.Optionlist.USI
            switch (f)
            {
                case FenSyurui.sfe_n:
                    {
                        syuturyoku.Append(Conv_Kihon.ToAlphabetSmall(Conv_Sasite.GetDstDan_WithoutErrorCheck(v)));
                    }
                    break;
                case FenSyurui.dfe_n:
                    {
                        syuturyoku.Append(Conv_Sasite.GetDstDan_WithoutErrorCheck(v).ToString());
                    }
                    break;
                default:
                    throw new Exception(string.Format("未定義 {0}", f));
            }

            //------------------------------------------------------------
            // 成
            //------------------------------------------------------------
            int natta;
            {
                // (v & m) >> s + 1。 v:バリュー、m:マスク、s:シフト
                natta = (v & (int)SasiteMask.NATTA) >> (int)SasiteShift.NATTA;
            }
            if (1 == natta)
            {
                syuturyoku.Append("+");
            }
        }

        /// <summary>
        /// 指し手符号の解説。
        /// </summary>
        /// <returns></returns>
        public static void AppendSetumei(FenSyurui f, Sasite ss, IHyojiMojiretu hyoji)
        {
            AppendFenTo(f, ss, hyoji);
        }
        public static void AppendSetumeiLine(FenSyurui f, Sasite ss, IHyojiMojiretu hyoji)
        {
            AppendFenTo(f, ss, hyoji);
            hyoji.AppendLine();
        }

        public static string ToSetumeiByRiyu(SasiteMatigaiRiyu riyu)
        {
            switch (riyu)
            {
                case SasiteMatigaiRiyu.Karappo: return "";// エラーなし
                case SasiteMatigaiRiyu.ParameterSyosikiMatigai: return "doコマンドのパラメーターの書式が間違っていました。";
                case SasiteMatigaiRiyu.NaiMotiKomaUti:
                    return "持ち駒が無いのに駒を打とうとしました。";
                case SasiteMatigaiRiyu.BangaiIdo: return "盤外に駒を動かそうとしました。";
                case SasiteMatigaiRiyu.TebanKomaNoTokoroheIdo: return "自分の駒が置いてあるところに、駒を動かそうとしました。";
                case SasiteMatigaiRiyu.KomaGaAruTokoroheUti: return "駒が置いてあるところに、駒を打ち込もうとしました。";
                case SasiteMatigaiRiyu.KuhakuWoIdo: return "空き升に駒が置いてあると思って、動かそうとしました。";
                case SasiteMatigaiRiyu.AiteNoKomaIdo: return "相手の駒を、動かそうとしました。";
                case SasiteMatigaiRiyu.NarenaiNari: return "ひよこ以外が、にわとりになろうとしました。";
                case SasiteMatigaiRiyu.SonoKomasyuruiKarahaArienaiUgoki: return "その駒の種類からは、ありえない動きをしました。";
                default: return "未定義のエラーです。";
            }
        }
        public static void AppendSetumei(SasiteMatigaiRiyu riyu, IHyojiMojiretu hyoji)
        {
            hyoji.Append(ToSetumeiByRiyu(riyu));
        }
        public static void AppendSetumeiLine(SasiteMatigaiRiyu riyu, IHyojiMojiretu hyoji)
        {
            hyoji.AppendLine(ToSetumeiByRiyu(riyu));
        }

    }

}
