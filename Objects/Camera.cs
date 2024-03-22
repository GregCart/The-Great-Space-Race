using BEPUphysics.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using The_Great_Space_Race;

namespace Objects
{
    // I stole this from the Getting Started tutorial ffor Bepu Physics github
    /// <summary>
    /// Basic camera class supporting mouse/keyboard/gamepad-based movement.
    /// </summary>
    public class Camera : GameComponent
    {
        /// <summary>
        /// Gets or sets the position of the camera.
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// Gets or sets the speed at which the camera moves.
        /// </summary>
        public float Speed { get; set; }

        /// <summary>
        /// Gets the view matrix of the camera.
        /// </summary>
        public Matrix ViewMatrix { get; private set; }
        /// <summary>
        /// Gets or sets the projection matrix of the camera.
        /// </summary>
        public Matrix ProjectionMatrix { get; set; }

        /// <summary>
        /// Gets the world transformation of the camera.
        /// </summary>
        public Matrix WorldMatrix { get; private set; }

        public float AspectRatio;

        /// <summary>
        /// Constructs a new camera.
        /// </summary>
        /// <param name="game">Game that this camera belongs to.</param>
        /// <param name="position">Initial position of the camera.</param>
        /// <param name="speed">Initial movement speed of the camera.</param>
        public Camera(Game game, Vector3 position, float speed): base(game)
        {
            Position = position;
            Speed = speed;
            //ProjectionMatrix = Matrix.CreatePerspectiveFieldOfViewRH(MathHelper.PiOver4, 4f / 3f, .1f, 10000.0f);
            Mouse.SetPosition(200, 200);
        }

        public override void Initialize()
        {
            base.Initialize();
        }
            

        /// <summary>
        /// Updates the camera's view matrix.
        /// </summary>
        /// <param name="dt">Timestep duration.</param>
        public void Update(Entity e)
        {
            if (AspectRatio == 0)
            {
                AspectRatio = ((GraphicsDeviceManager)Game.Services.GetService(typeof(GraphicsDeviceManager))).GraphicsDevice.Viewport.AspectRatio;
            }
            Matrix world = e.WorldTransform.toXNA();
            Position = world.Translation + BEPUutilities.Quaternion.Transform(new Vector3(0.0f, .35f, -1.8f).toBEPU(), e.Orientation).toXNA();
            //Position = world.Translation + new Vector3(0.0f, .35f, -1.8f);

            this.WorldMatrix = world;
            //this.ViewMatrix = Matrix.CreateLookAt(Position, Position + new Vector3(0.0f, 0f, .5f), world.Up);
            this.ViewMatrix = Matrix.CreateLookAt(Position, Position + BEPUutilities.Quaternion.Transform(new Vector3(0.0f, 0f, .5f).toBEPU(), e.Orientation).toXNA(), world.Up);
            this.ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(90f), AspectRatio, .2f, 10000f);

            //Debug.WriteLine("Camera: " + this.WorldMatrix.ToString());
        }
    }
}
