namespace kifuwarabe_shogiwin.consolegame.console.command
{
#if DEBUG
using kifuwarabe_shogithink.pure.logger;
using System.Collections.Generic;
using System.IO;
using System.Text;
#else
    using kifuwarabe_shogithink.pure.logger;
    using Nett;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
#endif

    public static class CommandA
    {
        public static void Atmark(ProgramSupport programSupport, string commandline, IHyojiMojiretu hyoji)
        {
            // 頭の「@」を取って、末尾に「.txt」を付けた文字は☆（＾▽＾）
            programSupport.commandBufferName = commandline.Substring("@".Length);

            var profilePath = System.Configuration.ConfigurationManager.AppSettings["Profile"];
            var toml = Toml.ReadFile(Path.Combine(profilePath, "Engine.toml"));
            var commandPath = toml.Get<TomlTable>("Resources").Get<string>("Command");
            string file = Path.Combine(profilePath, commandPath, $"{programSupport.commandBufferName}.txt");

            programSupport.commandBuffer.Clear();
            if (File.Exists(file)) // Visual Studioで「Unity」とか新しい構成を新規作成した場合は、出力パスも合わせろだぜ☆（＾▽＾）
            {
                programSupport.commandBuffer.AddRange(new List<string>(File.ReadAllLines(file)));
            }
            // 該当しないものは無視だぜ☆（＾▽＾）
        }
    }
}
