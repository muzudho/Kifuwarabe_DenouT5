#if DEBUG
using kifuwarabe_shogithink.pure;
using kifuwarabe_shogithink.pure.conv;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.ky.bb;
using kifuwarabe_shogithink.pure.ky.tobikiki;
using kifuwarabe_shogithink.pure.logger;
using kifuwarabe_shogithink.pure.speak.ky;
using System.Diagnostics;
#else
using kifuwarabe_shogithink.pure.accessor;
using kifuwarabe_shogithink.pure;
using kifuwarabe_shogithink.pure.conv;
using kifuwarabe_shogithink.pure.genkyoku;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.ky.bb;
using kifuwarabe_shogithink.pure.ky.tobikiki;
using kifuwarabe_shogithink.pure.logger;
using kifuwarabe_shogithink.pure.speak.ky;
using System.Diagnostics;
#endif

namespace kifuwarabe_shogiwin.speak.ban
{
    /// <summary>
    /// 何かと情報を出力するのに使うぜ☆（＾～＾）
    /// </summary>
    public abstract class SpkBan_1Column
    {
        /// <summary>
        /// 将棋盤をコンソールへ出力するぜ☆（＾▽＾）
        /// ログに向いた、シンプルな表示☆
        /// </summary>
        /// <returns></returns>
        public static void Setumei_Bitboard(string header, Bitboard bb, IHyojiMojiretu hyoji)
        {
            Debug.Assert(null != bb, "");

            SpkBan_MultiColumn.Setumei_Bitboard(new string[] { header }, new YomiBitboard[] { new YomiBitboard(bb) },
                " △ ", "　　",
                hyoji);
        }

        /// <summary>
        /// 将棋盤をコンソールへ出力するぜ☆（＾▽＾）
        /// ログに向いた、シンプルな表示☆
        /// </summary>
        /// <returns></returns>
        public static void Setumei_Kyokumen(int teme, IHyojiMojiretu hyoji)
        {
            int banYokoHaba_tmp = PureSettei.banYokoHaba;
            int banTateHaba_tmp = PureSettei.banTateHaba;

            Setumei_Hanyo((int dan, Masu ms) =>
            {
                Koma km = PureMemory.gky_ky.yomiKy.yomiShogiban.yomiIbashoBan.GetBanjoKoma(ms);
                return SpkKoma.ToSetumei(km);
            },
            banYokoHaba_tmp, banTateHaba_tmp, teme, hyoji);
        }
        /// <summary>
        /// 将棋盤をコンソールへ出力するぜ☆（＾▽＾）
        /// ログに向いた、シンプルな表示☆
        /// </summary>
        /// <returns></returns>
        public static void Setumei_Kyokumen(int teme, OjamaBanSyurui ojamaBanSyurui, IHyojiMojiretu hyoji)
        {
            int banYokoHaba_tmp = PureSettei.banYokoHaba;
            int banTateHaba_tmp = PureSettei.banTateHaba;
            if (ojamaBanSyurui == OjamaBanSyurui.Ht90)
            {
                int tmp = banYokoHaba_tmp;
                banYokoHaba_tmp = banTateHaba_tmp;
                banTateHaba_tmp = tmp;
            }

            Setumei_Hanyo((int dan, Masu ms) =>
            {
                bool exists = PureMemory.gky_ky.yomiKy.yomiShogiban.GetYomiOjamaBan(ojamaBanSyurui).ExistsBB(ms);
                return exists ? " 〇 " : "    ";
            },
            banYokoHaba_tmp, banTateHaba_tmp, teme, hyoji);
        }
        /// <summary>
        /// ビットボード用☆（＾▽＾）
        /// </summary>
        /// <returns></returns>
        public static void Setumei_Kyokumen(int teme, Bitboard bb, IHyojiMojiretu hyoji)
        {
            int banYokoHaba_tmp = PureSettei.banYokoHaba;
            int banTateHaba_tmp = PureSettei.banTateHaba;

            Setumei_Hanyo((int dan, Masu ms) =>
            {
                bool exists = bb.IsOn(ms);
                return exists ? " 〇 " : "    ";
            },
            banYokoHaba_tmp, banTateHaba_tmp, teme, hyoji);
        }
        /// <summary>
        /// 将棋盤をコンソールへ出力するぜ☆（＾▽＾）
        /// コンソールでゲームするのに向いた表示☆
        /// </summary>
        /// <returns></returns>
        public static void Setumei_NingenGameYo(int teme, IHyojiMojiretu hyoji)
        {
            int banYokoHaba_tmp = PureSettei.banYokoHaba;
            int banTateHaba_tmp = PureSettei.banTateHaba;
            Setumei_Hanyo((int dan, Masu ms) =>
                {
                    Koma km = PureMemory.gky_ky.yomiKy.yomiShogiban.yomiIbashoBan.GetBanjoKoma(ms);
                    return SpkKoma.ToSetumei(km);
                },
                banYokoHaba_tmp, banTateHaba_tmp, teme, hyoji);
        }

