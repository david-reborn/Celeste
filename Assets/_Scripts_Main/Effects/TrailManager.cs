using UnityEngine;
using System.Collections;

namespace myd.celeste.demo
{
    public class TrailManager : MonoBehaviour
    {
        private TrailManager.Snapshot[] snapshots = new TrailManager.Snapshot[64];
        private static BlendState MaxBlendState;
        private const int size = 64;
        private const int cols = 8;
        private const int rows = 8;
        private VirtualRenderTarget buffer;
        private bool dirty;

        public TrailManager()
        {
            this.Tag = (int)Tags.Global;
            this.Depth = 10;
            this.Add((Component)new BeforeRenderHook(new Action(this.BeforeRender)));
            this.Add((Component)new MirrorReflection());
        }

        public static void Add(Player player, Color color, float duration = 1f, bool frozenUpdate = false, bool useRawDeltaTime = false)
        {
            PlayerSprite playerSprite = player.Sprite;
            Monocle.Image sprite = (Monocle.Image)entity.Get<PlayerSprite>() ?? (Monocle.Image)entity.Get<Sprite>();
            PlayerHair hair = entity.Get<PlayerHair>();
            TrailManager.Add(entity.Position, sprite, hair, sprite.Scale, color, entity.Depth + 1, duration, frozenUpdate, useRawDeltaTime);
        }

        public static TrailManager.Snapshot Add(Vector2 position, Monocle.Image sprite, PlayerHair hair, Vector2 scale, Color color,
                int depth, float duration = 1f, bool frozenUpdate = false, bool useRawDeltaTime = false)
        {
            TrailManager manager = Engine.Scene.Tracker.GetEntity<TrailManager>();
            if (manager == null)
            {
                manager = new TrailManager();
                Engine.Scene.Add((Entity)manager);
            }
            for (int index = 0; index < manager.snapshots.Length; ++index)
            {
                if (manager.snapshots[index] == null)
                {
                    TrailManager.Snapshot snapshot = Engine.Pooler.Create<TrailManager.Snapshot>();
                    snapshot.Init(manager, index, position, sprite, hair, scale, color, duration, depth, frozenUpdate, useRawDeltaTime);
                    manager.snapshots[index] = snapshot;
                    manager.dirty = true;
                    Engine.Scene.Add((Entity)snapshot);
                    return snapshot;
                }
            }
            return (TrailManager.Snapshot)null;
        }


        public class Snapshot : Entity
        {
            public TrailManager Manager;
            public Monocle.Image Sprite;
            public Vector2 SpriteScale;
            public PlayerHair Hair;
            public int Index;
            public Color Color;
            public float Percent;
            public float Duration;
            public bool Drawn;
            public bool UseRawDeltaTime;

            public Snapshot()
            {
                this.Add((Component)new MirrorReflection());
            }

            public void Init(
              TrailManager manager,
              int index,
              Vector2 position,
              Monocle.Image sprite,
              PlayerHair hair,
              Vector2 scale,
              Color color,
              float duration,
              int depth,
              bool frozenUpdate,
              bool useRawDeltaTime)
            {
                this.Tag = (int)Tags.Global;
                if (frozenUpdate)
                    this.Tag |= (int)Tags.FrozenUpdate;
                this.Manager = manager;
                this.Index = index;
                this.Position = position;
                this.Sprite = sprite;
                this.SpriteScale = scale;
                this.Hair = hair;
                this.Color = color;
                this.Percent = 0.0f;
                this.Duration = duration;
                this.Depth = depth;
                this.Drawn = false;
                this.UseRawDeltaTime = useRawDeltaTime;
            }

            public override void Update()
            {
                if ((double)this.Duration <= 0.0)
                {
                    if (!this.Drawn)
                        return;
                    this.RemoveSelf();
                }
                else
                {
                    if ((double)this.Percent >= 1.0)
                        this.RemoveSelf();
                    this.Percent += /*(this.UseRawDeltaTime ? Engine.RawDeltaTime : Time.deltaTime)*/ Time.deltaTime / this.Duration;
                }
            }

            public override void Render()
            {
                VirtualRenderTarget buffer = this.Manager.buffer;
                Rectangle rectangle;
                ((Rectangle)ref rectangle).\u002Ector(this.Index % 8 * 64, this.Index / 8 * 64, 64, 64);
                float num = (double)this.Duration > 0.0 ? (float)(0.75 * (1.0 - (double)Ease.CubeOut(this.Percent))) : 1f;
                if (buffer == null)
                    return;
                Draw.SpriteBatch.Draw((Texture2D)(RenderTarget2D)buffer, this.Position, new Rectangle?(rectangle), Color.op_Multiply(this.Color, num), 0.0f, Vector2.op_Multiply(new Vector2(64f, 64f), 0.5f), Vector2.get_One(), (SpriteEffects)0, 0.0f);
            }

            public override void Removed(Scene scene)
            {
                if (this.Manager != null)
                    this.Manager.snapshots[this.Index] = (TrailManager.Snapshot)null;
                base.Removed(scene);
            }
        }
    }

}