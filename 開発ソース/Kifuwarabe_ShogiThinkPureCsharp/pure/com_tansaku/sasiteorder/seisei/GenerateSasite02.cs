#if DEBUG
using kifuwarabe_shogithink.pure.accessor;
using kifuwarabe_shogithink.pure.ikkyoku;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.ky.bb;
using kifuwarabe_shogithink.pure.sasite;
using System.Diagnostics;
#else
using kifuwarabe_shogithink.pure.accessor;
using kifuwarabe_shogithink.pure.ikkyoku;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.ky.bb;
using kifuwarabe_shogithink.pure.sasite;
using System.Diagnostics;
#endif

namespace kifuwarabe_shogithink.pure.com.sasiteorder.seisei
{

    /// <summary>
    /// 指し手生成だぜ☆（＾～＾）
    /// 
    /// 優先度
    /// （１）らいおんキャッチ☆
    /// </summary>
    public abstract class GenerateSasite02
    {
        #region 逼迫返討手
        // らいおん　逼迫返討手
        public static void GenerateRaion_HippakuKaeriutiTe()
        {
            if (PureMemory.hot_isNigerarenaiCheckerAr[PureMemory.kifu_nTeban])
            {
                // 逃げられない状況で、王手が掛かけられていれば☆（＾～＾）

                // らいおん　が自分から利きに飛び込むのを防ぐぜ☆（＾▽＾）ｗｗｗ
                GenerateSasite03.KesuRaionJisatusyu();

                while (PureMemory.ssss_bbVar_idosaki_narazu.Ref_PopNTZ(out PureMemory.ssss_ugoki_ms_dst))
                {
                    PureMemory.SetSsssGenk(
                        Tume1Hantei.CheckBegin_Tume1_BanjoKoma(),// 一手詰めルーチン☆
                        false,
                        false
                    );

                    // チェッカーを取るような全ての手は、選択肢に入れるぜ☆（＾～＾）
                    SasiteSeiseiAccessor.AddSasite_NarazuGood();

                    if (Tume1Hantei.CheckEnd_Tume1()) { break; }//終了☆
                }
            }
        }
        // 成れる駒（Nareru Koma）　逼迫返討手
        public static void GenerateNk_HippakuKaeriutiTe()
        {
            if (PureMemory.hot_isNigerarenaiCheckerAr[PureMemory.kifu_nTeban])
            {
                // 逃げられない状況で、王手が掛かけられていれば☆（＾～＾）

                // 成れる場合
                while (PureMemory.ssss_bbVar_idosaki_nari.Ref_PopNTZ(out PureMemory.ssss_ugoki_ms_dst))
                {
                    PureMemory.SetSsssGenk(
                        Tume1Hantei.CheckBegin_Tume1_BanjoKoma(),// 一手詰めルーチン☆
                        false,
                        false
                    );

                    // チェッカーを取るような全ての手は、選択肢に入れるぜ☆（＾～＾）
                    SasiteSeiseiAccessor.AddSasite_NariGood();

                    if (Tume1Hantei.CheckEnd_Tume1()) { break; }//終了☆
                }

                while (PureMemory.ssss_bbVar_idosaki_narazu.Ref_PopNTZ(out PureMemory.ssss_ugoki_ms_dst))
                {
                    PureMemory.SetSsssGenk(
                        Tume1Hantei.CheckBegin_Tume1_BanjoKoma(),// 一手詰めルーチン☆
                        false,
                        false
                    );

                    // チェッカーを取るような全ての手は、選択肢に入れるぜ☆（＾～＾）
                    SasiteSeiseiAccessor.AddSasite_NarazuGood();

                    if (Tume1Hantei.CheckEnd_Tume1()) { break; }//終了☆
                }
            }
        }
        // 成れない駒（X Koma）　逼迫返討手
        public static void GenerateXk_HippakuKaeriutiTe()
        {
            if (PureMemory.hot_isNigerarenaiCheckerAr[PureMemory.kifu_nTeban])
            {
                // 逃げられない状況で、王手が掛かけられていれば☆（＾～＾）

                while (PureMemory.ssss_bbVar_idosaki_narazu.Ref_PopNTZ(out PureMemory.ssss_ugoki_ms_dst))
                {
                    PureMemory.SetSsssGenk(
                        Tume1Hantei.CheckBegin_Tume1_BanjoKoma(),// 一手詰めルーチン☆
                        false,
                        false
                    );

                    // チェッカーを取るような全ての手は、選択肢に入れるぜ☆（＾～＾）
                    SasiteSeiseiAccessor.AddSasite_NarazuGood();

                    if (Tume1Hantei.CheckEnd_Tume1()) { break; }//終了☆
                }
            }
        }
        #endregion

