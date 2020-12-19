#if DEBUG
using kifuwarabe_shogithink.pure.com;
using kifuwarabe_shogithink.pure.com.jikan;
using kifuwarabe_shogithink.pure.genkyoku;
using kifuwarabe_shogithink.pure.ikkyoku;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.logger;
using kifuwarabe_shogithink.pure.listen.genkyoku;
using System.Collections.Generic;
using kifuwarabe_shogithink.pure.control;
#else
using kifuwarabe_shogithink.pure.com;
using kifuwarabe_shogithink.pure.com.jikan;
using kifuwarabe_shogithink.pure.control;
using kifuwarabe_shogithink.pure.ikkyoku;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.listen.genkyoku;
using kifuwarabe_shogithink.pure.logger;
using System.Collections.Generic;
#endif

namespace kifuwarabe_shogithink.pure
{
    /// <summary>
    /// アプリ。
    /// </summary>
    public static class PureAppli
    {
        static PureAppli()
        {
            syuturyoku1 = new MojiretuImpl();
            karappoSyuturyoku = new KarappoMojiretuImpl();

            dlgt_CreateJoho = Util_Joho.Dlgt_IgnoreJoho;
        }

        /// <summary>
        /// きふわらべのバージョン。
        /// 定跡登録で必要になるぜ☆（＾▽＾）
        /// 
        /// 100: バージョン追加
        /// 101: トライ判定修正
        /// 102: 手番を修正
        /// 103: メイトを試し
        /// 104: 読んでいる途中の手を指さないよう改造
        /// 105: 定跡を使うときは探索せずにすぐ指すように修正
        /// 106: 定跡をほぼ 0秒 で指すように改造
        /// 107: 指し手にランダム性を付けれるようにしたぜ☆（＾～＾）
        /// 108: 駒割りを　ひよこ100点　に桁上げ。ランダム二駒関係追加☆（＾▽＾）
        /// 109: ランダム二駒を止め、味方の駒の紐づきに加点、相手の駒の紐づきで減点☆（＾▽＾）
        /// 110: アスピレーション・ウィンドウ・サーチが　メートを壊していたので対応☆（＾～＾）；；
        /// 111: 指し手生成のオーダリングの王手回避判定を壊していたので修正☆（＾～＾）；；；
        /// 112: 持ち駒での王手の優先順位を上げたぜ☆（＾～＾）
        /// 113: 駒を取る手の優先順位を上げたぜ☆（＾▽＾）
        /// 114: 紐付打の優先順位を上げたぜ☆（＾▽＾）
        /// 115: ランダム二駒関係を　また実装したぜ☆（＾▽＾）
        /// 116: 機械学習を始めてみたぜ☆（＾～＾）
        /// 117: 二駒関係のインデックスを修正☆（＾～＾）
        /// 118: 二駒関係の評価値取得を修正☆（＾～＾）
        /// 119: 二駒関係のＰ２のインデックスを修正☆（＾～＾）千日手回避フラグも実装だぜ☆（＾▽＾）
        /// 120: 指し手生成のオーダリング、探索部の読み筋を修正☆（＞＿＜）
        /// 121: 定跡の点数と、メートの20000点に引っ張られていたので、学習は定跡無しで評価値の範囲内でするように修正☆（＾～＾）
        /// 122: SEE（static exchange evaluation) を導入したぜ☆（＾▽＾）
        /// 123: らいおんの逃げ道を開ける手　の優先順位を下げる仕掛けに少しずつ着手だぜ☆（＾▽＾）
        /// 124: SEE を修正したぜ☆（＾～＾）
        /// 125: 二駒関数を差分更新するようにしたぜ☆（*＾～＾*）
        /// 126: 勝負無し、という評価値を盛り込んでみるぜ☆（＾～＾）主に機械学習用☆
        /// 127: 駒割り評価値の配点を、－３２０００～３２０００の領域を広く使うように変更☆（＾～＾）
        /// 128: 二駒関係評価値の表を、半分は使わないように ---- や xxxx の文字で埋めたぜ☆（＾～＾）
        /// 129: 二駒関係評価値の係数のデフォルトを 1.0d に変更したぜ☆（＾～＾）
        /// 130: 探索一手詰め打ち切り　を入れてみたぜ☆（＾～＾）
        /// 131: 「駒を取る手」で「王手」できなかったので簡易的に修正したぜ☆（＞＿＜）
        /// </summary>
        public const int VERSION = 131;

