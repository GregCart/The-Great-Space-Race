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
        public Ring[] Rings;
        public bool IsOrdered;
        public int LastEntered;

        private static List<IObserver<ModelCollision>> Observers
        {
            get
            {
                if (Observers == null)
                {
                    Observers = new List<IObserver<ModelCollision>>();
                }

                return Observers;
            }
        }

        public Course(Game1 game) : base(game)
        {
            this.IsOrdered = false;
            this.LastEntered = -1;
            
        }

        public override void Initialize()
        {
            space = new Space();
            
            foreach(Ring r in Rings)
            {
                Game.Components.Add(r);
            }

            base.Initialize();

            foreach (Ring r in Rings)
            {
                space.Add(r.shape);
                space.Add(r.threshold);
            }
        }

        public void Initialize(bool isOrdered, List<Ring> rings) 
        {
            this.IsOrdered = isOrdered;
            this.Rings = rings.ToArray();

            this.Initialize();
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

            return this;
        }
    }
}
