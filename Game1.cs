using BEPUphysics;
using BEPUphysics.Entities;
using BEPUphysics.Entities.Prefabs;
using BEPUphysics.Paths.PathFollowing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Objects;
using System;
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

        //Space testSpace;
        Model ring;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            _graphics.GraphicsProfile = GraphicsProfile.HiDef;
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
            Services.AddService(typeof(Ship), ship);

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
            Services.AddService(typeof(GraphicsDeviceManager), _graphics);

            LoadTestCube();

            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            InputManager.Instance.Update(gameTime);
            RaceManager.Instance.Update(gameTime);
            
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            base.Draw(gameTime);
            _spriteBatch.End();
        }

        void LoadTestCube()
        {
            //ringType = Content.Load<Model>("Models/RingLampV3_FullRing_100_Halo");

            //testSpace = new Space();
            //testSpace.ForceUpdater.Gravity = new Vector3(0, -9.81f, 0);

            //Box ground = new Box(Vector3.Zero, 30, 1, 30);
            //testSpace.Add(ground);

            //testSpace.Add(new Box(new Vector3(0, 4, 0), 1, 1, 1, 1));
            //testSpace.Add(new Box(new Vector3(0, 8, 0), 1, 1, 1, 1));
            //testSpace.Add(new Box(new Vector3(0, 12, 0), 1, 1, 1, 1));

            //foreach (Entity e in testSpace.Entities)
            //{
            //   Box box = e as Box;
            //    if (box != null) //This won't create any graphics for an entity that isn't a box since the model being used is a box.
            //    {
            //        Matrix scaling = Matrix.CreateScale(box.Width, box.Height, box.Length); //Since the cube model is 1x1x1, it needs to be scaled to match the size of each individual box.
            //        EntityModel model = new EntityModel(e, ringType, scaling.toBEPU(), this);
            //        //Add the drawable game component for this entity to the game.
            //        Components.Add(model);
            //       model.UpdateContent();
            //        e.Tag = model; //set the object tag of this entity to the model so that it's easy to delete the graphics component later if the entity is removed.
            //    }
            //}
            List<Ring> rings = new List<Ring>();
            float prevx = 0f, prevy = 0f, prevz = 5f, prevRotX = 0f, prevRoty = 0f, prevRotz = 0f;
            float xyzBound = 20f, xyzOffset = 10f;
            float rotBound = 20, rotOffset = 10;
            for (int i = 0; i < 10; i++)
            {
                Ring r = new(this);
                Random rand = new Random();
                r.Initialize();
                float x = 0f, y = 0f, z = 0f, xRot = 0f, yRot = 0f, zRot = 0f;
                switch (i)
                {
                    case 0:
                        r.SetUp(RingType.Start, new Microsoft.Xna.Framework.Vector3(prevx, prevy, prevz), new Microsoft.Xna.Framework.Vector3(prevRotX, prevRoty, prevRotz));
                        break;
                    case 4:
                        x = (float)(prevx + (rand.NextDouble() % xyzBound/2) - xyzOffset); y = (float)(prevy + (rand.NextDouble() % xyzBound / 2) - xyzOffset); z = xyzBound + (float)(prevz + (rand.NextDouble() % xyzBound) - xyzOffset);
                        xRot = (float)(prevRotX + rand.NextDouble() % rotBound - rotOffset); yRot = (float)(prevRoty + rand.NextDouble() % rotBound - rotOffset); zRot = (float)(prevRotz + rand.NextDouble() % rotBound - rotOffset);
                        r.SetUp(RingType.HalfWay, new Microsoft.Xna.Framework.Vector3(x, y, z), new Microsoft.Xna.Framework.Vector3(xRot, yRot, zRot));
                        break;
                    case 9:
                        x = (float)(prevx + (rand.NextDouble() % xyzBound / 2) - xyzOffset); y = (float)(prevy + (rand.NextDouble() % xyzBound / 2) - xyzOffset); z = xyzBound + (float)(prevz + (rand.NextDouble() % xyzBound) - xyzOffset);
                        xRot = (float)(prevRotX + rand.NextDouble() % rotBound - rotOffset); yRot = (float)(prevRoty + rand.NextDouble() % rotBound - rotOffset); zRot = (float)(prevRotz + rand.NextDouble() % rotBound - rotOffset);
                        r.SetUp(RingType.Finnish, new Microsoft.Xna.Framework.Vector3(x, y, z), new Microsoft.Xna.Framework.Vector3(xRot, yRot, zRot));
                        break;
                    default:
                        x = (float)(prevx + (rand.NextDouble() % xyzBound) - xyzOffset); y = (float)(prevy + (rand.NextDouble() % xyzBound) - xyzOffset); z = xyzBound + (float)(prevz + (rand.NextDouble() % xyzBound) - xyzOffset);
                        xRot = (float)(prevRotX + rand.NextDouble() % rotBound - rotOffset); yRot = (float)(prevRoty + rand.NextDouble() % rotBound - rotOffset); zRot = (float)(prevRotz + rand.NextDouble() % rotBound - rotOffset);
                        r.SetUp(RingType.Normal, new Microsoft.Xna.Framework.Vector3(x, y, z), new Microsoft.Xna.Framework.Vector3(xRot, yRot, zRot));
                        break;
                }
                
                prevx = x; prevy = y; prevz = z;
                prevRotX = xRot; prevRoty = yRot; prevRotz = zRot;
                rings.Add(r);
            }
            SkyBox box = new SkyBox("SkyBox", this);
            box.Initialize();
            
            Course c = new(this);
            //c.Initialize();
            RaceManager.Instance.LoadActiveTrack(c);
            c.UpdateContent(false, rings, box);
        }
    }
}