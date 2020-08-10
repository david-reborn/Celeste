using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace myd.celeste.demo
{
    public class PlayerHair : MonoBehaviour
    {
        public Color Color;
        public Color Border = Color.black;
        public float Alpha = 1f;
        public bool SimulateMotion = true;
        public Vector2 StepPerSegment = new Vector2(0.0f, -2f);//TODO
        public float StepInFacingPerSegment = 0.5f;
        public float StepApproach = 64f;
        public float StepYSinePerSegment = 0.0f;
        public List<Vector2> Nodes = new List<Vector2>();
        private List<MTexture> bangs = null;
        private float wave = 0.0f;
        public string Hair = "characters/player/hair00";
        public Facings Facing;
        public bool DrawPlayerSpriteOutline;
        public PlayerSprite Sprite;

        public PlayerHairNode hairNodePrefab;
        private PlayerHairNode[] hairs;
        public void BindPlayerSprite(PlayerSprite sprite)
        {
            this.Color = Player.NormalHairColor;
            this.bangs = Gfx.Game.GetAtlasSubtextures("characters/player/bangs");
            this.Sprite = sprite;
            for (int index = 0; index < sprite.HairCount; ++index)
                this.Nodes.Add(Vector2.zero);
            this.hairs = new PlayerHairNode[sprite.HairCount];
            for(int i = 0; i < hairs.Length; i++)
            {
                this.hairs[i] = Instantiate<PlayerHairNode>(hairNodePrefab);
                this.hairs[i].transform.SetParent(this.transform, false);
            }
        }

        private void Start()
        {
            Vector2 vector2 =  new Vector2((float)(-(int)this.Facing * 200), -200f);//TODO
            for (int index = 0; index < this.Nodes.Count; ++index)
                this.Nodes[index] = vector2;
        }

        public void AfterUpdate()
        {
            Vector2 pos = this.Sprite.transform.position;
            Vector2 temp = this.Sprite.HairOffset * new Vector2((float)this.Facing, -1f);//TODO
            this.Nodes[0] = pos + new Vector2(0.0f, 9f * this.Sprite.Scale.y) + temp;
            Vector2 target = this.Nodes[0] + new Vector2((float)((double)-(int)this.Facing * (double)this.StepInFacingPerSegment * 2.0), (float)Math.Sin((double)this.wave) * this.StepYSinePerSegment) + this.StepPerSegment;
            Vector2 node = this.Nodes[0];
            float num1 = 3f;
            for (int index = 1; index < this.Sprite.HairCount; ++index)
            {
                if (index >= this.Nodes.Count)
                    this.Nodes.Add(this.Nodes[index - 1]);
                if (this.SimulateMotion)
                {
                    float num2 = (float)(1.0 - (double)index / (double)this.Sprite.HairCount * 0.5) * this.StepApproach;
                    this.Nodes[index] = Util.Approach(this.Nodes[index], target, num2 * Time.deltaTime);
                }
                if ((double)(this.Nodes[index] - node).magnitude > (double)num1)
                    this.Nodes[index] = node + (this.Nodes[index] - node).normalized * num1;
                target = this.Nodes[index] + new Vector2((float)-(int)this.Facing * this.StepInFacingPerSegment, (float)Math.Sin((double)this.wave + (double)index * 0.8f) * this.StepYSinePerSegment) + this.StepPerSegment;
                node = this.Nodes[index];
            }
        }
        private bool rendered = false;
        public void Update()
        {
            this.wave += Time.deltaTime * 4f;

            AfterUpdate();
            if (!rendered)
            {
                Render();
                rendered = true;
            }
            for (int index = 0; index < this.Nodes.Count; ++index)
            {
                Vector2 hairScale = this.GetHairScale(index);
                this.hairs[index].transform.position = Nodes[index];
                this.hairs[index].transform.localScale = hairScale;
            }

        }

        private void Render()
        {
            //if (!this.Sprite.HasHair)
            //    return;
            Vector2 origin = new Vector2(5f, 5f); //TODO
            Color color1 = this.Border * this.Alpha;
            Color color2 = this.Color * this.Alpha;
            //if (this.DrawPlayerSpriteOutline)
            //{
            //    Color color3 = this.Sprite.Color;
            //    Vector2 position = this.Sprite.Position;
            //    this.Sprite.Color = color1;
            //    this.Sprite.Position = position + new Vector2(0.0f, -1f);
            //    this.Sprite.Render();
            //    this.Sprite.Position = position + new Vector2(0.0f, 1f);
            //    this.Sprite.Render();
            //    this.Sprite.Position = position + new Vector2(-1f, 0.0f);
            //    this.Sprite.Render();
            //    this.Sprite.Position = position + new Vector2(1f, 0.0f);
            //    this.Sprite.Render();
            //    this.Sprite.Color = color3;
            //    this.Sprite.Position = position;
            //}
            this.Nodes[0] = this.Nodes[0].Floor();
            if (color1.a > (byte)0)
            {
                for (int index = 0; index < this.Sprite.HairCount; ++index)
                {
                    int hairFrame = this.Sprite.HairFrame;
                    MTexture mtexture = index == 0 ? this.bangs[hairFrame] : Gfx.Game["characters/player/hair00"];
                    Vector2 hairScale = this.GetHairScale(index);
                    this.hairs[index].Draw(mtexture, new Vector2(-1f, 0.0f), origin, color1, hairScale);
                    this.hairs[index].Draw(mtexture, new Vector2( 1f, 0.0f), origin, color1, hairScale);
                    this.hairs[index].Draw(mtexture, new Vector2( 0f, -1f), origin, color1, hairScale);
                    this.hairs[index].Draw(mtexture, new Vector2( 0f,  1f), origin, color1, hairScale);
                }
            }
            for (int index = this.Sprite.HairCount - 1; index >= 0; --index)
            {
                int hairFrame = this.Sprite.HairFrame;
                MTexture mTexture = (index == 0 ? this.bangs[hairFrame] : Gfx.Game["characters/player/hair00"]);
                this.hairs[index].Draw(mTexture, Vector2.zero, origin, color2, this.GetHairScale(index));
            }
        }

        private Vector2 GetHairScale(int index)
        {
            float y = (float)(0.25 + (1.0 - (double)index / (double)this.Sprite.HairCount) * 0.75);
            return new Vector2((index == 0 ? (float)this.Facing : y) * Math.Abs(this.Sprite.Scale.x), y);
        }
    }
}
