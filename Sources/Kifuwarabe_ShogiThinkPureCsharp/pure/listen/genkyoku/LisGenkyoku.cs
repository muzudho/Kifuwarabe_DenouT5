#if DEBUG
using kifuwarabe_shogithink.pure.control;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.listen.ky;
using kifuwarabe_shogithink.pure.logger;
using kifuwarabe_shogithink.pure.med.ky;
using kifuwarabe_shogithink.pure.speak.genkyoku;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using kifuwarabe_shogithink.fen;
#else
using kifuwarabe_shogithink.pure.control;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.listen.ky;
using kifuwarabe_shogithink.pure.logger;
using kifuwarabe_shogithink.pure.med.ky;
using kifuwarabe_shogithink.pure.speak.genkyoku;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using kifuwarabe_shogithink.fen;
#endif

namespace kifuwarabe_shogithink.pure.listen.genkyoku
{
    public static class LisGenkyoku
    {
        static LisGenkyoku()
        {
            tmp_syokikyokumenFen = new MojiretuImpl();
        }

        /// <summary>
        /// 毎回やるには重いが……☆（＾～＾）
        /// 局面を設定するぜ☆（＾～＾）
        /// </summary>
        public static void SetRule(
            GameRule gameRule,
            int banYokohaba,
            int banTatehaba,
            string banmen_z1,
            Dictionary<Motigoma, int> motigomaKaisiSettei
            )
        {
            IHyojiMojiretu hyoji = PureAppli.syuturyoku1;

            PureSettei.gameRule = gameRule;
            PureSettei.banYokoHaba = banYokohaba;
            PureSettei.banTateHaba = banTatehaba;

            // 設定を決めたあと、局面の配置をする前に、ビットボードを作り直すぜ☆（＾～＾）
            Util_Control.UpdateRule(
#if DEBUG
                "Shogi34 SetRule"
#endif
                );

            // 局面のクリアー
            PureMemory.gky_ky.Tukurinaosi_ClearKyokumen();

            if (LisBanmen.TryFail_SetBanjo(
                banmen_z1
#if DEBUG
                , PureAppli.syuturyoku1
#endif
            ))
            {
                throw new Exception(PureAppli.syuturyoku1.ToContents());
            }

            // 持駒の枚数をセット☆（＾～＾）
            foreach (KeyValuePair<Motigoma, int> entry in motigomaKaisiSettei)
            {
                SetMotiKoma(entry.Key, entry.Value);
            }

            //────────────────────────────────────────
            // 現局面を、平手初期局面として登録します
            //────────────────────────────────────────
            MedKyokumen.TorokuHirate();
        }

        /// <summary>
        /// 持駒の枚数をセットするぜ☆（＾～＾）
        /// </summary>
        /// <param name="mk"></param>
        /// <param name="count"></param>
        static void SetMotiKoma(Motigoma mk, int count)
        {
            PureMemory.gky_ky.motigomaItiran.Set(mk, count);
        }



