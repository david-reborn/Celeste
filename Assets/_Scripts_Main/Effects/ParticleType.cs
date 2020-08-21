
using System.Collections.Generic;
using UnityEngine;

namespace myd.celeste.demo
{
    public class ParticleType
    {
        private static List<ParticleType> AllTypes = new List<ParticleType>();
        public MTexture Source;
        public Chooser<MTexture> SourceChooser;
        public Color Color;
        public Color Color2;
        public ParticleType.ColorModes ColorMode;
        public ParticleType.FadeModes FadeMode;
        public float SpeedMin;
        public float SpeedMax;
        public float SpeedMultiplier;
        public Vector2 Acceleration;
        public float Friction;
        public float Direction;
        public float DirectionRange;
        public float LifeMin;
        public float LifeMax;
        public float Size;
        public float SizeRange;
        public float SpinMin;
        public float SpinMax;
        public bool SpinFlippedChance;
        public ParticleType.RotationModes RotationMode;
        public bool ScaleOut;
        public bool UseActualDeltaTime;

        public ParticleType()
        {
            this.Color = this.Color2 = Color.white;
            this.ColorMode = ParticleType.ColorModes.Static;
            this.FadeMode = ParticleType.FadeModes.None;
            this.SpeedMin = this.SpeedMax = 0.0f;
            this.SpeedMultiplier = 1f;
            this.Acceleration = Vector2.zero;
            this.Friction = 0.0f;
            this.Direction = this.DirectionRange = 0.0f;
            this.LifeMin = this.LifeMax = 0.0f;
            this.Size = 2f;
            this.SizeRange = 0.0f;
            this.SpinMin = this.SpinMax = 0.0f;
            this.SpinFlippedChance = false;
            this.RotationMode = ParticleType.RotationModes.None;
            ParticleType.AllTypes.Add(this);
        }

        public ParticleType(ParticleType copyFrom)
        {
            this.Source = copyFrom.Source;
            this.SourceChooser = copyFrom.SourceChooser;
            this.Color = copyFrom.Color;
            this.Color2 = copyFrom.Color2;
            this.ColorMode = copyFrom.ColorMode;
            this.FadeMode = copyFrom.FadeMode;
            this.SpeedMin = copyFrom.SpeedMin;
            this.SpeedMax = copyFrom.SpeedMax;
            this.SpeedMultiplier = copyFrom.SpeedMultiplier;
            this.Acceleration = copyFrom.Acceleration;
            this.Friction = copyFrom.Friction;
            this.Direction = copyFrom.Direction;
            this.DirectionRange = copyFrom.DirectionRange;
            this.LifeMin = copyFrom.LifeMin;
            this.LifeMax = copyFrom.LifeMax;
            this.Size = copyFrom.Size;
            this.SizeRange = copyFrom.SizeRange;
            this.RotationMode = copyFrom.RotationMode;
            this.SpinMin = copyFrom.SpinMin;
            this.SpinMax = copyFrom.SpinMax;
            this.SpinFlippedChance = copyFrom.SpinFlippedChance;
            this.ScaleOut = copyFrom.ScaleOut;
            this.UseActualDeltaTime = copyFrom.UseActualDeltaTime;
            ParticleType.AllTypes.Add(this);
        }

        public Particle Create(Particle particle, Vector2 position)
        {
            return this.Create(particle, position, this.Direction);
        }

        public Particle Create(Particle particle, Vector2 position, Color color)
        {
            return this.Create(particle, (Entity)null, position, this.Direction, color);
        }

        public Particle Create(Particle particle, Vector2 position, float direction)
        {
            return this.Create(particle, (Entity)null, position, direction, this.Color);
        }

        public Particle Create(Particle particle,
          Vector2 position,
          Color color,
          float direction)
        {
            return this.Create(particle, (Entity)null, position, direction, color);
        }

        public Particle Create(Particle particle,
          Entity entity,
          Vector2 position,
          float direction,
          Color color)
        {
            particle.Track = entity;
            particle.Type = this;
            particle.Position = position;
            particle.Source = this.SourceChooser == null ? (this.Source == null ? null/*Draw.Particle*/ : this.Source) : this.SourceChooser.Choose();
            particle.StartSize = (double)this.SizeRange == 0.0 ? (particle.Size = this.Size) : (particle.Size = this.Size - this.SizeRange * 0.5f + RandomUtil.Random.NextFloat(this.SizeRange));
            particle.StartColor = this.ColorMode != ParticleType.ColorModes.Choose ? (particle.Color = color) : (particle.Color = RandomUtil.Random.Choose<Color>(color, this.Color2));
            float angleRadians = (float)((double)direction - (double)this.DirectionRange / 2.0 + (double)RandomUtil.Random.NextFloat() * (double)this.DirectionRange);
            particle.Speed = Util.AngleToVector(angleRadians, RandomUtil.Random.Range(this.SpeedMin, this.SpeedMax));
            particle.StartLife = particle.Life = RandomUtil.Random.Range(this.LifeMin, this.LifeMax);
            particle.Rotation = this.RotationMode != ParticleType.RotationModes.Random ? (this.RotationMode != ParticleType.RotationModes.SameAsDirection ? 0.0f : angleRadians) : RandomUtil.Random.NextAngle();
            particle.Spin = RandomUtil.Random.Range(this.SpinMin, this.SpinMax);
            if (this.SpinFlippedChance)
                particle.Spin *= (float)RandomUtil.Random.Choose<int>(1, -1);
            return particle;
        }

        public enum ColorModes
        {
            Static,
            Choose,
            Blink,
            Fade,
        }

        public enum FadeModes
        {
            None,
            Linear,
            Late,
            InAndOut,
        }

        public enum RotationModes
        {
            None,
            Random,
            SameAsDirection,
        }
    }
}
