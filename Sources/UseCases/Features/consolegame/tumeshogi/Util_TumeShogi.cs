#if DEBUG
using kifuwarabe_shogithink.pure;
using kifuwarabe_shogithink.pure.com;
using kifuwarabe_shogithink.pure.control;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.listen.genkyoku;

using kifuwarabe_shogithink.pure.move;
using System.Collections.Generic;
#else
using kifuwarabe_shogithink.pure;
using kifuwarabe_shogithink.pure.com;
using kifuwarabe_shogithink.pure.control;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.listen.genkyoku;
using kifuwarabe_shogithink.pure.move;
using System.Collections.Generic;
#endif

namespace kifuwarabe_shogiwin.consolegame.tumeshogi
{
    public abstract class Util_TumeShogi
    {
        /// <summary>
        /// 詰将棋を用意するぜ☆
        /// </summary>
        public static bool Try_TumeShogi(FenSyurui f, int bango, out int out_nantedume
#if DEBUG
            , IDebugMojiretu reigai1
#endif
            )
        {
            // FIXME: 終わったら元に戻したいが☆（＾～＾）
            PureSettei.char_playerN[(int)Taikyokusya.T1] = MoveCharacter.TansakuNomi;
            PureSettei.char_playerN[(int)Taikyokusya.T2] = MoveCharacter.TansakuNomi;
            ComSettei.sikoJikan = 60000; // とりあえず 60 秒ぐらい☆
            ComSettei.sikoJikanRandom = 0;
            ComSettei.johoJikan = 0; // 情報全部出すぜ☆

            // 詰め手数 + 1 にしないと、詰んでるか判断できないぜ☆（＾▽＾）
            //int motonoSaidaiFukasa = Option_Application.Optionlist.SaidaiFukasa;

            switch (bango)
            {
                #region 1手詰め
                case 0:
                    {
                        out_nantedume = 1;
                        LisGenkyoku.SetRule(
                            GameRule.DobutuShogi, 3, 4,
                            "　ラ　" +
                            "き　ひ" +
                            "　ら　" +
                            "　　　"
                            , new Dictionary<Motigoma, int>()
                            {
                                    { Motigoma.K,0 },
                                    { Motigoma.Z,0 },
                                    { Motigoma.H,1 },
                                    { Motigoma.k,0 },
                                    { Motigoma.z,0 },
                                    { Motigoma.h,0 },
                            }
                        );
                    }
                    break;
#endregion
#region 1手詰め
                case 1:
                    {
                        out_nantedume = 1;
                        LisGenkyoku.SetRule(
                            GameRule.DobutuShogi, 3, 4,
                            "　　ラ" +
                            "き　　" +
                            "　ら　" +
                            "　　　"
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
                    }
                    break;
#endregion
#region 3手詰め
                case 2:
                    {
                        out_nantedume = 3;
                        LisGenkyoku.SetRule(
                            GameRule.DobutuShogi, 3, 4,
                            "　ゾラ" +
                            "　　　" +
                            "ぞ　　" +
                            "ら　　"
                            , new Dictionary<Motigoma, int>()
                            {
                                    { Motigoma.K,0 },
                                    { Motigoma.Z,1 },
                                    { Motigoma.H,1 },
                                    { Motigoma.k,0 },
                                    { Motigoma.z,0 },
                                    { Motigoma.h,0 },
                            }
                        );
                    }
                    break;
#endregion
#region 1手詰め
                case 3:
                    {
                        out_nantedume = 1;
                        LisGenkyoku.SetRule(
                            GameRule.DobutuShogi, 3, 4,
                            "　ゾ　" +
                            "　ぞラ" +
                            "ぞ　　" +
                            "ら　　"
                            , new Dictionary<Motigoma, int>()
                            {
                                    { Motigoma.K,0 },
                                    { Motigoma.Z,0 },
                                    { Motigoma.H,1 },
                                    { Motigoma.k,0 },
                                    { Motigoma.z,0 },
                                    { Motigoma.h,0 },
                            }
                        );
                    }
                    break;
#endregion
#region 1手詰め
                default:
                    {
                        out_nantedume = 1;
                        LisGenkyoku.SetRule(
                            GameRule.DobutuShogi, 3, 4,
                            "　ラ　" +
                            "き　き" +
                            "　にら" +
                            "ぞひぞ"
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
                    }
                    break;
#endregion
            }
            return true;
        }
    }
}
