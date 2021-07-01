#if DEBUG
using kifuwarabe_shogithink.pure.com.hyoka;
using kifuwarabe_shogithink.pure.control;
using Grayscale.Kifuwarabi.Entities.Take1Base;
#else
using kifuwarabe_shogithink.pure.control;
using kifuwarabe_shogithink.pure.com.hyoka;
using Grayscale.Kifuwarabi.Entities.Take1Base;
#endif

namespace kifuwarabe_shogithink.pure.ky
{
    public abstract class Conv_Koma
    {
        /// <summary>
        /// [先後駒列挙型]
        /// 一覧
        /// </summary>
        public static readonly Piece[] itiran =
        {
            Piece.K1,Piece.K2,// らいおん（対局者１、対局者２）
            Piece.B1,Piece.B2,// ぞう
            Piece.PB1,Piece.PB2,// パワーアップぞう
            Piece.R1,Piece.R2,// きりん
            Piece.PR1,Piece.PR2,// パワーアップきりん
            Piece.P1,Piece.P2,// ひよこ
            Piece.PP1,Piece.PP2,// にわとり
            Piece.G1,Piece.G2,// いぬ
            Piece.S1,Piece.S2,// ねこ
            Piece.PS1,Piece.PS2,// 成りねこ
            Piece.N1,Piece.N2,// うさぎ
            Piece.PN1,Piece.PN2,// 成りうさぎ
            Piece.L1,Piece.L2,// いのしし
            Piece.PL1,Piece.PL2,// 成りいのしし
        };
        /// <summary>
        /// [先後駒列挙型]
        /// 成り駒か否か
        /// </summary>
        public static readonly bool[] nari =
        {
            false,false,// らいおん（対局者１、対局者２）
            false,false,// ぞう
            true,true,// パワーアップぞう
            false,false,// きりん
            true,true,// パワーアップきりん
            false,false,// ひよこ
            true,true,// にわとり
            false,false,// いぬ
            false,false,// ねこ
            true,true,// 成りねこ
            false,false,// うさぎ
            true,true,// 成りうさぎ
            false,false,// いのしし
            true,true,// 成りいのしし
        };
        /// <summary>
        /// 一覧
        /// [対局者][駒種類]
        /// </summary>
        public static readonly Piece[][] itiranTai = new Piece[][]
        {
            new Piece[]{// 対局者１
                Piece.K1,// らいおん
                Piece.B1,// ぞう
                Piece.PB1,// パワーアップぞう
                Piece.R1,// きりん
                Piece.PR1,// パワーアップきりん
                Piece.P1,// ひよこ
                Piece.PP1,// にわとり
                Piece.G1,// いぬ
                Piece.S1,// ねこ
                Piece.PS1,// パワーアップねこ
                Piece.N1,// うさぎ
                Piece.PN1,// パワーアップうさぎ
                Piece.L1,// いのしし
                Piece.PL1,// パワーアップいのしし
            },
            new Piece[]{// 対局者２
                Piece.K2,
                Piece.B2,
                Piece.PB2,
                Piece.R2,
                Piece.PR2,
                Piece.P2,
                Piece.PP2,
                Piece.G2,
                Piece.S2,
                Piece.PS2,
                Piece.N2,
                Piece.PN2,
                Piece.L2,
                Piece.PL2,
            },
            new Piece[]{
                // 該当無し
            }
        };
        /// <summary>
        /// [対局者][駒種類]
        /// 指し手生成のオーダリング用（弱いもの順）
        /// </summary>
        public static readonly Piece[][] itiranYowaimonoJun = new Piece[][]
        {
            new Piece[]{
                Piece.P1,
                Piece.PP1,
                Piece.L1,
                Piece.PL1,
                Piece.N1,
                Piece.PN1,
                Piece.S1,
                Piece.PS1,
                Piece.G1,
                Piece.B1,
                Piece.PB1,
                Piece.R1,
                Piece.PR1,
                Piece.K1,
            },
            new Piece[]{
                Piece.P2,
                Piece.PP2,
                Piece.L2,
                Piece.PL2,
                Piece.N2,
                Piece.PN2,
                Piece.S2,
                Piece.PS2,
                Piece.G2,
                Piece.B2,
                Piece.PB2,
                Piece.R2,
                Piece.PR2,
                Piece.K2,
            }
        };
        /// <summary>
        /// [対局者][駒種類]
        /// 指し手生成のオーダリング用（強い駒順）
        /// </summary>
        public static readonly Piece[][] itiranTuyoimonoJun = new Piece[][]
        {
            new Piece[]{
                Piece.K1,
                Piece.PR1,
                Piece.PB1,
                Piece.PP1,
                Piece.PL1,
                Piece.PN1,
                Piece.PS1,
                Piece.R1,
                Piece.B1,
                Piece.G1,
                Piece.S1,
                Piece.N1,
                Piece.L1,
                Piece.P1,
            },
            new Piece[]{
                Piece.K2,
                Piece.PR2,
                Piece.PB2,
                Piece.PP2,
                Piece.PL2,
                Piece.PN2,
                Piece.PS2,
                Piece.R2,
                Piece.B2,
                Piece.G2,
                Piece.S2,
                Piece.N2,
                Piece.L2,
                Piece.P2,
            }
        };
        /// <summary>
        /// 飛び利きのある駒一覧（ディスカバード・アタック用）
        /// [イテレーション]
        /// </summary>
        public static readonly Piece[] itiranTobikiki = new Piece[]
        {
            Piece.B1,Piece.B2,// ぞう
            Piece.PB1,Piece.PB2,// パワーアップぞう
            Piece.R1,Piece.R2,// きりん
            Piece.PR1,Piece.PR2,// パワーアップきりん
            Piece.L1,Piece.L2,// いのしし
        };
        /// <summary>
        /// らいおんを除いた一覧。ジャム用。
        /// </summary>
        public static readonly Piece[] itiranRaionNozoku =
        {
            // ぞう（対局者１、対局者２）
            Piece.B1,Piece.B2,

            // パワーアップぞう
            Piece.PB1,Piece.PB2,

            // きりん
            Piece.R1,Piece.R2,

            // パワーアップきりん
            Piece.PR1,Piece.PR2,

            // ひよこ
            Piece.P1,Piece.P2,

            // にわとり
            Piece.PP1,Piece.PP2,

            // いぬ
            Piece.G1,Piece.G2,

            // ねこ
            Piece.S1,Piece.S2,

            // パワーアップねこ
            Piece.PS1,Piece.PS2,

            // うさぎ
            Piece.N1,Piece.N2,

            // パワーアップうさぎ
            Piece.PN1,Piece.PN2,

            // いのしし
            Piece.L1,Piece.L2,

            // パワーアップいのしし
            Piece.PL1,Piece.PL2,

        };
        ///// <summary>
        ///// [駒]
        ///// </summary>
        //public static int[] banjoKomaHyokatiNumber = new int[]
        //{
        //    // らいおん（対局者１、対局者２）
        //    (int)HyokaNumber.Hyokati_SeiNoSu_Raion, // R
        //    (int)HyokaNumber.Hyokati_SeiNoSu_Raion, // r

