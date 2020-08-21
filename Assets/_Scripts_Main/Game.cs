using myd.celeste;
using myd.celeste.demo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace myd.celeste.demo
{
    public class Game : MonoBehaviour
    {
        public GameObject spritePrefab;

        public static Game instance;
        [HideInInspector]
        public Player player;

        public void Awake()
        {
            Debug.Log("Game Awake");
            instance = this;
            Gfx.Game = Atlas.FromAtlas(Path.Combine("Graphics", "Atlases", "Gameplay"), Atlas.AtlasDataFormat.Packer);
        }
        private void Start()
        {
            Gfx.SpriteBank = new SpriteBank(Gfx.Game, Path.Combine("Graphics", "Sprites.xml"));
            //PlayerSprite.ClearFramesMetadata();
            //PlayerSprite.CreateFramesMetadata("player");
            //PlayerSprite.CreateFramesMetadata("player_no_backpack");
            //PlayerSprite.CreateFramesMetadata("badeline");
            //PlayerSprite.CreateFramesMetadata("player_badeline");
            //PlayerSprite.CreateFramesMetadata("player_playback");
            //Player playerPrefab = Resources.Load<Player>("Player");
            //this.player = Instantiate<Player>(playerPrefab);
            //this.player.Load(Vector2.zero, PlayerSpriteMode.Madeline);
        }

        public void Draw(MTexture mTexture, GameObject parent, Vector2 position, Color color, Vector2 scale)
        {
            if (mTexture == null)
            {
                return;
            }
            GameObject gb = Instantiate(spritePrefab);
            gb.transform.SetParent(parent.transform, false);
            gb.GetComponent<SpriteRenderer>().sprite = mTexture.GetSprite();
            gb.GetComponent<SpriteRenderer>().color = color;
            gb.transform.localPosition = position;
            gb.transform.localScale = scale;
        }

        private void Update()
        {
            //if (InputManager.Jump.Pressed)
            //{
            //    Debug.Log("Jumping");
            //}
        }
    }
}
