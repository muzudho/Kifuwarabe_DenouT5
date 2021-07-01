#if DEBUG
using kifuwarabe_shogithink.pure.ky.bb;
using kifuwarabe_shogithink.pure.ikkyoku;
using kifuwarabe_shogithink.pure.accessor;
using Grayscale.Kifuwarabi.Entities.Take1Base;
#else
using kifuwarabe_shogithink.pure.ikkyoku;
using kifuwarabe_shogithink.pure.accessor;
using kifuwarabe_shogithink.pure.ky.bb;
using Grayscale.Kifuwarabi.Entities.Take1Base;
#endif

namespace kifuwarabe_shogithink.pure.ky
{
    /// <summary>
    /// 局面だぜ☆（＾▽＾）
    /// 
    /// 千日手を判定するための、局面ハッシュを作るのに必要なものだけを入れろだぜ☆（*＾～＾*）
    /// 評価値とかは　ここに入れてはいけないぜ☆（＾～＾）
    /// </summary>
    public class Kyokumen
    {
        /// <summary>
        /// 読み取り専用にしたラッパー
        /// </summary>
        public class YomiKy
        {
            public YomiKy(Kyokumen ky)
            {
                hontai_ = ky;
            }
            Kyokumen hontai_;

            public Shogiban.YomiShogiban yomiShogiban
            {
                get
                {
                    return hontai_.shogiban.yomiShogiban;
                }
            }
            public MotigomaItiran.YomiMotigomaItiran yomiMotigomaItiran
            {
                get
                {
                    return hontai_.motigomaItiran.yomiMotigomaItiran;
                }
            }

            /// <summary>
            /// 駒が動いたことで、飛び利きが伸びそうな駒を返す。（ディスカバード・アタック判定用）
            /// </summary>
            public void TryInControl(Masu ms, out Piece[] out_discovered)
            {
                int iEnd = 0;
                out_discovered = new Piece[Conv_Koma.itiranTobikiki.Length + 1];
                foreach (Piece km_tobikiki in Conv_Koma.itiranTobikiki)
                {
                    if (hontai_.shogiban.kikiBan.yomiKikiBan.ExistsBBKiki(km_tobikiki, ms))
                    {
                        out_discovered[iEnd] = km_tobikiki;
                        iEnd++;
                    }
                }
                out_discovered[iEnd] = Piece.Yososu; // 終端子
            }
            public bool EqualsKiki(Piece km, Shogiban sg_hikaku)
            {
                return hontai_.shogiban.kikiBan.yomiKikiBan.EqualsKiki(km, sg_hikaku);
            }
        }

        #region 生成
        public Kyokumen()
        {
            // 将棋盤を生成する前に、盤のサイズを決めておくこと。
            yomiKy_ = new YomiKy(this);
            shogiban = new Shogiban();
            motigomaItiran = new MotigomaItiran();
        }
        public Kyokumen(Kyokumen src)
        {
            yomiKy_ = new YomiKy(this);
            shogiban = new Shogiban(src.shogiban);
            motigomaItiran = new MotigomaItiran(src.motigomaItiran);
        }
        #endregion
        public void Tukurinaosi_ClearKyokumen()
        {
            // TODO: 初期局面の評価値を算出したいんだぜ☆（＾～＾）
            PureMemory.gky_hyokati.Clear();

            // 「手目」を０に戻すぜ☆（＾～＾）
            MoveGenAccessor.BackTemeToFirst_AndClearTeme();

            // 棋譜はリセットしないぜ☆（*＾～＾*）
            motigomaItiran.Clear();
            shogiban.Tukurinaosi_Clear();

            // 差分更新のスタート地点となる、「利き」を作り直しておくぜ☆（＾～＾）
            PureMemory.gky_ky.shogiban.Tukurinaosi_RemakeKiki();
        }
        #region プロパティ―
        YomiKy yomiKy_;
        public YomiKy yomiKy
        {
            get
            {
                return yomiKy_;
            }
        }

        /// <summary>
        /// 各種データ・ボードまとめ☆（＾～＾）
        /// </summary>
        public Shogiban shogiban { get; set; }
        /// <summary>
        /// 持ち駒の数だぜ☆（＾▽＾）
        /// </summary>
        public MotigomaItiran motigomaItiran { get; set; }

        /// <summary>
        /// 左端筋
        /// </summary>
        public const int HIDARI_HAJI_SUJI = 1;
        /// <summary>
        /// 右端筋
        /// </summary>
        public int migiHaji_SUJI { get { return m_migiHaji_suji_; } }
        static int m_migiHaji_suji_ = 3;
        /// <summary>
        /// 上端段
        /// </summary>
        public const int UE_HAJI_DAN = 1;
        /// <summary>
        /// 下端段
        /// </summary>
        public int sitaHaji_DAN { get { return m_sitaHaji_dan_; } }
        static int m_sitaHaji_dan_ = 4;
        #endregion



        public void OnBanResized1(int banYokoHaba, int banTateHaba)
        {
            // Option_Application.Optionlist はまだ生成されていないタイミングでも呼び出されるぜ☆（＾～＾）引数で指定しろだぜ☆（*＾～＾*）
            m_migiHaji_suji_ = banYokoHaba;
            m_sitaHaji_dan_ = banTateHaba;
        }

    }
}
