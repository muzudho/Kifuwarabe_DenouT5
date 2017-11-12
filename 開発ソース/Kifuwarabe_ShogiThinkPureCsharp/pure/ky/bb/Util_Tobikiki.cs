#if DEBUG
using kifuwarabe_shogithink.pure.control;
using kifuwarabe_shogithink.pure.ky.tobikiki;
using System;
using System.Diagnostics;
#else
using kifuwarabe_shogithink.pure.ky.tobikiki;
using System;
#endif

namespace kifuwarabe_shogithink.pure.ky.bb
{
    /// <summary>
    /// 長い利きに関するもの
    /// 
    /// ナナメ段とか
    /// </summary>
    public static class Util_Tobikiki
    {
        #region プロパティ―
        /// <summary>
        /// [升,ブロック配置ハッシュキー]
        /// 
        /// 本将棋なら、８段目～２段目の７マスのブロック配置を調べ、
        /// ＯＯＯＯＯＯＯ　をインデックスとする
        /// </summary>
        public static int KIKIDIRLEN_TOBIKIKI = 2;
        /// <summary>
        /// 隣利きにはこれ。
        /// ojhsh:お邪魔ハッシュキー
        /// </summary>
        public static int OJHSH_TONARIKIKI = 0;
        public static int KIKIDIR_TONARIKIKI = 0;
        public static int KIKIDIRLEN_TONARIKIKI = 1;

        /// <summary>
        /// きりんの横利きのＢｌｏｃｋ駒の配置が何通りの数あるか
        /// </summary>
        public static int yokoToriSu;
        /// <summary>
        /// きりんの縦利きのＢｌｏｃｋ駒の配置が何通りの数あるか
        /// </summary>
        public static int tateToriSu;
        /// <summary>
        /// ぞうのナナメ利きのＢｌｏｃｋ駒の配置が何通りの数あるか
        /// ナナメは短い方の辺でもある。
        /// </summary>
        public static int nanameToriSu;
        /// <summary>
        /// 長い方の辺
        /// </summary>
        public static int nagaiHenToriSu;


        /// <summary>
        /// ナナメ段の番号早引き表
        /// [升][長い利きの方向]
        /// </summary>
        public static int[][] nanamedanD { get; set; }
        /// <summary>
        /// ナナメ段の行頭ｉｎｄｅｘ早引き表
        /// [ナナメ段番号][長い利きの方向]
        /// </summary>
        public static Masu[][] nanamedanAtama_chikanHyoDst { get; set; }
        public static Masu[][] nanamedanAtama_chikanHyoMotoMs { get; set; }
        /// <summary>
        /// まっすぐ段の行頭ｉｎｄｅｘ早引き表
        /// 縦の並びを　横の並びにするために、反時計回りに９０°回転させるもの。
        /// afterには移動先のマス番号。
        /// beforeは、本来の盤上のタテ筋の一番上のマス。
        /// [ナナメ段番号]
        /// </summary>
        public static Masu[] massugudanAtama_chikanHyoDst{ get; set; }
        public static Masu[] massugudanAtama_chikanHyoMotoMs { get; set; }
        /// <summary>
        /// まっすぐ段の番号早引き表
        /// [升]
        /// </summary>
        public static int[] massugudanD { get; set; }
        /// <summary>
        /// ナナメ段の行幅早引き表
        /// [ナナメ段番号][長い利きの方向]
        /// </summary>
        public static int[][] nanamedanHaba { get; set; }

        public static int MasSpanKT { get { return PureSettei.banYokoHaba; } }// yoko
        public static int MasSpanKY { get { return 1; } }// 1
        public static int MasSpanZHa { get { return PureSettei.banYokoHaba + 1; } }// yoko + 1
        public static int MasSpanZHs { get { return PureSettei.banYokoHaba - 1; } }// ×-yoko+1
        #endregion

