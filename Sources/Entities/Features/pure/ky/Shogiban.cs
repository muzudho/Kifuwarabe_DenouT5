#if DEBUG
using kifuwarabe_shogithink.pure.ky.bb;
using kifuwarabe_shogithink.pure.ky.tobikiki;

using System.Diagnostics;
#else
using kifuwarabe_shogithink.pure.com;
using kifuwarabe_shogithink.pure.com.hyoka;
using kifuwarabe_shogithink.pure.genkyoku;
using kifuwarabe_shogithink.pure.ky.bb;
using kifuwarabe_shogithink.pure.ky.tobikiki;
using System.Diagnostics;
#endif


namespace kifuwarabe_shogithink.pure.ky
{
    /// <summary>
    /// 盤を使ったデータまとめ☆（＾～＾）
    /// ４つの居場所盤を閉じ込めているぜ☆（＾▽＾）
    /// 
    /// 主な操作は次の２つ
    /// （１）盤上から駒を　取り除く
    /// （２）盤上に駒を　　置く
    /// </summary>
    public class Shogiban
    {
        public class YomiShogiban
        {
            public YomiShogiban(Shogiban sg)
            {
                hontai_ = sg;
                yomi_ojamaBanItiran_ = new OjamaBan.YomiOjamaBan[] {
                    hontai_.ojamaBan_ha45.yomiOjamaBan,
                    hontai_.ojamaBan_hs45.yomiOjamaBan,
                    hontai_.ojamaBan_ht90.yomiOjamaBan,
                };
            }
            Shogiban hontai_;

            /// <summary>
            /// お邪魔盤の一覧
            /// </summary>
            OjamaBan.YomiOjamaBan[] yomi_ojamaBanItiran_;

            public IbashoBan.YomiIbashoBan yomiIbashoBan
            {
                get
                {
                    return hontai_.ibashoBan_yk00.yomiIbashoBan;
                }
            }
            public KikiBan.YomiKikiBan yomiKikiBan
            {
                get
                {
                    return hontai_.kikiBan.yomiKikiBan;
                }
            }
            public OjamaBan.YomiOjamaBan GetYomiOjamaBan(OjamaBanSyurui ojamaBanSyurui)
            {
                return yomi_ojamaBanItiran_[(int)ojamaBanSyurui];
            }
        }
        #region 生成
        public Shogiban()
        {
            ibashoBan_yk00 = new IbashoBan();

            ojamaBan_ha45 = new OjamaBan();
            ojamaBan_hs45 = new OjamaBan();
            ojamaBan_ht90 = new OjamaBan();

            kikiBan = new KikiBan();

            // クリアー
            Tukurinaosi_Clear();

            yomiShogiban = new YomiShogiban(this);
        }
        public Shogiban(Shogiban src)
        {
            ibashoBan_yk00 = new IbashoBan(src.ibashoBan_yk00);

            ojamaBan_ha45 = new OjamaBan(src.ojamaBan_ha45);
            ojamaBan_hs45 = new OjamaBan(src.ojamaBan_hs45);
            ojamaBan_ht90 = new OjamaBan(src.ojamaBan_ht90);

            kikiBan = new KikiBan(src.kikiBan);

            yomiShogiban = new YomiShogiban(this);
        }
        #endregion
        #region 作り直し
        /// <summary>
        /// 
        /// </summary>
        public void Tukurinaosi_RemakeIbasho()
        {
            // 居場所盤
            ibashoBan_yk00.Tukurinaosi_Remake();
        }
        /// <summary>
        /// 利きを作り直すぜ☆（＾～＾）
        /// 先に駒を置いておくこと☆（＾～＾）
        /// </summary>
        /// <param name="yomiKy"></param>
        public void Tukurinaosi_RemakeKiki()
        {
            //ibashoBan_yk00.Tukurinaosi_Copy()

            kikiBan.Tukurinaosi_Remake();
        }
        public void Tukurinaosi_Copy(Shogiban source)
        {
            // 居場所盤
            ibashoBan_yk00.Tukurinaosi_Copy(source.ibashoBan_yk00);
            // お邪魔盤
            ojamaBan_ha45.Tukurinaosi_Copy(source.ojamaBan_ha45);
            ojamaBan_hs45.Tukurinaosi_Copy(source.ojamaBan_hs45);
            ojamaBan_ht90.Tukurinaosi_Copy(source.ojamaBan_ht90);
            // 利き盤
            kikiBan.Tukurinaosi_Copy(source.kikiBan);
        }
        public void Tukurinaosi_Clear()
        {
            // 居場所盤
            ibashoBan_yk00.Tukurinaosi_Clear();

            // お邪魔盤
            ojamaBan_ha45.Tukurinaosi_Clear();
            ojamaBan_hs45.Tukurinaosi_Clear();
            ojamaBan_ht90.Tukurinaosi_Clear();

            // 利き盤
            kikiBan.Tukurinaosi_Clear();
        }
        #endregion

