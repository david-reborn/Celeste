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
        private Tiles fgTiles;
        private Tiles bgTiles;

        private ColliderGrid colliderGrid;
        void Start()
        {
            Debug.Log("==读取GamePlay的Texture");
            Gfx.Game = Atlas.FromAtlas(Path.Combine("Graphics", "Atlases", "Gameplay"), Atlas.AtlasDataFormat.Packer);
            fgTiles = new Tiles(Path.Combine(Util.GAME_PATH_CONTENT, Path.Combine("Graphics", "ForegroundTiles.xml")));
            bgTiles = new Tiles(Path.Combine(Util.GAME_PATH_CONTENT, Path.Combine("Graphics", "BackgroundTiles.xml")));
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
            VirtualMap<char> backgroundData = stage.BackgroundData;

            colliderGrid = new ColliderGrid(foregroundData, 8, 8);


            fgTiles.GenerateTiles(this.fgTilemap, foregroundData, 0, 0, foregroundData.Columns, foregroundData.Rows, false, '0', fgBehaviour);
            bgTiles.GenerateTiles(this.bgTilemap, backgroundData, 0, 0, backgroundData.Columns, backgroundData.Rows, false, '0', bgBehaviour);

            

        }

        

        
    }
}