        #region 余裕返討手
        // らいおん　余裕返討手
        public static void GenerateRaion_YoyuKaeriutiTe()
        {
            if (!PureMemory.hot_isNigerarenaiCheckerAr[PureMemory.kifu_nTeban])
            {
                // 逃げられない状況で、王手が掛かけられている……、というわけでもなければ☆（＾～＾）

                // らいおん　が自分から　相手の利きに飛び込むのを防ぐぜ☆（＾▽＾）ｗｗｗ
                GenerateSasite03.KesuRaionJisatusyu();

                while (PureMemory.ssss_bbVar_idosaki_narazu.Ref_PopNTZ(out PureMemory.ssss_ugoki_ms_dst))
                {
                    PureMemory.SetSsssGenk(
                        Tume1Hantei.CheckBegin_Tume1_BanjoKoma(),// 一手詰めルーチン☆
                        false,
                        false
                    );

                    // チェッカーを取るような全ての手は、選択肢に入れるぜ☆（＾～＾）
                    SasiteSeiseiAccessor.AddSasite_NarazuGood();

                    if (Tume1Hantei.CheckEnd_Tume1()) { break; }//終了☆
                }
            }
        }
        // 成れる駒（Nareru Koma）　余裕返討手
        public static void GenerateNk_YoyuKaeriutiTe()
        {
            if (!PureMemory.hot_isNigerarenaiCheckerAr[PureMemory.kifu_nTeban])
            {
                // 逃げられない状況で、王手が掛かけられている……、というわけでもなければ☆（＾～＾）

                // 成れる場合
                while (PureMemory.ssss_bbVar_idosaki_nari.Ref_PopNTZ(out PureMemory.ssss_ugoki_ms_dst))
                {
                    PureMemory.SetSsssGenk(
                        Tume1Hantei.CheckBegin_Tume1_BanjoKoma(),// 一手詰めルーチン☆
                        false,
                        false
                    );

                    // チェッカーを取るような全ての手は、選択肢に入れるぜ☆（＾～＾）
                    SasiteSeiseiAccessor.AddSasite_NariGood();

                    if (Tume1Hantei.CheckEnd_Tume1()) { break; }//終了☆
                }

                while (PureMemory.ssss_bbVar_idosaki_narazu.Ref_PopNTZ(out PureMemory.ssss_ugoki_ms_dst))
                {
                    PureMemory.SetSsssGenk(
                        Tume1Hantei.CheckBegin_Tume1_BanjoKoma(),// 一手詰めルーチン☆
                        false,
                        false
                    );

                    // チェッカーを取るような全ての手は、選択肢に入れるぜ☆（＾～＾）
                    SasiteSeiseiAccessor.AddSasite_NarazuGood();

                    if (Tume1Hantei.CheckEnd_Tume1()) { break; }//終了☆
                }
            }
        }
        // 成れない駒（X Koma）　余裕返討手
        public static void GenerateXk_YoyuKaeriutiTe()
        {
            if (!PureMemory.hot_isNigerarenaiCheckerAr[PureMemory.kifu_nTeban])
            {
                // 逃げられない状況で、王手が掛かけられている……、というわけでもなければ☆（＾～＾）

                while (PureMemory.ssss_bbVar_idosaki_narazu.Ref_PopNTZ(out PureMemory.ssss_ugoki_ms_dst))
                {
                    PureMemory.SetSsssGenk(
                        Tume1Hantei.CheckBegin_Tume1_BanjoKoma(),// 一手詰めルーチン☆
                        false,
                        false
                    );

                    // チェッカーを取るような全ての手は、選択肢に入れるぜ☆（＾～＾）
                    SasiteSeiseiAccessor.AddSasite_NarazuGood();

                    if (Tume1Hantei.CheckEnd_Tume1()) { break; }//終了☆
                }
            }
        }
        #endregion

