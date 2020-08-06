using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.Tilemaps;
using System;
using System.Collections.Generic;
using System.Xml;

namespace myd.celeste.demo
{
    public class TileMapGenerator : MonoBehaviour
    {
        public Grid grid;
        public Tilemap fgTilemap;
        public Tilemap bgTilemap;

        public SolidTile tile;
        private Tiles foregroundTiles;
        private Tiles backgroundTiles;

        private ColliderGrid colliderGrid;

        void Start()
        {
            Debug.Log("==读取GamePlay的Texture");
            Gfx.Game = Atlas.FromAtlas(Path.Combine("Graphics", "Atlases", "Gameplay"), Atlas.AtlasDataFormat.Packer);
            foregroundTiles = new Tiles(Path.Combine(Util.GAME_PATH_CONTENT, Path.Combine("Graphics", "ForegroundTiles.xml")));
            backgroundTiles = new Tiles(Path.Combine(Util.GAME_PATH_CONTENT, Path.Combine("Graphics", "BackgroundTiles.xml")));
            AreaData.Load();
            Stage stage = new Stage(0, AreaMode.Normal);
            stage.Load();

            //生成地图数据
            Autotiler.Behaviour fgBehaviour = new Autotiler.Behaviour()
            {
                EdgesExtend = true,
                EdgesIgnoreOutOfLevel = false,
                PaddingIgnoreOutOfLevel = true
            };
            Autotiler.Behaviour bgBehaviour = new Autotiler.Behaviour()
            {
                EdgesExtend = true,
                EdgesIgnoreOutOfLevel = false,
                PaddingIgnoreOutOfLevel = false
            };
            VirtualMap<char> foregroundData = stage.ForegroundData;

            this.colliderGrid = new ColliderGrid(foregroundData.Columns, foregroundData.Rows, 8f, 8f);
            for (int x = 0; x < foregroundData.Columns; x += 50)
            {
                for (int y = 0; y < foregroundData.Rows; y += 50)
                {
                    if (foregroundData.AnyInSegmentAtTile(x, y))
                    {
                        int index1 = x;
                        for (int index2 = Math.Min(index1 + 50, foregroundData.Columns); index1 < index2; ++index1)
                        {
                            int index3 = y;
                            for (int index4 = Math.Min(index3 + 50, foregroundData.Rows); index3 < index4; ++index3)
                            {
                                if (foregroundData[index1, index3] != '0')
                                    this.colliderGrid[index1, index3] = true;
                                else
                                {
                                    this.colliderGrid[index1, index3] = false;
                                }
                            }
                        }
                    }
                }
            }
            //VirtualMap<char> backgroundData = stage.BackgroundData;
            foregroundTiles.GenerateTiles(this.fgTilemap, foregroundData, 0, 0, foregroundData.Columns, foregroundData.Rows, false, '0', fgBehaviour, colliderGrid);
            //bgTileDictionary.GenerateTiles(this.bgTilemap, backgroundData, 0, 0, backgroundData.Columns, backgroundData.Rows, false, '0', bgBehaviour);


        }

    }
}