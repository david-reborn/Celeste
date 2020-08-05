using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace myd.celeste.demo
{
    public class TileMapController : MonoBehaviour
    {
        public Tilemap mTileMap;

        public Tile tile;

        public void Update()
        {
            if (Input.GetKeyUp(KeyCode.Alpha0))
            {
                mTileMap.SetTile(Vector3Int.zero, tile);
            }

        }
    }
}
