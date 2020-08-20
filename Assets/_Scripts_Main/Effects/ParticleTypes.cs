using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace myd.celeste.demo
{
    public class ParticleTypes
    {
        public static ParticleType Dust;

        public static void Load()
        {
            Chooser<MTexture> chooser1 = new Chooser<MTexture>(new MTexture[4]
            {
                Gfx.Game["particles/smoke0"],
                Gfx.Game["particles/smoke1"],
                Gfx.Game["particles/smoke2"],
                Gfx.Game["particles/smoke3"]
            });
            ParticleTypes.Dust = new ParticleType()
            {
                SourceChooser = chooser1,
                Color = Color.white,
                Acceleration = new Vector2(0.0f, 4f),
                LifeMin = 0.3f,
                LifeMax = 0.5f,
                Size = 0.7f,
                SizeRange = 0.2f,
                Direction = 1.570796f,
                DirectionRange = 0.5f,
                SpeedMin = 5f,
                SpeedMax = 15f,
                RotationMode = ParticleType.RotationModes.Random,
                ScaleOut = true,
                UseActualDeltaTime = true
            };
        }
    }
}
