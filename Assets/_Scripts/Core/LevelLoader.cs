using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace myd.celeste
{
    public class LevelLoader 
    {
        private bool started = false;
        private Session session;
        private Vector2? startPosition;
        public Level Level { get; private set; }
        public SolidTiles solidTiles;
        public LevelLoader(Session session, Vector2? startPosition = null)
        {
            this.session = session;
            bool flag = startPosition == null;
            if (flag)
            {
                this.startPosition = session.RespawnPoint;
            }
            else
            {
                this.startPosition = startPosition;
            }
            this.Level = new Level();
            LoadingThread();

            Debug.Log("LoadingThread Finished");
        }

        //加载关卡数据
        private void LoadingThread()
        {
            //MapData表示整个地图的数据
            MapData mapData = this.session.MapData;
            
            AreaData areaData = AreaData.Get(this.session.Area.ID);
            //if (this.session.Area.ID == 0)
            //    SaveData.Instance.Assists.DashMode = Assists.DashModes.Normal;
            //this.Level.Add((Monocle.Renderer)(this.Level.Background = new BackdropRenderer()));
            //this.Level.Add((Entity)new DustEdges());
            //this.Level.Add((Entity)new WaterSurface());
            //this.Level.Add((Entity)new MirrorSurfaces());
            //this.Level.Add((Entity)new GlassBlockBg());
            //this.Level.Add((Entity)new LightningRenderer());
            //this.Level.Add((Entity)new SeekerBarrierRenderer());
            this.Level.Background = new BackdropRenderer();
            this.Level.BackgroundColor = mapData.BackgroundColor;
            this.Level.Background.Backdrops = mapData.CreateBackdrops(mapData.Background);
            foreach (Backdrop backdrop in this.Level.Background.Backdrops)
            {
                backdrop.Renderer = this.Level.Background;
            }
            //加载前景地图
            //this.Level.Foreground.Backdrops = mapData.CreateBackdrops(mapData.Foreground);
            //foreach (Backdrop backdrop in this.Level.Foreground.Backdrops)
            //{
            //    backdrop.Renderer = this.Level.Foreground;
            //}
            
            Rectangle tileBounds1 = mapData.TileBounds;
            Gfx.FGAutotiler.LevelBounds.Clear();
            VirtualMap<char> data1 = new VirtualMap<char>(tileBounds1.Width, tileBounds1.Height, '0');
            VirtualMap<char> data2 = new VirtualMap<char>(tileBounds1.Width, tileBounds1.Height, '0');
            VirtualMap<bool> virtualMap = new VirtualMap<bool>(tileBounds1.Width, tileBounds1.Height, false);
            Regex regex = new Regex("\\r\\n|\\n\\r|\\n|\\r");

            foreach (LevelData level in mapData.Levels)
            {
                Rectangle tileBounds2 = level.TileBounds;
                int left1 = tileBounds2.Left;
                tileBounds2 = level.TileBounds;
                int top1 = tileBounds2.Top;
                string[] strArray1 = regex.Split(level.Bg);
                for (int index1 = top1; index1 < top1 + strArray1.Length; ++index1)
                {
                    for (int index2 = left1; index2 < left1 + strArray1[index1 - top1].Length; ++index2)
                        data1[index2 - tileBounds1.X, index1 - tileBounds1.Y] = strArray1[index1 - top1][index2 - left1];
                }

                string[] strArray2 = regex.Split(level.Solids);
                for (int index1 = top1; index1 < top1 + strArray2.Length; ++index1)
                {
                    for (int index2 = left1; index2 < left1 + strArray2[index1 - top1].Length; ++index2)
                        data2[index2 - tileBounds1.X, index1 - tileBounds1.Y] = strArray2[index1 - top1][index2 - left1];
                }
                tileBounds2 = level.TileBounds;
                int left2 = tileBounds2.Left;
                while (true)
                {
                    int num1 = left2;
                    tileBounds2 = level.TileBounds;
                    int right = tileBounds2.Right;
                    if (num1 < right)
                    {
                        tileBounds2 = level.TileBounds;
                        int top2 = tileBounds2.Top;
                        while (true)
                        {
                            int num2 = top2;
                            tileBounds2 = level.TileBounds;
                            int bottom = tileBounds2.Bottom;
                            if (num2 < bottom)
                            {
                                virtualMap[left2 - tileBounds1.Left, top2 - tileBounds1.Top] = true;
                                ++top2;
                            }
                            else
                                break;
                        }
                        ++left2;
                    }
                    else
                        break;
                }
                Gfx.FGAutotiler.LevelBounds.Add(new Rectangle(level.TileBounds.X - tileBounds1.X, level.TileBounds.Y - tileBounds1.Y, level.TileBounds.Width, level.TileBounds.Height));
            }

            foreach (Rectangle rectangle in mapData.Filler)
            {
                for (int left = rectangle.Left; left < rectangle.Right; ++left)
                {
                    for (int top = rectangle.Top; top < rectangle.Bottom; ++top)
                    {
                        char ch1 = '0';
                        if (rectangle.Top - tileBounds1.Y > 0)
                        {
                            char ch2 = data2[left - tileBounds1.X, rectangle.Top - tileBounds1.Y - 1];
                            if (ch2 != '0')
                                ch1 = ch2;
                        }
                        if (ch1 == '0' && rectangle.Left - tileBounds1.X > 0)
                        {
                            char ch2 = data2[rectangle.Left - tileBounds1.X - 1, top - tileBounds1.Y];
                            if (ch2 != '0')
                                ch1 = ch2;
                        }
                        if (ch1 == '0' && rectangle.Right - tileBounds1.X < tileBounds1.Width - 1)
                        {
                            char ch2 = data2[rectangle.Right - tileBounds1.X, top - tileBounds1.Y];
                            if (ch2 != '0')
                                ch1 = ch2;
                        }
                        if (ch1 == '0' && rectangle.Bottom - tileBounds1.Y < tileBounds1.Height - 1)
                        {
                            char ch2 = data2[left - tileBounds1.X, rectangle.Bottom - tileBounds1.Y];
                            if (ch2 != '0')
                                ch1 = ch2;
                        }
                        if (ch1 == '0')
                            ch1 = '1';
                        data2[left - tileBounds1.X, top - tileBounds1.Y] = ch1;
                        virtualMap[left - tileBounds1.X, top - tileBounds1.Y] = true;
                    }
                }
            }
            using (List<LevelData>.Enumerator enumerator = mapData.Levels.GetEnumerator())
            {
            label_85:
                while (enumerator.MoveNext())
                {
                    LevelData current = enumerator.Current;
                    Rectangle tileBounds2 = current.TileBounds;
                    int left1 = tileBounds2.Left;
                    while (true)
                    {
                        int num1 = left1;
                        tileBounds2 = current.TileBounds;
                        int right = tileBounds2.Right;
                        if (num1 < right)
                        {
                            tileBounds2 = current.TileBounds;
                            int top = tileBounds2.Top;
                            char ch1 = data1[left1 - tileBounds1.X, top - tileBounds1.Y];
                            for (int index = 1; index < 4 && !virtualMap[left1 - tileBounds1.X, top - tileBounds1.Y - index]; ++index)
                                data1[left1 - tileBounds1.X, top - tileBounds1.Y - index] = ch1;
                            tileBounds2 = current.TileBounds;
                            int num2 = tileBounds2.Bottom - 1;
                            char ch2 = data1[left1 - tileBounds1.X, num2 - tileBounds1.Y];
                            for (int index = 1; index < 4 && !virtualMap[left1 - tileBounds1.X, num2 - tileBounds1.Y + index]; ++index)
                                data1[left1 - tileBounds1.X, num2 - tileBounds1.Y + index] = ch2;
                            ++left1;
                        }
                        else
                            break;
                    }
                    tileBounds2 = current.TileBounds;
                    int num3 = tileBounds2.Top - 4;
                    while (true)
                    {
                        int num1 = num3;
                        tileBounds2 = current.TileBounds;
                        int num2 = tileBounds2.Bottom + 4;
                        if (num1 < num2)
                        {
                            tileBounds2 = current.TileBounds;
                            int left2 = tileBounds2.Left;
                            char ch1 = data1[left2 - tileBounds1.X, num3 - tileBounds1.Y];
                            for (int index = 1; index < 4 && !virtualMap[left2 - tileBounds1.X - index, num3 - tileBounds1.Y]; ++index)
                                data1[left2 - tileBounds1.X - index, num3 - tileBounds1.Y] = ch1;
                            tileBounds2 = current.TileBounds;
                            int num4 = tileBounds2.Right - 1;
                            char ch2 = data1[num4 - tileBounds1.X, num3 - tileBounds1.Y];
                            for (int index = 1; index < 4 && !virtualMap[num4 - tileBounds1.X + index, num3 - tileBounds1.Y]; ++index)
                                data1[num4 - tileBounds1.X + index, num3 - tileBounds1.Y] = ch2;
                            ++num3;
                        }
                        else
                            goto label_85;
                    }
                }
            }
            using (List<LevelData>.Enumerator enumerator = mapData.Levels.GetEnumerator())
            {
            label_100:
                while (enumerator.MoveNext())
                {
                    LevelData current = enumerator.Current;
                    Rectangle tileBounds2 = current.TileBounds;
                    int left = tileBounds2.Left;
                    while (true)
                    {
                        int num1 = left;
                        tileBounds2 = current.TileBounds;
                        int right = tileBounds2.Right;
                        if (num1 < right)
                        {
                            tileBounds2 = current.TileBounds;
                            int top = tileBounds2.Top;
                            if (data2[left - tileBounds1.X, top - tileBounds1.Y] == '0')
                            {
                                for (int index = 1; index < 8; ++index)
                                    virtualMap[left - tileBounds1.X, top - tileBounds1.Y - index] = true;
                            }
                            tileBounds2 = current.TileBounds;
                            int num2 = tileBounds2.Bottom - 1;
                            if (data2[left - tileBounds1.X, num2 - tileBounds1.Y] == '0')
                            {
                                for (int index = 1; index < 8; ++index)
                                    virtualMap[left - tileBounds1.X, num2 - tileBounds1.Y + index] = true;
                            }
                            ++left;
                        }
                        else
                            goto label_100;
                    }
                }
            }
            using (List<LevelData>.Enumerator enumerator = mapData.Levels.GetEnumerator())
            {
            label_122:
                while (enumerator.MoveNext())
                {
                    LevelData current = enumerator.Current;
                    Rectangle tileBounds2 = current.TileBounds;
                    int left1 = tileBounds2.Left;
                    while (true)
                    {
                        int num1 = left1;
                        tileBounds2 = current.TileBounds;
                        int right = tileBounds2.Right;
                        if (num1 < right)
                        {
                            tileBounds2 = current.TileBounds;
                            int top = tileBounds2.Top;
                            char ch1 = data2[left1 - tileBounds1.X, top - tileBounds1.Y];
                            for (int index = 1; index < 4 && !virtualMap[left1 - tileBounds1.X, top - tileBounds1.Y - index]; ++index)
                                data2[left1 - tileBounds1.X, top - tileBounds1.Y - index] = ch1;
                            tileBounds2 = current.TileBounds;
                            int num2 = tileBounds2.Bottom - 1;
                            char ch2 = data2[left1 - tileBounds1.X, num2 - tileBounds1.Y];
                            for (int index = 1; index < 4 && !virtualMap[left1 - tileBounds1.X, num2 - tileBounds1.Y + index]; ++index)
                                data2[left1 - tileBounds1.X, num2 - tileBounds1.Y + index] = ch2;
                            ++left1;
                        }
                        else
                            break;
                    }
                    tileBounds2 = current.TileBounds;
                    int num3 = tileBounds2.Top - 4;
                    while (true)
                    {
                        int num1 = num3;
                        tileBounds2 = current.TileBounds;
                        int num2 = tileBounds2.Bottom + 4;
                        if (num1 < num2)
                        {
                            tileBounds2 = current.TileBounds;
                            int left2 = tileBounds2.Left;
                            char ch1 = data2[left2 - tileBounds1.X, num3 - tileBounds1.Y];
                            for (int index = 1; index < 4 && !virtualMap[left2 - tileBounds1.X - index, num3 - tileBounds1.Y]; ++index)
                                data2[left2 - tileBounds1.X - index, num3 - tileBounds1.Y] = ch1;
                            tileBounds2 = current.TileBounds;
                            int num4 = tileBounds2.Right - 1;
                            char ch2 = data2[num4 - tileBounds1.X, num3 - tileBounds1.Y];
                            for (int index = 1; index < 4 && !virtualMap[num4 - tileBounds1.X + index, num3 - tileBounds1.Y]; ++index)
                                data2[num4 - tileBounds1.X + index, num3 - tileBounds1.Y] = ch2;
                            ++num3;
                        }
                        else
                            goto label_122;
                    }
                }
            }
            Vector2 position = new Vector2((float)tileBounds1.X, (float)tileBounds1.Y) * 8f;
            RandomUtil.PushRandom(mapData.LoadSeed);
            Level level1 = this.Level;
            Level level2 = this.Level;
            BackgroundTiles backgroundTiles1;
            BackgroundTiles backgroundTiles2 = backgroundTiles1 = new BackgroundTiles(position, data1);
            //BackgroundTiles backgroundTiles3 = backgroundTiles1;
            level2.BgTiles = backgroundTiles1;
            //BackgroundTiles backgroundTiles4 = backgroundTiles3;
            //level2.BgTiles = backgroundTiles4;
            //level1.Add((Entity)backgroundTiles4);
            //Level level3 = this.Level;
            //Level level4 = this.Level;
            solidTiles = new SolidTiles(position, data2);
            //level3.Add((Entity)solidTiles4);
            //this.Level.BgData = data1;
            //this.Level.SolidsData = data2;
            RandomUtil.PopRandom();
            //this.Level.FgTilesLightMask = new TileGrid(8, 8, tileBounds1.Width, tileBounds1.Height);
            //this.Level.FgTilesLightMask.Color = Color.black;
            //foreach (LevelData level5 in mapData.Levels)
            //{
            //    int left = level5.TileBounds.Left;
            //    int top = level5.TileBounds.Top;
            //    int width = level5.TileBounds.Width;
            //    int height = level5.TileBounds.Height;
            //    if (!string.IsNullOrEmpty(level5.BgTiles))
            //    {
            //        int[,] tiles = Util.ReadCSVIntGrid(level5.BgTiles, width, height);
            //        backgroundTiles2.Tiles.Overlay(Gfx.SceneryTiles, tiles, left - tileBounds1.X, top - tileBounds1.Y);
            //    }
            //    if (!string.IsNullOrEmpty(level5.FgTiles))
            //    {
            //        int[,] tiles = Util.ReadCSVIntGrid(level5.FgTiles, width, height);
            //        solidTiles.Tiles.Overlay(Gfx.SceneryTiles, tiles, left - tileBounds1.X, top - tileBounds1.Y);
            //        this.Level.FgTilesLightMask.Overlay(Gfx.SceneryTiles, tiles, left - tileBounds1.X, top - tileBounds1.Y);
            //    }
            //}
            //if (areaData.OnLevelBegin != null)
            //    areaData.OnLevelBegin(this.Level);
            //this.Level.StartPosition = this.startPosition;
            //this.Level.Pathfinder = new Pathfinder(this.Level);
            //this.Loaded = true;
        }

        private VirtualMap<char> ReadMapChar(string tiles, int width, int height)
        {
            VirtualMap<char> mapData = new VirtualMap<char>(width, height, '0');
            int length = tiles.Length;
            int i = 0;
            int col = 0, row = 0;

            while (i < length)
            {
                char val = tiles[i++];
                if (val == '\r' || val == '\n')
                {
                    if (val == '\n')
                    {
                        row++;
                        col = 0;
                    }
                    continue;
                }
                mapData[col++, row] = val;
            }
            return mapData;
        }

    }
}