using System;

namespace kifuwarabe_shogithink.pure.sasite
{
    /// <summary>
    /// ２進数各桁
    /// 
    /// 3332 2111
    /// 
    /// 1 … 駒を取らない指し or 駒を取る指し or 打
    /// 2 … 緩慢 or 王手
    /// 3 … ぼっち or 捨て or 紐づき
    /// </summary>
    [Flags]
    public enum SasiteType
    {
        /// <summary>
        /// そもそも指し手に当てはまらない、あるいは項目を使わない場合☆
        /// </summary>
        N00_Karappo = 0x00,

        /// <summary>
        /// （成分） 0000 0001 駒を取る
        /// </summary>
        _a1_toru = 0x01,

        /// <summary>
        /// （成分） 0000 0010 駒を取らずに、盤上の駒を動かすぜ☆（打の反対）
        /// </summary>
        _a2_sasi = 0x02,

        /// <summary>
        /// （成分） 0000 0100 駒台から打つぜ☆（指しの反対）
        /// </summary>
        _a3_da = 0x04,

        /// <summary>
        /// （成分） 0000 1000 非王手だぜ☆　緩慢な手☆（＾～＾）
        /// </summary>
        _b1_kanman = 0x08,

        /// <summary>
        /// （成分） 0001 0000 王手だぜ☆
        /// </summary>
        _b2_ote = 0x10,

        /// <summary>
        /// （成分） 0010 0000 敵味方の利きがないところに打ち込む手だぜ☆　ぼっち☆　（捨てでも、紐付きでもない）
        /// </summary>
        _c1_botti = 0x20,

        /// <summary>
        /// （成分） 0100 0000 味方の利きより、敵の利きが多い所に打ち込む手だぜ☆　捨て☆　（ぼっちでも、紐付きでもない）
        /// </summary>
        _c2_sute = 0x40,

        /// <summary>
        /// （成分） 1000 0000 敵の利きより、味方の利きが多い所に打ち込む手だぜ☆　紐付き☆　（ぼっちでも、捨てでもない）
        /// </summary>
        _c3_himotuki = 0x80,

        /// <summary>
        /// （ｎｎ１）駒を取る手☆
        /// </summary>
        N01_KomaWoToruTe = _a1_toru,

        //────────────────────────────────────────

        /// <summary>
        /// （１１２）良くも悪くも、どれにも当てはまらない残りの盤上の手☆（略して「ぼっち緩慢指」）
        /// </summary>
        N02_BottiKanmanSasi = _c1_botti | _b1_kanman | _a2_sasi,

        /// <summary>
        /// （１１３）ぼっち緩慢打
        /// </summary>
        N03_BottiKanmanDa = _c1_botti | _b1_kanman | _a3_da,

        // ぼっち　と　王手　は組み合わないぜ☆(＾◇＾)　捨て王手、または　紐付王手　になるからな☆（＾▽＾）
        //────────────────────────────────────────

        /// <summary>
        /// （２１２）味方の利きもなく、敵の利きがあるところに盤上の駒を動かす手☆（略して「タダ捨て指し」）
        /// </summary>
        N04_SuteKanmanSasi = _c2_sute | _b1_kanman | _a2_sasi,

        /// <summary>
        /// （２１３）味方の利きもなく、敵の利きがあるところに打つ手☆（略して「タダ捨て打」）
        /// </summary>
        N05_SuteKanmanDa = _c2_sute | _b1_kanman | _a3_da,

        /// <summary>
        /// （２２２）捨て王手指
        /// </summary>
        N06_SuteOteZasi = _c2_sute | _b2_ote | _a2_sasi,
        /// <summary>
        /// （２２３）捨て王手指
        /// </summary>
        N07_SuteOteDa = _c2_sute | _b2_ote | _a3_da,

        //────────────────────────────────────────

        /// <summary>
        /// （３１２）紐付緩慢指
        /// </summary>
        N08_HimotukiKanmanSasi = _c3_himotuki | _b1_kanman | _a2_sasi,

