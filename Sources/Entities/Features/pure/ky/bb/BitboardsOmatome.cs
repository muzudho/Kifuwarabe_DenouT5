#if DEBUG
using kifuwarabe_shogithink.pure.control;
using kifuwarabe_shogithink.pure.ky.tobikiki;
using System;
using System.Diagnostics;
#else
using kifuwarabe_shogithink.pure.control;
using kifuwarabe_shogithink.pure.ky.tobikiki;
using System;
using System.Diagnostics;
#endif

namespace kifuwarabe_shogithink.pure.ky.bb
{
    public static class BitboardsOmatome
    {
        #region 初期化
        static BitboardsOmatome()
        {
            int size = 16;//とりあえず
            maskHyo = new Bitboard[size];
            maskHyo[0] = new Bitboard(0, 0);
            for (int exp = 1; exp < size; exp++)
            {
                maskHyo[exp] = new Bitboard(0, (ulong)Math.Pow(2, exp) - 1);
            }
        }
        #endregion
        #region プロパティ―
        /// <summary>
        /// ＡＮＤ演算用のマスク
        /// </summary>
        public static Bitboard[] maskHyo;
        /// <summary>
        /// 1段目～n段目。トライルールで使うぐらい？
        /// [0]が1段目。
        /// </summary>
        public static Bitboard[] bb_danArray;
        /// <summary>
        /// 1筋目～n筋目。駒の動きの端っこチェックで使う☆
        /// [0]が左端で、1筋目。
        /// </summary>
        public static Bitboard[] bb_sujiArray;
        public static Bitboard[] bb_hidariAgariSujiArray;
        public static Bitboard[] bb_hidariSagariSujiArray;
        /// <summary>
        /// [手番]
        /// ビットボード。トライできる升☆
        /// </summary>
        public static Bitboard[] bb_try;
        /// <summary>
        /// [手番]
        /// うさぎが右に飛べる升☆
        /// </summary>
        public static Bitboard[] bb_usagigaMiginiToberu;
        /// <summary>
        /// [手番]
        /// うさぎが左に飛べる升☆
        /// </summary>
        public static Bitboard[] bb_usagigaHidariniToberu;

        /// <summary>
        /// ビットボードのうち、
        /// 将棋盤として使っているビットを立てておけだぜ☆（＾～＾）
        /// </summary>
        public static Bitboard bb_boardArea { get { return bb_boardArea_; } }
        static Bitboard bb_boardArea_;

        /// <summary>
        /// [対局者]
        /// 成れるのに関連するマス☆（＾～＾）
        /// </summary>
        public static Bitboard[] bb_nareruZone;
        ///// <summary>
        ///// [先後駒]
        ///// そこに進みたいなら、成らないといけないゾーン☆（＾～＾）
        ///// 歩、香の１段目、桂の１、２段目だぜ☆（＾～＾）
        ///// </summary>
        //public static Bitboard[] bb_nareZone;
        ///// <summary>
        ///// [先後持駒]
        ///// 歩と香車、桂馬を打てるマスのマスクに利用☆（＾～＾）
        ///// それ以外の駒は、盤面全体に進める（打てる）ぜ☆（*＾～＾*）
        ///// </summary>
        /// <summary>
        /// [先後駒]
        /// 歩と香車、桂馬を打てるマスのマスクに利用☆（＾～＾）
        /// それ以外の駒は、盤面全体に進める（打てる）ぜ☆（*＾～＾*）
        /// </summary>
        public static Bitboard[] bb_uteruZone;
        #endregion

        public static class YomiBitboardsOmatome
        {
            public static bool IsIntersect_UsagigaMiginiToberu(Taikyokusya tai, Masu ms) { return BitboardsOmatome.bb_usagigaMiginiToberu[(int)tai].IsIntersect(ms); }
            public static bool IsIntersect_UsagigaHidariniToberu(Taikyokusya tai, Masu ms) { return BitboardsOmatome.bb_usagigaHidariniToberu[(int)tai].IsIntersect(ms); }

            public static bool IsIntersect_UeHajiDan(Masu ms) { return BitboardsOmatome.bb_danArray[0].IsIntersect(ms); }
            public static bool IsIntersect_SitaHajiDan(Masu ms) { return BitboardsOmatome.bb_danArray[BitboardsOmatome.bb_danArray.Length - 1].IsIntersect(ms); }
            public static bool IsIntersect_HidariHajiSuji(Masu ms) { return BitboardsOmatome.bb_sujiArray[0].IsIntersect(ms); }
            public static bool IsIntersect_MigiHajiSuji(Masu ms) { return BitboardsOmatome.bb_sujiArray[BitboardsOmatome.bb_sujiArray.Length - 1].IsIntersect(ms); }
        }

