using BEPUphysics;
using BEPUphysics.Entities;
using BEPUphysics.Entities.Prefabs;
using BEPUphysics.Paths.PathFollowing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Objects;
using System.Collections.Generic;
using The_Great_Space_Race.Objects;
using Vector3 = BEPUutilities.Vector3;

namespace The_Great_Space_Race
{
    public class Game1 : Game
    {
        private static InputManager inputManager;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private RaceManager raceManager;
        private List<Course> cources;

        private Ship ship;

        Space testSpace;
        Camera cam;
        Model ring;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
        }

        protected override void Initialize()
        {
            raceManager = new RaceManager(this);
            inputManager = new InputManager(this);
            cources = new List<Course>();
            ship = new Ship(this);

            Services.AddService(typeof(RaceManager), raceManager);
            Services.AddService(typeof(InputManager), inputManager);

            Components.Add(ship);
            foreach (Course c in cources)
            {
                Components.Add(c);
            }

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Services.AddService(typeof(SpriteBatch), _spriteBatch);


            LoadTestCube();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (InputManager.LeftClicked == true)
            {
                //If the user is clicking, start firing some boxes.
                //First, create a new dynamic box at the camera's location.
                Box toAdd = new Box(cam.Position, 1, 1, 1, 1);
                //Set the velocity of the new box to fly in the direction the camera is pointing.
                //Entities have a whole bunch of properties that can be read from and written to.
                //Try looking around in the entity's available properties to get an idea of what is available.
                toAdd.LinearVelocity = cam.WorldMatrix.Forward * 10;
                //Add the new box to the simulation.
                testSpace.Add(toAdd);

                //Add a graphical representation of the box to the drawable game components.
                EntityModel model = new EntityModel(toAdd, ring, Matrix.Identity.toBEPU(), this);
                Components.Add(model);
                toAdd.Tag = model;  //set the object tag of this entity to the model so that it's easy to delete the graphics component later if the entity is removed.
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Copy any parent transforms.
            Matrix[] transforms = new Matrix[ring.Bones.Count];
            ring.CopyAbsoluteBoneTransformsTo(transforms);
            // Draw the model. A model can have multiple meshes, so loop.
            foreach (ModelMesh mesh in ring.Meshes)
            {
                // This is where the mesh orientation is set, as well
                // as our camera and projection.
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    //WORLD TRANSFORM
                    effect.World = transforms[mesh.ParentBone.Index]
                        * Matrix.CreateTranslation(new (0.0f, 0.0f, 0.0f))
                        * Matrix.CreateScale(.02f);
                    //camera position, scale, rotation
                    effect.View = Matrix.CreateLookAt((new Vector3(-10, -10, -10)).toXNA(),
                        Vector3.Zero.toXNA(), Vector3.Up.toXNA());
                    effect.Projection = Matrix.CreatePerspectiveFieldOfView(
                        MathHelper.ToRadians(45.0f), 16f/9f,
                        1.0f, 10000.0f);
                }
                // Draw the mesh, using the effects set above.
                mesh.Draw();
            }
            base.Draw(gameTime);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            base.Draw(gameTime);
            _spriteBatch.End();
        }

        void LoadTestCube()
        {
            cam = new Camera(this, new Vector3(0, 3, 10), 5);
            ring = Content.Load<Model>("Models/RingLampV3_FullRing_100_Halo");

            testSpace = new Space();
            testSpace.ForceUpdater.Gravity = new Vector3(0, -9.81f, 0);

            Box ground = new Box(Vector3.Zero, 30, 1, 30);
            testSpace.Add(ground);

            testSpace.Add(new Box(new Vector3(0, 4, 0), 1, 1, 1, 1));
            testSpace.Add(new Box(new Vector3(0, 8, 0), 1, 1, 1, 1));
            testSpace.Add(new Box(new Vector3(0, 12, 0), 1, 1, 1, 1));

            foreach (Entity e in testSpace.Entities)
            {
                Box box = e as Box;
                if (box != null) //This won't create any graphics for an entity that isn't a box since the model being used is a box.
                {

                    Matrix scaling = Matrix.CreateScale(box.Width, box.Height, box.Length); //Since the cube model is 1x1x1, it needs to be scaled to match the size of each individual box.
                    EntityModel model = new EntityModel(e, ring, scaling.toBEPU(), this);
                    //Add the drawable game component for this entity to the game.
                    Components.Add(model);
                    model.UpdateContent();
                    e.Tag = model; //set the object tag of this entity to the model so that it's easy to delete the graphics component later if the entity is removed.
                }
            }
        }
    }
}