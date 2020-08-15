using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace myd.celeste.demo
{
    public class Player : Actor
    {
        

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

        private const float DodgeSlideSpeedMult = 1.2f;
        private const float DuckSuperJumpXMult = 1.25f;
        private const float DuckSuperJumpYMult = .5f;

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

        private const float DashSpeed = 240f;
        private const float EndDashSpeed = 160f;
        private const float EndDashUpMult = .75f;
        private const float DashTime = .15f;
        private const float DashCooldown = .2f;
        private const float DashRefillCooldown = .1f;
        private const int DashHJumpThruNudge = 6;
        private const int DashCornerCorrection = 4;
        private const int DashVFloorSnapDist = 3;
        private const float DashAttackTime = .3f;

        private const int WallJumpCheckDist = 3;
        private const float WallJumpForceTime = .16f;
        private const float WallJumpHSpeed = MaxRun + JumpHBoost;

        private const float WallSlideStartMax = 20f;
        private const float WallSlideTime = 1.2f;

        private const float BounceVarJumpTime = .2f;
        private const float BounceSpeed = -140f;
        private const float SuperBounceVarJumpTime = .2f;
        private const float SuperBounceSpeed = -185f;

        private const float SuperJumpSpeed = JumpSpeed;
        private const float SuperJumpH = 260f;
        private const float SuperWallJumpSpeed = -160f;
        private const float SuperWallJumpVarTime = .25f;
        private const float SuperWallJumpForceTime = .2f;
        private const float SuperWallJumpH = MaxRun + JumpHBoost * 2;


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
        public int Dashes;
        private float jumpGraceTimer;
        private bool AutoJump;
        private bool fastJump;
        public float AutoJumpTimer;
        private float varJumpSpeed;
        private float varJumpTimer;
        private int forceMoveX;
        private float forceMoveXTimer;
        private int hopWaitX;   // If you climb hop onto a moving solid, snap to beside it until you get above it
        private float hopWaitXSpeed;
        private Vector2 lastAim;
        private Collider2D climbHopSolid;
        private Vector2 climbHopSolidPosition;
        private float dashAttackTimer;
        private float highestAirY;
        private float wallSlideTimer = WallSlideTime;
        private int wallSlideDir;
        public float Stamina = ClimbMaxStamina;
        private float playFootstepOnLand;

        private bool dashStartedOnGround;
        private bool calledDashEvents;
        private float dashCooldownTimer;
        private float dashRefillCooldownTimer;
        private bool launched;
        private float launchedTimer;
        private float dashTrailTimer;
        private Vector2 beforeDashSpeed;
        public Vector2 DashDir;
        private int lastDashes;
        public bool StartedDashing
        {
            get; private set;
        }

        public Vector2? OverrideDashDirection;
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
            StateMachine.SetCallbacks(StDash, DashUpdate, DashCoroutine, DashBegin, DashEnd);
            StateMachine.State = StNormal;

            Facing = Facings.Right;
            lastDashes = Dashes = MaxDashes;
            lastAim = Vector2.right;
        }

        public void Update()
        {
            Sprite.Scale.x = Mathf.Abs(Sprite.Scale.x);
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


            playFootstepOnLand -= Time.deltaTime;

            //Highest Air Y
            if (onGround)
                highestAirY = this.transform.position.y;
            else
                highestAirY = Math.Min(this.transform.position.y, highestAirY);

            //Wall Slide
            if (wallSlideDir != 0)
            {
                Debug.Log("wallSlideDir:"+ wallSlideDir);
                wallSlideTimer = Math.Max(wallSlideTimer - Time.deltaTime, 0);
                wallSlideDir = 0;
            }

            //After Dash
            if (onGround && StateMachine.State != StClimb)
            {
                AutoJump = false;
                Stamina = ClimbMaxStamina;
                wallSlideTimer = WallSlideTime;
            }
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

            {
                if (dashCooldownTimer > 0)
                    dashCooldownTimer -= Time.deltaTime;
                if (dashRefillCooldownTimer > 0)
                    dashRefillCooldownTimer -= Time.deltaTime;
                else if (true) //(!Inventory.NoRefills)
                {
                    if (StateMachine.State == StSwim)
                        RefillDash();
                    else if (onGround)
                        //if (CollideCheck<Solid, NegaBlock>(Position + Vector2.UnitY) || CollideCheckOutside<JumpThru>(Position + Vector2.UnitY))
                        //    if (!CollideCheck<Spikes>(Position) || (SaveData.Instance.AssistMode && SaveData.Instance.Assists.Invincible))
                                RefillDash();
                }
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
                climbHopSolid = null;
            }

            //Climb Hop Solid Movement(到达墙顶时,一个小跳,恢复落地.会向Facing方向前进一小段)
            if (climbHopSolid != null)//&& !climbHopSolid.Collidable)
                climbHopSolid = null;
            else if (climbHopSolid != null && ((Vector2)climbHopSolid.transform.position != climbHopSolidPosition))
            {
                var move = (Vector2)climbHopSolid.transform.position - climbHopSolidPosition;
                climbHopSolidPosition = climbHopSolid.transform.position;
                MoveHExact((int)move.x);
                MoveVExact((int)move.y);
            }

            if (moveX != 0)
            {
                var to = (Facings)moveX;
                if (to != Facing && Ducking)
                    Sprite.Scale = new Vector2(0.8f, 1.2f);
                Facing = to;
            }

            //Aiming
            lastAim = InputManager.GetAimVector(Facing);

            //Hop Wait X
            if (hopWaitX != 0)
            {
                if (Math.Sign(Speed.x) == -hopWaitX || Speed.y > 0)
                {
                    Debug.Log(11);
                    hopWaitX = 0;
                }
                else if (!ColliderUtil.CollideCheck(mCollider, (Vector2)this.transform.position + Vector2.right * hopWaitX, PLATFORM_MASK))
                {
                    Speed.x = hopWaitXSpeed;
                    hopWaitX = 0;
                    Debug.Log("hopWaitXSpeed:" + hopWaitXSpeed + "--Speed.x:" + Speed.x);
                }
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

            Render();
        }

        private void Render()
        {
            Sprite.Scale.x *= (int)Facing;
            this.playerHair.Facing = Facing;
            Sprite.transform.localScale = Sprite.Scale;
        }

        private void UpdateSprite()
        {
            Sprite.Scale.x = Mathf.MoveTowards(Sprite.Scale.x, 1f, 1.75f * Time.deltaTime);
            Sprite.Scale.y = Mathf.MoveTowards(Sprite.Scale.y, 1f, 1.75f * Time.deltaTime);

            if (StateMachine.State == StClimb)
            {
                if (lastClimbMove < 0)
                    Sprite.Play(PlayerSprite.ClimbUp);
                else if (lastClimbMove > 0)
                    Sprite.Play(PlayerSprite.WallSlide);
                else if (InputManager.MoveX == -(int)Facing)
                {
                    if (Sprite.CurrentAnimationID != PlayerSprite.ClimbLookBack)
                        Sprite.Play(PlayerSprite.ClimbLookBackStart);
                }
                else
                    Sprite.Play(PlayerSprite.WallSlide);
            }
            else if (onGround)
            {
                fastJump = false;
                if (Holding == null && moveX != 0 && ColliderUtil.CollideCheck(mCollider, (Vector2)this.transform.position + Vector2.right * moveX, PLATFORM_MASK))
                {
                    Sprite.Play("push");
                }
                else if (Math.Abs(Speed.x) <= RunAccel / 40f && moveX == 0)
                {
                    if (Holding != null)
                    {
                        Sprite.Play(PlayerSprite.IdleCarry);
                    }
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
                    else
                    {
                        if (Sprite.CurrentAnimationID != null && !Sprite.CurrentAnimationID.Contains("idle"))
                            Sprite.Play(PlayerSprite.Idle);
                    }
                }
                else if (Holding != null)
                {
                    Sprite.Play(PlayerSprite.RunCarry);
                }
                else if (!Sprite.Running)
                {
                    if (Math.Abs(Speed.x) < MaxRun * .5f)
                        Sprite.Play(PlayerSprite.RunSlow);
                    else
                        Sprite.Play(PlayerSprite.RunFast);
                }
            } 
            // wall sliding
            else if (wallSlideDir != 0 && Holding == null)
            {
                Debug.Log("Sprite.Play(PlayerSprite.WallSlide)");
                Sprite.Play(PlayerSprite.WallSlide);
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
            this.playerHair.OnUpdate();
        }

        #region NormalState

        private int NormalUpdate()
        {
            //Use Lift Boost if walked off platform
            if (LiftBoost.y < 0 && wasOnGround && !onGround && Speed.y >= 0)
                Speed.y = LiftBoost.y;

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
                            Debug.Log("Enter StClimb");
                            Ducking = false;
                            return StClimb;
                        }

                        if (InputManager.MoveY < 1 )//&& level.Wind.Y <= 0)
                        {
                            for (int i = 1; i <= ClimbUpCheckDist; i++)
                            {
                                if (ColliderUtil.CollideCheck(mCollider, (Vector2)this.transform.position + Vector2.down * -i, PLATFORM_MASK) && ClimbCheck((int)Facing, -i))
                                {
                                    MoveVExact(-i);
                                    Ducking = false;
                                    return StClimb;
                                }
                            }
                        }
                    }
                }

                //Dashing
                if (CanDash)
                {
                    Speed += LiftBoost;
                    return StartDash();
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
                    }
                }
                else
                    maxFall = Mathf.MoveTowards(maxFall, mf, FastMaxAccel * Time.deltaTime);
            }

            if (!onGround)
            {
                float max = maxFall;

                //Wall Slide
                if ((moveX == (int)Facing || (moveX == 0 && InputManager.Grab.Check)) && InputManager.MoveY.Value != 1)
                {
                    if (Speed.y >= 0 && wallSlideTimer > 0 && Holding == null && ClimbBoundsCheck((int)Facing) && ColliderUtil.CollideCheck(mCollider, (Vector2)this.transform.position + Vector2.right * (int)Facing, PLATFORM_MASK) && CanUnDuck)
                    {
                        Ducking = false;
                        wallSlideDir = (int)Facing;
                    }

                    if (wallSlideDir != 0)
                    {
                        //if (wallSlideTimer > WallSlideTime * .5f && ClimbBlocker.Check(level, this, Position + Vector2.UnitX * wallSlideDir))
                        //    wallSlideTimer = WallSlideTime * .5f;

                        max = Mathf.Lerp(MaxFall, WallSlideStartMax, wallSlideTimer / WallSlideTime);
                        //if (wallSlideTimer / WallSlideTime > .65f)
                        //    CreateWallSlideParticles(wallSlideDir);
                    }
                }

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
            Debug.Log("Enter[Normal]");
        }

        private void NormalEnd()
        {
            wallBoostTimer = 0;
            wallSpeedRetentionTimer = 0;
            hopWaitX = 0;
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
            bool result = b1 && b2 && b3;
            return result;
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
            wallSlideTimer = WallSlideTime;
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
            //if (particles)
            //    Dust.Burst(BottomCenter, Calc.Up, 4);

            //SaveData.Instance.TotalJumps++;
        }

        private void WallJump(int dir)
        {
            Ducking = false;
            InputManager.Jump.ConsumeBuffer();
            jumpGraceTimer = 0;
            varJumpTimer = VarJumpTime;
            AutoJump = false;
            dashAttackTimer = 0;
            wallSlideTimer = WallSlideTime;
            wallBoostTimer = 0;
            if (moveX != 0)
            {
                forceMoveX = dir;
                forceMoveXTimer = WallJumpForceTime;
            }

            //Get lift of wall jumped off of
            if (LiftSpeed == Vector2.zero)
            {
                Collider2D wall = ColliderUtil.CollideFirst(this.mCollider, Vector2.right, WallJumpCheckDist, 0f, PLATFORM_MASK);
                //if (wall != null)
                    //LiftSpeed = wall.LiftSpeed;
            }

            Speed.x = WallJumpHSpeed * dir;
            Speed.y = JumpSpeed;
            Speed += LiftBoost;
            varJumpSpeed = Speed.y;

            //LaunchedBoostCheck();

            // wall-sound?
            //var pushOff = SurfaceIndex.GetPlatformByPriority(CollideAll<Platform>(Position - Vector2.UnitX * dir * 4, temp));
            //if (pushOff != null)
                //Play(Sfxs.char_mad_land, SurfaceIndex.Param, pushOff.GetWallSoundIndex(this, -dir));

            // jump sfx
            //Play(dir < 0 ? Sfxs.char_mad_jump_wall_right : Sfxs.char_mad_jump_wall_left);
            Sprite.Scale = new Vector2(.6f, 1.4f);

            //if (dir == -1)
            //    Dust.Burst(Center + Vector2.UnitX * 2, Calc.UpLeft, 4);
            //else
            //    Dust.Burst(Center + Vector2.UnitX * -2, Calc.UpRight, 4);

            SaveData.Instance.TotalWallJumps++;
        }

        private void ClimbJump()
        {
            if (!onGround)
            {
                Stamina -= ClimbJumpCost;

                //sweatSprite.Play("jump", true);
                //Input.Rumble(RumbleStrength.Light, RumbleLength.Medium);
            }

            //dreamJump = false;
            Jump(false, false);

            if (moveX == 0)
            {
                wallBoostDir = -(int)Facing;
                wallBoostTimer = ClimbJumpBoostTime;
            }

            if (Facing == Facings.Right)
            {
                //Play(Sfxs.char_mad_jump_climb_right);
                //Dust.Burst(Center + Vector2.UnitX * 2, Calc.UpLeft, 4);
            }
            else
            {
                //Play(Sfxs.char_mad_jump_climb_left);
                //Dust.Burst(Center + Vector2.UnitX * -2, Calc.UpRight, 4);
            }
        }

        private void SuperWallJump(int dir)
        {
            Ducking = false;
            InputManager.Jump.ConsumeBuffer();
            jumpGraceTimer = 0;
            varJumpTimer = SuperWallJumpVarTime;
            AutoJump = false;
            dashAttackTimer = 0;
            wallSlideTimer = WallSlideTime;
            wallBoostTimer = 0;

            Speed.x = SuperWallJumpH * dir;
            Speed.y = SuperWallJumpSpeed;
            Speed += LiftBoost;
            varJumpSpeed = Speed.y;
            launched = true;

            //Play(dir < 0 ? Sfxs.char_mad_jump_wall_right : Sfxs.char_mad_jump_wall_left);
            //Play(Sfxs.char_mad_jump_superwall);
            Sprite.Scale = new Vector2(.6f, 1.4f);

            //if (dir == -1)
            //    Dust.Burst(Center + Vector2.UnitX * 2, Calc.UpLeft, 4);
            //else
            //    Dust.Burst(Center + Vector2.UnitX * -2, Calc.UpRight, 4);

            SaveData.Instance.TotalWallJumps++;
        }

        private bool WallJumpCheck(int dir)
        {
            return ClimbBoundsCheck(dir) && CollideCheck((Vector2)this.transform.position + Vector2.right * dir * WallJumpCheckDist);
        }

        private void OnCollideH(CollisionData data)
        {
            Debug.Log("OnCollideH:"+data);
            Speed.x = 0;
        }

        private void OnCollideV(CollisionData data)
        {
            Speed.y = 0;
            if (StateMachine.State != StClimb)
            {
                float squish = Math.Min(Speed.y / FastMaxFall, 1);
                Sprite.Scale.x = Mathf.Lerp(1, 1.6f, squish);
                Sprite.Scale.y = Mathf.Lerp(1, .4f, squish);

                //if (Speed.Y >= MaxFall / 2)
                //    Dust.Burst(Position, Calc.Angle(new Vector2(0, -1)), 8);

                //if (highestAirY < Y - 50 && Speed.Y >= MaxFall && Math.Abs(Speed.X) >= MaxRun)
                //    Sprite.Play(PlayerSprite.RunStumble);

                //Input.Rumble(RumbleStrength.Light, RumbleLength.Short);

                //// landed SFX
                //var platform = SurfaceIndex.GetPlatformByPriority(CollideAll<Platform>(Position + new Vector2(0, 1), temp));
                //if (platform != null)
                //{
                //    var surface = platform.GetLandSoundIndex(this);
                //    if (surface >= 0)
                //        Play(playFootstepOnLand > 0f ? Sfxs.char_mad_footstep : Sfxs.char_mad_land, SurfaceIndex.Param, surface);
                //    if (platform is DreamBlock)
                //        (platform as DreamBlock).FootstepRipple(Position);
                //}
                playFootstepOnLand = 0f;
            }
        }

        public int MaxDashes
        {
            get
            {
                //if (SaveData.Instance.AssistMode && SaveData.Instance.Assists.DashMode != Assists.DashModes.Normal && !level.InCutscene)
                    return 2;
                //else
                //    return Inventory.Dashes;
            }
        }

        public bool RefillDash()
        {
            if (Dashes < MaxDashes)
            {
                Dashes = MaxDashes;
                return true;
            }
            else
                return false;
        }

        public bool UseRefill()
        {
            if (Dashes < MaxDashes || Stamina < ClimbTiredThreshold)
            {
                Dashes = MaxDashes;
                RefillStamina();
                return true;
            }
            else
                return false;
        }

        public void RefillStamina()
        {
            Stamina = ClimbMaxStamina;
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

        #region Climb
        private int ClimbUpdate()
        {
            climbNoMoveTimer -= Time.deltaTime;
            if (onGround)
                Stamina = ClimbMaxStamina;
            //Wall Jump
            if (InputManager.Jump.Pressed && (!Ducking || CanUnDuck))
            {
                if (moveX == -(int)Facing)
                    WallJump(-(int)Facing);
                else
                    ClimbJump();

                return StNormal;
            }

            //Dashing
            if (CanDash)
            {
                Debug.Log("CanDash");
                Speed += LiftBoost;
                return StartDash();
            }

            //Let go
            if (!InputManager.Grab.Check)
            {
                Speed += LiftBoost;
                //Play(Sfxs.char_mad_grab_letgo);
                return StNormal;
            }
            if (climbNoMoveTimer <= 0&&false) //&& booster != null)
            {
                //Wallbooster
                //wallBoosting = true;

                //if (conveyorLoopSfx == null)
                //    conveyorLoopSfx = Audio.Play(Sfxs.game_09_conveyor_activate, Position, "end", 0);
                //Audio.Position(conveyorLoopSfx, Position);

                //Speed.Y = Calc.Approach(Speed.Y, WallBoosterSpeed, WallBoosterAccel * Engine.DeltaTime);
                //LiftSpeed = Vector2.UnitY * Math.Max(Speed.Y, WallBoosterLiftSpeed);
                //Input.Rumble(RumbleStrength.Light, RumbleLength.Short);
            }
            else
            {
                //Climbing
                float target = 0;
                bool trySlip = false;
                if (climbNoMoveTimer <= 0)
                {
                    //if (ClimbBlocker.Check(Scene, this, Position + Vector2.UnitX * (int)Facing))
                    //    trySlip = true;
                    //else 
                    if (InputManager.MoveY.Value == -1)
                    {
                        //Climb向上
                        target = ClimbUpSpeed;
                        //Up Limit
                        if (ColliderUtil.CollideCheck(mCollider, (Vector2)this.transform.position - Vector2.down, PLATFORM_MASK))// || (ClimbHopBlockedCheck() && SlipCheck(-1)))
                        {
                            if (Speed.y < 0)
                                Speed.y = 0;
                            target = 0;
                            trySlip = true;
                        }
                        else if (SlipCheck())
                        {
                            //Hopping
                            Debug.Log("ClimbHop:"+this.transform.position);
                            ClimbHop();
                            return StNormal;
                        }
                    }
                    else if (InputManager.MoveY.Value == 1)
                    {
                        target = ClimbDownSpeed;

                        if (onGround)
                        {
                            if (Speed.y > 0)
                                Speed.y = 0;
                            target = 0;
                        }
                        else
                        {
                            //CreateWallSlideParticles((int)Facing);
                        }
                    }
                    else
                    {
                        trySlip = true;
                    }
                }
                else
                {
                    trySlip = true;
                }
                lastClimbMove = Math.Sign(target);

                //Slip down if hands above the ledge and no vertical input
                if (trySlip && SlipCheck())
                {
                    target = ClimbSlipSpeed;
                }

                //Set Speed
                Speed.y = Mathf.MoveTowards(Speed.y, target, ClimbAccel * Time.deltaTime);
            }
            
            ////Down Limit
            if (InputManager.MoveY.Value != 1 && Speed.y > 0 && !ColliderUtil.CollideCheck(mCollider, (Vector2)this.transform.position + new Vector2((int)Facing, 1), PLATFORM_MASK))
                Speed.y = 0;

            ////Stamina
            if (climbNoMoveTimer <= 0)
            {
                if (lastClimbMove == -1)
                {
                    Stamina -= ClimbUpCost * Time.deltaTime;

                    //if (Stamina <= ClimbTiredThreshold)
                    //    sweatSprite.Play("danger");
                    //else if (sweatSprite.CurrentAnimationID != "climbLoop")
                    //    sweatSprite.Play("climb");
                    //if (Scene.OnInterval(.2f))
                    //    Input.Rumble(RumbleStrength.Climb, RumbleLength.Short);
                }
                else
                {
                    if (lastClimbMove == 0)
                        Stamina -= ClimbStillCost * Time.deltaTime;

                    if (!onGround)
                    {
                        //PlaySweatEffectDangerOverride("still");
                        //if (Scene.OnInterval(.8f))
                        //    Input.Rumble(RumbleStrength.Climb, RumbleLength.Short);
                    }
                    else
                    {
                        //PlaySweatEffectDangerOverride("idle");
                    }
                }
            }
            else
            {
                //PlaySweatEffectDangerOverride("idle");
            }

            ////Too tired           
            //if (Stamina <= 0)
            //{
            //    Speed += LiftBoost;
            //    return StNormal;
            //}
            return StClimb;
        }

        private bool SlipCheck(float addY = 0)
        {
            Vector2 at;
            if (Facing == Facings.Right)
                at = TopRight + Vector2.down * (4 + addY);
            else
                at = TopLeft - Vector2.right + Vector2.down * (4 + addY);

            bool f1 = !ColliderUtil.CollideCheck(mCollider, at, PLATFORM_MASK);
            bool f2 = !ColliderUtil.CollideCheck(mCollider, at + Vector2.down * (-4 + addY), PLATFORM_MASK);
            return f1 && f2;
        }

        private void ClimbHop()
        {
            climbHopSolid = ColliderUtil.CollideFirst(mCollider, Vector2.right * (int)Facing, 1, 0, PLATFORM_MASK);
            playFootstepOnLand = 0.5f;

            if (climbHopSolid != null)
            {
                climbHopSolidPosition = climbHopSolid.transform.position;
                hopWaitX = (int)Facing;
                hopWaitXSpeed = (int)Facing * ClimbHopX;
            }
            else
            {
                hopWaitX = 0;
                Speed.x = (int)Facing * ClimbHopX;
            }

            Speed.y = Math.Min(Speed.y, ClimbHopY);
            forceMoveX = 0;
            forceMoveXTimer = ClimbHopForceTime;
            fastJump = false;
            //noWindTimer = ClimbHopNoWindTime;
            //Play(Sfxs.char_mad_climb_ledge);
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
            Debug.Log("Enter[Normal]");
            //Input.Rumble(RumbleStrength.Medium, RumbleLength.Short);

            //for (int i = 0; i < ClimbCheckDist; i++)
            //    if (!ColliderUtil.CollideCheck((Vector2)this.transform.position + Vector2.right * (int)Facing, PLATFORM_MASK))
            //        this.transform.position = this.transform.position + Vector3.right * (int)Facing;
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

        #region Dash State

        private bool wasDashB;

        public int StartDash()
        {
            wasDashB = Dashes == 2;
            Dashes = Math.Max(0, Dashes - 1);
            InputManager.Dash.ConsumeBuffer();
            return StDash;
        }

        public bool CanDash
        {
            get
            {
                return InputManager.Dash.Pressed && dashCooldownTimer <= 0 && Dashes > 0; //&&(TalkComponent.PlayerOver == null || !Input.Talk.Pressed);
            }
        }

        private void DashBegin()
        {
            Debug.Log("DashBegin");
            calledDashEvents = false;
            dashStartedOnGround = onGround;
            launched = false;

            if (Time.timeScale > 0.25f)
            {
                //冻帧
                //Celeste.Freeze(.05f);
            }
            dashCooldownTimer = DashCooldown;
            dashRefillCooldownTimer = DashRefillCooldown;
            StartedDashing = true;
            wallSlideTimer = WallSlideTime;
            dashTrailTimer = 0;

            //level.Displacement.AddBurst(Center, .4f, 8, 64, .5f, Ease.QuadOut, Ease.QuadOut);

            //Input.Rumble(RumbleStrength.Strong, RumbleLength.Medium);

            dashAttackTimer = DashAttackTime;
            beforeDashSpeed = Speed;
            Speed = Vector2.zero;
            DashDir = Vector2.zero;

            if (!onGround && Ducking && CanUnDuck)
                Ducking = false;
        }

        private void CallDashEvents()
        {
        }

        private void DashEnd()
        {
            Debug.Log("DashEnd");
            CallDashEvents();
        }

        private int DashUpdate()
        {
            Debug.Log("DashUpdate");
            StartedDashing = false;

            //Trail
            if (dashTrailTimer > 0)
            {
                dashTrailTimer -= Time.deltaTime;
                if (dashTrailTimer <= 0)
                {
                    //CreateTrail();
                }
            }

            //Grab Holdables
            if (Holding == null && InputManager.Grab.Check && !IsTired && CanUnDuck)
            {
                //Grabbing Holdables
                //foreach (Holdable hold in Scene.Tracker.GetComponents<Holdable>())
                //    if (hold.Check(this) && Pickup(hold))
                //        return StPickup;
            }

            if (DashDir.y == 0)
            {
                //JumpThru Correction
                //foreach (JumpThru jt in Scene.Tracker.GetEntities<JumpThru>())
                //    if (CollideCheck(jt) && Bottom - jt.Top <= DashHJumpThruNudge)
                //        MoveVExact((int)(jt.Top - Bottom));

                //Super Jump
                //if (CanUnDuck && InputManager.Jump.Pressed && jumpGraceTimer > 0)
                //{
                //    SuperJump();
                //    return StNormal;
                //}
            }

            if (DashDir.x == 0 && DashDir.y == -1)
            {
                if (InputManager.Jump.Pressed && CanUnDuck)
                {
                    if (WallJumpCheck(1))
                    {
                        SuperWallJump(-1);
                        return StNormal;
                    }
                    else if (WallJumpCheck(-1))
                    {
                        SuperWallJump(1);
                        return StNormal;
                    }
                }
            }
            else
            {
                if (InputManager.Jump.Pressed && CanUnDuck)
                {
                    if (WallJumpCheck(1))
                    {
                        WallJump(-1);
                        return StNormal;
                    }
                    else if (WallJumpCheck(-1))
                    {
                        WallJump(1);
                        return StNormal;
                    }
                }
            }

            //if (Speed != Vector2.zero && level.OnInterval(0.02f))
            //    level.ParticlesFG.Emit(wasDashB ? P_DashB : P_DashA, Center + Calc.Random.Range(Vector2.One * -2, Vector2.One * 2), DashDir.Angle());
            return StDash;
        }

        private IEnumerator DashCoroutine()
        {
            yield return null;

            var dir = lastAim;
            if (OverrideDashDirection.HasValue)
                dir = OverrideDashDirection.Value;

            var newSpeed = dir * DashSpeed;
            if (Math.Sign(beforeDashSpeed.x) == Math.Sign(newSpeed.x) && Math.Abs(beforeDashSpeed.x) > Math.Abs(newSpeed.y))
                newSpeed.x = beforeDashSpeed.x;
            Speed = newSpeed;

            //if (CollideCheck<Water>())
            //    Speed *= SwimDashSpeedMult;

            DashDir = dir;
            //TODO 震屏
            //SceneAs<Level>().DirectionalShake(DashDir, .2f);

            if (DashDir.x != 0)
                Facing = (Facings)Math.Sign(DashDir.x);

            CallDashEvents();

            //Feather particles
            //if (StateMachine.PreviousState == StStarFly)
            //    level.Particles.Emit(FlyFeather.P_Boost, 12, Center, Vector2.One * 4, (-dir).Angle());

            //Dash Slide
            if (onGround && DashDir.x != 0 && DashDir.y > 0 && Speed.y > 0)//&&!Inventory.DreamDash || !CollideCheck<DreamBlock>(Position + Vector2.UnitY)))
            {
                DashDir.x = Math.Sign(DashDir.x);
                DashDir.y = 0;
                Speed.y = 0;
                Speed.x *= DodgeSlideSpeedMult;
                Ducking = true;
            }

            //SlashFx.Burst(Center, DashDir.Angle());
            //CreateTrail();
            dashTrailTimer = .08f;

            //Swap Block check
            //if (DashDir.x != 0 && InputManager.Grab.Check)
            //{
            //    var swapBlock = CollideFirst<SwapBlock>(Position + Vector2.UnitX * Math.Sign(DashDir.X));
            //    if (swapBlock != null && swapBlock.Direction.X == Math.Sign(DashDir.X))
            //    {
            //        StateMachine.State = StClimb;
            //        Speed = Vector2.zero;
            //        yield break;
            //    }
            //}

            //Stick to Swap Blocks
            Vector2 swapCancel = Vector2.one;
            //foreach (SwapBlock swapBlock in Scene.Tracker.GetEntities<SwapBlock>())
            //{
            //    if (CollideCheck(swapBlock, Position + Vector2.UnitY))
            //    {
            //        if (swapBlock != null && swapBlock.Swapping)
            //        {
            //            if (DashDir.X != 0 && swapBlock.Direction.X == Math.Sign(DashDir.X))
            //                Speed.X = swapCancel.X = 0;
            //            if (DashDir.Y != 0 && swapBlock.Direction.Y == Math.Sign(DashDir.Y))
            //                Speed.Y = swapCancel.Y = 0;
            //        }
            //    }
            //}

            yield return DashTime;

            //CreateTrail();

            AutoJump = true;
            AutoJumpTimer = 0;
            if (DashDir.y <= 0)
            {
                Speed = DashDir * EndDashSpeed;
                Speed.x *= swapCancel.x;
                Speed.y *= swapCancel.y;
            }
            if (Speed.y < 0)
                Speed.y *= EndDashUpMult;
            StateMachine.State = StNormal;
        }
        #endregion
        public Holdable Holding { get; set; }

        public bool CanUnDuck
        {
            get
            {
                return true;
                //if (!Ducking)
                //    return true;

                //Collider was = Collider;
                //Collider = normalHitbox;
                //bool ret = !CollideCheck<Solid>();
                //Collider = was;
                //return ret;
            }
        }
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


        //public void OnDrawGizmos()
        //{
        //    Gizmos.DrawLine();
        //}
    }
}