        public delegate void DLGT_scanHidariAgariSujiMasu(int diagonals, Masu ms);
        /// <summary>
        ///────────────────────
        /// 左上がり筋一列ビットボード
        ///────────────────────
        /// o---
        /// -o--
        /// --o-
        /// ---o
        /// </summary>
        public static void ScanHidariAgariSuji(DLGT_scanHidariAgariSujiMasu dlgt_scanHidariAgariSujiMasu)
        {
            // 筋一列を立てます
            int w = PureSettei.banYokoHaba;// 横幅（width）
            int h = PureSettei.banTateHaba;// 縦幅（height）
            int d = PureSettei.banNanameDanLen;// ナナメ段高さ（diagonal）

            for (int iD = 0; iD < d; iD++)//diagonals
            {
                int rx = iD; // リバースｘ
                for (int iY = 0; iY < h; iY++, rx--)
                {
                    int x = -rx + (w - Pure.KATAGAWA1);//正x

                    if (-1 < rx && rx < w)
                    {
                        int index = iY * w + x;// 固定のインデックス
                        dlgt_scanHidariAgariSujiMasu(iD, (Masu)index);
                    }
                }
            }
        }
        public delegate void DLGT_scanHidariSagariSujiMasu(int diagonals, Masu ms);
        /// <summary>
        ///────────────────────
        /// 左下がり筋一列ビットボード
        ///────────────────────
        /// ---o
        /// --o-
        /// -o--
        /// o---
        /// </summary>
        public static void ScanHidariSagariSuji(DLGT_scanHidariSagariSujiMasu dlgt_scanHidariSagariSujiMasu)
        {
            // 筋一列を立てます
            int w = PureSettei.banYokoHaba;// 横幅（width）
            int h = PureSettei.banTateHaba;// 縦幅（height）
            int d = PureSettei.banNanameDanLen;// ナナメ段高さ（diagonal）
            for (int iD = 0; iD < d; iD++)//diagonals
            {
                int x = iD - h + 1; // 正x（縦幅-1だけマイナス方向に戻したところからスタート）
                for (int iY = h - 1; -1 < iY; iY--, x++)//最下段から最上段へ。（カーソルは右上に進んでいく）
                {
                    if (-1 < x && x < w)
                    {
                        int index = iY * w + x;// 固定のインデックス
                        dlgt_scanHidariSagariSujiMasu(iD, (Masu)index);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ms">0から昇順</param>
        /// <param name="diagonals">0から昇順</param>
        /// <param name="dst"></param>
        public delegate void DLGT_scanHanTokei90Masu(int diagonals, Masu ms,  Masu dst);
        /// <summary>
        /// 反時計回り
        /// </summary>
        public static void ScanHanTokei90(DLGT_scanHanTokei90Masu dlgt_scanHanTokei90Masu)
        {
            int rw = PureSettei.banTateHaba; // 横幅（reverse width）。縦幅をひっくり返したもの
            int rh = PureSettei.banYokoHaba; // 縦幅（reverse height）。横幅をひっくり返したもの
            Masu[] chikanHyo = new Masu[PureSettei.banHeimen];
            int[] diagonals = new int[PureSettei.banHeimen];

            // 置換表をまず作成
            {
                int dst = 0;
                for (int iD = 0; iD < rw; iD++)//diagonals 右端を０として左へ進んでいる
                {
                    int x = -iD + (rw-Pure.KATAGAWA1);//左から右に戻す

                    for (int y = 0; y < rh; y++)
                    {
                        int iMs = y * rw + x;
                        chikanHyo[iMs] = (Masu)dst;
                        diagonals[iMs] = iD;
                        dst++;
                    }
                }
            }

            // セット
            {
                // インターフェースの統一性を合わせるために、
                // ナナメ段の昇順にわざわざ変える
                for (int iD = 0; iD < rw; iD++)//diagonals 右端を０として左へ進んでいる
                {
                    int x = -iD + (rw - Pure.KATAGAWA1);//左から右に戻す

                    for (int y = 0; y < rh; y++)
                    {
                        int iMs = y * rw + x;
                        // 値をセット
                        dlgt_scanHanTokei90Masu(diagonals[iMs], (Masu)iMs, chikanHyo[iMs]);
                    }
                }
            }
        }

        /// <summary>
        /// 盤のサイズが設定されたあとに呼び出すこと
        /// </summary>
        public static void Tukurinaosi(
#if DEBUG
                string dbg_hint
#endif
            )
        {
            int tate = PureSettei.banTateHaba;
            int yoko = PureSettei.banYokoHaba;
            int heimen = PureSettei.banHeimen;
            int nanamedanDLen = PureSettei.banNanameDanLen;

            {
                //----------------------------------------
                // ビットボードの作り直し（端チェックをするものを先に）
                //----------------------------------------
                // 筋
                {
                    // o--
                    // o--
                    // o--
                    // o--
                    bb_sujiArray = new Bitboard[yoko];
                    // 左端列を立てる。
                    bb_sujiArray[0] = new Bitboard();
                    for (int iDan = 0; iDan < tate; iDan++)
                    {
                        bb_sujiArray[0].Standup((Masu)(iDan * yoko));
                    }
                    // 1ビットずつ左シフトしていく。
                    for (int iSuji = 1; iSuji < yoko; iSuji++)
                    {
                        bb_sujiArray[iSuji] = new Bitboard();
                        bb_sujiArray[iSuji].Set(bb_sujiArray[iSuji - 1]);
                        bb_sujiArray[iSuji].LeftShift(1);
                    }
                }
                // 段
                {
                    bb_danArray = new Bitboard[tate];
                    // 1段目のビットは全て立てるぜ☆（＾～＾）
                    bb_danArray[0] = new Bitboard();
                    for (int iSuji = 0; iSuji < yoko; iSuji++)
                    {
                        bb_danArray[0].Standup((Masu)iSuji);// 1段目は筋も升も同じ番号。
                    }
                    // 2段目以降は、左ビットシフト☆（＾～＾）
                    for (int iDan = 1; iDan < tate; iDan++)
                    {
                        bb_danArray[iDan] = new Bitboard();
                        bb_danArray[iDan].Set(bb_danArray[iDan - 1]);
                        bb_danArray[iDan].LeftShift(yoko);
                        //#if DEBUG
                        //                    Util_Machine.Syuturyoku.AppendLine("iDan=["+ iDan + "] KyokumenImpl.BB_DanArray[iDan]=["+ KyokumenImpl.BB_DanArray[iDan].Value + "] Option_Application.Optionlist.BanTateHaba=["+ Option_Application.Optionlist.BanTateHaba + "] Option_Application.Optionlist.BanYokoHaba=["+ Option_Application.Optionlist.BanYokoHaba + "]");
                        //                    Util_Machine.Flush(Util_Machine.Syuturyoku);
                        //#endif
                    }
                }

                //────────────────────
                // トライ用ビットボード
                //────────────────────
                bb_try = new Bitboard[Conv_Taikyokusya.itiran.Length];
                bb_try[(int)Taikyokusya.T1] = bb_danArray[0];
                bb_try[(int)Taikyokusya.T2] = bb_danArray[PureSettei.banTateHaba - 1];
                // トライ段
                {
                    bb_try[(int)Taikyokusya.T1] = bb_danArray[0];
                    bb_try[(int)Taikyokusya.T2] = bb_danArray[tate - 1];
                }

                //────────────────────
                // 成り用ビットボード
                //────────────────────
                bb_nareruZone = new Bitboard[Conv_Taikyokusya.itiran.Length];
                if (PureSettei.gameRule==GameRule.HonShogi)
                {
                    // 9x9マス盤を想定☆（＾～＾）
                    bb_nareruZone[(int)Taikyokusya.T1] = bb_danArray[0].Clone();
                    bb_nareruZone[(int)Taikyokusya.T1].Standup(bb_danArray[1]);
                    bb_nareruZone[(int)Taikyokusya.T1].Standup(bb_danArray[2]);
                    bb_nareruZone[(int)Taikyokusya.T2] = bb_danArray[PureSettei.banTateHaba - 1].Clone();
                    bb_nareruZone[(int)Taikyokusya.T2].Standup(bb_danArray[PureSettei.banTateHaba - 2]);
                    bb_nareruZone[(int)Taikyokusya.T2].Standup(bb_danArray[PureSettei.banTateHaba - 3]);
                }
                else
                {
                    bb_nareruZone[(int)Taikyokusya.T1] = bb_danArray[0].Clone();
                    bb_nareruZone[(int)Taikyokusya.T2] = bb_danArray[PureSettei.banTateHaba - 1].Clone();
                }


                //────────────────────
                // ビットボードのうち、ゲームに使っているビット
                //────────────────────
                bb_boardArea_ = new Bitboard();
                // ビットを立てていく。
                if (0 < PureSettei.banHeimen)
                {
                    bb_boardArea.Set(1);
                    for (int i = 1; i < PureSettei.banHeimen; i++)
                    {
                        bb_boardArea.LeftShift(1);
                        bb_boardArea.Standup((Masu)0);
                    }
                }
                else
                {
                    bb_boardArea.Clear();
                }

                //────────────────────
                // うさぎが飛べる升ビットボード
                //────────────────────
                bb_usagigaMiginiToberu = new Bitboard[Conv_Taikyokusya.itiran.Length];
                bb_usagigaHidariniToberu = new Bitboard[Conv_Taikyokusya.itiran.Length];
                foreach (Taikyokusya tai in Conv_Taikyokusya.itiran)
                {
                    bb_usagigaMiginiToberu[(int)tai] = new Bitboard();
                    bb_usagigaHidariniToberu[(int)tai] = new Bitboard();

                    bb_usagigaMiginiToberu[(int)tai].Set(bb_boardArea);
                    bb_usagigaHidariniToberu[(int)tai].Set(bb_boardArea);

                    // 1段目と2段目を省く
                    for (int iDanO0=0; iDanO0<2; iDanO0++)
                    {
                        bb_usagigaMiginiToberu[(int)tai].Sitdown(bb_danArray[Conv_Masu.ToDanO0_JibunSiten_ByDanO0(tai, iDanO0)]);
                        bb_usagigaHidariniToberu[(int)tai].Sitdown(bb_danArray[Conv_Masu.ToDanO0_JibunSiten_ByDanO0(tai, iDanO0)]);
                    }

                    // 右跳びは　９筋　を省く
                    bb_usagigaMiginiToberu[(int)tai].Sitdown(bb_sujiArray[Conv_Masu.ToSujiO0_JibunSiten_BySujiO0(tai, PureSettei.banYokoHaba-1)]);
                    // 左跳びは　１筋　を省く
                    bb_usagigaHidariniToberu[(int)tai].Sitdown(bb_sujiArray[Conv_Masu.ToSujiO0_JibunSiten_BySujiO0(tai, 0)]);
                }
            }






            //────────────────────
            // 筋一列ビットボード
            //────────────────────
            // o--
            // o--
            // o--
            // o--
            bb_sujiArray = new Bitboard[PureSettei.banYokoHaba];
            bb_sujiArray[0] = new Bitboard();
            //// 0x249 → 10 0100 1001(2進数)
            //BB_SujiArray[0].Set(0x249);
            // 筋一列を立てます
            for (int iDan=0; iDan<PureSettei.banTateHaba; iDan++)
            {
                bb_sujiArray[0].Standup((Masu)(iDan*PureSettei.banYokoHaba));
            }

            // 2筋目以降
            for (int iSuji=1; iSuji<PureSettei.banYokoHaba; iSuji++)
            {
                bb_sujiArray[iSuji] = new Bitboard();
                bb_sujiArray[iSuji].Set(bb_sujiArray[iSuji-1]);
                bb_sujiArray[iSuji].LeftShift(1);
            }

            //────────────────────
            // 段一列ビットボード
            //────────────────────
            // ooo
            // ---
            // ---
            // ---
            bb_danArray = new Bitboard[PureSettei.banTateHaba];
            bb_danArray[0] = new Bitboard();
            //// 0x07 → 111(2進数)
            //BB_DanArray[0].Set(0x07);
            // 段一列を立てます
            for (int iSuji = 0; iSuji < PureSettei.banYokoHaba; iSuji++)
            {
                bb_danArray[0].Standup((Masu)iSuji);
            }

            // 2段目以降
            for (int iDan = 1; iDan < PureSettei.banTateHaba; iDan++)
            {
                bb_danArray[iDan] = new Bitboard();
                bb_danArray[iDan].Set(bb_danArray[iDan-1]);
                bb_danArray[iDan].LeftShift(PureSettei.banYokoHaba);
            }

            //────────────────────
            // ローテート・ビットボードの置換表
            //────────────────────
            RotateChikanhyo.Tukurinaosi();

            //────────────────────
            // 左上がり筋一列ビットボード
            //────────────────────
            // o---
            // -o--
            // --o-
            // ---o
            {
                bb_hidariAgariSujiArray = new Bitboard[PureSettei.banNanameDanLen];
                int preDiagonals = -1;
                ScanHidariAgariSuji((int diagonals, Masu ms) =>
                {
                    if (preDiagonals != diagonals)
                    {
                        bb_hidariAgariSujiArray[diagonals] = new Bitboard();
                        preDiagonals = diagonals;
                    }
                    bb_hidariAgariSujiArray[diagonals].Standup(ms);
                });
            }

            //────────────────────
            // 左下がり筋一列ビットボード
            //────────────────────
            // ---o
            // --o-
            // -o--
            // o---
            {
                bb_hidariSagariSujiArray = new Bitboard[PureSettei.banNanameDanLen];
                int preDiagonals = -1;
                ScanHidariSagariSuji((int diagonals, Masu ms) =>
                {
                    if (preDiagonals != diagonals)
                    {
                        bb_hidariSagariSujiArray[diagonals] = new Bitboard();
                        preDiagonals = diagonals;
                    }
                    bb_hidariSagariSujiArray[diagonals].Standup(ms);
                });
            }


            //────────────────────
            // 持駒を進める（打てる）ゾーン
            //────────────────────
            // 駒の行き所のない段に打つのを防止
            // 段配列をセットしたあとで
            {
                bb_uteruZone = new Bitboard[Conv_Koma.itiran.Length];
                foreach (Koma km in Conv_Koma.itiran)
                {
                    // まず、盤面全体に打てるとするぜ☆（＾～＾）
                    bb_uteruZone[(int)km] = bb_boardArea.Clone();

                    // 次に個別に、打てない場所を除外しろだぜ☆（＾～＾）
                    if (PureSettei.gameRule == GameRule.HonShogi)
                    {
                        switch (km)
                        {
                            // ひよこ、いのしし
                            case Koma.H:
                            case Koma.S:
                                bb_uteruZone[(int)km].Sitdown(bb_danArray[0]);
                                break;
                            case Koma.h:
                            case Koma.s:
                                bb_uteruZone[(int)km].Sitdown(bb_danArray[tate - 1]);
                                break;
                            // うさぎ
                            case Koma.U:
                                bb_uteruZone[(int)km].Sitdown(bb_danArray[0]);
                                bb_uteruZone[(int)km].Sitdown(bb_danArray[1]);
                                break;
                            case Koma.u:
                                bb_uteruZone[(int)km].Sitdown(bb_danArray[tate - 1]);
                                bb_uteruZone[(int)km].Sitdown(bb_danArray[tate - 2]);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }


            //────────────────────
            // 駒の動き方
            //────────────────────
            KomanoUgokikataYk00.Tukurinaosi(
#if DEBUG
                dbg_hint
#endif
                );
        }



#region 駒の動き方
        /// <summary>
        /// 駒の動き方
        /// 横型ビットボード用（yk00）
        /// </summary>
        public static class KomanoUgokikataYk00
        {
            /// <summary>
            /// 隣利き
            /// </summary>
            static class TonarikikiElement
            {
                /// <summary>
                /// 横型ビットボード（yk00）上での、駒の動き方
                /// [先後駒][升]
                /// 
                /// FIXME: きりん　と　ぞう　は　飛び利きの方向　で分かれて入っている。２度手間☆（＾～＾）
                /// どうぶつしょうぎ　では無駄な挙動だが、本将棋に合わせておくかだぜ☆（＾～＾）
                /// 
                /// 飛び利き　は、
                /// 「きりん縦」「きりん横」「いのしし」「ぞう左上がり」「ぞう左下がり」の５つだけだぜ☆（＾～＾）
                /// </summary>
                public static Bitboard[][] ugokikataTon { get; set; }

                public static void Standup(Koma km, Masu ms, Bitboard bb_kiki)
                {
                    ugokikataTon[(int)km][(int)ms].Standup(bb_kiki);
                }
                public static void Standup(Koma km, Masu ms, Masu ms_add)
                {
                    ugokikataTon[(int)km][(int)ms].Standup(ms_add);
                }
            }
            /// <summary>
            /// 飛び利き
            /// </summary>
            static class TobikikiElement
            {
                #region プロパティ―
                /// <summary>
                /// [対局者][ハッシュキー用の飛び利きの方向][升][ブロック駒の配置パターン]
                /// 
                /// 横型ビットボード（yk00）上での、駒の動き方。
                /// １８０°回転した同じものだが、128ビットの反転はシンプルではないので、先後で作ってしまう。
                /// 
                /// FIXME: きりん　と　ぞう　は　飛び利きの方向　で分かれて入っている。２度手間☆（＾～＾）
                /// どうぶつしょうぎ　では無駄な挙動だが、本将棋に合わせておくかだぜ☆（＾～＾）
                /// 
                /// 飛び利き　は、
                /// 「きりん縦」「きりん横」「いのしし」「ぞう左上がり」「ぞう左下がり」の５つだけだぜ☆（＾～＾）
                /// </summary>
                public static Bitboard[][][][] ugokikataTob { get; set; }
                #endregion
                #region 編集
                public static void Standup(Taikyokusya tai, int nKikiDir, Masu ms,  int iBlocks, Bitboard bb_kiki)
                {
                    ugokikataTob[(int)tai][nKikiDir][(int)ms][iBlocks].Standup(bb_kiki);
                }
                public static void Standup(Taikyokusya tai, int nKikiDir, Masu ms,  int iBlocks, Masu ms_add)
                {
                    ugokikataTob[(int)tai][nKikiDir][(int)ms][iBlocks].Standup(ms_add);
                }
                #endregion
            }

            #region 状況（隣利き）
            public static Bitboard CloneElement(Koma km, Masu ms)
            {
                return TonarikikiElement.ugokikataTon[(int)km][(int)ms].Clone();
            }
            #endregion
            #region 状況（飛び利き）
            public static Bitboard CloneElement(Taikyokusya tai, TobikikiDirection kikiDir, Masu ms, int ojhsh)
            {
                return TobikikiElement.ugokikataTob[(int)tai][(int)kikiDir][(int)ms][ojhsh].Clone();
            }
            #endregion
            #region エレメントの影響（飛び利き）
            static void ToSet_ByElement(Taikyokusya tai, TobikikiDirection kikiDir, Masu ms, int ojhsh, Bitboard bb_update)
            {
                bb_update.Set(TobikikiElement.ugokikataTob[(int)tai][(int)kikiDir][(int)ms][ojhsh]);
            }
            static void ToStandup_ByElement(Taikyokusya tai, TobikikiDirection kikiDir, Masu ms, int ojhsh, Bitboard bb_update)
            {
                bb_update.Standup(TobikikiElement.ugokikataTob[(int)tai][(int)kikiDir][(int)ms][ojhsh]);
            }
            static void ToSitdown_ByElement(Taikyokusya tai, TobikikiDirection kikiDir, Masu ms, int ojhsh, Bitboard bb_update)
            {
                bb_update.Sitdown(TobikikiElement.ugokikataTob[(int)tai][(int)kikiDir][(int)ms][ojhsh]);
            }
            #endregion
            #region エレメントの影響（隣利き）
            static void ToSet_ByElement(Koma km, Masu ms, Bitboard bb_update)
            {
                bb_update.Set(TonarikikiElement.ugokikataTon[(int)km][(int)ms]);
            }
            static void ToStandup_ByElement(Koma km, Masu ms, Bitboard bb_update)
            {
                Debug.Assert(Conv_Masu.IsBanjo(ms));
                bb_update.Standup(TonarikikiElement.ugokikataTon[(int)km][(int)ms]);
            }
            static void ToSitdown_ByElement(Koma km, Masu ms, Bitboard bb_update)
            {
                bb_update.Sitdown(TonarikikiElement.ugokikataTon[(int)km][(int)ms]);
            }
            static Bitboard RefElement(Koma km, Masu ms)
            {
                return TonarikikiElement.ugokikataTon[(int)km][(int)ms];
            }
            #endregion

            #region エレメント
            public static void StandupElement(Taikyokusya tai, int nKikiDir, Masu ms, int iBlocks, Bitboard bb_kiki)
            {
                TobikikiElement.ugokikataTob[(int)tai][nKikiDir][(int)ms][iBlocks].Standup(bb_kiki);
            }
            public static void StandupElement(Taikyokusya tai, int nKikiDir, Masu ms,  int iBlocks, Masu ms_add)
            {
                TobikikiElement.ugokikataTob[(int)tai][nKikiDir][(int)ms][iBlocks].Standup(ms_add);
            }
            public static void StandupElement(Koma km, Masu ms, Bitboard bb_kiki)
            {
                TonarikikiElement.ugokikataTon[(int)km][(int)ms].Standup(bb_kiki);
            }
            public static void StandupElement(Koma km, Masu ms, Masu ms_add)
            {
                TonarikikiElement.ugokikataTon[(int)km][(int)ms].Standup(ms_add);
            }
            #endregion

            public delegate void DLGT_scanNagaiKikiRoutine(Taikyokusya tai, Masu ms, int iBlocks, Bitboard kikiBB);

#if TOBIKIKI_TEST_CODE
            //参考 縦列の頭
            kikiBB.Standup(atama_noRotateMotohyo);
                                //参考 マス
                                kikiBB.Standup((Masu) iMs);
                                //参考 縦列の尻
                                kikiBB.Standup(siri_noRotateMotohyo);
                                // pos の参考
                                {
                                    int pos = startNanamePos;
                                    // 指定マスから０方向へ（指定マスは含まない）
                                    for (int iMs2 = iMs - masuSpan; (int) atama_noRotateMotohyo <= iMs2; iMs2 -= masuSpan)
                                    {
                                        kikiBB.Standup((Masu) pos);
                                        //if (nanamedanBlocksBB.IsOn((Masu)pos))
                                        //{
                                        //    // 駒とぶつかったところまでビットを立てて抜ける
                                        //    break;
                                        //}
                                        pos--;
                                    }
    }
    //参考 お邪魔ブロック配置
    kikiBB.Standup(nanamedanBlocksBB);

                                            //参考 お邪魔ブロック配置
                                //kikiBB.Standup(new Bitboard(0,(ulong)iBlocks));



                                //kikiBB.Standup((Masu)startNanamePos);

                                // 1マス上ならこう書く
                                //int iMs2 = iMs - plusSideOffset;
                                //if (-1 < iMs2 && iMs2 < heimen)
                                //{
                                //    kikiBB.Standup((Masu)iMs2);
                                //}


                                //                                // 横一列に抜き出す前のビットボードでのマス
                                //                                int iMs2 = iMs - plusSideOffset;
                                //                                for (int nanamePos = startNanamePos - 1; -1 < nanamePos; nanamePos--)
                                //                                {
                                //                                    if (
                                //#if TOBIKIKI
                                //                                        nanamedanBlocksBB.IsOff((Masu)nanamePos)
                                //#else
                                //                                        true
                                //#endif
                                //                                        )
                                //                                    {
                                //                                        kikiBB.Standup((Masu)iMs2);
                                //                                    }
                                //                                    else
                                //                                    {
                                //                                        break;
                                //                                    }
                                //                                    iMs2 -= plusSideOffset;
                                //                                }

                                // ０から指定マス方向へ（指定マスは含まない）
                                //for (int iMs2 = (int)atama_noRotateMotohyo; iMs2 < iMs; iMs2 += masuSpan)

                                            // 最大から指定マス方向へ（指定マスは含まない）
                                //for (int iMs2 = (int)siri_noRotateMotohyo; iMs < iMs2; iMs2 -= masuSpan)

                                /*
                                // 横一列に抜き出す前のビットボードでのマス
                                int iMs2 = iMs + plusSideOffset;
                                for (int nanamePos = startNanamePos + 1; nanamePos < haba; nanamePos++)
                                {
                                    if (nanamedanBlocksBB.IsOff((Masu)nanamePos))
                                    {
                                        kikiBB.Standup((Masu)iMs2);
                                    }
                                    else
                                    {
                                        break;
                                    }
                                    iMs2 += plusSideOffset;
                                }
                                */

#endif

            public static void ScanTobikiki(
                DLGT_scanNagaiKikiRoutine dlgt_scanNagaiKikiRoutineMinusSide,
                DLGT_scanNagaiKikiRoutine dlgt_scanNagaiKikiRoutinePlusSide,
                TobikikiDirection kikiDir,
                int masuSpan,// マスとマスが離れている距離
                bool sakasa_forZHs,// ZHsのための逆さ
                int blocksToriSu
                )
            {
                //────────────────────
                // 将棋盤
                //────────────────────
                int heimen = PureSettei.banHeimen;
                Shogiban sg = new Shogiban();
                Bitboard nanamedanBlocksBB = new Bitboard();
                Bitboard bb_kiki = new Bitboard();

                for (int iTai = 0; iTai < Conv_Taikyokusya.itiran.Length; iTai++)
                {
                    Taikyokusya tai = Conv_Taikyokusya.itiran[iTai];
                    for (int iMs = 0; iMs < PureSettei.banHeimen; iMs++)
                    {
                        int nanamedanD;
                        Masu atama_reverseRotateChikanhyo;
                        Masu atama_noRotateMotohyo;
                        Masu siri_noRotateMotohyo;
                        int haba;
                        Util_Tobikiki.FromMasu(
                            kikiDir, masuSpan, sakasa_forZHs,
                            (Masu)iMs,
                            out nanamedanD,
                            out atama_reverseRotateChikanhyo,
                            out atama_noRotateMotohyo,
                            out siri_noRotateMotohyo,
                            out haba
                            );

                        // 幅の中で、駒のある場所
                        int startNanamePos;
                        if (sakasa_forZHs)
                        {
                            // 頭は数字が大きい方にある。指定のマスの方が小さい。
                            startNanamePos = ((int)atama_noRotateMotohyo - iMs) / masuSpan;//移動後の盤のマス番号
                        }
                        else
                        {
                            startNanamePos = (iMs - (int)atama_noRotateMotohyo) / masuSpan;//移動後の盤のマス番号
                        }

                        // 駒の配置
                        for (int iBlocks = 0; iBlocks < blocksToriSu; iBlocks++)
                        {
                            // お邪魔駒配置
                            nanamedanBlocksBB.Set((ulong)iBlocks);

                            // 診断：平手初期局面　飛車の横利き
#if DEBUG
                            if (kikiDir == TobikikiDirection.KY
                                && PureSettei.gameRule == GameRule.HonShogi
                                && 70 == iMs
                                && Convert.ToInt32("010000010", 2) == iBlocks // 130(10)
                            )
                            {
                                int a = 0;
                            }
#endif

                            // 中心から０に近い方へ
                            {
                                bb_kiki.Clear();

                                if (sakasa_forZHs)
                                {
                                    ToZeroSide_ForZHs(nanamedanBlocksBB, iMs, masuSpan, startNanamePos, siri_noRotateMotohyo, bb_kiki);
                                }
                                else
                                {
                                    ToSmall(nanamedanBlocksBB, iMs, masuSpan, startNanamePos, atama_noRotateMotohyo, bb_kiki);
                                }
                                dlgt_scanNagaiKikiRoutineMinusSide(tai, (Masu)iMs, iBlocks, bb_kiki);
                            }

                            // 中心から最大値に近い方へ
                            {
                                bb_kiki.Clear();

                                if (sakasa_forZHs)
                                {
                                    ToLargeSide_ForZHs(nanamedanBlocksBB, iMs, masuSpan, startNanamePos, atama_noRotateMotohyo, bb_kiki);
                                }
                                else
                                {
                                    ToBig(nanamedanBlocksBB, iMs, masuSpan, startNanamePos, siri_noRotateMotohyo, bb_kiki);
                                }

                                dlgt_scanNagaiKikiRoutinePlusSide(tai, (Masu)iMs, iBlocks, bb_kiki);
                            }

                        }
                    }
                }
            }

            /// <summary>
            /// 指定マスから０方向へ（指定マスは含まない）
            /// </summary>
            /// <param name="bb_nanamedanBlocks"></param>
            /// <param name="iMs"></param>
            /// <param name="masuSpan"></param>
            /// <param name="startNanamePos"></param>
            /// <param name="atama_noRotateMotohyo"></param>
            /// <param name="bb_kiki"></param>
            static void ToSmall(Bitboard bb_nanamedanBlocks, int iMs, int masuSpan, int startNanamePos, Masu atama_noRotateMotohyo, Bitboard bb_kiki)
            {
                int pos = startNanamePos - 1;

                //int iMs2;
                for (int iMs2 = iMs - masuSpan; (int)atama_noRotateMotohyo <= iMs2; iMs2 -= masuSpan)
                {
                    bb_kiki.Standup((Masu)iMs2);
                    if (bb_nanamedanBlocks.IsOn((Masu)pos))
                    {
                        // 駒とぶつかったところまでビットを立てて抜ける
                        break;
                    }

                    pos--;
                }

                //// 途中で抜けたら、埋めるぜ☆（＾▽＾）
                //// 2バイト目の 
                //for (; (int)atama_noRotateMotohyo <= iMs2; iMs2 -= masuSpan)
                //{

                //}
            }
            /// <summary>
            /// ZHsでは尻の方がマス番号が小さい
            /// </summary>
            /// <param name="nanamedanBlocksBB"></param>
            /// <param name="iMs"></param>
            /// <param name="masuSpan"></param>
            /// <param name="startNanamePos"></param>
            /// <param name="siri_noRotateMotohyo"></param>
            /// <param name="kikiBB"></param>
            static void ToZeroSide_ForZHs(Bitboard nanamedanBlocksBB, int iMs, int masuSpan, int startNanamePos, Masu siri_noRotateMotohyo, Bitboard kikiBB)
            {
                // 抜き出し位置は　尻の方が番号が大きい
                int pos = startNanamePos + 1;

                // 盤上では　尻の方がマス番号が小さい
                for (int iMs2 = iMs - masuSpan; (int)siri_noRotateMotohyo <= iMs2; iMs2 -= masuSpan)
                {
                    kikiBB.Standup((Masu)iMs2);
                    if (nanamedanBlocksBB.IsOn((Masu)pos))
                    {
                        // 駒とぶつかったところまでビットを立てて抜ける
                        break;
                    }

                    // 尻の方が大きい
                    pos++;
                }
            }
            /// <summary>
            /// ZHsでは、アタマの方が大きい
            /// </summary>
            /// <param name="nanamedanBlocksBB"></param>
            /// <param name="iMs"></param>
            /// <param name="masuSpan"></param>
            /// <param name="startNanamePos"></param>
            /// <param name="atama_noRotateMotohyo"></param>
            /// <param name="kikiBB"></param>
            static void ToLargeSide_ForZHs(Bitboard nanamedanBlocksBB, int iMs, int masuSpan, int startNanamePos, Masu atama_noRotateMotohyo, Bitboard kikiBB)
            {
                int pos = startNanamePos - 1;

                for (int iMs2 = iMs + masuSpan; iMs2<=(int)atama_noRotateMotohyo; iMs2 += masuSpan)
                {
                    kikiBB.Standup((Masu)iMs2);
                    if (nanamedanBlocksBB.IsOn((Masu)pos))
                    {
                        // 駒とぶつかったところまでビットを立てて抜ける
                        break;
                    }

                    pos--;
                }
            }
            /// <summary>
            /// 指定マスから最大方向へ（指定マスは含まない）
            /// </summary>
            /// <param name="nanamedanBlocksBB"></param>
            /// <param name="iMs"></param>
            /// <param name="masuSpan"></param>
            /// <param name="startNanamePos"></param>
            /// <param name="siri_noRotateMotohyo"></param>
            /// <param name="kikiBB"></param>
            static void ToBig(Bitboard nanamedanBlocksBB, int iMs, int masuSpan, int startNanamePos, Masu siri_noRotateMotohyo, Bitboard kikiBB)
            {
                int pos = startNanamePos + 1;

                for (int iMs2 = iMs + masuSpan; iMs2 <= (int)siri_noRotateMotohyo; iMs2 += masuSpan)
                {
                    kikiBB.Standup((Masu)iMs2);
                    if (nanamedanBlocksBB.IsOn((Masu)pos))
                    {
                        // 駒とぶつかったところまでビットを立てて抜ける
                        break;
                    }

                    pos++;
                }
            }

            /// <summary>
            /// この関数を呼び出すのは、ビットボード全体を作り直す関数からにすること☆（＾～＾）
            /// 
            /// 現在設定されている盤サイズ、飛び利きの指定に合わせて、
            /// ビットボードを用意するぜ☆（＾～＾）
            /// </summary>
            public static void Tukurinaosi(
#if DEBUG
                string dbg_hint
#endif
                )
            {
                int yoko = PureSettei.banYokoHaba;
                int heimen = PureSettei.banHeimen;

                //────────────────────
                // 長い利きの入れ物のサイズを先行して作り直すぜ☆（＾～＾）
                //────────────────────
                Util_Tobikiki.Tukurinaosi_1_IremonoSize();

                //────────────────────
                // 入れ物のサイズを確保☆（＾～＾）
                //────────────────────
                TobikikiElement.ugokikataTob = new Bitboard[Conv_Taikyokusya.itiran.Length][][][];
                for (int iTai=0; iTai< Conv_Taikyokusya.itiran.Length; iTai++)
                {
                    TobikikiElement.ugokikataTob[iTai] = new Bitboard[Conv_TobikikiDirection.tobikikiDirectionItiran.Length][][];
                    for (int iDir = 0; iDir < Conv_TobikikiDirection.tobikikiDirectionItiran.Length; iDir++)
                    {
                        TobikikiElement.ugokikataTob[iTai][iDir] = new Bitboard[heimen][];
                        for (int iMs = 0; iMs < heimen; iMs++)//ボードサイズを先に設定しておくこと。
                        {
                            // FIXME: 「長い利きの向き」はとりあえず大きめで、暫定
                            // FIXME: 長い方の辺が、一番でかいので暫定
                            TobikikiElement.ugokikataTob[iTai][iDir][iMs] = new Bitboard[Util_Tobikiki.nagaiHenToriSu];
                            for (int iOjhsh = 0; iOjhsh < Util_Tobikiki.nagaiHenToriSu; iOjhsh++)
                            {
                                TobikikiElement.ugokikataTob[iTai][iDir][iMs][iOjhsh] = new Bitboard();
                            }
                        }
                    }
                }
                TonarikikiElement.ugokikataTon = new Bitboard[Conv_Koma.itiran.Length][];
                foreach (Koma km_all in Conv_Koma.itiran)
                {
                    TonarikikiElement.ugokikataTon[(int)km_all] = new Bitboard[heimen];
                    for (int iMs = 0; iMs < heimen; iMs++)//ボードサイズを先に設定しておくこと。
                    {
                        // 隣利き
                        TonarikikiElement.ugokikataTon[(int)km_all][iMs] = new Bitboard();
                    }
                }

                //────────────────────
                // 隣利きを作り直すぜ☆（＾～＾）
                //────────────────────
                Util_Tonarikiki.Tukurinaosi();

                //────────────────────
                // 飛び利きを作り直すぜ☆（＾～＾）
                //────────────────────
                Util_Tobikiki.Tukurinaosi_2_Nakami(
#if DEBUG
                        dbg_hint
#endif
                    );
            }



            #region マージ
            static Bitboard bbTmp_ugokikata_forMerge = new Bitboard();
            public static void ToStandup_Merge( Koma km_all, Masu ms, Bitboard bb_update)
            {
                Debug.Assert(Conv_Masu.IsBanjo(ms));
                Merge_ToProperty(km_all, ms);

                // 駒の動ける先と、指定したビットボードのアンドを取れだぜ☆（＾～＾）
                bb_update.Standup(bbTmp_ugokikata_forMerge);
            }
            public static void ToSitdown_Merge(Koma km, Masu ms, Bitboard bb_update)
            {
                Debug.Assert(Conv_Masu.IsBanjo(ms));
                Merge_ToProperty( km, ms);

                // 駒の動ける先と、指定したビットボードのアンドを取れだぜ☆（＾～＾）
                bb_update.Sitdown(bbTmp_ugokikata_forMerge);
            }
            /// <summary>
            /// 指定された bb_update ビットボードの立っているフラグのうち、
            /// ms マスにいる km 駒の動き方で移動可能なフラグだけを残すんだぜ☆（＾～＾）
            /// 
            /// 結果は bbTmp_ugokikata_forMerge プロパティ―に入れておくぜ☆（＾～＾）
            /// </summary>
            /// <param name="yomiSg"></param>
            /// <param name="km_all"></param>
            /// <param name="ms"></param>
            static void Merge_ToProperty(Koma km_all, Masu ms)
            {
                Debug.Assert(Conv_Masu.IsBanjo(ms));

                // ビットボードを使い回すぜ☆（＾～＾）駒の動ける先を、このビットボードに書き足していけだぜ☆
                bbTmp_ugokikata_forMerge.Clear();

                // 動かした駒が手番側のもの、という想定
                Taikyokusya komaTeban = Med_Koma.KomaToTaikyokusya(km_all);
                Taikyokusya komaAiteban = Conv_Taikyokusya.Hanten(komaTeban);

                switch (km_all)
                {
                    case Koma.K:
                    case Koma.k:
                    case Koma.PK:
                    case Koma.pk:
                        {
                            // 横
                            TobikikiDirection kikiDir = TobikikiDirection.KY;
                            int ojhsh = (int)Util_Tobikiki.GetOjhsh(kikiDir, ms).value063;
                            ToStandup_ByElement(komaTeban, kikiDir, ms, ojhsh, bbTmp_ugokikata_forMerge);
                        }
                        {
                            // 縦
                            TobikikiDirection kikiDir = TobikikiDirection.KT;
                            int ojhsh = (int)Util_Tobikiki.GetOjhsh(kikiDir, ms).value063;
                            ToStandup_ByElement(komaTeban, kikiDir, ms, ojhsh, bbTmp_ugokikata_forMerge);
                        }
                        {
                            // 隣利きも。本将棋では意味ないが……☆（＾～＾）
                            ToStandup_ByElement(km_all, ms, bbTmp_ugokikata_forMerge);
                        }
                        break;
                    case Koma.Z:
                    case Koma.z:
                    case Koma.PZ:
                    case Koma.pz:
                        {
                            // 左上がり
                            TobikikiDirection kikiDir = TobikikiDirection.ZHa;
                            int ojhsh = (int)Util_Tobikiki.GetOjhsh(kikiDir, ms).value063;
                            ToStandup_ByElement(komaTeban, kikiDir, ms, ojhsh, bbTmp_ugokikata_forMerge);
                        }
                        {
                            // 左下がり
                            TobikikiDirection kikiDir = TobikikiDirection.ZHs;
                            int ojhsh = (int)Util_Tobikiki.GetOjhsh(kikiDir, ms).value063;
                            ToStandup_ByElement(komaTeban, kikiDir, ms, ojhsh, bbTmp_ugokikata_forMerge);
                        }
                        {
                            // 隣利きも。本将棋では意味ないが……☆（＾～＾）
                            ToStandup_ByElement(km_all, ms, bbTmp_ugokikata_forMerge);
                        }
                        break;
                    case Koma.S:
                    case Koma.s:
                    case Koma.PS:
                    case Koma.ps:
                        {
                            // いのしし縦
                            TobikikiDirection kikiDir = TobikikiDirection.S;
                            int ojhsh = (int)Util_Tobikiki.GetOjhsh(kikiDir, ms).value063;
                            ToStandup_ByElement(komaTeban, kikiDir, ms, ojhsh, bbTmp_ugokikata_forMerge);
                        }
                        {
                            // 隣利きも。本将棋では意味ないが……☆（＾～＾）
                            ToStandup_ByElement(km_all, ms, bbTmp_ugokikata_forMerge);
                        }
                        break;
                    default:
                        {
                            // 隣利き
                            ToStandup_ByElement(km_all, ms, bbTmp_ugokikata_forMerge);
                        }
                        break;
                }

                // 手番の駒があるところへは動けないぜ☆（＾～＾）
                PureMemory.gky_ky.yomiKy.yomiShogiban.yomiIbashoBan.ToSitdown_KomaZenbu(komaTeban, bbTmp_ugokikata_forMerge);
            }
            /// <summary>
            /// </summary>
            /// <param name="yomiSg"></param>
            /// <param name="km"></param>
            /// <param name="ms"></param>
            /// <param name="bb_update"></param>
            public static void ToSelect_MergeShogiban(Koma km, Masu ms, Bitboard bb_update)
            {
                Debug.Assert(Conv_Masu.IsBanjo(ms));
                Merge_ToProperty( km, ms);

                // 駒の動ける先と、指定したビットボードのアンドを取れだぜ☆（＾～＾）
                bb_update.Siborikomi(bbTmp_ugokikata_forMerge);                
            }
            public static void ToSet_Merge(
                Koma km,
                Masu ms,
                Bitboard bb_update
                )
            {
                Debug.Assert(Conv_Masu.IsBanjo(ms));
                Merge_ToProperty( km, ms);
                bb_update.Set(bbTmp_ugokikata_forMerge);
            }

            public static Bitboard Clone_Merge(Koma km, Masu ms_ibasho)
            {
                Debug.Assert(Conv_Masu.IsBanjo(ms_ibasho));
                Debug.Assert(Conv_Koma.IsOk(km), $"km=[{(int)km}]");

                Merge_ToProperty( km, ms_ibasho);
                return bbTmp_ugokikata_forMerge.Clone();
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="yomiSg"></param>
            /// <param name="km">駒</param>
            /// <param name="ms_src">駒のいる位置</param>
            /// <param name="ms_dst">調べたいマス</param>
            /// <returns>ms_dstマスが、駒の動き方に重なっていれば真</returns>
            public static bool IsIntersect(
                Koma km,
                Masu ms_src,
                Masu ms_dst
                )
            {
                Debug.Assert(Conv_Masu.IsBanjo(ms_src));
                Debug.Assert(Conv_Masu.IsBanjo(ms_dst));
                Merge_ToProperty( km, ms_src);
                return bbTmp_ugokikata_forMerge.IsIntersect(ms_dst);
            }
            #endregion
        }
#endregion


    }
}