        delegate string DLGT_dataBu(int dan, Masu ms);
        /// <summary>
        /// 汎用
        /// </summary>
        /// <returns></returns>
        static void Setumei_Hanyo(
            DLGT_dataBu dlgt_dataBu,
            int banYokoHaba_tmp,
            int banTateHaba_tmp,
            int teme,
            IHyojiMojiretu hyoji
            )
        {
            #region 盤の上の方
            // 2行目
            {
                // 何手目
                hyoji.Append(string.Format("図は{0,3}手まで ", teme));

                // 手番
                SpkTaikyokusya.AppendSetumeiName(PureMemory.GetTebanByTeme(teme), hyoji);
                hyoji.Append("の番");

                hyoji.AppendLine();
            }

            // 3行目 後手の持ち駒
            {
                // 後手の持ち駒の数
                foreach (MotigomaSyurui mks in Conv_MotigomaSyurui.itiran)
                {
                    Motigoma mk = Med_Koma.MotiKomasyuruiAndTaikyokusyaToMotiKoma(mks, Taikyokusya.T2);
                    if (PureMemory.gky_ky.yomiKy.yomiMotigomaItiran.HasMotigoma(mk))
                    {
                        hyoji.Append(Conv_MotigomaSyurui.GetHyojiName(mks)); hyoji.Append(PureMemory.gky_ky.yomiKy.yomiMotigomaItiran.Count(mk).ToString());
                    }
                }
                hyoji.AppendLine();
            }

            // 4行目
            {
                // Ａ　Ｂ　Ｃ　Ｄ　とか
                hyoji.Append("  ");
                SpkBanWaku.AppendLine_SujiFugo(banYokoHaba_tmp, hyoji);
            }

            // 5行目
            {
                hyoji.Append("  ");
                SpkBanWaku.AppendLine_TopBar(1, PureSettei.banYokoHaba, hyoji); // ┌──┬──┬──┐
            }
            #endregion

            // 5行目～13行目
            // 盤上
            for (int dan = 0; dan < PureSettei.banTateHaba; dan++)
            {
                // 6,8,10,12行目
                hyoji.Append(Conv_Kihon.ToZenkakuInteger(dan + 1));

                SpkBanWaku.AppendLine_Record_Cell4Hankakus2(
                    (Masu ms) => {
                        return dlgt_dataBu(dan, ms);
                    },
                    dan, banYokoHaba_tmp, hyoji);

                if (dan + 1 < PureSettei.banTateHaba)
                {
                    // 7,9,11行目
                    hyoji.Append("  ");
                    SpkBanWaku.AppendLine_MiddleBar(1, PureSettei.banYokoHaba, hyoji);//├──┼──┼──┤
                }
            }

            #region 盤の下の方
            // 13行目
            {
                hyoji.Append("  ");
                SpkBanWaku.AppendLine_BottomBar(1, PureSettei.banYokoHaba, hyoji);//└──┴──┴──┘
            }

            // 先手の持ち駒の数
            {
                foreach (MotigomaSyurui mks in Conv_MotigomaSyurui.itiran)
                {
                    Motigoma mk = Med_Koma.MotiKomasyuruiAndTaikyokusyaToMotiKoma(mks, Taikyokusya.T1);
                    if (PureMemory.gky_ky.yomiKy.yomiMotigomaItiran.HasMotigoma(mk))
                    {
                        hyoji.Append(Conv_MotigomaSyurui.GetHyojiName(mks)); hyoji.Append(PureMemory.gky_ky.yomiKy.yomiMotigomaItiran.Count(mk).ToString());
                    }
                }
                hyoji.AppendLine();
            }
            #endregion
        }