        #region らいおんキャッチ調査　｜　らいおんキャッチ
        // 成れる駒（Nareru Koma）　らいおんキャッチ調査　｜　らいおんキャッチ
        public static void GenerateNk_RaionCatch()
        {
            // 成れる場合
            if (PureMemory.ssss_bbVar_idosaki_nari.GetNTZ(out PureMemory.ssss_ugoki_ms_dst))
            {
                // らいおんをキャッチする手は、１つ見つければＯＫだぜ☆（＾～＾）

                if (SasiteType.N17_RaionCatchChosa == PureMemory.ssss_ugoki_kakuteiSsType)
                {
                    // 調査するだけ☆　らいおんキャッチできることが分かったので終了☆（＾～＾）
                    PureMemory.hot_raionCatchChosaAr[PureMemory.kifu_nTeban] = true;
                    return;
                }

                SasiteSeiseiAccessor.AddSasite_NariGood();
                PureMemory.SetTansakuUtikiri(TansakuUtikiri.RaionTukamaeta);
            }

            if (PureMemory.ssss_bbVar_idosaki_narazu.GetNTZ(out PureMemory.ssss_ugoki_ms_dst))
            {
                // らいおんをキャッチする手は、１つ見つければＯＫだぜ☆（＾～＾）

                if (SasiteType.N17_RaionCatchChosa == PureMemory.ssss_ugoki_kakuteiSsType)
                {
                    // 調査するだけ☆　らいおんキャッチできることが分かったので終了☆（＾～＾）
                    PureMemory.hot_raionCatchChosaAr[PureMemory.kifu_nTeban] = true;
                    return;
                }

                PureMemory.SetSsssGenk(
                    false,
                    false,
                    NigemitiWatasuKansyu.IsNigemitiWoAkeru()
                    );
                SasiteSeiseiAccessor.AddSasite_NarazuGoodXorBad();
                PureMemory.SetTansakuUtikiri(TansakuUtikiri.RaionTukamaeta);
            }
        }

        // らいおん、成れない駒（X Koma）　らいおんキャッチ調査　｜　らいおんキャッチ
        public static void GenerateRaionXk_RaionCatch()
        {
            // らいおん が、らいおんキャッチするために、利きに飛び込むのは有り☆（＾～＾）！

            if (PureMemory.ssss_bbVar_idosaki_narazu.GetNTZ(out PureMemory.ssss_ugoki_ms_dst))
            {
                // らいおんをキャッチする手は、１つ見つければＯＫだぜ☆（＾～＾）

                if (SasiteType.N17_RaionCatchChosa == PureMemory.ssss_ugoki_kakuteiSsType)
                {
                    // 調査するだけ☆　らいおんキャッチできることが分かったので終了☆（＾～＾）
                    PureMemory.hot_raionCatchChosaAr[PureMemory.kifu_nTeban] = true;
                    return;
                }

                PureMemory.SetSsssGenk(
                    false,
                    false,
                    NigemitiWatasuKansyu.IsNigemitiWoAkeru()
                    );
                SasiteSeiseiAccessor.AddSasite_NarazuGoodXorBad();
                PureMemory.SetTansakuUtikiri(TansakuUtikiri.RaionTukamaeta);
            }
        }
        #endregion

        #region トライ
        // らいおん　トライ
        public static void GenerateRaion_Try()
        {
            // らいおん　が自分から　相手の利きに飛び込むのを防ぐぜ☆（＾▽＾）ｗｗｗ
            GenerateSasite03.KesuRaionJisatusyu();

            Pure.Sc.Push("トライ", PureMemory.tnsk_hyoji);
            Bitboard trysakiBB = Util_TryRule.GetTrySaki(PureMemory.ssss_bbVar_idosaki_narazu, PureMemory.ssss_ugoki_ms_src);
            Pure.Sc.Pop();

            if (trysakiBB.GetNTZ(out PureMemory.ssss_ugoki_ms_dst))// トライはどこか１つ行けばいい
            {
                SasiteSeiseiAccessor.AddSasite_NarazuGood();
                PureMemory.SetTansakuUtikiri(TansakuUtikiri.Try);
            }
        }
        #endregion