        /// <summary>
        /// 出力する文字列を蓄えておくものだぜ☆（＾▽＾）
        /// 
        /// コンソールゲーム用、エラー用の区別なく１つに出力されるぜ☆（＾～＾）
        /// </summary>
        public static INewString syuturyoku1;
        /// <summary>
        /// 仕事をしない出力だぜ☆（＾▽＾）ｗｗｗ
        /// </summary>
        public static INewString karappoSyuturyoku { get; set; }

        public static GameMode gameMode { get; set; }

#region コンソールゲーム用の機能☆
        /// <summary>
        /// アプリケーション開始時☆
        /// アプリケーション設定完了時に呼び出せだぜ☆（＾▽＾）！
        /// </summary>
        public static bool TryFail_Begin2_Application(
#if DEBUG
            IDebugMojiretu reigai1
#endif
            )
        {
            Face_TimeManager.timeManager.stopwatch_Savefile.Start();// 定跡ファイルの保存間隔の計測
            Face_TimeManager.timeManager.stopwatch_RenzokuRandomRule.Start();

            // ３４将棋の平手初期局面を作るぜ☆（*＾～＾*）ｗｗｗｗ
            // どうぶつしょうぎの平手初期局面に変更するぜ☆ｗｗｗ（＾▽＾）
            LisGenkyoku.SetRule(
                GameRule.DobutuShogi, 3, 4,
                "キラゾ" +
                "　ヒ　" +
                "　ひ　" +
                "ぞらき"
                , new Dictionary<Motigoma, int>()
                {
                    { Motigoma.K,0 },
                    { Motigoma.Z,0 },
                    { Motigoma.H,0 },
                    { Motigoma.k,0 },
                    { Motigoma.z,0 },
                    { Motigoma.h,0 },
                }
            );

            // ゲームモード設定☆
            PureAppli.gameMode = GameMode.Karappo;
            return Pure.SUCCESSFUL_FALSE;
        }
#endregion

        public static bool TryFail_Init()
        {
            IHyojiMojiretu hyoji = syuturyoku1;

            //────────────────────────────────────────
            // （手順１）アプリケーション開始前に設定しろだぜ☆（＾▽＾）！
            //────────────────────────────────────────
            // アプリケーション開始後は Face_Kifuwarabe.Execute("set 名前 値") を使って設定してくれだぜ☆（＾▽＾）
            // ↓コメントアウトしているところは、デフォルト値を使っている☆（＾～＾）

            //Option_Application.Optionlist.JohoJikan = 3000;


            //Option_Application.Optionlist.P1Com = false;
            PureSettei.p2Com = true;//対局者２はコンピューター☆
            //Option_Application.Optionlist.PNChar = new MoveCharacter[] { MoveCharacter.HyokatiYusen, MoveCharacter.HyokatiYusen };
            //Option_Application.Optionlist.PNName = new string[] { "対局者１", "対局者２" };
            ComSettei.saidaiFukasa = 13;// コンピューターの読みの最大深さ


            //──────────
            // 思考時間
            //──────────
            ComSettei.sikoJikan = 5000;// 500; // 最低でも用意されているコンピューターが思考する時間（ミリ秒）
            ComSettei.sikoJikanRandom = 5000;// 1501;// 追加で増えるランダム時間の最大（この値未満）。 期待値を考えて設定しろだぜ☆（＾～＾）例： ( 500 + 1500 ) / 2 = 1000
                                             //Option_Application.Optionlist.UseTimeOver = true;


            // （手順３）アプリケーション開始時設定　を終えた後に　これを呼び出すこと☆（＾～＾）！
            if (TryFail_Begin2_Application(
                
#if DEBUG
                (IDebugMojiretu)hyoji
#endif
                ))
            {
                return Pure.FailTrue("Try_Begin2_Application");
            }

            return Pure.SUCCESSFUL_FALSE;
        }

        public static Util_Joho.Dlgt_CreateJoho dlgt_CreateJoho;
    }
}
