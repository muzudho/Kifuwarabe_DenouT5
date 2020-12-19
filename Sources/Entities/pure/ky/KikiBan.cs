#if DEBUG
using kifuwarabe_shogithink.pure.ky.bb;
using kifuwarabe_shogithink.pure.logger;
using System;
using System.Diagnostics;
#else
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.genkyoku;
using kifuwarabe_shogithink.pure.ky.bb;
using System;
using System.Diagnostics;
#endif


namespace kifuwarabe_shogithink.pure.ky
{
    /// <summary>
    /// 利き盤
    /// </summary>
    public class KikiBan
    {
        public class YomiKikiBan
        {
            public YomiKikiBan(KikiBan hontai)
            {
                hontai_ = hontai;
            }
            KikiBan hontai_;

            #region 状態（対局者別）
            public bool ExistsKikiZenbu(Taikyokusya tai, Masu ms_target)
            {
                return hontai_.BB_kikiZenbu.ToIsOn(tai, ms_target);
            }
            public bool IsIntersect_KikiZenbu(Taikyokusya tai, Bitboard bb_target)
            {
                return hontai_.BB_kikiZenbu.ToIsIntersect(tai, bb_target);
            }
            public bool IsIntersect_KikiZenbu(Taikyokusya tai, Masu ms_target)
            {
                return hontai_.BB_kikiZenbu.ToIsIntersect(tai, ms_target);
            }
            public int CountKikisuTotalZenbu(Taikyokusya tai)
            {
                return hontai_.CB_kikisuZenbu.GetTotal(tai);
            }
            public int CountKikisuZenbu(Taikyokusya tai, Masu ms)
            {
                return hontai_.CB_kikisuZenbu.Get(tai, ms);
            }
            public Bitboard CloneBBKikiZenbu(Taikyokusya tai)
            {
                return hontai_.BB_kikiZenbu.CloneKikiZenbu(tai);
            }
            #endregion
            #region 影響（対局者別）
            public void ToSitdown_BBKikiZenbu(Taikyokusya tai, Bitboard bb_update)
            {
                hontai_.BB_kikiZenbu.ToSitdown(tai, bb_update);
            }
            public void ToSelect_BBKikiZenbu(Taikyokusya tai, Bitboard bb_update)
            {
                hontai_.BB_kikiZenbu.ToSelect(tai, bb_update);
            }
            #endregion
            #region 状態（駒別）
            public bool IsActiveBBKiki()
            {
                return hontai_.BB_kikiKomabetu.IsActive();
            }
            public bool ExistsBBKiki(Koma km, Masu ms)
            {
                return hontai_.BB_kikiKomabetu.IsOn(km, ms);
            }
            public bool EqualsKiki(Koma km, Shogiban sg_target)
            {
                return sg_target.kikiBan.BB_kikiKomabetu.Equals(km, hontai_.BB_kikiKomabetu);//.RefBB_Kiki(km)
            }
            public int CountKikisuKomabetu(Koma km, Masu ms)
            {
                return hontai_.CB_kikisuKomabetu.Get(km, ms);
            }
            public Bitboard CloneBBKiki(Koma km)
            {
                return hontai_.BB_kikiKomabetu.Clone(km);
            }
            public YomiBitboard[] GetBB_WhereKiki(Taikyokusya tai)
            {
                return hontai_.BB_kikiKomabetu.GetBB_Where(tai);
            }
            #endregion
            //#region 編集（駒別）
            //public void ToStandup_Kiki(Koma km, Masu ms, Bitboard bb_update)
            //{
            //    //ms, 
            //    hontai_.BB_kikiKomabetu.ToStandup(km, bb_update);
            //}
            //#endregion
        }