        #region 作り直し
        /// <summary>
        /// 入れ物は先行して作り直せだぜ☆（＾～＾）
        /// </summary>
        public static void Tukurinaosi_1_IremonoSize()
        {
            int tate = PureSettei.banTateHaba;
            int yoko = PureSettei.banYokoHaba;
            int heimen = PureSettei.banHeimen;
            int nanamedanDLen = PureSettei.banNanameDanLen;

            // 入れ物のサイズを確保する前に☆（＾～＾）
            //────────────────────
            // ヨコ、タテ、ナナメが何通りの数か
            //────────────────────
            yokoToriSu = (int)Math.Pow(2, PureSettei.banYokoHaba);
            tateToriSu = (int)Math.Pow(2, PureSettei.banTateHaba);

            // ななめは短い辺
            nanameToriSu = (int)Math.Pow(2, Math.Min(PureSettei.banYokoHaba, PureSettei.banTateHaba));
            nagaiHenToriSu = (int)Math.Pow(2, Math.Max(PureSettei.banYokoHaba, PureSettei.banTateHaba));
        }
        public static void Tukurinaosi_2_Nakami(
#if DEBUG
                string dbg_hint
#endif
            )
        {
            int tate = PureSettei.banTateHaba;
            int yoko = PureSettei.banYokoHaba;
            int heimen = PureSettei.banHeimen;
            int nanamedanDLen = PureSettei.banNanameDanLen;

            // ナナメ段
            nanamedanD = new int[heimen][];
            for (int iMs = 0; iMs < heimen; iMs++)
            {
                nanamedanD[iMs] = new int[KIKIDIRLEN_TOBIKIKI];
            }
            nanamedanAtama_chikanHyoDst = new Masu[nanamedanDLen][];
            nanamedanAtama_chikanHyoMotoMs = new Masu[nanamedanDLen][];
            nanamedanHaba = new int[nanamedanDLen][];
            for (int iD = 0; iD < nanamedanDLen; iD++)
            {
                nanamedanAtama_chikanHyoDst[iD] = new Masu[KIKIDIRLEN_TOBIKIKI];
                nanamedanAtama_chikanHyoMotoMs[iD] = new Masu[KIKIDIRLEN_TOBIKIKI];
                nanamedanHaba[iD] = new int[KIKIDIRLEN_TOBIKIKI];
            }
            massugudanD = new int[heimen];
            massugudanAtama_chikanHyoDst = new Masu[tate];//置換表は反時計回りに９０°回転する前の表で、その横幅は、回転後の縦幅
            massugudanAtama_chikanHyoMotoMs = new Masu[tate];

            //────────────────────
            // 左上がり筋一列ビットボード
            //────────────────────
            // o---
            // -o--
            // --o-
            // ---o
            {
                int preDiagonals = -1;
                int haba = 0;
                int iKikiDirArrayIndex = Conv_TobikikiDirection.ToUgokikataArrayIndex(TobikikiDirection.ZHa);
                int dst = 0;
                BitboardsOmatome.ScanHidariAgariSuji((int diagonals, Masu ms) =>
                {
                    if (preDiagonals != diagonals)
                    {
                        preDiagonals = diagonals;
                        nanamedanAtama_chikanHyoDst[diagonals][iKikiDirArrayIndex] = (Masu)dst;
                        nanamedanAtama_chikanHyoMotoMs[diagonals][iKikiDirArrayIndex] = ms;
                        if (0 < diagonals)
                        {
                            nanamedanHaba[diagonals - 1][iKikiDirArrayIndex] = haba;
                        }
                        haba = 0;
                    }
                    nanamedanD[(int)ms][iKikiDirArrayIndex] = diagonals;
                    haba++;
                    dst++;
                });
                nanamedanHaba[nanamedanHaba.Length - 1][iKikiDirArrayIndex] = 1;//最後のマスの幅
            }

            //────────────────────
            // 左下がり筋一列ビットボード
            //────────────────────
            // ---o
            // --o-
            // -o--
            // o---
            {
                int preDiagonals = -1;
                int haba = 0;
                int iKikiDir = Conv_TobikikiDirection.ToUgokikataArrayIndex(TobikikiDirection.ZHs);
                int dst = 0;
                BitboardsOmatome.ScanHidariSagariSuji((int diagonals, Masu ms) =>
                {
                    if (preDiagonals != diagonals)
                    {
                        preDiagonals = diagonals;
                        nanamedanAtama_chikanHyoDst[diagonals][iKikiDir] = (Masu)dst;
                        nanamedanAtama_chikanHyoMotoMs[diagonals][iKikiDir] = ms;
                        if (0 < diagonals)
                        {
                            nanamedanHaba[diagonals - 1][iKikiDir] = haba;
                        }
                        haba = 0;
                    }
                    nanamedanD[(int)ms][iKikiDir] = diagonals;
                    haba++;
                    dst++;
                });
                nanamedanHaba[nanamedanHaba.Length - 1][iKikiDir] = 1;//最後のマスの幅
            }

            //────────────────────
            // 反時計回り一列ビットボード
            //────────────────────
            // ---o
            // ---o
            // ---o
            // ---o
            {
                int preDiagonals = -1;
                int iKikiDir = Conv_TobikikiDirection.ToUgokikataArrayIndex(TobikikiDirection.KT);
                BitboardsOmatome.ScanHanTokei90((int diagonals, Masu ms,  Masu dst) => {
                    if (preDiagonals != diagonals)
                    {
                        preDiagonals = diagonals;
                        massugudanAtama_chikanHyoDst[diagonals] = dst;
                        massugudanAtama_chikanHyoMotoMs[diagonals] = ms;
                    }
                    massugudanD[(int)ms] = diagonals;
                });
            }

            //────────────────────
            // 駒の動き（飛び利き）
            //────────────────────
            if (PureSettei.tobikikiTukau)
            {
                // （１）ＫＴ＿Ｕ　きりんの縦　上（先手いのしし兼用）
                // （２）ＫＴ＿Ｓ　きりんの縦　下（後手いのしし兼用）
                {
                    TobikikiDirection kikiDir = TobikikiDirection.KT;//いのししも変わらないだろ

                    BitboardsOmatome.KomanoUgokikataYk00.ScanTobikiki(
                        (Taikyokusya tai, Masu ms, int iBlocks, Bitboard kikiBB) => {
                            // 指定マスから０の方向に向かって
                            // きりん縦
                            BitboardsOmatome.KomanoUgokikataYk00.StandupElement(
                                tai, (int)kikiDir, ms, iBlocks, kikiBB);
                            // いのしし（先手）
                            if (tai == Taikyokusya.T1)
                            {
                                BitboardsOmatome.KomanoUgokikataYk00.StandupElement(
                                    tai, (int)TobikikiDirection.S, ms,iBlocks,kikiBB);
                            }
                        },
                        (Taikyokusya tai, Masu ms, int iBlocks, Bitboard kikiBB) =>
                        {
                            // 指定マスから最大の方向に向かって
                            // きりん縦
                            BitboardsOmatome.KomanoUgokikataYk00.StandupElement(
                                tai, (int)kikiDir, ms, iBlocks, kikiBB);
                            // いのしし（後手）
                            if (tai == Taikyokusya.T2)
                            {
                                BitboardsOmatome.KomanoUgokikataYk00.StandupElement(
                                    tai, (int)TobikikiDirection.S,ms,iBlocks,kikiBB);
                            }
                        },
                        kikiDir,
                        MasSpanKT,
                        false,
                        tateToriSu
                        
                    );
                }

// （３）ＫＹ＿Ｈ　きりんの横　左
// （４）ＫＹ＿Ｍ　きりんの横　右
                {
                    TobikikiDirection kikiDir = TobikikiDirection.KY;

                    BitboardsOmatome.KomanoUgokikataYk00.ScanTobikiki(
                        (Taikyokusya tai, Masu ms, int iBlocks, Bitboard bb_kiki) => {

                            // bb_kiki で 63(10) と出てきたときは、
                            // 111111(2) なのか、
                            // 63（飛車のいる段の先頭列）マスなのか、
                            // マス番号と間違えていないか　注意。

                            // 診断：平手初期局面　飛車の横利き
#if DEBUG
                            if (kikiDir == TobikikiDirection.KY
                                && PureSettei.gameRule == GameRule.HonShogi
                                && 70== (int)ms
                                &&
                                (
                                Convert.ToInt32("010000010", 2)==iBlocks // 130(10)
                                ||Convert.ToInt32("10111111", 2) == iBlocks // 191
                                )
                                
                            )
                            {
                                int a = 0;
                            }
#endif


                            // きりん横
                            BitboardsOmatome.KomanoUgokikataYk00.StandupElement(
                                tai, (int)kikiDir,ms,iBlocks,bb_kiki);

                            // 診断：平手初期局面　飛車の横利き
#if DEBUG
                            if (PureSettei.gameRule == GameRule.HonShogi
                                && 70 == (int)ms
                                &&
                                (
                                Convert.ToInt32("010000010", 2) == iBlocks // 130(10)
                                // || Convert.ToInt32("10111111", 2) == iBlocks // 191
                                )
                            )
                            {
                                Bitboard bb = BitboardsOmatome.KomanoUgokikataYk00.CloneElement(
                                    tai,
                                    kikiDir,
                                    (Masu)70,
                                    iBlocks
                                    );

                                ulong byte1 = (ulong)Convert.ToInt32("111111", 2); // 63

                                // bb.value64127 は、８段目の「２」列目からの数字が入っているぜ☆（＾～＾）
                                // 111111(2) = 63(10) という数字は ８八、７八、６八、５八、４八、３八　を指していて、
                                // 飛車の利き（角の升含む）を表しているぜ☆（＾～＾）

                                // 111111(2) = 63(10)  飛車が角の上まで。63 ではなく "011111110" 254 が入っていてほしい。
                                // 10111111(2) = 191(10) 飛車が角を貫通している。後手番が上書きしているようだ。
                                //                                               ~~~~~~
                                // が入ってる
                                // 
                                Debug.Assert(
                                    (bb.value64127 == byte1 && bb.value063 == 0)
                                    );
                            }
#endif

                        },
                        (Taikyokusya tai, Masu ms, int iBlocks, Bitboard kikiBB) =>
                        {
                            // きりん横
                            BitboardsOmatome.KomanoUgokikataYk00.StandupElement(
                                tai, (int)kikiDir,ms,iBlocks,kikiBB);
                        },
                        kikiDir, MasSpanKY, false, yokoToriSu
                    );

                    // 診断：平手初期局面　飛車の横利き
#if DEBUG
                    if (PureSettei.gameRule == GameRule.HonShogi)
                    {
                        int iBlocks = Convert.ToInt32("010000010", 2);//下位桁は９一の方 130(10)
                        Bitboard bb = BitboardsOmatome.KomanoUgokikataYk00.CloneElement(
                            Taikyokusya.T1,
                            kikiDir,
                            (Masu)70,
                            iBlocks
                            );
                        // 先手番なので、１バイト目の６４ビットと、２バイト目にデータが入っているはず☆（＾～＾）
                        ulong byte2 = (ulong)Convert.ToInt32("1111111", 2);// 63
                        string str2 = Convert.ToString((int)bb.value64127, 2);//"10111111" 191(10)
                        Debug.Assert(
                            (
                            (ulong)Convert.ToInt32("010000010", 2) == bb.value64127 // 130(10)
                            || (ulong)Convert.ToInt32("10111111", 2) == bb.value64127 // 191
                            )
                            &&
                            bb.value063 == 0
                        );
                    }
#endif
                }

                // （５）ＺＨａ＿Ｕ　ぞうの左上がり　上
                // （６）ＺＨａ＿Ｓ　ぞうの左上がり　下
                {
                    TobikikiDirection kikiDir = TobikikiDirection.ZHa;

                    BitboardsOmatome.KomanoUgokikataYk00.ScanTobikiki(
                        (Taikyokusya tai, Masu ms, int iBlocks, Bitboard kikiBB) => {
                            // ぞう横
                            BitboardsOmatome.KomanoUgokikataYk00.StandupElement(
                                tai, (int)kikiDir,ms,iBlocks,kikiBB);
                        },
                        (Taikyokusya tai, Masu ms, int iBlocks, Bitboard kikiBB) =>
                        {
                            // ぞう横
                            BitboardsOmatome.KomanoUgokikataYk00.StandupElement(
                                tai, (int)kikiDir,ms,iBlocks,kikiBB);
                        },
                        kikiDir, MasSpanZHa, false, yokoToriSu
                    );
                }
                // （７）ＺＨｓ＿Ｕ　ぞうの左下がり　上
                // （８）ＺＨｓ＿Ｓ　ぞうの左下がり　下
                {
                    TobikikiDirection kikiDir = TobikikiDirection.ZHs;

                    BitboardsOmatome.KomanoUgokikataYk00.ScanTobikiki(
                        (Taikyokusya tai, Masu ms, int iBlocks, Bitboard kikiBB) => {
                            // ぞう
                            BitboardsOmatome.KomanoUgokikataYk00.StandupElement(
                                tai, (int)kikiDir,ms,iBlocks,kikiBB);
                        },
                        (Taikyokusya tai, Masu ms, int iBlocks, Bitboard kikiBB) =>
                        {
                            // ぞう
                            BitboardsOmatome.KomanoUgokikataYk00.StandupElement(
                                tai, (int)kikiDir,ms,iBlocks,kikiBB);
                        },
                        kikiDir, MasSpanZHs, true, yokoToriSu
                    );
                }
            }
        }
        #endregion