        /// <summary>
        /// （３１３）紐付緩慢打
        /// </summary>
        N09_HimotukiKanmanDa = _c3_himotuki | _b1_kanman | _a3_da,

        /// <summary>
        /// （３２２）紐付王手指
        /// </summary>
        N10_HimozukiOteZasi = _c3_himotuki | _b2_ote | _a2_sasi,

        /// <summary>
        /// （３２３）紐付王手打
        /// </summary>
        N11_HimodukiOteDa = _c3_himotuki | _b2_ote | _a3_da,

        //────────────────────────────────────────


        /// <summary>
        /// 0001 0000 0000 らいおんを取る手☆
        /// </summary>
        N12_RaionCatch = 0x100,

        /// <summary>
        /// らいおんが他に逃げることができない場合で、王手を仕掛けてきた駒を取りにいく手☆（略して「逼迫返討手」）
        /// </summary>
        N13_HippakuKaeriutiTe = N12_RaionCatch << 1,

        /// <summary>
        /// らいおんは逃げることもできるが、王手を仕掛けてきた駒を取る手☆（略して「余裕返討手」）
        /// </summary>
        N14_YoyuKaeriutiTe = N13_HippakuKaeriutiTe << 1,

        /// <summary>
        /// 逃げろ手☆
        /// </summary>
        N15_NigeroTe = N14_YoyuKaeriutiTe << 1,

        /// <summary>
        /// トライの手☆（らいおん　のみ）
        /// </summary>
        N16_Try = N15_NigeroTe << 1,

        // 以下、利便上の指し手タイプ

        /// <summary>
        /// （オプション）らいおんを取る手があるか調査☆
        /// </summary>
        N17_RaionCatchChosa = N01_KomaWoToruTe << 1,

        // 以下、付属

        /// <summary>
        /// 良い手リスト、悪い手リストを、良い手リスト１本にマージするなら真☆（＾～＾）
        /// </summary>
        N18_Option_MergeGoodBad = N17_RaionCatchChosa << 1,

        /// <summary>
        /// 逃げ道を開ける手☆（＾～＾）開けたくて開けているわけではないぜ☆（＾▽＾）ｗｗｗ
        /// </summary>
        N19_Option_NigemitiWoAkeruTe = N18_Option_MergeGoodBad << 1,

        /// <summary>
        /// 仲間を見捨てる動き☆（＾～＾）利きを外して仲間が取られるような動きだぜ☆（＾▽＾）ｗｗｗ
        /// </summary>
        N20_Option_MisuteruUgoki = N19_Option_NigemitiWoAkeruTe << 1,

        /// <summary>
        /// 調査を除く、すべて☆
        /// </summary>
        N21_All =
            N01_KomaWoToruTe
            | N02_BottiKanmanSasi
            | N03_BottiKanmanDa         // 2016-12-22 追加
            | N04_SuteKanmanSasi
            | N05_SuteKanmanDa
            | N06_SuteOteZasi
            | N07_SuteOteDa             // 2016-12-22 追加
            | N08_HimotukiKanmanSasi    // 2016-12-22 追加
            | N09_HimotukiKanmanDa
            | N10_HimozukiOteZasi
            | N11_HimodukiOteDa
            | N12_RaionCatch
            | N13_HippakuKaeriutiTe
            | N14_YoyuKaeriutiTe
            | N15_NigeroTe
            | N16_Try
            //N17_RaionCatchChosa
            | N18_Option_MergeGoodBad
            | N19_Option_NigemitiWoAkeruTe // 2016-12-22 追加
            | N20_Option_MisuteruUgoki // 2016-12-22 追加
            ,// タダ捨ての手も、省かない方が強いみたいだぜ☆（＾～＾）

        ///// <summary>
        ///// 静止探索用☆　駒を取る手まで☆
        ///// </summary>
        //N22_All_SeisiTansaku = N13_HippakuKaeriutiTe | N14_YoyuKaeriutiTe | N12_RaionCatch | N15_NigeroTe | N16_Try | N10_HimotukiOteSasi | N06_SuteOteSasi | N01_KomaWoToruTe | N18_Option_MergeGoodBad
    }
}
