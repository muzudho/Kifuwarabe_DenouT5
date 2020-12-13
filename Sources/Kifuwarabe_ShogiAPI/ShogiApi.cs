#if DEBUG
using kifuwarabe_shogithink.pure;
using kifuwarabe_shogithink.pure.accessor;
using kifuwarabe_shogithink.pure.com;
using kifuwarabe_shogithink.pure.control;
using kifuwarabe_shogithink.pure.genkyoku;
using kifuwarabe_shogithink.pure.ikkyoku;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.listen.ky;
using kifuwarabe_shogithink.pure.listen.play;
using kifuwarabe_shogithink.pure.logger;
using kifuwarabe_shogithink.pure.med.ky;
using kifuwarabe_shogithink.pure.sasite;
using kifuwarabe_shogithink.pure.speak.genkyoku;
using kifuwarabe_shogithink.pure.speak.ky;
using kifuwarabe_shogithink.pure.speak.play;
using System;
using System.Collections.Generic;
using kifuwarabe_shogithink.pure.listen.genkyoku;
#else
using kifuwarabe_shogithink.pure;
using kifuwarabe_shogithink.pure.accessor;
using kifuwarabe_shogithink.pure.com;
using kifuwarabe_shogithink.pure.control;
using kifuwarabe_shogithink.pure.genkyoku;
using kifuwarabe_shogithink.pure.ikkyoku;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.listen.genkyoku;
using kifuwarabe_shogithink.pure.listen.play;
using kifuwarabe_shogithink.pure.logger;
using kifuwarabe_shogithink.pure.sasite;
using kifuwarabe_shogithink.pure.speak.genkyoku;
using kifuwarabe_shogithink.pure.speak.ky;
using kifuwarabe_shogithink.pure.speak.play;
using System;
using System.Collections.Generic;
#endif

namespace kifuwarabe_shogiapi
{
    /// <summary>
    /// 34将棋のサンプル・プログラムだぜ☆（＾～＾）
    /// </summary>
    public static class ShogiApi
    {
        public static void SetRule(
            GameRule gameRule,
            int banYokohaba,
            int banTatehaba,
            string banmen_z1,
            Dictionary<Motigoma, int> motigomaKaisiSettei
            )
        {
            LisGenkyoku.SetRule(
                gameRule,
                banYokohaba,
                banTatehaba,
                banmen_z1,
                motigomaKaisiSettei
            );
        }

        /// <summary>
        /// 何かエラーログが吐かれていれば取得できるぜ☆（＾～＾）
        /// </summary>
        /// <returns></returns>
        public static string ToLogString()
        {
            return PureAppli.syuturyoku1.ToContents();
        }

        /// <summary>
        /// アプリケーション起動時に１回だけ呼び出せだぜ☆（＾～＾）
        /// </summary>
        /// <returns></returns>
        public static void Init()
        {
            Interproject.project = new ShogiApiProject();

            if (PureAppli.TryFail_Init())
            {
                // エラーチェックはしません
            }
        }

        /// <summary>
        /// 設定　コンピューターの思考時間（最低限使える時間）
        /// </summary>
        /// <param name="value"></param>
        public static void SetOption_SikoJikan(int milliseconds)
        {
            ComSettei.sikoJikan = milliseconds;
        }
        /// <summary>
        /// 設定　コンピューターのランダム思考時間（追加で使える時間）
        /// </summary>
        /// <param name="value"></param>
        public static void SetOption_SikoJikanRandom(int milliseconds)
        {
            ComSettei.sikoJikanRandom = milliseconds;
        }



        /// <summary>
        /// １手作るぜ☆（＾～＾）
        /// </summary>
        public static void CreateSasite(string dfen, out Move out_sasite)
        {
            int caret = 0;
            if (!LisPlay.MatchFenSasite(PureSettei.fenSyurui, dfen, ref caret, out out_sasite))
            {
                out_sasite = Move.Toryo;//エラー
            }
        }

