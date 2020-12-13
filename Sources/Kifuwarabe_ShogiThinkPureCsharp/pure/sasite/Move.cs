using kifuwarabe_shogithink.pure.com.sasiteorder;

namespace kifuwarabe_shogithink.pure.sasite
{

    /// <summary>
    /// 指し手。
    /// イコールで一致を調べたいから、一意にする必要があるぜ☆（スコアなどを覚えさせてはいけない）
    /// 
    /// ビット演算ができるのは 32bit int型。
    /// 
    /// 4         3         2         1Byte
    /// 0000 0000 0000 0000 0000 0000 0000 0000
    ///                               ---- ---A
    ///                     ---- ---B
    ///                 --C
    ///             -- D
    /// E
    /// 
    /// A: 自升(8 bit) 0～127。本将棋にも対応できるだろう☆（＾▽＾）
    /// B: 至升(8 bit) 0～127。本将棋にも対応できるだろう☆（＾▽＾）
    /// C: 打った駒の種類(3 bit)
    ///     000 なし
    ///     001 ぞう
    ///     010 きりん
    ///     011 ひよこ
    ///     100 いぬ
    ///     101 ねこ
    ///     110 うさぎ
    ///     111 いのしし
    /// D: 成らない/成る(0～1)   (1 bit)
    /// E: +-符号(0～1)(1 bit)
    /// 
    /// 0 なら投了だぜ☆（＾▽＾）自升と至升と打った駒の種類が0に揃うことはないんで、被らないぜ☆（＾～＾）
    /// 1 0000 0001(2)=257(10) はパスにしようぜ☆（＾～＾）
    /// </summary>
    public enum Move
    {
        Toryo = 0,
        //Pass = 257
    }

    public abstract class MoveShift
    {
        /// <summary>
        /// 自升(8 bit)
        /// </summary>
        public const int SRC_MASU = 0;

        /// <summary>
        /// 至升(8 bit)
        /// </summary>
        public const int DST_MASU = SRC_MASU + 8;// 8ビット左から

        /// <summary>
        /// 打った駒の種類(3 bit)
        /// </summary>
        public const int UTTA_KOMASYURUI = DST_MASU + 8;

        /// <summary>
        /// 成らない(1 bit)
        /// </summary>
        public const int NATTA = UTTA_KOMASYURUI + 3;
    }

    public abstract class MoveMask
    {
        /// <summary>
        /// 自升 11111111 = 0xff
        /// </summary>
        public const int SRC_MASU = 0xff;

        /// <summary>
        /// 至升 11111111 = 0xff
        /// </summary>
        public const int DST_MASU = 0xff << MoveShift.DST_MASU;

        /// <summary>
        /// 打った駒の種類 111 = 0x07
        /// </summary>
        public const int UTTA_KOMASYURUI = 0x07 << MoveShift.UTTA_KOMASYURUI;

        /// <summary>
        /// 成ったか☆ 1 = 0x01
        /// </summary>
        public const int NATTA = 0x01 << MoveShift.NATTA;
    }

    /// <summary>
    /// 指し手が間違っている理由☆
    /// </summary>
    public enum MoveMatigaiRiyu
    {
        /// <summary>
        /// エラーなし
        /// </summary>
        Karappo,

        /// <summary>
        /// パラメーターの書式が間違っているdoコマンド
        /// </summary>
        ParameterSyosikiMatigai,

        /// <summary>
        /// 持駒が無いのに駒を打った☆
        /// </summary>
        NaiMotiKomaUti,

        /// <summary>
        /// 盤外に移動しようとしたぜ☆（＾～＾）
        /// </summary>
        BangaiIdo,

        /// <summary>
        /// 自分の駒が置いてあるところに、駒を動かしたぜ☆
        /// </summary>
        TebanKomaNoTokoroheIdo,
        /// <summary>
        /// 駒が置いてあるところに、駒を打ち込んだぜ☆
        /// </summary>
        KomaGaAruTokoroheUti,

        /// <summary>
        /// 空き升に駒が置いてあると思って、動かそうとしたぜ☆
        /// </summary>
        KuhakuWoIdo,

        /// <summary>
        /// 相手の駒を動かそうとするのは、イリーガル・ムーブだぜ☆（＾▽＾）
        /// </summary>
        AiteNoKomaIdo,

        /// <summary>
        /// ひよこ以外が、にわとりになろうとしました☆
        /// </summary>
        NarenaiNari,

        /// <summary>
        /// その駒の種類からは、ありえない動きをしました。
        /// </summary>
        SonoKomasyuruiKarahaArienaiUgoki
    }

    /// <summary>
    /// 指し手のキャラクター付け
    /// </summary>
    public enum MoveCharacter
    {
        /// <summary>
        /// 定跡の評価値の高いものを優先
        /// </summary>
        HyokatiYusen,

        /// <summary>
        /// 勝率の高いものを優先
        /// </summary>
        SyorituYusen,

        /// <summary>
        /// 勝率の高いもののみ☆
        /// </summary>
        SyorituNomi,

        /// <summary>
        /// 新手を優先（定跡の指し手にない合法手）
        /// </summary>
        SinteYusen,

        /// <summary>
        /// 新手のみ☆
        /// </summary>
        SinteNomi,

        /// <summary>
        /// 定跡も、成績も見ないぜ☆
        /// </summary>
        TansakuNomi,

        /// <summary>
        /// パース・エラーなどもこれ☆
        /// </summary>
        Yososu
    }
}
