#if DEBUG
using kifuwarabe_shogithink.pure.com;
using kifuwarabe_shogithink.pure.com.hyoka;
using kifuwarabe_shogithink.pure.control;
using kifuwarabe_shogithink.pure.ikkyoku;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.ky.bb;
using kifuwarabe_shogithink.pure.ky.tobikiki;
using kifuwarabe_shogithink.pure.move;

using System;
#else

using kifuwarabe_shogithink.pure.com;
using kifuwarabe_shogithink.pure.com.hyoka;
using kifuwarabe_shogithink.pure.control;
using kifuwarabe_shogithink.pure.ikkyoku;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.ky.bb;
using kifuwarabe_shogithink.pure.ky.tobikiki;
using kifuwarabe_shogithink.pure.move;
using System;
using System.Text;
#endif

namespace kifuwarabe_shogithink.pure
{
    public static class PureMemory
    {
        static PureMemory()
        {
            int taiLn = Conv_Taikyokusya.itiran.Length;

            #region 被王手判定
            // ビットボード
            hot_bb_checkerAr = new Bitboard[taiLn];
            hot_bb_raion8KinboAr = new Bitboard[taiLn];
            hot_bb_nigemitiWoFusaideiruAiteNoKomaAr = new Bitboard[taiLn];
            hot_bb_nigereruAr = new Bitboard[taiLn];
            hot_bb_nigeroAr = new Bitboard[taiLn];
            for (int iTai = 0; iTai < taiLn; iTai++)
            {
                hot_bb_checkerAr[iTai] = new Bitboard();
                hot_bb_raion8KinboAr[iTai] = new Bitboard();
                hot_bb_nigemitiWoFusaideiruAiteNoKomaAr[iTai] = new Bitboard();
                hot_bb_nigereruAr[iTai] = new Bitboard();
                hot_bb_nigeroAr[iTai] = new Bitboard();
            }

            hot_ms_raionAr = new Masu[taiLn];
            hot_outeKomasCountAr = new int[taiLn];
            hot_isNigerarenaiCheckerAr = new bool[taiLn];
            hot_raionCatchChosaAr = new bool[taiLn];
            #endregion

            #region 指し手生成（シングルスレッドを想定）
            ssss_moveList = new MoveList[PureMemory.MaxMoveDepth];
            ssss_moveListBad = new MoveList[PureMemory.MaxMoveDepth];
            ssss_bbVar_idosaki_narazu = new Bitboard();
            ssss_bbVar_idosaki_nari = new Bitboard();
            ssssTmp_bbVar_ibasho = new Bitboard();

            ssss_bbBase_idosaki01_checker = new Bitboard();
            ssss_bbBase_idosaki02_raionCatch = new Bitboard();
            ssss_bbBase_idosaki03_nigeroTe = new Bitboard();
            ssss_bbBase_idosaki04_try = new Bitboard();
            ssss_bbBase_idosaki05_komaWoToruTe = new Bitboard();
            ssss_bbBase_idosaki06_himodukiOteZasi = new Bitboard();
            ssss_bbBase_idosaki07_suteOteZasi = new Bitboard();
            ssss_bbBase_idosaki08_suteOteDa = new Bitboard();
            ssss_bbBase_idosaki09_himodukiOteDa = new Bitboard();
            ssss_bbBase_idosaki10_himodukiKanmanDa = new Bitboard();
            ssss_bbBase_idosaki11_himodukiKanmanZasi = new Bitboard();
            ssss_bbBase_idosaki12_bottiKanmanZasi = new Bitboard();
            ssss_bbBase_idosaki13_bottiKanmanDa = new Bitboard();
            ssss_bbBase_idosaki14_suteKanmanZasi = new Bitboard();
            ssss_bbBase_idosaki15_suteKanmanDa = new Bitboard();

            ssss_movePickerWoNuketaBasho1 = "";

            for (int iFukasa = 0; iFukasa < PureMemory.MaxMoveDepth; iFukasa++)
            {
                ssss_moveList[iFukasa] = new MoveList();
                ssss_moveListBad[iFukasa] = new MoveList();
            }
            #endregion

            #region ムーブス
            mvs_ssAr = new Move[KIFU_SIZE];
            #endregion
            #region 棋譜（コンピューター・プレイヤー同時に１つまで）
            kifu_syokiKyokumenFen = "";
            kifu_toraretaKsAr = new Komasyurui[KIFU_SIZE];
            kifu_moveArray = new Move[KIFU_SIZE];
            kifu_moveTypeArray = new MoveType[KIFU_SIZE];
            // 手番☆（＾～＾）
            kifu_tebanAr_ = new Taikyokusya[KIFU_SIZE];
            kifu_aitebanAr_ = new Taikyokusya[KIFU_SIZE];
            kifu_nTebanAr_ = new int[KIFU_SIZE];
            kifu_nAitebanAr_ = new int[KIFU_SIZE];

            // 初期局面手番を未設定にしておいて、ResetTebanArray( ) すれば、手番配列の初期値が入るぜ☆（＾～＾）
            kifu_syokikyokumenTeban = Taikyokusya.Yososu;
            ResetTebanArray(Taikyokusya.T1);

            // 配列等の初期化が終わったあとで、手目をリセット
            ClearTeme(
                //kifu_syokikyokumenTeban
                );
            #endregion

            #region 現局面（棋譜カーソルが指している局面）（コンピューター・プレイヤー同時に１つまで）
            gky_kekka = TaikyokuKekka.Karappo;
            gky_ky = new Kyokumen();
            gky_hyokati = new Hyokati();
            Util_Control.UpdateRule(
#if DEBUG
                "static PureMemory"
#endif
                );
            #endregion

            #region 探索（tnsk）
            tnsk_kohoMove = Move.Toryo;
            #endregion
        }

