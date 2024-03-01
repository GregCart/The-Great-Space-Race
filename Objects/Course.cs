using BEPUphysics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using The_Great_Space_Race;


namespace Objects
{
    public class Course : GameComponent, IObserver<ModelCollision>, IObservable<ModelCollision>
    {
        public Space space;
        public Ring[] rings;
        public bool IsOrdered;
        public int LastEntered;

        private List<IObserver<ModelCollision>> Observers;

        public Course(Game game) : base(game)
        {

        }

        public override void Initialize()
        {
            space = new Space();

            Observers = new List<IObserver<ModelCollision>>();

            base.Initialize();

            foreach (Ring r in rings)
            {
                space.Add(r.Shape);
                space.Add(r.Insides);
            }
        }

        public override void Update(GameTime gameTime)
        {
            space.Update();

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

        public void OnNext(ModelCollision value)
        {
            throw new NotImplementedException();
        }

        public IDisposable Subscribe(IObserver<ModelCollision> observer)
        {
            Observers.Add(observer);

            return null;
        }
    }
}