        #region 生成
        public KikiBan()
        {
            yomiKikiBan = new YomiKikiBan(this);
            BB_kikiKomabetu = new KikiKomabetuBitboardItiran();
            BB_kikiZenbu = new KikiZenbuBitboardItiran();
            CB_kikisuKomabetu = new KikisuKomabetuCountboardItiran();
            CB_kikisuZenbu = new KikisuZenbuCountboardItiran();

            bbVar_kiki_forOku = new Bitboard();
            bbVar_forTorinozokuMethod = new Bitboard();
        }
        public KikiBan(KikiBan src)
        {
            yomiKikiBan = new YomiKikiBan(this);
            BB_kikiKomabetu = new KikiKomabetuBitboardItiran(src.BB_kikiKomabetu);
            BB_kikiZenbu = new KikiZenbuBitboardItiran(src.BB_kikiZenbu);
            CB_kikisuKomabetu = new KikisuKomabetuCountboardItiran(src.CB_kikisuKomabetu);
            CB_kikisuZenbu = new KikisuZenbuCountboardItiran(src.CB_kikisuZenbu);

            bbVar_kiki_forOku = new Bitboard(src.bbVar_kiki_forOku);
            bbVar_forTorinozokuMethod = new Bitboard(src.bbVar_forTorinozokuMethod);
        }
        #endregion
        public YomiKikiBan yomiKikiBan;

        #region 作り直し
        public void Tukurinaosi_Remake()
        {
            BB_kikiKomabetu.Tukurinaosi_Remake();
            BB_kikiZenbu.Tukurinaosi_Remake(BB_kikiKomabetu);
            CB_kikisuKomabetu.Tukurinaosi_Remake();
            CB_kikisuZenbu.Tukurinaosi();
        }
        public void Tukurinaosi_Copy(KikiBan source)
        {
            BB_kikiKomabetu.Tukurinaosi_Copy(source.BB_kikiKomabetu);
            BB_kikiZenbu.Tukurinaosi_Copy(source.BB_kikiZenbu);
            CB_kikisuKomabetu.Tukurinaosi_Copy(source.CB_kikisuKomabetu);
            CB_kikisuZenbu.Tukurinaosi_Copy(source.CB_kikisuZenbu);
        }
        public void Tukurinaosi_Clear()
        {
            BB_kikiKomabetu.Tukurinaosi_Clear();
            BB_kikiZenbu.Tukurinaosi_Clear();
            CB_kikisuKomabetu.Tukurinaosi_Clear();
            CB_kikisuZenbu.Tukurinaosi_Clear();
        }
        /// <summary>
        /// 盤の大きさ変更に伴う作り直し☆
        /// </summary>
        public void TukurinaosiBanOkisaHenko4()
        {
            Shogiban new1 = new Shogiban();
            // マス数変更に対応して入れ直しているぜ☆（＾～＾）
            new1.kikiBan.CB_kikisuZenbu.Import(CB_kikisuZenbu);
            new1.kikiBan.CB_kikisuKomabetu.Import(CB_kikisuKomabetu);

            CB_kikisuZenbu = new1.kikiBan.CB_kikisuZenbu;
            CB_kikisuKomabetu = new1.kikiBan.CB_kikisuKomabetu;
        }
        #endregion

        /// <summary>
        /// [手番]
        /// ビットボード。利きがある升☆
        /// </summary>
        KikiZenbuBitboardItiran BB_kikiZenbu { get; set; }
        /// <summary>
        /// [駒]
        /// ビットボード。駒の利き（同じ駒は１つに合成）がある升☆
        /// </summary>
        KikiKomabetuBitboardItiran BB_kikiKomabetu { get; set; }
        /// <summary>
        /// [手番,升]
        /// カウントボード。利きが重なっている数☆
        /// </summary>
        KikisuZenbuCountboardItiran CB_kikisuZenbu { get; set; }
        /// <summary>
        /// [駒,升]
        /// カウントボード。利きが重なっている数☆
        /// </summary>
        KikisuKomabetuCountboardItiran CB_kikisuKomabetu { get; set; }