        #region 被王手判定
        /// <summary>
        /// 手番側（王手を掛けられる側）の　らいおん　がいる升だぜ☆（＾▽＾）
        /// らいおんが盤上にいないこともあるぜ☆（＾▽＾）
        /// [対局者]
        /// </summary>
        public static Masu[] hot_ms_raionAr;
        /// <summary>
        /// 自分の　らいおん　に王手をかけている駒がいる升だぜ☆（＾▽＾）
        /// [対局者]
        /// </summary>
        public static Bitboard[] hot_bb_checkerAr;
        /// <summary>
        /// 自分の　らいおん　の逃げ道を塞いでいる相手の駒がいる升だぜ☆（＾▽＾）
        /// </summary>
        public static Bitboard[] hot_bb_nigemitiWoFusaideiruAiteNoKomaAr { get; set; }
#if DEBUG
        ///// <summary>
        ///// 相手番の　駒がいる升　だぜ☆（＾▽＾）
        ///// </summary>
        //public Bitboard hot_OpponentKomaBB_TestNoTame { get; set; }
        ///// <summary>
        ///// 相手の　利き　全部の升だぜ☆（＾▽＾）
        ///// </summary>
        //public Bitboard hot_OpponentKikiZenbuBB_TestNoTame { get; set; }
        ///// <summary>
        ///// 自分の　らいおん　の塞がれている逃げ道の升だぜ☆（＾▽＾）
        ///// </summary>
        //public Bitboard hot_FusagiMitiBB_TestNoTame { get; set; }
#endif
        /// <summary>
        /// 王手を掛けている駒の数
        /// </summary>
        public static int[] hot_outeKomasCountAr;
        /// <summary>
        /// 王手をかけている１個の駒を取り除く必要があるかどうかだぜ☆（＾▽＾）
        /// このフラグが立っていれば、その駒を取る手を生成する必要があるし、
        /// 生成できなかったら負けだぜ☆（＾～＾）
        /// </summary>
        public static bool[] hot_isNigerarenaiCheckerAr;
        /// <summary>
        /// らいおん　が逃げる必要があるかどうかだぜ☆（＾▽＾）
        /// 優先度は、返討手の次だぜ☆
        /// 
        /// 逃げなくていいとき、逃げ道がないときの　どちらも 0 だぜ☆（＾▽＾）
        /// </summary>
        public static Bitboard[] hot_bb_nigeroAr;
        /// <summary>
        /// 逃げれる道☆（＾▽＾）
        /// </summary>
        public static Bitboard[] hot_bb_nigereruAr;
        /// <summary>
        /// らいおんキャッチ調査をして、らいおんをキャッチしていたら真☆（＾▽＾）
        /// </summary>
        public static bool[] hot_raionCatchChosaAr;
        /// <summary>
        /// 手番側（王手を掛けられる側）の　らいおん　の８近傍だぜ☆（＾▽＾）
        /// </summary>
        public static Bitboard[] hot_bb_raion8KinboAr;
        #endregion

