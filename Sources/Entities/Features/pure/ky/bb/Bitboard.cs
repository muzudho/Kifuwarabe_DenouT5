#if DEBUG
using System;
using System.Diagnostics;
#else
using System;
using System.Diagnostics;
#endif

namespace kifuwarabe_shogithink.pure.ky.bb
{
    /// <summary>
    /// ビット演算
    /// 
    /// 出典：
    /// 当面C#と.NETな記録 「[C#] 一番右端の立っているビット位置を求める「ものすごい」コード 」
    /// http://d.hatena.ne.jp/siokoshou/20090704#p1
    /// </summary>
    public class Bitboard
    {
        static Bitboard()
        {
            // m_ntzTable_ の初期化
            {
                m_ntzTable_ = new int[64];
                ulong hash = 0x03F566ED27179461UL;
                for (int i = 0; i < 64; i++)
                {
                    //                       上位6桁を残す
                    m_ntzTable_[hash >> 58] = i;
                    hash <<= 1; // 右に1桁ずらす
                }
            }
        }
        static readonly int[] m_ntzTable_;

        /// <summary>
        /// こっちは升
        /// </summary>
        const int MASU64 = 64;
        /// <summary>
        /// こっちは64ビット機の意味
        /// </summary>
        const int BIT64 = 64;

        public Bitboard()
        {
        }
        public Bitboard(ulong value64127, ulong value063)
        {
            m_value64127_ = value64127;
            m_value063_ = value063;
        }
        public Bitboard(Bitboard bb)
        {
            Set(bb);
        }

        /// <summary>
        /// 右から64～127ビット目。
        /// </summary>
        public ulong value64127 { get { return m_value64127_; } }
        /// <summary>
        /// 右から0～63ビット目。
        /// </summary>
        public ulong value063 { get { return m_value063_; } }
        ulong m_value64127_;
        ulong m_value063_;

        /// <summary>
        /// 人間用の文字列表記。計算には使わない
        /// </summary>
        /// <returns></returns>
        public string ToContents()
        {
            return $"{m_value64127_}_{m_value063_}";
        }

        public Bitboard Clear() {
            m_value64127_ = 0UL;
            m_value063_ = 0UL;
            return this;
        }
        public Bitboard Clone()
        {
            Bitboard bb = new Bitboard();
            bb.Set(m_value64127_, m_value063_);
            return bb;
        }

        /// <summary>
        /// 値のコピー☆（＾▽＾）
        /// </summary>
        /// <param name="value64127"></param>
        /// <param name="value063"></param>
        public void Set(ulong value64127, ulong value063) {
            m_value64127_ = value64127;
            m_value063_ = value063;
        }
        public void Set(ulong value063) {
            m_value64127_ = 0UL;
            m_value063_ = value063;
        }
        public void Set(Bitboard bb) {
            m_value64127_ = bb.value64127;
            m_value063_ = bb.value063;
        }

        ///// <summary>
        ///// 全てのビットを立てるぜ☆（＾～＾）
        ///// </summary>
        //public void StandupAllBits()
        //{
        //    m_value64127_ = ~0UL;
        //    m_value063_ = ~0UL;
        //}
        /// <summary>
        /// ビットを立てるぜ☆（＾～＾）
        /// </summary>
        /// <param name="bb"></param>
        public void Standup(Bitboard bb)
        {
            m_value64127_ |= bb.value64127;
            m_value063_ |= bb.value063;
        }
        public void Standup(Masu masu)
        {
            if ((int)masu < MASU64)
            {
                m_value063_ |= 1UL << (int)masu;
            }
            else
            {
                m_value64127_ |= 1UL << ((int)masu - MASU64);
            }
        }
        /// <summary>
        /// 指定のビットを、降ろすぜ☆（*＾～＾*）
        /// </summary>
        /// <param name="bb"></param>
        public Bitboard Sitdown(Bitboard bb)
        {
            m_value64127_ &= ~bb.value64127;
            m_value063_ &= ~bb.value063;
            return this;
        }
        public void Sitdown(Masu ms)// 立っているビットを降ろすぜ☆
        {
            if ((int)ms < MASU64)
            {
                m_value063_ &= ~0UL ^ (1UL << (int)ms); // 全ビット立ってるのが ~0UL
            }
            else
            {
                m_value64127_ &= ~0UL ^ (1UL << ((int)ms- MASU64));// 全ビット立ってるのが ~0UL
            }
        }
        /// <summary>
        /// 指定のビット以外、降ろすぜ☆（*＾～＾*）AND算☆（＾▽＾）
        /// </summary>
        /// <param name="bb"></param>
        public Bitboard Siborikomi(Bitboard bb)
        {
            m_value64127_ &= bb.value64127;
            m_value063_ &= bb.value063;
            return this;
        }

