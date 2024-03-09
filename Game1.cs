using BEPUphysics;
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
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

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