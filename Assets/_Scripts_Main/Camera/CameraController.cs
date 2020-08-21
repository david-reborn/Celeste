using UnityEngine;
using System.Collections;
using System;

namespace myd.celeste.demo
{
    public class CameraController : MonoBehaviour
    {
        public static CameraController instance;
        public float RawTimeActive;
        private Vector2 shakeDirection;
        private int lastDirectionalShake;
        private float shakeTimer;
        private Vector2 cameraPreShake;

        public Vector2 ShakeVector { get; private set; }
        public Vector2 originPosition;
        void Awake()
        {
            instance = this;
        }
        void Start()
        {
            originPosition = Camera.main.transform.position;
        }

        void Update()
        {
            RawTimeActive += Time.deltaTime;
            if ((double)this.shakeTimer > 0.0)
            {
                if (this.OnRawInterval(0.04f))
                {
                    int num2 = (int)Math.Ceiling((double)this.shakeTimer * 10.0);
                    if (this.shakeDirection == Vector2.zero)
                    {
                        this.ShakeVector = new Vector2((float)(-num2 + RandomUtil.Random.Next(num2 * 2 + 1)), (float)(-num2 + RandomUtil.Random.Next(num2 * 2 + 1)));
                    }
                    else
                    {
                        if (this.lastDirectionalShake == 0)
                            this.lastDirectionalShake = 1;
                        else
                            this.lastDirectionalShake *= -1;
                        this.ShakeVector = -this.shakeDirection * this.lastDirectionalShake * num2;
                    }
                }
                this.shakeTimer -= Time.deltaTime;
            }
            else
            {
                this.ShakeVector = Vector2.zero;
            }
            Camera.main.gameObject.transform.position = originPosition + this.ShakeVector;
        }

        public bool OnRawInterval(float interval)
        {
            return (int)((this.RawTimeActive - Time.deltaTime) / interval) < (int)(this.RawTimeActive / interval);
        }


        public void Shake(float time = 0.3f)
        {
            //if (Settings.Instance.DisableScreenShake)
            //    return;
            this.shakeDirection = Vector2.zero;
            this.shakeTimer = Math.Max(this.shakeTimer, time);
        }

        public void StopShake()
        {
            this.shakeTimer = 0.0f;
        }

        public void DirectionalShake(Vector2 dir, float time = 0.3f)
        {
            //if (Settings.Instance.DisableScreenShake)
            //    return;
            this.shakeDirection = dir.normalized;
            this.lastDirectionalShake = 0;
            this.shakeTimer = Math.Max(this.shakeTimer, time);
        }
    }
}