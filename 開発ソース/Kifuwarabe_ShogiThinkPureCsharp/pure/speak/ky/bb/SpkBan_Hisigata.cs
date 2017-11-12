#if DEBUG
using kifuwarabe_shogithink.pure;
using kifuwarabe_shogithink.pure.genkyoku;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.ky.bb;
using kifuwarabe_shogithink.pure.ky.tobikiki;
using kifuwarabe_shogithink.pure.logger;
using System;
#else
using kifuwarabe_shogithink.pure;
using kifuwarabe_shogithink.pure.genkyoku;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.ky.bb;
using kifuwarabe_shogithink.pure.ky.tobikiki;
using kifuwarabe_shogithink.pure.logger;
using System;
#endif

namespace kifuwarabe_shogithink.pure.speak.ky.bb
{
    /// <summary>
    /// デバッグ時によく使う、ビットボードの簡易表示☆（＾～＾）
    /// </summary>
    public static class SpkBan_Hisigata
    {
        public delegate string DLGT_DrawMasu(Masu ms);


        public static void ScanAndHyojiHisigata(
            DLGT_DrawMasu dlgt_DrawMasu,
            TobikikiDirection kikiDir, IHyojiMojiretu hyoji)
        {
            int dLen = PureSettei.banNanameDanLen;

            // 最大幅を算出
            int maxHaba = 0;
            for (int d = 0; d < dLen; d++)
            {
                int haba = Util_Tobikiki.GetNanameDanHaba(d, kikiDir);
                maxHaba = Math.Max(haba, maxHaba);
            }

            //hyoji.AppendLine("ひし形表示");
            int iMs = 0;
            for (int d = 0; d < dLen; d++)
            {
                int haba = Util_Tobikiki.GetNanameDanHaba(d, kikiDir);

                // インデント
                for (int i = maxHaba - haba; 0 < i; i--)
                {
                    hyoji.Append(" ");
                }

                for (int x = 0; x < haba; x++)
                {
                    hyoji.Append(dlgt_DrawMasu((Masu)iMs));
                    iMs++;
                }
                hyoji.AppendLine();
            }

            hyoji.AppendLine();
        }

        public static void Setumei_yk00(string header, Bitboard bb, IHyojiMojiretu hyoji)
        {
            if (header != "")
            {
                hyoji.AppendLine(header);
            }
            int dLen = PureSettei.banTateHaba;

            int iMs = 0;
            for (int d = 0; d < dLen; d++)
            {
                int haba = Util_Tobikiki.GetNanameDanHaba(d, TobikikiDirection.KY);

                for (int x = 0; x < haba; x++)
                {
                    hyoji.Append(bb.IsOn((Masu)iMs) ? "〇" : "・");
                    iMs++;
                }
                hyoji.AppendLine();
            }
        }

        public static void Setumei( TobikikiDirection kikiDir, IHyojiMojiretu hyoji)
        {
            OjamaBanSyurui ojamaBanSyurui = Conv_TobikikiDirection.ojamaBanSyuruiItiran[(int)kikiDir];
            switch (kikiDir)
            {
                case TobikikiDirection.KT:
                case TobikikiDirection.S:
                    {
                        OjamaBan.YomiOjamaBan yomiOjamaBan = PureMemory.gky_ky.yomiKy.yomiShogiban.GetYomiOjamaBan(ojamaBanSyurui);
                        int dLen = PureSettei.banYokoHaba;

                        hyoji.AppendLine("９０°回転");
                        int iMs = 0;
                        for (int d = 0; d < dLen; d++)
                        {
                            int haba = Util_Tobikiki.GetNanameDanHaba(d, kikiDir);

                            for (int x = 0; x < haba; x++)
                            {
                                hyoji.Append(yomiOjamaBan.ExistsBB((Masu)iMs) ? "〇" : "・");
                                iMs++;
                            }
                            hyoji.AppendLine();
                        }
                    }
                    break;
                case TobikikiDirection.KY:
                case TobikikiDirection.None:
                    {
                        // お邪魔盤は無いので、将棋盤から取ってくる
                        Bitboard bb_shogi = PureMemory.gky_ky.yomiKy.yomiShogiban.yomiIbashoBan.CloneKomaZenbuBothTai();

                        Setumei_yk00("回転なし", bb_shogi, hyoji);
                    }
                    break;
                case TobikikiDirection.ZHa:
                case TobikikiDirection.ZHs:
                    {
                        OjamaBan.YomiOjamaBan yomiOjamaBan = PureMemory.gky_ky.yomiKy.yomiShogiban.GetYomiOjamaBan(ojamaBanSyurui);

                        hyoji.AppendLine("４５°回転");
                        ScanAndHyojiHisigata((Masu ms)=>
                        {
                            return yomiOjamaBan.ExistsBB(ms) ? "〇" : "・";
                        },
                        kikiDir, hyoji);
                    }
                    break;
            }

        }
    }
}