        #region プロパティ―
        public YomiShogiban yomiShogiban;

        public IbashoBan.YomiIbashoBan yomiIbashoBan_yoko
        {
            get
            {
                return ibashoBan_yk00.yomiIbashoBan;
            }
        }


        /// <summary>
        /// 駒の居場所の記憶
        /// ヨコ型ビットボード（基本）
        /// </summary>
        public IbashoBan ibashoBan_yk00 { get; private set; }
        /// <summary>
        /// 駒の居場所の記憶
        /// ナナメ型ビットボード（右肩下がり４５°）
        /// </summary>
        OjamaBan ojamaBan_ha45 { get; set; }
        /// <summary>
        /// 駒の居場所の記憶
        /// ナナメ型ビットボード（左肩下がり４５°）
        /// </summary>
        OjamaBan ojamaBan_hs45 { get; set; }
        /// <summary>
        /// 駒の居場所の記憶
        /// タテ型ビットボード（時計回り９０°）
        /// </summary>
        OjamaBan ojamaBan_ht90 { get; set; }
        /// <summary>
        /// 駒の利きの記憶
        /// </summary>
        public KikiBan kikiBan { get; private set; }
        #endregion

        /// <summary>
        /// 駒を取り除きます
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="km"></param>
        void TorinozokuKoma(Masu ms, Koma km)
        {
            ibashoBan_yk00.N240_TorinozokuKoma(km, ms);

            ojamaBan_ha45.N240_TorinozokuKoma(RotateChikanhyo.chikanHyo_ha45[(int)ms]);
            ojamaBan_hs45.N240_TorinozokuKoma(RotateChikanhyo.chikanHyo_hs45[(int)ms]);
            ojamaBan_ht90.N240_TorinozokuKoma(RotateChikanhyo.chikanHyo_ht90[(int)ms]);
        }
        void OkuKoma(Masu ms, Koma km)
        {
            ibashoBan_yk00.N240_OkuKoma(km, ms);

            ojamaBan_ha45.N240_OkuKoma(RotateChikanhyo.chikanHyo_ha45[(int)ms]);
            ojamaBan_hs45.N240_OkuKoma(RotateChikanhyo.chikanHyo_hs45[(int)ms]);
            ojamaBan_ht90.N240_OkuKoma(RotateChikanhyo.chikanHyo_ht90[(int)ms]);
        }



        Bitboard bbTmp_kiki_forOku = new Bitboard();
        Bitboard bbTmp_kikiAite_forOku = new Bitboard();
        /// <summary>
        /// 盤上の駒を置くぜ☆（＾▽＾）
        /// </summary>
        /// <param name="ms_t1"></param>
        /// <param name="km_t1"></param>
        /// <param name="updateKiki">利きを先に作るか、駒を先に並べるか、という循環が発生するのを防ぐために</param>
        public bool TryFail_OkuKoma(
            Masu ms_t1,
            Koma km_t1,
            bool updateKiki
#if DEBUG
            , IDebugMojiretu reigai1
#endif
            )
        {
            Debug.Assert(Conv_Koma.IsOk(km_t1), string.Format("置けない駒 km_t1 を置こうとしました。 km_t1={0}",km_t1));
            Debug.Assert(Conv_Masu.IsBanjo(ms_t1), string.Format("ms={0}", ms_t1));

            // 取り除いた駒は、どちらの対局者の物か☆（＾～＾）
            Taikyokusya tai_put = Med_Koma.KomaToTaikyokusya(km_t1);


            //──────────
            // とりあえず、盤に駒を置くんだぜ☆（＾～＾）
            //──────────
            OkuKoma(ms_t1, km_t1);

            //──────────
            // TODO: 駒が増えた現状に更新する
            //──────────
            if (updateKiki)
            {
                // 駒の利き割りの評価値を増やすならここで☆（＾～＾）

                //──────────
                // 駒が増えた後の、関連する飛び利きを消す
                //──────────
                // 置いた駒がある　縦列きりん、横列きりん、縦列いのしし、左上がりぞう、左下がりぞう　を探す
                foreach(Taikyokusya tai in Conv_Taikyokusya.itiran)
                {
                    Komasyurui[] ksAr = new Komasyurui[]
                    {
                        Komasyurui.K,
                        Komasyurui.Z,
                        Komasyurui.S,
                    };

                    foreach (Komasyurui ks in ksAr)
                    {
                        Koma km_reverse = Med_Koma.KomasyuruiAndTaikyokusyaToKoma(ks, tai);

                        //bbTmp_kiki_forOku.Clear();
                        BitboardsOmatome.KomanoUgokikataYk00.ToSet_Merge(
                            km_reverse,// 相手番の駒にして、利きを飛ばしてきている位置を調べるのに使う
                            ms_t1,// 升
                            bbTmp_kiki_forOku // リーガルムーブの盤面を、ここに入れるぜ☆（＾～＾）
                        );

                        Masu ms_atk;
                        while (bbTmp_kiki_forOku.Ref_PopNTZ(out ms_atk))// 立っているビットを降ろすぜ☆
                        {
                            kikiBan.TorinozokuKiki(km_reverse, ms_atk);
                        }
                    }
                }

                //*
                // 利きを増やすぜ☆（＾～＾）
                kikiBan.OkuKiki(
                    km_t1, // 駒
                    ms_t1 // 升
                    );
                //*/

            }

            // TODO: 駒割り評価値を増やすならここだぜ☆（＾～＾）

            return Pure.SUCCESSFUL_FALSE;
        }


