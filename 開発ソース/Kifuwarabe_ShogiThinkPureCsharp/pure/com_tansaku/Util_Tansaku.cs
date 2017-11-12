#if DEBUG
using kifuwarabe_shogithink.pure.accessor;
using kifuwarabe_shogithink.pure.com.hyoka;
using kifuwarabe_shogithink.pure.com.sasiteorder;
using kifuwarabe_shogithink.pure.ikkyoku;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.logger;
using kifuwarabe_shogithink.pure.sasite;
using kifuwarabe_shogithink.pure.speak.ky;
using kifuwarabe_shogithink.pure.speak.play;
using System;
using System.Diagnostics;
#else
using kifuwarabe_shogithink.pure.accessor;
using kifuwarabe_shogithink.pure.com.hyoka;
using kifuwarabe_shogithink.pure.com.sasiteorder;
using kifuwarabe_shogithink.pure.ikkyoku;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.logger;
using kifuwarabe_shogithink.pure.sasite;
using System;
using System.Diagnostics;
#endif

namespace kifuwarabe_shogithink.pure.com
{
    /// <summary>
    /// 探索打ち切りフラグ☆
    /// </summary>
    public enum TansakuUtikiri
    {
        Karappo,

        /// <summary>
        /// 勝った。
        /// </summary>
        RaionTukamaeta,

        /// <summary>
        /// トライした（トライ・ルール）
        /// </summary>
        Try
    }

    /// <summary>
    /// 探索部だぜ☆（＾▽＾）
    /// </summary>
    public abstract class Util_Tansaku
    {
        /// <summary>
        /// Go する前に☆（＾～＾）
        /// </summary>
        public static void PreGo()
        {
            PureMemory.tnsk_itibanFukaiNekkoKaranoFukasa_JohoNoTameni = 0; // 読み筋情報に表示するための、読み終わった、一番深い根っこからの深さを覚えておくものだぜ☆（＾▽＾）
            // カウンターをクリアだぜ☆（＾▽＾）
            PureMemory.tnsk_tyakusyuEdas = 0;
            PureMemory.tnsk_kohoSasite = Sasite.Toryo;

            // ストップウォッチ
            ComSettei.timeManager.RestartStopwatch_Tansaku();
            ComSettei.timeManager.lastJohoTime = 0;

            // ループに入ると探索開始時にセットするんだが、最初の１回はループに入るために、何か設定しておく必要があるぜ☆（*＾～＾*）
            ComSettei.SetSikoJikan_KonkaiNoTansaku();

            PureMemory.InitTansaku();

#if DEBUG
            PureMemory.tnsk_syuryoRiyu = TansakuSyuryoRiyu.Kaisi;
#endif
        }

        static Hyokati tmp_bestHyokaSu = new Hyokati();
        static Hyokati tmp_currHyokaSu = new Hyokati();

