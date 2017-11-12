#if DEBUG
using kifuwarabe_shogithink.pure.ky.bb;
using System;
#else
using kifuwarabe_shogithink.pure.ky.bb;
using System;
#endif

namespace kifuwarabe_shogithink.pure.ky.tobikiki
{
    /// <summary>
    /// お邪魔盤（Occupied Bitboard）
    /// 
    /// ヨコ型が基本☆（＾～＾）
    /// </summary>
    public class OjamaBan
    {
        public class YomiOjamaBan
        {
            public YomiOjamaBan(OjamaBan hontai)
            {
                hontai_ = hontai;
            }
            OjamaBan hontai_;

            public Bitboard CloneBB()
            {
                return hontai_.value.Clone();
            }
            public YomiBitboard GetBB()
            {
                return new YomiBitboard(hontai_.value);
            }

            public bool ExistsBB(Masu ms)
            {
                return hontai_.value.IsOn(ms);
            }
            public Bitboard Nukidasi(Masu atama, int haba)
            {
                Bitboard bb = CloneBB();
#if DEBUG
                PureMemory.ssssDbg_bb_ojamaTai = bb.Clone();
#endif
                return Nukidasi_1(atama,haba, bb);
            }
            /// <summary>
            /// TODO: ４５°回転盤の抜き出しなど
            /// </summary>
            /// <param name="tai"></param>
            /// <param name="atama"></param>
            /// <param name="haba"></param>
            /// <param name="bb_update"></param>
            /// <returns></returns>
            public static Bitboard Nukidasi_1( Masu atama, int haba, Bitboard bb_update)
            {
                int rightShift = (int)atama;
#if DEBUG
                PureMemory.ssssDbg_rightShift = rightShift;
#endif
                bb_update.RightShift(rightShift);
#if DEBUG
                PureMemory.ssssDbg_bb_rightShifted = bb_update.Clone();
#endif

                Bitboard mask = BitboardsOmatome.maskHyo[haba];
#if DEBUG
                PureMemory.ssssDbg_bb_mask = mask.Clone();
#endif

                bb_update.Siborikomi(mask);
                return bb_update;
            }

            /// <summary>
            /// 指定のビットボードを更新します
            /// </summary>
            /// <param name="tai"></param>
            /// <param name="update_bb"></param>
            public void ToSitdown_BB(Bitboard update_bb)
            {
                update_bb.Sitdown(hontai_.value);
            }

            public void ToSet_BB(Bitboard update_bb)
            {
                update_bb.Set(hontai_.value);

            }
        }

        #region 生成
        public OjamaBan()
        {
            Tukurinaosi_Clear();
            yomiOjamaBan = new YomiOjamaBan(this);
        }
        public OjamaBan(OjamaBan src)
        {
            Tukurinaosi_Copy(src);
            yomiOjamaBan = new YomiOjamaBan(this);
        }
        #endregion
        #region プロパティ―
        public YomiOjamaBan yomiOjamaBan;
        /// <summary>
        /// ビットボード。盤上の駒のいる升☆
        /// </summary>
        Bitboard value { get; set; }
        #endregion
        #region 作り直し
        public void Tukurinaosi_Copy(OjamaBan src)
        {
            value = src.value.Clone();
        }
        public void Tukurinaosi_Clear()
        {
            if(null== value)
            {
                value = new Bitboard();
            }
            else
            {
                value.Clear();
            }
        }
        #endregion

        /// <summary>
        /// 駒を置きます
        /// </summary>
        /// <param name="ms"></param>
        public void N240_OkuKoma(Masu ms)
        {
            value.Standup(ms);
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
        public void N240_TorinozokuKoma(Masu ms)
        {
            value.Sitdown(ms);
        }

    }
}