        /// <summary>
        /// 駒全部の利きビットボード一覧
        /// （駒別を集計しただけ）
        /// </summary>
        class KikiZenbuBitboardItiran
        {
            #region 生成
            public KikiZenbuBitboardItiran()
            {
                Tukurinaosi_Clear();
            }
            public KikiZenbuBitboardItiran(KikiZenbuBitboardItiran src)
            {
                Tukurinaosi_Copy(src);
            }
            #endregion
            #region プロパティ―
            /// <summary>
            /// [手番]
            /// ビットボード。利きがある升☆ 駒の利きを全部合成
            /// </summary>
            public Bitboard[] valueTai { get; set; }
            #endregion
            #region 作り直し
            public void Tukurinaosi_Copy(KikiZenbuBitboardItiran src)
            {
                valueTai = new Bitboard[src.valueTai.Length];
                Array.Copy(src.valueTai, valueTai, src.valueTai.Length);
            }
            public void Tukurinaosi_Clear()
            {
                valueTai = new Bitboard[Conv_Taikyokusya.itiran.Length];
                for (int iTai = 0; iTai < Conv_Taikyokusya.itiran.Length; iTai++)
                {
                    valueTai[iTai] = new Bitboard();
                }
            }
            #endregion
            /// <summary>
            /// 駒別の利き を先に作っておいて、それをまとめるだけだぜ☆（＾～＾）
            /// </summary>
            /// <param name="bb_sourceKomabetuKiki"></param>
            public void Tukurinaosi_Remake(KikiKomabetuBitboardItiran bb_sourceKomabetuKiki)
            {
                Util_Bitboard.ClearBitboards(valueTai);

                foreach (Koma km_all in Conv_Koma.itiran)
                {
                    Taikyokusya tai = Med_Koma.KomaToTaikyokusya(km_all);
                    //Komasyurui ks = Med_Koma.KomaToKomasyurui(km);

                    bb_sourceKomabetuKiki.ToStandup(km_all, valueTai[(int)tai]);
                    //valueTai[(int)tai].Standup(bb_sourceKomabetuKiki.RefBB_Kiki(km));
                }
            }


            #region 状態
            public Bitboard CloneKikiZenbu(Taikyokusya tai)
            {
                return valueTai[(int)tai].Clone();
            }
            public bool ToIsIntersect(Taikyokusya tai, Bitboard bb_target)
            {
                return valueTai[(int)tai].IsIntersect(bb_target);
            }
            public bool ToIsIntersect(Taikyokusya tai, Masu ms_target)
            {
                return valueTai[(int)tai].IsIntersect(ms_target);
            }
            public bool ToIsOn(Taikyokusya tai, Masu ms_target)
            {
                return valueTai[(int)tai].IsOn(ms_target);
            }
            #endregion
            #region 編集
            public void Sitdown(Taikyokusya tai, Bitboard bb_target)
            {
                valueTai[(int)tai].Sitdown(bb_target);
            }
            public void Sitdown(Taikyokusya tai, Masu ms_target)
            {
                valueTai[(int)tai].Sitdown(ms_target);
            }
            public void Standup(Taikyokusya tai, Bitboard bb_target)
            {
                valueTai[(int)tai].Standup(bb_target);
            }
            public void Standup(Taikyokusya tai, Masu ms_target)
            {
                valueTai[(int)tai].Standup(ms_target);
            }
            #endregion
            #region 影響
            public void ToSelect(Taikyokusya tai, Bitboard bb_update)
            {
                bb_update.Siborikomi(valueTai[(int)tai]);
            }
            public void ToSitdown(Taikyokusya tai, Bitboard bb_update)
            {
                bb_update.Sitdown(valueTai[(int)tai]);
            }
            #endregion
        }
        /// <summary>
        /// 駒別利きビットボード一覧
        /// </summary>
        class KikiKomabetuBitboardItiran
        {
            #region 生成
            public KikiKomabetuBitboardItiran()
            {
                Tukurinaosi_Clear();
            }
            public KikiKomabetuBitboardItiran(KikiKomabetuBitboardItiran src)
            {
                Tukurinaosi_Copy(src);
            }
            #endregion
            #region プロパティ―
            /// <summary>
            /// [駒]
            /// ビットボード。駒の利き（同じ駒は１つに合成）がある升☆
            /// </summary>
            Bitboard[] valuesKm { get; set; }
            #endregion
            #region 作り直し
            public void Tukurinaosi_Copy(KikiKomabetuBitboardItiran src)
            {
                valuesKm = new Bitboard[src.valuesKm.Length];
                Array.Copy(src.valuesKm, valuesKm, src.valuesKm.Length);
            }
            public void Tukurinaosi_Clear()
            {
                valuesKm = new Bitboard[Conv_Koma.itiran.Length];
            }
            public void Tukurinaosi_Remake()
            {
                if (null == valuesKm)
                {
                    valuesKm = new Bitboard[Conv_Koma.itiran.Length];
                }
                Util_Bitboard.ClearBitboards(valuesKm);

                Bitboard bb_ibasho = new Bitboard();
                foreach (Koma km_all in Conv_Koma.itiran)
                {
                    PureMemory.gky_ky.yomiKy.yomiShogiban.yomiIbashoBan.ToSet_Koma(km_all, bb_ibasho);
                    Masu ms_ibasho;
                    while (bb_ibasho.Ref_PopNTZ(out ms_ibasho))
                    {
                        BitboardsOmatome.KomanoUgokikataYk00.ToStandup_Merge( km_all, ms_ibasho, valuesKm[(int)km_all]);
                    }
                }
            }
            #endregion