        /// <summary>
        /// 盤上の駒を取り除く。
        /// 
        /// よくある問題
        /// ──────
        /// 
        /// （１）きりんＡ　の右上に　きりんＢ　を打つ。
        /// （２）きりんＢ　を取り除く。
        /// （３）このとき、きりんＢ　の利きも取り除くが、きりんＡ　と被っている利きもある。
        /// これを消してしまうと、利きが欠けた　きりんＡ　ができてしまい、整合性が取れない。
        /// 
        /// </summary>
        /// <param name="ms_ibasho">取り除く駒がある升</param>
        /// <param name="km_remove">取り除く駒</param>
        /// <param name="ms_mirainihaKomagaAru">（飛び利き更新用）</param>
        /// <param name="updateKiki">偽にすると、利きを更新しません</param>
        /// <param name="yomiGky"></param>
        /// <param name="reigai1"></param>
        /// <returns></returns>
        public bool TryFail_TorinozokuKoma(
            Masu ms_ibasho,
            Koma km_remove,
            Masu ms_mirainihaKomagaAru,
            bool updateKiki
            
#if DEBUG
            , IDebugMojiretu reigai1
#endif
            )
        {
            Debug.Assert(Conv_Koma.IsOk(km_remove), "km_remove not ok");
            Debug.Assert(Conv_Masu.IsBanjo(ms_ibasho), "");

            // 取り除いた駒は、どちらの対局者の物か☆（＾～＾）
            Taikyokusya tai_removed = Med_Koma.KomaToTaikyokusya(km_remove);

            //────────────────────────────────────────
            // （１）利き表をいじる前☆（＾▽＾）
            //────────────────────────────────────────

            //──────────
            // とりあえず、駒を取り除いた状態にするぜ☆（＾～＾）
            //──────────
            TorinozokuKoma(ms_ibasho, km_remove);

            //──────────
            // 駒が取り除かれた現状に更新する
            //──────────
            if (updateKiki)
            {
                // 駒の利き割りの評価値を減らすならここで☆（＾～＾）

                kikiBan.TorinozokuKiki(km_remove, ms_ibasho);

                //──────────
                // 駒が減った後の、関連する飛び利きを増やす
                //──────────
                // 置いた駒がある　縦列きりん、横列きりん、縦列いのしし、左上がりぞう、左下がりぞう　を探す
                foreach(Taikyokusya tai in Conv_Taikyokusya.itiran)
                {
                    Komasyurui[] ksAr = new Komasyurui[]
                    {
                        Komasyurui.K,
                        Komasyurui.Z,
                        Komasyurui.S,
                    };

                    foreach (Komasyurui ks in ksAr)
                    {
                        Koma km_forReverse = Med_Koma.KomasyuruiAndTaikyokusyaToKoma(ks, tai);

                        BitboardsOmatome.KomanoUgokikataYk00.ToSet_Merge(
                            km_forReverse,// 相手番の駒にして、利きを飛ばしてきている位置を調べるのに使う
                            ms_ibasho,// 升
                            bbTmp_kiki_forOku // リーガルムーブの盤面を、ここに入れるぜ☆（＾～＾）
                        );

                        Masu ms_atk;
                        while (bbTmp_kiki_forOku.Ref_PopNTZ(out ms_atk))// 立っているビットを降ろすぜ☆
                        {
                            kikiBan.OkuKiki(km_forReverse, ms_atk);
                        }
                    }
                }
            }

            return Pure.SUCCESSFUL_FALSE;
        }

    }
}
