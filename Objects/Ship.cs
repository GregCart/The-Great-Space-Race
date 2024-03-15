using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Objects;
using System;
using System.Diagnostics;
using Microsoft.Xna.Framework.Audio;


namespace The_Great_Space_Race.Objects
{
    public class Ship : GameComponent, IObserver<InputEvent>
    {
        private const float MAX_SPEED = 10f;

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
            em = new EntityModel("Intergalactic_Spaceship-(Wavefront)", this.WorldMatrix.toBEPU(), .2f, this.Game);
            mainEngine = Game.Content.Load<SoundEffect>("Sounds/CartoonCarSound");
            Camera = new Camera(Game, WorldMatrix.Translation, 5);

            Game.Components.Add(em);
            Game.Components.Add(Camera);
            Game.Services.AddService(Camera);

            ((InputManager)Game.Services.GetService(typeof(InputManager))).Subscribe(this);
        }

        public void speedUp()
        {
            this.em.entity.LinearVelocity = new Vector3(this.em.entity.LinearVelocity.X, this.em.entity.LinearVelocity.Y, 
                                                MathF.Max(this.em.entity.LinearVelocity.Z - Speed, -MAX_SPEED)).toBEPU();
            if (playTime > mainEngine.Duration.TotalSeconds)
            {
                mainEngine.Play(.5f, 0f, 0f);
                playTime = 0.0f;
            }
        }

        public void slowDown()
        {
            this.em.entity.LinearVelocity = new Vector3(this.em.entity.LinearVelocity.X, this.em.entity.LinearVelocity.Y,
                                                MathF.Min(this.em.entity.LinearVelocity.Z + Speed, MAX_SPEED)).toBEPU();
            if (playTime > mainEngine.Duration.TotalSeconds)
            {
                mainEngine.Play(.5f, 0f, 0f);
                playTime = 0.0f;
            }
        }

        public void moveLeft() 
        {
            this.em.entity.LinearVelocity = new Vector3(MathF.Min(this.em.entity.LinearVelocity.X - Speed, -MAX_SPEED), 
                                                this.em.entity.LinearVelocity.Y, this.em.entity.LinearVelocity.Z).toBEPU();
        }

        public void moveRight()
        {
            this.em.entity.LinearVelocity = new Vector3(MathF.Max(this.em.entity.LinearVelocity.X + Speed, MAX_SPEED),
                                                this.em.entity.LinearVelocity.Y, this.em.entity.LinearVelocity.Z).toBEPU();
        }

        public void turnLeft()
        {
            this.em.entity.AngularVelocity = new BEPUutilities.Vector3(MathF.Min(this.em.entity.LinearVelocity.X + Speed, MAX_SPEED),
                                                this.em.entity.LinearVelocity.Y, this.em.entity.LinearVelocity.Z);
        }

        public void turnRight()
        {

        }

        public void lookUp()
        {

        }

        public void lookDown()
        {

        }

        

        public override void Update(GameTime gameTime)
        {
            //WorldMatrix = Matrix.CreateFromAxisAngle(Vector3.Right, Pitch) * Matrix.CreateFromAxisAngle(Vector3.Up, Yaw);
            WorldMatrix = em.entity.WorldTransform.toXNA();
            Debug.WriteLine(WorldMatrix.ToString());

            playTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            dt = (float)(Speed * gameTime.ElapsedGameTime.TotalSeconds);

            Camera.Update(WorldMatrix);

            base.Update(gameTime);
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
                    case Keys.W:
                        speedUp();
                        break;
                    case Keys.S:
                        slowDown();
                        break;
                    case Keys.A:
                        moveLeft();
                        break;
                    case Keys.D:
                        moveRight();
                        break;
                    case Keys.Up:
                        lookUp();
                        break;
                    case Keys.Down:
                        lookDown();
                        break;
                    case Keys.Left:
                        turnLeft();
                        break;
                    case Keys.Right:
                        turnRight();
                        break;
                }
            }
        }
    }
}