            #region 状態
            public Bitboard Clone(Koma km_key)
            {
                return valuesKm[(int)km_key].Clone();
            }
            //public bool Equals(Koma km_key, Bitboard bb_target)
            //{
            //    return valuesKm[(int)km_key].Equals(bb_target);
            //}
            public bool Equals(Koma km_key, KikiKomabetuBitboardItiran sameObject_target)
            {
                return valuesKm[(int)km_key].Equals(sameObject_target.valuesKm[(int)km_key]);
            }
            public bool IsActive()
            {
                foreach (Bitboard bb in valuesKm)
                {
                    if (null == bb) { return false; }
                }
                return true;
            }
            public bool IsOn(Koma km_key, Masu ms_target)
            {
                return valuesKm[(int)km_key].IsOn(ms_target);
            }
            #endregion
            #region 編集
            public void Standup(Koma km, Bitboard bb_target) { valuesKm[(int)km].Standup(bb_target); }
            public void Standup(Koma km, Masu ms_target) { valuesKm[(int)km].Standup(ms_target); }
            public void Sitdown(Koma km, Bitboard bb_target) { valuesKm[(int)km].Sitdown(bb_target); }
            public void Sitdown(Koma km, Masu ms_target) { valuesKm[(int)km].Sitdown(ms_target); }
            #endregion
            #region 影響
            public void ToStandup(Koma km, Bitboard bb_update)
            {
                Debug.Assert(null != valuesKm[(int)km], string.Format("kiki is null. km={0}", km));
                bb_update.Standup(valuesKm[(int)km]);
            }
            #endregion

