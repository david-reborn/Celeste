using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace myd.celeste.demo
{
    public class UnityParticle : MonoBehaviour
    {
        ParticleType mParticleData;

        public ParticleSystem mParticle;

        public void Start()
        {
            Chooser<MTexture> chooser1 = new Chooser<MTexture>(new MTexture[4]
            {
                Gfx.Game["particles/smoke0"],
                Gfx.Game["particles/smoke1"],
                Gfx.Game["particles/smoke2"],
                Gfx.Game["particles/smoke3"]
            });
            this.mParticleData = new ParticleType()
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
            //初始化粒子系统
            mParticle = this.GetComponent<ParticleSystem>();
            ParticleSystem.MainModule mainModule = mParticle.main;
            mainModule.playOnAwake = false;
            ParticleSystem.TextureSheetAnimationModule aTextureAnim = mParticle.textureSheetAnimation;
            aTextureAnim.SetSprite(0, chooser1[0].GetSprite());
            mainModule.startLifetime = new ParticleSystem.MinMaxCurve(this.mParticleData.LifeMin, this.mParticleData.LifeMax);
            mainModule.startSize = new ParticleSystem.MinMaxCurve(this.mParticleData.Size - this.mParticleData.SizeRange / 2f, this.mParticleData.Size + this.mParticleData.SizeRange / 2f);
            mainModule.startRotation = new ParticleSystem.MinMaxCurve(Mathf.Rad2Deg * (this.mParticleData.Direction - this.mParticleData.DirectionRange / 2f), Mathf.Rad2Deg * (this.mParticleData.Direction + this.mParticleData.DirectionRange / 2f));
            mainModule.startSpeed = new ParticleSystem.MinMaxCurve(this.mParticleData.SpeedMin, this.mParticleData.SpeedMax);
        }

        public void Update()
        {
            if (Input.GetKeyUp(KeyCode.W))
            {
                Play();
            }
        }

        public void Play()
        {
            this.mParticle.Play();
        }

    }
}