        /// <summary>
        /// コンピューターの思考の開始だぜ☆（＾▽＾）
        /// ここが入り口だぜ☆（＾～＾）
        /// 
        /// 最善手は yomisuji[0] に入っているぜ☆（＾▽＾）
        /// </summary>
        /// <returns></returns>
        public static bool TryFail_Go(IHyojiMojiretu hyoji)
        {
            tmp_bestHyokaSu.Clear();
            if (PureMemory.gky_ky.shogiban.yomiIbashoBan_yoko.IsEmpty(Med_Koma.KomasyuruiAndTaikyokusyaToKoma(Komasyurui.R, PureMemory.kifu_teban)))
            {
                // 自分のらいおんがいない局面の場合、投了☆
#if DEBUG
                PureMemory.tnsk_syuryoRiyu = TansakuSyuryoRiyu.JibunRaionInai;
                PureMemory.tnsk_kohoSasite = Sasite.Toryo;
                tmp_bestHyokaSu.tumeSu = Conv_Tumesu.Stalemate;
#endif
            }
            else
            {
                //────────────────────────────────────────
                // 反復深化ループ☆（＾～＾）
                //────────────────────────────────────────
                Sasite currSasite = Sasite.Toryo;
                tmp_currHyokaSu.Clear();
                for (HanpukuSinka.happaenoFukasa = 1;
                    // まだ思考に時間を使っていい
                    !ComSettei.timeManager.IsTimeOver_IterationDeeping()
                    ; HanpukuSinka.happaenoFukasa++)
                {
                    Debug.Assert(0 <= HanpukuSinka.happaenoFukasa && HanpukuSinka.happaenoFukasa < PureMemory.ssss_sasitelist.Length, "");

                    if (ComSettei.saidaiFukasa < HanpukuSinka.happaenoFukasa)
                    {
                        // 最大深さを超えた場合
                        Util_Joho.JohoMatome(
                            HanpukuSinka.happaenoFukasa,
                            tmp_bestHyokaSu,
                            hyoji
#if DEBUG
                            , "SaidaiFukasaGoe"
#endif
                            );
                        break;
                    }

                    // ここでは現在の局面に盤面を戻してあると思えだぜ☆（＾～＾）

                    Debug.Assert(1 <= HanpukuSinka.happaenoFukasa && HanpukuSinka.happaenoFukasa < PureMemory.ssss_sasitelist.Length, "");
                    ComSettei.SetSikoJikan_KonkaiNoTansaku();//思考時間（ランダム込み）を確定させるぜ☆（＾～＾）

                    PureMemory.SetTnskHyoji(hyoji);
                    //カウントダウン式の数字☆（＾▽＾） 反復深化探索の１週目は 1、２週目は 2 だぜ☆（＾▽＾）
                    PureMemory.SetTnskFukasa(HanpukuSinka.happaenoFukasa);
                    Tansaku_(
                        out currSasite,
                        out tmp_currHyokaSu// 相手番の指し手の評価値が入ってくるぜ☆（＾～＾）
                    );

                    // TODO: １手も読めていなければ、さっさと投了したいぜ☆（＾～＾）

                    if (tmp_currHyokaSu.isHaki)
                    {
                        // 時間切れ等の中途半端探索のとき☆
                        // この計算結果は、無視するぜ☆（＾～＾）

                        // ここに来るときは探索終了だぜ☆（＾～＾）
                        break;// 読みを終了しようなんだぜ☆
                    }
                    else
                    {
                        // 更新☆（＾▽＾）

                        PureMemory.tnsk_itibanFukaiNekkoKaranoFukasa_JohoNoTameni = HanpukuSinka.happaenoFukasa;
                        PureMemory.tnsk_kohoSasite = currSasite;
                        tmp_bestHyokaSu.ToSet(tmp_currHyokaSu);

                        if (Conv_Tumesu.CatchRaion == tmp_bestHyokaSu.tumeSu)
                        {
                            // 「０手詰められ」が返ってきているなら、負けました、をいう場面だぜ☆
#if DEBUG
                            PureMemory.tnsk_syuryoRiyu = TansakuSyuryoRiyu.Minus2TeTumerare;
#endif
                            break;// 読みを終了しようなんだぜ☆
                        }
                    }

                }//ループ

                // ストップウォッチ
                ComSettei.timeManager.stopwatch_Tansaku.Stop();
            }

            //────────────────────────────────────────
            // 詰め、詰められ
            //────────────────────────────────────────
            {
                Util_Taikyoku.Update(
                    tmp_bestHyokaSu,
                    PureMemory.kifu_teban
                    );
            }

            //────────────────────────────────────────
            // 指し手は決まった☆（＾～＾）
            // 指して、局面を進めておくぜ☆（＾～＾）
            //────────────────────────────────────────
            // 何これ
            if (DoSasiteOpe.TryFail_DoSasite_All(
                PureMemory.tnsk_kohoSasite,
                SasiteType.N00_Karappo
#if DEBUG
                , PureSettei.fenSyurui
                , (IDebugMojiretu)hyoji
                , true//アサート抑制
                , "TryFail_Go(1)"
#endif
                ))
            {
                return Pure.FailTrue("GenkyokuOpe.Try_DoSasite(1)");
            }
            // 手番を進めるぜ☆（＾～＾）
            SasiteSeiseiAccessor.AddKifu(PureMemory.tnsk_kohoSasite, SasiteType.N00_Karappo, PureMemory.dmv_ks_c);
#if DEBUG
            Util_Tansaku.Snapshot("Go(1)確定指し", PureMemory.tnsk_kohoSasite);
#endif


            // 指し手が決まったときにも、強制情報表示
            {
                if (0 == PureMemory.tnsk_itibanFukaiNekkoKaranoFukasa_JohoNoTameni)
                {
#if DEBUG
                        hyoji.AppendLine(
                            string.Format(
                                "0手投了してないかだぜ☆？（＾～＾）\n"+
                            " tansakuSyuryoRiyu=[{0}]\n" +
                            "Option_Application.Optionlist.SaidaiFukasa=[{1}]\n" +
                            "Option_Application.Optionlist.SikoJikan_KonkaiNoTansaku=[{2}]\n" +
                            "Option_Application.Optionlist.SikoJikan=[{3}]\n" +
                            "Option_Application.Optionlist.SikoJikanRandom=[{4}]\n" +
                                "",
                                PureMemory.tnsk_syuryoRiyu,
                                ComSettei.saidaiFukasa,
                                ComSettei.sikoJikan_KonkaiNoTansaku,
                                ComSettei.sikoJikan,
                                ComSettei.sikoJikanRandom
                                )
                            );
                        return Pure.FailTrue("0手投了");
#endif
                }
            }

#if DEBUG
            hyoji.AppendLine(string.Format("bestSasite: [{0}] ss={1}",
                PureMemory.tnsk_kaisiTeme,
                SpkSasite.ToString_Fen(PureSettei.fenSyurui, PureMemory.tnsk_kohoSasite)
                ));
#endif
            return Pure.SUCCESSFUL_FALSE;
        }

#if DEBUG
        public static void Snapshot(string header, Sasite bestSasite)
        {
            PureMemory.tnsk_hyoji.AppendLine(header);
            PureMemory.tnsk_hyoji.AppendLine(string.Format("fukasa={0} bestSasite={1} / kohoSasite={2}",
                PureMemory.tnsk_fukasa, bestSasite, PureMemory.tnsk_kohoSasite));
            // 局面表示
            PureMemory.tnsk_hyoji.AppendLine(SpkKyokumen.ToZen1Mojiretu());
            // その他の情報
            Interproject.project.SnapshotTansaku(PureMemory.tnsk_hyoji);
        }
#endif