        //    // ぞう
        //    (int)HyokaNumber.Hyokati_Rei, // Z
        //    (int)HyokaNumber.Hyokati_Rei, // z

        //    // パワーアップぞう
        //    (int)HyokaNumber.Hyokati_Rei,
        //    (int)HyokaNumber.Hyokati_Rei,

        //    // きりん
        //    (int)HyokaNumber.Hyokati_Rei, // K
        //    (int)HyokaNumber.Hyokati_Rei, // k

        //    // パワーアップきりん
        //    (int)HyokaNumber.Hyokati_Rei,
        //    (int)HyokaNumber.Hyokati_Rei,

        //    // ひよこ
        //    (int)HyokaNumber.Hyokati_Rei, // H
        //    (int)HyokaNumber.Hyokati_Rei, // h

        //    // にわとり
        //    (int)HyokaNumber.Hyokati_Rei, // PH
        //    (int)HyokaNumber.Hyokati_Rei, // ph

        //    // いぬ
        //    (int)HyokaNumber.Hyokati_Rei,
        //    (int)HyokaNumber.Hyokati_Rei,

        //    // ねこ
        //    (int)HyokaNumber.Hyokati_Rei,
        //    (int)HyokaNumber.Hyokati_Rei,

        //    // パワーアップねこ
        //    (int)HyokaNumber.Hyokati_Rei,
        //    (int)HyokaNumber.Hyokati_Rei,

        //    // うさぎ
        //    (int)HyokaNumber.Hyokati_Rei,
        //    (int)HyokaNumber.Hyokati_Rei,

