#if DEBUG
using kifuwarabe_shogithink.pure.ky.bb;
using System;
using Grayscale.Kifuwarabi.Entities.Take1Base;
#else
using kifuwarabe_shogithink.pure.ky.bb;
using System;
using Grayscale.Kifuwarabi.Entities.Take1Base;
#endif

namespace kifuwarabe_shogithink.pure.ky
{
    /// <summary>
    /// 居場所盤（Occupied Bitboard）
    /// 
    /// ヨコ型が基本☆（＾～＾）
    /// </summary>
    public class IbashoBan
    {
        public class YomiIbashoBan
        {
            public YomiIbashoBan(IbashoBan hontai)
            {
                hontai_ = hontai;
            }
            IbashoBan hontai_;

            #region 駒全部スキャン
            public delegate void DLGT_scanKomaZenbu(Masu ms);
            public void ScanKomaZenbu(DLGT_scanKomaZenbu dlgt_scanKomaZenbu, Taikyokusya tai)
            {
                // 駒全部
                Bitboard bbVar_komaZenbu = CloneKomaZenbu(tai);
                Masu ms_ibasho;
                while (bbVar_komaZenbu.Ref_PopNTZ(out ms_ibasho))
                {
                    dlgt_scanKomaZenbu(ms_ibasho);
                }
            }
            #endregion

            #region 状況（駒全部）
            public Bitboard CloneKomaZenbu(Taikyokusya tai)
            {
                return hontai_.BBItiran_Komazenbu.CloneBb(tai);
            }
            public YomiBitboard GetKomaZenbu(Taikyokusya tai)
            {
                return new YomiBitboard(hontai_.BBItiran_Komazenbu.RefBBKomaZenbu(tai));
            }
            public Bitboard CloneKomaZenbuBothTai()
            {
                Bitboard bb = hontai_.BBItiran_Komazenbu.CloneBb(Taikyokusya.T1);
                hontai_.BBItiran_Komazenbu.ToStandup(Taikyokusya.T2, bb);
                return bb;
            }
            public bool ExistsKomaZenbu(Masu ms, out Taikyokusya out_tai)
            {
                return hontai_.BBItiran_Komazenbu.Exists(ms, out out_tai);
            }
            public bool ExistsKomaZenbu(Taikyokusya tai, Masu ms)
            {
                return hontai_.BBItiran_Komazenbu.IsOn(tai,ms);
            }
            public bool ExistsKomaZenbu(Masu ms)
            {
                return hontai_.BBItiran_Komazenbu.Exists(ms);
            }
            /// <summary>
            /// 内部でクローンしてるので、探索に使うには重いぜ☆（＾～＾）
            /// 開発中用だぜ☆（＾▽＾）
            /// </summary>
            /// <param name="tai"></param>
            /// <param name="yomiIbashoBan2"></param>
            /// <returns></returns>
            public bool Equals_KomaZenbu_ForDevelop(Taikyokusya tai, IbashoBan.YomiIbashoBan yomiIbashoBan2)
            {
                return hontai_.BBItiran_Komazenbu.RefBBKomaZenbu(tai) == yomiIbashoBan2.CloneKomaZenbu(tai);
            }
            /// <summary>
            /// 指定のビットボードを更新します
            /// </summary>
            /// <param name="tai"></param>
            /// <param name="bb_update"></param>
            public void ToStandup_KomaZenbu(Taikyokusya tai, Bitboard bb_update)
            {
                hontai_.BBItiran_Komazenbu.ToStandup(tai,bb_update);
            }
            public void ToSitdown_KomaZenbu(Taikyokusya tai, Bitboard bb_update)
            {
                hontai_.BBItiran_Komazenbu.ToSitdown(tai, bb_update);
            }
            public void ToSet_KomaZenbu(Taikyokusya tai, Bitboard bb_update)
            {
                hontai_.BBItiran_Komazenbu.ToSet(tai, bb_update);
            }
            public void ToSelect_KomaZenbu(Taikyokusya tai, Bitboard bb_update)
            {
                hontai_.BBItiran_Komazenbu.ToSelect(tai, bb_update);

            }
            public Piece GetBanjoKoma(Masu ms)
            {
                Taikyokusya tai;
                if (ExistsKomaZenbu(ms, out tai))
                {
                    Komasyurui ks;
                    if (ExistsKoma(tai, ms, out ks))
                    {
                        return Med_Koma.KomasyuruiAndTaikyokusyaToKoma(ks, tai);
                    }
                }
                return Piece.Kuhaku;
            }
            #endregion

