using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;

namespace myd.celeste.demo
{
    /// <summary>
    /// 章节
    /// </summary>
    public class Stage
    {
        public static Stage Instance = null;

        private readonly Regex regex = new Regex("\\r\\n|\\n\\r|\\n|\\r");
        private int _Id;            //章节ID
        private AreaMode _Mode;     //章节模式
        private MapData _MapData;   //章节数据

        private VirtualMap<char> data1;     //Bg数据
        private VirtualMap<char> data2;     //Fg数据
        private VirtualMap<bool> virtualMap;

        private List<Rectangle> _LevelBounds = new List<Rectangle>();

        public VirtualMap<char> ForegroundData { get => data2; }
        public VirtualMap<char> BackgroundData { get => data1; }
        public Stage(int id, AreaMode mode)
        {
            this._Id = id;
            this._Mode = mode;
            Instance = this;
        }

        public void Load()
        {
            //加载章节地图数据
            _MapData = AreaData.Areas[_Id].Mode[(int)_Mode].MapData;

            //地图矩形
            Rectangle bounds = _MapData.TileBounds;
            data1 = new VirtualMap<char>(bounds.Width, bounds.Height, '0');
            data2 = new VirtualMap<char>(bounds.Width, bounds.Height, '0');
            virtualMap = new VirtualMap<bool>(bounds.Width, bounds.Height, false);

            //地图场景填充
            LoadMapData(bounds);
            //加载地图Filler数据
            LoadMapFiller(bounds);
            //修正场景数据
            ModifyMapData1(bounds);
            ModifyMapData2(bounds);
            ModifyMapData3(bounds);

        }