        //    // パワーアップうさぎ
        //    (int)HyokaNumber.Hyokati_Rei,
        //    (int)HyokaNumber.Hyokati_Rei,

        //    // いのしし
        //    (int)HyokaNumber.Hyokati_Rei,
        //    (int)HyokaNumber.Hyokati_Rei,

        //    // パワーアップいのしし
        //    (int)HyokaNumber.Hyokati_Rei,
        //    (int)HyokaNumber.Hyokati_Rei,

        //    0, // 空白
        //    0, // 要素数
        //};
        public static readonly string[] m_itimojiKoma_ = {
            // らいおん（対局者１、対局者２）
            "▲ら","▽ら",

            // ぞう
            "▲ぞ","▽ぞ",

            // パワーアップぞう
            "▲+Z","▽+Z",

            // きりん
            "▲き","▽き",

            // パワーアップきりん
            "▲+K","▽+K",

            // ひよこ
            "▲ひ","▽ひ",

            // にわとり
            "▲に","▽に",

            // いぬ
            "▲い","▽い",

            // ねこ
            "▲ね","▽ね",

            // パワーアップねこ
            "▲+N","▽+N",

            // うさぎ
            "▲う","▽う",

            // パワーアップうさぎ
            "▲+U","▽+U",

            // いのしし
            "▲し","▽し",

            // パワーアップいのしし
            "▲+S","▽+S",

            // 空白、要素数
            "　　","　　",
        };
        public static readonly string[] m_dfen_ = {
            "R","r",// らいおん（対局者１、対局者２）
            "Z","z",// ぞう
            "+Z","+z",// パワーアップぞう
            "K","k",
            "+K","+k",
            "H","h",
            "+H","+h",
            "I","i",
            "N","n",
            "+N","+n",
            "U","u",
            "+U","+u",
            "S","s",
            "+S","+s",
            " ","x",//空白升☆（エラー）と、要素数☆（エラー）
        };
        public static readonly string[] m_sfen_ = {
            "K","k",// らいおん（対局者１、対局者２）
            "B","b",// ぞう
            "+B","+b",// パワーアップぞう
            "R","r",
            "+R","+r",
            "P","p",
            "+P","+p",
            "G","g",
            "S","s",
            "+S","+s",
            "N","n",
            "+N","+n",
            "L","l",
            "+L","+l",
            " ","x",//空白升☆（エラー）と、要素数☆（エラー）
        };
        /// <summary>
        /// 目視確認用の文字列を返すぜ☆（＾▽＾）
        /// </summary>
        /// <param name="km"></param>
        /// <returns></returns>

        /// <summary>
        /// 
        /// </summary>
        static readonly string[] m_namae_ = {
            // らいおん（対局者１、対局者２）
            "らいおん", "ライオン",
            // ぞう
            "ぞう", "ゾウ",
            // パワーアップぞう
            "ぱわーぞう", "パワーゾウ",
            // きりん
            "きりん", "キリン",
            // パワーアップきりん
            "ぱわーきりん", "パワーキリン",
            // ひよこ
            "ひよこ", "ヒヨコ",
            // にわとり
            "にわとり", "ニワトリ",
            // いぬ
            "いぬ", "イヌ",
            // ねこ
            "ねこ", "ネコ",
            // パワーアップねこ
            "ぱわーねこ", "パワーネコ",
            // うさぎ
            "うさぎ", "ウサギ",
            // パワーアップうさぎ
            "ぱわーうさぎ", "パワーウサギ",
            // いのしし
            "いのしし", "イノシシ",
            // パワーアップいのしし
            "ぱわーいのしし", "パワーイノシシ",
            // 空白、要素数
            "　　", "　　",
        };
        public static string GetName(Piece km)
        {
            return m_namae_[(int)km];
        }

