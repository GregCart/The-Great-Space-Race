using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Objects;
using System;
using System.Diagnostics;
using Microsoft.Xna.Framework.Audio;
using BEPUutilities;

using Matrix = Microsoft.Xna.Framework.Matrix;
using MathHelper = Microsoft.Xna.Framework.MathHelper;
using BEPUphysics.Entities;
using BEPUphysics.Entities.Prefabs;


namespace The_Great_Space_Race.Objects
{
    public class Ship : GameComponent, IObserver<InputEvent>
    {
        private const float MAX_SPEED = 7f;

        public string Name { get; set; }
        public Camera Camera { get; set; }
        public EntityModel em { get; set; }
        public SoundEffect mainEngine;
        public Matrix WorldMatrix { get; set; }
        public float Yaw
        {
            get
            {
                return yaw;
            }
            set
            {
                yaw = MathHelper.WrapAngle(value);
            }
        }
        public float Pitch
        {
            get
            {
                return pitch;
            }
            set
            {
                pitch = MathHelper.Clamp(value, -MathHelper.PiOver2, MathHelper.PiOver2);
            }
        }
        public float Speed { get; set; }
        public Entity collider;

        float dt;
        float yaw = MathHelper.ToRadians(0);
        float pitch = MathHelper.ToRadians(0);
        float roll = MathHelper.ToRadians(0);
        float playTime = 100000.0f;

        public Ship(Game1 game) : base(game)
        {
            WorldMatrix = Matrix.Identity * Matrix.CreateFromYawPitchRoll(yaw, pitch, roll);
            Speed = .15f;
        }

        public override void Initialize()
        {
            em = new EntityModel("Intergalactic_Spaceship-(Wavefront)", this.WorldMatrix.toBEPU(), .2f, new BEPUutilities.Vector3(1, 1, 1), this.Game);
            mainEngine = Game.Content.Load<SoundEffect>("Sounds/CartoonCarSound");
            Camera = new Camera(Game, WorldMatrix.Translation, Speed);
            collider = new Capsule(em.Transform.Translation, 2f, 1f);

            Game.Components.Add(em);
            Game.Components.Add(Camera);
            Game.Services.AddService(Camera);

            Debug.WriteLine("em: " + this.em.entity.WorldTransform.ToString());
            Debug.WriteLine("collider:" + this.collider.WorldTransform.ToString());

            ((InputManager)Game.Services.GetService(typeof(InputManager))).Subscribe(this);
        }

        public void speedUp()
        {
            //this.em.entity.LinearVelocity = new Vector3(this.em.entity.LinearVelocity.X, this.em.entity.LinearVelocity.Y, 
            //                                   MathF.Max(this.em.entity.LinearVelocity.Z - Speed, -MAX_SPEED)).toBEPU();
            var tmp = new BEPUutilities.Vector3(0f, 0f, Speed);
            tmp = BEPUutilities.Quaternion.Transform(tmp, this.em.entity.Orientation);
            this.em.entity.ApplyLinearImpulse(ref tmp);
            if (playTime > mainEngine.Duration.TotalSeconds)
            {
                mainEngine.Play(.5f, 0f, 0f);
                playTime = 0.0f;
            }
        }

        public void slowDown()
        {
            //this.em.entity.LinearVelocity = new Vector3(this.em.entity.LinearVelocity.X, this.em.entity.LinearVelocity.Y,
            //                                    MathF.Min(this.em.entity.LinearVelocity.Z + Speed, MAX_SPEED)).toBEPU();
            var tmp = new BEPUutilities.Vector3(0f, 0f, -Speed/2);
            tmp = BEPUutilities.Quaternion.Transform(tmp, this.em.entity.Orientation);
            this.em.entity.ApplyLinearImpulse(ref tmp);
            if (playTime > mainEngine.Duration.TotalSeconds)
            {
                mainEngine.Play(.5f, 0f, 0f);
                playTime = 0.0f;
            }
        }

        public void moveLeft() 
        {
            //this.em.entity.LinearVelocity = new Vector3(MathF.Min(this.em.entity.LinearVelocity.X - Speed, -MAX_SPEED), 
            //                                    this.em.entity.LinearVelocity.Y, this.em.entity.LinearVelocity.Z).toBEPU();
            var tmp = new BEPUutilities.Vector3(Speed/3, 0f, 0f);
            tmp = BEPUutilities.Quaternion.Transform(tmp, this.em.entity.Orientation);
            this.em.entity.ApplyLinearImpulse(ref tmp);
        }

