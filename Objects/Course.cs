using BEPUphysics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using The_Great_Space_Race;
using The_Great_Space_Race.Objects;


namespace Objects
{
    public class Course : GameComponent, IObserver<ModelCollision>, IObservable<bool>
    {
        public string Name { get; set; }
        public Space space;
        public Ring[] Rings;
        public bool IsOrdered;
        public int LastEntered;

        private static List<IObserver<bool>> Observers;

        private Ship ship;


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

            /*foreach (Ring r in Rings)
            {
                space.Add(r.shape);
            }

            ship = Game.Components[0] as Ship;
            this.space.Add(ship.EntityModel.entity);*/
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
            //do nothing
        }

        public void OnError(Exception error)
        {
            Debug.WriteLine(error);
        }

        public void OnNext(ModelCollision value)
        {
            throw new NotImplementedException();
        }

        public IDisposable Subscribe(IObserver<bool> observer)
        {
            Observers.Add(observer);

            return this;
        }
    }
}
