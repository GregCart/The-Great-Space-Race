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
        public SkyBox skyBox;
        public Ring[] Rings;
        public bool IsOrdered;
        public bool HasFinnished;
        public double timer;
        public int RingsHit;

        private static List<IObserver<bool>> Observers;

        private Ship ship;


        public Course(Game1 game) : base(game)
        {
            this.IsOrdered = false;
            this.HasFinnished = false;
            
        }

        public override void Initialize()
        {
            if (space == null)
            {
                space = new Space();
            }

            this.Rings = new List<Ring>().ToArray();

            base.Initialize();

            ship = Game.Components[0] as Ship;
            this.space.Add(ship.em.entity);
        }

        public void UpdateContent(bool isOrdered, List<Ring> rings, SkyBox box) 
        //public void UpdateContent(bool isOrdered, List<Ring> rings)
        {
            this.IsOrdered = isOrdered;
            this.Rings = rings.ToArray();
            //this.skyBox = box;

            foreach (Ring r in Rings)
            {
                Game.Components.Add(r);
            }
            box.Initialize();
            Game.Components.Add(box);

            this.LoadContent();
        }

        public void LoadContent()
        {
            foreach (Ring r in Rings)
            {
                space.Add(r.em.entity);
                r.Subscribe(this);
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (!HasFinnished)
            {
                space.Update();
                timer += gameTime.ElapsedGameTime.Milliseconds;
            }

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
            if (value.type == EventType.Ring_Passed)
            {
                this.RingsHit++;
                RingPassed rp = (RingPassed)value;
                if (rp.ringType == RingType.Start)
                {
                    this.HasFinnished = false;
                } 
                else if (rp.ringType == RingType.Finnish)
                {
                    this.HasFinnished = true;
                } else
                {
                    int ring = Array.FindIndex(this.Rings, obj => obj.Equals(rp.ring));
                    this.Rings[ring + 1].isNextRing = true;
                }
            }
        }

        public IDisposable Subscribe(IObserver<bool> observer)
        {
            Observers.Add(observer);

            return this;
        }
    }
}
