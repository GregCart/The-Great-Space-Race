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
        public string Name { get; set; }
        public Camera Camera { get; set; }
        public EntityModel em { get; set; }
        public SoundEffect mainEngine;
        public Matrix WorldMatrix { get; set; }
        /*public float Yaw
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
        }*/
        public float Speed { get; set; }

        float dt;


        public Ship(Game1 game) : base(game)
        {
            WorldMatrix = Matrix.Identity;
        }

        public override void Initialize()
        {
            em = new EntityModel("Intergalactic_Spaceship-(Wavefront)", this.WorldMatrix.toBEPU(), .5f, this.Game);
            Camera = new Camera(Game, WorldMatrix.Translation - new Vector3(-1, -2, 0), 5);

            Game.Components.Add(em);
            Game.Components.Add(Camera);
            Game.Services.AddService(Camera);
        }

        public void speedUp()
        {
            
        }

        public void slowDown()
        {
            
        }

        public void moveLeft() 
        { 
        
        }

        public void moveRight()
        {

        }

        public void turnLeft()
        {

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