        #region 駒を取る手
        // らいおん　駒を取る手
        public static void GenerateRaion_KomaWoToruTe()
        {
            // トライ　は除外するぜ☆（＾▽＾）
            GenerateSasite03.KesuTry();

            // らいおん　が自分から　相手の利きに飛び込むのを防ぐぜ☆（＾▽＾）ｗｗｗ
            GenerateSasite03.KesuRaionJisatusyu();

            while (PureMemory.ssss_bbVar_idosaki_narazu.Ref_PopNTZ(out PureMemory.ssss_ugoki_ms_dst))
            {
                // 駒を取るような全ての手は、選択肢に入れるぜ☆（＾～＾）

                PureMemory.SetSsssGenk(
                    Tume1Hantei.CheckBegin_Tume1_BanjoKoma(),// 一手詰めルーチン☆
                    false,
                    NigemitiWatasuKansyu.IsNigemitiWoAkeru()
                    );

                SasiteSeiseiAccessor.AddSasite_NarazuGoodXorBad();

                if (Tume1Hantei.CheckEnd_Tume1()) { break; }//終了☆
            }
        }
        // 成れる駒（Nareru Koma）　駒を取る手（Good 逃げ道を開ける手、Bad 逃げ道を開けない手）
        public static void GenerateNk_KomaWoToruTe()
        {
            // 成れる場合
            while (PureMemory.ssss_bbVar_idosaki_nari.Ref_PopNTZ(out PureMemory.ssss_ugoki_ms_dst))
            {
                // 駒を取るような全ての手は、選択肢に入れるぜ☆（＾～＾）

                PureMemory.SetSsssGenk(
                    Tume1Hantei.CheckBegin_Tume1_BanjoKoma(),// 一手詰めルーチン☆
                    false,
                    NigemitiWatasuKansyu.IsNigemitiWoAkeru()
                    );

                SasiteSeiseiAccessor.AddSasite_NariGoodXorBad();

                if (Tume1Hantei.CheckEnd_Tume1()) { break; }//終了☆
            }

            while (PureMemory.ssss_bbVar_idosaki_narazu.Ref_PopNTZ(out PureMemory.ssss_ugoki_ms_dst))
            {
                // 駒を取るような全ての手は、選択肢に入れるぜ☆（＾～＾）

                PureMemory.SetSsssGenk(
                    Tume1Hantei.CheckBegin_Tume1_BanjoKoma(),// 一手詰めルーチン☆
                    false,
                    NigemitiWatasuKansyu.IsNigemitiWoAkeru()
                    );

                SasiteSeiseiAccessor.AddSasite_NarazuGoodXorBad();

                if (Tume1Hantei.CheckEnd_Tume1()) { break; }//終了☆
            }
        }
        // らいおん、成れない駒（X Koma）　駒を取る手
        public static void GenerateXk_KomaWoToruTe()
        {
            while (PureMemory.ssss_bbVar_idosaki_narazu.Ref_PopNTZ(out PureMemory.ssss_ugoki_ms_dst))
            {
                // 駒を取るような全ての手は、選択肢に入れるぜ☆（＾～＾）

                PureMemory.SetSsssGenk(
                    Tume1Hantei.CheckBegin_Tume1_BanjoKoma(),// 一手詰めルーチン☆
                    false,
                    NigemitiWatasuKansyu.IsNigemitiWoAkeru()
                    );

                SasiteSeiseiAccessor.AddSasite_NarazuGoodXorBad();

                if (Tume1Hantei.CheckEnd_Tume1()) { break; }//終了☆
            }
        }
        #endregion