        /// <summary>
        /// 初期局面としてセットするぜ☆（＾▽＾）
        /// </summary>
        public static bool TryFail_SetSyokiKyokumen_ByFen(
            FenSyurui f,
            string[] danMojiretu, // [0～3]1段目～4段目、[0～2]1筋目～3筋目
            string motigoma,
            string tb_Mojis  //手番
#if DEBUG
            ,IDebugMojiretu dbg_reigai
#endif
            )
        {
            PureMemory.gky_ky.Tukurinaosi_ClearKyokumen();

            // 持ち駒パース
            {
                //PureMemory.gky_ky.motigomaItiran.Clear();
                if ("-" != motigoma)// '-' は持ち駒無し
                {
                    int maisu = 0;
                    for (int caret = 0; caret < motigoma.Length; caret++)
                    {
                        char ch = motigoma[caret];

                        int numeric;
                        if (int.TryParse(ch.ToString(), out numeric))
                        {
                            maisu = maisu * 10 + numeric;
                        }
                        else
                        {
                            Motigoma mk;
                            if (! LisMotiKoma.TryParseFen(f, ch, out mk))
                            {
                                return Pure.FailTrue("TryParseFen");
                            }
                            else
                            {
                                // 枚数の指定がなかったとき（=0）は、1。
                                PureMemory.gky_ky.motigomaItiran.Set(mk, maisu == 0 ? 1 : maisu);
                                maisu = 0;
                            }
                        }
                    }
                }
            }

            // 盤上の升（既にクリアされているものとするぜ☆）
            int suji;
            for (int dan = 1; dan <= danMojiretu.Length; dan++) // 1段目～N段目 の順に解析。
            {
                //
                // "2z" のように、3列を 2桁 で表記しているので、タテ筋のループ・カウントの数え方には注意だぜ☆（＾～＾）
                //
                suji = 1;
                int ruikeiKuhakuSu = 0;//累計空白数
                bool isPowerupKoma = false;//パワーアップ駒（成りゴマ）
                for (int caret = 0; //caret < 3 &&
                    caret < danMojiretu[dan - 1].Length // 可変長配列☆
                    ; caret++)
                {
                    char moji = danMojiretu[dan - 1][caret];

                    int kuhaku;
                    if ('+' == moji)
                    {
                        isPowerupKoma = true;
                    }
                    else if (int.TryParse(moji.ToString(), out kuhaku))
                    {
                        // 数字は空き升の個数なので、筋を進めるぜ☆（＾▽＾）
                        // とりあえず 1～9 まで対応できるだろうなんだぜ☆（＾～＾）
                        //for (int i = 0; i < kuhaku; i++)
                        //{
                        ruikeiKuhakuSu = ruikeiKuhakuSu * 10 + kuhaku;
                        //}

                        //Mojiretu reigai1 = new MojiretuImpl();
                        //reigai1.AppendLine("未定義の空白の数 moji=[" + moji + "]");
                        //reigai1.AppendLine("dan   =[" + dan + "]");
                        //reigai1.AppendLine("caret =[" + caret + "]");
                        //reigai1.AppendLine("danMojiretu[dan-1] =[" + danMojiretu[dan - 1] + "]");

                        //throw new Exception(reigai1.ToContents());
                    }
                    else
                    {
                        // 駒でした。
                        if (0 < ruikeiKuhakuSu)
                        {
                            // 空白は置かなくていいのでは？
                            //Masu ms = Conv_Masu.ToMasu(suji, dan);
                            //Koma km_actual = GetBanjoKoma(ms);
                            //HerasuBanjoKoma(ms, km_actual, true);

                            suji += ruikeiKuhakuSu;
                            ruikeiKuhakuSu = 0;
                        }

                        Koma tmp;
                        if (!LisKoma.Try_ParseFen(f, (isPowerupKoma ? "+" + moji : moji.ToString()), out tmp))
                        {
#if DEBUG
                            Pure.Sc.AddErr(string.Format("SetNaiyoで未定義の駒が指定されました。 fen moji=[{0}]",moji));
#endif
                            return Pure.FailTrue("Try_ParseFen");
                        }
                        isPowerupKoma = false;

                        if(PureMemory.gky_ky.shogiban.TryFail_OkuKoma(//SetNaiyo
                            Conv_Masu.ToMasu(suji, dan), tmp, true
#if DEBUG
                            , dbg_reigai
#endif
                            ))
                        {
                            return Pure.FailTrue("TryFail_Oku");
                        }
                        // あとで適用

                        suji += 1;
                    }
                }

                if (0 < ruikeiKuhakuSu)
                {
                    // 空白は置かなくていいのでは？
                    //Masu ms = Conv_Masu.ToMasu(suji, dan);
                    //HerasuBanjoKoma(ms, GetBanjoKoma(ms), true);

                    suji += ruikeiKuhakuSu;
                    ruikeiKuhakuSu = 0;
                }
            }

            // 手番
            {
                Taikyokusya syokikyokumenTai;
                if (!Med_Parser.Try_MojiToTaikyokusya(f, tb_Mojis, out syokikyokumenTai))
                {
#if DEBUG
                    dbg_reigai.AppendLine(string.Format("SetNaiyoで未定義の手番が指定されたんだぜ☆ isSfen={0} 入力={1} 出力={2}",
                        f,
                        tb_Mojis,
                        syokikyokumenTai
                        ));
                    //reigai1.AppendLine("ky.Teban=[" + PureMemory.gky_ky.yomiKy.teban + "]");
                    //reigai1.AppendLine("BanTateHaba=[" + PureSettei.banTateHaba + "]");

                    dbg_reigai.AppendLine(string.Format("持ち駒数一覧({0}件)", danMojiretu.Length));
                    foreach (Motigoma mk in Conv_Motigoma.itiran)
                    {
                        dbg_reigai.AppendLine(string.Format("{0}={1}", mk, PureMemory.gky_ky.motigomaItiran.yomiMotigomaItiran.Count(mk)));
                    }
#endif
                    return Pure.FailTrue("Try_Taikyokusya");
                    //throw new Exception("対局者のパースエラー tb_Mojis=[" + tb_Mojis + "]"+reigai1.ToContents());
                }
                // 先手番始まりか、後手番始まりか、に合わせるぜ☆（＾～＾）
                PureMemory.ResetTebanArray(syokikyokumenTai);
                // 手番には 1 が入っていると思うんだが、無視して 0 スタートに固定するぜ☆（＾～＾）
                // PureMemory.ClearTeme();
            }

            return Pure.SUCCESSFUL_FALSE;
        }