        /// <summary>
        /// (2017-10-28) shiftを0～63から、0～無制限に対応したぜ☆（＾～＾）
        /// </summary>
        /// <param name="shift">0～無制限</param>
        /// <returns></returns>
        public Bitboard RightShift(int shift)
        {
            while (63<shift)
            {
                RightShift_1(63);
                shift -= 63;
            }

            RightShift_1(shift);
            return this;
        }
        Bitboard RightShift_1(int shift)
        {
            if (0 < shift) // 0を指定すると、「>>64-0」で「>>64」になり、挙動がおかしくなる？
            {
                Debug.Assert(-1 < shift && shift < 64, string.Format("shift={0} (range:0-63)", shift));
                // 上位ビットの右の方は、右にはみ出る。
                ulong hamideru = (~0UL >> (BIT64 - shift)) & m_value64127_; // 全ビット立ってるのが ~0UL

                // とりあえず、右シフトする
                m_value64127_ >>= shift;
                m_value063_ >>= shift;

                // はみ出た分を、左端に移動して、
                // 下位のビットの上の方へ、コピーしてみよう。
                m_value063_ |= hamideru << (BIT64 - shift);
            }
            return this;
        }
        /// <summary>
        /// (2017-10-28) shiftを0～63から、0～無制限に対応したぜ☆（＾～＾）
        /// </summary>
        /// <param name="shift">0～無制限</param>
        /// <returns></returns>
        public Bitboard LeftShift(int shift)
        {
            while (63 < shift)
            {
                LeftShift_1(63);
                shift -= 63;
            }

            LeftShift_1(shift);
            return this;
        }
        Bitboard LeftShift_1(int shift)
        {
            if (0 < shift) // シフトしないときに、このルーチンを使うと、挙動がおかしくなるだろうか？
            {
                Debug.Assert(-1 < shift && shift < 64, string.Format("shift={0} (range:0-63)", shift));

                // 下位ビットの左の方は、左にはみ出る。
                ulong hamideru = (~0UL << shift) & m_value063_; // 全ビット立ってるのが ~0UL

                // とりあえず、左シフトする
                m_value64127_ <<= shift;
                m_value063_ <<= shift;

                // はみ出た分を、右端に移動して、
                // 上位のビットの下の方へ、コピーしてみよう。
                m_value64127_ |= hamideru >> (BIT64 - shift);
            }
            return this;
        }

        /// <summary>
        /// 指定升のビットが立っていれば真。
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        public bool IsOn(Masu ms)
        {
            if ((int)ms < MASU64)
            {
                return 0UL != (value063 & (1UL << (int)ms));
            }
            else
            {
                return 0UL != (value64127 & (1UL << ((int)ms-MASU64)));
            }
        }

        public bool IsOff(Masu ms)
        {
            if ((int)ms < MASU64)
            {
                return 0UL == (value063 & (1UL << (int)ms));
            }
            else
            {
                return 0UL == (value64127 & (1UL << ((int)ms- MASU64)));
            }
        }
        public bool IsEmpty()
        {
            return 0UL == value64127 && 0UL == value063;
        }
        /// <summary>
        /// 一つでも重なっていれば
        /// </summary>
        /// <param name="bb"></param>
        /// <returns></returns>
        public bool IsIntersect(Bitboard bb)
        {
            return
                0UL != (value64127 & bb.value64127)
                ||
                0UL != (value063 & bb.value063);
        }
        public bool IsIntersect(Masu ms)
        {
            if ((int)ms < MASU64)
            {
                return 0UL != (value063 & (1UL << (int)ms));
            }
            else
            {
                return 0UL != (value64127 & (1UL << ((int)ms - MASU64)));
            }
        }

