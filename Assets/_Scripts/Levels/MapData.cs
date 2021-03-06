﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Globalization;
using UnityEngine.Rendering;
using Newtonsoft.Json;
namespace myd.celeste
{
    /// <summary>
    /// 章节地图配置信息，包括N个关卡
    /// </summary>
    public class MapData
    {
        public List<LevelData> Levels = new List<LevelData>();
        public List<Rectangle> Filler = new List<Rectangle>();
        public List<EntityData> Strawberries = new List<EntityData>();
        public List<EntityData> Goldenberries = new List<EntityData>();
        public Color BackgroundColor = Color.black;
        public AreaKey Area;
        public AreaData Data;
        public ModeProperties ModeData;
        public int DetectedStrawberries;
        public bool DetectedRemixNotes;
        public bool DetectedHeartGem;
        public BinaryPacker.Element Foreground;
        public BinaryPacker.Element Background;
        public Rectangle Bounds;

        public string Filename
        {
            get
            {
                return this.Data.Mode[(int)this.Area.Mode].Path;
            }
        }

        public string Filepath
        {
            get
            {
                return Path.Combine(Util.GAME_PATH_CONTENT, "Maps", this.Filename + ".bin");
            }
        }

        public Rectangle TileBounds
        {
            get
            {
                return new Rectangle(this.Bounds.X / 8, this.Bounds.Y / 8, (int)Math.Ceiling((double)this.Bounds.Width / 8.0), (int)Math.Ceiling((double)this.Bounds.Height / 8.0));
            }
        }

        public MapData(AreaKey area)
        {
            this.Area = area;
            this.Data = AreaData.Areas[this.Area.ID];
            this.ModeData = this.Data.Mode[(int)this.Area.Mode];
            this.Load();
        }

        public LevelData GetTransitionTarget(Level level, Vector2 position)
        {
            return this.GetAt(position);
        }

        public bool CanTransitionTo(Level level, Vector2 position)
        {
            LevelData transitionTarget = this.GetTransitionTarget(level, position);
            return transitionTarget != null && !transitionTarget.Dummy;
        }

        public int LoadSeed
        {
            get
            {
                int num = 0;
                foreach (char ch in this.Data.Name)
                    num += (int)ch;
                return num;
            }
        }

        public int LevelCount
        {
            get
            {
                int num = 0;
                foreach (LevelData level in this.Levels)
                {
                    if (!level.Dummy)
                        ++num;
                }
                return num;
            }
        }

        public void Reload()
        {
            this.Load();
        }