        #region 指し手生成「ssss」（シングルスレッドを想定）
        /// <summary>
        /// 指し手生成の手動表示で使う
        /// </summary>
        public const int FUKASA_MANUAL = 0;
        /// <summary>
        /// 128手先も読まないだろう☆（＾～＾）
        /// </summary>
        public const int MaxMoveDepth = 128;
        /// <summary>
        /// 合法手の数は、
        /// どうぶつしょうぎ では 38、
        /// 本将棋では 593 が上限のようだ。
        /// 駒の動かし方や、駒の数などルールを　ころころ　変えることもあるが、
        /// 600 もあれば十分だろう☆（＾▽＾）
        /// </summary>
        public const int MaxMove = 600; // 132 ざっくり、盤上12升×8方向 ＋ 持ち駒3種類×12升
        /// <summary>
        /// [深さ]
        /// 指し手リスト☆（＾～＾）
        /// </summary>
        public static MoveList[] ssss_moveList { get; set; }
        /// <summary>
        /// [深さ]
        /// 悪い指し手は、一旦　こっちに入れるんだぜ☆（＾～＾）あとで Movelist に入れなおすぜ☆（＾～＾）
        /// </summary>
        public static MoveList[] ssss_moveListBad { get; set; }
        /// <summary>
        /// 勝負無し
        /// </summary>
        public static bool ssss_isSyobuNasi;
        //──────────
        // 使い回す変数
        //──────────
        // 移動先

        // 王手を掛けてきている駒
        public static Bitboard ssss_bbBase_idosaki01_checker;