        /// <summary>
        /// 探索だぜ☆（＾▽＾）
        /// </summary>
        /// <param name="ky"></param>
        /// <param name="alpha"></param>
        /// <param name="fukasa">カウントダウン式の数字☆（＾▽＾） 反復深化探索の１週目の初期値は 1、２週目の初期値は 2 だぜ☆（＾▽＾）
        /// これがどんどんカウントダウンしていくぜ☆（＾▽＾） 0 で呼び出されたときは葉にしてすぐ処理を終われよ☆（＾▽＾）ｗｗｗ</param>
        /// <param name="out_yomisujiToBack"></param>
        /// <param name="out_bestHyokatiAb1">手番から見た指し手の評価値だぜ☆（＾～＾）</param>
        /// <param name="out_edaBest_Komawari_JohoNoTame">内訳の目視確認用に使うだけの項目。</param>
        /// <param name="out_edaBest__Okimari_JohoNoTame">内訳の目視確認用に使うだけの項目。</param>
        /// <param name="out_edaBest_____Riyu_JohoNoTame">内訳の目視確認用に使うだけの項目。</param>
        /// <param name="dlgt_CreateJoho"></param>
        /// <returns></returns>
        private static void Tansaku_(
            out Sasite out_bestSasite,
            out Hyokati out_bestHyokasu // 手番側の指し手の評価値だぜ☆（＾～＾）
            )
        {
            Debug.Assert(0 <= PureMemory.tnsk_fukasa && PureMemory.tnsk_fukasa < PureMemory.ssss_sasitelist.Length, "");

            out_bestSasite = Sasite.Toryo;
            out_bestHyokasu = null;

            //────────────────────────────────────────
            // 時間切れ判定
            //────────────────────────────────────────
            if (ComSettei.timeManager.IsTimeOver_TansakuChu())
            {
                out_bestSasite = Sasite.Toryo;
                out_bestHyokasu = new Hyokati(
                    Conv_Hyokati.Hyokati_Rei,
                    Conv_Tumesu.Stalemate,
                    true // 今回の探索の結果は破棄するぜ☆（＾～＾）
#if DEBUG
                    , Conv_Hyokati.Hyokati_Rei
                    , Conv_Hyokati.Hyokati_Rei
                    , Conv_Hyokati.Hyokati_Rei
                    , HyokaRiyu.JikanGire
                    , ""
#endif
                    );
#if DEBUG
                Util_Tansaku.Snapshot("時間切れだぜ☆（＾～＾）", out_bestSasite);
#endif
                return;
            }

            //────────────────────────────────────────
            // 葉
            //────────────────────────────────────────
            #region 葉
            if (
                // 深さ0 で呼び出されたときは、葉にしろということだぜ☆（＾▽＾）ｗｗｗ
                PureMemory.tnsk_fukasa == 0
                )
            {
                // 深さ（根っこからの深さ）は 1 以上で始まるから、ループの１週目は、スルーされるはずだぜ☆（＾▽＾）
                // １手指して　枝を伸ばしたとき、相手の手番の局面になっているな☆（＾▽＾）そのとき　ここを通る可能性があるぜ☆

                //
                // 末端局面で評価値を作らないぜ☆（＾～＾）！
                // 指し手を、指したときに作るんだぜ☆（＾～＾）！
                //

                PureMemory.tnsk_happaTeme = PureMemory.kifu_endTeme;

                // 「手目」カーソルは　DoSasite で１つ進んでいるはずなので、戻すんだぜ☆（＾～＾）
                out_bestSasite = PureMemory.kifu_sasiteAr[PureMemory.kifu_endTeme - 1];
                out_bestHyokasu = new Hyokati(PureMemory.gky_hyokati);


                // 葉で情報表示
                Util_Joho.JohoMatome(
                    PureMemory.tnsk_fukasa + 1,
                    out_bestHyokasu,
                    PureMemory.tnsk_hyoji
#if DEBUG
                    , out_bestHyokasu.dbg_riyu.ToString()
#endif
                    );
#if DEBUG
                Util_Tansaku.Snapshot("葉だぜ☆（＾～＾）", out_bestSasite);
#endif
                return;//枝を戻る（正常終了）
            }
#endregion

            //────────────────────────────────────────
            // 指し手生成
            //────────────────────────────────────────
            // グローバル変数 Util_SasiteSeisei.Sslist に指し手がセットされるぜ☆（＾▽＾）
            SasiteSeiseiAccessor.DoSasitePickerBegin(SasiteType.N21_All);
            SasitePicker01.SasitePicker_01(SasiteType.N21_All, true);

#region ステイルメイト
            //────────────────────────────────────────
            // ステイル・メイト
            //────────────────────────────────────────
            if (PureMemory.ssss_sasitelist[PureMemory.tnsk_fukasa].listCount<1)
            {
                // 詰んでるぜ☆（＾～＾）
                out_bestSasite = Sasite.Toryo;
                out_bestHyokasu = new Hyokati(
                    Conv_Hyokati.Hyokati_Rei,
                    Conv_Tumesu.Stalemate,
                    false
#if DEBUG
                    , Conv_Hyokati.Hyokati_Rei
                    , Conv_Hyokati.Hyokati_Rei
                    , Conv_Hyokati.Hyokati_Rei
                    , HyokaRiyu.Stalemate
                    ,""
#endif
                    );

                // ステイルメイトで情報表示
                Util_Joho.JohoMatome(
                    PureMemory.tnsk_fukasa + 1,// 深さは 0 になっているので、Tansaku していない状態（＝+1 して）に戻すぜ☆
                    out_bestHyokasu,
                    PureMemory.tnsk_hyoji
#if DEBUG
                    , "Stalemate"
#endif
                    );
#if DEBUG
                Util_Tansaku.Snapshot("ステイルメイトだぜ☆（＾～＾）", out_bestSasite);
                SasiteSeiseiAccessor.DumpSasiteSeisei(PureMemory.tnsk_hyoji);
#endif
                return;//枝を戻る（正常終了）
            }
#endregion


            for (int iSs=0; iSs<PureMemory.ssss_sasitelist[PureMemory.tnsk_fukasa].listCount; iSs++)
            {
                // 枝☆　適当にここらへんでカウントアップするかだぜ☆（＾～＾）
                PureMemory.tnsk_tyakusyuEdas++;



                //────────────────────────────────────────
                // 指す
                //────────────────────────────────────────

//#if DEBUG
//                Util_Tansaku.Snapshot("ドゥ前", hyoji);
//#endif

                Sasite ss_jibun = PureMemory.ssss_sasitelist[PureMemory.tnsk_fukasa].list_sasite[iSs];
                SasiteType ssType_jibun = PureMemory.ssss_sasitelist[PureMemory.tnsk_fukasa].list_sasiteType[iSs];

                if (DoSasiteOpe.TryFail_DoSasite_All(
                    ss_jibun,
                    ssType_jibun
#if DEBUG
                    , PureSettei.fenSyurui
                    , (IDebugMojiretu)PureMemory.tnsk_hyoji
                    , false
                    , "TryFail_Tansaku_(1)"
#endif
                    ))
                {
                    // 探索時にエラーが起こった場合は強制終了☆（＾～＾）
                    throw new Exception(PureMemory.tnsk_hyoji.ToContents());
                }
                // 手番を進めるぜ☆（＾～＾）
                SasiteSeiseiAccessor.AddKifu(
                    ss_jibun,
                    ssType_jibun,
                    PureMemory.dmv_ks_c);
#if DEBUG
                Util_Tansaku.Snapshot("ドゥ後・探索前", out_bestSasite);
#endif

                Sasite ss_aite;
                //SasiteType eda_sasiteType;
                // goto文で飛ぶと未割当になるので、ヌルでも入れておくぜ☆（＾～＾）
                Hyokati hyokasu_aiteToJibun = null;

                // この指し手が、駒を取った手かどうか☆

                PureMemory.SetTnskHyoji(PureMemory.tnsk_hyoji);
                // 探索者がプラスでスタートして、
                // 探索者の反対側はマイナスになり、
                // 探索者の反対側の反対側はプラスに戻るぜ☆（＾▽＾）
                PureMemory.DecreaseTnskFukasa();
                Tansaku_(
                    out ss_aite,
                    out hyokasu_aiteToJibun
                    // 相手番の指し手の評価値が入ってくるぜ☆（＾～＾）
                );
                PureMemory.IncreaseTnskFukasa();
//#if DEBUG
//                Util_Tansaku.Snapshot("探索後・アンドゥ前", hyoji);
//#endif

                //────────────────────────────────────────
                // 詰みを発見していれば、打ち切りフラグを立てるぜ☆（*＾～＾*）
                //────────────────────────────────────────
                bool undoAndBreak = false;
                if (hyokasu_aiteToJibun.tumeSu == Conv_Tumesu.CatchRaion)
                {
                    out_bestSasite = ss_jibun;
                    out_bestHyokasu = new Hyokati(
                    Conv_Hyokati.Hyokati_Rei,
                    Conv_Tumesu.CatchRaion,// この枝にこれるようなら、勝ち宣言だぜ☆（＾▽＾）
                    false
                #if DEBUG
                    , Conv_Hyokati.Hyokati_Rei
                    , Conv_Hyokati.Hyokati_Rei
                    , Conv_Hyokati.Hyokati_Rei
                    , HyokaRiyu.TansakuRaionCatch
                    , ""
                #endif
                        );
                    // 打ち切りで情報表示
                    Util_Joho.JohoMatome(
                        PureMemory.tnsk_fukasa,
                        out_bestHyokasu,
                        PureMemory.tnsk_hyoji
#if DEBUG
                        , "TansakuRaionCatch"
#endif
                        );

                    // 詰みではなく、らいおんきゃっち　または　トライかを　調べるぜ☆（＾～＾）
                    // この手番は、
                    // この指し手を選べば、勝てるという理屈だが……☆
                    undoAndBreak = true;
#if DEBUG
                    Util_Tansaku.Snapshot("らいおんキャッチだぜ☆（＾～＾）", out_bestSasite);
#endif
                }

                // 探索で先の枝から戻ってきたときは、評価の符号を反転し、詰め手数のカウントもアップするぜ☆（＾～＾）
                hyokasu_aiteToJibun.CountUpTume();
                hyokasu_aiteToJibun.ToHanten();



                if (UndoSasiteOpe.TryFail_UndoSasite(
#if DEBUG
                    PureSettei.fenSyurui
                    , (IDebugMojiretu)PureMemory.tnsk_hyoji
#endif
                    ))
                {
                    // 探索時にエラーが起こった場合は強制終了☆（＾～＾）
                    throw new Exception(PureMemory.tnsk_hyoji.ToContents());
                }

#if DEBUG
                Util_Tansaku.Snapshot("アンドゥ後", out_bestSasite);
#endif

                //────────────────────────────────────────
                // これ以上　弟要素を探索するのを止め、枝を戻るかどうか☆（＾～＾）
                //────────────────────────────────────────
#region 打ち切り各種
                if (undoAndBreak)
                {
                    // （１）千日手の権利を相手に渡すために低点数付け＜それ以降の手は読まない＞
                    // （２）らいおん　を捕獲した
                    // （３）トライ　した
#if DEBUG
                    Util_Tansaku.Snapshot("アンドゥ後ブレイクだぜ☆（＾～＾）", out_bestSasite);
#endif
                    break;//枝を戻る（正常終了）
                }
                #endregion

                //────────────────────────────────────────
                // アップデート・枝ベスト
                //────────────────────────────────────────
                #region アップデート・枝ベスト

                // 点数が付かないことがあって、その場合 ベスト指し手 を１度も選ばない
                // 「<=」にすると同点だったら、指し手のオーダリングの低いのを選ぶが☆（＾～＾）
                if (null== out_bestHyokasu)
                {
                    out_bestSasite = ss_jibun;
                    out_bestHyokasu = hyokasu_aiteToJibun;// new HyokaSu(eda_hyokasu);//ここで新規作成
#if DEBUG
                    Util_Tansaku.Snapshot("アップデート枝ベスト１回目だぜ☆（＾～＾）", out_bestSasite);
#endif
                }
                else if (out_bestHyokasu.hyokaTen < hyokasu_aiteToJibun.hyokaTen)
                {
                    out_bestSasite = ss_jibun;
                    out_bestHyokasu.ToSet(hyokasu_aiteToJibun);

                    // 兄弟の中で一番の読み筋だぜ☆（＾▽＾）
                    // ↓
                    // TODO: ここで情報を表示したいが……☆（＾～＾）
#if DEBUG
                    Util_Tansaku.Snapshot("アップデート枝ベストだぜ☆（＾～＾）", out_bestSasite);
#endif
                }
                #endregion

            }//指し手ループ
             //;

            // このノードでの最大評価を返すんだぜ☆（＾▽＾）
            // ここでアルファを返してしまうと、アルファが１回も更新されなかったときに、このノードの最大評価ではないものを返してしまうので不具合になるぜ☆（＾～＾）

            if (null == out_bestHyokasu)
            {
                out_bestSasite = Sasite.Toryo;
                out_bestHyokasu = new Hyokati(
                    Conv_Hyokati.Hyokati_Rei,
                    Conv_Tumesu.None,
                    false
#if DEBUG
                    , Conv_Hyokati.Hyokati_Rei
                    , Conv_Hyokati.Hyokati_Rei
                    , Conv_Hyokati.Hyokati_Rei
                    , HyokaRiyu.Fumei
                    , "ループ抜け"
#endif
                    );

                // ステイルメイトで情報表示
                Util_Joho.JohoMatome(
                    PureMemory.tnsk_fukasa,
                    out_bestHyokasu,
                    PureMemory.tnsk_hyoji
#if DEBUG
                    , "LoopOut"
#endif
                    );

#if DEBUG
                Util_Tansaku.Snapshot("評価の決まらない、ループ抜けだぜ☆（＾～＾）", out_bestSasite);
#endif
                return;//枝を戻る（正常終了）
            }

        }
    }
}
