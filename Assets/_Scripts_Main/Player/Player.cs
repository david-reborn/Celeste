using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace myd.celeste.demo
{
    public class Player : MonoBehaviour
    {
        public static readonly Color NormalHairColor = Util.HexToColor("AC3232");

        private static Chooser<string> idleColdOptions = new Chooser<string>().Add("idleA", 5f).Add("idleB", 3f).Add("idleC", 1f);

        [HideInInspector]
        public IntroTypes IntroType { get; set; }           //出生方式
        [HideInInspector]
        public PlayerSprite Sprite;

        [HideInInspector]
        public PlayerHair playerHair;

        public void Load(Vector2 position, PlayerSpriteMode spriteMode)
        {
            PlayerSprite playerSpritePrefab = Resources.Load<PlayerSprite>("PlayerSprite");
            this.Sprite = Instantiate(playerSpritePrefab);
            this.Sprite.Load(spriteMode);
            this.Sprite.transform.SetParent(this.transform, false);

            PlayerHair playerHairPrefab = Resources.Load<PlayerHair>("PlayerHair");
            this.playerHair = Instantiate(playerHairPrefab);
            this.playerHair.BindPlayerSprite(Sprite);
            this.playerHair.transform.SetParent(this.transform, false);
        }


        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Alpha1))
            {
                //string id = Player.idleColdOptions.Choose();
                //this.Sprite.Play(id, false, false);
            }
        }
    }
}