        public static Bitboard ssss_bbBase_idosaki02_raionCatch;
        public static Bitboard ssss_bbBase_idosaki03_nigeroTe;
        public static Bitboard ssss_bbBase_idosaki04_try;
        public static Bitboard ssss_bbBase_idosaki05_komaWoToruTe;
        public static Bitboard ssss_bbBase_idosaki06_himodukiOteZasi;
        public static Bitboard ssss_bbBase_idosaki07_suteOteZasi;
        public static Bitboard ssss_bbBase_idosaki08_suteOteDa;
        public static Bitboard ssss_bbBase_idosaki09_himodukiOteDa;
        public static Bitboard ssss_bbBase_idosaki10_himodukiKanmanDa;
        public static Bitboard ssss_bbBase_idosaki11_himodukiKanmanZasi;
        public static Bitboard ssss_bbBase_idosaki12_bottiKanmanZasi;
        public static Bitboard ssss_bbBase_idosaki13_bottiKanmanDa;
        public static Bitboard ssss_bbBase_idosaki14_suteKanmanZasi;
        public static Bitboard ssss_bbBase_idosaki15_suteKanmanDa;
        // 移動先コピー（成らず）
        public static Bitboard ssss_bbVar_idosaki_narazu;
        // 移動先コピー（成り）
        public static Bitboard ssss_bbVar_idosaki_nari;
        // 味方の駒 弱い駒から動かそう☆
        public static Bitboard ssssTmp_bbVar_ibasho;
        /// <summary>
        /// 指し手ピッカーを抜けた場所
        /// </summary>
        public static string ssss_movePickerWoNuketaBasho1;
        //──────────
        // １マスずつ駒の動きを調べている段階だぜ☆（＾～＾）
        //──────────
        /// <summary>
        /// 確定指し手タイプ
        /// </summary>
        public static MoveType ssss_ugoki_kakuteiSsType { get { return ssss_ugoki_kakuteiSsType_; } }
        public static bool ssss_ugoki_kakuteiDa { get { return ssss_ugoki_kakuteiDa_; } }
        static MoveType ssss_ugoki_kakuteiSsType_;
        static bool ssss_ugoki_kakuteiDa_;
        public static void SetKakuteiSsType(MoveType ssType, bool da)
        {
            ssss_ugoki_kakuteiSsType_ = ssType;
            ssss_ugoki_kakuteiDa_ = da;
        }
        /// <summary>
        /// 動かす駒
        /// </summary>
        public static Koma ssss_ugoki_km { get { return ssss_ugoki_km_; } }
        public static Koma ssss_ugoki_sakasaKm { get { return ssss_ugoki_sakasaKm_; } }
        public static Komasyurui ssss_ugoki_ks { get { return ssss_ugoki_ks_; } }
        static Koma ssss_ugoki_km_;
        /// <summary>
        /// 自分の駒を先後逆にしたものだぜ☆（＾～＾）指定のマスにその駒が行く方法を調べるのに使うんだぜ☆（＾～＾）
        /// </summary>
        static Koma ssss_ugoki_sakasaKm_;
        static Komasyurui ssss_ugoki_ks_;
        public static void SetSsssUgokiKm(Koma km)
        {
            ssss_ugoki_km_ = km;
            ssss_ugoki_ks_ = Med_Koma.KomaToKomasyurui(km);
            ssss_ugoki_sakasaKm_ = Med_Koma.KomasyuruiAndTaikyokusyaToKoma(ssss_ugoki_ks_, kifu_aiteban);
        }
        /// <summary>
        /// 動かす駒の移動元マス
        /// </summary>
        public static Masu ssss_ugoki_ms_src;
        /// <summary>
        /// 動かす駒の移動先マス
        /// </summary>
        public static Masu ssss_ugoki_ms_dst;
        //──────────
        // 持駒（mot）の打つ場所を１マスずつ調べている段階だぜ☆（＾～＾）
        //──────────
        public static MotigomaSyurui ssss_mot_mks { get { return ssss_mot_mks_; } }
        public static Motigoma ssss_mot_mg { get { return ssss_mot_mg_; } }
        /// <summary>
        /// 持ち駒を持っているときだけセットするぜ☆（＾～＾）
        /// </summary>
        public static Koma ssss_mot_km { get { return ssss_mot_km_; } }
        public static Komasyurui ssss_mot_ks { get { return ssss_mot_ks_; } }
        static MotigomaSyurui ssss_mot_mks_;
        static Motigoma ssss_mot_mg_;
        static Koma ssss_mot_km_;
        static Komasyurui ssss_mot_ks_;
        public static bool SetSsssMotMks_AndHasMotigoma(MotigomaSyurui val)
        {
            ssss_mot_mks_ = val;
            ssss_mot_mg_ = Med_Koma.MotiKomasyuruiAndTaikyokusyaToMotiKoma(val, kifu_teban);
            if (gky_ky.motigomaItiran.yomiMotigomaItiran.HasMotigoma(ssss_mot_mg_))
            {
                // 持ち駒を持っているときだけセットするぜ☆（＾～＾）
                ssss_mot_km_ = Med_Koma.MotiKomasyuruiAndTaikyokusyaToKoma(ssss_mot_mks_,kifu_teban);
                ssss_mot_ks_ = Med_Koma.MotiKomasyuruiToKomasyrui(ssss_mot_mks_);
                return true;
            }
            return false;
        }
        //──────────
        // 一手詰め判定（ジェネレート駒 genk）
        //──────────
        /// <summary>
        /// 一手詰め
        /// </summary>
        public static bool ssss_genk_tume1 { get { return ssss_genk_tume1_; } }
        static bool ssss_genk_tume1_;
        /// <summary>
        /// 見捨てる動き
        /// </summary>
        public static bool ssss_genk_misuteru { get { return ssss_genk_misuteru_; } }
        static bool ssss_genk_misuteru_;
        /// <summary>
        /// 逃げ道を開けて逃がしてしまう悪手（ヒューリスティックス）
        /// </summary>
        public static bool ssss_genk_nigasu { get { return ssss_genk_nigasu_; } }
        static bool ssss_genk_nigasu_;

