#if DEBUG
using kifuwarabe_shogithink.pure.control;
using kifuwarabe_shogithink.pure.move;
using System;
#else
using kifuwarabe_shogithink.pure.control;
using kifuwarabe_shogithink.pure.move;
using System;
#endif

namespace kifuwarabe_shogithink.pure
{
    /// <summary>
    /// 設定画面、設定ファイル等から設定できるものは、ここだぜ☆（＾～＾）
    /// そうでないものは PureAppli の方に書けだぜ☆（＾～＾）
    /// </summary>
    public static class PureSettei
    {
        static PureSettei()
        {
            gameRule = GameRule.DobutuShogi;
            // デフォルトでは、どうぶつしょうぎの 3x4 サイズ
            banYokoHaba = 3;
            banYokoHabaOld = 0;
            banTateHaba = 4;
            banTateHabaOld = 0;
            name_playerN = new string[] { "対局者１", "対局者２" };
            random = new Random();

//#if DEBUG
//            ittedumeTukau = false;
//#else
            ittedumeTukau = true;
//#endif

            char_playerN = new MoveCharacter[] { MoveCharacter.HyokatiYusen, MoveCharacter.HyokatiYusen };
            p1Com = false;
            p2Com = false;
        }

        /// <summary>
        /// ベースとなるゲームルール
        /// </summary>
        public static GameRule gameRule { get; set; }

        /// <summary>
        /// マスの数（算出）
        /// </summary>
        public static int banHeimen { get { return banYokoHaba * banTateHaba; } }

        /// <summary>
        /// 盤のヨコ幅が何マスか。
        /// </summary>
        public static int banYokoHaba { get; set; }
        public static int banYokoHabaOld { get; set; }

        /// <summary>
        /// 盤のタテ幅が何マスか。
        /// </summary>
        public static int banTateHaba { get; set; }
        public static int banTateHabaOld { get; set; }

        /// <summary>
        /// ナナメ筋の数（算出）
        /// </summary>
        public static int banNanameDanLen { get { return banYokoHaba + banTateHaba - 1; } }


        /// <summary>
        /// USI通信モードを途中でやめたくなったら偽にして使う☆（＾～＾）
        /// </summary>
        public static bool usi { get; set; }
        public static FenSyurui fenSyurui { get; set; }

        /// <summary>
        /// 対局者Ｎの表示名☆（＾▽＾）コンソール・ゲーム用だぜ☆
        /// </summary>
        public static string[] name_playerN { get; set; }

        /// <summary>
        /// 乱数だぜ☆（＾～＾）
        /// </summary>
        public static Random random { get; set; }
        /// <summary>
        /// 指し手生成で「一手詰め判定」を使う☆（＾～＾）
        /// デバッグ目的で使わないなら偽だぜ☆（＾▽＾）
        /// </summary>
        public static bool ittedumeTukau { get; set; }

        /// <summary>
        /// 飛び利きを使うなら真
        /// </summary>
        public static bool tobikikiTukau { get; set; }

        /// <summary>
        /// 対局者Ｎの指し手の性格☆（＾▽＾）
        /// </summary>
        public static MoveCharacter[] char_playerN { get; set; }
        /// <summary>
        /// 対局者１はコンピューター☆（＾▽＾）
        /// </summary>
        public static bool p1Com { get; set; }
        /// <summary>
        /// 対局者２はコンピューター☆（＾▽＾）
        /// </summary>
        public static bool p2Com { get; set; }

    }
}