            #region 状況（駒別）
            public Piece GetBanjoKoma(Taikyokusya tai, Masu ms)
            {
                Komasyurui ks;
                if (ExistsKoma(tai, ms, out ks))
                {
                    return Med_Koma.KomasyuruiAndTaikyokusyaToKoma(ks, tai);
                }
                return Piece.Kuhaku;
            }
            public bool IsIntersect(Piece km, Bitboard bb)
            {
                return hontai_.BBItiran_Komabetu.RefBBKoma(km).IsIntersect(bb);
            }
            public bool GetNTZ(Piece km, out Masu out_ms)
            {
                return hontai_.BBItiran_Komabetu.RefBBKoma(km).GetNTZ(out out_ms);
            }
            public bool IsEmpty(Piece km)
            {
                return hontai_.BBItiran_Komabetu.RefBBKoma(km).IsEmpty();
            }
            public Bitboard CloneKoma(Piece km)
            {
                return hontai_.BBItiran_Komabetu.RefBBKoma(km).Clone();
            }
            public YomiBitboard GetKoma(Piece km)
            {
                return new YomiBitboard(hontai_.BBItiran_Komabetu.RefBBKoma(km));
            }
            public bool IsOn(Piece km, Masu ms)
            {
                return hontai_.BBItiran_Komabetu.RefBBKoma(km).IsOn(ms);
            }
            public bool IsEmptyKoma(Piece km)
            {
                return hontai_.BBItiran_Komabetu.RefBBKoma(km).IsEmpty();
            }
            public bool ExistsKoma(Taikyokusya tai, Masu ms, out Komasyurui ks)
            {
                return hontai_.BBItiran_Komabetu.Exists(tai, ms, out ks);
            }
            public bool ExistsKoma(Taikyokusya tai, Masu ms)
            {
                return hontai_.BBItiran_Komabetu.Exists(tai, ms);
            }
            public bool ExistsKoma(Piece km, Masu ms)
            {
                return hontai_.BBItiran_Komabetu.RefBBKoma(km).IsOn(ms);
            }
            /// <summary>
            /// 内部でクローンしてるので、探索に使うには重いぜ☆（＾～＾）
            /// 開発中用だぜ☆（＾▽＾）
            /// </summary>
            /// <param name="tai"></param>
            /// <param name="yomiIbashoBan2"></param>
            /// <returns></returns>
            public bool Equals_Koma_ForDevelop(Piece km, IbashoBan.YomiIbashoBan yomiIbashoBan2)
            {
                return hontai_.CloneBb_Koma(km) == yomiIbashoBan2.CloneKoma(km);
            }
            #endregion
            #region 影響（駒別）
            public bool ToIsIntersect_Koma(Piece km, Bitboard bb_target)
            {
                return hontai_.BBItiran_Komabetu.ToIsIntersect(km, bb_target);
            }
            public void ToSet_Koma(Piece km, Bitboard bb_update)
            {
                bb_update.Set(hontai_.BBItiran_Komabetu.RefBBKoma(km));
            }
            public void ToStandup_Koma(Piece km, Bitboard bb_update)
            {
                bb_update.Standup(hontai_.BBItiran_Komabetu.RefBBKoma(km));
            }
            public void ToSitdown_Koma(Piece km, Bitboard bb_update)
            {
                bb_update.Sitdown(hontai_.BBItiran_Komabetu.RefBBKoma(km));
            }
            public void ToSelect_Koma(Piece km, Bitboard bb_update)
            {
                bb_update.Siborikomi(hontai_.BBItiran_Komabetu.RefBBKoma(km));
            }
            #endregion
        }
        public YomiIbashoBan yomiIbashoBan;
        #region 生成
        public IbashoBan()
        {
            BBItiran_Komazenbu = new KomaZenbuIbasyoBitboardItiran();
            BBItiran_Komabetu = new IbasyoKomabetuBitboardItiran();
            yomiIbashoBan = new YomiIbashoBan(this);
        }
        public IbashoBan(IbashoBan src)
        {
            Tukurinaosi_Copy(src);
            yomiIbashoBan = new YomiIbashoBan(this);
        }
        #endregion
        /// <summary>
        /// 
        /// </summary>
        public void Tukurinaosi_Remake()
        {
            BBItiran_Komazenbu.Tukurinaosi_Remake();
            BBItiran_Komabetu.Tukurinaosi_Remake();
        }
        public void Tukurinaosi_Copy(IbashoBan src)
        {
            BBItiran_Komabetu.Tukurinaosi_Copy(src.BBItiran_Komabetu);
            BBItiran_Komazenbu.Tukurinaosi_Copy(src.BBItiran_Komazenbu);
        }
        public void Tukurinaosi_Clear()
        {
            BBItiran_Komabetu.Tukurinaosi_Clear();
            BBItiran_Komazenbu.Tukurinaosi_Clear();
        }


