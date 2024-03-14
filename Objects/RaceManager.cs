using BEPUphysics;
using Microsoft.Xna.Framework;
using System;
using The_Great_Space_Race;


namespace Objects
{
    public class RaceManager : GameComponent, IObserver<IModelCollision>
    {
        private static RaceManager instance;

        public static RaceManager Instance
        {
            get
            {
                return instance;
            }
        }
        
        public Course activeTrack;

        private bool trackLoaded = false;

        public RaceManager(Game1 game): base(game)
        {
            instance = this;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (activeTrack != null && trackLoaded == false)
            {
                activeTrack.LoadContent();
                trackLoaded = true;
            }
            base.Update(gameTime);
        }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(IModelCollision value)
        {
            throw new NotImplementedException();
        }
    }
}
