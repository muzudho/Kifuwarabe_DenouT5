#if DEBUG
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.sasite;
using System.Diagnostics;
#else
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.sasite;
using System.Diagnostics;
#endif

namespace kifuwarabe_shogithink.pure.conv.genkyoku.play
{
    public abstract class Conv_Sasite
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ss"></param>
        /// <returns></returns>
        public static Masu GetSrcMasu_WithoutErrorCheck(int ss)
        {
            return (Masu)((ss & MoveMask.SRC_MASU) >> MoveShift.SRC_MASU);
            // if (Sasite.Toryo == ss || Conv_Sasite.IsUtta(ss)) { return KyokumenImpl.MASU_ERROR; }// エラーチェック付き
        }
        public static Masu GetDstMasu_WithoutErrorCheck(int ss)
        {
            return (Masu)((ss & MoveMask.DST_MASU) >> MoveShift.DST_MASU);
        }
        public static Masu GetDstMasu(Move ss)
        {
            // エラーチェック付き
            if (Move.Toryo == ss) { return Conv_Masu.masu_error; }
            return GetDstMasu_WithoutErrorCheck((int)ss);
        }
        /// <summary>
        /// 自筋
        /// </summary>
        /// <param name="ss">指し手</param>
        /// <returns></returns>
        public static int GetSrcSuji_WithoutErrorCheck(int ss)
        {
            // (v & m) >> s + 1。 v:バリュー、m:マスク、s:シフト
            return Conv_Masu.ToSujiO1_WithoutErrorCheck((int)GetSrcMasu_WithoutErrorCheck(ss));
            // if (Sasite.Toryo == ss || Conv_Sasite.IsUtta(ss)) { return Conv_Masu.ERROR_SUJI; } // エラーチェック付き
        }
        public static int GetSrcDan_WithoutErrorCheck(int ss)
        {
            // (v & m) >> s + 1。 v:バリュー、m:マスク、s:シフト
            return Conv_Masu.ToDanO1_WithoutErrorCheck((int)GetSrcMasu_WithoutErrorCheck(ss));
            // if (Sasite.Toryo == ss || Conv_Sasite.IsUtta(ss)) { return Conv_Masu.ERROR_DAN; }  // エラーチェック付き
        }
        public static int GetDstSuji_WithoutErrorCheck(int ss)
        {
            // (v & m) >> s + 1。 v:バリュー、m:マスク、s:シフト
            return Conv_Masu.ToSujiO1_WithoutErrorCheck((int)GetDstMasu_WithoutErrorCheck(ss));
            // if (Sasite.Toryo == ss) { return Conv_Masu.ERROR_SUJI; } // エラーチェック付き
        }
        public static int GetDstDan_WithoutErrorCheck(int ss)
        {
            // (v & m) >> s + 1。 v:バリュー、m:マスク、s:シフト
            return Conv_Masu.ToDanO1_WithoutErrorCheck((int)GetDstMasu_WithoutErrorCheck(ss));
            // if (Sasite.Toryo == ss) { return Conv_Masu.ERROR_DAN; } // 解析不能☆
        }
        public static void SetSrcMasu_WithoutErrorCheck(ref int ss, Masu ms_src)
        {
            // 筋と段☆（＾▽＾）盤外なら 0 なんだが、セットはせず無視だぜ☆（＾▽＾）
            ss |= (int)ms_src << (int)MoveShift.SRC_MASU;
            // if (Conv_Masu.IsBanjo(ms_src))
        }
        public static void SetDstMasu_WithoutErrorCheck(ref int ss, Masu ms_dst)
        {
            ss |= (int)ms_dst << (int)MoveShift.DST_MASU;
        }