        private void LoadMapData(Rectangle bounds)
        {
            _LevelBounds.Clear();
            foreach (LevelData level in _MapData.Levels)
            {
                Rectangle tileBounds2 = level.TileBounds;
                int left1 = tileBounds2.Left;
                tileBounds2 = level.TileBounds;
                int top1 = tileBounds2.Top;
                string[] strArray1 = regex.Split(level.Bg);
                for (int index1 = top1; index1 < top1 + strArray1.Length; ++index1)
                {
                    for (int index2 = left1; index2 < left1 + strArray1[index1 - top1].Length; ++index2)
                        data1[index2 - bounds.X, index1 - bounds.Y] = strArray1[index1 - top1][index2 - left1];
                }

                string[] strArray2 = regex.Split(level.Solids);
                for (int index1 = top1; index1 < top1 + strArray2.Length; ++index1)
                {
                    for (int index2 = left1; index2 < left1 + strArray2[index1 - top1].Length; ++index2)
                        data2[index2 - bounds.X, index1 - bounds.Y] = strArray2[index1 - top1][index2 - left1];
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
                                virtualMap[left2 - bounds.Left, top2 - bounds.Top] = true;
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
                _LevelBounds.Add(new Rectangle(level.TileBounds.X - bounds.X, level.TileBounds.Y - bounds.Y, level.TileBounds.Width, level.TileBounds.Height));
            }
        }

        private void LoadMapFiller(Rectangle bounds)
        {
            //Filler填充
            foreach (Rectangle rectangle in _MapData.Filler)
            {
                for (int left = rectangle.Left; left < rectangle.Right; ++left)
                {
                    for (int top = rectangle.Top; top < rectangle.Bottom; ++top)
                    {
                        char ch1 = '0';
                        if (rectangle.Top - bounds.Y > 0)
                        {
                            char ch2 = data2[left - bounds.X, rectangle.Top - bounds.Y - 1];
                            if (ch2 != '0')
                                ch1 = ch2;
                        }
                        if (ch1 == '0' && rectangle.Left - bounds.X > 0)
                        {
                            char ch2 = data2[rectangle.Left - bounds.X - 1, top - bounds.Y];
                            if (ch2 != '0')
                                ch1 = ch2;
                        }
                        if (ch1 == '0' && rectangle.Right - bounds.X < bounds.Width - 1)
                        {
                            char ch2 = data2[rectangle.Right - bounds.X, top - bounds.Y];
                            if (ch2 != '0')
                                ch1 = ch2;
                        }
                        if (ch1 == '0' && rectangle.Bottom - bounds.Y < bounds.Height - 1)
                        {
                            char ch2 = data2[left - bounds.X, rectangle.Bottom - bounds.Y];
                            if (ch2 != '0')
                                ch1 = ch2;
                        }
                        if (ch1 == '0')
                            ch1 = '1';
                        data2[left - bounds.X, top - bounds.Y] = ch1;
                        virtualMap[left - bounds.X, top - bounds.Y] = true;
                    }
                }
            }
        }
    
        private void ModifyMapData1(Rectangle bounds)
        {
            using (List<LevelData>.Enumerator enumerator = _MapData.Levels.GetEnumerator())
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
                            char ch1 = data1[left1 - bounds.X, top - bounds.Y];
                            for (int index = 1; index < 4 && !virtualMap[left1 - bounds.X, top - bounds.Y - index]; ++index)
                                data1[left1 - bounds.X, top - bounds.Y - index] = ch1;
                            tileBounds2 = current.TileBounds;
                            int num2 = tileBounds2.Bottom - 1;
                            char ch2 = data1[left1 - bounds.X, num2 - bounds.Y];
                            for (int index = 1; index < 4 && !virtualMap[left1 - bounds.X, num2 - bounds.Y + index]; ++index)
                                data1[left1 - bounds.X, num2 - bounds.Y + index] = ch2;
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
                            char ch1 = data1[left2 - bounds.X, num3 - bounds.Y];
                            for (int index = 1; index < 4 && !virtualMap[left2 - bounds.X - index, num3 - bounds.Y]; ++index)
                                data1[left2 - bounds.X - index, num3 - bounds.Y] = ch1;
                            tileBounds2 = current.TileBounds;
                            int num4 = tileBounds2.Right - 1;
                            char ch2 = data1[num4 - bounds.X, num3 - bounds.Y];
                            for (int index = 1; index < 4 && !virtualMap[num4 - bounds.X + index, num3 - bounds.Y]; ++index)
                                data1[num4 - bounds.X + index, num3 - bounds.Y] = ch2;
                            ++num3;
                        }
                        else
                            goto label_85;
                    }
                }
            }
        }

        private void ModifyMapData2(Rectangle bounds)
        {
            using (List<LevelData>.Enumerator enumerator = _MapData.Levels.GetEnumerator())
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
                            if (data2[left - bounds.X, top - bounds.Y] == '0')
                            {
                                for (int index = 1; index < 8; ++index)
                                    virtualMap[left - bounds.X, top - bounds.Y - index] = true;
                            }
                            tileBounds2 = current.TileBounds;
                            int num2 = tileBounds2.Bottom - 1;
                            if (data2[left - bounds.X, num2 - bounds.Y] == '0')
                            {
                                for (int index = 1; index < 8; ++index)
                                    virtualMap[left - bounds.X, num2 - bounds.Y + index] = true;
                            }
                            ++left;
                        }
                        else
                            goto label_100;
                    }
                }
            }
        }
    
        private void ModifyMapData3(Rectangle bounds)
        {
            using (List<LevelData>.Enumerator enumerator = _MapData.Levels.GetEnumerator())
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
                            char ch1 = data2[left1 - bounds.X, top - bounds.Y];
                            for (int index = 1; index < 4 && !virtualMap[left1 - bounds.X, top - bounds.Y - index]; ++index)
                                data2[left1 - bounds.X, top - bounds.Y - index] = ch1;
                            tileBounds2 = current.TileBounds;
                            int num2 = tileBounds2.Bottom - 1;
                            char ch2 = data2[left1 - bounds.X, num2 - bounds.Y];
                            for (int index = 1; index < 4 && !virtualMap[left1 - bounds.X, num2 - bounds.Y + index]; ++index)
                                data2[left1 - bounds.X, num2 - bounds.Y + index] = ch2;
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
                            char ch1 = data2[left2 - bounds.X, num3 - bounds.Y];
                            for (int index = 1; index < 4 && !virtualMap[left2 - bounds.X - index, num3 - bounds.Y]; ++index)
                                data2[left2 - bounds.X - index, num3 - bounds.Y] = ch1;
                            tileBounds2 = current.TileBounds;
                            int num4 = tileBounds2.Right - 1;
                            char ch2 = data2[num4 - bounds.X, num3 - bounds.Y];
                            for (int index = 1; index < 4 && !virtualMap[num4 - bounds.X + index, num3 - bounds.Y]; ++index)
                                data2[num4 - bounds.X + index, num3 - bounds.Y] = ch2;
                            ++num3;
                        }
                        else
                            goto label_122;
                    }
                }
            }
        }
    }
}
