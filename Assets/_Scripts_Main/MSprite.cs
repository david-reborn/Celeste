using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace myd.celeste.demo
{
    
    public class MSprite : MonoBehaviour
    {
        private SpriteRenderer renderer;

        void Awake()
        {
            this.renderer = GetComponent<SpriteRenderer>();
        }

        public void SetSprite(MTexture mTexture)
        {
            this.renderer.sprite = mTexture.USprite;
            this.transform.localPosition = new Vector3(mTexture.Offset.x, mTexture.Offset.y, 0);
        }

        public void SetSprite(Vector2 position, MTexture mTexture, Color color)
        {
            this.renderer.sprite = mTexture.USprite;
            this.renderer.color = color;
            this.transform.localPosition = new Vector3(position.x, position.y, 0) + new Vector3(mTexture.Offset.x, mTexture.Offset.y, 0);
        }

        public void SetSortingOrder(int order)
        {
            this.renderer.sortingOrder = order;
        }
    }
}