        public static void SetSsssGenk(bool tume1, bool misuteru, bool nigasu)
        {
            ssss_genk_tume1_ = tume1;
            ssss_genk_misuteru_ = misuteru;
            ssss_genk_nigasu_ = nigasu;
        }
        /// <summary>
        /// 悪手
        /// </summary>
        public static bool IsAkusyu_Ssss
        {
            get
            {
                return ssss_genk_misuteru_ && ssss_genk_nigasu_;
            }            
        }
        public static MoveType GetAkusyuType_Ssss()
        {
            MoveType ssType = MoveType.N00_Karappo;

            if (ssss_genk_misuteru)
            {
                ssType |= MoveType.N20_Option_MisuteruUgoki;
            }

            if (ssss_genk_nigasu)
            {
                 ssType |= MoveType.N19_Option_NigemitiWoAkeruTe;
            }

            return ssType;
        }

        //──────────
        // デバッグ用
        //──────────
        public static Bitboard ssssDbg_bb_ojamaTai;
        public static int ssssDbg_rightShift;
        public static Bitboard ssssDbg_bb_rightShifted;
        public static Bitboard ssssDbg_bb_mask;
        public static bool ssssDbg_sakasa_forZHs;
        public static OjamaBanSyurui ssssDbg_ojamaBanSyurui;
        public static int ssssDbg_masuSpan;
        public static int ssssDbg_nanamedanD;
        public static Masu ssssDbg_atama_reverseRotateChikanhyo;
        public static Masu ssssDbg_atama_noRotateMotohyo;
        public static Masu ssssDbg_siri_noRotateMotohyo;
        public static int ssssDbg_haba;
        #endregion

        #region 指し手生成ドゥムーブ「dmv」
        /// <summary>
        /// dmv_変数は、DoMoveOpe.BunkaiMove( ) で設定するぜ☆（＾～＾）
        /// 
        /// t0 は移動元だぜ☆（＾～＾）
        /// </summary>
        public static Masu dmv_ms_t0;
        public static Koma dmv_km_t0;
        public static MotigomaSyurui dmv_mks_t0;
        public static Motigoma dmv_mk_t0;
        /// <summary>
        /// 盤上と、打の駒種類を共通におまとめしたいぜ☆（＾～＾）
        /// </summary>
        public static Komasyurui dmv_ks_t0;
        /// <summary>
        /// t1 は移動先だぜ☆（＾～＾）
        /// </summary>
        public static Koma dmv_km_t1;
        /// <summary>
        /// 成れる駒は成り、成れない駒はそのまま☆（＾～＾）
        /// </summary>
        public static Komasyurui dmv_ks_t1;
        /// <summary>
        /// 移動先升
        /// </summary>
        public static Masu dmv_ms_t1;
        /// <summary>
        /// あれば、移動先の相手の駒（取られる駒; capture）
        /// </summary>
        public static Koma dmv_km_c;
        /// <summary>
        /// 取られた駒種類
        /// </summary>
        public static Komasyurui dmv_ks_c;
        public static Motigoma dmv_mk_c;
        #endregion