        #region ぼっち緩慢指、紐付き緩慢指
        // 仲間を見捨てる動きは Badへ☆（＾～＾）
        // らいおん　ぼっち緩慢指　｜　紐付き緩慢指　（らいおんは　捨て緩慢指し　をやらないぜ☆）
        public static void GenerateRaion_BottiKanmanZasi_HimodukiKanmanZasi()
        {
            // トライ　は除外するぜ☆（＾▽＾）
            GenerateSasite03.KesuTry();
            // らいおん　が自分から　相手の利きに飛び込むのを防ぐぜ☆（＾▽＾）ｗｗｗ
            GenerateSasite03.KesuRaionJisatusyu();

            while (PureMemory.ssss_bbVar_idosaki_narazu.Ref_PopNTZ(out PureMemory.ssss_ugoki_ms_dst))// 立っているビットを降ろすぜ☆
            {
                PureMemory.SetSsssGenk(
                    Tume1Hantei.CheckBegin_Tume1_BanjoKoma(),// 一手詰めルーチン☆
                    GenerateSasite03.IsMisuteruUgoki(),
                    false//逃げ道を開けて逃がすかどうかは判定しないぜ☆（＾～＾）
                    );

                SasiteSeiseiAccessor.AddSasite_NarazuGoodXorBad();

                if (Tume1Hantei.CheckEnd_Tume1()) { break; }//終了☆
            }
        }
        //ぼっち緩慢指　｜　紐付き緩慢指し
        public static void Generate02ZouKirinNado_bottiKanmanZasi_himodukiKanmanZasi()
        {
            // 王手も除外するぜ☆（＾▽＾）
            GenerateSasite03.KesuOte();

            while (PureMemory.ssss_bbVar_idosaki_narazu.Ref_PopNTZ(out PureMemory.ssss_ugoki_ms_dst))// 立っているビットを降ろすぜ☆
            {
                // タダ捨てではない動きであることを判定するぜ☆（＾～＾）
                if (!GenerateSasite03.TadasuteNoUgoki())
                {
                    PureMemory.SetSsssGenk(
                        Tume1Hantei.CheckBegin_Tume1_BanjoKoma(),// 一手詰めルーチン☆
                        GenerateSasite03.IsMisuteruUgoki(),
                        false//逃げ道を開けて逃がすかどうかは判定しないぜ☆（＾～＾）
                        );

                    // 成らずの指しをまずリストへ☆（＾～＾）
                    SasiteSeiseiAccessor.AddSasite_NarazuGoodXorBad();

                    if (Tume1Hantei.CheckEnd_Tume1()) { break; }//終了☆
                }
            }
        }
        // ぼっち緩慢指　｜　紐付緩慢指
        public static void Generate02HiyokoNado_BottiKanmanZasi_HimodukiKanmanZasi()
        {
            // 王手も除外するぜ☆（＾▽＾）
            GenerateSasite03.KesuOte();

            // 成れる場合
            while (PureMemory.ssss_bbVar_idosaki_nari.Ref_PopNTZ(out PureMemory.ssss_ugoki_ms_dst))// 立っているビットを降ろすぜ☆
            {
                if (!GenerateSasite03.TadasuteNoUgoki())// タダ捨てではない動きに限るぜ☆（＾▽＾）
                {
                    PureMemory.SetSsssGenk(
                        Tume1Hantei.CheckBegin_Tume1_BanjoKoma(),// 一手詰めルーチン☆
                        GenerateSasite03.IsMisuteruUgoki(),
                        false//逃げ道を開けて逃がすかどうかは判定しないぜ☆（＾～＾）
                    );

                    SasiteSeiseiAccessor.AddSasite_NariGood();

                    if (Tume1Hantei.CheckEnd_Tume1()) { break; }//終了☆
                }
            }

            while (PureMemory.ssss_bbVar_idosaki_narazu.Ref_PopNTZ(out PureMemory.ssss_ugoki_ms_dst))// 立っているビットを降ろすぜ☆
            {
                if (!GenerateSasite03.TadasuteNoUgoki())// タダ捨てではない動きに限るぜ☆（＾▽＾）
                {
                    // 一手詰めルーチン☆
                    PureMemory.SetSsssGenk(
                        Tume1Hantei.CheckBegin_Tume1_BanjoKoma(),
                        GenerateSasite03.IsMisuteruUgoki(),
                        false//逃げ道を開けて逃がすかどうかは判定しないぜ☆（＾～＾）
                    );

                    SasiteSeiseiAccessor.AddSasite_NarazuGoodXorBad();

                    if (Tume1Hantei.CheckEnd_Tume1()) { break; }//終了☆
                }
            }
        }
        // ぼっち緩慢指　｜　紐付緩慢指
        public static void Generate02NiwatoriNado_BottiKanmanZasi_HimodukiKanmanZasi()
        {
            // 王手も除外するぜ☆（＾▽＾）
            GenerateSasite03.KesuOte();

            while (PureMemory.ssss_bbVar_idosaki_narazu.Ref_PopNTZ(out PureMemory.ssss_ugoki_ms_dst))// 立っているビットを降ろすぜ☆
            {
                if (!GenerateSasite03.TadasuteNoUgoki())// タダ捨てではない動きに限るぜ☆（＾▽＾）
                {
                    // 一手詰めルーチン☆
                    PureMemory.SetSsssGenk(
                        Tume1Hantei.CheckBegin_Tume1_BanjoKoma(),
                        GenerateSasite03.IsMisuteruUgoki(),
                        false//逃げ道を開けて逃がすかどうかは判定しないぜ☆（＾～＾）
                        );

                    SasiteSeiseiAccessor.AddSasite_NarazuGoodXorBad();

                    if (Tume1Hantei.CheckEnd_Tume1()) { break; }//終了☆
                }
            }
        }
        #endregion

