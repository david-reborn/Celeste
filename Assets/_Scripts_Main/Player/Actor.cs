﻿using UnityEngine;
using System.Collections;
using System;

namespace myd.celeste.demo
{
    public class Actor : MonoBehaviour
    {
        public bool AllowPushing = true;
        public float LiftSpeedGraceTime = 0.16f;
        //public Collision SquishCallback;
        public bool TreatNaive;
        private Vector2 movementCounter;
        public bool IgnoreJumpThrus;
        private Vector2 currentLiftSpeed;
        private Vector2 lastLiftSpeed;
        private float liftSpeedTimer;

        protected BoxCollider2D mCollider = null;

        public bool MoveH(float moveH, Collision onCollide = null, Solid pusher = null)
        {
            this.movementCounter.x += moveH;
            int num = (int)Math.Round((double)this.movementCounter.x, MidpointRounding.ToEven);
            bool flag = num != 0;
            bool result;
            if (flag)
            {
                this.movementCounter.x -= (float)num;
                result = this.MoveHExact(num, onCollide, pusher);
            }
            else
            {
                result = false;
            }
            return result;
        }

        public bool MoveV(float moveV, Collision onCollide = null, Solid pusher = null)
        {
            this.movementCounter.y += moveV;
            int moveV1 = (int)Math.Round((double)this.movementCounter.y, MidpointRounding.ToEven);
            if ((uint)moveV1 <= 0U)
                return false;
            this.movementCounter.y -= (float)moveV1;
            return this.MoveVExact(moveV1, onCollide, pusher);
        }

        public bool MoveHExact(int moveH, Collision onCollide = null, Solid pusher = null)
        {
            Vector2 position = this.transform.position;
            Vector2 targetPosition = position + Vector2.right * (float)moveH;
            int num = Math.Sign(moveH);
            int num2 = 0;
            while (moveH != 0)
            {
                //Solid solid = base.CollideFirst<Solid>(this.Position + Vector2.UnitX * (float)num);
                //bool flag = solid != null;
                //if (flag)
                //{
                //    this.movementCounter.X = 0f;
                //    bool flag2 = onCollide != null;
                //    if (flag2)
                //    {
                //        onCollide(new CollisionData
                //        {
                //            Direction = Vector2.UnitX * (float)num,
                //            Moved = Vector2.UnitX * (float)num2,
                //            TargetPosition = targetPosition,
                //            Hit = solid,
                //            Pusher = pusher
                //        });
                //    }
                //    return true;
                //}
                num2 += num;
                moveH -= num;
                this.transform.position += Vector3.right * (float)num;
            }
            return false;
        }

        public bool MoveVExact(int moveV, Collision onCollide = null, Solid pusher = null)
        {
            Vector2 position = this.transform.position;
            Vector2 vector2 = position + Vector2.down * (float)moveV;
            int num1 = Math.Sign(moveV);
            int num2 = 0;
            while ((uint)moveV > 0U)
            {
                Collider2D platform1 = ColliderUtil.OverlapBox(mCollider, Vector2.down, (float)num1-0.001f, 0, Player.PLATFORM_MASK);
                if (platform1 != null)
                {
                    this.movementCounter.y = 0.0f;
                    if (onCollide != null)
                        onCollide(new CollisionData()
                        {
                            Direction = Vector2.down * (float)num1,
                            Moved = Vector2.down * (float)num2,
                            TargetPosition = vector2,
                            //Hit = platform1,
                            //Pusher = pusher
                        });
                    Debug.Log("Touch the Platform");
                    return true;
                }
                //if (moveV > 0 && !this.IgnoreJumpThrus)
                //{
                //    Platform platform2 = (Platform)this.CollideFirstOutside<JumpThru>(this.Position + Vector2.UnitY * (float)num1);
                //    if (platform2 != null)
                //    {
                //        this.movementCounter.Y = 0.0f;
                //        if (onCollide != null)
                //            onCollide(new CollisionData()
                //            {
                //                Direction = Vector2.UnitY * (float)num1,
                //                Moved = Vector2.UnitY * (float)num2,
                //                TargetPosition = vector2,
                //                Hit = platform2,
                //                Pusher = pusher
                //            });
                //        return true;
                //    }
                //}
                num2 += num1;
                moveV -= num1;
                this.transform.position += Vector3.down * (float)num1;
            }
            return false;
        }

        public Vector2 LiftSpeed
        {
            set
            {
                this.currentLiftSpeed = value;
                if (!(value != Vector2.zero) || (double)this.LiftSpeedGraceTime <= 0.0)
                    return;
                this.lastLiftSpeed = value;
                this.liftSpeedTimer = this.LiftSpeedGraceTime;
            }
            get
            {
                return this.currentLiftSpeed == Vector2.zero ? this.lastLiftSpeed : this.currentLiftSpeed;
            }
        }
    }
}