#if DEBUG
using System;
#else
using System;
#endif

namespace kifuwarabe_shogithink.pure.ky.tobikiki
{
    public enum TobikikiDirection
    {
        /// <summary>
        /// きりん縦
        /// </summary>
        KT,
        /// <summary>
        /// きりん横
        /// </summary>
        KY,
        /// <summary>
        /// いのしし
        /// </summary>
        S,
        /// <summary>
        /// ぞう左上がり
        /// </summary>
        ZHa,
        /// <summary>
        /// ぞう左下がり
        /// </summary>
        ZHs,
        /// <summary>
        /// 長い利き無し
        /// </summary>
        None,
    }

    public static class Conv_TobikikiDirection
    {
        public static TobikikiDirection[] tobikikiDirectionItiran = new TobikikiDirection[]
        {
            TobikikiDirection.KT,
            TobikikiDirection.KY,
            TobikikiDirection.S,
            TobikikiDirection.ZHa,
            TobikikiDirection.ZHs,
        };
        /// <summary>
        /// [飛び利きの方向]
        /// </summary>
        public static OjamaBanSyurui[] ojamaBanSyuruiItiran = new OjamaBanSyurui[]
        {
            OjamaBanSyurui.Ht90,
            OjamaBanSyurui.None,
            OjamaBanSyurui.Ht90,
            OjamaBanSyurui.Ha45,
            OjamaBanSyurui.Hs45,
        };

        public static bool[] sakasa_forZHs = new bool[]
        {
            false,
            false,
            false,
            false,
            true
        };

        public static int ToUgokikataArrayIndex(TobikikiDirection kikiDir)
        {
            switch (kikiDir)
            {
                case TobikikiDirection.KT:
                    return 0;
                case TobikikiDirection.KY:
                    return 1;
                case TobikikiDirection.S:
                    return 0;
                case TobikikiDirection.None:
                    return 0;
                case TobikikiDirection.ZHa:
                    return 0;
                case TobikikiDirection.ZHs:
                    return 1;
                default:
                    throw new Exception(string.Format("想定外の方向 dir={0}", kikiDir));
            }
        }

    }
}
