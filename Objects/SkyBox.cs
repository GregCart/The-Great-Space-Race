using Microsoft.Xna.Framework;
using The_Great_Space_Race;

namespace Objects
{
    public class SkyBox : DrawableGameComponent
    {
        public EntityModel em;

        private string modelPath;


        public SkyBox(string path, Game game) : base(game) {
            this.modelPath = path;
        }

        public override void Initialize()
        {
            this.em = new EntityModel(this.modelPath, Matrix.Identity.toBEPU(), .1f, new BEPUutilities.Vector3(3, 3, 3), Game);
            this.em.Initialize();

            this.Game.Components.Add(this.em);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        public void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {


            base.Draw(gameTime);
        }
    }
}
