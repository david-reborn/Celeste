using System;
using UnityEngine;

namespace myd.celeste.demo
{
    public class Particle : MonoBehaviour
    {

        public Entity Track;
        public ParticleType Type;
        public MTexture Source;
        public Color Color;
        public Color StartColor;
        public Vector2 Position;
        public Vector2 Speed;
        public float Size;
        public float StartSize;
        public float Life;
        public float StartLife;
        public float ColorSwitch;
        public float Rotation;
        public float Spin;

        private SpriteRenderer spriteRenderer;
        public void Awake()
        {
            this.spriteRenderer = this.GetComponent<SpriteRenderer>();
        }

        public void Reload()
        {
            this.spriteRenderer.color = Color;
            this.spriteRenderer.sprite = Source.GetSprite();
            this.gameObject.SetActive(true);
        }
        public bool SimulateFor(float duration)
        {
            if ((double)duration > (double)this.Life)
            {
                this.Life = 0.0f;
                this.gameObject.SetActive(false);
                return false;
            }
            //float num1 = Time.timeScale * ((float)Engine.Instance.get_TargetElapsedTime().Milliseconds / 1000f);
            float num1 = Time.deltaTime;
            if ((double)num1 > 0.0)
            {
                for (float num2 = 0.0f; (double)num2 < (double)duration; num2 += num1)
                    this.OnUpdate(new float?(num1));
            }
            return true;
        }

        public void OnUpdate(float? delta = null)
        {
            float num1 = !delta.HasValue ? Time.deltaTime : delta.Value;
            float t = this.Life / this.StartLife;
            this.Life -= num1;
            if ((double)this.Life <= 0.0)
            {
                this.gameObject.SetActive(false);
            }
            else
            {
                if (this.Type.RotationMode == ParticleType.RotationModes.SameAsDirection)
                {
                    if (this.Speed!=Vector2.zero)
                        this.Rotation = this.Speed.Angle();
                }
                else
                    this.Rotation += this.Spin * num1;
                float num2 = this.Type.FadeMode != ParticleType.FadeModes.Linear ? (this.Type.FadeMode != ParticleType.FadeModes.Late ? (this.Type.FadeMode != ParticleType.FadeModes.InAndOut ? 1f : ((double)t <= 0.75 ? ((double)t >= 0.25 ? 1f : t / 0.25f) : (float)(1.0 - ((double)t - 0.75) / 0.25))) : Math.Min(1f, t / 0.25f)) : t;
                if ((double)num2 == 0.0)
                {
                    this.Color = new Color(Color.r, Color.g, Color.b, 0);
                }
                else
                {
                    if (this.Type.ColorMode == ParticleType.ColorModes.Static)
                        this.Color = this.StartColor;
                    else if (this.Type.ColorMode == ParticleType.ColorModes.Fade)
                        this.Color = Color.Lerp(this.Type.Color2, this.StartColor, t);
                    else if (this.Type.ColorMode == ParticleType.ColorModes.Blink)
                        this.Color = RandomUtil.BetweenInterval(this.Life, 0.1f) ? this.StartColor : this.Type.Color2;
                    else if (this.Type.ColorMode == ParticleType.ColorModes.Choose)
                        this.Color = this.StartColor;
                    if ((double)num2 < 1.0)
                        this.Color = this.Color*num2;
                }
                this.Position = (this.Position + this.Speed * num1);
                this.Speed = this.Speed + this.Type.Acceleration*num1;
                this.Speed = Util.Approach(this.Speed, Vector2.zero, this.Type.Friction * num1);
                if ((double)this.Type.SpeedMultiplier != 1.0)
                    this.Speed = this.Speed*(float)Math.Pow((double)this.Type.SpeedMultiplier, (double)num1);
                if (!this.Type.ScaleOut)
                    return;
                this.Size = this.StartSize * Ease.CubeOut(t);
            }
        }

        public void OnRender()
        {
            Vector2 vector2 = new Vector2((float)(int)this.Position.x, (float)(int)this.Position.y);
            if (this.Track != null)
                vector2 = vector2 + this.Track.Position;
            this.transform.position = vector2;
            this.transform.localScale = Vector3.one * this.Size;
            this.transform.rotation = Quaternion.Euler(0, 0, Rotation);
            //Draw.SpriteBatch.Draw(this.Source.Texture.Texture, vector2, new Rectangle?(this.Source.ClipRect), this.Color, this.Rotation, this.Source.Center, this.Size, (SpriteEffects)0, 0.0f);
        }

        //public void Render(float alpha)
        //{
        //    Vector2 vector2;
        //    ((Vector2)ref vector2).\u002Ector((float)(int)this.Position.X, (float)(int)this.Position.Y);
        //    if (this.Track != null)
        //        vector2 = Vector2.op_Addition(vector2, this.Track.Position);
        //    Draw.SpriteBatch.Draw(this.Source.Texture.Texture, vector2, new Rectangle?(this.Source.ClipRect), Color.op_Multiply(this.Color, alpha), this.Rotation, this.Source.Center, this.Size, (SpriteEffects)0, 0.0f);
        //}
    }
}
