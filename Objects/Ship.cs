using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Objects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
namespace The_Great_Space_Race.Objects
{
    public class Ship : GameComponent, IObserver<InputEvent>
    {
        public string Name { get; set; }
        public Camera Camera { get; set; }
        public EntityModel EntityModel { get; set; }
        public Vector3 Position { get; set; }
        /// <summary>
        /// Gets the world transformation of the camera.
        /// </summary>
        public Matrix WorldMatrix { get; private set; }
        /// <summary>
        /// Gets the view matrix of the camera.
        /// </summary>
        public Matrix ViewMatrix { get; private set; }
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

        float yaw;
        float pitch;
        float dt;


        public Ship(Game1 game) : base(game)
        {

        }

        public override void Initialize()
        {

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
            WorldMatrix = Matrix.CreateFromAxisAngle(Vector3.Right, Pitch) * Matrix.CreateFromAxisAngle(Vector3.Up, Yaw);


            dt = (float)(Speed * gameTime.ElapsedGameTime.TotalSeconds);

            WorldMatrix = WorldMatrix * Matrix.CreateTranslation(Position);
            ViewMatrix = Matrix.Invert(WorldMatrix);
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