        #region 指し手生成アンドゥムーブ「umv」
        //
        // 動かす駒を t0 と呼ぶとする。
        //      移動元を t0、移動先を t1 と呼ぶとする。
        // 取られる駒を c と呼ぶとする。
        //      取られる駒の元位置は t1 、駒台は 3 と呼ぶとする。
        //
        public static Move umv_ss;
        public static Masu umv_ms_t1;
        public static Koma umv_km_t1;
        public static Komasyurui umv_ks_t1;
        public static Masu umv_ms_t0;
        public static Motigoma umv_mk_t0;
        public static Komasyurui umv_ks_t0;
        public static Koma umv_km_t0;
        public static Komasyurui umv_ks_c;
        public static Koma umv_km_c;
        public static Motigoma umv_mk_c;
        #endregion

        #region ムーブス「mvs」
        public static readonly Move[] mvs_ssAr;
        public static int mvs_endTeme;
        #endregion

        #region 棋譜「kifu」（コンピューター・プレイヤー同時に１つまで）
        /// <summary>
        /// 2000もあれば十分だろ☆（＾～＾）
        /// </summary>
        public const int KIFU_SIZE = 2048;

        /// <summary>
        /// 初期局面が 0手目 だぜ☆（＾～＾）
        /// </summary>
        public static int kifu_endTeme { get { return kifu_endTeme_; } }
        /// <summary>
        /// カーソルを前に進めるというよりは、最大値を１増やすという意味合いだぜ☆（＾～＾）
        /// </summary>
        public static void AddTeme()
        {
            kifu_endTeme_++;
            ExtractKifuCursor();
        }
        /// <summary>
        /// アンドゥなどで利用☆（＾～＾）
        /// カーソルを後ろに戻すというよりは、最大値を１減らすという意味合いだぜ☆（＾～＾）
        /// </summary>
        public static void RemoveTeme()
        {
            kifu_endTeme_--;
            ExtractKifuCursor();
        }
        /// <summary>
        /// カーソル位置を最初に戻すだけだぜ☆（＾～＾）
        /// </summary>
        public static void ClearTeme()
        {
            kifu_endTeme_ = 0;
            ExtractKifuCursor();
        }
        /// <summary>
        /// 「手目」カーソルは、新規追加された、まだ指されていない配列要素を指しているぜ☆（＾～＾）
        /// つまり、初期値として仮に入れてある、投了を指しているはずだぜ☆（＾～＾）
        /// </summary>
        static int kifu_endTeme_;
        /// <summary>
        /// 初期局面FEN
        /// </summary>
        public static string kifu_syokiKyokumenFen;
        /// <summary>
        /// 取られた駒の種類だぜ☆（＾▽＾）
        /// 該当がなければ Komasyurui.Yososu を入れておけだぜ☆（＾～＾）
        /// </summary>
        public static readonly Komasyurui[] kifu_toraretaKsAr;
        /// <summary>
        /// 未確定の、探索中の指し手が入っているぜ☆（＾▽＾）
        /// </summary>
        public static readonly Move[] kifu_moveArray;
        /// <summary>
        /// 読み筋に指し手タイプを出すことで、デバッグに使うために覚えておくぜ☆（＾▽＾）
        /// </summary>
        public static readonly MoveType[] kifu_moveTypeArray;
        /// <summary>
        /// 手番☆（＾～＾）
        /// </summary>
        public static Taikyokusya kifu_teban { get { return kifu_teban_; } }
        public static Taikyokusya GetTebanByTeme(int teme)
        {
            return kifu_tebanAr_[teme];
        }
        public static Taikyokusya kifu_aiteban { get { return kifu_aiteban_; } }
        public static int kifu_nTeban { get { return kifu_nTeban_; } }
        public static int kifu_nAiteban { get { return kifu_nAiteban_; } }
        /// <summary>
        /// kifu_teban_ 等は kifu_temeEnd_ と対応させる必要があるぜ☆（＾～＾）
        /// </summary>
        public static void ExtractKifuCursor()
        {
            kifu_teban_ = kifu_tebanAr_[kifu_endTeme];
            kifu_aiteban_ = kifu_aitebanAr_[kifu_endTeme];
            kifu_nTeban_ = kifu_nTebanAr_[kifu_endTeme];
            kifu_nAiteban_ = kifu_nAitebanAr_[kifu_endTeme];
        }
        /// <summary>
        /// デバッグ用
        /// </summary>
        /// <param name="iTeme"></param>
        /// <returns></returns>
        public static string ToString_KifuCursor(int iTeme)
        {
            return string.Format("({0,3}) teban={1} aiteban={2} nTeban={3} nAiteban={4}",
                iTeme,
                kifu_tebanAr_[iTeme],
                kifu_aitebanAr_[iTeme],
                kifu_nTebanAr_[iTeme],
                kifu_nAitebanAr_[iTeme]
                );
        }
        static readonly Taikyokusya[] kifu_tebanAr_;
        static readonly Taikyokusya[] kifu_aitebanAr_;
        static readonly int[] kifu_nTebanAr_;
        static readonly int[] kifu_nAitebanAr_;
        static Taikyokusya kifu_teban_;
        static Taikyokusya kifu_aiteban_;
        static int kifu_nTeban_;
        static int kifu_nAiteban_;
        /// <summary>
        /// 先手始まりか、後手始まりか
        /// </summary>
        static Taikyokusya kifu_syokikyokumenTeban;
        public static void ResetTebanArray(Taikyokusya kaisiTai)
        {
            if(kifu_syokikyokumenTeban!=kaisiTai)
            {
                kifu_syokikyokumenTeban = kaisiTai;
                switch (kaisiTai)
                {
                    case Taikyokusya.T1:
                        {
                            // 先手始まりケース
                            for (int iTeme_even = 0; iTeme_even < KIFU_SIZE; iTeme_even += 2)
                            {
                                kifu_tebanAr_[iTeme_even] = Taikyokusya.T1;
                                kifu_aitebanAr_[iTeme_even] = Taikyokusya.T2;
                                kifu_nTebanAr_[iTeme_even] = (int)Taikyokusya.T1;
                                kifu_nAitebanAr_[iTeme_even] = (int)Taikyokusya.T2;
                            }
                            for (int iTeme_odd = 1; iTeme_odd < KIFU_SIZE; iTeme_odd += 2)
                            {
                                // １つ飛ばしで、相手番☆（＾～＾）
                                kifu_tebanAr_[iTeme_odd] = Taikyokusya.T2;
                                kifu_aitebanAr_[iTeme_odd] = Taikyokusya.T1;
                                kifu_nTebanAr_[iTeme_odd] = (int)Taikyokusya.T2;
                                kifu_nAitebanAr_[iTeme_odd] = (int)Taikyokusya.T1;
                            }
                        }
                        break;
                    case Taikyokusya.T2:
                        {
                            // 後手始まりケース
                            for (int iTeme_even = 0; iTeme_even < KIFU_SIZE; iTeme_even += 2)
                            {
                                kifu_tebanAr_[iTeme_even] = Taikyokusya.T2;
                                kifu_aitebanAr_[iTeme_even] = Taikyokusya.T1;
                                kifu_nTebanAr_[iTeme_even] = (int)Taikyokusya.T2;
                                kifu_nAitebanAr_[iTeme_even] = (int)Taikyokusya.T1;
                            }
                            for (int iTeme_odd = 1; iTeme_odd < KIFU_SIZE; iTeme_odd += 2)
                            {
                                // １つ飛ばしで、相手番☆（＾～＾）
                                kifu_tebanAr_[iTeme_odd] = Taikyokusya.T1;
                                kifu_aitebanAr_[iTeme_odd] = Taikyokusya.T2;
                                kifu_nTebanAr_[iTeme_odd] = (int)Taikyokusya.T1;
                                kifu_nAitebanAr_[iTeme_odd] = (int)Taikyokusya.T2;
                            }
                        }
                        break;
                    default: throw new Exception(string.Format("未定義 tai={0}", kaisiTai));
                }
            }
        }
        #endregion

