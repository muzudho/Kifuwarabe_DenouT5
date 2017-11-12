using System;
using System.Diagnostics;

#if DEBUG
using kifuwarabe_shogithink.pure.logger;
#endif

namespace kifuwarabe_shogithink.pure.ky
{
    /// <summary>
    /// 持駒一覧
    /// </summary>
    public class MotigomaItiran
    {
        public class YomiMotigomaItiran
        {
            public YomiMotigomaItiran(MotigomaItiran hontai)
            {
                hontai_ = hontai;
            }
            MotigomaItiran hontai_;

            public int Count(Motigoma mg)
            {
                return hontai_.valueMk[(int)mg];
            }
            public int GetArrayLength()
            {
                return hontai_.valueMk.Length;
            }
            /// <summary>
            /// 持駒を持っているなら真☆
            /// </summary>
            /// <param name="mk"></param>
            /// <returns></returns>
            public bool HasMotigoma(Motigoma mk)
            {
                return 0 < hontai_.valueMk[(int)mk];
            }
            public bool IsEmpty()
            {
                for (int i = 0; i < hontai_.valueMk.Length; i++)
                {
                    if (0 < hontai_.valueMk[i])
                    {
                        return false;
                    }
                }
                return true;
            }
        }
        public YomiMotigomaItiran yomiMotigomaItiran;

        #region 生成
        public MotigomaItiran()
        {
            valueMk = new int[(int)Motigoma.Yososu];
            yomiMotigomaItiran = new YomiMotigomaItiran(this);
        }
        public MotigomaItiran(MotigomaItiran src)
        {
            valueMk = new int[(int)Motigoma.Yososu];
            Array.Copy(src.valueMk, valueMk, src.valueMk.Length);
            yomiMotigomaItiran = new YomiMotigomaItiran(this);
        }
        #endregion

        public MotigomaItiran Clear()
        {
            Array.Clear(valueMk,0,valueMk.Length);
            return this;
        }
        #region プロパティ―
        /// <summary>
        /// 持ち駒の数だぜ☆（＾▽＾）
        /// [持駒]
        /// </summary>
        int[] valueMk { get; set; }
        #endregion

        /// <summary>
        /// 持駒の枚数にマイナスを入れてはいけないぜ☆（＾～＾）
        /// </summary>
        /// <param name="mk"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public void Set(Motigoma mk, int count)
        {
            Debug.Assert(valueMk[(int)mk] < 0, "error 持駒の数にマイナスをセットした☆");
            valueMk[(int)mk] = count;
        }
        /// <summary>
        /// 値渡し☆（＾～＾）
        /// </summary>
        /// <param name="copy"></param>
        /// <returns></returns>
        public MotigomaItiran Set(MotigomaItiran copy)
        {
            Debug.Assert(valueMk.Length==copy.valueMk.Length,"持ち駒配列の長さが違う☆");
            Array.Copy(copy.valueMk,valueMk,valueMk.Length);
            return this;
        }
        public MotigomaItiran Add(Motigoma mk, int count)
        {
            valueMk[(int)mk] += count;
            return this;
        }
        public MotigomaItiran Fuyasu(Motigoma mk)
        {
            valueMk[(int)mk] ++;
            return this;
        }
        public bool Try_Herasu(out MotigomaItiran out_ret, Motigoma mk
#if DEBUG
            , IDebugMojiretu reigai1
#endif
            )
        {
            valueMk[(int)mk]--;
#if DEBUG
            if (valueMk[(int)mk] < 0)
            {
                reigai1.AppendLine("error 持駒の数がマイナス");
                out_ret = null;
                return false;
            }
#endif
            out_ret = this;
            return true;
        }
    }
}
