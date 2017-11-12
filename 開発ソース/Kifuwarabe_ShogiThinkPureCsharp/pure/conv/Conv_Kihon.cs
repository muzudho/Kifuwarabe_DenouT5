using System;

namespace kifuwarabe_shogithink.pure.conv
{
    public abstract class Conv_Kihon
    {
        /// <summary>
        /// 筋の数字をアルファベットに置き換えるために用意したぜ☆（＾▽＾）
        /// </summary>
        static string[] m_LargeAlphabets_ = { "×", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
        static string[] m_SmallAlphabets_ = { "×", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
        public static string[] ZenkakuAlphabet = new[] { "Ａ", "Ｂ", "Ｃ", "Ｄ", "Ｅ", "Ｆ", "Ｇ", "Ｈ", "Ｉ", "Ｊ", "Ｋ", "Ｌ", "Ｍ", "Ｎ", "Ｏ", "Ｐ", "Ｑ", "Ｒ", "Ｓ", "Ｔ", "Ｕ", "Ｖ", "Ｗ", "Ｘ", "Ｙ", "Ｚ" };
        static string[] ZenkakuInteger = new[] { "０", "１", "２", "３", "４", "５", "６", "７", "８", "９" };

        /// <summary>
        /// 1～9 を、a～i に変換するぜ☆（＾▽＾）
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string ToAlphabetSmall(int num)
        {
            if (0<=num && num<=9)
            {
                return Conv_Kihon.m_SmallAlphabets_[num];
            }
            else
            {
                throw new Exception("数字[" + num + "]をアルファベットに変えることはできませんでした。");
            }
        }
        public static string ToAlphabetLarge(int num)
        {
            if (0 <= num && num <= 9)
            {
                return Conv_Kihon.m_LargeAlphabets_[num];
            }
            else
            {
                throw new Exception("数字[" + num + "]をアルファベットに変えることはできませんでした。");
            }
        }

        /// <summary>
        /// a～i を、1～9 に変換します。
        /// </summary>
        /// <param name="alphabet"></param>
        /// <returns></returns>
        public static int AlphabetToInt(string alphabet)
        {
            // a を 1、b を 2、c を 3、...リミットチェックなし☆
            //無理 return alphabet.ToLower().ToCharArray()[0] - 'a' + 1;
            //*
            switch (alphabet.ToLower())
            {
                case "a": return 1;
                case "b": return 2;
                case "c": return 3;
                case "d": return 4;
                case "e": return 5;
                case "f": return 6;
                case "g": return 7;
                case "h": return 8;
                case "i": return 9;
                default: return -1;
            }
            // */
        }

        public static string ToZenkakuInteger(int value)
        {
            if(-1 < value && value < ZenkakuInteger.Length)
            {
                return ZenkakuInteger[value];
            }
            return "×";
        }
    }
}
