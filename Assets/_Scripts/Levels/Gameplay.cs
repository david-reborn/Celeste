using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


namespace myd.celeste
{
    public class Gameplay : MonoBehaviour
    {
        public static Gameplay instance;

        public Texture2D TileSet;
        private Dictionary<string, ExtSprite> Images = new Dictionary<string, ExtSprite>(StringComparer.OrdinalIgnoreCase);
        private AutoTiler foreground, background;

        public string name;
        public Sprite sprite;

        private char DefaultTile = '3';

        private void Awake()
        {
            instance = this;
        }

        void Start()
        {
            //解析所有图片
            ParseItems();

            foreground = new AutoTiler(Util.ReadResource("ForegroundTiles.xml"));
            background = new AutoTiler(Util.ReadResource("BackgroundTiles.xml"));

            int i = 0;
            foreach (string key in Images.Keys)
            {
                i++;
                Debug.Log(key);
                if (i > 1000)
                {
                    break;
                }
            }
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.A))
            {
                sprite = GetImage(name);
            }
            if (Input.GetKeyUp(KeyCode.G))
            {
                GenerateMap();
            }
        }

        /// <summary>
        /// 生成地图
        /// </summary>
        private void GenerateMap()
        {
            MapElement element = MapCoder.FromBinary(Util.GAME_PATH + "/Content/Maps/0-Intro.bin");
            Sprite chapter = Gameplay.instance.GenerateMap(element);

            this.GetComponent<SpriteRenderer>().sprite = chapter;
        }

        private void ParseItems()
        {
            using (Stream stream = Util.ReadResourceStream("Gameplay.meta"))
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    reader.ReadInt32();
                    reader.ReadString();
                    reader.ReadInt32();

                    int count = reader.ReadInt16();
                    for (int i = 0; i < count; i++)
                    {
                        string dataFile = reader.ReadString();
                        int spriteCount = reader.ReadInt16();
                        for (int j = 0; j < spriteCount; j++)
                        {
                            string path = reader.ReadString().Replace('\\', '/');
                            int x = reader.ReadInt16();
                            int y = reader.ReadInt16();
                            int width = reader.ReadInt16();
                            int height = reader.ReadInt16();

                            int xOffset = reader.ReadInt16();
                            int yOffset = reader.ReadInt16();
                            int realWidth = reader.ReadInt16();
                            int realHeight = reader.ReadInt16();

                            Images.Add(path, new ExtSprite(x, y, width, height, xOffset, yOffset, realWidth, realHeight));
                        }
                    }
                }
            }
        }
        public Sprite GetImage(string path)
        {
            if (!Images.TryGetValue(path, out ExtSprite bounds))
            {
                return null;
            }
            return Util.GetSubImage(TileSet, bounds);
        }

        public Sprite GenerateMap(MapElement element)
        {
            string package = element.Attr("_package", "map");
            TileGrid.DefaultBackground = Util.HexToColor("000000"); //Util.HexToColor("48106e");//

            List<MapElement> levels = element.Select("levels", "level");
            Rect chapterBounds = GetChapterBounds(levels);

            Rect viewport = new Rect(0, 0, chapterBounds.width, chapterBounds.height);
            //Rectangle viewport = new Rectangle(250, 3000, 300, 200);
            //Rectangle viewport = GetLevelBounds(levels, chapterBounds, "lvl_08-c");
            Texture2D chapterTexture2D = new Texture2D(Mathf.RoundToInt(viewport.width), Mathf.RoundToInt(viewport.height), TextureFormat.ARGB32, false);
            Sprite chapter = Sprite.Create(chapterTexture2D, viewport, new Vector2(0.5f, 0.5f));
            MapElement bgs = element.SelectFirst("Style", "Backgrounds");
            MapElement fgs = element.SelectFirst("Style", "Foregrounds");

            background.SetLevelBounds(levels);
            foreground.SetLevelBounds(levels);
            string sceneryTileset = "scenery";
            TileGrid scenery = GetTileset(sceneryTileset);

            //for (int i = 0; i < levels.Count; i++)
            //{
            //    MapElement level = levels[i];

            //    int x = level.AttrInt("x", 0);
            //    int y = level.AttrInt("y", 0);
            //    int width = level.AttrInt("width", 0);
            //    int height = level.AttrInt("height", 0);
            //    int widthTiles = width / 8;
            //    int heightTiles = height / 8;

            //    Vector2 pos = new Vector2(x - chapterBounds.x, y - chapterBounds.y);
            //    Vector2 offset = new Vector2(pos.x - viewport.x, pos.y - viewport.y);
            //    Rect levelBounds = new Rect(pos.x, pos.y, width, height);
            //    if (!levelBounds(viewport)) { continue; }
                
            //    TileGrid tiles = GenerateLevelTiles(level, "bg", widthTiles, heightTiles, background, out VirtualMap<char> solids);
            //    string tileset = level.SelectFirst("bgtiles").Attr("tileset", "Scenery");
            //    if (tileset.Equals("terrain", StringComparison.OrdinalIgnoreCase))
            //    {
            //        tileset = "scenery";
            //    }
            //    if (tileset != sceneryTileset)
            //    {
            //        scenery = GetTileset(tileset);
            //        sceneryTileset = tileset;
            //    }
            //    tiles.Overlay(level, "bgtiles", widthTiles, heightTiles, scenery);

            //    using (Bitmap map = tiles.DisplayMap(level, Backdrop.CreateBackdrops(bgs, levels), new Rect(pos, chapterBounds.size), true))
            //    {
            //        OverlayDecals(level.Select("bgdecals", "decal"), map);
            //        tiles = GenerateLevelTiles(level, "solids", widthTiles, heightTiles, foreground, out solids);
            //        OverlayEntities(level.SelectFirst("entities"), map, solids, true);
            //        Util.CopyTo(chapter, map, offset);
            //    }

            //    tileset = level.SelectFirst("fgtiles").Attr("tileset", "Scenery");
            //    if (tileset.Equals("terrain", StringComparison.OrdinalIgnoreCase))
            //    {
            //        tileset = "scenery";
            //    }
            //    if (tileset != sceneryTileset)
            //    {
            //        scenery = GetTileset(tileset);
            //        sceneryTileset = tileset;
            //    }

            //    tiles.Overlay(level, "fgtiles", widthTiles, heightTiles, scenery);
            //    using (Bitmap map = tiles.DisplayMap(level, Backdrop.CreateBackdrops(fgs, levels), new Rectangle(pos, chapterBounds.Size), false))
            //    {
            //        OverlayDecals(level.Select("fgdecals", "decal"), map);
            //        OverlayEntities(level.SelectFirst("entities"), map, solids, false);
            //        Util.CopyTo(chapter, map, offset);
            //    }

            //    //XmlNode objtiles = level.SelectSingleNode("objtiles");
            //}

            levels = element.Select("Filler", "rect");
            for (int i = 0; i < levels.Count; i++)
            {
                MapElement level = levels[i];

                int x = level.AttrInt("x", 0);
                int y = level.AttrInt("y", 0);
                int width = level.AttrInt("w", 0);
                int height = level.AttrInt("h", 0);

                Vector2 pos = new Vector2(x * 8 - chapterBounds.x, y * 8 - chapterBounds.y);
                Vector2 offset = new Vector2(pos.x - viewport.x, pos.y - viewport.y);
                Rect levelBounds = new Rect(pos.x, pos.y, width, height);
                if (!levelBounds.IntersectsWith(viewport)) { continue; }

                TileGrid tiles = foreground.GenerateOverlay(DefaultTile, 0, 0, width, height, null);
                using (Bitmap map = tiles.DisplayMap(null, null, chapterBounds, false))
                {
                    Util.CopyTo(chapter, map, pos);
                }
            }
            return chapter;
        }

        private Rect GetChapterBounds(List<MapElement> levels)
        {
            int minX = int.MaxValue, maxXW = int.MinValue, minY = int.MaxValue, maxYH = int.MinValue;

            //int levelCount = 0;
            for (int i = 0; i < levels.Count; i++)
            {
                MapElement level = levels[i];
                string name = level.Attr("name");
                try
                {
                    int x = level.AttrInt("x", 0);
                    int y = level.AttrInt("y", 0);
                    int width = level.AttrInt("width", 0);
                    int height = level.AttrInt("height", 0);

                    if (x < minX)
                    {
                        minX = x;
                    }
                    if (x + width > maxXW)
                    {
                        maxXW = x + width;
                    }
                    if (y < minY)
                    {
                        minY = y;
                    }
                    if (y + height > maxYH)
                    {
                        maxYH = y + height;
                    }
                    //levelCount++;
                }
                catch
                {
                    throw new Exception("Failed to read level properties for level: " + name);
                }
            }
            //Console.WriteLine(levelCount);

            return new Rect(minX, minY, maxXW - minX, maxYH - minY);
        }

        private TileGrid GetTileset(string tilesetName)
        {
            TileGrid tileset = null;
            Sprite sceneryTileset = GetImage("tilesets/" + tilesetName);
            {
                tileset = new TileGrid(8, 8, Mathf.RoundToInt(sceneryTileset.bounds.size.x) / 8, Mathf.RoundToInt(sceneryTileset.bounds.size.y) / 8);
                tileset.Load(sceneryTileset);
            }
            return tileset;
        }
    }
}