        #region 状況
        /// <summary>
        /// マス番号から、データ取得
        /// </summary>
        /// <param name="kikiDir"></param>
        /// <param name="masuSpan"></param>
        /// <param name="sakasa_forZHs"></param>
        /// <param name="ms"></param>
        /// <param name="nanamedanD"></param>
        /// <param name="atama_reverseRotateChikanhyo"></param>
        /// <param name="atama_noRotateMotohyo"></param>
        /// <param name="siri_noRotateMotohyo"></param>
        /// <param name="haba"></param>
        public static void FromMasu(
            TobikikiDirection kikiDir, int masuSpan, bool sakasa_forZHs,
            Masu ms,
            out int nanamedanD,
            out Masu atama_reverseRotateChikanhyo,
            out Masu atama_noRotateMotohyo,
            out Masu siri_noRotateMotohyo,
            out int haba)
        {
            nanamedanD = GetNanameDan(ms, kikiDir);
            atama_reverseRotateChikanhyo = GetNanameDanAtama_ReverseRotateChikanhyo(nanamedanD, kikiDir);
            atama_noRotateMotohyo = GetNanameDanAtama_NoRotateMotohyo(nanamedanD, kikiDir);
            haba = GetNanameDanHaba(nanamedanD, kikiDir);
            if (sakasa_forZHs)
            {
                // ZHs では尻の方が小さい
                siri_noRotateMotohyo = atama_noRotateMotohyo - (haba - 1) * masuSpan;
            }
            else
            {
                siri_noRotateMotohyo = atama_noRotateMotohyo + (haba - 1) * masuSpan;
            }
        }
        /// <summary>
        /// ナナメ段を抜き出してビットボードにします。
        /// </summary>
        /// <param name="yomiSg"></param>
        /// <param name="tai"></param>
        /// <param name="kikiDir"></param>
        /// <param name="ms"></param>
        /// <returns></returns>
        public static Bitboard GetOjhsh(
            TobikikiDirection kikiDir,            
            Masu ms
            )
        {
            bool sakasa_forZHs = Conv_TobikikiDirection.sakasa_forZHs[(int)kikiDir];
            OjamaBanSyurui ojamaBanSyurui = Conv_TobikikiDirection.ojamaBanSyuruiItiran[(int)kikiDir];
            int[] masuSpans = new int[]
            {
                    MasSpanKT,
                    MasSpanKY,
                    MasSpanKT,
                    MasSpanZHa,
                    MasSpanZHs,
            };
            int masuSpan = masuSpans[(int)kikiDir];

            int nanamedanD;
            Masu atama_reverseRotateChikanhyo;
            Masu atama_noRotateMotohyo;
            Masu siri_noRotateMotohyo;
            int haba;
            FromMasu(kikiDir, masuSpan, sakasa_forZHs, ms,
                out nanamedanD,
                out atama_reverseRotateChikanhyo,
                out atama_noRotateMotohyo,
                out siri_noRotateMotohyo,
                out haba
                );
#if DEBUG
            PureMemory.ssssDbg_sakasa_forZHs = sakasa_forZHs;
            PureMemory.ssssDbg_ojamaBanSyurui = ojamaBanSyurui;
            PureMemory.ssssDbg_masuSpan = masuSpan;
            PureMemory.ssssDbg_nanamedanD = nanamedanD;
            PureMemory.ssssDbg_atama_reverseRotateChikanhyo = atama_reverseRotateChikanhyo;
            PureMemory.ssssDbg_atama_noRotateMotohyo = atama_noRotateMotohyo;
            PureMemory.ssssDbg_siri_noRotateMotohyo = siri_noRotateMotohyo;
            PureMemory.ssssDbg_haba = haba;
#endif

            if (ojamaBanSyurui == OjamaBanSyurui.None)
            {
                // 横型と想定
                // 本将棋の場合、９一が再下位ビット
                // [ 0][ 1][ 2][ 3][ 4][ 5][ 6][ 7][ 8]
                // [ 9]...
                Bitboard bb = PureMemory.gky_ky.yomiKy.yomiShogiban.yomiIbashoBan.CloneKomaZenbuBothTai();
#if DEBUG
                PureMemory.ssssDbg_bb_ojamaTai = bb.Clone();
#endif
                return OjamaBan.YomiOjamaBan.Nukidasi_1(atama_reverseRotateChikanhyo, haba, bb);
            }
            else
            {
                OjamaBan.YomiOjamaBan yomiOjamaBan = PureMemory.gky_ky.yomiKy.yomiShogiban.GetYomiOjamaBan(ojamaBanSyurui);
                return yomiOjamaBan.Nukidasi(atama_reverseRotateChikanhyo, haba);
            }
        }