        /// <summary>
        /// [駒]
        /// ビットボード。駒別の 盤上の駒のいる升☆
        /// </summary>
        IbasyoKomabetuBitboardItiran BBItiran_Komabetu { get; set; }
        /// <summary>
        /// [手番]
        /// ビットボード。盤上の駒のいる升☆
        /// </summary>
        KomaZenbuIbasyoBitboardItiran BBItiran_Komazenbu { get; set; }

        /// <summary>
        /// 居場所ビットボード一覧（対局者別で全ての駒）
        /// </summary>
        class KomaZenbuIbasyoBitboardItiran
        {
            #region 生成
            public KomaZenbuIbasyoBitboardItiran()
            {
                Tukurinaosi_Remake();
            }
            public KomaZenbuIbasyoBitboardItiran(KomaZenbuIbasyoBitboardItiran src)
            {
                Tukurinaosi_Copy(src);
            }
            #endregion
            #region プロパティ―
            /// <summary>
            /// [手番]
            /// ビットボード。盤上の駒のいる升☆
            /// 
            /// どうぶつしょうぎで言うと A1 が最下位ビット。
            /// 本将棋で言うと ９一 が最下位ビット。
            /// </summary>
            Bitboard[] valueTai { get; set; }
            #endregion
            #region 作り直し
            /// <summary>
            /// 
            /// </summary>
            /// <param name="bbItiran_sourceKomazenbu"></param>
            public void Tukurinaosi_Remake()
            {
                valueTai = new Bitboard[Conv_Taikyokusya.itiran.Length];
                for (int i = 0; i < Conv_Taikyokusya.itiran.Length; i++)
                {
                    valueTai[i] = new Bitboard();
                }
            }
            /// <summary>
            /// 駒数が変更されていない場合、利用可能
            /// </summary>
            public void Tukurinaosi_Copy(KomaZenbuIbasyoBitboardItiran source)
            {
                valueTai = new Bitboard[source.valueTai.Length];
                Array.Copy(source.valueTai, valueTai, source.valueTai.Length);
            }
            /// <summary>
            /// 駒数が変更されていない場合、利用可能
            /// </summary>
            public void Tukurinaosi_Clear()
            {
                foreach (Bitboard bb in valueTai)
                {
                    bb.Clear();
                }
            }
            #endregion
            #region 状況
            public bool IsOn(Taikyokusya tai, Masu ms)
            {
                return valueTai[(int)tai].IsOn(ms);
            }
            public Bitboard CloneBb(Taikyokusya tai)
            {
                return valueTai[(int)tai].Clone();
            }
            #endregion
            #region 影響
            public void ToStandup(Taikyokusya tai, Bitboard bb_update)
            {
                bb_update.Standup(valueTai[(int)tai]);
            }
            public void ToSitdown(Taikyokusya tai, Bitboard bb_update)
            {
                bb_update.Sitdown(valueTai[(int)tai]);
            }
            public void ToSelect(Taikyokusya tai, Bitboard bb_update)
            {
                bb_update.Siborikomi(valueTai[(int)tai]);
            }
            public void ToSet(Taikyokusya tai, Bitboard bb_update)
            {
                bb_update.Set(valueTai[(int)tai]);
            }
            #endregion
            #region 編集
            public void Set(Taikyokusya tai, Bitboard bb_src)
            {
                valueTai[(int)tai].Set(bb_src);
            }
            public void Sitdown(Taikyokusya tai, Masu ms)
            {
                valueTai[(int)tai].Sitdown(ms);
            }
            public void Standup(Taikyokusya tai, Masu ms)
            {
                valueTai[(int)tai].Standup(ms);
            }
            public void Bitflip128(Taikyokusya tai, Masu ms)
            {
                valueTai[(int)tai].Bitflip128();
            }
            #endregion

