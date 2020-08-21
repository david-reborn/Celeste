using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace myd.celeste.demo
{
    /// <summary>
    /// 粒子系统
    /// </summary>
    public class ParticleController : MonoBehaviour
    {
        public static ParticleController instance;
        public Particle particlePrefab;
        public ParticleSystem2D Particles;
        public ParticleSystem2D ParticlesBG;
        public ParticleSystem2D ParticlesFG;
        public void Awake()
        {
            instance = this;
            Particles.Init(100, particlePrefab);
            ParticlesBG.Init(100, particlePrefab);
            ParticlesFG.Init(100, particlePrefab);
        }

        //初始化粒子配置
        public void Start()
        {
            ParticleTypes.Load();
        }

        public void Update()
        {
            if (Input.GetKeyUp(KeyCode.A))
            {
                Dust.Burst(Vector2.zero, Util.Angle(Vector2.up), 8);
            }
            if (Input.GetKeyUp(KeyCode.S))
            {
                CameraController.instance.DirectionalShake(Vector2.up, 0.2f);
            }
        }
    }

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
