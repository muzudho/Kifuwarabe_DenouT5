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
            Piece.R,Piece.r,// らいおん（対局者１、対局者２）
            Piece.Z,Piece.z,// ぞう
            Piece.PZ,Piece.pz,// パワーアップぞう
            Piece.K,Piece.k,// きりん
            Piece.PK,Piece.pk,// パワーアップきりん
            Piece.H,Piece.h,// ひよこ
            Piece.PH,Piece.ph,// にわとり
            Piece.I,Piece.i,// いぬ
            Piece.N,Piece.n,// ねこ
            Piece.PN,Piece.pn,// 成りねこ
            Piece.U,Piece.u,// うさぎ
            Piece.PU,Piece.pu,// 成りうさぎ
            Piece.S,Piece.s,// いのしし
            Piece.PS,Piece.ps,// 成りいのしし
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
                Piece.R,// らいおん
                Piece.Z,// ぞう
                Piece.PZ,// パワーアップぞう
                Piece.K,// きりん
                Piece.PK,// パワーアップきりん
                Piece.H,// ひよこ
                Piece.PH,// にわとり
                Piece.I,// いぬ
                Piece.N,// ねこ
                Piece.PN,// パワーアップねこ
                Piece.U,// うさぎ
                Piece.PU,// パワーアップうさぎ
                Piece.S,// いのしし
                Piece.PS,// パワーアップいのしし
            },
            new Piece[]{// 対局者２
                Piece.r,
                Piece.z,
                Piece.pz,
                Piece.k,
                Piece.pk,
                Piece.h,
                Piece.ph,
                Piece.i,
                Piece.n,
                Piece.pn,
                Piece.u,
                Piece.pu,
                Piece.s,
                Piece.ps,
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
                Piece.H,
                Piece.PH,
                Piece.S,
                Piece.PS,
                Piece.U,
                Piece.PU,
                Piece.N,
                Piece.PN,
                Piece.I,
                Piece.Z,
                Piece.PZ,
                Piece.K,
                Piece.PK,
                Piece.R,
            },
            new Piece[]{
                Piece.h,
                Piece.ph,
                Piece.s,
                Piece.ps,
                Piece.u,
                Piece.pu,
                Piece.n,
                Piece.pn,
                Piece.i,
                Piece.z,
                Piece.pz,
                Piece.k,
                Piece.pk,
                Piece.r,
            }
        };
        /// <summary>
        /// [対局者][駒種類]
        /// 指し手生成のオーダリング用（強い駒順）
        /// </summary>
        public static readonly Piece[][] itiranTuyoimonoJun = new Piece[][]
        {
            new Piece[]{
                Piece.R,
                Piece.PK,
                Piece.PZ,
                Piece.PH,
                Piece.PS,
                Piece.PU,
                Piece.PN,
                Piece.K,
                Piece.Z,
                Piece.I,
                Piece.N,
                Piece.U,
                Piece.S,
                Piece.H,
            },
            new Piece[]{
                Piece.r,
                Piece.pk,
                Piece.pz,
                Piece.ph,
                Piece.ps,
                Piece.pu,
                Piece.pn,
                Piece.k,
                Piece.z,
                Piece.i,
                Piece.n,
                Piece.u,
                Piece.s,
                Piece.h,
            }
        };
        /// <summary>
        /// 飛び利きのある駒一覧（ディスカバード・アタック用）
        /// [イテレーション]
        /// </summary>
        public static readonly Piece[] itiranTobikiki = new Piece[]
        {
            Piece.Z,Piece.z,// ぞう
            Piece.PZ,Piece.pz,// パワーアップぞう
            Piece.K,Piece.k,// きりん
            Piece.PK,Piece.pk,// パワーアップきりん
            Piece.S,Piece.s,// いのしし
        };
        /// <summary>
        /// らいおんを除いた一覧。ジャム用。
        /// </summary>
        public static readonly Piece[] itiranRaionNozoku =
        {
            // ぞう（対局者１、対局者２）
            Piece.Z,Piece.z,

            // パワーアップぞう
            Piece.PZ,Piece.pz,

            // きりん
            Piece.K,Piece.k,

            // パワーアップきりん
            Piece.PK,Piece.pk,

            // ひよこ
            Piece.H,Piece.h,

            // にわとり
            Piece.PH,Piece.ph,

            // いぬ
            Piece.I,Piece.i,

            // ねこ
            Piece.N,Piece.n,

            // パワーアップねこ
            Piece.PN,Piece.pn,

            // うさぎ
            Piece.U,Piece.u,

            // パワーアップうさぎ
            Piece.PU,Piece.pu,

            // いのしし
            Piece.S,Piece.s,

            // パワーアップいのしし
            Piece.PS,Piece.ps,

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
                case Piece.R: return Piece.r;
                case Piece.r: return Piece.R;

                case Piece.Z: return Piece.z;
                case Piece.z: return Piece.Z;

                case Piece.PZ: return Piece.pz;
                case Piece.pz: return Piece.PZ;

                case Piece.K: return Piece.k;
                case Piece.k: return Piece.K;

                case Piece.PK: return Piece.pk;
                case Piece.pk: return Piece.PK;

                case Piece.H: return Piece.h;
                case Piece.h: return Piece.H;

                case Piece.PH: return Piece.ph;
                case Piece.ph: return Piece.PH;

                case Piece.I: return Piece.i;
                case Piece.i: return Piece.I;

                case Piece.N: return Piece.n;
                case Piece.n: return Piece.N;

                case Piece.PN: return Piece.pn;
                case Piece.pn: return Piece.PN;

                case Piece.U: return Piece.u;
                case Piece.u: return Piece.U;

                case Piece.PU: return Piece.pu;
                case Piece.pu: return Piece.PU;

                case Piece.S: return Piece.s;
                case Piece.s: return Piece.S;

                case Piece.PS: return Piece.ps;
                case Piece.ps: return Piece.PS;

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
            return Piece.R <= km && km <= Piece.Kuhaku;
        }
        /// <summary>
        /// 空白、要素数以外の駒
        /// </summary>
        /// <param name="km"></param>
        /// <returns></returns>
        public static bool IsOk(Piece km)
        {
            return Piece.R <= km && km < Piece.Kuhaku;
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
                case Piece.K:
                case Piece.k:
                case Piece.Z:
                case Piece.z:
                case Piece.S:
                case Piece.s:
                case Piece.PK:
                case Piece.pk:
                case Piece.PZ:
                case Piece.pz:
                case Piece.PS:
                case Piece.ps:
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
