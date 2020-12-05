#if DEBUG
using kifuwarabe_shogithink.pure.logger;
using System.Collections.Generic;
using System.IO;
using System.Text;
#else
using kifuwarabe_shogithink.pure.logger;
using System.Collections.Generic;
using System.IO;
using System.Text;
#endif

namespace kifuwarabe_shogiwin.consolegame.console.command
{
    public static class CommandA
    {
        public static void Atmark(string commandline, IHyojiMojiretu hyoji)
        {
            // 頭の「@」を取って、末尾に「.txt」を付けた文字は☆（＾▽＾）
            CommandlineState.commandBufferName = commandline.Substring("@".Length);

            StringBuilder sb = new StringBuilder();
            sb.Append("Command/");
            sb.Append(CommandlineState.commandBufferName);
            sb.Append(".txt");
            string file = sb.ToString();

            CommandlineState.commandBuffer.Clear();
            if (File.Exists(file)) // Visual Studioで「Unity」とか新しい構成を新規作成した場合は、出力パスも合わせろだぜ☆（＾▽＾）
            {
                CommandlineState.commandBuffer.AddRange(new List<string>(File.ReadAllLines(file)));
            }
            // 該当しないものは無視だぜ☆（＾▽＾）
        }
    }
}
