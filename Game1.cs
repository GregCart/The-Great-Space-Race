using BEPUphysics;
using BEPUphysics.Entities.Prefabs;
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

        private TestEntityModel testEntityModel;
        private Space testSpace;
        private Box testBox;
        public Camera testCamera;
        public KeyboardState KeyboardState;
        public MouseState MouseState;


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
            testSpace = new Space();
            testBox = new Box(Vector3.Zero, 30, 1, 30);
            testCamera = new Camera(this, new Vector3(0, 3, 10), 5);

            testSpace.ForceUpdater.Gravity = new Vector3(0, -9.81f, 0);

            Services.AddService(typeof(RaceManager), raceManager);
            Services.AddService(typeof(InputManager), inputManager);

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

            testSpace.Add(testBox);

            testSpace.Add(new Box(new Vector3(0, 4, 0), 1, 1, 1, 1));
            testSpace.Add(new Box(new Vector3(0, 8, 0), 1, 1, 1, 1));
            testSpace.Add(new Box(new Vector3(0, 12, 0), 1, 1, 1, 1));
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            testSpace.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            base.Draw(gameTime);
            _spriteBatch.End();
        }
    }
}