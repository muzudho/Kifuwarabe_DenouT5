#if DEBUG
using kifuwarabe_shogithink.pure;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.genkyoku;
using kifuwarabe_shogithink.pure.ikkyoku;
using kifuwarabe_shogithink.pure.logger;
#else
using kifuwarabe_shogithink.pure;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.genkyoku;
using kifuwarabe_shogithink.pure.ikkyoku;
using kifuwarabe_shogithink.pure.logger;
#endif

namespace kifuwarabe_shogiwin.consolegame.console.command
{
    public static class CommandR
    {
        public static void Result( IHyojiMojiretu hyoji, CommandMode commandMode)
        {
            switch (commandMode)
            {
                case CommandMode.NingenYoConsoleGame:
                    {
                        switch (PureMemory.gky_kekka)
                        {
                            case TaikyokuKekka.Hikiwake:
                                {
                                    hyoji.AppendLine("┌─────────────────結　果─────────────────┐");
                                    hyoji.AppendLine("│　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　│");
                                    hyoji.AppendLine("│　　　　　　　　　　　　　　　　 引き分け 　　　　　　　　　　　　　　　　　│");
                                    hyoji.AppendLine("│　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　│");
                                    hyoji.AppendLine("└─────────────────────────────────────┘");
                                }
                                break;
                            case TaikyokuKekka.Taikyokusya1NoKati:
                                {
                                    hyoji.AppendLine("┌─────────────────結　果─────────────────┐");
                                    hyoji.AppendLine("│　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　│");
                                    hyoji.AppendLine("│　　　　　　　　　　　　　　　対局者１の勝ち　　　　　　　　　　　　　　　│");
                                    hyoji.AppendLine("│　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　│");
                                    hyoji.AppendLine("└─────────────────────────────────────┘");
                                }
                                break;
                            case TaikyokuKekka.Taikyokusya2NoKati:
                                {
                                    hyoji.AppendLine("┌─────────────────結　果─────────────────┐");
                                    hyoji.AppendLine("│　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　│");
                                    hyoji.AppendLine("│　　　　　　　　　　　　　　　対局者２の勝ち　　　　　　　　　　　　　　　│");
                                    hyoji.AppendLine("│　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　│");
                                    hyoji.AppendLine("└─────────────────────────────────────┘");
                                }
                                break;
                            case TaikyokuKekka.Karappo://thru
                            default:
                                break;
                        }
                    }
                    break;
                case CommandMode.TusinYo:
                    {
                        // 列挙型をそのまま出力するぜ☆（＾▽＾）
                        hyoji.Append("< result, kekka = ");
                        hyoji.AppendLine(PureMemory.gky_kekka.ToString());
                    }
                    break;
                default://thru
                case CommandMode.NigenYoConsoleKaihatu:
                    {
                        switch (PureMemory.gky_kekka)
                        {
                            case TaikyokuKekka.Hikiwake: hyoji.AppendLine("結果：　引き分け"); break;
                            case TaikyokuKekka.Taikyokusya1NoKati: hyoji.AppendLine("結果：　対局者１の勝ち"); break;
                            case TaikyokuKekka.Taikyokusya2NoKati: hyoji.AppendLine("結果：　対局者２の勝ち"); break;
                            case TaikyokuKekka.Karappo://thru
                            default:
                                break;
                        }
                    }
                    break;
            }
        }

        public static bool Try_Rnd(
#if DEBUG
            IDebugMojiretu reigai1
#endif
            )
        {
            if (!IkkyokuOpe.Try_Rnd(
#if DEBUG
                reigai1
#endif
                ))
            {
                return false;
            }
            return true;
        }

    }
}
