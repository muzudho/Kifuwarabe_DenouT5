#if DEBUG
using kifuwarabe_shogithink.pure.com.hyoka;
using kifuwarabe_shogithink.pure.control;
#else
using kifuwarabe_shogithink.pure.control;
using kifuwarabe_shogithink.pure.com.hyoka;
#endif

namespace kifuwarabe_shogithink.pure.ky
{
    /// <summary>
    /// 先後付きの盤上の駒だぜ☆（＾▽＾）
    /// </summary>
    public enum Koma
    {
        /// <summary>
        /// らいおん（対局者１，２）
        /// </summary>
        R,r,

        /// <summary>
        /// ぞう
        /// </summary>
        Z,z,

        /// <summary>
        /// パワーアップぞう
        /// </summary>
        PZ,pz,

        /// <summary>
        /// きりん
        /// </summary>
        K,k,

        /// <summary>
        /// パワーアップきりん
        /// </summary>
        PK,pk,

        /// <summary>
        /// ひよこ
        /// </summary>
        H,h,

        /// <summary>
        /// にわとり
        /// </summary>
        PH,ph,

        /// <summary>
        /// いぬ
        /// </summary>
        I,i,

        /// <summary>
        /// ねこ
        /// </summary>
        N,n,

        /// <summary>
        /// 成りねこ
        /// </summary>
        PN,pn,

        /// <summary>
        /// うさぎ
        /// </summary>
        U,u,

        /// <summary>
        /// 成りうさぎ
        /// </summary>
        PU,pu,

        /// <summary>
        /// いのしし
        /// </summary>
        S,s,

        /// <summary>
        /// 成りいのしし
        /// </summary>
        PS,ps,

        /// <summary>
        /// 空白☆ 駒のない升だぜ☆（＾▽＾）
        /// </summary>
        Kuhaku,

        /// <summary>
        /// 空白～後手のにわとり　までの要素の個数になるぜ☆（＾▽＾）
        /// </summary>
        Yososu
    }

