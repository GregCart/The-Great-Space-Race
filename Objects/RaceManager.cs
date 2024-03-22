using BEPUphysics;
using Microsoft.Xna.Framework;
using System;
using The_Great_Space_Race;


namespace Objects
{
    public class RaceManager : DrawableGameComponent, IObserver<RingPassed>
    {
        private static RaceManager instance;

        public static RaceManager Instance
        {
            get
            {
                return instance;
            }
        }
        
        protected Course activeTrack;

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

            base.Update(gameTime);
        }

        public void LoadActiveTrack(Course c)
        {
            this.activeTrack = c;
            Game.Components.Add(activeTrack);
            c.Initialize();
            activeTrack.LoadContent();
            trackLoaded = true;
        }

        public override void Draw(GameTime gameTime)
        {
            if (activeTrack.HasFinnished)
            {
                
            }

            base.Draw(gameTime);
        }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(RingPassed value)
        {
            if (value.ring == RingType.Finnish)
            {
                activeTrack.HasFinnished = true;
            }
        }
    }
}