        static readonly ICommandMojiretu tmp_syokikyokumenFen;
        /// <summary>
        /// 例: fen kr1/1h1/1H1/1R1 K2z 1
        /// 例: startpos
        /// 
        /// moves 以降は解析しないが、あれば文字列は返すぜ☆（＾～＾）
        /// </summary>
        /// <param name="line">頭に「fen 」を付けておかないと、パースに失敗する☆</param>
        /// <returns>解析の成否</returns>
        public static bool TryFail_MatchPositionvalue(
            FenSyurui f,//翻訳で切替
            string line,
            ref int caret, out string out_moves
#if DEBUG
            , IDebugMojiretu reigai1
#endif
            )
        {
            out_moves = "";

            Match m = Itiran_FenParser.GetKyokumenPattern(f).Match(line, caret);
            if (m.Success)
            {
                // キャレットを進めるぜ☆（＾▽＾）
                Util_String.SkipMatch(line, ref caret, m);

                // .Value は、該当しないときは空文字列か☆
                if (Itiran_FenParser.STARTPOS_LABEL == m.Groups[1].Value)
                {
                    // 初期局面をセットだぜ☆（＾～＾）
                    if(TryFail_SetSyokiKyokumen_ByFen(
                        f,
                        Itiran_FenParser.GetStartpos(f).Split('/'),  //1～N 段目
                        Itiran_FenParser.MOTIGOMA_NASI, // 持ち駒
                        Itiran_FenParser.TAIKYOKUSYA1
#if DEBUG
                        ,reigai1  //手番
#endif
                    )){
                        return Pure.FailTrue("Try_SetNaiyo(1)");
                    }
                }
                else
                {
                    // 初期局面をセットだぜ☆（＾～＾）
                    if (TryFail_SetSyokiKyokumen_ByFen(
                        f,
                        m.Groups[2].Value.Split('/'),  //1～N 段目
                        m.Groups[3].Value, // 持ち駒
                        m.Groups[4].Value
#if DEBUG
                        ,reigai1  //手番
#endif
                    ))
                    {
                        return Pure.FailTrue("Try_SetNaiyo(2)");
                    }
                }

                // TODO: moves
                if ("" != m.Groups[5].Value)
                {
                    out_moves = m.Groups[5].Value;
                }


                // 初期局面
                {
                    tmp_syokikyokumenFen.Clear();
                    SpkGenkyokuOpe.AppendFenTo(f, tmp_syokikyokumenFen);
                    PureMemory.kifu_syokiKyokumenFen = tmp_syokikyokumenFen.ToContents();
                }
                return Pure.SUCCESSFUL_FALSE;
            }

            {
                // FIXME:
#if DEBUG
                string msg = "パースに失敗だぜ☆（＾～＾）！ #麒麟 commandline=[" + line + "] caret=[" + caret + "]";
                reigai1.AppendLine(msg);
#endif
                return false;
            }
        }

    }
}