        public static bool CanDoSasite(Move inputSasite, out MoveMatigaiRiyu out_riyu, out string out_setumei)
        {
            // 指し手の合否チェック
            bool ret = GenkyokuOpe.CanDoSasite( inputSasite, out out_riyu);
            if (ret)
            {
                out_setumei = "";
            }
            else
            {
                // イリーガル・ムーブなどの、エラー理由表示☆（＾～＾）
                out_setumei = SpkSasite.ToSetumeiByRiyu(out_riyu);
            }
            return ret;
        }

        /// <summary>
        /// １手指すぜ☆（＾▽＾）
        /// </summary>
        public static void DoSasite(Move ss)
        {
            SasiteType ssType = SasiteType.N00_Karappo;
            if (DoSasiteOpe.TryFail_DoSasite_All( ss, ssType
#if DEBUG
                , PureSettei.fenSyurui
                , PureAppli.syuturyoku1
                , false
                , "Shogi34#DoSasite"
#endif
            ))
            {
                throw new Exception(PureAppli.syuturyoku1.ToContents());
            }
            MoveGenAccessor.AddKifu(ss, ssType,PureMemory.dmv_ks_c);
//#if DEBUG
//            Util_Tansaku.Snapshot("Shogi34", dbg_reigai);
//#endif
        }

        /// <summary>
        /// コンピューターに１手指させるぜ☆（＾～＾）
        /// エラーが無ければ、PureMemory.tnsk_kohoSasite にベストムーブが入っているぜ☆（＾～＾）
        /// </summary>
        public static void Go(out string out_sasiteFen)
        {
            // コンピューターに１手指させるぜ☆
            Util_Tansaku.PreGo();
            if (Util_Tansaku.TryFail_Go(PureAppli.syuturyoku1))
            {
                // エラーは強制終了☆（＾～＾）
                throw new Exception("Goでエラー☆（＾～＾）");
            }

            ICommandMojiretu sasite_str = new MojiretuImpl();
            SpkSasite.AppendFenTo(PureSettei.fenSyurui, PureMemory.tnsk_kohoSasite, sasite_str);
            out_sasiteFen = sasite_str.ToContents();
        }

        /// <summary>
        /// 通信データとして局面データを取得することもできるぜ☆（＾～＾）
        /// </summary>
        /// <returns></returns>
        public static string GetKyokumen_TusinYo()
        {
            IHyojiMojiretu str = new MojiretuImpl();
            SpkGenkyokuOpe.TusinYo_Line(PureSettei.fenSyurui, str);
            return str.ToContents();
        }

        /// <summary>
        /// 全角１文字データの羅列として局面データを取得することもできるぜ☆（＾～＾）
        /// </summary>
        /// <returns></returns>
        public static string GetKyokumen_Zen1()
        {
            return SpkKyokumen.ToZen1Mojiretu();
        }

        /// <summary>
        /// 盤面を駒データの配列として取得することもできるぜ☆（＾～＾）
        /// </summary>
        /// <returns></returns>
        public static Koma[] GetKyokumen_Hairetu()
        {
            Koma[] ret = new Koma[PureSettei.banHeimen];
            for (int iMs = 0; iMs < PureSettei.banHeimen; iMs++)
            {
                ret[iMs] = PureMemory.gky_ky.yomiKy.yomiShogiban.yomiIbashoBan.GetBanjoKoma((Masu)iMs);
            }
            return ret;
        }

        /// <summary>
        /// 持ち駒の枚数を取得します
        /// </summary>
        /// <param name="mg"></param>
        /// <returns></returns>
        public static int CountMotigoma(Motigoma mg)
        {
            return PureMemory.gky_ky.yomiKy.yomiMotigomaItiran.Count(mg);
        }



        public static TaikyokuKekka TaikyokuKekka
        {
            get
            {
                return PureMemory.gky_kekka;
            }
        }
    }
}