            public Bitboard[] RefBB_Where(Taikyokusya tai)
            {
                Bitboard[] bbItiran = new Bitboard[Conv_Komasyurui.itiran.Length];
                foreach (Komasyurui ks in Conv_Komasyurui.itiran)
                {
                    bbItiran[(int)ks] = valuesKm[(int)Med_Koma.KomasyuruiAndTaikyokusyaToKoma(ks, tai)];
                }
                return bbItiran;
            }
            public YomiBitboard[] GetBB_Where(Taikyokusya tai)
            {
                YomiBitboard[] bbItiran = new YomiBitboard[Conv_Komasyurui.itiran.Length];
                foreach (Komasyurui ks in Conv_Komasyurui.itiran)
                {
                    bbItiran[(int)ks] = new YomiBitboard(valuesKm[(int)Med_Koma.KomasyuruiAndTaikyokusyaToKoma(ks, tai)]);
                }
                return bbItiran;
            }
        }
        /// <summary>
        /// 駒全部の利きカウントボード一覧
        /// </summary>
        public class KikisuZenbuCountboardItiran
        {
            #region 生成
            public KikisuZenbuCountboardItiran()
            {
                Tukurinaosi_Clear();
            }
            public KikisuZenbuCountboardItiran(KikisuZenbuCountboardItiran src)
            {
                Tukurinaosi_Copy(src);
            }
            #endregion
            #region プロパティ―
            /// <summary>
            /// [手番],[升]
            /// カウントボード。利きが重なっている数☆升別
            /// </summary>
            int[][] valueTaiMs { get; set; }
            /// <summary>
            /// [手番]
            /// カウントボード。利きが重なっている数☆盤上トータル☆
            /// </summary>
            int[] valueTai { get; set; }
            #endregion
            #region 作り直し
            public void Tukurinaosi_Copy(KikisuZenbuCountboardItiran source)
            {
                valueTaiMs = new int[source.valueTaiMs.Length][];
                for (int i=0; i< source.valueTaiMs.Length; i++)
                {
                    valueTaiMs[i] = new int[source.valueTaiMs[i].Length];
                    Array.Copy(source.valueTaiMs[i], valueTaiMs[i], source.valueTaiMs[i].Length);
                }

                valueTai = new int[source.valueTai.Length];
                Array.Copy(source.valueTai, valueTai, source.valueTai.Length);
            }
            public void Tukurinaosi_Clear()
            {
                int heimen = PureSettei.banHeimen;

                if (null == valueTaiMs)
                {
                    // 新規生成
                    valueTaiMs = new int[Conv_Taikyokusya.itiran.Length][];
                    valueTai = new int[Conv_Taikyokusya.itiran.Length];
                }

                for (int iTai = 0; iTai < Conv_Taikyokusya.itiran.Length; iTai++)
                {
                    if (
                        // 新規生成
                        null == valueTaiMs[iTai]
                        ||
                        // 盤サイズ変更
                        valueTaiMs[iTai].Length != heimen)
                    {                        
                        valueTaiMs[iTai] = new int[heimen];
                    }
                    else
                    {
                        Array.Clear(valueTaiMs[iTai], 0, valueTaiMs[iTai].Length);
                    }
                    valueTai[iTai] = 0;
                }
            }
            /// <summary>
            /// マス数変更に対応
            /// </summary>
            /// <param name="src"></param>
            public void Import(KikisuZenbuCountboardItiran src)
            {
                for (int iTai = 0; iTai < Conv_Taikyokusya.itiran.Length; iTai++)
                {
                    int length = Math.Min(valueTaiMs[iTai].Length, src.GetMasubetuArrayLength((Taikyokusya)iTai));

                    for (int iMs = 0; iMs < length; iMs++)
                    {
                        valueTaiMs[iTai][iMs] = src.Get((Taikyokusya)iTai, (Masu)iMs);
                    }

                    //
                    valueTai[iTai] = src.valueTai[iTai];
                }
            }
            public void Tukurinaosi()
            {
                //// 新規生成
                //valueTaiMs = new int[Conv_Taikyokusya.itiran.Length][];

                Bitboard bbVar_ibasho = new Bitboard();
                Bitboard bbVar_ugokikata = new Bitboard();

                foreach (Taikyokusya tai in Conv_Taikyokusya.itiran)
                {
                    // とりあえず一旦クリアー☆（＾～＾）
                    valueTaiMs[(int)tai] = new int[PureSettei.banHeimen];
                    valueTai[(int)tai] = 0;

                    // 全ての駒種類について☆（＾～＾）
                    foreach (Komasyurui ks in Conv_Komasyurui.itiran)
                    {
                        Koma km = Med_Koma.KomasyuruiAndTaikyokusyaToKoma(ks, tai);
                        PureMemory.gky_ky.yomiKy.yomiShogiban.yomiIbashoBan.ToSet_Koma(km,
                            bbVar_ibasho // 駒の場所をここに覚えるぜ☆（＾～＾）
                            );
#if DEBUG
                        Bitboard bb_copy = bbVar_ibasho.Clone();
#endif

                        Masu ms_ibasho;
                        while (bbVar_ibasho.Ref_PopNTZ(out ms_ibasho))
                        {
                            BitboardsOmatome.KomanoUgokikataYk00.ToSet_Merge(
                                km,
                                ms_ibasho,
                                bbVar_ugokikata // 駒の利きをここに覚えるぜ☆（＾～＾）
                                );
#if DEBUG
                            Bitboard bb_copy2 = bbVar_ugokikata.Clone();
#endif
                            // 利きを１升ずつカウントアップしていくぜ☆（＾～＾）
                            Masu ms_kiki;
                            while (bbVar_ugokikata.Ref_PopNTZ(out ms_kiki))
                            {
                                valueTaiMs[(int)tai][(int)ms_kiki]++;
                                valueTai[(int)tai]++;
                            }
                        }
                    }
                }
            }
            #endregion
            #region 状態
            public int Get(Taikyokusya tai, Masu ms)
            {
                return valueTaiMs[(int)tai][(int)ms];
            }
            public int GetTotal(Taikyokusya tai)
            {
                return valueTai[(int)tai];
            }
            public int GetMasubetuArrayLength(Taikyokusya tai)
            {
                return valueTaiMs[(int)tai].Length;
            }
            /// <summary>
            /// [手番,升] 型のカウントボードを、ビットボードに変換するぜ☆（＾▽＾）
            /// </summary>
            /// <param name="tai"></param>
            /// <param name="kikiZenbuCB"></param>
            /// <returns></returns>
            public Bitboard CreateBitboard_PositiveNumber(Taikyokusya tai)
            {
                Bitboard bb = new Bitboard();

                for (int iMs = 0; iMs < PureSettei.banHeimen; iMs++)
                {
                    if (0 < Get(tai, (Masu)iMs))
                    {
                        bb.Standup((Masu)iMs);
                    }
                }

                return bb;
            }
            #endregion
            #region 編集
            public void Increase1(Taikyokusya tai, Masu ms, out int out_result)
            {
                // 1増やしてから、その結果（升別）を返します
                out_result = ++valueTaiMs[(int)tai][(int)ms];
                valueTai[(int)tai]++;
            }
            public void Decrease1(Taikyokusya tai, Masu ms, out int out_result)
            {
                // 1減らしてから、その結果（升別）を返します
                out_result = --valueTaiMs[(int)tai][(int)ms];
                valueTai[(int)tai]--;
            }
            /// <summary>
            /// 引き算
            /// </summary>
            /// <param name="km"></param>
            /// <param name="cbKomabetu_clear">こっちはクリアーされる</param>
            public void Substruct(Koma km, KikisuKomabetuCountboardItiran cbKomabetu_clear)
            {
                Taikyokusya tai = Med_Koma.KomaToTaikyokusya(km);

                for (int iMs = 0; iMs < valueTaiMs[(int)tai].Length; iMs++)
                {
                    int num = cbKomabetu_clear.Get(km, (Masu)iMs);
                    valueTaiMs[(int)tai][iMs] -= num;
                    valueTai[(int)tai] -= num;
                }

                cbKomabetu_clear.Tukurinaosi_Clear(km);
            }
            #endregion
        }
        /// <summary>
        /// 駒別の利きカウントボード一覧
        /// </summary>
        public class KikisuKomabetuCountboardItiran
        {
            #region 生成
            public KikisuKomabetuCountboardItiran()
            {
                Tukurinaosi_Clear();
            }
            public KikisuKomabetuCountboardItiran(KikisuKomabetuCountboardItiran src)
            {
                Tukurinaosi_Copy(src);
            }
            #endregion
            #region プロパティ―
            /// <summary>
            /// [駒,升]
            /// カウントボード。利きが重なっている数☆
            /// </summary>
            int[][] valueKmMs { get; set; }
            #endregion
            #region 作り直し
            public void Tukurinaosi_Copy(KikisuKomabetuCountboardItiran source)
            {
                valueKmMs = new int[source.valueKmMs.Length][];
                for (int i=0; i<valueKmMs.Length; i++)
                {
                    valueKmMs[i] = new int[source.valueKmMs[i].Length];
                    Array.Copy(source.valueKmMs[i], valueKmMs[i], source.valueKmMs[i].Length);
                }
            }
            public void Tukurinaosi_Clear()
            {
                int heimen = PureSettei.banHeimen;

                if (null == valueKmMs)
                {
                    valueKmMs = new int[Conv_Koma.itiran.Length][];
                }

                for (int iKm = 0; iKm < Conv_Koma.itiran.Length; iKm++)
                {
                    if (null == valueKmMs[iKm] || valueKmMs[iKm].Length != heimen)
                    {
                        valueKmMs[iKm] = new int[heimen]; // 升の数が分からない
                    }
                    else
                    {
                        Array.Clear(valueKmMs[iKm], 0, valueKmMs[iKm].Length);

                    }
                }
            }
            public void Tukurinaosi_Clear(Koma km)
            {
                Array.Clear(valueKmMs[(int)km], 0, valueKmMs[(int)km].Length);
            }
            #endregion

