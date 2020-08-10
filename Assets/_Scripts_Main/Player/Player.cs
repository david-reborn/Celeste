using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace myd.celeste.demo
{
    public class Player : Actor
    {
        public static LayerMask PLATFORM_MASK;

        #region Constants
        private const float DuckFriction = 500f;
        private const float MaxRun = 90f;
        private const float RunAccel = 1000f;
        private const float RunReduce = 400f;
        private const float AirMult = .65f;
        private const float HoldingMaxRun = 70f;

        private const float JumpGraceTime = 0.1f;
        private const float JumpSpeed = -105f;           //TODO 反方向 -105f;          
        private const float JumpHBoost = 40f;
        private const float VarJumpTime = .2f;

        private const float LiftYCap = -130f;
        private const float LiftXCap = 250f;

        public const float MaxFall = 160f;
        private const float Gravity = 900f;
        private const float HalfGravThreshold = 40f;
        private const float FastMaxFall = 240f;
        private const float FastMaxAccel = 300f;

        public const float ClimbMaxStamina = 110;
        private const float ClimbUpCost = 100 / 2.2f;
        private const float ClimbStillCost = 100 / 10f;
        private const float ClimbJumpCost = 110 / 4f;
        private const int ClimbCheckDist = 2;
        private const int ClimbUpCheckDist = 2;
        private const float ClimbNoMoveTime = .1f;
        public const float ClimbTiredThreshold = 20f;
        private const float ClimbUpSpeed = -45f;
        private const float ClimbDownSpeed = 80f;
        private const float ClimbSlipSpeed = 30f;
        private const float ClimbAccel = 900f;
        private const float ClimbGrabYMult = .2f;
        private const float ClimbHopY = -120f;
        private const float ClimbHopX = 100f;
        private const float ClimbHopForceTime = .2f;
        private const float ClimbJumpBoostTime = .2f;
        private const float ClimbHopNoWindTime = .3f;

        private const float WallSlideStartMax = 20f;
        private const float WallSlideTime = 1.2f;
        

        public const int StNormal = 0;
        public const int StClimb = 1;
        public const int StDash = 2;
        public const int StSwim = 3;
        public const int StBoost = 4;
        public const int StRedDash = 5;
        public const int StHitSquash = 6;
        public const int StLaunch = 7;
        public const int StPickup = 8;
        public const int StDreamDash = 9;
        public const int StSummitLaunch = 10;
        public const int StDummy = 11;
        public const int StIntroWalk = 12;
        public const int StIntroJump = 13;
        public const int StIntroRespawn = 14;
        public const int StIntroWakeUp = 15;
        public const int StBirdDashTutorial = 16;
        public const int StFrozen = 17;
        public const int StReflectionFall = 18;
        public const int StStarFly = 19;
        public const int StTempleFall = 20;
        public const int StCassetteFly = 21;
        public const int StAttract = 22;
        public const int StIntroMoonJump = 23;
        public const int StFlingBird = 24;
        public const int StIntroThinkForABit = 25;

        #endregion

        #region var
        public Vector2 Speed;
        public Facings Facing;

        private float wallSpeedRetentionTimer; // If you hit a wall, start this timer. If coast is clear within this timer, retain h-speed
        private float wallSpeedRetained;
        private int wallBoostDir;
        private float wallBoostTimer;   // If you climb jump and then do a sideways input within this timer, switch to wall jump
        private float climbNoMoveTimer;
        private int lastClimbMove;

        private float maxFall;
        private bool onGround;
        private bool wasOnGround;
        private int moveX;
        private int forceMoveX;
        private float forceMoveXTimer;

        private float jumpGraceTimer;
        private bool AutoJump;
        private bool fastJump;
        public float AutoJumpTimer;
        private float varJumpSpeed;
        private float varJumpTimer;
        private float dashAttackTimer;
        private float highestAirY;
        private float wallSlideTimer = WallSlideTime;
        public float Stamina = ClimbMaxStamina;
        #endregion

        public readonly Rect normalHitbox = new Rect(0, 5.5f, 8, 11);

        public static readonly Color NormalHairColor = Util.HexToColor("AC3232");

        private static Chooser<string> idleColdOptions = new Chooser<string>().Add("idleA", 5f).Add("idleB", 3f).Add("idleC", 1f);

        [HideInInspector]
        public IntroTypes IntroType { get; set; }           //出生方式
        [HideInInspector]
        public PlayerSprite Sprite;
        [HideInInspector]
        public PlayerHair playerHair;

        private Level level = new Level();
        private Collision onCollideH;
        private Collision onCollideV;
        public StateMachine StateMachine;

        public void Awake()
        {
            mCollider = this.gameObject.AddComponent<BoxCollider2D>();
            mCollider.offset = new Vector2(normalHitbox.x, normalHitbox.y);
            mCollider.size = new Vector2(normalHitbox.width, normalHitbox.height);

            onCollideH = OnCollideH;
            onCollideV = OnCollideV;

            PLATFORM_MASK = LayerMask.GetMask("Platform");
        }

        public void Load(Vector2 position, PlayerSpriteMode spriteMode)
        {
            PlayerSprite playerSpritePrefab = Resources.Load<PlayerSprite>("PlayerSprite");
            this.Sprite = Instantiate(playerSpritePrefab);
            this.Sprite.Load(spriteMode);
            this.Sprite.transform.SetParent(this.transform, false);

            PlayerHair playerHairPrefab = Resources.Load<PlayerHair>("PlayerHair");
            this.playerHair = Instantiate(playerHairPrefab);
            this.playerHair.BindPlayerSprite(Sprite);
            this.playerHair.transform.SetParent(this.transform, false);

            StateMachine = new StateMachine(23);
            StateMachine.SetCallbacks(StNormal, NormalUpdate, null, NormalBegin, NormalEnd);
            StateMachine.SetCallbacks(StClimb, ClimbUpdate, null, ClimbBegin, ClimbEnd);
            StateMachine.State = StNormal;

            Facing = Facings.Right;
        }

        public void Update()
        {
            //更新OnGround状态
            if (StateMachine.State == StDreamDash)
                onGround = OnSafeGround = false;
            else if (Speed.y >= 0)
            {
                //Platform first = null;
                Collider2D first = ColliderUtil.OverlapBox(this.mCollider, Vector2.down, 1, 0, PLATFORM_MASK);
                //Platform first = CollideFirst<Solid>(this.transform.position + Vector2.UnitY);
                //if (first == null)
                //    first = CollideFirstOutside<JumpThru>(Position + Vector2.UnitY);

                if (first != null)
                {
                    onGround = true;
                    //OnSafeGround = first.Safe;
                }
                else
                    onGround = OnSafeGround = false;
            }
            else
            {
                onGround = OnSafeGround = false;
            }
            //Highest Air Y
            if (onGround)
                highestAirY = this.transform.position.y;
            else
                highestAirY = Math.Min(this.transform.position.y, highestAirY);

            //更新参数
            if (onGround)
            {
                //dreamJump = false;
                jumpGraceTimer = JumpGraceTime;
            }
            else if (jumpGraceTimer > 0)
            {
                jumpGraceTimer -= Time.deltaTime;
            }

            //Var Jump
            if (varJumpTimer > 0)
                varJumpTimer -= Time.deltaTime;

            //Auto Jump
            if (AutoJumpTimer > 0)
            {
                if (AutoJump)
                {
                    AutoJumpTimer -= Time.deltaTime;
                    if (AutoJumpTimer <= 0)
                        AutoJump = false;
                }
                else
                    AutoJumpTimer = 0;
            }
            //处理输入
            //Force Move X
            if (forceMoveXTimer > 0)
            {
                forceMoveXTimer -= Time.deltaTime;
                moveX = forceMoveX;
            }
            else
            {
                moveX = InputManager.MoveX.Value;
                //climbHopSolid = null;
            }

            if (moveX != 0)
            {
                var to = (Facings)moveX;
                if (to != Facing && Ducking)
                    Sprite.Scale = new Vector2(0.8f, 1.2f);
                Facing = to;
            }
            //更新状态
            this.StateMachine.Update();

            //处理位移
            if (StateMachine.State != StDreamDash && StateMachine.State != StAttract)
                MoveH(Speed.x * Time.deltaTime, onCollideH, (Solid)null);
            if (StateMachine.State != StDreamDash && StateMachine.State != StAttract)
                MoveV(Speed.y * Time.deltaTime, onCollideV, (Solid)null);

            UpdateSprite();
            UpdateHair(true);
        }

        private void UpdateSprite()
        {
            Sprite.Scale.x = Mathf.MoveTowards(Sprite.Scale.x, 1f, 1.75f * Time.deltaTime);
            Sprite.Scale.y = Mathf.MoveTowards(Sprite.Scale.y, 1f, 1.75f * Time.deltaTime);

            Sprite.transform.localScale = Sprite.Scale;
            if (onGround)
            {
                //fastJump = false;
                //if (Holding == null && moveX != 0 && CollideCheck<Solid>(Position + Vector2.UnitX * moveX))
                //{
                //    Sprite.Play("push");
                //}
                //else if (Math.Abs(Speed.x) <= RunAccel / 40f && moveX == 0)
                //{
                //    //if (Holding != null)
                //    //{
                //    //    Sprite.Play(PlayerSprite.IdleCarry);
                //    //}
                //    //else if (!Scene.CollideCheck<Solid>(Position + new Vector2((int)Facing * 1, 2)) && !Scene.CollideCheck<Solid>(Position + new Vector2((int)Facing * 4, 2)) && !CollideCheck<JumpThru>(Position + new Vector2((int)Facing * 4, 2)))
                //    //{
                //    //    Sprite.Play(PlayerSprite.FrontEdge);
                //    //}
                //    //else if (!Scene.CollideCheck<Solid>(Position + new Vector2(-(int)Facing * 1, 2)) && !Scene.CollideCheck<Solid>(Position + new Vector2(-(int)Facing * 4, 2)) && !CollideCheck<JumpThru>(Position + new Vector2(-(int)Facing * 4, 2)))
                //    //{
                //    //    Sprite.Play("edgeBack");
                //    //}
                //    //else if (Input.MoveY.Value == -1)
                //    //{
                //    //    if (Sprite.LastAnimationID != PlayerSprite.LookUp)
                //    //        Sprite.Play(PlayerSprite.LookUp);
                //    //}
                //    //else
                //    {
                //        if (Sprite.CurrentAnimationID != null && !Sprite.CurrentAnimationID.Contains("idle"))
                //            Sprite.Play(PlayerSprite.Idle);
                //    }
                //}
                //else if (Holding != null)
                //{
                //    Sprite.Play(PlayerSprite.RunCarry);
                //}
                //else if (Math.Sign(Speed.X) == -moveX && moveX != 0)
                //{
                //    if (Math.Abs(Speed.X) > MaxRun)
                //        Sprite.Play(PlayerSprite.Skid);
                //    else if (Sprite.CurrentAnimationID != PlayerSprite.Skid)
                //        Sprite.Play(PlayerSprite.Flip);
                //}
                //else if (windDirection.X != 0 && windTimeout > 0f && (int)Facing == -Math.Sign(windDirection.X))
                //{
                //    Sprite.Play(PlayerSprite.RunWind);
                //}
                //else if (!Sprite.Running)
                //{
                //    if (Math.Abs(Speed.X) < MaxRun * .5f)
                //        Sprite.Play(PlayerSprite.RunSlow);
                //    else
                //        Sprite.Play(PlayerSprite.RunFast);
                //}
                if (Sprite.CurrentAnimationID != null && !Sprite.CurrentAnimationID.Contains("idle"))
                {
                    Sprite.Play(PlayerSprite.Idle);
                }
            }
            else if (Speed.y < 0)
            {
                if (Holding != null)
                {
                    Sprite.Play(PlayerSprite.JumpCarry);
                }
                else if (fastJump || Math.Abs(Speed.x) > MaxRun)
                {
                    fastJump = true;
                    Sprite.Play(PlayerSprite.JumpFast);
                }
                else
                    Sprite.Play(PlayerSprite.JumpSlow);
            }
            else
            {
                if (Holding != null)
                {
                    Sprite.Play(PlayerSprite.FallCarry);
                }
                else if (fastJump || Speed.y >= MaxFall) //|| level.InSpace)
                {
                    fastJump = true;
                    if (Sprite.LastAnimationID != PlayerSprite.FallFast)
                        Sprite.Play(PlayerSprite.FallFast);
                }
                else
                    Sprite.Play(PlayerSprite.FallSlow);
            }
        }

        private void UpdateHair(bool applyGravity)
        {

        }

#region NormalState

        private int NormalUpdate()
        {
            if (Holding == null)
            {
                if (InputManager.Grab.Check && !IsTired && !Ducking)
                {
                    //Grabbing Holdables
                    //foreach (Holdable hold in Scene.Tracker.GetComponents<Holdable>())
                    //    if (hold.Check(this) && Pickup(hold))
                    //        return StPickup;

                    //Climbing
                    if (Speed.y >= 0 && Math.Sign(Speed.x) != -(int)Facing)
                    {
                        if (ClimbCheck((int)Facing))
                        {
                            Ducking = false;
                            return StClimb;
                        }

                        //if (InputManager.MoveY < 1 && level.Wind.Y <= 0)
                        //{
                        //    for (int i = 1; i <= ClimbUpCheckDist; i++)
                        //    {
                        //        if (!CollideCheck<Solid>(Position + Vector2.UnitY * -i) && ClimbCheck((int)Facing, -i))
                        //        {
                        //            MoveVExact(-i);
                        //            Ducking = false;
                        //            return StClimb;
                        //        }
                        //    }
                        //}
                    }
                }

            }
            if (Ducking && onGround)
            {
                Speed.x = Mathf.MoveTowards(Speed.x, 0, DuckFriction * Time.deltaTime);
            }
            else
            {
                float mult = onGround ? 1 : AirMult;
                if (onGround && level.CoreMode == Session.CoreModes.Cold)
                    mult *= .3f;

                float max = Holding == null ? MaxRun : HoldingMaxRun;
                //if (level.InSpace)
                //    max *= SpacePhysicsMult;
                if (Math.Abs(Speed.x) > max && Math.Sign(Speed.x) == moveX)
                    Speed.x = Mathf.MoveTowards(Speed.x, max * moveX, RunReduce * mult * Time.deltaTime);  //Reduce back from beyond the max speed
                else
                    Speed.x = Mathf.MoveTowards(Speed.x, max * moveX, RunAccel * mult * Time.deltaTime);   //Approach the max speed
            }

            //Calculate current max fall speed
            {
                float mf = MaxFall;
                float fmf = FastMaxFall;

                //if (level.InSpace)
                //{
                //    mf *= SpacePhysicsMult;
                //    fmf *= SpacePhysicsMult;
                //}

                //Fast Fall
                if (InputManager.MoveY == 1 && Speed.y >= mf)
                {
                    maxFall = Mathf.MoveTowards(maxFall, fmf, FastMaxAccel * Time.deltaTime);

                    float half = mf + (fmf - mf) * .5f;
                    if (Speed.y >= half)
                    {
                        float spriteLerp = Math.Min(1f, (Speed.y - half) / (fmf - half));
                        Sprite.Scale.y = Mathf.Lerp(1f, .5f, spriteLerp);
                        Sprite.Scale.y = Mathf.Lerp(1f, 1.5f, spriteLerp);
                        Sprite.transform.localScale = Sprite.Scale;
                    }
                }
                else
                    maxFall = Mathf.MoveTowards(maxFall, mf, FastMaxAccel * Time.deltaTime);
            }

            if (!onGround)
            {
                float max = maxFall;

                //Wall Slide
                //if ((moveX == (int)Facing || (moveX == 0 && Input.Grab.Check)) && Input.MoveY.Value != 1)
                //{
                //    if (Speed.Y >= 0 && wallSlideTimer > 0 && Holding == null && ClimbBoundsCheck((int)Facing) && CollideCheck<Solid>(Position + Vector2.UnitX * (int)Facing) && CanUnDuck)
                //    {
                //        Ducking = false;
                //        wallSlideDir = (int)Facing;
                //    }

                //    if (wallSlideDir != 0)
                //    {
                //        if (wallSlideTimer > WallSlideTime * .5f && ClimbBlocker.Check(level, this, Position + Vector2.UnitX * wallSlideDir))
                //            wallSlideTimer = WallSlideTime * .5f;

                //        max = MathHelper.Lerp(MaxFall, WallSlideStartMax, wallSlideTimer / WallSlideTime);
                //        if (wallSlideTimer / WallSlideTime > .65f)
                //            CreateWallSlideParticles(wallSlideDir);
                //    }
                //}

                float mult = (Math.Abs(Speed.y) < HalfGravThreshold && (InputManager.Jump.Check || AutoJump)) ? .5f : 1f;

                //if (level.InSpace)
                //    mult *= SpacePhysicsMult;

                Speed.y = Mathf.MoveTowards(Speed.y, max, Gravity * mult * Time.deltaTime);
            }
            //Variable Jumping
            if (varJumpTimer > 0)
            {
                if (AutoJump || InputManager.Jump.Check)
                    Speed.y = Math.Min(Speed.y, varJumpSpeed);
                else
                    varJumpTimer = 0;
            }

            //Jumping
            if (InputManager.Jump.Pressed)
            {
               // Water water = null;
                if (jumpGraceTimer > 0)
                {
                    Jump();
                }
                //else if (CanUnDuck)
                //{
                //    bool canUnduck = CanUnDuck;
                //    if (canUnduck && WallJumpCheck(1))
                //    {
                //        if (Facing == Facings.Right && Input.Grab.Check && Stamina > 0 && Holding == null && !ClimbBlocker.Check(Scene, this, Position + Vector2.UnitX * WallJumpCheckDist))
                //            ClimbJump();
                //        else if (DashAttacking && DashDir.X == 0 && DashDir.Y == -1)
                //            SuperWallJump(-1);
                //        else
                //            WallJump(-1);
                //    }
                //    else if (canUnduck && WallJumpCheck(-1))
                //    {
                //        if (Facing == Facings.Left && Input.Grab.Check && Stamina > 0 && Holding == null && !ClimbBlocker.Check(Scene, this, Position + Vector2.UnitX * -WallJumpCheckDist))
                //            ClimbJump();
                //        else if (DashAttacking && DashDir.X == 0 && DashDir.Y == -1)
                //            SuperWallJump(1);
                //        else
                //            WallJump(1);
                //    }
                //    else if ((water = CollideFirst<Water>(Position + Vector2.UnitY * 2)) != null)
                //    {
                //        Jump();
                //        water.TopSurface.DoRipple(Position, 1);
                //    }
                //}
            }

            return StNormal;
        }

        private void NormalBegin()
        {
            maxFall = MaxFall;
        }

        private void NormalEnd()
        {
            wallBoostTimer = 0;
            wallSpeedRetentionTimer = 0;
            //hopWaitX = 0;
        }

        public bool ClimbBoundsCheck(int dir)
        {
            return Left + dir * ClimbCheckDist >= level.Bounds.Left && Right + dir * ClimbCheckDist < level.Bounds.Right;
        }

        public bool ClimbCheck(int dir, int yAdd = 0)
        {
            bool b1 = ClimbBoundsCheck(dir);
            Vector2 position = (Vector2)this.transform.position;
            //bool b2 = !ClimbBlocker.Check(Scene, this, position + Vector2.down * yAdd + Vector2.right * ClimbCheckDist * (int)Facing);
            bool b2 = true;
            bool b3 = ColliderUtil.OverlapBox(this.mCollider, position + new Vector2(dir * ClimbCheckDist, yAdd), 0, PLATFORM_MASK);
            return b1 && b2 && b3;
        }
        #endregion




        #region Jumps 'n' Stuff
        public bool OnSafeGround
        {
            get; private set;
        }

        public void Jump(bool particles = true, bool playSfx = true)
        {
            InputManager.Jump.ConsumeBuffer();
            jumpGraceTimer = 0;
            varJumpTimer = VarJumpTime;
            AutoJump = false;
            //dashAttackTimer = 0;
            //wallSlideTimer = WallSlideTime;
            ///wallBoostTimer = 0;

            Speed.x += JumpHBoost * moveX;
            Speed.y = JumpSpeed;
            Speed += LiftBoost;
            varJumpSpeed = Speed.y;

            //LaunchedBoostCheck();

            //if (playSfx)
            //{
            //    if (launched)
            //        Play(Sfxs.char_mad_jump_assisted);

            //    if (dreamJump)
            //        Play(Sfxs.char_mad_jump_dreamblock);
            //    else
            //        Play(Sfxs.char_mad_jump);
            //}

            Sprite.Scale = new Vector2(.6f, 1.4f);
            Sprite.transform.localScale = Sprite.Scale;
            //if (particles)
            //    Dust.Burst(BottomCenter, Calc.Up, 4);

            //SaveData.Instance.TotalJumps++;
        }

        private void OnCollideH(CollisionData data)
        {
            Speed.x = 0;
        }

        private void OnCollideV(CollisionData data)
        {
            Speed.y = 0;
        }

        private Vector2 LiftBoost
        {
            get
            {
                Vector2 val = LiftSpeed;

                if (Math.Abs(val.x) > LiftXCap)
                    val.x = LiftXCap * Math.Sign(val.y);

                if (val.y > 0)
                    val.y = 0;
                else if (val.y < LiftYCap)
                    val.y = LiftYCap;

                return val;
            }
        }

        #endregion

        #region
        private int ClimbUpdate()
        {
            return StClimb;
        }

        private void ClimbBegin()
        {
            AutoJump = false;
            Speed.x = 0;
            Speed.y *= ClimbGrabYMult;
            wallSlideTimer = WallSlideTime;
            climbNoMoveTimer = ClimbNoMoveTime;
            wallBoostTimer = 0;
            lastClimbMove = 0;

            //Input.Rumble(RumbleStrength.Medium, RumbleLength.Short);

            //for (int i = 0; i < ClimbCheckDist; i++)
            //    if (!CollideCheck<Solid>(Position + Vector2.UnitX * (int)Facing))
            //        Position += Vector2.UnitX * (int)Facing;
            //    else
            //        break;

            //// tell the thing we grabbed it
            //var platform = SurfaceIndex.GetPlatformByPriority(CollideAll<Solid>(Position + Vector2.UnitX * (int)Facing, temp));
            //if (platform != null)
            //    Play(Sfxs.char_mad_grab, SurfaceIndex.Param, platform.GetWallSoundIndex(this, (int)Facing));
        }

        private void ClimbEnd()
        {
            //if (conveyorLoopSfx != null)
            //{
            //    conveyorLoopSfx.setParameterValue("end", 1);
            //    conveyorLoopSfx.release();
            //    conveyorLoopSfx = null;
            //}
            wallSpeedRetentionTimer = 0;
            //if (sweatSprite != null && sweatSprite.CurrentAnimationID != "jump")
            //    sweatSprite.Play("idle");
        }
        #endregion
        public Holdable Holding { get; set; }

        public bool Ducking
        {
            get
            {
                //return this.Collider == this.duckHitbox || this.Collider == this.duckHurtbox;
                return false;
            }
            set
            {
                //if (value)
                //{
                //    this.Collider = (Collider)this.duckHitbox;
                //    this.hurtbox = this.duckHurtbox;
                //}
                //else
                //{
                //    this.Collider = (Collider)this.normalHitbox;
                //    this.hurtbox = this.normalHurtbox;
                //}
            }
        }

        private bool IsTired
        {
            get
            {
                return CheckStamina < ClimbTiredThreshold;
            }
        }

        private float CheckStamina
        {
            get
            {
                if (wallBoostTimer > 0)
                    return Stamina + ClimbJumpCost;
                else
                    return Stamina;
            }
        }

    }
}