        #region 紐付き王手指（Good 逃げ道を開けない手、Bad 逃げ道を開ける手）（らいおんは王手しないぜ☆（＾～＾））
        // 成れる駒（Nareru Koma）　紐付王手指
        public static void GenerateNk_HimodukiOteZasi()
        {
            // 王手だけに絞り込むぜ☆（＾～＾）
            GenerateSasite03.SiborikomiOte();
            // 紐を付ける手に絞り込むぜ☆（＾～＾）
            GenerateSasite03.SiborikomiHimoduke();

            // 成れる場合
            while (PureMemory.ssss_bbVar_idosaki_nari.Ref_PopNTZ(out PureMemory.ssss_ugoki_ms_dst))// 立っているビットを降ろすぜ☆
            {
                if (!GenerateSasite03.TadasuteNoUgoki())// タダ捨てではない動きに限る☆
                {
                    PureMemory.SetSsssGenk(
                        Tume1Hantei.CheckBegin_Tume1_BanjoKoma(),// 一手詰めルーチン☆
                        false,
                        NigemitiWatasuKansyu.IsNigemitiWoAkeru()
                        );

                    SasiteSeiseiAccessor.AddSasite_NariGoodXorBad();

                    if (Tume1Hantei.CheckEnd_Tume1()) { break; }//終了☆
                }
            }

            while (PureMemory.ssss_bbVar_idosaki_narazu.Ref_PopNTZ(out PureMemory.ssss_ugoki_ms_dst))// 立っているビットを降ろすぜ☆
            {
                if (!GenerateSasite03.TadasuteNoUgoki())// タダ捨てではない動きに限る☆
                {
                    PureMemory.SetSsssGenk(
                        Tume1Hantei.CheckBegin_Tume1_BanjoKoma(),// 一手詰めルーチン☆
                        false,
                        NigemitiWatasuKansyu.IsNigemitiWoAkeru()
                        );

                    SasiteSeiseiAccessor.AddSasite_NarazuGoodXorBad();

                    if (Tume1Hantei.CheckEnd_Tume1()) { break; }//終了☆
                }
            }
        }
        // 成れない駒（X Koma）　紐付王手指
        public static void GenerateXk_HimodukiOteZasi()
        {
            // 王手だけに絞り込むぜ☆（＾～＾）
            GenerateSasite03.SiborikomiOte();
            // 紐を付ける手に絞り込むぜ☆（＾～＾）
            GenerateSasite03.SiborikomiHimoduke();

            while (PureMemory.ssss_bbVar_idosaki_narazu.Ref_PopNTZ(out PureMemory.ssss_ugoki_ms_dst))// 立っているビットを降ろすぜ☆
            {
                if (!GenerateSasite03.TadasuteNoUgoki())// タダ捨てではない動きに限る☆
                {
                    PureMemory.SetSsssGenk(
                        Tume1Hantei.CheckBegin_Tume1_BanjoKoma(),// 一手詰めルーチン☆
                        false,
                        NigemitiWatasuKansyu.IsNigemitiWoAkeru()
                        );
                    SasiteSeiseiAccessor.AddSasite_NarazuGoodXorBad();

                    if (Tume1Hantei.CheckEnd_Tume1()) { break; }//終了☆
                }
            }
        }
        #endregion