    public abstract class Conv_Koma
    {
        /// <summary>
        /// [先後駒列挙型]
        /// 一覧
        /// </summary>
        public static readonly Koma[] itiran =
        {
            Koma.R,Koma.r,// らいおん（対局者１、対局者２）
            Koma.Z,Koma.z,// ぞう
            Koma.PZ,Koma.pz,// パワーアップぞう
            Koma.K,Koma.k,// きりん
            Koma.PK,Koma.pk,// パワーアップきりん
            Koma.H,Koma.h,// ひよこ
            Koma.PH,Koma.ph,// にわとり
            Koma.I,Koma.i,// いぬ
            Koma.N,Koma.n,// ねこ
            Koma.PN,Koma.pn,// 成りねこ
            Koma.U,Koma.u,// うさぎ
            Koma.PU,Koma.pu,// 成りうさぎ
            Koma.S,Koma.s,// いのしし
            Koma.PS,Koma.ps,// 成りいのしし
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
        public static readonly Koma[][] itiranTai = new Koma[][]
        {
            new Koma[]{// 対局者１
                Koma.R,// らいおん
                Koma.Z,// ぞう
                Koma.PZ,// パワーアップぞう
                Koma.K,// きりん
                Koma.PK,// パワーアップきりん
                Koma.H,// ひよこ
                Koma.PH,// にわとり
                Koma.I,// いぬ
                Koma.N,// ねこ
                Koma.PN,// パワーアップねこ
                Koma.U,// うさぎ
                Koma.PU,// パワーアップうさぎ
                Koma.S,// いのしし
                Koma.PS,// パワーアップいのしし
            },
            new Koma[]{// 対局者２
                Koma.r,
                Koma.z,
                Koma.pz,
                Koma.k,
                Koma.pk,
                Koma.h,
                Koma.ph,
                Koma.i,
                Koma.n,
                Koma.pn,
                Koma.u,
                Koma.pu,
                Koma.s,
                Koma.ps,
            },
            new Koma[]{
                // 該当無し
            }
        };
        /// <summary>
        /// [対局者][駒種類]
        /// 指し手生成のオーダリング用（弱いもの順）
        /// </summary>
        public static readonly Koma[][] itiranYowaimonoJun = new Koma[][]
        {
            new Koma[]{
                Koma.H,
                Koma.PH,
                Koma.S,
                Koma.PS,
                Koma.U,
                Koma.PU,
                Koma.N,
                Koma.PN,
                Koma.I,
                Koma.Z,
                Koma.PZ,
                Koma.K,
                Koma.PK,
                Koma.R,
            },
            new Koma[]{
                Koma.h,
                Koma.ph,
                Koma.s,
                Koma.ps,
                Koma.u,
                Koma.pu,
                Koma.n,
                Koma.pn,
                Koma.i,
                Koma.z,
                Koma.pz,
                Koma.k,
                Koma.pk,
                Koma.r,
            }
        };
        /// <summary>
        /// [対局者][駒種類]
        /// 指し手生成のオーダリング用（強い駒順）
        /// </summary>
        public static readonly Koma[][] itiranTuyoimonoJun = new Koma[][]
        {
            new Koma[]{
                Koma.R,
                Koma.PK,
                Koma.PZ,
                Koma.PH,
                Koma.PS,
                Koma.PU,
                Koma.PN,
                Koma.K,
                Koma.Z,
                Koma.I,
                Koma.N,
                Koma.U,
                Koma.S,
                Koma.H,
            },
            new Koma[]{
                Koma.r,
                Koma.pk,
                Koma.pz,
                Koma.ph,
                Koma.ps,
                Koma.pu,
                Koma.pn,
                Koma.k,
                Koma.z,
                Koma.i,
                Koma.n,
                Koma.u,
                Koma.s,
                Koma.h,
            }
        };
        /// <summary>
        /// 飛び利きのある駒一覧（ディスカバード・アタック用）
        /// [イテレーション]
        /// </summary>
        public static readonly Koma[] itiranTobikiki = new Koma[]
        {
            Koma.Z,Koma.z,// ぞう
            Koma.PZ,Koma.pz,// パワーアップぞう
            Koma.K,Koma.k,// きりん
            Koma.PK,Koma.pk,// パワーアップきりん
            Koma.S,Koma.s,// いのしし
        };
        /// <summary>
        /// らいおんを除いた一覧。ジャム用。
        /// </summary>
        public static readonly Koma[] itiranRaionNozoku =
        {
            // ぞう（対局者１、対局者２）
            Koma.Z,Koma.z,

            // パワーアップぞう
            Koma.PZ,Koma.pz,

            // きりん
            Koma.K,Koma.k,

            // パワーアップきりん
            Koma.PK,Koma.pk,

            // ひよこ
            Koma.H,Koma.h,

            // にわとり
            Koma.PH,Koma.ph,

            // いぬ
            Koma.I,Koma.i,

            // ねこ
            Koma.N,Koma.n,

            // パワーアップねこ
            Koma.PN,Koma.pn,

            // うさぎ
            Koma.U,Koma.u,

            // パワーアップうさぎ
            Koma.PU,Koma.pu,

            // いのしし
            Koma.S,Koma.s,

            // パワーアップいのしし
            Koma.PS,Koma.ps,

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
        public static string GetName(Koma km)
        {
            return m_namae_[(int)km];
        }

        public static string GetFen(FenSyurui f, Koma km)
        {
            return f==FenSyurui.sfe_n ? m_sfen_[(int)km] : m_dfen_[(int)km];
        }
        /// <summary>
        /// 先後を反転☆（＾～＾）
        /// </summary>
        /// <param name="km"></param>
        /// <returns></returns>
        public static Koma Hanten(Koma km)
        {
            switch (km)
            {
                case Koma.R: return Koma.r;
                case Koma.r: return Koma.R;

                case Koma.Z: return Koma.z;
                case Koma.z: return Koma.Z;

                case Koma.PZ: return Koma.pz;
                case Koma.pz: return Koma.PZ;

                case Koma.K: return Koma.k;
                case Koma.k: return Koma.K;

                case Koma.PK: return Koma.pk;
                case Koma.pk: return Koma.PK;

                case Koma.H: return Koma.h;
                case Koma.h: return Koma.H;

                case Koma.PH: return Koma.ph;
                case Koma.ph: return Koma.PH;

                case Koma.I: return Koma.i;
                case Koma.i: return Koma.I;

                case Koma.N: return Koma.n;
                case Koma.n: return Koma.N;

                case Koma.PN: return Koma.pn;
                case Koma.pn: return Koma.PN;

                case Koma.U: return Koma.u;
                case Koma.u: return Koma.U;

                case Koma.PU: return Koma.pu;
                case Koma.pu: return Koma.PU;

                case Koma.S: return Koma.s;
                case Koma.s: return Koma.S;

                case Koma.PS: return Koma.ps;
                case Koma.ps: return Koma.PS;

                case Koma.Kuhaku: return Koma.Kuhaku;
                default: break;
            }
            return Koma.Yososu;
        }

        /// <summary>
        /// 空白、要素数以外の駒
        /// </summary>
        /// <param name="km"></param>
        /// <returns></returns>
        public static bool IsOkOrKuhaku(Koma km)
        {
            return Koma.R <= km && km <= Koma.Kuhaku;
        }
        /// <summary>
        /// 空白、要素数以外の駒
        /// </summary>
        /// <param name="km"></param>
        /// <returns></returns>
        public static bool IsOk(Koma km)
        {
            return Koma.R <= km && km < Koma.Kuhaku;
        }

        /// <summary>
        /// 走り駒か。
        /// どうぶつしょうぎの　きりんとぞうは走り駒ではないが、変数構造を共用するので、走り駒とする。
        /// </summary>
        /// <param name="km"></param>
        /// <returns></returns>
        public static bool IsHasirigoma(Koma km)
        {
            switch (km)
            {
                case Koma.K:
                case Koma.k:
                case Koma.Z:
                case Koma.z:
                case Koma.S:
                case Koma.s:
                case Koma.PK:
                case Koma.pk:
                case Koma.PZ:
                case Koma.pz:
                case Koma.PS:
                case Koma.ps:
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