        #region 現局面（棋譜カーソルが指している局面）（コンピューター・プレイヤー同時に１つまで）
        /// <summary>
        /// ゲームの結果☆
        /// </summary>
        public static TaikyokuKekka gky_kekka;
        /// <summary>
        /// 局面だぜ☆（＾▽＾）
        /// この局面を使って、指し手を生成したりするんだぜ☆（＾～＾）
        /// </summary>
        public static Kyokumen gky_ky;
        /// <summary>
        /// 局面の評価値だぜ☆（＾～＾）
        /// </summary>
        public static Hyokati gky_hyokati;
        #endregion

        #region 探索
        /// <summary>
        /// 探索を開始した時点で「図はn手まで」だったかだぜ☆（＾▽＾）
        /// </summary>
        public static int tnsk_kaisiTeme { get { return tnsk_kaisiTeme_; } }
        static int tnsk_kaisiTeme_;
        public static Taikyokusya tnsk_kaisiTaikyokusya { get { return tnsk_kaisiTaikyokusya_; } }
        static Taikyokusya tnsk_kaisiTaikyokusya_;
        /// <summary>
        /// どこまで深く読んで、戻ってきているところか☆（＾～＾）
        /// </summary>
        public static int tnsk_happaTeme;
        public static void InitTansaku()
        {
            tnsk_kaisiTeme_ = kifu_endTeme;
            tnsk_kaisiTaikyokusya_ = kifu_tebanAr_[tnsk_kaisiTeme_];
            tnsk_happaTeme = kifu_endTeme;
        }