        #region 捨て王手指
        // 成れる駒（Nareru Koma）　捨て王手指
        public static void GenerateNk_SuteOteZasi()
        {
            // 王手に限る☆
            GenerateSasite03.SiborikomiOte();
            // 紐を付けない☆
            GenerateSasite03.KesuHimoduke();

            // 成れる場合
            while (PureMemory.ssss_bbVar_idosaki_nari.Ref_PopNTZ(out PureMemory.ssss_ugoki_ms_dst))// 立っているビットを降ろすぜ☆
            {
                if (GenerateSasite03.TadasuteNoUgoki())// タダ捨ての動きに限る☆
                {
                    PureMemory.SetSsssGenk(
                        Tume1Hantei.CheckBegin_Tume1_BanjoKoma(),// 一手詰めルーチン☆
                        false,
                        NigemitiWatasuKansyu.IsNigemitiWoAkeru()
                        );

                    SasiteSeiseiAccessor.AddSasite_NariGoodXorBad();

                    if (Tume1Hantei.CheckEnd_Tume1()) { break; }//終了☆
                }
            }

            while (PureMemory.ssss_bbVar_idosaki_narazu.Ref_PopNTZ(out PureMemory.ssss_ugoki_ms_dst))// 立っているビットを降ろすぜ☆
            {
                if (GenerateSasite03.TadasuteNoUgoki())// タダ捨ての動きに限る☆
                {
                    PureMemory.SetSsssGenk(
                        Tume1Hantei.CheckBegin_Tume1_BanjoKoma(),// 一手詰めルーチン☆
                        false,
                        NigemitiWatasuKansyu.IsNigemitiWoAkeru()
                        );

                    SasiteSeiseiAccessor.AddSasite_NarazuGoodXorBad();

                    if (Tume1Hantei.CheckEnd_Tume1()) { break; }//終了☆
                }
            }
        }
        // 成れない駒（X Koma）　捨て王手指し
        public static void GenerateNx_SuteOteZasi()
        {
            // 王手に限る☆
            GenerateSasite03.SiborikomiOte();
            // 紐を付けない☆
            GenerateSasite03.KesuHimoduke();

            while (PureMemory.ssss_bbVar_idosaki_narazu.Ref_PopNTZ(out PureMemory.ssss_ugoki_ms_dst))// 立っているビットを降ろすぜ☆
            {
                if (GenerateSasite03.TadasuteNoUgoki())// タダ捨ての動きに限る☆
                {
                    PureMemory.SetSsssGenk(
                        Tume1Hantei.CheckBegin_Tume1_BanjoKoma(),// 一手詰めルーチン☆
                        false,
                        NigemitiWatasuKansyu.IsNigemitiWoAkeru()
                        );

                    SasiteSeiseiAccessor.AddSasite_NarazuGoodXorBad();

                    if (Tume1Hantei.CheckEnd_Tume1()) { break; }//終了☆
                }
            }
        }
        #endregion

