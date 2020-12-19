using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace kifuwarabe_shogithink.pure.ky
{
    /// <summary>
    /// 先後を付けない、持駒だぜ☆（＾▽＾）
    /// </summary>
    public enum MotigomaSyurui
    {
        /// <summary>
        /// ぞう
        /// </summary>
        Z,

        /// <summary>
        /// きりん
        /// </summary>
        K,

        /// <summary>
        /// ひよこ
        /// </summary>
        H,

        /// <summary>
        /// いぬ
        /// </summary>
        I,

        /// <summary>
        /// ねこ
        /// </summary>
        Neko,

        /// <summary>
        /// うさぎ
        /// </summary>
        U,

        /// <summary>
        /// いのしし
        /// </summary>
        S,

        /// <summary>
        /// 要素の個数だぜ☆（＾▽＾）
        /// どの駒の種類にも当てはまらない場合に、これを使うことがある☆（＾▽＾）ｗｗｗ
        /// </summary>
        Yososu
    }

    public abstract class Conv_MotigomaSyurui
    {
        public static readonly MotigomaSyurui[] itiran = {
            MotigomaSyurui.Z,// ぞう
            MotigomaSyurui.K,// きりん
            MotigomaSyurui.H,// ひよこ
            MotigomaSyurui.I,
            MotigomaSyurui.Neko,
            MotigomaSyurui.U,
            MotigomaSyurui.S,
        };
        /// <summary>
        /// 指し手生成のオーダリング用
        /// 弱い駒順（持駒）
        /// </summary>
        public static readonly MotigomaSyurui[] itiranYowaimonoJun = {
            MotigomaSyurui.H,// ひよこ
            MotigomaSyurui.S,
            MotigomaSyurui.U,
            MotigomaSyurui.Neko,
            MotigomaSyurui.I,
            MotigomaSyurui.Z,// ぞう
            MotigomaSyurui.K,// きりん
        };
        /// <summary>
        /// 指し手生成のオーダリング用
        /// 強い駒順（持駒）
        /// </summary>
        public static readonly MotigomaSyurui[] itiranTuyoimonoJun = {
            MotigomaSyurui.K,// きりん
            MotigomaSyurui.Z,// ぞう
            MotigomaSyurui.I,
            MotigomaSyurui.Neko,
            MotigomaSyurui.U,
            MotigomaSyurui.S,
            MotigomaSyurui.H,// ひよこ
        };
        /// <summary>
        /// 要素の個数に、「なし」を１個加えたもの。
        /// </summary>
        public readonly static int SETS_LENGTH = itiran.Length + 1;

        /// <summary>
        /// どうぶつしょうぎFen
        /// </summary>
        public readonly static string[] m_dfen_ = {
            "Z",
            "K",
            "H",
            "I",
            "N",
            "U",
            "S",
            "×",
        };
        /// <summary>
        /// 本将棋Fen
        /// </summary>
        public readonly static string[] m_sfen_ = {
            "B",
            "R",
            "P",
            "G",
            "S",
            "N",
            "L",
            "×",
        };

        /// <summary>
        /// 表示用
        /// [持駒種類]
        /// </summary>
        static string[] m_hyojiName_ = {
            "ぞ",
            "き",
            "ひ",
            "い",
            "ね",
            "う",
            "し",
            "×"
        };
        public static string GetHyojiName(MotigomaSyurui mks) { return m_hyojiName_[(int)mks]; }

        public static bool IsOk(MotigomaSyurui mks)
        {
            return MotigomaSyurui.Z <= mks && mks < MotigomaSyurui.Yososu;
        }
    }

}