        private void Load()
        {
            if (!File.Exists(this.Filepath))
                return;
            this.Strawberries = new List<EntityData>();
            BinaryPacker.Element element = BinaryPacker.FromBinary(this.Filepath);
            //string file = JsonConvert.SerializeObject(element);
            //File.WriteAllText("F:/test.json", file);
            if (!element.Package.Equals(this.ModeData.Path))
                throw new Exception("Corrupted Level Data");
            foreach (BinaryPacker.Element child1 in element.Children)
            {
                if (child1.Name == "levels")
                {
                    this.Levels = new List<LevelData>();
                    foreach (BinaryPacker.Element child2 in child1.Children)
                    {
                        LevelData levelData = new LevelData(child2);
                        this.DetectedStrawberries += levelData.Strawberries;
                        if (levelData.HasGem)
                            this.DetectedRemixNotes = true;
                        if (levelData.HasHeartGem)
                            this.DetectedHeartGem = true;
                        this.Levels.Add(levelData);
                    }
                }
                else if (child1.Name == "Filler")
                {
                    this.Filler = new List<Rectangle>();
                    if (child1.Children != null)
                    {
                        foreach (BinaryPacker.Element child2 in child1.Children)
                            this.Filler.Add(new Rectangle((int)child2.Attributes["x"], (int)child2.Attributes["y"], (int)child2.Attributes["w"], (int)child2.Attributes["h"]));
                    }
                }
                else if (child1.Name == "Style")
                {
                    if (child1.HasAttr("color"))
                        this.BackgroundColor = Util.HexToColor(child1.Attr("color", ""));
                    if (child1.Children != null)
                    {
                        foreach (BinaryPacker.Element child2 in child1.Children)
                        {
                            if (child2.Name == "Backgrounds")
                                this.Background = child2;
                            else if (child2.Name == "Foregrounds")
                                this.Foreground = child2;
                        }
                    }
                }
            }
            foreach (LevelData level in this.Levels)
            {
                foreach (EntityData entity in level.Entities)
                {
                    if (entity.Name == "strawberry")
                        this.Strawberries.Add(entity);
                    else if (entity.Name == "goldenBerry")
                        this.Goldenberries.Add(entity);
                }
            }
            int num1 = int.MaxValue;
            int num2 = int.MaxValue;
            int num3 = int.MinValue;
            int num4 = int.MinValue;
            foreach (LevelData level in this.Levels)
            {
                if (level.Bounds.xMin < num1)
                    num1 = Mathf.RoundToInt(level.Bounds.xMin);
                if (level.Bounds.top < num2)
                    num2 = Mathf.RoundToInt(level.Bounds.top);
                if (level.Bounds.xMax > num3)
                    num3 = Mathf.RoundToInt(level.Bounds.xMax);
                if (level.Bounds.bottom > num4)
                    num4 = Mathf.RoundToInt(level.Bounds.bottom);
            }
            foreach (Rectangle rectangle in this.Filler)
            {
                if (rectangle.Left < num1)
                    num1 = rectangle.Left;
                if (rectangle.Top < num2)
                    num2 = rectangle.Top;
                if (rectangle.Right > num3)
                    num3 = rectangle.Right;
                if (rectangle.Bottom > num4)
                    num4 = rectangle.Bottom;
            }
            int num5 = 64;
            this.Bounds = new Rectangle(num1 - num5, num2 - num5, num3 - num1 + num5 * 2, num4 - num2 + num5 * 2);
            this.ModeData.TotalStrawberries = 0;
            this.ModeData.StartStrawberries = 0;
            this.ModeData.StrawberriesByCheckpoint = new EntityData[10, 25];
            for (int index = 0; this.ModeData.Checkpoints != null && index < this.ModeData.Checkpoints.Length; ++index)
            {
                if (this.ModeData.Checkpoints[index] != null)
                    this.ModeData.Checkpoints[index].Strawberries = 0;
            }
            foreach (EntityData strawberry in this.Strawberries)
            {
                if (!strawberry.Bool("moon", false))
                {
                    int index1 = strawberry.Int("checkpointID", 0);
                    int index2 = strawberry.Int("order", 0);
                    if (this.ModeData.StrawberriesByCheckpoint[index1, index2] == null)
                        this.ModeData.StrawberriesByCheckpoint[index1, index2] = strawberry;
                    if (index1 == 0)
                        ++this.ModeData.StartStrawberries;
                    else if (this.ModeData.Checkpoints != null)
                        ++this.ModeData.Checkpoints[index1 - 1].Strawberries;
                    ++this.ModeData.TotalStrawberries;
                }
            }
        }

        public int[] GetStrawberries(out int total)
        {
            total = 0;
            int[] numArray = new int[10];
            foreach (LevelData level in this.Levels)
            {
                foreach (EntityData entity in level.Entities)
                {
                    if (entity.Name == "strawberry")
                    {
                        ++total;
                        ++numArray[entity.Int("checkpointID", 0)];
                    }
                }
            }
            return numArray;
        }

        public LevelData StartLevel()
        {
            return this.GetAt(Vector2.zero);
        }

        public LevelData GetAt(Vector2 at)
        {
            foreach (LevelData level in this.Levels)
            {
                if (level.Check(at))
                    return level;
            }
            return (LevelData)null;
        }

        public LevelData Get(string levelName)
        {
            foreach (LevelData level in this.Levels)
            {
                if (level.Name.Equals(levelName))
                    return level;
            }
            return (LevelData)null;
        }

        public List<Backdrop> CreateBackdrops(BinaryPacker.Element data)
        {
            List<Backdrop> backdropList = new List<Backdrop>();
            if (data != null && data.Children != null)
            {
                foreach (BinaryPacker.Element child1 in data.Children)
                {
                    if (child1.Name.Equals("apply", StringComparison.OrdinalIgnoreCase))
                    {
                        if (child1.Children != null)
                        {
                            foreach (BinaryPacker.Element child2 in child1.Children)
                                backdropList.Add(this.ParseBackdrop(child2, child1));
                        }
                    }
                    else
                        backdropList.Add(this.ParseBackdrop(child1, (BinaryPacker.Element)null));
                }
            }
            return backdropList;
        }