        #region 捨て緩慢指（タダ捨て指し）
        // 成れる駒（Nareru Koma）　捨て緩慢指し（タダ捨て指し）
        public static void GenerateNk_SuteKanmanZasi()
        {
            // 王手も除外するぜ☆（＾▽＾）
            GenerateSasite03.KesuOte();

            // 成れる場合
            while (PureMemory.ssss_bbVar_idosaki_nari.Ref_PopNTZ(out PureMemory.ssss_ugoki_ms_dst))// 立っているビットを降ろすぜ☆
            {
                if (GenerateSasite03.TadasuteNoUgoki())// 相手の利きがあって、自分を除いた味方の利きがない升　に限るぜ☆（＾▽＾）ｗｗｗ
                {
                    PureMemory.SetSsssGenk(
                        false,// タダ捨てに、一手詰めは無いだろう☆（*＾～＾*）
                        GenerateSasite03.IsMisuteruUgoki(),
                        false//逃げ道を開けて逃がすかどうかは判定しないぜ☆（＾～＾）
                        );

                    SasiteSeiseiAccessor.AddSasite_NariGood();
                }
            }

            while (PureMemory.ssss_bbVar_idosaki_narazu.Ref_PopNTZ(out PureMemory.ssss_ugoki_ms_dst))// 立っているビットを降ろすぜ☆
            {
                if (GenerateSasite03.TadasuteNoUgoki())// 相手の利きがあって、自分を除いた味方の利きがない升　に限るぜ☆（＾▽＾）ｗｗｗ
                {
                    PureMemory.SetSsssGenk(
                        false,// タダ捨てに、一手詰めは無いだろう☆（*＾～＾*）
                        GenerateSasite03.IsMisuteruUgoki(),
                        false//逃げ道を開けて逃がすかどうかは判定しないぜ☆（＾～＾）
                        );

                    SasiteSeiseiAccessor.AddSasite_NarazuGoodXorBad();
                }
            }
        }
        // 成れない駒（X Koma）　捨て緩慢指し（タダ捨て指し）
        public static void GenerateXk_SuteKanmanZasi()
        {
            Debug.Assert(Conv_Koma.IsOk(PureMemory.ssss_ugoki_km), "");

            // 王手も除外するぜ☆（＾▽＾）
            GenerateSasite03.KesuOte();

            while (PureMemory.ssss_bbVar_idosaki_narazu.Ref_PopNTZ(out PureMemory.ssss_ugoki_ms_dst))// 立っているビットを降ろすぜ☆
            {
                if (GenerateSasite03.TadasuteNoUgoki())// 相手の利きがあって、自分を除いた味方の利きがない升　に限るぜ☆（＾▽＾）ｗｗｗ
                {
                    PureMemory.SetSsssGenk(
                        false,// タダ捨てに、一手詰めは無いだろう☆（*＾～＾*）
                        GenerateSasite03.IsMisuteruUgoki(),
                        false//逃げ道を開けて逃がすかどうかは判定しないぜ☆（＾～＾）
                        );

                    SasiteSeiseiAccessor.AddSasite_NarazuGoodXorBad();
                }
            }
        }
        #endregion

        #region 持ち駒を使った指し手生成
        /// <summary>
        /// グローバル変数 Util_SasiteSeisei.Sasitelist[fukasa] に、指し手が追加されていくぜ☆（＾▽＾）
        /// 紐付王手打
        /// </summary>
        /// <param name="mks"></param>
        /// <param name="mk"></param>
        /// <param name="ks"></param>
        public static void GenerateMotigoma_NormalDa()
        {
            while (PureMemory.ssss_bbVar_idosaki_narazu.Ref_PopNTZ(out PureMemory.ssss_ugoki_ms_dst))// 立っているビットを降ろすぜ☆
            {
                PureMemory.SetSsssGenk(
                    Tume1Hantei.CheckBegin_Ittedume_MotiKoma(),// 一手詰めルーチン☆
                    false,// 持ち駒を打つのに、見捨てる動きも無いだろう☆（＾～＾）
                    false // 持ち駒を打つのに、逃げ道を開けて逃がす動きも無いだろう☆（＾～＾）
                );

                SasiteSeiseiAccessor.AddSasite_UttaGood();

                if (Tume1Hantei.CheckEnd_Tume1()) { break; }//終了☆
            }
        }
        /// <summary>
        /// グローバル変数 Util_SasiteSeisei.Sasitelist[fukasa] に、指し手が追加されていくぜ☆（＾▽＾）
        /// 捨て緩慢打（タダ捨て打）
        /// </summary>
        /// <param name="mks"></param>
        /// <param name="mk"></param>
        /// <param name="ks"></param>
        public static void GenerateMotigoma_SuteDa()
        {
            while (PureMemory.ssss_bbVar_idosaki_narazu.Ref_PopNTZ(out PureMemory.ssss_ugoki_ms_dst))// 立っているビットを降ろすぜ☆
            {
                if (GenerateSasite03.TadasuteNoUgoki())// タダ捨ての動きに限る☆
                {
                    PureMemory.SetSsssGenk(
                        false,// タダ捨てに、一手詰めは無いだろう☆（*＾～＾*）
                        false,// 持ち駒を打つのに、見捨てる動きも無いだろう☆（＾～＾）
                        false // 持ち駒を打つのに、逃げ道を開けて逃がす動きも無いだろう☆（＾～＾）
                    );
                    
                    SasiteSeiseiAccessor.AddSasite_UttaGood();
                }
            }
        }
        #endregion

    }
}