            public Bitboard RefBBKomaZenbu(Taikyokusya tai)
            {
                return valueTai[(int)tai];
            }
            public bool Exists(Masu ms, out Taikyokusya out_tai)
            {
                for (int iTai = 0; iTai < Conv_Taikyokusya.itiran.Length; iTai++)
                {
                    out_tai = (Taikyokusya)iTai;
                    if (valueTai[iTai].IsOn(ms)) { return true; }
                }
                out_tai = Taikyokusya.Yososu;
                return false;
            }
            public bool Exists(Masu ms)
            {
                for (int iTai = 0; iTai < Conv_Taikyokusya.itiran.Length; iTai++)
                {
                    if (valueTai[iTai].IsOn(ms)) { return true; }
                }
                return false;
            }
        }
        /// <summary>
        /// 駒別居場所ビットボード一覧
        /// </summary>
        class IbasyoKomabetuBitboardItiran
        {
            #region 生成
            public IbasyoKomabetuBitboardItiran()
            {
                Tukurinaosi_Remake();
            }
            public IbasyoKomabetuBitboardItiran(IbasyoKomabetuBitboardItiran src)
            {
                Tukurinaosi_Copy(src);
            }
            #endregion
            #region プロパティ―
            /// <summary>
            /// [駒]
            /// ビットボード。駒別の 盤上の駒のいる升☆
            /// </summary>
            Bitboard[] valueKm { get; set; }
            #endregion
            #region 作り直し
            /// <summary>
            /// 
            /// </summary>
            public void Tukurinaosi_Remake()
            {
                valueKm = new Bitboard[Conv_Koma.itiran.Length];
                for (int iKm = 0; iKm < Conv_Koma.itiran.Length; iKm++)
                {
                    valueKm[iKm] = new Bitboard();
                }
            }
            /// <summary>
            /// 駒数が変更されていない場合、利用可能
            /// </summary>
            public void Tukurinaosi_Copy(IbasyoKomabetuBitboardItiran src)
            {
                valueKm = new Bitboard[src.valueKm.Length];
                Array.Copy(src.valueKm, valueKm, src.valueKm.Length);
            }
            /// <summary>
            /// 駒数が変更されていない場合、利用可能
            /// </summary>
            public void Tukurinaosi_Clear()
            {
                foreach (Bitboard bb in valueKm)
                {
                    bb.Clear();
                }
            }
            #endregion
            #region 状況
            public Bitboard CloneBb(Piece km)
            {
                return valueKm[(int)km].Clone();
            }
            #endregion
            #region 編集
            public void Set(Piece km, Bitboard bb_src)
            {
                valueKm[(int)km].Set(bb_src);
            }
            #endregion
            #region 影響
            public bool ToIsIntersect(Piece km, Bitboard bb_target)
            {
                return bb_target.IsIntersect(valueKm[(int)km]);
            }
            #endregion

            public Bitboard RefBBKoma(Piece km)
            {
                return valueKm[(int)km];
            }
            public bool Exists(Taikyokusya tai, Masu ms, out Komasyurui out_ks)
            {
                for (int iKm = 0; iKm < Conv_Koma.itiranTai[(int)tai].Length; iKm++)
                {
                    Piece km = Conv_Koma.itiranTai[(int)tai][iKm];
                    if (valueKm[(int)km].IsOn(ms))
                    {
                        out_ks = Med_Koma.KomaToKomasyurui(km);
                        return true;
                    }
                }
                out_ks = Komasyurui.Yososu;
                return false;
            }
            public bool Exists(Taikyokusya tai, Masu ms)
            {
                for (int iKm = 0; iKm < Conv_Koma.itiranTai[(int)tai].Length; iKm++)
                {
                    if (valueKm[(int)Conv_Koma.itiranTai[(int)tai][iKm]].IsOn(ms)) { return true; }
                }
                return false;
            }
        }

        public Bitboard CloneBb_Koma(Piece km)
        {
            return BBItiran_Komabetu.CloneBb(km);
        }
        public void Set_Koma(Piece km, Bitboard bb_src)
        {
            BBItiran_Komabetu.Set(km, bb_src);
        }
        public Bitboard CloneBB_KomaZenbu(Taikyokusya tai)
        {
            return BBItiran_Komazenbu.CloneBb(tai);
        }
        public void Set_KomaZenbu(Taikyokusya tai, Bitboard bb_src)
        {
            BBItiran_Komazenbu.Set(tai, bb_src);
        }

        /// <summary>
        /// 駒を置きます
        /// </summary>
        /// <param name="km"></param>
        /// <param name="ms"></param>
        public void N240_OkuKoma(Piece km, Masu ms)
        {
            BBItiran_Komazenbu.Standup(Med_Koma.KomaToTaikyokusya(km),ms);
            BBItiran_Komabetu.RefBBKoma(km).Standup(ms);
        }
        /// <summary>
        /// 盤上の駒を取り除きます
        /// 
        /// よくある問題
        /// ──────
        /// 
        /// （１）きりんＡ　の右上に　きりんＢ　を打つ。
        /// （２）きりんＢ　を取り除く。
        /// （３）このとき、きりんＢ　の利きも取り除くが、きりんＡ　と被っている利きもある。
        /// これを消してしまうと、利きが欠けた　きりんＡ　ができてしまい、整合性が取れない。
        /// 
        /// </summary>
        /// <param name="km"></param>
        /// <param name="ms"></param>
        public void N240_TorinozokuKoma(Piece km, Masu ms)
        {

            BBItiran_Komazenbu.Sitdown(Med_Koma.KomaToTaikyokusya(km), ms);

            BBItiran_Komabetu.RefBBKoma(km).Sitdown(ms);
        }


    }
}