        public static int GetNanameDan(Masu ms, TobikikiDirection kikiDir)
        {
            switch (kikiDir)
            {
                case TobikikiDirection.KY:
                case TobikikiDirection.None:
                    // 横方向（０オリジン）
                    return Conv_Masu.ToDanO0_WithoutErrorCheck((int)ms);
                case TobikikiDirection.KT:
                case TobikikiDirection.S:
                    // 縦方向
                    return massugudanD[(int)ms];
                case TobikikiDirection.ZHa:
                case TobikikiDirection.ZHs:
                    return nanamedanD[(int)ms][Conv_TobikikiDirection.ToUgokikataArrayIndex(kikiDir)];
                default:
                    throw new Exception(string.Format("想定外の方向 dir={0}", kikiDir));
            }
        }

        /// <summary>
        /// 回転前の盤（置換表）のナナメ段
        /// （１）４５°回転盤では、ハバが異なる段数の多い表
        /// （２）９０°回転盤では、タテ筋を横に寝かした置換表。その横幅は、元の盤の縦幅
        /// </summary>
        /// <param name="nanamedanD"></param>
        /// <param name="kikiDir"></param>
        /// <returns></returns>
        public static Masu GetNanameDanAtama_ReverseRotateChikanhyo(int nanamedanD, TobikikiDirection kikiDir)
        {
            switch (kikiDir)
            {
                case TobikikiDirection.KY:
                case TobikikiDirection.None:
                    // 横方向
                    return (Masu)(nanamedanD * PureSettei.banYokoHaba);
                case TobikikiDirection.KT:
                case TobikikiDirection.S:
                    // 上から下への縦方向を反時計回りに９０°回転させて、左から右に並べ替えた段の先頭列のマス番号
                    return massugudanAtama_chikanHyoDst[nanamedanD];
                case TobikikiDirection.ZHa:
                case TobikikiDirection.ZHs:
                    return nanamedanAtama_chikanHyoDst[nanamedanD][Conv_TobikikiDirection.ToUgokikataArrayIndex(kikiDir)];
                default:
                    throw new Exception(string.Format("想定外の方向 dir={0}", kikiDir));
            }
        }
        /// <summary>
        /// 置換表から見て、置換後の盤。つまり現状の盤
        /// </summary>
        /// <param name="nanamedanD"></param>
        /// <param name="kikiDir"></param>
        /// <returns></returns>
        public static Masu GetNanameDanAtama_NoRotateMotohyo(int nanamedanD, TobikikiDirection kikiDir)
        {
            switch (kikiDir)
            {
                case TobikikiDirection.KY:
                case TobikikiDirection.None:
                    // 横方向
                    return (Masu)(nanamedanD * PureSettei.banYokoHaba);
                case TobikikiDirection.KT:
                case TobikikiDirection.S:
                    // 上から下への縦方向を反時計回りに９０°回転させて、左から右に並べ替えた段の先頭列のマス番号
                    return massugudanAtama_chikanHyoMotoMs[nanamedanD];
                case TobikikiDirection.ZHa:
                case TobikikiDirection.ZHs:
                    return nanamedanAtama_chikanHyoMotoMs[nanamedanD][Conv_TobikikiDirection.ToUgokikataArrayIndex(kikiDir)];
                default:
                    throw new Exception(string.Format("想定外の方向 dir={0}", kikiDir));
            }
        }

        public static int GetNanameDanHaba(int nanamedanD, TobikikiDirection kikiDir)
        {
            switch (kikiDir)
            {
                case TobikikiDirection.KY:
                case TobikikiDirection.None:
                    // 横方向
                    return PureSettei.banYokoHaba;
                case TobikikiDirection.KT:
                case TobikikiDirection.S:
                    // 縦方向
                    return PureSettei.banTateHaba;
                case TobikikiDirection.ZHa:
                case TobikikiDirection.ZHs:
                    // ナナメ方向
                    return nanamedanHaba[nanamedanD][Conv_TobikikiDirection.ToUgokikataArrayIndex(kikiDir)];
                default:
                    throw new Exception(string.Format("想定外の方向 dir={0}", kikiDir));
            }
        }
        #endregion

    }

}
