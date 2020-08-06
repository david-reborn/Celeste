using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace myd.celeste.demo
{
    /// <summary>
    /// 前景Tile
    /// </summary>
    public class SolidTile : Tile
    {
        private Autotiler.Tiles tiles;

        public void SetTile(Autotiler.Tiles tiles)
        {
            this.tiles = tiles;
        }

        public override void GetTileData(Vector3Int location, ITilemap tilemap, ref TileData tileData)
        {
            MTexture texture = RandomUtil.Random.Choose<MTexture>(tiles.Textures);
            tileData.sprite = texture.GetSprite();
            tileData.color = Color.white;
            //var m = tileData.transform;
            //m.SetTRS(Vector3.zero, GetRotation((byte)mask), Vector3.one);
            //tileData.transform = m;
            tileData.flags = TileFlags.LockTransform;
            tileData.colliderType = this.colliderType;
            if (this.colliderType == ColliderType.None)
            {
                Debug.Log(111);
            }
        }

        public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
        {
            bool result = base.StartUp(position, tilemap, go);
            return result;
        }

#if UNITY_EDITOR
        // The following is a helper that adds a menu item to create a RoadTile Asset
        [MenuItem("TileMaps/SolidTile")]
        public static void CreateRoadTile()
        {
            string path = EditorUtility.SaveFilePanelInProject("Save Road Tile", "SolidTile", "Asset", "Save Road Tile", "Assets");
            if (path == "")
                return;
            AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<SolidTile>(), path);
        }
#endif
    }
}
