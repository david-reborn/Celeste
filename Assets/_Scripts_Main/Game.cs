using myd.celeste;
using myd.celeste.demo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._Scripts_Main
{
    public class Game : MonoBehaviour
    {
        public Player player;
        private void Start()
        {
            Gfx.Game = Atlas.FromAtlas(Path.Combine("Graphics", "Atlases", "Gameplay"), Atlas.AtlasDataFormat.Packer);
            Gfx.SpriteBank = new SpriteBank(Gfx.Game, Path.Combine("Graphics", "Sprites.xml"));

            Player playerPrefab = Resources.Load<Player>("Player");
            this.player = Instantiate<Player>(playerPrefab);
            this.player.Load(Vector2.zero, PlayerSpriteMode.Madeline);
        }

    }
}