            public void Import(KikisuKomabetuCountboardItiran src)
            {
                for (int iKm = 0; iKm < Conv_Koma.itiran.Length; iKm++)
                {
                    int length = Math.Min(valueKmMs[iKm].Length, src.GetArrayLength((Koma)iKm));

                    for (int iMs = 0; iMs < length; iMs++)
                    {
                        this.valueKmMs[iKm][iMs] = src.Get((Koma)iKm, (Masu)iMs);
                    }
                }
            }
            public void Tukurinaosi_Remake()
            {
                valueKmMs = new int[Conv_Koma.itiran.Length][];

                Bitboard bb_ibashoCopy = new Bitboard();
                Bitboard bb_ugokikataCopy = new Bitboard();
                // 盤上
                foreach (Koma km_all in Conv_Koma.itiran)
                {
                    Taikyokusya tai = Med_Koma.KomaToTaikyokusya(km_all);
                    Komasyurui ks = Med_Koma.KomaToKomasyurui(km_all);
                    valueKmMs[(int)km_all] = new int[PureSettei.banHeimen];

                    PureMemory.gky_ky.yomiKy.yomiShogiban.yomiIbashoBan.ToSet_Koma(km_all, bb_ibashoCopy);
                    Masu ms_ibasho;
                    while (bb_ibashoCopy.Ref_PopNTZ(out ms_ibasho))
                    {
                        BitboardsOmatome.KomanoUgokikataYk00.ToSet_Merge(
                            km_all,
                            ms_ibasho,
                            bb_ugokikataCopy);

                        Masu ms_kiki;
                        while (bb_ugokikataCopy.Ref_PopNTZ(out ms_kiki))
                        {
                            valueKmMs[(int)km_all][(int)ms_kiki]++;
                        }
                    }
                }
            }
            #region 状態
            public int Get(Koma km, Masu ms)
            {
                return valueKmMs[(int)km][(int)ms];
            }
            public int GetArrayLength(Koma km)
            {
                return valueKmMs[(int)km].Length;
            }
            #endregion
            #region 編集
            public void Increase1(Koma km, Masu ms, out int out_result)
            {
                // 1増やしてから、その結果を返します
                out_result = ++valueKmMs[(int)km][(int)ms];
            }
            public void Decrease1(Koma km, Masu ms, out int out_result)
            {
                // 1減らしてから、その結果を返します
                out_result = --valueKmMs[(int)km][(int)ms];
            }
            #endregion
        }




