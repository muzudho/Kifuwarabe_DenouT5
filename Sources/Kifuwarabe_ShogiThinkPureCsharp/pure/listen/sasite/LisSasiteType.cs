#if DEBUG
using kifuwarabe_shogithink.pure.logger;
using kifuwarabe_shogithink.pure.sasite;
#else
using kifuwarabe_shogithink.pure.logger;
using kifuwarabe_shogithink.pure.sasite;
#endif

namespace kifuwarabe_shogithink.pure.listen.sasite
{
    public abstract class LisSasiteType
    {
        /// <summary>
        /// 指し手符号の解説。
        /// </summary>
        /// <returns></returns>
        public static void Setumei(SasiteType ss, IHyojiMojiretu hyoji)
        {
            switch (ss)
            {
                case SasiteType.N00_Karappo: hyoji.Append("未該当"); break; // どれにも当てはまらない場合☆
                case SasiteType.N01_KomaWoToruTe: hyoji.Append("取"); break; // 駒を取る手☆
                case SasiteType.N02_BottiKanmanSasi: hyoji.Append("ぼ指"); break; // これより上にも、下にも、どれにも当てはまらない残りの手☆（略して「緩慢手」）
                case SasiteType.N03_BottiKanmanDa: hyoji.Append("ぼ打"); break; // ぼっち緩慢打
                case SasiteType.N04_SuteKanmanSasi: hyoji.Append("タダ指"); break; // 味方の利きもなく、敵の利きがあるところに盤上の駒を動かす手☆（略して「タダ捨て指し」）
                case SasiteType.N05_SuteKanmanDa: hyoji.Append("タダ打"); break; // 味方の利きもなく、敵の利きがあるところに打つ手☆（略して「タダ捨て打」）
                case SasiteType.N06_SuteOteZasi: hyoji.Append("タダ王"); break; // 盤上駒で緩慢王手☆（らいおん　以外）（駒を打つ王手は除く☆）（紐付きを除く☆）
                case SasiteType.N07_SuteOteDa: hyoji.Append("タダ王打"); break; // 盤上駒で緩慢王手☆（らいおん　以外）（駒を打つ王手は除く☆）（紐付きを除く☆）
                case SasiteType.N08_HimotukiKanmanSasi: hyoji.Append("紐指"); break;
                case SasiteType.N09_HimotukiKanmanDa: hyoji.Append("紐打"); break; // 味方の利きが紐づいているところに打つ緩慢手☆（略して「紐付緩慢打」）
                case SasiteType.N10_HimozukiOteZasi: hyoji.Append("紐王"); break; // 盤上駒で紐付王手☆（らいおん　以外）（駒を打つ王手は除く☆）
                case SasiteType.N11_HimodukiOteDa: hyoji.Append("紐王打"); break; // 味方の利きが紐づいているところに打つ王手☆（略して「紐付王手打」）
                case SasiteType.N12_RaionCatch: hyoji.Append("R取"); break; // らいおんを取る手☆
                case SasiteType.N13_HippakuKaeriutiTe: hyoji.Append("逼迫返討"); break; // らいおんが他に逃げることができない場合で、王手を仕掛けてきた駒を取りにいく手☆（略して「逼迫返討手」）
                case SasiteType.N14_YoyuKaeriutiTe: hyoji.Append("余裕返討"); break; // らいおんは逃げることもできるが、王手を仕掛けてきた駒を取る手☆（略して「余裕返討手」）
                case SasiteType.N15_NigeroTe: hyoji.Append("逃"); break; // 逃げろ手☆
                case SasiteType.N16_Try: hyoji.Append("Try"); break; // トライの手☆（らいおん　のみ）
                case SasiteType.N17_RaionCatchChosa: hyoji.Append("R調"); break; // （オプション）らいおんを取る手があるか調査☆
                case SasiteType.N18_Option_MergeGoodBad: hyoji.Append("OMGB"); break; // 良い手リスト、悪い手リストを、良い手リスト１本にマージするなら真☆（＾～＾）
                case SasiteType.N19_Option_NigemitiWoAkeruTe: hyoji.Append("ONAT"); break; // 逃げ道を開ける手☆（＾～＾）開けたくて開けているわけではないぜ☆（＾▽＾）ｗｗｗ
                case SasiteType.N20_Option_MisuteruUgoki: hyoji.Append("OMis"); break; // 仲間を見捨てる動き☆（＾～＾）利きを外して仲間が取られるような動きだぜ☆（＾▽＾）ｗｗｗ
                case SasiteType.N21_All: hyoji.Append("All_"); break; // 調査を除く、すべて☆
                //case SasiteType.N22_All_SeisiTansaku: syuturyoku.Append("AllS"); break; // 静止探索用☆　駒を取る手まで☆
                default: hyoji.Append("____"); break;//設定漏れ☆（＾▽＾）
            }
        }
    }
}