        public void moveRight()
        {
            //this.em.entity.LinearVelocity = new Vector3(MathF.Max(this.em.entity.LinearVelocity.X + Speed, MAX_SPEED),
            //                                    this.em.entity.LinearVelocity.Y, this.em.entity.LinearVelocity.Z).toBEPU();
            var tmp = new BEPUutilities.Vector3(-Speed/3, 0f, 0f);
            tmp = BEPUutilities.Quaternion.Transform(tmp, this.em.entity.Orientation);
            this.em.entity.ApplyLinearImpulse(ref tmp);
        }

        public void turnLeft()
        {
            //this.em.entity.AngularVelocity = new BEPUutilities.Vector3(MathF.Min(this.em.entity.LinearVelocity.X + Speed, MAX_SPEED),
            //                                    this.em.entity.LinearVelocity.Y, this.em.entity.LinearVelocity.Z);
            var tmp = new BEPUutilities.Vector3(0f, Speed / 5, 0f);
            tmp = BEPUutilities.Quaternion.Transform(tmp, this.em.entity.Orientation);
            this.em.entity.ApplyAngularImpulse(ref tmp);
        }

        public void turnRight()
        {
            var tmp = new BEPUutilities.Vector3(0f, -Speed / 5, 0f);
            tmp = BEPUutilities.Quaternion.Transform(tmp, this.em.entity.Orientation);
            this.em.entity.ApplyAngularImpulse(ref tmp);
        }

        public void lookUp()
        {
            var tmp = new BEPUutilities.Vector3(-Speed/5, 0f, 0f);
            tmp = BEPUutilities.Quaternion.Transform(tmp, this.em.entity.Orientation);
            this.em.entity.ApplyAngularImpulse(ref tmp);
        }

        public void lookDown()
        {
            var tmp = new BEPUutilities.Vector3(Speed / 5, 0f, 0f);
            tmp = BEPUutilities.Quaternion.Transform(tmp, this.em.entity.Orientation);
            this.em.entity.ApplyAngularImpulse(ref tmp);
        }

        public void tiltLeft()
        {
            var tmp = new BEPUutilities.Vector3(0f, 0f, -Speed / 5);
            tmp = BEPUutilities.Quaternion.Transform(tmp, this.em.entity.Orientation);
            this.em.entity.ApplyAngularImpulse(ref tmp);
        }

        public void tiltRight()
        {
            var tmp = new BEPUutilities.Vector3(0f, 0f, Speed / 5);
            tmp = BEPUutilities.Quaternion.Transform(tmp, this.em.entity.Orientation);
            this.em.entity.ApplyAngularImpulse(ref tmp);
        }

        public override void Update(GameTime gameTime)
        {
            em.entity.BecomeDynamic(1);

            //WorldMatrix = Matrix.CreateFromAxisAngle(Vector3.Right, Pitch) * Matrix.CreateFromAxisAngle(Vector3.Up, Yaw);
            WorldMatrix = em.entity.WorldTransform.toXNA();
            //Debug.WriteLine("Ship:  " + WorldMatrix.ToString());

            playTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            dt = (float)(Speed * gameTime.ElapsedGameTime.TotalSeconds);

            Camera.Update(em.entity);
            collider.WorldTransform = em.entity.WorldTransform;

            base.Update(gameTime);

            em.entity.ActivityInformation.Activate();
        }

        public void OnCompleted()
        {
            //Do Nothing
        }

        public void OnError(Exception error)
        {
            Debug.WriteLine(error.ToString());
        }

        public void OnNext(InputEvent value)
        {
            if (value.type == EventType.Key_Down)
            {
                
                switch (value.key)
                {
                    case Keys.Up:
                        speedUp();
                        break;
                    case Keys.Down:
                        slowDown();
                        break;
                    case Keys.Left:
                        moveLeft();
                        break;
                    case Keys.Right:
                        moveRight();
                        break;
                    case Keys.W:
                        lookUp();
                        break;
                    case Keys.S:
                        lookDown();
                        break;
                    case Keys.A:
                        turnLeft();
                        break;
                    case Keys.D:
                        turnRight();
                        break;
                    case Keys.Q:
                        tiltLeft();
                        break;
                    case Keys.E:
                        tiltRight();
                        break;
                }
            }
        }
    }
}
