using BEPUphysics;
using System;
using The_Great_Space_Race;


namespace Objects
{
    public class RaceManager : IObserver<ModelCollision>
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
        public static Course[] tracks;

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