        #region 編集（駒別）
        public void Standup_Kiki(Koma km, Bitboard bb_target)
        {
            BB_kikiKomabetu.Standup(km, bb_target);
        }
        public void Standup_Kiki(Koma km, Masu ms_target)
        {
            BB_kikiKomabetu.Standup(km, ms_target);
        }
        public void Sitdown_Kiki(Koma km, Bitboard bb_target)
        {
            BB_kikiKomabetu.Sitdown(km, bb_target);
        }
        public void Sitdown_Kiki(Koma km, Masu ms_target)
        {
            BB_kikiKomabetu.Sitdown(km, ms_target);
        }
        ///// <summary>
        ///// FIXME:暫定
        ///// </summary>
        ///// <param name="tai"></param>
        ///// <returns></returns>
        //public Bitboard[] RefBB_WhereKiki(Taikyokusya tai)
        //{
        //    return BB_kikiKomabetu.RefBB_Where(tai);
        //}
        #endregion
        #region 編集（対局者別）
        public void Standup_KikiZenbu(Taikyokusya tai, Bitboard bb_target)
        {
            BB_kikiZenbu.Standup(tai, bb_target);
        }
        public void Standup_KikiZenbu(Taikyokusya tai, Masu ms_target)
        {
            BB_kikiZenbu.Standup(tai, ms_target);
        }
        public void Sitdown_KikiZenbu(Taikyokusya tai, Bitboard bb_target)
        {
            BB_kikiZenbu.Sitdown(tai, bb_target);
        }
        public void Sitdown_KikiZenbu(Taikyokusya tai, Masu ms_target)
        {
            BB_kikiZenbu.Sitdown(tai, ms_target);
        }
        #endregion

        #region 対局者別
        public void SubstructFromKikisuZenbu(Koma km)
        {
            CB_kikisuZenbu.Substruct(km,
                CB_kikisuKomabetu // こっちはクリアーされる
                );
        }
        public Bitboard RefBB_FromKikisuZenbuPositiveNumber(Taikyokusya tai)
        {
            return CB_kikisuZenbu.CreateBitboard_PositiveNumber(tai);
        }
        #endregion