        /// <summary>
        /// 居場所（対局者別ですべての駒）
        /// </summary>
        /// <param name="hyoji"></param>
        public static void ToHyojiIbasho(string header, IHyojiMojiretu hyoji)
        {
            IbashoBan.YomiIbashoBan yomiIbashoBan = PureMemory.gky_ky.shogiban.ibashoBan_yk00.yomiIbashoBan;
            AppendKomaZenbuIbashoTo(header, yomiIbashoBan, hyoji);
            AppendKomaBetuIbashoTo(yomiIbashoBan, hyoji);
        }

        /// <summary>
        /// 居場所（対局者別ですべての駒）
        /// </summary>
        /// <param name="hyoji"></param>
        static void AppendKomaZenbuIbashoTo(string header, IbashoBan.YomiIbashoBan yomiIbashoBan, IHyojiMojiretu hyoji)
        {
            hyoji.AppendLine(string.Format("居場所（対局者別） {0}", header));
            SpkBan_MultiColumn.Setumei_Bitboard(
                new string[] { "対局者１", "対局者２" },
                new Bitboard[] {
                    yomiIbashoBan.CloneKomaZenbu(Taikyokusya.T1),//.GetKomaZenbu(Taikyokusya.T1)
                    yomiIbashoBan.CloneKomaZenbu(Taikyokusya.T2)//GetKomaZenbu(Taikyokusya.T2)
                }, hyoji);
            hyoji.AppendLine();
        }
        /// <summary>
        /// 駒別の居場所
        /// </summary>
        /// <param name="hyoji"></param>
        static void AppendKomaBetuIbashoTo(IbashoBan.YomiIbashoBan yomiIbashoBan, IHyojiMojiretu hyoji)
        {
            hyoji.AppendLine("駒別の居場所");
            foreach (Taikyokusya tai in Conv_Taikyokusya.itiran)// 対局者１、対局者２
            {
                // 駒別
                foreach (Koma km_tai in Conv_Koma.itiranTai[(int)tai])
                {
                    hyoji.Append(SpkBanWaku.CutHeaderBanWidthZenkaku(Conv_Koma.GetName(km_tai)));
                }
                hyoji.AppendLine();

                // 盤
                YomiBitboard[] bbHairetu = new YomiBitboard[Conv_Komasyurui.itiran.Length];
                int i = 0;
                foreach (Komasyurui ks in Conv_Komasyurui.itiran)
                {
                    bbHairetu[i] = yomiIbashoBan.GetKoma(Med_Koma.KomasyuruiAndTaikyokusyaToKoma(ks, tai));
                    i++;
                }
                SpkBan_MultiColumn.Setumei_Bitboard(null, bbHairetu,
                    " 〇 ", "　　",
                    hyoji);
            }
        }

        public static void HyojiKomaHairetuYososuMade(Masu ms, Koma[] kmHairetu, IHyojiMojiretu hyoji)
        {
            hyoji.Append("置くか除けるかした升=["+(Masu)ms+"] 関連する飛び利き駒一覧=[");
            foreach (Koma km_var in kmHairetu)
            {
                if (Koma.Yososu == km_var)
                {
                    break;
                }
                hyoji.Append(km_var.ToString());
            }
            hyoji.AppendLine("]");
        }

        public static void Setumei_Discovered(Masu ms_removed, IHyojiMojiretu hyoji)
        {
            Koma[] kmHairetu_control;
            PureMemory.gky_ky.yomiKy.TryInControl(ms_removed, out kmHairetu_control);

            Bitboard bb_relative = new Bitboard();//関連のある飛び利き駒

            // 飛び利きを計算し直す
            foreach (Koma km_var in kmHairetu_control)
            {
                if (Koma.Yososu == km_var) { break; }

                // 駒の居場所
                Bitboard bb_ibasho = new Bitboard();
                PureMemory.gky_ky.yomiKy.yomiShogiban.yomiIbashoBan.ToSet_Koma(km_var, bb_ibasho);

                Masu ms_ibasho;
                while (bb_ibasho.Ref_PopNTZ(out ms_ibasho))
                {
                    bb_relative.Standup(ms_ibasho);
                }
            }

            Setumei_Bitboard("関連する飛び利き駒", bb_relative, hyoji);
        }
    }
}
