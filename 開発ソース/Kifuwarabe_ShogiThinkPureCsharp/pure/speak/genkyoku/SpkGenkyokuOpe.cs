#if DEBUG
using kifuwarabe_shogithink.cui.ky;
using kifuwarabe_shogithink.pure.accessor;
using kifuwarabe_shogithink.pure.control;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.logger;
using kifuwarabe_shogithink.pure.speak.ky;
#else
using kifuwarabe_shogithink.cui.ky;
using kifuwarabe_shogithink.pure.control;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.logger;
using kifuwarabe_shogithink.pure.speak.ky;
#endif

namespace kifuwarabe_shogithink.pure.speak.genkyoku
{
    public static class SpkGenkyokuOpe
    {
        /// <summary>
        /// 改造Fen
        /// 例： fen kr1/1h1/1H1/1R1 K2z 1
        /// 盤上の駒配置、持ち駒の数、手番の対局者
        /// </summary>
        public static void AppendFenTo(
            FenSyurui f, ICommandMojiretu syuturyoku)
        {
            syuturyoku.Append(f==FenSyurui.sfe_n ? "sfen " : "fen ");

            // 盤上
            {
                int space = 0;

                for (int iDan = 0; iDan < PureSettei.banTateHaba; iDan++)
                {
                    for (int iSuji = 0; iSuji < PureSettei.banYokoHaba; iSuji++)
                    {
                        Masu ms = (Masu)(iDan * PureSettei.banYokoHaba + iSuji);

                        Taikyokusya tai;
                        if (PureMemory.gky_ky.yomiKy.yomiShogiban.yomiIbashoBan.ExistsKomaZenbu(ms, out tai))
                        {
                            if (0 < space)
                            {
                                syuturyoku.Append(space.ToString());
                                space = 0;
                            }

                            Komasyurui ks;
                            PureMemory.gky_ky.yomiKy.yomiShogiban.yomiIbashoBan.ExistsKoma(tai, ms, out ks);
                            SpkKoma.AppendFenTo(f, Med_Koma.KomasyuruiAndTaikyokusyaToKoma(ks, tai), syuturyoku);
                        }
                        else
                        {
                            space++;
                        }
                    }

                    if (0 < space)
                    {
                        syuturyoku.Append(space.ToString());
                        space = 0;
                    }

                    if (iDan + 1 < PureSettei.banTateHaba)
                    {
                        syuturyoku.Append("/");
                    }
                }
            }

            syuturyoku.Append(" ");

            // 持駒
            if (PureMemory.gky_ky.yomiKy.yomiMotigomaItiran.IsEmpty())
            {
                syuturyoku.Append("-");
            }
            else
            {
                for (int iMk = 0; iMk < Conv_Motigoma.itiran.Length; iMk++)
                {
                    int cnt = PureMemory.gky_ky.yomiKy.yomiMotigomaItiran.Count((Motigoma)iMk);
                    if (0 < cnt)
                    {
                        syuturyoku.Append(
                            cnt == 1 ?
                            SpkMotiKoma.GetFen(f, (Motigoma)iMk)// １個の時は数字は付かないぜ☆（＾～＾）
                            :
                            cnt.ToString() + SpkMotiKoma.GetFen(f, (Motigoma)iMk)
                            );
                    }
                }
            }

            // 手番
            syuturyoku.Append(" ");
            syuturyoku.Append(SpkTaikyokusya.ToFen(f, PureMemory.kifu_teban));

            //// moves
            //if (syuturyokuMoves)
            //{

            //}
        }

        /// <summary>
        /// 通信用
        /// fen(盤上の駒配置、持ち駒の数、手番の対局者) 何手目 同形反復の回数
        /// </summary>
        /// <param name="syuturyoku"></param>
        public static void TusinYo_Line( FenSyurui f, ICommandMojiretu syuturyoku)
        {
            // まず、fen を返すぜ☆（＾▽＾）
            // 盤上の駒配置、持ち駒の数、手番の対局者☆
            SpkGenkyokuOpe.AppendFenTo(f, syuturyoku);
            // 次は空白☆（＾～＾）
            syuturyoku.Append(" ");

            // 何手目かは 1 固定☆（＾▽＾）
            syuturyoku.Append("1");
            // 次は空白☆（＾～＾）
            syuturyoku.Append(" ");

            // #仲ルール かどうかは出さないぜ☆（＾▽＾）

            syuturyoku.AppendLine();
        }
        ///// <summary>
        ///// 通信用（簡易）
        ///// </summary>
        ///// <param name="syuturyoku"></param>
        //public void TusinYo_Kani(Mojiretu syuturyoku)
        //{
        //    // 盤上の駒（[0]文字目～[11]文字目）１桁という前提だぜ☆（＾▽＾）
        //    for (int iMs=0; iMs<KyokumenImpl.MASUS; iMs++)
        //    {
        //        Conv_Koma.TusinYo(this.BanjoKomas[iMs], syuturyoku);
        //    }

        //    // 先手の持ち駒（[12]文字目～[17]文字目）１桁という前提だぜ☆（＾▽＾）
        //    syuturyoku.Append(this.MotiKomas[(int)MotiKoma.Z].ToString());
        //    syuturyoku.Append(this.MotiKomas[(int)MotiKoma.K].ToString());
        //    syuturyoku.Append(this.MotiKomas[(int)MotiKoma.H].ToString());
        //    syuturyoku.Append(this.MotiKomas[(int)MotiKoma.z].ToString());
        //    syuturyoku.Append(this.MotiKomas[(int)MotiKoma.k].ToString());
        //    syuturyoku.Append(this.MotiKomas[(int)MotiKoma.h].ToString());

        //    // [18]文字目は空白☆（＾～＾）
        //    syuturyoku.Append(" ");

        //    // 手番（[19]文字目）１桁という前提だぜ☆（＾▽＾）
        //    Conv_Taikyokusya.TusinYo(this.Teban, syuturyoku);
        //    // 対局者名は出さないぜ☆（＾～＾）

        //    // 何手目（[20]文字目～[22]文字目）３桁を用意☆（＾▽＾）
        //    syuturyoku.Append(string.Format("{0,3}", this.Konoteme.ScanNantemadeBango()));
        //    // [23]文字目は空白☆（＾～＾）
        //    syuturyoku.Append(" ");

        //    // #仲ルール かどうかは出さないぜ☆（＾▽＾）

        //    syuturyoku.AppendLine();
        //}

    }
}