        private Backdrop ParseBackdrop(BinaryPacker.Element child, BinaryPacker.Element above)
        {
            Backdrop backdrop;
            if (child.Name.Equals("parallax", StringComparison.OrdinalIgnoreCase))
            {
                string id = child.Attr("texture", "");
                string str1 = child.Attr("atlas", "game");
                //Parallax parallax = new Parallax(!(str1 == "game") || !GFX.Game.Has(id) ? (!(str1 == "gui") || !GFX.Gui.Has(id) ? GFX.Misc[id] : GFX.Gui[id]) : GFX.Game[id]);
                Parallax parallax = null;
                if (Gfx.Game.Has(id))
                {
                    parallax = new Parallax(Gfx.Game[id]);
                }else
                {
                    parallax = new Parallax(Gfx.Misc[id]);
                }
                backdrop = (Backdrop)parallax;
                string str2 = "";
                if (child.HasAttr("blendmode"))
                    str2 = child.Attr("blendmode", "alphablend").ToLower();
                else if (above != null && above.HasAttr("blendmode"))
                    str2 = above.Attr("blendmode", "alphablend").ToLower();
                if (str2.Equals("additive"))
                    parallax.BlendState = BlendState.defaultValue;
                parallax.DoFadeIn = bool.Parse(child.Attr("fadeIn", "false"));
            }
            else if (child.Name.Equals("snowfg", StringComparison.OrdinalIgnoreCase))
                backdrop = (Backdrop)new Snow(true);
            else if (child.Name.Equals("snowbg", StringComparison.OrdinalIgnoreCase))
                backdrop = (Backdrop)new Snow(false);
            //else if (child.Name.Equals("windsnow", StringComparison.OrdinalIgnoreCase))
            //    backdrop = (Backdrop)new WindSnowFG();
            //else if (child.Name.Equals("dreamstars", StringComparison.OrdinalIgnoreCase))
            //    backdrop = (Backdrop)new DreamStars();
            //else if (child.Name.Equals("stars", StringComparison.OrdinalIgnoreCase))
            //    backdrop = (Backdrop)new StarsBG();
            //else if (child.Name.Equals("mirrorfg", StringComparison.OrdinalIgnoreCase))
            //    backdrop = (Backdrop)new MirrorFG();
            //else if (child.Name.Equals("reflectionfg", StringComparison.OrdinalIgnoreCase))
            //    backdrop = (Backdrop)new ReflectionFG();
            //else if (child.Name.Equals("godrays", StringComparison.OrdinalIgnoreCase))
            //    backdrop = (Backdrop)new Godrays();
            //else if (child.Name.Equals("tentacles", StringComparison.OrdinalIgnoreCase))
            //    backdrop = (Backdrop)new Tentacles((Tentacles.Side)Enum.Parse(typeof(Tentacles.Side), child.Attr("side", "Right")), Calc.HexToColor(child.Attr("color", "")), child.AttrFloat("offset", 0.0f));
            //else if (child.Name.Equals("northernlights", StringComparison.OrdinalIgnoreCase))
            //    backdrop = (Backdrop)new NorthernLights();
            //else if (child.Name.Equals("bossStarField", StringComparison.OrdinalIgnoreCase))
            //    backdrop = (Backdrop)new FinalBossStarfield();
            //else if (child.Name.Equals("petals", StringComparison.OrdinalIgnoreCase))
            //    backdrop = (Backdrop)new Petals();
            //else if (child.Name.Equals("heatwave", StringComparison.OrdinalIgnoreCase))
            //    backdrop = (Backdrop)new HeatWave();
            //else if (child.Name.Equals("corestarsfg", StringComparison.OrdinalIgnoreCase))
            //    backdrop = (Backdrop)new CoreStarsFG();
            //else if (child.Name.Equals("starfield", StringComparison.OrdinalIgnoreCase))
            //    backdrop = (Backdrop)new Starfield(Calc.HexToColor(child.Attr("color", "")), child.AttrFloat("speed", 1f));
            //else if (child.Name.Equals("planets", StringComparison.OrdinalIgnoreCase))
            //    backdrop = (Backdrop)new Planets((int)child.AttrFloat("count", 32f), child.Attr("size", "small"));
            //else if (child.Name.Equals("rain", StringComparison.OrdinalIgnoreCase))
            //    backdrop = (Backdrop)new RainFG();
            //else if (child.Name.Equals("stardust", StringComparison.OrdinalIgnoreCase))
            //{
            //    backdrop = (Backdrop)new StardustFG();
            //}   
            else
            {
                if (!child.Name.Equals("blackhole", StringComparison.OrdinalIgnoreCase))
                    throw new Exception("Background type " + child.Name + " does not exist");
                backdrop = (Backdrop)new BlackholeBG();
            }
            if (child.HasAttr("tag"))
                backdrop.Tags.Add(child.Attr("tag", ""));
            if (above != null && above.HasAttr("tag"))
                backdrop.Tags.Add(above.Attr("tag", ""));
            if (child.HasAttr("x"))
                backdrop.Position.x = child.AttrFloat("x", 0.0f);
            else if (above != null && above.HasAttr("x"))
                backdrop.Position.x = above.AttrFloat("x", 0.0f);
            if (child.HasAttr("y"))
                backdrop.Position.y = child.AttrFloat("y", 0.0f);
            else if (above != null && above.HasAttr("y"))
                backdrop.Position.y = above.AttrFloat("y", 0.0f);
            if (child.HasAttr("scrollx"))
                backdrop.Scroll.x = child.AttrFloat("scrollx", 0.0f);
            else if (above != null && above.HasAttr("scrollx"))
                backdrop.Scroll.x = above.AttrFloat("scrollx", 0.0f);
            if (child.HasAttr("scrolly"))
                backdrop.Scroll.y = child.AttrFloat("scrolly", 0.0f);
            else if (above != null && above.HasAttr("scrolly"))
                backdrop.Scroll.y = above.AttrFloat("scrolly", 0.0f);
            if (child.HasAttr("speedx"))
                backdrop.Speed.x = child.AttrFloat("speedx", 0.0f);
            else if (above != null && above.HasAttr("speedx"))
                backdrop.Speed.x = above.AttrFloat("speedx", 0.0f);
            if (child.HasAttr("speedy"))
                backdrop.Speed.y = child.AttrFloat("speedy", 0.0f);
            else if (above != null && above.HasAttr("speedy"))
                backdrop.Speed.y = above.AttrFloat("speedy", 0.0f);
            backdrop.Color = Color.white;
            if (child.HasAttr("color"))
                backdrop.Color = Util.HexToColor(child.Attr("color", ""));
            else if (above != null && above.HasAttr("color"))
                backdrop.Color = Util.HexToColor(above.Attr("color", ""));
            if (child.HasAttr("alpha"))
                backdrop.Color *= child.AttrFloat("alpha", 0.0f);
            else if (above != null && above.HasAttr("alpha"))
                backdrop.Color *= above.AttrFloat("alpha", 0.0f);
            if (child.HasAttr("flipx"))
                backdrop.FlipX = child.AttrBool("flipx", false);
            else if (above != null && above.HasAttr("flipx"))
                backdrop.FlipX = above.AttrBool("flipx", false);
            if (child.HasAttr("flipy"))
                backdrop.FlipY = child.AttrBool("flipy", false);
            else if (above != null && above.HasAttr("flipy"))
                backdrop.FlipY = above.AttrBool("flipy", false);
            if (child.HasAttr("loopx"))
                backdrop.LoopX = child.AttrBool("loopx", false);
            else if (above != null && above.HasAttr("loopx"))
                backdrop.LoopX = above.AttrBool("loopx", false);
            if (child.HasAttr("loopy"))
                backdrop.LoopY = child.AttrBool("loopy", false);
            else if (above != null && above.HasAttr("loopy"))
                backdrop.LoopY = above.AttrBool("loopy", false);
            if (child.HasAttr("wind"))
                backdrop.WindMultiplier = child.AttrFloat("wind", 0.0f);
            else if (above != null && above.HasAttr("wind"))
                backdrop.WindMultiplier = above.AttrFloat("wind", 0.0f);
            string list1 = (string)null;
            if (child.HasAttr("exclude"))
                list1 = child.Attr("exclude", "");
            else if (above != null && above.HasAttr("exclude"))
                list1 = above.Attr("exclude", "");
            if (list1 != null)
                backdrop.ExcludeFrom = this.ParseLevelsList(list1);
            string list2 = (string)null;
            if (child.HasAttr("only"))
                list2 = child.Attr("only", "");
            else if (above != null && above.HasAttr("only"))
                list2 = above.Attr("only", "");
            if (list2 != null)
                backdrop.OnlyIn = this.ParseLevelsList(list2);
            string str3 = (string)null;
            if (child.HasAttr("flag"))
                str3 = child.Attr("flag", "");
            else if (above != null && above.HasAttr("flag"))
                str3 = above.Attr("flag", "");
            if (str3 != null)
                backdrop.OnlyIfFlag = str3;
            string str4 = (string)null;
            if (child.HasAttr("notflag"))
                str4 = child.Attr("notflag", "");
            else if (above != null && above.HasAttr("notflag"))
                str4 = above.Attr("notflag", "");
            if (str4 != null)
                backdrop.OnlyIfNotFlag = str4;
            string str5 = (string)null;
            if (child.HasAttr("always"))
                str5 = child.Attr("always", "");
            else if (above != null && above.HasAttr("always"))
                str5 = above.Attr("always", "");
            if (str5 != null)
                backdrop.AlsoIfFlag = str5;
            bool? nullable = new bool?();
            if (child.HasAttr("dreaming"))
                nullable = new bool?(child.AttrBool("dreaming", false));
            else if (above != null && above.HasAttr("dreaming"))
                nullable = new bool?(above.AttrBool("dreaming", false));
            if (nullable.HasValue)
                backdrop.Dreaming = nullable;
            if (child.HasAttr("instantIn"))
                backdrop.InstantIn = child.AttrBool("instantIn", false);
            else if (above != null && above.HasAttr("instantIn"))
                backdrop.InstantIn = above.AttrBool("instantIn", false);
            if (child.HasAttr("instantOut"))
                backdrop.InstantOut = child.AttrBool("instantOut", false);
            else if (above != null && above.HasAttr("instantOut"))
                backdrop.InstantOut = above.AttrBool("instantOut", false);
            string str6 = (string)null;
            if (child.HasAttr("fadex"))
                str6 = child.Attr("fadex", "");
            else if (above != null && above.HasAttr("fadex"))
                str6 = above.Attr("fadex", "");
            if (str6 != null)
            {
                backdrop.FadeX = new Backdrop.Fader();
                string str1 = str6;
                char[] chArray1 = new char[1] { ':' };
                foreach (string str2 in str1.Split(chArray1))
                {
                    char[] chArray2 = new char[1] { ',' };
                    string[] strArray1 = str2.Split(chArray2);
                    if (strArray1.Length == 2)
                    {
                        string[] strArray2 = strArray1[0].Split('-');
                        string[] strArray3 = strArray1[1].Split('-');
                        float fadeFrom = float.Parse(strArray3[0], (IFormatProvider)CultureInfo.InvariantCulture);
                        float fadeTo = float.Parse(strArray3[1], (IFormatProvider)CultureInfo.InvariantCulture);
                        int num1 = 1;
                        int num2 = 1;
                        if (strArray2[0][0] == 'n')
                        {
                            num1 = -1;
                            strArray2[0] = strArray2[0].Substring(1);
                        }
                        if (strArray2[1][0] == 'n')
                        {
                            num2 = -1;
                            strArray2[1] = strArray2[1].Substring(1);
                        }
                        backdrop.FadeX.Add((float)(num1 * int.Parse(strArray2[0])), (float)(num2 * int.Parse(strArray2[1])), fadeFrom, fadeTo);
                    }
                }
            }
            string str7 = (string)null;
            if (child.HasAttr("fadey"))
                str7 = child.Attr("fadey", "");
            else if (above != null && above.HasAttr("fadey"))
                str7 = above.Attr("fadey", "");
            if (str7 != null)
            {
                backdrop.FadeY = new Backdrop.Fader();
                string str1 = str7;
                char[] chArray1 = new char[1] { ':' };
                foreach (string str2 in str1.Split(chArray1))
                {
                    char[] chArray2 = new char[1] { ',' };
                    string[] strArray1 = str2.Split(chArray2);
                    if (strArray1.Length == 2)
                    {
                        string[] strArray2 = strArray1[0].Split('-');
                        string[] strArray3 = strArray1[1].Split('-');
                        float fadeFrom = float.Parse(strArray3[0], (IFormatProvider)CultureInfo.InvariantCulture);
                        float fadeTo = float.Parse(strArray3[1], (IFormatProvider)CultureInfo.InvariantCulture);
                        int num1 = 1;
                        int num2 = 1;
                        if (strArray2[0][0] == 'n')
                        {
                            num1 = -1;
                            strArray2[0] = strArray2[0].Substring(1);
                        }
                        if (strArray2[1][0] == 'n')
                        {
                            num2 = -1;
                            strArray2[1] = strArray2[1].Substring(1);
                        }
                        backdrop.FadeY.Add((float)(num1 * int.Parse(strArray2[0])), (float)(num2 * int.Parse(strArray2[1])), fadeFrom, fadeTo);
                    }
                }
            }
            backdrop.OnRefresh();
            return backdrop;
        }

        private HashSet<string> ParseLevelsList(string list)
        {
            HashSet<string> stringSet = new HashSet<string>();
            string str1 = list;
            foreach (string str2 in str1.Split(','))
            {
                if (str2.Contains("*"))
                {
                    string pattern = "^" + Regex.Escape(str2).Replace("\\*", ".*") + "$";
                    foreach (LevelData level in this.Levels)
                    {
                        if (Regex.IsMatch(level.Name, pattern))
                            stringSet.Add(level.Name);
                    }
                }
                else
                    stringSet.Add(str2);
            }
            return stringSet;
        }
    }
}