        /// <summary>
        /// 利きを増やすぜ☆（＾～＾）
        /// </summary>
        Bitboard bbVar_kiki_forOku;
        public void OkuKiki(Koma km_t1,Masu ms_t1)
        {
            // 利き
            BitboardsOmatome.KomanoUgokikataYk00.ToSet_Merge(
                km_t1,//駒
                ms_t1,//置いた升
                bbVar_kiki_forOku//このビットボードをセットするぜ☆（＾～＾）
                );

            // 駒が増えたことにより、カバーが発生することがあるぜ☆（＾▽＾）

            // 盤上へ、利き　を重ね合わせるぜ☆（＾～＾）
            OkuKiki(//これをラッピングしたメソッドだぜ☆（＾～＾）
                km_t1,
                bbVar_kiki_forOku // このメソッド実行後に空っぽになるぜ☆（＾～＾）
                );
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="km_target"></param>
        /// <param name="bbVar_add">注意、このメソッド実行後に0になるぜ☆（＾～＾）</param>
        /// <param name="yomiKy"></param>
        /// <param name="reigai1"></param>
        /// <returns></returns>
        public void OkuKiki(Koma km_target, Bitboard bbVar_add)
        {
            Taikyokusya tai = Med_Koma.KomaToTaikyokusya(km_target);

            // ビットボードは、一気に更新するぜ☆（＾～＾）
            Standup_Kiki(km_target, bbVar_add);
            Standup_KikiZenbu(tai, bbVar_add);

            // カウントボードは、１升ずつ、足していくぜ☆（＾～＾）
            Masu ms_hit;
            while (bbVar_add.Ref_PopNTZ(out ms_hit))
            {
                int result_zenbu;
                CB_kikisuZenbu.Increase1(tai, ms_hit, out result_zenbu);

                int result_komabetu;
                CB_kikisuKomabetu.Increase1(km_target, ms_hit, out result_komabetu);
            }
        }





        Bitboard bbVar_forTorinozokuMethod;
        /// <summary>
        /// ビットボードを未取得のとき
        /// </summary>
        /// <param name="km_removed"></param>
        /// <param name="ms_ibasho"></param>
        /// <param name="yomiKy"></param>
        /// <param name="reigai1"></param>
        /// <returns></returns>
        public void TorinozokuKiki(Koma km_removed, Masu ms_ibasho)
        {
            BitboardsOmatome.KomanoUgokikataYk00.ToSet_Merge(
                km_removed, ms_ibasho, bbVar_forTorinozokuMethod);

            if (!bbVar_forTorinozokuMethod.IsEmpty())
            {
                //ビットボードを未取得のとき
                TorinozokuKiki(km_removed, bbVar_forTorinozokuMethod);
            }

            // TODO:  駒を除外したので、ディスカバード・アタックが発生することがあるぜ☆（＾▽＾）
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="km_removed"></param>
        /// <param name="bbVer_remove">注意、このメソッド実行後に0になるぜ☆（＾～＾）</param>
        /// <param name="yomiKy"></param>
        /// <param name="dbg_reigai"></param>
        /// <param name="hint"></param>
        /// <param name="changing"></param>
        /// <returns></returns>
        public void TorinozokuKiki(Koma km_removed,Bitboard bbVer_remove)
        {
            Taikyokusya teban = Med_Koma.KomaToTaikyokusya(km_removed);

            // １マスずつ、利きを減らしていくぜ☆（＾～＾）
            Masu ms_cur;
            while (bbVer_remove.Ref_PopNTZ(out ms_cur))
            {
                //────────────────────
                // まず　駒別を　減らす
                //────────────────────

                // （１）カウントボードの数字を減らす
                int result_komabetu;
                CB_kikisuKomabetu.Decrease1(km_removed, ms_cur, out result_komabetu);

                // （２）「カウントが無くなったら」ビットをＯＦＦにするんだぜ☆（＾～＾）まるごとＯＦＦにしてはいけないぜ☆（＾～＾）
                if (result_komabetu < 1)
                {
                    Sitdown_Kiki(km_removed, ms_cur);
                }

                //────────────────────
                // 次に　対局者別を　減らす
                //────────────────────

                // （１）カウントボードの数字を減らす
                int result_zenbu;
                CB_kikisuZenbu.Decrease1(teban, ms_cur, out result_zenbu);

                // （２）「カウントが無くなったら」ビットをＯＦＦにするんだぜ☆（＾～＾）まるごとＯＦＦにしてはいけないぜ☆（＾～＾）
                if (result_zenbu < 1)
                {
                    Sitdown_KikiZenbu(teban, ms_cur);
                }
            }

            // 現局面より、利きの数が減っているのが正解
        }
    }
}