        /// <summary>
        /// 盤上の駒を指したぜ☆（＾▽＾）（打つ以外の指し手☆）
        /// 
        /// 指し手に、取った駒を記録するのは止めるぜ☆（＾～＾）局面データの方に入れておこう☆（＾▽＾）
        /// </summary>
        /// <param name="ms_src"></param>
        /// <param name="ms_dst"></param>
        /// <param name="natta"></param>
        /// <returns></returns>
        public static Move ToSasite_01a_NarazuSasi(Masu ms_src, Masu ms_dst)
        {
#if DEBUG
            Debug.Assert(Conv_Masu.IsBanjoOrError(ms_src), "ms_src=["+ ms_src + "] kymS.masuYososu=[" + PureSettei.banHeimen + "]");
            Debug.Assert(Conv_Masu.IsBanjo(ms_dst), "盤外に指したぜ☆？");
#endif

            // バリュー
            int v = 0;

            // 筋と段☆（＾▽＾）盤外なら 0 だぜ☆（＾▽＾）
            SetSrcMasu_WithoutErrorCheck(ref v, ms_src);

            // 「打」のときは何もしないぜ☆（＾▽＾）

            SetDstMasu_WithoutErrorCheck(ref v, ms_dst);

            // 打った駒なし

            // 成らない☆（＾▽＾）

            return (Move)v;
        }
        /// <summary>
        /// 盤上の駒を指したぜ☆（＾▽＾）（打つ以外の指し手☆）
        /// 
        /// 指し手に、取った駒を記録するのは止めるぜ☆（＾～＾）局面データの方に入れておこう☆（＾▽＾）
        /// </summary>
        /// <param name="ms_src"></param>
        /// <param name="ms_dst"></param>
        /// <param name="natta"></param>
        /// <returns></returns>
        public static Move ToSasite_01b_NariSasi(Masu ms_src, Masu ms_dst)
        {
#if DEBUG
            Debug.Assert(Conv_Masu.IsBanjoOrError(ms_src), "");
            Debug.Assert(Conv_Masu.IsBanjo(ms_dst), "盤外に指したぜ☆？");
#endif

            // バリュー
            int v = 0;

            // 筋と段☆（＾▽＾）盤外なら 0 だぜ☆（＾▽＾）
            SetSrcMasu_WithoutErrorCheck(ref v, ms_src);

            // 「打」のときは何もしないぜ☆（＾▽＾）

            SetDstMasu_WithoutErrorCheck(ref v, ms_dst);

            // 打った駒なし

            // 成った☆（＾▽＾）
            v |= 1 << MoveShift.NATTA;

            return (Move)v;
        }
        /// <summary>
        /// 駒を打った指し手☆（＾▽＾）
        /// 空き升に打ち込む前提だぜ☆（＾～＾）！
        /// </summary>
        /// <param name="ms_dst"></param>
        /// <param name="mkUtta"></param>
        /// <param name="natta"></param>
        /// <returns></returns>
        public static Move ToSasite_01c_Utta(Masu ms_dst, MotigomaSyurui mkUtta)
        {
            Debug.Assert(MotigomaSyurui.Yososu != mkUtta,"");

            // バリュー
            int v = 0;

            // 元筋と元段☆（＾▽＾）「打」のときは何もしないぜ☆（＾▽＾）

            // 先筋と先段☆（＾▽＾）
            Conv_Sasite.SetDstMasu_WithoutErrorCheck(ref v, ms_dst);


            //必ず指定されているはず☆ if (MotiKomasyurui.Yososu != mkUtta)
            {
                // 変換（列挙型→ビット）
                // ぞう 0 → 1
                // きりん 1 → 2
                // ひよこ 2 → 3
                // ～中略～
                // いのしし 6 → 7
                // なし 7 → 0
                // 1 足して 8 で割った余り☆
                v |= (((int)mkUtta+1)% Conv_MotigomaSyurui.SETS_LENGTH) << (int)MoveShift.UTTA_KOMASYURUI;
            }

            // 打ったときは成れないぜ☆（＾▽＾）

            return (Move)v;
        }

        public static bool IsNatta(Move ss)
        {
            if (Move.Toryo == ss) { return false; }//解析不能☆

            int v = (int)ss;              // バリュー

            // 成ったか☆
            int natta;
            {
                int m = (int)MoveMask.NATTA;
                int s = (int)MoveShift.NATTA;
                natta = (v & m) >> s;
            }

            //────────────────────────────────────────
            // 組み立てフェーズ
            //────────────────────────────────────────

            return 0 != natta;
        }

        public static MotigomaSyurui GetUttaKomasyurui(Move ss)
        {
            if (Move.Toryo == ss) { return MotigomaSyurui.Yososu; }//解析不能☆

            // 式の形
            // (v & m) >> s;
            // v:バリュー、m:マスク、s:シフト☆
            int kirinuki = (((int)ss) & MoveMask.UTTA_KOMASYURUI) >> MoveShift.UTTA_KOMASYURUI;

            // 「なし」を 0 にするか、7 にするかの違いで変換している☆（＾～＾）
            // 打った駒の種類と数値変換（ビット→列挙型）
            // 000: なし 0 → 7
            // 001: ぞう 1 → 0
            // 010: きりん 2 → 1
            // 011: ひよこ 3 → 2
            // ～中略～
            // 111: いのしし 7→6
            return (MotigomaSyurui)(
                // 全体を1減らして、元の0を7に持っていきたいので、７足して８で割った余りにするぜ☆（＾▽＾）
                (kirinuki + Conv_MotigomaSyurui.itiran.Length) % Conv_MotigomaSyurui.SETS_LENGTH
                );
        }
        public static bool IsUtta(Move ss)
        {
            // 打か☆？
            return MotigomaSyurui.Yososu != Conv_Sasite.GetUttaKomasyurui(ss);//指定があれば
        }
    }

    public abstract class Conv_SasiteCharacter
    {
        public static readonly MoveCharacter[] items = new MoveCharacter[] {
            // enum の配列順にすること。
            MoveCharacter.HyokatiYusen,
            MoveCharacter.SyorituYusen,
            MoveCharacter.SyorituNomi,
            MoveCharacter.SinteYusen,
            MoveCharacter.SinteNomi,
            MoveCharacter.TansakuNomi,
        };
    }
}
