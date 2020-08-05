using UnityEngine;
using System.Collections;
using System.IO;
using myd.celeste;
using System.Text.RegularExpressions;
using System.Collections.Generic;

public class MyGameLoader : MonoBehaviour
{
    public static MyGameLoader instance;

    public int level;
    public GameObject spritePrefab;

    public void Awake()
    {
        instance = this;
    }

    void Start()
    {
        //读取配置文件
        Gfx.Game = Atlas.FromAtlas(Path.Combine("Graphics", "Atlases", "Gameplay"), Atlas.AtlasDataFormat.Packer);
        Gfx.Misc = Atlas.FromAtlas(Path.Combine("Graphics", "Atlases", "Misc"), Atlas.AtlasDataFormat.PackerNoAtlas);

        Gfx.SceneryTiles = new Tileset(Gfx.Game["tilesets/scenery"], 8, 8);
        //读取前景配置文件
        Gfx.BGAutotiler = new Autotiler(Path.Combine("Graphics", "BackgroundTiles.xml"));
        Gfx.FGAutotiler = new Autotiler(Path.Combine("Graphics", "ForegroundTiles.xml"));

        Debug.Log("==加载AreaData文件");
        SaveData.Start(new SaveData
        {
            Name = "test001",
            AssistMode = true,
            VariantMode = true
        }, 0);
        //加载区域
        AreaData.Load();

        Debug.Log("==创建Session,读取关卡地图");

        Session session = new Session(new AreaKey(0, AreaMode.Normal), null, null);

        MapData mapData = session.MapData;
        LevelData levelData = mapData.Levels[0];

        Debug.Log(levelData);

        Rectangle tileBounds1 = mapData.TileBounds;
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
        BackgroundTiles backgroundTiles = new BackgroundTiles(position, data1);


        //MTexture mTexture = Gfx.Game["tilesets/dirt"];
        //草地等等
        //MTexture mTexture = Gfx.Game["tilesets/scenery"];
        //StartCoroutine(DrawTiles(backgroundTiles.Tiles, Vector3.zero));


        /////////////////////////////////////////////////////
        ///构建BgTiles
        /////////////////////////////////////////////////////
        int l8 = levelData.TileBounds.Left;
        int t8 = levelData.TileBounds.Top;
        int w8 = levelData.TileBounds.Width;
        int h8 = levelData.TileBounds.Height;
        bool flag14 = !string.IsNullOrEmpty(levelData.BgTiles);
        if (flag14)
        {
            int[,] tiles = Util.ReadCSVIntGrid(levelData.BgTiles, w8, h8);
            backgroundTiles.Tiles.Overlay(Gfx.SceneryTiles, tiles, l8 - tileBounds1.X, t8 - tileBounds1.Y);
        }

        //BackdropRenderer backgroundRenderer = new BackdropRenderer();
        //backgroundRenderer.Backdrops = mapData.CreateBackdrops(mapData.Background);

        //BackdropRenderer foregroundRenderer = new BackdropRenderer();
        //foregroundRenderer.Backdrops = mapData.CreateBackdrops(mapData.Foreground);
        //foreach(Backdrop backdrop in backgroundRenderer.Backdrops)
        //{
        //    if(backdrop is Parallax)
        //    {
        //        ShowSprite((backdrop as Parallax).Texture);
        //    }

        //}
        //StartCoroutine(DrawTiles(backgroundTiles.Tiles, Vector3.zero));



        //foreach (DecalData bgDecal in levelData.BgDecals)
        //{
        //    new Decal(bgDecal.Texture, Vector3.zero + bgDecal.Position, bgDecal.Scale, 9000);
        //}
    }


    public GameObject ShowSprite(MTexture mTexture, string name = null)
    {
        GameObject gameObject = Instantiate(spritePrefab);
        if (name != null)
        {
            gameObject.name = name;
        }
        gameObject.transform.SetParent(this.transform, false);
        gameObject.GetComponent<SpriteRenderer>().sprite = mTexture.GetSprite();
        return gameObject;
    }

    private IEnumerator DrawTiles(TileGrid tileGrid,Vector3 offset)
    {
        Rectangle clippedRenderTiles = tileGrid.GetClippedRenderTiles();
        int count = 0;
        for (int left = clippedRenderTiles.Left; left < clippedRenderTiles.Right; ++left)
        {
            for (int top = clippedRenderTiles.Top; top < clippedRenderTiles.Bottom; ++top)
            {
                MTexture mTexture = tileGrid.Tiles[left, top];
                if (mTexture == null)
                {
                    continue;
                }
                mTexture = tileGrid.Tiles[left, top];
                count++;
                GameObject gb = Instantiate(spritePrefab);
                gb.transform.SetParent(this.transform, false);
                gb.GetComponent<SpriteRenderer>().sprite = mTexture.GetSprite();
                gb.transform.position = new Vector3((float)(left * tileGrid.TileWidth) / 100f, -(float)(top * tileGrid.TileHeight) / 100f, 0);
                yield return null;
            }
        }
    }
}