        #region 探索打ち切りフラグ
        public static TansakuUtikiri ssss_tansakuUtikiri_;
        /// <summary>
        /// もうらいおんを捕まえる手を見つけているので、指し手生成しない場合、真。
        /// </summary>
        /// <returns></returns>
        public static TansakuUtikiri ssss_tansakuUtikiri { get { return ssss_tansakuUtikiri_; } }
        public static void SetTansakuUtikiri(TansakuUtikiri val)
        {
            ssss_tansakuUtikiri_ = val;
        }
        #endregion

        /// <summary>
        /// ベストムーブの候補になっている指し手だぜ☆（＾～＾）
        /// </summary>
        public static Move tnsk_kohoMove;
        public static int tnsk_itibanFukaiNekkoKaranoFukasa_JohoNoTameni;
        /// <summary>
        /// 探索した枝数☆
        /// </summary>
        public static int tnsk_tyakusyuEdas;
        /// <summary>
        /// 深さ☆（＾～＾）末端局面が 0 だぜ☆（＾～＾）
        /// </summary>
        public static int tnsk_fukasa { get { return tnsk_fukasa_; } }
        public static void IncreaseTnskFukasa()
        {
            tnsk_fukasa_++;
        }
        public static void DecreaseTnskFukasa()
        {
            tnsk_fukasa_ --;
        }
        public static void SetTnskFukasa(int val)
        {
            tnsk_fukasa_ = val;
        }
        static int tnsk_fukasa_;

        /// <summary>
        /// 探索用の表示文字列
        /// </summary>
        public static StringBuilder tnsk_hyoji { get { return tnsk_hyoji_; } }
        public static void SetTnskHyoji(StringBuilder hyoji)
        {
            tnsk_hyoji_ = hyoji;
        }
        static StringBuilder tnsk_hyoji_;
#if DEBUG
        public static TansakuSyuryoRiyu tnsk_syuryoRiyu;
#endif
        #endregion
    }
}
