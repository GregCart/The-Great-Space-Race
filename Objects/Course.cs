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

        public override void Update(GameTime gameTime)
        {
            space.Update();

            base.Update(gameTime);
        }

        public override void Initialize()
        {
            space = new Space();

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

        public IDisposable Subscribe(IObserver<ModelCollision> observer)
        {
            throw new NotImplementedException();
        }
    }
}