        public override bool Equals(object b)
        {
            if (b is ulong)
            {
                return value64127 == 0UL && value063 == (ulong)b;
            }

            throw new Exception($"b type is {b.GetType().Name}");
        }
        public bool Equals(Bitboard b)
        {
            return
                m_value64127_ == b.value64127
                &&
                m_value063_ == b.value063;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// 立っているビットの数を数えるぜ☆（＾▽＾）
        /// </summary>
        /// <param name="bits"></param>
        /// <returns></returns>
        public int PopCnt()
        {
            if (0UL < m_value64127_)
            {
                int high = PopCnt64_(m_value64127_);

                int low = PopCnt64_(m_value063_);

                return  high + low;
            }
            return PopCnt64_(m_value063_);
        }
        /// <summary>
        /// 64bit版☆
        /// </summary>
        /// <param name="bits"></param>
        /// <returns></returns>
        int PopCnt64_(ulong value64bit)
        {
            ulong value3263 = ((~0UL << 32) & value64bit) >> 32;
            ulong value031 = (~0UL >> 32) & value64bit;
            return PopCnt32_((int)value3263) + PopCnt32_((int)value031);
        }
        /// <summary>
        /// これ、siging int 32bit 版のアルゴリズムなのでは？
        /// 「ビットを数える・探すアルゴリズム」http://www.nminoru.jp/~nminoru/programming/bitcount.html
        /// </summary>
        /// <param name="bits"></param>
        /// <returns></returns>
        int PopCnt32_(int bits)
        {
            bits = (bits & 0x55555555) + (bits >> 1 & 0x55555555);
            bits = (bits & 0x33333333) + (bits >> 2 & 0x33333333);
            bits = (bits & 0x0f0f0f0f) + (bits >> 4 & 0x0f0f0f0f);
            bits = (bits & 0x00ff00ff) + (bits >> 8 & 0x00ff00ff);
            return (bits & 0x0000ffff) + (bits >> 16 & 0x0000ffff);
        }

        /// <summary>
        /// NTZ で、0000....00000 といったビット（０）に対応する値。
        /// 00000.....000001 が 0 なので、00000.....00000 の方は変えることに。
        /// もともと 64 だったが、可変長ビットボードに対応したいので、その仕様は取っ払ったぜ☆（＾～＾）
        /// </summary>
        const Masu NTZ_FOUND_ZERO = (Masu)int.MaxValue;
        /// <summary>
        /// Number Of Trailing Zeros
        /// 右から見て最初に立っているビットの桁を返す。そのビットは 0 にするぜ☆
        /// 
        /// ビットボードが変更されるという目印に Ref_ と関数名の頭に付けたぜ☆（＾～＾）
        /// </summary>
        /// <param name="x">任意の２進数</param>
        /// <param name="out_result">右から数えた桁。該当しない場合、NTZ_NOT_FOUND</param>
        /// <returns></returns>
        public bool Ref_PopNTZ(out Masu out_result)
        {
            if (IsEmpty())
            {
                out_result = NTZ_FOUND_ZERO;
                return false;
            }

            if (0UL < value063)
            {
                //ulong bbRight = (this.Value063 & -this.Value063);
                ulong bbRight = (value063 & (~value063+1));
                ulong i = (bbRight * 0x03F566ED27179461UL) >> 58;

                out_result = (Masu)m_ntzTable_[i];
                Sitdown(out_result);//(2017-04-22 Add)立っているビットを降ろすぜ☆
                return true;
            }

            {
                //ulong bbRight = (ulong)(this.Value64127 & -this.Value64127);
                ulong bbRight = (value64127 & (~value64127+1));
                ulong i = (bbRight * 0x03F566ED27179461UL) >> 58;

                //out_result = (Masu)m_ntzTable_[i]; // FIXME: 64 足し忘れてないか？
                out_result = (Masu)m_ntzTable_[i] + MASU64; // FIXME: 64 足し忘れてないか？
                Sitdown(out_result+MASU64);
                return true;
            }
        }
        /// <summary>
        /// Number Of Trailing Zeros
        /// 右から見て最初に立っているビットの桁
        /// </summary>
        /// <param name="x">任意の２進数。マイナスは使うので符号付き</param>
        /// <param name="out_ms">右から数えた桁0～63+64。該当しない場合、NTZ_NOT_FOUND☆（＾～＾）</param>
        /// <returns>立っているビットがあれば真☆</returns>
        public bool GetNTZ(out Masu out_ms)
        {
            if (0< m_value063_ || ~0UL == m_value063_) // 全部のビットが立っているときはマイナスの最大値なので個別に一致判定☆ ~0UL☆
            {
                int num;
                GetNTZ_(m_value063_, out num);
                out_ms = (Masu)num;
                return true;
            }

            if (0 < m_value64127_ || ~0UL == m_value64127_)
            {
                int num;
                GetNTZ_(m_value64127_, out num);
                out_ms = (Masu)(num+MASU64);//下駄を履かせているぜ☆（＾～＾）
                return true;
            }

            // x が 0 の時と 1 の時のどちらも答えが 0 で被ってしまうので、
            // 0 の方は 64 に変えている☆（＾～＾）
            out_ms = NTZ_FOUND_ZERO;
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="masu_geta">下駄を履かす☆</param>
        /// <param name="out_num">0～63。該当しない場合、64</param>
        /// <returns></returns>
        void GetNTZ_(ulong x, out int out_num)//bool, int masu_geta, Masu
        {
            //if (x == 0)
            //{
            //    out_result = NTZ_NOT_FOUND;
            //    return false;
            //}
            // x が 0 の時と 1 の時のどちらも答えが 0 で被ってしまうので、
            // 0 の方は 64 に変えている☆（＾～＾）

            //ulong y = (ulong)(x & -x); // FIXME:
            ulong y = (x & (~x+1)); // FIXME:
            // 「-x」は、0と1を反転させて、1を足したもの。
            // 仮に4桁で説明すると、
            //           x                               -x 
            // ( 0) x が 0000 なら、-x は 1111 に1を足して 0000。
            // ( 1)      0001            1110            1111。
            // ( 2)      0010            1101            1110。
            // ( 3)      0011            1100            1101。
            // ( 4)      0100            1011            1100。
            // ( 5)      0101            1010            1011。
            // ( 6)      0110            1001            1010。
            // ( 7)      0111            1000            1001。
            // ( 8)      1000            0111            1000。
            // ( 9)      1001            0110            0111。
            // (10)      1010            0101            0110。
            // (11)      1011            0100            0101。
            // (12)      1100            0011            0100。
            // (13)      1101            0010            0011。
            // (14)      1110            0001            0010。
            // (15)      1111            0000            0001。
            // になる。 
            //
            // 「x & -x」は、x と -x の両方で 1 になっている桁を残して 0 にすること。
            // 仮に4桁で説明すると、
            //
            //    ( 0) ( 1) ( 2) ( 3) ( 4) ( 5) ( 6) ( 7) ( 8) ( 9) (10) (11) (12) (13) (14) (15)
            //  x 0000 0001 0010 0011 0100 0101 0110 0111 1000 1001 1010 1011 1100 1101 1110 1111
            // -x 0000 1111 1110 1101 1100 1011 1010 1001 1000 0111 0110 0101 0100 0011 0010 0001
            //    ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ----
            //  y 0000 0001 0010 0001 0100 0001 0010 0001 1000 0001 0010 0001 0100 0001 0010 0001
            //
            // となり、上図 x 行の１番右端のビットだけ残して０にしたものが y 行になっている☆
            // y の数字を見ると
            //
            // 0000
            // 0001
            // 0010
            // 0100
            // 1000
            //
            // といった数字があり、これは 0,1,2,4,8 といった 2のN乗の数字になっているぜ☆（＾～＾）

            //int i = (int)((y * 0x03F566ED27179461UL) >> 58);
            ulong i = (y * 0x03F566ED27179461UL) >> 58;
            // 「0x03F566ED27179461」（わたしは　お財布転ろ江戸に無い泣く鎧　と覚える）は
            // 「0000 0011 1111 0101 0110 0110 1110 1101 0010 0111 0001 0111 1001 0100 0110 0001」。
            // この数字の上位６桁に着目しながら　右に1桁ずつ　ずらしながら並べると、
            //
            // ( 0) = 000000 11 1111 0101 0110 0110 1110 1101 0010 0111 0001 0111 1001 0100 0110 0001
            // ( 1) = 000001 1 1111 0101 0110 0110 1110 1101 0010 0111 0001 0111 1001 0100 0110 0001
            // ( 3) = 000011  1111 0101 0110 0110 1110 1101 0010 0111 0001 0111 1001 0100 0110 0001
            // ( 7) = 000111  111 0101 0110 0110 1110 1101 0010 0111 0001 0111 1001 0100 0110 0001
            // (15) = 001111  11 0101 0110 0110 1110 1101 0010 0111 0001 0111 1001 0100 0110 0001
            // (31) = 011111  1 0101 0110 0110 1110 1101 0010 0111 0001 0111 1001 0100 0110 0001
            // (63) = 111111   0101 0110 0110 1110 1101 0010 0111 0001 0111 1001 0100 0110 0001
            // (62) = 111110   101 0110 0110 1110 1101 0010 0111 0001 0111 1001 0100 0110 0001
            // (61) = 111101   01 0110 0110 1110 1101 0010 0111 0001 0111 1001 0100 0110 0001
            // (58) = 111010   1 0110 0110 1110 1101 0010 0111 0001 0111 1001 0100 0110 0001
            // (53) = 110101    0110 0110 1110 1101 0010 0111 0001 0111 1001 0100 0110 0001
            // (42) = 101010    110 0110 1110 1101 0010 0111 0001 0111 1001 0100 0110 0001
            // (21) = 010101    10 0110 1110 1101 0010 0111 0001 0111 1001 0100 0110 0001
            // (43) = 101011    0 0110 1110 1101 0010 0111 0001 0111 1001 0100 0110 0001
            // (22) = 010110     0110 1110 1101 0010 0111 0001 0111 1001 0100 0110 0001
            // (44) = 101100     110 1110 1101 0010 0111 0001 0111 1001 0100 0110 0001
            // (25) = 011001     10 1110 1101 0010 0111 0001 0111 1001 0100 0110 0001
            // (51) = 110011     0 1110 1101 0010 0111 0001 0111 1001 0100 0110 0001
            // (38) = 100110      1110 1101 0010 0111 0001 0111 1001 0100 0110 0001
            // (13) = 001101      110 1101 0010 0111 0001 0111 1001 0100 0110 0001
            // (27) = 011011      10 1101 0010 0111 0001 0111 1001 0100 0110 0001
            // (55) = 110111      0 1101 0010 0111 0001 0111 1001 0100 0110 0001
            // (46) = 101110       1101 0010 0111 0001 0111 1001 0100 0110 0001
            // (29) = 011101       101 0010 0111 0001 0111 1001 0100 0110 0001
            // (59) = 111011       01 0010 0111 0001 0111 1001 0100 0110 0001
            // (54) = 110110       1 0010 0111 0001 0111 1001 0100 0110 0001
            // (45) = 101101        0010 0111 0001 0111 1001 0100 0110 0001
            // (26) = 011010        010 0111 0001 0111 1001 0100 0110 0001
            // (52) = 110100        10 0111 0001 0111 1001 0100 0110 0001
            // (41) = 101001        0 0111 0001 0111 1001 0100 0110 0001
            // (18) = 010010         0111 0001 0111 1001 0100 0110 0001
            // (36) = 100100         111 0001 0111 1001 0100 0110 0001
            // ( 9) = 001001         11 0001 0111 1001 0100 0110 0001
            // (19) = 010011         1 0001 0111 1001 0100 0110 0001
            // (39) = 100111          0001 0111 1001 0100 0110 0001
            // (14) = 001110          001 0111 1001 0100 0110 0001
            // (28) = 011100          01 0111 1001 0100 0110 0001
            // (56) = 111000          1 0111 1001 0100 0110 0001
            // (49) = 110001           0111 1001 0100 0110 0001
            // (34) = 100010           111 1001 0100 0110 0001
            // ( 5) = 000101           11 1001 0100 0110 0001
            // (11) = 001011           1 1001 0100 0110 0001
            // (23) = 010111            1001 0100 0110 0001
            // (47) = 101111            001 0100 0110 0001
            // (30) = 011110            01 0100 0110 0001
            // (60) = 111100            1 0100 0110 0001
            // (57) = 111001             0100 0110 0001
            // (50) = 110010             100 0110 0001
            // (37) = 100101             00 0110 0001
            // (10) = 001010             0 0110 0001
            // (20) = 010100              0110 0001
            // (40) = 101000              110 0001
            // (17) = 010001              10 0001
            // (35) = 100011              0 0001
            // ( 6) = 000110               0001
            // (12) = 001100               001
            // (24) = 011000               01
            // (48) = 110000               1
            // (33) = 100001
            // ( 2) = 000010
            // ( 4) = 000100
            // ( 8) = 001000
            // (16) = 010000
            // (32) = 100000
            // ( 0) = 000000
            // 
            // となり、000001(1)～111111(63)のパターンが出現している☆
            // 「y *」は 1倍、2倍、4倍、8倍 になり、右にずらす桁数が 1,2,3,4 になるのと同じ。
            // 「>> 58」すると 下位58桁を消すのと同じ。上位6桁（1～63） を切り抜くことができる。

            // ( 1)～(63)の出現順はバラバラなので、別途用意したテーブルを使って並べ替える☆
            out_num = m_ntzTable_[i];// + masu_geta (Masu)()
            //return true;
        }

        //public static long PopRight(ref long x)
        //{
        //    long bbRight = (x & -x);
        //    Util_Bitboard.BitOff(ref x, bbRight);// 立っているビットを降ろすぜ☆
        //    return bbRight;
        //}

        ///// <summary>
        ///// 右から見て最初に立っているビットだけを残して 0 にしたもの。
        ///// </summary>
        ///// <param name="x"></param>
        ///// <returns></returns>
        //public static long GetRight(long x)
        //{
        //    return x & -x;
        //}

        /// <summary>
        /// 左右反転☆（＾～＾）
        /// 点対象（１８０度回転）の効果を狙っているぜ☆（*＾～＾*）
        /// </summary>
        public void Bitflip128()
        {
            ulong tmp_v64127 = Bitflip64(m_value063_);
            ulong tmp_v063 = Bitflip64(m_value64127_);

            m_value64127_ = tmp_v64127;
            m_value063_ = tmp_v063;

            //return this;
        }
        /// <summary>
        /// 参考:「ビットの並びを反転する」http://blog.livedoor.jp/techblog1/archives/5365383.html
        /// </summary>
        ulong Bitflip64(ulong a)
        {
            ulong b = a;
            a = a & 0x5555555555555555;
            b = b ^ a;
            a = (a << 1) | (b >> 1);

            b = a;
            a = a & 0x3333333333333333;
            b = b ^ a;
            a = (a << 2) | (b >> 2);

            b = a;
            a = a & 0x0f0f0f0f0f0f0f0f;
            b = b ^ a;
            a = (a << 4) | (b >> 4);

            b = a;
            a = a & 0x00ff00ff00ff00ff;
            b = b ^ a;
            a = (a << 8) | (b >> 8);

            b = a;
            a = a & 0x0000ffff0000ffff;
            b = b ^ a;
            a = (a << 16) | (b >> 16);

            b = a;
            a = a & 0x00000000ffffffff;
            b = b ^ a;
            a = (a << 32) | (b >> 32);
            return a;
        }
    }
}
