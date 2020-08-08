using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


namespace myd.celeste.demo
{
    public class GamePlayerHair : MonoBehaviour
    {

        void Awake()
        {
            Gfx.Game = Atlas.FromAtlas(Path.Combine("Graphics", "Atlases", "Gameplay"), Atlas.AtlasDataFormat.Packer);
        }
        void Start()
        {
            PlayerSprite.ClearFramesMetadata();
            PlayerSprite.CreateFramesMetadata("player");
        }

        void Update()
        {

        }
    }
}