        public static string GetFen(FenSyurui f, Piece km)
        {
            return f==FenSyurui.sfe_n ? m_sfen_[(int)km] : m_dfen_[(int)km];
        }
        /// <summary>
        /// 先後を反転☆（＾～＾）
        /// </summary>
        /// <param name="km"></param>
        /// <returns></returns>
        public static Piece Hanten(Piece km)
        {
            switch (km)
            {
                case Piece.K1: return Piece.K2;
                case Piece.K2: return Piece.K1;

                case Piece.B1: return Piece.B2;
                case Piece.B2: return Piece.B1;

                case Piece.PB1: return Piece.PB2;
                case Piece.PB2: return Piece.PB1;

                case Piece.R1: return Piece.R2;
                case Piece.R2: return Piece.R1;

                case Piece.PR1: return Piece.PR2;
                case Piece.PR2: return Piece.PR1;

                case Piece.P1: return Piece.P2;
                case Piece.P2: return Piece.P1;

                case Piece.PP1: return Piece.PP2;
                case Piece.PP2: return Piece.PP1;

                case Piece.G1: return Piece.G2;
                case Piece.G2: return Piece.G1;

                case Piece.S1: return Piece.S2;
                case Piece.S2: return Piece.S1;

                case Piece.PS1: return Piece.PS2;
                case Piece.PS2: return Piece.PS1;

                case Piece.N1: return Piece.N2;
                case Piece.N2: return Piece.N1;

                case Piece.PN1: return Piece.PN2;
                case Piece.PN2: return Piece.PN1;

                case Piece.L1: return Piece.L2;
                case Piece.L2: return Piece.L1;

                case Piece.PL1: return Piece.PL2;
                case Piece.PL2: return Piece.PL1;

                case Piece.Kuhaku: return Piece.Kuhaku;
                default: break;
            }
            return Piece.Yososu;
        }

        /// <summary>
        /// 空白、要素数以外の駒
        /// </summary>
        /// <param name="km"></param>
        /// <returns></returns>
        public static bool IsOkOrKuhaku(Piece km)
        {
            return Piece.K1 <= km && km <= Piece.Kuhaku;
        }
        /// <summary>
        /// 空白、要素数以外の駒
        /// </summary>
        /// <param name="km"></param>
        /// <returns></returns>
        public static bool IsOk(Piece km)
        {
            return Piece.K1 <= km && km < Piece.Kuhaku;
        }

        /// <summary>
        /// 走り駒か。
        /// どうぶつしょうぎの　きりんとぞうは走り駒ではないが、変数構造を共用するので、走り駒とする。
        /// </summary>
        /// <param name="km"></param>
        /// <returns></returns>
        public static bool IsHasirigoma(Piece km)
        {
            switch (km)
            {
                case Piece.R1:
                case Piece.R2:
                case Piece.B1:
                case Piece.B2:
                case Piece.L1:
                case Piece.L2:
                case Piece.PR1:
                case Piece.PR2:
                case Piece.PB1:
                case Piece.PB2:
                case Piece.PL1:
                case Piece.PL2:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// 全角１文字表記
        /// （どうぶつしょうぎ　表記にのみ対応）
        /// </summary>
        public const string ZEN1_RAION1 = "ら";
        public const string ZEN1_RAION2 = "ラ";
        public const string ZEN1_ZOU1 = "ぞ";
        public const string ZEN1_ZOU2 = "ゾ";
        public const string ZEN1_POW_ZOU1 = "+Z";
        public const string ZEN1_POW_ZOU2 = "+z";
        public const string ZEN1_KIRIN1 = "き";
        public const string ZEN1_KIRIN2 = "キ";
        public const string ZEN1_POW_KIRIN1 = "+K";
        public const string ZEN1_POW_KIRIN2 = "+k";
        public const string ZEN1_HIYOKO1 = "ひ";
        public const string ZEN1_HIYOKO2 = "ヒ";
        public const string ZEN1_POW_HIYOKO1 = "+H";
        public const string ZEN1_POW_HIYOKO2 = "+h";
        public const string ZEN1_INU1 = "い";
        public const string ZEN1_INU2 = "イ";
        public const string ZEN1_NEKO1 = "ね";
        public const string ZEN1_NEKO2 = "ネ";
        public const string ZEN1_POW_NEKO1 = "+N";
        public const string ZEN1_POW_NEKO2 = "+n";
        public const string ZEN1_USAGI1 = "う";
        public const string ZEN1_USAGI2 = "ウ";
        public const string ZEN1_POW_USAGI1 = "+U";
        public const string ZEN1_POW_USAGI2 = "+u";
        public const string ZEN1_SISI1 = "し";
        public const string ZEN1_SISI2 = "シ";
        public const string ZEN1_POW_SISI1 = "+S";
        public const string ZEN1_POW_SISI2 = "+s";
        public const string ZEN1_KUHAKU_HAN = " ";
        public const string ZEN1_KUHAKU_ZEN = "　";


    }


}
