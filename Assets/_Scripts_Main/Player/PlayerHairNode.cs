using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace myd.celeste.demo
{
    public class PlayerHairNode : MonoBehaviour
    {
        public void Draw(MTexture mTexture, Vector2 position, Vector2 origin, Color color, Vector2 scale)
        {
            MSprite mSpritePrefab = Resources.Load<MSprite>("MSprite");
            MSprite mSprite = Instantiate(mSpritePrefab, this.transform, false);
            mSprite.name = "MSprite";
            mSprite.transform.SetParent(this.transform, false);
            mSprite.transform.localScale = scale;
            mTexture.LoadSprite(origin);
            mSprite.SetSprite(position, mTexture, color);
        }

    }
}
