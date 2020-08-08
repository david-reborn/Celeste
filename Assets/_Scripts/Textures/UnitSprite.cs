using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace myd.celeste.demo
{
    public class UnitSprite : MonoBehaviour
    {
        public MTexture Texture;
        [HideInInspector]
        public Vector2 Position;
        public float Rate = 1f;
        [HideInInspector]
        public bool UseRawDeltaTime;
        [HideInInspector]
        public Vector2? Justify;
        public Action<string> OnFinish;
        public Action<string> OnLoop;
        public Action<string> OnFrameChange;
        public Action<string> OnLastFrame;
        public Action<string, string> OnChange;
        private Atlas atlas;
        [HideInInspector]
        public string Path;
        private Dictionary<string, Animation> animations;
        protected Animation currentAnimation;
        private float animationTimer;
        private int width;
        private int height;
        public Vector2 Origin;

        protected MSprite mSprite;

        public bool Animating { get; private set; }

        public string CurrentAnimationID { get; private set; }

        public string LastAnimationID { get; private set; }

        public int CurrentAnimationFrame { get; private set; }

        public int CurrentAnimationTotalFrames
        {
            get
            {
                return this.currentAnimation != null ? this.currentAnimation.Frames.Length : 0;
            }
        }

        public float Width
        {
            get
            {
                return (float)this.width;
            }
        }

        public float Height
        {
            get
            {
                return (float)this.height;
            }
        }

        public void Awake()
        {
            MSprite mSpritePrefab = Resources.Load<MSprite>("MSprite");
            mSprite = Instantiate(mSpritePrefab, this.transform, false);
            mSprite.name = "MSprite";
        }

        public void Init(Atlas atlas, string path)
        {
            this.atlas = atlas;
            this.Path = path;
            this.animations = new Dictionary<string, Animation>((IEqualityComparer<string>)StringComparer.OrdinalIgnoreCase);
            this.CurrentAnimationID = "";
            
        }

        

        private void Update()
        {
            AnimUpdate();

            RenderUpdate();
        }

        private void RenderUpdate()
        {
        }

        private void AnimUpdate()
        {
            float deltatime = Time.deltaTime;
            if (!this.Animating)
                return;
            if (this.UseRawDeltaTime)
                this.animationTimer += deltatime * this.Rate;
            else
                this.animationTimer += deltatime * this.Rate;
            if ((double)Math.Abs(this.animationTimer) < (double)this.currentAnimation.Delay)
                return;
            this.CurrentAnimationFrame += Math.Sign(this.animationTimer);
            this.animationTimer -= (float)Math.Sign(this.animationTimer) * this.currentAnimation.Delay;
            if (this.CurrentAnimationFrame < 0 || this.CurrentAnimationFrame >= this.currentAnimation.Frames.Length)
            {
                string currentAnimationId1 = this.CurrentAnimationID;
                if (this.OnLastFrame != null)
                    this.OnLastFrame(this.CurrentAnimationID);
                string currentAnimationId2 = this.CurrentAnimationID;
                if (!(currentAnimationId1 == currentAnimationId2))
                    return;
                if (this.currentAnimation.Goto != null)
                {
                    this.CurrentAnimationID = this.currentAnimation.Goto.Choose();
                    if (this.OnChange != null)
                        this.OnChange(this.LastAnimationID, this.CurrentAnimationID);
                    this.LastAnimationID = this.CurrentAnimationID;
                    this.currentAnimation = this.animations[this.LastAnimationID];
                    this.CurrentAnimationFrame = this.CurrentAnimationFrame >= 0 ? 0 : this.currentAnimation.Frames.Length - 1;
                    this.SetFrame(this.currentAnimation.Frames[this.CurrentAnimationFrame]);
                    if (this.OnLoop == null)
                        return;
                    this.OnLoop(this.CurrentAnimationID);
                }
                else
                {
                    this.CurrentAnimationFrame = this.CurrentAnimationFrame >= 0 ? this.currentAnimation.Frames.Length - 1 : 0;
                    this.Animating = false;
                    string currentAnimationId3 = this.CurrentAnimationID;
                    this.CurrentAnimationID = "";
                    this.currentAnimation = (Animation)null;
                    this.animationTimer = 0.0f;
                    if (this.OnFinish == null)
                        return;
                    this.OnFinish(currentAnimationId3);
                }
            }
            else
            {
                this.SetFrame(this.currentAnimation.Frames[this.CurrentAnimationFrame]);
            }
        }

        private void SetFrame(MTexture texture)
        {
            if (texture == this.Texture)
                return;
            this.Texture = texture;
            if (this.width == 0)
                this.width = texture.Width;
            if (this.height == 0)
                this.height = texture.Height;
            if (this.Justify.HasValue)
            {
                this.Origin = new Vector2((float)this.Texture.Width * (float)this.Justify.Value.x, (float)this.Texture.Height * (float)this.Justify.Value.y);
                //this.mSprite.transform.localPosition = new Vector2(this.Origin.x, this.Origin.y) - this.Texture.DrawOffset; 
            }
            else
            {
                //this.mSprite.transform.localPosition = new Vector2(this.Origin.x - this.Texture.Center.x-this.Texture.DrawOffset.x, this.Origin.y - this.Texture.Center.y - this.Texture.DrawOffset.x);
            }
            texture.LoadSprite(this.Origin);
            this.mSprite.SetSprite(texture);
            if (this.OnFrameChange == null)
                return;
            this.OnFrameChange(this.CurrentAnimationID);
        }

        public void AddLoop(string id, string path, float delay)
        {
            this.animations[id] = new Animation()
            {
                Delay = delay,
                Frames = this.GetFrames(path, (int[])null),
                Goto = new Chooser<string>(id, 1f)
            };
        }

        public void AddLoop(string id, string path, float delay, params int[] frames)
        {
            this.animations[id] = new Animation()
            {
                Delay = delay,
                Frames = this.GetFrames(path, frames),
                Goto = new Chooser<string>(id, 1f)
            };
        }

        public void AddLoop(string id, float delay, params MTexture[] frames)
        {
            this.animations[id] = new Animation()
            {
                Delay = delay,
                Frames = frames,
                Goto = new Chooser<string>(id, 1f)
            };
        }

        public void Add(string id, string path)
        {
            this.animations[id] = new Animation()
            {
                Delay = 0.0f,
                Frames = this.GetFrames(path, (int[])null),
                Goto = (Chooser<string>)null
            };
        }

        public void Add(string id, string path, float delay)
        {
            this.animations[id] = new Animation()
            {
                Delay = delay,
                Frames = this.GetFrames(path, (int[])null),
                Goto = (Chooser<string>)null
            };
        }

        public void Add(string id, string path, float delay, params int[] frames)
        {
            this.animations[id] = new Animation()
            {
                Delay = delay,
                Frames = this.GetFrames(path, frames),
                Goto = (Chooser<string>)null
            };
        }

        public void Add(string id, string path, float delay, string into)
        {
            this.animations[id] = new Animation()
            {
                Delay = delay,
                Frames = this.GetFrames(path, (int[])null),
                Goto = Chooser<string>.FromString<string>(into)
            };
        }

        public void Add(string id, string path, float delay, Chooser<string> into)
        {
            this.animations[id] = new Animation()
            {
                Delay = delay,
                Frames = this.GetFrames(path, (int[])null),
                Goto = into
            };
        }

        public void Add(string id, string path, float delay, string into, params int[] frames)
        {
            this.animations[id] = new Animation()
            {
                Delay = delay,
                Frames = this.GetFrames(path, frames),
                Goto = Chooser<string>.FromString<string>(into)
            };
        }

        public void Add(string id, float delay, string into, params MTexture[] frames)
        {
            this.animations[id] = new Animation()
            {
                Delay = delay,
                Frames = frames,
                Goto = Chooser<string>.FromString<string>(into)
            };
        }

        public void Add(
          string id,
          string path,
          float delay,
          Chooser<string> into,
          params int[] frames)
        {
            this.animations[id] = new Animation()
            {
                Delay = delay,
                Frames = this.GetFrames(path, frames),
                Goto = into
            };
        }

        private MTexture[] GetFrames(string path, int[] frames = null)
        {
            MTexture[] mtextureArray1;
            if (frames == null || frames.Length == 0)
            {
                mtextureArray1 = this.atlas.GetAtlasSubtextures(this.Path + path).ToArray();
            }
            else
            {
                string key = this.Path + path;
                MTexture[] mtextureArray2 = new MTexture[frames.Length];
                for (int index = 0; index < frames.Length; ++index)
                {
                    MTexture atlasSubtexturesAt = this.atlas.GetAtlasSubtexturesAt(key, frames[index]);
                    if (atlasSubtexturesAt == null)
                        throw new Exception("Can't find sprite " + key + " with index " + (object)frames[index]);
                    mtextureArray2[index] = atlasSubtexturesAt;
                }
                mtextureArray1 = mtextureArray2;
            }
            this.width = Math.Max(mtextureArray1[0].Width, this.width);
            this.height = Math.Max(mtextureArray1[0].Height, this.height);
            return mtextureArray1;
        }

        public void Play(string id, bool restart = false, bool randomizeFrame = false)
        {
            if (!(this.CurrentAnimationID != id | restart))
                return;
            if (this.OnChange != null)
                this.OnChange(this.LastAnimationID, id);
            this.LastAnimationID = this.CurrentAnimationID = id;
            this.currentAnimation = this.animations[id];
            this.Animating = (double)this.currentAnimation.Delay > 0.0;
            if (randomizeFrame)
            {
                this.animationTimer = RandomUtil.Random.NextFloat(this.currentAnimation.Delay);
                this.CurrentAnimationFrame = RandomUtil.Random.Next(this.currentAnimation.Frames.Length);
            }
            else
            {
                this.animationTimer = 0.0f;
                this.CurrentAnimationFrame = 0;
            }
            this.SetFrame(this.currentAnimation.Frames[this.CurrentAnimationFrame]);
        }

        internal UnitSprite()
        {
        }

        internal UnitSprite CreateClone()
        {
            return this.CloneInto(new UnitSprite());
        }

        internal UnitSprite CloneInto(UnitSprite clone)
        {
            clone.Texture = this.Texture;
            clone.Position = this.Position;
            clone.Justify = this.Justify;
            clone.Origin = this.Origin;
            clone.animations = new Dictionary<string, Animation>((IDictionary<string, Animation>)this.animations, (IEqualityComparer<string>)StringComparer.OrdinalIgnoreCase);
            clone.currentAnimation = this.currentAnimation;
            clone.animationTimer = this.animationTimer;
            clone.width = this.width;
            clone.height = this.height;
            clone.Animating = this.Animating;
            clone.CurrentAnimationID = this.CurrentAnimationID;
            clone.LastAnimationID = this.LastAnimationID;
            clone.CurrentAnimationFrame = this.CurrentAnimationFrame;
            return clone;
        }

        public UnitSprite CenterOrigin()
        {
            this.Origin.x = this.Width / 2f;
            this.Origin.y = this.Height / 2f;
            return this;
        }

        public UnitSprite JustifyOrigin(Vector2 at)
        {
            this.Origin.x = this.Width * at.x;
            this.Origin.y = this.Height * at.y;
            return this;
        }

        public class Animation
        {
            public float Delay;
            public MTexture[] Frames;
            public Chooser<string> Goto;
        }

    }
}
