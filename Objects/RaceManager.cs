using BEPUphysics;
using Microsoft.Xna.Framework;
using System;
using The_Great_Space_Race;


namespace Objects
{
    public class RaceManager : GameComponent, IObserver<ModelCollision>
    {
        private static RaceManager instance;

        public static RaceManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new RaceManager();
                }

                return instance;
            }
        }
        public static Course activeTrack;

        public RaceManager(Game1 game): base(game)
        {

        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(ModelCollision value)
        {
            throw new NotImplementedException();
        }
    }
}
