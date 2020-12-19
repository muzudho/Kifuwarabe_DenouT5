#if DEBUG
using kifuwarabe_shogithink.pure;
using kifuwarabe_shogithink.pure.com;
using kifuwarabe_shogithink.pure.control;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.listen.genkyoku;
using kifuwarabe_shogithink.pure.logger;
using kifuwarabe_shogiwin.consolegame;
using kifuwarabe_shogiwin.consolegame.console;
using kifuwarabe_shogiwin.consolegame.console.command;
using kifuwarabe_shogiwin.consolegame.machine;
using kifuwarabe_shogiwin.project;
using kifuwarabe_shogiwin.speak;
using System;
using System.Collections.Generic;
#else
using Grayscale.Kifuwarabi.Entities.Logging;
using Grayscale.Kifuwarabi.UseCases;
using kifuwarabe_shogithink.pure;
using kifuwarabe_shogithink.pure.com;
using kifuwarabe_shogithink.pure.control;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.listen.genkyoku;
using kifuwarabe_shogithink.pure.logger;
using kifuwarabe_shogiwin.consolegame;
using kifuwarabe_shogiwin.consolegame.console;
using kifuwarabe_shogiwin.consolegame.console.command;
using kifuwarabe_shogiwin.consolegame.machine;
using kifuwarabe_shogiwin.project;
using kifuwarabe_shogiwin.speak;
using Nett;
using System;
using System.Collections.Generic;
using System.IO;
#endif

namespace kifuwarabe_shogiwin
{
    public class Program
    {

        /// <summary>
        /// ここからコンソール・アプリケーションが始まるぜ☆（＾▽＾）
        /// 
        /// ＰＣのコンソール画面のプログラムなんだぜ☆（＾▽＾）
        /// Ｕｎｉｔｙでは中身は要らないぜ☆（＾～＾）
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            var playing = new Playing();

            // （手順２）きふわらべの応答は、文字列になって　ここに入るぜ☆（＾▽＾）
            // syuturyoku.ToContents() メソッドで中身を取り出せるぜ☆（＾～＾）
            IHyojiMojiretu hyoji = PureAppli.syuturyoku1;
            Interproject.project = new WinconsoleProject();

            if (PureAppli.TryFail_Init())
            {
                Logger.Flush(hyoji);
                throw new Exception(hyoji.ToContents());
            }

            // コンソールゲーム用の設定上書き
            ConsolegameSettei.Init_PureAppliOverride();



            // まず最初に「USI\n」が届くかどうかを判定☆（＾～＾）
            Util_ConsoleGame.ReadCommandline(hyoji);
            if (CommandlineState.commandline=="usi")
            {
                // 「将棋所」で本将棋を指す想定☆（＾～＾）
                // CommandA.Atmark("@USI9x9", hyoji);

                PureSettei.usi = true;
                PureSettei.fenSyurui = FenSyurui.sfe_n;

                PureSettei.p1Com = false;
                PureSettei.p2Com = false;
                PureSettei.tobikikiTukau = true; // FIXME: 飛び利きはまだ不具合修正されていないぜ☆（＾～＾）
                ComSettei.himodukiHyokaTukau = true; // FIXME: 紐付き評価は、使うとしておこう☆（＾～＾）
                // ルールを確定してから　局面を作れだぜ☆（＾～＾）
                LisGenkyoku.SetRule(
                    GameRule.HonShogi, 9, 9,
@"シウネイライネウシ
　キ　　　　　ゾ　
ヒヒヒヒヒヒヒヒヒ
　　　　　　　　　
　　　　　　　　　
　　　　　　　　　
ひひひひひひひひひ
　ぞ　　　　　き　
しうねいらいねうし"
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

                var profilePath = System.Configuration.ConfigurationManager.AppSettings["Profile"];
                var toml = Toml.ReadFile(Path.Combine(profilePath, "Engine.toml"));

                var engineName = toml.Get<TomlTable>("Engine").Get<string>("Name");
                Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                var engineAuthor = toml.Get<TomlTable>("Engine").Get<string>("Author");

                playing.UsiOk(CommandlineState.commandline, $"{engineName} {version.Major}.{version.Minor}.{version.Build}", engineAuthor, hyoji);
            }
            else
            {
                SpkNaration.Speak_TitleGamen(hyoji);// とりあえず、タイトル画面表示☆（＾～＾）
                Logger.Flush(hyoji);
            }

            //Face_Kifuwarabe.Execute("", Option_Application.Kyokumen, syuturyoku); // 空打ちで、ゲームモードに入るぜ☆（＾▽＾）
            // 空打ちで、ゲームモードに入るぜ☆（＾▽＾）
            if (Console01.TryFail_Execute(playing, CommandlineState.commandline, hyoji))
            {
                Logger.Flush(hyoji);
                Console.WriteLine("おわり☆（＾▽＾）");
                Console.ReadKey();
                //throw new Exception(syuturyoku.ToContents());
            }
            // 開発モードでは、ユーザー入力を待機するぜ☆（＾▽＾）

            // （手順５）アプリケーション終了時に呼び出せだぜ☆（＾▽＾）！
            Console02.End_Application(PureAppli.syuturyoku1);
        }

    }
}
