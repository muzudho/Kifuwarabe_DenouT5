#if DEBUG
using kifuwarabe_shogithink.pure.accessor;
using kifuwarabe_shogithink.pure.ky;
#else
using kifuwarabe_shogithink.pure.accessor;
using kifuwarabe_shogithink.pure.genkyoku;
using kifuwarabe_shogithink.pure.ky;
#endif

namespace kifuwarabe_shogithink.pure.com.hyoka
{
    /// <summary>
    /// 評価値☆（＾～＾）
    /// 
    /// アルファ・ベータ探索で使う評価の数値だぜ☆（＾～＾）
    /// 詰め手数（メート）も兼ねているぜ☆（＾▽＾）
    /// </summary>
    public class Hyokati
    {
        #region 生成
        public Hyokati()
        {

        }
        public Hyokati(Hyokati src)
        {
            hyokaTen_ = src.hyokaTen;
            tumeSu = src.tumeSu;
            isHaki = src.isHaki;
#if DEBUG
            dbg_riyu = src.dbg_riyu;
            dbg_riyuHosoku = src.dbg_riyuHosoku;
#endif
        }
        public Hyokati(
            int hyokaTen,
            int tumeSu,
            bool isHaki
#if DEBUG
            ,int dbg_okimari,
            int dbg_himowari,
            int dbg_kikiwari,
            HyokaRiyu dbg_riyu,
            string dbg_riyuHosoku
#endif
            )
        {
            this.hyokaTen_ = hyokaTen;
            this.tumeSu = tumeSu;
            this.isHaki = isHaki;
#if DEBUG
            this.dbg_riyu = dbg_riyu;
            this.dbg_riyuHosoku = dbg_riyuHosoku;
#endif
        }
#endregion
#region プロパティ―
        /// <summary>
        /// 内訳ではない、トータルの評価値だぜ☆（＾～＾）
        /// </summary>
        public int hyokaTen { get { return hyokaTen_; } }
        int hyokaTen_;
        /// <summary>
        /// メート☆（＾～＾）
        /// 
        /// 偶数なら　手番の負け、
        /// 奇数なら　手番の勝ちだぜ☆（＾▽＾）
        /// 
        /// 使わないときは -1 にしてけだぜ☆（＾～＾）
        /// </summary>
        public int tumeSu;
        /// <summary>
        /// 破棄☆（＾～＾）
        /// 時間切れでループを抜けた場合など☆（＾～＾）
        /// </summary>
        public bool isHaki;
#if DEBUG
        /// <summary>
        /// 読み筋用の評価理由
        /// </summary>
        public HyokaRiyu dbg_riyu;
        /// <summary>
        /// 評価理由のコメント
        /// </summary>
        public string dbg_riyuHosoku;
#endif
        #endregion
        #region 作り直し
        public void Clear()
        {
            hyokaTen_ = Conv_Hyokati.Hyokati_Rei;
            tumeSu = Conv_Hyokati.Hyokati_Rei;
            isHaki = false;
#if DEBUG
            dbg_riyu = HyokaRiyu.Yososu;
            dbg_riyuHosoku = "";
#endif
        }
        #endregion

        #region 影響
        public void ToSet(
            Hyokati src
            )
        {
            src.hyokaTen_ = hyokaTen;
            src.tumeSu = tumeSu;
            src.isHaki = isHaki;
#if DEBUG
            src.dbg_riyu = dbg_riyu;
            src.dbg_riyuHosoku = dbg_riyuHosoku;
#endif
        }
        #endregion
        #region 編集
        public void Set(
            int hyokaTen,
            int tumeSu,
            bool isHaki
#if DEBUG
            , HyokaRiyu dbg_riyu
            , string dbg_riyuHosoku
#endif
            )
        {
            this.hyokaTen_ = hyokaTen;
            this.tumeSu = tumeSu;
            this.isHaki = isHaki;
#if DEBUG
            this.dbg_riyu = dbg_riyu;
            this.dbg_riyuHosoku = dbg_riyuHosoku;
#endif
        }
        public void AddKomawari(int ten)
        {
            hyokaTen_ += ten;
        }
        public bool IsMinusTen()
        {
            return hyokaTen < 0;
        }
        /// <summary>
        /// 詰みの場合、数字をカウントアップするぜ☆（＾▽＾）
        /// </summary>
        /// <returns></returns>
        public void CountUpTume()
        {
            if (tumeSu != Conv_Tumesu.None)
            {
                tumeSu++; // 何手詰めの数字が大きくなるぜ☆
            }
        }
        #endregion


        public string ToString_Ten()
        {
            return hyokaTen.ToString();
        }
        public bool IsKatu()
        {
            return tumeSu != Conv_Tumesu.None && tumeSu % 2 == 0;
        }
        public int GetKatu()
        {
            if (IsKatu())
            {
                return tumeSu;
            }
            else
            {
                return Conv_Tumesu.None;
            }
        }
        public bool IsMakeru()
        {
            return tumeSu != Conv_Tumesu.None && tumeSu % 2 == 1;
        }
        public int GetMakeru()
        {
            if (IsMakeru())
            {
                return tumeSu;
            }
            else
            {
                return Conv_Tumesu.None;
            }
        }

        /// <summary>
        /// 符号を反転したものを返す
        /// </summary>
        public void ToHanten()
        {
            Set(
                -hyokaTen,
                tumeSu,
                isHaki
#if DEBUG
                , dbg_riyu
                , dbg_riyuHosoku
#endif
                );
        }


    }

    public static class Conv_Tumesu
    {
        /// <summary>
        /// 詰めが発生していないときだぜ☆（＾～＾）
        /// </summary>
        public const int None = -3;
        /// <summary>
        /// －２手詰め☆　手番が相手のらいおんを取った時点だぜ☆（＾～＾）
        /// </summary>
        public const int CatchRaion = -2;
        /// <summary>
        /// 合法手なし、ステイルメイト（－１手詰められの別名）
        /// </summary>
        public const int Stalemate = -1;
    }


    public static class Conv_Hyokati
    {
        /// <summary>
        /// ０点
        /// </summary>
        public const int Hyokati_Rei = 0;
    }

}
