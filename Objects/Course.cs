using BEPUphysics;
using Microsoft.Xna.Framework;
using System;
using The_Great_Space_Race;


namespace Objects
{
    public class Course : GameComponent, IObserver<ModelCollision>, IObservable<ModelCollision>
    {
        public Space space;
        public Ring[] rings;
        public bool IsOrdered;
        public int LastEntered;

        public Course(Game game) : base(game)
        {

        }

        public override void Initialize()
        {
            space = new Space();

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            space.Update();

            base.Update(gameTime);
        }

        void IObserver<ModelCollision>.OnCompleted()
        {
            throw new NotImplementedException();
        }

        void IObserver<ModelCollision>.OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        void IObserver<ModelCollision>.OnNext(ModelCollision value)
        {
            throw new NotImplementedException();
        }

        IDisposable IObservable<ModelCollision>.Subscribe(IObserver<ModelCollision> observer)
        {
            throw new NotImplementedException();
        }
    }
}
